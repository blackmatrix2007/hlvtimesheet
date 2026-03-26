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
    public partial class uc_administrator_teamwork : System.Web.UI.UserControl
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

                ExecuteData xl = new ExecuteData("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@");

                string query = "SELECT pk_seq,'[' + ISNULL((SELECT ten FROM ChucVu WHERE pk_seq = a.chucvu_fk), '') + '] ' + ten AS ten FROM DanhSachNhanSu a WHERE trangthai in (1, 2, 3) ORDER BY capbac, ten ";
                DataTable dt = xl.ReadTable(query);

                if (language.Equals("1"))
                {
                    ddlNhanSu.Items.Add(new ListItem("Choose staff", "0"));
                }
                else if (language.Equals("2"))
                {
                    ddlNhanSu.Items.Add(new ListItem("Chọn nhân sự", "0"));
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlNhanSu.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                dt.Clear();
                dt.Clone();
                this.InitData(pageID, xl);
            }
        }

        private void InitData(int pageID, ExecuteData xl)
        {
            if (xl == null)
                xl = new ExecuteData("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@");

            quyen = xl.getRole(Session["userId"].ToString(), Request.QueryString["func"].ToString());

            //Lưu lại các thông tin đang search khi phân trang
            string search = "";
            if (Request.QueryString["search"] != null)
                search = Request.QueryString["search"].ToString();
            if (search.Trim().Length > 0 && search.Contains(";;"))
            {
                ddlNhanSu.SelectedValue = xl.GetValue(search, 0);                
                txtTen.Value = xl.GetValue(search, 1);
            }

            string query = " SELECT ROW_NUMBER() OVER(ORDER BY a.ma ASC) AS stt, a.pk_seq, a.ma, a.ten, " +
                "    a.trangthai, a.ghichu, " +
                "    ISNULL((SELECT ten FROM DanhSachNhanSu WHERE pk_seq = a.quanly_fk), '') quanly,   " +
                "    CONVERT(nvarchar(10), a.ngaysua, 105) + ' ' + CONVERT(CHAR(8), a.ngaysua, 14) as ngaysua, c.ten as nguoisua " +
                " FROM NhomLamViec a INNER JOIN NhanVien c on a.nguoisua = c.pk_seq WHERE a.pk_seq > 0 ";

            if (ddlNhanSu.SelectedValue.Trim().Length > 3)
                query += " and a.pk_seq in (SELECT nhomlamviec_fk FROM NhomLamViec_NhanSu WHERE nhansu_fk = '" + ddlNhanSu.SelectedValue + "') ";
            if (txtTen.Value.Trim().Length > 0)
                query += " and a.ten like N'%" + xl.Change_AV(txtTen.Value) + "%'  ";

            query += " AND a.quanly_fk in (SELECT pk_seq FROM DanhSachNhanSu WHERE nhanvien_fk = '" + Session["userId"].ToString() + "') ";

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
                string trangthai = xl.GetStatus(language, "", dt.Rows[i]["trangthai"].ToString());

                noidung += "<tr>  " +
                "	<td>" + (i + 1).ToString() + "</td> " +
                //"    <td>" + dt.Rows[i]["ma"].ToString() + "</td> " +
                "    <td>" + dt.Rows[i]["ten"].ToString() + "</td> " +
                "    <td>" + dt.Rows[i]["quanly"].ToString() + "</td> " +
                "    <td>" + trangthai + "</td> " +
                "	<td> ";
                if (language.Equals("1"))
                {
                    if (quyen[ExecuteData.CapNhat].Equals("1"))
                        noidung += "	<i class='fa fa-fw fa-edit'></i> <a href='Administrator.aspx?func=97&action=capnhat&id=" + dt.Rows[i]["pk_seq"].ToString() + "' title='Edit' >Edit</a> &nbsp; ";

                    if (quyen[ExecuteData.Xoa].Equals("1"))
                        noidung += "	<i class='fa fa-fw fa-trash-o'></i> <a href='Administrator.aspx?func=97&action=delete&id=" + dt.Rows[i]["pk_seq"].ToString() + "' >Delete </a> &nbsp; ";
                }
                else if (language.Equals("2"))
                {
                    if (quyen[ExecuteData.CapNhat].Equals("1"))
                        noidung += "	<i class='fa fa-fw fa-edit'></i> <a href='Administrator.aspx?func=97&action=capnhat&id=" + dt.Rows[i]["pk_seq"].ToString() + "' title='Edit' >Sửa</a> &nbsp; ";

                    if (quyen[ExecuteData.Xoa].Equals("1"))
                        noidung += "	<i class='fa fa-fw fa-trash-o'></i> <a href='Administrator.aspx?func=97&action=delete&id=" + dt.Rows[i]["pk_seq"].ToString() + "' >Xóa </a> &nbsp; ";
                }
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
            "             Line " + tuDONG + "-" + denDONG + " of total  " + tongsoDONG + " line, page " + pageID + " of " + soTRANG.ToString() +
            "         </td>" +
            "     </tr>" +
            " </table>";

        }

        protected void lbTimkiem_Click(object sender, EventArgs e)
        {
            this.InitData(pageID, null);
        }

        protected void ddlNhanSu_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.InitData(pageID, null);
        }
    }
}