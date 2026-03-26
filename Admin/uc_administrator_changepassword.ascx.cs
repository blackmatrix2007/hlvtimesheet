using HLVTimeSheet.AcsessData;
using HLVTimeSheet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HLVTimeSheet.Admin
{
    public partial class uc_administrator_changepassword : System.Web.UI.UserControl
    {
        public string language = "1";
        protected void Page_Load(object sender, EventArgs e)
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
        protected void lbCapnhat_Click(object sender, EventArgs e)
        {
            if (txtPassword.Value.Trim().Length <= 0)
            {
                if (language.Equals("1"))
                    lbMsg.Text = "Please enter a new password";
                else if (language.Equals("2"))
                    lbMsg.Text = "Vui lòng nhập mật khẩu mới";
                return;
            }

            if (txtPassword.Value.Trim().Length < 6)
            {
                if (language.Equals("1"))
                    lbMsg.Text = "The new password must be 6 characters or more";
                else if (language.Equals("2"))
                    lbMsg.Text = "Mật khẩu mới phải tư 6 ký tự trở lên";

                return;
            }

            if (!txtPassword2.Value.Trim().Equals(txtPassword.Value.Trim()))
            {

                if (language.Equals("1"))
                    lbMsg.Text = "Confirmation password does not match. Please check again";
                else if (language.Equals("2"))
                    lbMsg.Text = "Mật khẩu xác nhận lại không khớp. Vui lòng kiểm tra lại";

                return;
            }

            ExecuteData xl = new ExecuteData();
            string userId = Session["userId"].ToString();

            

            string query = "UPDATE NhanVien SET matkhau = pwdencrypt('" + txtPassword.Value.Trim() + "'), nguoisua = '" + userId + "', ngaysua = getdate() WHERE pk_seq = '" + userId + "' ";
            if (xl.ExecuteNonQuerySQL(query))
            {
                // Hệ thống OC
                NhanVien nv = new NhanVien();
                string msg = nv.ChangePassword_PC("", txtPassword.Value, userId);
                if (msg.Length > 10)
                {
                    lbMsg.Text = "Error! Please try again.";
                }
                msg = nv.ChangePassword_OC("", txtPassword.Value, userId);
                if (msg.Length > 10)
                {
                    lbMsg.Text = "Error! Please try again.";
                }
                
                msg = nv.ChangePassword_NWHP("", txtPassword.Value, userId);
                if (msg.Length > 10)
                {
                    lbMsg.Text = "Error! Please try again.";
                }

                msg = nv.ChangePassword_BWHP("", txtPassword.Value, userId);
                if (msg.Length > 10)
                {
                    lbMsg.Text = "Error! Please try again.";
                }

                msg = nv.ChangePassword_WHVP("", txtPassword.Value, userId);
                if (msg.Length > 10)
                {
                    lbMsg.Text = "Error! Please try again.";
                }
                msg = nv.ChangePassword_WS("", txtPassword.Value, userId);
                if (msg.Length > 10)
                {
                    lbMsg.Text = "Error! Please try again.";
                }

                if (language.Equals("1"))
                    lbMsg.Text = "Confirmation password does not match. Please check again";
                else if (language.Equals("2"))
                    lbMsg.Text = "Đổi mật khẩu thành công";
            }                
            else
            {
                if (language.Equals("1"))
                    lbMsg.Text = "Error! Please try again.";
                else if (language.Equals("2"))
                    lbMsg.Text = "Lỗi khi đổi mật khẩu ";

            }
        }
    }
}