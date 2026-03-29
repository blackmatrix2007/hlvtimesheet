using System;
using System.Net;
using System.Web;
using HLVTimeSheet.Model.DeviceManager;

namespace HLVTimeSheet.Admin.Hander
{
    public class hdFaceImage : IHttpHandler
    {
        private const string BASE_URL = "https://device.erp-x.com";

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Cache.SetNoStore();

            string path = context.Request.QueryString["path"];
            if (string.IsNullOrEmpty(path) || !path.StartsWith("/uploads/"))
            {
                context.Response.StatusCode  = 400;
                context.Response.ContentType = "text/plain";
                context.Response.Write("Bad request: missing or invalid path");
                return;
            }

            // Load API key từ config (DB hoặc Web.config)
            var cfg    = DeviceManagerConfig.Load();
            string url = BASE_URL + path;

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Method    = "GET";
                req.Timeout   = 15000;
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36";
                req.Headers["x-api-key"] = cfg.ApiKey;

                HttpWebResponse resp;
                try
                {
                    resp = (HttpWebResponse)req.GetResponse();
                }
                catch (WebException wex)
                {
                    var errResp = wex.Response as HttpWebResponse;
                    int code = errResp != null ? (int)errResp.StatusCode : 502;
                    context.Response.StatusCode  = code;
                    context.Response.ContentType = "text/plain";
                    context.Response.Write($"Upstream {code}: {wex.Message}");
                    return;
                }

                using (resp)
                using (var stream = resp.GetResponseStream())
                {
                    context.Response.ContentType = resp.ContentType ?? "image/jpeg";
                    context.Response.Cache.SetCacheability(HttpCacheability.Private);
                    context.Response.Cache.SetMaxAge(TimeSpan.FromHours(1));

                    var buf = new byte[8192];
                    int read;
                    while ((read = stream.Read(buf, 0, buf.Length)) > 0)
                        context.Response.OutputStream.Write(buf, 0, read);
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode  = 500;
                context.Response.ContentType = "text/plain";
                context.Response.Write("Error: " + ex.GetType().Name + ": " + ex.Message);
            }
        }

        public bool IsReusable => false;
    }
}
