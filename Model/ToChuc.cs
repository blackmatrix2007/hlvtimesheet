using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using HLVTimeSheet.AcsessData;

namespace HLVTimeSheet.Model
{
    class ToChuc
    {
        public string GetInfor_Brand( string id )
        {
            string info = "";

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            string sql = "SELECT pk_seq, ma, ten, ISNULL(diachi, '') diachi, trangthai " +
                         " FROM NhaPhanPhoi WHERE pk_seq = '" + id + "' ";
            SqlCommand command = new SqlCommand(sql, connect, transaction);
            command.CommandTimeout = int.MaxValue;
            command.CommandText = sql;
            SqlDataReader obj = command.ExecuteReader();
            if (obj != null)
            {
                DataTable dt = new DataTable();
                dt.Load(obj);

                info = dt.Rows[0]["ma"].ToString() + " -- " + dt.Rows[0]["ten"].ToString() + " -- " + dt.Rows[0]["diachi"].ToString() + " -- " + dt.Rows[0]["trangthai"].ToString();

                dt.Clone();
                dt.Clear();
            }

            return info;
        }
        public string Update_Brand(string id, string ma, string ten, string diachi, string trangthai, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                int coma = 1;
                string sql = "SELECT COUNT(*) FROM NhaPhanPhoi WHERE ma = N'" + ma + "' ";
                if (id.Trim().Length > 0)
                    sql += " and pk_seq != '" + id + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (obj != null && obj.ToString().Trim().Length > 0)
                    coma = int.Parse(obj.ToString());

                if (coma >= 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Mã chi nhánh: " + ma + " đã tồn tại trong hệ thống.";
                }

                if (id.Trim().Length <= 0)
                {
                    sql = "INSERT NhaPhanPhoi( ma, ten, diachi, trangthai, nguoitao, nguoisua ) " +
                            "values( N'" + ma + "', N'" + ten + "', N'" + diachi + "', N'" + trangthai + "', '" + userId + "', '" + userId + "' ) ";
                }
                else
                {
                    sql = "UPDATE NhaPhanPhoi SET ma = N'" + ma + "', ten = N'" + ten + "', diachi = N'" + diachi + "', trangthai = '" + trangthai + "', nguoisua = '" + userId + "', ngaysua = getdate() " + 
                         " WHERE pk_seq = '" + id + "' ";
                }

                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Error! Data cannot updated...: " + sql;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Error! Data cannot updated...: " + ex.Message;
            }

            connect.Close();

