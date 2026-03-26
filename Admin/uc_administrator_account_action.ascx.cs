using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLVTimeSheet.AcsessData;
using System.Data;
using HLVTimeSheet.Model;

namespace HLVTimeSheet.Admin
{
    public partial class uc_administrator_account_action : System.Web.UI.UserControl
    {
        public string id;
        public string trangthai;
        public string language = "1";
        public string token = "";
        public string roleAdmin="";
        public uc_administrator_account_action()
        {
            this.id = "";
            this.trangthai = "0";
        }

        public uc_administrator_account_action(string id)
        {
            this.id = id;
            this.trangthai = "0";
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

            ExecuteData xl = new ExecuteData();

            if (!IsPostBack)
            {
                ddlPhanLoai.Items.Clear();

                if(language.Equals("1"))
                {
                    ddlPhanLoai.Items.Add(new ListItem("Center", "1"));
                    ddlPhanLoai.Items.Add(new ListItem("Customer", "0"));

                    ddlNgonNgu.Items.Add(new ListItem("English", "1"));
                    ddlNgonNgu.Items.Add(new ListItem("Vietnam", "2"));
                }
                else if (language.Equals("2"))
                {
                    ddlPhanLoai.Items.Add(new ListItem("Trung tâm", "1"));
                    ddlPhanLoai.Items.Add(new ListItem("Khách hàng", "0"));

                    ddlNgonNgu.Items.Add(new ListItem("Tiếng Anh", "1"));
                    ddlNgonNgu.Items.Add(new ListItem("Tiếng Việt", "2"));
                }
                
                ddlPhanLoai.SelectedValue = "1";

                string query = "SELECT pk_seq, tenquyen as ten FROM NhomQuyen WHERE trangthai = '1' ORDER BY tenquyen ";
                DataTable dt = xl.ReadTable(query);

                ddlNhomQuyen.Items.Add(new ListItem("", ""));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlNhomQuyen.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                query = "SELECT pk_seq, ma + ', ' + hoten as ten FROM KhachHang WHERE trangthai = '1' ORDER BY ma ";
                dt = xl.ReadTable(query);
                ddlKhachHang.Items.Add(new ListItem("", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlKhachHang.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                string _disabled = "disabled='disabled'";
                if (Session["userId"].ToString().Equals("100000") || Session["userId"].ToString().Equals("100001"))
                {
                    roleAdmin = "1";
                    _disabled = "";
                }

                //INIT Nhom quyen
                if (this.id.Trim().Length <= 0)
                {
                    query = "SELECT pk_seq, tenquyen AS ten, maquyen AS ma, 0 as chon  " +
                            "FROM NhomQuyen WHERE TrangThai = '1' ORDER BY ma asc ";
                }
                else
                {
                    query = "SELECT b.pk_seq, tenquyen AS ten, maquyen AS ma, 1 as chon  " +
                            "FROM NhanVien_Quyen_NhomQuyen a INNER JOIN NhomQuyen b on a.nhomquyen_fk = b.pk_seq  WHERE a.nhanvien_fk = '" + this.id + "'     " +
                            "UNION ALL      " +
                            "SELECT pk_seq, tenquyen AS ten, maquyen AS ma, 0 as chon     " +
                            "FROM NhomQuyen WHERE TrangThai = '1'  and pk_seq not in ( SELECT nhomquyen_fk FROM NhanVien_Quyen_NhomQuyen WHERE nhanvien_fk = '" + this.id + "'  ) ORDER BY ma desc ";
                }
                DataTable dtKh = xl.ReadTable(query);

                if (dtKh.Rows.Count > 0)
                {
                    string str = "  ";

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

                //INIT QUYEN System
                //if (this.id.Trim().Length <= 0)
                //{
                //    query = "SELECT b.pk_seq, b.ma, b.ten, b.nameEnglish, 0 as chon  " +
                //            "FROM HeThong b WHERE b.trangthai = '1'  ";
                //}
                //else
                //{
                //    query = "SELECT b.pk_seq, b.ma, b.ten, b.nameEnglish, 1 as chon  " +
                //            "FROM NhanVien_Quyen_HeThong a INNER JOIN HeThong b on a.hethong_fk = b.pk_seq WHERE a.nhanvien_fk = '" + this.id + "'     " +
                //            "UNION ALL " +
                //            "SELECT b.pk_seq, b.ma, b.ten, b.nameEnglish, 0 as chon     " +
                //            "FROM HeThong b WHERE b.trangthai = '1' AND b.pk_seq NOT IN( SELECT hethong_fk FROM NhanVien_Quyen_HeThong WHERE nhanvien_fk = '" + this.id + "')  ";
                //}
                //dtKh = xl.ReadTable(query);
                //if (dtKh.Rows.Count > 0)
                //{
                //    string str = "  ";

                //    for (int i = 0; i < dtKh.Rows.Count; i++)
                //    {
                //        str += " <tr> ";

                //        str += " <td >" + (i + 1).ToString() + " </td> ";
                //        str += " <td >" + dtKh.Rows[i]["ma"].ToString() + " </td> ";

                //        if (language.Equals("1"))
                //            str += " <td >" + dtKh.Rows[i]["nameEnglish"].ToString() + " </td> ";
                //        else if (language.Equals("2"))
                //            str += " <td >" + dtKh.Rows[i]["ten"].ToString() + " </td> ";


                //        if (dtKh.Rows[i]["chon"].ToString().Equals("1"))
                //        {
                //            str += " <td style='text-align:center;' > <input type='checkbox' name='systemIds'  value='" + dtKh.Rows[i]["pk_seq"].ToString() + "'  checked " + _disabled + "/>  </td> ";
                //        }
                //        else
                //        {
                //            str += " <td style='text-align:center;' > <input type='checkbox' name='systemIds'  value='" + dtKh.Rows[i]["pk_seq"].ToString() + "'  " + _disabled + "/>  </td> ";
                //        }

                //        str += " </tr> ";
                //    }
                //    ltSystem.Text = str;
                //}

                //INIT QUYEN Khuvuc
                if (this.id.Trim().Length <= 0)
                {
                    query = "SELECT pk_seq, tenkho, makho, 1 as chon  " +
                            "FROM Kho WHERE TrangThai = '1' ORDER BY makho asc ";
                }
                else
                {
                    query = "SELECT b.pk_seq, b.tenkho, b.makho, 1 as chon  " +
                            "FROM NhanVien_Quyen_Kho a INNER JOIN Kho b on a.kho_fk = b.pk_seq  WHERE a.nhanvien_fk = '" + this.id + "'     " +
                            "UNION ALL      " +
                            "SELECT pk_seq, tenkho, makho, 1 as chon     " +
                            "FROM Kho WHERE TrangThai = '1'  and pk_seq not in ( SELECT kho_fk FROM NhanVien_Quyen_Kho WHERE nhanvien_fk = '" + this.id + "'  )  ";
                }

                dtKh = xl.ReadTable(query);
                if (dtKh.Rows.Count > 0)
                {
                    string str = "  ";

                    for (int i = 0; i < dtKh.Rows.Count; i++)
                    {
                        str += " <tr> ";

                        str += " <td >" + (i + 1).ToString() + " </td> ";
                        str += " <td >" + dtKh.Rows[i]["makho"].ToString() + " </td> ";
                        str += " <td >" + dtKh.Rows[i]["tenkho"].ToString() + " </td> ";

                        if (dtKh.Rows[i]["chon"].ToString().Equals("1"))
                        {
                            str += " <td style='text-align:center;' > <input type='checkbox' name='khoIds'  value='" + dtKh.Rows[i]["pk_seq"].ToString() + "'  checked />  </td> ";
                        }
                        else
                        {
                            str += " <td style='text-align:center;' > <input type='checkbox' name='khoIds'  value='" + dtKh.Rows[i]["pk_seq"].ToString() + "'  />  </td> ";
                        }

                        str += " </tr> ";
                    }


                    ltKho.Text = str;

                }

            }

            if (this.id.Trim().Length > 0)
            {
                string query = "SELECT ma, ten, trangthai, diachi, dienthoai, ISNULL(trungtam, 0) as phanloai, npp_fk, ISNULL(khachhang_fk, 0) khachhang_fk, ngonngu, mail, ISNULL(dangnhap, '') as dangnhap, token " +
                                " FROM NhanVien nv WHERE nv.pk_seq = '" + this.id + "'";

                DataTable dt = xl.ReadTable(query);
                if (dt.Rows.Count > 0)
                {
                    txtMa.Value = dt.Rows[0]["ma"].ToString();
                    txtTen.Value = dt.Rows[0]["ten"].ToString();
                    txtDienThoai.Text = dt.Rows[0]["dienthoai"].ToString();
                    txtDiaChi.Text = dt.Rows[0]["diachi"].ToString();
                    txtMail.Value = dt.Rows[0]["mail"].ToString();

                    ddlPhanLoai.SelectedValue = dt.Rows[0]["phanloai"].ToString();
                    ddlNgonNgu.SelectedValue = dt.Rows[0]["ngonngu"].ToString();
                    ddlKhachHang.SelectedValue = dt.Rows[0]["khachhang_fk"].ToString();
                    
                    txtDangNhap.Value = dt.Rows[0]["dangnhap"].ToString();
                    this.token = dt.Rows[0]["token"].ToString();

                    if (dt.Rows[0]["trangthai"].ToString().Equals("1"))
                        chkTrangThai.Checked = true;
                }

            }
            else
            {
                txtMa.Value = "";
                txtTen.Value = "";
                txtDienThoai.Text = "";
                txtDienThoai.Text = "";
                txtMail.Value = "";
                txtDangNhap.Value = "";

                chkTrangThai.Checked = true;
            }
        }

        protected void lbLuuLai_Click(object sender, EventArgs e)
        {
            txtMa.Value = txtDangNhap.Value;
          
            if (txtTen.Value.Trim().Length <= 0)
            {
                if (language.Equals("1"))
                {
                    lblError.Text = "Please you must be input fullname!";
                }
                else if (language.Equals("2"))
                {
                    lblError.Text = "Vui lòng nhập tên";
                }
                
                return;
            }
            
            if (txtDangNhap.Value.Trim().Length <= 0)
            {
                if (language.Equals("1"))
                {
                    lblError.Text = "Please you must be input username!";
                }
                else if (language.Equals("2"))
                {
                    lblError.Text = "Vui lòng nhập tên đăng nhập";
                }
                
                return;
            }
           
            ExecuteData xl = new ExecuteData();

            string query = "";

            string trangthai = "0";
            if (chkTrangThai.Checked)
                trangthai = "1";

            if (Session["userId"].ToString().Equals("100000") || Session["userId"].ToString().Equals("100001"))
            {
                roleAdmin = "1";
            }

            string khoanhanvien = "0";
            
            query = " SELECT COUNT(*) as sodong FROM NhanVien WHERE dangnhap = N'" + txtDangNhap.Value + "' ";
            if (this.id.Trim().Length > 0)
                query += " and pk_seq != '" + this.id + "' ";
            object obj = xl.ExecuteScalarSQL(query);
            if (int.Parse(obj.ToString()) >= 1)
            {
                if (language.Equals("1"))
                {
                    lblError.Text = "Username has been duplicated";
                }
                else if (language.Equals("2"))
                {
                    lblError.Text = "Tên đăng nhập của nhân viên đã bị trùng ";
                }
            }
            else
            {                
                string khachhang_fk = ddlKhachHang.SelectedValue;
                if (this.id.Trim().Length <= 0)
                {
                    token = this.MaSo() + this.MaChu() + DateTime.Now.ToString("ddMMyyyyhhmmss");

                    query = "INSERT NhanVien(ma, ten, trangthai, diachi, dienthoai, mail, trungtam, khoanhanvien, khachhang_fk , dangnhap, matkhau, ngonngu, token, nguoitao, nguoisua)  " +
                            "SELECT N'" + txtMa.Value + "', N'" + txtTen.Value + "', '" + trangthai + "', N'" + txtDiaChi.Text + "', '" + txtDienThoai.Text + "', N'" + txtMail.Value + "', '1', " + khoanhanvien + " ," + khachhang_fk + ", N'" + txtDangNhap.Value + "', pwdencrypt(N'" + txtMatkhau.Value + "'), '" + ddlNgonNgu.SelectedValue + "', N'" + token + "', '" + Session["userId"].ToString() + "', '" + Session["userId"].ToString() + "' ";
                }
                else
                {
                    query = "UPDATE NhanVien SET ma = N'" + txtMa.Value + "', ten = N'" + txtTen.Value + "', trangthai = '" + trangthai + "', diachi = N'" + txtDiaChi.Text + "', " +
                            "  dienthoai = N'" + txtDienThoai.Text + "', mail = N'" + txtMail.Value + "', trungtam = '1', khoanhanvien = " + khoanhanvien + ", khachhang_fk = " + khachhang_fk + ", dangnhap = N'" + txtDangNhap.Value + "', " +
                            "  ngonngu = '" + ddlNgonNgu.SelectedValue + "', nguoisua = '" + Session["userId"].ToString() + "', ngaysua = getdate() ";

                    if (txtMatkhau.Value.Trim().Length > 0)
                        query += " , matkhau = pwdencrypt(N'" + txtMatkhau.Value + "') ";

                    query += " WHERE pk_seq = '" + this.id + "' ";
                }

                if (xl.ExecuteNonQuerySQL(query))
                {
                    lblError.Text = "";

                    // Tạo tài khoản trên hệ thống khác
                    NhanVien nv = new NhanVien();
                    string msg = nv.INSERET_Account_PC(txtDangNhap.Value, txtMatkhau.Value, txtTen.Value, trangthai, ddlPhanLoai.SelectedValue, txtDiaChi.Text, txtDienThoai.Text, txtMail.Value, ddlNgonNgu.SelectedValue, token, Session["userId"].ToString());
                    if(msg.Length > 10)
                    {
                        lblError.Text = msg;
                        return;
                    }
                    msg = nv.INSERET_Account_OC(txtDangNhap.Value, txtMatkhau.Value, txtTen.Value, trangthai, ddlPhanLoai.SelectedValue, txtDiaChi.Text, txtDienThoai.Text, txtMail.Value, ddlNgonNgu.SelectedValue, token, Session["userId"].ToString());
                    if (msg.Length > 10)
                    {
                        lblError.Text = msg;
                        return;
                    }

                    msg = nv.INSERET_Account_WS(txtDangNhap.Value, txtMatkhau.Value, txtTen.Value, trangthai, ddlPhanLoai.SelectedValue, txtDiaChi.Text, txtDienThoai.Text, txtMail.Value, ddlNgonNgu.SelectedValue, token, Session["userId"].ToString());
                    if (msg.Length > 10)
                    {
                        lblError.Text = msg;
                        return;
                    }

                    msg = nv.INSERET_Account_WHVinhPhuc(txtDangNhap.Value, txtMatkhau.Value, txtTen.Value, trangthai, ddlPhanLoai.SelectedValue, txtDiaChi.Text, txtDienThoai.Text, txtMail.Value, ddlNgonNgu.SelectedValue, token, Session["userId"].ToString());
                    if (msg.Length > 10)
                    {
                        lblError.Text = msg;
                        return;
                    }

                    msg = nv.INSERET_Account_NWHaiPhong(txtDangNhap.Value, txtMatkhau.Value, txtTen.Value, trangthai, ddlPhanLoai.SelectedValue, txtDiaChi.Text, txtDienThoai.Text, txtMail.Value, ddlNgonNgu.SelectedValue, token, Session["userId"].ToString());
                    if (msg.Length > 10)
                    {
                        lblError.Text = msg;
                        return;
                    }

                    //CHEN PHAN QUYEN
                    string nv_fk = this.id;
                    if (nv_fk.Trim().Length <= 0)
                        nv_fk = xl.ExecuteScalarSQL("SELECT IDENT_CURRENT('NhanVien')").ToString();

                    xl.ExecuteScalarSQL("DELETE NhanVien_Quyen_NhomQuyen WHERE nhanvien_fk = '" + nv_fk + "' ");                    
                    if (Request.Form["nhomquyenIds"] != null)
                    {
                        string NhomquyenIDS = Request.Form["nhomquyenIds"].ToString();
                        xl.ExecuteNonQuerySQL(" INSERT NhanVien_Quyen_NhomQuyen(nhanvien_fk, nhomquyen_fk) SELECT '" + nv_fk + "', pk_seq FROM NhomQuyen WHERE pk_seq in ( " + NhomquyenIDS + " ) ");
                    }

                    xl.ExecuteScalarSQL("DELETE NhanVien_Quyen_Location WHERE nhanvien_fk = '" + nv_fk + "' ");
                    if (Request.Form["locationIds"] != null)
                    {
                        string LocationIDS = Request.Form["locationIds"].ToString();
                        xl.ExecuteNonQuerySQL(" INSERT NhanVien_Quyen_Location(nhanvien_fk, location_fk) SELECT '" + nv_fk + "', pk_seq FROM Location WHERE pk_seq in ( " + LocationIDS + " ) ");
                    }

                    //KHU VUC
                    xl.ExecuteScalarSQL("DELETE NhanVien_Quyen_KhuVuc WHERE nhanvien_fk = '" + nv_fk + "' ");
                    if (Request.Form["khuvucIds"] != null)
                    {
                        string KhuvucIDS = Request.Form["khuvucIds"].ToString();
                        xl.ExecuteNonQuerySQL(" INSERT NhanVien_Quyen_KhuVuc(nhanvien_fk, khuvuc_fk) SELECT '" + nv_fk + "', pk_seq FROM KHUVUC WHERE pk_seq in ( " + KhuvucIDS + " ) ");
                    }

                    //Kho
                    xl.ExecuteScalarSQL("DELETE NhanVien_Quyen_Kho WHERE nhanvien_fk = '" + nv_fk + "' ");
                    if (Request.Form["khoIds"] != null)
                    {
                        string khoIds = Request.Form["khoIds"].ToString();
                        xl.ExecuteNonQuerySQL(" INSERT NhanVien_Quyen_Kho(nhanvien_fk, kho_fk) SELECT '" + nv_fk + "', pk_seq FROM Kho WHERE pk_seq in ( " + khoIds + " ) ");
                    }

                    //if (roleAdmin.Equals("1"))
                    //{
                    //    // System
                    //    xl.ExecuteScalarSQL("DELETE NhanVien_Quyen_HeThong WHERE nhanvien_fk = '" + nv_fk + "' ");
                    //    if (Request.Form["systemIds"] != null)
                    //    {
                    //        string systemIds = Request.Form["systemIds"].ToString();
                    //        xl.ExecuteNonQuerySQL(" INSERT NhanVien_Quyen_HeThong(nhanvien_fk, hethong_fk) SELECT '" + nv_fk + "', pk_seq FROM HeThong WHERE pk_seq in ( " + systemIds + " ) ");
                    //    }
                    //}

                    Response.Redirect("Administrator.aspx?func=92");
                }
                else
                {
                    if (language.Equals("1"))
                    {
                        lblError.Text = "Cannot save this information. Please try agian! ";
                    }
                    else if (language.Equals("2"))
                    {
                        lblError.Text = "Không thể lưu thông tin. Vui lòng kiểm tra lại! ";
                    }
                }
            }
        }

        protected void ddlPhanLoai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public string MaSo()
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

        public string MaChu()
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