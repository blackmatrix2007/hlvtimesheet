using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using HLVTimeSheet.Model.DeviceManager;

namespace HLVTimeSheet.Admin.Hander
{
    /// <summary>
    /// Proxy ảnh khuôn mặt từ DeviceManager (URL nội bộ cần x-api-key).
    /// Gọi: /Admin/Hander/hdFaceImage.ashx?path=/uploads/faces/.../xxx.jpg
    /// </summary>
    public class hdFaceImage : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string path = context.Request.QueryString["path"];
            if (string.IsNullOrEmpty(path))
            {
                context.Response.StatusCode = 400;
                return;
            }

            var config = DeviceManagerConfig.Load();
            string url = "https://device.erp-x.com" + path;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("x-api-key", config.ApiKey);
                    var response = Task.Run(() => client.GetAsync(url)).GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        context.Response.StatusCode = (int)response.StatusCode;
                        return;
                    }

                    byte[] bytes = Task.Run(() => response.Content.ReadAsByteArrayAsync()).GetAwaiter().GetResult();
                    string mime  = response.Content.Headers.ContentType?.MediaType ?? "image/jpeg";

                    context.Response.ContentType = mime;
                    context.Response.Cache.SetCacheability(HttpCacheability.Private);
                    context.Response.Cache.SetMaxAge(TimeSpan.FromHours(1));
                    context.Response.BinaryWrite(bytes);
                }
            }
            catch
            {
                context.Response.StatusCode = 502;
            }
        }

        public bool IsReusable => false;
    }
}
