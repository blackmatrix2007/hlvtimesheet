using HLVTimeSheet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HLVTimeSheet.Admin.Hander
{
    /// <summary>
    /// Summary description for hdCalendar
    /// </summary>
    public class hdCalendar : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;

            string action = "";
            if (context.Request.QueryString["action"] != null)
                action = context.Request.QueryString["action"].ToString();

            if (action.Equals("updateSetDate"))
            {
                UpdateSetDate(context);
            }
            else if (action.Equals("deleteSetDate"))
            {
                DeleteSetDate(context);
            }
            else if (action.Equals("getInforDatetime"))
            {
                GetInforDatetime(context);
            }
        }

        private void GetInforDatetime(HttpContext context)
        {
            string thoigian = "";
            if (context.Request.QueryString["thoigian"] != null)
                thoigian = context.Request.QueryString["thoigian"].ToString();

            string phongban = "";
            if (context.Request.QueryString["phongban"] != null)
                phongban = context.Request.QueryString["phongban"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                CalendarControlModel calendarObj = new CalendarControlModel();

                string msg = calendarObj.GetInforDatetime(thoigian, phongban);

                context.Response.Write(msg);
            }
        }

        private void UpdateSetDate(HttpContext context)
        {
            string thoigian = "";
            string phongban = "";
            string loaingaynghi = "";
            string trangthai = "";

            if (context.Request.QueryString["thoigian"] != null)
                thoigian = context.Request.QueryString["thoigian"].ToString();

            if (context.Request.QueryString["phongban"] != null)
                phongban = context.Request.QueryString["phongban"].ToString();

            if (context.Request.QueryString["loaingaynghi"] != null)
                loaingaynghi = context.Request.QueryString["loaingaynghi"].ToString();

            if (context.Request.QueryString["trangthai"] != null)
                trangthai = context.Request.QueryString["trangthai"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                CalendarControlModel calendarObj = new CalendarControlModel();

                string msg = calendarObj.UpdateSetDate(thoigian, phongban, loaingaynghi, trangthai, userId);

                context.Response.Write(msg);
            }
        }

        private void DeleteSetDate(HttpContext context)
        {
            string thoigian = "";
            string phongban = "";
           
            if (context.Request.QueryString["thoigian"] != null)
                thoigian = context.Request.QueryString["thoigian"].ToString();

            if (context.Request.QueryString["phongban"] != null)
                phongban = context.Request.QueryString["phongban"].ToString();
           
            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                CalendarControlModel calendarObj = new CalendarControlModel();

                string msg = calendarObj.DeleteSetDate(thoigian, phongban);

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