using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HLVTimeSheet.Model.DeviceManager
{
    // ─── API Response wrapper ─────────────────────────────────────────────────────

    public class ApiResponse<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    // ─── Employee ─────────────────────────────────────────────────────────────────

    public class DmEmployee
    {
        [JsonProperty("employee_code")]
        public string EmployeeCode { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("face_images")]
        public List<DmFaceImage> FaceImages { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
    }

    public class DmFaceImage
    {
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("is_primary")]
        public bool IsPrimary { get; set; }
    }

    public class DmEmployeeListResponse
    {
        [JsonProperty("employees")]
        public List<DmEmployee> Employees { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }
    }

    // ─── Attendance ───────────────────────────────────────────────────────────────

    public class DmAttendanceLog
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("employee_code")]
        public string EmployeeCode { get; set; }

        [JsonProperty("employee_name")]
        public string EmployeeName { get; set; }

        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        /// <summary>"check_in" hoặc "check_out"</summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("confidence_score")]
        public double ConfidenceScore { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("synced")]
        public bool Synced { get; set; }
    }

    public class DmCheckInListResponse
    {
        [JsonProperty("checkIns")]
        public List<DmAttendanceLog> CheckIns { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }

    public class DmTodaySummary
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("total_check_ins")]
        public int TotalCheckIns { get; set; }

        [JsonProperty("total_check_outs")]
        public int TotalCheckOuts { get; set; }

        [JsonProperty("unique_employees")]
        public int UniqueEmployees { get; set; }

        [JsonProperty("recent_logs")]
        public List<DmAttendanceLog> RecentLogs { get; set; }
    }

    // ─── Webhook payload (DeviceManager → HLVTimeSheet) ──────────────────────────
    // Tương ứng với WebhookAttendanceDto trong source/business-service-nodejs

    public class WebhookAttendanceDto
    {
        /// <summary>"attendance.checkin" hoặc "attendance.checkout"</summary>
        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("employeeCode")]
        public string EmployeeCode { get; set; }

        /// <summary>Thời gian chấm công ISO 8601</summary>
        [JsonProperty("checkingTime")]
        public string CheckingTime { get; set; }

        /// <summary>Ảnh khuôn mặt base64 (tuỳ chọn)</summary>
        [JsonProperty("imageBase64")]
        public string ImageBase64 { get; set; }

        [JsonProperty("deviceName")]
        public string DeviceName { get; set; }

        [JsonProperty("deviceId")]
        public int? DeviceId { get; set; }

        [JsonProperty("latitude")]
        public double? Latitude { get; set; }

        [JsonProperty("longitude")]
        public double? Longitude { get; set; }

        [JsonProperty("gpsAccuracy")]
        public double? GpsAccuracy { get; set; }

        [JsonProperty("locationId")]
        public int? LocationId { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        /// <summary>Điểm tin cậy nhận diện khuôn mặt (0.0 – 1.0)</summary>
        [JsonProperty("faceConfidence")]
        public double? FaceConfidence { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        /// <summary>Chữ ký HMAC-SHA256 để xác thực webhook</summary>
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }

    // ─── Kết quả xử lý webhook ───────────────────────────────────────────────────

    public class WebhookResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int? RecordId { get; set; }
        public bool IsValid { get; set; }
        public string RejectionReason { get; set; }
    }
}
