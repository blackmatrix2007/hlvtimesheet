using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HLVTimeSheet.AcsessData;

namespace HLVTimeSheet.Admin
{
    public partial class uc_administrator_rolepermission_action : System.Web.UI.UserControl
    {
        public string id;
        public string trangthai;
        public string language = "1";

        public uc_administrator_rolepermission_action()
        {
            this.id = "";
            this.trangthai = "0";
        }

        public uc_administrator_rolepermission_action(string id)
        {
            this.id = id;
            this.trangthai = "0";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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

            ExecuteData xl = new ExecuteData();

            if (!IsPostBack)
            {
                chkTrangThai.Checked = true;

                if (this.id.Trim().Length > 0)
                {
                    string query = "SELECT maquyen, tenquyen, trangthai FROM NHOMQUYEN WHERE pk_seq = '" + this.id + "' ";
                    DataTable dtNv = xl.ReadTable(query);
                    if (dtNv.Rows.Count > 0)
                    {
                        txtMa.Value = dtNv.Rows[0]["maquyen"].ToString();
                        txtTen.Value = dtNv.Rows[0]["tenquyen"].ToString();
                        if (dtNv.Rows[0]["trangthai"].ToString().Equals("1"))
                            chkTrangThai.Checked = true;
                        else
                            chkTrangThai.Checked = false;
                    }
                }

                //INIT QUYEN
                string sql = "";

                if (Session["userId"].ToString().Equals("100001"))
                {
                    sql = " SELECT pk_seq as group_fk, a.ten_group AS nameEnglish, a.ten_top_menu AS tenTiengViet, '-1' as maungdung, STT,  -1 as STT_CHUCNANG, 0 as IS_GROUP, '' AS tenChucNangTiengViet, '' AS functionEnglish, " +
                    " ISNULL(b.chkALL_XEM, 0) as xem, ISNULL(b.chkALL_XOA, 0) as xoa, ISNULL(b.chkALL_TAOMOI, 0) as taomoi, ISNULL(b.chkALL_CAPNHAT, 0) as capnhat, ISNULL(b.chkALL_CHOT, 0) as chot, ISNULL(b.chkALL_HUYCHOT, 0) as huyChot  " +
                    " FROM Group_ChucNang a LEFT JOIN NhomQuyen_GroupChucNang b on a.pk_seq = b.group_fk and b.nhomquyen_fk = '" + (this.id.Trim().Length > 0 ? this.id : "-1") + "' WHERE HIENTHI = '1' " +
                    " UNION " +
                    " SELECT b.pk_seq as group_fk, '' AS nameEnglish, '' AS tenTiengViet, a.pk_seq as maungdung, b.STT, a.stt as STT_CHUCNANG, a.IS_GROUP, a.ten AS tenChucNangTiengViet, a.nameEnglish AS functionEnglish, " +
                    "      ISNULL(c.xem, 0) as xem, ISNULL(c.xoa, 0) as xoa, ISNULL(c.taomoi, 0) as taomoi, ISNULL(c.sua, 0) as capnhat, ISNULL(c.chot, 0) as chot, ISNULL(c.huyChot, 0) as huyChot   " +
                    " FROM ChucNang a INNER JOIN Group_ChucNang b on a.GROUP_FK = b.pk_seq " +
                    "     LEFT JOIN NhomQuyen_ChucNang_ChiTiet c on a.pk_seq = c.chucnang_fk and c.nhomquyen_fk = '" + (this.id.Trim().Length > 0 ? this.id : "-1") + "' " +
                    " WHERE a.hienthi = '1' and a.hienthi_admin = '1' and b.HIENTHI = '1' " +
                    " ORDER BY STT asc, STT_ChucNang asc ";
                }
                else
                {
                    sql = " SELECT pk_seq as group_fk, a.ten_group AS nameEnglish, a.ten_top_menu AS tenTiengViet, '-1' as maungdung, STT,  -1 as STT_CHUCNANG, 0 as IS_GROUP, '' AS tenChucNangTiengViet, '' AS functionEnglish, " +
                    " ISNULL(b.chkALL_XEM, 0) as xem, ISNULL(b.chkALL_XOA, 0) as xoa, ISNULL(b.chkALL_TAOMOI, 0) as taomoi, ISNULL(b.chkALL_CAPNHAT, 0) as capnhat, ISNULL(b.chkALL_CHOT, 0) as chot, ISNULL(b.chkALL_HUYCHOT, 0) as huyChot  " +
                    " FROM Group_ChucNang a LEFT JOIN NhomQuyen_GroupChucNang b on a.pk_seq = b.group_fk and b.nhomquyen_fk = '" + (this.id.Trim().Length > 0 ? this.id : "-1") + "' WHERE HIENTHI = '1' " +
                    " UNION " +
                    " SELECT b.pk_seq as group_fk, '' AS nameEnglish, '' AS tenTiengViet, a.pk_seq as maungdung, b.STT, a.stt as STT_CHUCNANG, a.IS_GROUP, a.ten AS tenChucNangTiengViet, a.nameEnglish AS functionEnglish, " +
                    "      ISNULL(c.xem, 0) as xem, ISNULL(c.xoa, 0) as xoa, ISNULL(c.taomoi, 0) as taomoi, ISNULL(c.sua, 0) as capnhat, ISNULL(c.chot, 0) as chot, ISNULL(c.huyChot, 0) as huyChot   " +
                    " FROM ChucNang a INNER JOIN Group_ChucNang b on a.GROUP_FK = b.pk_seq " +
                    "     LEFT JOIN NhomQuyen_ChucNang_ChiTiet c on a.pk_seq = c.chucnang_fk and c.nhomquyen_fk = '" + (this.id.Trim().Length > 0 ? this.id : "-1") + "' " +
                    " WHERE a.hienthi = '1' and a.hienthi_admin = '1' and b.HIENTHI = '1' " +
                    " ORDER BY STT asc, STT_ChucNang asc ";
                }
                DataTable dtKh = xl.ReadTable(sql);

                if (dtKh.Rows.Count > 0)
                {
                    string str = " ";

                    for (int i = 0; i < dtKh.Rows.Count; i++)
                    {
                        str += " <tr> ";

                        string groupId = dtKh.Rows[i]["group_fk"].ToString();
                        string tenGROUP = "";
                        string tenchucnang = "";

                        if(language.Equals("1"))
                        {
                            tenGROUP = dtKh.Rows[i]["nameEnglish"].ToString().ToUpper();
                            tenchucnang = dtKh.Rows[i]["functionEnglish"].ToString();
                        }
                        else if (language.Equals("2"))
                        {
                            tenGROUP = dtKh.Rows[i]["tenTiengViet"].ToString().ToUpper();
                            tenchucnang = dtKh.Rows[i]["tenChucNangTiengViet"].ToString();
                        }

                        string is_group = dtKh.Rows[i]["IS_GROUP"].ToString();

                        string xem = dtKh.Rows[i]["xem"].ToString();
                        string xoa = dtKh.Rows[i]["xoa"].ToString();
                        string taomoi = dtKh.Rows[i]["taomoi"].ToString();
                        string capnhat = dtKh.Rows[i]["capnhat"].ToString();
                        string chot = dtKh.Rows[i]["chot"].ToString();
                        string huychot = dtKh.Rows[i]["huychot"].ToString();

                        if (tenGROUP.Trim().Length > 0)
                        {
                            str += " <td style='font-weight:bold; font-size:1.2em;' > <input type='hidden' name='groupIds'  value='" + groupId + "' /> " + tenGROUP + " </td> ";

                            str += " <td style='text-align:center;' > <input type='checkbox' name='" + groupId + "_GRxemIds'  value='" + dtKh.Rows[i]["group_fk"].ToString() + "' " + (xem.Equals("1") ? "checked" : "") + " onchange=\"chonHetGroup(this, '" + groupId + "_xemIds')\" />  </td> ";
                            str += " <td style='text-align:center;' > <input type='checkbox' name='" + groupId + "_GRxoaIds'  value='" + dtKh.Rows[i]["group_fk"].ToString() + "' " + (xoa.Equals("1") ? "checked" : "") + " onchange=\"chonHetGroup(this, '" + groupId + "_xoaIds')\"  />  </td> ";
                            str += " <td style='text-align:center;' > <input type='checkbox' name='" + groupId + "_GRtaomoiIds'  value='" + dtKh.Rows[i]["group_fk"].ToString() + "' " + (taomoi.Equals("1") ? "checked" : "") + " onchange=\"chonHetGroup(this, '" + groupId + "_taomoiIds')\"  />  </td> ";
                            str += " <td style='text-align:center;' > <input type='checkbox' name='" + groupId + "_GRcapnhatIds'  value='" + dtKh.Rows[i]["group_fk"].ToString() + "' " + (capnhat.Equals("1") ? "checked" : "") + " onchange=\"chonHetGroup(this, '" + groupId + "_capnhatIds')\" />  </td> ";
                            str += " <td style='text-align:center;' > <input type='checkbox' name='" + groupId + "_GRchotIds'  value='" + dtKh.Rows[i]["group_fk"].ToString() + "' " + (chot.Equals("1") ? "checked" : "") + " onchange=\"chonHetGroup(this, '" + groupId + "_chotIds')\" />  </td> ";
                            str += " <td style='text-align:center;' > <input type='checkbox' name='" + groupId + "_GRhuychotIds'  value='" + dtKh.Rows[i]["group_fk"].ToString() + "' " + (huychot.Equals("1") ? "checked" : "") + " onchange=\"chonHetGroup(this, '" + groupId + "_huychotIds')\" />  </td> ";

                        }
                        else
                        {
                            if (is_group.Equals("1"))
                            {
                                str += " <td style='font-weight:bold; font-size:1.1em; padding-left:25px; color:red;' >" + tenchucnang + " </td> ";
                                
                                str += " <td style='text-align:center;' > </td> ";
                                str += " <td style='text-align:center;' > </td> ";
                                str += " <td style='text-align:center;' > </td> ";
                                str += " <td style='text-align:center;' > </td> ";
                                str += " <td style='text-align:center;' > </td> ";
                                str += " <td style='text-align:center;' > </td> ";
                            }
                            else
                            {
                                str += " <td style='padding-left:50px;' >" + tenchucnang + " </td> ";
                                
                                str += " <td style='text-align:center;' > <input type='checkbox' name='" + groupId + "_xemIds'  value='" + dtKh.Rows[i]["maungdung"].ToString() + "' " + (xem.Equals("1") ? "checked" : "") + " />  </td> ";
                                str += " <td style='text-align:center;' > <input type='checkbox' name='" + groupId + "_xoaIds'  value='" + dtKh.Rows[i]["maungdung"].ToString() + "' " + (xoa.Equals("1") ? "checked" : "") + "  />  </td> ";
                                str += " <td style='text-align:center;' > <input type='checkbox' name='" + groupId + "_taomoiIds'  value='" + dtKh.Rows[i]["maungdung"].ToString() + "' " + (taomoi.Equals("1") ? "checked" : "") + " />  </td> ";
                                str += " <td style='text-align:center;' > <input type='checkbox' name='" + groupId + "_capnhatIds'  value='" + dtKh.Rows[i]["maungdung"].ToString() + "' " + (capnhat.Equals("1") ? "checked" : "") + " />  </td> ";
                                str += " <td style='text-align:center;' > <input type='checkbox' name='" + groupId + "_chotIds'  value='" + dtKh.Rows[i]["maungdung"].ToString() + "' " + (chot.Equals("1") ? "checked" : "") + " />  </td> ";
                                str += " <td style='text-align:center;' > <input type='checkbox' name='" + groupId + "_huychotIds'  value='" + dtKh.Rows[i]["maungdung"].ToString() + "' " + (huychot.Equals("1") ? "checked" : "") + " />  </td> ";
                            }
                        }

                        str += " </tr> ";
                    }

                    ltQuyen_CN.Text = str;

                }
            }

        }

