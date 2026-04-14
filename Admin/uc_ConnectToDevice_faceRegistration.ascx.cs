using HLVTimeSheet.Model.DeviceManager;
using static HLVTimeSheet.Model.DeviceManager.DeviceManagerLogger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HLVTimeSheet.Admin
{
    public partial class uc_ConnectToDevice_faceRegistration : System.Web.UI.UserControl
    {
        public string[] quyen = new string[] { "0", "0", "0", "0", "0", "0" };

        public string language = "1";
        protected void Page_Load(object sender, EventArgs e)
        {
            string lang = Request.QueryString["lang"];
            if (lang != null)
            {
                Session["language"] = lang;
                language = lang;
            }
            else if (Session["language"] != null)
            {
                language = Session["language"].ToString();
            }

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
            Debug.WriteLine($"[DeviceFaceRegister] LoadDropdown: {dt.Rows.Count} nhân viên từ DB");
            foreach (DataRow row in dt.Rows)
            {
                string code = row["mapNV"].ToString();
                string label = $"{code} — {row["hoTen"]} ({row["phongBan"]})";
                ddlNhanVien.Items.Add(new ListItem(label, code));
            }
        }

        // ─── Upload ảnh & đăng ký lên DeviceManager ──────────────────────────────

        protected void BtnUpload_Click(object sender, EventArgs e)
        {
            lblResult.Text = "";

            string employeeCode = ddlNhanVien.SelectedValue;
            Debug.WriteLine($"[DeviceFaceRegister] BtnUpload_Click: employeeCode={employeeCode}, hasFile={fuAnh.HasFile}");
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
            Debug.WriteLine($"[DeviceFaceRegister] Upload: file={fileName}, size={imageBytes.Length}, mime={mime}");

            // Lấy thông tin nhân viên để điền fullName, department, position
            string fullName = "", department = "", position = "";
            var svc = new EmployeeSyncService();
            DataTable dt = svc.GetActiveEmployeesFromDb();
            foreach (DataRow row in dt.Rows)
            {
                if (row["mapNV"].ToString() == employeeCode)
                {
                    fullName = row["hoTen"].ToString();
                    department = row["phongBan"].ToString();
                    position = row["chucVu"].ToString();
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
                DeviceManagerLogger.Log("FACE-REG", logMsg);
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
                DeviceManagerLogger.LogError("FACE-REG", $"RegisterFace [{employeeCode}]", ex);
                Debug.WriteLine($"[DeviceFaceRegister] RegisterFace EXCEPTION [{employeeCode}]: {ex}");
                lblResult.Text = Alert("danger", $"Lỗi kết nối DeviceManager: {HttpUtility.HtmlEncode(ex.Message)}");
            }
        }

        // ─── Xóa khuôn mặt ───────────────────────────────────────────────────────

        protected void BtnRemoveFace_Click(object sender, EventArgs e)
        {
            string code = hdnRemoveCode.Value;
            Debug.WriteLine($"[DeviceFaceRegister] BtnRemoveFace_Click: code={code}");
            if (string.IsNullOrEmpty(code)) return;

            try
            {
                var svc = new EmployeeSyncService();
                bool ok = Task.Run(async () => await svc.DeleteEmployeeAsync(code)).GetAwaiter().GetResult();
                var logMsg = $"DeleteFace [{code}]: ok={ok}";
                DeviceManagerLogger.Log("FACE-REG", logMsg);
                Debug.WriteLine("[DeviceFaceRegister] " + logMsg);
                lblRemoveResult.Text = ok
                    ? Alert("success", $"Đã xóa khuôn mặt của <strong>{HttpUtility.HtmlEncode(code)}</strong> khỏi DeviceManager.")
                    : Alert("warning", $"DeviceManager không xác nhận xóa <strong>{HttpUtility.HtmlEncode(code)}</strong>.");
            }
            catch (Exception ex)
            {
                DeviceManagerLogger.LogError("FACE-REG", $"DeleteFace [{code}]", ex);
                Debug.WriteLine($"[DeviceFaceRegister] DeleteFace EXCEPTION [{code}]: {ex.Message}");
                lblRemoveResult.Text = Alert("danger", $"Lỗi: {HttpUtility.HtmlEncode(ex.Message)}");
            }

            Task.Run(async () => await RenderListAsync()).GetAwaiter().GetResult();
        }

        // ─── Đồng bộ avatar từ hlv-ws lên DeviceManager ─────────────────────────

        protected void BtnSyncAvatar_Click(object sender, EventArgs e)
        {
            string code = hdnSyncCode.Value;
            Debug.WriteLine($"[DeviceFaceRegister] BtnSyncAvatar_Click: code={code}");
            if (string.IsNullOrEmpty(code)) return;

            lblSyncResult.Text = "";
            try
            {
                // Lấy thông tin nhân viên + hinhanh từ DB
                var svc = new EmployeeSyncService();
                DataTable dt = svc.GetActiveEmployeesFromDb();
                string fullName = "", department = "", position = "", hinhanh = "";
                foreach (DataRow row in dt.Rows)
                {
                    if (row["mapNV"].ToString() == code)
                    {
                        fullName   = row["hoTen"].ToString();
                        department = row["phongBan"].ToString();
                        position   = row["chucVu"].ToString();
                        hinhanh    = row["hinhanh"].ToString();
                        break;
                    }
                }

                if (string.IsNullOrEmpty(hinhanh))
                {
                    lblSyncResult.Text = Alert("warning", $"Nhân viên <strong>{HttpUtility.HtmlEncode(code)}</strong> chưa có ảnh avatar trong hệ thống.");
                    return;
                }

                // Tải ảnh từ hlv-ws
                string avatarUrl = $"http://hlv-ws.giangdc.company/Admin/Avatar/{Uri.EscapeDataString(hinhanh)}";
                byte[] imageBytes;
                string fileName;
                string mimeType;
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(15);
                    var response = Task.Run(async () => await httpClient.GetAsync(avatarUrl)).GetAwaiter().GetResult();
                    string respContentType = response.Content.Headers.ContentType?.MediaType ?? "";
                    if (!response.IsSuccessStatusCode)
                    {
                        DeviceManagerLogger.Log("FACE-REG", $"SyncAvatar [{code}]: HTTP {(int)response.StatusCode} từ {avatarUrl}");
                        lblSyncResult.Text = Alert("danger", $"Không tải được ảnh avatar ({response.StatusCode}): {HttpUtility.HtmlEncode(avatarUrl)}");
                        return;
                    }
                    imageBytes = Task.Run(async () => await response.Content.ReadAsByteArrayAsync()).GetAwaiter().GetResult();
                    fileName   = System.IO.Path.GetFileName(hinhanh);

                    // Phát hiện MIME type từ magic bytes (ưu tiên hơn Content-Type header)
                    mimeType = DetectMimeType(imageBytes, fileName);

                    string magic4 = imageBytes.Length >= 4 ? BitConverter.ToString(imageBytes, 0, 4) : "?";
                    DeviceManagerLogger.Log("FACE-REG", $"SyncAvatar [{code}]: url={avatarUrl}, httpContentType={respContentType}, detectedMime={mimeType}, size={imageBytes.Length} bytes, magic={magic4}");

                    // Kiểm tra có phải ảnh không (magic bytes)
                    if (!mimeType.StartsWith("image/"))
                    {
                        lblSyncResult.Text = Alert("danger", $"File tải về không phải ảnh (magic={magic4}, contentType={respContentType}). Có thể server trả về HTML. URL: {HttpUtility.HtmlEncode(avatarUrl)}");
                        return;
                    }
                }

                // Đăng ký lên DeviceManager
                var result = Task.Run(async () =>
                    await svc.RegisterEmployeeFaceAsync(code, fullName, imageBytes, fileName, department, position, mimeType: mimeType)
                ).GetAwaiter().GetResult();

                var logMsg = $"SyncAvatar [{code}]: IsSuccess={result?.IsSuccess}, Message={result?.Message}";
                DeviceManagerLogger.Log("FACE-REG", logMsg);
                Debug.WriteLine("[DeviceFaceRegister] " + logMsg);

                if (result?.IsSuccess == true)
                {
                    lblSyncResult.Text = Alert("success",
                        $"Đã đồng bộ avatar của <strong>{HttpUtility.HtmlEncode(fullName)}</strong> ({code}) lên DeviceManager thành công.");
                    Task.Run(async () => await RenderListAsync()).GetAwaiter().GetResult();
                }
                else if (result?.Duplicates?.Count > 0)
                {
                    var dup = result.Duplicates[0];
                    lblSyncResult.Text = Alert("warning",
                        $"Ảnh trùng với <strong>{HttpUtility.HtmlEncode(dup.FullName)}</strong> ({HttpUtility.HtmlEncode(dup.EmployeeCode)}) — độ tương đồng {HttpUtility.HtmlEncode(dup.Similarity)}.");
                }
                else
                {
                    lblSyncResult.Text = Alert("danger",
                        $"DeviceManager: {HttpUtility.HtmlEncode(result?.Message ?? "Không rõ")}");
                }
            }
            catch (Exception ex)
            {
                DeviceManagerLogger.LogError("FACE-REG", $"SyncAvatar [{code}]", ex);
                Debug.WriteLine($"[DeviceFaceRegister] SyncAvatar EXCEPTION [{code}]: {ex.Message}");
                lblSyncResult.Text = Alert("danger", $"Lỗi: {HttpUtility.HtmlEncode(ex.Message)}");
            }
        }

        // ─── Refresh danh sách ────────────────────────────────────────────────────

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            Task.Run(async () => await RenderListAsync()).GetAwaiter().GetResult();
        }

        private async Task RenderListAsync()
        {
            var svc = new EmployeeSyncService();
            DataTable dt = svc.GetActiveEmployeesFromDb();

            // Lấy danh sách đã đăng ký từ DeviceManager (code → URL ảnh đầu tiên)
            var faceUrls = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var dmList = await svc.GetAllEmployeesFromDeviceAsync(limit: 500);
                Debug.WriteLine($"[DeviceFaceRegister] RenderList: {dmList.Count} nhân viên từ DeviceManager");
                foreach (var emp in dmList)
                    if (!string.IsNullOrEmpty(emp.EmployeeCode))
                    {
                        string imgPath = emp.FaceImages?.Count > 0 ? emp.FaceImages[0].ImageUrl ?? "" : "";
                        faceUrls[emp.EmployeeCode] = imgPath;
                        Debug.WriteLine($"[DeviceFaceRegister]   {emp.EmployeeCode} → faceUrl={imgPath}");
                    }
            }
            catch (Exception ex) { Debug.WriteLine($"[DeviceFaceRegister] RenderList DeviceManager ERROR: {ex.Message}"); }

            var registered = new System.Collections.Generic.HashSet<string>(faceUrls.Keys, StringComparer.OrdinalIgnoreCase);

            var sb = new StringBuilder();
            sb.AppendFormat("<p class='text-muted mb-2'>Tổng: <strong>{0}</strong> nhân viên — " +
                            "Đã đăng ký khuôn mặt: <strong>{1}</strong></p>",
                dt.Rows.Count, registered.Count);

            sb.Append("<table class='table table-hover table-bordered table-striped' style='font-size:small'>");
            sb.Append("<tr> " +
                    " <th style='text-align:center; width:3%;'>#</th> " +
                    " <th style='text-align:center; width:8%;'>Code</th> " +
                    " <th style='text-align:center; width:15%;'>Name</th> " +
                    " <th style='text-align:center; width:10%;'>Department</th> " +
                    " <th style='text-align:center; width:8%;'>Avatar HLV-WS</th> " +
                    " <th style='text-align:center; width:10%;'>Khuôn mặt Device</th> " +
                    " <th style='text-align:center; width:18%;'>Action</th> " +
                    "</tr><tbody>");
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                string code     = row["mapNV"].ToString();
                string hinhanh  = row["hinhanh"].ToString();
                bool hasface    = registered.Contains(code);
                string imgUrl   = hasface && faceUrls.TryGetValue(code, out string u) ? u : "";
                if (!string.IsNullOrEmpty(imgUrl) && imgUrl.StartsWith("/"))
                    imgUrl = "https://device.erp-x.com" + imgUrl;

                // Cột avatar từ hlv-ws
                string avatarUrl = string.IsNullOrEmpty(hinhanh) ? "" :
                    $"http://hlv-ws.giangdc.company/Admin/Avatar/{Uri.EscapeUriString(hinhanh)}";
                string avatarCell = string.IsNullOrEmpty(avatarUrl)
                    ? "<span class='text-muted'>—</span>"
                    : $"<img src='{HttpUtility.HtmlAttributeEncode(avatarUrl)}' style='width:48px;height:48px;object-fit:cover;border-radius:6px;border:1px solid #ccc;' onerror=\"this.outerHTML='<span class=text-muted>Lỗi ảnh</span>'\" />";

                // Cột khuôn mặt đã đăng ký trên Device
                string faceCell = hasface
                    ? (string.IsNullOrEmpty(imgUrl)
                        ? "<span class='badge-ok'>Đã đăng ký</span>"
                        : $"<img src='{HttpUtility.HtmlAttributeEncode(imgUrl)}' style='width:48px;height:48px;object-fit:cover;border-radius:6px;border:2px solid #28a745;' onerror=\"this.style.display='none'\" /> <span class='badge-ok'>Đã đăng ký</span>")
                    : "<span class='badge-no'>Chưa đăng ký</span>";

                // Actions
                string actions = $"<a href='#' onclick=\"var d=document.getElementById('{ddlNhanVien.ClientID}');d.value='{HttpUtility.JavaScriptStringEncode(code)}';window.scrollTo({{top:0,behavior:'smooth'}});d.focus();return false;\" class='btn btn-xs btn-outline-primary btn-sm mr-1'>Upload ảnh</a>";
                if (!string.IsNullOrEmpty(hinhanh))
                    actions += $" <a href='#' onclick=\"syncAvatar('{HttpUtility.JavaScriptStringEncode(code)}');return false;\" class='btn btn-xs btn-outline-success btn-sm mr-1' title='Dùng avatar HLV-WS làm khuôn mặt nhận diện'>Đồng bộ avatar</a>";
                if (hasface)
                    actions += $" <a href='#' style='color:red;' onclick=\"removeFace('{HttpUtility.JavaScriptStringEncode(code)}');return false;\" class='btn btn-xs btn-outline-danger btn-sm'>Xóa khuôn mặt</a>";
                i++;
                sb.AppendFormat(
                    "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td>" +
                    "<td style='text-align:center'>{4}</td><td>{5}</td><td>{6}</td></tr>",
                    HttpUtility.HtmlEncode(i.ToString()),
                    HttpUtility.HtmlEncode(code),
                    HttpUtility.HtmlEncode(row["hoTen"].ToString()),
                    HttpUtility.HtmlEncode(row["phongBan"].ToString()),
                    avatarCell,
                    faceCell,
                    actions);
            }

            sb.Append("</tbody></table>");
            litList.Text = sb.ToString();
        }

        // ─── Helper ───────────────────────────────────────────────────────────────

        private static string Alert(string type, string msg)
            => $"<div class='alert alert-{type} mt-2'>{msg}</div>";

        /// <summary>Phát hiện MIME type từ magic bytes, fallback về extension.</summary>
        private static string DetectMimeType(byte[] bytes, string fileName)
        {
            if (bytes != null && bytes.Length >= 4)
            {
                // JPEG: FF D8 FF
                if (bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF)
                    return "image/jpeg";
                // PNG: 89 50 4E 47
                if (bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
                    return "image/png";
                // GIF: 47 49 46 38
                if (bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46)
                    return "image/gif";
                // WebP: 52 49 46 46 ... 57 45 42 50
                if (bytes.Length >= 12 && bytes[0] == 0x52 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x46)
                    return "image/webp";
            }
            // Fallback theo extension
            string ext = System.IO.Path.GetExtension(fileName ?? "").ToLowerInvariant();
            if (ext == ".jpg" || ext == ".jpeg") return "image/jpeg";
            if (ext == ".png") return "image/png";
            if (ext == ".gif") return "image/gif";
            if (ext == ".webp") return "image/webp";
            return "application/octet-stream"; // không phải ảnh
        }


    }
}