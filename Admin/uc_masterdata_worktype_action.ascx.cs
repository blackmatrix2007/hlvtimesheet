using HLVTimeSheet.AcsessData;
using HLVTimeSheet.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HLVTimeSheet.Admin
{
    public partial class uc_masterdata_worktype_action : System.Web.UI.UserControl
    {
        public string id;
        public string language = "1";
        public string nhomsanpham_fk = "0";
        public uc_masterdata_worktype_action()
        {
            this.id = "";
        }

        public uc_masterdata_worktype_action(string id)
        {
            this.id = id;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ExecuteData xl = new ExecuteData();

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

            if(Request.QueryString["groupItem"] != null)
                this.nhomsanpham_fk = Request.QueryString["groupItem"].ToString();

            if (!IsPostBack)
            {              
                string query = "SELECT pk_seq, ten FROM DonViTinh WHERE trangthai = '1' ORDER BY ten ASC ";
                DataTable dt = xl.ReadTable(query);

                //ddlDonViTinh.Items.Add(new ListItem("Select unit", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlDonViTinh.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                query = "SELECT pk_seq, ten FROM NhomSanPham WHERE trangthai = '1' ORDER BY ten ASC ";
                dt = xl.ReadTable(query);

                ddlNhomSanPham.Items.Add(new ListItem("Select group", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlNhomSanPham.Items.Add(new ListItem(dt.Rows[i]["ten"].ToString(), dt.Rows[i]["pk_seq"].ToString()));
                }

                ddlNhomSanPham.SelectedValue = this.nhomsanpham_fk;

                txtMa.Text = "";
                txtTen.Text = "";
                txtNameEnglish.Text = "";
                txtCycleTime.Value = "0";
                txtCycleTarget.Value = "0";
                txtDonGia.Value = "0";

                chkTrangThai.Checked = true;
            }

            if (this.id.Trim().Length > 0)
            {
                string query = "SELECT pk_seq, ma, ten, nameEnglish, dvt_fk, cycletime, cycletarget, showCustomer, nhomsanpham_fk, giaban, trangthai " +
                " FROM SanPham WHERE pk_seq = '" + id + "' ";
                DataTable dt = xl.ReadTable(query);

                if (dt.Rows.Count > 0)
                {
                    txtMa.Text = dt.Rows[0]["ma"].ToString();
                    txtTen.Text = dt.Rows[0]["ten"].ToString();
                    txtNameEnglish.Text = dt.Rows[0]["nameEnglish"].ToString();
                    txtCycleTime.Value = dt.Rows[0]["cycletime"].ToString();
                    txtCycleTarget.Value = dt.Rows[0]["cycletarget"].ToString();
                    txtDonGia.Value = FormatString.ForMatNumber(dt.Rows[0]["giaban"].ToString());
                    ddlNhomSanPham.SelectedValue = dt.Rows[0]["nhomsanpham_fk"].ToString();
                    ddlDonViTinh.SelectedValue = dt.Rows[0]["dvt_fk"].ToString();

                    if (dt.Rows[0]["trangthai"].ToString().Equals("1"))
                        chkTrangThai.Checked = true;
                    if (dt.Rows[0]["showCustomer"].ToString().Equals("1"))
                        ckShowCustomer.Checked = true;
                }

            }
            //this.loadJobList();
        }
       
        protected void loadJobList()
        {
            string query = "";
            int index = 0;
            string str = "";
            ExecuteData xl = new ExecuteData();
            DataTable tbUnit = xl.ReadTable("SELECT pk_seq, ten FROM DonViTinh WHERE trangthai = '1' ORDER BY ten ASC ");
            
            int code = 0;

            query = "SELECT TOP(1) REPLACE(ma, 'HLV', '') FROM SanPham ORDER BY pk_seq DESC ";
            object obj = xl.ExecuteScalarSQL(query);
            if (obj != null)
                code = int.Parse(obj.ToString());

            if (this.id.Trim().Length > 3)
            {
                query = "SELECT '1' AS STT, a.pk_seq AS sanpham_fk, a.ma AS masp, a.ten AS tensp, a.dvt_fk, a.cycletime, a.giaban " +                   
                    " FROM SanPham a  " +
                    " WHERE a.trangThai = '1' AND a.nganhhang_fk = '" + this.id + "' " +
                    " ORDER BY a.ma ASC ";
                DataTable dt = xl.ReadTable(query);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string _delete = " <a href='javascript:deleteSanPham(" + i + ");' style='color:red;' > <i class='fa fa-fw fa-trash-o' style='color:red;'></i> </a>   ";

                        str += " <tr> ";
                        str += " <td style='display:none;'><input type='hidden' class='form-control' name='sanpham_fk' value='" + dt.Rows[i]["sanpham_fk"].ToString() + "' ></td> ";
                        
                        str += " <td style='text-align:center;'>" + _delete + "</td>";                        
                        str += " <td><input type='text' class='form-control' name='masp' style='text-align:left; font-size:small; padding:1px;' value='" + dt.Rows[i]["masp"].ToString() + "' autocomplete='off' readonly></td>";
                        str += " <td><input type='text' class='form-control' name='tensp' style='text-align:left; font-size:small; padding:1px;' value='" + dt.Rows[i]["tensp"].ToString() + "' autocomplete='off'></td>";
                        str += " <td>" + this.getUnit(tbUnit, dt.Rows[i]["dvt_fk"].ToString(), i) + "</td> ";                        
                        str += " <td><input type='text' class='form-control' name='cycletime' style='text-align:right; font-size:small;' value='" +dt.Rows[i]["cycletime"].ToString() + "'></td> ";
                        str += " <td><input type='text' class='form-control' name='giaban' style='text-align:right; font-size:small;' value='" + FormatString.ForMatNumber(dt.Rows[i]["giaban"].ToString()) + "'  ></td> ";                        
                        str += " </tr> ";
                    }

                    ltSanPham.Text = str;
                    index = dt.Rows.Count;
                }
            }

            str = "";
            for (int i = index; i < index + 10; i++)
            {
                code++;
                string strCode = code.ToString();
                string a = "";

                while (strCode.Length < 4)
                {
                    a += "0";
                    strCode = a + code.ToString();
                }    
                
                strCode = "HLV" + strCode;

                string _delete = " <a href='javascript:deleteSanPham(" + i + ");' style='color:red;' > <i class='fa fa-fw fa-trash-o' style='color:red;'></i> </a>   ";

                str += " <tr> ";
                str += " <td style='display:none;'><input type='hidden' class='form-control' name='sanpham_fk' value='0' ></td> ";

                str += " <td style='text-align:center;'>" + _delete + "</td>";
                str += " <td><input type='text' class='form-control' name='masp' style='text-align:left; font-size:small; padding:1px;' value='" + strCode + "' autocomplete='off' readonly></td>";
                str += " <td><input type='text' class='form-control' name='tensp' style='text-align:left; font-size:small; padding:1px;' value='' autocomplete='off'></td>";
                str += " <td>" + this.getUnit(tbUnit, "", i) + "</td> ";
                str += " <td><input type='text' class='form-control' name='cycletime' style='text-align:right; font-size:small;' value=''></td> ";
                str += " <td><input type='text' class='form-control' name='giaban' style='text-align:right; font-size:small;' value=''  ></td> ";
                str += " </tr> ";
                str += " </tr> ";
            }
            ltSanPham.Text += str;
        }

        private string getUnit(DataTable tbUnit, string unitId, int pos)
        {
            string size = "<select class='form-control' name = 'unitId'>";
            size += " <option value='' > </option> ";
            for (int i = 0; i < tbUnit.Rows.Count; i++)
            {
                if (tbUnit.Rows[i]["pk_seq"].ToString().Equals(unitId))
                    size += " <option value='" + tbUnit.Rows[i]["pk_seq"].ToString() + "' selected >" + tbUnit.Rows[i]["ten"].ToString() + "</option> ";
                else
                    size += " <option value='" + tbUnit.Rows[i]["pk_seq"].ToString() + "'  >" + tbUnit.Rows[i]["ten"].ToString() + "</option> ";
            }
            size += " </select> ";
            return size;
        }
    }
}