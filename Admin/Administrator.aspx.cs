using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using HLVTimeSheet.Admin.UserControls;
using HLVTimeSheet.AcsessData;

namespace HLVTimeSheet.Admin
{
    public partial class Administrator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucLeftMenu ucleft = (ucLeftMenu)this.Master.FindControl("ucLeftMenu1");
            ucleft.action = "9";

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
                case "31":
                    link = "Branch";

                    if (action.Trim().Length <= 0)
                    {
                        control = Page.LoadControl("uc_tochuc_chinhanh.ascx");
                    }
                    else
                    {
                        string id = "";
                        if (Request.QueryString["id"] != null)
                            id = Request.QueryString["id"].ToString();

                        if (action.Equals("delete"))
                        {
                            ExecuteData xl = new ExecuteData();

                            string sql = "UPDATE NhaPhanPhoi SET trangthai = 0 WHERE pk_seq = '" + id + "' ";
                            xl.ExecuteNonQuerySQL(sql);
                            Response.Redirect("Administrator.aspx?func=31");
                        }
                        else
                        {
                            control = LoadControl("uc_tochuc_chinhanh.ascx", id);
                        }
                    }
                    break;
                case "94":
                    link = "Department";

                    if (action.Trim().Length <= 0)
                    {
                        control = Page.LoadControl("uc_administrator_department.ascx");
                    }
                    else
                    {
                        string id = "";
                        if (Request.QueryString["id"] != null)
                            id = Request.QueryString["id"].ToString();

                        if (action.Equals("delete"))
                        {
                            ExecuteData xl = new ExecuteData();

                            string sql = "UPDATE PhongBan SET trangthai = 0 WHERE pk_seq = '" + id + "' ";
                            xl.ExecuteNonQuerySQL(sql);
                            Response.Redirect("Administrator.aspx?func=94");
                        }
                        else
                        {
                            control = LoadControl("uc_administrator_department.ascx", id);
                        }
                    }
                    break;
                case "95":
                    link = "Staff";

                    if (action.Trim().Length <= 0)
                    {
                        control = Page.LoadControl("uc_administrator_staff.ascx");
                    }
                    else
                    {
                        string id = "";
                        if (Request.QueryString["id"] != null)
                            id = Request.QueryString["id"].ToString();

                        if (action.Equals("delete"))
                        {
                            ExecuteData xl = new ExecuteData();

                            string sql = "UPDATE DanhSachNhanSu SET trangthai = 0 WHERE pk_seq = '" + id + "' ";
                            xl.ExecuteNonQuerySQL(sql);
                            Response.Redirect("Administrator.aspx?func=95");
                        }
                        else
                        {
                            control = LoadControl("uc_administrator_staff_action.ascx", id);
                        }
                    }
                    break;
                case "96":
                    link = "Position";

                    if (action.Trim().Length <= 0)
                    {
                        control = Page.LoadControl("uc_administrator_position.ascx");
                    }
                    else
                    {
                        string id = "";
                        if (Request.QueryString["id"] != null)
                            id = Request.QueryString["id"].ToString();

                        if (action.Equals("delete"))
                        {
                            ExecuteData xl = new ExecuteData();

                            string sql = "UPDATE ChucVu SET trangthai = 0 WHERE pk_seq = '" + id + "' ";
                            xl.ExecuteNonQuerySQL(sql);
                            Response.Redirect("Administrator.aspx?func=96");
                        }
                        else
                        {
                            control = LoadControl("uc_administrator_position.ascx", id);
                        }
                    }
                    break;
                case "91":
                    link = "Role permission";

                    if (action.Trim().Length <= 0)
                    {
                        control = Page.LoadControl("uc_administrator_rolepermission.ascx");
                    }
                    else
                    {
                        string id = "";
                        if (Request.QueryString["id"] != null)
                            id = Request.QueryString["id"].ToString();

                        if (action.Equals("delete"))
                        {
                            ExecuteData xl = new ExecuteData();
                            xl.ExecuteNonQuerySQL("UPDATE NHOMQUYEN SET trangthai = 0 WHERE pk_seq = '" + id + "' ");

                            Response.Redirect("Administrator.aspx?func=91");
                        }
                        else
                        {
                            control = LoadControl("uc_administrator_rolepermission_action.ascx", id);
                        }
                    }

                    break;
                case "92":
                    link = "User";

                    if (action.Trim().Length <= 0)
                    {
                        control = Page.LoadControl("uc_administrator_account.ascx");
                    }
                    else
                    {
                        string id = "";
                        if (Request.QueryString["id"] != null)
                            id = Request.QueryString["id"].ToString();

                        if (action.Equals("delete"))
                        {
                            try
                            {
                                ExecuteData xl = new ExecuteData();
                                xl.ExecuteNonQuerySQL("UPDATE NHANVIEN SET trangthai = 0 WHERE pk_seq = '" + id + "' ");
                            }
                            catch { }

                            Response.Redirect("Administrator.aspx?func=92");
                        }
                        else
                        {
                            control = LoadControl("uc_administrator_account_action.ascx", id);
                        }
                    }

                    break;
                case "97":
                    link = "Team work";

                    if (action.Trim().Length <= 0)
                    {
                        control = Page.LoadControl("uc_administrator_teamwork.ascx");
                    }
                    else
                    {
                        string id = "";
                        if (Request.QueryString["id"] != null)
                            id = Request.QueryString["id"].ToString();

                        if (action.Equals("delete"))
                        {
                            try
                            {
                                ExecuteData xl = new ExecuteData();
                                xl.ExecuteNonQuerySQL("UPDATE NhomLamViec SET trangthai = 0 WHERE pk_seq = '" + id + "' ");
                            }
                            catch { }

                            Response.Redirect("Administrator.aspx?func=97");
                        }
                        else
                        {
                            control = LoadControl("uc_administrator_teamwork_action.ascx", id);
                        }
                    }

                    break;
                case "98":
                    link = "Holiday";

                    if (action.Trim().Length <= 0)
                    {
                        control = Page.LoadControl("uc_administrator_holiday.ascx");
                    }
                    else
                    {
                        string id = "";
                        if (Request.QueryString["id"] != null)
                            id = Request.QueryString["id"].ToString();

                        if (action.Equals("delete"))
                        {
                            try
                            {
                                ExecuteData xl = new ExecuteData();
                                xl.ExecuteNonQuerySQL("DELETE NgayNghiLe WHERE pk_seq = '" + id + "' ");
                            }
                            catch { }

                            Response.Redirect("Administrator.aspx?func=98");
                        }
                        else
                        {
                            control = LoadControl("uc_administrator_holiday_action.ascx", id);
                        }
                    }

                    break;
                case "99":
                    link = "Division";

                    if (action.Trim().Length <= 0)
                    {
                        control = Page.LoadControl("uc_administrator_division.ascx");
                    }
                    else
                    {
                        string id = "";
                        if (Request.QueryString["id"] != null)
                            id = Request.QueryString["id"].ToString();

                        if (action.Equals("delete"))
                        {
                            try
                            {
                                ExecuteData xl = new ExecuteData();
                                xl.ExecuteNonQuerySQL("DELETE BoPhan WHERE pk_seq = '" + id + "' ");
                            }
                            catch { }

                            Response.Redirect("Administrator.aspx?func=99");
                        }
                        else
                        {
                            control = LoadControl("uc_administrator_division.ascx", id);
                        }
                    }

                    break;
                case "93":
                    link = "Master control";
                    control = Page.LoadControl("uc_administrator_mastercontrol.ascx");
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