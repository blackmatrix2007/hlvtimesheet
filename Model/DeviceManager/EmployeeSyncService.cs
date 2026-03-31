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
    ///
    /// Tham khảo: source/business-service-nodejs/src/modules/device-manager/device-manager.service.ts
    /// </summary>
    public class EmployeeSyncService
    {
        private readonly DeviceManagerApiClient _client;
        private readonly DeviceManagerConfig _config;

        public EmployeeSyncService(string chiNhanhId = null)
        {
            _config = DeviceManagerConfig.Load(chiNhanhId);
            _client = new DeviceManagerApiClient(_config);
        }

        // ─── Lấy danh sách nhân viên từ DeviceManager ────────────────────────────

        public async Task<List<DmEmployee>> GetAllEmployeesFromDeviceAsync(int page = 1, int limit = 200)
        {
            // API trả về { "code": 200, "body": [...], "page": {...} } — không phải { "data": ... }
            var endpoint = $"employees/{_config.CustomerId}?page={page}&limit={limit}";
            var result = await _client.GetAsync<ApiListResponse<DmEmployee>>(endpoint);
            return result?.Body ?? new List<DmEmployee>();
        }

        public async Task<DmEmployee> GetEmployeeFromDeviceAsync(string employeeCode)
        {
            // API trả về { "code": 200, "body": { employee } }
            var endpoint = $"employees/{_config.CustomerId}/{employeeCode}";
            var result = await _client.GetAsync<ApiListResponse<DmEmployee>>(endpoint);
            return result?.Body?.Count > 0 ? result.Body[0] : null;
        }

        // ─── Đăng ký khuôn mặt ───────────────────────────────────────────────────

        /// <summary>
        /// Đăng ký ảnh khuôn mặt nhân viên lên DeviceManager.
        /// Tương đương registerFace() trong device-manager.service.ts.
        /// </summary>
        public async Task<RegisterFaceResponse> RegisterEmployeeFaceAsync(
            string employeeCode,
            string fullName,
            byte[] faceImageBytes,
            string fileName,
            string department = null,
            string position   = null,
            string email      = null,
            string phone      = null)
        {
            if (faceImageBytes == null || faceImageBytes.Length == 0)
                throw new ArgumentException("Dữ liệu ảnh khuôn mặt không được rỗng.");

            var endpoint = $"employees/{_config.CustomerId}/register-face";

            using (var form = new MultipartFormDataContent())
            {
                // Text fields TRƯỚC file — Multer đọc req.body.employeeCode trong destination()
                // chỉ khi text fields đến trước file trong multipart stream
                form.Add(new StringContent(employeeCode),      "employeeCode");
                form.Add(new StringContent(fullName ?? ""),    "fullName");
                form.Add(new StringContent("true"),            "isPrimary");

                if (!string.IsNullOrEmpty(department)) form.Add(new StringContent(department), "department");
                if (!string.IsNullOrEmpty(position))   form.Add(new StringContent(position),   "position");
                if (!string.IsNullOrEmpty(email))      form.Add(new StringContent(email),      "email");
                if (!string.IsNullOrEmpty(phone))      form.Add(new StringContent(phone),      "phone");

                var imageContent = new ByteArrayContent(faceImageBytes);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                form.Add(imageContent, "faceImage", fileName);

                return await _client.PostMultipartAsync<RegisterFaceResponse>(endpoint, form);
            }
        }

        // ─── Xóa nhân viên ───────────────────────────────────────────────────────

        /// <summary>
        /// Xóa nhân viên và toàn bộ ảnh khuôn mặt khỏi DeviceManager.
        /// Tương đương deleteEmployee() trong device-manager.service.ts.
        /// </summary>
        public async Task<bool> DeleteEmployeeAsync(string employeeCode)
        {
            var endpoint = $"employees/{_config.CustomerId}/{employeeCode}";
            var result = await _client.DeleteAsync<ApiResponse<object>>(endpoint);
            // API trả {"message":"Employee deleted successfully"} — không có "success"
            return result?.Message?.Contains("deleted") == true || result?.Success == true;
        }

        // ─── Sync tất cả nhân viên tới một thiết bị ──────────────────────────────

        public async Task<bool> SyncAllToDeviceAsync(string deviceId)
        {
            var endpoint = $"employees/{_config.CustomerId}/sync-to-device/{deviceId}";
            var result = await _client.PostAsync<ApiResponse<object>>(endpoint, new { });
            return result?.Success ?? false;
        }

        // ─── Lấy danh sách nhân viên từ SQL Server HLVTimeSheet ──────────────────

        /// <summary>
        /// Truy vấn nhân viên đang làm việc từ bảng DanhSachNhanSu.
        /// Cột: mapNV, hoTen, phongBan, chucVu, email, dienThoai.
        /// </summary>
        public DataTable GetActiveEmployeesFromDb()
        {
            var conn = new ConnectionDatabase();
            using (var sqlConn = new SqlConnection(conn.ReturnConnectionDatabase()))
            {
                sqlConn.Open();
                const string sql = @"
                    SELECT
                        ns.ma                                                                   AS mapNV,
                        ns.ten                                                                  AS hoTen,
                        ISNULL(pb.ten, '')                                                      AS phongBan,
                        ISNULL((SELECT cv.ten FROM ChucVu cv WHERE cv.pk_seq = ns.chucvu_fk),'') AS chucVu,
                        ISNULL(ns.mail, '')                                                     AS email,
                        ISNULL(ns.dienthoai, '')                                                AS dienThoai
                    FROM DanhSachNhanSu ns
                    LEFT JOIN PhongBan pb ON pb.pk_seq = ns.phongban_fk
                    WHERE ns.trangthai = 1
                    ORDER BY ns.capbac, ns.ma ";

                using (var cmd = new SqlCommand(sql, sqlConn))
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