        protected void lbLuuLai_Click(object sender, EventArgs e)
        {
            if (txtMa.Value.Trim().Length <= 0)
            {
                if (language.Equals("1"))
                {
                    lblError.Text = "Please you must be input code! ";
                }
                else if (language.Equals("2"))
                {
                    lblError.Text = "Vui lòng nhập mã";
                }
                
                return;
            }

            if (txtTen.Value.Trim().Length <= 0)
            {
                if (language.Equals("1"))
                {
                    lblError.Text = "Please you must be input description! ";
                }
                else if (language.Equals("2"))
                {
                    lblError.Text = "Vui lòng nhập mô tả nhóm quyền ";
                }

                
                return;
            }

            string trangthai = "0";
            if (chkTrangThai.Checked)
                trangthai = "1";
            ExecuteData xl = new ExecuteData();

            string query = "";
            // Kiểm tra  mã 
            query = " SELECT COUNT(*) FROM NhomQuyen WHERE maquyen = N'" + txtMa.Value + "' ";
            if (this.id.Length > 3)
                query += " AND pk_seq != '" + this.id + "' ";

            object obj = xl.ExecuteScalarSQL(query);
            if(obj != null)
            {
                if(int.Parse(obj.ToString()) > 0)
                {
                    if (language.Equals("1"))
                    {
                        lblError.Text = "Code is duplicate! Please input again. ";
                    }
                    else if (language.Equals("2"))
                    {
                        lblError.Text = "Mã đã tồn tại. Vui lòng kiểm tra lại ";
                    }
                    
                    return;
                }
            }

            if (this.id.Trim().Length <= 0)
            {
                query = "INSERT NhomQuyen(maquyen, tenquyen, trangthai, nguoitao, nguoisua)  " +
                        "SELECT N'" + txtMa.Value + "', N'" + txtTen.Value + "', '" + trangthai + "', '" + Session["userId"].ToString() + "', '" + Session["userId"].ToString() + "' ";
            }
            else
            {
                query = "UPDATE NhomQuyen SET maquyen = N'" + txtMa.Value + "', tenquyen = N'" + txtTen.Value + "', trangthai = '" + trangthai + "', " +
                        "  nguoisua = '" + Session["userId"].ToString() + "', ngaysua = getdate() WHERE pk_seq = '" + this.id + "' ";
            }

            xl.ExecuteNonQuerySQL(query);
            Response.Redirect("Administrator.aspx?func=91");
            
        }
    
    }
}