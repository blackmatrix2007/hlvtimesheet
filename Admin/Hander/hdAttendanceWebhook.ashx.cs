using System;
using System.Diagnostics;
using System.IO;
using System.Web;
using HLVTimeSheet.Model.DeviceManager;
using Newtonsoft.Json;
using static HLVTimeSheet.Model.DeviceManager.DeviceManagerLogger;

namespace HLVTimeSheet.Admin.Hander
{
    /// <summary>
    /// Webhook endpoint nhận sự kiện chấm công PUSH từ DeviceManager.
    ///
    /// DeviceManager cấu hình:
    ///   Webhook URL: https://{domain}/Admin/Hander/hdAttendanceWebhook.ashx
    ///
    /// DeviceManager POST JSON payload (WebhookAttendanceDto):
    ///   {
    ///     "event": "attendance.checkin",          // hoặc "attendance.checkout"
    ///     "employeeCode": "NV001",
    ///     "checkingTime": "2026-03-26T08:30:00+07:00",
    ///     "faceConfidence": 0.92,
    ///     "latitude": 10.7769,
    ///     "longitude": 106.7009,
    ///     "gpsAccuracy": 5.0,
    ///     "deviceName": "Camera-Lobby",
    ///     "source": "arcface-device",
    ///     "signature": "hmac-sha256-hex"          // tuỳ chọn
    ///   }
    ///
    /// Tham khảo: source/business-service-nodejs/src/modules/attendance/attendance.controller.ts
    ///            phương thức processWebhook()
    /// </summary>
    public class hdAttendanceWebhook : IHttpHandler
    {
        public bool IsReusable => false;

        public void ProcessRequest(HttpContext context)
        {
            Debug.WriteLine($"[Webhook] {DateTime.Now:HH:mm:ss} {context.Request.HttpMethod} {context.Request.Url}");
            DeviceManagerLogger.Log("WEBHOOK", $"{context.Request.HttpMethod} from {context.Request.UserHostAddress}");
            context.Response.ContentType = "application/json";
            context.Response.Expires     = -1;

            // Chỉ cho phép POST
            if (!context.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                Debug.WriteLine("[Webhook] 405 Method Not Allowed");
                context.Response.StatusCode = 405;
                context.Response.Write(Json(new { success = false, message = "Method Not Allowed" }));
                return;
            }

            // Đọc body JSON
            string body;
            try
            {
                using (var reader = new StreamReader(context.Request.InputStream))
                    body = reader.ReadToEnd();
                Debug.WriteLine($"[Webhook] Body: {body}");
                DeviceManagerLogger.Log("WEBHOOK", $"Body: {body}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Webhook] Đọc body lỗi: {ex.Message}");
                context.Response.StatusCode = 400;
                context.Response.Write(Json(new { success = false, message = "Không đọc được body: " + ex.Message }));
                return;
            }

            // Deserialize
            WebhookAttendanceDto dto;
            try
            {
                dto = JsonConvert.DeserializeObject<WebhookAttendanceDto>(body);
                Debug.WriteLine($"[Webhook] Parsed: event={dto?.Event}, employeeCode={dto?.EmployeeCode}, checkingTime={dto?.CheckingTime}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Webhook] JSON parse lỗi: {ex.Message}");
                DeviceManagerLogger.LogError("WEBHOOK", "JSON parse failed", ex);
                context.Response.StatusCode = 400;
                context.Response.Write(Json(new { success = false, message = "JSON không hợp lệ: " + ex.Message }));
                return;
            }

            // Validate bắt buộc
            if (dto == null || string.IsNullOrEmpty(dto.EmployeeCode) || string.IsNullOrEmpty(dto.CheckingTime))
            {
                Debug.WriteLine("[Webhook] 400 Thiếu employeeCode hoặc checkingTime");
                context.Response.StatusCode = 400;
                context.Response.Write(Json(new { success = false, message = "employeeCode và checkingTime là bắt buộc." }));
                return;
            }

            if (dto.Event != "attendance.checkin" && dto.Event != "attendance.checkout")
            {
                Debug.WriteLine($"[Webhook] 400 Event không hỗ trợ: {dto.Event}");
                context.Response.StatusCode = 400;
                context.Response.Write(Json(new { success = false, message = $"event '{dto.Event}' không được hỗ trợ." }));
                return;
            }

            // Xử lý
            try
            {
                var svc    = new AttendanceSyncService();
                var result = svc.ProcessWebhook(dto);

                int statusCode = result.Success ? 200 : 422;
                Debug.WriteLine($"[Webhook] Result: success={result.Success}, recordId={result.RecordId}, isValid={result.IsValid}, msg={result.Message}");
                DeviceManagerLogger.Log("WEBHOOK", $"Result: success={result.Success}, recordId={result.RecordId}, isValid={result.IsValid}, msg={result.Message}");
                context.Response.StatusCode = statusCode;
                context.Response.Write(Json(new
                {
                    success         = result.Success,
                    message         = result.Message,
                    recordId        = result.RecordId,
                    isValid         = result.IsValid,
                    rejectionReason = result.RejectionReason,
                }));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Webhook] EXCEPTION: {ex.Message}\n{ex.StackTrace}");
                DeviceManagerLogger.LogError("WEBHOOK", ex.Message, ex);
                // Trả 500 để DeviceManager tự retry
                context.Response.StatusCode = 500;
                context.Response.Write(Json(new
                {
                    success = false,
                    message = "Lỗi server: " + ex.Message,
                }));
            }
        }

        private static string Json(object obj) => JsonConvert.SerializeObject(obj);
    }
}
