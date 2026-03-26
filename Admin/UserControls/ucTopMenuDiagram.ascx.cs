using HLVTimeSheet.AcsessData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HLVTimeSheet.Admin.UserControls
{
    public partial class ucTopMenuDiagram : System.Web.UI.UserControl
    {
        public string worktime = "";
        public string language = "1";
        public string path = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            path = HttpContext.Current.Request.Url.AbsoluteUri;
            path = path.Replace("&lang=1", "");
            path = path.Replace("&lang=2", "");

            if (path.Contains("Admin/Index.aspx") || path.Contains("Admin/Homepage.aspx")
                 || path.Contains("Admin/PalletTracking.aspx"))
                path = path + "?";

            if (!IsPostBack)
            {
                string lang = Request.QueryString["lang"];
                if (lang != null)
                {
                    Session["language"] = lang;
                    language = lang;
                }
                else if (Session["language"] != null)
                {
                    language = Session["language"].ToString();
                }
            }
        }
    }
}