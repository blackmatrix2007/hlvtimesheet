using HLVTimeSheet.AcsessData;
using HLVTimeSheet.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace HLVTimeSheet.Admin.Hander
{
    /// <summary>
    /// Summary description for hdTimeKeeping
    /// </summary>
    public class hdTimeKeeping : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;

            string action = "";
            if (context.Request.QueryString["action"] != null)
                action = context.Request.QueryString["action"].ToString();

            switch(action)
            {
                case "saveInforCheckIn":
                    SaveInforCheckIn(context);
                    break;
                case "viewStaff":
                    GetInforStaff_CheckIn(context);
                    break;
                case "viewStaffDetail":
                    GetInforStaff_CheckInDetail(context);
                    break;
                case "viewStaffYearTotal":
                    GetInforStaff_YearTotal(context);
                    break;
                case "viewStaffYearDetail":
                    GetInforStaff_YearDetail(context);
                    break;
                default: break;
            }               

        }

        private void SaveInforCheckIn(HttpContext context)
        {
            string ngaynhap = "";
            string phongban = "";
            string nhansu = "";
            string trangthai = "";
            string gioIn = "";
            string phutIn = "";
            string gioOut = "";
            string phutOut = "";

            string hinhanhIn = "";
            string hinhanhOut = "";
            string idMayCheckIn = "";
            string idMayCheckOut = "";

            if (context.Request.QueryString["ngaynhap"] != null)
                ngaynhap = context.Request.QueryString["ngaynhap"].ToString();

            if (context.Request.QueryString["trangthai"] != null)
                trangthai = context.Request.QueryString["trangthai"].ToString();

            if (context.Request.QueryString["phongban"] != null)
                phongban = context.Request.QueryString["phongban"].ToString();

            if (context.Request.QueryString["nhansu"] != null)
                nhansu = context.Request.QueryString["nhansu"].ToString();

            if (context.Request.QueryString["gioIn"] != null)
                gioIn = context.Request.QueryString["gioIn"].ToString();

            if (context.Request.QueryString["phutIn"] != null)
                phutIn = context.Request.QueryString["phutIn"].ToString();

            if (context.Request.QueryString["gioOut"] != null)
                gioOut = context.Request.QueryString["gioOut"].ToString();

            if (context.Request.QueryString["phutOut"] != null)
                phutOut = context.Request.QueryString["phutOut"].ToString();

            if (context.Request.QueryString["hinhanhIn"] != null)
                hinhanhIn = context.Request.QueryString["hinhanhIn"].ToString();

            if (context.Request.QueryString["hinhanhOut"] != null)
                hinhanhOut = context.Request.QueryString["hinhanhOut"].ToString();

            if (context.Request.QueryString["idMayCheckIn"] != null)
                idMayCheckIn = context.Request.QueryString["idMayCheckIn"].ToString();

            if (context.Request.QueryString["idMayCheckOut"] != null)
                idMayCheckOut = context.Request.QueryString["idMayCheckOut"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                TimeKeepingController timeObj = new TimeKeepingController();

                string msg = timeObj.INSERT_TimeKeeping_New(ngaynhap, phongban, nhansu, gioIn, phutIn, gioOut, phutOut, "0", hinhanhIn, hinhanhOut, idMayCheckIn, idMayCheckOut, trangthai, userId);

                context.Response.Write(msg);
            }
        }

        private void GetInforStaff_CheckIn(HttpContext context)
        {
            string nhansu = "";
            if (context.Request.QueryString["nhansu"] != null)
                nhansu = context.Request.QueryString["nhansu"].ToString();

            string ngaynhap = "";
            if (context.Request.QueryString["ngaynhap"] != null)
                ngaynhap = context.Request.QueryString["ngaynhap"].ToString();

            TimeKeepingController timeObj = new TimeKeepingController();

            string msg = timeObj.GET_InformationStaff_CheckIn(nhansu, ngaynhap);

            context.Response.Write(msg);
        }

        private void GetInforStaff_YearTotal(HttpContext context)
        {
            string nhansu = "";
            if (context.Request.QueryString["nhansu"] != null)
                nhansu = context.Request.QueryString["nhansu"].ToString();
         
            TimeKeepingController timeObj = new TimeKeepingController();

            string msg = timeObj.GET_InformationStaff_Total(nhansu);

            context.Response.Write(msg);
        }

        private void GetInforStaff_CheckInDetail(HttpContext context)
        {
            string nhansu = "";
            if (context.Request.QueryString["nhansu"] != null)
                nhansu = context.Request.QueryString["nhansu"].ToString();

            string ngaynhap = "";
            if (context.Request.QueryString["ngaynhap"] != null)
                ngaynhap = context.Request.QueryString["ngaynhap"].ToString();

            string fontsize = "small; font-weight: bolder;";

            string content = " <table class='table table-bordered' style='font-size:" + fontsize + "; border: 0.5px solid black;'> " +
            " <tr style='background-color:lightgray;'> " +
            " 	<th style='text-align:center; width:10%; border: 0.5px solid black;'>No.</th> " +
            " 	<th style='text-align:center; width:40%; border: 0.5px solid black;'>Location</th> " +
            " 	<th style='text-align:center; width:50%; border: 0.5px solid black;'>Time</th> ";
            content += " </tr> ";

            ExecuteData xl = new ExecuteData();
            string sql = " SELECT b.thoigian, b.idMayChamCong " +
            " FROM ChamCong a INNER JOIN ChamCong_ChiTiet b ON a.pk_seq = b.chamcong_fk AND b.nhansu_fk = '" + nhansu + "' AND b.ngaynhap = '" + ngaynhap + "' " +
            " ORDER BY b.pk_seq DESC ";
            DataTable dt = xl.ReadTable(sql);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    content += " <tr> ";
                    content += "<td style='text-align:center; border: 0.5px solid black;'>" + (i + 1).ToString() + "</td> ";
                    content += "<td style='text-align:center; border: 0.5px solid black;'>" + dt.Rows[i]["idMayChamCong"].ToString() + "</td> ";
                    content += "<td style='text-align:center; border: 0.5px solid black;'>" + dt.Rows[i]["thoigian"].ToString() + "</td> ";
                    content += " </tr> ";
                }
            }
            else
            {
                content += " <tr> ";
                content += "<td style='text-align:center; border: 0.5px solid black;' colspan='3'>No data</td> ";                
                content += " </tr> ";
            }
                content += "</table>";
            context.Response.Write(content);
        }

        private void GetInforStaff_YearDetail(HttpContext context)
        {
            string nhansu = "";
            if (context.Request.QueryString["nhansu"] != null)
                nhansu = context.Request.QueryString["nhansu"].ToString();

            string fontsize = "small; font-weight: bolder;";

            ExecuteData xl = new ExecuteData();
            string _style = "";
            string style = "";
            string styleDay = "";
            string styleTextDay = "";
            string backgroundColor = "";
            string content = "";
            string today = DateTime.Now.ToString("dd-MM-yyyy");
            string year = DateTime.Now.ToString("yyyy");

            content += "<table class='table table-hover table-bordered table-striped' style='font-size:smaller;'>";
            content += "<tr> " +
            "    <th style='text-align:center; border:1px solid black; width:12%; background-color:lightgray;' rowspan='2'>Month</th> " +
            "    <th style='text-align:center; border:1px solid black; width:82.75%; background-color:lightgray;' colspan='31'>Day of the month (" + year + ")</th> " +
            "       </tr> ";
            content += "<tr> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>01</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>02</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>03</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>04</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>05</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>06</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>07</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>08</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>09</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>10</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>11</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>12</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>13</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>14</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>15</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>16</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>17</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>18</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>19</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>20</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>21</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>22</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>23</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>24</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>25</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>26</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>27</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>28</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>29</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>30</th> " +
            " <th style='text-align:center; width:2.75%; border:1px solid black; background-color:lightgray;'>31</th> " +
            " </tr> ";

            int monthMax = 12;           
            for (int x = 1; x <= monthMax; x++) // Tháng 
            {
                int songayFulltime = 0;
                int songayHafltime = 0;
                int songayApprovedLeave = 0;
                int songayAB = 0;
                int songayLateArrival = 0;
                int songayNotCompleted = 0;
                string _thang = (x).ToString();
                if (x < 10)
                    _thang = "0" + _thang;
                int songaytrongThang = 0;
                string ngaydauthang = "01-" + _thang + "-" + year;
                string sql = "SELECT CONVERT(nvarchar(10), (DATEADD(DAY,-1, DATEADD(MM, DATEDIFF(MM, 0, CONVERT(datetime, '" + ngaydauthang + "', 105))+1, 0))), 105)";
                string ngaycuoithang = xl.ExecuteScalarSQL(sql).ToString();

                sql = "SELECT DATEDIFF(DAY, CONVERT(datetime, '" + ngaydauthang + "', 105), CONVERT(datetime, '" + ngaycuoithang + "', 105))";
                object obj = xl.ExecuteScalarSQL(sql);
                if (obj != null)
                {
                    songaytrongThang = int.Parse(obj.ToString()) + 1;
                }

                content += "<tr>";
                content += "<td style='text-align:center; border:1px solid black; font-weight: bolder;'> " + FormatString.returnNameMonth(x - 1) + "</td> ";

                for (int z = 1; z < 32; z++) // Ngày 
                {
                    string _ngay = (z).ToString();
                    if (z < 10)
                        _ngay = "0" + _ngay;

                    string _borderBottom = "border-bottom:1px solid black;";
                    string info = "";
                    string textNoteLate = "";
                    string textNoteOT = "";
                    string trangthai = "";
                    string thoigian = "";
                    string thoigianIn = "";
                    string thoigianOut = "";
                    int gioStart = 0;
                    int gioEnd = 0;
                    int phutStart = 0;
                    int phutEnd = 0;

                    int gioIn = 0;
                    int gioOut = 0;
                    int phutIn = 0;
                    int phutOut = 0;

                    if (z <= songaytrongThang)
                    {
                        _style = "";
                        // thứ hiện tại
                        if (int.Parse(year) > 0 && x > 0 && z > 0)
                        {
                            DateTime date = new DateTime(int.Parse(year), x, z);
                            if (date.DayOfWeek.ToString().Equals("Sunday"))
                                _style = "color: red; ";                         
                        }


                        sql = " SELECT a.trangthai, b.nhansu_fk, b.thoigian, b.loai, b.gio, b.phut, b.gioStart, b.phutStart, b.gioEnd, b.phutEnd " +
                       " FROM ChamCong a INNER JOIN ChamCong_ChiTiet b ON a.pk_seq = b.chamcong_fk AND b.trangthai in (1, 2, 3, 4, 5) AND a.nhansu_fk = '" + nhansu + "' AND a.ngaynhap = '" + _ngay + "' " +
                       " ORDER BY loai, gio, phut ASC ";
                        DataTable dtK = xl.ReadTable(sql);
                        for (int i = 0; i < dtK.Rows.Count; i++)
                        {
                            string loai = dtK.Rows[i]["loai"].ToString();
                            trangthai = dtK.Rows[i]["trangthai"].ToString();

                            gioStart = int.Parse(dtK.Rows[i]["gioStart"].ToString());
                            gioEnd = int.Parse(dtK.Rows[i]["gioEnd"].ToString());
                            phutStart = int.Parse(dtK.Rows[i]["phutStart"].ToString());
                            phutEnd = int.Parse(dtK.Rows[i]["phutEnd"].ToString());

                            switch (loai)
                            {
                                case "1":
                                    thoigianIn = dtK.Rows[i]["thoigian"].ToString();
                                    gioIn = int.Parse(dtK.Rows[i]["gio"].ToString());
                                    phutIn = int.Parse(dtK.Rows[i]["phut"].ToString());
                                    break;
                                case "2":
                                    thoigianOut = dtK.Rows[i]["thoigian"].ToString();
                                    gioOut = int.Parse(dtK.Rows[i]["gio"].ToString());
                                    phutOut = int.Parse(dtK.Rows[i]["phut"].ToString());
                                    break;
                                default:
                                    break;
                            }
                        }

                        thoigian = thoigianIn + "-" + thoigianOut;
                        if (thoigian.Length < 2)
                            thoigian = "";

                        styleTextDay = "color:black;";
                       
                        double valueIn = gioStart * 60 + phutStart;
                        double valueOut = gioEnd * 60 + phutEnd;

                        double actualIn = gioIn * 60 + phutIn;
                        double actualOut = gioOut * 60 + phutOut;

                        double chenhlechIn = 0;
                        double chenhlechOut = 0;

                        switch (trangthai)
                        {
                            case "1":
                                if (actualIn > valueIn)
                                {
                                    chenhlechIn = Math.Round((actualIn - valueIn) / 60, 1);
                                    textNoteLate = "Late: " + chenhlechIn.ToString();
                                }
                                if (actualOut > valueOut)
                                {
                                    chenhlechOut = Math.Round((actualOut - valueOut) / 60, 1);
                                    textNoteOT = "OT: " + chenhlechOut;
                                }
                                styleDay = "background-color: lightgreen;";
                                songayFulltime++;

                                if (!_ngay.Equals(today) && ((thoigianIn.Length > 3 && thoigianOut.Length < 3) || (thoigianIn.Length < 3 && thoigianOut.Length > 3)))
                                {
                                    styleDay = "background-color: lightpink;";
                                    songayNotCompleted++;
                                    songayFulltime--;
                                }


                                break;
                            case "2":
                                styleDay = "background-color: yellow;";
                                songayHafltime++;

                                if (!_ngay.Equals(today) && ((thoigianIn.Length > 3 && thoigianOut.Length < 3) || (thoigianIn.Length < 3 && thoigianOut.Length > 3)))
                                {
                                    styleDay = "background-color: lightpink;";
                                    songayNotCompleted++;
                                    songayHafltime--;
                                }

                                break;
                            case "3":
                                styleDay = "background-color: orange;";
                                songayApprovedLeave++;


                                break;
                            case "4":
                                styleDay = "background-color: orangered;";
                                songayAB++;
                                break;
                            case "5":
                                if (actualIn > valueIn)
                                {
                                    chenhlechIn = Math.Round((actualIn - valueIn) / 60, 1);
                                    textNoteLate = "Late: " + chenhlechIn.ToString();
                                }
                                if (actualOut > valueOut)
                                {
                                    chenhlechOut = Math.Round((actualOut - valueOut) / 60, 1);
                                    textNoteOT = "OT: " + chenhlechOut;
                                }
                                styleDay = "background-color: lightgray;";
                                songayLateArrival++;

                                if (!_ngay.Equals(today) && ((thoigianIn.Length > 3 && thoigianOut.Length < 3) || (thoigianIn.Length < 3 && thoigianOut.Length > 3)))
                                {
                                    styleDay = "background-color: lightpink;";
                                    songayNotCompleted++;
                                    songayLateArrival--;
                                }

                                break;
                            default:
                                break;
                        }

                        // Xác định tình trạng ngày làm việc
                        info = "<div style='text-align:center; float:left; width:100%; height:33.3%;" + styleDay + "'>" + thoigian + "</div>";
                        info += "<div style='text-align:center; float:left; width:100%; height:33.3%;" + styleDay + "'>" + textNoteLate + "</div>";
                        info += "<div style='text-align:center; float:left; width:100%; height:33.3%;" + styleDay + "'>" + textNoteOT + "</div>";

                        content += "<td style='text-align:center; border:1px solid lightgray; " + _borderBottom + " " + backgroundColor + _style + "'>" + info + "</td>";
                        styleDay = "";
                        info = "";
                    }
                    else
                        content += "<td style='text-align:center; border:1px solid lightgray; " + _borderBottom + "'></td>";

                    backgroundColor = "";
                }               

            }

            content += "</table>";

            context.Response.Write(content);
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