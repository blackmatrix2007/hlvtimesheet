using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Drawing;
using HLVTimeSheet.Model;
using Aspose.Cells;
using HLVTimeSheet.AcsessData;
using System.Text;

namespace HLVTimeSheet.Admin.Hander
{
    /// <summary>
    /// Summary description for hdBaoCaoBH
    /// </summary>
    public class hdBaoCaoBH : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {            
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;

            string action = "";
            if (context.Request.QueryString["action"] != null)
                action = context.Request.QueryString["action"].ToString();

            if (action.Equals("doanhsoExel"))
            {
                string solieu = "";
                if (context.Request.QueryString["solieu"] != null)
                    solieu = context.Request.QueryString["solieu"].ToString();

                if (solieu.Equals("1"))
                {
                    ExportToExcel_StockOutTotal(context);
                }
                else if (solieu.Equals("4"))
                {
                    ExportToExcel_StockOutDetail(context);
                }
            }            
            else if (action.Equals("trahangExel"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=BaoCaoTraHang.xls");
                context.Response.ContentType = "application/ms-excel";

                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = ExportToExcel_TRAHANG(context);
                response.Write(exportContent);

            }
            else if (action.Equals("donhang_excel"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=TongHopDonHangXuat.xls");
                context.Response.ContentType = "application/ms-excel";

                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = ExportToExcel_TONGHOPDONHANG(context);
                response.Write(exportContent);
            }
            else if (action.Equals("theodoi_donhang_excel"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=TheoDoiDonHangXuat.xls");
                context.Response.ContentType = "application/ms-excel";

                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = ExportToExcel_THEODOIDONHANG(context);
                response.Write(exportContent);
            }
            else if (action.Equals("LaiLoTheoDonHang"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=BaoCaoLaiLoTheDonHang.xls");
                context.Response.ContentType = "application/ms-excel";

                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = ExportToExcel_LAILOTHEODONHANG(context);
                response.Write(exportContent);
            }
            else if (action.Equals("dlnHANGHOA"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=DanhSachHangHoa.xls");
                context.Response.ContentType = "application/ms-excel";
                context.Response.Charset = Encoding.UTF8.WebName;

                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = ExportToExcel_DLNHANGHOA(context);
                response.Write(exportContent);
            }
            else if (action.Equals("tonghopgiaohang"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=TongHopGiaoHang.xls");
                context.Response.ContentType = "application/ms-excel";

                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = ExportToExcel_TongHopGiaoHang(context);
                response.Write(exportContent);
            }
            else if (action.Equals("chitietgiaohang"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=ChiTietGiaoHang.xls");
                context.Response.ContentType = "application/ms-excel";

                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = ExportToExcel_ChiTietGiaoHang(context);
                response.Write(exportContent);
            }
            else if (action.Equals("dathangExcel"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=Order.xls");
                context.Response.ContentType = "application/ms-excel";
                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = DatHang_ID_EXCEL(context);
                response.Write(exportContent);
            }
            else if (action.Equals("xuathangExcel"))
            {
                StockOut_ID_EXCEL(context);
            }
            else if (action.Equals("dieuchuyenExcel"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=DieuChuyen.xls");
                context.Response.ContentType = "application/ms-excel";
                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = DieuChuyen_ID_EXCEL(context);
                response.Write(exportContent);
            }
            else if (action.Equals("xuatkhacExcel"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=StockOut.xls");
                context.Response.ContentType = "application/ms-excel";
                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = XuatKhac_ID_EXCEL(context);
                response.Write(exportContent);
            }
            else if (action.Equals("fileTemplateStockOut"))
            {
                string exportContent = ExportToExcel_FileTemplateStockOut(context);
            }
            else if (action.Equals("reportPickUp"))
            {
                string exportContent = ExportToExcel_PickUp(context);
            }
            else if (action.Equals("pickUpExcelTotal"))
            {
                string exportContent = ExportToExcel_PickUpTotal(context);
            }
            else if (action.Equals("pickUpExcelDetail"))
            {
                //string exportContent = ExportToExcel_PackingList(context);

                context.Response.AddHeader("content-disposition", "attachment; filename=Report_PackingList.xls");
                context.Response.ContentType = "application/ms-excel";
                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = ExportToExcel_PickUpDetail_New(context);
                response.Write(exportContent);
            }
        }

        private string ExportToExcel_StockOutTotal(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();

           
            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();

            string _exportContent = "";
            ExecuteData xl = new ExecuteData();

            string condition = " ";
            if (tungay.Trim().Length > 0)
                condition += " and convert( datetime, dh.ngaygiaohang, 105) >= convert( datetime, '" + tungay + "', 105) ";
            if (denngay.Trim().Length > 0)
                condition += " and convert( datetime, dh.ngaygiaohang, 105) <= convert( datetime, '" + denngay + "', 105) ";
            
            if (khachhang.Trim().Length > 3)
                condition += " and dh_sp.khachhang_fk = '" + khachhang + "' ";
            if (chungloai.Trim().Length > 3)
                condition += " and sp.chungloai_fk = '" + chungloai + "' ";
            

            DataTable baocao = new DataTable("ReportStockOut_Total");

            baocao.Columns.Add("DeliveryDate", typeof(string));
            baocao.Columns.Add("ID", typeof(string));
            baocao.Columns.Add("Customer", typeof(string));
            baocao.Columns.Add("Grade", typeof(string));
            baocao.Columns.Add("Model", typeof(string));
            baocao.Columns.Add("Quantity", typeof(string));


            string sql = " SELECT dh.ngaygiaohang, dh.pk_seq AS donhang_fk, kh.ma makh, kh.hoten AS tenkh, sp.ma AS masp, sp.ten AS tensp, " +
                    " 	ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') chungloai, SUM(dh_sp.soluong) soluong" +
                    " FROM DonHang dh INNER JOIN DonHang_SanPham_ChiTiet dh_sp ON dh.pk_seq = dh_sp.donhang_fk " +
                    " 	INNER JOIN KhachHang kh ON dh_sp.khachhang_fk = kh.pk_seq " +
                    " 	INNER JOIN SanPham sp ON dh_sp.sanpham_fk = sp.pk_seq " + condition +
                    " WHERE dh.trangthai IN (1) " +
                    " GROUP BY dh.ngaygiaohang, dh.pk_seq, kh.ma, kh.hoten, sp.ma, sp.ten, sp.chungloai_fk " +
                    " ORDER BY CONVERT(datetime, dh.ngaygiaohang, 105), kh.hoten, sp.ten ";

            DataTable dt = xl.ReadTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string ngaygiaohang = dt.Rows[i]["ngaygiaohang"].ToString();
                string donhang_fk = dt.Rows[i]["donhang_fk"].ToString();

                string tenkh = dt.Rows[i]["tenkh"].ToString();
                string tensp = dt.Rows[i]["tensp"].ToString();
                chungloai = dt.Rows[i]["chungloai"].ToString();
               
                double soluong = double.Parse(dt.Rows[i]["soluong"].ToString());
                
                DataRow dr = baocao.NewRow();
             
                dr[0] = ngaygiaohang;
                dr[1] = donhang_fk;
                dr[2] = tenkh;
                dr[3] = tensp;
                dr[4] = chungloai;
                dr[5] = FormatString.ForMatNumber(soluong.ToString());
               
                baocao.Rows.Add(dr);
            }

            string fileName = "";

           fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\ReportStockOut_Total.xlsm";

            Workbook workbook = new Workbook(fileName);
            Worksheet worksheet = workbook.Worksheets[0];


            worksheet.Cells.ImportDataTable(baocao, true, "A5");
            worksheet.AutoFitColumns();

            worksheet.Cells["A1"].PutValue("REPORT STOCK OUT TOTAL");

            //worksheet.Cells["A3"].PutValue("Thời gian tạo ");
            //worksheet.Cells["B3"].PutValue(tungay + " đến " + denngay);

            worksheet.Cells["A2"].PutValue("Created date");
            worksheet.Cells["B2"].PutValue(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));

            worksheet.Cells["A3"].PutValue("Username");
            worksheet.Cells["B3"].PutValue(context.Session["userName"].ToString());

            HttpResponse response = context.Response;
            workbook.Save(response, "ReportStockOut_Total.xlsm", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsm));

            return _exportContent;
        }

        private string ExportToExcel_StockOutDetail(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();


            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();

            string _exportContent = "";
            ExecuteData xl = new ExecuteData();

            string condition = " ";
            if (tungay.Trim().Length > 0)
                condition += " and convert( datetime, dh.ngaygiaohang, 105) >= convert( datetime, '" + tungay + "', 105) ";
            if (denngay.Trim().Length > 0)
                condition += " and convert( datetime, dh.ngaygiaohang, 105) <= convert( datetime, '" + denngay + "', 105) ";

            if (khachhang.Trim().Length > 3)
                condition += " and dh_sp.khachhang_fk = '" + khachhang + "' ";
            if (chungloai.Trim().Length > 3)
                condition += " and sp.chungloai_fk = '" + chungloai + "' ";


            DataTable baocao = new DataTable("ReportStockOut_Detail");

            baocao.Columns.Add("DeliveryDate", typeof(string));
            baocao.Columns.Add("ID", typeof(string));
            baocao.Columns.Add("Customer", typeof(string));
            baocao.Columns.Add("Grade", typeof(string));
            baocao.Columns.Add("Model", typeof(string));

            baocao.Columns.Add("Vin", typeof(string));
            baocao.Columns.Add("Frame", typeof(string));
            baocao.Columns.Add("Engine", typeof(string));
            baocao.Columns.Add("Color", typeof(string));
            baocao.Columns.Add("Location", typeof(string));
            
            string sql = " SELECT dh.ngaygiaohang, dh.pk_seq AS donhang_fk, kh.ma makh, kh.hoten AS tenkh, sp.ma AS masp, sp.ten AS tensp, " +
                    " 	ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') chungloai," +
                    " 	ISNULL((SELECT ten FROM Size WHERE pk_seq = dh_sp.mausac_fk), '') mausac," +
                    " 	ISNULL((SELECT ten FROM Location WHERE pk_seq = dh_sp.location_fk), '') location, " +
                    " 	dh_sp.sokhung, dh_sp.dongco, dh_sp.mavach " +
                    " FROM DonHang dh INNER JOIN DonHang_SanPham_ChiTiet dh_sp ON dh.pk_seq = dh_sp.donhang_fk " +
                    " 	INNER JOIN KhachHang kh ON dh_sp.khachhang_fk = kh.pk_seq " +
                    " 	INNER JOIN SanPham sp ON dh_sp.sanpham_fk = sp.pk_seq " + condition +
                    " WHERE dh.trangthai IN (1) " +
                    " ORDER BY CONVERT(datetime, dh.ngaygiaohang, 105), kh.hoten, sp.ten, dh_sp.mavach ";

            DataTable dt = xl.ReadTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string ngaygiaohang = dt.Rows[i]["ngaygiaohang"].ToString();
                string donhang_fk = dt.Rows[i]["donhang_fk"].ToString();
                chungloai = dt.Rows[i]["chungloai"].ToString();
                string tenkh = dt.Rows[i]["tenkh"].ToString();
                string tensp = dt.Rows[i]["tensp"].ToString();

                string mavach = dt.Rows[i]["mavach"].ToString();
                string sokhung = dt.Rows[i]["sokhung"].ToString();
                string dongco = dt.Rows[i]["dongco"].ToString();
                string mausac = dt.Rows[i]["mausac"].ToString();
                string location = dt.Rows[i]["location"].ToString();

                DataRow dr = baocao.NewRow();

                dr[0] = ngaygiaohang;
                dr[1] = donhang_fk;
                dr[2] = tenkh;
                dr[3] = tensp;
                dr[4] = chungloai;

                dr[5] = mavach;
                dr[6] = sokhung;
                dr[7] = dongco;
                dr[8] = mausac;
                dr[9] = location;

                baocao.Rows.Add(dr);
            }

            string fileName = "";

            fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\ReportStockOut_Detail.xlsm";

            Workbook workbook = new Workbook(fileName);
            Worksheet worksheet = workbook.Worksheets[0];


            worksheet.Cells.ImportDataTable(baocao, true, "A5");
            worksheet.AutoFitColumns();

            worksheet.Cells["A1"].PutValue("REPORT STOCK OUT DETAIL");

            //worksheet.Cells["A3"].PutValue("Thời gian tạo ");
            //worksheet.Cells["B3"].PutValue(tungay + " đến " + denngay);

            worksheet.Cells["A2"].PutValue("Created date");
            worksheet.Cells["C2"].PutValue(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));

            worksheet.Cells["A3"].PutValue("Username");
            worksheet.Cells["C3"].PutValue(context.Session["userName"].ToString());

            HttpResponse response = context.Response;
            workbook.Save(response, "ReportStockOut_Detail.xlsm", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsm));

            return _exportContent;
        }

        private string ExportToExcel_PickUp(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();

            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();

            string _exportContent = "";
            ExecuteData xl = new ExecuteData();

            string condition = " ";
            if (tungay.Trim().Length > 0)
                condition += " and convert( datetime, dh.ngaygiaohang, 105) >= convert( datetime, '" + tungay + "', 105) ";
            if (denngay.Trim().Length > 0)
                condition += " and convert( datetime, dh.ngaygiaohang, 105) <= convert( datetime, '" + denngay + "', 105) ";

            if (khachhang.Trim().Length > 3)
                condition += " and dh.khachhang_fk = '" + khachhang + "' ";
            if (chungloai.Trim().Length > 3)
                condition += " and sp.chungloai_fk = '" + chungloai + "' ";


            DataTable baocao = new DataTable("Report_PickUp");

            baocao.Columns.Add("Date", typeof(string));
            baocao.Columns.Add("ID", typeof(string));
            baocao.Columns.Add("Customer", typeof(string));
            baocao.Columns.Add("InvoiceNo", typeof(string));

            baocao.Columns.Add("ItemJP", typeof(string));
            baocao.Columns.Add("ItemVN", typeof(string));

            baocao.Columns.Add("PLNo", typeof(string));
            baocao.Columns.Add("LotNo", typeof(string));
            baocao.Columns.Add("CartonNo", typeof(string));

            baocao.Columns.Add("Color", typeof(string));
            baocao.Columns.Add("Location", typeof(string));
            baocao.Columns.Add("Pallet", typeof(string));
            baocao.Columns.Add("Qty", typeof(string));

            string sql = " SELECT dh.ngaygiaohang, dh.pk_seq AS donhang_fk, kh.ma makh, kh.ma AS tenkh, sp.ma AS masp, sp.codeEnglish, sp.ten AS tensp, " +
                    " 	ISNULL((SELECT ten FROM Size WHERE pk_seq = dh_sp.mausac_fk), '') mausac," +
                    " 	ISNULL((SELECT ten FROM Location WHERE pk_seq = dh_sp.location_fk), '') location, " +
                    " 	ISNULL((SELECT ten FROM Pallet WHERE pk_seq = dh_sp.pallet_fk), '') pallet," +
                    " 	dh_sp.solo, dh_sp.plNo, dh_sp.lotNo, dh_sp.cartonNo, dh_sp.soluong, dh_sp.mavach " +
                    " FROM DonHang dh INNER JOIN DonHang_SanPham_ChiTiet dh_sp ON dh.pk_seq = dh_sp.donhang_fk " +
                    " 	INNER JOIN KhachHang kh ON dh_sp.khachhang_fk = kh.pk_seq " +
                    " 	INNER JOIN SanPham sp ON dh_sp.sanpham_fk = sp.pk_seq " + condition +
                    " WHERE dh.trangthai IN (0, 1) AND dh_sp.scanByPDA = 0 " +
                    " ORDER BY CONVERT(datetime, dh.ngaygiaohang, 105), kh.hoten, dh_sp.location_fk, dh_sp.pallet_fk, sp.ten, dh_sp.solo ";

            DataTable dt = xl.ReadTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string ngaygiaohang = dt.Rows[i]["ngaygiaohang"].ToString();
                string donhang_fk = dt.Rows[i]["donhang_fk"].ToString();
                string tenkh = dt.Rows[i]["tenkh"].ToString();
                string solo = dt.Rows[i]["solo"].ToString();

                string codeEnglish = dt.Rows[i]["codeEnglish"].ToString();
                string masp = dt.Rows[i]["masp"].ToString();
                
                string plNo = dt.Rows[i]["plNo"].ToString();
                string lotNo = dt.Rows[i]["lotNo"].ToString();
                string cartonNo = dt.Rows[i]["cartonNo"].ToString();
                string mausac = dt.Rows[i]["mausac"].ToString();
                string location = dt.Rows[i]["location"].ToString();
                string pallet = dt.Rows[i]["pallet"].ToString();
                string soluong = dt.Rows[i]["soluong"].ToString();

                DataRow dr = baocao.NewRow();

                dr[0] = ngaygiaohang;
                dr[1] = donhang_fk;
                dr[2] = tenkh;
                dr[3] = solo;

                dr[4] = codeEnglish;
                dr[5] = masp;

                dr[6] = plNo;
                dr[7] = lotNo;
                dr[8] = cartonNo;

                dr[9] = mausac;
                dr[10] = location;
                dr[11] = pallet;                
                dr[12] = FormatString.ForMatNumber(soluong);

                baocao.Rows.Add(dr);
            }

            string fileName = "";

            fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\Report_PickUp.xlsm";

            Workbook workbook = new Workbook(fileName);
            Worksheet worksheet = workbook.Worksheets[0];


            worksheet.Cells.ImportDataTable(baocao, true, "A5");
            worksheet.AutoFitColumns();

            worksheet.Cells["A1"].PutValue("REPORT PICK UP");

            worksheet.Cells["A2"].PutValue("Created date");
            worksheet.Cells["C2"].PutValue(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));

            worksheet.Cells["A3"].PutValue("Username");
            worksheet.Cells["C3"].PutValue(context.Session["userName"].ToString());

            HttpResponse response = context.Response;
            workbook.Save(response, "Report_PickUp.xlsm", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsm));

            return _exportContent;
        }

