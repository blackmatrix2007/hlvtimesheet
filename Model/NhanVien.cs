using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using HLVTimeSheet.AcsessData;

namespace HLVTimeSheet.Model
{
    public class NhanVien
    {

        public string LayThongTin(string id)
        {
            string info = "";

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            string sql = "SELECT pk_seq, dangnhap, hoten, dienthoai, congty, convert(varchar(10), ngaytao, 103) as ngaytao " +
                         " FROM NHANVIEN WHERE pk_seq = '" + id + "' ";
            SqlCommand command = new SqlCommand(sql, connect, transaction);
            command.CommandTimeout = int.MaxValue;
            command.CommandText = sql;
            SqlDataReader obj = command.ExecuteReader();
            if (obj != null)
            {
                DataTable dt = new DataTable();
                dt.Load(obj);

                info = dt.Rows[0]["dangnhap"].ToString() + " -- " + dt.Rows[0]["hoten"].ToString() + " -- " + dt.Rows[0]["dienthoai"].ToString() + " -- " + dt.Rows[0]["congty"].ToString();

                dt.Clone();
                dt.Clear();
            }

            return info;
        }

        public string ThemMoi(string Tendangnhap, string Hovaten, string Dienthoai, string Congty, string Matkhau, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                int codangnhap = 1;
                string sql = "SELECT COUNT(*) FROM NhanVien WHERE dangnhap = N'" + Tendangnhap + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (obj != null && obj.ToString().Trim().Length > 0)
                    codangnhap = int.Parse(obj.ToString());

                if (codangnhap >= 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Tên đăng nhập: " + Tendangnhap + " đã tồn tại trong hệ thống.";
                }

                sql = "INSERT NhanVien( dangnhap, hoten, dienthoai, congty, matkhau, trangthai, nguoitao, nguoisua ) " +
                    "SELECT N'" + Tendangnhap + "', N'" + Hovaten + "', N'" + Dienthoai + "', N'" + Congty + "', PWDENCRYPT('" + Matkhau + "'), '1', '" + userId + "', '" + userId + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Không thể thêm mới nhân viên: " + sql;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Không thể thêm mới nhân viên: " + ex.Message;
            }

            connect.Close();

            return "";
        }

