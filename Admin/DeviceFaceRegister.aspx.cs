using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLVTimeSheet.Model.DeviceManager;

namespace HLVTimeSheet.Admin
{
    public partial class DeviceFaceRegister : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadNhanVienDropdown();
                Task.Run(async () => await RenderListAsync()).GetAwaiter().GetResult();
            }
        }

        // ─── Load dropdown nhân viên từ DB ───────────────────────────────────────

        private void LoadNhanVienDropdown()
        {
            ddlNhanVien.Items.Clear();
            ddlNhanVien.Items.Add(new ListItem("-- Chọn nhân viên --", ""));

            var svc = new EmployeeSyncService();
            DataTable dt = svc.GetActiveEmployeesFromDb();
            foreach (DataRow row in dt.Rows)
            {
                string code  = row["mapNV"].ToString();
                string label = $"{code} — {row["hoTen"]} ({row["phongBan"]})";
                ddlNhanVien.Items.Add(new ListItem(label, code));
            }
        }

        // ─── Upload ảnh & đăng ký lên DeviceManager ──────────────────────────────

        protected void BtnUpload_Click(object sender, EventArgs e)
        {
            lblResult.Text = "";

            string employeeCode = ddlNhanVien.SelectedValue;
            if (string.IsNullOrEmpty(employeeCode))
            {
                lblResult.Text = Alert("warning", "Vui lòng chọn nhân viên.");
                return;
            }

            if (!fuAnh.HasFile)
            {
                lblResult.Text = Alert("warning", "Vui lòng chọn file ảnh.");
                return;
            }

            long maxBytes = 5 * 1024 * 1024; // 5 MB
            if (fuAnh.PostedFile.ContentLength > maxBytes)
            {
                lblResult.Text = Alert("warning", "Ảnh vượt quá 5 MB.");
                return;
            }

            string mime = fuAnh.PostedFile.ContentType;
            if (!mime.StartsWith("image/"))
            {
                lblResult.Text = Alert("warning", "Chỉ chấp nhận file ảnh JPEG hoặc PNG.");
                return;
            }

            // Đọc bytes
            byte[] imageBytes = new byte[fuAnh.PostedFile.ContentLength];
            fuAnh.PostedFile.InputStream.Read(imageBytes, 0, imageBytes.Length);
            string fileName = fuAnh.FileName;

            // Lấy thông tin nhân viên để điền fullName, department, position
            string fullName = "", department = "", position = "";
            var svc = new EmployeeSyncService();
            DataTable dt = svc.GetActiveEmployeesFromDb();
            foreach (DataRow row in dt.Rows)
            {
                if (row["mapNV"].ToString() == employeeCode)
                {
                    fullName   = row["hoTen"].ToString();
                    department = row["phongBan"].ToString();
                    position   = row["chucVu"].ToString();
                    break;
                }
            }

            // Gọi DeviceManager API
            try
            {
                var result = Task.Run(async () =>
                    await svc.RegisterEmployeeFaceAsync(
                        employeeCode, fullName, imageBytes, fileName,
                        department, position)
                ).GetAwaiter().GetResult();

                var logMsg = $"RegisterFace [{employeeCode}]: IsSuccess={result?.IsSuccess}, Message={result?.Message}, Duplicates={result?.Duplicates?.Count ?? 0}";
                WriteLog(logMsg);
                Debug.WriteLine("[DeviceFaceRegister] " + logMsg);

                if (result?.IsSuccess == true)
                {
                    lblResult.Text = Alert("success",
                        $"Đã đăng ký khuôn mặt cho <strong>{HttpUtility.HtmlEncode(fullName)}</strong> ({employeeCode}) thành công.");
                    Task.Run(async () => await RenderListAsync()).GetAwaiter().GetResult();
                }
                else if (result?.Duplicates?.Count > 0)
                {
                    var dup = result.Duplicates[0];
                    lblResult.Text = Alert("warning",
                        $"Ảnh trùng với nhân viên đã đăng ký: <strong>{HttpUtility.HtmlEncode(dup.FullName)}</strong> ({HttpUtility.HtmlEncode(dup.EmployeeCode)}) — độ tương đồng {HttpUtility.HtmlEncode(dup.Similarity)}. Vui lòng dùng ảnh khác.");
                }
                else
                {
                    lblResult.Text = Alert("danger",
                        $"DeviceManager: {HttpUtility.HtmlEncode(result?.Message ?? "Không rõ")}");
                }
            }
            catch (Exception ex)
            {
                WriteLog($"RegisterFace EXCEPTION [{employeeCode}]: {ex}");
                lblResult.Text = Alert("danger", $"Lỗi kết nối DeviceManager: {HttpUtility.HtmlEncode(ex.Message)}");
            }
        }

        // ─── Xóa khuôn mặt ───────────────────────────────────────────────────────

        protected void BtnRemoveFace_Click(object sender, EventArgs e)
        {
            string code = hdnRemoveCode.Value;
            if (string.IsNullOrEmpty(code)) return;

            try
            {
                var svc = new EmployeeSyncService();
                bool ok = Task.Run(async () => await svc.DeleteEmployeeAsync(code)).GetAwaiter().GetResult();
                var logMsg = $"DeleteFace [{code}]: ok={ok}";
                WriteLog(logMsg);
                Debug.WriteLine("[DeviceFaceRegister] " + logMsg);
                lblRemoveResult.Text = ok
                    ? Alert("success", $"Đã xóa khuôn mặt của <strong>{HttpUtility.HtmlEncode(code)}</strong> khỏi DeviceManager.")
                    : Alert("warning", $"DeviceManager không xác nhận xóa <strong>{HttpUtility.HtmlEncode(code)}</strong>.");
            }
            catch (Exception ex)
            {
                WriteLog($"DeleteFace EXCEPTION [{code}]: {ex.Message}");
                Debug.WriteLine($"[DeviceFaceRegister] DeleteFace EXCEPTION [{code}]: {ex.Message}");
                lblRemoveResult.Text = Alert("danger", $"Lỗi: {HttpUtility.HtmlEncode(ex.Message)}");
            }

            Task.Run(async () => await RenderListAsync()).GetAwaiter().GetResult();
        }

        // ─── Refresh danh sách ────────────────────────────────────────────────────

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            Task.Run(async () => await RenderListAsync()).GetAwaiter().GetResult();
        }

        private async Task RenderListAsync()
        {
            var svc     = new EmployeeSyncService();
            DataTable dt = svc.GetActiveEmployeesFromDb();

            // Build image base URL từ config (bỏ /api suffix, giống FaceGallery.jsx)
            var cfg = DeviceManagerConfig.Load();
            string imageBaseUrl = (cfg.BaseUrl ?? "https://device.erp-x.com/api").TrimEnd('/');
            if (imageBaseUrl.EndsWith("/api", StringComparison.OrdinalIgnoreCase))
                imageBaseUrl = imageBaseUrl.Substring(0, imageBaseUrl.Length - 4);

            // Lấy danh sách đã đăng ký từ DeviceManager (code → URL ảnh đầu tiên)
            var faceUrls = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var dmList = await svc.GetAllEmployeesFromDeviceAsync(limit: 500);
                foreach (var emp in dmList)
                    if (!string.IsNullOrEmpty(emp.EmployeeCode))
                        faceUrls[emp.EmployeeCode] = emp.FaceImages?.Count > 0
                            ? emp.FaceImages[0].ImageUrl ?? ""
                            : "";
            }
            catch { /* DeviceManager không kết nối được → bỏ qua */ }

            var registered = new System.Collections.Generic.HashSet<string>(faceUrls.Keys, StringComparer.OrdinalIgnoreCase);

            var sb = new StringBuilder();
            sb.AppendFormat("<p class='text-muted mb-2'>Tổng DB: <strong>{0}</strong> nhân viên — " +
                            "Đã đăng ký khuôn mặt: <strong>{1}</strong></p>",
                dt.Rows.Count, registered.Count);

            sb.Append("<table class='table table-sm table-bordered table-hover'>");
            sb.Append("<thead class='thead-light'>" +
                      "<tr><th>Mã NV</th><th>Họ tên</th><th>Phòng ban</th><th>Chức vụ</th>" +
                      "<th>Khuôn mặt</th><th>Thao tác</th></tr></thead><tbody>");

            foreach (DataRow row in dt.Rows)
            {
                string code    = row["mapNV"].ToString();
                bool   hasface = registered.Contains(code);
                string imgUrl  = hasface && faceUrls.TryGetValue(code, out string u) ? u : "";
                // Load ảnh trực tiếp từ device.erp-x.com (giống FaceGallery.jsx)
                if (!string.IsNullOrEmpty(imgUrl) && imgUrl.StartsWith("/"))
                    imgUrl = imageBaseUrl + imgUrl;

                string faceCell = hasface
                    ? (string.IsNullOrEmpty(imgUrl)
                        ? "<span class='badge-ok'>Đã đăng ký</span>"
                        : $"<img src='{HttpUtility.HtmlAttributeEncode(imgUrl)}' style='width:52px;height:52px;object-fit:cover;border-radius:6px;border:2px solid #28a745;vertical-align:middle' onerror=\"this.style.display='none'\" /> <span class='badge-ok' style='vertical-align:middle'>Đã đăng ký</span>")
                    : "<span class='badge-no'>Chưa đăng ký</span>";

                string actions = $"<a href='#' onclick=\"var d=document.getElementById('{ddlNhanVien.ClientID}');d.value='{HttpUtility.JavaScriptStringEncode(code)}';window.scrollTo({{top:0,behavior:'smooth'}});d.focus();return false;\" class='btn btn-xs btn-outline-primary btn-sm mr-1'>Upload ảnh</a>";
                if (hasface)
                    actions += $"<a href='#' onclick=\"removeFace('{HttpUtility.JavaScriptStringEncode(code)}');return false;\" class='btn btn-xs btn-outline-danger btn-sm'>Xóa khuôn mặt</a>";

                sb.AppendFormat(
                    "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td>" +
                    "<td>{4}</td><td>{5}</td></tr>",
                    HttpUtility.HtmlEncode(code),
                    HttpUtility.HtmlEncode(row["hoTen"].ToString()),
                    HttpUtility.HtmlEncode(row["phongBan"].ToString()),
                    HttpUtility.HtmlEncode(row["chucVu"].ToString()),
                    faceCell,
                    actions);
            }

            sb.Append("</tbody></table>");
            litList.Text = sb.ToString();
        }

        // ─── Helper ───────────────────────────────────────────────────────────────

        private static string Alert(string type, string msg)
            => $"<div class='alert alert-{type} mt-2'>{msg}</div>";

        private void WriteLog(string message)
        {
            try
            {
                string logDir  = Server.MapPath("~/App_Data/logs");
                if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);
                string logFile = Path.Combine(logDir, $"device_{DateTime.Today:yyyyMMdd}.log");
                string line    = $"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}";
                File.AppendAllText(logFile, line, Encoding.UTF8);
            }
            catch { /* không để log lỗi làm crash app */ }
        }
    }
}
