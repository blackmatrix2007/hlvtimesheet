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
    public partial class uc_calendar_setup_action : System.Web.UI.UserControl
    {
        public string language = "1";
        public string typeNow = "1";

        public string OffThang01 = "0";
        public string OffThang02 = "0";
        public string OffThang03 = "0";
        public string OffThang04 = "0";
        public string OffThang05 = "0";
        public string OffThang06 = "0";
        public string OffThang07 = "0";
        public string OffThang08 = "0";
        public string OffThang09 = "0";
        public string OffThang10 = "0";
        public string OffThang11 = "0";
        public string OffThang12 = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            ExecuteData xl = new ExecuteData();

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

            string ngaynhap = "";
            if (Request.QueryString["date"] != null)
                ngaynhap = Request.QueryString["date"].ToString();

            string phongban_fk = "";
            if (Request.QueryString["depa"] != null)
                phongban_fk = Request.QueryString["depa"].ToString();

            if (ngaynhap.Length > 3)
                txtNgayNhap.Text = ngaynhap;
            else
                txtNgayNhap.Text = DateTime.Now.ToString("dd-MM-yyyy");

            if (!IsPostBack)
            {
                string query = "SELECT pk_seq, '[' + ma + '] ' + ten AS ten FROM PhongBan WHERE trangthai in (1) ORDER BY ma ";
                DataTable dt = xl.ReadTable(query);

                ddlPhongBan.Items.Add(new ListItem("", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlPhongBan.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                query = "SELECT pk_seq, ma, ten FROM LoaiNgayNghi WHERE trangthai = 1 ORDER BY pk_seq  ";
                dt = xl.ReadTable(query);

                ddlLoaiNgayNghi.Items.Add(new ListItem("", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlLoaiNgayNghi.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                // 
                for (int i = 0; i < 24; i++)
                {
                    string str = i.ToString();
                    if (i < 10)
                        str = "0" + i.ToString();

                    ddlGioStart.Items.Add(new ListItem(str, str));
                    ddlGioEnd.Items.Add(new ListItem(str, str));
                    ddlBreakOneHourStart.Items.Add(new ListItem(str, str));
                    ddlBreakOneHourEnd.Items.Add(new ListItem(str, str));
                    ddlLunchBreakHourStart.Items.Add(new ListItem(str, str));
                    ddlLunchBreakHourEnd.Items.Add(new ListItem(str, str));
                    ddlBreakTwoHourStart.Items.Add(new ListItem(str, str));
                    ddlBreakTwoHourEnd.Items.Add(new ListItem(str, str));
                    ddlOverTimeHourStart.Items.Add(new ListItem(str, str));
                    ddlOverTimeHourEnd.Items.Add(new ListItem(str, str));                    
                }

                for (int i = 0; i < 60; i++)
                {
                    string str = i.ToString();
                    if (i < 10)
                        str = "0" + i.ToString();

                    ddlPhutStart.Items.Add(new ListItem(str, str));
                    ddlPhutEnd.Items.Add(new ListItem(str, str));
                    ddlBreakOneMinuteStart.Items.Add(new ListItem(str, str));
                    ddlBreakOneMinuteEnd.Items.Add(new ListItem(str, str));                   
                    ddlLunchBreakMinuteStart.Items.Add(new ListItem(str, str));
                    ddlLunchBreakMinuteEnd.Items.Add(new ListItem(str, str));
                    ddlBreakTwoMinuteStart.Items.Add(new ListItem(str, str));
                    ddlBreakTwoMinuteEnd.Items.Add(new ListItem(str, str));
                    ddlOverTimeMinuteStart.Items.Add(new ListItem(str, str));
                    ddlOverTimeMinuteEnd.Items.Add(new ListItem(str, str));
                }

                ddlYear.Items.Clear();
                ddlYear.Items.Add(new ListItem("2026", "2026"));
                ddlYear.Items.Add(new ListItem("2027", "2027"));
                ddlYear.Items.Add(new ListItem("2028", "2028"));

                if (phongban_fk.Length > 0)
                    ddlPhongBan.SelectedValue = phongban_fk;

                this.loadInfoYear();
                this.loadCountDateInYear(xl);
            }
        }

        private void loadCountDateInYear(ExecuteData xl)
        {
            if (xl == null)
                xl = new ExecuteData();

            lblSoNgayChuNhat.Text = "";
            lbltSoNgayNghiLe.Text = "";
            lblSoNgayNghiCongTy.Text = "";
            lblSoNgayNghiKhac.Text = "";

            string tungay = "01-01-" + ddlYear.SelectedValue;
            string denngay = "31-12-" + ddlYear.SelectedValue;

            string sql = "SELECT pk_seq, ten, mausac FROM LoaiNgayNghi " +
                " WHERE trangthai = 1 ORDER BY pk_seq ";
            DataTable dt = xl.ReadTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string soNgayNghi = "0";
                string loaingaynghi_fk = dt.Rows[i]["pk_seq"].ToString();
                sql = "SELECT COUNT(*) AS soluong FROM NgayNghiLe_PhongBan WHERE phongban_fk = '" + ddlPhongBan.SelectedValue + "' AND CONVERT(datetime, ngaynghi, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND CONVERT(datetime, denngay, 105) <= CONVERT(datetime, '" + denngay + "', 105) AND loaingaynghi_fk = '" + loaingaynghi_fk + "' AND trangthai = 1 ";
                object obj = xl.ExecuteScalarSQL(sql);
                if (obj != null)
                {
                    soNgayNghi = obj.ToString();
                }

                switch (loaingaynghi_fk)
                {
                    case "100001":
                        lblSoNgayChuNhat.Text = soNgayNghi;
                        break;
                    case "100002":
                        sql = "SELECT COUNT(*) AS soluong FROM NgayNghiLe_PhongBan WHERE phongban_fk = '0' AND loaingaynghi_fk = '" + loaingaynghi_fk + "' AND trangthai = 1 AND hangnam = 1 ";
                        obj = xl.ExecuteScalarSQL(sql);
                        if (obj != null)
                        {
                            soNgayNghi = (int.Parse(soNgayNghi) + int.Parse(obj.ToString())).ToString();
                        }
                        lbltSoNgayNghiLe.Text = soNgayNghi;
                        break;
                    case "100003":
                        lblSoNgayNghiCongTy.Text = soNgayNghi;
                        break;
                    case "100004":
                        lblSoNgayNghiKhac.Text = soNgayNghi;
                        break;
                    case "100005":
                        break;
                    case "100006":
                        break;
                    case "100007":
                        break;
                    case "100008":
                        break;
                    default:
                        break;
                }
            }

            for (int i = 1; i <= 12; i++)
            {
                string thang = i.ToString();
                if (i < 10)
                    thang = "0" + i.ToString();
                tungay = "01-" + thang + "-" + ddlYear.SelectedValue;
                sql = "SELECT CONVERT(nvarchar(10), (DATEADD(DAY,-1, DATEADD(MM, DATEDIFF(MM, 0, CONVERT(datetime, '" + tungay + "', 105)) + 1, 0))), 105)";
                denngay = xl.ExecuteScalarSQL(sql).ToString();

                sql = " SELECT ISNULL(SUM(soluong), 0) soluong " +
                    " FROM " +
                    " ( " +
                    "	SELECT COUNT(*) AS soluong FROM NgayNghiLe_PhongBan WHERE phongban_fk = '" + ddlPhongBan.SelectedValue + "' AND hangnam = 1 AND thang = 1 AND loaingaynghi_fk = 100002 AND trangthai = 1 " +
                    "	UNION ALL " +
                    "	SELECT COUNT(*) AS soluong FROM NgayNghiLe_PhongBan WHERE phongban_fk = '" + ddlPhongBan.SelectedValue + "' AND hangnam = 0 AND trangthai = 1 AND CONVERT(datetime, ngaynghi, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND CONVERT(datetime, denngay, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                    " ) A ";
                object obj = xl.ExecuteScalarSQL(sql);
                if (obj != null)
                {
                    switch (i)
                    {
                        case 1:
                            OffThang01 = obj.ToString();
                            break;
                        case 2:
                            OffThang02 = obj.ToString();
                            break;
                        case 3:
                            OffThang03 = obj.ToString();
                            break;
                        case 4:
                            OffThang04 = obj.ToString();
                            break;
                        case 5:
                            OffThang05 = obj.ToString();
                            break;
                        case 6:
                            OffThang06 = obj.ToString();
                            break;
                        case 7:
                            OffThang07 = obj.ToString();
                            break;
                        case 8:
                            OffThang08 = obj.ToString();
                            break;
                        case 9:
                            OffThang09 = obj.ToString();
                            break;
                        case 10:
                            OffThang10 = obj.ToString();
                            break;
                        case 11:
                            OffThang11 = obj.ToString();
                            break;
                        case 12:
                            OffThang12 = obj.ToString();
                            break;
                        default:
                            break;
                    }
                }
            }

            sql = "SELECT pk_seq, ten, gioStart, phutStart, gioEnd, phutEnd, loai " + 
                " FROM GioLamViec " + 
                " WHERE trangthai in (1) AND phongban_fk = '" + ddlPhongBan.SelectedValue + "' " + 
                " ORDER BY loai, gioStart, phutStart ASC ";
            dt = xl.ReadTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string loai = dt.Rows[i]["loai"].ToString();
                string gioStart = dt.Rows[i]["gioStart"].ToString();
                string phutStart = dt.Rows[i]["phutStart"].ToString();
                string gioEnd = dt.Rows[i]["gioEnd"].ToString();
                string phutEnd = dt.Rows[i]["phutEnd"].ToString();


                if (gioStart.Length < 2)
                    gioStart = "0" + gioStart;
                if (phutStart.Length < 2)
                    phutStart = "0" + phutStart;
                if (gioEnd.Length < 2)
                    gioEnd = "0" + gioEnd;
                if (phutEnd.Length < 2)
                    phutEnd = "0" + phutEnd;

                switch (loai)
                {
                    case "1":
                        ddlGioStart.SelectedValue = gioStart;
                        ddlPhutStart.SelectedValue = phutStart;
                        ddlGioEnd.SelectedValue = gioEnd;
                        ddlPhutEnd.SelectedValue = phutEnd;
                        break;
                    case "2":
                        ddlBreakOneHourStart.SelectedValue = gioStart;
                        ddlBreakOneMinuteStart.SelectedValue = phutStart;
                        ddlBreakOneHourEnd.SelectedValue = gioEnd;
                        ddlBreakOneMinuteEnd.SelectedValue = phutEnd;
                        break;
                    case "3":
                        ddlLunchBreakHourStart.SelectedValue = gioStart;
                        ddlLunchBreakMinuteStart.SelectedValue = phutStart;
                        ddlLunchBreakHourEnd.SelectedValue = gioEnd;
                        ddlLunchBreakMinuteEnd.SelectedValue = phutEnd;
                        break;
                    case "4":
                        ddlBreakTwoHourStart.SelectedValue = gioStart;
                        ddlBreakTwoMinuteStart.SelectedValue = phutStart;
                        ddlBreakTwoHourEnd.SelectedValue = gioEnd;
                        ddlBreakTwoMinuteEnd.SelectedValue = phutEnd;
                        break;
                    case "5":
                        ddlOverTimeHourStart.SelectedValue = gioStart;
                        ddlOverTimeMinuteStart.SelectedValue = phutStart;
                        ddlOverTimeHourEnd.SelectedValue = gioEnd;
                        ddlOverTimeMinuteEnd.SelectedValue = phutEnd;
                        break;
                    default:
                        break;
                }    
            }
        }

        private void loadInfoYear()
        {
            ltYear.Text = "";
            string content = "";
            content += "<tr>" +
            " <td style='border:1px solid black; padding:1px; height:28px;'><input type='text' class='form-control' style='font-size:small; padding: 0px; height:28px; text-align:right;' name='totalThang01' value='' readonly></td> " +
            " <td style='border:1px solid black; padding:1px; height:28px;'><input type='text' class='form-control' style='font-size:small; padding: 0px; height:28px; text-align:right;' name='totalThang02' value='' readonly></td> " +
            " <td style='border:1px solid black; padding:1px; height:28px;'><input type='text' class='form-control' style='font-size:small; padding: 0px; height:28px; text-align:right;' name='totalThang03' value='' readonly></td> " +
            " <td style='border:1px solid black; padding:1px; height:28px;'><input type='text' class='form-control' style='font-size:small; padding: 0px; height:28px; text-align:right;' name='totalThang04' value='' readonly></td> " +
            " <td style='border:1px solid black; padding:1px; height:28px;'><input type='text' class='form-control' style='font-size:small; padding: 0px; height:28px; text-align:right;' name='totalThang05' value='' readonly></td> " +
            " <td style='border:1px solid black; padding:1px; height:28px;'><input type='text' class='form-control' style='font-size:small; padding: 0px; height:28px; text-align:right;' name='totalThang06' value='' readonly></td> " +
            " <td style='border:1px solid black; padding:1px; height:28px;'><input type='text' class='form-control' style='font-size:small; padding: 0px; height:28px; text-align:right;' name='totalThang07' value='' readonly></td> " +
            " <td style='border:1px solid black; padding:1px; height:28px;'><input type='text' class='form-control' style='font-size:small; padding: 0px; height:28px; text-align:right;' name='totalThang08' value='' readonly></td> " +
            " <td style='border:1px solid black; padding:1px; height:28px;'><input type='text' class='form-control' style='font-size:small; padding: 0px; height:28px; text-align:right;' name='totalThang09' value='' readonly></td> " +
            " <td style='border:1px solid black; padding:1px; height:28px;'><input type='text' class='form-control' style='font-size:small; padding: 0px; height:28px; text-align:right;' name='totalThang10' value='' readonly></td> " +
            " <td style='border:1px solid black; padding:1px; height:28px;'><input type='text' class='form-control' style='font-size:small; padding: 0px; height:28px; text-align:right;' name='totalThang11' value='' readonly></td> " +
            " <td style='border:1px solid black; padding:1px; height:28px;'><input type='text' class='form-control' style='font-size:small; padding: 0px; height:28px; text-align:right;' name='totalThang12' value='' readonly></td> " +
            " <td style='border:1px solid black; padding:1px; height:28px;'><input type='text' class='form-control' style='font-size:small; padding: 0px; height:28px; text-align:right;' name='totalYear' value='' readonly></td> " +
            " <td style='border:1px solid black; padding:1px; height:28px;'><input type='text' class='form-control' style='font-size:small; padding: 0px; height:28px; text-align:right;' name='hourDay' value='8' readonly></td> " +
            " <td style='border:1px solid black; padding:1px; height:28px;'><input type='text' class='form-control' style='font-size:small; padding: 0px; height:28px; text-align:right;' name='totalHourWork' value='' readonly></td> ";

           ltYear.Text = content;
        }

        protected void ddlPhongBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.loadInfoYear();
            this.loadCountDateInYear(null);
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void lbLuuLai_Click(object sender, EventArgs e)
        {           
            string msg = "";
            CalendarControlModel calendarObj = new CalendarControlModel();

            string gioStart = "";
            string phutStart = "";
            string gioEnd = "";
            string phutEnd = "";

            gioStart = ddlGioStart.SelectedValue + ";" + ddlBreakOneHourStart.SelectedValue + ";" + ddlLunchBreakHourStart.SelectedValue + ";" + ddlBreakTwoHourStart.SelectedValue + ";" + ddlOverTimeHourStart.SelectedValue;
            phutStart = ddlPhutStart.SelectedValue + ";" + ddlBreakOneMinuteStart.SelectedValue + ";" + ddlLunchBreakMinuteStart.SelectedValue + ";" + ddlBreakTwoMinuteStart.SelectedValue + ";" + ddlOverTimeMinuteStart.SelectedValue;
            gioEnd = ddlGioEnd.SelectedValue + ";" + ddlBreakOneHourEnd.SelectedValue + ";" + ddlLunchBreakHourEnd.SelectedValue + ";" + ddlBreakTwoHourEnd.SelectedValue + ";" + ddlOverTimeHourEnd.SelectedValue;
            phutEnd = ddlPhutEnd.SelectedValue + ";" + ddlBreakOneMinuteEnd.SelectedValue + ";" + ddlLunchBreakMinuteEnd.SelectedValue + ";" + ddlBreakTwoMinuteEnd.SelectedValue + ";" + ddlOverTimeMinuteEnd.SelectedValue;

            msg = calendarObj.INSERT_WorkingHourse(ddlPhongBan.SelectedValue, gioStart, phutStart, gioEnd, phutEnd, "1", Session["userId"].ToString());

            if (msg.Trim().Length <= 10)
            {
                lblError.Text = "";
                Response.Redirect("Calendar.aspx");
            }
            else
            {
                lblError.Text = msg;
            }
        }       
    }
}