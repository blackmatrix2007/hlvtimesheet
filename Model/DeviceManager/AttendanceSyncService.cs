using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HLVTimeSheet.AcsessData;
using System.Configuration;

namespace HLVTimeSheet.Model.DeviceManager
{
    /// <summary>
    /// Xử lý dữ liệu chấm công từ DeviceManager theo hai luồng:
    ///
    ///   1. PUSH (ưu tiên): DeviceManager gọi webhook → ProcessWebhookAsync()
    ///      → Ghi ngay vào ChamCong_Device khi nhân viên chạm tay / nhận mặt
    ///
    ///   2. PULL (dự phòng): Chủ động kéo dữ liệu → PullAndSaveAsync()
    ///      → Dùng khi webhook bị mất hoặc cần đồng bộ lại dữ liệu cũ
    ///
    /// Tham khảo: source/business-service-nodejs/src/modules/attendance/attendance.service.ts
    /// </summary>
    public class AttendanceSyncService
    {
        private readonly DeviceManagerConfig _config;
        private readonly DeviceManagerApiClient _client;

        public AttendanceSyncService(string chiNhanhId = null)
        {
            _config = DeviceManagerConfig.Load(chiNhanhId);
            _client = new DeviceManagerApiClient(_config);
        }

        // ═══════════════════════════════════════════════════════════════════════════
        // LUỒNG 1: PUSH — Nhận webhook từ DeviceManager
        // ═══════════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Xử lý webhook từ DeviceManager.
        /// Tương đương processWebhook() trong attendance.service.ts.
        ///
        /// Payload JSON mẫu:
        /// {
        ///   "event": "attendance.checkin",
        ///   "employeeCode": "NV001",
        ///   "checkingTime": "2026-03-26T08:30:00+07:00",
        ///   "faceConfidence": 0.92,
        ///   "latitude": 10.7769,
        ///   "longitude": 106.7009,
        ///   "deviceName": "Camera-Lobby",
        ///   "source": "arcface-device",
        ///   "signature": "hmac-sha256-hex"
        /// }
        /// </summary>
        public WebhookResult ProcessWebhook(WebhookAttendanceDto dto)
        {
            // 1. Xác thực chữ ký HMAC-SHA256 nếu webhook secret được cấu hình
            if (!string.IsNullOrEmpty(_config.WebhookSecret) &&
                !string.IsNullOrEmpty(dto.Signature))
            {
                if (!VerifySignature(dto))
                    return new WebhookResult { Success = false, Message = "Chữ ký webhook không hợp lệ." };
            }

            // 2. Parse thời gian
            if (!DateTime.TryParse(dto.CheckingTime, out DateTime thoiGian))
                return new WebhookResult { Success = false, Message = "checkingTime không hợp lệ." };

            thoiGian = thoiGian.ToLocalTime();

            // 3. Kiểm tra sự kiện
            bool laVao = dto.Event == "attendance.checkin";
            if (!laVao && dto.Event != "attendance.checkout")
                return new WebhookResult { Success = false, Message = $"Event '{dto.Event}' không được hỗ trợ." };

            // 4. Ghi vào SQL Server + validate + alert
            var conn = new ConnectionDatabase();
            using (var sqlConn = new SqlConnection(conn.ReturnConnectionDatabaseWS()))
            {
                sqlConn.Open();
                EnsureTableExists(sqlConn);

                // ── Validate: mapNV phải tồn tại trong DanhSachNhanSu ─────────────
                bool employeeExists = EmployeeExistsInDb(sqlConn, dto.EmployeeCode);
                if (!employeeExists)
                {
                    // Ghi vào DB (audit) nhưng đánh isValid=false
                    InsertAttendanceLog(sqlConn, new ChamCongDeviceRecord
                    {
                        MapNV           = dto.EmployeeCode,
                        Loai            = laVao ? "check_in" : "check_out",
                        ThoiGian        = thoiGian,
                        DeviceId        = dto.DeviceId?.ToString(),
                        DeviceName      = dto.DeviceName,
                        DiemTin         = dto.FaceConfidence,
                        Latitude        = dto.Latitude,
                        Longitude       = dto.Longitude,
                        GpsAccuracy     = dto.GpsAccuracy,
                        IsValid         = false,
                        IsDuplicate     = false,
                        AttemptNumber   = 1,
                        RejectionReason = "Mã nhân viên không tồn tại trong HLVTimeSheet",
                        Source          = dto.Source ?? "device-manager",
                    });

                    // Gửi cảnh báo (fire-and-forget)
                    var alert = new AlertService();
                    Task.Run(async () => await alert.AlertUnknownEmployeeAsync(
                        dto.EmployeeCode, dto.DeviceName ?? "", thoiGian));

                    return new WebhookResult
                    {
                        Success         = true, // trả 200 để DeviceManager không retry
                        Message         = $"Mã nhân viên '{dto.EmployeeCode}' không tồn tại — đã ghi audit.",
                        IsValid         = false,
                        RejectionReason = "Mã nhân viên không tồn tại trong HLVTimeSheet",
                    };
                }

                // ── Duplicate check ───────────────────────────────────────────────
                int attemptNo = GetNextAttemptNumber(sqlConn, dto.EmployeeCode, thoiGian.Date);
                bool isDuplicate     = attemptNo > 1;
                bool isValid         = !isDuplicate;
                string rejectionReason = isDuplicate ? "Chấm công trùng lặp trong ngày" : null;

                int id = InsertAttendanceLog(sqlConn, new ChamCongDeviceRecord
                {
                    MapNV           = dto.EmployeeCode,
                    Loai            = laVao ? "check_in" : "check_out",
                    ThoiGian        = thoiGian,
                    DeviceId        = dto.DeviceId?.ToString(),
                    DeviceName      = dto.DeviceName,
                    DiemTin         = dto.FaceConfidence,
                    Latitude        = dto.Latitude,
                    Longitude       = dto.Longitude,
                    GpsAccuracy     = dto.GpsAccuracy,
                    IsValid         = isValid,
                    IsDuplicate     = isDuplicate,
                    AttemptNumber   = attemptNo,
                    RejectionReason = rejectionReason,
                    Source          = dto.Source ?? "device-manager",
                });

                // ── Cảnh báo face confidence thấp ────────────────────────────────
                double faceThreshold = GetFaceConfidenceThreshold();
                if (dto.FaceConfidence.HasValue && dto.FaceConfidence.Value < faceThreshold)
                {
                    var alert = new AlertService();
                    Task.Run(async () => await alert.AlertLowFaceConfidenceAsync(
                        dto.EmployeeCode, thoiGian,
                        dto.FaceConfidence.Value, faceThreshold,
                        dto.DeviceName ?? ""));
                }

                return new WebhookResult
                {
                    Success         = true,
                    Message         = isValid ? "Ghi nhận chấm công thành công." : "Ghi nhận (audit) - " + rejectionReason,
                    RecordId        = id,
                    IsValid         = isValid,
                    RejectionReason = rejectionReason,
                };
            }
        }

