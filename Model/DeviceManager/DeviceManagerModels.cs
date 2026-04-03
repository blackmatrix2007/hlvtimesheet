using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HLVTimeSheet.Model.DeviceManager
{
    // ─── API Response wrapper (chấm công + today/summary) ────────────────────────
    // Dạng: { "success": true, "data": { ... } }

    public class ApiResponse<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    // ─── Response đăng ký khuôn mặt ──────────────────────────────────────────────
    // Dạng: { "employee": {...}, "faceVerification": {...}, "message": "..." }
    // KHÔNG có field "success"

    public class RegisterFaceResponse
    {
        [JsonProperty("employee")]
        public DmEmployee Employee { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("duplicates")]
        public List<DuplicateFace> Duplicates { get; set; }

        public bool IsSuccess => Employee != null;
    }

    public class DuplicateFace
    {
        [JsonProperty("employeeCode")]
        public string EmployeeCode { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("similarity")]
        public string Similarity { get; set; }
    }

    // ─── API Response wrapper (danh sách nhân viên) ───────────────────────────────
    // Dạng: { "code": 200, "message": "Success", "body": [...], "page": { ... } }

    public class ApiListResponse<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("body")]
        public List<T> Body { get; set; }

        [JsonProperty("page")]
        public ApiPageInfo Page { get; set; }
    }

    public class ApiPageInfo
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }
    }

    // ─── Employee ─────────────────────────────────────────────────────────────────
    // Actual API fields: employeeCode, fullName, faces[].faceImageUrl

    public class DmEmployee
    {
        [JsonProperty("employeeCode")]
        public string EmployeeCode { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("faces")]
        public List<DmFaceImage> FaceImages { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }
    }

    public class DmFaceImage
    {
        [JsonProperty("faceImageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("isPrimary")]
        public bool IsPrimary { get; set; }

        [JsonProperty("faceQuality")]
        public string FaceQuality { get; set; }
    }

    // ─── Attendance ───────────────────────────────────────────────────────────────
    // Actual API fields: id (UUID), employeeCode, checkInTime, localConfidence

    public class DmAttendanceLog
    {
        // id là UUID string từ DeviceManager
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("employeeCode")]
        public string EmployeeCode { get; set; }

        [JsonProperty("employeeName")]
        public string EmployeeName { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        /// <summary>"check_in" hoặc "check_out"</summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>Độ tin cậy nhận diện — field thực tế là "localConfidence" (string)</summary>
        [JsonProperty("localConfidence")]
        public string LocalConfidence { get; set; }

        public double ConfidenceScore
        {
            get
            {
                double.TryParse(LocalConfidence, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out double v);
                return v;
            }
        }

        /// <summary>Thời gian chấm công — PULL API dùng "checkInTime", Today summary dùng "timestamp"</summary>
        [JsonProperty("checkInTime")]
        public DateTime CheckInTime { get; set; }

        [JsonProperty("timestamp")]
        public DateTime TimestampUtc { get; set; }

        /// <summary>Lấy thời gian từ field nào có giá trị</summary>
        public DateTime Timestamp => CheckInTime != default ? CheckInTime : TimestampUtc;
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

        // Actual fields: checkIns, checkOuts, totalLogs, logs
        [JsonProperty("checkIns")]
        public int TotalCheckIns { get; set; }

        [JsonProperty("checkOuts")]
        public int TotalCheckOuts { get; set; }

        [JsonProperty("totalLogs")]
        public int TotalLogs { get; set; }

        [JsonProperty("logs")]
        public List<DmAttendanceLog> RecentLogs { get; set; }

        // Computed — không có trực tiếp trong API
        public int UniqueEmployees => RecentLogs?.Count ?? 0;
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
        public string DeviceId { get; set; }

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
