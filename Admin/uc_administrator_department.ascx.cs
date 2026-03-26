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
    public partial class uc_administrator_department : System.Web.UI.UserControl
    {
        public string[] quyen = new string[] { "0", "0", "0", "0", "0", "0" };
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

            if (!IsPostBack)
            {
                //ExecuteData xl = new ExecuteData();
                ExecuteData xl = new ExecuteData("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@");

                string query = "SELECT pk_seq, ma, ten FROM BoPhan WHERE trangthai = '1' " +
                   " ORDER BY ten ";
                DataTable dt = xl.ReadTable(query);
                ddlBoPhan.Items.Add(new ListItem("", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlBoPhan.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                query = "SELECT pk_seq, ma, ten FROM NhaPhanPhoi WHERE trangthai = '1' " +
                   " ORDER BY stt ";
                dt = xl.ReadTable(query);
                ddlChiNhanh.Items.Add(new ListItem("", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlChiNhanh.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                ddlGroup.Items.Clear();
                ddlGroup.Items.Add(new ListItem("Operation", "1"));
                ddlGroup.Items.Add(new ListItem("Office", "2"));
                this.InitData(xl);
            }
        }

        private void InitData(ExecuteData xl)
        {
            if (xl == null)
                xl = new ExecuteData("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@");

            quyen = xl.getRole(Session["userId"].ToString(), Request.QueryString["func"].ToString());

            string query = " SELECT a.pk_seq, a.ma, a.ten, a.trangthai, " +
                           "    ISNULL((SELECT ten FROM BoPhan WHERE pk_seq = a.bophan_fk), '') bophan, " +
                           "    ISNULL((SELECT ten FROM NhaPhanPhoi WHERE pk_seq = a.chinhanh_fk), '') chinhanh, " +
                           "    CONVERT(nvarchar(10), a.ngaysua, 105) + ' ' + CONVERT(CHAR(8), a.ngaysua, 14) as ngaysua, c.ten as nguoisua " +
                           " FROM PhongBan a INNER JOIN  NhanVien c on a.nguoisua = c.pk_seq WHERE a.pk_seq > 0 ";

            if (txtTenLocation.Value.Trim().Length > 0)
                query += " and dbo.ftBodau( a.ten ) like N'%" + xl.Change_AV(txtTenLocation.Value) + "%'  ";

            query += " ORDER BY ma ASC ";
            DataTable dt = xl.ReadTable(query);
            string noidung = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string trangthai = xl.GetStatus("1", "", dt.Rows[i]["trangthai"].ToString());

                noidung += "<tr>  " +
                "	<td>" + (i + 1).ToString() + "</td> " +
                "    <td>" + dt.Rows[i]["ma"].ToString() + "</td> " +
                "    <td>" + dt.Rows[i]["ten"].ToString() + "</td> " +
                "    <td>" + dt.Rows[i]["chinhanh"].ToString() + "</td> " +
                "    <td>" + dt.Rows[i]["bophan"].ToString() + "</td> " +
                "    <td>" + trangthai + "</td> " +
                "    <td>" + dt.Rows[i]["ngaysua"].ToString() + "</td> " +
                "    <td>" + dt.Rows[i]["nguoisua"].ToString() + "</td> " +
                "	<td> ";

                if (language.Equals("1"))
                {
                    if (quyen[ExecuteData.CapNhat].Equals("1"))
                        noidung += "	    <i class='fa fa-fw fa-edit'></i> <a href='javascript:void(0);' data-toggle='modal' data-target='#exampleModal' data-whatever='" + dt.Rows[i]["pk_seq"].ToString() + "' >Edit </a> &nbsp; ";

                    if (quyen[ExecuteData.Xoa].Equals("1"))
                        noidung += "	    <i class='fa fa-fw fa-trash-o'></i> <a href='Administrator.aspx?func=94&action=delete&id=" + dt.Rows[i]["pk_seq"].ToString() + "' >Delete </a> &nbsp; ";

                }
                else if (language.Equals("2"))
                {
                    if (quyen[ExecuteData.CapNhat].Equals("1"))
                        noidung += "	    <i class='fa fa-fw fa-edit'></i> <a href='javascript:void(0);' data-toggle='modal' data-target='#exampleModal' data-whatever='" + dt.Rows[i]["pk_seq"].ToString() + "' >Sửa</a> &nbsp; ";

                    if (quyen[ExecuteData.Xoa].Equals("1"))
                        noidung += "	<i class='fa fa-fw fa-trash-o'></i> <a href='Administrator.aspx?func=94&action=delete&id=" + dt.Rows[i]["pk_seq"].ToString() + "' >Xóa </a> &nbsp; ";
                }

                noidung += "	</td> " +
                             "</tr> ";
            }
            dt.Clone();

            ltData.Text = noidung;
        }

        protected void lbTimkiem_Click(object sender, EventArgs e)
        {
            this.InitData(null);
        }
    }
}