        // ═══════════════════════════════════════════════════════════════════════════
        // LUỒNG 2: PULL — Chủ động kéo từ DeviceManager API
        // ═══════════════════════════════════════════════════════════════════════════

        public async Task<DmTodaySummary> GetTodaySummaryAsync()
        {
            var result = await _client.GetAsync<ApiResponse<DmTodaySummary>>("attendance/today/summary");
            return result?.Data;
        }

        public async Task<List<DmAttendanceLog>> GetCheckInsAsync(
            string startDate,
            string endDate,
            string employeeCode = null)
        {
            // DeviceManager dùng Between(startDate, endDate) với ngày không có giờ
            // → endDate phải +1 ngày để bao gồm toàn bộ ngày endDate
            string endDateExclusive = endDate;
            if (DateTime.TryParse(endDate, out DateTime dtEnd))
                endDateExclusive = dtEnd.AddDays(1).ToString("yyyy-MM-dd");

            var endpoint = $"attendance/external/check-ins" +
                           $"?companyId={_config.CustomerId}&startDate={startDate}&endDate={endDateExclusive}";
            if (!string.IsNullOrEmpty(employeeCode))
                endpoint += $"&employeeCode={employeeCode}";

            var result = await _client.GetAsync<ApiResponse<DmCheckInListResponse>>(endpoint);
            return result?.Data?.CheckIns ?? new List<DmAttendanceLog>();
        }

