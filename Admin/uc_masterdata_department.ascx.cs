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
    public partial class uc_masterdata_department : System.Web.UI.UserControl
    {
        public string[] quyen = new string[] { "0", "0", "0", "0", "0", "0" };
        public int pageID = 1;
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
                if (Request.QueryString["pageNumber"] != null)
                    pageID = int.Parse(Request.QueryString["pageNumber"].ToString());
                
                this.InitData(pageID, null);
            }
        }

        private void InitData(int pageID, ExecuteData xl)
        {
            if (xl == null)
                xl = new ExecuteData();

            quyen = xl.getRole(Session["userId"].ToString(), Request.QueryString["func"].ToString());

            string[] quyenGIA = new string[] { "0", "0", "0", "0", "0" };
            quyenGIA = xl.getRole(Session["userId"].ToString(), "2");

            //Lưu lại các thông tin đang search khi phân trang
            string search = "";
            if (Request.QueryString["search"] != null)
                search = Request.QueryString["search"].ToString();
            if (search.Trim().Length > 0 && search.Contains(";;"))
            {
                txtSanPham.Text = xl.GetValue(search, 0);
                ddlNganhHang.SelectedValue = xl.GetValue(search, 1);
                ddlChungLoai.SelectedValue = xl.GetValue(search, 2);
            }

            string query = " SELECT ROW_NUMBER() OVER(ORDER BY a.ma ASC) AS stt, a.pk_seq, a.ma, a.ten, a.trangthai, " +
                "   CONVERT(nvarchar(10), a.ngaysua, 105) + ' ' + CONVERT(CHAR(8), a.ngaysua, 14) as ngaysua, c.ten as nguoisua " +
                " FROM PhongBan a INNER JOIN NhanVien c on a.nguoisua = c.pk_seq " +
                " WHERE a.pk_seq > 0 AND a.trangthai in (1) ";

            if (txtSanPham.Text.Trim().Length > 0)
                query += " and (a.ten like N'%" + xl.Change_AV(txtSanPham.Text.Trim()) + "%')";
            if (ddlNganhHang.SelectedValue.Trim().Length > 3)
                query += " and a.pk_seq = N'" + ddlNganhHang.SelectedValue + "'  ";

            //CAI TIEN PHAN TRANG CHI LOAD 50 SP TREN 1 TRANG
            string queryTOTAL = " SELECT COUNT(stt) FROM ( " + query + " ) DH  ";
            object tongsoDONG = xl.ExecuteScalarSQL(queryTOTAL);
            double soDONG = 200;
            double soTRANG = 0;

            if (tongsoDONG != null)
                soTRANG = Math.Round((double.Parse(tongsoDONG.ToString()) / soDONG) + 0.5);

            if (pageID <= 1)
                pageID = 1;
            if (pageID > soTRANG)
                pageID = (int)soTRANG;

            double tuDONG = 0;
            double denDONG = 0;
            if (pageID <= 1)
            {
                tuDONG = 1;
                denDONG = soDONG;
            }
            else
            {
                tuDONG = (pageID - 1) * soDONG + 1;
                denDONG = pageID * soDONG;
            }


            string queryCHITIET = " SELECT * FROM ( " + query + " ) DH WHERE DH.stt >= " + tuDONG.ToString() + " and DH.stt <= " + denDONG.ToString();
            DataTable dt = xl.ReadTable(queryCHITIET);

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
                    noidung += " <i class='fa fa-fw fa-edit'></i> <a href='MasterData.aspx?func=7&action=capnhat&id=" + dt.Rows[i]["pk_seq"].ToString() + "' title='Edit' >Edit</a> &nbsp; ";

                //if (quyen[ExecuteData.Xoa].Equals("1"))
                //    noidung += "	    <i class='fa fa-fw fa-trash-o' style='color:red;'></i> <a style='color:red;' href='MasterData.aspx?func=7&action=delete&id=" + dt.Rows[i]["pk_seq"].ToString() + "' title='Delete' >Delete </a> &nbsp; ";

                noidung += "	</td> " +
                    "</tr> ";
            }

            dt.Clone();

            ltData.Text = noidung;

            //PHAN TRANG
            string strPage = "";
            for (int i = 1; i <= (int)soTRANG; i++)
            {
                if (pageID == i)
                    strPage += "<option value='" + i.ToString() + "' selected='selected' >" + i.ToString() + "</option>";
                else
                    strPage += "<option value='" + i.ToString() + "' >" + i.ToString() + "</option>";
            }

            ltPhanTrang.Text = " <table style='width:100%;' > " +
            "     <tr>" +
            "         <td>" +
            "             <ul class='pagination pagination-sm no-margin '>" +
            "                 <li><a href='javascript:void(0);'><img src='../Images/first.gif' width='16' height='16' title='The first page' onclick='moveTO2(-1, 1)' /></a></li>" +
            "                 <li><a href='javascript:void(0);'><img src='../Images/previous.gif' width='16' height='16' title='Previous' onclick='moveTO2(-1, -1)' /></a></li>" +
            "                 <li><a href='javascript:void(0);'><img src='../Images/next.gif' width='16' height='16' title='Next' onclick='moveTO2(1, -1)' /></a></li>" +
            "                 <li><a href='javascript:void(0);'><img src='../Images/last.gif' width='16' height='16' title='Last' onclick='moveTO2(1, 1)' /></a></li>" +
            "                 <li style='padding-left:10px;' >" +
            "                     <input type='hidden' id='pagedropdownNUMBER_MAX' value='" + soTRANG + "' >" +
            "                     <SELECT id='pagedropdownNUMBER' style='width:50px; height:28px;' onchange='moveTO(this.value)' >" +
                        strPage +
            "                     </select>" +
            "                 </li>" +
            "                 <li style='margin-left:30px;' >" +
            "                     Show " +
            "                     <SELECT style='width:50px; height:28px;' >" +
            "                         <option value='200' >200</option>" +
            "                     </select>" +
            "                     line on page " +
            "                 </li>" +
            "             </ul>" +
            "         </td>" +
            "         <td style='text-align:right;' >" +
            "             Line " + tuDONG + "-" + denDONG + " of total " + tongsoDONG + " line, Page " + pageID + " of " + soTRANG.ToString() +
            "         </td>" +
            "     </tr>" +
            " </table>";

        }

        protected void lbTimkiem_Click(object sender, EventArgs e)
        {
            pageID = 1;
            this.InitData(pageID, null);
        }

        protected void txtSanPham_TextChanged(object sender, EventArgs e)
        {
            pageID = 1;
            this.InitData(pageID, null);
        }
    }
}