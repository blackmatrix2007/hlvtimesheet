using System;
using System.Data;
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

                if (result?.Success == true)
                {
                    lblResult.Text = Alert("success",
                        $"Đã đăng ký khuôn mặt cho <strong>{HttpUtility.HtmlEncode(fullName)}</strong> ({employeeCode}) thành công.");
                    // Refresh danh sách
                    Task.Run(async () => await RenderListAsync()).GetAwaiter().GetResult();
                }
                else
                {
                    lblResult.Text = Alert("danger",
                        $"DeviceManager trả về lỗi: {HttpUtility.HtmlEncode(result?.Message ?? "Không rõ")}");
                }
            }
            catch (Exception ex)
            {
                lblResult.Text = Alert("danger", $"Lỗi kết nối DeviceManager: {HttpUtility.HtmlEncode(ex.Message)}");
            }
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

            // Lấy danh sách đã đăng ký từ DeviceManager
            System.Collections.Generic.HashSet<string> registered =
                new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var dmList = await svc.GetAllEmployeesFromDeviceAsync(limit: 500);
                foreach (var emp in dmList)
                    if (!string.IsNullOrEmpty(emp.EmployeeCode))
                        registered.Add(emp.EmployeeCode);
            }
            catch { /* DeviceManager không kết nối được → bỏ qua */ }

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
                sb.AppendFormat(
                    "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td>" +
                    "<td><span class='{4}'>{5}</span></td>" +
                    "<td><a href='DeviceFaceRegister.aspx' onclick=\"selectEmployee('{0}');return false;\" class='btn btn-xs btn-outline-primary btn-sm'>Upload ảnh</a></td></tr>",
                    HttpUtility.HtmlEncode(code),
                    HttpUtility.HtmlEncode(row["hoTen"].ToString()),
                    HttpUtility.HtmlEncode(row["phongBan"].ToString()),
                    HttpUtility.HtmlEncode(row["chucVu"].ToString()),
                    hasface ? "badge-ok" : "badge-no",
                    hasface ? "Đã đăng ký" : "Chưa đăng ký");
            }

            sb.Append("</tbody></table>");
            litList.Text = sb.ToString();
        }

        // ─── Helper ───────────────────────────────────────────────────────────────

        private static string Alert(string type, string msg)
            => $"<div class='alert alert-{type} mt-2'>{msg}</div>";
    }
}
