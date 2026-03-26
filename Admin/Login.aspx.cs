using HLVTimeSheet.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HLVTimeSheet.Admin
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["userName"] = "";
            Session["userId"] = "";
            Session["typesOfAccount"] = "";
            
        }

        protected void lbDangNhap_Click(object sender, EventArgs e)
        {
            logIn();
        }

        protected void txtUserName_TextChanged(object sender, System.EventArgs e)
        {
            //logIn();
        }

        protected void txtPassword_TextChanged(object sender, System.EventArgs e)
        {
            logIn();
        }

        private void logIn()
        {
            string msg = "";
            lblError.Text = "";
            if (txtPassword.Text.Trim().Length > 0 && txtUserName.Text.Trim().Length > 0)
            {
                ThanhVien member = new ThanhVien();

                int kq = int.Parse(member.CheckDangNhap(txtUserName.Text, txtPassword.Text));
                if (kq == -2)
                {
                    msg = "Warning! Access failed.";
                }
                else if (kq == -1)
                {
                    msg = "Username is incorrect";
                }
                else if (kq == 0)
                {
                    msg = "Password is incorrect";
                }

                if (msg.Length > 0)
                    lblError.Text = msg;
                else
                    Response.Redirect("~/Admin/Homepage.aspx");
            }
        }
    }
}