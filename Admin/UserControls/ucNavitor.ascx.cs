using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HLVTimeSheet.Admin.UserControls
{
    public partial class ucNavitor : System.Web.UI.UserControl
    {
        public string parent = "";
        public string parentLevel2 = "";
        public string action = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (parent.Trim().Length > 0)
            {
                this.ucNvparent.Text = parent;
            }

            if (parentLevel2.Trim().Length > 0)
            {
                this.ucNvparentLevel2.Text = parentLevel2;
            }

            if (action.Trim().Length > 0)
            {
                this.ucNvaction.Text = action;
            }
        }
    }
}