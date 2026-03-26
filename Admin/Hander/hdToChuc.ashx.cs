using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HLVTimeSheet.Model;
using HLVTimeSheet.AcsessData;
using Aspose.Cells;

namespace HLVTimeSheet.Admin.Hander
{
    /// <summary>
    /// Summary description for hdToChuc
    /// </summary>
    public class hdToChuc : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;

            string action = "";
            if (context.Request.QueryString["action"] != null)
                action = context.Request.QueryString["action"].ToString();

            if (action.Equals("UpdateBrand"))
            {
                UpdateBrand(context);
            }
            else if (action.Equals("GetInforBrand"))
            {
                GetInforBrand(context);
            }
            else if (action.Equals("UpdateWarehouse")) // Warehouse
            {
                UpdateWarehouse(context);
            }
            else if (action.Equals("GetInforWarehouse"))
            {
                GetInforWarehouse(context);
            }
            else if (action.Equals("UpdatePosition")) // Position
            {
                UpdatePosition(context);
            }
            else if (action.Equals("GetInforPosition"))
            {
                GetInforPosition(context);
            }
            else if (action.Equals("UpdateDepartment")) // Department
            {
                UpdateDepartment(context);
            }
            else if (action.Equals("GetInforDepartment"))
            {
                GetInforDepartment(context);
            }
            else if (action.Equals("UpdateDivision")) // Division
            {
                UpdateDivision(context);
            }
            else if (action.Equals("GetInforDivision"))
            {
                GetInforDivision(context);
            }            
            else if (action.Equals("danhmuchanghoa"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=ListOfItems.xls");
                context.Response.ContentType = "application/ms-excel";
                context.Response.Charset = Encoding.UTF8.WebName;

                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = ExportToExcel_DanhMucHangHoa(context);
                response.Write(exportContent);
            }    
        }
        
