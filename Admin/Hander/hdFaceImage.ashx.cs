using System;
using System.Net;
using System.Web;

namespace HLVTimeSheet.Admin.Hander
{
    public class hdFaceImage : IHttpHandler
    {
        private const string API_KEY     = "ck_a49fbf00754cc51f3b20d3eb719fffe6";
        private const string BASE_URL    = "https://device.erp-x.com";

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

            string url = BASE_URL + path;

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Method  = "GET";
                req.Timeout = 15000;
                req.Headers["x-api-key"] = API_KEY;
                req.UserAgent = "HLVTimeSheet/1.0";

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
