using System;
using System.IO;
using System.Net;
using System.Web;
using HLVTimeSheet.Model.DeviceManager;

namespace HLVTimeSheet.Admin.Hander
{
    /// <summary>
    /// Proxy ảnh khuôn mặt từ DeviceManager (URL cần x-api-key).
    /// Gọi: /Admin/Hander/hdFaceImage.ashx?path=/uploads/faces/.../xxx.jpg
    /// </summary>
    public class hdFaceImage : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string path = context.Request.QueryString["path"];
            if (string.IsNullOrEmpty(path) || !path.StartsWith("/uploads/"))
            {
                context.Response.StatusCode = 400;
                return;
            }

            var config = DeviceManagerConfig.Load();
            string url = "https://device.erp-x.com" + path;

            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Method  = "GET";
                req.Timeout = 10000;
                req.Headers.Add("x-api-key", config.ApiKey);

                using (var resp = (HttpWebResponse)req.GetResponse())
                using (var stream = resp.GetResponseStream())
                {
                    context.Response.ContentType = resp.ContentType ?? "image/jpeg";
                    context.Response.Cache.SetCacheability(HttpCacheability.Private);
                    context.Response.Cache.SetMaxAge(TimeSpan.FromHours(1));

                    var buf = new byte[4096];
                    int read;
                    while ((read = stream.Read(buf, 0, buf.Length)) > 0)
                        context.Response.OutputStream.Write(buf, 0, read);
                }
            }
            catch (WebException ex) when (ex.Response is HttpWebResponse er)
            {
                context.Response.StatusCode = (int)er.StatusCode;
            }
            catch
            {
                context.Response.StatusCode = 502;
            }
        }

        public bool IsReusable => false;
    }
}
