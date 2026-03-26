using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using HLVTimeSheet.AcsessData;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Web.SessionState;

namespace HLVTimeSheet.Model
{
    public class ThanhVien : System.Web.HttpApplication
    {
        public string CheckDangNhap(string tendn, string pass)
        {

            if (tendn.ToUpper().Contains("OR ") || pass.ToUpper().Contains("OR ") || tendn.ToUpper().Contains("'") || pass.ToUpper().Contains("'"))
            {
                return "-2";  //SQL INNER JOIN
            }

            HttpContext context = HttpContext.Current;
            ExecuteData xl = new ExecuteData();

            string query = "SELECT pk_seq FROM nhanvien WHERE trangthai = 1 and upper(dangnhap) = '" + tendn.ToUpper() + "'";
            object kq = xl.ExecuteScalarSQL(query);
            if (kq == null)
            {
                context.Session["userName"] = "";
                context.Session["userId"] = "";
                context.Session["isThanhvien"] = "";
                context.Session["dnThanhvien"] = "";

                return "-1"; //sai user
            }
            else
            {
                query = "SELECT pk_seq, ten, ngonngu, token FROM NhanVien WHERE upper(dangnhap) = '" + tendn.ToUpper() + "' and PWDCOMPARE('" + pass + "', matkhau) = 1 and trangthai = '1'";
                DataTable dt = xl.ReadTable(query);

                if (dt.Rows.Count <= 0)
                {
                    context.Session["userName"] = "";
                    context.Session["userId"] = "";
                    context.Session["isThanhvien"] = "0";
                    context.Session["dnThanhvien"] = "";

                    return "0"; //sai mat khau
                }
                else
                {
                    context.Session["userName"] = dt.Rows[0]["ten"].ToString();
                    context.Session["userId"] = dt.Rows[0]["pk_seq"].ToString();
                    context.Session["isThanhvien"] = "0";
                    context.Session["token"] = dt.Rows[0]["token"].ToString();
                    context.Session["language"] = dt.Rows[0]["ngonngu"].ToString();

                    //// Lấy IP của tk đăng nhập.
                    string ipLocal = this.ipLocal();
                    string ipNetwork = this.ipNetwork();
                    
                    string msg = "";
                    msg = NhatKyTruyCap(dt.Rows[0]["pk_seq"].ToString(), ipLocal, ipNetwork);
                    if (msg.Length > 10)
                    {
                        return msg;
                    }

                    return "1";
                }
            }

        }

        public string CheckLogin_Token(string token)
        {

            if (token.ToUpper().Contains("OR ") || token.ToUpper().Contains("'"))
            {
                return "-2";  //SQL INNER JOIN
            }

            HttpContext context = HttpContext.Current;
            ExecuteData xl = new ExecuteData();

            string query = "SELECT pk_seq, ten, ngonngu, token FROM NhanVien WHERE token = N'" + token + "' AND trangthai = '1'";
            DataTable dt = xl.ReadTable(query);

            if (dt.Rows.Count <= 0)
            {
                context.Session["userName"] = "";
                context.Session["userId"] = "";
                context.Session["language"] = "0";
                context.Session["token"] = "";
                context.Session["isThanhvien"] = "0";

                return "0"; //sai mat khau
            }
            else
            {
                context.Session["userName"] = dt.Rows[0]["ten"].ToString();
                context.Session["userId"] = dt.Rows[0]["pk_seq"].ToString();
                context.Session["isThanhvien"] = "0";
                context.Session["token"] = dt.Rows[0]["token"].ToString();
                context.Session["language"] = dt.Rows[0]["ngonngu"].ToString();

                //// Lấy IP của tk đăng nhập.
                string ipLocal = this.ipLocal();
                string ipNetwork = this.ipNetwork();

                string msg = "";
                msg = NhatKyTruyCap(dt.Rows[0]["pk_seq"].ToString(), ipLocal, ipNetwork);
                if (msg.Length > 10)
                {
                    return msg;
                }

                return "1";
            }

        }
        private string Check_NhatKyTruyCap(string userId, string ipLocal, string ipNetwork)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                // Xóa hết nhật ký truy cập của ngày trước đó.
                string sql = " DELETE NhatKyTruyCap WHERE CONVERT(datetime, thoigiantruycap, 105) < CONVERT(datetime, '" + DateTime.Now.ToString("dd-MM-yyyy") + "', 105)";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();