        /// <summary>
        /// Kéo dữ liệu từ API và upsert vào ChamCong_Device.
        /// Dùng để đồng bộ dữ liệu bị miss hoặc sync lại dữ liệu cũ.
        /// </summary>
        public async Task<int> PullAndSaveAsync(string startDate, string endDate)
        {
            var logs = await GetCheckInsAsync(startDate, endDate);
            if (logs.Count == 0) return 0;

            var conn = new ConnectionDatabase();
            using (var sqlConn = new SqlConnection(conn.ReturnConnectionDatabaseWS()))
            {
                sqlConn.Open();
                EnsureTableExists(sqlConn);

                int saved = 0;
                foreach (var log in logs)
                    saved += UpsertFromPullLog(sqlConn, log);

                return saved;
            }
        }

        // ═══════════════════════════════════════════════════════════════════════════
        // Đọc dữ liệu đã lưu (dùng cho bảng công)
        // ═══════════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Giờ vào (check_in đầu tiên hợp lệ) và giờ ra (check_out cuối cùng hợp lệ)
        /// theo từng ngày trong tháng — dùng để lập bảng công.
        /// </summary>
        public DataTable GetDailyCheckInOut(string mapNV, int nam, int thang)
        {
            var conn = new ConnectionDatabase();
            using (var sqlConn = new SqlConnection(conn.ReturnConnectionDatabaseWS()))
            {
                sqlConn.Open();
                EnsureTableExists(sqlConn);

                const string sql = @"
                    SELECT
                        mapNV,
                        CAST(thoiGian AS DATE)                                              AS ngay,
                        MIN(CASE WHEN loai='check_in'  AND isValid=1 THEN thoiGian END)    AS gioVao,
                        MAX(CASE WHEN loai='check_out' AND isValid=1 THEN thoiGian END)    AS gioRa,
                        MAX(diemTin)                                                        AS diemTinMax,
                        COUNT(*)                                                            AS tongLuot
                    FROM ChamCong_Device
                    WHERE mapNV = @mapNV
                      AND YEAR(thoiGian) = @nam
                      AND MONTH(thoiGian) = @thang
                    GROUP BY mapNV, CAST(thoiGian AS DATE)
                    ORDER BY ngay";

                using (var cmd = new SqlCommand(sql, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@mapNV", mapNV);
                    cmd.Parameters.AddWithValue("@nam",   nam);
                    cmd.Parameters.AddWithValue("@thang", thang);
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════════════════
        // Private helpers
        // ═══════════════════════════════════════════════════════════════════════════

        // ── Validate nhân viên có trong DanhSachNhanSu không ────────────────────

        private static bool EmployeeExistsInDb(SqlConnection conn, string mapNV)
        {
            const string sql = @"
                SELECT COUNT(1) FROM DanhSachNhanSu
                WHERE ma = @mapNV AND trangthai = 1";
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@mapNV", mapNV);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        // ── Đọc ngưỡng face confidence từ DB settings ───────────────────────────

        private static double GetFaceConfidenceThreshold()
        {
            try
            {
                var dbConn = new ConnectionDatabase();
                using (var sqlConn = new SqlConnection(dbConn.ReturnConnectionDatabaseWS()))
                {
                    sqlConn.Open();
                    const string sql = @"
                        SELECT TOP 1 faceConfidenceThreshold
                        FROM DeviceManagerSettings
                        WHERE laDinhChinh = 1 AND dangHoatDong = 1";
                    using (var cmd = new SqlCommand(sql, sqlConn))
                    {
                        var val = cmd.ExecuteScalar();
                        if (val != null && val != DBNull.Value)
                            return Convert.ToDouble(val);
                    }
                }
            }
            catch { }
            // Fallback: Web.config hoặc giá trị mặc định 0.6
            if (double.TryParse(
                    ConfigurationManager.AppSettings["DeviceManager_FaceThreshold"],
                    out double cfg))
                return cfg;
            return 0.6;
        }

        private bool VerifySignature(WebhookAttendanceDto dto)
        {
            // Tái tạo payload để kiểm tra chữ ký (giống pattern trong source)
            var payload = $"{dto.Event}:{dto.EmployeeCode}:{dto.CheckingTime}";
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_config.WebhookSecret)))
            {
                var computed = BitConverter.ToString(
                    hmac.ComputeHash(Encoding.UTF8.GetBytes(payload)))
                    .Replace("-", "").ToLowerInvariant();
                return computed == dto.Signature?.ToLowerInvariant();
            }
        }

        private static int GetNextAttemptNumber(SqlConnection conn, string mapNV, DateTime ngay)
        {
            const string sql = @"
                SELECT ISNULL(MAX(attemptNumber), 0) + 1
                FROM ChamCong_Device
                WHERE mapNV = @mapNV AND CAST(thoiGian AS DATE) = @ngay";
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@mapNV", mapNV);
                cmd.Parameters.AddWithValue("@ngay", ngay.Date);
                return (int)cmd.ExecuteScalar();
            }
        }

        private static int InsertAttendanceLog(SqlConnection conn, ChamCongDeviceRecord r)
        {
            // Webhook push không có dmLogId → tự sinh UUID để tránh lỗi UNIQUE NULL
            string dmLogId = "webhook-" + Guid.NewGuid().ToString();

            const string sql = @"
                INSERT INTO ChamCong_Device
                    (dmLogId, mapNV, loai, thoiGian, deviceId, deviceName, diemTin,
                     latitude, longitude, gpsAccuracy,
                     isValid, isDuplicate, attemptNumber, rejectionReason, source)
                OUTPUT INSERTED.pk_seq
                VALUES
                    (@dmLogId, @mapNV, @loai, @thoiGian, @deviceId, @deviceName, @diemTin,
                     @latitude, @longitude, @gpsAccuracy,
                     @isValid, @isDuplicate, @attemptNumber, @rejectionReason, @source)";

            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@dmLogId",         dmLogId);
                cmd.Parameters.AddWithValue("@mapNV",           r.MapNV);
                cmd.Parameters.AddWithValue("@loai",            r.Loai);
                cmd.Parameters.AddWithValue("@thoiGian",        r.ThoiGian);
                cmd.Parameters.AddWithValue("@deviceId",        (object)r.DeviceId     ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@deviceName",      (object)r.DeviceName   ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@diemTin",         (object)r.DiemTin      ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@latitude",        (object)r.Latitude     ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@longitude",       (object)r.Longitude    ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@gpsAccuracy",     (object)r.GpsAccuracy  ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@isValid",         r.IsValid);
                cmd.Parameters.AddWithValue("@isDuplicate",     r.IsDuplicate);
                cmd.Parameters.AddWithValue("@attemptNumber",   r.AttemptNumber);
                cmd.Parameters.AddWithValue("@rejectionReason", (object)r.RejectionReason ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@source",          r.Source ?? "device-manager");
                return (int)cmd.ExecuteScalar();
            }
        }

        private static int UpsertFromPullLog(SqlConnection conn, DmAttendanceLog log)
        {
            // PULL: dùng dmLogId (UUID string) để tránh trùng lặp
            const string sql = @"
                MERGE ChamCong_Device AS target
                USING (SELECT @dmLogId AS dmLogId) AS source ON target.dmLogId = source.dmLogId
                WHEN MATCHED THEN UPDATE SET
                    loai        = @loai,
                    thoiGian    = @thoiGian,
                    deviceId    = @deviceId,
                    diemTin     = @diemTin,
                    ngaySync    = GETDATE()
                WHEN NOT MATCHED THEN INSERT
                    (dmLogId, mapNV, loai, thoiGian, deviceId, diemTin, isValid, source)
                VALUES
                    (@dmLogId, @mapNV, @loai, @thoiGian, @deviceId, @diemTin, 1, 'pull');";

            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@dmLogId",  (object)log.Id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@mapNV",    log.EmployeeCode ?? "");
                cmd.Parameters.AddWithValue("@loai",     log.Type ?? "check_in");
                cmd.Parameters.AddWithValue("@thoiGian", log.Timestamp);
                cmd.Parameters.AddWithValue("@deviceId", (object)log.DeviceId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@diemTin",  log.ConfidenceScore);
                return cmd.ExecuteNonQuery();
            }
        }

