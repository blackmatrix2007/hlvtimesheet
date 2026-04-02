using System;
using System.IO;
using System.Web;

namespace HLVTimeSheet.Model.DeviceManager
{
    /// <summary>
    /// Ghi log ra file App_Data/logs/devicemanager_YYYY-MM-DD.log
    /// Log luôn bật.
    /// </summary>
    public static class DeviceManagerLogger
    {
        private static readonly object _lock = new object();

        public static bool IsEnabled => true;

        public static void Log(string category, string message)
        {
            if (!IsEnabled) return;
            try
            {
                string logDir = GetLogDir();
                string logFile = Path.Combine(logDir,
                    $"devicemanager_{DateTime.Now:yyyy-MM-dd}.log");

                string line = $"[{DateTime.Now:HH:mm:ss.fff}] [{category}] {message}";
                lock (_lock)
                {
                    File.AppendAllText(logFile, line + Environment.NewLine,
                        System.Text.Encoding.UTF8);
                }
            }
            catch { /* không để lỗi log phá app */ }
        }

        public static void LogRequest(string method, string url, string body = null)
        {
            if (!IsEnabled) return;
            string msg = $"{method} {url}";
            if (!string.IsNullOrEmpty(body))
                msg += $"\n  Body: {body}";
            Log("HTTP-REQ", msg);
        }

        public static void LogResponse(int statusCode, string url, string body = null)
        {
            if (!IsEnabled) return;
            string msg = $"{statusCode} ← {url}";
            if (!string.IsNullOrEmpty(body))
                msg += $"\n  Body: {Truncate(body, 500)}";
            Log("HTTP-RES", msg);
        }

        public static void LogError(string category, string message, Exception ex = null)
        {
            if (!IsEnabled) return;
            string msg = message;
            if (ex != null)
                msg += $"\n  Exception: {ex.GetType().Name}: {ex.Message}";
            Log("ERROR-" + category, msg);
        }

        private static string GetLogDir()
        {
            string baseDir;
            try
            {
                baseDir = HttpContext.Current?.Server.MapPath("~/App_Data/logs")
                          ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "logs");
            }
            catch
            {
                baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "logs");
            }
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);
            return baseDir;
        }

        private static string Truncate(string s, int max) =>
            s == null ? "" : (s.Length <= max ? s : s.Substring(0, max) + "...[truncated]");
    }
}
