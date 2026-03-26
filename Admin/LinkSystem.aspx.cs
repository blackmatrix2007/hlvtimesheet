using HLVTimeSheet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HLVTimeSheet.Admin
{
    public partial class LinkSystem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string token = Request.QueryString["token"];
                if (token != null)
                {
                    // Lấy thông tin tài khoản
                    ThanhVien tv = new ThanhVien();
                    string result = tv.CheckLogin_Token(token);
                    if (result.Equals("1"))
                    {
                        Response.Redirect("~/Admin/LayoutMonthTimeSheet.aspx");
                    }
                }

                Response.Redirect("~/Admin/Login.aspx");
            }
        }
    }
}