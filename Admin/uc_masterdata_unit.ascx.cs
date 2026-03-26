using HLVTimeSheet.AcsessData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace HLVTimeSheet.Admin
{
    public partial class uc_masterdata_unit : System.Web.UI.UserControl
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
            ExecuteData xl = new ExecuteData();
            quyen = xl.getRole(Session["userId"].ToString(), Request.QueryString["func"].ToString());

            string query = " SELECT a.pk_seq, a.ma, a.ten, a.trangthai, " +
                           "    CONVERT(nvarchar(10), a.ngaysua, 105) + ' ' + CONVERT(CHAR(8), a.ngaysua, 14) as ngaysua, c.ten as nguoisua " +
                           " FROM DonViTinh a INNER JOIN  NhanVien c on a.nguoisua = c.pk_seq WHERE a.pk_seq > 0 ";

            if (txtTendonvi.Value.Trim().Length > 0)
                query += " and dbo.ftBodau( a.ten ) like N'%" + xl.Change_AV(txtTendonvi.Value) + "%'  ";

            query += " ORDER BY pk_seq DESC ";
            DataTable dt = xl.ReadTable(query);
            string noidung = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string trangthai = xl.GetStatus("1", "", dt.Rows[i]["trangthai"].ToString());

                noidung += "<tr>  " +
                             "	<td>" + (i + 1).ToString() + "</td> " +
                             "    <td>" + dt.Rows[i]["ma"].ToString() + "</td> " +
                             "    <td>" + dt.Rows[i]["ten"].ToString() + "</td> " +
                             "    <td>" + trangthai + "</td> " +
                             "    <td>" + dt.Rows[i]["ngaysua"].ToString() + "</td> " +
                             "    <td>" + dt.Rows[i]["nguoisua"].ToString() + "</td> " +
                             "	<td> ";
                if (quyen[ExecuteData.CapNhat].Equals("1"))
                    noidung += "	    <i class='fa fa-fw fa-edit'></i> <a href='javascript:void(0);' data-toggle='modal' data-target='#exampleModal' data-whatever='" + dt.Rows[i]["pk_seq"].ToString() + "' >Edit </a> &nbsp; ";

                if (quyen[ExecuteData.Xoa].Equals("1"))
                    noidung += "	    <i class='fa fa-fw fa-trash-o' style='color:red;'></i> <a style='color:red;' href='MasterData.aspx?func=8&action=delete&id=" + dt.Rows[i]["pk_seq"].ToString() + "' >Delete </a> &nbsp; ";

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