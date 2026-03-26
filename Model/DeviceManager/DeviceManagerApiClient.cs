using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HLVTimeSheet.Model.DeviceManager
{
    /// <summary>
    /// HTTP client giao tiếp với DeviceManager REST API.
    /// Sử dụng X-API-Key header để xác thực.
    /// </summary>
    public class DeviceManagerApiClient : IDisposable
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public DeviceManagerApiClient() : this(
            DeviceManagerConfig.BaseUrl,
            DeviceManagerConfig.ApiKey)
        { }

        public DeviceManagerApiClient(string baseUrl, string apiKey)
        {
            _apiKey = apiKey;
            _http = new HttpClient { BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/") };
            _http.DefaultRequestHeaders.Add("X-API-Key", _apiKey);
            _http.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _http.Timeout = TimeSpan.FromSeconds(30);
        }

        // ─── GET ──────────────────────────────────────────────────────────────────

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _http.GetAsync(endpoint);
            await EnsureSuccessAsync(response);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        // ─── POST (JSON) ──────────────────────────────────────────────────────────

        public async Task<T> PostAsync<T>(string endpoint, object body)
        {
            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(endpoint, content);
            await EnsureSuccessAsync(response);
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseJson);
        }

        // ─── POST (multipart/form-data) ───────────────────────────────────────────

        public async Task<T> PostMultipartAsync<T>(string endpoint, MultipartFormDataContent form)
        {
            var response = await _http.PostAsync(endpoint, form);
            await EnsureSuccessAsync(response);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        // ─── DELETE ───────────────────────────────────────────────────────────────

        public async Task<T> DeleteAsync<T>(string endpoint)
        {
            var response = await _http.DeleteAsync(endpoint);
            await EnsureSuccessAsync(response);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
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
