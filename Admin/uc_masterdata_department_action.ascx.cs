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
    public partial class uc_masterdata_department_action : System.Web.UI.UserControl
    {
        public string id;
        public string language = "1";

        public uc_masterdata_department_action()
        {
            this.id = "";
        }

        public uc_masterdata_department_action(string id)
        {
            this.id = id;
        }

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
                txtMa.Text = "";
                txtTen.Text = "";
                chkTrangThai.Checked = true;
            }

            if (this.id.Trim().Length > 0)
            {
                string query = "SELECT ma, ten, trangthai " +
                " FROM PhongBan WHERE pk_seq = '" + this.id + "' ";
                DataTable dt = xl.ReadTable(query);

                if (dt.Rows.Count > 0)
                {
                    txtMa.Text = dt.Rows[0]["ma"].ToString();
                    txtTen.Text = dt.Rows[0]["ten"].ToString();

                    if (dt.Rows[0]["trangthai"].ToString().Equals("1"))
                        chkTrangThai.Checked = true;
                }

            }
            this.loadJobList();
            this.loadStaffList();
        }

        protected void loadJobList()
        {
            string query = "";
            string str = "";
            ExecuteData xl = new ExecuteData();

            if (this.id.Trim().Length > 3)
            {
                query =
                    "SELECT '1' AS STT, a.pk_seq AS sanpham_fk, a.ma AS masp, a.ten AS tensp, a.dvt_fk, a.cycletime, a.giaban, 1 AS chon, " +
                    " ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = a.dvt_fk), '') donvi " +
                    " FROM SanPham a INNER JOIN PhongBan_SanPham b ON a.pk_seq = b.sanpham_fk AND b.phongban_fk = '" + this.id + "' " +
                    " WHERE a.trangThai = '1' " +
                    " UNION ALL " + 
                    "SELECT '2' AS STT, a.pk_seq AS sanpham_fk, a.ma AS masp, a.ten AS tensp, a.dvt_fk, a.cycletime, a.giaban, 0 AS chon, " +
                    " ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = a.dvt_fk), '') donvi " +
                    " FROM SanPham a  " +
                    " WHERE a.trangThai = '1' AND a.pk_seq NOT IN (SELECT sanpham_fk FROM PhongBan_SanPham WHERE phongban_fk = '" + this.id + "') " +
                    " ORDER BY STT, masp ASC ";
                DataTable dt = xl.ReadTable(query);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str += " <tr> ";
                        str += " <td style='text-align:center;'>" + (i + 1).ToString() + "</td>";
                        str += " <td style='text-align:center;'>" + dt.Rows[i]["masp"].ToString() + "</td>";
                        str += " <td>" + dt.Rows[i]["tensp"].ToString() + "</td>";
                        str += " <td style='text-align:center;'>" + dt.Rows[i]["donvi"].ToString() + "</td>";
                        str += " <td style='text-align:right;'>" + dt.Rows[i]["cycletime"].ToString() + "</td>";
                        str += " <td style='text-align:right;'>" + FormatString.ForMatNumber(dt.Rows[i]["giaban"].ToString()) + "</td>";

                        if (dt.Rows[i]["chon"].ToString().Equals("1"))
                        {
                            str += " <td style='text-align:center;' > <input type='checkbox' name='sanpham_fk'  value='" + dt.Rows[i]["sanpham_fk"].ToString() + "'  checked />  </td> ";
                        }
                        else
                        {
                            str += " <td style='text-align:center;' > <input type='checkbox' name='sanpham_fk'  value='" + dt.Rows[i]["sanpham_fk"].ToString() + "'  />  </td> ";
                        }

                        str += " </tr> ";
                    }

                    ltSanPham.Text = str;
                }
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
            "	SELECT '1' AS STT, a.pk_seq AS nhansu_fk, a.ma, a.ten AS nhanvien, a.capbac, 1 AS chon, " +
            "		ISNULL((SELECT ten FROM NhaPhanPhoi WHERE pk_seq = a.chinhanh_fk), '') chinhanh, " +
            "		ISNULL((SELECT ten FROM ChucVu WHERE pk_seq = a.chucvu_fk), '') chucvu, " +
            "		ISNULL((SELECT ten FROM PhongBan WHERE pk_seq = a.phongban_fk), '') phongban " +
            "	FROM DanhSachNhanSu a INNER JOIN PhongBan_NhanSu b ON a.pk_seq = b.nhansu_fk AND b.phongban_fk = '" + this.id + "' " + 
            " WHERE trangthai in (1) " +
            " UNION ALL " +
            "	SELECT '2' AS STT, a.pk_seq AS nhansu_fk, a.ma, a.ten AS nhanvien, a.capbac, 0 AS chon, " +
            "		ISNULL((SELECT ten FROM NhaPhanPhoi WHERE pk_seq = a.chinhanh_fk), '') chinhanh, " +
            "		ISNULL((SELECT ten FROM ChucVu WHERE pk_seq = a.chucvu_fk), '') chucvu, " +
            "		ISNULL((SELECT ten FROM PhongBan WHERE pk_seq = a.phongban_fk), '') phongban " +
            "	FROM DanhSachNhanSu a " +
            " WHERE trangthai in (1) AND a.pk_seq NOT IN (SELECT nhansu_fk FROM PhongBan_NhanSu WHERE phongban_fk = '" + this.id + "') " +
            ") B ORDER BY B.phongban, B.capbac, B.nhanvien ";
            DataTable dt = xl.ReadTable(query);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str += " <tr> ";                    
                    str += " <td style='text-align:center;'>" + (i + 1).ToString() + "</td>";

                    str += " <td style='text-align:center;'>" + dt.Rows[i]["chinhanh"].ToString() + "</td>";
                    str += " <td>" + dt.Rows[i]["chucvu"].ToString() + "</td>";
                    str += " <td>" + dt.Rows[i]["phongban"].ToString() + "</td>";
                    str += " <td style='text-align:center;'>" + dt.Rows[i]["ma"].ToString() + "</td>";
                    str += " <td>" + dt.Rows[i]["nhanvien"].ToString() + "</td>";

                    if (dt.Rows[i]["chon"].ToString().Equals("1"))
                    {
                        str += " <td style='text-align:center;' > <input type='checkbox' name='nhansu_fk'  value='" + dt.Rows[i]["nhansu_fk"].ToString() + "'  checked />  </td> ";
                    }
                    else
                    {
                        str += " <td style='text-align:center;' > <input type='checkbox' name='nhansu_fk'  value='" + dt.Rows[i]["nhansu_fk"].ToString() + "'  />  </td> ";
                    }

                    str += " </tr> ";
                }

                ltStaff.Text = str;
            }
        }

        protected void lbLuuLai_Click(object sender, EventArgs e)
        {
            string phongban_fk = this.id;
            string sanpham_fk = Request.Form["sanpham_fk"].ToString();
            string nhansu_fk = Request.Form["nhansu_fk"].ToString();
            string userId = Session["userId"].ToString();

            MasterDataModel masterData = new MasterDataModel();
            string msg = masterData.CREATE_Department(phongban_fk, sanpham_fk, nhansu_fk, userId);
            if(msg.Length < 10)
            {
                Response.Redirect("MasterData.aspx?func=7");
            }
            else
            {
                lblError.Text = msg;
            } 
                
        }
    }
}