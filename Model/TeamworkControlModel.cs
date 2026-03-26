using HLVTimeSheet.AcsessData;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HLVTimeSheet.Model
{
    public class TeamworkControlModel
    {
        public string INSERT_Teamwork(string ma, string ten, string quanly_fk, string ghichu, string nhansu, string trangthai, string nguoitao)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@"));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = 0;
            try
            {
                // Kiểm tra ngày nhập, giờ bắt đầu
                string sql = "SELECT COUNT(*) FROM NhomLamViec WHERE ten = N'" + ten + "' AND quanly_fk = '" + quanly_fk + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (obj != null)
                {
                    if (int.Parse(obj.ToString()) > 0)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "1.Error! The teamwork . ";
                    }
                }

                sql = "INSERT NhomLamViec( ten, quanly_fk, ghichu, trangthai, nguoitao, nguoisua) " +
                    " SELECT N'" + ten + "', N'" + quanly_fk + "', N'" + ghichu + "', '" + trangthai + "', '" + nguoitao + "', '" + nguoitao + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "2.Error! Cannot created new this teamwork.";
                }

                sql = "SELECT IDENT_CURRENT('NhomLamViec')";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                string nhomlamviec_fk = command.ExecuteScalar().ToString();

                sql = "UPDATE NhomLamViec SET ma = N'" + nhomlamviec_fk + "' " +
                   " WHERE pk_seq = '" + nhomlamviec_fk + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();

                if (nhansu.Length > 3)
                {
                    //SELECT * FROM DanhSachNhanSu WHERE nhanvien_fk = 0
                    sql = "INSERT NhomLamViec_NhanSu(nhomlamviec_fk, nhansu_fk) " +
                    " SELECT '" + nhomlamviec_fk + "', pk_seq FROM DanhSachNhanSu WHERE pk_seq IN (" + nhansu + ") ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "2.Error! Cannot created new this teamwork.";
                    }

                    sql = "INSERT NhomLamViec_NhanSu(nhomlamviec_fk, nhansu_fk) " +
                    " SELECT '" + nhomlamviec_fk + "', '" + quanly_fk + "' ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "2.Error! Cannot created new this teamwork.";
                    }
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                connect.Close();
                return "3.Error! Cannot created new this teamwork.";
            }

            connect.Close();
            return "";
        }

        public string UPDATE_Teamwork(string id, string ma, string ten, string quanly_fk, string ghichu, string nhansu, string trangthai, string nguoitao)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@"));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = 0;
            try
            {
                // Kiểm tra ngày nhập, giờ bắt đầu
                string sql = "SELECT COUNT(*) FROM NhomLamViec WHERE ten = N'" + ten + "' AND quanly_fk = '" + quanly_fk + "' AND pk_seq != '" + id + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (obj != null)
                {
                    if (int.Parse(obj.ToString()) > 0)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "1.Error! The teamwork . ";
                    }
                }

                sql = "UPDATE NhomLamViec SET ma = N'" + ma + "', ten = N'" + ten + "', quanly_fk = N'" + quanly_fk + "', ghichu = N'" + ghichu + "', trangthai = '" + trangthai + "', nguoisua = '" + nguoitao + "', ngaysua = GETDATE() " +
                    " WHERE pk_seq = '" + id + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "2.Error! Cannot updated teamwork.";
                }

                string nhomlamviec_fk = id;

                sql = "DELETE NhomLamViec_NhanSu WHERE nhomlamviec_fk = '" + nhomlamviec_fk + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();

                if (nhansu.Length > 3)
                {
                    //SELECT * FROM DanhSachNhanSu WHERE nhanvien_fk = 0
                    sql = "INSERT NhomLamViec_NhanSu(nhomlamviec_fk, nhansu_fk) " +
                   " SELECT '" + nhomlamviec_fk + "', pk_seq FROM DanhSachNhanSu WHERE pk_seq IN (" + nhansu + ") ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "2.Error! Cannot updated teamwork.";
                    }
                }

                sql = "INSERT NhomLamViec_NhanSu(nhomlamviec_fk, nhansu_fk) " +
                   " SELECT '" + nhomlamviec_fk + "', '" + quanly_fk + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "2.Error! Cannot created new this teamwork.";
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                connect.Close();
                return "3.Error! Cannot updated this teamwork.";
            }

            connect.Close();
            return "";
        }

        public string DELETE_Teamwork(string id, string nguoitao)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@"));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = 0;
            try
            {
                string sql = "UPDATE NhomLamViec SET  trangthai = '0', nguoisua = N'" + nguoitao + "', ngaysua = GETDATE() " +
                    " WHERE pk_seq = '" + id + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "2.Error! Cannot updated this teamwork.";
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                connect.Close();
                return "3.Error! Cannot updated this teamwork.";
            }

            connect.Close();
            return "";
        }
    }
}