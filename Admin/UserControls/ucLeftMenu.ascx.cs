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
    public partial class ucLeftMenu : System.Web.UI.UserControl
    {
        public string action = "";
        public string func = "";
        public string language = "1";

        public int stt;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null || Session["userId"].ToString().Trim().Length <= 0)
                {
                    Response.Redirect("~/Admin/Login.aspx");
                    return;
                }

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

                //ACTION LUU GROUP ACTIVE

                //LAY GROUP HIEN TAI
                string query = "SELECT pk_seq, ten_group AS nameEnglish, ten_top_menu AS tenTiengViet, ICON_LEFT, page_URL FROM Group_ChucNang WHERE HIENTHI = '1' and HIENTHI_LEFTMENU = '1' AND LEVEL in (0, 3) ";

                query += " and pk_seq in (SELECT distinct b.GROUP_FK FROM NhomQuyen_ChucNang_ChiTiet a INNER JOIN ChucNang b on a.chucnang_fk = b.pk_seq " +
                         " WHERE xem = '1' and nhomquyen_fk in ( SELECT nhomquyen_fk FROM NhanVien_Quyen_NhomQuyen WHERE nhanvien_fk = '" + Session["userId"].ToString() + "' ) ) ";

                query += " ORDER BY STT asc ";
                DataTable dtGROUP = xl.ReadTable(query);

                string leftmenu = "";
                for (int i = 0; i < dtGROUP.Rows.Count; i++)
                {
                    string _class = "";
                    if (action.Equals(dtGROUP.Rows[i]["pk_seq"].ToString()))
                        _class = "active";

                    if (language.Equals("1"))
                    {
                        if (dtGROUP.Rows[i]["pk_seq"].ToString().Equals("10"))
                        {
                            leftmenu += "<li class='treeview " + _class + " ' >  " +
                            "    <a href='" + dtGROUP.Rows[i]["page_URL"].ToString() + "'> " +
                            "    <i class='" + dtGROUP.Rows[i]["ICON_LEFT"].ToString() + "'></i> <span>" + dtGROUP.Rows[i]["nameEnglish"].ToString() + "</span> " +
                            "    </a> ";
                        }
                        else
                        {
                            leftmenu += "<li class='treeview " + _class + " ' >  " +
                            "    <a href='#'> " +
                            "    <i class='" + dtGROUP.Rows[i]["ICON_LEFT"].ToString() + "'></i> <span>" + dtGROUP.Rows[i]["nameEnglish"].ToString() + "</span> <i class='fa fa-angle-left pull-right'></i> " +
                            "    </a> " +
                            "    <ul class='treeview-menu'> ";

                            query = " SELECT pk_seq, nameEnglish, ten AS tenTiengViet, PAGE_URL, hienduongPhancach FROM ChucNang " +
                                    " WHERE GROUP_FK = '" + dtGROUP.Rows[i]["pk_seq"].ToString() + "' and HIENTHI = '1' and HIENTHI_LEFTMENU = '1' AND LEVEL in (0, 2) ";

                            query += " and pk_seq in ( SELECT chucnang_fk FROM NhomQuyen_ChucNang_ChiTiet WHERE xem = '1' and nhomquyen_fk in ( SELECT nhomquyen_fk FROM NhanVien_Quyen_NhomQuyen WHERE nhanvien_fk = '" + Session["userId"].ToString() + "' ) ) ";

                            query += " ORDER BY stt asc ";
                            DataTable dt = xl.ReadTable(query);
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                string _class2 = "";
                                if (func.Equals(dt.Rows[j]["pk_seq"].ToString()))
                                    _class2 = " class='active' ";

                                leftmenu += "		<li " + _class2 + " ><a href='" + dt.Rows[j]["PAGE_URL"].ToString() + "?func=" + dt.Rows[j]["pk_seq"].ToString() + "'><i class='fa fa-circle-o'></i> " + dt.Rows[j]["nameEnglish"].ToString() + "</a></li> ";

                                if (dt.Rows[j]["hienduongPhancach"].ToString().Equals("1"))
                                    leftmenu += " <li class='divider'></li> ";
                            }
                            dt.Clone();
                        }

                        leftmenu += "    </ul> " +
                                        "</li> ";
                    }
                    else if (language.Equals("2"))
                    {
                        if (dtGROUP.Rows[i]["pk_seq"].ToString().Equals("10"))
                        {
                            leftmenu += "<li class='treeview " + _class + " ' >  " +
                            "    <a href='" + dtGROUP.Rows[i]["page_URL"].ToString() + "'> " +
                            "    <i class='" + dtGROUP.Rows[i]["ICON_LEFT"].ToString() + "'></i> <span>" + dtGROUP.Rows[i]["tenTiengViet"].ToString() + "</span> " +
                            "    </a> ";
                        }
                        else
                        {
                            leftmenu += "<li class='treeview " + _class + " ' >  " +
                            "    <a href='#'> " +
                            "    <i class='" + dtGROUP.Rows[i]["ICON_LEFT"].ToString() + "'></i> <span>" + dtGROUP.Rows[i]["tenTiengViet"].ToString() + "</span> <i class='fa fa-angle-left pull-right'></i> " +
                            "    </a> " +
                            "    <ul class='treeview-menu'> ";

                            query = " SELECT pk_seq, nameEnglish, ten AS tenTiengViet, PAGE_URL, hienduongPhancach FROM ChucNang " +
                                    " WHERE GROUP_FK = '" + dtGROUP.Rows[i]["pk_seq"].ToString() + "' and HIENTHI = '1' and HIENTHI_LEFTMENU = '1' AND LEVEL in (0, 3) ";

                            query += " and pk_seq in ( SELECT chucnang_fk FROM NhomQuyen_ChucNang_ChiTiet WHERE xem = '1' and nhomquyen_fk in ( SELECT nhomquyen_fk FROM NhanVien_Quyen_NhomQuyen WHERE nhanvien_fk = '" + Session["userId"].ToString() + "' ) ) ";

                            query += " ORDER BY stt asc ";
                            DataTable dt = xl.ReadTable(query);
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                string _class2 = "";
                                if (func.Equals(dt.Rows[j]["pk_seq"].ToString()))
                                    _class2 = " class='active' ";

                                leftmenu += "		<li " + _class2 + " ><a href='" + dt.Rows[j]["PAGE_URL"].ToString() + "?func=" + dt.Rows[j]["pk_seq"].ToString() + "'><i class='fa fa-circle-o'></i> " + dt.Rows[j]["tenTiengViet"].ToString() + "</a></li> ";

                                if (dt.Rows[j]["hienduongPhancach"].ToString().Equals("1"))
                                    leftmenu += " <li class='divider'></li> ";
                            }
                            dt.Clone();
                        }

                        leftmenu += "    </ul> " +
                                        "</li> ";
                    }
                }
                dtGROUP.Clone();

                if (language.Equals("1"))
                    leftmenu += "<li class='treeview'><a href = 'Login.aspx'><i class='fa fa-sign-out'></i> <span>Logout</span></a></li>";
                else if (language.Equals("2"))
                    leftmenu += "<li class='treeview'><a href = 'Login.aspx'><i class='fa fa-sign-out'></i> <span>Đăng xuất</span></a></li>";

                ltLeftmenu.Text = leftmenu;

            }
        }
    }
}