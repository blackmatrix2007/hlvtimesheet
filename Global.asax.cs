using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace HLVTimeSheet
{
   
    public class Global : System.Web.HttpApplication
    {
        
        void Application_Start(object sender, EventArgs e)
        {
          
            // Code that runs on application startup
            RegisterRoutes(RouteTable.Routes);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.MapPageRoute("A/thu-cam-on/", "A/thu-cam-on/", "~/Views/A/LetterRCF.aspx");

            //routes.MapPageRoute("A/canh-bao/", "A/canh-bao/", "~/Views/A/MessageRCF.aspx");

            //routes.MapPageRoute("A", "A/{*N}", "~/Views/A/InfoBarcodeRCF.aspx");

            //routes.MapPageRoute("Edit/{*qr}", "Edit/{*qr}", "~/Views/Edit/UpdateBarcode.aspx");

            //routes.MapPageRoute("Admin/dang-ky/{*keysale}", "Admin/dang-ky/{*keysale}", "~/Admin/Registier.aspx");

            //routes.MapPageRoute("", "{*N}", "~/Views/InfoBarcode.aspx");
           
        }
      
        protected void Session_Start(object sender, EventArgs e)
        {
            Session["userName"] = "";
            Session["userId"] = "";
            Session["server"] = "";
            Session["keydata"] = "";
            Session["token"] = "";
            Session["typesOfAccount"] = "";
            Session["language"] = "1";
            Session["fromDay"] = "";
            Session["toDay"] = "";
            Session["timeline"] = "";
            Session["flagPrinter"] = null;

            if (Application["Online"] == null)
                Application["Online"] = "0";

            Application["Online"] = (int.Parse(Application["Online"].ToString()) + 1).ToString();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }
    }
}