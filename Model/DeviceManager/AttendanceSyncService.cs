using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using HLVTimeSheet.AcsessData;

namespace HLVTimeSheet.Model.DeviceManager
{
    /// <summary>
    /// Kéo dữ liệu chấm công từ DeviceManager (máy nhận diện khuôn mặt)
    /// và ghi vào bảng ChamCong_Device trong SQL Server HLVTimeSheet.
    ///
    /// Luồng:
    ///   DeviceManager API → AttendanceSyncService.PullAndSaveAsync()
    ///                     → Upsert vào bảng ChamCong_Device
    ///                     → TimeKeepingController đọc ChamCong_Device khi lập bảng công
    /// </summary>
    public class AttendanceSyncService
    {
        private readonly DeviceManagerApiClient _client;
        private readonly string _customerId;

        public AttendanceSyncService() : this(new DeviceManagerApiClient(), DeviceManagerConfig.CustomerId) { }

        public AttendanceSyncService(DeviceManagerApiClient client, string customerId)
        {
            _client = client;
            _customerId = customerId;
        }

        // ─── Lấy dữ liệu từ DeviceManager API ────────────────────────────────────

        /// <summary>
        /// Lấy bản tổng hợp chấm công hôm nay từ DeviceManager.
        /// </summary>
        public async Task<DmTodaySummary> GetTodaySummaryAsync()
        {
            var result = await _client.GetAsync<ApiResponse<DmTodaySummary>>("attendance/today/summary");
            return result?.Data;
        }

        /// <summary>
        /// Lấy danh sách check-in/check-out trong khoảng thời gian.
        /// </summary>
        /// <param name="startDate">yyyy-MM-dd</param>
        /// <param name="endDate">yyyy-MM-dd</param>
        /// <param name="employeeCode">null = tất cả nhân viên</param>
        public async Task<List<DmAttendanceLog>> GetCheckInsAsync(
            string startDate,
            string endDate,
            string employeeCode = null)
        {
            var endpoint = $"attendance/external/check-ins" +
                           $"?companyId={_customerId}&startDate={startDate}&endDate={endDate}";

            if (!string.IsNullOrEmpty(employeeCode))
                endpoint += $"&employeeCode={employeeCode}";

            var result = await _client.GetAsync<ApiResponse<DmCheckInListResponse>>(endpoint);
            return result?.Data?.CheckIns ?? new List<DmAttendanceLog>();
        }

        /// <summary>
        /// Lấy lịch sử chấm công theo nhân viên.
        /// </summary>
        public async Task<List<DmAttendanceLog>> GetByEmployeeAsync(
            string employeeCode,
            string fromDate = null,
            string toDate = null,
            int limit = 100)
        {
            var endpoint = $"attendance/employee/{employeeCode}?limit={limit}";
            if (!string.IsNullOrEmpty(fromDate)) endpoint += $"&from={fromDate}";
            if (!string.IsNullOrEmpty(toDate)) endpoint += $"&to={toDate}";

            var result = await _client.GetAsync<ApiResponse<List<DmAttendanceLog>>>(endpoint);
            return result?.Data ?? new List<DmAttendanceLog>();
        }

        // ─── Lưu vào SQL Server ───────────────────────────────────────────────────

        /// <summary>
        /// Kéo dữ liệu chấm công từ DeviceManager và upsert vào bảng ChamCong_Device.
        /// Tạo bảng tự động nếu chưa tồn tại.
        /// </summary>
        /// <param name="startDate">yyyy-MM-dd</param>
        /// <param name="endDate">yyyy-MM-dd</param>
        /// <returns>Số bản ghi được lưu/cập nhật</returns>
        public async Task<int> PullAndSaveAsync(string startDate, string endDate)
        {
            var logs = await GetCheckInsAsync(startDate, endDate);
            if (logs.Count == 0) return 0;

            var conn = new ConnectionDatabase();
            using (var sqlConn = new SqlConnection(conn.ReturnConnectionDatabaseWS()))
            {
                sqlConn.Open();

                // Tạo bảng nếu chưa có
                EnsureTableExists(sqlConn);

                int saved = 0;
                foreach (var log in logs)
                {
                    saved += UpsertLog(sqlConn, log);
                }

                return saved;
            }
        }

        // ─── Đọc dữ liệu đã lưu ──────────────────────────────────────────────────

