using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;
using HLVTimeSheet.AcsessData;
using System.Data;

namespace ISO.Page.Autocomplete
{
    /// <summary>
    /// Summary description for sanphamList
    /// </summary>
    public class sanphamList : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string query = context.Request.QueryString["letters"];

            string flag = context.Request.QueryString["flag"];
            if (flag == null)
                flag = "";
            
            if (flag.Contains("donhang"))
            {
                context.Response.Clear();
                context.Response.ContentType = "application/json; charset=utf-8";

                string term = context.Request.QueryString["term"];
                if (term == null)
                    term = "";

                string khoId = context.Request.QueryString["khoId"];
                if (khoId == null)
                    khoId = "";

                string khId = context.Request.QueryString["khId"];
                if (khId == null)
                    khId = "-1";

                string ngaynhap = context.Request.QueryString["ngaynhap"];
                if (ngaynhap == null || ngaynhap.Trim().Length <= 0)
                    ngaynhap = DateTime.Now.ToString("dd-MM-yyyy");

                string donhangId = context.Request.QueryString["donhangId"];
                if (donhangId == null || donhangId.Trim().Length <= 0)
                    donhangId = "-1";

                string nhaphanphoi = context.Request.QueryString["nhaphanphoi"];
                if (nhaphanphoi == null || nhaphanphoi.Trim().Length <= 0)
                    nhaphanphoi = "-1";

                if (khId.Trim().Length > 0 && ngaynhap.Trim().Length > 0)
                {
                    ExecuteData xl = new ExecuteData();

                    string sql = "";

                    object objNPP = xl.ExecuteScalarSQL(" SELECT npp_fk FROM NhanVien WHERE pk_seq = '" + context.Session["userId"].ToString() + "' ");
                    string npp_fk = "-1";
                    if (objNPP != null)
                    {
                        npp_fk = objNPP.ToString();
                    }

                    if (nhaphanphoi.Trim().Length <= 0)
                        nhaphanphoi = npp_fk;

                    string condition = "";
                    if (term.Length > 0)
                        condition += " and ( c.matracuu like N'%" + xl.Change_AV(term) + "%') ";

                    sql = "SELECT distinct c.masanpham + ' --- ' + a.ten + ' [' + CAST((ISNULL(c.mavach, -1)) AS nvarchar(20)) +'] [' + ISNULL(b.ten, 'NA') + '] [' + c.SOLO + '] [' + cast ( ( ISNULL(c.avai, 0) + ISNULL(donhang.soluong, 0) ) as varchar(20) ) + '] [' + cast( cast( c.dongia as numeric(18, 0) ) as varchar (10) ) + '] [' + cast( ISNULL( ( SELECT soluong2 FROM  QUYCACH WHERE sanpham_fk = a.pk_seq and dvt1_fk = a.dvt_fk ), 1 ) as varchar( 10 ) ) + ']' as label, a.pk_seq as ID " +
                        "FROM  SanPham a INNER JOIN DonViTinh b on a.dvt_fk = b.pk_seq INNER JOIN NganhHang nh on a.nganhhang_fk = nh.pk_seq  " +
                        "       INNER JOIN Kho_SanPham_ChiTiet c on a.pk_seq = c.sanpham_fk AND c.npp_fk = '" + nhaphanphoi + "' AND c.avai > 0 " + condition + 
                        //"       INNER JOIN BangGiaBan_SanPham bgbSP ON bgbSP.sanpham_fk =  c.sanpham_fk " +
                        "       LEFT JOIN ( " +
                        "         SELECT kho_fk, sanpham_fk, solo, soluong, mavach " +
                        "         FROM  DonHang_SanPham dh_sp INNER JOIN DonHang dh on dh_sp.donhang_fk = dh.pk_seq  " +
                        "         WHERE dh_sp.donhang_fk = '" + donhangId + "' " +
                        "      ) donhang on c.kho_fk = donhang.kho_fk and a.pk_seq = donhang.sanpham_fk and c.mavach = donhang.mavach " +
                        "WHERE a.trangthai = '1'  and c.giaban > 0 ";

                    if (term.Length > 0)
                        sql += " and ( c.matracuu like N'%" + xl.Change_AV(term) + "%') ";

                    DataTable dtSanpham = xl.ReadTable(sql);

                    string json = GetJson(dtSanpham);

                    dtSanpham.Clear();
                    dtSanpham.Clone();

                    context.Response.Write(json);
                }
            }            
            else if (flag.Contains("lockhachhang"))
            {
                ExecuteData xl = new ExecuteData();

                string sql = "SELECT top(150) ma + ', ' + hoten + ' - ' + diachi as label, pk_seq as ID FROM  KhachHang WHERE TrangThai = '1' ";

                if (query.Length > 0)
                    sql += " and timkiem like N'%" + xl.Change_AV(query) + "%' ";

                sql += "ORDER BY ma asc, hoten asc";

                DataTable dtSanpham = xl.ReadTable(sql);
                for (int i = 0; i < dtSanpham.Rows.Count; i++)
                {
                    if (convertToUnSign(dtSanpham.Rows[i]["label"].ToString().ToUpper()).Contains(convertToUnSign(query.ToUpper())))
                    {
                        context.Response.Write(dtSanpham.Rows[i]["label"].ToString() + "[" + dtSanpham.Rows[i]["ID"].ToString() + "]|");
                    }
                }
                dtSanpham.Clone();

            }
            else if (flag.Contains("khachhang"))
            {
                context.Response.Clear();
                context.Response.ContentType = "application/json; charset=utf-8";

                ExecuteData xl = new ExecuteData();

                string term = context.Request.QueryString["term"];
                if (term == null)
                    term = "";

                string sql = "SELECT ma + ', ' + hoten + ' - ' + diachi as label, pk_seq as ID FROM  KhachHang WHERE TrangThai = '1' ";

                if (term.Length > 0)
                    sql += " and timkiem like N'%" + xl.Change_AV(term) + "%' ";

                DataTable dtSanpham = xl.ReadTable(sql);

                string json = GetJson(dtSanpham);

                dtSanpham.Clear();
                dtSanpham.Clone();

                context.Response.Write(json);
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

        public string GetJson(DataTable dt)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;

            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName.Trim(), dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

    }
}