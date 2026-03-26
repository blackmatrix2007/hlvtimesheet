using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HLVTimeSheet.AcsessData;
using System.Text.RegularExpressions;
using System.Data;
using System.Text;

namespace HLVTimeSheet.Admin.Autocomplete
{
    /// <summary>
    /// Summary description for dkkmList
    /// </summary>
    public class dkkmList : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string query = context.Request.QueryString["letters"];
            string flag = context.Request.QueryString["flag"];

            string khoId = "";
            if (context.Request.QueryString["khoId"] != null)
                khoId = context.Request.QueryString["khoId"];

            string khachhang = "0";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"];

            string kho = "0";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"];

            string pallet = "";
            if (context.Request.QueryString["pallet"] != null)
                pallet = context.Request.QueryString["pallet"];

            string location = "";
            if (context.Request.QueryString["location"] != null)
                location = context.Request.QueryString["location"];

            string lotNo = "";
            if (context.Request.QueryString["lotNo"] != null)
                lotNo = context.Request.QueryString["lotNo"];

            string cartonNo = "";
            if (context.Request.QueryString["cartonNo"] != null)
                cartonNo = context.Request.QueryString["cartonNo"];

            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (flag.Equals("loadPallet"))
            {
                ExecuteData xl = new ExecuteData();

                string sql = "SELECT ten as label, pk_seq as ID FROM Pallet WHERE trangThai in (1, 2, 3) ";
                if (query.Length > 0)
                    sql += " AND ten like N'%" + pallet + "%' ";

                sql += " ORDER BY ten ASC ";

                DataTable dt = xl.ReadTable(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (convertToUnSign(dt.Rows[i]["label"].ToString().ToUpper()).Contains(convertToUnSign(query.ToUpper())))
                    {
                        context.Response.Write(dt.Rows[i]["label"].ToString() + "[" + dt.Rows[i]["ID"].ToString() + "]|");
                    }
                }
                dt.Clone();
            }
            else if (flag.Equals("loadLocation"))
            {
                ExecuteData xl = new ExecuteData();

                string sql = "SELECT ten as label, pk_seq as ID FROM Location WHERE trangThai in (1) AND dasudung < chieucao ";
                if (query.Length > 0)
                    sql += " AND ten like N'%" + location + "%' ";

                sql += " ORDER BY ten ASC ";

                DataTable dt = xl.ReadTable(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (convertToUnSign(dt.Rows[i]["label"].ToString().ToUpper()).Contains(convertToUnSign(location.ToUpper())))
                    {
                        context.Response.Write(dt.Rows[i]["label"].ToString() + "[" + dt.Rows[i]["ID"].ToString() + "]|");
                    }
                }
                dt.Clone();
            }
            else if (flag.Equals("loadLotNo"))
            {
                ExecuteData xl = new ExecuteData();

                string sql = "SELECT DISTINCT b.plNo, b.lotNo as label, b.plNo as ID FROM NhapHang a INNER JOIN NhapHang_SanPham_ChiTiet b ON a.pk_seq = b.nhaphang_fk " + 
                    " WHERE a.trangThai in (0) AND b.pallet_fk = 0 ";
                if (query.Length > 0)
                    sql += " AND b.lotNo like N'%" + lotNo + "%' ";

                sql += " ORDER BY b.lotNo ASC ";

                DataTable dt = xl.ReadTable(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (convertToUnSign(dt.Rows[i]["label"].ToString().ToUpper()).Contains(convertToUnSign(location.ToUpper())))
                    {
                        context.Response.Write(dt.Rows[i]["label"].ToString() + "[" + dt.Rows[i]["plNo"].ToString() + "]|");
                    }
                }
                dt.Clone();
            }
            else if (flag.Equals("loadCartonNo"))
            {
                ExecuteData xl = new ExecuteData();

                string sql = "SELECT DISTINCT b.cartonNo as label, b.cartonNo as ID FROM NhapHang a INNER JOIN NhapHang_SanPham_ChiTiet b ON a.pk_seq = b.nhaphang_fk " +
                    " WHERE a.trangThai in (0) AND b.pallet_fk = 0 ";
                if (lotNo.Length > 0)
                    sql += " AND b.lotNo = N'" + lotNo + "' ";
                if (cartonNo.Length > 0)
                    sql += " AND b.cartonNo like N'%" + cartonNo + "%' ";

                sql += " ORDER BY b.cartonNo ASC ";

                DataTable dt = xl.ReadTable(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (convertToUnSign(dt.Rows[i]["label"].ToString().ToUpper()).Contains(convertToUnSign(location.ToUpper())))
                    {
                        context.Response.Write(dt.Rows[i]["label"].ToString() + "[" + dt.Rows[i]["ID"].ToString() + "]|");
                    }
                }
                dt.Clone();
            }
            else if (flag.Equals("lockhachhangNEW"))
            {
                ExecuteData xl = new ExecuteData();

                string sql = "SELECT TOP(200) ma + ' --- ' + hoten + ' --- ' + diachi + ' --- ' + ISNULL(diachigiaohang, '') AS label, pk_seq as ID  " +
                             " FROM KhachHang a WHERE TrangThai = '1' ";
                if (query.Length > 0)
                    sql += " and timkiem like N'%" + xl.Change_AV(query) + "%' ";

                sql += "ORDER BY ma asc";

                DataTable dt = xl.ReadTable(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    context.Response.Write(dt.Rows[i]["label"].ToString() + " --- " + dt.Rows[i]["ID"].ToString() + "|");
                }
                dt.Clone();
            }                        
            else
            {
                ExecuteData xl = new ExecuteData();                
                string sql = "SELECT top(10) a.ma, a.ten, b.ten as donvi, a.giaban, 1 AS tonkho " +
                             "   FROM SanPham a INNER JOIN DonViTinh b on a.dvt_fk = b.pk_seq " +
                             "   WHERE a.trangthai = '1' AND a.timkiem like N'%" + xl.Change_AV(query) + "%' ORDER BY a.ma asc ";
                DataTable dt = xl.ReadTable(sql);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (convertToUnSign(dt.Rows[i]["ma"].ToString().ToUpper()).Contains(convertToUnSign(query.ToUpper()))
                            || convertToUnSign(dt.Rows[i]["ten"].ToString().ToUpper()).Contains(convertToUnSign(query.ToUpper())))
                    {
                        context.Response.Write(dt.Rows[i]["ma"].ToString() + " -- " + dt.Rows[i]["ten"].ToString() + " -- " + dt.Rows[i]["donvi"].ToString() + " -- " + dt.Rows[i]["tonkho"].ToString() + " -- " + dt.Rows[i]["giaban"].ToString() + "|");
                    }
                }
                dt.Clone();
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public string convertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }  
    }
}