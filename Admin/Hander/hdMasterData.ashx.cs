using HLVTimeSheet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HLVTimeSheet.Admin.Hander
{
    /// <summary>
    /// Summary description for hdMasterData
    /// </summary>
    public class hdMasterData : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;

            string action = "";
            if (context.Request.QueryString["action"] != null)
                action = context.Request.QueryString["action"].ToString();

            if (action.Equals("updateUnit"))
            {
                UPDATE_Unit(context);
            }
            else if (action.Equals("getInfo_Unit"))
            {
                GetInfo_Unit(context);
            }
            else if (action.Equals("saveProduction"))
            {
                SaveProduction(context);
            }
            else if (action.Equals("saveGroupItem"))
            {
                SaveGroupItem(context);
            }
            else if (action.Equals("getInfoItem_AddGroup"))
            {
                GetInfoItem_AddGroup(context);
            }
            else if (action.Equals("saveAddGroupItem"))
            {
                SaveAddGroupItem(context);
            }
            else if (action.Equals("saveShowCustomerItem"))
            {
                SaveShowCustomerItem(context);
            }
            else if (action.Equals("saveCostStaff"))
            {
                SaveCostStaff(context);
            }
            
        }

        private void GetInfo_Unit(HttpContext context)
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
                MasterDataModel masterObj = new MasterDataModel();

                string msg = masterObj.GetInfor_Unit(id);

                context.Response.Write(msg);
            }
        }

        private void UPDATE_Unit(HttpContext context)
        {
            string id = "";
            string ma = "";
            string ten = "";
            string trangthai = "";

            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            if (context.Request.QueryString["ma"] != null)
                ma = context.Request.QueryString["ma"].ToString();

            if (context.Request.QueryString["ten"] != null)
                ten = context.Request.QueryString["ten"].ToString();

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
                MasterDataModel masterObj = new MasterDataModel();

                string msg = masterObj.Update_Unit(id, ma, ten, trangthai, userId);

                context.Response.Write(msg);
            }
        }

        private void SaveProduction(HttpContext context)
        {
            string sanpham_fk = "";
            string ma = "";
            string nameEnglish = "";
            string ten = "";
            string donvi = "";
            string trangthai = "";
            string cycletime = "";
            string cycletarget = "";
            string nhomsanpham = "";
            string showCustomer = "";
            string dongia = "";

            if (context.Request.QueryString["sanpham_fk"] != null)
                sanpham_fk = context.Request.QueryString["sanpham_fk"].ToString();

            if (context.Request.QueryString["ma"] != null)
                ma = context.Request.QueryString["ma"].ToString();

            if (context.Request.QueryString["ten"] != null)
                ten = context.Request.QueryString["ten"].ToString();

            if (context.Request.QueryString["donvi"] != null)
                donvi = context.Request.QueryString["donvi"].ToString();

            if (context.Request.QueryString["trangthai"] != null)
                trangthai = context.Request.QueryString["trangthai"].ToString();

            if (context.Request.QueryString["cycletime"] != null)
                cycletime = context.Request.QueryString["cycletime"].ToString();

            if (context.Request.QueryString["dongia"] != null)
                dongia = context.Request.QueryString["dongia"].ToString();


            if (context.Request.QueryString["cycletarget"] != null)
                cycletarget = context.Request.QueryString["cycletarget"].ToString();

            if (context.Request.QueryString["nameEnglish"] != null)
                nameEnglish = context.Request.QueryString["nameEnglish"].ToString();


            if (context.Request.QueryString["nhomsanpham"] != null)
                nhomsanpham = context.Request.QueryString["nhomsanpham"].ToString();

            if (context.Request.QueryString["showCustomer"] != null)
                showCustomer = context.Request.QueryString["showCustomer"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                MasterDataModel masterObj = new MasterDataModel();

                if (cycletarget.Length <= 0)
                    cycletarget = "0";
                if (cycletime.Length <= 0)
                    cycletime = "0";
                if (dongia.Length <= 0)
                    dongia = "0";

                string msg = "";
                if (sanpham_fk.Length > 3)
                    msg = masterObj.UPDATE_Product(sanpham_fk, ma, ten, nameEnglish, trangthai, cycletime, cycletarget, nhomsanpham, showCustomer, dongia, donvi, userId);
                else
                    msg = masterObj.INSERT_Product(ma, ten, nameEnglish, trangthai, cycletime, cycletarget, nhomsanpham, showCustomer, dongia, donvi, userId);

                context.Response.Write(msg);
            }
        }

        private void SaveGroupItem(HttpContext context)
        {
            string ma = "";
            string ten = "";           
            string trangthai = "";
            string bangmamau = "";
                      
            if (context.Request.QueryString["ma"] != null)
                ma = context.Request.QueryString["ma"].ToString();

            if (context.Request.QueryString["ten"] != null)
                ten = context.Request.QueryString["ten"].ToString();

            if (context.Request.QueryString["bangmamau"] != null)
                bangmamau = context.Request.QueryString["bangmamau"].ToString();

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
                MasterDataModel masterObj = new MasterDataModel();

                string msg = "";

                msg = masterObj.CREATE_GroupItem(ma, ten, trangthai, bangmamau, userId);

                context.Response.Write(msg);
            }
        }

        private void GetInfoItem_AddGroup(HttpContext context)
        {
            string sanpham_fk = "0";
            if (context.Request.QueryString["sanpham_fk"] != null)
                sanpham_fk = context.Request.QueryString["sanpham_fk"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                MasterDataModel masterObj = new MasterDataModel();

                string msg = masterObj.GetInfoItem_AddGroup(sanpham_fk);

                context.Response.Write(msg);
            }
        }

        private void SaveAddGroupItem(HttpContext context)
        {
            string sanpham_fk = "0";
            if (context.Request.QueryString["sanpham_fk"] != null)
                sanpham_fk = context.Request.QueryString["sanpham_fk"].ToString();

            string nhomsanpham_fk = "0";
            if (context.Request.QueryString["nhomsanpham_fk"] != null)
                nhomsanpham_fk = context.Request.QueryString["nhomsanpham_fk"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                MasterDataModel masterObj = new MasterDataModel();

                string msg = masterObj.ADD_ItemIntoGroup(sanpham_fk, nhomsanpham_fk, userId);

                context.Response.Write(msg);
            }
        }

        private void SaveShowCustomerItem(HttpContext context)
        {
            string sanpham_fk = "0";
            if (context.Request.QueryString["sanpham_fk"] != null)
                sanpham_fk = context.Request.QueryString["sanpham_fk"].ToString();

            string showCustomer = "0";
            if (context.Request.QueryString["showCustomer"] != null)
                showCustomer = context.Request.QueryString["showCustomer"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                MasterDataModel masterObj = new MasterDataModel();

                string msg = masterObj.SAVE_ShowCustomerItem(sanpham_fk, showCustomer, userId);

                context.Response.Write(msg);
            }
        }

        private void SaveCostStaff(HttpContext context)
        {
            string thang = "0";
            if (context.Request.QueryString["thang"] != null)
                thang = context.Request.QueryString["thang"].ToString();

            string nam = "0";
            if (context.Request.QueryString["nam"] != null)
                nam = context.Request.QueryString["nam"].ToString();

            string nhansu = "0";
            if (context.Request.QueryString["nhansu"] != null)
                nhansu = context.Request.QueryString["nhansu"].ToString();

            string phongban = "0";
            if (context.Request.QueryString["phongban"] != null)
                phongban = context.Request.QueryString["phongban"].ToString();

            string chiphi = "0";
            if (context.Request.QueryString["chiphi"] != null)
                chiphi = context.Request.QueryString["chiphi"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                MasterDataModel masterObj = new MasterDataModel();

                string msg = masterObj.SAVE_CostStaff(thang, nam, nhansu, phongban, chiphi, userId);

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