            return "";
        }
        public string GetInfor_Warehouse(string id)
        {
            string info = "";

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            string sql = "SELECT pk_seq, makho, tenkho, ISNULL(diachi, '') diachi, ISNULL(khuvuc, '') khuvuc, ISNULL(loaikho, 0) loaikho, " +
                " ISNULL(thutu1, '0') thutu1, ISNULL(thutu2, '0') thutu2, ISNULL(thutu3, '0') thutu3, trangthai " +
                " FROM Kho WHERE pk_seq = '" + id + "' ";
            SqlCommand command = new SqlCommand(sql, connect, transaction);
            command.CommandTimeout = int.MaxValue;
            command.CommandText = sql;
            SqlDataReader obj = command.ExecuteReader();
            if (obj != null)
            {
                DataTable dt = new DataTable();
                dt.Load(obj);

                info = dt.Rows[0]["makho"].ToString() + " -- " + dt.Rows[0]["tenkho"].ToString() + " -- " + 
                    dt.Rows[0]["diachi"].ToString() + " -- " + dt.Rows[0]["khuvuc"].ToString() + " -- " + dt.Rows[0]["loaikho"].ToString() + " -- " +
                    dt.Rows[0]["thutu1"].ToString() + " -- " + dt.Rows[0]["thutu2"].ToString() + " -- " + dt.Rows[0]["thutu3"].ToString() + " -- " +
                    dt.Rows[0]["trangthai"].ToString();

                dt.Clone();
                dt.Clear();
            }

            return info;
        }
        public string Update_Warehouse(string id, string ma, string ten, string diachi, string khuvuc, string trangthai, string loaikho, string thutu1, string thutu2, string thutu3, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                int coma = 1;
                string sql = "SELECT COUNT(*) FROM Kho WHERE makho = N'" + ma + "' ";
                if (id.Trim().Length > 0)
                    sql += " and pk_seq != '" + id + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (obj != null && obj.ToString().Trim().Length > 0)
                    coma = int.Parse(obj.ToString());

                if (coma >= 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Mã kho: " + ma + " đã tồn tại trong hệ thống.";
                }

                string thutuxuat = "";
                if (thutu1.Trim().Length > 3)
                    thutuxuat += thutu1 + ",";
                if (thutu2.Trim().Length > 3)
                    thutuxuat += thutu2 + ",";
                if (thutu3.Trim().Length > 3)
                    thutuxuat += thutu3 + ",";

                if (thutuxuat.Trim().Length > 3)
                    thutuxuat = thutuxuat.Substring(0, thutuxuat.Length - 1);

                if (id.Trim().Length <= 0)
                {
                    sql = "INSERT Kho( makho, tenkho, diachi, khuvuc, trangthai, loaikho, thutuxuat, thutu1, thutu2, thutu3, nguoitao, nguoisua ) " +
                            "SELECT N'" + ma + "', N'" + ten + "', N'" + diachi + "', N'" + khuvuc + "', N'" + trangthai + "', '" + loaikho + "', N'" + thutuxuat + "', N'" + thutu1 + "', N'" + thutu2 + "', N'" + thutu3 + "', '" + userId + "', '" + userId + "' ";
                }
                else
                {
                    sql = "UPDATE Kho SET makho = N'" + ma + "', tenkho = N'" + ten + "', diachi = N'" + diachi + "', khuvuc = N'" + khuvuc + "', trangthai = '" + trangthai + "', " + 
                        " thutuxuat = N'" + thutuxuat + "', thutu1 = N'" + thutu1 + "', thutu2 = N'" + thutu2 + "', thutu3 = N'" + thutu3 + "' , " + 
                        " loaikho = '" + loaikho + "', nguoisua = '" + userId + "', ngaysua = getdate() " +
                         " WHERE pk_seq = '" + id + "' ";
                }
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Error! Data cannot updated...: " + sql;
                }

                // Tạo Kho_SanPham
                if (id.Trim().Length <= 0)
                {
                    sql = " INSERT Kho_SanPham(kho_fk, npp_fk, sanpham_fk, soluong, booked, avai, giaton) " +
                          " SELECT (SELECT IDENT_CURRENT('Kho')), b.pk_seq, c.pk_seq, 0, 0, 0, 0  " +
                          " FROM NhaPhanPhoi b, SanPham c ";

                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                    //if (kq < 1)
                    //{
                    //    transaction.Rollback();
                    //    connect.Close();
                    //    return "2.Error! Cannot updated data... hàng hóa";
                    //}
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Error! Data cannot updated...: " + ex.Message;
            }

            connect.Close();

