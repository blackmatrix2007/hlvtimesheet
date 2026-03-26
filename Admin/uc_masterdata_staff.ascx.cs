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
    public partial class uc_masterdata_staff : System.Web.UI.UserControl
    {

        public string language = "1";
      
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

            if (!IsPostBack)
            {
                txtNgayNhap.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtNgayNhap.Enabled = false;

                this.loadStaffList();
            }
        }

        protected void loadStaffList()
        {
            string query = "";
            string str = "";
            ExecuteData xl = new ExecuteData();
            
            query = "SELECT * " +
            "FROM " +
            "( " +
            "	SELECT a.pk_seq AS nhansu_fk, a.ma, a.ten AS nhanvien, a.capbac, " +
            "		ISNULL((SELECT ten FROM NhaPhanPhoi WHERE pk_seq = a.chinhanh_fk), '') chinhanh, " +
            "		ISNULL((SELECT ten FROM ChucVu WHERE pk_seq = a.chucvu_fk), '') chucvu, " +
            "		ISNULL((SELECT ten FROM PhongBan WHERE pk_seq = a.phongban_fk), '') phongban, " +
            "      a.result, a.normalOver, a.night, a.nightOver, a.holiday " + 
            "	FROM DanhSachNhanSu a WHERE trangthai in (1) " +
            ") B ORDER BY B.phongban, B.capbac, B.nhanvien ";
            DataTable dt = xl.ReadTable(query);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {          
                    str += " <tr> ";
                    str += " <td style='display:none;'><input type='hidden' class='form-control' name='nhansu_fk' value='" + dt.Rows[i]["nhansu_fk"].ToString() + "' ></td> ";

                    str += " <td style='text-align:center;'>" + (i + 1).ToString() + "</td>";

                    str += " <td style='text-align:center;'>" + dt.Rows[i]["chinhanh"].ToString() + "</td>";
                    str += " <td>" + dt.Rows[i]["chucvu"].ToString() + "</td>";
                    str += " <td>" + dt.Rows[i]["phongban"].ToString() + "</td>";
                    str += " <td style='text-align:center;'>" + dt.Rows[i]["ma"].ToString() + "</td>";
                    str += " <td>" + dt.Rows[i]["nhanvien"].ToString() + "</td>";

                    str += " <td><input type='text' class='form-control' name='result' style='text-align:right; height:20px; font-size:small;' value='" + FormatString.ForMatNumber(dt.Rows[i]["result"].ToString()) + "'  ></td> ";
                    str += " <td><input type='text' class='form-control' name='normalOver' style='text-align:right; height:20px; font-size:small;' value='" + FormatString.ForMatNumber(dt.Rows[i]["normalOver"].ToString()) + "'  ></td> ";
                    str += " <td><input type='text' class='form-control' name='night' style='text-align:right; height:20px; font-size:small;' value='" + FormatString.ForMatNumber(dt.Rows[i]["night"].ToString()) + "'  ></td> ";
                    str += " <td><input type='text' class='form-control' name='nightOver' style='text-align:right; height:20px; font-size:small;' value='" + FormatString.ForMatNumber(dt.Rows[i]["nightOver"].ToString()) + "'  ></td> ";
                    str += " <td><input type='text' class='form-control' name='holiday' style='text-align:right; height:20px; font-size:small;' value='" + FormatString.ForMatNumber(dt.Rows[i]["holiday"].ToString()) + "'  ></td> ";
                    str += " </tr> ";
                }

                ltSanPham.Text = str;
            }
        }       
    }
}