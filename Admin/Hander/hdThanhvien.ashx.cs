using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HLVTimeSheet.Model;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Drawing;
using HLVTimeSheet.AcsessData;
using System.Data;

namespace HLVTimeSheet.Admin.Hander
{
    /// <summary>
    /// Summary description for hdThanhvien
    /// </summary>
    public class hdThanhvien : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;

            string action = "";
            if (context.Request.QueryString["action"] != null)
                action = context.Request.QueryString["action"].ToString();

            if (action.Equals("dangnhap"))
            {
                DangNhapHeThong(context);
            }
            else if (action.Equals("dangxuat"))
            {
                DangXuatHeThong(context);
            }
            else if (action.Equals("bchoahong"))
            {
                BaoCaoHoaHong(context);
            }
        }

        private void BaoCaoHoaHong(HttpContext context)
        {
            context.Response.AddHeader("content-disposition", "attachment; filename=DanhSachHoaHongThanhVien.xls");
            context.Response.ContentType = "application/ms-excel";
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string exportContent = TaoBaoCao(context);
            response.Write(exportContent);
        }

        private string TaoBaoCao(HttpContext context)
        {
            string hoahongId = "";
            if (context.Request.QueryString["hoahongId"] != null)
                hoahongId = context.Request.QueryString["hoahongId"].ToString();

            string laymatai = "";
            if (context.Request.QueryString["laymatai"] != null)
                laymatai = context.Request.QueryString["laymatai"].ToString();
           
            string ngaythang = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();

            string _exportContent = "";
            using (StringWriter sb = new StringWriter())
            {
                using (HtmlTextWriter htmlWriter = new HtmlTextWriter(sb))
                {
                    Table table = new Table();
                    table.GridLines = GridLines.Both;

                    //table.BorderWidth = new Unit(1);

                    //Tieu de
                    TableHeaderCell header = new TableHeaderCell();
                    header.RowSpan = 1;
                    header.ColumnSpan = 9;
                    header.Text = "DANH SÁCH HOA HỒNG THÀNH VIÊN";
                    header.Font.Bold = true;
                    header.BackColor = Color.Red;
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;

                    // Add the header to a new row.
                    TableRow headerRow = new TableRow();
                    headerRow.Cells.Add(header);

                    table.Rows.Add(headerRow);


                    //Tieu de
                    headerRow = new TableRow();
                    header = new TableHeaderCell();

                    header.Text = "Ngày báo cáo: " + ngaythang;
                    header.ColumnSpan = 9;
                    header.Font.Bold = true;
                    //header.BackColor = Color.LightGray;
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);

                    table.Rows.Add(headerRow);


                    //Chen 2 row khoang cach
                    for (int i = 0; i < 1; i++)
                    {
                        TableRow rowS = new TableRow();
                        table.Rows.Add(rowS);
                    }

                    string[] tieude = new string[] { " STT ", "ID Cây chính", "User Name", "Họ và tên", "Số CMND", "Level", "Doanh thu", "Ngày đạt", "Từ thành viên" };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;
                        header.BackColor = Color.LightGray;
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();

                    string query = " SELECT ROW_NUMBER() OVER(ORDER BY STT ASC) AS stt, Idcaychinh, userName, hoten, cmnd, ngaysinh, taikhoannganhang, ngaydat as ngaynhap, isnull(level, -1) as level, isnull(sotien, 0) as sotien " +
                                   " FROM ThanhVien WHERE 1 = 1  ";

                    if (laymatai.Equals("0"))
                        query += " and isMatai = '0' ";

                    if (hoahongId.Trim().Length > 0)
                    {
                        query += " and isnull(level, -1) >= '" + hoahongId + "' ";
                    }

                    //if (txtCMND.Value.Trim().Length > 0)
                       // query += " and ( cmnd like N'%" + txtCMND.Value.Trim() + "%' or userName like N'%" + txtCMND.Value.Trim() + "%' ) ";

                    //Nếu là thành viên đăng nhập thì chỉ thấy thông tin của nó
                    if (context.Session["isThanhvien"].ToString().Equals("1"))
                    {
                        query += " and ( Idcaychinh + cmnd ) = '" + context.Session["dnThanhvien"].ToString() + "' ";
                    }

                    query += " ORDER BY stt asc";

                    DataTable dt = xl.ReadTable(query);

                    double tongtienHH = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        double sotien = double.Parse(dt.Rows[i]["sotien"].ToString());
                        tongtienHH += sotien;

                        string[] data = new string[] { dt.Rows[i]["stt"].ToString(), dt.Rows[i]["Idcaychinh"].ToString(), dt.Rows[i]["userName"].ToString(), dt.Rows[i]["hoten"].ToString(), dt.Rows[i]["cmnd"].ToString(), 
                                                        dt.Rows[i]["level"].ToString(), FormatString.ForMatNumber(sotien.ToString()), dt.Rows[i]["ngaynhap"].ToString(), " " };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();

                            if (j >= 10)
                                cell.HorizontalAlign = HorizontalAlign.Right;

                            cell.Text = data[j];

                            //cell.Attributes.Add("style", @"mso-number-format:\@;");

                            row.Cells.Add(cell);
                        }

                        table.Rows.Add(row);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        string[] data = new string[] { "TỔNG CỘNG", Math.Round(tongtienHH, 2).ToString(), "", "" };

                        TableRow row = new TableRow();
                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Right;
                            cell.BackColor = Color.GreenYellow;

                            if (j == 0)
                            {
                                cell.HorizontalAlign = HorizontalAlign.Center;
                                cell.ColumnSpan = 6;
                            }

                            cell.Text = data[j];

                            //cell.Attributes.Add("style", @"mso-number-format:\@;");

                            row.Cells.Add(cell);
                        }

                        table.Rows.Add(row);
                    }

                    table.RenderControl(htmlWriter);
                    _exportContent = sb.ToString();
                }
            }
            return _exportContent;
        }

        private void DangXuatHeThong(HttpContext context)
        {
            throw new NotImplementedException();
        }

        private void DangNhapHeThong(HttpContext context)
        {
            //string Tendangnhap = "";
            //string Matkhau = "";

            //if (context.Request.QueryString["Tendangnhap"] != null)
            //    Tendangnhap = context.Request.QueryString["Tendangnhap"].ToString();

            //if (context.Request.QueryString["Matkhau"] != null)
            //    Matkhau = context.Request.QueryString["Matkhau"].ToString();

            //ThanhVien tv = new ThanhVien();
            //string kq = tv.CheckDangNhap(Tendangnhap, Matkhau);

            
            //if (kq.Length > 10)
            //{
            //    context.Response.Write(kq);
            //    return;
            //}
            //if (kq.Equals("-2"))
            //{
            //    context.Response.Write("Bạn đang truy cập không hợp lệ. Vui lòng liên hệ đến admin để được hỗ trợ. ");
            //    return;
            //}
            //else if (kq.Equals("-1"))
            //{
            //    context.Response.Write("Tên đăng nhập không đúng");
            //    return;
            //}
            //else if (kq.Equals("0"))
            //{
            //    context.Response.Write("Mật khẩu không đúng");
            //    return;
            //}
            
            context.Response.Write("OK");
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