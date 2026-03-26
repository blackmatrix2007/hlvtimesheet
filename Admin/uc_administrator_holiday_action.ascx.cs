using HLVTimeSheet.AcsessData;
using HLVTimeSheet.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HLVTimeSheet.Admin
{
    public partial class uc_administrator_holiday_action : System.Web.UI.UserControl
    {
        public string id;        
        public string language = "1";

        public uc_administrator_holiday_action()
        {
            this.id = "";            
        }

        public uc_administrator_holiday_action(string id)
        {
            this.id = id;            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ExecuteData xl = new ExecuteData("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@");

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

            if (!IsPostBack)
            {
                ddlLoai.Items.Add(new ListItem("Yearly", "1"));
                ddlLoai.Items.Add(new ListItem("One year", "0"));
            }

            if (this.id.Trim().Length > 0)
            {
                string query = " SELECT pk_seq, ngaynghi, denngay, noidung, hangnam, trangthai " +
                " FROM NgayNghiLe WHERE pk_seq = '" + id + "' ";
                DataTable dt = xl.ReadTable(query);

                if (dt.Rows.Count > 0)
                {
                    txtTuNgay.Text = dt.Rows[0]["ngaynghi"].ToString();
                    txtDenNgay.Text = dt.Rows[0]["denngay"].ToString();
                    txtNoiDung.Text = dt.Rows[0]["noidung"].ToString();

                    ddlLoai.SelectedValue = dt.Rows[0]["hangnam"].ToString();

                    if(dt.Rows[0]["trangthai"].ToString().Equals("1"))
                        chkTrangThai.Checked = true;
                    else
                        chkTrangThai.Checked = false;
                }

            }
            else
            {
                txtTuNgay.Text = "";
                txtDenNgay.Text = "";
                txtNoiDung.Text = "";                
                ddlLoai.SelectedValue = "1";               
                chkTrangThai.Checked = true;

            }
        }
        protected void lbLuuLai_Click(object sender, EventArgs e)
        {           
            if (txtTuNgay.Text.Trim().Length <= 0)
            {
                lblError.Text = "Error! You must be to input datetime";
                return;
            }

            if (txtDenNgay.Text.Trim().Length <= 0)
            {
                lblError.Text = "Error! You must be to input datetime";
                return;
            }

         
            string trangthai = "0";
            if (chkTrangThai.Checked)
                trangthai = "1";

            string msg = "";
            MasterControlModel controlModel = new MasterControlModel();

            msg = controlModel.SAVE_Holidays(id, txtTuNgay.Text, txtDenNgay.Text, ddlLoai.SelectedValue, txtNoiDung.Text, trangthai, Session["userId"].ToString());

            if (msg.Trim().Length <= 10)
            {
                lblError.Text = "";
                Response.Redirect("Administrator.aspx?func=98");
            }
            else
            {
                lblError.Text = "Cannot save this information. Please try agian! ";
            }

        }        
    }
}