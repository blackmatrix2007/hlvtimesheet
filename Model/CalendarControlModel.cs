using HLVTimeSheet.AcsessData;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Text.RegularExpressions;

namespace HLVTimeSheet.Model
{
    public class CalendarControlModel
    {
        public string GetInforDatetime(string thoigian, string phongban_fk)
        {
            string info = "";

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            string ngay = thoigian.Substring(0, 2);
            string thang = thoigian.Substring(3, 2);
           
            string sql = "SELECT pk_seq, ngaynghi, denngay, loaingaynghi_fk, hangnam, trangthai " +
                         " FROM NgayNghiLe_PhongBan WHERE CONVERT(datetime, ngaynghi, 105) <= CONVERT(datetime, '" + thoigian + "', 105) AND CONVERT(datetime, denngay, 105) >= CONVERT(datetime, '" + thoigian + "', 105) AND phongban_fk = '" + phongban_fk + "' AND hangnam = '0' " +
                         " UNION ALL " +
                         " SELECT pk_seq, ngaynghi, denngay, loaingaynghi_fk, hangnam, trangthai " +
                         " FROM NgayNghiLe_PhongBan WHERE ngay = '" + ngay + "' AND thang = N'" + thang + "' AND phongban_fk = 0 AND hangnam = '1' ";
            SqlCommand command = new SqlCommand(sql, connect, transaction);
            command.CommandTimeout = int.MaxValue;
            command.CommandText = sql;
            SqlDataReader obj = command.ExecuteReader();
            if (obj != null)
            {
                DataTable dt = new DataTable();
                dt.Load(obj);
                if(dt.Rows.Count > 0)
                {                                       
                    info = dt.Rows[0]["pk_seq"].ToString() + " -- " + thoigian + " -- " + dt.Rows[0]["loaingaynghi_fk"].ToString();
                }                    
                dt.Clone();
                dt.Clear();
            }

            transaction.Commit();

            return info;
        }

        public string UpdateSetDate(string thoigian, string phongban_fk, string loaingaynghi_fk, string trangthai, string userId)
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
                string sql = "SELECT COUNT(*) FROM NgayNghiLe_PhongBan WHERE CONVERT(datetime, ngaynghi, 105) <= CONVERT(datetime, '" + thoigian + "', 105) AND CONVERT(datetime, denngay, 105) >= CONVERT(datetime, '" + thoigian + "', 105) AND phongban_fk = '" + phongban_fk + "' AND hangnam = '0' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (obj != null && obj.ToString().Trim().Length > 0)
                    coma = int.Parse(obj.ToString());
                
                if (coma >= 1)
                {
                    sql = "UPDATE NgayNghiLe_PhongBan SET loaingaynghi_fk = N'" + loaingaynghi_fk + "', nguoisua = N'" + userId + "', ngaysua = getdate() " +
                         " WHERE CONVERT(datetime, ngaynghi, 105) <= CONVERT(datetime, '" + thoigian + "', 105) AND CONVERT(datetime, denngay, 105) >= CONVERT(datetime, '" + thoigian + "', 105) AND phongban_fk = '" + phongban_fk + "' AND hangnam = '0' ";
                }
                else
                {
                    sql = "INSERT NgayNghiLe_PhongBan(ngaynghi, denngay, phongban_fk, loaingaynghi_fk, trangthai, nguoitao, nguoisua) " +
                        " SELECT N'" + thoigian + "', N'" + thoigian + "', '" + phongban_fk + "', '" + loaingaynghi_fk + "', N'" + trangthai + "', '" + userId + "', '" + userId + "' ";
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

        public string DeleteSetDate(string thoigian, string phongban_fk)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();
            try
            {
                string sql = "DELETE NgayNghiLe_PhongBan WHERE ngaynghi = '" + thoigian + "' AND phongban_fk = '" + phongban_fk + "' AND hangnam = 0 ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                int kq = command.ExecuteNonQuery();
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

        public string INSERT_WorkingHourse(string phongban_fk, string gioStart, string phutStart, string gioEnd, string phutEnd, string trangthai, string userId)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {

                string[] arrGioStart = Regex.Split(gioStart, ";");
                string[] arrPhutStart = Regex.Split(phutStart, ";");
                string[] arrGioEnd = Regex.Split(gioEnd, ";");
                string[] arrPhutEnd = Regex.Split(phutEnd, ";");
                
                for (int i = 0; i < arrGioStart.Length; i++)
                {
                    string _gioStart = arrGioStart[i];
                    string _phutStart = arrPhutStart[i];
                    string _gioEnd = arrGioEnd[i];
                    string _phutEnd = arrPhutEnd[i];

                    string ten = "";
                    string loai = (i + 1).ToString();

                    string sql = "SELECT COUNT(*) FROM GioLamViec WHERE phongban_fk = '" + phongban_fk + "' AND loai = '" + loai + "' ";
                    SqlCommand command = new SqlCommand(sql, connect, transaction);
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    object obj = command.ExecuteScalar();
                    if (obj != null && int.Parse(obj.ToString()) > 0)                    
                    {
                        sql = "UPDATE GioLamViec SET gioStart = N'" + _gioStart + "', phutStart = '" + _phutStart + "', gioEnd = '" + _gioEnd + "', phutEnd = '" + _phutEnd + "', nguoisua = N'" + userId + "', ngaysua = getdate() " +
                             " WHERE phongban_fk = '" + phongban_fk + "' AND loai = '" + loai + "' ";
                        command.CommandTimeout = int.MaxValue;
                        command.CommandText = sql;
                        kq = command.ExecuteNonQuery();
                        if (kq < 1)
                        {
                            transaction.Rollback();
                            connect.Close();
                            return "Error! Data cannot updated...: " + sql;
                        }
                    }
                    else
                    {
                        switch (loai)
                        {
                            case "1":
                                ten = "Working time";
                                break;
                            case "2":
                                ten = "Break 01";
                                break;
                            case "3":
                                ten = "Lunch Break";
                                break;
                            case "4":
                                ten = "Break 02";
                                break;
                            case "5":
                                ten = "Over time";
                                break;
                            default:
                                break;
                        }
                        sql = "INSERT GioLamViec(phongban_fk, ten, loai, gioStart, phutStart, gioEnd, phutEnd, trangthai, nguoitao, nguoisua) " +
                            " SELECT '" + phongban_fk + "', N'" + ten + "', N'" + loai + "', '" + _gioStart + "', '" + _phutStart + "', '" + _gioEnd + "', '" + _phutEnd + "', '" + trangthai + "', '" + userId + "', '" + userId + "' ";
                        command.CommandTimeout = int.MaxValue;
                        command.CommandText = sql;
                        kq = command.ExecuteNonQuery();
                        if (kq < 1)
                        {
                            transaction.Rollback();
                            connect.Close();
                            return "Error! Data cannot updated...: " + sql;
                        }
                    }                    
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