using HLVTimeSheet.AcsessData;
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
    public partial class MasterData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucLeftMenu ucleft = (ucLeftMenu)this.Master.FindControl("ucLeftMenu1");
            ucleft.action = "1";

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
                case "5":
                    link = "Work type";

                    if (action.Trim().Length <= 0)
                    {
                        control = Page.LoadControl("uc_masterdata_worktype.ascx");
                    }
                    else
                    {
                        string id = "";
                        if (Request.QueryString["id"] != null)
                            id = Request.QueryString["id"].ToString();

                        if (action.Equals("delete"))
                        {
                            //ExecuteData xl = new ExecuteData();

                            //string sql = "UPDATE NhaPhanPhoi SET trangthai = 0 WHERE pk_seq = '" + id + "' ";
                            //xl.ExecuteNonQuerySQL(sql);
                            //Response.Redirect("MasterData.aspx?func=5");
                        }
                        else
                        {
                            control = LoadControl("uc_masterdata_worktype_action.ascx", id);
                        }
                    }
                    break;
                case "6":
                    link = "Staff";
                    control = Page.LoadControl("uc_masterdata_staff.ascx");
                    break;
                case "7":
                    link = "Department";

                    if (action.Trim().Length <= 0)
                    {
                        control = Page.LoadControl("uc_masterdata_department.ascx");
                    }
                    else
                    {
                        string id = "";
                        if (Request.QueryString["id"] != null)
                            id = Request.QueryString["id"].ToString();

                        control = LoadControl("uc_masterdata_department_action.ascx", id);
                    }
                    break;
                case "8":
                    link = "Unit";

                    if (action.Trim().Length <= 0)
                    {
                        control = Page.LoadControl("uc_masterdata_unit.ascx");
                    }
                    else
                    {
                        string id = "";
                        if (Request.QueryString["id"] != null)
                            id = Request.QueryString["id"].ToString();

                        if (action.Equals("delete"))
                        {
                            ExecuteData xl = new ExecuteData();

                            string sql = " 	SELECT COUNT(*) as sodong FROM SanPham WHERE dvt_fk = '" + id + "'";
                            object obj = xl.ExecuteScalarSQL(sql);
                            if (obj != null)
                            {
                                if (int.Parse(obj.ToString()) > 0)
                                {
                                    sql = "UPDATE DonViTinh SET trangthai = 0 WHERE pk_seq = '" + id + "' ";
                                    xl.ExecuteNonQuerySQL(sql);
                                }
                                else
                                {
                                    sql = "DELETE QUYCACH WHERE dvt1_fk = '" + id + "' ";
                                    xl.ExecuteNonQuerySQL(sql);

                                    sql = "DELETE DonViTinh WHERE pk_seq = '" + id + "' ";
                                    xl.ExecuteNonQuerySQL(sql);
                                }
                            }

                            Response.Redirect("MasterData.aspx?func=8");
                        }
                        else
                        {
                            control = LoadControl("uc_masterdata_unit.ascx", id);
                        }
                    }

                    break;
                case "9":
                    link = "Setup Staff";

                    if (action.Trim().Length <= 0)
                    {
                        control = Page.LoadControl("uc_masterdata_setupstaff.ascx");
                    }
                    else
                    {
                        string id = "";
                        if (Request.QueryString["id"] != null)
                            id = Request.QueryString["id"].ToString();

                        control = LoadControl("uc_masterdata_setupstaff_action.ascx", id);
                    }
                    break;
                case "999":
                    link = "Change password";
                    control = Page.LoadControl("uc_administrator_changepassword.ascx");
                    break;

                default:
                    link = " ";

                    control = Page.LoadControl("uc_indexMasterdata.ascx");

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