using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLVTimeSheet.AcsessData;
using System.Data;

namespace HLVTimeSheet.Admin.UserControls
{
    public partial class ucTopMenu : System.Web.UI.UserControl
    {
        public string action = "";
        public string func = "";
        public string language = "1";
        public string path = "";
        public int stt;

        protected void Page_Load(object sender, EventArgs e)
        {
            path = HttpContext.Current.Request.Url.AbsoluteUri;
            path = path.Replace("&lang=1", "");
            path = path.Replace("&lang=2", "");

            if (path.Contains("Admin/Index.aspx") || path.Contains("Admin/Homepage.aspx") 
                || path.Contains("Admin/DiagramWarehouse.aspx") || path.Contains("Admin/DiagramStockIn.aspx")
                || path.Contains("Admin/DiagramStockOut.aspx") || path.Contains("Admin/StockOutPlan.aspx")
                || path.Contains("Admin/GeneralPlan.aspx") || path.Contains("Admin/DiagramDelivery.aspx"))
                path = path + "?";

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
                if (Session["userId"] == null || Session["userId"].ToString().Trim().Length <= 0)
                {
                    Response.Redirect("~/Admin/Login.aspx");
                    return;
                }

                ExecuteData xl = new ExecuteData();

                //ACTION LUU GROUP ACTIVE

                //LAY GROUP HIEN TAI
                string query = "SELECT pk_seq, ten_group AS nameEnglish, ten_top_menu AS tenTiengViet, ICON_LEFT FROM Group_ChucNang WHERE hienthi = '1' AND hienthi_LEFTMENU = '1' AND LEVEL in (0, 3) ";

                query += " AND pk_seq in (SELECT distinct b.group_fk FROM NhomQuyen_ChucNang_ChiTiet a INNER JOIN ChucNang b on a.chucnang_fk = b.pk_seq " +
                         " WHERE xem = '1' AND nhomquyen_fk in ( SELECT nhomquyen_fk FROM NhanVien_Quyen_NhomQuyen WHERE nhanvien_fk = '" + Session["userId"].ToString() + "' )) ";

                query += " ORDER BY STT ASC ";
                DataTable dtGROUP = xl.ReadTable(query);

                string topmenu = "";
                for (int i = 0; i < dtGROUP.Rows.Count; i++)
                {
                    
                    if (dtGROUP.Rows[i]["pk_seq"].ToString().Equals("31"))
                    {
                        if (language.Equals("1")) // Tiếng Anh
                        {
                            topmenu += "<li>  " +
                                   "<a href='TimeSheet.aspx'>" + dtGROUP.Rows[i]["nameEnglish"].ToString() + "</a> " +
                                   "<ul> ";
                        }
                        else if (language.Equals("2"))  // Tiếng Việt
                        {
                            topmenu += "<li>  " +
                                  "<a href='TimeSheet.aspx'>" + dtGROUP.Rows[i]["tenTiengViet"].ToString() + "</a> " +
                                  "<ul> ";
                        }
                    }
                    else if (dtGROUP.Rows[i]["pk_seq"].ToString().Equals("33"))
                    {
                        if (language.Equals("1")) // Tiếng Anh
                        {
                            topmenu += "<li>  " +
                                   "<a href='Calendar.aspx'>" + dtGROUP.Rows[i]["nameEnglish"].ToString() + "</a> " +
                                   "<ul> ";
                        }
                        else if (language.Equals("2"))  // Tiếng Việt
                        {
                            topmenu += "<li>  " +
                                  "<a href='Calendar.aspx'>" + dtGROUP.Rows[i]["tenTiengViet"].ToString() + "</a> " +
                                  "<ul> ";
                        }
                    }
                    else if (dtGROUP.Rows[i]["pk_seq"].ToString().Equals("35"))
                    {
                        if (language.Equals("1")) // Tiếng Anh
                        {
                            topmenu += "<li>  " +
                                   "<a href='Salary.aspx'>" + dtGROUP.Rows[i]["nameEnglish"].ToString() + "</a> " +
                                   "<ul> ";
                        }
                        else if (language.Equals("2"))  // Tiếng Việt
                        {
                            topmenu += "<li>  " +
                                  "<a href='Salary.aspx'>" + dtGROUP.Rows[i]["tenTiengViet"].ToString() + "</a> " +
                                  "<ul> ";
                        }
                    }
                    else if (dtGROUP.Rows[i]["pk_seq"].ToString().Equals("10"))
                    {
                        if (language.Equals("1")) // Tiếng Anh
                        {
                            topmenu += "<li>  " +
                                   "<a href='Report.aspx'>" + dtGROUP.Rows[i]["nameEnglish"].ToString() + "</a> " +
                                   "<ul> ";
                        }
                        else if (language.Equals("2"))  // Tiếng Việt
                        {
                            topmenu += "<li>  " +
                                  "<a href='Report.aspx'>" + dtGROUP.Rows[i]["tenTiengViet"].ToString() + "</a> " +
                                  "<ul> ";
                        }
                    }
                    else
                    {
                        if (language.Equals("1")) // Tiếng Anh
                        {
                            topmenu += "<li class='dropdown'>  " +
                                  "<a href='#' class='dropdown-toggle' data-toggle='dropdown'>" + dtGROUP.Rows[i]["nameEnglish"].ToString() + " <span class='caret'></span></a> " +
                                  "<ul class='dropdown-menu' role='menu'> ";
                        }
                        else if (language.Equals("2"))  // Tiếng Việt
                        {
                            topmenu += "<li class='dropdown'>  " +
                                   "<a href='#' class='dropdown-toggle' data-toggle='dropdown'>" + dtGROUP.Rows[i]["tenTiengViet"].ToString() + " <span class='caret'></span></a> " +
                                   "<ul class='dropdown-menu' role='menu'> ";
                        }

                        query = " SELECT pk_seq, nameEnglish, ten AS tenTiengViet, PAGE_URL, hienduongPhancach FROM ChucNang WHERE group_fk = '" + dtGROUP.Rows[i]["pk_seq"].ToString() + "' AND hienthi = '1' AND LEVEL in (0, 3) ";
                        query += " AND pk_seq in ( SELECT chucnang_fk FROM NhomQuyen_ChucNang_ChiTiet WHERE xem = '1' AND nhomquyen_fk in ( SELECT nhomquyen_fk FROM NhanVien_Quyen_NhomQuyen WHERE nhanvien_fk = '" + Session["userId"].ToString() + "' ) ) ";
                        query += " ORDER BY stt asc ";

                        DataTable dt = xl.ReadTable(query);
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (language.Equals("1")) // Tiếng Anh
                            {
                                topmenu += "    <li><a href='" + dt.Rows[j]["PAGE_URL"].ToString() + "?func=" + dt.Rows[j]["pk_seq"].ToString() + "'>" + dt.Rows[j]["nameEnglish"].ToString() + "</a></li> ";
                                if (dt.Rows[j]["hienduongPhancach"].ToString().Equals("1"))
                                    topmenu += " <li class='divider'></li> ";
                            }
                            else if (language.Equals("2")) // Tiếng Việt
                            {
                                topmenu += "    <li><a href='" + dt.Rows[j]["PAGE_URL"].ToString() + "?func=" + dt.Rows[j]["pk_seq"].ToString() + "'>" + dt.Rows[j]["tenTiengViet"].ToString() + "</a></li> ";
                                if (dt.Rows[j]["hienduongPhancach"].ToString().Equals("1"))
                                    topmenu += " <li class='divider'></li> ";
                            }
                        }
                        dt.Clone();
                    }

                    topmenu += "    </ul> " +
                        "</li> ";

                }
                dtGROUP.Clone();

                ltTopmenu.Text = topmenu;

            }
        }
    }
}