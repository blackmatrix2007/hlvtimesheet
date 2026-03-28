using HLVTimeSheet.AcsessData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HLVTimeSheet.Admin
{
    public partial class Homepage : System.Web.UI.Page
    {
        public string[] roleSY = new string[] { "0", "0", "0", "0", "0", "0" };

        public string language = "1";
        public string hlvParkingCar = "";
        public string hlvOperationControl = "";
        public string hlvWorkSchedule = "";
        public string HLVWarehouseVP = "";
        public string HLVBondedWHHP = "";
        public string HLVNormalWHHP = "";
        public string hlvStaffingArrangement = "";
        public string hlvDocumentWarning = "";        
        public string hlvJisseki = "";
        public string hlvTimeSheet = "";

        public string rolePC = "";
        public string roleOC = "";
        public string roleWS = "";
        public string roleVPW = "";
        public string roleHPBondedWH = "";
        public string roleHPNormalWH = "";
        public string roleSA = "";
        public string roleDW = "";
        public string roleJisseki = "";
        public string roleTimeSheet = "";
        public string roleDevice = "1";         // DeviceManager - luôn hiển thị cho admin
        public string hlvDeviceManager = "Máy chấm công";
        public string yearNow = "";
        public string token = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userId"] == null || Session["userId"].ToString().Trim().Length <= 3 || Session["token"] == null || Session["token"].ToString().Trim().Length <= 3)
            {
                Response.Redirect("~/Admin/Login.aspx");
                return;
            }

            string lang = Request.QueryString["lang"];
            if (lang != null)
            {
                Session["language"] = lang;
                language = lang;
            }
            else if (Session["language"] != null)
            {
                language = Session["language"].ToString();
            }

            token = Session["token"].ToString();

            if (!IsPostBack)
            {
                this.InitData();
            }
        }
        private void InitData()
        {
            ExecuteData xl = new ExecuteData(@"103.159.50.204, 89", "GDC_HLV_ParkingCar", "giangdc", "giangdc@");
            yearNow = "2020 - " + DateTime.Now.ToString("yyyy");
            string sql = "SELECT ht.pk_seq, ht.ma, ht.ten, ht.nameEnglish, ISNULL((SELECT COUNT(*) FROM NhanVien_Quyen_HeThong a INNER JOIN NhanVien b ON a.nhanvien_fk = b.pk_seq WHERE b.token = '" + Session["token"].ToString() + "' AND hethong_fk = ht.pk_seq), 0) AS quyen " +
               " FROM HeThong ht ORDER BY ht.stt ";
            DataTable dt = xl.ReadTable(sql);
            if (dt.Rows.Count > 0)
            {
                if (language.Equals("1"))
                {
                    hlvParkingCar = dt.Rows[0]["nameEnglish"].ToString();
                    hlvOperationControl = dt.Rows[1]["nameEnglish"].ToString();
                    hlvWorkSchedule = dt.Rows[2]["nameEnglish"].ToString();
                    HLVWarehouseVP = dt.Rows[3]["nameEnglish"].ToString();
                    HLVBondedWHHP = dt.Rows[4]["nameEnglish"].ToString();
                    HLVNormalWHHP = dt.Rows[5]["nameEnglish"].ToString();
                    hlvStaffingArrangement = dt.Rows[6]["nameEnglish"].ToString();
                    hlvDocumentWarning = dt.Rows[7]["nameEnglish"].ToString();
                    hlvJisseki = dt.Rows[8]["nameEnglish"].ToString();
                    hlvTimeSheet = dt.Rows[9]["nameEnglish"].ToString();
                }
                else if (language.Equals("2"))
                {
                    hlvParkingCar = dt.Rows[0]["ten"].ToString();
                    hlvOperationControl = dt.Rows[1]["ten"].ToString();
                    hlvWorkSchedule = dt.Rows[2]["ten"].ToString();
                    HLVWarehouseVP = dt.Rows[3]["ten"].ToString();
                    HLVBondedWHHP = dt.Rows[4]["ten"].ToString();
                    HLVNormalWHHP = dt.Rows[5]["ten"].ToString();
                    hlvStaffingArrangement = dt.Rows[6]["ten"].ToString();
                    hlvDocumentWarning = dt.Rows[7]["ten"].ToString();
                    hlvJisseki = dt.Rows[8]["ten"].ToString();
                    hlvTimeSheet = dt.Rows[9]["nameEnglish"].ToString();
                }

                roleSY[0] = dt.Rows[0]["quyen"].ToString();
                roleSY[1] = dt.Rows[1]["quyen"].ToString();
                roleSY[2] = dt.Rows[2]["quyen"].ToString();
                roleSY[3] = dt.Rows[3]["quyen"].ToString();
                roleSY[4] = dt.Rows[4]["quyen"].ToString();
                roleSY[5] = dt.Rows[5]["quyen"].ToString();

                rolePC = dt.Rows[0]["quyen"].ToString();
                roleOC = dt.Rows[1]["quyen"].ToString();
                roleWS = dt.Rows[2]["quyen"].ToString();
                roleVPW = dt.Rows[3]["quyen"].ToString();
                roleHPBondedWH = dt.Rows[4]["quyen"].ToString();
                roleHPNormalWH = dt.Rows[5]["quyen"].ToString();
                roleSA = dt.Rows[6]["quyen"].ToString();
                roleDW = dt.Rows[7]["quyen"].ToString();
                roleJisseki = dt.Rows[8]["quyen"].ToString();
                roleTimeSheet = dt.Rows[9]["quyen"].ToString();
            }
        }
    }
}