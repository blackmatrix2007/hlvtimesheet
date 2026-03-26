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
    public partial class uc_masterdata_worktype : System.Web.UI.UserControl
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

                ExecuteData xl = new ExecuteData();
                
                string query = "SELECT pk_seq, ten FROM NhomSanPham WHERE trangthai = '1' ORDER BY ten ASC ";
                DataTable dt = xl.ReadTable(query);

                ddlNhomSanPham.Items.Add(new ListItem("Select group", "0"));
                ddlAddGroup.Items.Add(new ListItem("Select group", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlNhomSanPham.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                    ddlAddGroup.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                query = "SELECT pk_seq, ten, ISNULL((SELECT COUNT(*) FROM BangMaMau_NhomSanPham WHERE bangmamau_fk = a.pk_seq), 0) soluong FROM BangMaMau a WHERE a.trangthai = 1 ORDER BY soluong, ten ASC ";
                dt = xl.ReadTable(query);

                ddlBangmaMau.Items.Add(new ListItem("Select color", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlBangmaMau.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                ddlTrangThai.Items.Add(new ListItem("On", "1"));
                ddlTrangThai.Items.Add(new ListItem("Off", "0"));
                ddlTrangThai.Items.Add(new ListItem("All", ""));

                this.loadCodeGroupItem(xl);

                this.InitData(pageID, xl);
            }
        }

        private void InitData(int pageID, ExecuteData xl)
        {
            if (xl == null)
                xl = new ExecuteData();

            quyen = xl.getRole(Session["userId"].ToString(), Request.QueryString["func"].ToString());

         
            //Lưu lại các thông tin đang search khi phân trang
            string search = "";
            if (Request.QueryString["search"] != null)
                search = Request.QueryString["search"].ToString();
            if (search.Trim().Length > 0 && search.Contains(";;"))
            {
                txtSanPham.Text = xl.GetValue(search, 0);
                ddlNhomSanPham.SelectedValue = xl.GetValue(search, 1);
                ddlTrangThai.SelectedValue = xl.GetValue(search, 2);
            }

            string query = " SELECT ROW_NUMBER() OVER(ORDER BY n.stt, a.ma ASC) AS stt, a.pk_seq, a.ma AS masp, a.ten, a.nameEnglish AS shortName, a.cycletarget, a.trangthai, a.cycletime, a.giaban, n.ma AS nhomsanpham, a.nhomsanpham_fk, n.stt AS sothutu, a.showCustomer, " +
                "   ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = a.dvt_fk), '') donvi, " +
                "   ISNULL((SELECT ten FROM BangMaMau WHERE pk_seq = n.bangmamau_fk), '') mausac, " +
                "   CONVERT(nvarchar(10), a.ngaysua, 105) + ' ' + CONVERT(CHAR(8), a.ngaysua, 14) as ngaysua, c.ten as nguoisua " +
                " FROM SanPham a INNER JOIN NhanVien c on a.nguoisua = c.pk_seq " +    
                " LEFT JOIN NhomSanPham n ON a.nhomsanpham_fk = n.pk_seq " + 
                " WHERE a.pk_seq > 0 ";

            if (txtSanPham.Text.Trim().Length > 0)
                query += " and (a.timkiem like N'%" + xl.Change_AV(txtSanPham.Text.Trim()) + "%')";
            if (ddlNhomSanPham.SelectedValue.Trim().Length > 3)
                query += " and a.nhomsanpham_fk = N'" + ddlNhomSanPham.SelectedValue + "'  ";
            if (ddlTrangThai.SelectedValue.Trim().Length > 0)
                query += " and a.trangthai = N'" + ddlTrangThai.SelectedValue + "'  ";

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
                string styleColor = "";
                string showCusChecked = "";
                string showCustomer = dt.Rows[i]["showCustomer"].ToString();
                string trangthai = xl.GetStatus("1", "", dt.Rows[i]["trangthai"].ToString());
                string mausac = dt.Rows[i]["mausac"].ToString();
                if(mausac.Length > 1)
                {
                    styleColor = "background-color: " + mausac + ";";
                }


                if (showCustomer.Equals("1"))
                    showCusChecked = "checked";

                noidung += "<tr>  " +
                "    <td style='display:none;'><input type='text' class='form-control' name='sanpham_fk' value='" + dt.Rows[i]["pk_seq"].ToString() + "' />  </td> " +
                "	 <td style='" + styleColor + "'>" + (i + 1).ToString() + "</td> " +
                "    <td style='" + styleColor + "'>" + dt.Rows[i]["nhomsanpham"].ToString() + "</td> " +
                "    <td style='" + styleColor + "'>" + dt.Rows[i]["shortName"].ToString() + "</td> " +
                "    <td style='" + styleColor + "'>" + dt.Rows[i]["ten"].ToString() + "</td> " +
                "    <td style='" + styleColor + "'>" + dt.Rows[i]["masp"].ToString() + "</td> " +
                "    <td style='" + styleColor + "'>" + dt.Rows[i]["donvi"].ToString() + "</td> " +
                "    <td style='text-align:right;'>" + dt.Rows[i]["cycletime"].ToString() + "</td> " +
                "    <td style='text-align:right;'>" + dt.Rows[i]["cycletarget"].ToString() + "</td> " +
                "    <td style='text-align:right;'>" + FormatString.ForMatNumber(dt.Rows[i]["giaban"].ToString()) + "</td> " +
                "    <td style='text-align:right;'><label class='switch' ><input type='checkbox' name='ckCustom' onchange='saveChangeCuctomer(" + i + ")' " + showCusChecked + "><span class='slider round'></span></label></td> " +
                "	<td> ";
                if (quyen[ExecuteData.CapNhat].Equals("1"))
                    noidung += " <i class='fa fa-fw fa-edit'></i> <a href='MasterData.aspx?func=5&action=capnhat&id=" + dt.Rows[i]["pk_seq"].ToString() + "' title='Edit' >Edit</a> &nbsp; ";

                //noidung += " <i class='fa fa-plus-square'></i> <a href='javascript:void(0);' data-toggle='modal' data-target='#exampleModalAddGroup' data-whatever='" + dt.Rows[i]["pk_seq"].ToString() + "' >Add group </a> &nbsp; ";

                noidung += " <i class='fa fa-fw fa-edit'></i> <a href='MasterData.aspx?func=5&action=taomoi&groupItem=" + dt.Rows[i]["nhomsanpham_fk"].ToString() + "' title='Edit' >Add item</a> &nbsp; ";

                if (quyen[ExecuteData.Xoa].Equals("1"))
                    noidung += " <i class='fa fa-fw fa-trash-o' style='color:red;'></i> <a style='color:red;' href='MasterData.aspx?func=5&action=delete&id=" + dt.Rows[i]["pk_seq"].ToString() + "' title='Delete' >Delete </a> &nbsp; ";

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

        private void loadCodeGroupItem(ExecuteData xl)
        {
            if (xl == null)
                xl = new ExecuteData();

            string ma = "1";

            string query = "SELECT TOP(1) pk_seq, ma, ten " + 
                " FROM NhomSanPham ORDER BY pk_seq DESC ";
            DataTable dt = xl.ReadTable(query);
            if(dt.Rows.Count > 0)
            {
                ma = dt.Rows[0]["ma"].ToString();
                if (ma.Length > 0)
                    ma = (int.Parse(ma) + 1).ToString();                
            }

            txtMa.Value = ma;
            txtTen.Value = ma;
        }
    }
}