        internal static void EnsureTableExists(SqlConnection conn)
        {
            const string ddl = @"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'ChamCong_Device'
                )
                BEGIN
                    CREATE TABLE ChamCong_Device (
                        pk_seq          INT IDENTITY(1,1) PRIMARY KEY,
                        dmLogId         NVARCHAR(100),              -- UUID từ DeviceManager (PULL)
                        mapNV           NVARCHAR(50)  NOT NULL,
                        loai            NVARCHAR(20)  NOT NULL,     -- check_in / check_out
                        thoiGian        DATETIME      NOT NULL,
                        deviceId        NVARCHAR(100),
                        deviceName      NVARCHAR(200),
                        diemTin         FLOAT,                      -- face confidence 0.0-1.0

                        -- GPS
                        latitude        FLOAT,
                        longitude       FLOAT,
                        gpsAccuracy     FLOAT,

                        -- Audit trail
                        isValid         BIT           DEFAULT 1,
                        isDuplicate     BIT           DEFAULT 0,
                        attemptNumber   INT           DEFAULT 1,
                        rejectionReason NVARCHAR(500),
                        source          NVARCHAR(100) DEFAULT 'device-manager',

                        ngaySync        DATETIME      DEFAULT GETDATE(),

                    );
                    CREATE INDEX IX_ChamCong_Device_mapNV_thoiGian
                        ON ChamCong_Device (mapNV, thoiGian);
                    CREATE INDEX IX_ChamCong_Device_Valid
                        ON ChamCong_Device (mapNV, thoiGian, isValid, loai);
                    -- Filtered unique index: chỉ enforce khi dmLogId IS NOT NULL (webhook push để NULL)
                    CREATE UNIQUE INDEX UQ_ChamCong_Device_DmLogId
                        ON ChamCong_Device (dmLogId) WHERE dmLogId IS NOT NULL;
                END
                ELSE
                BEGIN
                    -- Migration: đổi dmLogId từ INT → NVARCHAR(100) nếu cần
                    IF EXISTS (
                        SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                        WHERE TABLE_NAME = 'ChamCong_Device'
                          AND COLUMN_NAME = 'dmLogId'
                          AND DATA_TYPE = 'int'
                    )
                    BEGIN
                        ALTER TABLE ChamCong_Device DROP CONSTRAINT IF EXISTS UQ_ChamCong_Device_DmLogId;
                        ALTER TABLE ChamCong_Device ALTER COLUMN dmLogId NVARCHAR(100);
                    END