        private string ExportToExcel_PickUpTotal(HttpContext context)
        {
            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();

            string _exportContent = "";
            ExecuteData xl = new ExecuteData();

            string condition = " ";
           
            if (id.Trim().Length > 3)
                condition += " and dh.pk_seq = '" + id + "' ";


            DataTable baocao = new DataTable("Report_PickUpTotal");

            baocao.Columns.Add("S/MARKS", typeof(string));
            baocao.Columns.Add(" COMMODITY", typeof(string));
            baocao.Columns.Add("COLOR", typeof(string));
            baocao.Columns.Add("QUANTITY (M)", typeof(string));
            baocao.Columns.Add("UNIT PRICE (YDS)", typeof(string));
            baocao.Columns.Add("AMOUNT", typeof(string));

            string sql = "SELECT * FROM ( " +
                    " SELECT dh_sp.plNo, sp.ten AS tensp, sp.ma AS itemVN, sp.codeEnglish AS itemJP, " +
                    " ISNULL((SELECT ten FROM Size WHERE pk_seq = dh_sp.mausac_fk), '') color, SUM(dh_sp.soluongQUYDOI) AS soluong " +
                    " FROM DonHang dh INNER JOIN DonHang_SanPham_ChiTiet dh_sp ON dh.pk_seq = dh_sp.donhang_fk " +
                    " 	INNER JOIN SanPham sp ON dh_sp.sanpham_fk = sp.pk_seq " + condition +
                    " WHERE dh.trangthai IN (0, 1) " +
                    " GROUP BY dh_sp.plNo, sp.ten, sp.ma, sp.codeEnglish, dh_sp.mausac_fk  " +
                    " ) A " +
                    " ORDER BY A.itemJP, A.color, A.plNo ";

            DataTable dt = xl.ReadTable(sql);

            double totalQty = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string plNo = dt.Rows[i]["plNo"].ToString();
                string tensp = dt.Rows[i]["itemJP"].ToString();
                string color = dt.Rows[i]["color"].ToString();
                string soluong = dt.Rows[i]["soluong"].ToString();

                DataRow dr = baocao.NewRow();


                if (i > 0 && i < dt.Rows.Count)
                {
                    if (plNo.Equals(dt.Rows[i - 1]["plNo"].ToString()))
                    {
                        plNo = "";
                    }
                }

                if (i > 0 && i < dt.Rows.Count)
                {
                    if (tensp.Equals(dt.Rows[i - 1]["itemJP"].ToString()))
                    {
                        tensp = "";
                    }
                }

                // Kiểm tra để add total nếu khác mã item.
                if ((i > 0 && tensp.Length > 0))
                {
                    DataRow drT = baocao.NewRow();
                    drT[0] = "";
                    drT[1] = "";
                    drT[2] = "Total";
                    drT[3] = FormatString.ForMatNumber(totalQty.ToString());
                    drT[4] = "";
                    drT[5] = "";
                    totalQty = 0;

                    baocao.Rows.Add(drT);
                    
                }
                
                dr[0] = plNo;
                dr[1] = tensp;
                dr[2] = color;
                dr[3] = FormatString.ForMatNumber(soluong);
                dr[4] = "";
                dr[5] = "";

                totalQty += double.Parse(soluong);
                baocao.Rows.Add(dr);

                if (i == dt.Rows.Count - 1)
                {
                    DataRow drT = baocao.NewRow();
                    drT[0] = "";
                    drT[1] = "";
                    drT[2] = "Total";
                    drT[3] = FormatString.ForMatNumber(totalQty.ToString());
                    drT[4] = "";
                    drT[5] = "";
                    totalQty = 0;
                    baocao.Rows.Add(drT);
                }

            }

            string fileName = "";

            fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\Report_PickUpTotal.xls";

            Workbook workbook = new Workbook(fileName);
            Worksheet worksheet = workbook.Worksheets[0];

            worksheet.Cells.ImportDataTable(baocao, false, "A5");
            //worksheet.AutoFitColumns();

            //worksheet.Cells["A2"].PutValue("Created date");
            //worksheet.Cells["B2"].PutValue(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));

            //worksheet.Cells["A3"].PutValue("Username");
            //worksheet.Cells["B3"].PutValue(context.Session["userName"].ToString());

            HttpResponse response = context.Response;
            workbook.Save(response, "Report_PickUp" + id + ".xls", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsm));