        private void GetInforPosition(HttpContext context)
        {
            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                ToChuc tochuc = new ToChuc();

                string msg = tochuc.GetInfor_Position(id);

                context.Response.Write(msg);
            }
        }

        private void UpdatePosition(HttpContext context)
        {
            string id = "";
            string ma = "";
            string ten = "";
            string capbac = "0";
            string trangthai = "";

            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            if (context.Request.QueryString["ma"] != null)
                ma = context.Request.QueryString["ma"].ToString();

            if (context.Request.QueryString["ten"] != null)
                ten = context.Request.QueryString["ten"].ToString();

            if (context.Request.QueryString["capbac"] != null)
                capbac = context.Request.QueryString["capbac"].ToString();

            if (context.Request.QueryString["trangthai"] != null)
                trangthai = context.Request.QueryString["trangthai"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                ToChuc tochuc = new ToChuc();

                string msg = tochuc.Update_Position(id, ma, ten, capbac, trangthai, userId);

                context.Response.Write(msg);
            }
        }

        private void GetInforDepartment(HttpContext context)
        {
            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                ToChuc tochuc = new ToChuc();

                string msg = tochuc.GetInfor_Department(id);

                context.Response.Write(msg);
            }
        }

        private void UpdateDepartment(HttpContext context)
        {
            string id = "";
            string ma = "";
            string ten = "";
            string chinhanh_fk = "";
            string bophan_fk = "";
            string loai = "1";
            string trangthai = "";

            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            if (context.Request.QueryString["ma"] != null)
                ma = context.Request.QueryString["ma"].ToString();

            if (context.Request.QueryString["ten"] != null)
                ten = context.Request.QueryString["ten"].ToString();

            if (context.Request.QueryString["chinhanh_fk"] != null)
                chinhanh_fk = context.Request.QueryString["chinhanh_fk"].ToString();

            if (context.Request.QueryString["bophan_fk"] != null)
                bophan_fk = context.Request.QueryString["bophan_fk"].ToString();

            if (context.Request.QueryString["loai"] != null)
                loai = context.Request.QueryString["loai"].ToString();

            if (context.Request.QueryString["trangthai"] != null)
                trangthai = context.Request.QueryString["trangthai"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                ToChuc tochuc = new ToChuc();

                string msg = tochuc.Update_Department(id, ma, ten, chinhanh_fk, bophan_fk, loai, trangthai, userId);

                context.Response.Write(msg);
            }
        }

        private void GetInforDivision(HttpContext context)
        {
            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                ToChuc tochuc = new ToChuc();

                string msg = tochuc.GetInfor_Division(id);

                context.Response.Write(msg);
            }
        }

        private void UpdateDivision(HttpContext context)
        {
            string id = "";
            string ma = "";
            string ten = "";            
            string trangthai = "";

            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            if (context.Request.QueryString["ma"] != null)
                ma = context.Request.QueryString["ma"].ToString();

            if (context.Request.QueryString["ten"] != null)
                ten = context.Request.QueryString["ten"].ToString();
          
            if (context.Request.QueryString["trangthai"] != null)
                trangthai = context.Request.QueryString["trangthai"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                ToChuc tochuc = new ToChuc();

                string msg = tochuc.Update_Division(id, ma, ten, trangthai, userId);

                context.Response.Write(msg);
            }
        }
     
        private void GetInforBrand(HttpContext context)
        {
            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                ToChuc tochuc = new ToChuc();

                string msg = tochuc.GetInfor_Brand(id);

                context.Response.Write(msg);
            }
        }

        private void UpdateBrand(HttpContext context)
        {
            string id = "";
            string ma = "";
            string ten = "";
            string diachi = "";
            string trangthai = "";

            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            if (context.Request.QueryString["ma"] != null)
                ma = context.Request.QueryString["ma"].ToString();

            if (context.Request.QueryString["ten"] != null)
                ten = context.Request.QueryString["ten"].ToString();

            if (context.Request.QueryString["diachi"] != null)
                diachi = context.Request.QueryString["diachi"].ToString();

            if (context.Request.QueryString["trangthai"] != null)
                trangthai = context.Request.QueryString["trangthai"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                ToChuc tochuc = new ToChuc();

                string msg = tochuc.Update_Brand(id, ma, ten, diachi, trangthai, userId);

                context.Response.Write(msg);
            }
        }

        private void GetInforWarehouse(HttpContext context)
        {
            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                ToChuc tochuc = new ToChuc();

                string msg = tochuc.GetInfor_Warehouse(id);

                context.Response.Write(msg);
            }
        }

        private void UpdateWarehouse(HttpContext context)
        {
            string id = "";
            string ma = "";
            string ten = "";
            string diachi = "";
            string khuvuc = "";
            string trangthai = "";
            string loaikho = "";
            string thutu1 = "0";
            string thutu2 = "0";
            string thutu3 = "0";

            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            if (context.Request.QueryString["ma"] != null)
                ma = context.Request.QueryString["ma"].ToString();

            if (context.Request.QueryString["ten"] != null)
                ten = context.Request.QueryString["ten"].ToString();

            if (context.Request.QueryString["diachi"] != null)
                diachi = context.Request.QueryString["diachi"].ToString();

            if (context.Request.QueryString["khuvuc"] != null)
                khuvuc = context.Request.QueryString["khuvuc"].ToString();

            if (context.Request.QueryString["trangthai"] != null)
                trangthai = context.Request.QueryString["trangthai"].ToString();

            if (context.Request.QueryString["loaikho"] != null)
                loaikho = context.Request.QueryString["loaikho"].ToString();

            if (context.Request.QueryString["thutu1"] != null)
                thutu1 = context.Request.QueryString["thutu1"].ToString();

            if (context.Request.QueryString["thutu2"] != null)
                thutu2 = context.Request.QueryString["thutu2"].ToString();

            if (context.Request.QueryString["thutu3"] != null)
                thutu3 = context.Request.QueryString["thutu3"].ToString();

            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                ToChuc tochuc = new ToChuc();

                string msg = tochuc.Update_Warehouse(id, ma, ten, diachi, khuvuc, trangthai, loaikho, thutu1, thutu2, thutu3, userId);

                context.Response.Write(msg);
            }
        }

        private string ExportToExcel_DanhMucHangHoa(HttpContext context)
        {
            string ngaythang = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();

            string _exportContent = "";
            using (StringWriter sb = new StringWriter())
            {
                using (HtmlTextWriter htmlWriter = new HtmlTextWriter(sb))
                {
                    Table table = new Table();
                    table.GridLines = GridLines.Horizontal;

                    table.BorderWidth = new Unit(1);

                    //Tieu de
                    TableHeaderCell header = new TableHeaderCell();
                    header.RowSpan = 1;
                    header.ColumnSpan = 7;
                    header.Text = "LIST OF ITEMS";
                    header.Font.Bold = true;
                    header.BackColor = Color.Red;
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;
                    header.Font.Name = "Arial";

                    // Add the header to a new row.
                    TableRow headerRow = new TableRow();
                    headerRow.Cells.Add(header);

                    table.Rows.Add(headerRow);


                    //Tieu de
                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 7;
                    header.Text = "Date: " + ngaythang;
                    header.Font.Bold = true;
                    header.Font.Name = "Arial";
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


                    string[] tieude = new string[] { "No", "PartCode", "PartName", "Specifications", "Unit", "Unit-Change", "Exchange", "Status",
                        "Product type", "GenerateCode", "Exchange" };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;
                        header.BackColor = Color.LightGreen;
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;
                        header.Font.Name = "Arial";

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();

                    string sql = " SELECT b.ma as masp, b.ten AS tensp, CASE b.trangthai when 0 then N'Cancelled' else N'Activate' end as trangthai,  " +
                                 "  ISNULL(e.ten, '') as nganhhang, ISNULL(f.ten, '') as nhanhang, ISNULL(g.ten, '') as chungloai,   " +
                                 "  CASE b.loaisanpham_fk WHEN 0 THEN 'Unknow' " +
                                 "                        WHEN 100001 THEN 'Material'  " +
                                 "                        WHEN 100002 THEN 'Semi-finished goods'  " +
                                 "                        WHEN 100003 THEN 'Finished goods'  " +
                                 "                        WHEN 100004 THEN 'Chemical' " +
                                 "                        WHEN 100005 THEN 'Oil' " +
                                 "                        WHEN 100006 THEN 'Orther' END loaisanpham, " +
                                 "  ISNULL((SELECT ma FROM SanPham WHERE pk_seq = b.banthanhpham_fk), '') banthanhpham, b.quydoibanthanhpham, " +
                                 //"  ISNULL((SELECT ma FROM SanPham WHERE pk_seq = b.thanhpham_fk), '') thanhpham, b.quydoithanhpham, " +
                                 "  ISNULL(c.ten, 'NA') as donvi, ISNULL(b.TRONGLUONG, 0) as TRONGLUONG, ISNULL(b.THETICH, 0) as THETICH, ISNULL(q.soluong1, 0) as quycach " +
                                 " FROM SanPham b LEFT JOIN DonViTinh c on b.dvt_fk = c.pk_seq  " +                                 
                                 " 	LEFT JOIN NganhHang e on b.nganhhang_fk = e.pk_seq  " +
                                 " 	LEFT JOIN NhanHang f on b.nhanhang_fk = f.pk_seq  " +
                                 " 	LEFT JOIN ChungLoai g on b.chungloai_fk = g.pk_seq  " +                                
                                 " 	LEFT JOIN QuyCach q on b.pk_seq = q.sanpham_fk " +
                                 " WHERE b.trangthai = '1' " +
                                 " ORDER BY b.ma ASC ";

                    DataTable dt = xl.ReadTable(sql);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string masp = dt.Rows[i]["masp"].ToString();                       
                        string tensp = dt.Rows[i]["tensp"].ToString();
                        string trangthai = dt.Rows[i]["trangthai"].ToString();                        
                        string nganhhang = dt.Rows[i]["nganhhang"].ToString();
                        string nhanhang = dt.Rows[i]["nhanhang"].ToString();
                        string chungloai = dt.Rows[i]["chungloai"].ToString();
                        string donvi = dt.Rows[i]["donvi"].ToString();
                        string trongluong = dt.Rows[i]["trongluong"].ToString();
                        string thetich = dt.Rows[i]["thetich"].ToString();
                        string loaisanpham = dt.Rows[i]["loaisanpham"].ToString();
                        string banthanhpham = dt.Rows[i]["banthanhpham"].ToString();
                        string quydoibanthanhpham = dt.Rows[i]["quydoibanthanhpham"].ToString();
                        //string thanhpham = dt.Rows[i]["thanhpham"].ToString();
                        //string quydoithanhpham = dt.Rows[i]["quydoithanhpham"].ToString();
                        string quycach = dt.Rows[i]["quycach"].ToString();

                        string[] data = new string[] { (i + 1).ToString(), masp, tensp, chungloai, donvi, "", quycach, trangthai,
                           loaisanpham, banthanhpham, quydoibanthanhpham};

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();

                            if (j == 0)
                                cell.Width = Unit.Parse("70");
                            else if (j == 1 || j == 3 || j == 8 || j == 9)
                                cell.Width = Unit.Parse("150");
                            else if (j == 2)
                                cell.Width = Unit.Parse("450");                            
                            else
                                cell.Width = Unit.Parse("100");

                            if (j == 0 || j == 4 || j == 7)
                                cell.HorizontalAlign = HorizontalAlign.Center;

                            if (j == 6 || j == 10)
                                cell.HorizontalAlign = HorizontalAlign.Right;

                            cell.Text = data[j];

                            header.Font.Name = "Arial";

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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}