using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HLVTimeSheet.Model;

namespace HLVTimeSheet.Admin.Hander
{
    /// <summary>
    /// Summary description for hdNhanvien
    /// </summary>
    public class hdNhanvien : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;

            string action = "";
            if (context.Request.QueryString["action"] != null)
                action = context.Request.QueryString["action"].ToString();

            if (action.Equals("taomoi"))
            {
                TaoMoiNhanVien(context);
            }
            else if (action.Equals("init"))
            {
                InitThongTin(context);
            }
            
        }

        private void InitThongTin(HttpContext context)
        {
            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                NhanVien nhanvien = new NhanVien();

                string msg = nhanvien.LayThongTin(id);

                context.Response.Write(msg);
            }
        }

        private void TaoMoiNhanVien(HttpContext context)
        {
            string id = "";
            string Tendangnhap = "";
            string Hovaten = "";
            string Dienthoai = "";
            string Congty = "";
            string Matkhau = "";

            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            if (context.Request.QueryString["Tendangnhap"] != null)
                Tendangnhap = context.Request.QueryString["Tendangnhap"].ToString();

            if (context.Request.QueryString["Hovaten"] != null)
                Hovaten = context.Request.QueryString["Hovaten"].ToString();

            if (context.Request.QueryString["Dienthoai"] != null)
                Dienthoai = context.Request.QueryString["Dienthoai"].ToString();

            if (context.Request.QueryString["Congty"] != null)
                Congty = context.Request.QueryString["Congty"].ToString();

            if (context.Request.QueryString["Matkhau"] != null)
                Matkhau = context.Request.QueryString["Matkhau"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                NhanVien nhanvien = new NhanVien();

                string msg = "";

                if (id.Trim().Length > 0)
                    msg = nhanvien.CapNhat(id, Tendangnhap, Hovaten, Dienthoai, Congty, Matkhau, userId);
                else
                    msg = nhanvien.ThemMoi(Tendangnhap, Hovaten, Dienthoai, Congty, Matkhau, userId);

                context.Response.Write(msg);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}