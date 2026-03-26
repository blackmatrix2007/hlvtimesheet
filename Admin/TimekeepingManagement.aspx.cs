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
    public partial class TimekeepingManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucLeftMenu ucleft = (ucLeftMenu)this.Master.FindControl("ucLeftMenu1");
            ucleft.action = "5";

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
                    linkLevel2 = "Create";
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

            switch (func)
            {
                case "10":
                    link = "Input Jisseki";

                    if (action.Trim().Length <= 0)
                    {
                        control = Page.LoadControl("uc_timekeeping_dailyattendance.ascx");
                    }
                    else
                    {
                        string id = "";
                        if (Request.QueryString["id"] != null)
                            id = Request.QueryString["id"].ToString();

                        if (action.Equals("delete"))
                        {

                        }
                        else
                        {
                            control = LoadControl("uc_timekeeping_dailyattendance_action.ascx", id);
                        }
                    }
                    break;

                case "12":
                    link = "Report Jisseki";
                    control = Page.LoadControl("uc_timekeeping_reportjisseki.ascx");
                    break;

                case "999":
                    link = "Change password";
                    control = Page.LoadControl("uc_administrator_changepassword.ascx");
                    break;

                default:
                    link = " ";

                    control = Page.LoadControl("uc_blank.ascx");

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