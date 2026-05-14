using HLVTimeSheet.Admin.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HLVTimeSheet.Admin
{
    public partial class Report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucLeftMenu ucleft = (ucLeftMenu)this.Master.FindControl("ucLeftMenu1");
            ucleft.action = "10";

            if (Session["userId"] == null || Session["userId"].ToString().Trim().Length <= 0)
            {
                Response.Redirect("~/Admin/Login.aspx");
                return;
            }

            //Load dong UserControl
            System.Web.UI.Control control = new Control();

            string action = Request.QueryString["action"];
            if (action == null)
                action = "";

            string linkLevel2 = "";
            switch (action)
            {
                case "taomoi":
                    linkLevel2 = "Created";
                    break;
                case "capnhat":
                    linkLevel2 = "Update";
                    break;
                case "chot":
                    linkLevel2 = "Approve";
                    break;
            }

            string func = Request.QueryString["func"];
            if (func == null)
                func = "";
            string link = "";

            string parentLinkL2 = "List report ";

            switch (func)
            {
                case "101":
                    link = "Report";
                    control = Page.LoadControl("uc_report_dailyattendance.ascx");
                    break;
              
                default:
                    link = "Report";
                    control = Page.LoadControl("uc_indexReport.ascx");
                    break;
            }

            ucleft.func = func;
           
            panelControl.Controls.Add(control);


        }

        private UserControl LoadControl(string UserControlPath, params object[] constructorParameters)
        {
            List<Type> constParamTypes = new List<Type>();
            foreach (object constParam in constructorParameters)
            {
                constParamTypes.Add(constParam.GetType());
            }

            UserControl ctl = Page.LoadControl(UserControlPath) as UserControl;

            // Find the relevant constructor
            ConstructorInfo constructor = ctl.GetType().BaseType.GetConstructor(constParamTypes.ToArray());

            //And then call the relevant constructor
            if (constructor == null)
            {
                throw new MemberAccessException("The requested constructor was not found on : " + ctl.GetType().BaseType.ToString());
            }
            else
            {
                constructor.Invoke(ctl, constructorParameters);
            }

            // Finally return the fully initialized UC
            return ctl;
        }
    }
}