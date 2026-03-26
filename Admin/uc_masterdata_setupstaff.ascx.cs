using HLVTimeSheet.AcsessData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HLVTimeSheet.Admin
{
    public partial class uc_masterdata_setupstaff : System.Web.UI.UserControl
    {

        public string trangthai;
        public string language = "1";
        public string versionPlan = "1";
        public uc_masterdata_setupstaff()
        {
            this.trangthai = "0";
        }
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

            if (!IsPostBack)
            {
                ExecuteData xl = new ExecuteData();
                string query = "SELECT pk_seq, '[ ' + ma + ' ] ' + ten AS ten FROM NhaPhanPhoi WHERE trangthai = '1' ORDER BY ma ";
                DataTable dt = xl.ReadTable(query);

                ddlChiNhanh.Items.Add(new ListItem("Branch", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlChiNhanh.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                query = "SELECT pk_seq, '[ ' + ma + ' ] ' + ten AS ten FROM PhongBan WHERE trangthai = '1' ORDER BY ma ";
                dt = xl.ReadTable(query);

                ddlPhongBan.Items.Add(new ListItem("Department", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlPhongBan.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                ddlNam.Items.Add(new ListItem("Year", "0"));
                int namhientai = int.Parse(DateTime.Now.ToString("yyyy"));

                for (int i = namhientai - 8; i < namhientai + 2; i++)
                {
                    ddlNam.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
                ddlNam.SelectedValue = DateTime.Now.ToString("yyyy");

                for (int i = 1; i <= 12; i++)
                {
                    string t = i.ToString();
                    if (i < 10)
                        t = "0" + t;

                    ddlThang.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }

                ddlThang.SelectedValue = DateTime.Now.ToString("MM");
                txtNgayNhap.Value = DateTime.Now.ToString("dd-MM-yyyy");
                this.loadVersionSale();
            }

        }

        private void loadInfo(ExecuteData xl)
        {
            if (xl == null)
                xl = new ExecuteData();
            //this.loadTitle();

            ltInfor.Text = "";

            string style = "";
            string title = "";
            string content = "";
            string condition = "";
            string sql = "";

            sql = "SELECT pk_seq AS phongban_fk, ma, ten, chinhanh_fk FROM PhongBan WHERE trangthai = 1 ORDER BY stt, ma ";
            DataTable dtP = xl.ReadTable(sql);

            txtSoPhong.Value = dtP.Rows.Count.ToString();
            title += " <tr>" +
               " <th style='text-align:center; width:3%; border:1px solid black; background-color:lightgray; padding:1px;' rowspan='2'>#</th>" +
               " <th style='text-align:center; width:12%; border:1px solid black; background-color:lightgray; padding:1px;' rowspan='2'>Name</th>" +
               //" <th style='text-align:center; width:5%; border:1px solid black; background-color:lightgray; padding:1px;' rowspan='2'>ID</th>" +
               " <th style='text-align:center; width:12%; border:1px solid black; background-color:lightgray; padding:1px;' rowspan='2'>Position</th>" +
               " <th style='text-align:center; width:72%; border:1px solid black; background-color:lightgray; padding:1px;' colspan='" + dtP.Rows.Count + "'>Chi phí cho nhân viên ở từng vị trí làm việc</th>";
            title += " </tr>";

            title += " <tr>";
            for(int i = 0; i < dtP.Rows.Count; i++)
            {
                title += " <th style='text-align:center; width:6%; border:1px solid black; background-color:lightgray; padding:1px;'>" + dtP.Rows[i]["ma"].ToString() + "</th>";
            }                         
            title += " </tr>";

            if (ddlChiNhanh.SelectedValue.Trim().Length > 3)
                condition += " AND a.chinhanh_fk = '" + ddlChiNhanh.SelectedValue + "' ";
            if (ddlPhongBan.SelectedValue.Trim().Length > 3)
                condition += " AND a.phongban_fk = '" + ddlPhongBan.SelectedValue + "' ";

            sql = "SELECT pk_seq AS nhansu_fk, ma, ten, hinhanh, phongban_fk, ISNULL((SELECT ten FROM ChucVu WHERE pk_seq = a.chucvu_fk), '') chucvu, chucvu_fk, capbac " + 
                " FROM DanhSachNhanSu a " + 
                " WHERE trangthai = 1 " + condition + 
                " ORDER BY capbac, ten ";
            DataTable dtK = xl.ReadTable(sql);

            for (int i = 0; i < dtK.Rows.Count; i++)
            {
                style = "";
                if (i % 2 == 1)
                    style = " background-color: antiquewhite;";

                content += "<tr>" +
                " <td style='display:none;'><input type='text' class='form-control' name='nhansu_fk' value='" + dtK.Rows[i]["nhansu_fk"].ToString() + "'></td> " + 
                " <td style='text-align:center; border:1px solid black; padding:1px; height:18px; font-size:small;'>" + (i + 1).ToString() + "</td>" +

                " <td style='text-align:left; border:1px solid black; padding:1px; height:18px; font-size:small;" + style + "'>" + dtK.Rows[i]["ten"].ToString() + "</td>" +
                //" <td style='text-align:left; border:1px solid black; padding:1px; height:18px; font-size:small;" + style + "'>" + dtK.Rows[i]["ma"].ToString() + "</td>" +
                " <td style='text-align:left; border:1px solid black; padding:1px; height:18px; font-size:small;" + style + "'>" + dtK.Rows[i]["chucvu"].ToString() + "</td>";

                for (int j = 0; j < dtP.Rows.Count; j++)
                {
                    string str = this.returnStr(j);
                    string chiphi = "";

                    sql = "SELECT chiphi FROM DanhSachNhanSu_ChiPhi WHERE nhansu_fk = '" + dtK.Rows[i]["nhansu_fk"].ToString() + "' AND phongban_fk = '" + dtP.Rows[j]["phongban_fk"].ToString() + "' AND thang = '" + ddlThang.SelectedValue + "' AND nam = '" + ddlNam.SelectedValue + "' ";
                    DataTable dtS = xl.ReadTable(sql);

                    if (dtS.Rows.Count > 0)
                        chiphi = FormatString.ForMatNumber_New(dtS.Rows[0]["chiphi"].ToString());

                    content += " <td style='display:none;'><input type='text' class='form-control' name='phongban_fk" + str + "' style='text-align:right; padding:1px; height:18px; font-size:small;" + style + "' value='" + dtP.Rows[j]["phongban_fk"].ToString() + "'></td>";
                    content += " <td style='display:none;'><input type='text' class='form-control' name='chiphiOLD" + str + "' style='text-align:right; padding:1px; height:18px; font-size:small;" + style + "' onkeyup='FormartNumberJS(this);' value='" + chiphi + "'></td>";
                    content += " <td style='text-align:right; border:1px solid black; " + style + "'><input type='text' class='form-control' name='chiphi" + str + "' style='text-align:right; padding:1px; height:18px; font-size:small;" + style + "' onkeyup='FormartNumberJS(this);' value='" + chiphi + "'></td>";
                }
                content += " </tr>";
            }

            dtK.Clear();
            dtK.Clone();

            //content += "</table> ";
            ltInfor.Text += title;
            ltInfor.Text += content;
            ltInfor.Text += title;
        }

        private void loadVersionSale()
        {
            //string versionChooes = ddlPhienBan.SelectedValue;

            //txtNgayNhap.Value = DateTime.Now.ToString("dd-MM-yyyy");

            //ddlPhienBan.Items.Clear();
            //if (ddlNam.SelectedValue.Trim().Length > 3)
            //{
            //    ExecuteData xl = new ExecuteData();
            //    string sql = "SELECT TOP(1) pk_seq, thang, nam FROM DanhSachNhanSu_ChiPhi  " +
            //        " WHERE trangthai in (1) AND nam = '" + ddlNam.SelectedValue + "' AND thang = '" + ddlThang.SelectedValue + "' " +
            //        " ORDER BY pk_seq DESC ";
            //    DataTable dt = xl.ReadTable(sql);
            //    if (dt.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            string str = "Version " + dt.Rows[i]["thang"].ToString();
            //            ddlPhienBan.Items.Add(new ListItem(str, dt.Rows[i]["pk_seq"].ToString()));
            //        }

            //        if (versionChooes.Length > 3)
            //            ddlPhienBan.SelectedValue = versionChooes;
            //    }
            //    else
            //        ddlPhienBan.Items.Add(new ListItem("Version", "0"));

            //   
            //}

            this.loadInfo(null);
        }

        protected void ddlPhienBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.loadVersionSale();
        }

        protected void lbTimkiem_Click(object sender, EventArgs e)
        {
            this.loadVersionSale();
        }

        protected void ddlChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.loadVersionSale();
        }

        protected void ddlPhongBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.loadVersionSale();
        }

        protected void ddlNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.loadVersionSale();
        }

        protected void ddlThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.loadVersionSale();
        }

        private string returnStr(int number)
        {
            if (number < 10)
                return "0" + number.ToString();
            else
                return number.ToString();
        }
    }
}