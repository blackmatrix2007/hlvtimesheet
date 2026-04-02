using HLVTimeSheet.AcsessData;
using HLVTimeSheet.Model.DeviceManager;
using static HLVTimeSheet.Model.DeviceManager.DeviceManagerLogger;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HLVTimeSheet.Admin
{
    public partial class uc_ConnectToDevice_deviceSettings : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadCurrentSettings();
        }

        // ─── Nạp cấu hình hiện tại ───────────────────────────────────────────────

        private void LoadCurrentSettings()
        {
            var conn = new ConnectionDatabase();
            using (var sqlConn = new SqlConnection(conn.ReturnConnectionDatabase()))
            {
                sqlConn.Open();
                DeviceManagerConfig.EnsureSettingsTableExists(sqlConn);

                const string sql = @"
                    SELECT TOP 1 baseUrl, customerId, webhookSecret, dangHoatDong, smtpHost, smtpPort, smtpSsl, smtpUser, alertEmails, faceConfidenceThreshold
                    FROM DeviceManagerSettings
                    WHERE laDinhChinh = 1
                    ORDER BY pk_seq DESC";

                using (var cmd = new SqlCommand(sql, sqlConn))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtBaseUrl.Text = reader["baseUrl"].ToString();
                        txtCustomerId.Text = reader["customerId"].ToString();
                        // Không load API key / password — user phải nhập lại để đổi
                        chkEnabled.Checked = reader["dangHoatDong"] != DBNull.Value && (bool)reader["dangHoatDong"];
                        txtSmtpHost.Text = reader["smtpHost"]?.ToString() ?? "";
                        txtSmtpPort.Text = reader["smtpPort"]?.ToString() ?? "587";
                        chkSmtpSsl.Checked = reader["smtpSsl"] != DBNull.Value && (bool)reader["smtpSsl"];
                        txtSmtpUser.Text = reader["smtpUser"]?.ToString() ?? "";
                        txtAlertEmails.Text = reader["alertEmails"]?.ToString() ?? "";
                        txtFaceThreshold.Text = reader["faceConfidenceThreshold"]?.ToString() ?? "0.6";
                    }
                }
            }
        }

        // ─── Lưu ─────────────────────────────────────────────────────────────────

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBaseUrl.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerId.Text))
            {
                lblMsg.Text = Alert("warning", "Base URL và Customer ID là bắt buộc.");
                return;
            }

            if (!double.TryParse(txtFaceThreshold.Text, out double threshold) ||
                threshold < 0 || threshold > 1)
            {
                lblMsg.Text = Alert("warning", "Ngưỡng độ tin cậy khuôn mặt phải là số từ 0 đến 1.");
                return;
            }

            DeviceManagerLogger.Log("SETTINGS", $"BtnSave: baseUrl={txtBaseUrl.Text.Trim()}");
            try
            {
                var conn = new ConnectionDatabase();
                using (var sqlConn = new SqlConnection(conn.ReturnConnectionDatabaseWS()))
                {
                    sqlConn.Open();
                    DeviceManagerConfig.EnsureSettingsTableExists(sqlConn);
                    EnsureSmtpColumnsExist(sqlConn);

                    // Kiểm tra đã có bản ghi chính chưa
                    const string checkSql = "SELECT COUNT(*) FROM DeviceManagerSettings WHERE laDinhChinh = 1";
                    int count;
                    using (var cmd = new SqlCommand(checkSql, sqlConn))
                        count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // UPDATE — chỉ cập nhật apiKey / webhookSecret nếu user nhập
                        string updateSql = @"
                            UPDATE DeviceManagerSettings SET
                                baseUrl                  = @baseUrl,
                                customerId               = @customerId,
                                dangHoatDong             = @enabled,
                                smtpHost                 = @smtpHost,
                                smtpPort                 = @smtpPort,
                                smtpSsl                  = @smtpSsl,
                                smtpUser                 = @smtpUser,
                                alertEmails              = @alertEmails,
                                faceConfidenceThreshold  = @threshold"
                                + (string.IsNullOrEmpty(txtApiKey.Text) ? "" : ", apiKey = @apiKey")
                                + (string.IsNullOrEmpty(txtWebhookSecret.Text) ? "" : ", webhookSecret = @webhookSecret")
                                + (string.IsNullOrEmpty(txtSmtpPass.Text) ? "" : ", smtpPass = @smtpPass")
                                + " WHERE laDinhChinh = 1";

                        using (var cmd = new SqlCommand(updateSql, sqlConn))
                        {
                            AddCommonParams(cmd, threshold);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // INSERT lần đầu
                        const string insertSql = @"
                            INSERT INTO DeviceManagerSettings
                                (baseUrl, customerId, apiKey, webhookSecret, dangHoatDong, laDinhChinh,
                                 smtpHost, smtpPort, smtpSsl, smtpUser, smtpPass, alertEmails, faceConfidenceThreshold)
                            VALUES
                                (@baseUrl, @customerId, @apiKey, @webhookSecret, @enabled, 1,
                                 @smtpHost, @smtpPort, @smtpSsl, @smtpUser, @smtpPass, @alertEmails, @threshold)";

                        using (var cmd = new SqlCommand(insertSql, sqlConn))
                        {
                            AddCommonParams(cmd, threshold);
                            cmd.Parameters.AddWithValue("@apiKey", txtApiKey.Text);
                            cmd.Parameters.AddWithValue("@webhookSecret", txtWebhookSecret.Text);
                            cmd.Parameters.AddWithValue("@smtpPass", txtSmtpPass.Text);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                DeviceManagerLogger.Log("SETTINGS", "Save OK");
                lblMsg.Text = Alert("success", "Đã lưu cấu hình thành công.");
            }
            catch (Exception ex)
            {
                DeviceManagerLogger.LogError("SETTINGS", "Save failed", ex);
                lblMsg.Text = Alert("danger", $"Lỗi lưu cấu hình: {ex.Message}");
            }
        }

        // ─── Gửi email test ───────────────────────────────────────────────────────

        protected void BtnTestEmail_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSmtpHost.Text) || string.IsNullOrWhiteSpace(txtAlertEmails.Text))
            {
                lblMsg.Text = Alert("warning", "Nhập SMTP Host và Email nhận trước khi test.");
                return;
            }

            try
            {
                if (!int.TryParse(txtSmtpPort.Text, out int port)) port = 587;

                var alert = new AlertService(
                    txtSmtpHost.Text, port, chkSmtpSsl.Checked,
                    txtSmtpUser.Text, txtSmtpPass.Text,
                    txtAlertEmails.Text);

                Task.Run(async () =>
                    await alert.SendAsync(
                        "Test cấu hình email HLVTimeSheet",
                        "<p>Email test từ hệ thống HLVTimeSheet — cấu hình SMTP đang hoạt động.</p>")
                ).GetAwaiter().GetResult();

                lblMsg.Text = Alert("success", "Đã gửi email test thành công.");
            }
            catch (Exception ex)
            {
                DeviceManagerLogger.LogError("SETTINGS", "TestEmail failed", ex);
                lblMsg.Text = Alert("danger", $"Gửi email thất bại: {ex.Message}");
            }
        }

        // ─── Helpers ─────────────────────────────────────────────────────────────

        private void AddCommonParams(SqlCommand cmd, double threshold)
        {
            if (!int.TryParse(txtSmtpPort.Text, out int port)) port = 587;
            cmd.Parameters.AddWithValue("@baseUrl", txtBaseUrl.Text.Trim());
            cmd.Parameters.AddWithValue("@customerId", txtCustomerId.Text.Trim());
            cmd.Parameters.AddWithValue("@enabled", chkEnabled.Checked);
            cmd.Parameters.AddWithValue("@smtpHost", txtSmtpHost.Text.Trim());
            cmd.Parameters.AddWithValue("@smtpPort", port);
            cmd.Parameters.AddWithValue("@smtpSsl", chkSmtpSsl.Checked);
            cmd.Parameters.AddWithValue("@smtpUser", txtSmtpUser.Text.Trim());
            cmd.Parameters.AddWithValue("@alertEmails", txtAlertEmails.Text.Trim());
            cmd.Parameters.AddWithValue("@threshold", threshold);

            if (!string.IsNullOrEmpty(txtApiKey.Text))
                cmd.Parameters.AddWithValue("@apiKey", txtApiKey.Text);
            if (!string.IsNullOrEmpty(txtWebhookSecret.Text))
                cmd.Parameters.AddWithValue("@webhookSecret", txtWebhookSecret.Text);
            if (!string.IsNullOrEmpty(txtSmtpPass.Text))
                cmd.Parameters.AddWithValue("@smtpPass", txtSmtpPass.Text);
        }

        private static void EnsureSmtpColumnsExist(SqlConnection conn)
        {
            const string ddl = @"
                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                               WHERE TABLE_NAME='DeviceManagerSettings' AND COLUMN_NAME='smtpHost')
                BEGIN
                    ALTER TABLE DeviceManagerSettings ADD
                        smtpHost                 NVARCHAR(200),
                        smtpPort                 INT           DEFAULT 587,
                        smtpSsl                  BIT           DEFAULT 1,
                        smtpUser                 NVARCHAR(200),
                        smtpPass                 NVARCHAR(500),
                        alertEmails              NVARCHAR(500),
                        faceConfidenceThreshold  FLOAT         DEFAULT 0.6;
                END";
            using (var cmd = new SqlCommand(ddl, conn))
                cmd.ExecuteNonQuery();
        }

        private static string Alert(string type, string msg)
            => $"<div class='alert alert-{type} mt-2'>{msg}</div>";
    }
}