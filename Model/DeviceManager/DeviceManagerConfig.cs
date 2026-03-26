using System.Configuration;

namespace HLVTimeSheet.Model.DeviceManager
{
    /// <summary>
    /// Cấu hình kết nối tới hệ thống DeviceManager (máy chấm công nhận diện khuôn mặt)
    /// BaseUrl, CustomerId, ApiKey được đọc từ Web.config (appSettings)
    /// </summary>
    public static class DeviceManagerConfig
    {
        // Đọc từ Web.config appSettings; fallback về giá trị mặc định nếu chưa cấu hình
        public static string BaseUrl =>
            ConfigurationManager.AppSettings["DeviceManager_BaseUrl"] ?? "https://device.erp-x.com/api";

        public static string CustomerId =>
            ConfigurationManager.AppSettings["DeviceManager_CustomerId"] ?? "578f3f6f-14db-4adf-9c02-fdad454273ea";

        public static string ApiKey =>
            ConfigurationManager.AppSettings["DeviceManager_ApiKey"] ?? "ck_a49fbf00754cc51f3b20d3eb719fffe6";
    }
}