                    -- Migration: đổi UNIQUE CONSTRAINT → filtered UNIQUE INDEX (cho phép nhiều NULL)
                    IF EXISTS (
                        SELECT 1 FROM sys.objects
                        WHERE name = 'UQ_ChamCong_Device_DmLogId' AND type = 'UQ'
                    )
                    BEGIN
                        ALTER TABLE ChamCong_Device DROP CONSTRAINT UQ_ChamCong_Device_DmLogId;
                    END
                    IF NOT EXISTS (
                        SELECT 1 FROM sys.indexes
                        WHERE name = 'UQ_ChamCong_Device_DmLogId'
                          AND object_id = OBJECT_ID('ChamCong_Device')
                    )
                    BEGIN
                        CREATE UNIQUE INDEX UQ_ChamCong_Device_DmLogId
                            ON ChamCong_Device (dmLogId) WHERE dmLogId IS NOT NULL;
                    END
                END";

            using (var cmd = new SqlCommand(ddl, conn))
                cmd.ExecuteNonQuery();
        }

        // ─── Internal record struct ───────────────────────────────────────────────

        private class ChamCongDeviceRecord
        {
            public string   MapNV           { get; set; }
            public string   Loai            { get; set; }
            public DateTime ThoiGian        { get; set; }
            public string   DeviceId        { get; set; }
            public string   DeviceName      { get; set; }
            public double?  DiemTin         { get; set; }
            public double?  Latitude        { get; set; }
            public double?  Longitude       { get; set; }
            public double?  GpsAccuracy     { get; set; }
            public bool     IsValid         { get; set; }
            public bool     IsDuplicate     { get; set; }
            public int      AttemptNumber   { get; set; }
            public string   RejectionReason { get; set; }
            public string   Source          { get; set; }
        }
    }
}