        public string INSERET_Account_PC(string dangnhap, string matkhau, string ten, string trangthai, string phanloai, string diachi, string dienthoai, string mail, string ngonngu, string token, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabasePC("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                string sql = "SELECT COUNT(*) FROM NhanVien WHERE dangnhap = N'" + dangnhap + "' AND token != N'" + token + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) > 0)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Tên đăng nhập: " + dangnhap + " đã tồn tại trong hệ thống. OC " ;
                }

                sql = "SELECT COUNT(*) FROM NhanVien WHERE token = N'" + token + "' ";                
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                obj = command.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) > 0)
                {
                    sql = "UPDATE NhanVien SET ma = N'" + dangnhap + "', ten = N'" + ten + "', trangthai = '" + trangthai + "', trungtam = '" + phanloai + "', diachi = N'" + diachi + "', " +
                    "  dienthoai = N'" + dienthoai + "', mail = N'" + mail + "', dangnhap = N'" + dangnhap + "', " +
                    "  ngonngu = '" + ngonngu + "', nguoisua = '" + userId + "', ngaysua = getdate() ";
                    if (matkhau.Length > 0)
                        sql += ", matkhau = pwdencrypt(N'" + matkhau + "') ";

                    sql += " WHERE token = N'" + token + "' ";
                }
                else
                {
                    sql = "INSERT NhanVien(ma, ten, trangthai, diachi, dienthoai, mail, trungtam, khoanhanvien, khachhang_fk , dangnhap, matkhau, ngonngu, token, nguoitao, nguoisua)  " +
                    "SELECT N'" + dangnhap + "', N'" + ten + "', '" + trangthai + "', N'" + diachi + "', '" + dienthoai + "', N'" + mail + "', '" + phanloai + "', 0, 0, N'" + dangnhap + "', pwdencrypt(N'" + matkhau + "'), '" + ngonngu + "', N'" + token + "', '" + userId + "', '" + userId + "' ";
                }
                
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Không thể thêm mới nhân viên: " + sql;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Không thể thêm mới nhân viên: " + ex.Message;
            }

            connect.Close();

            return "";
        }

        public string INSERET_Account_OC(string dangnhap, string matkhau, string ten, string trangthai, string phanloai, string diachi, string dienthoai, string mail, string ngonngu, string token, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabaseOC("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                string sql = "SELECT COUNT(*) FROM NhanVien WHERE dangnhap = N'" + dangnhap + "' AND token != N'" + token + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) > 0)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Tên đăng nhập: " + dangnhap + " đã tồn tại trong hệ thống. OC ";
                }

                sql = "SELECT COUNT(*) FROM NhanVien WHERE token = N'" + token + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                obj = command.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) > 0)
                {
                    sql = "UPDATE NhanVien SET ma = N'" + dangnhap + "', ten = N'" + ten + "', trangthai = '" + trangthai + "', trungtam = '" + phanloai + "', diachi = N'" + diachi + "', " +
                            "  dienthoai = N'" + dienthoai + "', mail = N'" + mail + "', dangnhap = N'" + dangnhap + "', " +
                            "  ngonngu = '" + ngonngu + "', nguoisua = '" + userId + "', ngaysua = getdate() ";
                    if (matkhau.Length > 0)
                        sql += ", matkhau = pwdencrypt(N'" + matkhau + "') ";

                    sql += " WHERE token = N'" + token + "' ";
                }
                else
                {
                    sql = "INSERT NhanVien(ma, ten, trangthai, diachi, dienthoai, mail, trungtam, khoanhanvien, khachhang_fk , dangnhap, matkhau, ngonngu, token, nguoitao, nguoisua)  " +
                    "SELECT N'" + dangnhap + "', N'" + ten + "', '" + trangthai + "', N'" + diachi + "', '" + dienthoai + "', N'" + mail + "', '" + phanloai + "', 0, 0, N'" + dangnhap + "', pwdencrypt(N'" + matkhau + "'), '" + ngonngu + "', N'" + token + "', '" + userId + "', '" + userId + "' ";
                }

                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Không thể thêm mới nhân viên: " + sql;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Không thể thêm mới nhân viên: " + ex.Message;
            }

            connect.Close();

            return "";
        }

        public string INSERET_Account_WS(string dangnhap, string matkhau, string ten, string trangthai, string phanloai, string diachi, string dienthoai, string mail, string ngonngu, string token, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabaseWS("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                string sql = "SELECT COUNT(*) FROM NhanVien WHERE dangnhap = N'" + dangnhap + "' AND token != N'" + token + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) > 0)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Tên đăng nhập: " + dangnhap + " đã tồn tại trong hệ thống. WS ";
                }

                sql = "SELECT COUNT(*) FROM NhanVien WHERE token = N'" + token + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                obj = command.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) > 0)
                {
                    sql = "UPDATE NhanVien SET ma = N'" + dangnhap + "', ten = N'" + ten + "', trangthai = '" + trangthai + "', trungtam = '" + phanloai + "', diachi = N'" + diachi + "', " +
                            "  dienthoai = N'" + dienthoai + "', mail = N'" + mail + "', dangnhap = N'" + dangnhap + "', " +
                            "  ngonngu = '" + ngonngu + "', nguoisua = '" + userId + "', ngaysua = getdate() ";
                    if (matkhau.Length > 0)
                        sql += ", matkhau = pwdencrypt(N'" + matkhau + "') ";

                    sql += " WHERE token = N'" + token + "' ";
                }
                else
                {
                    sql = "INSERT NhanVien(ma, ten, trangthai, diachi, dienthoai, mail, trungtam, khoanhanvien, khachhang_fk , dangnhap, matkhau, ngonngu, token, nguoitao, nguoisua)  " +
                    "SELECT N'" + dangnhap + "', N'" + ten + "', '" + trangthai + "', N'" + diachi + "', '" + dienthoai + "', N'" + mail + "', '" + phanloai + "', 0, 0, N'" + dangnhap + "', pwdencrypt(N'" + matkhau + "'), '" + ngonngu + "', N'" + token + "', '" + userId + "', '" + userId + "' ";
                }

                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Không thể thêm mới nhân viên: " + sql;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Không thể thêm mới nhân viên: " + ex.Message;
            }

            connect.Close();

            return "";
        }

        public string INSERET_Account_WHVinhPhuc(string dangnhap, string matkhau, string ten, string trangthai, string phanloai, string diachi, string dienthoai, string mail, string ngonngu, string token, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabaseWHVP("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                string sql = "SELECT COUNT(*) FROM NhanVien WHERE dangnhap = N'" + dangnhap + "' AND token != N'" + token + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) > 0)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Tên đăng nhập: " + dangnhap + " đã tồn tại trong hệ thống. VHVP ";
                }

                sql = "SELECT COUNT(*) FROM NhanVien WHERE token = N'" + token + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                obj = command.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) > 0)
                {
                    sql = "UPDATE NhanVien SET ma = N'" + dangnhap + "', ten = N'" + ten + "', trangthai = '" + trangthai + "', trungtam = '" + phanloai + "', diachi = N'" + diachi + "', " +
                    "  dienthoai = N'" + dienthoai + "', mail = N'" + mail + "', dangnhap = N'" + dangnhap + "', " +
                    "  ngonngu = '" + ngonngu + "', nguoisua = '" + userId + "', ngaysua = getdate() ";
                    if (matkhau.Length > 0)
                        sql += ", matkhau = pwdencrypt(N'" + matkhau + "') ";

                    sql += " WHERE token = N'" + token + "' ";
                }
                else
                {
                    sql = "INSERT NhanVien(ma, ten, trangthai, diachi, dienthoai, mail, trungtam, khoanhanvien, khachhang_fk , dangnhap, matkhau, ngonngu, token, nguoitao, nguoisua)  " +
                    "SELECT N'" + dangnhap + "', N'" + ten + "', '" + trangthai + "', N'" + diachi + "', '" + dienthoai + "', N'" + mail + "', '" + phanloai + "', 0, 0, N'" + dangnhap + "', pwdencrypt(N'" + matkhau + "'), '" + ngonngu + "', N'" + token + "', '" + userId + "', '" + userId + "' ";
                }

                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Không thể thêm mới nhân viên: " + sql;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Không thể thêm mới nhân viên: " + ex.Message;
            }

            connect.Close();

            return "";
        }

        public string INSERET_Account_NWHaiPhong(string dangnhap, string matkhau, string ten, string trangthai, string phanloai, string diachi, string dienthoai, string mail, string ngonngu, string token, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabaseNWHP("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                string sql = "SELECT COUNT(*) FROM NhanVien WHERE dangnhap = N'" + dangnhap + "' AND token != N'" + token + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) > 0)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Tên đăng nhập: " + dangnhap + " đã tồn tại trong hệ thống. VHVP ";
                }

                sql = "SELECT COUNT(*) FROM NhanVien WHERE token = N'" + token + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                obj = command.ExecuteScalar();
                if (obj != null && int.Parse(obj.ToString()) > 0)
                {
                    sql = "UPDATE NhanVien SET ma = N'" + dangnhap + "', ten = N'" + ten + "', trangthai = '" + trangthai + "', trungtam = '" + phanloai + "', diachi = N'" + diachi + "', " +
                    "  dienthoai = N'" + dienthoai + "', mail = N'" + mail + "', dangnhap = N'" + dangnhap + "', " +
                    "  ngonngu = '" + ngonngu + "', nguoisua = '" + userId + "', ngaysua = getdate() ";
                    if (matkhau.Length > 0)
                        sql += ", matkhau = pwdencrypt(N'" + matkhau + "') ";

                    sql += " WHERE token = N'" + token + "' ";
                }
                else
                {
                    sql = "INSERT NhanVien(ma, ten, trangthai, diachi, dienthoai, mail, trungtam, khoanhanvien, khachhang_fk , dangnhap, matkhau, ngonngu, token, nguoitao, nguoisua)  " +
                    "SELECT N'" + dangnhap + "', N'" + ten + "', '" + trangthai + "', N'" + diachi + "', '" + dienthoai + "', N'" + mail + "', '" + phanloai + "', 0, 0, N'" + dangnhap + "', pwdencrypt(N'" + matkhau + "'), '" + ngonngu + "', N'" + token + "', '" + userId + "', '" + userId + "' ";
                }

                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Không thể thêm mới nhân viên: " + sql;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Không thể thêm mới nhân viên: " + ex.Message;
            }

            connect.Close();

            return "";
        }

        public string ChangePassword_PC(string dangnhap, string matkhau, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabasePC("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                string sql = "UPDATE NhanVien SET  matkhau = pwdencrypt('" + matkhau + "'), nguoisua = '" + userId + "', ngaysua = getdate() WHERE pk_seq = '" + userId + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Không thể thêm mới nhân viên: " + sql;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Không thể thêm mới nhân viên: " + ex.Message;
            }

            connect.Close();

            return "";
        }

        public string ChangePassword_OC(string dangnhap, string matkhau, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabaseOC("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                string sql = "UPDATE NhanVien SET  matkhau = pwdencrypt('" + matkhau + "'), nguoisua = '" + userId + "', ngaysua = getdate() WHERE pk_seq = '" + userId + "' ";                
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Không thể thêm mới nhân viên: " + sql;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Không thể thêm mới nhân viên: " + ex.Message;
            }

            connect.Close();

            return "";
        }

        public string ChangePassword_WS(string dangnhap, string matkhau, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabaseWS("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                string sql = "UPDATE NhanVien SET  matkhau = pwdencrypt('" + matkhau + "'), nguoisua = '" + userId + "', ngaysua = getdate() WHERE pk_seq = '" + userId + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Không thể thêm mới nhân viên: " + sql;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Không thể thêm mới nhân viên: " + ex.Message;
            }

            connect.Close();

            return "";
        }

        public string ChangePassword_WHVP(string dangnhap, string matkhau, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabaseWHVP("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                string sql = "UPDATE NhanVien SET  matkhau = pwdencrypt('" + matkhau + "'), nguoisua = '" + userId + "', ngaysua = getdate() WHERE pk_seq = '" + userId + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Không thể thêm mới nhân viên: " + sql;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Không thể thêm mới nhân viên: " + ex.Message;
            }

            connect.Close();

            return "";
        }

        public string ChangePassword_NWHP(string dangnhap, string matkhau, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabaseNWHP("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                string sql = "UPDATE NhanVien SET  matkhau = pwdencrypt('" + matkhau + "'), nguoisua = '" + userId + "', ngaysua = getdate() WHERE pk_seq = '" + userId + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Không thể thêm mới nhân viên: " + sql;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Không thể thêm mới nhân viên: " + ex.Message;
            }

            connect.Close();

            return "";
        }

        public string ChangePassword_BWHP(string dangnhap, string matkhau, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabaseBWHP("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                string sql = "UPDATE NhanVien SET  matkhau = pwdencrypt('" + matkhau + "'), nguoisua = '" + userId + "', ngaysua = getdate() WHERE pk_seq = '" + userId + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Không thể thêm mới nhân viên: " + sql;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Không thể thêm mới nhân viên: " + ex.Message;
            }

            connect.Close();

            return "";
        }

        public string CapNhat(string id, string Tendangnhap, string Hovaten, string Dienthoai, string Congty, string Matkhau, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            string sql = "";
            try
            {
                int codangnhap = 1;
                sql = "SELECT COUNT(*) FROM NhanVien WHERE dangnhap = N'" + Tendangnhap + "' and pk_seq != '" + id + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (obj != null && obj.ToString().Trim().Length > 0)
                    codangnhap = int.Parse(obj.ToString());

                if (codangnhap >= 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Tên đăng nhập: " + Tendangnhap + " đã tồn tại trong hệ thống.";
                }

                sql = "UPDATE NhanVien SET dangnhap = N'" + Tendangnhap + "', hoten = N'" + Hovaten + "', dienthoai = N'" + Dienthoai + "', congty = N'" + Congty + "', " +
                      " nguoisua = '" + userId + "', ngaysua = getdate() ";

                if (Matkhau.Trim().Length > 0)
                    sql += " , matkhau = PWDENCRYPT('" + Matkhau + "') ";

                sql += " WHERE PK_SEQ = '" + id + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Error! Cannot updated data... nhân viên: " + sql;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Error! Cannot updated data... nhân viên: " + ex.Message;
            }

            connect.Close();

            return "";
        }
    }
}