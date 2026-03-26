using HLVTimeSheet.AcsessData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HLVTimeSheet.Model
{
    public class StaffController
    {
        public string INSERT_Staff(string ma, string ten, string tenhienthi, string chinhanh_fk, string phongban_fk, string phongbanSupport_fk, string chucvu_fk, string nhanvien_fk, string dienthoai, string mail, string diachi, string ngaysinh, string ngaybatdaulam, string gioitinh, string hinhanh, string trangthai, string tungay, string denngay, string lydo, string hienthi, string dangnhap, string matkhau, string token, string phongban, string nhansu, string nhomquyen, string nguoitao)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@"));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = 0;
            try
            {
                string sql = "SELECT COUNT(*) FROM DanhSachNhanSu WHERE ma = N'" + ma + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (int.Parse(obj.ToString()) > 0)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "1.Error! Staff is duplicate.";
                }

                string loai = "0";
                sql = "SELECT loai FROM PhongBan WHERE pk_seq = '" + phongban_fk + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                obj = command.ExecuteScalar();
                if (obj == null)
                {
                    loai = "1";
                }
                else
                    loai = obj.ToString();

                if (trangthai.Equals("2") || trangthai.Equals("3"))
                {
                    if(tungay.Length < 3 || denngay.Length < 3)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "2.Error! Time is not right";
                    }

                    sql = "SELECT DATEDIFF(DAY, CONVERT(datetime, '" + tungay + "', 105), CONVERT(datetime, '" + denngay + "', 105)) ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    obj = command.ExecuteScalar();
                    if (obj != null)
                    {
                        if (int.Parse(obj.ToString()) < 0)
                        {
                            transaction.Rollback();
                            connect.Close();
                            return "2.Error! Time is not right";
                        }
                    }
                }

                // Kiểm tra dangnhap
                sql = "SELECT COUNT(*) FROM NhanVien WHERE dangnhap = N'" + dangnhap + "' AND token != N'" + token + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                obj = command.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) > 0)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Tên đăng nhập: " + dangnhap + " đã tồn tại trong hệ thống. WS ";
                }

                sql = "INSERT DanhSachNhanSu(ma, ten, tenhienthi, chinhanh_fk, phongban_fk, phongbanGoc_fk, phongbanSupport_fk, chucvu_fk, capbac, loai, nhanvien_fk, dienthoai, mail, diachi, ngaysinh, ngaybatdaulam, gioitinh, hinhanh, trangthai, hientrang, tungay, denngay, lydo, hienthi, timkiem, nguoitao, nguoisua) " +
                    " SELECT N'" + ma + "', N'" + ten + "', N'" + tenhienthi + "', N'" + chinhanh_fk + "', N'" + phongban_fk + "', '" + phongban_fk + "', N'" + phongbanSupport_fk + "', N'" + chucvu_fk + "', ISNULL((SELECT capbac FROM ChucVu WHERE pk_seq = N'" + chucvu_fk + "'), 0), '" + loai + "', '" + nhanvien_fk + "', N'" + dienthoai + "', N'" + mail + "', N'" + diachi + "', N'" + ngaysinh + "', N'" + ngaybatdaulam + "', '" + gioitinh + "', N'" + hinhanh + "', '" + trangthai + "', '" + trangthai + "', N'" + tungay + "', N'" + denngay + "', N'" + lydo + "', '" + hienthi + "', dbo.ftBoDau(N'" + ma + "' + ' ' + N'" + ten + "' + ' ' + N'" + dienthoai + "' + ' ' + N'" + mail + "' + ' ' + N'" + diachi + "'), '" + nguoitao + "', '" + nguoitao + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "2.Error! Cannot created new this staff.";
                }

                sql = "SELECT IDENT_CURRENT('DanhSachNhanSu')";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                string nhansu_fk = command.ExecuteScalar().ToString();

                if (phongban.Length > 3)
                {
                    sql = "INSERT DanhSachNhanSu_PhongBan(phongban_fk, nhansu_fk) SELECT pk_seq, " + nhansu_fk + " FROM PhongBan WHERE pk_seq in (" + phongban + ") ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "6.Error! Cannot created new this staff.";
                    }
                }

                if (nhansu.Length > 3)
                {
                    sql = "INSERT DanhSachNhanSu_QuanLy(quanly_fk, nhansu_fk) SELECT " + nhansu_fk + ", pk_seq FROM DanhSachNhanSu WHERE pk_seq in (" + nhansu + ") ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "6.Error! Cannot created new this staff.";
                    }
                }

                if (trangthai.Equals("2") || trangthai.Equals("3"))
                {
                    sql = "INSERT DanhSachNhanSu_ThoiGianNghi(nhansu_fk, lydo, tungay, denngay) SELECT " + nhansu_fk + ", N'" + lydo + "', N'" + tungay + "', N'" + denngay + "' ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "6.Error! Cannot created new this staff.";
                    }
                }

                if (nhansu.Length < 3)
                {
                    sql = " INSERT DanhSachNhanSu_QuanLy(quanly_fk, nhansu_fk) " +
                    " SELECT " + nhansu_fk + ", nhansu_fk " +
                    " FROM DanhSachNhanSu_QuanLy " +
                    " WHERE quanly_fk in (SELECT pk_seq FROM DanhSachNhanSu WHERE phongban_fk = " + phongban_fk + " AND capbac = ISNULL((SELECT capbac FROM ChucVu WHERE pk_seq = N'" + chucvu_fk + "'), 0) AND chinhanh_fk = " + chinhanh_fk + " AND pk_seq != " + nhansu_fk + ") " +
                    "	AND nhansu_fk NOT IN (SELECT nhansu_fk FROM DanhSachNhanSu_QuanLy WHERE quanly_fk = " + nhansu_fk + ") ";
                        command.CommandTimeout = int.MaxValue;
                        command.CommandText = sql;
                        kq = command.ExecuteNonQuery();
                }

                // Dựa trên việc thiet lap phong cho nhan vien, xac dinh nguoi quan ly truc tiep
                //if(phongban_fk.Length > 3)
                //{
                //    string quanly_fk = "";
                //    // Xác định người quản lý trực tiếp của phòng
                //    sql = "SELECT DISTINCT b.quanly_fk FROM DanhSachNhanSu a INNER JOIN DanhSachNhanSu_QuanLy b ON a.pk_seq = b.nhansu_fk WHERE a.chinhanh_fk = '" + chinhanh_fk + "' AND a.phongban_fk = '" + phongban_fk + "' ";
                //    command.CommandTimeout = int.MaxValue;
                //    command.CommandText = sql;
                //    obj = command.ExecuteScalar();
                //    if (obj != null)
                //    {
                //        quanly_fk = obj.ToString();
                //        sql = "INSERT DanhSachNhanSu_QuanLy(quanly_fk, nhansu_fk) SELECT " + quanly_fk + ", '" + nhansu_fk + "' ";
                //        command.CommandTimeout = int.MaxValue;
                //        command.CommandText = sql;
                //        kq = command.ExecuteNonQuery();
                //        if (kq < 1)
                //        {
                //            transaction.Rollback();
                //            connect.Close();
                //            return "6.Error! Cannot created new this staff.";
                //        }
                //    }                        
                //}

                // tài khoản
                if (dangnhap.Length > 0)
                {
                    // Tạo tài khoản và nhóm quyền
                    NhanVien nv = new NhanVien();
                    string msg = nv.INSERET_Account_WS(dangnhap, matkhau, ten, "1", "1", diachi, dienthoai, mail, "1", token, nguoitao);
                    //if(msg.Length > 10)
                    //{
                    //    transaction.Rollback();
                    //    connect.Close();
                    //    return "3.Error! Cannot created new this staff." + msg;
                    //}
                    msg = nv.INSERET_Account_PC(dangnhap, matkhau, ten, "1", "1", diachi, dienthoai, mail, "1", token, nguoitao);
                    //if (msg.Length > 10)
                    //{

                    //}
                    msg = nv.INSERET_Account_OC(dangnhap, matkhau, ten, "1", "1", diachi, dienthoai, mail, "1", token, nguoitao);
                    //if (msg.Length > 10)
                    //{

                    //}

                    if(nhomquyen.Length > 0)
                    {
                        sql = "DELETE NhanVien_Quyen_NhomQuyen WHERE nhanvien_fk = ISNULL((SELECT pk_seq FROM NhanVien WHERE dangnhap = N'" + dangnhap + "'), 0) ";
                        command.CommandTimeout = int.MaxValue;
                        command.CommandText = sql;
                        kq = command.ExecuteNonQuery();

                        sql = " INSERT NhanVien_Quyen_NhomQuyen(nhanvien_fk, nhomquyen_fk) " + 
                            " SELECT ISNULL((SELECT pk_seq FROM NhanVien WHERE dangnhap = N'" + dangnhap + "'), 0), pk_seq FROM NhomQuyen WHERE pk_seq in ( " + nhomquyen + " ) ";
                        command.CommandTimeout = int.MaxValue;
                        command.CommandText = sql;
                        kq = command.ExecuteNonQuery();
                    }

                    sql = "UPDATE DanhSachNhanSu SET nhanvien_fk = ISNULL((SELECT pk_seq FROM NhanVien WHERE dangnhap = N'" + dangnhap + "'), 0) WHERE pk_seq = '" + nhansu_fk + "' ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                connect.Close();
                return "3.Error! Cannot created new this staff.";
            }

            connect.Close();
            return "";
        }

        public string UPDATE_Staff(string id, string ma, string ten, string tenhienthi, string chinhanh_fk, string phongban_fk, string phongbanSupport_fk, string chucvu_fk, string nhanvien_fk, string dienthoai, string mail, string diachi, string ngaysinh, string ngaybatdaulam, string gioitinh, string hinhanh, string trangthai, string tungay, string denngay, string lydo, string hienthi, string dangnhap, string matkhau, string token, string phongban, string nhansu, string nhomquyen, string nguoitao)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@"));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = 0;
            try
            {
                string sql = "SELECT COUNT(*) FROM DanhSachNhanSu WHERE ma = N'" + ma + "' AND pk_seq != '" + id + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (int.Parse(obj.ToString()) > 0)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "1.Error! staff is duplicate.";
                }

                string loai = "0";
                sql = "SELECT loai FROM PhongBan WHERE pk_seq = '" + phongban_fk + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                obj = command.ExecuteScalar();
                if (obj == null)
                {
                    loai = "1";
                }
                else
                    loai = obj.ToString();

                sql = "SELECT trangthai FROM DanhSachNhanSu WHERE pk_seq = '" + id + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                obj = command.ExecuteScalar();

                if (trangthai.Equals("3") && phongbanSupport_fk.Equals("0"))
                {
                    // Nếu cấp bậc = 9, thì báo lỗi
                    sql = "SELECT capbac FROM DanhSachNhanSu WHERE pk_seq = '" + id + "' ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    object checkLevel = command.ExecuteScalar();
                    if (checkLevel != null && int.Parse(checkLevel.ToString()) == 9)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "2.Error! Please choose department support";
                    }
                }

                if (trangthai.Equals("2") || trangthai.Equals("3"))
                {
                    if (tungay.Length < 3 || denngay.Length < 3)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "2.Error! Time is not right";
                    }

                    sql = "SELECT DATEDIFF(DAY, CONVERT(datetime, '" + tungay + "', 105), CONVERT(datetime, '" + denngay + "', 105)) ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    obj = command.ExecuteScalar();
                    if (obj != null)
                    {
                        if (int.Parse(obj.ToString()) < 0)
                        {
                            transaction.Rollback();
                            connect.Close();
                            return "2.Error! Time is not right";
                        }
                    }
                }
                else if (trangthai.Equals("1"))
                {
                    // Xóa các cài đặt đã thiết lập
                    sql = " SELECT a.pk_seq, a.trangthai, a.capbac, ISNULL(a.tungay, '') tungay, ISNULL(a.denngay, '') denngay, phongban_fk, phongbanSupport_fk, " +
                     " ISNULL((SELECT COUNT(*) FROM DanhSachNhanSu b  " +
                     "	WHERE CONVERT(datetime, b.tungay, 105) <= CONVERT(datetime, '" + DateTime.Now.ToString("dd-MM-yyyy") + "', 105)  " +
                     "		AND CONVERT(datetime, b.denngay, 105) >= CONVERT(datetime, '" + DateTime.Now.ToString("dd-MM-yyyy") + "', 105)  " +
                     "		AND b.pk_seq = a.pk_seq), 0) flagIN,  " +
                     " ISNULL((SELECT COUNT(*) FROM DanhSachNhanSu b  " +
                     "	WHERE CONVERT(datetime, b.tungay, 105) > CONVERT(datetime, '" + DateTime.Now.ToString("dd-MM-yyyy") + "', 105)  " +
                     "		AND b.pk_seq = a.pk_seq AND b.hientrang != b.trangthai), 0) flagOUT  " +
                     " FROM DanhSachNhanSu a " +
                     " WHERE a.trangthai in (1, 2, 3) AND a.pk_seq = '" + id + "' ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    SqlDataReader objNS = command.ExecuteReader();
                    if (objNS != null)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(objNS);

                        if (dt.Rows.Count > 0 && int.Parse(dt.Rows[0]["flagIN"].ToString()) > 0 && dt.Rows[0]["trangthai"].ToString().Equals("3"))
                        {
                            if (dt.Rows[0]["capbac"].ToString().Equals("9"))
                            {

                                sql = " DELETE DanhSachNhanSu_QuanLy WHERE quanly_fk = '" + id + "' ";
                                command.CommandTimeout = int.MaxValue;
                                command.CommandText = sql;
                                kq = command.ExecuteNonQuery();

                                // Xóa nhân sự đang quản lý ai
                                sql = " DELETE DanhSachNhanSu_QuanLy WHERE nhansu_fk = '" + id + "' ";
                                command.CommandTimeout = int.MaxValue;
                                command.CommandText = sql;
                                kq = command.ExecuteNonQuery();

                                // Tạo lai ai đang quản lý nhân sự
                                // Nhân sự quản lý ai
                                // Ai đang quản lý nhân sự này
                                sql = "INSERT DanhSachNhanSu_QuanLy(quanly_fk, nhansu_fk)" +
                                    " SELECT quanly_fk, '" + id + "' FROM DanhSachNhanSu_LichSuQuanLy WHERE nhansu_fk = '" + id + "' ";
                                command.CommandTimeout = int.MaxValue;
                                command.CommandText = sql;
                                kq = command.ExecuteNonQuery();
                                // Nhân sự này đang quản lý ai
                                sql = "INSERT DanhSachNhanSu_QuanLy(quanly_fk, nhansu_fk)" +
                                   " SELECT '" + id + "', nhansu_fk FROM DanhSachNhanSu_LichSuQuanLy WHERE quanly_fk = '" + id + "' ";
                                command.CommandTimeout = int.MaxValue;
                                command.CommandText = sql;
                                kq = command.ExecuteNonQuery();

                                // Xóa lịch sử
                                // Xóa Ai đang quản lý nhân sự này
                                // Xóa Nhân sự này đang quản lý ai                    
                                sql = " DELETE DanhSachNhanSu_LichSuQuanLy WHERE quanly_fk = '" + id + "' ";
                                command.CommandTimeout = int.MaxValue;
                                command.CommandText = sql;
                                kq = command.ExecuteNonQuery();
                                // Xóa nhân sự đang quản lý ai
                                sql = " DELETE DanhSachNhanSu_LichSuQuanLy WHERE nhansu_fk = '" + id + "' ";
                                command.CommandTimeout = int.MaxValue;
                                command.CommandText = sql;
                                kq = command.ExecuteNonQuery();
                            }

                            sql = "UPDATE DanhSachNhanSu SET tungay = '', denngay = '', lydo = '', hientrang = 1, trangthai = 1, phongban_fk = phongbanGoc_fk WHERE pk_seq = " + id + " ";
                            command.CommandTimeout = int.MaxValue;
                            command.CommandText = sql;
                            kq = command.ExecuteNonQuery();
                        }
                        else
                        {
                            // 
                            sql = " DELETE DanhSachNhanSu_LichSuQuanLy WHERE quanly_fk = '" + id + "' ";
                            command.CommandTimeout = int.MaxValue;
                            command.CommandText = sql;
                            kq = command.ExecuteNonQuery();

                            sql = " DELETE DanhSachNhanSu_LichSuQuanLy WHERE nhansu_fk = '" + id + "' ";
                            command.CommandTimeout = int.MaxValue;
                            command.CommandText = sql;
                            kq = command.ExecuteNonQuery();

                            sql = "UPDATE DanhSachNhanSu SET tungay = '', denngay = '', lydo = '', hientrang = 1, trangthai = 1, phongban_fk = phongbanGoc_fk WHERE pk_seq = " + id + " ";
                            command.CommandTimeout = int.MaxValue;
                            command.CommandText = sql;
                            kq = command.ExecuteNonQuery();
                        }

                        dt.Clone();
                        dt.Clear();
                    }
                }

                if (int.Parse(obj.ToString()) == 1)
                {
                    sql = "UPDATE DanhSachNhanSu SET ma = N'" + ma + "', ten = N'" + ten + "', tenhienthi = N'" + tenhienthi + "', chinhanh_fk = N'" + chinhanh_fk + "', phongban_fk = N'" + phongban_fk + "', phongbanGoc_fk = '" + phongban_fk + "', phongbanSupport_fk = '" + phongbanSupport_fk + "', chucvu_fk = N'" + chucvu_fk + "', capbac = ISNULL((SELECT capbac FROM ChucVu WHERE pk_seq = N'" + chucvu_fk + "'), 0), loai = '" + loai + "', dienthoai = N'" + dienthoai + "', mail = N'" + mail + "', diachi = N'" + diachi + "', ngaysinh = N'" + ngaysinh + "', ngaybatdaulam = N'" + ngaybatdaulam + "', gioitinh = '" + gioitinh + "', hinhanh = N'" + hinhanh + "', trangthai = N'" + trangthai + "', tungay = N'" + tungay + "', denngay = N'" + denngay + "', lydo = N'" + lydo + "', hienthi = '" + hienthi + "', timkiem = dbo.ftBoDau(N'" + ma + "' + ' ' + N'" + ten + "' + ' ' + N'" + dienthoai + "' + ' ' + N'" + mail + "' + ' ' + N'" + diachi + "'), nguoisua =  N'" + nguoitao + "', ngaysua = GETDATE() " +
                        " WHERE pk_seq = '" + id + "' ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "2.Error! Cannot updated this staff.";
                    }
                }
                else
                {
                    sql = "UPDATE DanhSachNhanSu SET ma = N'" + ma + "', ten = N'" + ten + "', tenhienthi = N'" + tenhienthi + "', chinhanh_fk = N'" + chinhanh_fk + "', chucvu_fk = N'" + chucvu_fk + "', capbac = ISNULL((SELECT capbac FROM ChucVu WHERE pk_seq = N'" + chucvu_fk + "'), 0), loai = '" + loai + "', dienthoai = N'" + dienthoai + "', mail = N'" + mail + "', diachi = N'" + diachi + "', ngaysinh = N'" + ngaysinh + "', ngaybatdaulam = N'" + ngaybatdaulam + "', gioitinh = '" + gioitinh + "', hinhanh = N'" + hinhanh + "', trangthai = N'" + trangthai + "', tungay = N'" + tungay + "', denngay = N'" + denngay + "', lydo = N'" + lydo + "', hienthi = '" + hienthi + "', timkiem = dbo.ftBoDau(N'" + ma + "' + ' ' + N'" + ten + "' + ' ' + N'" + dienthoai + "' + ' ' + N'" + mail + "' + ' ' + N'" + diachi + "'), nguoisua =  N'" + nguoitao + "', ngaysua = GETDATE() " +
                        " WHERE pk_seq = '" + id + "' ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "2.Error! Cannot updated this staff.";
                    }
                }

                // Kiểm tra dangnhap
                sql = "SELECT COUNT(*) FROM NhanVien WHERE dangnhap = N'" + dangnhap + "' AND token != N'" + token + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                obj = command.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) > 0)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Tên đăng nhập: " + dangnhap + " đã tồn tại trong hệ thống. WS ";
                }
                
                sql = "DELETE DanhSachNhanSu_PhongBan WHERE nhansu_fk = '" + id + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();

                sql = "DELETE DanhSachNhanSu_QuanLy WHERE quanly_fk = '" + id + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();

                if (phongban.Length > 3)
                {
                    sql = "INSERT DanhSachNhanSu_PhongBan(phongban_fk, nhansu_fk) SELECT pk_seq, " + id + " FROM PhongBan WHERE pk_seq in (" + phongban + ") ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "6.Error! Cannot created new this staff.";
                    }
                }

                if (nhansu.Length > 3)
                {
                    sql = "INSERT DanhSachNhanSu_QuanLy(quanly_fk, nhansu_fk) SELECT " + id + ", pk_seq FROM DanhSachNhanSu WHERE pk_seq in (" + nhansu + ") ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "6.Error! Cannot created new this staff.";
                    }
                }

                if (trangthai.Equals("2") || trangthai.Equals("3"))
                {
                    sql = "INSERT DanhSachNhanSu_ThoiGianNghi(nhansu_fk, phongbanSupport_fk, lydo, tungay, denngay) SELECT " + id + ", '" + phongbanSupport_fk + "', N'" + lydo + "', N'" + tungay + "', N'" + denngay + "' ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "6.Error! Cannot created new this staff.";
                    }
                }

                if (nhansu.Length < 3)
                {
                    sql = " INSERT DanhSachNhanSu_QuanLy(quanly_fk, nhansu_fk) " +
                    " SELECT " + id + ", nhansu_fk " +
                    " FROM DanhSachNhanSu_QuanLy " +
                    " WHERE quanly_fk in (SELECT pk_seq FROM DanhSachNhanSu WHERE phongban_fk = " + phongban_fk + " AND capbac = ISNULL((SELECT capbac FROM ChucVu WHERE pk_seq = N'" + chucvu_fk + "'), 0) AND chinhanh_fk = " + chinhanh_fk + " AND pk_seq != " + id + ") " +
                    "	AND nhansu_fk NOT IN (SELECT nhansu_fk FROM DanhSachNhanSu_QuanLy WHERE quanly_fk = " + id + ") ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                }

                // tài khoản
                if (dangnhap.Length > 0)
                {
                    // Tạo tài khoản và nhóm quyền
                    NhanVien nv = new NhanVien();
                    string msg = nv.INSERET_Account_WS(dangnhap, matkhau, ten, "1", "1", diachi, dienthoai, mail, "1", token, nguoitao);
                    //if(msg.Length > 10)
                    //{
                    //    transaction.Rollback();
                    //    connect.Close();
                    //    return "3.Error! Cannot created new this staff." + msg;
                    //}
                    msg = nv.INSERET_Account_PC(dangnhap, matkhau, ten, "1", "1", diachi, dienthoai, mail, "1", token, nguoitao);
                    //if (msg.Length > 10)
                    //{

                    //}
                    msg = nv.INSERET_Account_OC(dangnhap, matkhau, ten, "1", "1", diachi, dienthoai, mail, "1", token, nguoitao);
                    //if (msg.Length > 10)
                    //{

                    //}

                    if (nhomquyen.Length > 0)
                    {
                        sql = "DELETE NhanVien_Quyen_NhomQuyen WHERE nhanvien_fk = ISNULL((SELECT pk_seq FROM NhanVien WHERE dangnhap = N'" + dangnhap + "'), 0) ";
                        command.CommandTimeout = int.MaxValue;
                        command.CommandText = sql;
                        kq = command.ExecuteNonQuery();

                        sql = " INSERT NhanVien_Quyen_NhomQuyen(nhanvien_fk, nhomquyen_fk) " +
                             " SELECT ISNULL((SELECT pk_seq FROM NhanVien WHERE dangnhap = N'" + dangnhap + "'), 0), pk_seq FROM NhomQuyen WHERE pk_seq in ( " + nhomquyen + " ) ";
                        command.CommandTimeout = int.MaxValue;
                        command.CommandText = sql;
                        kq = command.ExecuteNonQuery();
                    }

                    sql = "UPDATE DanhSachNhanSu SET nhanvien_fk = ISNULL((SELECT pk_seq FROM NhanVien WHERE dangnhap = N'" + dangnhap + "'), 0) WHERE pk_seq = '" + id + "' ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                connect.Close();
                return "3.Error! Cannot updated this staff.";
            }

            connect.Close();
            return "";
        }

        public string DELETE_Staff(string id, string nguoitao)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@"));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = 0;
            try
            {
                string sql = "UPDATE DanhSachNhanSu SET  trangthai = '0', hientrang = '0', nguoisua = N'" + nguoitao + "', ngaysua = GETDATE() " +
                    " WHERE pk_seq = '" + id + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "2.Error! Cannot updated this staff.";
                }

                sql = "DELETE DanhSachNhanSu_QuanLy WHERE quanly_fk = '" + id + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                connect.Close();
                return "3.Error! Cannot updated this staff.";
            }

            connect.Close();
            return "";
        }

        public string UPDATE_QuicklyStaff(string id,  string chinhanh_fk, string phongbanSupport_fk, string chucvu_fk, string trangthai, string tungay, string denngay, string lydo, string nguoitao)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@"));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = 0;
            try
            {
                string sql = "";
                SqlCommand command = new SqlCommand(sql, connect, transaction);

                if (trangthai.Equals("1"))
                    lydo = "";

                if (trangthai.Equals("3") && phongbanSupport_fk.Equals("0"))
                {
                    // Nếu cấp bậc = 9, thì báo lỗi
                    sql = "SELECT capbac FROM DanhSachNhanSu WHERE pk_seq = '" + id + "' ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    object checkLevel = command.ExecuteScalar();
                    if (checkLevel != null && int.Parse(checkLevel.ToString()) == 9)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "2.Error! Please choose department support";
                    }
                }

                if (trangthai.Equals("2") || trangthai.Equals("3"))
                {
                    if (tungay.Length < 3 || denngay.Length < 3)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "2.Error! Time is not right";
                    }

                    sql = "SELECT DATEDIFF(DAY, CONVERT(datetime, '" + tungay + "', 105), CONVERT(datetime, '" + denngay + "', 105)) ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    object obj = command.ExecuteScalar();
                    if (obj != null)
                    {
                        if (int.Parse(obj.ToString()) < 0)
                        {
                            transaction.Rollback();
                            connect.Close();
                            return "2.Error! Time is not right";
                        }
                    }

                    sql = "INSERT DanhSachNhanSu_ThoiGianNghi(nhansu_fk, phongbanSupport_fk, lydo, tungay, denngay) SELECT " + id + ", '" + phongbanSupport_fk + "', N'" + lydo + "', N'" + tungay + "', N'" + denngay + "' ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "6.Error! Cannot created new this staff.";
                    }
                }
                else if (trangthai.Equals("1"))
                {
                    // Xóa các cài đặt đã thiết lập
                    sql = " SELECT a.pk_seq, a.trangthai, a.capbac, ISNULL(a.tungay, '') tungay, ISNULL(a.denngay, '') denngay, phongban_fk, phongbanSupport_fk, " +
                     " ISNULL((SELECT COUNT(*) FROM DanhSachNhanSu b  " +
                     "	WHERE CONVERT(datetime, b.tungay, 105) <= CONVERT(datetime, '" + DateTime.Now.ToString("dd-MM-yyyy") + "', 105)  " +
                     "		AND CONVERT(datetime, b.denngay, 105) >= CONVERT(datetime, '" + DateTime.Now.ToString("dd-MM-yyyy") + "', 105)  " +
                     "		AND b.pk_seq = a.pk_seq), 0) flagIN,  " +
                     " ISNULL((SELECT COUNT(*) FROM DanhSachNhanSu b  " +
                     "	WHERE CONVERT(datetime, b.tungay, 105) > CONVERT(datetime, '" + DateTime.Now.ToString("dd-MM-yyyy") + "', 105)  " +
                     "		AND b.pk_seq = a.pk_seq AND b.hientrang != b.trangthai), 0) flagOUT  " +
                     " FROM DanhSachNhanSu a " +
                     " WHERE a.trangthai in (1, 2, 3) AND a.pk_seq = '" + id + "' ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    SqlDataReader objNS = command.ExecuteReader();
                    if (objNS != null)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(objNS);

                        if (dt.Rows.Count > 0 && int.Parse(dt.Rows[0]["flagIN"].ToString()) > 0 && dt.Rows[0]["trangthai"].ToString().Equals("3"))
                        {
                            if (dt.Rows[0]["capbac"].ToString().Equals("9"))
                            {
                                sql = " DELETE DanhSachNhanSu_QuanLy WHERE quanly_fk = '" + id + "' ";
                                command.CommandTimeout = int.MaxValue;
                                command.CommandText = sql;
                                kq = command.ExecuteNonQuery();

                                sql = " DELETE DanhSachNhanSu_QuanLy WHERE nhansu_fk = '" + id + "' ";
                                command.CommandTimeout = int.MaxValue;
                                command.CommandText = sql;
                                kq = command.ExecuteNonQuery();

                                sql = "INSERT DanhSachNhanSu_QuanLy(quanly_fk, nhansu_fk)" +
                                    " SELECT quanly_fk, '" + id + "' FROM DanhSachNhanSu_LichSuQuanLy WHERE nhansu_fk = '" + id + "' ";
                                command.CommandTimeout = int.MaxValue;
                                command.CommandText = sql;
                                kq = command.ExecuteNonQuery();

                                sql = "INSERT DanhSachNhanSu_QuanLy(quanly_fk, nhansu_fk)" +
                                   " SELECT '" + id + "', nhansu_fk FROM DanhSachNhanSu_LichSuQuanLy WHERE quanly_fk = '" + id + "' ";
                                command.CommandTimeout = int.MaxValue;
                                command.CommandText = sql;
                                kq = command.ExecuteNonQuery();

                                sql = " DELETE DanhSachNhanSu_LichSuQuanLy WHERE quanly_fk = '" + id + "' ";
                                command.CommandTimeout = int.MaxValue;
                                command.CommandText = sql;
                                kq = command.ExecuteNonQuery();

                                sql = " DELETE DanhSachNhanSu_LichSuQuanLy WHERE nhansu_fk = '" + id + "' ";
                                command.CommandTimeout = int.MaxValue;
                                command.CommandText = sql;
                                kq = command.ExecuteNonQuery();

                            }

                            sql = "UPDATE DanhSachNhanSu SET hientrang = 1, trangthai = 1, phongban_fk = phongbanGoc_fk WHERE pk_seq = " + id + " ";
                            command.CommandTimeout = int.MaxValue;
                            command.CommandText = sql;
                            kq = command.ExecuteNonQuery();
                        }
                        else 
                        {
                            // 
                            sql = " DELETE DanhSachNhanSu_LichSuQuanLy WHERE quanly_fk = '" + id + "' ";
                            command.CommandTimeout = int.MaxValue;
                            command.CommandText = sql;
                            kq = command.ExecuteNonQuery();

                            sql = " DELETE DanhSachNhanSu_LichSuQuanLy WHERE nhansu_fk = '" + id + "' ";
                            command.CommandTimeout = int.MaxValue;
                            command.CommandText = sql;
                            kq = command.ExecuteNonQuery();

                            sql = "UPDATE DanhSachNhanSu SET hientrang = 1, trangthai = 1, phongban_fk = phongbanGoc_fk WHERE pk_seq = " + id + " ";
                            command.CommandTimeout = int.MaxValue;
                            command.CommandText = sql;
                            kq = command.ExecuteNonQuery();
                        }

                        dt.Clone();
                        dt.Clear();
                    }
                }
               
                sql = "UPDATE DanhSachNhanSu SET trangthai = N'" + trangthai + "', phongbanSupport_fk = '" + phongbanSupport_fk + "', tungay = N'" + tungay + "', denngay = N'" + denngay + "', lydo = N'" + lydo + "', nguoisua =  N'" + nguoitao + "', ngaysua = GETDATE() " +
                  " WHERE pk_seq = '" + id + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "2.Error! Cannot updated this staff.";
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                connect.Close();
                return "3.Error! Cannot updated this staff.";
            }

            connect.Close();
            return "";
        }

        public string GET_InformationStaff(string id)
        {
            string info = "";

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@"));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            string sql = "SELECT pk_seq, ma, ten, trangthai, diachi, dienthoai, gioitinh, mail, ngaysinh, chinhanh_fk, phongbanGoc_fk, phongbanSupport_fk, chucvu_fk, " +
                " ISNULL(ngaybatdaulam, '') ngaybatdaulam, ISNULL(tungay, '') tungay, ISNULL(denngay, '') denngay, ISNULL(lydo, '') lydo, ISNULL(hinhanh, 'avatardefault.png') hinhanh " +
                " FROM DanhSachNhanSu WHERE pk_seq = '" + id + "' ";
            SqlCommand command = new SqlCommand(sql, connect, transaction);
            command.CommandTimeout = int.MaxValue;
            command.CommandText = sql;
            SqlDataReader obj = command.ExecuteReader();
            if (obj != null)
            {
                DataTable dt = new DataTable();
                dt.Load(obj);

                info = dt.Rows[0]["pk_seq"].ToString() + " -- " + dt.Rows[0]["ten"].ToString() + " -- " + dt.Rows[0]["diachi"].ToString() + " -- " +
                    dt.Rows[0]["dienthoai"].ToString() + " -- " + dt.Rows[0]["gioitinh"].ToString() + " -- " + dt.Rows[0]["mail"].ToString() + " -- " +
                    dt.Rows[0]["ngaysinh"].ToString() + " -- " + dt.Rows[0]["ngaybatdaulam"].ToString() + " -- " + dt.Rows[0]["trangthai"].ToString() + " -- " +
                    dt.Rows[0]["chinhanh_fk"].ToString() + " -- " + dt.Rows[0]["phongbanGoc_fk"].ToString() + " -- " + dt.Rows[0]["phongbanSupport_fk"].ToString() + " -- " + 
                    dt.Rows[0]["chucvu_fk"].ToString() + " -- " + dt.Rows[0]["tungay"].ToString() + " -- " + dt.Rows[0]["denngay"].ToString() + " -- " + 
                    dt.Rows[0]["lydo"].ToString() + " -- " + dt.Rows[0]["hinhanh"].ToString();

                dt.Clone();
                dt.Clear();
            }

            return info;
        }
        
    }
}