                sql = "SELECT COUNT(*) FROM NhatKyTruyCap WHERE nhanvien_fk = '" + userId + "' AND ipLocal != N'" + ipLocal + "' AND ipNetwork != N'" + ipNetwork + "' ";
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                object obj = command.ExecuteScalar();
                if(obj != null)
                {
                    if (int.Parse(obj.ToString()) < 1)
                    {
                        sql = "UPDATE NhatKyTruyCap SET nhanvien_fk = '" + userId + "' WHERE ipLocal = N'" + ipLocal + "' AND ipNetwork = N'" + ipNetwork + "' ";
                        command.CommandTimeout = int.MaxValue;
                        command.CommandText = sql;
                        kq = command.ExecuteNonQuery();
                    }
                    else
                    {
                        // Thông báo chặn
                        string thoigiantruycap = "";
                        sql = "SELECT sessionID, nhanvien_fk, ipLocal, ipNetwork, thoigiantruycap FROM NhatKyTruyCap WHERE nhanvien_fk = '" + userId + "' ";
                        command.CommandTimeout = int.MaxValue;
                        command.CommandText = sql;
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader != null)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                ipLocal = dt.Rows[0]["ipLocal"].ToString();
                                ipNetwork = dt.Rows[0]["ipNetwork"].ToString();
                                thoigiantruycap = dt.Rows[0]["thoigiantruycap"].ToString();

                            }
                        }
                        return " Tài khoản đã được truy cập vào hệ thống từ một thiết bị khác, có địa chỉ ip local: " + ipLocal + " thời gian truy cập: " + thoigiantruycap;
                    }
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Cannot save access information in system: " + ex.Message;
            }

            connect.Close();

            return "";
        }

        private string NhatKyTruyCap(string userId, string ipLocal, string ipNetwork)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            int kq = -1;
            try
            {
                // 
                string sql = "INSERT NhatKyTruyCap(nhanvien_fk, ipLocal, ipNetwork, thoigiantruycap)" +
                    " SELECT '" + userId + "', N'" + ipLocal + "', '" + ipNetwork + "', GETDATE() ";
                SqlCommand command = new SqlCommand(sql, connect, transaction);
                command.CommandTimeout = int.MaxValue;
                command.CommandText = sql;
                kq = command.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Cannot save access information in system: " + ex.Message;
            }

            connect.Close();

            return "";
        }

        private string ipLocal()
        {
            //var host = Dns.GetHostEntry(Dns.GetHostName());
            //foreach (var ip in host.AddressList)
            //{
            //    if (ip.AddressFamily == AddressFamily.InterNetwork)
            //    {
            //        return ip.ToString();
            //    }
            //}
            return "0";
            //System.Web.HttpContext context = System.Web.HttpContext.Current;
            //string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            //if (!string.IsNullOrEmpty(ipAddress))
            //{
            //    string[] addresses = ipAddress.Split(',');
            //    if (addresses.Length != 0)
            //    {
            //        return addresses[0];
            //    }
            //}

            //return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public string ipNetwork()
        {
            string ipNetwork = "0";

            //// check IP using DynDNS's service
            //WebRequest request = WebRequest.Create("http://checkip.dyndns.org");
            //WebResponse response = request.GetResponse();
            //StreamReader stream = new StreamReader(response.GetResponseStream());

            //// IMPORTANT: SET Proxy to null, to drastically INCREASE the speed of request
            ////request.Proxy = null;
            //// read complete response
            //ipNetwork = stream.ReadToEnd().Replace("<html><head><title>Current IP Check</title></head><body>Current IP Address: ", "");
            //ipNetwork = ipNetwork.Replace("</body></html>\r\n", "");

            return ipNetwork;
        }
    }
}