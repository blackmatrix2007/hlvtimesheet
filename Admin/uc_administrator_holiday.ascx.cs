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
    public partial class uc_administrator_holiday : System.Web.UI.UserControl
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
                this.InitData();
            }
        }

        private void InitData()
        {
            ExecuteData xl = new ExecuteData("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@");
            quyen = xl.getRole(Session["userId"].ToString(), Request.QueryString["func"].ToString());

            string query = " SELECT a.pk_seq, a.ngaynghi, a.denngay, a.noidung, a.trangthai, " +
                           " CASE hangnam WHEN 0 THEN N'One year' " +
                           "              WHEN 1 THEN N'Yearly' END hangnam, " + 
                           "    CONVERT(nvarchar(10), a.ngaysua, 105) + ' ' + CONVERT(CHAR(8), a.ngaysua, 14) as ngaysua, c.ten as nguoisua " +
                           " FROM NgayNghiLe a INNER JOIN NHANVIEN c on a.nguoisua = c.pk_seq " +
                           " WHERE a.pk_seq > 0 ";

            if (txtNoiDung.Value.Trim().Length > 0)
                query += " and dbo.ftBodau( a.noidung ) like N'%" + xl.Change_AV(txtNoiDung.Value) + "%'  ";

            query += " ORDER BY nam DESC, thang, ngay ASC ";
            DataTable dt = xl.ReadTable(query);
            string noidung = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string trangthai = xl.GetStatus("1", "", dt.Rows[i]["trangthai"].ToString());

                noidung += "<tr>  " +
                             "	<td>" + (i + 1).ToString() + "</td> " +
                             "    <td>" + dt.Rows[i]["ngaynghi"].ToString() + "</td> " +
                             "    <td>" + dt.Rows[i]["denngay"].ToString() + "</td> " +
                             "    <td>" + dt.Rows[i]["noidung"].ToString() + "</td> " +
                             "    <td>" + dt.Rows[i]["hangnam"].ToString() + "</td> " +
                             "    <td>" + trangthai + "</td> " +
                             "    <td>" + dt.Rows[i]["ngaysua"].ToString() + "</td> " +
                             "    <td>" + dt.Rows[i]["nguoisua"].ToString() + "</td> " +
                             "	<td> ";

                if (language.Equals("1"))
                {
                    if (quyen[ExecuteData.CapNhat].Equals("1"))
                        noidung += "	    <i class='fa fa-fw fa-edit'></i> <a href='Administrator.aspx?func=98&action=capnhat&id=" + dt.Rows[i]["pk_seq"].ToString() + "' title='Edit' >Edit</a> &nbsp; ";

                    if (quyen[ExecuteData.Xoa].Equals("1"))
                        noidung += "	    <i class='fa fa-fw fa-trash-o'></i> <a href='Administrator.aspx?func=98&action=delete&id=" + dt.Rows[i]["pk_seq"].ToString() + "' >Delete </a> &nbsp; ";

                }
                else if (language.Equals("2"))
                {
                    if (quyen[ExecuteData.CapNhat].Equals("1"))
                        noidung += "	    <i class='fa fa-fw fa-edit'></i> <a href='Administrator.aspx?func=98&action=capnhat&id=" + dt.Rows[i]["pk_seq"].ToString() + "' title='Sửa' >Sửa</a> &nbsp; ";

                    if (quyen[ExecuteData.Xoa].Equals("1"))
                        noidung += "	<i class='fa fa-fw fa-trash-o'></i> <a href='Administrator.aspx?func=98&action=delete&id=" + dt.Rows[i]["pk_seq"].ToString() + "' >Xóa </a> &nbsp; ";
                }

                noidung += "	</td> " +
                             "</tr> ";
            }
            dt.Clone();

            ltData.Text = noidung;
        }

        protected void lbTimkiem_Click(object sender, EventArgs e)
        {
            this.InitData();
        }
    }
}