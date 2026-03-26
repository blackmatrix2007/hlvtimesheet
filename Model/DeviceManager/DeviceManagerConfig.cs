using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using HLVTimeSheet.AcsessData;

namespace HLVTimeSheet.Model.DeviceManager
{
    /// <summary>
    /// Cấu hình kết nối tới hệ thống DeviceManager.
    ///
    /// Thứ tự ưu tiên (giống source/business-service-nodejs):
    ///   1. Bảng DeviceManagerSettings trong SQL Server (per-company)
    ///   2. Web.config appSettings (fallback toàn cục)
    /// </summary>
    public class DeviceManagerConfig
    {
        public string BaseUrl { get; set; }
        public string CustomerId { get; set; }
        public string ApiKey { get; set; }
        public bool Enabled { get; set; }
        public string WebhookSecret { get; set; }

        // ─── Load từ DB ───────────────────────────────────────────────────────────

        /// <summary>
        /// Nạp cấu hình từ bảng DeviceManagerSettings.
        /// Nếu bảng chưa tồn tại hoặc chưa có dữ liệu, trả về cấu hình từ Web.config.
        /// </summary>
        public static DeviceManagerConfig Load(string chiNhanhId = null)
        {
            try
            {
                var conn = new ConnectionDatabase();
                using (var sqlConn = new SqlConnection(conn.ReturnConnectionDatabaseWS()))
                {
                    sqlConn.Open();
                    EnsureSettingsTableExists(sqlConn);

                    string sql = string.IsNullOrEmpty(chiNhanhId)
                        ? "SELECT TOP 1 * FROM DeviceManagerSettings WHERE laDinhChinh = 1 AND dangHoatDong = 1"
                        : "SELECT TOP 1 * FROM DeviceManagerSettings WHERE chiNhanhId = @chiNhanhId AND dangHoatDong = 1";

                    using (var cmd = new SqlCommand(sql, sqlConn))
                    {
                        if (!string.IsNullOrEmpty(chiNhanhId))
                            cmd.Parameters.AddWithValue("@chiNhanhId", chiNhanhId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new DeviceManagerConfig
                                {
                                    BaseUrl       = reader["baseUrl"].ToString(),
                                    CustomerId    = reader["customerId"].ToString(),
                                    ApiKey        = reader["apiKey"].ToString(),
                                    WebhookSecret = reader["webhookSecret"].ToString(),
                                    Enabled       = (bool)reader["dangHoatDong"],
                                };
                            }
                        }
                    }
                }
            }
            catch { /* Bảng chưa tồn tại hoặc lỗi kết nối → dùng fallback */ }

            // Fallback: Web.config
            return FromWebConfig();
        }

        private static DeviceManagerConfig FromWebConfig() => new DeviceManagerConfig
        {
            BaseUrl       = ConfigurationManager.AppSettings["DeviceManager_BaseUrl"]     ?? "https://device.erp-x.com/api",
            CustomerId    = ConfigurationManager.AppSettings["DeviceManager_CustomerId"]  ?? "578f3f6f-14db-4adf-9c02-fdad454273ea",
            ApiKey        = ConfigurationManager.AppSettings["DeviceManager_ApiKey"]      ?? "ck_a49fbf00754cc51f3b20d3eb719fffe6",
            WebhookSecret = ConfigurationManager.AppSettings["DeviceManager_WebhookSecret"] ?? "",
            Enabled       = true,
        };

        // ─── DDL bảng cấu hình ───────────────────────────────────────────────────

        internal static void EnsureSettingsTableExists(SqlConnection conn)
        {
            const string ddl = @"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'DeviceManagerSettings'
                )
                BEGIN
                    CREATE TABLE DeviceManagerSettings (
                        pk_seq          INT IDENTITY(1,1) PRIMARY KEY,
                        chiNhanhId      NVARCHAR(50),               -- NULL = áp dụng toàn công ty
                        baseUrl         NVARCHAR(500) NOT NULL,     -- https://device.erp-x.com/api
                        customerId      NVARCHAR(100) NOT NULL,     -- UUID từ DeviceManager
                        apiKey          NVARCHAR(255) NOT NULL,     -- X-API-Key
                        apiSecret       NVARCHAR(255),
                        webhookSecret   NVARCHAR(255),              -- HMAC-SHA256 secret
                        laDinhChinh     BIT DEFAULT 1,              -- bản ghi mặc định
                        dangHoatDong    BIT DEFAULT 1,
                        ngayTao         DATETIME DEFAULT GETDATE(),
                        nguoiTao        NVARCHAR(100)
                    );
                END";

            using (var cmd = new SqlCommand(ddl, conn))
                cmd.ExecuteNonQuery();
        }
    }
}
