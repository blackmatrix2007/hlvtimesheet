using HLVTimeSheet.AcsessData;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace HLVTimeSheet.Model
{
    public class MasterControlModel
    {
        public string SAVE_Symbol(string symbol, string nhansu_fk, string nguoitao)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@"));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = 0;
            try
            {
                if (symbol.Length > 0)
                    symbol = symbol.Substring(0, symbol.Length - 1);

                string sql = "DELETE KyHieu_NhanSu_Color WHERE nhansu_fk = '" + nhansu_fk + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();

                string[] syArr = Regex.Split(symbol, ";");
                for (int i = 0; i < syArr.Length; i++)
                {
                    if (syArr[i].Trim().Length > 0)
                    {
                        string[] sy = Regex.Split(syArr[i], "_");

                        // Kiểm tra mã có bị trùng không

                        if (sy[0].Trim().Length < 3) // 
                        {
                            sql = "INSERT KyHieu(ma, ten, trangthai, nguoitao, nguoisua) " +
                                " SELECT N'" + sy[1].Trim() + "', N'" + sy[2].Trim() + "', N'" + sy[4].Trim() + "', '" + nguoitao + "', '" + nguoitao + "'  ";
                            command.CommandTimeout = int.MaxValue;
                            command.CommandText = sql;
                            kq = command.ExecuteNonQuery();
                            if (kq < 1)
                            {
                                transaction.Rollback();
                                connect.Close();
                                return "1.Error! Cannot created new this symbol." + sy[1].Trim();
                            }

                            sql = "SELECT IDENT_CURRENT('KyHieu')";
                            command.CommandTimeout = int.MaxValue;
                            command.CommandText = sql;
                            string kyhieu_fk = command.ExecuteScalar().ToString();

                            // Tạo màu cho loạt nhân sự
                            sql = "INSERT KyHieu_NhanSu_Color(kyhieu_fk, nhansu_fk, color) " +
                                " SELECT '" + kyhieu_fk + "', '" + nhansu_fk + "', N'#" + sy[3].Trim() + "' ";
                            command.CommandTimeout = int.MaxValue;
                            command.CommandText = sql;
                            kq = command.ExecuteNonQuery();
                            if (kq < 1)
                            {
                                transaction.Rollback();
                                connect.Close();
                                return "1.Error! Cannot created new this symbol." + sy[1].Trim();
                            }

                            // Tạo màu cho nhân sự còn lại
                            sql = "INSERT KyHieu_NhanSu_Color(kyhieu_fk, nhansu_fk, color) " +
                                " SELECT '" + kyhieu_fk + "', pk_seq, N'#" + sy[3].Trim() + "' FROM DanhSachNhanSu WHERE pk_seq != '" + nhansu_fk + "' ";
                            command.CommandTimeout = int.MaxValue;
                            command.CommandText = sql;
                            kq = command.ExecuteNonQuery();
                            //if (kq < 1)
                            //{
                            //    transaction.Rollback();
                            //    connect.Close();
                            //    return "1.Error! Cannot created new this symbol." + sy[1].Trim();
                            //}

                        }
                        else if (sy[0].Trim().Length > 3) // 
                        {                           
                            sql = "UPDATE KyHieu SET ma = N'" + sy[1].Trim() + "', ten = N'" + sy[2].Trim() + "', trangthai =  N'" + sy[4].Trim() + "', nguoisua = '" + nguoitao + "', ngaysua = GETDATE() " +
                                " WHERE pk_seq = N'" + sy[0].Trim() + "' ";
                            command.CommandTimeout = int.MaxValue;
                            command.CommandText = sql;
                            kq = command.ExecuteNonQuery();
                            if (kq < 1)
                            {
                                transaction.Rollback();
                                connect.Close();
                                return "1.Error! Cannot created new this symbol." + sy[1].Trim();
                            }

                            sql = "INSERT KyHieu_NhanSu_Color(kyhieu_fk, nhansu_fk, color) " +
                                 " SELECT N'" + sy[0].Trim() + "', '" + nhansu_fk + "', N'#" + sy[3].Trim() + "' ";
                            command.CommandTimeout = int.MaxValue;
                            command.CommandText = sql;
                            kq = command.ExecuteNonQuery();
                            if (kq < 1)
                            {
                                transaction.Rollback();
                                connect.Close();
                                return "1.Error! Cannot created new this symbol." + sy[1].Trim();
                            }
                        }
                    }
                }
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                connect.Close();
                return "4.Error! Cannot created new this symbol.";
            }

            connect.Close();
            return "";
        }

        public string DELETE_Symbol(string symbol, string nguoitao)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@"));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = 0;
            try
            {
                // Kiểm tra ký hiệu đã được sử dụng chưa
                // Chưa, thì delete, nếu đã có thì updated
                string sql = "SELECT COUNT(*) FROM LichLamViec WHERE kyhieu_fk = '" + symbol + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (obj != null)
                {
                    if (int.Parse(obj.ToString()) >= 0)
                    {
                        sql = "UPDATE KyHieu SET ma = ma + '" + DateTime.Now.ToString("ddMMyy") + "', ten = ten + '" + DateTime.Now.ToString("ddMMyy") + "', trangthai = 2, nguoisua = '" + nguoitao + "', ngaysua = GETDATE() " +
                            " WHERE pk_seq = '" + symbol + "' ";
                        command.CommandTimeout = int.MaxValue;
                        command.CommandText = sql;
                        kq = command.ExecuteNonQuery();
                        if (kq < 1)
                        {
                            transaction.Rollback();
                            connect.Close();
                            return "2.Error! Cannot updated this Symbol.";
                        }
                    }
                    else
                    {
                        sql = "DELETE KyHieu WHERE pk_seq = '" + symbol + "' ";
                        command.CommandTimeout = int.MaxValue;
                        command.CommandText = sql;
                        kq = command.ExecuteNonQuery();
                        if (kq < 1)
                        {
                            transaction.Rollback();
                            connect.Close();
                            return "2.Error! Cannot updated this Symbol.";
                        }

                        sql = "DELETE KyHieu_NhanSu_Color WHERE kyhieu_fk = '" + symbol + "' ";
                        command.CommandTimeout = int.MaxValue;
                        command.CommandText = sql;
                        kq = command.ExecuteNonQuery();
                        //if (kq < 1)
                        //{
                        //    transaction.Rollback();
                        //    connect.Close();
                        //    return "2.Error! Cannot updated this Symbol.";
                        //}
                    }

                    

                }


                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                connect.Close();
                return "4.Error! Cannot created new this symbol.";
            }

            connect.Close();
            return "";
        }

        public string SAVE_Holidays(string id, string tungay, string denngay, string hangnam, string noidung, string trangthhai, string nguoitao)
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
                string ngay = tungay.Substring(0, 2);
                string thang = tungay.Substring(3, 2);
                string nam = tungay.Substring(6, 4);

                if (id.Length < 3)
                    sql = "INSERT NgayNghiLe(ngaynghi, denngay, ngay, thang, nam, hangnam, noidung, trangthai, nguoitao, nguoisua)" +
                        " SELECT '" + tungay + "', '" + denngay + "', '" + ngay + "', '" + thang + "', '" + nam + "', '" + hangnam + "', N'" + noidung + "', '" + trangthhai + "', '" + nguoitao + "', '" + nguoitao + "' ";
                else
                    sql = "UPDATE NgayNghiLe SET ngaynghi = '" + tungay + "', denngay = '" + denngay + "', ngay = '" + ngay + "', thang = '" + thang + "', nam = '" + nam + "', hangnam = '" + hangnam + "', noidung = N'" + noidung + "', trangthai = '" + trangthhai + "', nguoisua = '" + nguoitao + "', ngaysua = GETDATE() WHERE pk_seq = '" + id + "' ";

                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                connect.Close();
                return "4.Error! Cannot created new this holidays.";
            }

            connect.Close();
            return "";
        }

        public string DELETE_Holidays(string id)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("103.159.50.204, 89", "GDC_HLV_WorkSchedule", "giangdc", "giangdc@"));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            try
            {                
                string sql = "DELETE NgayNghiLe WHERE pk_seq = '" + id + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                command.ExecuteNonQuery();
                
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                connect.Close();
                return "4.Error! Cannot created new this holidays.";
            }

            connect.Close();
            return "";
        }
    }
}