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
    public partial class uc_administrator_teamwork_action : System.Web.UI.UserControl
    {
        public string id;
        public string trangthai;
        public string language = "1";
        public string listNhanSu = "";

        public uc_administrator_teamwork_action()
        {
            this.id = "";
            this.trangthai = "0";
        }

        public uc_administrator_teamwork_action(string id)
        {
            this.id = id;
            this.trangthai = "0";
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
                string query = "SELECT pk_seq,'[' + ISNULL((SELECT ten FROM ChucVu WHERE pk_seq = a.chucvu_fk), '') + '] ' + ten AS ten FROM DanhSachNhanSu a WHERE trangthai in (1, 2, 3) ORDER BY capbac, ten ";
                DataTable dt = xl.ReadTable(query);

                ddlNhanSu.Items.Add(new ListItem("", "0"));
                ddlQuanLy.Items.Add(new ListItem("", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlQuanLy.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                    ddlNhanSu.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                // 
                query = "SELECT pk_seq FROM DanhSachNhanSu WHERE nhanvien_fk = '" + Session["userId"].ToString() + "' ";
                object obj = xl.ExecuteScalarSQL(query);
                if (obj != null)
                    ddlQuanLy.SelectedValue = obj.ToString();
               
                dt.Clear();
                dt.Clone();
            }

            if (this.id.Trim().Length > 3)
            {
                string query = "SELECT a.ma, a.ten, a.ghichu, a.quanly_fk, a.trangthai " +
                " FROM NhomLamViec a WHERE a.pk_seq = '" + this.id + "' ";
                DataTable dt = xl.ReadTable(query);

                if (dt.Rows.Count > 0)
                {
                    txtMa.Text = dt.Rows[0]["ma"].ToString();
                    txtTenNhom.Text = dt.Rows[0]["ten"].ToString();
                    txtGhiChu.Text = dt.Rows[0]["ghichu"].ToString();
                    ddlQuanLy.SelectedValue = dt.Rows[0]["quanly_fk"].ToString();

                    this.trangthai = dt.Rows[0]["trangthai"].ToString();
                    if (trangthai.Equals("1"))
                        ckStatus.Checked = true;

                    query = "SELECT nhansu_fk FROM NhomLamViec_NhanSu WHERE nhomlamviec_fk = '" + this.id + "' ";
                    dt = xl.ReadTable(query);
                    if (dt.Rows.Count > 0)
                    {
                        listNhanSu = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            listNhanSu += dt.Rows[i]["nhansu_fk"].ToString() + ",";                           
                        }
                        if (listNhanSu.Length > 3)
                            listNhanSu = listNhanSu.Substring(0, listNhanSu.Length - 1);
                    }

                   
                }
            }           
        }
        protected void lbLuuLai_Click(object sender, EventArgs e)
        {
            if (txtTenNhom.Text.Trim().Length <= 0)
            {
                lblError.Text = "Error! You must be input name";
                return;
            }

            if (ddlNhanSu.Value.Trim().Length < 3)
            {
                lblError.Text = "Error! You must be choose staff";
                return;
            }

            if (ddlQuanLy.SelectedValue.Trim().Length < 3)
            {
                lblError.Text = "Error! You must be choose admin";
                return;
            }

            string trangthai = "0";
            if (ckStatus.Checked)
                trangthai = "1";

            string msg = "";

            TeamworkControlModel teamworkControl = new TeamworkControlModel();

            if (this.id.Trim().Length <= 3)
            {
                msg = teamworkControl.INSERT_Teamwork(txtMa.Text, txtTenNhom.Text, ddlQuanLy.SelectedValue, txtGhiChu.Text, txtNhanSu.Value, trangthai, Session["userId"].ToString());
            }
            else
            {
                msg = teamworkControl.UPDATE_Teamwork(id, txtMa.Text, txtTenNhom.Text, ddlQuanLy.SelectedValue, txtGhiChu.Text, txtNhanSu.Value, trangthai, Session["userId"].ToString());
            }

            if (msg.Trim().Length <= 10)
            {               
                Response.Redirect("Administrator.aspx?func=97");
            }
            else
            {
                lblError.Text = msg;
            }

        }

    }
}