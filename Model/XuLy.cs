using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using HLVTimeSheet.AcsessData;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Data.SqlClient;

namespace HLVTimeSheet.Model
{
    public class XuLy
    {
        public static DataTable createTable(string tableName, string[] columnName)
        {
            DataTable dt = new DataTable(tableName);

            for (int i = 0; i < columnName.Length; i++)
            {
                dt.Columns.Add(columnName[i], Type.GetType("System.String"));
            }

            return dt;
        }

        public static string getQuyen_NhanVien(string nhanvien_fk)
        {
            string nhanvien = nhanvien_fk;

            ExecuteData xl = new ExecuteData();

            DataTable dt = xl.ReadTable("SELECT nhanvien_fk FROM NhanVien_Quyen_NhanVien WHERE nhanvienql_fk = '" + nhanvien_fk + "'");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nhanvien += ", " + dt.Rows[i]["nhanvien_fk"].ToString();
                }
            }

            return nhanvien;
        }

        public static string removeDauPhay(string str)
        {
            string kq = Regex.Replace(str, ",", "");
            return kq;
        }

        public static string removePercent(string str)
        {
            string kq = Regex.Replace(str, "%", "");
            return kq;
        }

        public static string ForMatNumber(string input)
        {
            try
            {
                string kq = double.Parse(input).ToString("#,#", CultureInfo.InvariantCulture);
                if (kq.Trim().Length <= 0)
                    kq = "0";

                return kq;
            }
            catch (Exception e)
            {
                return "0";
            }

        }

        public static string ForMatNumber_SoLe(string input)
        {
            if (input.Trim().Length <= 0)
                return "0";

            if (!input.Contains("."))
                return double.Parse(input).ToString("#,#", CultureInfo.InvariantCulture);

            try
            {
                string[] data = Regex.Split(input, @"\.");

                string phanNGUYEN = double.Parse(data[0]).ToString("#,#", CultureInfo.InvariantCulture);
                if (phanNGUYEN.Trim().Length <= 0)
                    phanNGUYEN = "0";

                string phanle = "0";
                if (data[1].Trim().Length >= 4)
                    phanle = data[1].Substring(0, 4);
                else
                    phanle = data[1];

                while (phanle.EndsWith("0"))
                    phanle = phanle.Substring(0, phanle.Length - 1);

                if (phanle.Trim().Length > 0 && double.Parse(phanle) > 0)
                    return phanNGUYEN + "." + phanle;
                else
                    return phanNGUYEN;
            }
            catch (Exception e)
            {
                return "0";
            }

        }

        public static string ForMatNumber_SoLeNEW(string input)
        {
            if (input.Trim().Length <= 0)
                return "0";

            if (!input.Contains("."))
                return double.Parse(input).ToString("#,#", CultureInfo.InvariantCulture);

            try
            {
                string[] data = Regex.Split(input, @"\.");

                string phanNGUYEN = double.Parse(data[0]).ToString("#,#", CultureInfo.InvariantCulture);
                if (phanNGUYEN.Trim().Length <= 0)
                    phanNGUYEN = "0";

                string phanle = "0";
                if (data[1].Trim().Length >= 4)
                    phanle = data[1].Substring(0, 4);
                else
                    phanle = data[1];

                while (phanle.EndsWith("0"))
                    phanle = phanle.Substring(0, phanle.Length - 1);

                if (phanle.Trim().Length > 0 && double.Parse(phanle) > 0)
                    return phanNGUYEN + "." + phanle;
                else
                    return phanNGUYEN;
            }
            catch (Exception e)
            {
                return "0";
            }
        }

        public static string GhiNhan_TaiKhoan(SqlConnection connect, SqlTransaction transaction, string sohieu, string sohieu_doiung, string sotienNO, string sotienCO, string nghiepvu, string id_nghiepvu, string ngaynghiepvu, string loaidoituong, string id_doituong, string khoanmuc)
        {
            try
            {
                if (id_doituong.Trim().Length <= 0)
                    id_doituong = "NULL";
                if (sotienNO.Trim().Length <= 0)
                    sotienNO = "0";
                if (sotienCO.Trim().Length <= 0)
                    sotienCO = "0";
                if (id_nghiepvu.Trim().Length <= 0)
                    id_nghiepvu = "NULL";

                string query = "insert NHATKYTAIKHOAN(sohieu, sohieu_doiung, no, co, nghiepvu, id_nghiepvu, ngaynghiepvu, loaidoituong, id_doituong, khoanmuc) " +
                               " values('" + sohieu + "', '" + sohieu_doiung + "', '" + sotienNO + "', '" + sotienCO + "', N'" + nghiepvu + "', " + id_nghiepvu + ", '" + ngaynghiepvu + "', N'" + loaidoituong + "', " + id_doituong + ", N'" + khoanmuc + "' )";

                int kq = new SqlCommand(query, connect, transaction).ExecuteNonQuery();
                if (kq < 1)
                {
                    return "Error! Cannot updated data... NHATKYTAIKHOAN: " + query;
                }

                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public static string Get_Tai_Khoan(string selected, string start)
        {
            string query = "SELECT sohieu, cast( sohieu as varchar(10) ) + ', ' + ten as ten, '" + selected + "' as selected FROM TAIKHOANKETOAN WHERE trangthai = '1' ";

            if (start.Trim().Length > 0)
            {
                string[] _strat = Regex.Split(start, ",");

                string condition = "";
                for (int i = 0; i < _strat.Length; i++)
                {
                    condition += " cast( sohieu as varchar(10) ) like '" + _strat[i] + "%' ";
                    if (i < _strat.Length - 1)
                        condition += " OR ";
                }
                if (condition.Trim().Length > 0)
                    query = query + " AND ( " + condition + " ) ";
            }

            query += " ORDER BY stt asc ";
            return query;
        }

        public static string LoaiKho(SqlConnection connect, SqlTransaction transaction, string kho_fk)
        {
            if (kho_fk == null || kho_fk.Trim().Length < 6)
                return "0";
            else
            {
                string sql = " SELECT loaikho FROM Kho WHERE pk_seq = '" + kho_fk + "'  ";
                return (new SqlCommand(sql, connect, transaction).ExecuteScalar().ToString());
            }            
        }        

        public static string SoPhieuXuat(string loaikho)
        {
            if (loaikho.Equals("1"))
                return "SO-";
            else if (loaikho.Equals("2"))
                return "SO-";
            return "SO-";
        }
        public static string SoPhieuNhap(string loaikho)
        {
            if (loaikho.Equals("1"))
                return "SI-";
            else if (loaikho.Equals("2"))
                return "SI-";
            else return "SI-";
        }

        public static string SoPhieuTransferPallet(string loaikho)
        {
            if (loaikho.Equals("1"))
                return "TFP-";
            else if (loaikho.Equals("2"))
                return "TFP-";
            else return "TFP-";
        }

        public static string SoPhieuReceivePallet(string loaikho)
        {
            if (loaikho.Equals("1"))
                return "RP-";
            else if (loaikho.Equals("2"))
                return "RP-";
            else return "RP-";
        }
    }
}