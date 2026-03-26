using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HLVTimeSheet.Model.DeviceManager
{
    // ─── Response wrapper ────────────────────────────────────────────────────────

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
}