        /// <summary>
        /// Lấy dữ liệu chấm công từ bảng ChamCong_Device cho một nhân viên trong tháng.
        /// </summary>
        public DataTable GetDeviceAttendanceByMonth(string mapNV, int nam, int thang)
        {
            var conn = new ConnectionDatabase();
            using (var sqlConn = new SqlConnection(conn.ReturnConnectionDatabaseWS()))
            {
                sqlConn.Open();
                EnsureTableExists(sqlConn);

                string sql = @"
                    SELECT
                        mapNV, loai, thoiGian, deviceId, diemTin
                    FROM ChamCong_Device
                    WHERE mapNV = @mapNV
                      AND YEAR(thoiGian) = @nam
                      AND MONTH(thoiGian) = @thang
                    ORDER BY thoiGian";

                using (var cmd = new SqlCommand(sql, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@mapNV", mapNV);
                    cmd.Parameters.AddWithValue("@nam", nam);
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

        /// <summary>
        /// Lấy giờ vào (check_in đầu tiên) và giờ ra (check_out cuối cùng) của nhân viên theo ngày.
        /// Trả về DataTable với cột: mapNV, ngay, gioVao, gioRa.
        /// </summary>
        public DataTable GetDailyCheckInOut(string mapNV, int nam, int thang)
        {
            var conn = new ConnectionDatabase();
            using (var sqlConn = new SqlConnection(conn.ReturnConnectionDatabaseWS()))
            {
                sqlConn.Open();
                EnsureTableExists(sqlConn);

                string sql = @"
                    SELECT
                        mapNV,
                        CAST(thoiGian AS DATE)              AS ngay,
                        MIN(CASE WHEN loai='check_in'  THEN thoiGian END) AS gioVao,
                        MAX(CASE WHEN loai='check_out' THEN thoiGian END) AS gioRa
                    FROM ChamCong_Device
                    WHERE mapNV = @mapNV
                      AND YEAR(thoiGian) = @nam
                      AND MONTH(thoiGian) = @thang
                    GROUP BY mapNV, CAST(thoiGian AS DATE)
                    ORDER BY ngay";

                using (var cmd = new SqlCommand(sql, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@mapNV", mapNV);
                    cmd.Parameters.AddWithValue("@nam", nam);
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

        // ─── Private helpers ──────────────────────────────────────────────────────

        private static void EnsureTableExists(SqlConnection conn)
        {
            string ddl = @"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'ChamCong_Device'
                )
                BEGIN
                    CREATE TABLE ChamCong_Device (
                        pk_seq      INT IDENTITY(1,1) PRIMARY KEY,
                        dmLogId     INT          NOT NULL,   -- ID từ DeviceManager
                        mapNV       NVARCHAR(50) NOT NULL,   -- Mã nhân viên
                        loai        NVARCHAR(20) NOT NULL,   -- 'check_in' / 'check_out'
                        thoiGian    DATETIME     NOT NULL,
                        deviceId    NVARCHAR(100),
                        diemTin     FLOAT,                  -- confidence_score
                        ngaySync    DATETIME     DEFAULT GETDATE(),
                        CONSTRAINT UQ_ChamCong_Device_DmLogId UNIQUE (dmLogId)
                    );
                    CREATE INDEX IX_ChamCong_Device_mapNV_thoiGian
                        ON ChamCong_Device (mapNV, thoiGian);
                END";

            using (var cmd = new SqlCommand(ddl, conn))
                cmd.ExecuteNonQuery();
        }

        private static int UpsertLog(SqlConnection conn, DmAttendanceLog log)
        {
            string sql = @"
                MERGE ChamCong_Device AS target
                USING (SELECT @dmLogId AS dmLogId) AS source ON target.dmLogId = source.dmLogId
                WHEN MATCHED THEN
                    UPDATE SET
                        loai     = @loai,
                        thoiGian = @thoiGian,
                        deviceId = @deviceId,
                        diemTin  = @diemTin,
                        ngaySync = GETDATE()
                WHEN NOT MATCHED THEN
                    INSERT (dmLogId, mapNV, loai, thoiGian, deviceId, diemTin)
                    VALUES (@dmLogId, @mapNV, @loai, @thoiGian, @deviceId, @diemTin);";

            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@dmLogId", log.Id);
                cmd.Parameters.AddWithValue("@mapNV", log.EmployeeCode ?? "");
                cmd.Parameters.AddWithValue("@loai", log.Type ?? "check_in");
                cmd.Parameters.AddWithValue("@thoiGian", log.Timestamp);
                cmd.Parameters.AddWithValue("@deviceId", (object)log.DeviceId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@diemTin", log.ConfidenceScore);
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
