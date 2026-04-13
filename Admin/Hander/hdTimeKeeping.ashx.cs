using HLVTimeSheet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HLVTimeSheet.Admin.Hander
{
    /// <summary>
    /// Summary description for hdTimeKeeping
    /// </summary>
    public class hdTimeKeeping : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;

            string action = "";
            if (context.Request.QueryString["action"] != null)
                action = context.Request.QueryString["action"].ToString();

            if (action.Equals("saveInforCheckIn"))
            {
                SaveInforCheckIn(context);
            }
            else if (action.Equals("viewStaff"))
            {
                GetInforStaff_CheckIn(context);
            }

        }

        private void SaveInforCheckIn(HttpContext context)
        {
            string ngaynhap = "";
            string phongban = "";
            string nhansu = "";
            string trangthai = "";
            string gioIn = "";
            string phutIn = "";
            string gioOut = "";
            string phutOut = "";

            string hinhanhIn = "";
            string hinhanhOut = "";
            string idMayCheckIn = "";
            string idMayCheckOut = "";

            if (context.Request.QueryString["ngaynhap"] != null)
                ngaynhap = context.Request.QueryString["ngaynhap"].ToString();

            if (context.Request.QueryString["trangthai"] != null)
                trangthai = context.Request.QueryString["trangthai"].ToString();

            if (context.Request.QueryString["phongban"] != null)
                phongban = context.Request.QueryString["phongban"].ToString();

            if (context.Request.QueryString["nhansu"] != null)
                nhansu = context.Request.QueryString["nhansu"].ToString();

            if (context.Request.QueryString["gioIn"] != null)
                gioIn = context.Request.QueryString["gioIn"].ToString();

            if (context.Request.QueryString["phutIn"] != null)
                phutIn = context.Request.QueryString["phutIn"].ToString();

            if (context.Request.QueryString["gioOut"] != null)
                gioOut = context.Request.QueryString["gioOut"].ToString();

            if (context.Request.QueryString["phutOut"] != null)
                phutOut = context.Request.QueryString["phutOut"].ToString();

            if (context.Request.QueryString["hinhanhIn"] != null)
                hinhanhIn = context.Request.QueryString["hinhanhIn"].ToString();

            if (context.Request.QueryString["hinhanhOut"] != null)
                hinhanhOut = context.Request.QueryString["hinhanhOut"].ToString();

            if (context.Request.QueryString["idMayCheckIn"] != null)
                idMayCheckIn = context.Request.QueryString["idMayCheckIn"].ToString();

            if (context.Request.QueryString["idMayCheckOut"] != null)
                idMayCheckOut = context.Request.QueryString["idMayCheckOut"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                TimeKeepingController timeObj = new TimeKeepingController();

                string msg = timeObj.INSERT_TimeKeeping_New(ngaynhap, phongban, nhansu, gioIn, phutIn, gioOut, phutOut, "0", hinhanhIn, hinhanhOut, idMayCheckIn, idMayCheckOut, trangthai, userId);

                context.Response.Write(msg);
            }
        }

        private void GetInforStaff_CheckIn(HttpContext context)
        {
            string nhansu = "";
            if (context.Request.QueryString["nhansu"] != null)
                nhansu = context.Request.QueryString["nhansu"].ToString();

            string ngaynhap = "";
            if (context.Request.QueryString["ngaynhap"] != null)
                ngaynhap = context.Request.QueryString["ngaynhap"].ToString();

            TimeKeepingController timeObj = new TimeKeepingController();

            string msg = timeObj.GET_InformationStaff_CheckIn(nhansu, ngaynhap);

            context.Response.Write(msg);
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