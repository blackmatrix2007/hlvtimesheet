using HLVTimeSheet.AcsessData;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace HLVTimeSheet.Model
{
    public class MasterDataModel
    {
        public string GetInfor_Unit(string id)
        {
            string info = "";

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            string sql = "SELECT pk_seq, ma, ten, trangthai " +
                         " FROM DONVITINH WHERE pk_seq = '" + id + "' ";
            SqlCommand command = new SqlCommand(sql, connect, transaction);
            command.CommandTimeout = int.MaxValue;
            command.CommandText = sql;
            SqlDataReader obj = command.ExecuteReader();
            if (obj != null)
            {
                DataTable dt = new DataTable();
                dt.Load(obj);

                info = dt.Rows[0]["ma"].ToString() + " -- " + dt.Rows[0]["ten"].ToString() + " -- " + dt.Rows[0]["trangthai"].ToString();

                dt.Clone();
                dt.Clear();
            }

            return info;
        }

        public string GetInfoItem_AddGroup(string id)
        {
            string info = "";

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            string sql = "SELECT pk_seq, ma, ten, nameEnglish, nhomsanpham_fk, trangthai " +
                         " FROM SanPham WHERE pk_seq = '" + id + "' ";
            SqlCommand command = new SqlCommand(sql, connect, transaction);
            command.CommandTimeout = int.MaxValue;
            command.CommandText = sql;
            SqlDataReader obj = command.ExecuteReader();
            if (obj != null)
            {
                DataTable dt = new DataTable();
                dt.Load(obj);

                info = dt.Rows[0]["nameEnglish"].ToString() + " -- " + dt.Rows[0]["ten"].ToString() + " -- " + dt.Rows[0]["ma"].ToString() + " -- " + dt.Rows[0]["nhomsanpham_fk"].ToString();

                dt.Clone();
                dt.Clear();
            }
            transaction.Commit();

            return info;
        }

        public string Update_Unit(string id, string ma, string ten, string trangthai, string userId)
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
                string sql = "SELECT COUNT(*) FROM DONVITINH WHERE ma = N'" + ma + "' ";
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
                    return "Mã đơn vị: " + ma + " đã tồn tại trong hệ thống.";
                }

                if (id.Trim().Length <= 0)
                {
                    sql = "INSERT DonViTinh( ma, ten, trangthai, nguoitao, nguoisua ) " +
                            "values( N'" + ma + "', N'" + ten + "', N'" + trangthai + "', '" + userId + "', '" + userId + "' ) ";
                }
                else
                {
                    sql = "UPDATE DonViTinh SET ma = N'" + ma + "', ten = N'" + ten + "', trangthai = '" + trangthai + "', nguoisua = '" + userId + "', ngaysua = getdate() " +
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

        public string INSERT_Product(string ma, string ten, string nameEnglish, string trangthai, string cycletime, string cycletarget, string nhomsanpham_fk, string showCustomer, string giaban, string dvt_fk, string nguoitao)
        {

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            string dvkd_fk = "100001";
            string nganhhang_fk = "100001";
            string nhanhang_fk = "0";

            string chungloai_fk = "0";            
            string dinhmucton = "0";
            string giamua = "0";
            string giaton = "0";
            string trongluong = "0";
            string thetich = "0";

            if (giaban.Trim().Length <= 0)
                giaban = "0";
            else
                giaban = FormatString.removeComma(giaban);

            string sanpham_fk = "";
            try
            {
                //CHECK TRUNG TEN SP
                string sql = "SELECT COUNT(*)  FROM SanPham WHERE ma = N'" + ma + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object objMA = command.ExecuteScalar();
                if (int.Parse(objMA.ToString()) > 0)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Error! Item code is duplicate.";
                }

               
                sql = "INSERT SanPham(ma, ten, nameEnglish, trangthai, dvt_fk, dvkd_fk, nganhhang_fk, nhanhang_fk, chungloai_fk, nguoitao, nguoisua, giaton, trongluong, thetich, kichthuoc, hansudung, dinhmucton, nhomsanpham_fk, giamua, giaban, khachhang_fk, cycletime, cycletarget, showCustomer) " +
                    " SELECT N'" + ma + "', N'" + ten + "', N'" + nameEnglish + "', '" + trangthai + "', '" + dvt_fk + "', " + dvkd_fk + ", " + nganhhang_fk + ", " + nhanhang_fk + ", " + chungloai_fk + ", '" + nguoitao + "', '" + nguoitao + "', '" + giaton + "', '" + trongluong + "',  " +
                    " '" + thetich + "', N'', '365', " + dinhmucton + " , " + nhomsanpham_fk + ", '" + giamua + "', '" + giaban + "', '0', '" + cycletime + "', '" + cycletarget + "', '" + showCustomer + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                int kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "1.Error! Cannot created new this item.";
                }

                sql = "SELECT IDENT_CURRENT('SanPham')";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                sanpham_fk = command.ExecuteScalar().ToString();

                sql = " UPDATE SanPham SET timkiem = dbo.ftBoDau( ma + ' ' + ten  ) WHERE pk_seq = '" + sanpham_fk + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "3.4.Error! Cannot created new this item.";
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                connect.Close();
                return "4.Error! Cannot created new this item.";
            }

            connect.Close();

            return sanpham_fk;
        }

        public string UPDATE_Product(string id, string ma, string ten, string nameEnglish, string trangthai, string cycletime, string cycletarget, string nhomsanpham_fk, string showCustomer, string giaban, string dvt_fk, string nguoitao)
        {

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            if (giaban.Trim().Length <= 0)
                giaban = "0";
            else
                giaban = FormatString.removeComma(giaban);

            try
            {
                //CHECK TRUNG TEN SP                
                string sql = "SELECT COUNT(*)  FROM SanPham WHERE ma = N'" + ma.Trim() + "' and pk_seq != '" + id + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object objMA = command.ExecuteScalar();
                if (int.Parse(objMA.ToString()) > 0)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Mã đã tồn tại trong hệ thống.";
                }

                sql = "UPDATE SanPham SET ma = N'" + ma + "', ten = N'" + ten + "', nameEnglish = N'" + nameEnglish + "', trangthai = '" + trangthai + "', dvt_fk = '" + dvt_fk + "', nhomsanpham_fk = '" + nhomsanpham_fk + "', " +
                    " giaban = '" + giaban + "', cycletime = '" + cycletime + "', cycletarget = '" + cycletarget + "', showCustomer = '" + showCustomer + "' , nguoisua = '" + nguoitao + "', ngaysua = getdate() " +
                    " WHERE pk_seq = '" + id + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                int kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "1.Error! Cannot updated this item.";
                }

                sql = " UPDATE SanPham SET timkiem = dbo.ftBoDau( ma + ' ' + nameEnglish + ' ' + ten  ) WHERE pk_seq = '" + id + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "3.4.Error! Cannot updated this item.";
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                connect.Close();
                return "4.Error! Cannot updated this item.";
            }

            connect.Close();

            return id;
        }

        public string CREATE_Department(string phongban, string sanpham, string nhansu, string nguoitao)
        {

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();
            
            try
            {
                //CHECK TRUNG TEN SP
                string sql = "DELETE PhongBan_NhanSu WHERE phongban_fk = '" + phongban + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                int kq = command.ExecuteNonQuery();

                sql = "DELETE PhongBan_SanPham WHERE phongban_fk = '" + phongban + "' ";                
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();

                if (nhansu.Length > 3)
                {
                    sql = "INSERT PhongBan_NhanSu(phongban_fk, nhansu_fk) " +
                    " SELECT '" + phongban + "', pk_seq  " +
                    " FROM DanhSachNhanSu WHERE pk_seq in (" + nhansu + ") ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                }

                if (sanpham.Length > 3)
                {
                    sql = "INSERT PhongBan_SanPham(phongban_fk, sanpham_fk) " +
                   " SELECT '" + phongban + "', pk_seq  " +
                   " FROM SanPham WHERE pk_seq in (" + sanpham + ") ";
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
                return "4.Error! Cannot created new this item.";
            }

            connect.Close();

            return "";
        }

        public string CREATE_GroupItem(string ma, string ten, string trangthai, string bangmamau, string nguoitao)
        {

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            try
            {
                int coma = 0;
                string sql = "SELECT COUNT(*) FROM NhomSanPham WHERE ma = N'" + ma + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;

                object obj = command.ExecuteScalar();
                if (obj != null && obj.ToString().Trim().Length > 0)
                    coma = int.Parse(obj.ToString());

                if (coma == 0)
                {
                    sql = "INSERT NhomSanPham(ma, ten, trangthai, bangmamau_fk, stt, nguoitao, nguoisua) " +
                        " SELECT N'" + ma + "', N'" + ten + "', N'" + trangthai + "', '" + bangmamau + "', '" + ma + "', '" + nguoitao + "', '" + nguoitao + "' ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    int kq = command.ExecuteNonQuery();
                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Error! Data cannot updated...: " + sql;
                    }
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                connect.Close();
                return "4.Error! Cannot created new this item.";
            }

            connect.Close();

            return "";
        }

        public string ADD_ItemIntoGroup(string sanpham_fk, string nhomsanpham_fk, string nguoitao)
        {

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            try
            {
                string sql = "UPDATE SanPham SET nhomsanpham_fk = '" + nhomsanpham_fk + "' WHERE pk_seq = '" + sanpham_fk + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                int kq = command.ExecuteNonQuery();
               
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                connect.Close();
                return "4.Error! Cannot created new this item.";
            }

            connect.Close();

            return "";
        }

        public string SAVE_ShowCustomerItem(string sanpham_fk, string showCustomer, string nguoitao)
        {

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            try
            {
                string sql = "UPDATE SanPham SET showCustomer = '" + showCustomer + "' WHERE pk_seq = '" + sanpham_fk + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                int kq = command.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                connect.Close();
                return "4.Error! Cannot created new this item.";
            }

            connect.Close();

            return "";
        }

        public string SAVE_CostStaff(string thang, string nam, string nhansu, string phongban, string chiphi, string nguoitao)
        {

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            try
            {
                bool flag = false;
                string sql = "SELECT COUNT(*) FROM DanhSachNhanSu_ChiPhi WHERE thang = '" + thang + "' AND nam = '" + nam + "' ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if (int.Parse(obj.ToString()) > 0)
                {
                    flag = true;
                }

                string[] nsArr = Regex.Split(nhansu, ";");
                string[] pbArr = Regex.Split(phongban, ";");
                string[] cpArr = Regex.Split(chiphi, ";");

                //string spINSERT = "";
                for (int i = 0; i < nsArr.Length; i++)
                {
                    string nhansu_fk = nsArr[i];
                    string[] cp = Regex.Split(cpArr[i], "_");

                    sql = "DELETE DanhSachNhanSu_ChiPhi WHERE thang = '" + thang + "' AND nam = '" + nam + "' AND nhansu_fk = '" + nhansu_fk + "' ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    int kq = command.ExecuteNonQuery();

                    sql = "INSERT DanhSachNhanSu_ChiPhi(thang, nam, nhansu_fk, phongban_fk, chiphi)";
                    for (int j = 0; j < pbArr.Length; j++)
                    {
                        string phongban_fk = pbArr[j];
                        string _chiphi = cp[j];
                        _chiphi = FormatString.removeComma(_chiphi);

                        if (j < pbArr.Length - 1)
                            sql += " SELECT N'" + thang + "', N'" + nam + "', '" + nhansu_fk + "', '" + phongban_fk + "', '" + _chiphi + "' UNION ALL ";
                        else
                            sql += " SELECT N'" + thang + "', N'" + nam + "', '" + nhansu_fk + "', '" + phongban_fk + "', '" + _chiphi + "' ";
                    }
                    if(sql.Length > 100)
                    {
                        command.CommandTimeout = int.MaxValue;
                        command.CommandText = sql;
                        kq = command.ExecuteNonQuery();
                    }
                    sql = "";

                }
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                connect.Close();
                return "4.Error! Cannot created new this item.";
            }

            connect.Close();

            return "";
        }

        public string createKeyData(SqlCommand command)
        {
            string keyData = "";
            while (keyData.Length < 4)
            {
                if (keyData.Length == 2)
                    keyData += FormatString.returnRandomNumber(0);
                if (keyData.Length == 3)
                    keyData += FormatString.returnRandomChart(5);

                keyData += FormatString.returnRandomString(2);
            }

            string sql = "SELECT COUNT(*) FROM SanPham WHERE keyData = N'" + keyData + "'";
            command.CommandTimeout = int.MaxValue;
            command.CommandText = sql;
            object obj = command.ExecuteScalar();
            if (obj != null)
            {
                if (int.Parse(obj.ToString()) > 0)
                    createKeyData(command);
            }

            return keyData;
        }
    }
}