            return "";
        }     
        public string GetInfor_Zone(string id)
        {
            string info = "";

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            string sql = "SELECT pk_seq, ma, ten, kho_fk, trangthai " +
                         " FROM Zone WHERE pk_seq = '" + id + "' ";
            SqlCommand command = new SqlCommand(sql, connect, transaction);
            command.CommandTimeout = int.MaxValue;
            command.CommandText = sql;
            SqlDataReader obj = command.ExecuteReader();
            if (obj != null)
            {
                DataTable dt = new DataTable();
                dt.Load(obj);

                info = dt.Rows[0]["ma"].ToString() + " -- " + dt.Rows[0]["ten"].ToString() + " -- " + 
                    dt.Rows[0]["kho_fk"].ToString() + " -- " + dt.Rows[0]["trangthai"].ToString();

                dt.Clone();
                dt.Clear();
            }

            return info;
        }
        public string Update_Zone(string id, string ma, string ten, string kho, string trangthai, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                int coma = 1;
                string sql = "SELECT COUNT(*) FROM Zone WHERE ma = N'" + ma + "' ";
                if (id.Trim().Length > 0)
                    sql += " and pk_seq != '" + id + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (obj != null && obj.ToString().Trim().Length > 0)
                    coma = int.Parse(obj.ToString());

                if (coma >= 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Mã zone: " + ma + " đã tồn tại trong hệ thống.";
                }

                if (id.Trim().Length <= 0)
                {
                    sql = " INSERT Zone( ma, ten, kho_fk, trangthai, nguoitao, nguoisua ) " +
                    " SELECT N'" + ma + "', N'" + ten + "', '" + kho + "', N'" + trangthai + "', '" + userId + "', '" + userId + "'  ";
                }
                else
                {
                    sql = "UPDATE Zone SET ma = N'" + ma + "', ten = N'" + ten + "', kho_fk = '" + kho + "', trangthai = '" + trangthai + "', nguoisua = '" + userId + "', ngaysua = getdate() " +
                         " WHERE pk_seq = '" + id + "' ";
                }
                
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Error! Data cannot updated...: " + sql;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Error! Data cannot updated...: " + ex.Message;
            }

            connect.Close();

            return "";
        }

        public string GetInfor_Position(string id)
        {
            string info = "";

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@"));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            string sql = "SELECT pk_seq, ma, ten, capbac, trangthai " +
                         " FROM ChucVu WHERE pk_seq = '" + id + "' ";
            SqlCommand command = new SqlCommand(sql, connect, transaction);
            command.CommandTimeout = int.MaxValue;
            command.CommandText = sql;
            SqlDataReader obj = command.ExecuteReader();
            if (obj != null)
            {
                DataTable dt = new DataTable();
                dt.Load(obj);

                info = dt.Rows[0]["ma"].ToString() + " -- " + dt.Rows[0]["ten"].ToString() + " -- " + dt.Rows[0]["capbac"].ToString() + " -- " + dt.Rows[0]["trangthai"].ToString();

                dt.Clone();
                dt.Clear();
            }

            return info;
        }
        public string Update_Position(string id, string ma, string ten, string capbac, string trangthai, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@"));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                int coma = 1;
                string sql = "SELECT COUNT(*) FROM ChucVu WHERE ma = N'" + ma + "' ";
                if (id.Trim().Length > 0)
                    sql += " and pk_seq != '" + id + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (obj != null && obj.ToString().Trim().Length > 0)
                    coma = int.Parse(obj.ToString());

                if (coma >= 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Mã chức vụ: " + ma + " đã tồn tại trong hệ thống.";
                }

                if (id.Trim().Length <= 0)
                {
                    sql = " INSERT ChucVu( ma, ten, capbac, trangthai, nguoitao, nguoisua ) " +
                    " SELECT N'" + ma + "', N'" + ten + "', '" + capbac + "', N'" + trangthai + "', '" + userId + "', '" + userId + "'  ";
                }
                else
                {
                    sql = "UPDATE ChucVu SET ma = N'" + ma + "', ten = N'" + ten + "', capbac = '" + capbac + "', trangthai = '" + trangthai + "', nguoisua = '" + userId + "', ngaysua = getdate() " +
                         " WHERE pk_seq = '" + id + "' ";
                }

                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Error! Data cannot updated...: " + sql;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Error! Data cannot updated...: " + ex.Message;
            }

            connect.Close();

            return "";
        }
        public string GetInfor_Department(string id)
        {
            string info = "";

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@"));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            string sql = "SELECT pk_seq, ma, ten, ISNULL(chinhanh_fk, 0) chinhanh_fk, ISNULL(bophan_fk, 0) bophan_fk, loai, trangthai " +
                         " FROM PhongBan WHERE pk_seq = '" + id + "' ";
            SqlCommand command = new SqlCommand(sql, connect, transaction);
            command.CommandTimeout = int.MaxValue;
            command.CommandText = sql;
            SqlDataReader obj = command.ExecuteReader();
            if (obj != null)
            {
                DataTable dt = new DataTable();
                dt.Load(obj);

                info = dt.Rows[0]["ma"].ToString() + " -- " + dt.Rows[0]["ten"].ToString() + " -- " +
                    dt.Rows[0]["chinhanh_fk"].ToString() + " -- " + dt.Rows[0]["bophan_fk"].ToString() + " -- " +
                    dt.Rows[0]["loai"].ToString() + " -- " + dt.Rows[0]["trangthai"].ToString();

                dt.Clone();
                dt.Clear();
            }

            return info;
        }
        public string Update_Department(string id, string ma, string ten, string chinhanh_fk, string bophan_fk, string loai, string trangthai, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@"));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                int coma = 1;
                string sql = "SELECT COUNT(*) FROM PhongBan WHERE ma = N'" + ma + "' ";
                if (id.Trim().Length > 0)
                    sql += " and pk_seq != '" + id + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (obj != null && obj.ToString().Trim().Length > 0)
                    coma = int.Parse(obj.ToString());

                if (coma >= 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Mã phòng ban: " + ma + " đã tồn tại trong hệ thống.";
                }

                if (id.Trim().Length <= 0)
                {
                    sql = " INSERT PhongBan( ma, ten, chinhanh_fk, bophan_fk, loai, trangthai, nguoitao, nguoisua ) " +
                    " SELECT N'" + ma + "', N'" + ten + "', N'" + chinhanh_fk + "', N'" + bophan_fk + "', '" + loai + "', N'" + trangthai + "', '" + userId + "', '" + userId + "'  ";
                }
                else
                {
                    sql = "UPDATE PhongBan SET ma = N'" + ma + "', ten = N'" + ten + "', chinhanh_fk = N'" + chinhanh_fk + "', bophan_fk = '" + bophan_fk + "', loai = '" + loai + "', trangthai = '" + trangthai + "', nguoisua = '" + userId + "', ngaysua = getdate() " +
                         " WHERE pk_seq = '" + id + "' ";
                }

                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Error! Data cannot updated...: " + sql;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Error! Data cannot updated...: " + ex.Message;
            }

            connect.Close();

            return "";
        }
        public string GetInfor_Division(string id)
        {
            string info = "";

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@"));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            string sql = "SELECT pk_seq, ma, ten, trangthai " +
                         " FROM BoPhan WHERE pk_seq = '" + id + "' ";
            SqlCommand command = new SqlCommand(sql, connect, transaction);
            command.CommandTimeout = int.MaxValue;
            command.CommandText = sql;
            SqlDataReader obj = command.ExecuteReader();
            if (obj != null)
            {
                DataTable dt = new DataTable();
                dt.Load(obj);

                info = dt.Rows[0]["ma"].ToString() + " -- " + dt.Rows[0]["ten"].ToString() + " -- " +
                    dt.Rows[0]["trangthai"].ToString();

                dt.Clone();
                dt.Clear();
            }

            return info;
        }
        public string Update_Division(string id, string ma, string ten, string trangthai, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@"));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                int coma = 1;
                string sql = "SELECT COUNT(*) FROM BoPhan WHERE ma = N'" + ma + "' ";
                if (id.Trim().Length > 0)
                    sql += " and pk_seq != '" + id + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (obj != null && obj.ToString().Trim().Length > 0)
                    coma = int.Parse(obj.ToString());

                if (coma >= 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Mã bộ phận: " + ma + " đã tồn tại trong hệ thống.";
                }

                if (id.Trim().Length <= 0)
                {
                    sql = " INSERT BoPhan( ma, ten, trangthai, nguoitao, nguoisua ) " +
                    " SELECT N'" + ma + "', N'" + ten + "', N'" + trangthai + "', '" + userId + "', '" + userId + "'  ";
                }
                else
                {
                    sql = "UPDATE BoPhan SET ma = N'" + ma + "', ten = N'" + ten + "', trangthai = '" + trangthai + "', nguoisua = '" + userId + "', ngaysua = getdate() " +
                         " WHERE pk_seq = '" + id + "' ";
                }

                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Error! Data cannot updated...: " + sql;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Error! Data cannot updated...: " + ex.Message;
            }

            connect.Close();

            return "";
        }
    }
}
