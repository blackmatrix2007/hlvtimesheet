using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLVTimeSheet.Admin.UserControls;
using HLVTimeSheet.AcsessData;
using System.Data;

namespace HLVTimeSheet.Admin
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public string language = "1";
        public string roleSys = "1";
        protected void Page_Load(object sender, EventArgs e)
        {
            ucLeftMenu ucleft = (ucLeftMenu)this.Master.FindControl("ucLeftMenu1");
            ucleft.action = "3";

            ExecuteData xl = new ExecuteData();

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

            if (Session["userId"] == null || Session["userId"].ToString().Trim().Length <= 3 || Session["token"] == null || Session["token"].ToString().Trim().Length <= 3)
            {
                Response.Redirect("~/Admin/Login.aspx");
                return;
            }

            string sql = "SELECT ISNULL(khachhang_fk, 0) FROM NhanVien WHERE pk_seq = '" + Session["userId"].ToString() + "' ";
            object obj = xl.ExecuteScalarSQL(sql);
            if (int.Parse(obj.ToString()) > 0)
            {
                roleSys = "0";
                Response.Redirect("~/Admin/Report.aspx");
            }
          
        }
    }
}