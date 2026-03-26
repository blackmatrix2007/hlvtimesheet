using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HLVTimeSheet.Model;

namespace HLVTimeSheet.Admin.Hander
{
    /// <summary>
    /// Summary description for hdPhanquyen
    /// </summary>
    public class hdPhanquyen : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string type = context.Request.QueryString["type"];
            if (type == null)
                type = "";

            if (type.Equals("updateQuyen"))
            {
                updateQUYEN(context);
            }
        }

        private void updateQUYEN(HttpContext context)
        {
            string id = "";
            string maquyen = "";
            string diengiai = "";
            string trangthai = "";

            string groupXEMSelected = "";
            string groupXOASelected = "";
            string groupTAOMOISelected = "";
            string groupCAPNHATSelected = "";
            string groupCHOTSelected = "";
            string groupHUYCHOTSelected = "";

            string xemSelected = "";
            string xoaSelected = "";
            string taomoiSelected = "";
            string capnhatSelected = "";
            string chotSelected = "";
            string huychotSelected = "";

            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            if (context.Request.QueryString["ma"] != null)
                maquyen = context.Request.QueryString["ma"].ToString();

            if (context.Request.QueryString["diengiai"] != null)
                diengiai = context.Request.QueryString["diengiai"].ToString();

            if (context.Request.QueryString["trangthai"] != null)
                trangthai = context.Request.QueryString["trangthai"].ToString();

            if (context.Request.QueryString["groupXEMSelected"] != null)
                groupXEMSelected = context.Request.QueryString["groupXEMSelected"].ToString();

            if (context.Request.QueryString["groupXOASelected"] != null)
                groupXOASelected = context.Request.QueryString["groupXOASelected"].ToString();

            if (context.Request.QueryString["groupTAOMOISelected"] != null)
                groupTAOMOISelected = context.Request.QueryString["groupTAOMOISelected"].ToString();

            if (context.Request.QueryString["groupCAPNHATSelected"] != null)
                groupCAPNHATSelected = context.Request.QueryString["groupCAPNHATSelected"].ToString();

            if (context.Request.QueryString["groupCHOTSelected"] != null)
                groupCHOTSelected = context.Request.QueryString["groupCHOTSelected"].ToString();

            if (context.Request.QueryString["groupHUYCHOTSelected"] != null)
                groupHUYCHOTSelected = context.Request.QueryString["groupHUYCHOTSelected"].ToString();

            if (context.Request.QueryString["xemSelected"] != null)
                xemSelected = context.Request.QueryString["xemSelected"].ToString();

            if (context.Request.QueryString["xoaSelected"] != null)
                xoaSelected = context.Request.QueryString["xoaSelected"].ToString();

            if (context.Request.QueryString["taomoiSelected"] != null)
                taomoiSelected = context.Request.QueryString["taomoiSelected"].ToString();

            if (context.Request.QueryString["capnhatSelected"] != null)
                capnhatSelected = context.Request.QueryString["capnhatSelected"].ToString();

            if (context.Request.QueryString["chotSelected"] != null)
                chotSelected = context.Request.QueryString["chotSelected"].ToString();

            if (context.Request.QueryString["huychotSelected"] != null)
                huychotSelected = context.Request.QueryString["huychotSelected"].ToString();

            if (maquyen.Trim().Length <= 0)
            {
                context.Response.Write("Vui lòng kiểm tra lại mã quyền");
                return;
            }

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                PhanQuyen phanquyen = new PhanQuyen();

                string msg = phanquyen.NhomQuyen_Update_UngDung(id, maquyen, diengiai, trangthai, 
                    groupXEMSelected, groupXOASelected, groupTAOMOISelected, groupCAPNHATSelected, groupCHOTSelected, groupHUYCHOTSelected, 
                    xemSelected, xoaSelected, taomoiSelected, capnhatSelected, chotSelected, huychotSelected, userId);

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