            return _exportContent;
        }

        private string ExportToExcel_PickUpDetail(HttpContext context)
        {
            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();

            string _exportContent = "";
            ExecuteData xl = new ExecuteData();

            string condition = " ";

            if (id.Trim().Length > 3)
                condition += " and dh.pk_seq = '" + id + "' ";


            DataTable baocao = new DataTable("Report_PickUpDetail");

            baocao.Columns.Add("1", typeof(string));
            baocao.Columns.Add("2", typeof(string));
            baocao.Columns.Add("3", typeof(string));
            baocao.Columns.Add("4", typeof(string));
            baocao.Columns.Add("5", typeof(string));
            baocao.Columns.Add("6", typeof(string));
            baocao.Columns.Add("7", typeof(string));
            baocao.Columns.Add("8", typeof(string));
            baocao.Columns.Add("9", typeof(string));

            string sql = "SELECT * FROM ( " +
                    " SELECT dh_sp.plNo, dh_sp.sewingNo, sp.ten AS tensp, sp.ma AS itemVN, sp.codeEnglish AS itemJP, " +
                    " ISNULL((SELECT ten FROM Size WHERE pk_seq = dh_sp.mausac_fk), '') color, SUM(dh_sp.soluongQUYDOI) AS soluong " +
                    " FROM DonHang dh INNER JOIN DonHang_SanPham_ChiTiet dh_sp ON dh.pk_seq = dh_sp.donhang_fk " +
                    " 	INNER JOIN SanPham sp ON dh_sp.sanpham_fk = sp.pk_seq " + condition +
                    " WHERE dh.trangthai IN (0, 1) " +
                    " GROUP BY dh_sp.plNo, dh_sp.sewingNo, sp.ten, sp.ma, sp.codeEnglish, dh_sp.mausac_fk  " +
                    " ) A " +
                    " ORDER BY A.itemJP, A.color, A.plNo ";

            DataTable dt = xl.ReadTable(sql);

            double totalQty = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string plNo = dt.Rows[i]["plNo"].ToString();
                string sewingNo = dt.Rows[i]["sewingNo"].ToString();
                string color = dt.Rows[i]["color"].ToString();

                string soluong1 = "";
                string soluong2 = "";
                string soluong3 = "";

                // Detail
                sql = " SELECT * " +
                 " FROM  " +
                 " (  " +
                 "    SELECT lotNo, cartonNo, NoOf, roll, sewingNo, soluongQUYDOI AS soluong, soluong1, soluong2, soluong3, netWeight, grossWeight,  " +
                 "    ISNULL((SELECT ten FROM Size WHERE pk_seq = dh.mausac_fk), '') color  " +
                 "    FROM DonHang_SanPham_ChiTiet dh  " +
                 "    WHERE DonHang_SanPham_ChiTiet WHERE dh.donhang_fk = '" + id + "' AND dh.plNo = N'" + plNo + "'  " +
                 ") A " + 
                 " ORDER BY A.color, A.lotNo, A.cartonNo ";
                DataTable dtSP = xl.ReadTable(sql);
                for (int j = 0; j < dtSP.Rows.Count; j++)
                {
                    DataRow dr = baocao.NewRow();


                    dr[0] = plNo;
                    dr[1] = "";
                    dr[2] = color;
                    dr[3] = FormatString.ForMatNumber("");
                    dr[4] = "";
                    dr[5] = "";

                    totalQty += double.Parse(soluong1);
                    baocao.Rows.Add(dr);

                    if (i == dt.Rows.Count - 1)
                    {
                        DataRow drT = baocao.NewRow();
                        drT[0] = "";
                        drT[1] = "";
                        drT[2] = "Total";
                        drT[3] = FormatString.ForMatNumber(totalQty.ToString());
                        drT[4] = "";
                        drT[5] = "";
                        totalQty = 0;
                        baocao.Rows.Add(drT);
                    }
                }                
            }

            string fileName = "";

            fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\Report_PickUpDetail.xlsx";

            Workbook workbook = new Workbook(fileName);
            Worksheet worksheet = workbook.Worksheets[0];

            worksheet.Cells.ImportDataTable(baocao, false, "A5");
            //worksheet.AutoFitColumns();

            //worksheet.Cells["A2"].PutValue("Created date");
            //worksheet.Cells["B2"].PutValue(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));

            //worksheet.Cells["A3"].PutValue("Username");
            //worksheet.Cells["B3"].PutValue(context.Session["userName"].ToString());

            HttpResponse response = context.Response;
            workbook.Save(response, "Report_PickUpDetail" + id + ".xlsx", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsm));

            return _exportContent;
        }

        private string ExportToExcel_PickUpDetail_New(HttpContext context)
        {
            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string ngaythang = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();

            string _exportContent = "";
            using (StringWriter sb = new StringWriter())
            {
                using (HtmlTextWriter htmlWriter = new HtmlTextWriter(sb))
                {
                    System.Web.UI.WebControls.Table table = new System.Web.UI.WebControls.Table();
                    
                    //Tieu de
                    TableHeaderCell header = new TableHeaderCell();
                    header.RowSpan = 1;
                    header.ColumnSpan = 11;
                    header.Text = "Honda Logicom Viet Nam Co., ltd";
                    header.Font.Bold = true;
                    header.Font.Size = FontUnit.Point(11);
                    header.Font.Name = "Arial";
                    //header.BackColor = System.Drawing.Color.Red;
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    // Add the header to a new row.
                    TableRow headerRow = new TableRow();
                    headerRow.Cells.Add(header);

                    table.Rows.Add(headerRow);


                    //Tieu de
                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 11;
                    header.Text = "Hoai Nam Building, No. 6, Hai Ba Trung Street, Hung Vuong Ward, Phuc Yen City, Vinh Phuc Province";
                    header.Font.Bold = false;
                    header.Font.Size = FontUnit.Point(10);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);


                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.RowSpan = 1;
                    header.ColumnSpan = 11;
                    header.Text = "PACKING LIST";
                    header.Font.Bold = true;
                    header.Font.Size = FontUnit.Point(15);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();
                    string query = "SELECT pk_seq, sophieu, ngaydonhang, ngaygiaohang FROM DonHang WHERE pk_seq = '" + id + "' ";
                    DataTable dtINFO = xl.ReadTable(query);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 11;
                    header.Text = " No: " + id + "  - Date: " + dtINFO.Rows[0]["ngaydonhang"].ToString();
                    header.Font.Italic = true;
                    header.Font.Bold = false;
                    header.Font.Size = FontUnit.Point(10);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 11;
                    header.Text = " Contract No: " + dtINFO.Rows[0]["sophieu"].ToString();
                    header.Font.Italic = true;
                    header.Font.Bold = false;
                    header.Font.Size = FontUnit.Point(10);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    //Chen 2 row khoang cach
                    for (int i = 0; i < 1; i++)
                    {
                        TableRow rowS = new TableRow();
                        table.Rows.Add(rowS);
                    }

                    string[] tieude = new string[] { "Carton No", "No of Carton", "Lot No", "Roll", "Qty (M)", "NetWeight", "GrossWeight", "Location" };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;
                        header.BackColor = Color.LightGray;
                        header.BorderWidth = new Unit(0.5, UnitType.Pixel);
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;
                        header.Font.Size = FontUnit.Point(10);
                        header.Font.Name = "Arial";

                       if (i == 2)
                            header.Width = Unit.Parse("120");
                        else if (i == 3)
                            header.Width = Unit.Parse("300");
                        else
                            header.Width = Unit.Parse("100");

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);
                    
                    double totalNoOf_All = 0;
                    double totalRoll_All = 0;
                    double totalQty_All = 0;
                    double totalSV_All = 0;
                    double totalNet_All = 0;
                    double totalGross_All = 0;

                    string condition = "";
                    if (id.Trim().Length > 3)
                        condition += " and dh.pk_seq = '" + id + "' ";

                    query = "SELECT * FROM ( " +
                   " SELECT dh_sp.sanpham_fk, dh_sp.plNo, dh_sp.sewingNo, sp.ten AS tensp, sp.ma AS itemVN, sp.codeEnglish AS itemJP, dh_sp.mausac_fk, " + 
                   " ISNULL((SELECT ten FROM Size WHERE pk_seq = dh_sp.mausac_fk), '') color, SUM(dh_sp.soluongQUYDOI) AS soluong " +
                   " FROM DonHang dh INNER JOIN DonHang_SanPham_ChiTiet dh_sp ON dh.pk_seq = dh_sp.donhang_fk " +
                   " 	INNER JOIN SanPham sp ON dh_sp.sanpham_fk = sp.pk_seq " + condition +
                   " WHERE dh.trangthai IN (0, 1) " +
                   " GROUP BY dh_sp.sanpham_fk, dh_sp.plNo, dh_sp.sewingNo, sp.ten, sp.ma, sp.codeEnglish, dh_sp.mausac_fk " +
                   " ) A " +
                   " ORDER BY A.itemJP, A.color, A.plNo, A.sewingNo ";

                    DataTable dt = xl.ReadTable(query);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string sanpham_fk = dt.Rows[i]["sanpham_fk"].ToString();
                        string plNo = dt.Rows[i]["plNo"].ToString();
                        string sewingNo = dt.Rows[i]["sewingNo"].ToString();
                        string color = dt.Rows[i]["color"].ToString();
                        string mausac_fk = dt.Rows[i]["mausac_fk"].ToString();
                        string itemVN = dt.Rows[i]["itemVN"].ToString();
                        string itemJP = dt.Rows[i]["itemJP"].ToString();

                        double totalNoOf_PL = 0;
                        double totalRoll_PL = 0;
                        double totalQty_PL = 0;
                        double totalSV_PL = 0;
                        double totalNet_PL = 0;
                        double totalGross_PL = 0;
                        // Detail -- 1 là số, 2 là chữ

                        query = " SELECT * " +
                         " FROM  " +
                         " (  " +
                         "    SELECT 1 AS stt, lotNo, cartonNo, sortBy, NoOf, roll, sewingNo, soluongQUYDOI AS soluong, soluong1, soluong2, soluong3, netWeight, grossWeight,  " +
                         "    ISNULL((SELECT ten FROM Location WHERE pk_seq = dh.location_fk), '') location, " +
                         "    ISNULL((SELECT ten FROM Size WHERE pk_seq = dh.mausac_fk), '') color  " +
                         "    FROM DonHang_SanPham_ChiTiet dh  " +
                         "    WHERE dh.donhang_fk = '" + id + "' AND dh.sanpham_fk = '" + sanpham_fk + "' AND dh.plNo = N'" + plNo + "' AND dh.mausac_fk = '" + mausac_fk + "' AND dh.sewingNo = N'" + sewingNo + "' " +
                         ") A " +
                         " ORDER BY A.stt, A.color, A.lotNo, A.sortBy ";
                        DataTable dtSP = xl.ReadTable(query);

                        if (dtSP.Rows.Count > 0)
                        {
                            for (int t = 0; t < dtSP.Rows.Count; t++)
                            {                                
                                string cartonNo = dtSP.Rows[t]["cartonNo"].ToString();
                                string lotNo = dtSP.Rows[t]["lotNo"].ToString();
                                string NoOf = dtSP.Rows[t]["NoOf"].ToString();
                                color = dtSP.Rows[t]["color"].ToString();
                                string roll = dtSP.Rows[t]["roll"].ToString();
                                string location = dtSP.Rows[t]["location"].ToString();
                                
                                string soluong = FormatString.ForMatNumber(dtSP.Rows[t]["soluong"].ToString());                                
                                string netWeight = FormatString.ForMatNumber(dtSP.Rows[t]["netWeight"].ToString());
                                string grossWeight = FormatString.ForMatNumber(dtSP.Rows[t]["grossWeight"].ToString());

                                totalNoOf_PL += 0;
                                totalQty_PL += double.Parse(dtSP.Rows[t]["soluong"].ToString());
                                totalRoll_PL += double.Parse(dtSP.Rows[t]["roll"].ToString());
                                
                                totalNet_PL += double.Parse(dtSP.Rows[t]["netWeight"].ToString());
                                totalGross_PL += double.Parse(dtSP.Rows[t]["grossWeight"].ToString());

                                totalNoOf_All += 0;
                                totalQty_All += double.Parse(dtSP.Rows[t]["soluong"].ToString());
                                totalRoll_All += double.Parse(dtSP.Rows[t]["roll"].ToString());
                                
                                totalNet_All += double.Parse(dtSP.Rows[t]["netWeight"].ToString());
                                totalGross_All += double.Parse(dtSP.Rows[t]["grossWeight"].ToString());

                                if (t > 0 && t < dtSP.Rows.Count && lotNo.Equals(dtSP.Rows[t - 1]["lotNo"].ToString()))
                                    lotNo = "";
                                else
                                    lotNo = "'" + lotNo;

                                // Hiển thị dòng đầu tiên
                                if (t == 0)
                                {
                                    TableRow row = new TableRow();
                                    string[] data1 = new string[] { plNo, " (" +  itemVN + ") " + color + " (SEWING C/NO: " + sewingNo + ")" };

                                    for (int j = 0; j < data1.Length; j++)
                                    {
                                        TableCell cell = new TableCell();

                                        if (j == 1)
                                            cell.ColumnSpan = 7;
                                        
                                        cell.Font.Bold = true;
                                        cell.Text = data1[j];
                                        cell.Font.Size = FontUnit.Point(10);
                                        cell.Font.Name = "Arial";

                                        row.Cells.Add(cell);
                                    }

                                    table.Rows.Add(row);
                                }

                                //if (t > 0)
                                {
                                    TableRow row = new TableRow();
                                    string[] data2 = new string[] { cartonNo, NoOf, lotNo, roll, soluong, netWeight, grossWeight, location };
                                    for (int j = 0; j < data2.Length; j++)
                                    {
                                        TableCell cell = new TableCell();

                                        if (j == 3 || j == 4 || j == 5 || j == 7 || j == 8 || j == 9 || j == 10)
                                            cell.HorizontalAlign = HorizontalAlign.Center;

                                        if (j == 1 || j == 2 || j == 6)
                                            cell.HorizontalAlign = HorizontalAlign.Center;

                                        cell.Text = data2[j];
                                        cell.Font.Size = FontUnit.Point(10);
                                        cell.Font.Name = "Arial";

                                        row.Cells.Add(cell);
                                    }
                                    table.Rows.Add(row);
                                }

                                // Total
                                if (t == dtSP.Rows.Count - 1)
                                {
                                    TableRow row = new TableRow();
                                    string[] data3 = new string[] { "", "", "", "S.Total", 
                                    totalRoll_PL.ToString(), totalQty_PL.ToString(), totalNet_PL.ToString(), totalGross_PL.ToString(), "" };

                                    for (int j = 0; j < data3.Length; j++)
                                    {
                                        TableCell cell = new TableCell();

                                        cell.Font.Bold = true;
                                        cell.HorizontalAlign = HorizontalAlign.Center;
                                        cell.BackColor = Color.LightSteelBlue;
                                        cell.Text = data3[j];
                                        cell.Font.Size = FontUnit.Point(10);
                                        cell.Font.Name = "Arial";

                                        row.Cells.Add(cell);
                                    }

                                    table.Rows.Add(row);
                                }
                            }
                            // Tính tổng tùng PL No.
                        }
                    }

                    //Chen 2 row khoang cach
                    for (int i = 0; i < 1; i++)
                    {
                        TableRow rowS = new TableRow();
                        table.Rows.Add(rowS);
                    }

                    // Tổng đơn
                    string[] data4 = new string[] { "", "", "", "G.TOTAL",
                                    totalRoll_All.ToString(), totalQty_All.ToString(), totalNet_All.ToString(), totalGross_All.ToString(), "" };

                    TableRow rowEND = new TableRow();

                    for (int j = 0; j < data4.Length; j++)
                    {
                        TableCell cell = new TableCell();
                        
                        cell.Font.Bold = true;
                        cell.HorizontalAlign = HorizontalAlign.Center;
                        cell.BackColor = Color.LightSteelBlue;
                        cell.Text = data4[j];
                        cell.Font.Size = FontUnit.Point(10);
                        cell.Font.Name = "Arial";

                        rowEND.Cells.Add(cell);
                    }

                    table.Rows.Add(rowEND);

                    dt.Clear();
                    dt.Clone();
                    dtINFO.Clear();
                    dtINFO.Clone();

                    table.RenderControl(htmlWriter);
                    _exportContent = sb.ToString();
                }
            }
            return _exportContent;
        }

        private string ExportToExcel_PackingList(HttpContext context)
        {
            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string _exportContent = "";
            ExecuteData xl = new ExecuteData();

            string condition = " ";

            if (id.Trim().Length > 3)
                condition += " and dh.pk_seq = '" + id + "' ";

            DataTable baocao = new DataTable("PackingList");

            baocao.Columns.Add("1", typeof(string));
            baocao.Columns.Add("2", typeof(string));
            baocao.Columns.Add("3", typeof(string));
            baocao.Columns.Add("4", typeof(string));
            baocao.Columns.Add("5", typeof(string));
            baocao.Columns.Add("6", typeof(string));
            baocao.Columns.Add("7", typeof(string));
            baocao.Columns.Add("8", typeof(string));
            baocao.Columns.Add("9", typeof(string));
            baocao.Columns.Add("10", typeof(string));
            baocao.Columns.Add("11", typeof(string));
            baocao.Columns.Add("12", typeof(string));
            baocao.Columns.Add("13", typeof(string));
            baocao.Columns.Add("14", typeof(string));
            baocao.Columns.Add("15", typeof(string));
            baocao.Columns.Add("16", typeof(string));
            baocao.Columns.Add("17", typeof(string));
            baocao.Columns.Add("18", typeof(string));

            double totalNoOf_All = 0;
            double totalRoll_All = 0;
            double totalQty_All = 0;            
            double totalNet_All = 0;
            double totalGross_All = 0;

            string sql = "SELECT * FROM ( " +
                   " SELECT dh_sp.plNo, dh_sp.sewingNo, sp.ten AS tensp, sp.ma AS itemVN, sp.codeEnglish AS itemJP, dh_sp.mausac_fk, " +
                   " ISNULL((SELECT ten FROM Size WHERE pk_seq = dh_sp.mausac_fk), '') color, SUM(dh_sp.soluongQUYDOI) AS soluong " +
                   " FROM DonHang dh INNER JOIN DonHang_SanPham_ChiTiet dh_sp ON dh.pk_seq = dh_sp.donhang_fk " +
                   " 	INNER JOIN SanPham sp ON dh_sp.sanpham_fk = sp.pk_seq " + condition +
                   " WHERE dh.trangthai IN (0, 1) " +
                   " GROUP BY dh_sp.plNo, dh_sp.sewingNo, sp.ten, sp.ma, sp.codeEnglish, dh_sp.mausac_fk " +
                   " ) A " +
                   " ORDER BY A.itemJP, A.color, A.plNo, A.sewingNo ";

            DataTable dt = xl.ReadTable(sql);

            //double totalQty = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string plNo = dt.Rows[i]["plNo"].ToString();
                string sewingNo = dt.Rows[i]["sewingNo"].ToString();
                string color = dt.Rows[i]["color"].ToString();
                string mausac_fk = dt.Rows[i]["mausac_fk"].ToString();
                string itemVN = dt.Rows[i]["itemVN"].ToString();
                string itemJP = dt.Rows[i]["itemJP"].ToString();

                DataRow drA = baocao.NewRow();

                drA[0] = plNo;
                drA[1] = "";
                drA[2] = "";
                drA[3] = "";
                drA[4] = "(" + itemVN + ") " + color + "(SEWING C/O: " + sewingNo + ")";
                drA[5] = "";
                drA[6] = "";
                drA[7] = "";
                drA[8] = color;
                drA[9] = "";
                drA[10] = "";
                drA[11] = "";
                drA[12] = "";
                drA[13] = "";
                drA[14] = "";
                drA[15] = "";
                drA[16] = "";
                
                baocao.Rows.Add(drA);

                double totalNoOf_PL = 0;
                double totalRoll_PL = 0;
                double totalQty_PL = 0;                
                double totalNet_PL = 0;
                double totalGross_PL = 0;

                // Detail
                sql = " SELECT * " +
                " FROM  " +
                " (  " +
                "    SELECT 1 AS stt, lotNo, cartonNo, sortBy, NoOf, roll, sewingNo, soluongQUYDOI AS soluong, soluong1, soluong2, soluong3, netWeight, grossWeight,  " +
                "    ISNULL((SELECT ten FROM Size WHERE pk_seq = dh.mausac_fk), '') color  " +
                "    FROM DonHang_SanPham_ChiTiet dh  " +
                "    WHERE dh.donhang_fk = '" + id + "' AND dh.plNo = N'" + plNo + "' AND dh.mausac_fk = '" + mausac_fk + "' AND dh.sewingNo = N'" + sewingNo + "' " +
                ") A " +
                " ORDER BY A.stt, A.color, A.lotNo, A.sortBy ";

                DataTable dtSP = xl.ReadTable(sql);
                for (int t = 0; t < dtSP.Rows.Count; t++)
                {
                    DataRow drB = baocao.NewRow();

                    string cartonNo = dtSP.Rows[t]["cartonNo"].ToString();
                    string lotNo = dtSP.Rows[t]["lotNo"].ToString();
                    string NoOf = dtSP.Rows[t]["NoOf"].ToString();
                    color = dtSP.Rows[t]["color"].ToString();
                    string roll = dtSP.Rows[t]["roll"].ToString();                    
                    string soluong = FormatString.ForMatNumber(dtSP.Rows[t]["soluong"].ToString());
                    
                    string netWeight = FormatString.ForMatNumber(dtSP.Rows[t]["netWeight"].ToString());
                    string grossWeight = FormatString.ForMatNumber(dtSP.Rows[t]["grossWeight"].ToString());

                    totalNoOf_PL += 0;
                    totalQty_PL += double.Parse(dtSP.Rows[t]["soluong"].ToString());
                    totalRoll_PL += double.Parse(dtSP.Rows[t]["roll"].ToString());
                    
                    totalNet_PL += double.Parse(dtSP.Rows[t]["netWeight"].ToString());
                    totalGross_PL += double.Parse(dtSP.Rows[t]["grossWeight"].ToString());

                    totalNoOf_All += 0;
                    totalQty_All += double.Parse(dtSP.Rows[t]["soluong"].ToString());
                    totalRoll_All += double.Parse(dtSP.Rows[t]["roll"].ToString());
                    
                    totalNet_All += double.Parse(dtSP.Rows[t]["netWeight"].ToString());
                    totalGross_All += double.Parse(dtSP.Rows[t]["grossWeight"].ToString());

                    if (t > 0 && t < dtSP.Rows.Count && lotNo.Equals(dtSP.Rows[t - 1]["lotNo"].ToString()))
                        lotNo = "";
                    else
                        lotNo = "'" + lotNo;

                    drB[0] = cartonNo;
                    drB[1] = NoOf;
                    drB[2] = lotNo;
                    drB[3] = "";
                    drB[4] = FormatString.ForMatNumber(soluong);
                    drB[5] = "0";
                    drB[6] = "";
                    drB[7] = "";
                    drB[8] = "";
                    drB[9] = "";
                    drB[10] = "";
                    drB[11] = "";
                    drB[12] = roll;
                    drB[13] = FormatString.ForMatNumber(soluong);                    
                    drB[14] = FormatString.ForMatNumber(netWeight);
                    drB[15] = FormatString.ForMatNumber(grossWeight);
                    drB[16] = "";

                    baocao.Rows.Add(drB);

                    if (t == dtSP.Rows.Count - 1)
                    {
                        DataRow drT = baocao.NewRow();
                        
                        drT[0] = "";
                        drT[1] = "";
                        drT[2] = "";
                        drT[3] = "";
                        drT[4] = "S.TOTAL";
                        drT[5] = "";
                        drT[6] = "";
                        drT[7] = "";
                        drT[8] = "";
                        drT[9] = "";
                        drT[10] = "";
                        drT[11] = "";
                        drT[12] = FormatString.ForMatNumber(totalRoll_PL.ToString());
                        drT[13] = FormatString.ForMatNumber(totalQty_PL.ToString());                        
                        drT[14] = FormatString.ForMatNumber(totalNet_PL.ToString());
                        drT[15] = FormatString.ForMatNumber(totalGross_PL.ToString());
                        drT[16] = "";
                        
                        baocao.Rows.Add(drT);
                    }
                }

                if (i == dt.Rows.Count - 1)
                {
                    DataRow drT = baocao.NewRow();

                    drT[0] = "";
                    drT[1] = "";
                    drT[2] = "";
                    drT[3] = "";
                    drT[4] = "G.TOTAL";
                    drT[5] = "";
                    drT[6] = "";
                    drT[7] = "";
                    drT[8] = "";
                    drT[9] = "";
                    drT[10] = "";
                    drT[11] = "";
                    drT[12] = FormatString.ForMatNumber(totalRoll_All.ToString());
                    drT[13] = FormatString.ForMatNumber(totalQty_All.ToString());                    
                    drT[14] = FormatString.ForMatNumber(totalNet_All.ToString());
                    drT[15] = FormatString.ForMatNumber(totalGross_All.ToString());
                    drT[16] = "";
                    
                    baocao.Rows.Add(drT);

                    DataRow drZ = baocao.NewRow();

                    drZ[0] = "";
                    drZ[1] = "(PACKS)";
                    drZ[2] = "";
                    drZ[3] = "";
                    drZ[4] = "";
                    drZ[5] = "";
                    drZ[6] = "";
                    drZ[7] = "";
                    drZ[8] = "";
                    drZ[9] = "";
                    drZ[10] = "";
                    drZ[11] = "";
                    drZ[12] = "(ROLLS)";
                    drZ[13] = "(M)";                    
                    drZ[14] = "(KGS)";
                    drZ[15] = "(KGS)";
                    drZ[16] = "";
                    
                    baocao.Rows.Add(drZ);
                }
            }

            string fileName = "";

            fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\PackingList.xlsx";

            Workbook workbook = new Workbook(fileName);
            Worksheet worksheet = workbook.Worksheets[0];

            worksheet.Cells.ImportDataTable(baocao, false, "A4");
            //worksheet.AutoFitColumns();

            //worksheet.Cells["A2"].PutValue("Created date");
            //worksheet.Cells["B2"].PutValue(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));

            //worksheet.Cells["A3"].PutValue("Username");
            //worksheet.Cells["B3"].PutValue(context.Session["userName"].ToString());

            HttpResponse response = context.Response;
            workbook.Save(response, "PackingList_" + id + ".xlsx", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsx));

            return _exportContent;
        }

        private string ExportToExcel_DLNHANGHOA(HttpContext context)
        {
            string nccId = "";
            if (context.Request.QueryString["nccId"] != null)
                nccId = context.Request.QueryString["nccId"].ToString();

            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();

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
                    header.ColumnSpan = 5;
                    header.Text = "DANH SACH SAN PHAM";
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

                    header.Text = "Ngay tao: " + ngaythang;
                    header.Font.Bold = true;
                    header.ColumnSpan = 5;
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

                    string[] tieude = new string[] { "Nha Cung Cap", "Ma Hang", "Ten Hang", "Gia Von", "Gia Ban" };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;
                        header.BackColor = Color.Blue;
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();

                    string condition = "";
                    if (nccId.Trim().Length > 0)
                        condition += " AND a.ncc_fk = '" + nccId + "' ";


                    string[] quyenGIA = new string[] { "0", "0", "0", "0", "0" };
                    quyenGIA = xl.getRole(userId.ToString(), "2");

                    string sql = "SELECT b.ten as nccTen, a.ma, a.ten, a.giamua, a.giaban FROM SANPHAM a INNER JOIN  NHACUNGCAP b on a.ncc_fk = b.pk_seq " +
                                 "   WHERE a.trangthai = '1' " + condition +
                                 "   ORDER BY b.ten asc, a.ten asc ";

                    DataTable dt = xl.ReadTable(sql);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string tenNCC = dt.Rows[i]["nccTen"].ToString();
                        string ma = dt.Rows[i]["ma"].ToString();
                        string ten = dt.Rows[i]["ten"].ToString();

                        string giavon = FormatString.ForMatNumber( dt.Rows[i]["giamua"].ToString() );
                        if (!quyenGIA[ExecuteData.CapNhat].Equals("1"))
                            giavon = "X";

                        double giaban = double.Parse(dt.Rows[i]["giaban"].ToString());

                        string[] data = new string[] { tenNCC, ma, ten,  
                                                   giavon, FormatString.ForMatNumber(giaban.ToString()) };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            cell.VerticalAlign = VerticalAlign.Middle;

                            if (j == 0 || j == 2 )
                                cell.Width = Unit.Parse("300");
                            else
                            {
                                if (j == 1)
                                    cell.Width = Unit.Parse("150");
                                else
                                    cell.Width = Unit.Parse("100");
                            }

                            if (j >= 3)
                                cell.HorizontalAlign = HorizontalAlign.Right;

                            cell.Text = data[j];

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

        private string ExportToExcel_LAILOTHEODONHANG(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string nganhhang = "";
            if (context.Request.QueryString["nganhhang"] != null)
                nganhhang = context.Request.QueryString["nganhhang"].ToString();

            string nhanhang = "";
            if (context.Request.QueryString["nhanhang"] != null)
                nhanhang = context.Request.QueryString["nhanhang"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();

            string layDACHOT = "0";
            if (context.Request.QueryString["layDACHOT"] != null)
                layDACHOT = context.Request.QueryString["layDACHOT"].ToString();

            string kenh = "";
            if (context.Request.QueryString["kenh"] != null)
                kenh = context.Request.QueryString["kenh"].ToString();

            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();

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
                    header.ColumnSpan = 5;
                    header.Text = "BÁO CÁO LÃI LỖ THEO ĐƠN HÀNG";
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

                    header.Text = "Thời gian từ: " + tungay + " đến " + denngay;
                    header.Font.Bold = true;
                    header.ColumnSpan = 5;
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

                    string[] tieude = new string[] { "Kênh", " Warehouse ", "Nhân viên bán hàng", "Customer", "Số đơn hàng", "Số hóa đơn", "Day", "Doanh số", "Tiền vốn", "Lợi nhuận", "% lợi nhuận" };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;
                        header.BackColor = Color.Blue;
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();

                    string condition = "";
                    if (kenh.Trim().Length > 0)
                        condition += " AND kbh.pk_seq = '" + kenh + "' ";
                    if (kho.Trim().Length > 0)
                        condition += " AND kho.pk_seq = '" + kho + "' ";
                    if (nganhhang.Trim().Length > 0)
                        condition += " AND dh_sp.sanpham_fk in ( SELECT pk_seq FROM SANPHAM WHERE nganhhang_fk = '" + nganhhang + "' )  ";
                    if (nhanhang.Trim().Length > 0)
                        condition += " AND sp.nhanhang_fk = '" + nhanhang + "' ";
                    if (chungloai.Trim().Length > 0)
                        condition += " AND sp.chungloai_fk = '" + chungloai + "' ";
                    if (layDACHOT.Equals("0"))  //LAY TAT CA DON HANG
                        condition += " AND dh.trangthai != '2'  ";
                    else
                        condition += " AND dh.trangthai in (1, 3, 4, 5, 6) ";

                    string sql = "SELECT doanhso.makenh, doanhso.makho, doanhso.sodonhang, doanhso.ngaydonhang, doanhso.nvbhMA, doanhso.nvbhTEN, doanhso.khMA, doanhso.khTEN,     " +
                                 "		SUM(doanhso.doanhTHU) doanhTHU, SUM(doanhso.giaVON) giaVON, SUM(doanhso.doanhTHU - doanhso.giaVON) as loinhuan,  " +
                                 "		case SUM(doanhso.doanhTHU) when 0 then 0 else	SUM( doanhso.doanhTHU - doanhso.giaVON ) * 100 / SUM(doanhTHU) end as ptLOINHUAN, " +
                                 "		ISNULL( ( SELECT top(1) sohoadon FROM HOADON WHERE trangthai in (0, 1) and loaihoadon = 0 and pk_seq in ( SELECT hoadon_fk FROM HOADON_DONHANG WHERE donhang_fk = doanhso.sodonhang ) ), -1) as sohoadon  " +
                                 "FROM     " +
                                 "(     " +
                                 " 	 SELECT kbh.ma makenh, kho.makho makho, dh.pk_seq sodonhang, dh.ngaydonhang, nvbh.ma as nvbhMa, nvbh.ten as nvbhTEN, kh.ma as khMa, kh.hoten as khTEN,       " +
                                 "				SUM( ( dh_sp.soluong * dh_sp.dongia * ( 1 + dh_sp.thueVAT / 100.0 ) )  ) -   " +
                                 "				SUM( ISNULL(trahang.soluong, 0) * ISNULL(trahang.dongia * ( 1 + dh_sp.thueVAT / 100.0 ), 0)  ) -    " +
                                 "				( dh_sp.chietkhauTONG + dh_sp.chietkhau + dh_sp.chietkhauKM ) * ( 1 + dh_sp.thueVAT / 100.0 )    as doanhTHU, " +
                                 "				SUM( ( dh_sp.soluongQUYDOI * ISNULL(dh_sp.giaton, 0) * ( 1 + dh_sp.thueVAT / 100.0 ) )  ) - " +
                                 "              SUM( ISNULL(trahang.soluong, 0) * ISNULL(trahang.dongia * ( 1 + dh_sp.thueVAT / 100.0 ), 0)  ) -    " +
                                 "              ( dh_sp.chietkhauTONG + dh_sp.chietkhau + dh_sp.chietkhauKM ) * ( 1 + dh_sp.thueVAT / 100.0 )    as giaVON  " +
                                 " 	 FROM DonHang dh INNER JOIN  DonHang_SanPham dh_sp on dh.pk_seq = dh_sp.donhang_fk      " +
                                 "			INNER JOIN  NhanVienBanHang nvbh on dh.nvbh_fk = nvbh.pk_seq      " +
                                 "			INNER JOIN  KhachHang kh on dh.khachhang_fk = kh.pk_seq      " +
                                 "            INNER JOIN  KenhBanHang kbh ON kbh.pk_seq = kh.kbh_fk   " +
                                 "            INNER JOIN  Kho kho ON kho.pk_seq = dh.kho_fk   " +
                                 "			LEFT JOIN      " +
                                 "			(     " +
                                 "				SELECT dth.donhang_fk, dth_sp.sanpham_fk, dth_sp.soluong, dth_sp.dongia     " +
                                 "				FROM DonTraHang dth INNER JOIN  DonTraHang_SanPham dth_sp on dth.pk_seq = dth_sp.dontrahang_fk	     " +
                                 "				WHERE dth.trangthai = '1' and dth.loaidonhang = '1' and donhang_fk is not null     " +
                                 "			)     " +
                                 "			trahang on dh.pk_seq = trahang.donhang_fk and dh_sp.sanpham_fk = trahang.sanpham_fk     " +
                                 " 	 WHERE dh.donhang_logistic = 0 and dh.pk_seq not in ( SELECT donhang_fk FROM DonTraHang WHERE trangthai = '1' and loaidonhang = '0' )   " +
                                 " 					and convert( datetime, dh.ngaydonhang, 105) >= convert( datetime, '" + tungay + "', 105)      " +
                                 "					and convert( datetime, dh.ngaydonhang, 105) <= convert( datetime, '" + denngay + "', 105)    " + condition +
                                 " 	 group by kbh.ma, kho.makho, dh.ngaydonhang, nvbh.ma, nvbh.ten, kh.ma, kh.hoten, dh_sp.thueVAT, dh_sp.chietkhauTONG, dh_sp.chietkhau, dh_sp.chietkhauKM, dh.pk_seq       " +
                                 ")     " +
                                 "doanhso      " +
                                 " GROUP BY doanhso.makenh, doanhso.makho, doanhso.sodonhang, doanhso.ngaydonhang, doanhso.nvbhMA, doanhso.nvbhTEN, doanhso.khMA, doanhso.khTEN " +
                                 "ORDER BY doanhso.makho, doanhso.makenh, convert( datetime, doanhso.ngaydonhang, 105) ";

                    DataTable dt = xl.ReadTable(sql);

                    double totalDOANHSO_NVBH = 0;
                    double totalGIAVON_NVBH = 0;

                    double totalDOANHSO2 = 0;
                    double totalGIAVON2 = 0;
                    double totalLOINHUAN2 = 0;
                    double avgLOINHUAN2 = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string sohoadon = dt.Rows[i]["sohoadon"].ToString();
                        if (sohoadon.Equals("-1"))
                            sohoadon = "";

                        string makenh = dt.Rows[i]["makenh"].ToString();
                        string makho = dt.Rows[i]["makho"].ToString();
                        string sodonhang = dt.Rows[i]["sodonhang"].ToString();
                        string ngaydonhang = dt.Rows[i]["ngaydonhang"].ToString() ;
                        string tenNVBH = dt.Rows[i]["nvbhTEN"].ToString();
                        string tenKH = dt.Rows[i]["khTEN"].ToString();

                        double doanhso = double.Parse(dt.Rows[i]["doanhTHU"].ToString());
                        double giavon = double.Parse(dt.Rows[i]["giaVON"].ToString());

                        double loinhuan = doanhso - giavon;
                        double pt_loinhuan = Math.Round(loinhuan / doanhso, 2) * 100;

                        string[] data = new string[] { makho, makenh, tenNVBH, tenKH, sodonhang, sohoadon, ngaydonhang, 
                                                    FormatString.ForMatNumber(doanhso.ToString()), FormatString.ForMatNumber(giavon.ToString()), 
                                                    FormatString.ForMatNumber(loinhuan.ToString()),  FormatString.ForMatNumber(pt_loinhuan.ToString()) };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            cell.VerticalAlign = VerticalAlign.Middle;

                            if (j == 3)
                                cell.Width = Unit.Parse("230");
                            else
                            {
                                if (j == 2)
                                    cell.Width = Unit.Parse("150");
                                else
                                    cell.Width = Unit.Parse("100");
                            }

                            if (j >= 5)
                                cell.HorizontalAlign = HorizontalAlign.Right;

                            cell.Text = data[j];

                            row.Cells.Add(cell);
                        }

                        table.Rows.Add(row);

                        totalDOANHSO_NVBH += doanhso;
                        totalGIAVON_NVBH += giavon;


                        totalDOANHSO2 += doanhso;
                        totalGIAVON2 += giavon;

                        bool addTOTAL = false;
                        if (i == dt.Rows.Count - 1)
                        {
                            addTOTAL = true;
                        }

                        if (addTOTAL)
                        {
                            //VE ROW NEW
                            totalLOINHUAN2 = totalDOANHSO2 - totalGIAVON2;
                            avgLOINHUAN2 = Math.Round(totalLOINHUAN2 / totalDOANHSO2, 2) * 100;
                            TableRow row2 = new TableRow();
                            data = new string[] { " TOTAL", FormatString.ForMatNumber(totalDOANHSO2.ToString()), FormatString.ForMatNumber(totalGIAVON2.ToString()), 
                                                    FormatString.ForMatNumber(totalLOINHUAN2.ToString()), avgLOINHUAN2.ToString() };

                            for (int j = 0; j < data.Length; j++)
                            {
                                TableCell cell = new TableCell();
                                cell.BackColor = Color.GreenYellow;
                                cell.Font.Bold = true;
                                cell.VerticalAlign = VerticalAlign.Middle;

                                if (j == 0)
                                {
                                    cell.ColumnSpan = 7;
                                    cell.HorizontalAlign = HorizontalAlign.Center;
                                }

                                if (j >= 1)
                                    cell.HorizontalAlign = HorizontalAlign.Right;

                                cell.Text = data[j];
                                row2.Cells.Add(cell);
                            }

                            table.Rows.Add(row2);

                        }
                    }
                    table.RenderControl(htmlWriter);
                    _exportContent = sb.ToString();
                }

            }

            return _exportContent;
        }

        private string ExportToExcel_THEODOIDONHANG(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string nvbhId = "";
            if (context.Request.QueryString["nvbhId"] != null)
                nvbhId = context.Request.QueryString["nvbhId"].ToString();

            string khId = "";
            if (context.Request.QueryString["khId"] != null)
                khId = context.Request.QueryString["khId"].ToString();

            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            string loaidonhang = "";
            if (context.Request.QueryString["loaidonhang"] != null)
                loaidonhang = context.Request.QueryString["loaidonhang"].ToString();

            string trangthai = "";
            if (context.Request.QueryString["trangthai"] != null)
                trangthai = context.Request.QueryString["trangthai"].ToString();

            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();

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
                    header.Text = "THEO DÕI ĐƠN HÀNG XUẤT";
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

                    if (tungay.Trim().Length > 0 && denngay.Trim().Length > 0)
                        header.Text = "Thời gian: từ " + tungay + " đến " + denngay;
                    header.Font.Bold = true;
                    header.ColumnSpan = 9;
                    //header.BackColor = Color.LightGray;
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    //Tieu de
                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.Text = "Thời gian tạo:  " + ngaythang;
                    header.Font.Bold = true;
                    header.ColumnSpan = 9;
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


                    string[] tieude = new string[] { "STT", "Loại đơn hàng", " Warehouse ", "Chủng loại", "Date", "Delivery date", "Số đơn hàng", 
                        "Customer code", "PartCode", "Description", "Xuất xứ", "Location", "Bin", "Số lô", 
                        "Quantity", "Unit", "Quantity lẻ", "Unit lẻ", "Khối lượng(kg)", "Thể tích(m3)" };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;
                        header.BackColor = Color.Blue;
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();

                    string condition = "  ";
                    if (nvbhId.Trim().Length > 0)
                        condition += " AND b.pk_seq = '" + nvbhId + "' ";
                    if (loaidonhang.Trim().Length > 0)
                        condition += " AND a.donhangkygui = '" + loaidonhang + "' ";
                    if (khId.Trim().Length > 0)
                        condition += " AND c.pk_seq = '" + khId + "' ";
                    if (tungay.Trim().Length > 0)
                        condition += " AND convert( datetime, a.ngaydonhang, 105) >= convert( datetime, '" + tungay + "', 105) ";
                    if (denngay.Trim().Length > 0)
                        condition += " AND convert( datetime, a.ngaydonhang, 105) <= convert( datetime, '" + denngay + "', 105) ";
                    if (trangthai.Trim().Length > 0)
                        condition += " AND a.trangthai = '" + trangthai + "' ";
                    //if (nganhhangId.Trim().Length > 0)
                    //    condition += " and a.pk_seq in ( SELECT donhang_fk FROM DONHANG_SANPHAM WHERE sanpham_fk in ( SELECT pk_seq FROM SANPHAM WHERE nganhhang_fk = '" + nganhhangId + "' ) ) ";

                    if (kho.Trim().Length > 0)
                        condition += " AND a.kho_fk = '" + kho + "'";

                    string sql = "  SELECT CASE donhangkygui WHEN 1 THEN (SELECT ma FROM NHOMKHACHHANG WHERE pk_seq = A.doituongKG) " +
                             " 					     WHEN 0 THEN (SELECT makho FROM Kho WHERE pk_seq = A.kho_fk) END makho, A.chungloai, A.maKH, A.sodonhang, " +
                             " 	    A.DonHangKyGui, A.ngaydonhang, A.ngaygiaohang, A.xuatxu, A.location, A.bin, A.solo, A.ma, A.ten, A.donvitinh, SUM(A.soluongQUYDOI) soluong, A.quycach,  " +
                             " 		ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = (SELECT dvt1_fk FROM QuyCach WHERE sanpham_fk = A.sanpham_fk)), '') donviQuyDoi, ISNULL(A.trongluong, 0) trongluong, ISNULL(A.thetich, 0) thetich  " +
                             " 	FROM         " +
                             " 	( SELECT  a.kho_fk, a.doituongKG, cl.ten as chungloai, kh.ma maKH, a.pk_seq sodonhang, a.ngaydonhang, a.ngaygiaohang, a.donhangkygui, d.sanpham_fk, d.solo, c.ma, c.ten, dvt.ten as donvitinh, " +
                             "              b.quycach, d.soluongQUYDOI, '' as scheme,   " +
                             " 				d.xuatxu_fk, xx.ten xuatxu, d.location_fk, loc.ma location, d.bin_fk, bin.ten bin, ISNULL(c.trongluong, 0) trongluong, ISNULL(c.thetich, 0) thetich, 1 as stt   " +
                             " 	  FROM DonHang a INNER JOIN  DONHANG_SANPHAM b on a.pk_seq = b.donhang_fk    " +
                             " 			INNER JOIN  DonHang_SanPham_ChiTiet d ON b.donhang_fk = d.donhang_fk AND b.sanpham_fk = d.sanpham_fk   " +
                             " 			INNER JOIN  SANPHAM c on b.sanpham_fk = c.pk_seq   " +
                             "          INNER JOIN  KhachHang kh ON a.khachhang_fk = kh.pk_seq " +
                             "          LEFT JOIN  ChungLoai cl ON c.chungloai_fk = cl.pk_seq " +
                             " 			INNER JOIN  XuatXu xx ON d.xuatxu_fk = xx.pk_seq   " +
                             " 			INNER JOIN  Location loc ON d.location_fk = loc.pk_seq   " +
                             " 			INNER JOIN  Bin bin ON d.bin_fk = bin.pk_seq   " +
                             " 			INNER JOIN  DonViTinh dvt ON c.dvt_fk = dvt.pk_seq   " +
                             " 		WHERE a.trangthai != '3' " + condition +
                             " 	   UNION ALL    " +
                             " 		SELECT b.kho_fk, a.doituongKG, cl.ten as chungloai, kh.ma maKH, a.pk_seq sodonhang, a.ngaydonhang, a.ngaygiaohang, a.donhangkygui, d.pk_seq as sanpham_fk, b.solo, ISNULL(d.ma, '') as ma, ISNULL(d.ten, '') as ten, dvt.ten as donvitinh, " +
                             " 				 b.quycach, ISNULL(b.soluong, 0) as soluongQuyDoi,c.scheme,   " +
                             " 				 b.xuatxu_fk, xx.ten xuatxu, loc.pk_seq location_fk, loc.ma location , bin.pk_seq as bin_fk, bin.ten  bin, ISNULL(d.trongluong, 0) trongluong, ISNULL(d.thetich, 0) thetich, 2 as stt   " +
                             " 		FROM DonHang a INNER JOIN  DONHANG_CTKM_TRAKM b on a.pk_seq = b.donhang_fk    " +
                             " 			INNER JOIN  CHUONGTRINHKHUYENMAI c on b.ctkm_fk = c.pk_seq    " +
                             " 			LEFT JOIN  SANPHAM d on b.spMA = d.MA    " +
                              "         INNER JOIN  KhachHang kh ON a.khachhang_fk = kh.pk_seq " +
                             "          LEFT JOIN  ChungLoai cl ON d.chungloai_fk = cl.pk_seq " +
                             " 			INNER JOIN  XuatXu xx ON b.xuatxu_fk = xx.pk_seq   " +
                             " 			INNER JOIN  Location loc ON b.location_fk = loc.pk_seq   " +
                             " 			INNER JOIN  Bin bin ON b.bin_fk = bin.pk_seq     " +
                             " 			INNER JOIN  DonViTinh dvt ON d.dvt_fk = dvt.pk_seq   " +
                             " 		WHERE a.trangthai != '3'     " + condition +
                             " 	) A LEFT JOIN    (    " +
                             " 		SELECT  dth.kho_fk, dth_sp.sanpham_fk, dth_sp_ct.xuatxu_fk, dth_sp_ct.location_fk, dth_sp_ct.bin_fk, dth_sp_ct.solo, SUM(dth_sp_ct.soluongQuyDoi) soluongTRA   " +
                             " 		FROM DonTraHang dth INNER JOIN  DonTraHang_SanPham dth_sp on dth.pk_seq = dth_sp.dontrahang_fk	      " +
                             " 			INNER JOIN  DonTraHang_SanPham_ChiTiet dth_sp_ct on dth_sp.sanpham_fk = dth_sp_ct.dontrahang_fk AND dth_sp.dontrahang_fk = dth_sp_ct.dontrahang_fk   " +
                             " 	   WHERE dth.trangthai = '1' and dth.loaidonhang = '1' and donhang_fk is not null 		   " +
                             " 			AND  convert( datetime, dth.ngaydonhang, 105) >= convert( datetime, '" + tungay + "', 105)        " +
                             " 			AND convert( datetime, dth.ngaydonhang, 105) <= convert( datetime, '" + denngay + "', 105)    " +
                             " 	   GROUP BY dth_sp.sanpham_fk, dth.kho_fk, dth_sp_ct.xuatxu_fk, dth_sp_ct.location_fk, dth_sp_ct.bin_fk, dth_sp_ct.solo  " +
                             " 	 ) trahangTABLE ON trahangTABLE.sanpham_fk = A.sanpham_fk AND trahangTABLE.kho_fk = A.kho_fk   " +
                             " 			AND trahangTABLE.location_fk = A.location_fk AND trahangTABLE.bin_fk = A.bin_fk AND trahangTABLE.xuatxu_fk = A.xuatxu_fk  AND trahangTABLE.solo = A.solo " +
                             "  GROUP BY A.ngaydonhang, A.kho_fk, A.doituongKG, A.DonHangKyGui, A.chungloai, A.maKH, A.sodonhang, A.ngaygiaohang, " +
                             "              A.xuatxu, A.location, A.bin, A.sanpham_fk, A.ma, A.ten, A.donvitinh, A.quycach, A.solo, A.trongluong, A.thetich " +
                             "  ORDER BY A.DonHangKyGui, makho, CONVERT(datetime, A.ngaydonhang, 105), CONVERT(datetime, A.ngaygiaohang, 105), A.chungloai, A.maKH, A.ma, A.ten, A.xuatxu, A.location, A.bin,   CONVERT(datetime, A.solo, 105) ASC ";

                    DataTable dt = xl.ReadTable(sql);

                    bool checkLoaiDonHang = true;

                    double totalTHUNG_NGAY = 0;
                    double totalLE_NGAY = 0;

                    double totalTHUNG_KHO = 0;
                    double totalLE_KHO = 0;

                    double TOTALthung_ALL = 0;
                    double TOTALle_ALL = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string donhangkygui = dt.Rows[i]["donhangkygui"].ToString();
                        if (donhangkygui.Equals("0"))
                        {
                            loaidonhang = "Hàng bán";
                        }
                        else
                        {
                            loaidonhang = "Hàng ký gửi";
                        }
                        string makho = dt.Rows[i]["makho"].ToString();
                        string chungloai = dt.Rows[i]["chungloai"].ToString();

                        string maKH = dt.Rows[i]["maKH"].ToString();
                        //string tenKH = dt.Rows[i]["tenKH"].ToString();

                        string sodonhang = dt.Rows[i]["sodonhang"].ToString();
                        string ngaydonhang = dt.Rows[i]["ngaydonhang"].ToString() ;

                        string ngaygiaohang = dt.Rows[i]["ngaygiaohang"].ToString() ;

                        string xuatxu = dt.Rows[i]["xuatxu"].ToString();
                        string location = dt.Rows[i]["location"].ToString();
                        string bin = dt.Rows[i]["bin"].ToString();
                        string solo = dt.Rows[i]["solo"].ToString();
                        string masp = dt.Rows[i]["ma"].ToString();
                        string tensp = dt.Rows[i]["ten"].ToString();
                        string donviQuyDoi = dt.Rows[i]["donviQuyDoi"].ToString();
                        string donvichuan = dt.Rows[i]["donvitinh"].ToString();

                        double soluong = double.Parse(dt.Rows[i]["soluong"].ToString());
                        double quycach = double.Parse(dt.Rows[i]["quycach"].ToString());
                        double trongluong = double.Parse(dt.Rows[i]["trongluong"].ToString());
                        double thetich = double.Parse(dt.Rows[i]["thetich"].ToString());

                        trongluong = trongluong * soluong;
                        thetich = thetich * soluong;

                        double sothung = 0;
                        double soluongLe = 0;
                        if (quycach <= 0)
                        {
                            sothung = 0;
                            soluongLe = soluong;
                        }
                        else
                        {
                            if (soluong % quycach == 0)
                                sothung = soluong / quycach;

                            soluongLe = Math.Round((soluong - (sothung * quycach)), 3);
                        }

                        totalTHUNG_NGAY += sothung;
                        totalTHUNG_KHO += sothung;
                        TOTALthung_ALL += sothung;

                        totalLE_NGAY += soluongLe;
                        totalLE_KHO += soluongLe;
                        TOTALle_ALL += soluongLe;

                        string[] data = new string[] { };

                        if (1 == 0)
                        {
                            #region Loai don hang (Ban - Ky gui)

                            if (donhangkygui.Equals("0") && checkLoaiDonHang == true)
                            {
                                //VE ROW NEW
                                TableRow row2 = new TableRow();
                                data = new string[] { " HÀNG BÁN " };
                                for (int jj = 0; jj < data.Length; jj++)
                                {
                                    TableCell cell = new TableCell();
                                    cell.BackColor = Color.LightGreen;
                                    cell.Font.Bold = true;
                                    cell.VerticalAlign = VerticalAlign.Middle;
                                    cell.ColumnSpan = 17;
                                    cell.HorizontalAlign = HorizontalAlign.Center;
                                    cell.Text = data[jj];
                                    row2.Cells.Add(cell);
                                }
                                checkLoaiDonHang = false;

                                table.Rows.Add(row2);
                            }
                            else if (donhangkygui.Equals("1") && checkLoaiDonHang == false)
                            {
                                //VE ROW NEW
                                TableRow row2 = new TableRow();
                                data = new string[] { " HÀNG KÝ GỬI " };
                                for (int jj = 0; jj < data.Length; jj++)
                                {
                                    TableCell cell = new TableCell();
                                    cell.BackColor = Color.LightGreen;
                                    cell.Font.Bold = true;
                                    cell.VerticalAlign = VerticalAlign.Middle;
                                    cell.ColumnSpan = 17;
                                    cell.HorizontalAlign = HorizontalAlign.Center;
                                    cell.Text = data[jj];
                                    row2.Cells.Add(cell);
                                }

                                checkLoaiDonHang = true;
                                table.Rows.Add(row2);
                            }

                            #endregion
                        }

                        data = new string[] { (i + 1).ToString(), loaidonhang, makho, chungloai, ngaydonhang, ngaygiaohang, sodonhang, maKH, masp, tensp, xuatxu, location, bin, solo,
                            FormatString.ForMatNumber(sothung.ToString()), donviQuyDoi, FormatString.ForMatNumber(soluongLe.ToString()), donvichuan, FormatString.ForMatNumber(trongluong.ToString()), FormatString.ForMatNumber(thetich.ToString()) };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            cell.VerticalAlign = VerticalAlign.Middle;

                            if (j < 9 || j == 10 || j == 13)
                            {
                                if (j == 0)
                                {
                                    cell.Width = Unit.Parse("50");
                                }
                                else if (j == 2)
                                {
                                    cell.Width = Unit.Parse("100");
                                }
                                else if (j == 1 || j == 4)
                                {
                                    cell.Width = Unit.Parse("100");
                                }
                                else if (j == 7)
                                {
                                    cell.Width = Unit.Parse("120");
                                }
                                else
                                    cell.Width = Unit.Parse("80");
                                cell.HorizontalAlign = HorizontalAlign.Center;
                            }
                            else if (j == 9)
                            {
                                cell.Width = Unit.Parse("450");
                                cell.HorizontalAlign = HorizontalAlign.Left;
                            }
                            else if (j == 10 || j == 11 || j == 12)
                            {
                                cell.Width = Unit.Parse("120");
                                cell.HorizontalAlign = HorizontalAlign.Center;
                            }
                            else if (j == 14 || j == 16)
                            {
                                cell.Width = Unit.Parse("60");
                                cell.HorizontalAlign = HorizontalAlign.Right;
                            }

                            cell.Text = data[j];

                            row.Cells.Add(cell);

                        }

                        table.Rows.Add(row);

                        if (1 == 0)
                        {
                            #region Total NGAY
                            //THEO NGAY
                            bool addNGAY = false;
                            if (i < dt.Rows.Count - 1)
                            {
                                if (!ngaydonhang.Equals(dt.Rows[i + 1]["ngaydonhang"].ToString()))
                                    addNGAY = true;
                            }
                            else
                                addNGAY = true;

                            //THEO KHO
                            bool kho_BEFORE = false;
                            if (i < dt.Rows.Count - 1)
                            {
                                if (!makho.Equals(dt.Rows[i + 1]["makho"].ToString()))
                                    kho_BEFORE = true;
                            }
                            else
                                kho_BEFORE = true;

                            if (addNGAY)
                            {

                                //VE ROW NEW
                                TableRow row2 = new TableRow();
                                data = new string[] { " DATE " + ngaydonhang + " __ TOTAL ", 
                                        FormatString.ForMatNumber(totalTHUNG_NGAY.ToString()), "", FormatString.ForMatNumber(totalLE_NGAY.ToString()), "" };
                                for (int jj = 0; jj < data.Length; jj++)
                                {
                                    TableCell cell = new TableCell();
                                    cell.BackColor = Color.LightGray;
                                    cell.Font.Bold = true;
                                    cell.VerticalAlign = VerticalAlign.Middle;

                                    if (jj == 0)
                                    {
                                        cell.ColumnSpan = 9;
                                        cell.HorizontalAlign = HorizontalAlign.Center;
                                    }

                                    if (jj >= 1)
                                        cell.HorizontalAlign = HorizontalAlign.Right;

                                    cell.Text = data[jj];
                                    row2.Cells.Add(cell);
                                }

                                if (kho_BEFORE)
                                {
                                    totalTHUNG_KHO = 0;
                                    totalLE_KHO = 0;
                                }

                                totalTHUNG_NGAY = 0;
                                totalLE_NGAY = 0;

                                table.Rows.Add(row2);

                            }

                            #endregion

                            #region Total Kho

                            if (kho_BEFORE)
                            {
                                //string  makho = dt.Rows[i]["makho"].ToString();

                                //VE ROW NEW
                                TableRow row2 = new TableRow();
                                data = new string[] { makho + " __ TOTAL ", 
                                FormatString.ForMatNumber(totalTHUNG_KHO.ToString()), "", FormatString.ForMatNumber(totalLE_KHO.ToString()), "" };

                                for (int jj = 0; jj < data.Length; jj++)
                                {
                                    TableCell cell = new TableCell();
                                    cell.BackColor = Color.LightBlue;
                                    cell.Font.Bold = true;
                                    cell.VerticalAlign = VerticalAlign.Middle;

                                    if (jj == 0)
                                    {
                                        cell.ColumnSpan = 9;
                                        cell.HorizontalAlign = HorizontalAlign.Center;
                                    }

                                    if (jj >= 1)
                                        cell.HorizontalAlign = HorizontalAlign.Right;

                                    cell.Text = data[jj];
                                    row2.Cells.Add(cell);
                                }

                                if (addNGAY)
                                {
                                    totalTHUNG_NGAY = 0;
                                    totalLE_NGAY = 0;
                                }

                                totalTHUNG_KHO = 0;
                                totalLE_KHO = 0;

                                table.Rows.Add(row2);
                            }

                            #endregion

                            #region Total ALL

                            bool addTOTAL = false;
                            if (i == dt.Rows.Count - 1)
                            {
                                addTOTAL = true;
                            }

                            if (addTOTAL)
                            {
                                //VE ROW NEW
                                TableRow row2 = new TableRow();
                                data = new string[] { "TOTAL", FormatString.ForMatNumber(TOTALthung_ALL.ToString()), "", FormatString.ForMatNumber(TOTALle_ALL.ToString()), "" };

                                for (int j = 0; j < data.Length; j++)
                                {
                                    TableCell cell = new TableCell();
                                    cell.BackColor = Color.GreenYellow;
                                    cell.Font.Bold = true;
                                    cell.VerticalAlign = VerticalAlign.Middle;

                                    if (j == 0)
                                    {
                                        cell.ColumnSpan = 9;
                                        cell.HorizontalAlign = HorizontalAlign.Center;
                                    }

                                    if (j >= 1)
                                        cell.HorizontalAlign = HorizontalAlign.Right;

                                    cell.Text = data[j];
                                    row2.Cells.Add(cell);
                                }

                                table.Rows.Add(row2);

                            }

                            #endregion
                        }

                    }
                    table.RenderControl(htmlWriter);
                    _exportContent = sb.ToString();
                }
            }
            return _exportContent;
        }

        private string ExportToExcel_TONGHOPDONHANG(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string nvbhId = "";
            if (context.Request.QueryString["nvbhId"] != null)
                nvbhId = context.Request.QueryString["nvbhId"].ToString();

            string khId = "";
            if (context.Request.QueryString["khId"] != null)
                khId = context.Request.QueryString["khId"].ToString();

            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            //string nganhhangId = "";
            //if (context.Request.QueryString["nganhhangId"] != null)
            //    nganhhangId = context.Request.QueryString["nganhhangId"].ToString();

            string trangthai = "";
            if (context.Request.QueryString["trangthai"] != null)
                trangthai = context.Request.QueryString["trangthai"].ToString();

            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();

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
                    header.ColumnSpan = 7;
                    header.Text = "TỔNG HỢP ĐƠN HÀNG XUẤT";
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

                    if (tungay.Trim().Length > 0 && denngay.Trim().Length > 0)
                        header.Text = "Thời gian: từ    " + tungay + " đến " + denngay;
                    else
                        header.Text = "Thời gian tạo:   "  + ngaythang;
                    header.Font.Bold = true;
                    header.ColumnSpan = 7;
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


                    string[] tieude = new string[] { "STT", "Kênh", " Warehouse ", "Day", "Chứng từ", "Nhân viên", "Customer", 
                        "Tổng tiền", "Chiết khâu ĐH", "Tổng tiền trả", "Tổng tiền khuyến mại", "Tổng chiết khấu", "Tổng tiền thanh toán", "Ghi chú" };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;
                        header.BackColor = Color.Blue;
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();

                    string condition = "  ";
                    if (nvbhId.Trim().Length > 0)
                        condition += " AND nvbh.pk_seq = '" + nvbhId + "' ";
                    if (khId.Trim().Length > 0)
                        condition += " AND kh.pk_seq = '" + nvbhId + "' ";
                    if (tungay.Trim().Length > 0)
                        condition += " AND convert( datetime, dh.ngaydonhang, 105) >= convert( datetime, '" + tungay + "', 105) ";
                    if (denngay.Trim().Length > 0)
                        condition += " AND convert( datetime, dh.ngaydonhang, 105) <= convert( datetime, '" + denngay + "', 105) ";
                    if (trangthai.Trim().Length > 0)
                        condition += " AND dh.trangthai = '" + trangthai + "' ";
                    if (kho.Trim().Length > 0)
                        condition += " AND dh.kho_fk = '" + kho + "' ";

                    //if (nganhhangId.Trim().Length > 0)
                    //    condition += " and dh.pk_seq in ( SELECT donhang_fk FROM DONHANG_SANPHAM WHERE sanpham_fk in ( SELECT pk_seq FROM SANPHAM WHERE nganhhang_fk = '" + nganhhangId + "' ) ) ";


                    //string sql = "SELECT doanhso.ngaydonhang, doanhso.pk_seq chungtu, doanhso.nvbhTEN, doanhso.khTEN,  " +
                    //             "	doanhso.tongtienMUA, doanhso.chietkhauDH, doanhso.tongtienTRA, doanhso.chietkhauKM,  " +
                    //             "	(doanhso.tongtienMUA - doanhso.tongtienTRA - doanhso.chietkhauDH - doanhso.chietkhauKM)  as tongtien_sauKM, doanhso.ghichu " +
                    //             "FROM   (    	  " +
                    //             "	SELECT dh.ngaydonhang, nvbh.ten as nvbhTEN, kh.hoten as khTEN, dh.pk_seq, '' as ctkm, " +
                    //            //" ISNULL(dh.TONGSOTIEN, 0) tongtienMUA, " +
                    //              "		SUM( ( ( dh_sp.soluong ) * ISNULL(dh_sp.dongiaNEW, dh_sp.dongia) )  ) as tongtienMUA, " +
                    //             "		SUM( ISNULL(trahang.soluong, 0) * ISNULL(trahang.dongia, 0)  ) as tongtienTRA, " +
                    //            // "		SUM( ISNULL(khuyenmai.soluong, 0) * ISNULL(khuyenmai.dongia, 0)  ) as tongtienKM, " +
                    //             "		SUM( ISNULL(kmCHIETKHAU.tongCHIETKHAU, 0) ) as chietkhauKM, " +
                    //             "		CASE WHEN dh.pt_chietkhau < 100  " +
                    //             "		then SUM (  ( ( dh_sp.soluong * ISNULL(dh_sp.dongiaSAUCHIA, dh_sp.dongia) - dh_sp.chietkhau ) " +
                    //             "			 - ISNULL( kmCHIETKHAU.tongCHIETKHAU, 0 ) ) * dh.pt_chietkhau / 100 ) " +
                    //             "		else dh.pt_chietkhau end as chietkhauDH, dh.ghichu " +
                    //             "	FROM DonHang dh  " +
                    //             "		INNER JOIN  DonHang_SanPham dh_sp on dh.pk_seq = dh_sp.donhang_fk " +
                    //             "		INNER JOIN  NhanVienBanHang nvbh on dh.nvbh_fk = nvbh.pk_seq " +
                    //             "		INNER JOIN  KhachHang kh on dh.khachhang_fk = kh.pk_seq  " +
                    //             "		LEFT JOIN  (   				 " +
                    //             "			SELECT dth.donhang_fk, dth_sp.sanpham_fk, dth_sp.soluong, dth_sp.dongia " +
                    //             "			FROM DonTraHang dth  " +
                    //             "				INNER JOIN  DonTraHang_SanPham dth_sp on dth.pk_seq = dth_sp.dontrahang_fk " +
                    //             "			WHERE dth.trangthai = '1' and dth.loaidonhang = '1' and donhang_fk is not null  " +
                    //             "				)trahang on dh.pk_seq = trahang.donhang_fk  " +
                    //             //"		LEFT JOIN  (   				 " +
                    //             //"			SELECT km.donhang_fk, sp.pk_seq as sanpham_fk, km.soluong, km.tonggiatri / km.soluong as dongia " +
                    //             //"			FROM DonHang_CTkm_TraKM km INNER JOIN  SANPHAM sp on km.spMA = sp.MA " +
                    //             //"			WHERE km.soluong != 0  " +
                    //             //"				)khuyenmai on dh.pk_seq = khuyenmai.donhang_fk  " +
                    //             "		LEFT JOIN  ( " +
                    //             "			SELECT a.DONHANG_FK, d.sanpham_fk, SUM(a.TONGGIATRI) as tongCHIETKHAU " +
                    //             "			FROM DONHANG_CTKM_TRAKM a " +
                    //             "				INNER JOIN  ChuongTrinhKM_DKKM b on a.ctkm_fk = b.ctkm_fk " +
                    //             "				INNER JOIN  DieuKienKhuyenMai_SanPham d on b.dkkm_fk = d.dieukien_fk " +
                    //             "				INNER JOIN  DonHang_SanPham e on a.DONHANG_FK = e.donhang_fk and e.sanpham_fk = d.sanpham_fk " +
                    //             "			WHERE TRAKM_FK in ( SELECT pk_seq FROM TraKhuyenMai WHERE loaitra = '2' and chietkhau > 0 ) " +
                    //             "			group by a.DONHANG_FK, d.sanpham_fk  " +
                    //             "				)kmCHIETKHAU on dh.pk_seq = kmCHIETKHAU.DONHANG_FK and dh_sp.sanpham_fk = kmCHIETKHAU.sanpham_fk   	  " +
                    //             "WHERE dh.pk_seq not in ( SELECT donhang_fk FROM DonTraHang WHERE trangthai = '1' and loaidonhang = '0' )  " +
                    //             "		AND dh.trangthai != '3' " + condition +
                    //             "group by dh.ngaydonhang, dh.pk_seq, nvbh.ten, kh.hoten, dh.pt_chietkhau, dh.ghichu  " +
                    //             ")doanhso     " +
                    //             "ORDER BY convert( datetime, doanhso.ngaydonhang, 105), doanhso.nvbhTEN, doanhso.khTEN ";


                    string sql = "SELECT  doanhso.makenh, doanhso.makho, doanhso.ngaydonhang, doanhso.pk_seq chungtu, doanhso.nvbhTEN, doanhso.khTEN,   " +
                                "		doanhso.tongtienMUA, doanhso.tongtienTRA, doanhso.chietkhauKM,   " +
                                "		CASE WHEN pt_chietkhau < 100 then ( ( doanhso.tongtienMUA - doanhso.chietkhauKM ) * pt_chietkhau / 100 ) else pt_chietkhau end as chietkhauDH,  " +
                                "		doanhso.tongsotien_saukm as tongtien_sauKM, doanhso.ghichu  " +
                                "FROM    " +
                                "(    	   " +
                                "	SELECT kbh.ma makenh, kho.makho makho, dh.ngaydonhang, dh.tongsotien_saukm, nvbh.ten as nvbhTEN, kh.hoten as khTEN, dh.pk_seq, '' as ctkm,  " +
                                " 		 ( SELECT sum(soluong * dongia ) FROM DonHang_SanPham WHERE donhang_fk = dh.pk_seq ) as tongtienMUA,  " +
                                "			ISNULL(trahang.tonggiatri, 0) as tongtienTRA, ISNULL(khuyenmai.tonggiatri, 0) as chietkhauKM,  " +
                                "			ISNULL(dh.pt_chietkhau, 0) as pt_chietkhau, dh.ghichu  " +
                                "	FROM DonHang dh   " +
                                "		INNER JOIN  NhanVienBanHang nvbh on dh.nvbh_fk = nvbh.pk_seq  " +
                                "		INNER JOIN  KhachHang kh on dh.khachhang_fk = kh.pk_seq   " +
                                "           INNER JOIN  KenhBanHang kbh ON kbh.pk_seq = kh.kbh_fk " +
                                "           INNER JOIN  Kho kho ON kho.pk_seq = dh.kho_fk " +
                                "		LEFT JOIN   " +
                                "		(   				  " +
                                "			SELECT dth.donhang_fk, sum(dth_sp.soluong * dth_sp.dongia) as tonggiatri " +
                                "			FROM DonTraHang dth   " +
                                "				INNER JOIN  DonTraHang_SanPham dth_sp on dth.pk_seq = dth_sp.dontrahang_fk  " +
                                "			WHERE dth.trangthai = '1' and dth.loaidonhang = '1' and donhang_fk is not null   " +
                                "			group by dth.donhang_fk " +
                                "		) " +
                                "		trahang on dh.pk_seq = trahang.donhang_fk  " +
                                "		LEFT JOIN   " +
                                "		(   				  " +
                                "			SELECT dth.donhang_fk, sum(tonggiatri) as tonggiatri " +
                                "			FROM DONHANG_CTKM_TRAKM dth   " +
                        //"			WHERE TRAKM_FK in ( SELECT pk_seq FROM TraKhuyenMai WHERE loaitra = '2' and chietkhau > 0 )   " +
                                "			group by dth.donhang_fk " +
                                "		) " +
                                "		khuyenmai on dh.pk_seq = khuyenmai.donhang_fk   " +
                                "	WHERE dh.donhangkygui = '0' AND dh.pk_seq not in ( SELECT donhang_fk FROM DonTraHang WHERE trangthai = '1' and loaidonhang = '0' )   " +
                                "			AND dh.trangthai != '2'   " + condition +
                                ") " +
                                "doanhso      " +
                                "ORDER BY convert( datetime, doanhso.ngaydonhang, 105), doanhso.makenh, doanhso.makho, doanhso.nvbhTEN, doanhso.khTEN ";

                    DataTable dt = xl.ReadTable(sql);

                    double totalTONGTIEN = 0;
                    double totalTONGTIEN_NGAY = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string makenh = dt.Rows[i]["makenh"].ToString();
                        string makho = dt.Rows[i]["makho"].ToString();

                        double tongtien = double.Parse(dt.Rows[i]["tongtienMUA"].ToString());
                        double chietkhauDH = double.Parse(dt.Rows[i]["chietkhauDH"].ToString());
                        double tongtienTRA = double.Parse(dt.Rows[i]["tongtienTRA"].ToString());
                        double chietkhauKM = double.Parse(dt.Rows[i]["chietkhauKM"].ToString());
                        double tongtienCK = chietkhauDH + chietkhauKM;

                        double tongtien_saukm = double.Parse(dt.Rows[i]["tongtien_sauKM"].ToString());

                        ///double tongtien_saukm = tongtien - chietkhauKM - chietkhauDH - tongtienTRA;

                        totalTONGTIEN += tongtien_saukm;
                        totalTONGTIEN_NGAY += tongtien_saukm;

                        string[] data = new string[] { (i + 1).ToString(), makenh, makho, dt.Rows[i]["ngaydonhang"].ToString(), dt.Rows[i]["chungtu"].ToString(), dt.Rows[i]["nvbhTEN"].ToString(), dt.Rows[i]["khTEN"].ToString(), 
                                                        FormatString.ForMatNumber(tongtien.ToString()), FormatString.ForMatNumber(chietkhauDH.ToString()), FormatString.ForMatNumber(tongtienTRA.ToString()),
                                                        FormatString.ForMatNumber(chietkhauKM.ToString()), FormatString.ForMatNumber(tongtienCK.ToString()), FormatString.ForMatNumber(tongtien_saukm.ToString()), dt.Rows[i]["ghichu"].ToString()  };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            cell.VerticalAlign = VerticalAlign.Middle;

                            if (j == 5 || j == 6 || j == 13)
                            {
                                cell.Width = Unit.Parse("250");
                                cell.HorizontalAlign = HorizontalAlign.Left;
                            }
                            else
                            {
                                if (j == 0 || j == 1 || j == 2)
                                {
                                    cell.Width = Unit.Parse("45");
                                    cell.HorizontalAlign = HorizontalAlign.Center;
                                }
                                else
                                {
                                    cell.Width = Unit.Parse("100");

                                    if (j > 7 && j < 13)
                                        cell.HorizontalAlign = HorizontalAlign.Right;
                                }
                            }


                            cell.Text = data[j];

                            row.Cells.Add(cell);

                        }

                        table.Rows.Add(row);


                        //THEM TOTAL THEO NGAY
                        bool addNGAY = false;
                        if (i < dt.Rows.Count - 1)
                        {
                            if (!dt.Rows[i]["ngaydonhang"].ToString().Equals(dt.Rows[i + 1]["ngaydonhang"].ToString()))
                                addNGAY = true;
                        }
                        else
                            addNGAY = true;

                        if (addNGAY)
                        {
                            string ngay = dt.Rows[i]["ngaydonhang"].ToString();

                            //VE ROW NEW
                            TableRow row2 = new TableRow();
                            data = new string[] { ngay + " Tổng cộng ", FormatString.ForMatNumber(totalTONGTIEN_NGAY.ToString()), "" };

                            for (int jj = 0; jj < data.Length; jj++)
                            {
                                TableCell cell = new TableCell();
                                cell.BackColor = Color.LightGray;
                                cell.Font.Bold = true;
                                cell.VerticalAlign = VerticalAlign.Middle;

                                if (jj == 0)
                                {
                                    cell.ColumnSpan = 12;
                                    cell.HorizontalAlign = HorizontalAlign.Center;
                                }

                                if (jj >= 1)
                                    cell.HorizontalAlign = HorizontalAlign.Right;

                                cell.Text = data[jj];
                                row2.Cells.Add(cell);
                            }

                            table.Rows.Add(row2);

                            totalTONGTIEN_NGAY = 0;

                        }


                        bool addTOTAL = false;
                        if (i == dt.Rows.Count - 1)
                        {
                            addTOTAL = true;
                        }

                        if (addTOTAL)
                        {
                            //VE ROW NEW
                            TableRow row2 = new TableRow();
                            data = new string[] { " TỔNG CỘNG", FormatString.ForMatNumber(totalTONGTIEN.ToString()), "" };

                            for (int j = 0; j < data.Length; j++)
                            {
                                TableCell cell = new TableCell();
                                cell.BackColor = Color.GreenYellow;
                                cell.Font.Bold = true;
                                cell.VerticalAlign = VerticalAlign.Middle;

                                if (j == 0)
                                {
                                    cell.ColumnSpan = 12;
                                    cell.HorizontalAlign = HorizontalAlign.Center;
                                }

                                if (j >= 1)
                                    cell.HorizontalAlign = HorizontalAlign.Right;

                                cell.Text = data[j];
                                row2.Cells.Add(cell);
                            }

                            table.Rows.Add(row2);

                        }


                    }
                    table.RenderControl(htmlWriter);
                    _exportContent = sb.ToString();
                }
            }
            return _exportContent;
        }

        private string ExportToExcel_TRAHANG(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string nganhhang = "";
            if (context.Request.QueryString["nganhhang"] != null)
                nganhhang = context.Request.QueryString["nganhhang"].ToString();

            string nhanhang = "";
            if (context.Request.QueryString["nhanhang"] != null)
                nhanhang = context.Request.QueryString["nhanhang"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();

            string kenh = "";
            if (context.Request.QueryString["kenh"] != null)
                kenh = context.Request.QueryString["kenh"].ToString();

            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();

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
                    header.ColumnSpan = 5;
                    header.Text = "BÁO CÁO HÀNG TRẢ VỀ TỪ Customer";
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

                    header.Text = "Created date:" + ngaythang;
                    header.Font.Bold = true;
                    header.ColumnSpan = 5;
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


                    string[] tieude = new string[] { " Warehouse ", "Specification", "Số đơn trả hàng", "Ngày trả", "Số đơn hàng", "PartCode", "Description", "Unit", "Scheme", "Quantity trả", "Số tiền trả" };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;
                        header.BackColor = Color.Blue;
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();

                    string condition = "  ";
                    if (kenh.Trim().Length > 0)
                        condition += " AND dth.khachhang_fk in ( SELECT pk_seq FROM KHACHHANG WHERE kbh_fk = '" + kenh + "' ) ";
                    if (tungay.Trim().Length > 0)
                        condition += " AND convert( datetime, dth.ngaydonhang, 105) >= convert( datetime, '" + tungay + "', 105) ";
                    if (denngay.Trim().Length > 0)
                        condition += " AND convert( datetime, dth.ngaydonhang, 105) <= convert( datetime, '" + denngay + "', 105) ";

                    string sql = "SELECT kho.tenkho as khoTEN, nh.ten as nhTEN, dh.donhang_fk, dh.dthId, dh.ngaydonhang, sp.ma as spMA, sp.ten as spTEN, dvt.ten as donvi, dh.scheme, ISNULL(dh.soluong, 0) soluong, ISNULL(dh.thanhtien, 0) thanhtien  " +
                                 "FROM  " +
                                 "(  " +
                                 "	SELECT dth.donhang_fk, dth.pk_seq as dthId, dth.kho_fk, dth.ngaydonhang, dth_sp.sanpham_fk, sum(dth_sp.soluongQUYDOI) as soluong,  " +
                                 "			sum(dth_sp.soluong * dth_sp.dongia * ( 1 + dth_sp.thueVAT / 100.0) ) as thanhtien, '' as scheme    " +
                                 "	FROM DonTraHang dth INNER JOIN  DonTraHang_SanPham dth_sp on dth.pk_seq = dth_sp.dontrahang_fk	    " +
                                 "	WHERE dth.trangthai = '1' and dth_sp.soluong > 0   " + condition +
                                 "	group by dth.donhang_fk, dth.pk_seq, dth.kho_fk, dth.nvbh_fk, dth.ngaydonhang, dth_sp.sanpham_fk  " +
                                 "union  " +
                                 "	SELECT dth.donhang_fk, dth.pk_seq as dthId, dth_sp.kho_fk, dth.ngaydonhang, dth_sp.sanpham_fk, sum(dth_sp.soluongQUYDOI) as soluong,  " +
                                 "			0 as thanhtien, ctkm.scheme    " +
                                 "	FROM DonTraHang dth INNER JOIN  DONTRAHANG_SPKM dth_sp on dth.pk_seq = dth_sp.dontrahang_fk " +
                                 "			INNER JOIN  CHUONGTRINHKHUYENMAI ctkm on dth_sp.ctkm_fk = ctkm.pk_seq " +
                                 "	WHERE dth.trangthai = '1' and dth_sp.soluong > 0   " + condition +
                                 "	group by dth.donhang_fk, dth.pk_seq, dth_sp.kho_fk, dth.nvbh_fk, dth.ngaydonhang, dth_sp.sanpham_fk, ctkm.scheme   " +
                                 ")  " +
                                 "dh INNER JOIN  SANPHAM sp on dh.sanpham_fk = sp.pk_seq  " +
                                 "   INNER JOIN  NGANHHANG nh on sp.nganhhang_fk = nh.pk_seq  " +
                                 "   INNER JOIN  DONVITINH dvt on sp.dvt_fk = dvt.pk_seq   " +
                                 "   INNER JOIN  KHO kho on dh.kho_fk = kho.pk_seq WHERE 1 = 1 " + (kho.Trim().Length > 0 ? " AND dh.kho_fk = '" + kho + "' " : "") +
                                 "ORDER BY kho.pk_seq asc, nh.ten asc, convert(datetime, ngaydonhang, 105) asc, sp.ten asc ";

                    DataTable dt = xl.ReadTable(sql);

                    double totalSOLUONG2 = 0;
                    double totalTHANHTIEN2 = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        double soluong = double.Parse(dt.Rows[i]["soluong"].ToString());
                        double thanhtien = double.Parse(dt.Rows[i]["thanhtien"].ToString());

                        string[] data = new string[] { dt.Rows[i]["khoTEN"].ToString(), dt.Rows[i]["nhTEN"].ToString(), dt.Rows[i]["dthId"].ToString(), dt.Rows[i]["ngaydonhang"].ToString(), dt.Rows[i]["donhang_fk"].ToString(), dt.Rows[i]["spMA"].ToString(), 
                                                        dt.Rows[i]["spTEN"].ToString(), dt.Rows[i]["donvi"].ToString(), dt.Rows[i]["scheme"].ToString(), FormatString.ForMatNumber(soluong.ToString()), FormatString.ForMatNumber(thanhtien.ToString()) };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            cell.VerticalAlign = VerticalAlign.Middle;

                            if (j == 5)
                                cell.Width = Unit.Parse("230");
                            else
                            {
                                if (j == 1 || j == 4)
                                    cell.Width = Unit.Parse("130");
                                else
                                    cell.Width = Unit.Parse("100");
                            }

                            if (j >= 9)
                                cell.HorizontalAlign = HorizontalAlign.Right;

                            cell.Text = data[j];

                            row.Cells.Add(cell);
                        }

                        table.Rows.Add(row);


                        totalSOLUONG2 += soluong;
                        totalTHANHTIEN2 += thanhtien;

                        bool addTOTAL = false;
                        if (i == dt.Rows.Count - 1)
                        {
                            addTOTAL = true;
                        }

                        if (addTOTAL)
                        {
                            //VE ROW NEW
                            TableRow row2 = new TableRow();
                            data = new string[] { " TOTAL", FormatString.ForMatNumber(totalSOLUONG2.ToString()), FormatString.ForMatNumber(totalTHANHTIEN2.ToString()) };

                            for (int j = 0; j < data.Length; j++)
                            {
                                TableCell cell = new TableCell();
                                cell.BackColor = Color.GreenYellow;
                                cell.Font.Bold = true;
                                cell.VerticalAlign = VerticalAlign.Middle;

                                if (j == 0)
                                {
                                    cell.ColumnSpan = 9;
                                    cell.HorizontalAlign = HorizontalAlign.Center;
                                }

                                if (j >= 1)
                                    cell.HorizontalAlign = HorizontalAlign.Right;

                                cell.Text = data[j];
                                row2.Cells.Add(cell);
                            }

                            table.Rows.Add(row2);

                        }


                    }
                    table.RenderControl(htmlWriter);
                    _exportContent = sb.ToString();
                }
            }
            return _exportContent;
        }

        private string ExportToExcel_TongHopGiaoHang(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string nvbhId = "";
            if (context.Request.QueryString["nvbhId"] != null)
                nvbhId = context.Request.QueryString["nvbhId"].ToString();

            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();
           
            string trangthai = "";
            if (context.Request.QueryString["trangthai"] != null)
                trangthai = context.Request.QueryString["trangthai"].ToString();

            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();

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
                    header.ColumnSpan = 11;
                    header.Text = "TỔNG HỢP GIAO HÀNG";
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

                    if (tungay.Trim().Length > 0 && denngay.Trim().Length > 0)
                        header.Text = "Thời gian: từ    " + tungay + " đến " + denngay;
                    else
                        header.Text = "Thời gian tạo:   "  + ngaythang;
                    header.Font.Bold = true;
                    header.ColumnSpan = 11;
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


                    string[] tieude = new string[] { " Warehouse ", "Đặt hàng", "Trạng thái", "Số hoá đơn", "Số chứng từ", "Đơn hàng",
                        "Customer code", "Customer", "Shipping address", 
                        "Shipping address", "Quantity đặt", "Quantity giao" };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;
                        header.BackColor = Color.Blue;
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();

                    string sql = "";

                    string condition = "";
                    if (tungay.Trim().Length > 0)
                        condition += " and convert(datetime, dh.ngaydathang, 105) >= convert( datetime, '" + tungay + "', 105) ";
                    if (denngay.Trim().Length > 0)
                        condition += " and convert(datetime, dh.ngaydathang, 105) <= convert( datetime, '" + denngay + "', 105) ";
                    if (kho.Trim().Length > 3)
                        condition += " and dh.kho_fk = '" + kho + "' ";
                    if (khachhang.Trim().Length > 3)
                        condition += " and dh.khachhangnhan_fk = '" + khachhang + "' ";

                    sql = " SELECT ISNULL((SELECT makho FROM Kho WHERE pk_seq = dh.kho_fk), '') kho, kh.ma, kh.hoten, ISNULL(dh.diachigiaohang, kh.diachi) diachigiaohang, dh.pk_seq AS dathang, dh.sochungtu, dh.sohoadon, " +
                    "   ISNULL((SELECT pk_seq FROM DonHang WHERE dathang_fk = dh.pk_seq), 0) donhang, " + 
                    " 	SUM(dh_ct.soluongQuyDoi) soluongdat, ISNULL(SUM(dh_gh.soluongQuyDoi), 0) soluonggiao, ISNULL(SUM(dh_gh.soluongQuyDoi * dh_gh.dongia), 0) thanhtien,  " +
                    " CASE dh.trangthai WHEN 0 THEN N'Đang xử lý' " +
                    "                   WHEN 1 THEN N'Đã xử lý' " +
                    "                   WHEN 2 THEN N'Đã huỷ' " +
                    "                   WHEN 3 THEN N'Đã lập phiếu' " +
                    "                   WHEN 4 THEN N'Đã chuyển SO' " +
                    "                   WHEN 5 THEN N'Hoàn thành' END trangthai " +
                    " FROM KhachHang kh INNER JOIN  DatHang dh ON kh.pk_seq = dh.khachhang_fk " + condition +
                    " 	INNER JOIN DatHang_SanPham dh_ct ON dh.pk_seq = dh_ct.dathang_fk" +
                    " 	LEFT JOIN DatHang_SanPham_GiaoHang dh_gh ON dh_ct.dathang_fk = dh_gh.dathang_fk AND dh_ct.sanpham_fk = dh_gh.sanpham_fk  " +
                    " WHERE kh.trangthai = 1 and dh.trangthai NOT IN (2) " +
                    " GROUP BY dh.kho_fk, kh.ma, kh.hoten, dh.diachigiaohang, kh.diachi, dh.pk_seq, dh.trangthai, dh.sochungtu, dh.sohoadon ";
                    

                    DataTable dt = xl.ReadTable(sql);
                    
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();
                        
                        string makho = dt.Rows[i]["kho"].ToString();
                        string dathang = dt.Rows[i]["dathang"].ToString();
                        string donhang = dt.Rows[i]["donhang"].ToString();
                        string sohoadon = dt.Rows[i]["sohoadon"].ToString();
                        string sochungtu = dt.Rows[i]["sochungtu"].ToString();
                        trangthai = dt.Rows[i]["trangthai"].ToString();
                        string makh = dt.Rows[i]["ma"].ToString();
                        khachhang = dt.Rows[i]["hoten"].ToString();
                        string diachigiaohang = dt.Rows[i]["diachigiaohang"].ToString();
                        string soluongdat = dt.Rows[i]["soluongdat"].ToString();
                        string soluonggiao = dt.Rows[i]["soluonggiao"].ToString();
                        string thanhtien = dt.Rows[i]["thanhtien"].ToString();

                        string[] data = new string[] { makho, dathang, trangthai, sohoadon, sochungtu, donhang, makh, khachhang, diachigiaohang,
                            FormatString.ForMatNumber(soluongdat.ToString()), FormatString.ForMatNumber(soluonggiao.ToString())};

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            cell.VerticalAlign = VerticalAlign.Middle;

                            if (j == 7)
                            {
                                cell.Width = Unit.Parse("400");
                                cell.HorizontalAlign = HorizontalAlign.Left;
                            }
                            else if (j == 8)
                            {
                                cell.Width = Unit.Parse("500");
                                cell.HorizontalAlign = HorizontalAlign.Left;
                            }
                            else if (j == 0)
                            {
                                cell.Width = Unit.Parse("90");
                                cell.HorizontalAlign = HorizontalAlign.Center;
                            }
                            else if (j == 6)
                            {
                                cell.Width = Unit.Parse("120");
                                cell.HorizontalAlign = HorizontalAlign.Center;
                            }
                            else
                            {
                                cell.Width = Unit.Parse("100");                                    
                            }
                            
                            if(j > 8)
                                cell.HorizontalAlign = HorizontalAlign.Right;

                            cell.Text = data[j];

                            row.Cells.Add(cell);

                        }

                        table.Rows.Add(row);


                        ////THEM TOTAL THEO NGAY
                        //bool addNGAY = false;
                        //if (i < dt.Rows.Count - 1)
                        //{
                        //    if (!dt.Rows[i]["ngaydonhang"].ToString().Equals(dt.Rows[i + 1]["ngaydonhang"].ToString()))
                        //        addNGAY = true;
                        //}
                        //else
                        //    addNGAY = true;

                        //if (addNGAY)
                        //{
                        //    string ngay = dt.Rows[i]["ngaydonhang"].ToString();

                        //    //VE ROW NEW
                        //    TableRow row2 = new TableRow();
                        //    data = new string[] { ngay + " Tổng cộng ", FormatString.ForMatNumber(totalTONGTIEN_NGAY.ToString()), "" };

                        //    for (int jj = 0; jj < data.Length; jj++)
                        //    {
                        //        TableCell cell = new TableCell();
                        //        cell.BackColor = Color.LightGray;
                        //        cell.Font.Bold = true;
                        //        cell.VerticalAlign = VerticalAlign.Middle;

                        //        if (jj == 0)
                        //        {
                        //            cell.ColumnSpan = 12;
                        //            cell.HorizontalAlign = HorizontalAlign.Center;
                        //        }

                        //        if (jj >= 1)
                        //            cell.HorizontalAlign = HorizontalAlign.Right;

                        //        cell.Text = data[jj];
                        //        row2.Cells.Add(cell);
                        //    }

                        //    table.Rows.Add(row2);

                        //    totalTONGTIEN_NGAY = 0;

                        //}


                        //bool addTOTAL = false;
                        //if (i == dt.Rows.Count - 1)
                        //{
                        //    addTOTAL = true;
                        //}

                        //if (addTOTAL)
                        //{
                        //    //VE ROW NEW
                        //    TableRow row2 = new TableRow();
                        //    data = new string[] { " TỔNG CỘNG", FormatString.ForMatNumber(totalTONGTIEN.ToString()), "" };

                        //    for (int j = 0; j < data.Length; j++)
                        //    {
                        //        TableCell cell = new TableCell();
                        //        cell.BackColor = Color.GreenYellow;
                        //        cell.Font.Bold = true;
                        //        cell.VerticalAlign = VerticalAlign.Middle;

                        //        if (j == 0)
                        //        {
                        //            cell.ColumnSpan = 12;
                        //            cell.HorizontalAlign = HorizontalAlign.Center;
                        //        }

                        //        if (j >= 1)
                        //            cell.HorizontalAlign = HorizontalAlign.Right;

                        //        cell.Text = data[j];
                        //        row2.Cells.Add(cell);
                        //    }

                        //    table.Rows.Add(row2);

                        //}


                    }
                    table.RenderControl(htmlWriter);
                    _exportContent = sb.ToString();
                }
            }
            return _exportContent;
        }

        private string ExportToExcel_ChiTietGiaoHang(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string nvbhId = "";
            if (context.Request.QueryString["nvbhId"] != null)
                nvbhId = context.Request.QueryString["nvbhId"].ToString();

            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            string trangthai = "";
            if (context.Request.QueryString["trangthai"] != null)
                trangthai = context.Request.QueryString["trangthai"].ToString();

            string nhanhang = "";
            if (context.Request.QueryString["nhanhang"] != null)
                nhanhang = context.Request.QueryString["nhanhang"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();
            
            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();

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
                    header.Text = "Detail GIAO HÀNG";
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

                    if (tungay.Trim().Length > 0 && denngay.Trim().Length > 0)
                        header.Text = "Thời gian: từ    " + tungay + " đến " + denngay;
                    else
                        header.Text = "Thời gian tạo:   "  + ngaythang;
                    header.Font.Bold = true;
                    header.ColumnSpan = 9;
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


                    string[] tieude = new string[] { " Warehouse ", "Customer code", "Customer", "Đia chỉ giao hàng", 
                        "Số đơn hàng", "Số hoá đơn", "Số chứng từ", "Ngày đặt", "Ngày yêu cầu", "Đơn hàng", 
                        "PartCode", "Sản phẩm", "Unit", "Quantity đặt", "Quantity giao", "HSD"};

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;
                        header.BackColor = Color.Blue;
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();

                    string sql = "";

                    string condition = "";
                    string conditionSP = "";
                    if (tungay.Trim().Length > 0)
                        condition += " and convert( datetime, dh.ngaydathang, 105) >= convert( datetime, '" + tungay + "', 105) ";
                    if (denngay.Trim().Length > 0)
                        condition += " and convert( datetime, dh.ngaydathang, 105) <= convert( datetime, '" + denngay + "', 105) ";
                    if (kho.Trim().Length > 3)
                        condition += " and dh.kho_fk = '" + kho + "' ";
                    if (khachhang.Trim().Length > 0)
                        condition += " and dh.khachhangnhan_fk = '" + khachhang + "' ";
                    if (trangthai.Trim().Length > 0)
                    {
                        if (int.Parse(trangthai) < 10)
                            condition += " and dh.trangthai in (" + trangthai + ") ";
                    }


                    if (chungloai.Trim().Length > 3)
                        conditionSP += " and sp.chungloai_fk = '" + chungloai + "' ";
                    if (nhanhang.Trim().Length > 3)
                        conditionSP += " and sp.nhanhang_fk = '" + nhanhang + "' ";
                    
                    sql = " SELECT ISNULL((SELECT makho FROM Kho WHERE pk_seq = dh.kho_fk), '') AS kho, dh.pk_seq as sodathang, dh.sochungtu, dh.sohoadon, dh.ngaydathang, dh.ngaygiaohang, " + 
                    "       kh.ma, kh.hoten, ISNULL(dh.diachigiaohang, kh.diachi) diachigiaohang, " +
                    "       CASE dh.trangthai WHEN 0 THEN N'Đang xử lý' " +
                    "                         WHEN 1 THEN N'Đã xử lý' " +
                    "                         WHEN 2 THEN N'Đã huỷ' " +
                    "                         WHEN 3 THEN N'Đang lập phiếu' " +
                    "                         WHEN 4 THEN N'Đã chuyển SO' " +
                    "                         WHEN 5 THEN N'Đã giao' ELSE N'Không xác định' END trangthai, " +
                    "       ISNULL((SELECT pk_seq FROM DonHang WHERE dathang_fk = dh.pk_seq), 0) donhang, " +
                    "       sp.pk_seq as spId, sp.ma AS masp, sp.ten AS tensp, ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = dh_ct.dvt_fk), '') AS donvi,  " +
                    " 	    dh_gh.solo, SUM(dh_ct.soluongQuyDoi) as soluongdat, ISNULL((SUM(dh_gh.soluongQuyDoi)), 0) soluonggiao  " +
                    " 	FROM DatHang dh INNER JOIN  KhachHang kh  ON kh.pk_seq = dh.khachhang_fk   " + condition +
                    " 		INNER JOIN  DatHang_SanPham dh_ct ON dh.pk_seq = dh_ct.dathang_fk " +
                    " 		INNER JOIN  DatHang_SanPham_GiaoHang dh_gh ON dh_ct.dathang_fk = dh_gh.dathang_fk AND dh_ct.sanpham_fk = dh_gh.sanpham_fk " +
                    " 		INNER JOIN  SanPham sp ON dh_ct.sanpham_fk = sp.pk_seq   " + conditionSP +
                    " 	WHERE kh.trangthai = 1 and dh.trangthai NOT IN ( 2 )  " +
                    "  GROUP BY dh.kho_fk, dh.pk_seq, dh.ngaydathang, dh.ngaygiaohang, dh.trangthai, kh.ma, kh.hoten, sp.pk_seq, sp.ma, sp.ten, dh_ct.dvt_fk, dh.sochungtu, dh.sohoadon, dh.diachigiaohang, kh.diachi, dh_gh.solo " +
                    " ORDER BY  dh.sochungtu, dh.pk_seq, sp.ma ";
                                                
                    DataTable dt = xl.ReadTable(sql);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string makho = dt.Rows[i]["kho"].ToString();                       
                        string sodathang = dt.Rows[i]["sodathang"].ToString();
                        string sohoadon = dt.Rows[i]["sohoadon"].ToString();
                        string sochungtu = dt.Rows[i]["sochungtu"].ToString();                        
                        string donhang = dt.Rows[i]["donhang"].ToString();
                        string ngaydathang = dt.Rows[i]["ngaydathang"].ToString();
                        string ngaygiaohang = dt.Rows[i]["ngaygiaohang"].ToString() ;
                        trangthai = dt.Rows[i]["trangthai"].ToString();
                        string makh = dt.Rows[i]["ma"].ToString();
                        khachhang = dt.Rows[i]["hoten"].ToString();
                        string diachigiaohang = dt.Rows[i]["diachigiaohang"].ToString();
                        string masp = dt.Rows[i]["masp"].ToString();
                        string tensp = dt.Rows[i]["tensp"].ToString();
                        string donvi = dt.Rows[i]["donvi"].ToString();
                        string soluongdat = dt.Rows[i]["soluongdat"].ToString();
                        string soluonggiao = dt.Rows[i]["soluonggiao"].ToString();
                        string solo = dt.Rows[i]["solo"].ToString() ;

                        string[] data = new string[] { makho, makh, khachhang, diachigiaohang, 
                            sodathang, sohoadon, sochungtu, ngaydathang, ngaygiaohang, donhang, masp, tensp, donvi,
                            FormatString.ForMatNumber(soluongdat.ToString()), FormatString.ForMatNumber(soluonggiao.ToString()), solo };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            cell.VerticalAlign = VerticalAlign.Middle;

                            if(j == 0)
                                cell.Width = Unit.Parse("90");
                            else if (j == 1 || j == 10)
                            {
                                cell.Width = Unit.Parse("120");                                
                            }
                            else if(j == 2 || j == 3|| j == 11)
                            {
                                cell.Width = Unit.Parse("400");
                            }                           
                            else
                                cell.Width = Unit.Parse("100");

                            if (j > 12)
                                cell.HorizontalAlign = HorizontalAlign.Right;
                            cell.Text = data[j];

                            row.Cells.Add(cell);

                        }

                        table.Rows.Add(row);


                        ////THEM TOTAL THEO NGAY
                        //bool addNGAY = false;
                        //if (i < dt.Rows.Count - 1)
                        //{
                        //    if (!dt.Rows[i]["ngaydonhang"].ToString().Equals(dt.Rows[i + 1]["ngaydonhang"].ToString()))
                        //        addNGAY = true;
                        //}
                        //else
                        //    addNGAY = true;

                        //if (addNGAY)
                        //{
                        //    string ngay = dt.Rows[i]["ngaydonhang"].ToString();

                        //    //VE ROW NEW
                        //    TableRow row2 = new TableRow();
                        //    data = new string[] { ngay + " Tổng cộng ", FormatString.ForMatNumber(totalTONGTIEN_NGAY.ToString()), "" };

                        //    for (int jj = 0; jj < data.Length; jj++)
                        //    {
                        //        TableCell cell = new TableCell();
                        //        cell.BackColor = Color.LightGray;
                        //        cell.Font.Bold = true;
                        //        cell.VerticalAlign = VerticalAlign.Middle;

                        //        if (jj == 0)
                        //        {
                        //            cell.ColumnSpan = 12;
                        //            cell.HorizontalAlign = HorizontalAlign.Center;
                        //        }

                        //        if (jj >= 1)
                        //            cell.HorizontalAlign = HorizontalAlign.Right;

                        //        cell.Text = data[jj];
                        //        row2.Cells.Add(cell);
                        //    }

                        //    table.Rows.Add(row2);

                        //    totalTONGTIEN_NGAY = 0;

                        //}


                        //bool addTOTAL = false;
                        //if (i == dt.Rows.Count - 1)
                        //{
                        //    addTOTAL = true;
                        //}

                        //if (addTOTAL)
                        //{
                        //    //VE ROW NEW
                        //    TableRow row2 = new TableRow();
                        //    data = new string[] { " TỔNG CỘNG", FormatString.ForMatNumber(totalTONGTIEN.ToString()), "" };

                        //    for (int j = 0; j < data.Length; j++)
                        //    {
                        //        TableCell cell = new TableCell();
                        //        cell.BackColor = Color.GreenYellow;
                        //        cell.Font.Bold = true;
                        //        cell.VerticalAlign = VerticalAlign.Middle;

                        //        if (j == 0)
                        //        {
                        //            cell.ColumnSpan = 12;
                        //            cell.HorizontalAlign = HorizontalAlign.Center;
                        //        }

                        //        if (j >= 1)
                        //            cell.HorizontalAlign = HorizontalAlign.Right;

                        //        cell.Text = data[j];
                        //        row2.Cells.Add(cell);
                        //    }

                        //    table.Rows.Add(row2);

                        //}


                    }
                    table.RenderControl(htmlWriter);
                    _exportContent = sb.ToString();
                }
            }
            return _exportContent;
        }

        private string DatHang_ID_EXCEL(HttpContext context)
        {
            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string ngaythang = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();

            string _exportContent = "";
            using (StringWriter sb = new StringWriter())
            {
                using (HtmlTextWriter htmlWriter = new HtmlTextWriter(sb))
                {
                    System.Web.UI.WebControls.Table table = new System.Web.UI.WebControls.Table();
                    //table.GridLines = GridLines.Both;

                    //table.BorderWidth = new Unit(1);

                    //Tieu de
                    TableHeaderCell header = new TableHeaderCell();
                    header.RowSpan = 1;
                    header.ColumnSpan = 7;
                    header.Text = "Honda Logicom Viet Nam Co., ltd";
                    header.Font.Bold = true;
                    header.Font.Size = FontUnit.Point(11);
                    header.Font.Name = "Arial";
                    //header.BackColor = System.Drawing.Color.Red;
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    // Add the header to a new row.
                    TableRow headerRow = new TableRow();
                    headerRow.Cells.Add(header);

                    table.Rows.Add(headerRow);


                    //Tieu de
                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 7;
                    header.Text = " Hoai Nam Building, No. 6, Hai Ba Trung Street, Hung Vuong Ward, Phuc Yen City, Vinh Phuc Province ";
                    header.Font.Bold = false;
                    header.Font.Size = FontUnit.Point(10);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);


                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 8;
                    header.Text = "INFORMATION OF ORDER";
                    header.Font.Bold = true;
                    header.Font.Size = FontUnit.Point(15);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();
                    string query = "SELECT a.ngaydathang, a.ngaygiaohang, ISNULL(a.ghichu, '') ghichu, a.trangthai, " +
                        "    CASE a.loaikho WHEN 0 THEN ISNULL((SELECT ma FROM KhachHang WHERE pk_seq = a.khachhang_fk), '') " +
                        "                    ELSE ISNULL((SELECT makho FROM Kho WHERE pk_seq = a.khonhan_fk), '') END khonhan, " +
                        " ISNULL((SELECT makho FROM Kho WHERE pk_seq = a.kho_fk), '') khoxuat, " +
                        "  	CASE a.loaixuat WHEN 1 THEN N'Local' " +
                        "  				 WHEN 2 THEN N'Partner' " +
                        "  				 ELSE N'Unknow' END tenloaixuat " +
                        " FROM DatHang a " +                   
                        " WHERE a.pk_seq = '" + id + "'";
                    DataTable dtINFO = xl.ReadTable(query);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 7;
                    header.Text = " ID: " + id + "  - Date: " + dtINFO.Rows[0]["ngaydathang"].ToString() + "  - Delivery date: " + dtINFO.Rows[0]["ngaygiaohang"].ToString();
                    header.Font.Italic = true;
                    header.Font.Bold = false;
                    header.Font.Size = FontUnit.Point(10);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    //Chen 2 row khoang cach
                    for (int i = 0; i < 1; i++)
                    {
                        TableRow rowS = new TableRow();
                        table.Rows.Add(rowS);
                    }

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 8;
                    header.Text = "From: " + dtINFO.Rows[0]["khoxuat"].ToString();
                    header.Font.Italic = false;
                    header.Font.Bold = true;
                    header.Font.Size = FontUnit.Point(9);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;
                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 8;
                    header.Text = "To: " + dtINFO.Rows[0]["khonhan"].ToString();
                    header.Font.Bold = true;
                    header.Font.Size = FontUnit.Point(10);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);
                    
                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 8;
                    header.Text = "Note: " + dtINFO.Rows[0]["ghichu"].ToString();
                    header.Font.Italic = false;
                    header.Font.Bold = false;
                    header.Font.Size = FontUnit.Point(9);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    string[] tieude = new string[] { "No", "PartCode", "PartName", "Unit", "Quantity" };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;
                        header.BackColor = Color.LightGreen;
                        header.BorderWidth = new Unit(0.5, UnitType.Pixel);
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;
                        header.Font.Size = FontUnit.Point(10);
                        header.Font.Name = "Arial";

                        if (i == 0)
                            header.Width = Unit.Parse("60");
                        else if (i == 2)
                            header.Width = Unit.Parse("300");
                        else
                        {
                            if (i == 1)
                                header.Width = Unit.Parse("150");
                            else
                                header.Width = Unit.Parse("100");
                        }

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);


                    query = " SELECT sp.ma,  sp.ten, dvt.ten AS donvi, b.solo, b.soluong  " +
                    " FROM DatHang a INNER JOIN DatHang_SanPham b on a.pk_seq = b.dathang_fk  AND a.pk_seq = '" + id + "'  " +
                    " 	INNER JOIN SanPham sp on b.sanpham_fk = sp.pk_seq  " +
                    " 	INNER JOIN DonViTinh dvt on b.dvt_fk = dvt.pk_seq  " +
                    " WHERE a.pk_seq = " + id + "  " +
                    " ORDER BY sp.ma ASC ";
                    DataTable dt = xl.ReadTable(query);
                    
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                       
                        string masp = dt.Rows[i]["ma"].ToString();                       
                        string tensp = dt.Rows[i]["ten"].ToString();
                        string donvi = dt.Rows[i]["donvi"].ToString();                        
                        string soluong = dt.Rows[i]["soluong"].ToString();
                        
                        string[] data = new string[] { (i + 1).ToString(), masp, tensp, donvi, FormatString.ForMatNumber(soluong) };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();

                            if (j == 7)
                                cell.HorizontalAlign = HorizontalAlign.Right;

                            if (j == 0 || j == 1 || j == 3 || j == 4 || j == 5)
                                cell.HorizontalAlign = HorizontalAlign.Center;

                            cell.Text = data[j];
                            cell.Font.Size = FontUnit.Point(10);
                            cell.Font.Name = "Arial";

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

        private string StockOut_ID_EXCEL(HttpContext context)
        {
            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string _exportContent = "";

            DataTable baocao = new DataTable("ReportStockOut_ID");

            //baocao.Columns.Add("Customer", typeof(string));
            //baocao.Columns.Add("Model", typeof(string));
            //baocao.Columns.Add("Grade", typeof(string));
            //baocao.Columns.Add("Vin", typeof(string));
            //baocao.Columns.Add("Frame", typeof(string));
            //baocao.Columns.Add("Engine", typeof(string));
            //baocao.Columns.Add("Color", typeof(string));
            //baocao.Columns.Add("Location", typeof(string));
            //baocao.Columns.Add("Pickup", typeof(string));
            //baocao.Columns.Add("PPO", typeof(string));
            //baocao.Columns.Add("Finish", typeof(string));
            //baocao.Columns.Add("Delivery", typeof(string));

            baocao.Columns.Add("", typeof(string));
            baocao.Columns.Add("", typeof(string));
            baocao.Columns.Add("", typeof(string));
            baocao.Columns.Add("", typeof(string));
            baocao.Columns.Add("", typeof(string));
            baocao.Columns.Add("", typeof(string));
            baocao.Columns.Add("", typeof(string));
            baocao.Columns.Add("", typeof(string));
            baocao.Columns.Add("", typeof(string));
            baocao.Columns.Add("", typeof(string));
            baocao.Columns.Add("", typeof(string));
            baocao.Columns.Add("", typeof(string));
            
            string sql = "";
            ExecuteData xl = new ExecuteData();

            if (id.Length < 3)
                id = "0";

            sql = "SELECT sp.ma AS masp, sp.codeEnglish, sp.ten AS tensp, " + 
                "   dh_sp.solo, dh_sp.plNo, dh_sp.cartonNo, dh_sp.lotNoOrder, dh_sp.lotNo, dh_sp.sewingNo, dh_sp.mavach, (dh_sp.soluong) AS soluong, " +                 
                 "	ISNULL((SELECT ten FROM Size WHERE pk_seq = dh_sp.mausac_fk), '') AS mausac, " +
                 "	ISNULL((SELECT ten FROM Pallet WHERE pk_seq = dh_sp.pallet_fk), '') AS pallet, " +
                 "	ISNULL((SELECT ten FROM Location WHERE pk_seq = dh_sp.location_fk), '') AS location " +                 
                 "FROM DonHang dh INNER JOIN DonHang_SanPham_ChiTiet dh_sp ON dh.pk_seq = dh_sp.donhang_fk AND dh_sp.donhang_fk = '" + id + "' " +
                 " INNER JOIN SanPham sp on dh_sp.sanpham_fk = sp.pk_seq  " +
                 "WHERE dh_sp.pk_seq > 0 " +
                 //" ORDER BY CONVERT(datetime, scantime3, 105) ASC ";
                 " ORDER BY sp.codeEnglish, dh_sp.solo, dh_sp.mausac_fk, dh_sp.sewingNo, dh_sp.lotNoOrder, dh_sp.plNo, dh_sp.cartonNo, dh_sp.lotNo, dh_sp.location_fk, dh_sp.pallet_fk ASC ";

            DataTable dt = xl.ReadTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {              
                string masp = dt.Rows[i]["masp"].ToString();
                string codeEnglish = dt.Rows[i]["codeEnglish"].ToString();
                string solo = dt.Rows[i]["solo"].ToString();
                string mausac = dt.Rows[i]["mausac"].ToString();
                string sewingNo = dt.Rows[i]["sewingNo"].ToString();                
                string lotNoOrder = dt.Rows[i]["lotNoOrder"].ToString();

                string plNo = dt.Rows[i]["plNo"].ToString();
                string cartonNo = dt.Rows[i]["cartonNo"].ToString();
                string lotNo = dt.Rows[i]["lotNo"].ToString();

                string location = dt.Rows[i]["location"].ToString();
                string pallet = dt.Rows[i]["pallet"].ToString();

                string soluong = dt.Rows[i]["soluong"].ToString();

                DataRow dr = baocao.NewRow();

                dr[0] = codeEnglish;
                dr[1] = masp;
                dr[2] = solo;
                dr[3] = mausac;
                dr[4] = sewingNo;                
                dr[5] = lotNoOrder;
                dr[6] = plNo;
                dr[7] = cartonNo;
                dr[8] = lotNo;
                dr[9] = location;
                dr[10] = pallet;
                dr[11] = FormatString.ForMatNumber(soluong);

                baocao.Rows.Add(dr);
            }

            string fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\ReportStockOut_ID.xlsm";

            Workbook workbook = new Workbook(fileName);
            Worksheet worksheet = workbook.Worksheets[0];

            worksheet.Cells.ImportDataTable(baocao, true, "A9");
            worksheet.AutoFitColumns();


            sql = "SELECT a.ngaygiaohang, ISNULL(k.makho, '') makho, ISNULL(kh.ma, '') khachhang, " +
            "  ISNULL(a.sophieu, '') sophieu, ISNULL(a.ghichu, '') ghichu " +
            " FROM DonHang a INNER JOIN kho k ON a.kho_fk = k.pk_seq " +
            " LEFT JOIN khachhang kh ON a.khachhang_fk = kh.pk_seq " +
            " WHERE a.pk_seq = '" + id + "'";
            dt = xl.ReadTable(sql);
            if (dt.Rows.Count > 0)
            {
                string ngaygiaohang = dt.Rows[0]["ngaygiaohang"].ToString();
                //string makho = dt.Rows[0]["makho"].ToString();
                string khachhang = dt.Rows[0]["khachhang"].ToString();
                //string sohopdong = dt.Rows[0]["sohopdong"].ToString();
                string sophieu = dt.Rows[0]["sophieu"].ToString();
                //string loainhap = dt.Rows[0]["loainhap"].ToString();
                string ghichu = dt.Rows[0]["ghichu"].ToString();

                worksheet.Cells["A5"].PutValue("No: " + sophieu + " - Date: " + ngaygiaohang);
                //worksheet.Cells["A6"].PutValue("Customer: ");
                //worksheet.Cells["F6"].PutValue(khachhang);

                //worksheet.Cells["A7"].PutValue("Option: ");
                //worksheet.Cells["F7"].PutValue(loainhap);

                worksheet.Cells["A6"].PutValue("Note: ");
                worksheet.Cells["F6"].PutValue(ghichu);

            }

            HttpResponse response = context.Response;
            workbook.Save(response, "ReportStockOut_ID.xlsm", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsm));

            return _exportContent;
        }

        private string DieuChuyen_ID_EXCEL(HttpContext context)
        {
            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string ngaythang = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();

            string _exportContent = "";
            using (StringWriter sb = new StringWriter())
            {
                using (HtmlTextWriter htmlWriter = new HtmlTextWriter(sb))
                {
                    System.Web.UI.WebControls.Table table = new System.Web.UI.WebControls.Table();
                    //table.GridLines = GridLines.Both;

                    //table.BorderWidth = new Unit(1);

                    //Tieu de
                    TableHeaderCell header = new TableHeaderCell();
                    header.RowSpan = 1;
                    header.ColumnSpan = 7;
                    header.Text = "Honda Logicom Viet Nam Co., ltd";
                    header.Font.Bold = true;
                    header.Font.Size = FontUnit.Point(11);
                    header.Font.Name = "Arial";
                    //header.BackColor = System.Drawing.Color.Red;
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    // Add the header to a new row.
                    TableRow headerRow = new TableRow();
                    headerRow.Cells.Add(header);

                    table.Rows.Add(headerRow);


                    //Tieu de
                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 7;
                    header.Text = " Hoai Nam Building, No. 6, Hai Ba Trung Street, Hung Vuong Ward, Phuc Yen City, Vinh Phuc Province ";
                    header.Font.Bold = false;
                    header.Font.Size = FontUnit.Point(10);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);


                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 7;
                    header.Text = "THÔNG TIN XUẤT HÀNG";
                    header.Font.Bold = true;
                    header.Font.Size = FontUnit.Point(15);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();
                    string query = "SELECT a.ngaynhap, ISNULL(b.makho, '') AS kho, a.pk_seq AS sohoadon, ISNULL(a.ghichu, '') ghichu " +
                                   " FROM DoiLoBin a INNER JOIN Kho b on a.kho_fk = b.pk_seq  AND a.pk_seq = '" + id + "' " +
                                   " WHERE a.pk_seq = '" + id + "'";
                    DataTable dtINFO = xl.ReadTable(query);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 7;
                    header.Text = " Số điều chuyển: " + id + "  - Ngày điều chuyển: " + dtINFO.Rows[0]["ngaynhap"].ToString();
                    header.Font.Italic = true;
                    header.Font.Bold = false;
                    header.Font.Size = FontUnit.Point(10);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    //Chen 2 row khoang cach
                    for (int i = 0; i < 1; i++)
                    {
                        TableRow rowS = new TableRow();
                        table.Rows.Add(rowS);
                    }

                    //headerRow = new TableRow();
                    //header = new TableHeaderCell();
                    //header.ColumnSpan = 3;
                    //header.Text = "Siêu thị nhập: " + dtINFO.Rows[0]["cuahang"].ToString();
                    //header.Font.Italic = false;
                    //header.Font.Bold = true;
                    //header.Font.Size = FontUnit.Point(9);
                    //header.Font.Name = "Arial";
                    //header.HorizontalAlign = HorizontalAlign.Left;
                    //header.VerticalAlign = VerticalAlign.Middle;

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 7;
                    header.Text = "Kho: " + dtINFO.Rows[0]["kho"].ToString();
                    header.Font.Bold = true;
                    header.Font.Size = FontUnit.Point(10);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 7;
                    header.Text = "Ghi chú: " + dtINFO.Rows[0]["ghichu"].ToString();
                    header.Font.Italic = false;
                    header.Font.Bold = false;
                    header.Font.Size = FontUnit.Point(9);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    string[] tieude = new string[] { "STT", "Items", "Name Items", "Unit", "Location", "Location mới", "Quantity" };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;
                        header.BackColor = Color.LightGreen;
                        header.BorderWidth = new Unit(0.5, UnitType.Pixel);
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;
                        header.Font.Size = FontUnit.Point(10);
                        header.Font.Name = "Arial";

                        if (i == 2)
                            header.Width = Unit.Parse("450");
                        else
                        {
                            if (i == 1)
                                header.Width = Unit.Parse("150");
                            else if (i == 0)
                                header.Width = Unit.Parse("60");
                            else
                                header.Width = Unit.Parse("100");
                        }

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);


                    query = " SELECT sp.ma, sp.ten, dvt.ten AS donvi, b.solo, ISNULL(loc.ma, ' ') AS location, b.soluong,   " +
                        "  ISNULL((SELECT ma FROM Location WHERE pk_seq = b.location_new_fk), '') locationNEW " +
                    " FROM DoiLoBin a INNER JOIN DoiLoBin_SanPham_ChiTiet b on a.pk_seq = b.doilobin_fk  AND a.pk_seq = '" + id + "'  " +
                    " 	INNER JOIN SanPham sp on b.sanpham_fk = sp.pk_seq  " +
                    " 	INNER JOIN DonViTinh dvt on b.dvt_fk = dvt.pk_seq  " +
                    " 	LEFT JOIN Location loc ON b.location_fk = loc.pk_seq  " +
                    " WHERE a.pk_seq = " + id + "  " +
                    " ORDER BY sp.ma, loc.ma ASC ";
                    DataTable dt = xl.ReadTable(query);

                    double totalTIENHANG = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string masp = dt.Rows[i]["ma"].ToString();
                        string tensp = dt.Rows[i]["ten"].ToString();
                        string donvi = dt.Rows[i]["donvi"].ToString();
                        //string solo = dt.Rows[i]["solo"].ToString();
                        string location = dt.Rows[i]["location"].ToString();
                        string locationNEW = dt.Rows[i]["locationNEW"].ToString();
                        string soluong = dt.Rows[i]["soluong"].ToString();

                        string[] data = new string[] { (i + 1).ToString(), masp, tensp, donvi, location, locationNEW, FormatString.ForMatNumber(soluong) };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();

                            if (j == 10)
                                cell.HorizontalAlign = HorizontalAlign.Right;

                            if (j == 0 || j == 3 || j == 4 || j == 5)
                                cell.HorizontalAlign = HorizontalAlign.Center;

                            cell.Text = data[j];
                            cell.Font.Size = FontUnit.Point(10);
                            cell.Font.Name = "Arial";

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

        private string XuatKhac_ID_EXCEL(HttpContext context)
        {
            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string ngaythang = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();

            string _exportContent = "";
            using (StringWriter sb = new StringWriter())
            {
                using (HtmlTextWriter htmlWriter = new HtmlTextWriter(sb))
                {
                    System.Web.UI.WebControls.Table table = new System.Web.UI.WebControls.Table();
                    //table.GridLines = GridLines.Both;

                    //table.BorderWidth = new Unit(1);

                    //Tieu de
                    TableHeaderCell header = new TableHeaderCell();
                    header.RowSpan = 1;
                    header.ColumnSpan = 6;
                    header.Text = "Honda Logicom Viet Nam Co., ltd";
                    header.Font.Bold = true;
                    header.Font.Size = FontUnit.Point(11);
                    header.Font.Name = "Arial";
                    //header.BackColor = System.Drawing.Color.Red;
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    // Add the header to a new row.
                    TableRow headerRow = new TableRow();
                    headerRow.Cells.Add(header);

                    table.Rows.Add(headerRow);


                    //Tieu de
                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 6;
                    header.Text = "Hoai Nam Building, No. 6, Hai Ba Trung Street, Hung Vuong Ward, Phuc Yen City, Vinh Phuc Province";
                    header.Font.Bold = false;
                    header.Font.Size = FontUnit.Point(10);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);


                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 6;
                    header.Text = "INFORMATION EXPORT";
                    header.Font.Bold = true;
                    header.Font.Size = FontUnit.Point(15);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();
                    string query = "SELECT a.ngayxuat, ISNULL(k.makho, '') makho, ISNULL(a.ghichu, '') ghichu,  " +
                        " CASE a.loaikho WHEN 0 THEN ISNULL((SELECT ma FROM KhachHang WHERE pk_seq = a.khachhang_fk), '') " +
                        "                 ELSE ISNULL((SELECT makho FROM Kho WHERE pk_seq = a.khonhan_fk), '') END khonhan, " +
                        " CASE a.loaixuat WHEN 1 THEN N'Local' " +
                        "                 WHEN 2 THEN N'Partner' " +
                        "                 WHEN 3 THEN N'Orther' " +
                        "                 WHEN 4 THEN N'Demolished' ELSE N'Unknow'  END loaixuat " +
                        " FROM XuatKhac a INNER JOIN kho k ON a.kho_fk = k.pk_seq " +
                        " WHERE a.pk_seq = '" + id + "'";
                    DataTable dtINFO = xl.ReadTable(query);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 6;
                    header.Text = " No: " + id + "  - Date: " + dtINFO.Rows[0]["ngayxuat"].ToString();
                    header.Font.Italic = true;
                    header.Font.Bold = false;
                    header.Font.Size = FontUnit.Point(10);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    //Chen 2 row khoang cach
                    for (int i = 0; i < 1; i++)
                    {
                        TableRow rowS = new TableRow();
                        table.Rows.Add(rowS);
                    }

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 6;
                    header.Text = "From: " + dtINFO.Rows[0]["makho"].ToString();
                    header.Font.Italic = false;
                    header.Font.Bold = true;
                    header.Font.Size = FontUnit.Point(10);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 6;
                    header.Text = "To: " + dtINFO.Rows[0]["khonhan"].ToString();
                    header.Font.Italic = false;
                    header.Font.Bold = true;
                    header.Font.Size = FontUnit.Point(10);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 6;
                    header.Text = "Stock out type: " + dtINFO.Rows[0]["loaixuat"].ToString();
                    header.Font.Bold = true;
                    header.Font.Size = FontUnit.Point(10);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 6;
                    header.Text = "Note: " + dtINFO.Rows[0]["ghichu"].ToString();
                    header.Font.Italic = false;
                    header.Font.Bold = false;
                    header.Font.Size = FontUnit.Point(9);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    string[] tieude = new string[] { "No", "Part Code", "Name", "Unit", "Product date", "Lot No", "Serial No", "Quantity" };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;
                        header.BackColor = Color.LightGreen;
                        header.BorderWidth = new Unit(0.5, UnitType.Pixel);
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;
                        header.Font.Size = FontUnit.Point(10);
                        header.Font.Name = "Arial";

                        if (i == 0)
                            header.Width = Unit.Parse("60");
                        else if (i == 1)
                            header.Width = Unit.Parse("150");
                        else
                        {
                            if (i == 2)
                                header.Width = Unit.Parse("350");
                            else if (i == 5)
                                header.Width = Unit.Parse("120");
                            else
                                header.Width = Unit.Parse("90");
                        }

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    query = " SELECT sp.ma, sp.ten, dvt.ten AS donvi, b.solo, b.lotNo, b.serialNo, b.soluong, b.saiso,  " +
                        "   ISNULL(sp.datebanhang, '0') datebanhang, (SELECT DATEDIFF(DAY, CONVERT(datetime, '" + dtINFO.Rows[0]["ngayxuat"].ToString() + "', 105), CONVERT(datetime, b.solo, 105))) AS ngaysudung " +
                        " FROM XuatKhac_SanPham_ChiTiet b INNER JOIN SanPham sp on b.sanpham_fk = sp.pk_seq  AND b.xuatkhac_fk = '" + id + "' " +
                        " 	INNER JOIN DonViTinh dvt on b.dvt_fk = dvt.pk_seq  " +
                        " WHERE b.xuatkhac_fk = " + id + "  " +
                        " ORDER BY sp.ma ASC ";

                    DataTable dt = xl.ReadTable(query);

                    double totalTIENHANG = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string masp = dt.Rows[i]["ma"].ToString();
                        string tensp = dt.Rows[i]["ten"].ToString();
                        string donvi = dt.Rows[i]["donvi"].ToString();
                        string lotNo = dt.Rows[i]["lotNo"].ToString();
                        string serialNo = dt.Rows[i]["serialNo"].ToString();
                        string solo = dt.Rows[i]["solo"].ToString();
                        string soluong = dt.Rows[i]["soluong"].ToString();
                        string saiso = dt.Rows[i]["saiso"].ToString();

                        string pt_hsd = "0";
                        if (dt.Rows[i]["datebanhang"].ToString().Equals("0"))
                        {
                            pt_hsd = "None";
                        }
                        else
                        {
                            pt_hsd = (Math.Round(double.Parse(dt.Rows[i]["ngaysudung"].ToString()) / double.Parse(dt.Rows[i]["datebanhang"].ToString()), 2) * 100).ToString();
                        }

                        string[] data = new string[] { (i + 1).ToString(), masp, tensp, donvi, solo, lotNo, serialNo, FormatString.ForMatNumber(soluong) };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();

                            if (j > 5)
                                cell.HorizontalAlign = HorizontalAlign.Right;

                            if (j == 0 || j == 1 || j == 3 || j == 4)
                                cell.HorizontalAlign = HorizontalAlign.Center;

                            cell.Text = data[j];
                            cell.Font.Size = FontUnit.Point(10);
                            cell.Font.Name = "Arial";

                            row.Cells.Add(cell);
                        }

                        table.Rows.Add(row);
                    }

                    dt.Clear();
                    dt.Clone();
                    dtINFO.Clear();
                    dtINFO.Clone();

                    table.RenderControl(htmlWriter);
                    _exportContent = sb.ToString();
                }
            }
            return _exportContent;
        }

        private string ExportToExcel_FileTemplateStockOut(HttpContext context)
        {

            string _exportContent = "";
            string fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\FileTemplateStockOut.xlsm";
            Workbook workbook = new Workbook(fileName);
            HttpResponse response = context.Response;
            workbook.Save(response, "FileTemplateStockOut.xlsm", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsm));

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