using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HLVTimeSheet.Model.DeviceManager
{
    /// <summary>
    /// HTTP client giao tiếp với DeviceManager REST API.
    /// Xác thực bằng X-API-Key header (giống source/business-service-nodejs).
    /// </summary>
    public class DeviceManagerApiClient : IDisposable
    {
        private readonly HttpClient _http;

        public DeviceManagerApiClient(DeviceManagerConfig config)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            _http = new HttpClient
            {
                BaseAddress = new Uri(config.BaseUrl.TrimEnd('/') + "/"),
                Timeout     = TimeSpan.FromSeconds(30),
            };
            _http.DefaultRequestHeaders.Add("X-API-Key", config.ApiKey);
            _http.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // ─── GET ──────────────────────────────────────────────────────────────────

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var url = new Uri(_http.BaseAddress, endpoint).ToString();
            DeviceManagerLogger.LogRequest("GET", url);
            var response = await _http.GetAsync(endpoint);
            string body = await response.Content.ReadAsStringAsync();
            DeviceManagerLogger.LogResponse((int)response.StatusCode, url, body);
            await EnsureSuccessAsync(response, body);
            return JsonConvert.DeserializeObject<T>(body);
        }

        // ─── POST JSON ────────────────────────────────────────────────────────────

        public async Task<T> PostAsync<T>(string endpoint, object body)
        {
            var url = new Uri(_http.BaseAddress, endpoint).ToString();
            string reqBody = JsonConvert.SerializeObject(body);
            DeviceManagerLogger.LogRequest("POST", url, reqBody);
            var content  = new StringContent(reqBody, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(endpoint, content);
            string resBody = await response.Content.ReadAsStringAsync();
            DeviceManagerLogger.LogResponse((int)response.StatusCode, url, resBody);
            await EnsureSuccessAsync(response, resBody);
            return JsonConvert.DeserializeObject<T>(resBody);
        }

        // ─── POST multipart/form-data ─────────────────────────────────────────────

        public async Task<T> PostMultipartAsync<T>(string endpoint, MultipartFormDataContent form)
        {
            var url = new Uri(_http.BaseAddress, endpoint).ToString();
            DeviceManagerLogger.LogRequest("POST(multipart)", url);
            var response = await _http.PostAsync(endpoint, form);
            string resBody = await response.Content.ReadAsStringAsync();
            DeviceManagerLogger.LogResponse((int)response.StatusCode, url, resBody);
            await EnsureSuccessAsync(response, resBody);
            return JsonConvert.DeserializeObject<T>(resBody);
        }

        // ─── DELETE ───────────────────────────────────────────────────────────────

        public async Task<T> DeleteAsync<T>(string endpoint)
        {
            var url = new Uri(_http.BaseAddress, endpoint).ToString();
            DeviceManagerLogger.LogRequest("DELETE", url);
            var response = await _http.DeleteAsync(endpoint);
            string resBody = await response.Content.ReadAsStringAsync();
            DeviceManagerLogger.LogResponse((int)response.StatusCode, url, resBody);
            await EnsureSuccessAsync(response, resBody);
            return JsonConvert.DeserializeObject<T>(resBody);
        }

        // ─── Helpers ──────────────────────────────────────────────────────────────

        private static async Task EnsureSuccessAsync(HttpResponseMessage response, string body = null)
        {
            if (!response.IsSuccessStatusCode)
            {
                if (body == null) body = await response.Content.ReadAsStringAsync();
                DeviceManagerLogger.LogError("API", $"HTTP {(int)response.StatusCode} error: {body}");
                throw new HttpRequestException(
                    $"DeviceManager API lỗi {(int)response.StatusCode}: {body}");
            }
        }

        public void Dispose() => _http?.Dispose();
    }
}
