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
            var response = await _http.GetAsync(endpoint);
            await EnsureSuccessAsync(response);
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        // ─── POST JSON ────────────────────────────────────────────────────────────

        public async Task<T> PostAsync<T>(string endpoint, object body)
        {
            var content  = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(endpoint, content);
            await EnsureSuccessAsync(response);
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        // ─── POST multipart/form-data ─────────────────────────────────────────────

        public async Task<T> PostMultipartAsync<T>(string endpoint, MultipartFormDataContent form)
        {
            var response = await _http.PostAsync(endpoint, form);
            await EnsureSuccessAsync(response);
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        // ─── DELETE ───────────────────────────────────────────────────────────────

        public async Task<T> DeleteAsync<T>(string endpoint)
        {
            var response = await _http.DeleteAsync(endpoint);
            await EnsureSuccessAsync(response);
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        // ─── Helpers ──────────────────────────────────────────────────────────────

        private static async Task EnsureSuccessAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"DeviceManager API lỗi {(int)response.StatusCode}: {body}");
            }
        }

        public void Dispose() => _http?.Dispose();
    }
}
