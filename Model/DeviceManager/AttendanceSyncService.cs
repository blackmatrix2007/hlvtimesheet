using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HLVTimeSheet.AcsessData;

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

            // 4. Ghi vào SQL Server
            var conn = new ConnectionDatabase();
            using (var sqlConn = new SqlConnection(conn.ReturnConnectionDatabaseWS()))
            {
                sqlConn.Open();
                EnsureTableExists(sqlConn);

                int attemptNo = GetNextAttemptNumber(sqlConn, dto.EmployeeCode, thoiGian.Date);
                bool isDuplicate = attemptNo > 1;

                // Kiểm tra nếu đã có bản ghi hợp lệ cùng loại → đánh dấu duplicate
                bool isValid = !isDuplicate;
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
            var endpoint = $"attendance/external/check-ins" +
                           $"?companyId={_config.CustomerId}&startDate={startDate}&endDate={endDate}";
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
            const string sql = @"
                INSERT INTO ChamCong_Device
                    (mapNV, loai, thoiGian, deviceId, deviceName, diemTin,
                     latitude, longitude, gpsAccuracy,
                     isValid, isDuplicate, attemptNumber, rejectionReason, source)
                OUTPUT INSERTED.pk_seq
                VALUES
                    (@mapNV, @loai, @thoiGian, @deviceId, @deviceName, @diemTin,
                     @latitude, @longitude, @gpsAccuracy,
                     @isValid, @isDuplicate, @attemptNumber, @rejectionReason, @source)";

            using (var cmd = new SqlCommand(sql, conn))
            {
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
            // PULL: dùng dmLogId để tránh trùng lặp
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
                cmd.Parameters.AddWithValue("@dmLogId",  log.Id);
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
                        dmLogId         INT,                        -- ID từ DeviceManager (PULL)
                        mapNV           NVARCHAR(50)  NOT NULL,
                        loai            NVARCHAR(20)  NOT NULL,     -- check_in / check_out
                        thoiGian        DATETIME      NOT NULL,
                        deviceId        NVARCHAR(100),
                        deviceName      NVARCHAR(200),
                        diemTin         FLOAT,                      -- face confidence 0.0-1.0

                        -- GPS (từ source/attendance.entity.ts)
                        latitude        FLOAT,
                        longitude       FLOAT,
                        gpsAccuracy     FLOAT,

                        -- Audit trail (từ source/attendance.entity.ts)
                        isValid         BIT           DEFAULT 1,    -- bản ghi chính thức
                        isDuplicate     BIT           DEFAULT 0,
                        attemptNumber   INT           DEFAULT 1,    -- lần chấm thứ mấy trong ngày
                        rejectionReason NVARCHAR(500),
                        source          NVARCHAR(100) DEFAULT 'device-manager',

                        ngaySync        DATETIME      DEFAULT GETDATE(),

                        CONSTRAINT UQ_ChamCong_Device_DmLogId UNIQUE (dmLogId)
                    );
                    CREATE INDEX IX_ChamCong_Device_mapNV_thoiGian
                        ON ChamCong_Device (mapNV, thoiGian);
                    CREATE INDEX IX_ChamCong_Device_Valid
                        ON ChamCong_Device (mapNV, thoiGian, isValid, loai);
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
