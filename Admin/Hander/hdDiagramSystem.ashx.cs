using HLVTimeSheet.AcsessData;
using HLVTimeSheet.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace HLVTimeSheet.Admin.Hander
{
    /// <summary>
    /// Summary description for hdDiagramSystem
    /// </summary>
    public class hdDiagramSystem : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string action = context.Request.QueryString["action"];
            if (action == null)
                action = "";

            if (action.Equals("layoutMonthTimeSheet"))
            {
                LayoutMonthTimeSheetFunction(context);
            }
            else if (action.Equals("monthlyDivDetail"))
            {
                MonthlyDivDetail(context);
            }
        }

        private void LayoutMonthTimeSheetFunction(HttpContext context)
        {
            ExecuteData xl = new ExecuteData();

            string language = "1";
            if (context.Request.QueryString["language"] != null)
                language = context.Request.QueryString["language"].ToString();

            int _month = int.Parse(DateTime.Now.ToString("MM"));
            int _year = int.Parse(DateTime.Now.ToString("yyyy"));
            int days = DateTime.DaysInMonth(_year, _month);
            
            string sql = "";

            string content = "";
            string _styleColor = "";
            int count = 0;
            //bool flag = false;

            string dayNow = DateTime.Now.ToString("dd");

            string styleGray = "background-color:lightgray;";
            string styleBackgroundNow = "background-color:Linen; ";

            content += " <table class='table table-hover table-bordered table-striped' style='font-size:smaller;'> ";
            content += " <tr>";
            content += " <th style='text-align:center; border:0.5px solid black; padding-top:0.5px; padding-bottom: 0.5px; " + styleGray + "' rowspan='2' colspan='2'>Location</th>";

            for (int i = 1; i <= days; i++)
            {
                string t = "";
                if (i < 10)
                    t = "0" + i.ToString();
                else
                    t = i.ToString();

                // thứ hiện tại
                DateTime date = new DateTime(_year, _month, i);
                int thutrongtuan = this.returnDayofWeek(date.DayOfWeek.ToString()); // 0 là thứ 2, 6 là chủ nhật
                _styleColor = "";
                if (thutrongtuan == 6)
                    _styleColor = " color:red; ";

                if (t.Equals(dayNow))
                    content += " <th style='text-align:center; width:2.6%; border:0.5px solid black; padding-top:0.5px; padding-bottom: 0.5px;  " + styleBackgroundNow + _styleColor + " '>" + t + "</th>";
                else
                    content += " <th style='text-align:center; width:2.6%; border:0.5px solid black; padding-top:0.5px; padding-bottom: 0.5px;  " + styleGray + _styleColor + " '>" + t + "</th>";
            }
            content += " <th style='text-align:center; border:0.5px solid black; padding-top:0.5px; padding-bottom: 0.5px; " + styleGray + "' rowspan='2'>Total</th>";
            content += " </tr>";

            //////////////////////////////
            content += " <tr>";
            for (int i = 1; i <= days; i++)
            {
                string t = "";
                if (i < 10)
                    t = "0" + i.ToString();
                else
                    t = i.ToString();

                // thứ hiện tại
                DateTime date = new DateTime(_year, _month, i);
                int thutrongtuan = this.returnDayofWeek(date.DayOfWeek.ToString()); // 0 là thứ 2, 6 là chủ nhật

                _styleColor = "";
                if (thutrongtuan == 6)
                    _styleColor = " color:red; ";

                if (t.Equals(dayNow))
                    content += " <th style='text-align:center; width:2.6%; border:0.5px solid black; padding-top:0.5px; padding-bottom: 0.5px;  " + styleBackgroundNow + _styleColor + " '>" + this.returnTitleWeeky(language, thutrongtuan) + "</th>";
                else
                    content += " <th style='text-align:center; width:2.6%; border:0.5px solid black; padding-top:0.5px; padding-bottom: 0.5px;  " + styleGray + _styleColor + " '>" + this.returnTitleWeeky(language, thutrongtuan) + "</th>";
            }
            content += " </tr>";

            //
            sql = " SELECT a.pk_seq AS phongban_fk, a.ma, a.ten " +
            " FROM PhongBan a INNER JOIN NhaPhanPhoi b ON a.chinhanh_fk = b.pk_seq" +
            " WHERE a.trangthai in (1)" +
            " ORDER BY b.stt, a.ma ";
            DataTable dt = xl.ReadTable(sql);
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                string sqlOn = "";
                string sqlHoliday = "";
                string sqlSuppport = "";

                double totalOn = 0;
                double totalHoliday = 0;
                double totalSupport = 0;

                for (int j = 1; j <= days; j++)
                    {
                        string t = "";
                        if (j < 10)
                            t = "0" + j.ToString();
                        else
                            t = j.ToString();

                        string _ngaynhap = t + "-" + _month + "-" + _year;

                        sqlOn += " SELECT * " +
                        " FROM " +
                        " ( " +
                        "	SELECT '1' AS STT, 'On' AS textStatus, '" + _ngaynhap + "' ngaynhap, COUNT(*) AS soluong FROM DanhSachNhanSuLichSu a INNER JOIN DanhSachNhanSuLichSu_ChiTiet b ON a.pk_seq = b.danhsach_fk AND b.phongban_fk = '" + dt.Rows[i]["phongban_fk"].ToString() + "' AND CONVERT(datetime, a.ngaynhap, 105) = CONVERT(datetime, '" + _ngaynhap + "', 105) AND b.hientrang = 1 " +
                        " ) A ";
                        if (j < days)
                            sqlOn += " UNION ALL ";

                        sqlHoliday += " SELECT * " +
                        " FROM " +
                        " ( " +
                        "	SELECT '2' AS STT, 'Holiday' AS textStatus, '" + _ngaynhap + "' ngaynhap, COUNT(*) AS soluong FROM DanhSachNhanSuLichSu a INNER JOIN DanhSachNhanSuLichSu_ChiTiet b ON a.pk_seq = b.danhsach_fk AND b.phongban_fk = '" + dt.Rows[i]["phongban_fk"].ToString() + "' AND CONVERT(datetime, a.ngaynhap, 105) = CONVERT(datetime, '" + _ngaynhap + "', 105) AND b.hientrang = 2 " +
                        " ) A ";
                        if (j < days)
                            sqlHoliday += " UNION ALL ";

                        sqlSuppport += " SELECT * " +
                        " FROM " +
                        " ( " +
                        "	SELECT '3' AS STT, 'Support' AS textStatus, '" + _ngaynhap + "' ngaynhap, COUNT(*) AS soluong FROM DanhSachNhanSuLichSu a INNER JOIN DanhSachNhanSuLichSu_ChiTiet b ON a.pk_seq = b.danhsach_fk AND b.phongban_fk = '" + dt.Rows[i]["phongban_fk"].ToString() + "' AND CONVERT(datetime, a.ngaynhap, 105) = CONVERT(datetime, '" + _ngaynhap + "', 105) AND b.hientrang = 3 " +
                        " ) A ";
                        if (j < days)
                            sqlSuppport += " UNION ALL ";
                    }

                if (sqlOn.Length > 10)
                    sqlOn = " SELECT B.* FROM (" + sqlOn + ") B ORDER BY CONVERT(datetime, B.ngaynhap, 105), B.STT ";

                if (sqlHoliday.Length > 10)
                    sqlHoliday = " SELECT B.* FROM (" + sqlHoliday + ") B ORDER BY CONVERT(datetime, B.ngaynhap, 105), B.STT ";

                if (sqlSuppport.Length > 10)
                    sqlSuppport = " SELECT B.* FROM (" + sqlSuppport + ") B ORDER BY CONVERT(datetime, B.ngaynhap, 105), B.STT ";

                // On
                DataTable dtOn = xl.ReadTable(sqlOn);
                DataTable dtHoliday = xl.ReadTable(sqlHoliday);
                DataTable dtSupport = xl.ReadTable(sqlSuppport);


                if (count % 2 == 0)
                    styleGray = "";
                else
                    styleGray = "background-color:lightgray;";
                count++;

                content += " <tr>";
                content += " <th style='text-align:center; width:2.6%; border:0.5px solid black; padding-top:0.5px; padding-bottom: 0.5px; font-weight: normal; " + styleGray + "' rowspan='3'>" + dt.Rows[i]["ma"].ToString() + "</th>";
                content += " <th style='text-align:center; width:2.6%; border:0.5px solid black; padding-top:0.5px; padding-bottom: 0.5px; font-weight: normal; " + styleGray + " '>MP</th>";
                for (int z = 0; z < dtOn.Rows.Count; z++)
                {
                    string t = "";
                    if (z < 9)
                        t = "0" + (z + 1).ToString();
                    else
                        t = (z + 1).ToString();

                    totalOn += double.Parse(dtOn.Rows[z]["soluong"].ToString());
                    string soluong = FormatString.ForMatNumber_New(dtOn.Rows[z]["soluong"].ToString());

                    if (t.Equals(dayNow))
                        content += " <th style='text-align:center; width:2.6%; border:0.5px solid black; padding-top:0.5px; padding-bottom: 0.5px; font-weight: normal; " + styleBackgroundNow + " '>" + soluong + "</th>";
                    else
                        content += " <th style='text-align:center; width:2.6%; border:0.5px solid black; padding-top:0.5px; padding-bottom: 0.5px; font-weight: normal; " + styleGray + " '>" + soluong + "</th>";
                }
                content += " <th style='text-align:center; width:2.6%; border:0.5px solid black; padding-top:0.5px; padding-bottom: 0.5px; font-weight: normal; " + styleGray + " '>" + FormatString.ForMatNumber_New(totalOn.ToString()) + "</th>";
                content += " </tr>";

                if (count % 2 == 0)
                    styleGray = "";
                else
                    styleGray = "background-color:lightgray;";
                count++;

                content += " <tr>";                
                content += " <th style='text-align:center; width:2.6%; border:0.5px solid black; padding-top:0.5px; padding-bottom: 0.5px; font-weight: normal; " + styleGray + " '>S.P</th>";
                for (int z = 0; z < dtSupport.Rows.Count; z++)
                {
                    string t = "";
                    if (z < 9)
                        t = "0" + (z + 1).ToString();
                    else
                        t = (z + 1).ToString();

                    totalSupport += double.Parse(dtSupport.Rows[z]["soluong"].ToString());
                    string soluong = FormatString.ForMatNumber_New(dtSupport.Rows[z]["soluong"].ToString());

                    if (t.Equals(dayNow))
                        content += " <th style='text-align:center; width:2.6%; border:0.5px solid black; padding-top:0.5px; padding-bottom: 0.5px; font-weight: normal; " + styleBackgroundNow + " '>" + soluong + "</th>";
                    else
                        content += " <th style='text-align:center; width:2.6%; border:0.5px solid black; padding-top:0.5px; padding-bottom: 0.5px; font-weight: normal; " + styleGray + " '>" + soluong + "</th>";
                }
                content += " <th style='text-align:center; width:2.6%; border:0.5px solid black; padding-top:0.5px; padding-bottom: 0.5px; font-weight: normal; " + styleGray + " '>" + FormatString.ForMatNumber_New(totalSupport.ToString()) + "</th>";
                content += " </tr>";

                if (count % 2 == 0)
                    styleGray = "";
                else
                    styleGray = "background-color:lightgray;";
                count++;

                content += " <tr>";                
                content += " <th style='text-align:center; width:2.6%; border:0.5px solid black; padding-top:0.5px; padding-bottom: 0.5px; font-weight: normal; " + styleGray + " '>Off</th>";
                for (int z = 0; z < dtHoliday.Rows.Count; z++)
                {
                    string t = "";
                    if (z < 9)
                        t = "0" + (z + 1).ToString();
                    else
                        t = (z + 1).ToString();

                    totalHoliday += double.Parse(dtHoliday.Rows[z]["soluong"].ToString());
                    string soluong = FormatString.ForMatNumber_New(dtHoliday.Rows[z]["soluong"].ToString());
                   
                    if (t.Equals(dayNow))
                        content += " <th style='text-align:center; width:2.6%; border:0.5px solid black; padding-top:0.5px; padding-bottom: 0.5px; font-weight: normal; " + styleBackgroundNow + " '>" + soluong + "</th>";
                    else
                        content += " <th style='text-align:center; width:2.6%; border:0.5px solid black; padding-top:0.5px; padding-bottom: 0.5px; font-weight: normal; " + styleGray + " '>" + soluong + "</th>";
                }
                content += " <th style='text-align:center; width:2.6%; border:0.5px solid black; padding-top:0.5px; padding-bottom: 0.5px; font-weight: normal; " + styleGray + " '>" + FormatString.ForMatNumber_New(totalHoliday.ToString()) + "</th>";
                content += " </tr>";
            }    

            content += "</table>";

            context.Response.Write(content);
        }

        private void MonthlyDivDetail(HttpContext context)
        {
            string language = "1";
            if (context.Request.QueryString["language"] != null)
                language = context.Request.QueryString["language"].ToString();

            string thang = "";
            if (context.Request.QueryString["thang"] != null)
                thang = context.Request.QueryString["thang"].ToString();

            string nam = "";
            if (context.Request.QueryString["nam"] != null)
                nam = context.Request.QueryString["nam"].ToString();

            string phongban = "";
            if (context.Request.QueryString["phongban"] != null)
                phongban = context.Request.QueryString["phongban"].ToString();

            string userId = "0";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            // Xác định thứ của ngày đầu tiên trong tháng.
            DateTime dateTime = new DateTime(int.Parse(nam), int.Parse(thang), 1);
            string weekdays = dateTime.DayOfWeek.ToString();
            string styleBackgroundColor = "";            
            int numberOfDaysInMonth = DateTime.DaysInMonth(int.Parse(nam), int.Parse(thang));                       
            string _thang = DateTime.Now.ToString("MM");
            string _nam = DateTime.Now.ToString("yyyy"); ;
            int _dayOfTheFirstDayOfTheMonth = this.returnDayofWeek(weekdays);
            //int _countDaysOfMonth = 1;
           
            ExecuteData xl = new ExecuteData();

            // lấy thông tin nhân sự
            string condition = "";
            string sql = "";
           
            string content = " <table class='table table-bordered' style='border:1px solid black;'> " +
                            " <tr style='background-color:LightGray; font-size:small;'> " +                            
                            " 	<th style='text-align:center; width:14%; font-size:smaller; padding:2px;'>Mon</th> " +
                            " 	<th style='text-align:center; width:14%; font-size:smaller; padding:2px;'>Tue</th> " +
                            " 	<th style='text-align:center; width:14%; font-size:smaller; padding:2px;'>Wed</th> " +
                            " 	<th style='text-align:center; width:14%; font-size:smaller; padding:2px;'>Thu</th> " +
                            " 	<th style='text-align:center; width:14%; font-size:smaller; padding:2px;'>Fri</th> " +
                            " 	<th style='text-align:center; width:14%; font-size:smaller; padding:2px;'>Sat</th> " +
                            " 	<th style='text-align:center; width:14%; font-size:smaller; padding:2px; color:Red; font-weight: bolder'>Sun</th> " +
                            " </tr> ";

            if (language.Equals("2"))
            {
                content = " <table class='table table-bordered' style='border:1px solid black;'> " +
                        " <tr style='background-color:LightGray; font-size:small;'> " +                        
                        " 	<th style='text-align:center; width:14%; font-size:smaller; padding:2px;'>Thứ Hai</th> " +
                        " 	<th style='text-align:center; width:14%; font-size:smaller; padding:2px;'>Thứ Ba</th> " +
                        " 	<th style='text-align:center; width:14%; font-size:smaller; padding:2px;'>Thứ Tư</th> " +
                        " 	<th style='text-align:center; width:14%; font-size:smaller; padding:2px;'>Thứ Năm</th> " +
                        " 	<th style='text-align:center; width:14%; font-size:smaller; padding:2px;'>Thứ Sáu</th> " +
                        " 	<th style='text-align:center; width:14%; font-size:smaller; padding:2px;'>Thứ Bảy</th> " +
                        " 	<th style='text-align:center; width:14%; font-size:smaller; padding:2px; color:Red; font-weight: bolder'>Chủ Nhật</th> " +
                        " </tr> ";
            }

            //sql = "";
            //DataTable dt = xl.ReadTable(sql);

            int countDaysOfMonth = 1;
            int dayOfTheFirstDayOfTheMonth = this.returnDayofWeek(weekdays);
            int _day = 0;
            string day = "";
          
            string _styleDay = "";

            if (thang.Equals(_thang) && nam.Equals(_nam))
                styleBackgroundColor = "background-color: lightcyan;";

            for (int i = 0; i < 6; i++)
            {
                if (i == 0)
                {
                    content += " <tr> ";

                    for (int j = 0; j < 7; j++)
                    {       
                        if (j < dayOfTheFirstDayOfTheMonth)
                        {
                            content += "<td style='padding:2px; color:white;'>.</td> ";
                        }
                        else
                        {
                            if (thang.Length > 1)
                            {
                                day = countDaysOfMonth.ToString() + "-" + thang + "-" + nam;

                                if(countDaysOfMonth < 10)
                                    day = "0" + countDaysOfMonth.ToString() + "-" + thang + "-" + nam;
                                //
                                //            

                                DateTime dt = new DateTime(int.Parse(nam), int.Parse(thang), countDaysOfMonth);
                                _day = returnDayofWeek(dt.DayOfWeek.ToString());

                                _styleDay = this.returnWeekend(xl, countDaysOfMonth, int.Parse(thang), int.Parse(nam), phongban);
                                content += "<td style='padding:2px; " + styleBackgroundColor + "'><a href='javascript:void(0);' data-toggle='modal' data-target='#exampleModal' data-whatever='" + day + "' " + _styleDay + ">" + countDaysOfMonth.ToString() + "</a><input type='hidden' class='form-control' style='font-size:small; padding: 0px; height:28px; text-align:right;' name='thang" + thang + "' value='" + _day + "' readonly></td> ";                               
                                countDaysOfMonth++;
                            }
                            else
                            {
                                content += "<td style='padding:2px; color:white;'>.</td> ";
                            }    
                                
                        }

                        if (countDaysOfMonth >= numberOfDaysInMonth)
                            thang = "";
                    }
                    content += " </tr> ";
                }
                else
                {
                    content += " <tr> ";
                    for (int j = 0; j < 7; j++)
                    {
                        if (thang.Length > 1)
                        {
                            day = countDaysOfMonth.ToString() + "-" + thang + "-" + nam;
                            _styleDay = this.returnWeekend(xl, countDaysOfMonth, int.Parse(thang), int.Parse(nam), phongban);

                            if (countDaysOfMonth < 10)
                                day = "0" + countDaysOfMonth.ToString() + "-" + thang + "-" + nam;

                            DateTime dt = new DateTime(int.Parse(nam), int.Parse(thang), countDaysOfMonth);
                            _day = returnDayofWeek(dt.DayOfWeek.ToString());

                            content += "<td style='padding:2px; " + styleBackgroundColor + "'><a href='javascript:void(0);' data-toggle='modal' data-target='#exampleModal' data-whatever='" + day + "' " + _styleDay + ">" + countDaysOfMonth.ToString() + "</a><input type='hidden' class='form-control' style='font-size:small; padding: 0px; height:28px; text-align:right;' name='thang" + thang + "' value='" + _day + "' readonly></td> ";
                            if (countDaysOfMonth == numberOfDaysInMonth)
                            {
                                countDaysOfMonth = 1;
                                thang = "";
                            }
                            else
                                countDaysOfMonth++;
                        }
                        else
                        {                            
                            content += "<td style='padding:2px; color:white;'>.</td> ";
                        }    
                    }
                    content += " </tr> ";
                }
            }

            //dt.Clear();
            //dt.Clone();

            content += "</table>";

            context.Response.Write(content);
        }

        private string returnWeekend(ExecuteData xl, int ngay, int thang, int nam, string phongban_fk)
        {
            DateTime dt = new DateTime(nam, thang, ngay);
            string dayNow = DateTime.Now.ToString("dd");
            string monthNow = DateTime.Now.ToString("MM");
            string yearNow = DateTime.Now.ToString("yyyy");

            if (xl == null)
                xl = new ExecuteData();

            string sql = "SELECT COUNT(*) FROM NgayNghiLe_PhongBan WHERE ngay = " + ngay + " AND thang = " + thang + " AND hangnam = 1 AND trangthai = 1 ";
            object obj = xl.ExecuteScalarSQL(sql);
            if (obj != null)
            {
                if (int.Parse(obj.ToString()) > 0)
                {
                    if (ngay == int.Parse(dayNow) && thang == int.Parse(monthNow) && nam == int.Parse(yearNow))
                        return "style='display: inline-block; text-align: center; margin-left:-2px; color:Red; font-weight: bolder; border: 0.75px solid black; width: 35px; height: 18px; border-radius: 50%;' ";
                    return "style='color:Red; font-weight: bolder;' ";
                }                    
                else
                {
                    string thoigian = ngay.ToString() + "-" + thang + "-" + nam;

                    sql = "SELECT ISNULL((SELECT mausac FROM LoaiNgayNghi WHERE pk_seq = loaingaynghi_fk), '') AS mausac FROM NgayNghiLe_PhongBan " +
                        " WHERE CONVERT(datetime, ngaynghi, 105) <= CONVERT(datetime, '" + thoigian + "', 105) AND CONVERT(datetime, denngay, 105) >= CONVERT(datetime, '" + thoigian + "', 105) AND phongban_fk = '" + phongban_fk + "' AND hangnam = 0 AND trangthai = 1 ";
                    DataTable dtR = xl.ReadTable(sql);
                    
                    if (dtR.Rows.Count > 0)
                    {
                        string color = dtR.Rows[0]["mausac"].ToString();

                        if (ngay == int.Parse(dayNow) && thang == int.Parse(monthNow) && nam == int.Parse(yearNow))
                            return "style='display: inline-block; text-align: center; margin-left:-2px; color:" + color + "; font-weight: bolder; border: 0.75px solid black; width: 35px; height: 18px; border-radius: 50%;' ";
                        // return "style='display: inline-block; text-align: left; margin-left:-1px; color:" + color + "; font-weight: bolder; border: 0.75px solid gray; width: 100%; height: 100%;' ";
                        return "style='color:" + color + "; font-weight: bolder;' ";
                    }                            
                }
            }

            if (returnDayofWeek(dt.DayOfWeek.ToString()) == 5)
            {
                if (ngay == int.Parse(dayNow) && thang == int.Parse(monthNow) && nam == int.Parse(yearNow))
                    return "style='display: inline-block; text-align: center; margin-left:-2px; color:blue; font-weight: bolder; border: 0.75px solid black; width: 35px; height: 18px; border-radius: 50%;' ";
                return "style='color:blue;' ";
            }

            if (returnDayofWeek(dt.DayOfWeek.ToString()) == 6)
            {
                if (ngay == int.Parse(dayNow) && thang == int.Parse(monthNow) && nam == int.Parse(yearNow))
                    return "style='display: inline-block; text-align: center; margin-left:-2px; color:Red; font-weight: bolder; border: 0.75px solid black; width: 35px; height: 18px; border-radius: 50%;' ";
                return "style='color:Red; font-weight: bolder;' ";
            }

            if (ngay == int.Parse(dayNow) && thang == int.Parse(monthNow) && nam == int.Parse(yearNow))
                return "style='display: inline-block; text-align: center; margin-left:-2px; color:Black; font-weight: bolder; border: 0.75px solid black; width: 35px; height: 18px; border-radius: 50%;' ";

            return "style='color:Black;' ";

        }

        private int returnDayofWeek(string day)
        {
            if (day.Equals("Monday"))
                return 0;
            else if (day.Equals("Tuesday"))
                return 1;
            else if (day.Equals("Wednesday"))
                return 2;
            else if (day.Equals("Thursday"))
                return 3;
            else if (day.Equals("Friday"))
                return 4;
            else if (day.Equals("Saturday"))
                return 5;
            else return 6;
        }

        private string returnTitleWeeky(string language, int day)
        {
            string _day = "";
            if (language.Equals("1"))
            {
                switch (day)
                {
                    case 0:
                        _day = "Mon";
                        break;
                    case 1:
                        _day = "Tue";
                        break;
                    case 2:
                        _day = "Wed";
                        break;
                    case 3:
                        _day = "Thu";
                        break;
                    case 4:
                        _day = "Fri";
                        break;
                    case 5:
                        _day = "Sat";
                        break;
                    case 6:
                        _day = "Sun";
                        break;
                }
            }
            else
            {
                switch (day)
                {
                    case 0:
                        _day = "Thứ Hai";
                        break;
                    case 1:
                        _day = "Thứ Ba";
                        break;
                    case 2:
                        _day = "Thứ Tư";
                        break;
                    case 3:
                        _day = "Thứ Năm";
                        break;
                    case 4:
                        _day = "Thứ Sáu";
                        break;
                    case 5:
                        _day = "Thứ Bảy";
                        break;
                    case 6:
                        _day = "Chủ Nhật";
                        break;
                }
            }

            return _day;
        }

        public string GetBackgroundColor(string location, string aspect, string khachhang_fk, string trangthai)
        {
            string result = "";
                switch (aspect)
                {
                    case "0":
                        result = "";
                        break;
                    case "1":
                        result = "; background-color:orange";
                        break;
                    case "2":
                        result = "; background-color:yellow";
                        break;
                    case "3":
                    if (khachhang_fk.Equals("100001"))
                        result = "; background-color:Aqua";
                    else if (khachhang_fk.Equals("100002"))
                        result = "; background-color:SandyBrown";
                    else if (khachhang_fk.Equals("100003"))
                        result = "; background-color:Greenyellow";
                    else if (khachhang_fk.Equals("100004"))
                        result = "; background-color:DeepSkyBlue";
                    else if (khachhang_fk.Equals("100005"))
                        result = "; background-color:Turquoise";
                    else
                        result = "; background-color:red";
                        break;
                    case "9":
                        result = "; background-color:pink";
                    break;
                    default:
                    result = "; background-color:red";
                    break;
                }

            if (trangthai.Equals("0"))
                result = "; background-color:DeepSkyBlue";
            if (trangthai.Equals("1"))
                result = "; background-color:yellow";

            return result;
        }

        private string returnNumber(string str)
        {
            if (str.Equals("0"))
                return "";
            else
                return FormatString.ForMatNumber(str);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}