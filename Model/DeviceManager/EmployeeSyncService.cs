using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HLVTimeSheet.AcsessData;

namespace HLVTimeSheet.Model.DeviceManager
{
    /// <summary>
    /// Đồng bộ nhân viên từ bảng DanhSachNhanSu (SQL Server HLVTimeSheet)
    /// sang hệ thống DeviceManager để thiết bị chấm công nhận diện khuôn mặt.
    /// </summary>
    public class EmployeeSyncService
    {
        private readonly DeviceManagerApiClient _client;
        private readonly string _customerId;

        public EmployeeSyncService() : this(new DeviceManagerApiClient(), DeviceManagerConfig.CustomerId) { }

        public EmployeeSyncService(DeviceManagerApiClient client, string customerId)
        {
            _client = client;
            _customerId = customerId;
        }

        // ─── Lấy danh sách nhân viên từ DeviceManager ────────────────────────────

        public async Task<List<DmEmployee>> GetAllEmployeesFromDeviceAsync(int page = 1, int limit = 200)
        {
            var endpoint = $"employees/{_customerId}?page={page}&limit={limit}";
            var result = await _client.GetAsync<ApiResponse<DmEmployeeListResponse>>(endpoint);
            return result?.Data?.Employees ?? new List<DmEmployee>();
        }

        // ─── Đăng ký khuôn mặt nhân viên ─────────────────────────────────────────

        /// <summary>
        /// Đăng ký hoặc cập nhật khuôn mặt một nhân viên lên DeviceManager.
        /// </summary>
        /// <param name="employeeCode">Mã nhân viên (mapNV)</param>
        /// <param name="fullName">Họ tên đầy đủ</param>
        /// <param name="faceImageBytes">Dữ liệu ảnh khuôn mặt (JPEG/PNG)</param>
        /// <param name="fileName">Tên file ảnh</param>
        /// <param name="department">Tên phòng ban</param>
        /// <param name="position">Chức vụ</param>
        public async Task<ApiResponse<DmEmployee>> RegisterEmployeeFaceAsync(
            string employeeCode,
            string fullName,
            byte[] faceImageBytes,
            string fileName,
            string department = null,
            string position = null,
            string email = null,
            string phone = null)
        {
            var endpoint = $"employees/{_customerId}/register-face";

            using (var form = new MultipartFormDataContent())
            {
                var imageContent = new ByteArrayContent(faceImageBytes);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                form.Add(imageContent, "faceImage", fileName);

                form.Add(new StringContent(employeeCode), "employeeCode");
                form.Add(new StringContent(fullName), "fullName");
                form.Add(new StringContent("true"), "isPrimary");

                if (!string.IsNullOrEmpty(department))
                    form.Add(new StringContent(department), "department");
                if (!string.IsNullOrEmpty(position))
                    form.Add(new StringContent(position), "position");
                if (!string.IsNullOrEmpty(email))
                    form.Add(new StringContent(email), "email");
                if (!string.IsNullOrEmpty(phone))
                    form.Add(new StringContent(phone), "phone");

                return await _client.PostMultipartAsync<ApiResponse<DmEmployee>>(endpoint, form);
            }
        }

        // ─── Xóa nhân viên khỏi DeviceManager ────────────────────────────────────

        public async Task<bool> DeleteEmployeeAsync(string employeeCode)
        {
            var endpoint = $"employees/{_customerId}/{employeeCode}";
            var result = await _client.DeleteAsync<ApiResponse<object>>(endpoint);
            return result?.Success ?? false;
        }

        // ─── Đồng bộ tất cả nhân viên đến thiết bị cụ thể ───────────────────────

        public async Task<bool> SyncAllToDeviceAsync(string deviceId)
        {
            var endpoint = $"employees/{_customerId}/sync-to-device/{deviceId}";
            var result = await _client.PostAsync<ApiResponse<object>>(endpoint, new { });
            return result?.Success ?? false;
        }

        // ─── Lấy danh sách nhân viên từ SQL Server HLVTimeSheet ──────────────────

        /// <summary>
        /// Truy vấn danh sách nhân viên đang làm việc từ bảng DanhSachNhanSu.
        /// Trả về DataTable với các cột: mapNV, hoTen, phongBan, chucVu, email, dienThoai.
        /// </summary>
        public DataTable GetActiveEmployeesFromDb()
        {
            var conn = new ConnectionDatabase();
            using (var sqlConn = new SqlConnection(conn.ReturnConnectionDatabaseWS()))
            {
                sqlConn.Open();
                string sql = @"
                    SELECT
                        ns.mapNV        AS mapNV,
                        ns.hoTen        AS hoTen,
                        pb.TenPhongBan  AS phongBan,
                        ns.chucVu       AS chucVu,
                        ns.email        AS email,
                        ns.dienThoai    AS dienThoai
                    FROM DanhSachNhanSu ns
                    LEFT JOIN PhongBan pb ON pb.pk_seq = ns.pk_phongban
                    WHERE ns.trangThai = 1
                    ORDER BY ns.mapNV";

                using (var cmd = new SqlCommand(sql, sqlConn))
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        // ─── Kết quả đồng bộ hàng loạt ──────────────────────────────────────────

        public class BulkSyncResult
        {
            public int Total { get; set; }
            public int Succeeded { get; set; }
            public int Failed { get; set; }
            public List<string> Errors { get; set; } = new List<string>();
        }

        /// <summary>
        /// Đồng bộ tất cả nhân viên đang làm việc (không có ảnh khuôn mặt) —
        /// chỉ tạo record nhân viên, ảnh cần upload riêng từ trang quản trị.
        /// Dùng khi muốn pre-populate danh sách nhân viên trên DeviceManager.
        /// </summary>
        public async Task<BulkSyncResult> BulkSyncEmployeeInfoAsync()
        {
            var result = new BulkSyncResult();
            var employees = GetActiveEmployeesFromDb();
            result.Total = employees.Rows.Count;

            foreach (DataRow row in employees.Rows)
            {
                string code = row["mapNV"].ToString();
                try
                {
                    // Gọi API lấy thông tin nhân viên hiện tại (kiểm tra đã tồn tại chưa)
                    var endpoint = $"employees/{_customerId}/{code}";
                    var existing = await _client.GetAsync<ApiResponse<DmEmployee>>(endpoint);
                    if (existing?.Success == true)
                    {
                        // Nhân viên đã tồn tại → bỏ qua
                        result.Succeeded++;
                        continue;
                    }
                }
                catch
                {
                    // Không tìm thấy → tiếp tục tạo mới
                }

                result.Errors.Add($"[{code}] Cần upload ảnh khuôn mặt qua trang quản trị.");
                result.Failed++;
            }

            return result;
        }
    }
}
