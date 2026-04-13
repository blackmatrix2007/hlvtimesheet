using HLVTimeSheet.AcsessData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HLVTimeSheet.Admin
{
    public partial class uc_timekeeping_dailyattendance : System.Web.UI.UserControl
    {
        string[] rolesAttributePP = new string[] { "0", "0", "0", "0", "0", "0", "0", "0" };
        
        public string language = "1";
        public int pageID = 1;
        public int soDONG = 100;

        public uc_timekeeping_dailyattendance()
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
                if (Request.QueryString["pageNumber"] != null)
                    pageID = int.Parse(Request.QueryString["pageNumber"].ToString());
                if (Request.QueryString["line"] != null)
                    soDONG = int.Parse(Request.QueryString["line"].ToString());

                string phongban_fk = "";
                if (Request.QueryString["depa"] != null)
                    phongban_fk = Request.QueryString["depa"].ToString();

                ExecuteData xl = new ExecuteData();
                string query = "SELECT pk_seq, '[ ' + ma + ' ] ' + ten AS ten " +
                    " FROM PhongBan WHERE trangthai = 1 " +
                    " ORDER BY ma ";
                DataTable dt = xl.ReadTable(query);

                ddlPhongBan.Items.Add(new ListItem("", "0"));
                ddlPhongBanShow.Items.Add(new ListItem("", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlPhongBan.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                    ddlPhongBanShow.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                query = "SELECT pk_seq, ten " +
                    " FROM ChucVu WHERE trangthai = 1 " +
                    " ORDER BY ten ";
                dt = xl.ReadTable(query);

                ddlChucVu.Items.Add(new ListItem("", "0"));                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlChucVu.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));                    
                }

                query = "SELECT pk_seq, ten " +
                   " FROM NhaPhanPhoi WHERE trangthai = 1 " +
                   " ORDER BY ten ";
                dt = xl.ReadTable(query);

                ddlChiNhanh.Items.Add(new ListItem("", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlChiNhanh.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                ddlNam.Items.Add(new ListItem("Year", "0"));
                int namhientai = int.Parse(DateTime.Now.ToString("yyyy"));

                for (int i = namhientai - 8; i < namhientai + 2; i++)
                {
                    ddlNam.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }

                ddlThang.Items.Add(new ListItem("Month", "0"));

                for (int i = 1; i <= 12; i++)
                {
                    string str = i.ToString();
                    if (str.Length < 2)
                        str = "0" + str;
                    ddlThang.Items.Add(new ListItem(str.ToString(), str));
                }

                ddlTrangThai.Items.Add(new ListItem("", "0"));
                ddlTrangThai.Items.Add(new ListItem("Full time", "1"));
                ddlTrangThai.Items.Add(new ListItem("Hafl time", "2"));
                ddlTrangThai.Items.Add(new ListItem("Approved leave", "3"));
                ddlTrangThai.Items.Add(new ListItem("AB", "4"));
                ddlTrangThai.Items.Add(new ListItem("Late arrival", "5"));

                ddlNam.SelectedValue = DateTime.Now.ToString("yyyy");
                ddlThang.SelectedValue = DateTime.Now.ToString("MM");

                if (phongban_fk.Length > 3)
                    ddlPhongBan.SelectedValue = phongban_fk;

                txtSpace.Value = "Check out";

                ddlGioStart.Items.Clear();
                ddlGioEnd.Items.Clear();
                for (int i = 0; i < 24; i++)
                {
                    string str = i.ToString();
                    if (i < 10)
                        str = "0" + i.ToString();

                    ddlGioStart.Items.Add(new ListItem(str, str));
                    ddlGioEnd.Items.Add(new ListItem(str, str));                    
                }

                int j = 0;
                ddlPhutStart.Items.Clear();
                ddlPhutEnd.Items.Clear();
                while (j < 60)
                {
                    string str = j.ToString();
                    if (j < 10)
                        str = "0" + j.ToString();

                    ddlPhutStart.Items.Add(new ListItem(str, str));
                    ddlPhutEnd.Items.Add(new ListItem(str, str));
                    
                    j++;
                }

                this.loadVersion();
                this.loadInfo(xl);
            }

        }

        private void loadInfo(ExecuteData xl)
        {
            if (xl == null)
                xl = new ExecuteData();
            //this.loadTitle();

            ltInfor.Text = "";
            string tungay = txtTuNgay.Value;
            string denngay = txtDenNgay.Value;
            
            string style = "";
            string styleDay = "";
            string styleTextDay = "";
            string title = "";
            string title2 = "";
            string content = "";

            string condition = "";           
            string sql = "";

            sql = "SELECT DATEDIFF(DAY, CONVERT(datetime, '" + tungay + "', 105), CONVERT(datetime, '" + denngay + "', 105))";
            object obj = xl.ExecuteScalarSQL(sql);
            if (obj != null)
                txtSoNgay.Value = (int.Parse(obj.ToString()) + 1).ToString();
            else
                txtSoNgay.Value = "0";

            string today = DateTime.Now.ToString("dd-MM-yyyy");
            int days = int.Parse(txtSoNgay.Value);
            List<string> listDate = FormatString.returnListDay(tungay, denngay);

            int count = 0;
            title += " <tr>" +
            " <th style='text-align:center; border:1px solid black; height:20px; width:2%; background-color:silver;' rowspan='2'>#</th>" +
            " <th style='text-align:center; border:1px solid black; height:20px; width:12%; background-color:silver;' rowspan='2'>Name</th>";
            foreach (string _ngaynhap in listDate)
            {
                // thứ hiện tại
                string _colorDay = "";
                string day_show = "";
                string _backgroundTitle = "background-color:silver;";
                int day_tungay = int.Parse(_ngaynhap.Substring(0, 2));
                int month_tungay = int.Parse(_ngaynhap.Substring(3, 2));
                int year_tungay = int.Parse(_ngaynhap.Substring(6, 4));

                if (day_tungay == 1 || count == 0 || count == listDate.Count - 1)
                    day_show = day_tungay.ToString() + "/" + month_tungay.ToString();
                else
                    day_show = day_tungay.ToString();

                DateTime date = new DateTime(year_tungay, month_tungay, day_tungay);
                if (date.DayOfWeek.ToString().Equals("Sunday"))
                    _colorDay = "color: red;";

                if (_ngaynhap.Equals(today))
                    _backgroundTitle = "background-color:lightskyblue;";

                title += " <th style='text-align:center; border:1px solid black; height:20px; width:2.5%; " + _backgroundTitle + _colorDay + " '>" + day_show + "</th>";
                title2 += " <th style='text-align:center; border:1px solid black; height:20px; width:2.5%; " + _backgroundTitle + _colorDay + " '>" + date.DayOfWeek.ToString().Substring(0, 3) + "</th>";

                int dayInMonth = DateTime.DaysInMonth(year_tungay, month_tungay);

                if (day_tungay == dayInMonth)
                {
                    if (month_tungay == 12)
                    {
                        year_tungay++;
                        month_tungay = 1;
                    }
                    else
                    {
                        month_tungay++;
                    }

                    day_tungay = 0;
                }
                day_tungay++;
                count++;
            }

            title += " <th style='text-align:center; border:1px solid black; height:20px; width:5%; background-color:silver;' rowspan='2'>Total</th>";
            title += " </tr>";

            title += title2;

            if (ddlPhongBan.SelectedValue.Trim().Length > 3)
            {
                condition += " AND (a.phongban_fk = '" + ddlPhongBan.SelectedValue + "' OR a.phongbanSupport_fk = '" + ddlPhongBan.SelectedValue + "') ";
            }

            txtNgayNhap.Value = today;

            sql = " SELECT ROW_NUMBER() OVER(ORDER BY a.capbac ASC) AS stt, a.pk_seq AS nhansu_fk, a.ma, a.ten, a.hinhanh, a.capbac " +
                " FROM DanhSachNhanSu a " +
                " WHERE a.trangthai in (1) " + condition;

            //CAI TIEN PHAN TRANG CHI LOAD 50 SP TREN 1 TRANG
            string queryTOTAL = " SELECT COUNT(stt) FROM ( " + sql + " ) DH  ";
            object tongsoDONG = xl.ExecuteScalarSQL(queryTOTAL);
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

            string queryCHITIET = " SELECT * FROM ( " + sql + " ) DH WHERE DH.stt >= " + tuDONG.ToString() + " and DH.stt <= " + denDONG.ToString() + " ORDER BY DH.capbac, DH.ten ";

            DataTable dt = xl.ReadTable(queryCHITIET);

            for (int z = 0; z < dt.Rows.Count; z++)
            {
                int songayFulltime = 0;
                int songayHafltime = 0;
                int songayApprovedLeave = 0;
                int songayAB = 0;
                int songayLateArrival = 0;

                string nhansu_fk = dt.Rows[z]["nhansu_fk"].ToString();
                string hinhanh = dt.Rows[z]["hinhanh"].ToString();
                if (hinhanh.Length < 3)
                    hinhanh = "avatardefault.png";
                
                string info = "<div style='text-align:left; margin-left:1%; float:left; width:34%;' id='divMbr" + nhansu_fk + "'><img src='https://hlv-ws-ssl.giangdc.company/Admin/Avatar/" + hinhanh + "' style='max-height:50%; max-width:50%;' /></div>";                

                string tennhansu = "<div style='text-align:left; width:100%; margin-top:10px;'>" + dt.Rows[z]["ten"].ToString() + "</div>";
                tennhansu += "<div style='text-align:center; width:100%; margin-top:3px; font-size:small;'>" + dt.Rows[z]["ma"].ToString() + "</div>";
                tennhansu = "<div style='text-align:left; float:left; width:65%;'>" + tennhansu + "</div>";

                info += tennhansu;

                content += "<tr>";
                content += " <td style='text-align:center; border:1px solid black; padding:1px; height:28px; font-size:small;'>" + (z + 1).ToString() + "</td>";
                content += " <td style='text-align:left; border:1px solid black; padding:1px; height:28px; font-size:small;'>" + info + "</td>";

                foreach (string _ngaynhap in listDate)
                {
                    info = "";
                    string textNoteLate = ".";
                    string textNoteOT = ".";
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

                    sql = " SELECT a.trangthai, b.nhansu_fk, b.thoigian, b.loai, b.gio, b.phut, b.gioStart, b.phutStart, b.gioEnd, b.phutEnd " +
                        " FROM ChamCong a INNER JOIN ChamCong_ChiTiet b ON a.pk_seq = b.chamcong_fk AND b.trangthai in (1, 2, 3, 4, 5) AND a.nhansu_fk = '" + nhansu_fk + "' AND a.ngaynhap = '" + _ngaynhap + "' " + 
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
                    {
                        thoigian = "NO DATA";
                        textNoteLate = "NO DATA";
                        textNoteOT = "NO DATA";
                        styleTextDay = "color:white;";
                    }
                    else
                    {
                        styleTextDay = "color:black;";
                    }

                    double valueIn = gioStart * 60 + phutStart;
                    double valueOut = gioEnd * 60 + phutEnd;

                    double actualIn = gioIn * 60 + phutIn;
                    double actualOut = gioOut * 60 + phutOut;

                    double chenhlechIn = 0;
                    double chenhlechOut = 0;
                    
                    switch(trangthai)
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
                            break;
                        case "2":
                            styleDay = "background-color: yellow;";
                            songayHafltime++;
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
                            break;
                        default:
                            break;
                    }    

                    // Xác định tình trạng ngày làm việc
                    info = "<div style='text-align:center; float:left; width:100%; height:33.3%;" + styleDay + "'><a href='javascript:void(0);' " + styleDay + "  data-toggle='modal' data-target='#exampleModal' data-whatever='" + _ngaynhap + "--" + nhansu_fk + "' style='font-size:smaller;" + styleTextDay + "'>" + thoigian + "</a></div>";
                    info += "<div style='text-align:center; float:left; width:100%; height:33.3%;" + styleDay + "'><a href='javascript:void(0);' " + styleDay + "  data-toggle='modal' data-target='#exampleModal' data-whatever='" + _ngaynhap + "--" + nhansu_fk + "' style='font-size:smaller;" + styleTextDay + "'>" + textNoteLate + "</a></div>";
                    info += "<div style='text-align:center; float:left; width:100%; height:33.3%;" + styleDay + "'><a href='javascript:void(0);' " + styleDay + "  data-toggle='modal' data-target='#exampleModal' data-whatever='" + _ngaynhap + "--" + nhansu_fk + "' style='font-size:smaller;" + styleTextDay + "'>" + textNoteOT + "</a></div>";

                    content += " <td style='text-align:left; border:1px solid black; padding:1px; height:28px; font-size:smaller;'>" + info + "</td>";
                    styleDay = "";
                    info = "";
                }
                info = "";
                info += "<div style='text-align:center; float:left; width:33.33%; height:50%; padding:5%; background-color: lightgreen;'>" + songayFulltime.ToString() + "</div>";
                info += "<div style='text-align:center; float:left; width:33.33%; height:50%; padding:5%; background-color: yellow;'>" + songayHafltime.ToString() + "</div>";
                info += "<div style='text-align:center; float:left; width:33.33%; height:50%; padding:5%; background-color: orange;'>" + songayApprovedLeave.ToString() + "</div>";
                info += "<div style='text-align:center; float:left; width:33.33%; height:50%; padding:5%; background-color: orangered;'>" + songayAB.ToString() + "</div>";
                info += "<div style='text-align:center; float:left; width:33.33%; height:50%; padding:5%; background-color: lightgray;'>" + songayLateArrival.ToString() + "</div>";
                info += "<div style='text-align:center; float:left; width:33.33%; height:50%; padding:5%; background-color: white;'></div>";
                content += " <td style='text-align:left; border:1px solid black; padding:1px; height:28px; font-size:small;'>" + info + "</td>";
                content += "</tr>";
            }
            ltInfor.Text += title;
            ltInfor.Text += content;
            ltInfor.Text += title;

            // PHAN TRANG
            string strPage = "";
            for (int i = 1; i <= (int)soTRANG; i++)
            {
                if (pageID == i)
                    strPage += "<option value='" + i.ToString() + "' selected='selected' >" + i.ToString() + "</option>";
                else
                    strPage += "<option value='" + i.ToString() + "' >" + i.ToString() + "</option>";
            }

            ltPhanTrang.Text = " <table style='font-size:small; text-align:center;' > " +
            "     <tr>" +
            "         <td style='width:50%;'>" +
            "             <ul class='pagination pagination-sm no-margin '>" +
            "                 <li><a href='javascript:void(0);'><img src='../Images/first.gif' width='16' height='16' title='The first page' onclick='moveTO2(-1, 1)' /></a></li>" +
            "                 <li><a href='javascript:void(0);'><img src='../Images/previous.gif' width='16' height='16' title='Previous' onclick='moveTO2(-1, -1)' /></a></li>" +
            "                 <li><a href='javascript:void(0);'><img src='../Images/next.gif' width='16' height='16' title='Next' onclick='moveTO2(1, -1)' /></a></li>" +
            "                 <li><a href='javascript:void(0);'><img src='../Images/last.gif' width='16' height='16' title='Last' onclick='moveTO2(1, 1)' /></a></li>" +
            "                 <li style='padding-left:10px;' >" +
            "                     <input type='hidden' id='pagedropdownNUMBER_MAX' value='" + soTRANG + "' >" +
            "                     <select id='pagedropdownNUMBER' style='height:28px;' onchange='moveTO(this.value)' >" + strPage +
            "                     </select>" +
            "                 </li>" +
            "                 <li style='margin-left:30px;' >" +
            "                     Show " +
            "                     <select style='height:28px;' onchange='ShowAll(this.value)'>" +
            "                         <option value='100' >100</option>" +
            "                         <option value='300' >300</option>" +
            "                         <option value='500' >500</option>" +
            "                         <option value='1000'>all</option>" +
            "                     </select>" +
            "                     line on page " +
            "                 </li>" +
            "             </ul>" +
            "         </td>" +
            "         <td style='width:50%; text-align:right;' >" +
            "             Line " + tuDONG + "-" + denDONG + " of total  " + tongsoDONG + " line, page " + pageID + " of " + soTRANG.ToString() +
            "         </td>" +
            "     </tr>" +
            " </table>";
        }

        private void loadNumberDay()
        {
            ExecuteData xl = new ExecuteData();
            txtTuNgay.Value = "01-" + ddlThang.SelectedValue + "-" + ddlNam.SelectedValue;
            string sql = "SELECT CONVERT(nvarchar(10), (DATEADD(DAY,-1, DATEADD(MM, DATEDIFF(MM, 0, CONVERT(datetime, '" + txtTuNgay.Value + "', 105))+1, 0))), 105)";
            txtDenNgay.Value = xl.ExecuteScalarSQL(sql).ToString();

            txtNgayNhap.Value = DateTime.Now.ToString("dd-MM-yyyy");

        }

        private void loadVersion()
        {
            this.loadNumberDay();
            this.loadInfo(null);            
        }

        protected void lbTimkiem_Click(object sender, EventArgs e)
        {
            this.loadVersion();
        }

        protected void ddlPhienBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.loadVersion();
        }

        protected void ddlPhongBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.loadVersion();
        }

        protected void ddlThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.loadVersion();
        }

        protected void ddlNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.loadVersion();
        }

        private string returnStr(int number)
        {
            if (number < 10)
                return "0" + number.ToString();
            else
                return number.ToString();
        }

        private string returnNumber(string str)
        {
            if (str.Equals("0"))
                return "";
            else
                return FormatString.ForMatNumber(str);
        }
    }
}