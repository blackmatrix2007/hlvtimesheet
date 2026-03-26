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
    public partial class uc_administrator_staff_action : System.Web.UI.UserControl
    {
        public string id;
        public string language = "1";
        public string token = "";

        public uc_administrator_staff_action()
        {
            this.id = "";
        }

        public uc_administrator_staff_action(string id)
        {
            this.id = id;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ExecuteData xl = new ExecuteData("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@");
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

            string query = "";
            if (!Page.IsPostBack)
            {
                query = "SELECT pk_seq, ma, ten FROM NhaPhanPhoi WHERE trangthai = '1' " +
                    " ORDER BY ma ";
                DataTable dt = xl.ReadTable(query);

                if (language.Equals("1"))
                {
                    ddlChiNhanh.Items.Add(new ListItem("Choose branch", "0"));
                }
                else if (language.Equals("2"))
                {
                    ddlChiNhanh.Items.Add(new ListItem("Chọn chi nhánh", "0"));
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlChiNhanh.Items.Add(new ListItem(dt.Rows[i]["ma"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                query = "SELECT pk_seq, '[ ' + ma + ' ] ' + ten AS ten FROM PhongBan WHERE trangthai = 1 ORDER BY ma ";
                dt = xl.ReadTable(query);
                ddlPhongBan.Items.Add(new ListItem("", "0"));
                ddlPhongBanSupport.Items.Add(new ListItem("", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlPhongBan.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                    ddlPhongBanSupport.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                query = "SELECT pk_seq, ten FROM ChucVu WHERE trangthai = 1 ORDER BY ten ";
                dt = xl.ReadTable(query);
                ddlChucVu.Items.Add(new ListItem("", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlChucVu.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                query = "SELECT pk_seq, dangnhap AS ten FROM NhanVien WHERE trangthai = 1 ORDER BY dangnhap ";
                dt = xl.ReadTable(query);
                ddlTaiKhoan.Items.Add(new ListItem("", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlTaiKhoan.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                if (language.Equals("1"))
                {
                    ddlGioiTinh.Items.Add(new ListItem("Choose gender", "0"));
                    ddlGioiTinh.Items.Add(new ListItem("Male", "1"));
                    ddlGioiTinh.Items.Add(new ListItem("Female", "2"));
                    ddlGioiTinh.Items.Add(new ListItem("Unknown", "3"));

                    ddlTrangThai.Items.Add(new ListItem("On", "1"));
                    ddlTrangThai.Items.Add(new ListItem("Off", "0"));
                    ddlTrangThai.Items.Add(new ListItem("Holiday", "2"));
                    ddlTrangThai.Items.Add(new ListItem("Support", "3"));

                    ddlHienThi.Items.Add(new ListItem("Show", "1"));
                    ddlHienThi.Items.Add(new ListItem("None", "0"));
                }
                else if (language.Equals("2"))
                {
                    ddlGioiTinh.Items.Add(new ListItem("Chọn giới tính", "0"));
                    ddlGioiTinh.Items.Add(new ListItem("Nam", "1"));
                    ddlGioiTinh.Items.Add(new ListItem("Nữ", "2"));
                    ddlGioiTinh.Items.Add(new ListItem("Không xác định", "3"));

                    ddlTrangThai.Items.Add(new ListItem("Hoạt động", "1"));
                    ddlTrangThai.Items.Add(new ListItem("Không hoạt động", "0"));
                    ddlTrangThai.Items.Add(new ListItem("Nghỉ làm", "2"));
                    ddlTrangThai.Items.Add(new ListItem("Hỗ trợ", "3"));

                    ddlHienThi.Items.Add(new ListItem("Hiển thị", "1"));
                    ddlHienThi.Items.Add(new ListItem("Không", "0"));
                }

                divHinhUpLoad.InnerHtml = "<img src='/Admin/Avatar/avatardefault.png' style='max-height:300px; max-width:200px' />";

                dt.Clear();
                dt.Clone();
            }

            if (this.id.Trim().Length > 3)
            {
                query = " SELECT pk_seq, ma, ten, tenhienthi, trangthai, diachi, dienthoai, gioitinh, mail, ngaysinh, trangthai, chinhanh_fk, phongban_fk, phongbanSupport_fk, ISNULL(a.ngaybatdaulam, '') ngaybatdaulam, " +
                    "   chucvu_fk, nhanvien_fk, ISNULL(hinhanh, 'avatardefault.png') hinhanh, ISNULL(a.tungay, '') tungay, ISNULL(a.denngay, '') denngay, ISNULL(a.lydo, '') lydo, " +
                    "   ISNULL((SELECT capbac FROM ChucVu WHERE pk_seq = a.chucvu_fk), 0) capbac, hienthi, " +
                    "   ISNULL((SELECT dangnhap FROM NhanVien WHERE pk_seq = a.nhanvien_fk), '') dangnhap, " +
                    "   ISNULL((SELECT token FROM NhanVien WHERE pk_seq = a.nhanvien_fk), '') token " +
                    " FROM DanhSachNhanSu a WHERE a.pk_seq = '" + id + "' ";
                DataTable dt = xl.ReadTable(query);

                if (dt.Rows.Count > 0)
                {
                    txtMa.Value = dt.Rows[0]["ma"].ToString();
                    txtTen.Text = dt.Rows[0]["ten"].ToString();
                    txtTenHienThi.Text = dt.Rows[0]["tenhienthi"].ToString();
                    txtDiaChi.Text = dt.Rows[0]["diachi"].ToString();
                    txtMail.Text = dt.Rows[0]["mail"].ToString();
                    txtNgaySinh.Text = dt.Rows[0]["ngaysinh"].ToString();
                    txtNgayBatDauLam.Text = dt.Rows[0]["ngaybatdaulam"].ToString();
                    txtDienThoai.Value = dt.Rows[0]["dienthoai"].ToString();
                    txtDangNhap.Value = dt.Rows[0]["dangnhap"].ToString();
                    txtTuNgay.Text = dt.Rows[0]["tungay"].ToString();
                    txtDenNgay.Text = dt.Rows[0]["denngay"].ToString();
                    txtLyDo.Text = dt.Rows[0]["lydo"].ToString();

                    ddlHienThi.SelectedValue = dt.Rows[0]["hienthi"].ToString();
                    ddlGioiTinh.SelectedValue = dt.Rows[0]["gioitinh"].ToString();
                    ddlTrangThai.SelectedValue = dt.Rows[0]["trangthai"].ToString();

                    ddlTaiKhoan.SelectedValue = dt.Rows[0]["nhanvien_fk"].ToString();
                    ddlChucVu.SelectedValue = dt.Rows[0]["chucvu_fk"].ToString();
                    ddlChiNhanh.SelectedValue = dt.Rows[0]["chinhanh_fk"].ToString();
                    ddlPhongBan.SelectedValue = dt.Rows[0]["phongban_fk"].ToString();
                    
                    txtCapBac.Value = dt.Rows[0]["capbac"].ToString();

                    this.token = dt.Rows[0]["token"].ToString();
                    txtToken.Value = dt.Rows[0]["token"].ToString();

                    divHinhUpLoad.InnerHtml = "<img src='/Admin/Avatar/" + dt.Rows[0]["hinhanh"].ToString() + "' style='max-height:150px; max-width:100px' />";
                    txtHinhAnh.Value = dt.Rows[0]["hinhanh"].ToString();

                    if(ddlTrangThai.SelectedValue.Trim().Equals("3"))
                        ddlPhongBanSupport.Value = dt.Rows[0]["phongbanSupport_fk"].ToString();

                    //txtTuNgay.Enabled = false;
                    //txtDenNgay.Enabled = false;
                    //txtLyDo.Enabled = false;
                    //if (ddlTrangThai.SelectedValue.Trim().Equals("2") || ddlTrangThai.SelectedValue.Trim().Equals("3"))
                    //{
                    //    txtTuNgay.Enabled = true;
                    //    txtDenNgay.Enabled = true;
                    //    txtLyDo.Enabled = true;
                    //}

                }
            }
            else
            {
                txtMa.Value = "";
                txtDangNhap.Value = "";
                txtTen.Text = "";
                txtTenHienThi.Text = "";
                txtDiaChi.Text = "";
                txtMail.Text = "";
                txtNgaySinh.Text = "";
                txtNgayBatDauLam.Text = "";
                txtTuNgay.Text = "";
                txtDenNgay.Text = "";
                txtLyDo.Text = "";
                txtDienThoai.Value = "";
                txtCapBac.Value = "0";
                ddlPhongBanSupport.Value = "0";
                divHinhUpLoad.InnerHtml = "<img src='/Admin/Avatar/avatardefault.png' style='max-height:150px; max-width:100px' />";
                //txtHinhAnh.Value = "avatardefault.png";
            }

            // Load
            this.loadInforDepartment(xl);
            this.loadInforStaff(xl);
            this.loadRolePermission(xl);

        }

        protected void lbLuuLai_Click(object sender, EventArgs e)
        {
            if (txtMa.Value.Trim().Length <= 0)
            {
                lblError.Text = "Vui lòng nhập ma ";
                return;
            }

            if (txtTen.Text.Trim().Length <= 0)
            {
                lblError.Text = "Vui lòng nhập tên nhân viên";
                return;
            }

            string msg = "";
            StaffController staffController = new StaffController();

            string phongbanIds = "";
            if (Request.Form["phongbanIds"] != null)
                phongbanIds = Request.Form["phongbanIds"].ToString();

            string nhansuIds = "";
            if (Request.Form["nhansuIds"] != null)
                nhansuIds = Request.Form["nhansuIds"].ToString();

            string nhomquyenIds = "";
            if (Request.Form["nhomquyenIds"] != null)
                nhomquyenIds = Request.Form["nhomquyenIds"].ToString();

            if(txtToken.Value.Length < 10)
                token = this.MaSo() + this.MaChu() + DateTime.Now.ToString("ddMMyyyyhhmmss");

            if (!ddlTrangThai.SelectedValue.Trim().Equals("3"))
                ddlPhongBanSupport.Value = ddlPhongBan.SelectedValue;

            string tungay = txtTuNgay.Text;
            if (this.id.Trim().Length < 3)
            {                
                msg = staffController.INSERT_Staff(txtMa.Value, txtTen.Text, txtTenHienThi.Text, ddlChiNhanh.SelectedValue, ddlPhongBan.SelectedValue, ddlPhongBanSupport.Value, ddlChucVu.SelectedValue, ddlTaiKhoan.SelectedValue, txtDienThoai.Value, txtMail.Text, txtDiaChi.Text, txtNgaySinh.Text, txtNgayBatDauLam.Text, ddlGioiTinh.SelectedValue, txtHinhAnh.Value, ddlTrangThai.SelectedValue, txtTuNgay.Text, txtDenNgay.Text, txtLyDo.Text, ddlHienThi.SelectedValue, txtDangNhap.Value, txtMatkhau.Value, token, phongbanIds, nhansuIds, nhomquyenIds, Session["userId"].ToString());
            }
            else
            {                
                msg = staffController.UPDATE_Staff(id, txtMa.Value, txtTen.Text, txtTenHienThi.Text, ddlChiNhanh.SelectedValue, ddlPhongBan.SelectedValue, ddlPhongBanSupport.Value, ddlChucVu.SelectedValue, ddlTaiKhoan.SelectedValue, txtDienThoai.Value, txtMail.Text, txtDiaChi.Text, txtNgaySinh.Text, txtNgayBatDauLam.Text, ddlGioiTinh.SelectedValue, txtHinhAnh.Value, ddlTrangThai.SelectedValue, txtTuNgay.Text, txtDenNgay.Text, txtLyDo.Text, ddlHienThi.SelectedValue, txtDangNhap.Value, txtMatkhau.Value, token, phongbanIds, nhansuIds, nhomquyenIds, Session["userId"].ToString());
            }

            if (msg.Trim().Length <= 10)
            {
                lblError.Text = "";
                Response.Redirect("Administrator.aspx?func=95");
            }
            else
            {
                lblError.Text = msg;
            }
        }

        protected void ddlChucVu_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCapBac.Value = "0";

            ExecuteData xl = new ExecuteData("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@");
            string sql = "SELECT ISNULL(capbac, 0) capbac FROM ChucVu WHERE pk_seq = '" + ddlChucVu.SelectedValue + "' ";
            object obj = xl.ExecuteScalarSQL(sql);
            if (obj != null)
                txtCapBac.Value = obj.ToString();

            this.loadInforStaff(xl);
            this.loadInforDepartment(xl);
        }

        protected void ddlTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtTuNgay.Enabled = false;
            //txtDenNgay.Enabled = false;
            //txtLyDo.Enabled = false;
            //txtTuNgay.Text = "";
            //txtDenNgay.Text = "";
            //txtLyDo.Text = "";
            //if (ddlTrangThai.SelectedValue.Trim().Equals("2") || ddlTrangThai.SelectedValue.Trim().Equals("3"))
            //{
            //    txtTuNgay.Enabled = true;
            //    txtDenNgay.Enabled = true;
            //    txtLyDo.Enabled = true;
            //}
        }

        private void loadInforStaff(ExecuteData xl)
        {
            if (xl == null)
                xl = new ExecuteData("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@");

            int capbac = 0;
            if (txtCapBac.Value.Length > 0)
                capbac = int.Parse(txtCapBac.Value);

            string condition = "";
            if (capbac > 1)
            {
                condition += " AND a.chinhanh_fk = " + ddlChiNhanh.SelectedValue + " ";
            }
            
            condition += " AND cv.capbac > " + capbac;

            if (id.Length < 3)
                id = "0";

            string content = "";
            string sql = " SELECT a.pk_seq, a.ma, a.ten, cv.ten AS chucvu, ISNULL((SELECT ten FROM PhongBan WHERE pk_seq = a.phongban_fk), '') phongban, a.capbac, 1 AS chon " +
            " FROM DanhSachNhanSu a INNER JOIN ChucVu cv ON a.chucvu_fk = cv.pk_seq " +
            " WHERE a.trangthai in (1, 2, 3) AND a.pk_seq IN (SELECT nhansu_fk FROM DanhSachNhanSu_QuanLy WHERE quanly_fk = " + this.id + ") " +
            " UNION ALL " +
            " SELECT a.pk_seq, a.ma, a.ten, cv.ten AS chucvu, ISNULL((SELECT ten FROM PhongBan WHERE pk_seq = a.phongban_fk), '') phongban, a.capbac, 0 AS chon " +
            " FROM DanhSachNhanSu a INNER JOIN ChucVu cv ON a.chucvu_fk = cv.pk_seq AND a.pk_seq NOT IN (SELECT nhansu_fk FROM DanhSachNhanSu_QuanLy)  " + condition + 
            " WHERE a.trangthai in (1, 2, 3) " +
            " ORDER BY chon DESC, phongban, capbac, chucvu, ma ";
            DataTable dt = xl.ReadTable(sql);
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                content += " <tr> " +
                    " <td>" + (i + 1).ToString() + "</td> " +
                    " <td>" + dt.Rows[i]["ma"].ToString() + "</td> " +
                    " <td>" + dt.Rows[i]["ten"].ToString() + "</td> " +
                    " <td>" + dt.Rows[i]["phongban"].ToString() + "</td> " +
                    " <td>" + dt.Rows[i]["chucvu"].ToString() + "</td> ";                    
                if (dt.Rows[i]["chon"].ToString().Equals("1"))
                {
                    content += " <td style='text-align:center;' > <input type='checkbox' name='nhansuIds'  value='" + dt.Rows[i]["pk_seq"].ToString() + "'  checked />  </td> ";
                }
                else
                {
                    content += " <td style='text-align:center;' > <input type='checkbox' name='nhansuIds'  value='" + dt.Rows[i]["pk_seq"].ToString() + "'  />  </td> ";
                }

                content += " </tr> ";
            }
            dt.Clear();
            dt.Clone();

            ltNhanSu.Text = content;
        }

        private void loadInforDepartment(ExecuteData xl)
        {
            if (xl == null)
                xl = new ExecuteData("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@");

            int capbac = 0;
            if (txtCapBac.Value.Length > 0)
                capbac = int.Parse(txtCapBac.Value);

            if (id.Length < 3)
                id = "0";

            string content = "";
            if (capbac <= 6)
            {
                string sql = " SELECT a.pk_seq, a.ma, a.ten, 1 AS chon " +
                " FROM PhongBan a INNER JOIN DanhSachNhanSu_PhongBan b ON a.pk_seq = b.phongban_fk AND b.nhansu_fk = " + this.id + " " +
                " WHERE a.trangthai in (1) " +
                " UNION ALL " +
                " SELECT a.pk_seq, a.ma, a.ten, 0 AS chon " +
                " FROM PhongBan a  " +
                " WHERE a.trangthai in (1) AND a.pk_seq NOT IN (SELECT phongban_fk FROM DanhSachNhanSu_PhongBan WHERE nhansu_fk = " + this.id + ") " +
                " ORDER BY chon DESC, ma ";
                DataTable dt = xl.ReadTable(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    content += " <tr> " +
                        " <td>" + (i + 1).ToString() + "</td> " +
                        " <td>" + dt.Rows[i]["ma"].ToString() + "</td> " +
                        " <td>" + dt.Rows[i]["ten"].ToString() + "</td> ";
                    if (dt.Rows[i]["chon"].ToString().Equals("1"))
                    {
                        content += " <td style='text-align:center;' > <input type='checkbox' name='phongbanIds'  value='" + dt.Rows[i]["pk_seq"].ToString() + "'  checked />  </td> ";
                    }
                    else
                    {
                        content += " <td style='text-align:center;' > <input type='checkbox' name='phongbanIds'  value='" + dt.Rows[i]["pk_seq"].ToString() + "'  />  </td> ";
                    }

                    content += " </tr> ";
                }
                dt.Clear();
                dt.Clone();
            }
            ltData.Text = content;
        }

        private void loadRolePermission(ExecuteData xl)
        {
            string sql = "";
            if (ddlTaiKhoan.SelectedValue.Trim().Length < 3)
            {
                sql = "SELECT pk_seq, tenquyen AS ten, maquyen AS ma, 0 as chon  " +
                        "FROM NhomQuyen WHERE TrangThai = '1' ORDER BY ma asc ";
            }
            else
            {
                sql = "SELECT b.pk_seq, tenquyen AS ten, maquyen AS ma, 1 as chon  " +
                        "FROM NhanVien_Quyen_NhomQuyen a INNER JOIN NhomQuyen b on a.nhomquyen_fk = b.pk_seq  WHERE a.nhanvien_fk = '" + ddlTaiKhoan.SelectedValue + "'     " +
                        "UNION ALL      " +
                        "SELECT pk_seq, tenquyen AS ten, maquyen AS ma, 0 as chon     " +
                        "FROM NhomQuyen WHERE TrangThai = '1'  and pk_seq not in ( SELECT nhomquyen_fk FROM NhanVien_Quyen_NhomQuyen WHERE nhanvien_fk = '" + ddlTaiKhoan.SelectedValue + "'  ) ORDER BY ma desc ";
            }

            DataTable dtKh = xl.ReadTable(sql);

            if (dtKh.Rows.Count > 0)
            {
                string str = " ";

                for (int i = 0; i < dtKh.Rows.Count; i++)
                {
                    str += " <tr> ";
                    str += " <td >" + (i + 1).ToString() + " </td> ";
                    str += " <td >" + dtKh.Rows[i]["ma"].ToString() + " </td> ";
                    str += " <td >" + dtKh.Rows[i]["ten"].ToString() + " </td> ";

                    if (dtKh.Rows[i]["chon"].ToString().Equals("1"))
                    {
                        str += " <td style='text-align:center;' > <input type='checkbox' name='nhomquyenIds'  value='" + dtKh.Rows[i]["pk_seq"].ToString() + "'  checked />  </td> ";
                    }
                    else
                    {
                        str += " <td style='text-align:center;' > <input type='checkbox' name='nhomquyenIds'  value='" + dtKh.Rows[i]["pk_seq"].ToString() + "'  />  </td> ";
                    }
                    str += " </tr> ";
                }
                ltNhomQuyen.Text = str;
            }
        }

        private string MaSo()
        {
            string makichhoat = "";

            Random rd = new Random();
            while (makichhoat.Length < 6)
            {
                int kq = rd.Next(0, 9);
                makichhoat = makichhoat + kq.ToString();
            }

            return makichhoat;
        }

        private string MaChu()
        {
            string kytu = "";
            string[] array = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "V", "U", "X", "Y", "Z", "W",
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "v", "u", "x", "y", "z", "w"};
            Random rd = new Random();
            while (kytu.Length < 12)
            {
                int kq = rd.Next(0, 51);
                kytu += array[kq];
            }

            return kytu;
        }
    }
}