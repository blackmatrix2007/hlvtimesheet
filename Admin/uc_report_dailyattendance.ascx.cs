using Google.Api.Gax;
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
    public partial class uc_report_dailyattendance : System.Web.UI.UserControl
    {
        public string language = "1";
        public string tungay = "";
        public string denngay = "";
        public string phongban = "";

        public uc_report_dailyattendance()
        {
            
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
                if (Request.QueryString["todate"] != null)
                    tungay = Request.QueryString["todate"].ToString();
                if (Request.QueryString["frdate"] != null)
                    denngay = Request.QueryString["frdate"].ToString();
                if (Request.QueryString["de"] != null)
                    phongban = Request.QueryString["de"].ToString();
              
                ExecuteData xl = new ExecuteData();
                string query = "SELECT pk_seq, '[ ' + ma + ' ] ' + ten AS ten " +
                    " FROM PhongBan WHERE trangthai = 1 " +
                    " ORDER BY ma ";
                DataTable dt = xl.ReadTable(query);

                ddlPhongBan.Items.Add(new ListItem("All", "0"));                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlPhongBan.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));                    
                }

                txtTuNgay.Text = "01-" + DateTime.Now.ToString("MM") + "-" + DateTime.Now.ToString("yyyy");
                txtDenNgay.Text = DateTime.Now.ToString("dd-MM-yyyy");

                if (tungay.Length > 3)
                    txtTuNgay.Text = tungay;
                if (denngay.Length > 3)
                    txtDenNgay.Text = denngay;
                if (phongban.Length > 3)
                    ddlPhongBan.SelectedValue = phongban;

                //this.loadInfo(null);
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
            string title2 = "";
            string title3 = "";
            string content = "";
            string condition = "";
            string _backgroundTitle = "background-color:lightgray; ";
            string _colorDay = "";
            string sql = "";
            int count = 0;
            string today = DateTime.Now.ToString("dd-MM-yyyy");
            List<string> listDate = FormatString.returnListDay(txtTuNgay.Text, txtDenNgay.Text);

            title += " <tr>" +
                " <th style='text-align:center; width:50px; border:1px solid black; background-color:lightgray; padding:1px;' rowspan='3'>#</th>" +
                " <th style='text-align:center; width:60px; border:1px solid black; background-color:lightgray; padding:1px;' rowspan='3'>Depart</th>" +
                " <th style='text-align:center; width:50px; border:1px solid black; background-color:lightgray; padding:1px;' rowspan='3'>Code</th>" +
                " <th style='text-align:center; width:120px; border:1px solid black; background-color:lightgray; padding:1px;' rowspan='3'>Name</th>";

            foreach (string _ngaynhap in listDate)
            {
                int day_tungay = int.Parse(_ngaynhap.Substring(0, 2));
                int month_tungay = int.Parse(_ngaynhap.Substring(3, 2));
                int year_tungay = int.Parse(_ngaynhap.Substring(6, 4));
                string day_show = "";
                if (day_tungay == 1 || count == 0 || count == listDate.Count - 1)
                    day_show = day_tungay.ToString() + "/" + month_tungay.ToString();
                else
                    day_show = day_tungay.ToString();

                _colorDay = "";
                _backgroundTitle = "background-color:lightgray; ";

                DateTime date = new DateTime(year_tungay, month_tungay, day_tungay);
                if (date.DayOfWeek.ToString().Equals("Sunday"))
                    _colorDay = "color: red;";

                if (_ngaynhap.Equals(today))
                    _backgroundTitle = "background-color:lightgreen;";

                title += " <th style='text-align:center; width:75px; border:1px solid black; " + _backgroundTitle + _colorDay + " padding:1px;' colspan='5'>" + day_show + "</th>";
                title2 += " <th style='text-align:center; width:75px; border:1px solid black; " + _backgroundTitle + _colorDay + " padding:1px;' colspan='5'>" + date.DayOfWeek.ToString() + "</th>";

                title3 += " <th style='text-align:center; width:15px; border:1px solid black; background-color:moccasin; font-size:smaller; font-weight:normal; padding:1px;'>Result</th>";
                title3 += " <th style='text-align:center; width:15px; border:1px solid black; background-color:moccasin; font-size:smaller; font-weight:normal; padding:1px;'>Normal over</th>";
                title3 += " <th style='text-align:center; width:15px; border:1px solid black; background-color:paleTurquoise; font-size:smaller; font-weight:normal; padding:1px;'>Night</th>";
                title3 += " <th style='text-align:center; width:15px; border:1px solid black; background-color:paleTurquoise; font-size:smaller; font-weight:normal; padding:1px;'>Night over</th>";
                title3 += " <th style='text-align:center; width:15px; border:1px solid black; background-color:lightYellow; font-size:smaller; font-weight:normal; padding:1px;'>Holiday</th>";
            }
            title += " </tr>";
            title2 += " </tr>";
            title3 += " </tr>";

            title += title2;
            title += title3;

            //if (ddlPhongBan.SelectedValue.Trim().Length > 3)
            //    condition += " AND ct.sanpham_fk = '" + ddlSanPham.SelectedValue + "' ";
            //if (ddlNam.SelectedValue.Trim().Length > 0)
            //    condition += " AND a.nam = '" + ddlNam.SelectedValue + "' ";
            //if (ddlThang.SelectedValue.Trim().Length > 0)
            //    condition += " AND a.thang = '" + ddlThang.SelectedValue + "' ";
            //if (ddlPhienBan.SelectedValue.Trim().Length > 3)
            //    condition += " AND b.nangsuat_fk = '" + ddlPhienBan.SelectedValue + "' ";

            if (ddlPhongBan.SelectedValue.Trim().Length > 3)
            {                         
                condition += " AND (a.phongban_fk in (SELECT pk_seq FROM PhongBan WHERE nhomphong_fk in (SELECT nhomphong_fk FROM PhongBan WHERE pk_seq = " + ddlPhongBan.SelectedValue + ")) OR a.phongbanSupport_fk in (SELECT pk_seq FROM PhongBan WHERE nhomphong_fk in (SELECT nhomphong_fk FROM PhongBan WHERE pk_seq = " + ddlPhongBan.SelectedValue + "))) ";
            }

            sql = " SELECT a.pk_seq AS nhansu_fk, a.ma, a.ten, a.hinhanh, a.capbac, ISNULL((SELECT ma FROM PhongBan WHERE pk_seq = a.phongban_fk), '') phongban " +
                " FROM DanhSachNhanSu a " +
                " WHERE a.trangthai in (1) " + condition +
                " ORDER BY a.capbac, a.ma ";
            DataTable dt = xl.ReadTable(sql);
            for (int z = 0; z < dt.Rows.Count; z++)
            {
                string nhansu_fk = dt.Rows[z]["nhansu_fk"].ToString();

                content += "<tr>";
                content += " <td style='text-align:center; border:1px solid black; padding:1px; height:28px; font-size:smaller;'>" + (z + 1).ToString() + "</td>";
                content += " <td style='text-align:center; border:1px solid black; padding:1px; height:28px; font-size:smaller;'>" + dt.Rows[z]["phongban"].ToString() + "</td>";
                content += " <td style='text-align:center; border:1px solid black; padding:1px; height:28px; font-size:smaller;'>" + dt.Rows[z]["ma"].ToString() + "</td>";
                content += " <td style='text-align:left; border:1px solid black; padding:1px; height:28px; font-size:smaller;'>" + dt.Rows[z]["ten"].ToString() + "</td>";
                foreach (string _ngaynhap in listDate)
                {
                    double result = 0;
                    double normalOver = 0;
                    double night = 0;
                    double nightOver = 0;
                    double holiday = 0;

                    string resultShow = "";
                    string normalOverShow = "";
                    string nightShow = "";
                    string nightOverShow = "";
                    string holidayShow = "";

                    int gioStart = 0;
                    int gioEnd = 0;
                    int phutStart = 0;
                    int phutEnd = 0;

                    int gioIn = 0;
                    int gioOut = 0;
                    int phutIn = 0;
                    int phutOut = 0;

                    sql = " SELECT a.trangthai, b.nhansu_fk, b.thoigian, b.loai, b.gio, b.phut, b.gioStart, b.phutStart, b.gioEnd, b.phutEnd " +
                       " FROM ChamCong a INNER JOIN ChamCong_ChiTiet b ON a.pk_seq = b.chamcong_fk AND b.trangthai in (1, 2, 3, 4, 5) AND a.nhansu_fk = '" + nhansu_fk + "' AND a.ngaynhap = '" + _ngaynhap + "' " +
                       " ORDER BY loai, gio, phut ASC ";
                    DataTable dtK = xl.ReadTable(sql);
                    if(dtK.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtK.Rows.Count; i++)
                        {
                            if (i == 0)
                            {
                                gioStart = int.Parse(dtK.Rows[i]["gioStart"].ToString());
                                phutStart = int.Parse(dtK.Rows[i]["phutStart"].ToString());

                                gioIn = int.Parse(dtK.Rows[i]["gio"].ToString());
                                phutIn = int.Parse(dtK.Rows[i]["phut"].ToString());
                            }
                            gioEnd = int.Parse(dtK.Rows[i]["gioEnd"].ToString());
                            phutEnd = int.Parse(dtK.Rows[i]["phutEnd"].ToString());

                            gioOut = int.Parse(dtK.Rows[i]["gio"].ToString());
                            phutOut = int.Parse(dtK.Rows[i]["phut"].ToString());
                        }                                
                    }

                    double valueIn = gioStart * 60 + phutStart;
                    double valueOut = gioEnd * 60 + phutEnd;

                    double actualIn = gioIn * 60 + phutIn;
                    double actualOut = gioOut * 60 + phutOut;

                    double chenhlech = actualOut - actualIn;
                    if (chenhlech > 580)
                    {
                        resultShow = "8";
                        normalOverShow = Math.Round((chenhlech - 60 - 480) / 60, 2).ToString();
                    }
                    else if (chenhlech > 240)
                    {
                        resultShow = Math.Round((chenhlech - 60) / 60, 2).ToString();
                    }
                    else if(chenhlech != 0)
                    {
                        resultShow = Math.Round(chenhlech / 60, 2).ToString();
                    }   
                    

                    if (result > 0)
                        resultShow = result.ToString();
                    if (normalOver > 0)
                        normalOverShow = normalOver.ToString();
                    if (night > 0)
                        nightShow = night.ToString();
                    if (nightOver > 0)
                        nightOverShow = nightOver.ToString();
                    if (holiday > 0)
                        holidayShow = holiday.ToString();

                    content += " <td style='text-align:center; border:1px solid black; padding:1px; height:28px; font-size:smaller;'>" + resultShow + "</td>";
                    content += " <td style='text-align:center; border:1px solid black; padding:1px; height:28px; font-size:smaller;'>" + normalOverShow + "</td>";
                    content += " <td style='text-align:center; border:1px solid black; padding:1px; height:28px; font-size:smaller;'>" + nightShow + "</td>";
                    content += " <td style='text-align:center; border:1px solid black; padding:1px; height:28px; font-size:smaller;'>" + nightOverShow + "</td>";
                    content += " <td style='text-align:center; border:1px solid black; padding:1px; height:28px; font-size:smaller;'>" + holidayShow + "</td>";
                }
            }

            ltInfor.Text += title;
            ltInfor.Text += content;
            ltInfor.Text += title;
        }

        protected void ddlPhongBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.loadInfo(null);
        }
        protected void txtTuNgay_TextChanged(object sender, EventArgs e)
        {
            this.loadInfo(null);

        }

        protected void txtDenNgay_TextChanged(object sender, EventArgs e)
        {
            this.loadInfo(null);
        }
    }
}