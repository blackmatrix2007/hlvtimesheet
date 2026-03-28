using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using HLVTimeSheet.AcsessData;

namespace HLVTimeSheet.Model.DeviceManager
{
    /// <summary>
    /// Gửi email cảnh báo khi phát hiện chấm công bất thường:
    ///   - Face confidence thấp hơn ngưỡng cấu hình
    ///   - Nhân viên không tồn tại trong DanhSachNhanSu
    ///
    /// SMTP config đọc từ bảng DeviceManagerSettings (cột smtpHost, smtpPort, ...).
    /// </summary>
    public class AlertService
    {
        private readonly string _smtpHost;
        private readonly int    _smtpPort;
        private readonly bool   _useSsl;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly string _alertEmails;   // CSV

        // ─── Khởi tạo từ DB settings ──────────────────────────────────────────────

        public AlertService()
        {
            LoadFromDb(out _smtpHost, out _smtpPort, out _useSsl,
                       out _smtpUser, out _smtpPass, out _alertEmails);
        }

        // Khởi tạo trực tiếp (dùng cho test từ UI)
        public AlertService(string host, int port, bool ssl,
                            string user, string pass, string emails)
        {
            _smtpHost    = host;
            _smtpPort    = port;
            _useSsl      = ssl;
            _smtpUser    = user;
            _smtpPass    = pass;
            _alertEmails = emails;
        }

        // ─── Gửi email ────────────────────────────────────────────────────────────

        public async Task SendAsync(string subject, string htmlBody)
        {
            if (string.IsNullOrWhiteSpace(_smtpHost) || string.IsNullOrWhiteSpace(_alertEmails))
                return; // Chưa cấu hình → bỏ qua

            using (var client = new SmtpClient(_smtpHost, _smtpPort))
            {
                client.EnableSsl             = _useSsl;
                client.DeliveryMethod        = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;

                if (!string.IsNullOrEmpty(_smtpUser))
                    client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);

                var from = string.IsNullOrEmpty(_smtpUser)
                    ? "noreply@hlvtimesheet.local"
                    : _smtpUser;

                using (var msg = new MailMessage())
                {
                    msg.From       = new MailAddress(from, "HLVTimeSheet");
                    msg.Subject    = subject;
                    msg.Body       = htmlBody;
                    msg.IsBodyHtml = true;

                    foreach (var email in _alertEmails.Split(','))
                    {
                        string e = email.Trim();
                        if (!string.IsNullOrEmpty(e))
                            msg.To.Add(new MailAddress(e));
                    }

                    if (msg.To.Count == 0) return;

                    await client.SendMailAsync(msg);
                }
            }
        }

        // ─── Cảnh báo chấm công bất thường ──────────────────────────────────────

        /// <summary>
        /// Gửi cảnh báo khi face confidence thấp hơn ngưỡng cho phép.
        /// </summary>
        public async Task AlertLowFaceConfidenceAsync(
            string employeeCode, DateTime thoiGian, double confidence, double threshold, string deviceName)
        {
            string subject = $"[Cảnh báo] Chấm công không đủ tin cậy — {employeeCode}";
            string body    = $@"
                <p>Hệ thống phát hiện chấm công với độ tin cậy khuôn mặt thấp:</p>
                <table border='1' cellpadding='6' cellspacing='0' style='border-collapse:collapse'>
                    <tr><td><b>Nhân viên</b></td><td>{HtmlEncode(employeeCode)}</td></tr>
                    <tr><td><b>Thời gian</b></td><td>{thoiGian:dd/MM/yyyy HH:mm:ss}</td></tr>
                    <tr><td><b>Thiết bị</b></td><td>{HtmlEncode(deviceName)}</td></tr>
                    <tr><td><b>Độ tin cậy</b></td><td style='color:red'><b>{confidence:P1}</b> (ngưỡng: {threshold:P1})</td></tr>
                </table>
                <p>Vui lòng kiểm tra lại bản ghi chấm công này.</p>";

            await SendAsync(subject, body);
        }

        /// <summary>
        /// Gửi cảnh báo khi nhân viên từ DeviceManager không có trong DanhSachNhanSu.
        /// </summary>
        public async Task AlertUnknownEmployeeAsync(string employeeCode, string deviceName, DateTime thoiGian)
        {
            string subject = $"[Cảnh báo] Mã nhân viên không tồn tại — {employeeCode}";
            string body    = $@"
                <p>DeviceManager gửi dữ liệu chấm công cho nhân viên không có trong HLVTimeSheet:</p>
                <table border='1' cellpadding='6' cellspacing='0' style='border-collapse:collapse'>
                    <tr><td><b>Mã nhân viên</b></td><td style='color:red'><b>{HtmlEncode(employeeCode)}</b></td></tr>
                    <tr><td><b>Thời gian</b></td><td>{thoiGian:dd/MM/yyyy HH:mm:ss}</td></tr>
                    <tr><td><b>Thiết bị</b></td><td>{HtmlEncode(deviceName)}</td></tr>
                </table>
                <p>Kiểm tra lại mã nhân viên trên DeviceManager và DanhSachNhanSu.</p>";

            await SendAsync(subject, body);
        }

        // ─── Load cấu hình từ DB ──────────────────────────────────────────────────

        private static void LoadFromDb(
            out string host, out int port, out bool ssl,
            out string user, out string pass, out string emails)
        {
            host = ""; port = 587; ssl = true; user = ""; pass = ""; emails = "";
            try
            {
                var conn = new ConnectionDatabase();
                using (var sqlConn = new SqlConnection(conn.ReturnConnectionDatabaseWS()))
                {
                    sqlConn.Open();
                    const string sql = @"
                        SELECT TOP 1 smtpHost, smtpPort, smtpSsl, smtpUser, smtpPass, alertEmails
                        FROM DeviceManagerSettings
                        WHERE laDinhChinh = 1 AND dangHoatDong = 1
                          AND smtpHost IS NOT NULL AND smtpHost <> ''";

                    using (var cmd = new SqlCommand(sql, sqlConn))
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            host   = r["smtpHost"]?.ToString() ?? "";
                            port   = r["smtpPort"] != DBNull.Value ? Convert.ToInt32(r["smtpPort"]) : 587;
                            ssl    = r["smtpSsl"] != DBNull.Value && (bool)r["smtpSsl"];
                            user   = r["smtpUser"]?.ToString() ?? "";
                            pass   = r["smtpPass"]?.ToString() ?? "";
                            emails = r["alertEmails"]?.ToString() ?? "";
                        }
                    }
                }
            }
            catch { /* DB chưa có cấu hình SMTP → gửi email bị bỏ qua */ }
        }

        private static string HtmlEncode(string s)
            => System.Web.HttpUtility.HtmlEncode(s ?? "");
    }
}
