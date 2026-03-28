using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using HLVTimeSheet.Model.DeviceManager;
using System.Web.UI.WebControls;

namespace HLVTimeSheet.Admin
{
    public partial class DeviceSync : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFrom.Text       = DateTime.Today.ToString("yyyy-MM-dd");
                txtTo.Text         = DateTime.Today.ToString("yyyy-MM-dd");
                txtImportFrom.Text = DateTime.Today.ToString("yyyy-MM-dd");
                txtImportTo.Text   = DateTime.Today.ToString("yyyy-MM-dd");

                // Hiển thị URL webhook để admin copy vào DeviceManager
                var baseUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}";
                litWebhookUrl.Text = HttpUtility.HtmlEncode(
                    $"{baseUrl}/Admin/Hander/hdAttendanceWebhook.ashx");
            }
        }

        // ─── PULL thủ công ────────────────────────────────────────────────────────

        protected void BtnPull_Click(object sender, EventArgs e)
        {
            Task.Run(async () => await PullAsync()).GetAwaiter().GetResult();
        }

        private async Task PullAsync()
        {
            try
            {
                var svc   = new AttendanceSyncService();
                int saved = await svc.PullAndSaveAsync(txtFrom.Text, txtTo.Text);
                lblPullResult.Text =
                    $"<div class='alert alert-success'>Đã lưu/cập nhật <strong>{saved}</strong> bản ghi chấm công.</div>";
            }
            catch (Exception ex)
            {
                lblPullResult.Text = $"<div class='alert alert-danger'>Lỗi: {ex.Message}</div>";
            }
        }

        // ─── Import vào bảng công ────────────────────────────────────────────────

        protected void BtnImport_Click(object sender, EventArgs e)
        {
            if (!DateTime.TryParse(txtImportFrom.Text, out DateTime tuNgay) ||
                !DateTime.TryParse(txtImportTo.Text,   out DateTime denNgay))
            {
                lblImportResult.Text = "<div class='alert alert-warning'>Ngày không hợp lệ.</div>";
                return;
            }

            try
            {
                var svc    = new DeviceAttendanceImportService();
                var result = svc.ImportRange(tuNgay, denNgay, nguoiTao: "device-import");

                var sb = new StringBuilder();
                sb.AppendFormat(
                    "<div class='alert alert-{0}'>" +
                    "Đã xử lý <strong>{1}</strong> bản ghi — " +
                    "Thành công: <strong>{2}</strong> — " +
                    "Bỏ qua (NV không tồn tại): <strong>{3}</strong>",
                    result.Errors.Count == 0 ? "success" : "warning",
                    result.Processed, result.Succeeded, result.Skipped);

                if (result.Errors.Count > 0)
                {
                    sb.Append("<ul class='mt-2 mb-0'>");
                    foreach (var err in result.Errors)
                        sb.AppendFormat("<li>{0}</li>", HttpUtility.HtmlEncode(err));
                    sb.Append("</ul>");
                }
                sb.Append("</div>");
                lblImportResult.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                lblImportResult.Text = $"<div class='alert alert-danger'>Lỗi: {ex.Message}</div>";
            }
        }

        // ─── Tổng hợp hôm nay ────────────────────────────────────────────────────

        protected void BtnToday_Click(object sender, EventArgs e)
        {
            Task.Run(async () => await LoadTodayAsync()).GetAwaiter().GetResult();
        }

        private async Task LoadTodayAsync()
        {
            try
            {
                var svc     = new AttendanceSyncService();
                var summary = await svc.GetTodaySummaryAsync();
                if (summary == null) { litToday.Text = "<p class='text-muted'>Không có dữ liệu.</p>"; return; }

                var sb = new StringBuilder();
                sb.Append("<table class='table table-sm table-bordered'>");
                sb.Append("<tr><th>Ngày</th><th>Check-in</th><th>Check-out</th><th>NV duy nhất</th></tr>");
                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>",
                    summary.Date, summary.TotalCheckIns, summary.TotalCheckOuts, summary.UniqueEmployees);
                sb.Append("</table>");

                if (summary.RecentLogs?.Count > 0)
                {
                    sb.Append("<h6 class='mt-3'>Gần đây:</h6>");
                    sb.Append("<table class='table table-sm'>");
                    sb.Append("<tr><th>Mã NV</th><th>Họ tên</th><th>Loại</th><th>Thời gian</th><th>Độ tin cậy</th></tr>");
                    foreach (var log in summary.RecentLogs)
                    {
                        sb.AppendFormat(
                            "<tr><td>{0}</td><td>{1}</td><td><span class='badge-{2}'>{3}</span></td><td>{4:HH:mm:ss}</td><td>{5:P0}</td></tr>",
                            HttpUtility.HtmlEncode(log.EmployeeCode),
                            HttpUtility.HtmlEncode(log.EmployeeName),
                            log.Type == "check_in" ? "success" : "warning",
                            log.Type == "check_in" ? "Vào" : "Ra",
                            log.Timestamp,
                            log.ConfidenceScore);
                    }
                    sb.Append("</table>");
                }

                litToday.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                litToday.Text = $"<div class='alert alert-danger'>Lỗi: {ex.Message}</div>";
            }
        }

        // ─── Debug: Kiểm tra mapping mã NV ──────────────────────────────────────

        protected void BtnCheckMapping_Click(object sender, EventArgs e)
        {
            try
            {
                var svc = new DeviceAttendanceImportService();
                var rows = svc.GetMappingDebug();

                var sb = new StringBuilder();
                sb.Append("<table class='table table-sm table-bordered'>");
                sb.Append("<tr><th>Mã trong ChamCong_Device</th><th>Tìm thấy trong DanhSachNhanSu?</th><th>Tên</th><th>Phòng ban</th><th>Trạng thái</th></tr>");
                foreach (System.Data.DataRow row in rows.Rows)
                {
                    bool found = !string.IsNullOrEmpty(row["pk_seq"].ToString());
                    sb.AppendFormat(
                        "<tr><td><strong>{0}</strong></td>" +
                        "<td><span class='badge-{1}'>{2}</span></td>" +
                        "<td>{3}</td><td>{4}</td><td>{5}</td></tr>",
                        HttpUtility.HtmlEncode(row["mapNV"].ToString()),
                        found ? "success" : "danger",
                        found ? "Khớp" : "Không tìm thấy",
                        HttpUtility.HtmlEncode(row["ten"].ToString()),
                        HttpUtility.HtmlEncode(row["phongban"].ToString()),
                        HttpUtility.HtmlEncode(row["trangthai"].ToString()));
                }
                sb.Append("</table>");
                litMapping.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                litMapping.Text = $"<div class='alert alert-danger'>Lỗi: {ex.Message}</div>";
            }
        }

        // ─── Danh sách nhân viên ─────────────────────────────────────────────────

        protected void BtnListEmp_Click(object sender, EventArgs e)
        {
            Task.Run(async () => await LoadEmployeesAsync()).GetAwaiter().GetResult();
        }

        private async Task LoadEmployeesAsync()
        {
            try
            {
                var svc       = new EmployeeSyncService();
                var employees = await svc.GetAllEmployeesFromDeviceAsync();

                var sb = new StringBuilder();
                sb.AppendFormat("<p>Tổng: <strong>{0}</strong> nhân viên đã đăng ký</p>", employees.Count);
                sb.Append("<table class='table table-sm table-bordered'>");
                sb.Append("<tr><th>Mã NV</th><th>Họ tên</th><th>Phòng ban</th><th>Chức vụ</th><th>Ảnh khuôn mặt</th></tr>");
                foreach (var emp in employees)
                {
                    int faceCount = emp.FaceImages?.Count ?? 0;
                    sb.AppendFormat(
                        "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td>" +
                        "<td><span class='badge-{4}'>{5} ảnh</span></td></tr>",
                        HttpUtility.HtmlEncode(emp.EmployeeCode),
                        HttpUtility.HtmlEncode(emp.FullName),
                        HttpUtility.HtmlEncode(emp.Department ?? ""),
                        HttpUtility.HtmlEncode(emp.Position  ?? ""),
                        faceCount > 0 ? "success" : "danger",
                        faceCount);
                }
                sb.Append("</table>");
                litEmployees.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                litEmployees.Text = $"<div class='alert alert-danger'>Lỗi: {ex.Message}</div>";
            }
        }
    }
}
