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
using System.Text.RegularExpressions;
using System.Collections;

namespace HLVTimeSheet.Admin.Hander
{
    /// <summary>
    /// Summary description for hdBaoCaoTK
    /// </summary>
    public class hdBaoCaoTK : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;

            string action = "";
            if (context.Request.QueryString["action"] != null)
                action = context.Request.QueryString["action"].ToString();

            if (action.Equals("tonhientai"))
            {
                string solieu = "";
                if (context.Request.QueryString["solieu"] != null)
                    solieu = context.Request.QueryString["solieu"].ToString();

                if (solieu.Equals("1"))
                {
                    string exportContent = ExportToExcel_Inventory_Total(context);
                }
                else if (solieu.Equals("2"))
                {
                    string exportContent = ExportToExcel_Inventory_Color(context);
                }
                else if (solieu.Equals("3"))
                {
                    string exportContent = ExportToExcel_Inventory_Line(context);
                }
                else if (solieu.Equals("4"))
                {
                    string exportContent = ExportToExcel_Inventory_Detail(context);
                }
            }
            else if (action.Equals("tonhientaiPartner"))
            {
                string solieu = "";
                if (context.Request.QueryString["solieu"] != null)
                    solieu = context.Request.QueryString["solieu"].ToString();

                if (solieu.Equals("1"))
                {
                    context.Response.AddHeader("content-disposition", "attachment; filename=Inventory_Partner_Total.xls");
                    context.Response.ContentType = "application/ms-excel";

                    HttpRequest request = context.Request;
                    HttpResponse response = context.Response;
                    string exportContent = ExportToExcel_Inventory_Partner_Total(context);
                    response.Write(exportContent);
                }
                else if (solieu.Equals("2"))
                {
                    context.Response.AddHeader("content-disposition", "attachment; filename=Inventory_Partner_LotNo.xls");
                    context.Response.ContentType = "application/ms-excel";

                    HttpRequest request = context.Request;
                    HttpResponse response = context.Response;
                    string exportContent = ExportToExcel_Inventory_Partner_LotNo(context);
                    response.Write(exportContent);
                }
                else
                {
                    context.Response.AddHeader("content-disposition", "attachment; filename=Inventory_Partner_Detail.xls");
                    context.Response.ContentType = "application/ms-excel";

                    HttpRequest request = context.Request;
                    HttpResponse response = context.Response;
                    string exportContent = ExportToExcel_Inventory_Partner_Detail(context);
                    response.Write(exportContent);
                }
            }
            else if (action.Equals("canhbaotonkho"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=BaoCaoCanhBaoHangTonKho.xls");
                context.Response.ContentType = "application/ms-excel";

                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = ExportToExcel_CANHBAOHANGTONKHO(context);
                response.Write(exportContent);
            }
            else if (action.Equals("nhapxuatton"))
            {
                string solieu = "";
                if (context.Request.QueryString["solieu"] != null)
                    solieu = context.Request.QueryString["solieu"].ToString();

                if (solieu.Equals("1"))
                {
                    context.Response.AddHeader("content-disposition", "attachment; filename=StocInOut_Total.xls");
                    context.Response.ContentType = "application/ms-excel";

                    HttpRequest request = context.Request;
                    HttpResponse response = context.Response;
                    string exportContent = NhapXuatTonTong(context);
                    response.Write(exportContent);
                }
                else if (solieu.Equals("2"))
                {
                    context.Response.AddHeader("content-disposition", "attachment; filename=StockIn_Out_LotNo.xls");
                    context.Response.ContentType = "application/ms-excel";

                    HttpRequest request = context.Request;
                    HttpResponse response = context.Response;
                    string exportContent = ExportToExcel_NXT_LotNo(context);
                    response.Write(exportContent);
                }
                else if (solieu.Equals("3"))
                {
                    context.Response.AddHeader("content-disposition", "attachment; filename=StockIn_Out_Location.xls");
                    context.Response.ContentType = "application/ms-excel";

                    HttpRequest request = context.Request;
                    HttpResponse response = context.Response;
                    string exportContent = ExportToExcel_NXT_Location(context);
                    response.Write(exportContent);
                }
                else if (solieu.Equals("4"))
                {
                    context.Response.AddHeader("content-disposition", "attachment; filename=StockIn_Out_Detail.xls");
                    context.Response.ContentType = "application/ms-excel";

                    HttpRequest request = context.Request;
                    HttpResponse response = context.Response;
                    string exportContent = ExportToExcel_NXT_CHITIET(context);
                    response.Write(exportContent);
                }
            }
            else if (action.Equals("nhapxuattonPartner"))
            {
                string solieu = "";
                if (context.Request.QueryString["solieu"] != null)
                    solieu = context.Request.QueryString["solieu"].ToString();

                if (solieu.Equals("1"))
                {
                    context.Response.AddHeader("content-disposition", "attachment; filename=StocInOutPartner_Total.xls");
                    context.Response.ContentType = "application/ms-excel";

                    HttpRequest request = context.Request;
                    HttpResponse response = context.Response;
                    string exportContent = NhapXuatTonTong_Partner(context);
                    response.Write(exportContent);
                }
                else if (solieu.Equals("2"))
                {
                    context.Response.AddHeader("content-disposition", "attachment; filename=StockInOutPartner_LotNo.xls");
                    context.Response.ContentType = "application/ms-excel";

                    HttpRequest request = context.Request;
                    HttpResponse response = context.Response;
                    string exportContent = ExportToExcel_NXT_LotNo_Partner(context);
                    response.Write(exportContent);
                }
                else if (solieu.Equals("3"))
                {
                    context.Response.AddHeader("content-disposition", "attachment; filename=StockInOutPartner_Detail.xls");
                    context.Response.ContentType = "application/ms-excel";

                    HttpRequest request = context.Request;
                    HttpResponse response = context.Response;
                    string exportContent = ExportToExcel_NXT_CHITIET_Partner(context);
                    response.Write(exportContent);
                }
            }
            else if (action.Equals("tonhientai_HSD"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=BaoCaoTonKho_HanSuDung.xls");
                context.Response.ContentType = "application/ms-excel";

                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = ExportToExcel_Inventory_HSD(context);
                response.Write(exportContent);
            }
            else if (action.Equals("phantichruiro"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=PhanTichRuiRo.xls");
                context.Response.ContentType = "application/ms-excel";

                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = ExportToExcel_PhanTichRuiRo(context);
                response.Write(exportContent);
            }
            else if (action.Equals("baocaothongtinsanpham"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=BaoCaoThongTinSanPham.xls");
                context.Response.ContentType = "application/ms-excel";

                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = BaoCaoThongTinSanPham(context);
                response.Write(exportContent);
            }
            else if (action.Equals("capnhatLocation"))
            {
                string exportContent = ExportToExcel_CapNhatLocation(context);
            }
        }
        private string ExportToExcel_PhanTichRuiRo(HttpContext context)
        {
            string khoId = "";
            if (context.Request.QueryString["khoId"] != null)
                khoId = context.Request.QueryString["khoId"].ToString();

            string nhomhang = "1";
            if (context.Request.QueryString["nhomhang"] != null && context.Request.QueryString["nhomhang"].ToString().Trim().Length > 0)
                nhomhang = context.Request.QueryString["nhomhang"].ToString();

            string nganhhang = "1";
            if (context.Request.QueryString["nganhhang"] != null && context.Request.QueryString["nganhhang"].ToString().Trim().Length > 0)
                nganhhang = context.Request.QueryString["nganhhang"].ToString();

            string locaction = "1";
            if (context.Request.QueryString["locaction"] != null && context.Request.QueryString["locaction"].ToString().Trim().Length > 0)
                locaction = context.Request.QueryString["locaction"].ToString();

            string sanphamId = "1";
            if (context.Request.QueryString["sanphamId"] != null && context.Request.QueryString["sanphamId"].ToString().Trim().Length > 0)
                sanphamId = context.Request.QueryString["sanphamId"].ToString();

            string ppTinhTBban = "3";
            if (context.Request.QueryString["ppTinhTBban"] != null && context.Request.QueryString["ppTinhTBban"].ToString().Trim().Length > 0)
                ppTinhTBban = context.Request.QueryString["ppTinhTBban"].ToString();

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
                    header.Text = "BÁO CÁO PHÂN TÍCH RỦI RO";
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

                    header.Text = "Ngày tạo: " + ngaythang;
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

                    string[] tieude = new string[] { "Mã hàng hóa", "Tên hàng hóa", "Đơn vị tính", "Location", "Bin", "Tồn kho", "Trung bình bán", "Hạn sử dụng", "Số ngày còn lại", "Số lượng rớt hàng" };

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

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();

                    string condition = "  ";
                    if (nhomhang.Trim().Length > 0)
                        condition += " AND sp.nhomsanpham_fk = '" + nhomhang + "' ";
                    if (nganhhang.Trim().Length > 0)
                        condition += " AND sp.nganhhang_fk = '" + nganhhang + "' ";
                    if (sanphamId.Trim().Length > 0)
                        condition += " AND ct.sanpham_fk = '" + sanphamId + "' ";

                    string sql = "";
                    
                    //sql =   " SELECT sp.pk_seq as spId, sp.ma, sp.ten, dv.ten as donvi, ISNULL( xx.ten, '' ) as xuatxu, ISNULL( loc.ten, '' ) as loc, ISNULL( bin.ten, '' ) as bin,   " +
                    //        "  			ISNULL(ct.soluong, 0) as soluong, ISNULL(ct.booked, 0) as booked, ISNULL(ct.avai, 0) as  avai, " +
                    //        " 			ct.solo as hansudung,	ISNULL( dbo.ftGetTrungBinhBan( sp.pk_seq, ct.kho_fk, 3 ), 0 ) as tbBan,  " +
                    //        " 			DATEDIFF(dd, GETDATE(), convert( datetime, ct.solo, 105) ) as songayCONLAI,  " +
                    //        " 			ISNULL( dbo.ftGetTrungBinhBan( sp.pk_seq, ct.kho_fk, 3 ), 0 ) * DATEDIFF(dd, GETDATE(), convert( datetime, ct.solo, 105) ) as dukienBAN " +
                    //        " into #TEMP_ROTHANG " +
                    //        " FROM SANPHAM sp   " +
                    //        "  		INNER JOIN DonViTinh dv on sp.dvt_fk = dv.pk_seq  " +
                    //        "  		INNER JOIN Kho_SanPham_ChiTiet ct on sp.pk_seq = ct.sanpham_fk and ct.kho_fk = '" + khoId + "'  " +
                    //        "  		LEFT JOIN Location loc on ct.location_fk = loc.pk_seq  " +
                    //        "  		LEFT JOIN BIN bin on ct.bin_fk = bin.pk_seq  " +
                    //        "  		LEFT JOIN XUATXU xx on ct.xuatxu_fk = xx.pk_seq  " +
                    //        " WHERE sp.trangthai = 1 " +
                    //        " ORDER BY ten asc, convert( datetime, ct.solo, 105) asc ";
                    //xl.ExecuteNonQuerySQL(sql);

                    //sql =  " SELECT dt.*,  " +
                    //        " 	ISNULL( ( SELECT SUM( dukienBAN ) FROM #TEMP_ROTHANG WHERE spId = dt.spId and convert( datetime, hansudung, 105) < convert( datetime, dt.hansudung, 105) ), 0 ) as bantrongQUAKHU " +
                    //        " FROM #TEMP_ROTHANG dt ";

                    sql = "exec pr_bc_phantichruiro 100000, 1, 3 ";
                    DataTable dt = xl.ReadTable(sql);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        double tonkho = double.Parse(dt.Rows[i]["avai"].ToString());
                        double tbBan = double.Parse(dt.Rows[i]["tbBan"].ToString());
                        string hansudung = dt.Rows[i]["hansudung"].ToString();
                        int songayCONLAI = int.Parse(dt.Rows[i]["songayCONLAI"].ToString());
                        double dukienBAN = double.Parse(dt.Rows[i]["dukienBAN"].ToString());
                        double bantrongQUAKHU = double.Parse(dt.Rows[i]["bantrongQUAKHU"].ToString());

                        double soluongROTHANG = 0;
                        if (tbBan <= 0)
                            soluongROTHANG = tonkho;
                        else if (bantrongQUAKHU >= tonkho)
                            soluongROTHANG = tonkho;
                        else
                            soluongROTHANG = this.TinhSoLuongRotHang(dt, dt.Rows[i]["spId"].ToString(), tonkho, tbBan, songayCONLAI, bantrongQUAKHU, hansudung);

                        if (soluongROTHANG > 0)
                        {
                            string[] data = new string[] { dt.Rows[i]["ma"].ToString(), dt.Rows[i]["ten"].ToString(), dt.Rows[i]["donvi"].ToString(), 
                                                        dt.Rows[i]["loc"].ToString(), dt.Rows[i]["bin"].ToString(),
                                                        FormatString.ForMatNumber(tonkho.ToString()), FormatString.ForMatNumber(tbBan.ToString()), hansudung, songayCONLAI.ToString(), FormatString.ForMatNumber(soluongROTHANG.ToString()) };

                            for (int j = 0; j < data.Length; j++)
                            {
                                TableCell cell = new TableCell();
                                cell.HorizontalAlign = HorizontalAlign.Left;
                                cell.VerticalAlign = VerticalAlign.Middle;

                                if (j == 1 )
                                    cell.Width = Unit.Parse("250");
                                else if (j == 3)
                                    cell.Width = Unit.Parse("160");
                                else if (j == 9)
                                    cell.Width = Unit.Parse("120");
                                else
                                    cell.Width = Unit.Parse("100");

                                if (j == 5 || j == 6 || j == 8 || j == 9)
                                    cell.HorizontalAlign = HorizontalAlign.Right;
                                else if (j == 7)
                                    cell.HorizontalAlign = HorizontalAlign.Center;

                                cell.Text = data[j];

                                row.Cells.Add(cell);
                            }

                            table.Rows.Add(row);
                        }
                    }
                    table.RenderControl(htmlWriter);
                    _exportContent = sb.ToString();

                    //Xóa bảng temp
                    //sql = "If(OBJECT_ID('tempdb..#TEMP_ROTHANG') Is Not Null) Drop Table #TEMP_ROTHANG";
                    //xl.ExecuteNonQuerySQL(sql);

                }
            }
            return _exportContent;
        }
        private double TinhSoLuongRotHang(DataTable dt, string spId, double tonkho, double tbBan, int songayCONLAI, double bantrongQUAKHU, string hansudung)
        {
            double kq = 0;

            //B1. lấy số ngày còn bán được với hạn sử dụng này
            double soluongBAN = songayCONLAI * tbBan;
            
            //B2. Tính số lượng trước ngày hết hạn này sẽ sử dụng ( tồn kho phải giảm lượng hàng này trước, thiếu bao nhiêu mới dùng tiếp )
            double tonkhoCL = tonkho - bantrongQUAKHU;

            kq = tonkhoCL - soluongBAN;

            return kq;
        }
        private string ExportToExcel_NXT_CHITIET(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string khoId = "";
            if (context.Request.QueryString["khoId"] != null)
                khoId = context.Request.QueryString["khoId"].ToString();

            //khoId = khoId.Substring(0, khoId.Length - 1);

            string nganhhangId = "";
            if (context.Request.QueryString["nganhhang"] != null)
                nganhhangId = context.Request.QueryString["nganhhang"].ToString();

            //string theogia = "0";
            //if (context.Request.QueryString["theogia"] != null)
            //    theogia = context.Request.QueryString["theogia"].ToString();

            //string xemtheo = "0";
            //if (context.Request.QueryString["xemtheo"] != null)
            //    xemtheo = context.Request.QueryString["xemtheo"].ToString();

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
                    header.ColumnSpan = 5;
                    header.Text = "REPORT STOCK IN - OUT DETAIL";
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
                    header.ColumnSpan = 5;
                    header.Text = "Date from: " + tungay + " to " + denngay;

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
                    
                    string[] tieude = new string[] { "Warehouse", "Specification", "Part code", "Description", "Unit", "Origin", "Location", "Pro.date", "LotNo", "SerialNo", "Type",
                                                      "Stock closure", "Period " + tungay, "Stock in", "Total", "Stock out", "Total", "Ending stocks " + denngay };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;

                        //if (i == 10) //Tồn đầu
                        //    header.ColumnSpan = 2;
                        //if (i == 11) //Tồn đầu
                        //    header.ColumnSpan = 6;
                        if (i == 13) //Nhập
                            header.ColumnSpan = 2;
                        //else if (i == 13) //Total
                        //    header.ColumnSpan = 4;
                        else if (i == 15) //Xuất
                            header.ColumnSpan = 2;
                        //else if (i == 14) //Total
                        //    header.ColumnSpan = 2;
                        //else if (i == 16) //Ending stocks
                        //    header.ColumnSpan = 2;
                        //else
                        //    header.RowSpan = 2;

                        header.BackColor = Color.LightGray;
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    tieude = new string[] {"", "", "", "", "", "", "", "", "", "", "",
                                        "", "", 
                                        "Stock in","Transfer", "",
                                        "Stock out", "Transfer", "", "" };

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

                    string query = "SELECT TOP(1) nam, thang, thangks  " +
                    "FROM " +
                    "( " +
                    "	SELECT nam, thang, '01-' + case when thang < 10 then '0' + cast(thang as varchar(10)) else cast(thang as varchar(10)) end + '-' + cast(nam as varchar(10)) as thangks " +
                    "	FROM KHOASOTHANG   " +
                    ") " +
                    "DATA " +
                    "WHERE CONVERT(datetime, thangks, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                    "ORDER BY nam desc, thang desc ";

                    DataTable dtKS = xl.ReadTable(query);

                    string thangks = "";
                    string namks = "";
                    int thangTIEPTHEO = 0;
                    int namTIEPTHEO = 0;
                    if (dtKS.Rows.Count > 0)
                    {
                        thangks = dtKS.Rows[0]["thang"].ToString();
                        namks = dtKS.Rows[0]["nam"].ToString();
                        thangTIEPTHEO = int.Parse(dtKS.Rows[0]["thang"].ToString());
                        namTIEPTHEO = int.Parse(dtKS.Rows[0]["nam"].ToString());

                        if (int.Parse(thangks) == 12)
                        {
                            thangTIEPTHEO = 1;
                            namTIEPTHEO += 1;
                        }
                        else
                            thangTIEPTHEO += 1;
                    }

                    string dauthang = "01-" + (thangTIEPTHEO < 10 ? "0" + thangTIEPTHEO.ToString() : thangTIEPTHEO.ToString()) + "-" + namTIEPTHEO.ToString();

                    string[] khoIds = Regex.Split(khoId, ",");
                    string[] thangBC = Regex.Split(tungay, "-");
                    
                    for (int count = 0; count < khoIds.Length; count++)
                    {
                        khoId = khoIds[count];
                        string khoTen = xl.ExecuteScalarSQL("SELECT makho FROM KHO WHERE pk_seq = '" + khoIds[count] + "'").ToString();

                        //cureent dd-mm-yyyy ==> change to yyyy-mm-dd
                        //tungay = xl.convertToDate_YYYYMMDD(tungay);
                        //denngay = xl.convertToDate_YYYYMMDD(denngay);

                        query = " SELECT (SELECT ten FROM NganhHang WHERE pk_seq = sp.nganhhang_fk) nganhhang, ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') chungloai, " +
                            "    sp.ma AS masp, sp.nameEnglish AS tensp, (SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk) AS dvt, " +
                            "    CASE DAUKY.trangthai WHEN 0 THEN N'Ok'   " +
                            "                         WHEN 1 THEN 'Loos'  " +
                            "                         WHEN 2 THEN 'NG'  " +
                            "                         WHEN 3 THEN 'Test'  " +
                            "                         WHEN 4 THEN 'Orther' ELSE N'Unknow' END trangthai, " +
                            "    DAUKY.xuatxu_fk, ISNULL((SELECT ma FROM XuatXu WHERE pk_seq = DAUKY.xuatxu_fk), '') xuatxu, " + 
                            "    ISNULL((SELECT ma FROM Location WHERE pk_seq = DAUKY.location_fk), '') location, DAUKY.bin_fk, " +
                            "    ISNULL(DAUKY.lotNo, '') lotNo, ISNULL(DAUKY.serialNo, '') serialNo, ISNULL(DAUKY.solo, '') solo, " +
                            "    ISNULL(dauky.tondauKS, 0) AS tondauKS, ISNULL(dauky.soluong, 0) AS tondau, ISNULL(dauky.thanhtienTONDAU, 0) AS thanhtienTONDAU, " +
                            "    ISNULL(TRONGKY.nhapNCC, 0) nhapNCC, ISNULL(TRONGKY.traNCC, 0) traNCC, ISNULL(TRONGKY.kiemke, 0) kiemke, ISNULL(TRONGKY.dieuchinh, 0) dieuchinh, " +
                            "    ISNULL(TRONGKY.xuathangBAN, 0) xuathangBAN, ISNULL(TRONGKY.xuatKM, 0) xuatKM, 0 AS chiagia, ISNULL(TRONGKY.trahangban, 0) trahangban, ISNULL(TRONGKY.trahangKM, 0) trahangKM, " +
                            "    ISNULL(TRONGKY.nhapDoiLoBin, 0) nhapDoiLoBin, ISNULL(TRONGKY.xuatDoiLoBin, 0) xuatDoiLoBin, ISNULL(TRONGKY.nhapkhac, 0) nhapkhac, ISNULL(TRONGKY.xuatkhac, 0) xuatkhac, ISNULL(TRONGKY.xuattieuhao, 0) xuattieuhao, ISNULL(dauky.giamua, 0) giamua, ISNULL(dauky.giaban, 0) giaban, ISNULL(dauky.giaton, 0) giaton, " +
                            "    TRONGKY.thanhtienNhap, TRONGKY.thanhtienDCTK, TRONGKY.thanhtienKiemKe, TRONGKY.thanhtienNhapKhac, " +
                            "    TRONGKY.thanhtienNhapTRA, TRONGKY.thanhtienTKCG, TRONGKY.thanhtienTraBH, TRONGKY.thanhtienTraKM, " +
                            "    TRONGKY.thanhtienXuat, TRONGKY.thanhtienXuatKM, TRONGKY.thanhtienXuatKhac " +
                            "  FROM SanPham sp INNER JOIN " +
                            " ( " +
                            " 	SELECT  '" + khoId + "' AS kho_fk, kho.sanpham_fk, kho.xuatxu_fk, kho.location_fk, kho.bin_fk, kho.lotNo, kho.serialNo, kho.solo, kho.trangthai, ISNULL(tondau.soluong, 0) AS tondauKS,  " +
                            " 		ISNULL(tondau.soluong, 0) + ISNULL(nhap.soluong, 0) - ISNULL(tra.soluong, 0) + " +
                            " 		ISNULL(dieuchinh.soluong, 0) - ISNULL(xuathangban.soluong, 0) - ISNULL(xuatkhuyenmai.soluong, 0) + " +
                            " 		ISNULL(trahangban.soluong, 0) + ISNULL(trahangkhuyenmai.soluong, 0) + " +
                            "       ISNULL(nhapDoiLoBin.soluong, 0) - ISNULL(xuatDoiLoBin.soluong, 0) + " + 
                            " 		ISNULL(nhapkhac.soluong, 0) - ISNULL(xuatkhac.soluong, 0) - ISNULL(xuatTieuHao.soluong, 0) AS soluong, " +
                            " 		ISNULL( ( SELECT TOP(1) dongia FROM BangGiaMua_SanPham WHERE sanpham_fk = kho.sanpham_fk), 0 ) AS giamua, " +
                            " 		ISNULL( ( SELECT TOP(1) dongia FROM BangGiaBan_SanPham WHERE sanpham_fk = kho.sanpham_fk), 0 ) AS giaban, 1 AS giaton, ISNULL(tondau.thanhtienTONDAU, 0) AS thanhtienTONDAU " +
                            " 	FROM Kho_SanPham_ChiTiet kho " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'1.Tồn đầu' AS loaict, sanpham_fk, xuatxu_fk, location_fk, bin_fk, lotNo, serialNo, solo, trangthai, soluong, soluong * giaban AS thanhtienTONDAU  " +
                            " 		FROM TonKhoThang_ChiTiet" +
                            " 		WHERE thang = '" + thangks + "' AND nam = '" + namks + "' AND kho_fk = '" + khoId + "'  " +
                            " 	) " +
                            " 	tondau ON kho.sanpham_fk = tondau.sanpham_fk AND kho.xuatxu_fk = tondau.xuatxu_fk AND kho.location_fk = tondau.location_fk AND kho.bin_fk = tondau.bin_fk " +
                            " 		AND kho.lotNo = tondau.lotNo AND kho.serialNo = tondau.serialNo AND kho.solo = tondau.solo " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'2.1.Nhập hàng' AS loaict, b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluongQuyDoi) soluong, ISNULL(b.giamua , 0) gianhap " +
                            " 		FROM NhapHang a INNER JOIN NhapHang_SanPham_ChiTiet b ON a.pk_seq = b.nhaphang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaynhap, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            "         GROUP BY b.sanpham_fk, b.giamua, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            " 	) " +
                            " 	nhap ON kho.sanpham_fk = nhap.sanpham_fk AND kho.xuatxu_fk = nhap.xuatxu_fk AND kho.location_fk = nhap.location_fk AND kho.bin_fk = nhap.bin_fk " +
                            " 		AND kho.lotNo = nhap.lotNo AND kho.serialNo = nhap.serialNo AND kho.solo = nhap.solo AND kho.trangthai = nhap.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'3.Trả hàng NCC' AS loaict, b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong * b.dongia) thanhtienNhapTRA " +
                            " 		FROM TraHang a INNER JOIN TraHang_SanPham_ChiTiet b ON a.pk_seq = b.trahang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaytra, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaytra, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            " 	) " +
                            " 	tra ON kho.sanpham_fk = tra.sanpham_fk AND kho.xuatxu_fk = tra.xuatxu_fk AND kho.location_fk = tra.location_fk AND kho.bin_fk = tra.bin_fk " +
                            " 		AND kho.lotNo = tra.lotNo AND kho.serialNo = tra.serialNo AND kho.solo = tra.solo AND kho.trangthai = tra.loaihanghoa " +
                            //" 	LEFT JOIN " +
                            //" 	( " +
                            //" 		SELECT N'4.Kiểm kho' AS loaict,  b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluongkiem - b.ton) AS soluong, SUM((b.soluongkiem - b.ton)*giamuatb) thanhtienKiemKho " +
                            //" 		FROM KiemKho a INNER JOIN KiemKho_SanPham b ON a.pk_seq = b.kiemkho_fk " +
                            //" 		WHERE CONVERT(datetime, a.ngaykiem, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            //" 				and CONVERT(datetime, a.ngaykiem, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            //" 		GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            //" 	) " +
                            //" 	kiemke ON kho.sanpham_fk = kiemke.sanpham_fk AND kho.xuatxu_fk = kiemke.xuatxu_fk AND kho.location_fk = kiemke.location_fk AND kho.bin_fk = kiemke.bin_fk " +
                            //" 		AND kho.lotNo = kiemke.lotNo AND kho.serialNo = kiemke.serialNo AND kho.solo = kiemke.solo AND kho.trangthai = kiemke.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'5.Điều chỉnh tồn kho' AS loaict, b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.dieuchinh) AS soluong, SUM(b.dieuchinh * b.giamuatb) thanhtienDCTK " +
                            " 		FROM DieuChinhTonKho a INNER JOIN DieuChinhTonKho_SanPham b ON a.pk_seq = b.dieuchinh_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaydieuchinh, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaydieuchinh, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.giamuatb, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            " 	) " +
                            " 	dieuchinh ON kho.sanpham_fk = dieuchinh.sanpham_fk AND kho.xuatxu_fk = dieuchinh.xuatxu_fk AND kho.location_fk = dieuchinh.location_fk AND kho.bin_fk = dieuchinh.bin_fk " +
                            " 		AND kho.lotNo = dieuchinh.lotNo AND kho.serialNo = dieuchinh.serialNo AND kho.solo = dieuchinh.solo AND kho.trangthai = dieuchinh.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'6.Stock out' AS loaict, b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.SOLUONGQUYDOI) AS soluong, 1 thanhtienXuat " +
                            "         FROM DonHang a INNER JOIN DonHang_SanPham_ChiTiet b ON a.pk_seq = b.donhang_FK  " +
                            "         WHERE CONVERT(datetime, a.ngaygiaohang, 105) >= CONVERT(datetime,'" + dauthang + "', 105) AND a.trangthai not in (0, 2) AND a.kho_fk = '" + khoId + "'  " +
                            "                 AND CONVERT(datetime, a.ngaygiaohang, 105) < CONVERT(datetime, '" + tungay + "', 105)   " +
                            "         GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            " 	) " +
                            " 	xuathangban ON kho.sanpham_fk = xuathangban.sanpham_fk AND kho.xuatxu_fk = xuathangban.xuatxu_fk AND kho.location_fk = xuathangban.location_fk AND kho.bin_fk = xuathangban.bin_fk " +
                            " 		AND kho.lotNo = xuathangban.lotNo AND kho.serialNo = xuathangban.serialNo AND kho.solo = xuathangban.solo AND kho.trangthai = xuathangban.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'6.Xuất khuyến mại' AS loaict, c.pk_seq AS sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.SOLUONG) AS soluong, SUM(b.TongGiaTri) thanhtienXuatKM " +
                            "         FROM DonHang a INNER JOIN DONHANG_CTKM_TRAKM b ON a.pk_seq = b.donhang_fk  " +
                            "                 INNER JOIN SANPHAM c ON b.spMA = c.MA  " +
                            "         WHERE CONVERT(datetime, a.ngaygiaohang, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai not in (0, 2) AND b.kho_fk = '" + khoId + "'  " +
                            "                 AND CONVERT(datetime, a.ngaygiaohang, 105) < CONVERT(datetime, '" + tungay + "', 105)   " +
                            "         GROUP BY c.pk_seq, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            " 	) " +
                            " 	xuatkhuyenmai ON kho.sanpham_fk = xuatkhuyenmai.sanpham_fk AND kho.xuatxu_fk = xuatkhuyenmai.xuatxu_fk AND kho.location_fk = xuatkhuyenmai.location_fk AND kho.bin_fk = xuatkhuyenmai.bin_fk " +
                            " 		AND kho.lotNo = xuatkhuyenmai.lotNo AND kho.serialNo = xuatkhuyenmai.serialNo AND kho.solo = xuatkhuyenmai.solo AND kho.trangthai = xuatkhuyenmai.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'10.Trả hàng khách hàng' AS loaict, b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluongQUYDOI) AS soluong, SUM(b.soluong*b.dongia) thanhtienTraKH " +
                            " 		FROM DonTraHang a INNER JOIN DonTraHang_SanPham_ChiTiet b ON a.pk_seq = b.dontrahang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaydonhang, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = 1 AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaydonhang, 105) < CONVERT(datetime, '" + dauthang + "', 105)   " +
                            " 		GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            "  ) " +
                            " 	trahangban ON kho.sanpham_fk = trahangban.sanpham_fk AND kho.xuatxu_fk = trahangban.xuatxu_fk AND kho.location_fk = trahangban.location_fk AND kho.bin_fk = trahangban.bin_fk " +
                            " 		AND kho.lotNo = trahangban.lotNo AND kho.serialNo = trahangban.serialNo AND kho.solo = trahangban.solo AND kho.trangthai = trahangban.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'11.Trả hàng khuyến mại' AS loaict,  b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluong) AS soluong, SUM(b.soluong*b.dongia) AS thanhtienTraKHKM " +
                            " 		FROM DonTraHang a INNER JOIN DonTraHang_SPKM_ChiTiet b ON a.pk_seq = b.dontrahang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaydonhang, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = 1 AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaydonhang, 105) < CONVERT(datetime, '" + tungay + "', 105)  " +
                            " 		GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa" +
                            " 		" +
                            " 	) " +
                            " 	trahangkhuyenmai ON kho.sanpham_fk = trahangkhuyenmai.sanpham_fk AND kho.xuatxu_fk = trahangkhuyenmai.xuatxu_fk AND kho.location_fk = trahangkhuyenmai.location_fk AND kho.bin_fk = trahangkhuyenmai.bin_fk " +
                            " 		AND kho.lotNo = trahangkhuyenmai.lotNo AND kho.serialNo = trahangkhuyenmai.serialNo AND kho.solo = trahangkhuyenmai.solo  AND kho.trangthai = trahangkhuyenmai.loaihanghoa " +                            
                            " LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'14.Nhập khác' AS loaict, b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluongQUYDOI) AS soluong, SUM(b.soluong*b.dongia) thanhtienNhapKhac " +
                            " 		FROM NhapKhac a INNER JOIN NhapKhac_SanPham_ChiTiet b ON a.pk_seq = b.nhapkhac_fk           " +
                            " 		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'   " +
                            " 				and CONVERT(datetime, a.ngaynhap, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            " 	) " +
                            " 	nhapkhac ON kho.sanpham_fk = nhapkhac.sanpham_fk AND kho.xuatxu_fk = nhapkhac.xuatxu_fk AND kho.location_fk = nhapkhac.location_fk AND kho.bin_fk = nhapkhac.bin_fk " +
                            " 		AND kho.lotNo = nhapkhac.lotNo AND kho.serialNo = nhapkhac.serialNo AND kho.solo = nhapkhac.solo  AND kho.trangthai = nhapkhac.loaihanghoa " +
                            "    LEFT JOIN  " +
                            "    ( " +
                            " 		SELECT N'15.Xuất khác' AS loaict,  b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienXuatKhac " +
                            " 		FROM XuatKhac a INNER JOIN XuatKhac_SanPham_ChiTiet b ON a.pk_seq = b.xuatkhac_fk " +
                            " 		WHERE CONVERT(datetime, a.ngayxuat, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'   " +
                            " 				and CONVERT(datetime, a.ngayxuat, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            " 	) " +
                            " 	xuatkhac ON kho.sanpham_fk = xuatkhac.sanpham_fk AND kho.xuatxu_fk = xuatkhac.xuatxu_fk AND kho.location_fk = xuatkhac.location_fk AND kho.bin_fk = xuatkhac.bin_fk " +
                            " 		AND kho.lotNo = xuatkhac.lotNo AND kho.serialNo = xuatkhac.serialNo AND kho.solo = xuatkhac.solo AND kho.trangthai = xuatkhac.loaihanghoa " +
                            " LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'16.Nhập đổi location' AS loaict, b.sanpham_fk, b.xuatxu_new_fk AS xuatxu_fk, b.location_new_fk AS location_fk, b.bin_new_fk AS bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluongQUYDOI) AS soluong, SUM(b.soluong*b.dongia) thanhtienNhapKhac " +
                            " 		FROM DoiLoBin a INNER JOIN DoiLoBin_SanPham_ChiTiet b ON a.pk_seq = b.doilobin_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'   " +
                            " 				and CONVERT(datetime, a.ngaynhap, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.xuatxu_new_fk, b.location_new_fk, b.bin_new_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            " 	) " +
                            " 	nhapDoiLoBin ON kho.sanpham_fk = nhapDoiLoBin.sanpham_fk AND kho.xuatxu_fk = nhapDoiLoBin.xuatxu_fk AND kho.location_fk = nhapDoiLoBin.location_fk AND kho.bin_fk = nhapDoiLoBin.bin_fk " +
                            " 		AND kho.lotNo = nhapDoiLoBin.lotNo AND kho.serialNo = nhapDoiLoBin.serialNo AND kho.solo = nhapDoiLoBin.solo AND kho.trangthai = nhapDoiLoBin.loaihanghoa " +
                            "    LEFT JOIN  " +
                            "    ( " +
                            " 		SELECT N'15.Xuất đổi lo bin' AS loaict,  b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienXuatKhac " +
                            " 		FROM DoiLoBin a INNER JOIN DoiLoBin_SanPham_ChiTiet b ON a.pk_seq = b.doilobin_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'   " +
                            " 				and CONVERT(datetime, a.ngaynhap, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            " 	) " +
                            " 	xuatDoiLoBin ON kho.sanpham_fk = xuatDoiLoBin.sanpham_fk AND kho.xuatxu_fk = xuatDoiLoBin.xuatxu_fk AND kho.location_fk = xuatDoiLoBin.location_fk AND kho.bin_fk = xuatDoiLoBin.bin_fk " +
                            " 		AND kho.lotNo = xuatDoiLoBin.lotNo AND kho.serialNo = xuatDoiLoBin.serialNo AND kho.solo = xuatDoiLoBin.solo AND kho.trangthai = xuatDoiLoBin.loaihanghoa " +
                            "    LEFT JOIN  " +
                            "    (  " +
                            "    	SELECT N'19.Xuất tiêu hao nguyên liệu' AS loaict,  b.vattu_fk AS sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluongQUYDOI) AS soluong  " +
                            "    	FROM LenhSanXuat_TieuHao a INNER JOIN LenhSanXuat_TieuHao_ChiTiet b ON a.pk_seq = b.tieuhao_fk  " +
                            "    	WHERE CONVERT(datetime, a.ngaytieuhao, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'     " +
                            "    			and CONVERT(datetime, a.ngaytieuhao, 105) <= CONVERT(datetime, '" + tungay + "', 105) " +
                            "    	GROUP BY b.vattu_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            "    )  " +
                            "    xuatTieuHao ON kho.sanpham_fk = xuatTieuHao.sanpham_fk AND kho.xuatxu_fk = xuatTieuHao.xuatxu_fk AND kho.location_fk = xuatTieuHao.location_fk AND kho.bin_fk = xuatTieuHao.bin_fk " +
                            " 	AND kho.lotNo = xuatTieuHao.lotNo AND kho.serialNo = xuatTieuHao.serialNo AND kho.solo = xuatTieuHao.solo AND kho.trangthai = xuatTieuHao.loaihanghoa " +
                            " 	WHERE kho.kho_fk = '" + khoId + "' " +
                            " ) " +
                            " DAUKY ON sp.pk_seq = DAUKY.sanpham_fk " +
                            " LEFT JOIN " +
                            " ( " +
                            " 	SELECT  '" + khoId + "'  AS kho_fk, kho.sanpham_fk, kho.xuatxu_fk, kho.location_fk, kho.bin_fk, kho.lotNo, kho.serialNo, kho.solo, kho.trangthai, " +
                            " 		ISNULL(nhap.soluong, 0) AS nhapNCC, ISNULL(nhap.thanhtienNhap, 0) thanhtienNhap, 		" +
                            " 		ISNULL(tra.soluong, 0) AS traNCC, ISNULL(tra.thanhtienNhapTRA, 0)  thanhtienNhapTRA, " +
                            " 		0 AS kiemke, 0 thanhtienKiemKe, ISNULL(dieuchinh.soluong, 0) AS dieuchinh, ISNULL(dieuchinh.thanhtienDCTK, 0) thanhtienDCTK, " +
                            " 		ISNULL(xuathangban.soluong, 0) AS xuathangBAN, ISNULL(xuathangban.thanhtienXuat, 0) thanhtienXuat, " +
                            " 		ISNULL(xuatkhuyenmai.soluong, 0) AS xuatKM, ISNULL(xuatkhuyenmai.thanhtienXuatKM, 0) thanhtienXuatKM, " +
                            " 		0 chiagia, 0 thanhtienTKCG, ISNULL(trahangban.soluong, 0) AS trahangban, ISNULL(trahangban.thanhtienTraBH, 0) AS thanhtienTraBH, " +
                            " 		ISNULL(trakhuyenmai.soluong, 0) AS trahangKM, ISNULL(trakhuyenmai.thanhtienTraKM, 0) AS thanhtienTraKM, " +
                            " 		ISNULL(nhapDoiLoBin.soluong, 0) AS nhapDoiLoBin, ISNULL(xuatDoiLoBin.soluong, 0) AS xuatDoiLoBin, " +
                            " 		ISNULL(nhapkhac.soluong, 0) AS nhapkhac, ISNULL(nhapkhac.thanhtienNhapKhac, 0) AS thanhtienNhapKhac, " +
                            " 		ISNULL(xuatkhac.soluong, 0) AS xuatkhac, ISNULL(xuatkhac.thanhtienXuatKhac, 0) AS thanhtienXuatKhac, ISNULL(xuatTieuHao.soluong, 0) AS xuattieuhao  " +
                            " 	FROM Kho_SanPham_ChiTiet kho " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'2.1.Nhập hàng' AS loaict, b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluongQuyDoi) soluong, ISNULL(SUM(b.soluong * b.giamua), 0) thanhtienNhap " +
                            " 		FROM NhapHang a INNER JOIN NhapHang_SanPham_ChiTiet b ON a.pk_seq = b.nhaphang_fk " +
                            " 	    WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaynhap, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            "         GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            " 	) " +
                            " 	nhap ON kho.sanpham_fk = nhap.sanpham_fk AND kho.xuatxu_fk = nhap.xuatxu_fk AND kho.location_fk = nhap.location_fk AND kho.bin_fk = nhap.bin_fk " +
                            " 		AND kho.lotNo = nhap.lotNo AND kho.serialNo = nhap.serialNo AND kho.solo = nhap.solo AND kho.trangthai = nhap.loaihanghoa	" +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'3.Trả hàng NCC' AS loaict, b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienNhapTRA " +
                            " 		FROM TraHang a INNER JOIN TraHang_SanPham_ChiTiet b ON a.pk_seq = b.trahang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaytra, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaytra, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa  " +
                            " 	) " +
                            " 	tra ON kho.sanpham_fk = tra.sanpham_fk AND kho.xuatxu_fk = tra.xuatxu_fk AND kho.location_fk = tra.location_fk AND kho.bin_fk = tra.bin_fk " +
                            " 		AND kho.lotNo = tra.lotNo AND kho.serialNo = tra.serialNo AND kho.solo = tra.solo AND kho.trangthai = tra.loaihanghoa " +
                            //" 	LEFT JOIN " +
                            //" 	( " +
                            //" 		SELECT N'4.Kiểm kho' AS loaict,  b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluongkiem - b.ton) AS soluong, SUM((b.soluongkiem - b.ton) * giamuatb) thanhtienKiemKe " +
                            //" 		FROM KiemKho a INNER JOIN KiemKho_SanPham b ON a.pk_seq = b.kiemkho_fk " +
                            //" 		WHERE CONVERT(datetime, a.ngaykiem, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            //" 				and CONVERT(datetime, a.ngaykiem, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            //" 		GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            //" 	) " +
                            //" 	kiemke ON kho.sanpham_fk = kiemke.sanpham_fk AND kho.xuatxu_fk = kiemke.xuatxu_fk AND kho.location_fk = kiemke.location_fk AND kho.bin_fk = kiemke.bin_fk " +
                            //" 		AND kho.lotNo = kiemke.lotNo AND kho.serialNo = kiemke.serialNo AND kho.solo = kiemke.solo AND kho.trangthai = kiemke.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'5.Điều chỉnh tồn kho' AS loaict,  b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.dieuchinh) AS soluong, SUM(b.dieuchinh * b.giamuatb) thanhtienDCTK " +
                            " 		FROM DieuChinhTonKho a INNER JOIN DieuChinhTonKho_SanPham b ON a.pk_seq = b.dieuchinh_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaydieuchinh, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaydieuchinh, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            " 	) " +
                            " 	dieuchinh ON kho.sanpham_fk = dieuchinh.sanpham_fk AND kho.xuatxu_fk = dieuchinh.xuatxu_fk AND kho.location_fk = dieuchinh.location_fk AND kho.bin_fk = dieuchinh.bin_fk " +
                            " 		AND kho.lotNo = dieuchinh.lotNo AND kho.serialNo = dieuchinh.serialNo AND kho.solo = dieuchinh.solo AND kho.trangthai = dieuchinh.loaihanghoa " +
                            " LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'6.Stock out' AS loaict, b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.SOLUONGQUYDOI) AS soluong, 1 thanhtienXuat " +
                            "         FROM DonHang a INNER JOIN DonHang_SanPham_ChiTiet b ON a.pk_seq = b.donhang_FK  " +
                            "         WHERE CONVERT(datetime, a.ngaygiaohang, 105) >= CONVERT(datetime,'" + tungay + "', 105) AND a.trangthai IN (1) AND a.kho_fk = '" + khoId + "'  " +
                            "                 AND CONVERT(datetime, a.ngaygiaohang, 105) <= CONVERT(datetime, '" + denngay + "', 105)   " +
                            "         GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa  " +
                            " 	) " +
                            " 	xuathangban ON kho.sanpham_fk = xuathangban.sanpham_fk AND kho.xuatxu_fk = xuathangban.xuatxu_fk AND kho.location_fk = xuathangban.location_fk AND kho.bin_fk = xuathangban.bin_fk " +
                            " 		AND kho.lotNo = xuathangban.lotNo AND kho.serialNo = xuathangban.serialNo AND kho.solo = xuathangban.solo AND kho.trangthai = xuathangban.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'6.Xuất khuyến mại' AS loaict, c.pk_seq AS sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.SOLUONG) AS soluong, SUM(b.TongGiaTri) thanhtienXuatKM  " +
                            "         FROM DonHang a INNER JOIN DONHANG_CTKM_TRAKM b ON a.pk_seq = b.donhang_fk  " +
                            "                 INNER JOIN SANPHAM c ON b.spMA = c.MA  " +
                            "         WHERE CONVERT(datetime, a.ngaygiaohang, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai IN (1) AND b.kho_fk = '" + khoId + "'  " +
                            "                 AND CONVERT(datetime, a.ngaygiaohang, 105) <= CONVERT(datetime, '" + denngay + "', 105)   " +
                            "         GROUP BY c.pk_seq, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa  " +
                            " 	) " +
                            " 	xuatkhuyenmai ON kho.sanpham_fk = xuatkhuyenmai.sanpham_fk AND kho.xuatxu_fk = xuatkhuyenmai.xuatxu_fk AND kho.location_fk = xuatkhuyenmai.location_fk AND kho.bin_fk = xuatkhuyenmai.bin_fk " +
                            " 		AND kho.lotNo = xuatkhuyenmai.lotNo AND kho.serialNo = xuatkhuyenmai.serialNo AND kho.solo = xuatkhuyenmai.solo AND kho.trangthai = xuatkhuyenmai.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'10.Trả hàng khách hàng' AS loaict,  b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluongQUYDOI) AS soluong, SUM(b.dongia*b.soluong) thanhtienTraBH " +
                            " 		FROM DonTraHang a INNER JOIN DonTraHang_SanPham_ChiTiet b ON a.pk_seq = b.dontrahang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaydonhang, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = 1 AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaydonhang, 105) <= CONVERT(datetime, '" + denngay + "', 105)   " +
                            " 		GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            " 	) " +
                            " trahangban ON kho.sanpham_fk = trahangban.sanpham_fk AND kho.xuatxu_fk = trahangban.xuatxu_fk AND kho.location_fk = trahangban.location_fk AND kho.bin_fk = trahangban.bin_fk " +
                            " 		AND kho.lotNo = trahangban.lotNo AND kho.serialNo = trahangban.serialNo AND kho.solo = trahangban.solo AND kho.trangthai = trahangban.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'11.Trả hàng khuyến mại' AS loaict, b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluong) AS soluong, SUM(b.soluong*dongia) thanhtienTraKM " +
                            " 		FROM DonTraHang a INNER JOIN DonTraHang_SPKM_ChiTiet b ON a.pk_seq = b.dontrahang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaydonhang, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = 1 AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaydonhang, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                            " 		GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa	" +
                            " 	) " +
                            " 	trakhuyenmai ON kho.sanpham_fk = trakhuyenmai.sanpham_fk AND kho.xuatxu_fk = trakhuyenmai.xuatxu_fk AND kho.location_fk = trakhuyenmai.location_fk AND kho.bin_fk = trakhuyenmai.bin_fk " +
                            " 		AND kho.lotNo = trakhuyenmai.lotNo AND kho.serialNo = trakhuyenmai.serialNo AND kho.solo = trakhuyenmai.solo AND kho.trangthai = trakhuyenmai.loaihanghoa 	" +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'14.Nhập khác' AS loaict, b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienNhapKhac" +
                            " 		FROM NhapKhac a INNER JOIN NhapKhac_SanPham_ChiTiet b ON a.pk_seq = b.nhapkhac_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'   " +
                            " 				and CONVERT(datetime, a.ngaynhap, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            " 	) " +
                            " 	nhapkhac ON kho.sanpham_fk = nhapkhac.sanpham_fk AND kho.xuatxu_fk = nhapkhac.xuatxu_fk AND kho.location_fk = nhapkhac.location_fk AND kho.bin_fk = nhapkhac.bin_fk " +
                            " 		AND kho.lotNo = nhapkhac.lotNo AND kho.serialNo = nhapkhac.serialNo AND kho.solo = nhapkhac.solo AND kho.trangthai = nhapkhac.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'15.Xuất khác' AS loaict,  b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienXuatKhac " +
                            " 		FROM XuatKhac a INNER JOIN XuatKhac_SanPham_ChiTiet b ON a.pk_seq = b.xuatkhac_fk " +
                            " 		WHERE CONVERT(datetime, a.ngayxuat, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'   " +
                            " 				and CONVERT(datetime, a.ngayxuat, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            " 	) " +
                            "   xuatkhac ON kho.sanpham_fk = xuatkhac.sanpham_fk AND kho.xuatxu_fk = xuatkhac.xuatxu_fk AND kho.location_fk = xuatkhac.location_fk AND kho.bin_fk = xuatkhac.bin_fk " +
                            " 		AND kho.lotNo = xuatkhac.lotNo AND kho.serialNo = xuatkhac.serialNo AND kho.solo = xuatkhac.solo AND kho.trangthai = xuatkhac.loaihanghoa " +
                            " LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'16.Nhập đổi location' AS loaict, b.sanpham_fk, b.xuatxu_new_fk AS xuatxu_fk, b.location_new_fk AS location_fk, b.bin_new_fk AS bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluongQUYDOI) AS soluong, SUM(b.soluong*b.dongia) thanhtienNhapKhac " +
                            " 		FROM DoiLoBin a INNER JOIN DoiLoBin_SanPham_ChiTiet b ON a.pk_seq = b.doilobin_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'   " +
                            " 				and CONVERT(datetime, a.ngaynhap, 105) < CONVERT(datetime, '" + denngay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.xuatxu_new_fk, b.location_new_fk, b.bin_new_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            " 	) " +
                            " 	nhapDoiLoBin ON kho.sanpham_fk = nhapDoiLoBin.sanpham_fk AND kho.xuatxu_fk = nhapDoiLoBin.xuatxu_fk AND kho.location_fk = nhapDoiLoBin.location_fk AND kho.bin_fk = nhapDoiLoBin.bin_fk " +
                            " 		AND kho.lotNo = nhapDoiLoBin.lotNo AND kho.serialNo = nhapDoiLoBin.serialNo AND kho.solo = nhapDoiLoBin.solo AND kho.trangthai = nhapDoiLoBin.loaihanghoa " +
                            "    LEFT JOIN  " +
                            "    ( " +
                            " 		SELECT N'17.Xuất đổi lô bin' AS loaict,  b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienXuatKhac " +
                            " 		FROM DoiLoBin a INNER JOIN DoiLoBin_SanPham_ChiTiet b ON a.pk_seq = b.doilobin_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'   " +
                            " 				and CONVERT(datetime, a.ngaynhap, 105) < CONVERT(datetime, '" + denngay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            " 	) " +
                            " 	xuatDoiLoBin ON kho.sanpham_fk = xuatDoiLoBin.sanpham_fk AND kho.xuatxu_fk = xuatDoiLoBin.xuatxu_fk AND kho.location_fk = xuatDoiLoBin.location_fk AND kho.bin_fk = xuatDoiLoBin.bin_fk " +
                            " 		AND kho.lotNo = xuatDoiLoBin.lotNo AND kho.serialNo = xuatDoiLoBin.serialNo AND kho.solo = xuatDoiLoBin.solo AND kho.trangthai = xuatDoiLoBin.loaihanghoa 	" +
                            "    LEFT JOIN  " +
                            "    (  " +
                            "    	SELECT N'19.Xuất tiêu hao nguyên liệu' AS loaict,  b.vattu_fk AS sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa, SUM(b.soluongQUYDOI) AS soluong  " +
                            "    	FROM LenhSanXuat_TieuHao a INNER JOIN LenhSanXuat_TieuHao_ChiTiet b ON a.pk_seq = b.tieuhao_fk  " +
                            "    	WHERE CONVERT(datetime, a.ngaytieuhao, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'     " +
                            "    			and CONVERT(datetime, a.ngaytieuhao, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            "    	GROUP BY b.vattu_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, a.loaihanghoa " +
                            "    )  " +
                            "    xuatTieuHao ON kho.sanpham_fk = xuatTieuHao.sanpham_fk AND kho.xuatxu_fk = xuatTieuHao.xuatxu_fk AND kho.location_fk = xuatTieuHao.location_fk AND kho.bin_fk = xuatTieuHao.bin_fk " +
                            " AND kho.lotNo = xuatTieuHao.lotNo AND kho.serialNo = xuatTieuHao.serialNo AND kho.solo = xuatTieuHao.solo AND kho.trangthai = xuatTieuHao.loaihanghoa " +
                            " 	WHERE kho.kho_fk = '" + khoId + "' " +
                            " ) " +
                            " TRONGKY ON DAUKY.sanpham_fk = TRONGKY.sanpham_fk AND DAUKY.xuatxu_fk = TRONGKY.xuatxu_fk AND DAUKY.location_fk = TRONGKY.location_fk AND DAUKY.bin_fk = TRONGKY.bin_fk " +
                            " 		AND DAUKY.lotNo = TRONGKY.lotNo AND DAUKY.serialNo = TRONGKY.serialNo AND DAUKY.solo = TRONGKY.solo  AND DAUKY.trangthai = TRONGKY.trangthai " +
                            "  INNER JOIN NganhHang nh ON sp.nganhhang_fk = nh.pk_seq " +
                            "  LEFT JOIN ChungLoai cl ON sp.chungloai_fk = cl.pk_seq " +
                            "  WHERE 1 = 1 ";

                        //if (nganhhangId.Trim().Length > 0)
                        //    query += " AND sp.nganhhang in ( SELECT ten FROM NGANHHANG WHERE pk_seq = '" + nganhhangId + "' ) ";

                        DataTable dt = xl.ReadTable(query);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TableRow row = new TableRow();

                            string nganhhang = dt.Rows[i]["nganhhang"].ToString();
                            string chungloai = dt.Rows[i]["chungloai"].ToString();
                            string masp = dt.Rows[i]["masp"].ToString();
                            string tensp = dt.Rows[i]["tensp"].ToString();
                            string dvt = dt.Rows[i]["dvt"].ToString();
                            string trangthai = dt.Rows[i]["trangthai"].ToString();

                            string location = dt.Rows[i]["location"].ToString();
                            //string bin = dt.Rows[i]["bin"].ToString();
                            string xuatxu = dt.Rows[i]["xuatxu"].ToString();
                            string lotNo = dt.Rows[i]["lotNo"].ToString();
                            string serialNo = dt.Rows[i]["serialNo"].ToString();
                            string solo = dt.Rows[i]["solo"].ToString();
                            //string ngaynhap = dt.Rows[i]["ngaynhap"].ToString();

                            //double giamua = double.Parse(dt.Rows[i]["giamua"].ToString());
                            //double giaban = double.Parse(dt.Rows[i]["giaban"].ToString());
                            //double giaton = double.Parse(dt.Rows[i]["giaton"].ToString());

                            //if (theogia.Equals("0"))  //XEM THEO GIA MUA
                            //    giaban = giamua;
                            //else if (theogia.Equals("0"))  //XEM THEO GIA BAN
                            //    giamua = giaban;
                            //else
                               // giamua = giaton;

                            double tondauKS = double.Parse(dt.Rows[i]["tondauKS"].ToString());

                            double tondau = double.Parse(dt.Rows[i]["tondau"].ToString());
                            //double tt_tondau = double.Parse(dt.Rows[i]["thanhtienTONDAU"].ToString());

                            double nhapmuaNCC = double.Parse(dt.Rows[i]["nhapNCC"].ToString());
                            //double tt_nhapmuaNCC = double.Parse(dt.Rows[i]["thanhtienNhap"].ToString());

                            double kiemke = double.Parse(dt.Rows[i]["kiemke"].ToString());
                            //double tt_kiemke = double.Parse(dt.Rows[i]["thanhtienKiemKe"].ToString());

                            //double dieuchinh = double.Parse(dt.Rows[i]["dieuchinh"].ToString());
                            //double tt_dieuchinh = double.Parse(dt.Rows[i]["thanhtienDCTK"].ToString());

                            //double thuhoiBAN = double.Parse(dt.Rows[i]["thuhoiBAN"].ToString());
                            //double tt_thuhoiBAN = double.Parse(dt.Rows[i]["thanhtienTHBH"].ToString());

                            //double thuhoiKM = double.Parse(dt.Rows[i]["thuhoiKM"].ToString());
                            //double tt_thuhoiKM = double.Parse(dt.Rows[i]["thanhtienTHKM"].ToString());

                            double trahangban = double.Parse(dt.Rows[i]["trahangban"].ToString());
                            //double tt_trahangban = double.Parse(dt.Rows[i]["thanhtienTraBH"].ToString());

                            //double trahangKM = double.Parse(dt.Rows[i]["trahangKM"].ToString());
                            //double tt_trahangKM = double.Parse(dt.Rows[i]["thanhtienTraKM"].ToString());

                            //double nhapchuyenkho = double.Parse(dt.Rows[i]["nhapchuyenkho"].ToString());
                            //double tt_nhapchuyenkho = double.Parse(dt.Rows[i]["thanhtienNhapKhacCK"].ToString());

                            //double nhapkhac = double.Parse(dt.Rows[i]["nhapkhac"].ToString());
                            //double tt_nhapkhac = double.Parse(dt.Rows[i]["thanhtienNhapKhac"].ToString());

                            double nhapdoilobin = double.Parse(dt.Rows[i]["nhapdoilobin"].ToString());
                            //double tt_nhapdoilobin = double.Parse(dt.Rows[i]["thanhtienNhapDOILOBIN"].ToString());

                            //double nhaplsx = double.Parse(dt.Rows[i]["nhaplsx"].ToString());
                            //double tt_nhaplsx = double.Parse(dt.Rows[i]["thanhtienNhapLSX"].ToString());

                            //XUAT
                            double xuathangBAN = double.Parse(dt.Rows[i]["xuathangBAN"].ToString());
                            //double tt_xuathangBAN = double.Parse(dt.Rows[i]["thanhtienXuat"].ToString());

                            //THEO DON VI
                            //double XUAT_soluongCHUAN = double.Parse(dt.Rows[i]["XUAT_soluongCHUAN"].ToString());
                            //double XUAT_soluongQD = double.Parse(dt.Rows[i]["XUAT_soluongQD"].ToString());

                            //double xuathangKM = double.Parse(dt.Rows[i]["xuatKM"].ToString());
                            //double tt_xuathangKM = double.Parse(dt.Rows[i]["thanhtienXuatKM"].ToString());

                            double trahangNCC = double.Parse(dt.Rows[i]["traNCC"].ToString());
                            //double tt_trahangNCC = double.Parse(dt.Rows[i]["thanhtienNhapTRA"].ToString());

                            //double chiagia = double.Parse(dt.Rows[i]["chiagia"].ToString());
                            //double tt_chiagia = double.Parse(dt.Rows[i]["thanhtienTKCG"].ToString());

                            //double xuatkhac = double.Parse(dt.Rows[i]["xuatkhac"].ToString());
                            //double tt_xuatkhac = double.Parse(dt.Rows[i]["thanhtienXuatKhac"].ToString());

                            //double xuatchuyenkho = double.Parse(dt.Rows[i]["xuatchuyenkho"].ToString());
                            //double tt_xuatchuyenkho = double.Parse(dt.Rows[i]["thanhtienXuatKhacCK"].ToString());

                            double xuatdoilobin = double.Parse(dt.Rows[i]["xuatdoilobin"].ToString());
                            //double tt_xuatdoilobin = double.Parse(dt.Rows[i]["thanhtienXuatDOILOBIN"].ToString());

                            //double xuatTieuHao = double.Parse(dt.Rows[i]["xuatTieuHao"].ToString());
                            //double tt_xuatTieuHao = double.Parse(dt.Rows[i]["thanhtienXuatTieuHao"].ToString());

                            tondauKS = Math.Round(tondauKS, 3);
                            tondau = Math.Round(tondau, 3);

                            nhapmuaNCC = Math.Round(nhapmuaNCC, 3);
                            kiemke = Math.Round(kiemke, 3);
                            //dieuchinh = Math.Round(dieuchinh, 3);
                            trahangNCC = Math.Round(trahangNCC, 3);
                            //nhapkhac = Math.Round(nhapkhac, 3);
                            //nhapchuyenkho = Math.Round(nhapchuyenkho, 3);
                            //dieuchinh = Math.Round(dieuchinh, 3);
                            nhapdoilobin = Math.Round(nhapdoilobin, 3);
                            //nhaplsx = Math.Round(nhaplsx, 3);

                            xuathangBAN = Math.Round(xuathangBAN, 3);
                            //xuathangKM = Math.Round(xuathangKM, 3);
                            //chiagia = Math.Round(chiagia, 3);
                            //xuatkhac = Math.Round(xuatkhac, 3);
                            //xuatchuyenkho = Math.Round(xuatchuyenkho, 3);
                            xuatdoilobin = Math.Round(xuatdoilobin, 3);
                            //xuatTieuHao = Math.Round(xuatTieuHao, 3);
                            //thuhoiBAN = Math.Round(thuhoiBAN, 3);
                            //thuhoiKM = Math.Round(thuhoiKM, 3);
                            trahangban = Math.Round(trahangban, 3);
                            //trahangKM = Math.Round(trahangKM, 3);

                            //double tongNHAP = nhapmuaNCC + kiemke + dieuchinh - trahangNCC + nhapkhac +
                            //   nhapchuyenkho + nhapdoilobin + nhaplsx;
                            //double tt_tongNHAP = tt_nhapmuaNCC + tt_kiemke + tt_dieuchinh - tt_trahangNCC + tt_nhapchuyenkho + tt_nhapkhac + tt_nhapdoilobin + tt_nhaplsx;


                            //double tongXUAT = xuathangBAN + xuathangKM + chiagia + xuatkhac + xuatchuyenkho + xuatdoilobin + xuatTieuHao - thuhoiBAN - thuhoiKM - trahangban - trahangKM;
                            //double tt_tongXUAT = tt_xuathangBAN + tt_xuathangKM + tt_chiagia + tt_xuatkhac + tt_xuatchuyenkho + tt_xuatdoilobin + tt_xuatTieuHao -
                            //    tt_thuhoiBAN - tt_thuhoiKM - tt_trahangban - tt_trahangKM;

                            double tongNHAP = nhapmuaNCC + kiemke + nhapdoilobin - trahangNCC;
                            //double tt_tongNHAP = tt_nhapmuaNCC + tt_kiemke  - tt_trahangNCC;

                            double tongXUAT = xuathangBAN - trahangban + xuatdoilobin ;
                            //double tt_tongXUAT = tt_xuathangBAN - tt_trahangban;

                            double toncuoi = tondau + tongNHAP - tongXUAT;
                            //double tt_toncuoi = giamua * toncuoi;

                            //string[] data = new string[] { khoTen, nganhhang, masp, tensp, dvt, xuatxu, location, bin, solo, ngaynhap, 
                            //                FormatString.ForMatNumber(tondauKS.ToString()), "", 
                            //                FormatString.ForMatNumber(tondau.ToString()), FormatString.ForMatNumber(tt_tondau.ToString()), 
                            //                FormatString.ForMatNumber(nhapmuaNCC.ToString()), FormatString.ForMatNumber(tt_nhapmuaNCC.ToString()), 
                            //                FormatString.ForMatNumber(trahangNCC.ToString()), FormatString.ForMatNumber(tt_trahangNCC.ToString()), 
                            //                FormatString.ForMatNumber(kiemke.ToString()), FormatString.ForMatNumber(tt_kiemke.ToString()), 
                            //                FormatString.ForMatNumber(dieuchinh.ToString()), FormatString.ForMatNumber(tt_dieuchinh.ToString()),                                           
                            //                FormatString.ForMatNumber(nhapchuyenkho.ToString()), FormatString.ForMatNumber(tt_nhapchuyenkho.ToString()),  
                            //                FormatString.ForMatNumber(nhapkhac.ToString()), FormatString.ForMatNumber(tt_nhapkhac.ToString()), 
                            //                FormatString.ForMatNumber(nhapdoilobin.ToString()), FormatString.ForMatNumber(tt_nhapdoilobin.ToString()),
                            //                FormatString.ForMatNumber(nhaplsx.ToString()), FormatString.ForMatNumber(tt_nhaplsx.ToString()),
                            //                FormatString.ForMatNumber(tongNHAP.ToString()), FormatString.ForMatNumber(tt_tongNHAP.ToString()), 
                            //                FormatString.ForMatNumber(xuathangBAN.ToString()), FormatString.ForMatNumber(tt_xuathangBAN.ToString()), 
                            //                FormatString.ForMatNumber(trahangban.ToString()), FormatString.ForMatNumber(tt_trahangban.ToString()),                                             
                            //                FormatString.ForMatNumber(xuatchuyenkho.ToString()), FormatString.ForMatNumber(tt_xuatchuyenkho.ToString()), 
                            //                FormatString.ForMatNumber(xuatkhac.ToString()), FormatString.ForMatNumber(tt_xuatkhac.ToString()), 
                            //                FormatString.ForMatNumber(xuatdoilobin.ToString()), FormatString.ForMatNumber(tt_xuatdoilobin.ToString()),
                            //                FormatString.ForMatNumber(xuatTieuHao.ToString()), FormatString.ForMatNumber(tt_xuatTieuHao.ToString()),
                            //                FormatString.ForMatNumber(tongXUAT.ToString()), FormatString.ForMatNumber(tt_tongXUAT.ToString()),   
                            //                FormatString.ForMatNumber(toncuoi.ToString()), FormatString.ForMatNumber(tt_toncuoi.ToString())  };

                            string[] data = new string[] { khoTen, chungloai, masp, tensp, dvt, xuatxu, location, solo, lotNo, serialNo, trangthai, 
                                            FormatString.ForMatNumber(tondauKS.ToString()), 
                                            FormatString.ForMatNumber(tondau.ToString()), 
                                            FormatString.ForMatNumber(nhapmuaNCC.ToString()), 
                                            FormatString.ForMatNumber(nhapdoilobin.ToString()), 
                                            //FormatString.ForMatNumber(kiemke.ToString()), 
                                            FormatString.ForMatNumber(tongNHAP.ToString()), 
                                            FormatString.ForMatNumber(xuathangBAN.ToString()), 
                                            //FormatString.ForMatNumber(trahangban.ToString()),
                                            FormatString.ForMatNumber(xuatdoilobin.ToString()),
                                            FormatString.ForMatNumber(tongXUAT.ToString()),   
                                            FormatString.ForMatNumber(toncuoi.ToString())  };

                            for (int j = 0; j < data.Length; j++)
                            {
                                TableCell cell = new TableCell();

                                //if (data[0].StartsWith("0"))
                                //data[0] = "'" + data[0];

                                if (j == 3)
                                    cell.Width = Unit.Parse("450");

                                if (j >= 9)
                                {
                                    cell.Width = Unit.Parse("100");
                                    cell.HorizontalAlign = HorizontalAlign.Right;
                                }

                                cell.Text = data[j];

                                //cell.Attributes.Add("style", @"mso-number-format:\@;");

                                row.Cells.Add(cell);
                            }

                            table.Rows.Add(row);
                        }
                    }

                    table.RenderControl(htmlWriter);
                    _exportContent = sb.ToString();
                }
            }
            return _exportContent;
        }
        private string ExportToExcel_NXT_LotNo(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string khoId = "";
            if (context.Request.QueryString["khoId"] != null)
                khoId = context.Request.QueryString["khoId"].ToString();

            //khoId = khoId.Substring(0, khoId.Length - 1);

            string nganhhangId = "";
            if (context.Request.QueryString["nganhhang"] != null)
                nganhhangId = context.Request.QueryString["nganhhang"].ToString();

            //string theogia = "0";
            //if (context.Request.QueryString["theogia"] != null)
            //    theogia = context.Request.QueryString["theogia"].ToString();

            //string xemtheo = "0";
            //if (context.Request.QueryString["xemtheo"] != null)
            //    xemtheo = context.Request.QueryString["xemtheo"].ToString();

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
                    header.ColumnSpan = 5;
                    header.Text = "REPORT STOCK IN - OUT FOLLOW LOTNO";
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
                    header.ColumnSpan = 5;
                    header.Text = "Date from: " + tungay + " to " + denngay;

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

                    string[] tieude = new string[] { "Warehouse", "Specification", "Part code", "Description", "Unit", "Lot No", "Type", 
                                                      "Stock closure", "Period " + tungay, "Stock in", "Total", "Stock out", "Total", "Ending stocks " + denngay };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;

                        //if (i == 10) //Tồn đầu
                        //    header.ColumnSpan = 2;
                        //if (i == 11) //Tồn đầu
                        //    header.ColumnSpan = 6;
                        //if (i == 11) //Nhập
                        //    header.ColumnSpan = 2;
                        //else if (i == 13) //Total
                        //    header.ColumnSpan = 4;
                        //else if (i == 14) //Xuất
                        //    header.ColumnSpan = 2;
                        //else if (i == 14) //Total
                        //    header.ColumnSpan = 2;
                        //else if (i == 16) //Ending stocks
                        //    header.ColumnSpan = 2;
                        //else
                        //    header.RowSpan = 2;

                        header.BackColor = Color.LightGray;
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    tieude = new string[] {"", "", "", "", "", "", "", "", 
                                        "",
                                        "",
                                        "",
                                        "",
                                        "", "" };

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

                    string query = "SELECT TOP(1) nam, thang, thangks  " +
                    "FROM " +
                    "( " +
                    "	SELECT nam, thang, '01-' + case when thang < 10 then '0' + cast(thang as varchar(10)) else cast(thang as varchar(10)) end + '-' + cast(nam as varchar(10)) as thangks " +
                    "	FROM KHOASOTHANG   " +
                    ") " +
                    "DATA " +
                    "WHERE CONVERT(datetime, thangks, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                    "ORDER BY nam desc, thang desc ";

                    DataTable dtKS = xl.ReadTable(query);

                    string thangks = "";
                    string namks = "";
                    int thangTIEPTHEO = 0;
                    int namTIEPTHEO = 0;
                    if (dtKS.Rows.Count > 0)
                    {
                        thangks = dtKS.Rows[0]["thang"].ToString();
                        namks = dtKS.Rows[0]["nam"].ToString();
                        thangTIEPTHEO = int.Parse(dtKS.Rows[0]["thang"].ToString());
                        namTIEPTHEO = int.Parse(dtKS.Rows[0]["nam"].ToString());

                        if (int.Parse(thangks) == 12)
                        {
                            thangTIEPTHEO = 1;
                            namTIEPTHEO += 1;
                        }
                        else
                            thangTIEPTHEO += 1;
                    }

                    string dauthang = "01-" + (thangTIEPTHEO < 10 ? "0" + thangTIEPTHEO.ToString() : thangTIEPTHEO.ToString()) + "-" + namTIEPTHEO.ToString();

                    string[] khoIds = Regex.Split(khoId, ",");
                    string[] thangBC = Regex.Split(tungay, "-");

                    for (int count = 0; count < khoIds.Length; count++)
                    {
                        khoId = khoIds[count];
                        string khoTen = xl.ExecuteScalarSQL("SELECT makho FROM KHO WHERE pk_seq = '" + khoIds[count] + "'").ToString();

                        //cureent dd-mm-yyyy ==> change to yyyy-mm-dd
                        //tungay = xl.convertToDate_YYYYMMDD(tungay);
                        //denngay = xl.convertToDate_YYYYMMDD(denngay);

                        query = " SELECT DISTINCT (SELECT ten FROM NganhHang WHERE pk_seq = sp.nganhhang_fk) nganhhang, ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') chungloai, " +
                            "    sp.ma AS masp, sp.nameEnglish AS tensp, (SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk) AS dvt, ISNULL(DAUKY.lotNo, '') lotNo, " +
                            "    CASE DAUKY.trangthai WHEN 0 THEN N'Ok'   " +
                            "                         WHEN 1 THEN 'Loos'  " +
                            "                         WHEN 2 THEN 'NG'  " +
                            "                         WHEN 3 THEN 'Test'  " +
                            "                         WHEN 4 THEN 'Orther' ELSE N'Unknow' END trangthai, " +
                            "    ISNULL(dauky.tondauKS, 0) AS tondauKS, ISNULL(dauky.soluong, 0) AS tondau, ISNULL(dauky.thanhtienTONDAU, 0) AS thanhtienTONDAU, " +
                            "    ISNULL(TRONGKY.nhapNCC, 0) nhapNCC, ISNULL(TRONGKY.traNCC, 0) traNCC, ISNULL(TRONGKY.kiemke, 0) kiemke, ISNULL(TRONGKY.dieuchinh, 0) dieuchinh, " +
                            "    ISNULL(TRONGKY.xuathangBAN, 0) xuathangBAN, ISNULL(TRONGKY.xuatKM, 0) xuatKM, 0 AS chiagia, ISNULL(TRONGKY.trahangban, 0) trahangban, ISNULL(TRONGKY.trahangKM, 0) trahangKM, " +
                            "    ISNULL(TRONGKY.nhapkhac, 0) nhapkhac, ISNULL(TRONGKY.xuatkhac, 0) xuatkhac, ISNULL(TRONGKY.xuattieuhao, 0) xuattieuhao, ISNULL(dauky.giamua, 0) giamua, ISNULL(dauky.giaban, 0) giaban, ISNULL(dauky.giaton, 0) giaton, " +
                            "    TRONGKY.thanhtienNhap, TRONGKY.thanhtienDCTK, TRONGKY.thanhtienKiemKe, TRONGKY.thanhtienNhapKhac, " +
                            "    TRONGKY.thanhtienNhapTRA, TRONGKY.thanhtienTKCG, TRONGKY.thanhtienTraBH, TRONGKY.thanhtienTraKM, " +
                            "    TRONGKY.thanhtienXuat, TRONGKY.thanhtienXuatKM, TRONGKY.thanhtienXuatKhac " +
                            "  FROM SanPham sp INNER JOIN " +
                            " ( " +
                            " 	SELECT DISTINCT '" + khoId + "' AS kho_fk, kho.sanpham_fk, kho.lotNo, kho.trangthai, ISNULL(tondau.soluong, 0) AS tondauKS,  " +
                            " 		ISNULL(tondau.soluong, 0) + ISNULL(nhap.soluong, 0) - ISNULL(tra.soluong, 0) + " +
                            " 		ISNULL(dieuchinh.soluong, 0) - ISNULL(xuathangban.soluong, 0) - ISNULL(xuatkhuyenmai.soluong, 0) + " +
                            " 		ISNULL(trahangban.soluong, 0) + ISNULL(trahangkhuyenmai.soluong, 0) + " +
                            " 		ISNULL(nhapkhac.soluong, 0) - ISNULL(xuatkhac.soluong, 0) - ISNULL(xuatTieuHao.soluong, 0) AS soluong, " +
                            " 		ISNULL( ( SELECT TOP(1) dongia FROM BangGiaMua_SanPham WHERE sanpham_fk = kho.sanpham_fk), 0 ) AS giamua, " +
                            " 		ISNULL( ( SELECT TOP(1) dongia FROM BangGiaBan_SanPham WHERE sanpham_fk = kho.sanpham_fk), 0 ) AS giaban, 1 AS giaton, ISNULL(tondau.thanhtienTONDAU, 0) AS thanhtienTONDAU " +
                            " 	FROM Kho_SanPham_ChiTiet kho " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'1.Tồn đầu' AS loaict, sanpham_fk, lotNo, trangthai, SUM(soluong) soluong, SUM(soluong * giaban) AS thanhtienTONDAU  " +
                            " 		FROM TonKhoThang_ChiTiet" +
                            " 		WHERE thang = '" + thangks + "' AND nam = '" + namks + "' AND kho_fk = '" + khoId + "'  " +
                            "       GROUP BY sanpham_fk, lotNo, trangthai " + 
                            " 	) " +
                            " 	tondau ON kho.sanpham_fk = tondau.sanpham_fk AND kho.lotNo = tondau.lotNo AND kho.trangthai = tondau.trangthai " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'2.1.Nhập hàng' AS loaict, b.sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.soluongQuyDoi) soluong, ISNULL(b.giamua , 0) gianhap " +
                            " 		FROM NhapHang a INNER JOIN NhapHang_SanPham_ChiTiet b ON a.pk_seq = b.nhaphang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaynhap, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            "         GROUP BY b.sanpham_fk, b.giamua, b.lotNo, a.loaihanghoa " +
                            " 	) " +
                            " 	nhap ON kho.sanpham_fk = nhap.sanpham_fk AND kho.lotNo = nhap.lotNo AND kho.trangthai = nhap.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'3.Trả hàng NCC' AS loaict, b.sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong * b.dongia) thanhtienNhapTRA " +
                            " 		FROM TraHang a INNER JOIN TraHang_SanPham_ChiTiet b ON a.pk_seq = b.trahang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaytra, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaytra, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.lotNo, a.loaihanghoa " +
                            " 	) " +
                            " 	tra ON kho.sanpham_fk = tra.sanpham_fk AND kho.lotNo = tra.lotNo AND kho.trangthai = tra.loaihanghoa " +
                            //" 	LEFT JOIN " +
                            //" 	( " +
                            //" 		SELECT N'4.Kiểm kho' AS loaict,  b.sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.soluongkiem - b.ton) AS soluong, SUM((b.soluongkiem - b.ton)*giamuatb) thanhtienKiemKho " +
                            //" 		FROM KiemKho a INNER JOIN KiemKho_SanPham b ON a.pk_seq = b.kiemkho_fk " +
                            //" 		WHERE CONVERT(datetime, a.ngaykiem, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            //" 				and CONVERT(datetime, a.ngaykiem, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            //" 		GROUP BY b.sanpham_fk, b.lotNo, a.loaihanghoa " +
                            //" 	) " +
                            //" 	kiemke ON kho.sanpham_fk = kiemke.sanpham_fk AND kho.lotNo = kiemke.lotNo AND kho.trangthai = kiemke.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'5.Điều chỉnh tồn kho' AS loaict, b.sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.dieuchinh) AS soluong, SUM(b.dieuchinh * b.giamuatb) thanhtienDCTK " +
                            " 		FROM DieuChinhTonKho a INNER JOIN DieuChinhTonKho_SanPham b ON a.pk_seq = b.dieuchinh_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaydieuchinh, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaydieuchinh, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.giamuatb, b.lotNo, a.loaihanghoa " +
                            " 	) " +
                            " 	dieuchinh ON kho.sanpham_fk = dieuchinh.sanpham_fk AND kho.lotNo = dieuchinh.lotNo AND kho.trangthai = dieuchinh.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'6.Stock out' AS loaict, b.sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.SOLUONGQUYDOI) AS soluong, 1 thanhtienXuat " +
                            "         FROM DonHang a INNER JOIN DonHang_SanPham_ChiTiet b ON a.pk_seq = b.donhang_FK  " +
                            "         WHERE CONVERT(datetime, a.ngaygiaohang, 105) >= CONVERT(datetime,'" + dauthang + "', 105) AND a.trangthai not in (0, 2) AND a.kho_fk = '" + khoId + "'  " +
                            "                 AND CONVERT(datetime, a.ngaygiaohang, 105) < CONVERT(datetime, '" + tungay + "', 105)   " +
                            "         GROUP BY b.sanpham_fk, b.lotNo, a.loaihanghoa " +
                            " 	) " +
                            " 	xuathangban ON kho.sanpham_fk = xuathangban.sanpham_fk AND kho.lotNo = xuathangban.lotNo AND kho.trangthai = xuathangban.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'6.Xuất khuyến mại' AS loaict, c.pk_seq AS sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.SOLUONG) AS soluong, SUM(b.TongGiaTri) thanhtienXuatKM " +
                            "         FROM DonHang a INNER JOIN DONHANG_CTKM_TRAKM b ON a.pk_seq = b.donhang_fk  " +
                            "                 INNER JOIN SANPHAM c ON b.spMA = c.MA  " +
                            "         WHERE CONVERT(datetime, a.ngaygiaohang, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai not in (0, 2) AND b.kho_fk = '" + khoId + "'  " +
                            "                 AND CONVERT(datetime, a.ngaygiaohang, 105) < CONVERT(datetime, '" + tungay + "', 105)   " +
                            "         GROUP BY c.pk_seq, b.lotNo, a.loaihanghoa " +
                            " 	) " +
                            " 	xuatkhuyenmai ON kho.sanpham_fk = xuatkhuyenmai.sanpham_fk AND kho.lotNo = xuatkhuyenmai.lotNo AND kho.trangthai = xuatkhuyenmai.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'10.Trả hàng khách hàng' AS loaict, b.sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.soluongQUYDOI) AS soluong, SUM(b.soluong*b.dongia) thanhtienTraKH " +
                            " 		FROM DonTraHang a INNER JOIN DonTraHang_SanPham_ChiTiet b ON a.pk_seq = b.dontrahang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaydonhang, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = 1 AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaydonhang, 105) < CONVERT(datetime, '" + dauthang + "', 105)   " +
                            " 		GROUP BY b.sanpham_fk, b.lotNo, a.loaihanghoa " +
                            "  ) " +
                            " 	trahangban ON kho.sanpham_fk = trahangban.sanpham_fk AND kho.lotNo = trahangban.lotNo AND kho.trangthai = trahangban.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'11.Trả hàng khuyến mại' AS loaict,  b.sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.soluong) AS soluong, SUM(b.soluong*b.dongia) AS thanhtienTraKHKM " +
                            " 		FROM DonTraHang a INNER JOIN DonTraHang_SPKM_ChiTiet b ON a.pk_seq = b.dontrahang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaydonhang, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = 1 AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaydonhang, 105) < CONVERT(datetime, '" + tungay + "', 105)  " +
                            " 		GROUP BY b.sanpham_fk, b.lotNo, a.loaihanghoa " +
                            " 	) " +
                            " 	trahangkhuyenmai ON kho.sanpham_fk = trahangkhuyenmai.sanpham_fk AND kho.lotNo = trahangkhuyenmai.lotNo AND kho.trangthai = trahangkhuyenmai.loaihanghoa " +
                            " LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'14.Nhập khác' AS loaict, b.sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.soluongQUYDOI) AS soluong, SUM(b.soluong*b.dongia) thanhtienNhapKhac " +
                            " 		FROM NhapKhac a INNER JOIN NhapKhac_SanPham_ChiTiet b ON a.pk_seq = b.nhapkhac_fk           " +
                            " 		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'   " +
                            " 				and CONVERT(datetime, a.ngaynhap, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.lotNo, a.loaihanghoa " +
                            " 	) " +
                            " 	nhapkhac ON kho.sanpham_fk = nhapkhac.sanpham_fk AND kho.lotNo = nhapkhac.lotNo AND kho.trangthai = nhapkhac.loaihanghoa " +
                            "    LEFT JOIN  " +
                            "    ( " +
                            " 		SELECT N'15.Xuất khác' AS loaict, b.sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienXuatKhac " +
                            " 		FROM XuatKhac a INNER JOIN XuatKhac_SanPham_ChiTiet b ON a.pk_seq = b.xuatkhac_fk " +
                            " 		WHERE CONVERT(datetime, a.ngayxuat, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'   " +
                            " 				and CONVERT(datetime, a.ngayxuat, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.lotNo, a.loaihanghoa " +
                            " 	) " +
                            " 	xuatkhac ON kho.sanpham_fk = xuatkhac.sanpham_fk AND kho.lotNo = xuatkhac.lotNo AND kho.trangthai = xuatkhac.loaihanghoa " +
                            "    LEFT JOIN  " +
                            "    (  " +
                            "    	SELECT N'19.Xuất tiêu hao nguyên liệu' AS loaict,  b.vattu_fk AS sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.soluongQUYDOI) AS soluong  " +
                            "    	FROM LenhSanXuat_TieuHao a INNER JOIN LenhSanXuat_TieuHao_ChiTiet b ON a.pk_seq = b.tieuhao_fk  " +
                            "    	WHERE CONVERT(datetime, a.ngaytieuhao, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'     " +
                            "    			and CONVERT(datetime, a.ngaytieuhao, 105) <= CONVERT(datetime, '" + tungay + "', 105) " +
                            "    	GROUP BY b.vattu_fk, b.lotNo, a.loaihanghoa " +
                            "    )  " +
                            "    xuatTieuHao ON kho.sanpham_fk = xuatTieuHao.sanpham_fk AND kho.lotNo = xuatTieuHao.lotNo AND kho.trangthai = xuatTieuHao.loaihanghoa " +
                            " 	WHERE kho.kho_fk = '" + khoId + "' " +                            
                            " ) " +
                            " DAUKY ON sp.pk_seq = DAUKY.sanpham_fk " +
                            " LEFT JOIN " +
                            " ( " +
                            " 	SELECT DISTINCT '" + khoId + "'  AS kho_fk, kho.sanpham_fk, kho.lotNo, kho.trangthai, " +
                            " 		ISNULL(nhap.soluong, 0) AS nhapNCC, ISNULL(nhap.thanhtienNhap, 0) thanhtienNhap, 		" +
                            " 		ISNULL(tra.soluong, 0) AS traNCC, ISNULL(tra.thanhtienNhapTRA, 0)  thanhtienNhapTRA, " +
                            " 		0 AS kiemke, 0 thanhtienKiemKe, ISNULL(dieuchinh.soluong, 0) AS dieuchinh, ISNULL(dieuchinh.thanhtienDCTK, 0) thanhtienDCTK, " +
                            " 		ISNULL(xuathangban.soluong, 0) AS xuathangBAN, ISNULL(xuathangban.thanhtienXuat, 0) thanhtienXuat, " +
                            " 		ISNULL(xuatkhuyenmai.soluong, 0) AS xuatKM, ISNULL(xuatkhuyenmai.thanhtienXuatKM, 0) thanhtienXuatKM, " +
                            " 		0 chiagia, 0 thanhtienTKCG, ISNULL(trahangban.soluong, 0) AS trahangban, ISNULL(trahangban.thanhtienTraBH, 0) AS thanhtienTraBH, " +
                            " 		ISNULL(trakhuyenmai.soluong, 0) AS trahangKM, ISNULL(trakhuyenmai.thanhtienTraKM, 0) AS thanhtienTraKM, " +
                            " 		ISNULL(nhapkhac.soluong, 0) AS nhapkhac, ISNULL(nhapkhac.thanhtienNhapKhac, 0) AS thanhtienNhapKhac, " +
                            " 		ISNULL(xuatkhac.soluong, 0) AS xuatkhac, ISNULL(xuatkhac.thanhtienXuatKhac, 0) AS thanhtienXuatKhac, ISNULL(xuatTieuHao.soluong, 0) AS xuattieuhao  " +
                            " 	FROM Kho_SanPham_ChiTiet kho " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'2.1.Nhập hàng' AS loaict,  b.sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.soluongQuyDoi) soluong, ISNULL(SUM(b.soluong * b.giamua), 0) thanhtienNhap " +
                            " 		FROM NhapHang a INNER JOIN NhapHang_SanPham_ChiTiet b ON a.pk_seq = b.nhaphang_fk " +
                            " 	    WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaynhap, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            "         GROUP BY b.sanpham_fk, b.lotNo, a.loaihanghoa " +
                            " 	) " +
                            " 	nhap ON kho.sanpham_fk = nhap.sanpham_fk AND kho.lotNo = nhap.lotNo AND kho.trangthai = nhap.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'3.Trả hàng NCC' AS loaict, b.sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienNhapTRA " +
                            " 		FROM TraHang a INNER JOIN TraHang_SanPham_ChiTiet b ON a.pk_seq = b.trahang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaytra, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaytra, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.lotNo, a.loaihanghoa " +
                            " 	) " +
                            " 	tra ON kho.sanpham_fk = tra.sanpham_fk AND kho.lotNo = tra.lotNo AND kho.trangthai = tra.loaihanghoa " +
                            //" 	LEFT JOIN " +
                            //" 	( " +
                            //" 		SELECT N'4.Kiểm kho' AS loaict,  b.sanpham_fk, b.lotNo, SUM(b.soluongkiem - b.ton) AS soluong, SUM((b.soluongkiem - b.ton) * giamuatb) thanhtienKiemKe " +
                            //" 		FROM KiemKho a INNER JOIN KiemKho_SanPham b ON a.pk_seq = b.kiemkho_fk " +
                            //" 		WHERE CONVERT(datetime, a.ngaykiem, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            //" 				and CONVERT(datetime, a.ngaykiem, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            //" 		GROUP BY b.sanpham_fk, b.lotNo " +
                            //" 	) " +
                            //" 	kiemke ON kho.sanpham_fk = kiemke.sanpham_fk AND kho.lotNo = kiemke.lotNo " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'5.Điều chỉnh tồn kho' AS loaict,  b.sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.dieuchinh) AS soluong, SUM(b.dieuchinh * b.giamuatb) thanhtienDCTK " +
                            " 		FROM DieuChinhTonKho a INNER JOIN DieuChinhTonKho_SanPham b ON a.pk_seq = b.dieuchinh_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaydieuchinh, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaydieuchinh, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.lotNo, a.loaihanghoa " +
                            " 	) " +
                            " 	dieuchinh ON kho.sanpham_fk = dieuchinh.sanpham_fk AND kho.lotNo = dieuchinh.lotNo  AND kho.trangthai = dieuchinh.loaihanghoa " +
                            " LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'6.Stock out' AS loaict, b.sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.SOLUONGQUYDOI) AS soluong, 1 thanhtienXuat " +
                            "         FROM DonHang a INNER JOIN DonHang_SanPham_ChiTiet b ON a.pk_seq = b.donhang_FK  " +
                            "         WHERE CONVERT(datetime, a.ngaygiaohang, 105) >= CONVERT(datetime,'" + tungay + "', 105) AND a.trangthai IN (1) AND a.kho_fk = '" + khoId + "'  " +
                            "                 AND CONVERT(datetime, a.ngaygiaohang, 105) <= CONVERT(datetime, '" + denngay + "', 105)   " +
                            "         GROUP BY b.sanpham_fk, b.lotNo, a.loaihanghoa " +
                            " 	) " +
                            " 	xuathangban ON kho.sanpham_fk = xuathangban.sanpham_fk AND kho.lotNo = xuathangban.lotNo AND kho.trangthai = xuathangban.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'6.Xuất khuyến mại' AS loaict, c.pk_seq AS sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.SOLUONG) AS soluong, SUM(b.TongGiaTri) thanhtienXuatKM  " +
                            "         FROM DonHang a INNER JOIN DONHANG_CTKM_TRAKM b ON a.pk_seq = b.donhang_fk  " +
                            "                 INNER JOIN SANPHAM c ON b.spMA = c.MA  " +
                            "         WHERE CONVERT(datetime, a.ngaygiaohang, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai IN (1) AND b.kho_fk = '" + khoId + "'  " +
                            "                 AND CONVERT(datetime, a.ngaygiaohang, 105) <= CONVERT(datetime, '" + denngay + "', 105)   " +
                            "         GROUP BY c.pk_seq, b.lotNo, a.loaihanghoa " +
                            " 	) " +
                            " 	xuatkhuyenmai ON kho.sanpham_fk = xuatkhuyenmai.sanpham_fk AND kho.lotNo = xuatkhuyenmai.lotNo AND kho.trangthai = xuatkhuyenmai.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'10.Trả hàng khách hàng' AS loaict,  b.sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.soluongQUYDOI) AS soluong, SUM(b.dongia*b.soluong) thanhtienTraBH " +
                            " 		FROM DonTraHang a INNER JOIN DonTraHang_SanPham_ChiTiet b ON a.pk_seq = b.dontrahang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaydonhang, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = 1 AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaydonhang, 105) <= CONVERT(datetime, '" + denngay + "', 105)   " +
                            " 		GROUP BY b.sanpham_fk, b.lotNo, a.loaihanghoa " +
                            " 	) " +
                            " trahangban ON kho.sanpham_fk = trahangban.sanpham_fk AND kho.lotNo = trahangban.lotNo AND kho.trangthai = trahangban.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'11.Trả hàng khuyến mại' AS loaict, b.sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.soluong) AS soluong, SUM(b.soluong*dongia) thanhtienTraKM " +
                            " 		FROM DonTraHang a INNER JOIN DonTraHang_SPKM_ChiTiet b ON a.pk_seq = b.dontrahang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaydonhang, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = 1 AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaydonhang, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                            " 		GROUP BY b.sanpham_fk, b.lotNo, a.loaihanghoa " +
                            " 	) " +
                            " 	trakhuyenmai ON kho.sanpham_fk = trakhuyenmai.sanpham_fk AND kho.lotNo = trakhuyenmai.lotNo AND kho.trangthai = trakhuyenmai.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'14.Nhập khác' AS loaict, b.sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienNhapKhac" +
                            " 		FROM NhapKhac a INNER JOIN NhapKhac_SanPham_ChiTiet b ON a.pk_seq = b.nhapkhac_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'   " +
                            " 				and CONVERT(datetime, a.ngaynhap, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.lotNo, a.loaihanghoa " +
                            " 	) " +
                            " 	nhapkhac ON kho.sanpham_fk = nhapkhac.sanpham_fk AND kho.lotNo = nhapkhac.lotNo AND kho.trangthai = nhapkhac.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'15.Xuất khác' AS loaict,  b.sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienXuatKhac " +
                            " 		FROM XuatKhac a INNER JOIN XuatKhac_SanPham_ChiTiet b ON a.pk_seq = b.xuatkhac_fk " +
                            " 		WHERE CONVERT(datetime, a.ngayxuat, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'   " +
                            " 				and CONVERT(datetime, a.ngayxuat, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.lotNo, a.loaihanghoa " +
                            " 	) " +
                            "   xuatkhac ON kho.sanpham_fk = xuatkhac.sanpham_fk AND kho.lotNo = xuatkhac.lotNo AND kho.trangthai = xuatkhac.loaihanghoa " +
                            "    LEFT JOIN  " +
                            "    (  " +
                            "    	SELECT N'19.Xuất tiêu hao nguyên liệu' AS loaict,  b.vattu_fk AS sanpham_fk, b.lotNo, a.loaihanghoa, SUM(b.soluongQUYDOI) AS soluong  " +
                            "    	FROM LenhSanXuat_TieuHao a INNER JOIN LenhSanXuat_TieuHao_ChiTiet b ON a.pk_seq = b.tieuhao_fk  " +
                            "    	WHERE CONVERT(datetime, a.ngaytieuhao, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'     " +
                            "    			and CONVERT(datetime, a.ngaytieuhao, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            "    	GROUP BY b.vattu_fk, b.lotNo, a.loaihanghoa " +
                            "    )  " +
                            "    xuatTieuHao ON kho.sanpham_fk = xuatTieuHao.sanpham_fk AND kho.lotNo = xuatTieuHao.lotNo AND kho.trangthai = xuatTieuHao.loaihanghoa " +
                            " 	WHERE kho.kho_fk = '" + khoId + "'  " +
                            " ) " +
                            " TRONGKY ON DAUKY.sanpham_fk = TRONGKY.sanpham_fk AND DAUKY.lotNo = TRONGKY.lotNo AND DAUKY.trangthai = TRONGKY.trangthai " +
                            //"  INNER JOIN NganhHang nh ON sp.nganhhang_fk = nh.pk_seq " +
                            //"  LEFT JOIN ChungLoai cl ON sp.chungloai_fk = cl.pk_seq " +
                            "  WHERE 1 = 1 ";

                        //if (nganhhangId.Trim().Length > 0)
                        //    query += " AND sp.nganhhang in ( SELECT ten FROM NGANHHANG WHERE pk_seq = '" + nganhhangId + "' ) ";

                        DataTable dt = xl.ReadTable(query);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TableRow row = new TableRow();

                            string nganhhang = dt.Rows[i]["nganhhang"].ToString();
                            string chungloai = dt.Rows[i]["chungloai"].ToString();
                            string masp = dt.Rows[i]["masp"].ToString();
                            string tensp = dt.Rows[i]["tensp"].ToString();
                            string dvt = dt.Rows[i]["dvt"].ToString();
                            string trangthai = dt.Rows[i]["trangthai"].ToString();

                            //string location = dt.Rows[i]["location"].ToString();
                            //string bin = dt.Rows[i]["bin"].ToString();
                            //string xuatxu = dt.Rows[i]["xuatxu"].ToString();
                            string lotNo = dt.Rows[i]["lotNo"].ToString();
                            //string serialNo = dt.Rows[i]["serialNo"].ToString();
                            //string solo = dt.Rows[i]["solo"].ToString();
                            //string ngaynhap = dt.Rows[i]["ngaynhap"].ToString();

                            //double giamua = double.Parse(dt.Rows[i]["giamua"].ToString());
                            //double giaban = double.Parse(dt.Rows[i]["giaban"].ToString());
                            //double giaton = double.Parse(dt.Rows[i]["giaton"].ToString());

                            //if (theogia.Equals("0"))  //XEM THEO GIA MUA
                            //    giaban = giamua;
                            //else if (theogia.Equals("0"))  //XEM THEO GIA BAN
                            //    giamua = giaban;
                            //else
                            // giamua = giaton;

                            double tondauKS = double.Parse(dt.Rows[i]["tondauKS"].ToString());

                            double tondau = double.Parse(dt.Rows[i]["tondau"].ToString());
                            //double tt_tondau = double.Parse(dt.Rows[i]["thanhtienTONDAU"].ToString());

                            double nhapmuaNCC = double.Parse(dt.Rows[i]["nhapNCC"].ToString());
                            //double tt_nhapmuaNCC = double.Parse(dt.Rows[i]["thanhtienNhap"].ToString());

                            double kiemke = double.Parse(dt.Rows[i]["kiemke"].ToString());
                            //double tt_kiemke = double.Parse(dt.Rows[i]["thanhtienKiemKe"].ToString());

                            //double dieuchinh = double.Parse(dt.Rows[i]["dieuchinh"].ToString());
                            //double tt_dieuchinh = double.Parse(dt.Rows[i]["thanhtienDCTK"].ToString());

                            //double thuhoiBAN = double.Parse(dt.Rows[i]["thuhoiBAN"].ToString());
                            //double tt_thuhoiBAN = double.Parse(dt.Rows[i]["thanhtienTHBH"].ToString());

                            //double thuhoiKM = double.Parse(dt.Rows[i]["thuhoiKM"].ToString());
                            //double tt_thuhoiKM = double.Parse(dt.Rows[i]["thanhtienTHKM"].ToString());

                            double trahangban = double.Parse(dt.Rows[i]["trahangban"].ToString());
                            //double tt_trahangban = double.Parse(dt.Rows[i]["thanhtienTraBH"].ToString());

                            //double trahangKM = double.Parse(dt.Rows[i]["trahangKM"].ToString());
                            //double tt_trahangKM = double.Parse(dt.Rows[i]["thanhtienTraKM"].ToString());

                            //double nhapchuyenkho = double.Parse(dt.Rows[i]["nhapchuyenkho"].ToString());
                            //double tt_nhapchuyenkho = double.Parse(dt.Rows[i]["thanhtienNhapKhacCK"].ToString());

                            //double nhapkhac = double.Parse(dt.Rows[i]["nhapkhac"].ToString());
                            //double tt_nhapkhac = double.Parse(dt.Rows[i]["thanhtienNhapKhac"].ToString());

                            //double nhapdoilobin = double.Parse(dt.Rows[i]["nhapdoilobin"].ToString());
                            //double tt_nhapdoilobin = double.Parse(dt.Rows[i]["thanhtienNhapDOILOBIN"].ToString());

                            //double nhaplsx = double.Parse(dt.Rows[i]["nhaplsx"].ToString());
                            //double tt_nhaplsx = double.Parse(dt.Rows[i]["thanhtienNhapLSX"].ToString());

                            //XUAT
                            double xuathangBAN = double.Parse(dt.Rows[i]["xuathangBAN"].ToString());
                            //double tt_xuathangBAN = double.Parse(dt.Rows[i]["thanhtienXuat"].ToString());

                            //THEO DON VI
                            //double XUAT_soluongCHUAN = double.Parse(dt.Rows[i]["XUAT_soluongCHUAN"].ToString());
                            //double XUAT_soluongQD = double.Parse(dt.Rows[i]["XUAT_soluongQD"].ToString());

                            //double xuathangKM = double.Parse(dt.Rows[i]["xuatKM"].ToString());
                            //double tt_xuathangKM = double.Parse(dt.Rows[i]["thanhtienXuatKM"].ToString());

                            double trahangNCC = double.Parse(dt.Rows[i]["traNCC"].ToString());
                            //double tt_trahangNCC = double.Parse(dt.Rows[i]["thanhtienNhapTRA"].ToString());

                            //double chiagia = double.Parse(dt.Rows[i]["chiagia"].ToString());
                            //double tt_chiagia = double.Parse(dt.Rows[i]["thanhtienTKCG"].ToString());

                            //double xuatkhac = double.Parse(dt.Rows[i]["xuatkhac"].ToString());
                            //double tt_xuatkhac = double.Parse(dt.Rows[i]["thanhtienXuatKhac"].ToString());

                            //double xuatchuyenkho = double.Parse(dt.Rows[i]["xuatchuyenkho"].ToString());
                            //double tt_xuatchuyenkho = double.Parse(dt.Rows[i]["thanhtienXuatKhacCK"].ToString());

                            //double xuatdoilobin = double.Parse(dt.Rows[i]["xuatdoilobin"].ToString());
                            //double tt_xuatdoilobin = double.Parse(dt.Rows[i]["thanhtienXuatDOILOBIN"].ToString());

                            //double xuatTieuHao = double.Parse(dt.Rows[i]["xuatTieuHao"].ToString());
                            //double tt_xuatTieuHao = double.Parse(dt.Rows[i]["thanhtienXuatTieuHao"].ToString());

                            tondauKS = Math.Round(tondauKS, 3);
                            tondau = Math.Round(tondau, 3);

                            nhapmuaNCC = Math.Round(nhapmuaNCC, 3);
                            kiemke = Math.Round(kiemke, 3);
                            //dieuchinh = Math.Round(dieuchinh, 3);
                            trahangNCC = Math.Round(trahangNCC, 3);
                            //nhapkhac = Math.Round(nhapkhac, 3);
                            //nhapchuyenkho = Math.Round(nhapchuyenkho, 3);
                            //dieuchinh = Math.Round(dieuchinh, 3);
                            //nhapdoilobin = Math.Round(nhapdoilobin, 3);
                            //nhaplsx = Math.Round(nhaplsx, 3);

                            xuathangBAN = Math.Round(xuathangBAN, 3);
                            //xuathangKM = Math.Round(xuathangKM, 3);
                            //chiagia = Math.Round(chiagia, 3);
                            //xuatkhac = Math.Round(xuatkhac, 3);
                            //xuatchuyenkho = Math.Round(xuatchuyenkho, 3);
                            //xuatdoilobin = Math.Round(xuatdoilobin, 3);
                            //xuatTieuHao = Math.Round(xuatTieuHao, 3);
                            //thuhoiBAN = Math.Round(thuhoiBAN, 3);
                            //thuhoiKM = Math.Round(thuhoiKM, 3);
                            trahangban = Math.Round(trahangban, 3);
                            //trahangKM = Math.Round(trahangKM, 3);

                            //double tongNHAP = nhapmuaNCC + kiemke + dieuchinh - trahangNCC + nhapkhac +
                            //   nhapchuyenkho + nhapdoilobin + nhaplsx;
                            //double tt_tongNHAP = tt_nhapmuaNCC + tt_kiemke + tt_dieuchinh - tt_trahangNCC + tt_nhapchuyenkho + tt_nhapkhac + tt_nhapdoilobin + tt_nhaplsx;


                            //double tongXUAT = xuathangBAN + xuathangKM + chiagia + xuatkhac + xuatchuyenkho + xuatdoilobin + xuatTieuHao - thuhoiBAN - thuhoiKM - trahangban - trahangKM;
                            //double tt_tongXUAT = tt_xuathangBAN + tt_xuathangKM + tt_chiagia + tt_xuatkhac + tt_xuatchuyenkho + tt_xuatdoilobin + tt_xuatTieuHao -
                            //    tt_thuhoiBAN - tt_thuhoiKM - tt_trahangban - tt_trahangKM;

                            double tongNHAP = nhapmuaNCC + kiemke - trahangNCC;
                            //double tt_tongNHAP = tt_nhapmuaNCC + tt_kiemke  - tt_trahangNCC;

                            double tongXUAT = xuathangBAN - trahangban;
                            //double tt_tongXUAT = tt_xuathangBAN - tt_trahangban;

                            double toncuoi = tondau + tongNHAP - tongXUAT;
                            //double tt_toncuoi = giamua * toncuoi;

                            //string[] data = new string[] { khoTen, nganhhang, masp, tensp, dvt, xuatxu, location, bin, solo, ngaynhap, 
                            //                FormatString.ForMatNumber(tondauKS.ToString()), "", 
                            //                FormatString.ForMatNumber(tondau.ToString()), FormatString.ForMatNumber(tt_tondau.ToString()), 
                            //                FormatString.ForMatNumber(nhapmuaNCC.ToString()), FormatString.ForMatNumber(tt_nhapmuaNCC.ToString()), 
                            //                FormatString.ForMatNumber(trahangNCC.ToString()), FormatString.ForMatNumber(tt_trahangNCC.ToString()), 
                            //                FormatString.ForMatNumber(kiemke.ToString()), FormatString.ForMatNumber(tt_kiemke.ToString()), 
                            //                FormatString.ForMatNumber(dieuchinh.ToString()), FormatString.ForMatNumber(tt_dieuchinh.ToString()),                                           
                            //                FormatString.ForMatNumber(nhapchuyenkho.ToString()), FormatString.ForMatNumber(tt_nhapchuyenkho.ToString()),  
                            //                FormatString.ForMatNumber(nhapkhac.ToString()), FormatString.ForMatNumber(tt_nhapkhac.ToString()), 
                            //                FormatString.ForMatNumber(nhapdoilobin.ToString()), FormatString.ForMatNumber(tt_nhapdoilobin.ToString()),
                            //                FormatString.ForMatNumber(nhaplsx.ToString()), FormatString.ForMatNumber(tt_nhaplsx.ToString()),
                            //                FormatString.ForMatNumber(tongNHAP.ToString()), FormatString.ForMatNumber(tt_tongNHAP.ToString()), 
                            //                FormatString.ForMatNumber(xuathangBAN.ToString()), FormatString.ForMatNumber(tt_xuathangBAN.ToString()), 
                            //                FormatString.ForMatNumber(trahangban.ToString()), FormatString.ForMatNumber(tt_trahangban.ToString()),                                             
                            //                FormatString.ForMatNumber(xuatchuyenkho.ToString()), FormatString.ForMatNumber(tt_xuatchuyenkho.ToString()), 
                            //                FormatString.ForMatNumber(xuatkhac.ToString()), FormatString.ForMatNumber(tt_xuatkhac.ToString()), 
                            //                FormatString.ForMatNumber(xuatdoilobin.ToString()), FormatString.ForMatNumber(tt_xuatdoilobin.ToString()),
                            //                FormatString.ForMatNumber(xuatTieuHao.ToString()), FormatString.ForMatNumber(tt_xuatTieuHao.ToString()),
                            //                FormatString.ForMatNumber(tongXUAT.ToString()), FormatString.ForMatNumber(tt_tongXUAT.ToString()),   
                            //                FormatString.ForMatNumber(toncuoi.ToString()), FormatString.ForMatNumber(tt_toncuoi.ToString())  };

                            string[] data = new string[] { khoTen, chungloai, masp, tensp, dvt, lotNo, trangthai, 
                                            FormatString.ForMatNumber(tondauKS.ToString()),
                                            FormatString.ForMatNumber(tondau.ToString()),
                                            FormatString.ForMatNumber(nhapmuaNCC.ToString()), 
                                            //FormatString.ForMatNumber(trahangNCC.ToString()), 
                                            //FormatString.ForMatNumber(kiemke.ToString()),
                                            FormatString.ForMatNumber(tongNHAP.ToString()),
                                            FormatString.ForMatNumber(xuathangBAN.ToString()),
                                            //FormatString.ForMatNumber(trahangban.ToString()),
                                            FormatString.ForMatNumber(tongXUAT.ToString()),
                                            FormatString.ForMatNumber(toncuoi.ToString())  };

                            for (int j = 0; j < data.Length; j++)
                            {
                                TableCell cell = new TableCell();

                                //if (data[0].StartsWith("0"))
                                //data[0] = "'" + data[0];

                                if (j == 3)
                                    cell.Width = Unit.Parse("450");

                                if (j >= 6)
                                {
                                    cell.Width = Unit.Parse("100");
                                    cell.HorizontalAlign = HorizontalAlign.Right;
                                }

                                cell.Text = data[j];

                                //cell.Attributes.Add("style", @"mso-number-format:\@;");

                                row.Cells.Add(cell);
                            }

                            table.Rows.Add(row);
                        }
                    }

                    table.RenderControl(htmlWriter);
                    _exportContent = sb.ToString();
                }
            }
            return _exportContent;
        }
        private string ExportToExcel_NXT_Location(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string khoId = "";
            if (context.Request.QueryString["khoId"] != null)
                khoId = context.Request.QueryString["khoId"].ToString();

            //khoId = khoId.Substring(0, khoId.Length - 1);

            string nganhhangId = "";
            if (context.Request.QueryString["nganhhang"] != null)
                nganhhangId = context.Request.QueryString["nganhhang"].ToString();

            //string theogia = "0";
            //if (context.Request.QueryString["theogia"] != null)
            //    theogia = context.Request.QueryString["theogia"].ToString();

            //string xemtheo = "0";
            //if (context.Request.QueryString["xemtheo"] != null)
            //    xemtheo = context.Request.QueryString["xemtheo"].ToString();

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
                    header.ColumnSpan = 5;
                    header.Text = "REPORT STOCK IN - OUT FOLLOW LOCATION";
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
                    header.ColumnSpan = 5;
                    header.Text = "Date from: " + tungay + " to " + denngay;

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

                    string[] tieude = new string[] { "Warehouse", "Specification", "Part code", "Description", "Unit", "Location", "Type",
                                                      "Stock closure", "Period " + tungay, "Stock in", "Total", "Stock out", "Total", "Ending stocks " + denngay };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;

                        //if (i == 10) //Tồn đầu
                        //    header.ColumnSpan = 2;
                        //if (i == 11) //Tồn đầu
                        //    header.ColumnSpan = 6;
                        //if (i == 11) //Nhập
                        //    header.ColumnSpan = 2;
                        //else if (i == 13) //Total
                        //    header.ColumnSpan = 4;
                        //else if (i == 14) //Xuất
                        //    header.ColumnSpan = 2;
                        //else if (i == 14) //Total
                        //    header.ColumnSpan = 2;
                        //else if (i == 16) //Ending stocks
                        //    header.ColumnSpan = 2;
                        //else
                        //    header.RowSpan = 2;

                        header.BackColor = Color.LightGray;
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    tieude = new string[] {"", "", "", "", "", "", "", "",
                                        "",
                                        "",
                                        "",
                                        "",
                                        "", "" };

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

                    string query = "SELECT TOP(1) nam, thang, thangks  " +
                    "FROM " +
                    "( " +
                    "	SELECT nam, thang, '01-' + case when thang < 10 then '0' + cast(thang as varchar(10)) else cast(thang as varchar(10)) end + '-' + cast(nam as varchar(10)) as thangks " +
                    "	FROM KHOASOTHANG   " +
                    ") " +
                    "DATA " +
                    "WHERE CONVERT(datetime, thangks, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                    "ORDER BY nam desc, thang desc ";

                    DataTable dtKS = xl.ReadTable(query);

                    string thangks = "";
                    string namks = "";
                    int thangTIEPTHEO = 0;
                    int namTIEPTHEO = 0;
                    if (dtKS.Rows.Count > 0)
                    {
                        thangks = dtKS.Rows[0]["thang"].ToString();
                        namks = dtKS.Rows[0]["nam"].ToString();
                        thangTIEPTHEO = int.Parse(dtKS.Rows[0]["thang"].ToString());
                        namTIEPTHEO = int.Parse(dtKS.Rows[0]["nam"].ToString());

                        if (int.Parse(thangks) == 12)
                        {
                            thangTIEPTHEO = 1;
                            namTIEPTHEO += 1;
                        }
                        else
                            thangTIEPTHEO += 1;
                    }

                    string dauthang = "01-" + (thangTIEPTHEO < 10 ? "0" + thangTIEPTHEO.ToString() : thangTIEPTHEO.ToString()) + "-" + namTIEPTHEO.ToString();

                    string[] khoIds = Regex.Split(khoId, ",");
                    string[] thangBC = Regex.Split(tungay, "-");

                    for (int count = 0; count < khoIds.Length; count++)
                    {
                        khoId = khoIds[count];
                        string khoTen = xl.ExecuteScalarSQL("SELECT makho FROM KHO WHERE pk_seq = '" + khoIds[count] + "'").ToString();

                        //cureent dd-mm-yyyy ==> change to yyyy-mm-dd
                        //tungay = xl.convertToDate_YYYYMMDD(tungay);
                        //denngay = xl.convertToDate_YYYYMMDD(denngay);

                        query = " SELECT DISTINCT (SELECT ten FROM NganhHang WHERE pk_seq = sp.nganhhang_fk) nganhhang, ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') chungloai, " +
                            "    sp.ma AS masp, sp.nameEnglish AS tensp, (SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk) AS dvt, ISNULL(loc.ten, '') location, " +
                            "    CASE DAUKY.trangthai WHEN 0 THEN N'Ok'   " +
                            "                         WHEN 1 THEN 'Loos'  " +
                            "                         WHEN 2 THEN 'NG'  " +
                            "                         WHEN 3 THEN 'Test'  " +
                            "                         WHEN 4 THEN 'Orther' ELSE N'Unknow' END trangthai, " +
                            "    ISNULL(dauky.tondauKS, 0) AS tondauKS, ISNULL(dauky.soluong, 0) AS tondau, ISNULL(dauky.thanhtienTONDAU, 0) AS thanhtienTONDAU, " +
                            "    ISNULL(TRONGKY.nhapNCC, 0) nhapNCC, ISNULL(TRONGKY.traNCC, 0) traNCC, ISNULL(TRONGKY.kiemke, 0) kiemke, ISNULL(TRONGKY.dieuchinh, 0) dieuchinh, " +
                            "    ISNULL(TRONGKY.xuathangBAN, 0) xuathangBAN, ISNULL(TRONGKY.xuatKM, 0) xuatKM, 0 AS chiagia, ISNULL(TRONGKY.trahangban, 0) trahangban, ISNULL(TRONGKY.trahangKM, 0) trahangKM, " +
                            "    ISNULL(TRONGKY.nhapkhac, 0) nhapkhac, ISNULL(TRONGKY.xuatkhac, 0) xuatkhac, ISNULL(TRONGKY.xuattieuhao, 0) xuattieuhao, ISNULL(dauky.giamua, 0) giamua, ISNULL(dauky.giaban, 0) giaban, ISNULL(dauky.giaton, 0) giaton, " +
                            "    TRONGKY.thanhtienNhap, TRONGKY.thanhtienDCTK, TRONGKY.thanhtienKiemKe, TRONGKY.thanhtienNhapKhac, " +
                            "    TRONGKY.thanhtienNhapTRA, TRONGKY.thanhtienTKCG, TRONGKY.thanhtienTraBH, TRONGKY.thanhtienTraKM, " +
                            "    TRONGKY.thanhtienXuat, TRONGKY.thanhtienXuatKM, TRONGKY.thanhtienXuatKhac " +
                            "  FROM SanPham sp INNER JOIN " +
                            " ( " +
                            " 	SELECT DISTINCT '" + khoId + "' AS kho_fk, kho.sanpham_fk, kho.location_fk, kho.trangthai, ISNULL(tondau.soluong, 0) AS tondauKS,  " +
                            " 		ISNULL(tondau.soluong, 0) + ISNULL(nhap.soluong, 0) - ISNULL(tra.soluong, 0) + " +
                            " 		ISNULL(dieuchinh.soluong, 0) - ISNULL(xuathangban.soluong, 0) - ISNULL(xuatkhuyenmai.soluong, 0) + " +
                            " 		ISNULL(trahangban.soluong, 0) + ISNULL(trahangkhuyenmai.soluong, 0) + " +
                            " 		ISNULL(nhapkhac.soluong, 0) - ISNULL(xuatkhac.soluong, 0) - ISNULL(xuatTieuHao.soluong, 0) AS soluong, " +
                            " 		ISNULL( ( SELECT TOP(1) dongia FROM BangGiaMua_SanPham WHERE sanpham_fk = kho.sanpham_fk), 0 ) AS giamua, " +
                            " 		ISNULL( ( SELECT TOP(1) dongia FROM BangGiaBan_SanPham WHERE sanpham_fk = kho.sanpham_fk), 0 ) AS giaban, 1 AS giaton, ISNULL(tondau.thanhtienTONDAU, 0) AS thanhtienTONDAU " +
                            " 	FROM Kho_SanPham_ChiTiet kho " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'1.Tồn đầu' AS loaict, sanpham_fk, location_fk, trangthai, SUM(soluong) soluong, SUM(soluong * giaban) AS thanhtienTONDAU  " +
                            " 		FROM TonKhoThang_ChiTiet" +
                            " 		WHERE thang = '" + thangks + "' AND nam = '" + namks + "' AND kho_fk = '" + khoId + "'  " +
                            "       GROUP BY sanpham_fk, location_fk, trangthai " +
                            " 	) " +
                            " 	tondau ON kho.sanpham_fk = tondau.sanpham_fk AND kho.location_fk = tondau.location_fk AND kho.trangthai = tondau.trangthai " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'2.1.Nhập hàng' AS loaict, b.sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.soluongQuyDoi) soluong, ISNULL(b.giamua , 0) gianhap " +
                            " 		FROM NhapHang a INNER JOIN NhapHang_SanPham_ChiTiet b ON a.pk_seq = b.nhaphang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaynhap, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            "         GROUP BY b.sanpham_fk, b.giamua, b.location_fk, a.loaihanghoa " +
                            " 	) " +
                            " 	nhap ON kho.sanpham_fk = nhap.sanpham_fk AND kho.location_fk = nhap.location_fk AND kho.trangthai = nhap.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'3.Trả hàng NCC' AS loaict, b.sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong * b.dongia) thanhtienNhapTRA " +
                            " 		FROM TraHang a INNER JOIN TraHang_SanPham_ChiTiet b ON a.pk_seq = b.trahang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaytra, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaytra, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.location_fk, a.loaihanghoa " +
                            " 	) " +
                            " 	tra ON kho.sanpham_fk = tra.sanpham_fk AND kho.location_fk = tra.location_fk AND kho.trangthai = tra.loaihanghoa " +
                            //" 	LEFT JOIN " +
                            //" 	( " +
                            //" 		SELECT N'4.Kiểm kho' AS loaict,  b.sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.soluongkiem - b.ton) AS soluong, SUM((b.soluongkiem - b.ton)*giamuatb) thanhtienKiemKho " +
                            //" 		FROM KiemKho a INNER JOIN KiemKho_SanPham b ON a.pk_seq = b.kiemkho_fk " +
                            //" 		WHERE CONVERT(datetime, a.ngaykiem, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            //" 				and CONVERT(datetime, a.ngaykiem, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            //" 		GROUP BY b.sanpham_fk, b.location_fk, a.loaihanghoa " +
                            //" 	) " +
                            //" 	kiemke ON kho.sanpham_fk = kiemke.sanpham_fk AND kho.location_fk = kiemke.location_fk AND kho.trangthai = kiemke.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'5.Điều chỉnh tồn kho' AS loaict, b.sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.dieuchinh) AS soluong, SUM(b.dieuchinh * b.giamuatb) thanhtienDCTK " +
                            " 		FROM DieuChinhTonKho a INNER JOIN DieuChinhTonKho_SanPham b ON a.pk_seq = b.dieuchinh_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaydieuchinh, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaydieuchinh, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.giamuatb, b.location_fk, a.loaihanghoa " +
                            " 	) " +
                            " 	dieuchinh ON kho.sanpham_fk = dieuchinh.sanpham_fk AND kho.location_fk = dieuchinh.location_fk AND kho.trangthai = dieuchinh.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'6.Stock out' AS loaict, b.sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.SOLUONGQUYDOI) AS soluong, 1 thanhtienXuat " +
                            "         FROM DonHang a INNER JOIN DonHang_SanPham_ChiTiet b ON a.pk_seq = b.donhang_FK  " +
                            "         WHERE CONVERT(datetime, a.ngaygiaohang, 105) >= CONVERT(datetime,'" + dauthang + "', 105) AND a.trangthai not in (0, 2) AND a.kho_fk = '" + khoId + "'  " +
                            "                 AND CONVERT(datetime, a.ngaygiaohang, 105) < CONVERT(datetime, '" + tungay + "', 105)   " +
                            "         GROUP BY b.sanpham_fk, b.location_fk, a.loaihanghoa " +
                            " 	) " +
                            " 	xuathangban ON kho.sanpham_fk = xuathangban.sanpham_fk AND kho.location_fk = xuathangban.location_fk AND kho.trangthai = xuathangban.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'6.Xuất khuyến mại' AS loaict, c.pk_seq AS sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.SOLUONG) AS soluong, SUM(b.TongGiaTri) thanhtienXuatKM " +
                            "         FROM DonHang a INNER JOIN DONHANG_CTKM_TRAKM b ON a.pk_seq = b.donhang_fk  " +
                            "                 INNER JOIN SANPHAM c ON b.spMA = c.MA  " +
                            "         WHERE CONVERT(datetime, a.ngaygiaohang, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai not in (0, 2) AND b.kho_fk = '" + khoId + "'  " +
                            "                 AND CONVERT(datetime, a.ngaygiaohang, 105) < CONVERT(datetime, '" + tungay + "', 105)   " +
                            "         GROUP BY c.pk_seq, b.location_fk, a.loaihanghoa " +
                            " 	) " +
                            " 	xuatkhuyenmai ON kho.sanpham_fk = xuatkhuyenmai.sanpham_fk AND kho.location_fk = xuatkhuyenmai.location_fk AND kho.trangthai = xuatkhuyenmai.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'10.Trả hàng khách hàng' AS loaict, b.sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.soluongQUYDOI) AS soluong, SUM(b.soluong*b.dongia) thanhtienTraKH " +
                            " 		FROM DonTraHang a INNER JOIN DonTraHang_SanPham_ChiTiet b ON a.pk_seq = b.dontrahang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaydonhang, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = 1 AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaydonhang, 105) < CONVERT(datetime, '" + dauthang + "', 105)   " +
                            " 		GROUP BY b.sanpham_fk, b.location_fk, a.loaihanghoa " +
                            "  ) " +
                            " 	trahangban ON kho.sanpham_fk = trahangban.sanpham_fk AND kho.location_fk = trahangban.location_fk AND kho.trangthai = trahangban.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'11.Trả hàng khuyến mại' AS loaict,  b.sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.soluong) AS soluong, SUM(b.soluong*b.dongia) AS thanhtienTraKHKM " +
                            " 		FROM DonTraHang a INNER JOIN DonTraHang_SPKM_ChiTiet b ON a.pk_seq = b.dontrahang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaydonhang, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = 1 AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaydonhang, 105) < CONVERT(datetime, '" + tungay + "', 105)  " +
                            " 		GROUP BY b.sanpham_fk, b.location_fk, a.loaihanghoa " +
                            " 	) " +
                            " 	trahangkhuyenmai ON kho.sanpham_fk = trahangkhuyenmai.sanpham_fk AND kho.location_fk = trahangkhuyenmai.location_fk AND kho.trangthai = trahangkhuyenmai.loaihanghoa " +
                            " LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'14.Nhập khác' AS loaict, b.sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.soluongQUYDOI) AS soluong, SUM(b.soluong*b.dongia) thanhtienNhapKhac " +
                            " 		FROM NhapKhac a INNER JOIN NhapKhac_SanPham_ChiTiet b ON a.pk_seq = b.nhapkhac_fk           " +
                            " 		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'   " +
                            " 				and CONVERT(datetime, a.ngaynhap, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.location_fk, a.loaihanghoa " +
                            " 	) " +
                            " 	nhapkhac ON kho.sanpham_fk = nhapkhac.sanpham_fk AND kho.location_fk = nhapkhac.location_fk AND kho.trangthai = nhapkhac.loaihanghoa " +
                            "    LEFT JOIN  " +
                            "    ( " +
                            " 		SELECT N'15.Xuất khác' AS loaict, b.sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienXuatKhac " +
                            " 		FROM XuatKhac a INNER JOIN XuatKhac_SanPham_ChiTiet b ON a.pk_seq = b.xuatkhac_fk " +
                            " 		WHERE CONVERT(datetime, a.ngayxuat, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'   " +
                            " 				and CONVERT(datetime, a.ngayxuat, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.location_fk, a.loaihanghoa " +
                            " 	) " +
                            " 	xuatkhac ON kho.sanpham_fk = xuatkhac.sanpham_fk AND kho.location_fk = xuatkhac.location_fk AND kho.trangthai = xuatkhac.loaihanghoa " +
                            "    LEFT JOIN  " +
                            "    (  " +
                            "    	SELECT N'19.Xuất tiêu hao nguyên liệu' AS loaict,  b.vattu_fk AS sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.soluongQUYDOI) AS soluong  " +
                            "    	FROM LenhSanXuat_TieuHao a INNER JOIN LenhSanXuat_TieuHao_ChiTiet b ON a.pk_seq = b.tieuhao_fk  " +
                            "    	WHERE CONVERT(datetime, a.ngaytieuhao, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'     " +
                            "    			and CONVERT(datetime, a.ngaytieuhao, 105) <= CONVERT(datetime, '" + tungay + "', 105) " +
                            "    	GROUP BY b.vattu_fk, b.location_fk, a.loaihanghoa " +
                            "    )  " +
                            "    xuatTieuHao ON kho.sanpham_fk = xuatTieuHao.sanpham_fk AND kho.location_fk = xuatTieuHao.location_fk AND kho.trangthai = xuatTieuHao.loaihanghoa " +
                            " 	WHERE kho.kho_fk = '" + khoId + "' " +
                            " ) " +
                            " DAUKY ON sp.pk_seq = DAUKY.sanpham_fk " +
                            " LEFT JOIN " +
                            " ( " +
                            " 	SELECT DISTINCT '" + khoId + "'  AS kho_fk, kho.sanpham_fk, kho.location_fk, kho.trangthai, " +
                            " 		ISNULL(nhap.soluong, 0) AS nhapNCC, ISNULL(nhap.thanhtienNhap, 0) thanhtienNhap, 		" +
                            " 		ISNULL(tra.soluong, 0) AS traNCC, ISNULL(tra.thanhtienNhapTRA, 0)  thanhtienNhapTRA, " +
                            " 		0 AS kiemke, 0 thanhtienKiemKe, ISNULL(dieuchinh.soluong, 0) AS dieuchinh, ISNULL(dieuchinh.thanhtienDCTK, 0) thanhtienDCTK, " +
                            " 		ISNULL(xuathangban.soluong, 0) AS xuathangBAN, ISNULL(xuathangban.thanhtienXuat, 0) thanhtienXuat, " +
                            " 		ISNULL(xuatkhuyenmai.soluong, 0) AS xuatKM, ISNULL(xuatkhuyenmai.thanhtienXuatKM, 0) thanhtienXuatKM, " +
                            " 		0 chiagia, 0 thanhtienTKCG, ISNULL(trahangban.soluong, 0) AS trahangban, ISNULL(trahangban.thanhtienTraBH, 0) AS thanhtienTraBH, " +
                            " 		ISNULL(trakhuyenmai.soluong, 0) AS trahangKM, ISNULL(trakhuyenmai.thanhtienTraKM, 0) AS thanhtienTraKM, " +
                            " 		ISNULL(nhapkhac.soluong, 0) AS nhapkhac, ISNULL(nhapkhac.thanhtienNhapKhac, 0) AS thanhtienNhapKhac, " +
                            " 		ISNULL(xuatkhac.soluong, 0) AS xuatkhac, ISNULL(xuatkhac.thanhtienXuatKhac, 0) AS thanhtienXuatKhac, ISNULL(xuatTieuHao.soluong, 0) AS xuattieuhao  " +
                            " 	FROM Kho_SanPham_ChiTiet kho " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'2.1.Nhập hàng' AS loaict,  b.sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.soluongQuyDoi) soluong, ISNULL(SUM(b.soluong * b.giamua), 0) thanhtienNhap " +
                            " 		FROM NhapHang a INNER JOIN NhapHang_SanPham_ChiTiet b ON a.pk_seq = b.nhaphang_fk " +
                            " 	    WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaynhap, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            "         GROUP BY b.sanpham_fk, b.location_fk, a.loaihanghoa " +
                            " 	) " +
                            " 	nhap ON kho.sanpham_fk = nhap.sanpham_fk AND kho.location_fk = nhap.location_fk AND kho.trangthai = nhap.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'3.Trả hàng NCC' AS loaict, b.sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienNhapTRA " +
                            " 		FROM TraHang a INNER JOIN TraHang_SanPham_ChiTiet b ON a.pk_seq = b.trahang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaytra, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaytra, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.location_fk, a.loaihanghoa " +
                            " 	) " +
                            " 	tra ON kho.sanpham_fk = tra.sanpham_fk AND kho.location_fk = tra.location_fk AND kho.trangthai = tra.loaihanghoa " +
                            //" 	LEFT JOIN " +
                            //" 	( " +
                            //" 		SELECT N'4.Kiểm kho' AS loaict,  b.sanpham_fk, b.location_fk, SUM(b.soluongkiem - b.ton) AS soluong, SUM((b.soluongkiem - b.ton) * giamuatb) thanhtienKiemKe " +
                            //" 		FROM KiemKho a INNER JOIN KiemKho_SanPham b ON a.pk_seq = b.kiemkho_fk " +
                            //" 		WHERE CONVERT(datetime, a.ngaykiem, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            //" 				and CONVERT(datetime, a.ngaykiem, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            //" 		GROUP BY b.sanpham_fk, b.location_fk " +
                            //" 	) " +
                            //" 	kiemke ON kho.sanpham_fk = kiemke.sanpham_fk AND kho.location_fk = kiemke.location_fk " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'5.Điều chỉnh tồn kho' AS loaict,  b.sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.dieuchinh) AS soluong, SUM(b.dieuchinh * b.giamuatb) thanhtienDCTK " +
                            " 		FROM DieuChinhTonKho a INNER JOIN DieuChinhTonKho_SanPham b ON a.pk_seq = b.dieuchinh_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaydieuchinh, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaydieuchinh, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.location_fk, a.loaihanghoa " +
                            " 	) " +
                            " 	dieuchinh ON kho.sanpham_fk = dieuchinh.sanpham_fk AND kho.location_fk = dieuchinh.location_fk  AND kho.trangthai = dieuchinh.loaihanghoa " +
                            " LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'6.Stock out' AS loaict, b.sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.SOLUONGQUYDOI) AS soluong, 1 thanhtienXuat " +
                            "         FROM DonHang a INNER JOIN DonHang_SanPham_ChiTiet b ON a.pk_seq = b.donhang_FK  " +
                            "         WHERE CONVERT(datetime, a.ngaygiaohang, 105) >= CONVERT(datetime,'" + tungay + "', 105) AND a.trangthai IN (1) AND a.kho_fk = '" + khoId + "'  " +
                            "                 AND CONVERT(datetime, a.ngaygiaohang, 105) <= CONVERT(datetime, '" + denngay + "', 105)   " +
                            "         GROUP BY b.sanpham_fk, b.location_fk, a.loaihanghoa " +
                            " 	) " +
                            " 	xuathangban ON kho.sanpham_fk = xuathangban.sanpham_fk AND kho.location_fk = xuathangban.location_fk AND kho.trangthai = xuathangban.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'6.Xuất khuyến mại' AS loaict, c.pk_seq AS sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.SOLUONG) AS soluong, SUM(b.TongGiaTri) thanhtienXuatKM  " +
                            "         FROM DonHang a INNER JOIN DONHANG_CTKM_TRAKM b ON a.pk_seq = b.donhang_fk  " +
                            "                 INNER JOIN SANPHAM c ON b.spMA = c.MA  " +
                            "         WHERE CONVERT(datetime, a.ngaygiaohang, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai IN (1) AND b.kho_fk = '" + khoId + "'  " +
                            "                 AND CONVERT(datetime, a.ngaygiaohang, 105) <= CONVERT(datetime, '" + denngay + "', 105)   " +
                            "         GROUP BY c.pk_seq, b.location_fk, a.loaihanghoa " +
                            " 	) " +
                            " 	xuatkhuyenmai ON kho.sanpham_fk = xuatkhuyenmai.sanpham_fk AND kho.location_fk = xuatkhuyenmai.location_fk AND kho.trangthai = xuatkhuyenmai.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'10.Trả hàng khách hàng' AS loaict,  b.sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.soluongQUYDOI) AS soluong, SUM(b.dongia*b.soluong) thanhtienTraBH " +
                            " 		FROM DonTraHang a INNER JOIN DonTraHang_SanPham_ChiTiet b ON a.pk_seq = b.dontrahang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaydonhang, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = 1 AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaydonhang, 105) <= CONVERT(datetime, '" + denngay + "', 105)   " +
                            " 		GROUP BY b.sanpham_fk, b.location_fk, a.loaihanghoa " +
                            " 	) " +
                            " trahangban ON kho.sanpham_fk = trahangban.sanpham_fk AND kho.location_fk = trahangban.location_fk AND kho.trangthai = trahangban.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'11.Trả hàng khuyến mại' AS loaict, b.sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.soluong) AS soluong, SUM(b.soluong*dongia) thanhtienTraKM " +
                            " 		FROM DonTraHang a INNER JOIN DonTraHang_SPKM_ChiTiet b ON a.pk_seq = b.dontrahang_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaydonhang, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = 1 AND a.kho_fk = '" + khoId + "'  " +
                            " 				and CONVERT(datetime, a.ngaydonhang, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                            " 		GROUP BY b.sanpham_fk, b.location_fk, a.loaihanghoa " +
                            " 	) " +
                            " 	trakhuyenmai ON kho.sanpham_fk = trakhuyenmai.sanpham_fk AND kho.location_fk = trakhuyenmai.location_fk AND kho.trangthai = trakhuyenmai.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'14.Nhập khác' AS loaict, b.sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienNhapKhac" +
                            " 		FROM NhapKhac a INNER JOIN NhapKhac_SanPham_ChiTiet b ON a.pk_seq = b.nhapkhac_fk " +
                            " 		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'   " +
                            " 				and CONVERT(datetime, a.ngaynhap, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.location_fk, a.loaihanghoa " +
                            " 	) " +
                            " 	nhapkhac ON kho.sanpham_fk = nhapkhac.sanpham_fk AND kho.location_fk = nhapkhac.location_fk AND kho.trangthai = nhapkhac.loaihanghoa " +
                            " 	LEFT JOIN " +
                            " 	( " +
                            " 		SELECT N'15.Xuất khác' AS loaict,  b.sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienXuatKhac " +
                            " 		FROM XuatKhac a INNER JOIN XuatKhac_SanPham_ChiTiet b ON a.pk_seq = b.xuatkhac_fk " +
                            " 		WHERE CONVERT(datetime, a.ngayxuat, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'   " +
                            " 				and CONVERT(datetime, a.ngayxuat, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            " 		GROUP BY b.sanpham_fk, b.location_fk, a.loaihanghoa " +
                            " 	) " +
                            "   xuatkhac ON kho.sanpham_fk = xuatkhac.sanpham_fk AND kho.location_fk = xuatkhac.location_fk AND kho.trangthai = xuatkhac.loaihanghoa " +
                            "    LEFT JOIN  " +
                            "    (  " +
                            "    	SELECT N'19.Xuất tiêu hao nguyên liệu' AS loaict,  b.vattu_fk AS sanpham_fk, b.location_fk, a.loaihanghoa, SUM(b.soluongQUYDOI) AS soluong  " +
                            "    	FROM LenhSanXuat_TieuHao a INNER JOIN LenhSanXuat_TieuHao_ChiTiet b ON a.pk_seq = b.tieuhao_fk  " +
                            "    	WHERE CONVERT(datetime, a.ngaytieuhao, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.kho_fk = '" + khoId + "'     " +
                            "    			and CONVERT(datetime, a.ngaytieuhao, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            "    	GROUP BY b.vattu_fk, b.location_fk, a.loaihanghoa " +
                            "    )  " +
                            "    xuatTieuHao ON kho.sanpham_fk = xuatTieuHao.sanpham_fk AND kho.location_fk = xuatTieuHao.location_fk AND kho.trangthai = xuatTieuHao.loaihanghoa " +
                            " 	WHERE kho.kho_fk = '" + khoId + "'  " +
                            " ) " +
                            " TRONGKY ON DAUKY.sanpham_fk = TRONGKY.sanpham_fk AND DAUKY.location_fk = TRONGKY.location_fk AND DAUKY.trangthai = TRONGKY.trangthai " +
                            "  LEFT JOIN Location loc ON DAUKY.location_fk = loc.pk_seq " +
                            //"  LEFT JOIN ChungLoai cl ON sp.chungloai_fk = cl.pk_seq " +
                            "  WHERE 1 = 1 ";

                        //if (nganhhangId.Trim().Length > 0)
                        //    query += " AND sp.nganhhang in ( SELECT ten FROM NGANHHANG WHERE pk_seq = '" + nganhhangId + "' ) ";

                        DataTable dt = xl.ReadTable(query);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TableRow row = new TableRow();

                            string nganhhang = dt.Rows[i]["nganhhang"].ToString();
                            string chungloai = dt.Rows[i]["chungloai"].ToString();
                            string masp = dt.Rows[i]["masp"].ToString();
                            string tensp = dt.Rows[i]["tensp"].ToString();
                            string dvt = dt.Rows[i]["dvt"].ToString();
                            string trangthai = dt.Rows[i]["trangthai"].ToString();

                            //string location = dt.Rows[i]["location"].ToString();
                            //string bin = dt.Rows[i]["bin"].ToString();
                            //string xuatxu = dt.Rows[i]["xuatxu"].ToString();
                            string location = dt.Rows[i]["location"].ToString();
                            //string serialNo = dt.Rows[i]["serialNo"].ToString();
                            //string solo = dt.Rows[i]["solo"].ToString();
                            //string ngaynhap = dt.Rows[i]["ngaynhap"].ToString();

                            //double giamua = double.Parse(dt.Rows[i]["giamua"].ToString());
                            //double giaban = double.Parse(dt.Rows[i]["giaban"].ToString());
                            //double giaton = double.Parse(dt.Rows[i]["giaton"].ToString());

                            //if (theogia.Equals("0"))  //XEM THEO GIA MUA
                            //    giaban = giamua;
                            //else if (theogia.Equals("0"))  //XEM THEO GIA BAN
                            //    giamua = giaban;
                            //else
                            // giamua = giaton;

                            double tondauKS = double.Parse(dt.Rows[i]["tondauKS"].ToString());

                            double tondau = double.Parse(dt.Rows[i]["tondau"].ToString());
                            //double tt_tondau = double.Parse(dt.Rows[i]["thanhtienTONDAU"].ToString());

                            double nhapmuaNCC = double.Parse(dt.Rows[i]["nhapNCC"].ToString());
                            //double tt_nhapmuaNCC = double.Parse(dt.Rows[i]["thanhtienNhap"].ToString());

                            double kiemke = double.Parse(dt.Rows[i]["kiemke"].ToString());
                            //double tt_kiemke = double.Parse(dt.Rows[i]["thanhtienKiemKe"].ToString());

                            //double dieuchinh = double.Parse(dt.Rows[i]["dieuchinh"].ToString());
                            //double tt_dieuchinh = double.Parse(dt.Rows[i]["thanhtienDCTK"].ToString());

                            //double thuhoiBAN = double.Parse(dt.Rows[i]["thuhoiBAN"].ToString());
                            //double tt_thuhoiBAN = double.Parse(dt.Rows[i]["thanhtienTHBH"].ToString());

                            //double thuhoiKM = double.Parse(dt.Rows[i]["thuhoiKM"].ToString());
                            //double tt_thuhoiKM = double.Parse(dt.Rows[i]["thanhtienTHKM"].ToString());

                            double trahangban = double.Parse(dt.Rows[i]["trahangban"].ToString());
                            //double tt_trahangban = double.Parse(dt.Rows[i]["thanhtienTraBH"].ToString());

                            //double trahangKM = double.Parse(dt.Rows[i]["trahangKM"].ToString());
                            //double tt_trahangKM = double.Parse(dt.Rows[i]["thanhtienTraKM"].ToString());

                            //double nhapchuyenkho = double.Parse(dt.Rows[i]["nhapchuyenkho"].ToString());
                            //double tt_nhapchuyenkho = double.Parse(dt.Rows[i]["thanhtienNhapKhacCK"].ToString());

                            //double nhapkhac = double.Parse(dt.Rows[i]["nhapkhac"].ToString());
                            //double tt_nhapkhac = double.Parse(dt.Rows[i]["thanhtienNhapKhac"].ToString());

                            //double nhapdoilobin = double.Parse(dt.Rows[i]["nhapdoilobin"].ToString());
                            //double tt_nhapdoilobin = double.Parse(dt.Rows[i]["thanhtienNhapDOILOBIN"].ToString());

                            //double nhaplsx = double.Parse(dt.Rows[i]["nhaplsx"].ToString());
                            //double tt_nhaplsx = double.Parse(dt.Rows[i]["thanhtienNhapLSX"].ToString());

                            //XUAT
                            double xuathangBAN = double.Parse(dt.Rows[i]["xuathangBAN"].ToString());
                            //double tt_xuathangBAN = double.Parse(dt.Rows[i]["thanhtienXuat"].ToString());

                            //THEO DON VI
                            //double XUAT_soluongCHUAN = double.Parse(dt.Rows[i]["XUAT_soluongCHUAN"].ToString());
                            //double XUAT_soluongQD = double.Parse(dt.Rows[i]["XUAT_soluongQD"].ToString());

                            //double xuathangKM = double.Parse(dt.Rows[i]["xuatKM"].ToString());
                            //double tt_xuathangKM = double.Parse(dt.Rows[i]["thanhtienXuatKM"].ToString());

                            double trahangNCC = double.Parse(dt.Rows[i]["traNCC"].ToString());
                            //double tt_trahangNCC = double.Parse(dt.Rows[i]["thanhtienNhapTRA"].ToString());

                            //double chiagia = double.Parse(dt.Rows[i]["chiagia"].ToString());
                            //double tt_chiagia = double.Parse(dt.Rows[i]["thanhtienTKCG"].ToString());

                            //double xuatkhac = double.Parse(dt.Rows[i]["xuatkhac"].ToString());
                            //double tt_xuatkhac = double.Parse(dt.Rows[i]["thanhtienXuatKhac"].ToString());

                            //double xuatchuyenkho = double.Parse(dt.Rows[i]["xuatchuyenkho"].ToString());
                            //double tt_xuatchuyenkho = double.Parse(dt.Rows[i]["thanhtienXuatKhacCK"].ToString());

                            //double xuatdoilobin = double.Parse(dt.Rows[i]["xuatdoilobin"].ToString());
                            //double tt_xuatdoilobin = double.Parse(dt.Rows[i]["thanhtienXuatDOILOBIN"].ToString());

                            //double xuatTieuHao = double.Parse(dt.Rows[i]["xuatTieuHao"].ToString());
                            //double tt_xuatTieuHao = double.Parse(dt.Rows[i]["thanhtienXuatTieuHao"].ToString());

                            tondauKS = Math.Round(tondauKS, 3);
                            tondau = Math.Round(tondau, 3);

                            nhapmuaNCC = Math.Round(nhapmuaNCC, 3);
                            kiemke = Math.Round(kiemke, 3);
                            //dieuchinh = Math.Round(dieuchinh, 3);
                            trahangNCC = Math.Round(trahangNCC, 3);
                            //nhapkhac = Math.Round(nhapkhac, 3);
                            //nhapchuyenkho = Math.Round(nhapchuyenkho, 3);
                            //dieuchinh = Math.Round(dieuchinh, 3);
                            //nhapdoilobin = Math.Round(nhapdoilobin, 3);
                            //nhaplsx = Math.Round(nhaplsx, 3);

                            xuathangBAN = Math.Round(xuathangBAN, 3);
                            //xuathangKM = Math.Round(xuathangKM, 3);
                            //chiagia = Math.Round(chiagia, 3);
                            //xuatkhac = Math.Round(xuatkhac, 3);
                            //xuatchuyenkho = Math.Round(xuatchuyenkho, 3);
                            //xuatdoilobin = Math.Round(xuatdoilobin, 3);
                            //xuatTieuHao = Math.Round(xuatTieuHao, 3);
                            //thuhoiBAN = Math.Round(thuhoiBAN, 3);
                            //thuhoiKM = Math.Round(thuhoiKM, 3);
                            trahangban = Math.Round(trahangban, 3);
                            //trahangKM = Math.Round(trahangKM, 3);

                            //double tongNHAP = nhapmuaNCC + kiemke + dieuchinh - trahangNCC + nhapkhac +
                            //   nhapchuyenkho + nhapdoilobin + nhaplsx;
                            //double tt_tongNHAP = tt_nhapmuaNCC + tt_kiemke + tt_dieuchinh - tt_trahangNCC + tt_nhapchuyenkho + tt_nhapkhac + tt_nhapdoilobin + tt_nhaplsx;


                            //double tongXUAT = xuathangBAN + xuathangKM + chiagia + xuatkhac + xuatchuyenkho + xuatdoilobin + xuatTieuHao - thuhoiBAN - thuhoiKM - trahangban - trahangKM;
                            //double tt_tongXUAT = tt_xuathangBAN + tt_xuathangKM + tt_chiagia + tt_xuatkhac + tt_xuatchuyenkho + tt_xuatdoilobin + tt_xuatTieuHao -
                            //    tt_thuhoiBAN - tt_thuhoiKM - tt_trahangban - tt_trahangKM;

                            double tongNHAP = nhapmuaNCC + kiemke - trahangNCC;
                            //double tt_tongNHAP = tt_nhapmuaNCC + tt_kiemke  - tt_trahangNCC;

                            double tongXUAT = xuathangBAN - trahangban;
                            //double tt_tongXUAT = tt_xuathangBAN - tt_trahangban;

                            double toncuoi = tondau + tongNHAP - tongXUAT;
                            //double tt_toncuoi = giamua * toncuoi;

                            //string[] data = new string[] { khoTen, nganhhang, masp, tensp, dvt, xuatxu, location, bin, solo, ngaynhap, 
                            //                FormatString.ForMatNumber(tondauKS.ToString()), "", 
                            //                FormatString.ForMatNumber(tondau.ToString()), FormatString.ForMatNumber(tt_tondau.ToString()), 
                            //                FormatString.ForMatNumber(nhapmuaNCC.ToString()), FormatString.ForMatNumber(tt_nhapmuaNCC.ToString()), 
                            //                FormatString.ForMatNumber(trahangNCC.ToString()), FormatString.ForMatNumber(tt_trahangNCC.ToString()), 
                            //                FormatString.ForMatNumber(kiemke.ToString()), FormatString.ForMatNumber(tt_kiemke.ToString()), 
                            //                FormatString.ForMatNumber(dieuchinh.ToString()), FormatString.ForMatNumber(tt_dieuchinh.ToString()),                                           
                            //                FormatString.ForMatNumber(nhapchuyenkho.ToString()), FormatString.ForMatNumber(tt_nhapchuyenkho.ToString()),  
                            //                FormatString.ForMatNumber(nhapkhac.ToString()), FormatString.ForMatNumber(tt_nhapkhac.ToString()), 
                            //                FormatString.ForMatNumber(nhapdoilobin.ToString()), FormatString.ForMatNumber(tt_nhapdoilobin.ToString()),
                            //                FormatString.ForMatNumber(nhaplsx.ToString()), FormatString.ForMatNumber(tt_nhaplsx.ToString()),
                            //                FormatString.ForMatNumber(tongNHAP.ToString()), FormatString.ForMatNumber(tt_tongNHAP.ToString()), 
                            //                FormatString.ForMatNumber(xuathangBAN.ToString()), FormatString.ForMatNumber(tt_xuathangBAN.ToString()), 
                            //                FormatString.ForMatNumber(trahangban.ToString()), FormatString.ForMatNumber(tt_trahangban.ToString()),                                             
                            //                FormatString.ForMatNumber(xuatchuyenkho.ToString()), FormatString.ForMatNumber(tt_xuatchuyenkho.ToString()), 
                            //                FormatString.ForMatNumber(xuatkhac.ToString()), FormatString.ForMatNumber(tt_xuatkhac.ToString()), 
                            //                FormatString.ForMatNumber(xuatdoilobin.ToString()), FormatString.ForMatNumber(tt_xuatdoilobin.ToString()),
                            //                FormatString.ForMatNumber(xuatTieuHao.ToString()), FormatString.ForMatNumber(tt_xuatTieuHao.ToString()),
                            //                FormatString.ForMatNumber(tongXUAT.ToString()), FormatString.ForMatNumber(tt_tongXUAT.ToString()),   
                            //                FormatString.ForMatNumber(toncuoi.ToString()), FormatString.ForMatNumber(tt_toncuoi.ToString())  };

                            string[] data = new string[] { khoTen, chungloai, masp, tensp, dvt, location, trangthai,
                                            FormatString.ForMatNumber(tondauKS.ToString()),
                                            FormatString.ForMatNumber(tondau.ToString()),
                                            FormatString.ForMatNumber(nhapmuaNCC.ToString()), 
                                            //FormatString.ForMatNumber(trahangNCC.ToString()), 
                                            //FormatString.ForMatNumber(kiemke.ToString()),
                                            FormatString.ForMatNumber(tongNHAP.ToString()),
                                            FormatString.ForMatNumber(xuathangBAN.ToString()),
                                            //FormatString.ForMatNumber(trahangban.ToString()),
                                            FormatString.ForMatNumber(tongXUAT.ToString()),
                                            FormatString.ForMatNumber(toncuoi.ToString())  };

                            for (int j = 0; j < data.Length; j++)
                            {
                                TableCell cell = new TableCell();

                                //if (data[0].StartsWith("0"))
                                //data[0] = "'" + data[0];

                                if (j == 3)
                                    cell.Width = Unit.Parse("450");

                                if (j >= 6)
                                {
                                    cell.Width = Unit.Parse("100");
                                    cell.HorizontalAlign = HorizontalAlign.Right;
                                }

                                cell.Text = data[j];

                                //cell.Attributes.Add("style", @"mso-number-format:\@;");

                                row.Cells.Add(cell);
                            }

                            table.Rows.Add(row);
                        }
                    }

                    table.RenderControl(htmlWriter);
                    _exportContent = sb.ToString();
                }
            }
            return _exportContent;
        }
        private string NhapXuatTonTong(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string khoId = "";
            if (context.Request.QueryString["khoId"] != null)
                khoId = context.Request.QueryString["khoId"].ToString();

            //khoId = khoId.Substring(0, khoId.Length - 1);

            string nganhhangId = "";
            if (context.Request.QueryString["nganhhangId"] != null)
                nganhhangId = context.Request.QueryString["nganhhangId"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();

            string theogia = "0";
            if (context.Request.QueryString["theogia"] != null)
                theogia = context.Request.QueryString["theogia"].ToString();

            string xemtheo = "0";
            if (context.Request.QueryString["xemtheo"] != null)
                xemtheo = context.Request.QueryString["xemtheo"].ToString();

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
                    header.ColumnSpan = 15;
                    header.Text = "TOTAL STOCK IN-OUT REPORT ";
                    header.Font.Bold = true;
                    header.BackColor = Color.Red;
                    header.Font.Name = "Arial";
                    header.Font.Size = FontUnit.Point(11);
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;

                    // Add the header to a new row.
                    TableRow headerRow = new TableRow();
                    headerRow.Cells.Add(header);

                    table.Rows.Add(headerRow);


                    //Tieu de
                    headerRow = new TableRow();
                    header = new TableHeaderCell();

                    header.Text = "Date from: " + tungay + " to " + denngay;
                    header.ColumnSpan = 15;
                    header.Font.Bold = true;
                    header.Font.Name = "Arial";
                    header.Font.Size = FontUnit.Point(10);
                    //header.BackColor = Color.LightGray;
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);

                    table.Rows.Add(headerRow);

                    //Tieu de
                    headerRow = new TableRow();
                    header = new TableHeaderCell();

                    header.Text = "Created date: " + DateTime.Now.ToString("dd-MM-yyyy");
                    header.ColumnSpan = 15;
                    header.Font.Bold = true;
                    header.Font.Name = "Arial";
                    header.Font.Size = FontUnit.Point(10);
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

                    string[] tieude = new string[] { "Warehouse", "Specification", "PartCode", "Description", "Unit", 
                                    "Stock closure ", "Start - " + tungay, "Stock in", "Stock out", "Ending stocks - " + denngay };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;

                        if (i >= 5) //Số hiệu chứng từ
                            header.ColumnSpan = 2;

                        header.BackColor = Color.LightGray;
                        header.Font.Name = "Arial";
                        header.Font.Size = FontUnit.Point(9);
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);


                    tieude = new string[] { "", "", "", "", "",
                        "Quantity", "Total value", "Quantity", "Total value", "Quantity", "Total value", "Quantity", "Total value", "Quantity", "Total value" };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;
                        header.BackColor = Color.LightGray;
                        header.Font.Name = "Arial";
                        header.Font.Size = FontUnit.Point(9);
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }
                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();

                    string query = "SELECT TOP(1) nam, thang, thangks  " +
                    "FROM " +
                    "( " +
                    "	SELECT nam, thang, '01-' + case when thang < 10 then '0' + cast(thang as varchar(10)) else cast(thang as varchar(10)) end + '-' + cast(nam as varchar(10)) as thangks " +
                    "	FROM KHOASOTHANG   " +
                    ") " +
                    "DATA " +
                    "WHERE CONVERT(datetime, thangks, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                    "ORDER BY nam desc, thang desc ";

                    DataTable dtKS = xl.ReadTable(query);

                    string thangks = "";
                    string namks = "";
                    int thangTIEPTHEO = 0;
                    int namTIEPTHEO = 0;
                    if (dtKS.Rows.Count > 0)
                    {
                        thangks = dtKS.Rows[0]["thang"].ToString();
                        namks = dtKS.Rows[0]["nam"].ToString();
                        thangTIEPTHEO = int.Parse(dtKS.Rows[0]["thang"].ToString());
                        namTIEPTHEO = int.Parse(dtKS.Rows[0]["nam"].ToString());

                        if (int.Parse(thangks) == 12)
                        {
                            thangTIEPTHEO = 1;
                            namTIEPTHEO += 1;
                        }
                        else
                            thangTIEPTHEO += 1;
                    }

                    string dauthang = "01-" + (thangTIEPTHEO < 10 ? "0" + thangTIEPTHEO.ToString() : thangTIEPTHEO.ToString()) + "-" + namTIEPTHEO.ToString();
                    string condition = "";
                    if (nganhhangId.Trim().Length > 3)
                        condition += " and sp.nganhhang_fk = '" + nganhhangId + "' ";
                    if (chungloai.Trim().Length > 3)
                        condition += " and sp.chungloai_fk = '" + chungloai + "' ";

                    string[] khoIds = Regex.Split(khoId, ",");
                    string[] thangBC = Regex.Split(tungay, "-");

                    string conditionDONHANG = "";
                    //if (xemtheo.Equals("1")) //don hang ban thu kho da xac nhan
                    //    conditionDONHANG = " AND trangthai_kho = '1' ";

                    for (int count = 0; count < khoIds.Length; count++)
                    {
                        khoId = khoIds[count];
                        string khoTen = xl.ExecuteScalarSQL("SELECT makho FROM KHO WHERE pk_seq = '" + khoIds[count] + "'").ToString();

                        //DataTable dt = xl.getTableFromProc("XuatNhapTon", new string[] { "@khoId", "@tungay", "@denngay" }, new string[] { khoIds[count], tungay, denngay });

                        query = "SELECT (SELECT ten FROM NganhHang WHERE pk_seq = sp.nganhhang_fk) nganhhang, ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') chungloai, " +
                            "   sp.ma as masp, sp.nameEnglish as tensp, (SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk) as dvt, " +
                            "   ISNULL(dauky.tondauKS, 0) as tondauKS, ISNULL(dauky.soluong, 0) as tondau, ISNULL(dauky.thanhtienTONDAU, 0) as thanhtienTONDAU,  " +
                            "	TRONGKY.nhapNCC, TRONGKY.nhapKM, TRONGKY.traNCC, TRONGKY.kiemke, TRONGKY.dieuchinh, TRONGKY.xuathangBAN, TRONGKY.xuatKM, " +
                            "	TRONGKY.chiagia, TRONGKY.trahangban, TRONGKY.trahangKM, " +
                            "   TRONGKY.nhapkhac, TRONGKY.xuatkhac, TRONGKY.nhapchuyenkho, TRONGKY.xuatchuyenkho, TRONGKY.nhaplenhsanxuat, TRONGKY.xuattieuhao, dauky.giamua, dauky.giaban, dauky.giaton, " +
                            "   TRONGKY.thanhtienNhap, TRONGKY.thanhtienNhapKM, TRONGKY.thanhtienDCTK, TRONGKY.thanhtienKiemKe, TRONGKY.thanhtienNhapKhac, TRONGKY.thanhtienNhapKhacCK, " +
                            "   TRONGKY.thanhtienNhapTRA, TRONGKY.thanhtienTKCG, TRONGKY.thanhtienTraBH, TRONGKY.thanhtienTraKM, " +
                            "   TRONGKY.thanhtienXuat, TRONGKY.thanhtienXuatKM, TRONGKY.thanhtienXuatKhac, TRONGKY.thanhtienXuatKhacCK " +
                            " FROM SanPham sp INNER JOIN " +
                            "( " +
                            "	SELECT " + khoId + " as kho_fk, sp.pk_seq as sanpham_fk,  ISNULL(tondau.soluong, 0) as tondauKS,  " +
                            "			ISNULL(tondau.soluong, 0) + ISNULL(nhap.soluong, 0) + ISNULL(nhapKM.soluong, 0) - ISNULL(tra.soluong, 0) + " +
                            "			0 + ISNULL(dieuchinh.soluong, 0) - " +
                            "			ISNULL(xuathangban.soluong, 0) - ISNULL(xuatkhuyenmai.soluong, 0) + " +
                            "			ISNULL(chiagia.soluong, 0) + ISNULL(trahangban.soluong, 0) + ISNULL(trahangkhuyenmai.soluong, 0) + " +
                            "			ISNULL(nhapkhac.soluong, 0) - ISNULL(xuatkhac.soluong, 0) + ISNULL(nhapchuyenkho.soluong, 0) - ISNULL(xuatchuyenkho.soluong, 0) +  " +
                            "           ISNULL(nhaplsx.soluong, 0) - ISNULL(xuatTieuHao.soluong, 0) as SOLUONG, " +
                            "			ISNULL( ( SELECT TOP(1) dongia FROM BangGiaMua_SanPham WHERE sanpham_fk = sp.pk_seq ), 0 ) as giamua, " +
                            "			ISNULL( ( SELECT TOP(1) dongia FROM BangGiaBan_SanPham WHERE sanpham_fk = sp.pk_seq ), 0 ) as giaban, " +
                            "			1 AS giaton, ISNULL(tondau.thanhtienTONDAU, 0) as thanhtienTONDAU " +
                            "	FROM SanPham sp " +
                            "	LEFT JOIN " +
                            "	( " +
                            "		SELECT N'1.Tồn đầu' as loaict, sanpham_fk, SUM(soluong) as soluong, SUM(soluong * giaton) as thanhtienTONDAU  " +
                            "		FROM TonKhoThang   " +
                            "		WHERE THANG = '" + thangks + "' and NAM = '" + namks + "' and kho_fk = " + khoId + " " +
                            "		GROUP BY sanpham_fk " +
                            "	) " +
                            "	tondau on sp.pk_seq = tondau.sanpham_fk " +
                            "	LEFT JOIN " +
                            "	( " +
                            "		SELECT nhap.loaict, nhap.sanpham_fk, SUM(soluong) as soluong, SUM(soluong * gianhap) as thanhtienNhap " +
                            "		FROM " +
                            "		( " +
                            "			SELECT N'2.1.Nhập hàng' as loaict,  b.sanpham_fk, SUM(b.soluongQuyDoi) soluong, ISNULL(b.gianhapQuyDoi , 0) gianhap " +
                            "			FROM NhapHang a INNER JOIN NhapHang_SanPham b on a.pk_seq = b.nhaphang_fk " +
                            "			WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + dauthang + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + " " +
                            "					and CONVERT(datetime, a.ngaynhap, 105) < CONVERT(datetime, '" + tungay + "', 105) and a.LOAIHOADON = '0' " +
                            "           GROUP BY b.sanpham_fk, gianhapQuyDoi " +
                            "		) " +
                            "		nhap GROUP BY nhap.loaict, nhap.sanpham_fk " +
                            "	) " +
                            "	nhap on sp.pk_seq = nhap.sanpham_fk  " +
                            "	LEFT JOIN " +
                            "	( " +
                            "		SELECT nhap.loaict, nhap.sanpham_fk, SUM(soluong) as soluong, thanhtienNhapKM " +
                            "		FROM " +
                            "		( " +
                            "			SELECT N'2.2.Nhập hàng khuyến mại' as loaict,  b.sanpham_fk, SUM(b.soluongQuyDoi) soluong, SUM(soluong*gianhap) thanhtienNhapKM " +
                            "			FROM NhapHang a INNER JOIN NhapHang_SanPham b on a.pk_seq = b.nhaphang_fk " +
                            "			WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + dauthang + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + " " +
                            "					and CONVERT(datetime, a.ngaynhap, 105) < CONVERT(datetime, '" + tungay + "', 105) and a.LOAIHOADON = '1' " +
                            "                   GROUP BY b.sanpham_fk " +
                            "		) " +
                            "		nhap GROUP BY nhap.loaict, nhap.sanpham_fk, nhap.thanhtienNhapKM " +
                            "	) " +
                            "	nhapKM on sp.pk_seq = nhapKM.sanpham_fk  " +
                            "	LEFT JOIN " +
                            "	( " +
                            "		SELECT N'3.Trả hàng NCC' as loaict, b.sanpham_fk, SUM(b.soluong) as soluong, SUM(b.soluong * b.giatra) thanhtienNhapTRA " +
                            "		FROM TraHang a INNER JOIN TraHang_SanPham b on a.pk_seq = b.trahang_fk " +
                            "		WHERE CONVERT(datetime, a.ngaytra, 105) >= CONVERT(datetime, '" + dauthang + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + " " +
                            "				and CONVERT(datetime, a.ngaytra, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            "		GROUP BY b.sanpham_fk " +
                            "	) " +
                            "	tra on sp.pk_seq = tra.sanpham_fk  " +
                            //"	LEFT JOIN " +
                            //"	( " +
                            //"		SELECT N'4.Kiểm kho' as loaict,  b.sanpham_fk, SUM(b.soluongkiem - b.ton) as soluong, SUM((b.soluongkiem - b.ton)*giamuatb) thanhtienKiemKho " +
                            //"		FROM KiemKho a INNER JOIN KiemKho_SanPham b on a.pk_seq = b.kiemkho_fk " +
                            //"		WHERE CONVERT(datetime, a.ngaykiem, 105) >= CONVERT(datetime, '" + dauthang + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + " " +
                            //"				and CONVERT(datetime, a.ngaykiem, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            //"		GROUP BY b.sanpham_fk " +
                            //"	) " +
                            //"	kiemke on sp.pk_seq = kiemke.sanpham_fk  " +
                            "	LEFT JOIN " +
                            "	( " +
                            "		SELECT N'5.Điều chỉnh tồn kho' as loaict,  b.sanpham_fk, SUM(b.dieuchinh) as soluong, SUM(b.dieuchinh * b.giamuatb) thanhtienDCTK " +
                            "		FROM DieuChinhTonKho a INNER JOIN DieuChinhTonKho_SanPham b on a.pk_seq = b.dieuchinh_fk " +
                            "		WHERE CONVERT(datetime, a.ngaydieuchinh, 105) >= CONVERT(datetime, '" + dauthang + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + " " +
                            "				and CONVERT(datetime, a.ngaydieuchinh, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            "		GROUP BY b.sanpham_fk, b.giamuatb" +
                            "	) " +
                            "	dieuchinh on sp.pk_seq = dieuchinh.sanpham_fk  " +
                            "	LEFT JOIN " +
                            "	( " +
                            "		SELECT N'6.Stock out' as loaict,  b.sanpham_fk,SUM(b.SOLUONGQUYDOI) as soluong, 1 thanhtienXuat " +
                            "        FROM DONHANG a INNER JOIN DonHang_SanPham_ChiTiet b on a.pk_seq = b.donhang_FK  " +
                            "        WHERE CONVERT(datetime, a.ngaygiaohang, 105) >= CONVERT(datetime,'" + dauthang + "', 105) and a.trangthai not in ( 0, 2 ) and a.kho_fk = '" + khoId + "'  " +
                            "                and CONVERT(datetime, a.ngaygiaohang, 105) < CONVERT(datetime, '" + tungay + "', 105)  " + conditionDONHANG +
                            "        GROUP BY b.sanpham_fk " +
                            "	) " +
                            "	xuathangban on sp.pk_seq = xuathangban.sanpham_fk  " +
                            "	LEFT JOIN " +
                            "	( " +
                            "		SELECT N'6.Xuất khuyến mại' as loaict, c.pk_seq as sanpham_fk, SUM(b.SOLUONG) as soluong, SUM(b.TongGiaTri) thanhtienXuatKM " +
                            "        FROM DONHANG a INNER JOIN DONHANG_CTKM_TRAKM b on a.pk_seq = b.donhang_fk  " +
                            "                INNER JOIN SANPHAM c on b.spMA = c.MA  " +
                            "        WHERE CONVERT(datetime, a.ngaygiaohang, 105) >= CONVERT(datetime, '" + dauthang + "', 105) and a.trangthai not in ( 0, 2) and b.kho_fk = '" + khoId + "'  " +
                            "                and CONVERT(datetime, a.ngaygiaohang, 105) < CONVERT(datetime, '" + tungay + "', 105)  " + conditionDONHANG +
                            "        GROUP BY c.pk_seq " +
                            "	) " +
                            "	xuatkhuyenmai on sp.pk_seq = xuatkhuyenmai.sanpham_fk  " +
                            "	LEFT JOIN " +
                            "	( " +
                            "		SELECT N'9.Tổng kết chia giá' as loaict,  b.sanpham_fk, SUM(b.CHUYEN) as soluong, SUM(b.CHUYEN*b.DonGia) thanhtienTKCG " +
                            "		FROM TONGKETCHIAGIA a INNER JOIN TONGKETCHIAGIA_SANPHAM b on a.pk_seq = b.TONGKET_FK " +
                            "		WHERE CONVERT(datetime, a.ngaythuchien, 105) >= CONVERT(datetime, '" + dauthang + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + "  " +
                            "				and CONVERT(datetime, a.ngaythuchien, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                            "		GROUP BY b.sanpham_fk " +
                            "	) " +
                            "	chiagia on sp.pk_seq = chiagia.sanpham_fk  " +
                            "	LEFT JOIN " +
                            "	( " +
                            "		SELECT loaict, sanpham_fk, SUM(soluong) as soluong, ISNULL(SUM(thanhtienTraKH), 0) thanhtienTraKH  " +
                            "		FROM " +
                            "		( " +
                            "			SELECT N'10.Trả hàng khách hàng' as loaict,  b.sanpham_fk, SUM(b.soluongQUYDOI) as soluong, SUM(b.soluong*b.dongia) thanhtienTraKH " +
                            "			FROM DonTraHang a INNER JOIN DonTraHang_SanPham b on a.pk_seq = b.dontrahang_fk " +
                            "			WHERE CONVERT(datetime, a.ngaydonhang, 105) >= CONVERT(datetime, '" + dauthang + "', 105) and a.trangthai = 1 and a.kho_fk = " + khoId + " " +
                            "					and CONVERT(datetime, a.ngaydonhang, 105) < CONVERT(datetime, '" + tungay + "', 105)   " +
                            "			GROUP BY b.sanpham_fk " +
                            "		) " +
                            "		trahang GROUP BY loaict, sanpham_fk " +
                            "	) " +
                            "	trahangban on sp.pk_seq = trahangban.sanpham_fk  " +
                            "	LEFT JOIN " +
                            "	( " +
                            "		SELECT loaict, sanpham_fk, SUM(soluong) as soluong, thanhtienTraKHKM  " +
                            "		FROM " +
                            "		( " +
                            "			SELECT N'11.Trả hàng khuyến mại' as loaict,  b.sanpham_fk, SUM(b.soluong) as soluong, SUM(b.soluong*b.dongia) as thanhtienTraKHKM " +
                            "			FROM DonTraHang a INNER JOIN DonTraHang_SPKM b on a.pk_seq = b.dontrahang_fk " +
                            "			WHERE CONVERT(datetime, a.ngaydonhang, 105) >= CONVERT(datetime, '" + dauthang + "', 105) and a.trangthai = 1 and b.kho_fk = " + khoId + " " +
                            "					and CONVERT(datetime, a.ngaydonhang, 105) < CONVERT(datetime, '" + tungay + "', 105)  " +
                            "			GROUP BY b.sanpham_fk " +
                            "		) " +
                            "		trahang GROUP BY loaict, sanpham_fk, thanhtienTraKHKM " +
                            "	) " +
                            "	trahangkhuyenmai on sp.pk_seq = trahangkhuyenmai.sanpham_fk  " +
                            "	LEFT JOIN  " +
                             "	( " +
                             "		SELECT N'12.Nhập chuyển kho' as loaict,  b.sanpham_fk, SUM(c.soluongQuyDoi) as soluong, SUM(b.soluong*b.dongia) thanhtienNhapKhacCK " +
                             "		FROM NHAPKHAC a INNER JOIN NHAPKHAC_SANPHAM b on a.pk_seq = b.NHAPKHAC_FK " +
                             "         INNER JOIN NhapKhac_SanPham_ChiTiet c ON b.nhapkhac_fk= c.nhapkhac_fk AND b.sanpham_fk = c.sanpham_fk " +
                             "		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + dauthang + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + "  " +
                             "				and CONVERT(datetime, a.ngaynhap, 105) < CONVERT(datetime, '" + tungay + "', 105) and loainhap = 1 " +
                             "		GROUP BY b.sanpham_fk " +
                             "	) " +
                             "	nhapchuyenkho on sp.pk_seq = nhapchuyenkho.sanpham_fk  " +
                             "	LEFT JOIN " +
                             "	( " +
                             "		SELECT N'13.Xuất chuyển kho' as loaict,  b.sanpham_fk, SUM(c.soluongQuyDoi) as soluong, SUM(b.soluong*b.dongia) thanhtienXuatKhacCK " +
                             "		FROM XUATKHAC a INNER JOIN XUATKHAC_SANPHAM b on a.pk_seq = b.XUATKHAC_FK " +
                             "         INNER JOIN XuatKhac_SanPham_ChiTiet c ON b.xuatkhac_fk= c.xuatkhac_fk AND b.sanpham_fk = c.sanpham_fk " +
                             "		WHERE CONVERT(datetime, a.ngayxuat, 105) >= CONVERT(datetime, '" + dauthang + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + "  " +
                             "				and CONVERT(datetime, a.ngayxuat, 105) < CONVERT(datetime, '" + tungay + "', 105) and loaixuat = 3 " +
                             "		GROUP BY b.sanpham_fk " +
                             "	) " +
                             "	xuatchuyenkho on sp.pk_seq = xuatchuyenkho.sanpham_fk  " +
                             "	LEFT JOIN " +
                             "	( " +
                             "		SELECT N'14.Nhập khác' as loaict,  b.sanpham_fk, SUM(c.soluong) as soluong, SUM(b.soluong*b.dongia) thanhtienNhapKhac " +
                             "		FROM NHAPKHAC a INNER JOIN NHAPKHAC_SANPHAM b on a.pk_seq = b.NHAPKHAC_FK " +
                             "         INNER JOIN NhapKhac_SanPham_ChiTiet c ON b.nhapkhac_fk= c.nhapkhac_fk AND b.sanpham_fk = c.sanpham_fk " +
                             "		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + dauthang + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + "  " +
                             "				and CONVERT(datetime, a.ngaynhap, 105) < CONVERT(datetime, '" + tungay + "', 105) and loainhap = 2 " +
                             "		GROUP BY b.sanpham_fk " +
                             "	) " +
                             "	nhapkhac on sp.pk_seq = nhapkhac.sanpham_fk  " +
                             "	LEFT JOIN " +
                             "	( " +
                             "		SELECT N'15.Xuất khác' as loaict,  b.sanpham_fk, SUM(c.soluong) as soluong, SUM(b.soluong*b.dongia) thanhtienXuatKhac " +
                             "		FROM XUATKHAC a INNER JOIN XUATKHAC_SANPHAM b on a.pk_seq = b.XUATKHAC_FK " +
                             "         INNER JOIN XuatKhac_SanPham_ChiTiet c ON b.xuatkhac_fk= c.xuatkhac_fk AND b.sanpham_fk = c.sanpham_fk " +
                             "		WHERE CONVERT(datetime, a.ngayxuat, 105) >= CONVERT(datetime, '" + dauthang + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + "  " +
                             "				and CONVERT(datetime, a.ngayxuat, 105) < CONVERT(datetime, '" + tungay + "', 105) and loaixuat != 3 " +
                             "		GROUP BY b.sanpham_fk " +
                             "	) " +
                             "	xuatkhac on sp.pk_seq = xuatkhac.sanpham_fk  " +
                             "	LEFT JOIN  " +
                             "   (  " +
                             "   	SELECT N'18.Nhập lệnh sản xuất' as loaict,  b.sanpham_fk, SUM(b.soluongQUYDOI) as soluong  " +
                             "   	FROM NhapKhac a INNER JOIN NhapKhac_SanPham_ChiTiet b on a.pk_seq = b.nhapkhac_fk  " +
                             "   	WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + dauthang + "', 105) and a.trangthai = '1' and a.kho_fk =  " + khoId + "  " +
                             "   			and CONVERT(datetime, a.ngaynhap, 105) <= CONVERT(datetime, '" + tungay + "', 105) and a.loainhap = '4' " +
                             "   	GROUP BY b.sanpham_fk " +
                             "   )  " +
                             "   nhaplsx on sp.pk_seq = nhaplsx.sanpham_fk " +
                             "   LEFT JOIN  " +
                             "   (  " +
                             "   	SELECT N'19.Xuất tiêu hao nguyên liệu' as loaict,  b.vattu_fk as sanpham_fk, SUM(b.soluongQUYDOI) as soluong  " +
                             "   	FROM LenhSanXuat_TieuHao a INNER JOIN LenhSanXuat_TieuHao_ChiTiet b on a.pk_seq = b.tieuhao_fk  " +
                             "   	WHERE CONVERT(datetime, a.ngaytieuhao, 105) >= CONVERT(datetime, '" + dauthang + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + "    " +
                             "   			and CONVERT(datetime, a.ngaytieuhao, 105) <= CONVERT(datetime, '" + tungay + "', 105) " +
                             "   	GROUP BY b.vattu_fk " +
                             "   )  " +
                             "   xuatTieuHao on sp.pk_seq = xuatTieuHao.sanpham_fk " +
                            "	WHERE sp.pk_seq > 0 AND sp.pk_seq in (SELECT DISTINCT sanpham_fk FROM Kho_SanPham_ChiTiet WHERE kho_fk = '" + khoId + "') " + condition +
                            ") " +
                            "DAUKY ON sp.pk_seq = DAUKY.sanpham_fk LEFT JOIN " +
                            "( " +
                            "	SELECT " + khoId + " as kho_fk, sp.pk_seq as sanpham_fk,   " +
                            "			ISNULL(nhap.soluong, 0) as nhapNCC, ISNULL(nhap.thanhtienNhap, 0) thanhtienNhap, " +
                            "           ISNULL(nhapKM.soluong, 0) as nhapKM, ISNULL(nhapKM.thanhtienNhapKM, 0)  thanhtienNhapKM, " +
                            "           ISNULL(tra.soluong, 0) as traNCC, ISNULL(tra.thanhtienNhapTRA, 0)  thanhtienNhapTRA, " +
                            "			0 as kiemke, 0 thanhtienKiemKe, " +
                            "           ISNULL(dieuchinh.soluong, 0) as dieuchinh, ISNULL(dieuchinh.thanhtienDCTK, 0) thanhtienDCTK, " +
                            "			ISNULL(xuathangban.soluong, 0) as xuathangBAN, ISNULL(xuathangban.thanhtienXuat, 0) thanhtienXuat," +
                            "           ISNULL(xuatkhuyenmai.soluong, 0) as xuatKM, ISNULL(xuatkhuyenmai.thanhtienXuatKM, 0) thanhtienXuatKM," +
                            "			ISNULL(chiagia.soluong, 0) as chiagia, ISNULL(chiagia.thanhtienTKCG, 0) as thanhtienTKCG, " +
                            "           ISNULL(trahangban.soluong, 0) as trahangban, ISNULL(trahangban.thanhtienTraBH, 0) as thanhtienTraBH, " +
                            "           ISNULL(trahangkhuyenmai.soluong, 0) as trahangKM, ISNULL(trahangkhuyenmai.thanhtienTraKM, 0) as thanhtienTraKM, " +
                            "			ISNULL(nhapkhac.soluong, 0) as nhapkhac, ISNULL(nhapkhac.thanhtienNhapKhac, 0) as thanhtienNhapKhac," +
                            "           ISNULL(xuatkhac.soluong, 0) as xuatkhac, ISNULL(xuatkhac.thanhtienXuatKhac, 0) as thanhtienXuatKhac, " +
                            "           ISNULL(nhapchuyenkho.soluong, 0) as nhapchuyenkho, ISNULL(nhapchuyenkho.thanhtienNhapKhacCK, 0) as thanhtienNhapKhacCK, " +
                            "           ISNULL(xuatchuyenkho.soluong, 0) as xuatchuyenkho, ISNULL(xuatchuyenkho.thanhtienXuatKhacCK, 0) as thanhtienXuatKhacCK, " +
                            "           ISNULL(nhaplsx.soluong, 0) as nhaplenhsanxuat,  ISNULL(xuatTieuHao.soluong, 0) as xuattieuhao  " +
                            "	FROM SanPham sp  " +
                            "	LEFT JOIN " +
                            "	( " +
                            "		SELECT nhap.loaict, nhap.sanpham_fk, SUM(soluong) as soluong, SUM(soluong*gianhap) thanhtienNhap" +
                            "		FROM " +
                            "		( " +
                            "			SELECT N'2.1.Nhập hàng' as loaict,  b.sanpham_fk, SUM(b.soluongQuyDoi) soluong, ISNULL(b.gianhapQuyDoi , 0) gianhap " +
                            "			FROM NhapHang a INNER JOIN NhapHang_SanPham b on a.pk_seq = b.nhaphang_fk " +
                            "			WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + tungay + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + " " +
                            "					and CONVERT(datetime, a.ngaynhap, 105) <= CONVERT(datetime, '" + denngay + "', 105) and a.LOAIHOADON = '0' " +
                            "           GROUP BY b.sanpham_fk, b.gianhapQuyDoi " +
                            "		) " +
                            "		nhap GROUP BY nhap.loaict, nhap.sanpham_fk " +
                            "	) " +
                            "	nhap on sp.pk_seq = nhap.sanpham_fk  " +
                            "	LEFT JOIN " +
                            "	( " +
                            "		SELECT nhap.loaict, nhap.sanpham_fk, SUM(soluong) as soluong, thanhtienNhapKM " +
                            "		FROM " +
                            "		( " +
                            "			SELECT N'2.2.Nhập hàng khuyến mại' as loaict,  b.sanpham_fk, SUM(b.soluongQuyDoi) soluong, SUM(b.soluong*b.gianhap) thanhtienNhapKM  " +
                            "			FROM NhapHang a INNER JOIN NhapHang_SanPham b on a.pk_seq = b.nhaphang_fk " +
                            "			WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + tungay + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + " " +
                            "					and CONVERT(datetime, a.ngaynhap, 105) <= CONVERT(datetime, '" + denngay + "', 105) and a.LOAIHOADON = '1' " +
                            "            GROUP BY b.sanpham_fk     " +
                            "		) " +
                            "		nhap GROUP BY nhap.loaict, nhap.sanpham_fk, nhap.thanhtienNhapKM " +
                            "	) " +
                            "	nhapKM on sp.pk_seq = nhapKM.sanpham_fk  " +
                            "	LEFT JOIN " +
                            "	( " +
                            "		SELECT N'3.Trả hàng NCC' as loaict, b.sanpham_fk, SUM(b.soluong) as soluong, SUM(b.soluong*b.giatra) thanhtienNhapTRA " +
                            "		FROM TraHang a INNER JOIN TraHang_SanPham b on a.pk_seq = b.trahang_fk " +
                            "		WHERE CONVERT(datetime, a.ngaytra, 105) >= CONVERT(datetime, '" + tungay + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + " " +
                            "				and CONVERT(datetime, a.ngaytra, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            "		GROUP BY b.sanpham_fk " +
                            "	) " +
                            "	tra on sp.pk_seq = tra.sanpham_fk  " +
                            //"	LEFT JOIN " +
                            //"	( " +
                            //"		SELECT N'4.Kiểm kho' as loaict,  b.sanpham_fk, SUM(b.soluongkiem - b.ton) as soluong, SUM((b.soluongkiem - b.ton) * giamuatb) thanhtienKiemKe " +
                            //"		FROM KiemKho a INNER JOIN KiemKho_SanPham b on a.pk_seq = b.kiemkho_fk " +
                            //"		WHERE CONVERT(datetime, a.ngaykiem, 105) >= CONVERT(datetime, '" + tungay + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + " " +
                            //"				and CONVERT(datetime, a.ngaykiem, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            //"		GROUP BY b.sanpham_fk " +
                            //"	) " +
                            //"	kiemke on sp.pk_seq = kiemke.sanpham_fk  " +
                            "	LEFT JOIN " +
                            "	( " +
                            "		SELECT N'5.Điều chỉnh tồn kho' as loaict,  b.sanpham_fk, SUM(b.dieuchinh) as soluong, SUM(b.dieuchinh * b.giamuatb) thanhtienDCTK " +
                            "		FROM DieuChinhTonKho a INNER JOIN DieuChinhTonKho_SanPham b on a.pk_seq = b.dieuchinh_fk " +
                            "		WHERE CONVERT(datetime, a.ngaydieuchinh, 105) >= CONVERT(datetime, '" + tungay + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + " " +
                            "				and CONVERT(datetime, a.ngaydieuchinh, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            "		GROUP BY b.sanpham_fk " +
                            "	) " +
                            "	dieuchinh on sp.pk_seq = dieuchinh.sanpham_fk  " +
                            "	LEFT JOIN " +
                            "	( " +
                            "		SELECT N'6.Stock out' as loaict,  b.sanpham_fk, SUM(b.SOLUONGQUYDOI) as soluong, 1 thanhtienXuat " +
                            "        FROM DONHANG a INNER JOIN DonHang_SanPham_ChiTiet b on a.pk_seq = b.donhang_FK  " +
                            "        WHERE CONVERT(datetime, a.ngaygiaohang, 105) >= CONVERT(datetime,'" + tungay + "', 105) and a.trangthai not in ( 0, 2 ) and a.kho_fk = '" + khoId + "'  " +
                            "                and CONVERT(datetime, a.ngaygiaohang, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " + conditionDONHANG +
                            "        GROUP BY b.sanpham_fk " +
                            "	) " +
                            "	xuathangban on sp.pk_seq = xuathangban.sanpham_fk  " +
                            "	LEFT JOIN " +
                            "	( " +
                            "		SELECT N'6.Xuất khuyến mại' as loaict, c.pk_seq as sanpham_fk, SUM(b.SOLUONG) as soluong, SUM(b.TongGiaTri) thanhtienXuatKM  " +
                            "        FROM DONHANG a INNER JOIN DONHANG_CTKM_TRAKM b on a.pk_seq = b.donhang_fk  " +
                            "                INNER JOIN SANPHAM c on b.spMA = c.MA  " +
                            "        WHERE CONVERT(datetime, a.ngaygiaohang, 105) >= CONVERT(datetime, '" + tungay + "', 105) and a.trangthai not in ( 0, 2 ) and b.kho_fk = '" + khoId + "'  " +
                            "                and CONVERT(datetime, a.ngaygiaohang, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " + conditionDONHANG +
                            "        GROUP BY c.pk_seq " +
                            "	) " +
                            "	xuatkhuyenmai on sp.pk_seq = xuatkhuyenmai.sanpham_fk  " +
                            "	LEFT JOIN " +
                            "	( " +
                            "		SELECT N'9.Tổng kết chia giá' as loaict,  b.sanpham_fk, SUM(b.CHUYEN) as soluong, SUM(b.CHUYEN*b.dongia) thanhtienTKCG " +
                            "		FROM TONGKETCHIAGIA a INNER JOIN TONGKETCHIAGIA_SANPHAM b on a.pk_seq = b.TONGKET_FK " +
                            "		WHERE CONVERT(datetime, a.ngaythuchien, 105) >= CONVERT(datetime, '" + tungay + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + "  " +
                            "				and CONVERT(datetime, a.ngaythuchien, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                            "		GROUP BY b.sanpham_fk " +
                            "	) " +
                            "	chiagia on sp.pk_seq = chiagia.sanpham_fk  " +
                            "	LEFT JOIN " +
                            "	( " +
                            "		SELECT loaict, sanpham_fk, SUM(soluong) as soluong, thanhtienTraBH  " +
                            "		FROM " +
                            "		( " +
                            "			SELECT N'10.Trả hàng khách hàng' as loaict,  b.sanpham_fk, SUM(b.soluongQUYDOI) as soluong, SUM(b.dongia*b.soluong) thanhtienTraBH " +
                            "			FROM DonTraHang a INNER JOIN DonTraHang_SanPham b on a.pk_seq = b.dontrahang_fk " +
                            "			WHERE CONVERT(datetime, a.ngaydonhang, 105) >= CONVERT(datetime, '" + tungay + "', 105) and a.trangthai = 1 and a.kho_fk = " + khoId + " " +
                            "					and CONVERT(datetime, a.ngaydonhang, 105) <= CONVERT(datetime, '" + denngay + "', 105)   " +
                            "			GROUP BY b.sanpham_fk " +
                            "		) " +
                            "		trahang GROUP BY loaict, sanpham_fk, thanhtienTraBH " +
                            "	) " +
                            "	trahangban on sp.pk_seq = trahangban.sanpham_fk  " +
                            "	LEFT JOIN " +
                            "	( " +
                            "		SELECT loaict, sanpham_fk, SUM(soluong) as soluong, thanhtienTraKM  " +
                            "		FROM " +
                            "		( " +
                            "			SELECT N'11.Trả hàng khuyến mại' as loaict,  b.sanpham_fk, SUM(b.soluong) as soluong, SUM(b.soluong*dongia) thanhtienTraKM " +
                            "			FROM DonTraHang a INNER JOIN DonTraHang_SPKM b on a.pk_seq = b.dontrahang_fk " +
                            "			WHERE CONVERT(datetime, a.ngaydonhang, 105) >= CONVERT(datetime, '" + tungay + "', 105) and a.trangthai = 1 and b.kho_fk = " + khoId + " " +
                            "					and CONVERT(datetime, a.ngaydonhang, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                            "			GROUP BY b.sanpham_fk " +
                            "		) " +
                            "		trahang GROUP BY loaict, sanpham_fk, thanhtienTraKM " +
                            "	) " +
                            "	trahangkhuyenmai on sp.pk_seq = trahangkhuyenmai.sanpham_fk  " +
                            "	LEFT JOIN  " +
                             "	( " +
                             "		SELECT N'12.Nhập chuyển kho' as loaict,  b.sanpham_fk, SUM(c.soluongQuyDoi) as soluong, SUM(b.soluong*b.dongia) thanhtienNhapKhacCK " +
                             "		FROM NHAPKHAC a INNER JOIN NHAPKHAC_SANPHAM b on a.pk_seq = b.NHAPKHAC_FK " +
                             "         INNER JOIN NhapKhac_SanPham_ChiTiet c ON b.nhapkhac_fk= c.nhapkhac_fk AND b.sanpham_fk = c.sanpham_fk " +
                             "		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + tungay + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + "  " +
                             "				and CONVERT(datetime, a.ngaynhap, 105) <= CONVERT(datetime, '" + denngay + "', 105) and loainhap = 1 " +
                             "		GROUP BY b.sanpham_fk " +
                             "	) " +
                             "	nhapchuyenkho on sp.pk_seq = nhapchuyenkho.sanpham_fk  " +
                             "	LEFT JOIN " +
                             "	( " +
                             "		SELECT N'13.Xuất chuyển kho' as loaict,  b.sanpham_fk, SUM(c.soluongQuyDoi) as soluong, SUM(b.soluong*b.dongia) thanhtienXuatKhacCK " +
                             "		FROM XUATKHAC a INNER JOIN XUATKHAC_SANPHAM b on a.pk_seq = b.XUATKHAC_FK " +
                             "              INNER JOIN XuatKhac_SanPham_ChiTiet c ON b.xuatkhac_fk = c.xuatkhac_fk AND b.sanpham_fk = c.sanpham_fk " +
                             "		WHERE CONVERT(datetime, a.ngayxuat, 105) >= CONVERT(datetime, '" + tungay + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + "  " +
                             "				and CONVERT(datetime, a.ngayxuat, 105) <= CONVERT(datetime, '" + denngay + "', 105) and loaixuat = 3 " +
                             "		GROUP BY b.sanpham_fk " +
                             "	) " +
                             "	xuatchuyenkho on sp.pk_seq = xuatchuyenkho.sanpham_fk  " +
                             "	LEFT JOIN " +
                             "	( " +
                             "		SELECT N'14.Nhập khác' as loaict,  b.sanpham_fk, SUM(c.soluongQuyDoi) as soluong, SUM(b.soluong*b.dongia) thanhtienNhapKhac" +
                             "		FROM NHAPKHAC a INNER JOIN NHAPKHAC_SANPHAM b on a.pk_seq = b.NHAPKHAC_FK " +
                             "         INNER JOIN NhapKhac_SanPham_ChiTiet c ON b.nhapkhac_fk= c.nhapkhac_fk AND b.sanpham_fk = c.sanpham_fk " +
                             "		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + tungay + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + "  " +
                             "				and CONVERT(datetime, a.ngaynhap, 105) <= CONVERT(datetime, '" + denngay + "', 105) and loainhap = 2 " +
                             "		GROUP BY b.sanpham_fk " +
                             "	) " +
                             "	nhapkhac on sp.pk_seq = nhapkhac.sanpham_fk  " +
                             "	LEFT JOIN " +
                             "	( " +
                             "		SELECT N'15.Xuất khác' as loaict,  b.sanpham_fk, SUM(c.soluongQuyDoi) as soluong, SUM(b.soluong*b.dongia) thanhtienXuatKhac " +
                             "		FROM XUATKHAC a INNER JOIN XUATKHAC_SANPHAM b on a.pk_seq = b.XUATKHAC_FK " +
                             "              INNER JOIN XuatKhac_SanPham_ChiTiet c ON b.xuatkhac_fk = c.xuatkhac_fk AND b.sanpham_fk = c.sanpham_fk " +
                             "		WHERE CONVERT(datetime, a.ngayxuat, 105) >= CONVERT(datetime, '" + tungay + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + "  " +
                             "				and CONVERT(datetime, a.ngayxuat, 105) <= CONVERT(datetime, '" + denngay + "', 105) and loaixuat != 3 " +
                             "		GROUP BY b.sanpham_fk " +
                             "	) " +
                             "	xuatkhac on sp.pk_seq = xuatkhac.sanpham_fk  " +
                             "	LEFT JOIN  " +
                             "   (  " +
                             "   	SELECT N'18.Nhập lệnh sản xuất' as loaict,  b.sanpham_fk, SUM(b.soluongQUYDOI) as soluong  " +
                             "   	FROM NhapKhac a INNER JOIN NhapKhac_SanPham_ChiTiet b on a.pk_seq = b.nhapkhac_fk  " +
                             "   	WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + tungay + "', 105) and a.trangthai = '1' and a.kho_fk =  " + khoId + "  " +
                             "   			and CONVERT(datetime, a.ngaynhap, 105) <= CONVERT(datetime, '" + denngay + "', 105) and a.loainhap = '4' " +
                             "   	GROUP BY b.sanpham_fk " +
                             "   )  " +
                             "   nhaplsx on sp.pk_seq = nhaplsx.sanpham_fk " +
                             "   LEFT JOIN  " +
                             "   (  " +
                             "   	SELECT N'19.Xuất tiêu hao nguyên liệu' as loaict,  b.vattu_fk as sanpham_fk, SUM(b.soluongQUYDOI) as soluong  " +
                             "   	FROM LenhSanXuat_TieuHao a INNER JOIN LenhSanXuat_TieuHao_ChiTiet b on a.pk_seq = b.tieuhao_fk  " +
                             "   	WHERE CONVERT(datetime, a.ngaytieuhao, 105) >= CONVERT(datetime, '" + tungay + "', 105) and a.trangthai = '1' and a.kho_fk = " + khoId + "    " +
                             "   			and CONVERT(datetime, a.ngaytieuhao, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                             "   	GROUP BY b.vattu_fk " +
                             "   )  " +
                             "   xuatTieuHao on sp.pk_seq = xuatTieuHao.sanpham_fk " +
                            "	WHERE sp.pk_seq > 0 AND sp.pk_seq in (SELECT DISTINCT sanpham_fk FROM Kho_SanPham_ChiTiet WHERE kho_fk = '" + khoId + "') " + condition +
                            ") " +
                            "TRONGKY ON sp.pk_seq = TRONGKY.sanpham_fk " +
                            //" INNER JOIN NganhHang nh on sp.nganhhang_fk = nh.pk_seq " +
                            //" LEFT JOIN ChungLoai cl ON sp.chungloai_fk = cl.pk_seq " +
                            " WHERE 1 = 1 ";

                        if (nganhhangId.Trim().Length > 3)
                            query += " AND sp.nganhhang_fk = '" + nganhhangId + "' ";
                        if (chungloai.Trim().Length > 3)
                            query += " AND sp.chungloai_fk = '" + chungloai + "' ";

                        query += " ORDER BY sp.chungloai_fk, sp.ma ";

                        DataTable dt = xl.ReadTable(query);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TableRow row = new TableRow();

                            string nganhhang = dt.Rows[i]["nganhhang"].ToString();
                            chungloai = dt.Rows[i]["chungloai"].ToString();
                            string masp = dt.Rows[i]["masp"].ToString();
                            string tensp = dt.Rows[i]["tensp"].ToString();
                            string dvt = dt.Rows[i]["dvt"].ToString();

                            double giamua = double.Parse(dt.Rows[i]["giamua"].ToString());
                            double giaban = double.Parse(dt.Rows[i]["giaban"].ToString());
                            double giaton = double.Parse(dt.Rows[i]["giaton"].ToString());

                            if (theogia.Equals("0"))  //XEM THEO GIA MUA
                                giaban = giamua;
                            else if (theogia.Equals("0"))  //XEM THEO GIA BAN
                                giamua = giaban;
                            else
                                giamua = giaton;

                            double tondauKS = double.Parse(dt.Rows[i]["tondauKS"].ToString());

                            double tondau = double.Parse(dt.Rows[i]["tondau"].ToString());
                            //double tt_tondau = giamua * tondau;
                            double tt_tondau = double.Parse(dt.Rows[i]["thanhtienTONDAU"].ToString());

                            double nhapmuaNCC = double.Parse(dt.Rows[i]["nhapNCC"].ToString());
                            //double tt_nhapmuaNCC = giamua * nhapmuaNCC;
                            double tt_nhapmuaNCC = double.Parse(dt.Rows[i]["thanhtienNhap"].ToString());

                            double kiemke = double.Parse(dt.Rows[i]["kiemke"].ToString());
                            //double tt_kiemke = giamua * kiemke;
                            double tt_kiemke = double.Parse(dt.Rows[i]["thanhtienKiemKe"].ToString());

                            double dieuchinh = double.Parse(dt.Rows[i]["dieuchinh"].ToString());
                            //double tt_dieuchinh = giamua * dieuchinh;
                            double tt_dieuchinh = double.Parse(dt.Rows[i]["thanhtienDCTK"].ToString());
                            
                            double trahangban = double.Parse(dt.Rows[i]["trahangban"].ToString());
                            //double tt_trahangban = giamua * trahangban;
                            double tt_trahangban = double.Parse(dt.Rows[i]["thanhtienTraBH"].ToString());

                            double trahangKM = double.Parse(dt.Rows[i]["trahangKM"].ToString());
                            //double tt_trahangKM = giamua * trahangKM;
                            double tt_trahangKM = double.Parse(dt.Rows[i]["thanhtienTraKM"].ToString());

                            double nhapchuyenkho = double.Parse(dt.Rows[i]["nhapchuyenkho"].ToString());
                            //double tt_nhapchuyenkho = giamua * nhapchuyenkho;
                            double tt_nhapchuyenkho = double.Parse(dt.Rows[i]["thanhtienNhapKhacCK"].ToString());

                            double nhapkhac = double.Parse(dt.Rows[i]["nhapkhac"].ToString());
                            //double tt_nhapkhac = giamua * nhapkhac;
                            double tt_nhapkhac = double.Parse(dt.Rows[i]["thanhtienNhapKhac"].ToString()); ;

                            double nhaplenhsanxuat = double.Parse(dt.Rows[i]["nhaplenhsanxuat"].ToString());

                            //XUAT
                            double xuathangBAN = double.Parse(dt.Rows[i]["xuathangBAN"].ToString());
                            //double tt_xuathangBAN = giamua * xuathangBAN;
                            double tt_xuathangBAN = double.Parse(dt.Rows[i]["thanhtienXuat"].ToString());

                            //THEO DON VI
                            //double XUAT_soluongCHUAN = double.Parse(dt.Rows[i]["XUAT_soluongCHUAN"].ToString());
                            //double XUAT_soluongQD = double.Parse(dt.Rows[i]["XUAT_soluongQD"].ToString());

                            double xuathangKM = double.Parse(dt.Rows[i]["xuatKM"].ToString());
                            //double tt_xuathangKM = giamua * xuathangKM;
                            double tt_xuathangKM = double.Parse(dt.Rows[i]["thanhtienXuatKM"].ToString());

                            double trahangNCC = double.Parse(dt.Rows[i]["traNCC"].ToString());
                            //double tt_trahangNCC = giamua * trahangNCC;
                            double tt_trahangNCC = double.Parse(dt.Rows[i]["thanhtienNhapTRA"].ToString());

                            double chiagia = double.Parse(dt.Rows[i]["chiagia"].ToString());
                            //double tt_chiagia = giamua * chiagia;
                            double tt_chiagia = double.Parse(dt.Rows[i]["thanhtienTKCG"].ToString());

                            double xuatkhac = double.Parse(dt.Rows[i]["xuatkhac"].ToString());
                            //double tt_xuatkhac = giamua * xuatkhac;
                            double tt_xuatkhac = double.Parse(dt.Rows[i]["thanhtienXuatKhac"].ToString());

                            double xuatchuyenkho = double.Parse(dt.Rows[i]["xuatchuyenkho"].ToString());
                            //double tt_xuatchuyenkho = giamua * xuatchuyenkho;
                            double tt_xuatchuyenkho = double.Parse(dt.Rows[i]["thanhtienXuatKhacCK"].ToString());

                            double xuattieuhao = double.Parse(dt.Rows[i]["xuattieuhao"].ToString());

                            tondauKS = Math.Round(tondauKS, 3);
                            tondau = Math.Round(tondau, 3);

                            nhapmuaNCC = Math.Round(nhapmuaNCC, 3);
                            kiemke = Math.Round(kiemke, 3);
                            dieuchinh = Math.Round(dieuchinh, 3);
                            trahangNCC = Math.Round(trahangNCC, 3);
                            nhapkhac = Math.Round(nhapkhac, 3);
                            nhapchuyenkho = Math.Round(nhapchuyenkho, 3);
                            dieuchinh = Math.Round(dieuchinh, 3);
                            nhaplenhsanxuat = Math.Round(nhaplenhsanxuat, 3);

                            xuathangBAN = Math.Round(xuathangBAN, 3);
                            xuathangKM = Math.Round(xuathangKM, 3);
                            chiagia = Math.Round(chiagia, 3);
                            xuatkhac = Math.Round(xuatkhac, 3);
                            xuatchuyenkho = Math.Round(xuatchuyenkho, 3);
                            xuattieuhao = Math.Round(xuattieuhao, 3);                           
                            trahangban = Math.Round(trahangban, 3);
                            trahangKM = Math.Round(trahangKM, 3);

                            double tongNHAP = nhapmuaNCC + kiemke + dieuchinh + nhapkhac + nhapchuyenkho + nhaplenhsanxuat - trahangNCC;
                            double tt_tongNHAP = tt_nhapmuaNCC + tt_kiemke + tt_dieuchinh + tt_nhapchuyenkho + tt_nhapkhac;

                            double tongXUAT = xuathangBAN + xuathangKM + chiagia + xuatkhac + xuatchuyenkho + xuattieuhao - trahangban - trahangKM;
                            double tt_tongXUAT = tt_xuathangBAN + tt_xuathangKM + tt_trahangNCC + tt_chiagia + tt_xuatkhac + tt_xuatchuyenkho - tt_trahangban - tt_trahangKM;


                            //double toncuoi = tondau + nhapmuaNCC - trahangNCC + kiemke + dieuchinh - xuathangBAN - xuathangKM + thuhoiBAN + thuhoiKM + chiagia + trahangban + trahangKM;
                            double toncuoi = tondau + tongNHAP - tongXUAT;
                            double tt_toncuoi = giamua * toncuoi;

                            string[] data = new string[] { khoTen, chungloai, masp, tensp, dvt,
                                            FormatString.ForMatNumber(tondauKS.ToString()), "",
                                            FormatString.ForMatNumber(tondau.ToString()), FormatString.ForMatNumber(tt_tondau.ToString()),
                                            FormatString.ForMatNumber(tongNHAP.ToString()), FormatString.ForMatNumber(tt_tongNHAP.ToString()),
                                            FormatString.ForMatNumber(tongXUAT.ToString()), FormatString.ForMatNumber(tt_tongXUAT.ToString()),
                                            FormatString.ForMatNumber(toncuoi.ToString()), FormatString.ForMatNumber(tt_toncuoi.ToString())  };

                            for (int j = 0; j < data.Length; j++)
                            {
                                TableCell cell = new TableCell();

                                //if (data[0].StartsWith("0"))
                                //data[0] = "'" + data[0];

                                if (j == 1)
                                    cell.Width = Unit.Parse("180");
                                else if (j == 2)
                                    cell.Width = Unit.Parse("150");
                                else if (j == 3)
                                    cell.Width = Unit.Parse("400");
                                else
                                    cell.Width = Unit.Parse("80");

                                if(j == 0 || j == 1 || j == 4)
                                    cell.HorizontalAlign = HorizontalAlign.Center;
                                if (j >= 5)
                                    cell.HorizontalAlign = HorizontalAlign.Right;

                                cell.Text = data[j];

                                cell.Font.Size = FontUnit.Point(9);
                                cell.Font.Name = "Arial";

                                row.Cells.Add(cell);
                            }

                            table.Rows.Add(row);
                        }
                    }

                    table.RenderControl(htmlWriter);
                    _exportContent = sb.ToString();
                }
            }
            return _exportContent;
        }
        private string ExportToExcel_NXT_CHITIET_Partner(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string khoId = "";
            if (context.Request.QueryString["khoId"] != null)
                khoId = context.Request.QueryString["khoId"].ToString();

            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

            //khoId = khoId.Substring(0, khoId.Length - 1);

            string nganhhangId = "";
            if (context.Request.QueryString["nganhhang"] != null)
                nganhhangId = context.Request.QueryString["nganhhang"].ToString();

            //string theogia = "0";
            //if (context.Request.QueryString["theogia"] != null)
            //    theogia = context.Request.QueryString["theogia"].ToString();

            //string xemtheo = "0";
            //if (context.Request.QueryString["xemtheo"] != null)
            //    xemtheo = context.Request.QueryString["xemtheo"].ToString();

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
                    header.ColumnSpan = 5;
                    header.Text = "REPORT STOCK IN - OUT PARTNER DETAIL";
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
                    header.ColumnSpan = 5;
                    header.Text = "Date from: " + tungay + " to " + denngay;

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

                    string[] tieude = new string[] { "Partner", "Specification", "Part code", "Description", "Unit", "Location", "Producte date", "Lot No", "SerialNo",
                                                      "Stock closure", "Period " + tungay, "Stock in", "Total", "Stock out", "Total", "Ending stocks " + denngay };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;

                        //if (i == 10) //Tồn đầu
                        //    header.ColumnSpan = 2;
                        //if (i == 11) //Tồn đầu
                        //    header.ColumnSpan = 6;
                        if (i == 11) //Nhập
                            header.ColumnSpan = 2;
                        //else if (i == 13) //Total
                        //    header.ColumnSpan = 4;
                        //else if (i == 14) //Xuất
                        //    header.ColumnSpan = 2;
                        //else if (i == 14) //Total
                        //    header.ColumnSpan = 2;
                        //else if (i == 16) //Ending stocks
                        //    header.ColumnSpan = 2;
                        //else
                        //    header.RowSpan = 2;

                        header.BackColor = Color.LightGray;
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    tieude = new string[] {"", "", "", "", "", "", "", "", "", "",
                                        "",
                                        "Stock in",
                                        "",
                                        "Stock out",
                                        "", "" };

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

                    string query = "SELECT TOP(1) nam, thang, thangks  " +
                    "FROM " +
                    "( " +
                    "	SELECT nam, thang, '01-' + case when thang < 10 then '0' + cast(thang as varchar(10)) else cast(thang as varchar(10)) end + '-' + cast(nam as varchar(10)) as thangks " +
                    "	FROM KHOASOTHANG   " +
                    ") " +
                    "DATA " +
                    "WHERE CONVERT(datetime, thangks, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                    "ORDER BY nam desc, thang desc ";

                    DataTable dtKS = xl.ReadTable(query);

                    string thangks = "";
                    string namks = "";
                    int thangTIEPTHEO = 0;
                    int namTIEPTHEO = 0;
                    if (dtKS.Rows.Count > 0)
                    {
                        thangks = dtKS.Rows[0]["thang"].ToString();
                        namks = dtKS.Rows[0]["nam"].ToString();
                        thangTIEPTHEO = int.Parse(dtKS.Rows[0]["thang"].ToString());
                        namTIEPTHEO = int.Parse(dtKS.Rows[0]["nam"].ToString());

                        if (int.Parse(thangks) == 12)
                        {
                            thangTIEPTHEO = 1;
                            namTIEPTHEO += 1;
                        }
                        else
                            thangTIEPTHEO += 1;
                    }

                    string dauthang = "01-" + (thangTIEPTHEO < 10 ? "0" + thangTIEPTHEO.ToString() : thangTIEPTHEO.ToString()) + "-" + namTIEPTHEO.ToString();

                    string[] khoIds = Regex.Split(khoId, ",");
                    string[] thangBC = Regex.Split(tungay, "-");


                    query = " SELECT kh.ma AS khachhang, (SELECT ten FROM NganhHang WHERE pk_seq = sp.nganhhang_fk) nganhhang, ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') chungloai, " +
                        "    sp.ma AS masp, sp.nameEnglish AS tensp, (SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk) AS dvt, " +
                        "    DAUKY.xuatxu_fk, ISNULL((SELECT ma FROM Location WHERE pk_seq = DAUKY.location_fk), '') location, DAUKY.bin_fk, " +
                        "    ISNULL(DAUKY.lotNo, '') lotNo, ISNULL(DAUKY.serialNo, '') serialNo, ISNULL(DAUKY.solo, '') solo, " +
                        "    ISNULL(dauky.tondauKS, 0) AS tondauKS, ISNULL(dauky.soluong, 0) AS tondau, 1 AS thanhtienTONDAU, " +
                        "    ISNULL(TRONGKY.nhapkhac, 0) nhapkhac, ISNULL(TRONGKY.xuatkhac, 0) xuatkhac  " +
                        "  FROM SanPham sp LEFT JOIN " +
                        " ( " +
                        " 	SELECT  '" + khachhang + "' AS khachhang_fk, kho.sanpham_fk, kho.xuatxu_fk, kho.location_fk, kho.bin_fk, kho.lotNo, kho.serialNo, kho.solo, ISNULL(tondau.soluong, 0) AS tondauKS,  " +
                        " 		ISNULL(tondau.soluong, 0) + ISNULL(nhapkhac.soluong, 0) - ISNULL(xuatkhac.soluong, 0) AS soluong " +
                        " 	FROM KhoKyGui_SanPham_ChiTiet kho " +
                        " 	LEFT JOIN " +
                        " 	( " +
                        " 		SELECT N'1.Tồn đầu' AS loaict, sanpham_fk, xuatxu_fk, location_fk, bin_fk, lotNo, serialNo, solo, soluong, 1 AS thanhtienTONDAU  " +
                        " 		FROM TonKhoKyGuiThang_ChiTiet" +
                        " 		WHERE thang = '" + thangks + "' AND nam = '" + namks + "' AND nhomkhachhang_fk = '" + khachhang + "'  " +
                        " 	) " +
                        " 	tondau ON kho.sanpham_fk = tondau.sanpham_fk AND kho.xuatxu_fk = tondau.xuatxu_fk AND kho.location_fk = tondau.location_fk AND kho.bin_fk = tondau.bin_fk " +
                        " 		AND kho.lotNo = tondau.lotNo AND kho.serialNo = tondau.serialNo AND kho.solo = tondau.solo " +
                        " LEFT JOIN " +
                        " 	( " +
                        " 		SELECT N'14.Nhập khác' AS loaict, b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, SUM(b.soluongQUYDOI) AS soluong, SUM(b.soluong*b.dongia) thanhtienNhapKhac " +
                        " 		FROM NhapKhac a INNER JOIN NhapKhac_SanPham_ChiTiet b ON a.pk_seq = b.nhapkhac_fk           " +
                        " 		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.khachhang_fk = '" + khachhang + "'   " +
                        " 				and CONVERT(datetime, a.ngaynhap, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                        " 		GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo " +
                        " 	) " +
                        " 	nhapkhac ON kho.sanpham_fk = nhapkhac.sanpham_fk AND kho.xuatxu_fk = nhapkhac.xuatxu_fk AND kho.location_fk = nhapkhac.location_fk AND kho.bin_fk = nhapkhac.bin_fk " +
                        " 		AND kho.lotNo = nhapkhac.lotNo AND kho.serialNo = nhapkhac.serialNo AND kho.solo = nhapkhac.solo  		" +
                        "    LEFT JOIN  " +
                        "    ( " +
                        " 		SELECT N'15.Xuất khác' AS loaict,  b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienXuatKhac " +
                        " 		FROM XuatKhac a INNER JOIN XuatKhac_SanPham_ChiTiet b ON a.pk_seq = b.xuatkhac_fk " +
                        " 		WHERE CONVERT(datetime, a.ngayxuat, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.khachhang_fk = '" + khachhang + "'   " +
                        " 				and CONVERT(datetime, a.ngayxuat, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                        " 		GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo " +
                        " 	) " +
                        " 	xuatkhac ON kho.sanpham_fk = xuatkhac.sanpham_fk AND kho.xuatxu_fk = xuatkhac.xuatxu_fk AND kho.location_fk = xuatkhac.location_fk AND kho.bin_fk = xuatkhac.bin_fk " +
                        " 		AND kho.lotNo = xuatkhac.lotNo AND kho.serialNo = xuatkhac.serialNo AND kho.solo = xuatkhac.solo 	" +
                        " 	WHERE kho.khachhang_fk = '" + khachhang + "'  " +
                        " ) " +
                        " DAUKY ON sp.pk_seq = DAUKY.sanpham_fk " +
                        " LEFT JOIN " +
                        " ( " +
                        " 	SELECT  '" + khachhang + "'  AS khachhang_fk, kho.sanpham_fk, kho.xuatxu_fk, kho.location_fk, kho.bin_fk, kho.lotNo, kho.serialNo, kho.solo, " +
                        " 		ISNULL(nhapkhac.soluong, 0) AS nhapkhac, ISNULL(nhapkhac.thanhtienNhapKhac, 0) AS thanhtienNhapKhac, " +
                        " 		ISNULL(xuatkhac.soluong, 0) AS xuatkhac, ISNULL(xuatkhac.thanhtienXuatKhac, 0) AS thanhtienXuatKhac  " +
                        " 	FROM KhoKyGui_SanPham_ChiTiet kho " +
                        " 	LEFT JOIN " +
                        " 	( " +
                        " 		SELECT N'14.Nhập khác' AS loaict, b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienNhapKhac" +
                        " 		FROM NhapKhac a INNER JOIN NhapKhac_SanPham_ChiTiet b ON a.pk_seq = b.nhapkhac_fk " +
                        " 		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.khachhang_fk = '" + khachhang + "'   " +
                        " 				and CONVERT(datetime, a.ngaynhap, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                        " 		GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo " +
                        " 	) " +
                        " 	nhapkhac ON kho.sanpham_fk = nhapkhac.sanpham_fk AND kho.xuatxu_fk = nhapkhac.xuatxu_fk AND kho.location_fk = nhapkhac.location_fk AND kho.bin_fk = nhapkhac.bin_fk " +
                        " 		AND kho.lotNo = nhapkhac.lotNo AND kho.serialNo = nhapkhac.serialNo AND kho.solo = nhapkhac.solo " +
                        " 	LEFT JOIN " +
                        " 	( " +
                        " 		SELECT N'15.Xuất khác' AS loaict,  b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienXuatKhac " +
                        " 		FROM XuatKhac a INNER JOIN XuatKhac_SanPham_ChiTiet b ON a.pk_seq = b.xuatkhac_fk " +
                        " 		WHERE CONVERT(datetime, a.ngayxuat, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.khachhang_fk = '" + khachhang + "'   " +
                        " 				and CONVERT(datetime, a.ngayxuat, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                        " 		GROUP BY b.sanpham_fk, b.xuatxu_fk, b.location_fk, b.bin_fk, b.lotNo, b.serialNo, b.solo " +
                        " 	) " +
                        " " +
                        " xuatkhac ON kho.sanpham_fk = xuatkhac.sanpham_fk AND kho.xuatxu_fk = xuatkhac.xuatxu_fk AND kho.location_fk = xuatkhac.location_fk AND kho.bin_fk = xuatkhac.bin_fk " +
                        " 		AND kho.lotNo = xuatkhac.lotNo AND kho.serialNo = xuatkhac.serialNo AND kho.solo = xuatkhac.solo 	" +
                        " 	WHERE kho.khachhang_fk = '" + khachhang + "'  " +
                        " ) " +
                        " TRONGKY ON DAUKY.sanpham_fk = TRONGKY.sanpham_fk AND DAUKY.xuatxu_fk = TRONGKY.xuatxu_fk AND DAUKY.location_fk = TRONGKY.location_fk AND DAUKY.bin_fk = TRONGKY.bin_fk " +
                        " 		AND DAUKY.lotNo = TRONGKY.lotNo AND DAUKY.serialNo = TRONGKY.serialNo AND DAUKY.solo = TRONGKY.solo " +
                        //"  INNER JOIN NganhHang nh ON sp.nganhhang_fk = nh.pk_seq " +
                        "  INNER JOIN KhachHang kh ON Dauky.khachhang_fk = kh.pk_seq " +
                        //"  LEFT JOIN ChungLoai cl ON sp.chungloai_fk = cl.pk_seq " +
                        "  WHERE 1 = 1 AND sp.pk_seq in (SELECT DISTINCT sanpham_fk FROM KhoKyGui_SanPham_ChiTiet WHERE khachhang_fk = '" + khachhang + "') ";

                    //if (nganhhangId.Trim().Length > 0)
                    //    query += " AND sp.nganhhang in ( SELECT ten FROM NGANHHANG WHERE pk_seq = '" + nganhhangId + "' ) ";

                    DataTable dt = xl.ReadTable(query);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();
                        khachhang = dt.Rows[i]["khachhang"].ToString();
                        string nganhhang = dt.Rows[i]["nganhhang"].ToString();
                        string chungloai = dt.Rows[i]["chungloai"].ToString();
                        string masp = dt.Rows[i]["masp"].ToString();
                        string tensp = dt.Rows[i]["tensp"].ToString();
                        string dvt = dt.Rows[i]["dvt"].ToString();

                        string location = dt.Rows[i]["location"].ToString();
                        //string bin = dt.Rows[i]["bin"].ToString();
                        //string xuatxu = dt.Rows[i]["xuatxu"].ToString();
                        string lotNo = dt.Rows[i]["lotNo"].ToString();
                        string serialNo = dt.Rows[i]["serialNo"].ToString();
                        string solo = dt.Rows[i]["solo"].ToString();
                        //string ngaynhap = dt.Rows[i]["ngaynhap"].ToString();

                       
                        double tondauKS = double.Parse(dt.Rows[i]["tondauKS"].ToString());

                        double tondau = double.Parse(dt.Rows[i]["tondau"].ToString());
                        //double tt_tondau = double.Parse(dt.Rows[i]["thanhtienTONDAU"].ToString());
                        
                        double nhapkhac = double.Parse(dt.Rows[i]["nhapkhac"].ToString());
                        
                        //XUAT
                        double xuatkhac = double.Parse(dt.Rows[i]["xuatkhac"].ToString());
                        
                        tondauKS = Math.Round(tondauKS, 3);
                        tondau = Math.Round(tondau, 3);
                        
                        nhapkhac = Math.Round(nhapkhac, 3);

                        xuatkhac = Math.Round(xuatkhac, 3);
                       
                        
                        double tongNHAP = nhapkhac;
                        //double tt_tongNHAP = tt_nhapmuaNCC + tt_kiemke  - tt_trahangNCC;

                        double tongXUAT = xuatkhac;
                        //double tt_tongXUAT = tt_xuathangBAN - tt_trahangban;

                        double toncuoi = tondau + tongNHAP - tongXUAT;
                        //double tt_toncuoi = giamua * toncuoi;

                        string[] data = new string[] { khachhang, chungloai, masp, tensp, dvt, location, solo, lotNo, serialNo,
                                            FormatString.ForMatNumber(tondauKS.ToString()),
                                            FormatString.ForMatNumber(tondau.ToString()),
                                            FormatString.ForMatNumber(nhapkhac.ToString()), 
                                            
                                            FormatString.ForMatNumber(tongNHAP.ToString()),
                                            FormatString.ForMatNumber(xuatkhac.ToString()),
                                            "",
                                            FormatString.ForMatNumber(tongXUAT.ToString()),
                                            FormatString.ForMatNumber(toncuoi.ToString())  };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();

                            //if (data[0].StartsWith("0"))
                            //data[0] = "'" + data[0];

                            if (j == 3)
                                cell.Width = Unit.Parse("450");

                            if (j >= 9)
                            {
                                cell.Width = Unit.Parse("100");
                                cell.HorizontalAlign = HorizontalAlign.Right;
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
        private string ExportToExcel_NXT_LotNo_Partner(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string khoId = "";
            if (context.Request.QueryString["khoId"] != null)
                khoId = context.Request.QueryString["khoId"].ToString();

            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

            //khoId = khoId.Substring(0, khoId.Length - 1);

            string nganhhangId = "";
            if (context.Request.QueryString["nganhhang"] != null)
                nganhhangId = context.Request.QueryString["nganhhang"].ToString();

            //string theogia = "0";
            //if (context.Request.QueryString["theogia"] != null)
            //    theogia = context.Request.QueryString["theogia"].ToString();

            //string xemtheo = "0";
            //if (context.Request.QueryString["xemtheo"] != null)
            //    xemtheo = context.Request.QueryString["xemtheo"].ToString();

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
                    header.ColumnSpan = 5;
                    header.Text = "REPORT STOCK IN - OUT PARTER FOLLOW LOTNO";
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
                    header.ColumnSpan = 5;
                    header.Text = "Date from: " + tungay + " to " + denngay;

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

                    string[] tieude = new string[] { "Partner", "Specification", "Part code", "Description", "Unit", "Lot No",
                                                      "Stock closure", "Period " + tungay, "Stock in", "Total", "Stock out", "Total", "Ending stocks " + denngay };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;

                        //if (i == 10) //Tồn đầu
                        //    header.ColumnSpan = 2;
                        //if (i == 11) //Tồn đầu
                        //    header.ColumnSpan = 6;
                        //if (i == 11) //Nhập
                        //    header.ColumnSpan = 2;
                        //else if (i == 13) //Total
                        //    header.ColumnSpan = 4;
                        //else if (i == 14) //Xuất
                        //    header.ColumnSpan = 2;
                        //else if (i == 14) //Total
                        //    header.ColumnSpan = 2;
                        //else if (i == 16) //Ending stocks
                        //    header.ColumnSpan = 2;
                        //else
                        //    header.RowSpan = 2;

                        header.BackColor = Color.LightGray;
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    tieude = new string[] {"", "", "", "", "", "", "",
                                        "",
                                        "Stock in",
                                        "",
                                        "Stock out",
                                        "", "" };

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

                    string query = "SELECT TOP(1) nam, thang, thangks  " +
                    "FROM " +
                    "( " +
                    "	SELECT nam, thang, '01-' + case when thang < 10 then '0' + cast(thang as varchar(10)) else cast(thang as varchar(10)) end + '-' + cast(nam as varchar(10)) as thangks " +
                    "	FROM KHOASOTHANG   " +
                    ") " +
                    "DATA " +
                    "WHERE CONVERT(datetime, thangks, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                    "ORDER BY nam desc, thang desc ";

                    DataTable dtKS = xl.ReadTable(query);

                    string thangks = "";
                    string namks = "";
                    int thangTIEPTHEO = 0;
                    int namTIEPTHEO = 0;
                    if (dtKS.Rows.Count > 0)
                    {
                        thangks = dtKS.Rows[0]["thang"].ToString();
                        namks = dtKS.Rows[0]["nam"].ToString();
                        thangTIEPTHEO = int.Parse(dtKS.Rows[0]["thang"].ToString());
                        namTIEPTHEO = int.Parse(dtKS.Rows[0]["nam"].ToString());

                        if (int.Parse(thangks) == 12)
                        {
                            thangTIEPTHEO = 1;
                            namTIEPTHEO += 1;
                        }
                        else
                            thangTIEPTHEO += 1;
                    }

                    string dauthang = "01-" + (thangTIEPTHEO < 10 ? "0" + thangTIEPTHEO.ToString() : thangTIEPTHEO.ToString()) + "-" + namTIEPTHEO.ToString();

                    string[] khoIds = Regex.Split(khoId, ",");
                    string[] thangBC = Regex.Split(tungay, "-");


                    query = " SELECT DISTINCT kh.ma AS khachhang, (SELECT ten FROM NganhHang WHERE pk_seq = sp.nganhhang_fk) nganhhang, ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') chungloai, " +
                        "    sp.ma AS masp, sp.nameEnglish AS tensp, (SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk) AS dvt, " +
                        "    ISNULL(DAUKY.lotNo, '') lotNo, " +
                        "    ISNULL(dauky.tondauKS, 0) AS tondauKS, ISNULL(dauky.soluong, 0) AS tondau, ISNULL(dauky.thanhtienTONDAU, 0) AS thanhtienTONDAU, " +
                        "    ISNULL(TRONGKY.nhapkhac, 0) nhapkhac, ISNULL(TRONGKY.xuatkhac, 0) xuatkhac " + 
                        "  FROM SanPham sp LEFT JOIN " +
                        " ( " +
                        " 	SELECT DISTINCT '" + khachhang + "' AS khachhang_fk, kho.sanpham_fk, kho.lotNo, ISNULL(tondau.soluong, 0) AS tondauKS, tondau.thanhtienTONDAU, " +
                        " 		ISNULL(tondau.soluong, 0) + ISNULL(nhapkhac.soluong, 0) - ISNULL(xuatkhac.soluong, 0) AS soluong " + 
                        " 	FROM KhoKyGui_SanPham_ChiTiet kho " +
                        " 	LEFT JOIN " +
                        " 	( " +
                        " 		SELECT N'1.Tồn đầu' AS loaict, sanpham_fk, lotNo, SUM(soluong) soluong, 1 AS thanhtienTONDAU  " +
                        " 		FROM TonKhoKyGuiThang_ChiTiet" +
                        " 		WHERE thang = '" + thangks + "' AND nam = '" + namks + "' AND nhomkhachhang_fk = '" + khachhang + "'  " +
                        "       GROUP BY sanpham_fk, lotNo " +
                        " 	) " +
                        " 	tondau ON kho.sanpham_fk = tondau.sanpham_fk AND kho.lotNo = tondau.lotNo " +
                        " LEFT JOIN " +
                        " 	( " +
                        " 		SELECT N'14.Nhập khác' AS loaict, b.sanpham_fk, b.lotNo, SUM(b.soluongQUYDOI) AS soluong, SUM(b.soluong*b.dongia) thanhtienNhapKhac " +
                        " 		FROM NhapKhac a INNER JOIN NhapKhac_SanPham_ChiTiet b ON a.pk_seq = b.nhapkhac_fk           " +
                        " 		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.khachhang_fk = '" + khachhang + "'   " +
                        " 				and CONVERT(datetime, a.ngaynhap, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                        " 		GROUP BY b.sanpham_fk, b.lotNo " +
                        " 	) " +
                        " 	nhapkhac ON kho.sanpham_fk = nhapkhac.sanpham_fk AND kho.lotNo = nhapkhac.lotNo " +
                        "    LEFT JOIN  " +
                        "    ( " +
                        " 		SELECT N'15.Xuất khác' AS loaict,  b.sanpham_fk, b.lotNo, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienXuatKhac " +
                        " 		FROM XuatKhac a INNER JOIN XuatKhac_SanPham_ChiTiet b ON a.pk_seq = b.xuatkhac_fk " +
                        " 		WHERE CONVERT(datetime, a.ngayxuat, 105) >= CONVERT(datetime, '" + dauthang + "', 105) AND a.trangthai = '1' AND a.khachhang_fk = '" + khachhang + "'   " +
                        " 				and CONVERT(datetime, a.ngayxuat, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                        " 		GROUP BY b.sanpham_fk, b.lotNo " +
                        " 	) " +
                        " 	xuatkhac ON kho.sanpham_fk = xuatkhac.sanpham_fk AND kho.lotNo = xuatkhac.lotNo " +
                        " 	WHERE kho.khachhang_fk = '" + khachhang + "' " +
                        " ) " +
                        " DAUKY ON sp.pk_seq = DAUKY.sanpham_fk " +
                        " LEFT JOIN " +
                        " ( " +
                        " 	SELECT DISTINCT '" + khachhang + "'  AS khachhang_fk, kho.sanpham_fk, kho.lotNo, " +
                        " 		ISNULL(nhapkhac.soluong, 0) AS nhapkhac, ISNULL(nhapkhac.thanhtienNhapKhac, 0) AS thanhtienNhapKhac, " +
                        " 		ISNULL(xuatkhac.soluong, 0) AS xuatkhac, ISNULL(xuatkhac.thanhtienXuatKhac, 0) AS thanhtienXuatKhac  " +
                        " 	FROM KhoKyGui_SanPham_ChiTiet kho " +
                        " 	LEFT JOIN " +
                        " 	( " +
                        " 		SELECT N'14.Nhập khác' AS loaict, b.sanpham_fk, b.lotNo,SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienNhapKhac" +
                        " 		FROM NhapKhac a INNER JOIN NhapKhac_SanPham_ChiTiet b ON a.pk_seq = b.nhapkhac_fk " +
                        " 		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.khachhang_fk = '" + khachhang + "'   " +
                        " 				and CONVERT(datetime, a.ngaynhap, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                        " 		GROUP BY b.sanpham_fk, b.lotNo " +
                        " 	) " +
                        " 	nhapkhac ON kho.sanpham_fk = nhapkhac.sanpham_fk AND kho.lotNo = nhapkhac.lotNo " +
                        " 	LEFT JOIN " +
                        " 	( " +
                        " 		SELECT N'15.Xuất khác' AS loaict,  b.sanpham_fk, b.lotNo, SUM(b.soluongQuyDoi) AS soluong, SUM(b.soluong*b.dongia) thanhtienXuatKhac " +
                        " 		FROM XuatKhac a INNER JOIN XuatKhac_SanPham_ChiTiet b ON a.pk_seq = b.xuatkhac_fk " +
                        " 		WHERE CONVERT(datetime, a.ngayxuat, 105) >= CONVERT(datetime, '" + tungay + "', 105) AND a.trangthai = '1' AND a.khachhang_fk = '" + khachhang + "'   " +
                        " 				and CONVERT(datetime, a.ngayxuat, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                        " 		GROUP BY b.sanpham_fk, b.lotNo " +
                        " 	) " +
                        " " +
                        " xuatkhac ON kho.sanpham_fk = xuatkhac.sanpham_fk AND kho.lotNo = xuatkhac.lotNo " +
                        " 	WHERE kho.khachhang_fk = '" + khachhang + "'  " +
                        " ) " +
                        " TRONGKY ON DAUKY.sanpham_fk = TRONGKY.sanpham_fk AND DAUKY.lotNo = TRONGKY.lotNo " +
                        " INNER JOIN KhachHang kh ON DauKy.khachhang_fk = kh.pk_seq " +
                        //"  INNER JOIN NganhHang nh ON sp.nganhhang_fk = nh.pk_seq " +
                        //"  LEFT JOIN ChungLoai cl ON sp.chungloai_fk = cl.pk_seq " +
                        "  WHERE 1 = 1 AND sp.pk_seq in (SELECT DISTINCT sanpham_fk FROM KhoKyGui_SanPham_ChiTiet WHERE khachhang_fk = '" + khachhang + "') ";

                    //if (nganhhangId.Trim().Length > 0)
                    //    query += " AND sp.nganhhang in ( SELECT ten FROM NGANHHANG WHERE pk_seq = '" + nganhhangId + "' ) ";

                    DataTable dt = xl.ReadTable(query);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();
                        khachhang = dt.Rows[i]["khachhang"].ToString();
                        string nganhhang = dt.Rows[i]["nganhhang"].ToString();
                        string chungloai = dt.Rows[i]["chungloai"].ToString();
                        string masp = dt.Rows[i]["masp"].ToString();
                        string tensp = dt.Rows[i]["tensp"].ToString();
                        string dvt = dt.Rows[i]["dvt"].ToString();

                        //string location = dt.Rows[i]["location"].ToString();
                        //string bin = dt.Rows[i]["bin"].ToString();
                        //string xuatxu = dt.Rows[i]["xuatxu"].ToString();
                        string lotNo = dt.Rows[i]["lotNo"].ToString();
                        //string serialNo = dt.Rows[i]["serialNo"].ToString();
                        //string solo = dt.Rows[i]["solo"].ToString();
                        //string ngaynhap = dt.Rows[i]["ngaynhap"].ToString();

                        //double giamua = double.Parse(dt.Rows[i]["giamua"].ToString());
                        //double giaban = double.Parse(dt.Rows[i]["giaban"].ToString());
                        //double giaton = double.Parse(dt.Rows[i]["giaton"].ToString());

                        //if (theogia.Equals("0"))  //XEM THEO GIA MUA
                        //    giaban = giamua;
                        //else if (theogia.Equals("0"))  //XEM THEO GIA BAN
                        //    giamua = giaban;
                        //else
                        // giamua = giaton;

                        double tondauKS = double.Parse(dt.Rows[i]["tondauKS"].ToString());

                        double tondau = double.Parse(dt.Rows[i]["tondau"].ToString());
                        //double tt_tondau = double.Parse(dt.Rows[i]["thanhtienTONDAU"].ToString());
                        
                        double nhapkhac = double.Parse(dt.Rows[i]["nhapkhac"].ToString());
                        
                        //XUAT
                        double xuatkhac = double.Parse(dt.Rows[i]["xuatkhac"].ToString());
                        
                        tondauKS = Math.Round(tondauKS, 3);
                        tondau = Math.Round(tondau, 3);
                        
                        nhapkhac = Math.Round(nhapkhac, 3);
                       
                        xuatkhac = Math.Round(xuatkhac, 3);
                        
                        double tongNHAP = nhapkhac;

                        double tongXUAT = xuatkhac;

                        double toncuoi = tondau + tongNHAP - tongXUAT;
                       
                        string[] data = new string[] { khachhang, chungloai, masp, tensp, dvt, lotNo,
                                            FormatString.ForMatNumber(tondauKS.ToString()),
                                            FormatString.ForMatNumber(tondau.ToString()),
                                            FormatString.ForMatNumber(nhapkhac.ToString()), 
                                            //FormatString.ForMatNumber(trahangNCC.ToString()), 
                                            //FormatString.ForMatNumber(kiemke.ToString()),
                                            FormatString.ForMatNumber(tongNHAP.ToString()),
                                            FormatString.ForMatNumber(xuatkhac.ToString()),
                                            //FormatString.ForMatNumber(trahangban.ToString()),
                                            FormatString.ForMatNumber(tongXUAT.ToString()),
                                            FormatString.ForMatNumber(toncuoi.ToString())  };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();

                            //if (data[0].StartsWith("0"))
                            //data[0] = "'" + data[0];

                            if (j == 3)
                                cell.Width = Unit.Parse("450");

                            if (j >= 6)
                            {
                                cell.Width = Unit.Parse("100");
                                cell.HorizontalAlign = HorizontalAlign.Right;
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
        private string NhapXuatTonTong_Partner(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string khoId = "";
            if (context.Request.QueryString["khoId"] != null)
                khoId = context.Request.QueryString["khoId"].ToString();

            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

            //khoId = khoId.Substring(0, khoId.Length - 1);

            string nganhhangId = "";
            if (context.Request.QueryString["nganhhangId"] != null)
                nganhhangId = context.Request.QueryString["nganhhangId"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();

            string theogia = "0";
            if (context.Request.QueryString["theogia"] != null)
                theogia = context.Request.QueryString["theogia"].ToString();

            string xemtheo = "0";
            if (context.Request.QueryString["xemtheo"] != null)
                xemtheo = context.Request.QueryString["xemtheo"].ToString();

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
                    header.ColumnSpan = 15;
                    header.Text = "TOTAL STOCK IN-OUT PARTNER REPORT ";
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

                    header.Text = "Date from: " + tungay + " to " + denngay;
                    header.ColumnSpan = 15;
                    header.Font.Bold = true;
                    //header.BackColor = Color.LightGray;
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);

                    table.Rows.Add(headerRow);

                    //Tieu de
                    headerRow = new TableRow();
                    header = new TableHeaderCell();

                    header.Text = "Created date: " + DateTime.Now.ToString("dd-MM-yyyy");
                    header.ColumnSpan = 15;
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

                    string[] tieude = new string[] { "Partner", "Specification", "PartCode", "Description", "Unit",
                                    "Stock closure ", "Start - " + tungay, "Stock in", "Stock out", "Ending stocks - " + denngay };

                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;

                        if (i >= 5) //Số hiệu chứng từ
                            header.ColumnSpan = 2;

                        header.BackColor = Color.LightGray;
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);


                    tieude = new string[] { "", "", "", "", "", "Quantity", "", "Quantity", "", "Quantity", "", "Quantity", "", "Quantity", "" };

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

                    string query = "SELECT TOP(1) nam, thang, thangks  " +
                    "FROM " +
                    "( " +
                    "	SELECT nam, thang, '01-' + case when thang < 10 then '0' + cast(thang as varchar(10)) else cast(thang as varchar(10)) end + '-' + cast(nam as varchar(10)) as thangks " +
                    "	FROM KHOASOTHANG   " +
                    ") " +
                    "DATA " +
                    "WHERE CONVERT(datetime, thangks, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                    "ORDER BY nam desc, thang desc ";

                    DataTable dtKS = xl.ReadTable(query);

                    string thangks = "";
                    string namks = "";
                    int thangTIEPTHEO = 0;
                    int namTIEPTHEO = 0;
                    if (dtKS.Rows.Count > 0)
                    {
                        thangks = dtKS.Rows[0]["thang"].ToString();
                        namks = dtKS.Rows[0]["nam"].ToString();
                        thangTIEPTHEO = int.Parse(dtKS.Rows[0]["thang"].ToString());
                        namTIEPTHEO = int.Parse(dtKS.Rows[0]["nam"].ToString());

                        if (int.Parse(thangks) == 12)
                        {
                            thangTIEPTHEO = 1;
                            namTIEPTHEO += 1;
                        }
                        else
                            thangTIEPTHEO += 1;
                    }

                    string dauthang = "01-" + (thangTIEPTHEO < 10 ? "0" + thangTIEPTHEO.ToString() : thangTIEPTHEO.ToString()) + "-" + namTIEPTHEO.ToString();
                    string condition = "";
                    if (nganhhangId.Trim().Length > 3)
                        condition += " and sp.nganhhang_fk = '" + nganhhangId + "' ";
                    if (chungloai.Trim().Length > 3)
                        condition += " and sp.chungloai_fk = '" + chungloai + "' ";

                    string[] khoIds = Regex.Split(khoId, ",");
                    string[] thangBC = Regex.Split(tungay, "-");

                    query = "SELECT kh.ma AS khachhang, (SELECT ten FROM NganhHang WHERE pk_seq = sp.nganhhang_fk) nganhhang, ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') chungloai, " +
                        "   sp.ma as masp, sp.nameEnglish as tensp, (SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk) as dvt, " +
                        "   ISNULL(dauky.tondauKS, 0) as tondauKS, ISNULL(dauky.soluong, 0) as tondau, ISNULL(dauky.thanhtienTONDAU, 0) as thanhtienTONDAU,  " +
                        "   TRONGKY.nhapkhac, TRONGKY.xuatkhac " +
                        " FROM SanPham sp LEFT JOIN " +
                        "( " +
                        "	SELECT " + khachhang + " as khachhang_fk, sp.pk_seq as sanpham_fk,  ISNULL(tondau.soluong, 0) as tondauKS,  " +
                        "			ISNULL(tondau.soluong, 0) +  " +
                        "			ISNULL(nhapkhac.soluong, 0) - ISNULL(xuatkhac.soluong, 0) AS soluong,   " +
                        "			1 AS giaton, ISNULL(tondau.thanhtienTONDAU, 0) as thanhtienTONDAU " +
                        "	FROM SanPham sp " +
                        "	LEFT JOIN " +
                        "	( " +
                        "		SELECT N'1.Tồn đầu' as loaict, sanpham_fk, SUM(soluong) as soluong, SUM(soluong * giaton) as thanhtienTONDAU  " +
                        "		FROM TonKhoKyGuiThang   " +
                        "		WHERE THANG = '" + thangks + "' and NAM = '" + namks + "' and nhomkhachhang_fk = " + khachhang + " " +
                        "		GROUP BY sanpham_fk " +
                        "	) " +
                        "	tondau on sp.pk_seq = tondau.sanpham_fk  " +
                         "	LEFT JOIN " +
                         "	( " +
                         "		SELECT N'14.Nhập khác' as loaict,  b.sanpham_fk, SUM(b.soluong) as soluong, 1 thanhtienNhapKhac " +
                         "		FROM NHAPKHAC a INNER JOIN NhapKhac_SanPham_ChiTiet b ON a.pk_seq = b.nhapkhac_fk " +
                         "		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + dauthang + "', 105) and a.trangthai = '1' and a.khachhang_fk = " + khachhang + "  " +
                         "				and CONVERT(datetime, a.ngaynhap, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                         "		GROUP BY b.sanpham_fk " +
                         "	) " +
                         "	nhapkhac on sp.pk_seq = nhapkhac.sanpham_fk  " +
                         "	LEFT JOIN " +
                         "	( " +
                         "		SELECT N'15.Xuất khác' as loaict, b.sanpham_fk, SUM(b.soluong) as soluong, 1 thanhtienXuatKhac " +
                         "		FROM XUATKHAC a INNER JOIN XuatKhac_SanPham_ChiTiet b ON a.pk_seq = b.xuatkhac_fk " +
                         "		WHERE CONVERT(datetime, a.ngayxuat, 105) >= CONVERT(datetime, '" + dauthang + "', 105) and a.trangthai = '1' and a.khachhang_fk = " + khachhang + "  " +
                         "				and CONVERT(datetime, a.ngayxuat, 105) < CONVERT(datetime, '" + tungay + "', 105) " +
                         "		GROUP BY b.sanpham_fk " +
                         "	) " +
                         "	xuatkhac on sp.pk_seq = xuatkhac.sanpham_fk  " +
                        "	WHERE sp.pk_seq > 0 " + condition +
                        ") " +
                        "DAUKY ON sp.pk_seq = DAUKY.sanpham_fk LEFT JOIN " +
                        "( " +
                        "	SELECT " + khachhang + " as khachhang_fk, sp.pk_seq as sanpham_fk,   " +
                        "			ISNULL(nhapkhac.soluong, 0) as nhapkhac, ISNULL(nhapkhac.thanhtienNhapKhac, 0) as thanhtienNhapKhac," +
                        "           ISNULL(xuatkhac.soluong, 0) as xuatkhac, ISNULL(xuatkhac.thanhtienXuatKhac, 0) as thanhtienXuatKhac " +
                        "	FROM SanPham sp  " +
                         "	LEFT JOIN " +
                         "	( " +
                         "		SELECT N'14.Nhập khác' as loaict,  b.sanpham_fk, SUM(b.soluongQuyDoi) as soluong, 1 thanhtienNhapKhac" +
                         "		FROM NHAPKHAC a INNER JOIN NhapKhac_SanPham_ChiTiet b ON a.pk_seq = b.nhapkhac_fk " +
                         "		WHERE CONVERT(datetime, a.ngaynhap, 105) >= CONVERT(datetime, '" + tungay + "', 105) and a.trangthai = '1' and a.khachhang_fk = " + khachhang + "  " +
                         "				and CONVERT(datetime, a.ngaynhap, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                         "		GROUP BY b.sanpham_fk " +
                         "	) " +
                         "	nhapkhac on sp.pk_seq = nhapkhac.sanpham_fk  " +
                         "	LEFT JOIN " +
                         "	( " +
                         "		SELECT N'15.Xuất khác' as loaict, b.sanpham_fk, SUM(b.soluongQuyDoi) as soluong, 1 thanhtienXuatKhac " +
                         "		FROM XUATKHAC a INNER JOIN XuatKhac_SanPham_ChiTiet b ON a.pk_seq = b.xuatkhac_fk " +
                         "		WHERE CONVERT(datetime, a.ngayxuat, 105) >= CONVERT(datetime, '" + tungay + "', 105) and a.trangthai = '1' and a.khachhang_fk = " + khachhang + "  " +
                         "				and CONVERT(datetime, a.ngayxuat, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                         "		GROUP BY b.sanpham_fk " +
                         "	) " +
                         "	xuatkhac on sp.pk_seq = xuatkhac.sanpham_fk  " +
                        "	WHERE sp.pk_seq > 0 " + condition +
                        ") " +
                        "TRONGKY ON sp.pk_seq = TRONGKY.sanpham_fk " +
                        " INNER JOIN KhachHang kh ON DAUKY.khachhang_fk = kh.pk_seq " + 
                        " WHERE 1 = 1 AND sp.pk_seq in (SELECT DISTINCT sanpham_fk FROM KhoKyGui_SanPham_ChiTiet WHERE khachhang_fk = '" + khachhang + "') ";

                    if (nganhhangId.Trim().Length > 3)
                        query += " AND sp.nganhhang_fk = '" + nganhhangId + "' ";
                    if (chungloai.Trim().Length > 3)
                        query += " AND sp.chungloai_fk = '" + chungloai + "' ";

                    query += " ORDER BY sp.chungloai_fk, sp.ma ";

                    DataTable dt = xl.ReadTable(query);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        khachhang = dt.Rows[i]["khachhang"].ToString();
                        string nganhhang = dt.Rows[i]["nganhhang"].ToString();
                        chungloai = dt.Rows[i]["chungloai"].ToString();
                        string masp = dt.Rows[i]["masp"].ToString();
                        string tensp = dt.Rows[i]["tensp"].ToString();
                        string dvt = dt.Rows[i]["dvt"].ToString();
                        
                        double tondauKS = double.Parse(dt.Rows[i]["tondauKS"].ToString());

                        double tondau = double.Parse(dt.Rows[i]["tondau"].ToString());
                        //double tt_tondau = giamua * tondau;
                        double tt_tondau = double.Parse(dt.Rows[i]["thanhtienTONDAU"].ToString());
                        
                        double nhapkhac = double.Parse(dt.Rows[i]["nhapkhac"].ToString());
                       
                        //XUAT
                        double xuatkhac = double.Parse(dt.Rows[i]["xuatkhac"].ToString());
                      
                        tondauKS = Math.Round(tondauKS, 3);
                        tondau = Math.Round(tondau, 3);
                        
                        nhapkhac = Math.Round(nhapkhac, 3);
                       
                        xuatkhac = Math.Round(xuatkhac, 3);
                        
                        double tongNHAP = nhapkhac;
                        
                        double tongXUAT = xuatkhac ;
                        
                        double toncuoi = tondau + tongNHAP - tongXUAT;
                        
                        string[] data = new string[] { khachhang, chungloai, masp, tensp, dvt,
                                            FormatString.ForMatNumber(tondauKS.ToString()), "",
                                            FormatString.ForMatNumber(tondau.ToString()), "",
                                            FormatString.ForMatNumber(tongNHAP.ToString()), "",
                                            FormatString.ForMatNumber(tongXUAT.ToString()), "",
                                            FormatString.ForMatNumber(toncuoi.ToString()), "" };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            
                            if (j == 1)
                                cell.Width = Unit.Parse("180");
                            else if (j == 2)
                                cell.Width = Unit.Parse("150");
                            else if (j == 3)
                                cell.Width = Unit.Parse("400");
                            else
                                cell.Width = Unit.Parse("80");

                            if (j >= 5)
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
        private string ExportToExcel_Inventory_Total(HttpContext context)
        {           
            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();

            string nganhhang = "";
            if (context.Request.QueryString["nganhhang"] != null)
                nganhhang = context.Request.QueryString["nganhhang"].ToString();

            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

            string solo = "";
            if (context.Request.QueryString["solo"] != null)
                solo = context.Request.QueryString["solo"].ToString();

            string plNo = "";
            if (context.Request.QueryString["plNo"] != null)
                plNo = context.Request.QueryString["plNo"].ToString();

            string lotNo = "";
            if (context.Request.QueryString["lotNo"] != null)
                lotNo = context.Request.QueryString["lotNo"].ToString();

            string location = "";
            if (context.Request.QueryString["location"] != null)
                location = context.Request.QueryString["location"].ToString();

            string pallet = "";
            if (context.Request.QueryString["pallet"] != null)
                pallet = context.Request.QueryString["pallet"].ToString();

            string loaihanghoa = "";
            if (context.Request.QueryString["loaihanghoa"] != null)
                loaihanghoa = context.Request.QueryString["loaihanghoa"].ToString();

            string sanpham = "";
            if (context.Request.QueryString["sanpham"] != null)
                sanpham = context.Request.QueryString["sanpham"].ToString();

            string ngaynhap = "";
            if (context.Request.QueryString["ngaynhap"] != null)
                ngaynhap = context.Request.QueryString["ngaynhap"].ToString();

            string loaibaocao = "";
            if (context.Request.QueryString["loaibaocao"] != null)
                loaibaocao = context.Request.QueryString["loaibaocao"].ToString();

            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();
            string _exportContent = "";
          
            DataTable baocao = new DataTable("Inventory_Total");

            baocao.Columns.Add("Item", typeof(string));
            baocao.Columns.Add("Customer", typeof(string));
            baocao.Columns.Add("Factory", typeof(string));
            baocao.Columns.Add("Unit", typeof(string));
            baocao.Columns.Add("Quantity", typeof(double));
            
            ExecuteData xl = new ExecuteData();

            string condition = "";
            string conditionCT = "";

            // check quyền
            string sql = "SELECT ISNULL(khachhang_fk, 0) FROM NhanVien WHERE pk_seq = '" + userId + "' ";
            object obj = xl.ExecuteScalarSQL(sql);
            if (int.Parse(obj.ToString()) > 0)
                khachhang = obj.ToString();

            if (chungloai.Trim().Length > 3)
                condition += " and sp.chungloai_fk = '" + chungloai + "' ";
            if (nganhhang.Trim().Length > 3)
                condition += " and sp.nganhhang_fk = '" + nganhhang + "' ";
            if (khachhang.Trim().Length > 3)
            {
                condition += " and ct.khachhang_fk = '" + khachhang + "' ";
                conditionCT += " and khachhang_fk = '" + khachhang + "' ";
            }
            if (sanpham.Trim().Length > 3)
                condition += " and ct.sanpham_fk = '" + sanpham + "' ";
            if (location.Trim().Length > 3)
            {
                condition += " and ct.location_fk = '" + location + "' ";
                conditionCT += " and location_fk = '" + location + "' ";
            }
            if (pallet.Trim().Length > 3)
                condition += " and ct.pallet_fk = '" + pallet + "' ";

            if (ngaynhap.Trim().Length > 3)
            {
                condition += " and ct.ngaynhap = '" + ngaynhap + "' ";
                conditionCT += " and ngaynhap = '" + ngaynhap + "' ";
            }
            if (solo.Trim().Length > 0)
            {
                condition += " and ct.solo = N'" + solo + "' ";
                conditionCT += " and solo = N'" + solo + "' ";
            }
            if (plNo.Trim().Length > 0)
            {
                condition += " and ct.plNo = N'" + plNo + "' ";
                conditionCT += " and plNo = N'" + plNo + "' ";
            }
            if (lotNo.Trim().Length > 0)
            {
                condition += " and ct.lotNo = N'" + lotNo + "' ";
                conditionCT += " and lotNo = N'" + lotNo + "' ";
            }
            if (loaibaocao.Trim().Equals("1"))
            {

            }
            else if (loaibaocao.Trim().Equals("2"))
            {
                condition += " AND ct.soluong = 0 ";
                condition += " AND soluong = 0 ";
            }
            else if (loaibaocao.Trim().Equals("3"))
            {
                condition += " AND ct.soluong > 0 ";
                conditionCT += " AND soluong > 0 ";
            }

            sql = " SELECT ct.sanpham_fk, sp.ma AS masp, sp.ten AS tensp, sp.codeEnglish, dv.ma as donvi, ct.utcOrderNo AS factory, " +
                "	ISNULL((SELECT ma FROM KhachHang WHERE pk_seq = ct.khachhang_fk), '') khachhang, " +
                " 	ISNULL((SELECT COUNT(*) FROM(SELECT DISTINCT mavach FROM Kho_SanPham_ChiTiet WHERE sanpham_fk = ct.sanpham_fk AND khachhang_fk = ct.khachhang_fk AND utcOrderNo = ct.utcOrderNo " + conditionCT + ") A), 0) soluong " +
                " FROM SanPham sp " +
                " 		INNER JOIN DonViTinh dv on sp.dvt_fk = dv.pk_seq " +
                " 		INNER JOIN Kho_SanPham_ChiTiet ct on sp.pk_seq = ct.sanpham_fk " + condition +
                " WHERE ct.pk_seq > " +
                " GROUP BY ct.sanpham_fk, sp.ma, sp.codeEnglish, dv.ma, ct.utcOrderNo, ct.khachhang_fk ";

            sql += " ORDER BY ct.khachhang_fk, ct.utcOrderNo, sp.ma ASC ";
           
            DataTable dt = xl.ReadTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                
                string ma = dt.Rows[i]["masp"].ToString();
                string ten = dt.Rows[i]["tensp"].ToString();
                string codeEnglish = dt.Rows[i]["codeEnglish"].ToString();
                khachhang = dt.Rows[i]["khachhang"].ToString();
                string donvi = dt.Rows[i]["donvi"].ToString();
                string factory = dt.Rows[i]["factory"].ToString();
                
                string soluong = dt.Rows[i]["soluong"].ToString();
               
                DataRow dr = baocao.NewRow();

                dr[0] = ma;
                dr[1] = khachhang;
                dr[2] = factory;
                dr[3] = donvi;
                dr[4] = FormatString.ForMatNumber(soluong);
                
                baocao.Rows.Add(dr);
            }

            string fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\Inventory_Total.xlsm";

            Workbook workbook = new Workbook(fileName);
            Worksheet worksheet = workbook.Worksheets[0];

            worksheet.Cells.ImportDataTable(baocao, true, "A4");
            worksheet.AutoFitColumns();

            worksheet.Cells["A1"].PutValue("INVENTORY - TOTAL");

            worksheet.Cells["A2"].PutValue("Created date");
            worksheet.Cells["C2"].PutValue(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));

            worksheet.Cells["A3"].PutValue("Username");
            worksheet.Cells["C3"].PutValue(context.Session["userName"].ToString());

            HttpResponse response = context.Response;
            workbook.Save(response, "Inventory_Total.xlsm", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsm));

            return _exportContent;
        }
        private string ExportToExcel_Inventory_Color(HttpContext context)
        {
            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();

            string nganhhang = "";
            if (context.Request.QueryString["nganhhang"] != null)
                nganhhang = context.Request.QueryString["nganhhang"].ToString();

            string line = "";
            if (context.Request.QueryString["line"] != null)
                line = context.Request.QueryString["line"].ToString();

            string loaibaocao = "";
            if (context.Request.QueryString["loaibaocao"] != null)
                loaibaocao = context.Request.QueryString["loaibaocao"].ToString();

            string loailocation = "";
            if (context.Request.QueryString["loailocation"] != null)
                loailocation = context.Request.QueryString["loailocation"].ToString();

            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();
            string _exportContent = "";

            DataTable baocao = new DataTable("Inventory_Color");

            baocao.Columns.Add("Model", typeof(string));
            baocao.Columns.Add("Grade", typeof(string));
            baocao.Columns.Add("Color", typeof(string));
            baocao.Columns.Add("Quantity", typeof(double));
            baocao.Columns.Add("Booked", typeof(double));
            baocao.Columns.Add("Avai", typeof(double));

            ExecuteData xl = new ExecuteData();

            string condition = "";

            if (chungloai.Trim().Length > 3)
                condition += " and sp.chungloai_fk = '" + chungloai + "' ";

            if (nganhhang.Trim().Length > 3)
                condition += " and sp.nganhhang_fk = '" + nganhhang + "' ";

            if (line.Trim().Length > 3)
                condition += " and ct.line_fk = '" + line + "' ";


            if (loaibaocao.Trim().Equals("1"))
            {

            }
            else if (loaibaocao.Trim().Equals("2"))
            {
                condition += " AND ct.soluong = 0 ";
            }
            else if (loaibaocao.Trim().Equals("3"))
            {
                condition += " AND ct.soluong > 0 ";
            }

            if (loailocation.Trim().Length > 0)
            {
                condition += " AND ct.location_fk in (SELECT pk_seq FROM Location WHERE dasudung = '" + loailocation + "') ";
            }

            string sql = " SELECT sp.ma, sp.ten, dv.ma as donvi, " +
                 "   ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') chungloai, " +
                 "   ISNULL((SELECT ten FROM MauSac WHERE pk_seq = ct.mausac_fk), '') mausac, " +
                 " 	    ISNULL(SUM(ct.soluong), 0) as soluong, ISNULL(SUM(ct.booked), 0) as booked, ISNULL(SUM(ct.avai), 0) as  avai " +
                 " FROM SanPham sp  " +
                 " 		INNER JOIN DonViTinh dv on sp.dvt_fk = dv.pk_seq " +
                 " 		INNER JOIN Kho_SanPham_ChiTiet ct on sp.pk_seq = ct.sanpham_fk and ct.kho_fk = '" + kho + "' " + condition +
                 " WHERE ct.pk_seq > 0  " +
                 " GROUP BY sp.ma, sp.ten, dv.ma, sp.chungloai_fk, ct.mausac_fk ";

            sql += " ORDER BY sp.ma ASC ";

            DataTable dt = xl.ReadTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                
                string ma = dt.Rows[i]["ma"].ToString();
                string ten = dt.Rows[i]["ten"].ToString();
                string donvi = dt.Rows[i]["donvi"].ToString();
                chungloai = dt.Rows[i]["chungloai"].ToString();
                string mausac = dt.Rows[i]["mausac"].ToString();

                string soluong = dt.Rows[i]["soluong"].ToString();
                string booked = dt.Rows[i]["booked"].ToString();
                string avai = dt.Rows[i]["avai"].ToString();

                DataRow dr = baocao.NewRow();


                dr[0] = ten;
                dr[1] = chungloai;
                dr[2] = mausac;
                dr[3] = FormatString.ForMatNumber(soluong);
                dr[4] = FormatString.ForMatNumber(booked);
                dr[5] = FormatString.ForMatNumber(avai);

                baocao.Rows.Add(dr);
            }

            string fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\Inventory_Color.xlsm";

            Workbook workbook = new Workbook(fileName);
            Worksheet worksheet = workbook.Worksheets[0];

            worksheet.Cells.ImportDataTable(baocao, true, "A4");
            worksheet.AutoFitColumns();

            worksheet.Cells["A1"].PutValue("INVENTORY - COLOR");

            worksheet.Cells["A2"].PutValue("Created date");
            worksheet.Cells["C2"].PutValue(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));

            worksheet.Cells["A3"].PutValue("Username");
            worksheet.Cells["C3"].PutValue(context.Session["userName"].ToString());

            HttpResponse response = context.Response;
            workbook.Save(response, "Inventory_Color.xlsm", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsm));

            return _exportContent;
        }
        private string ExportToExcel_Inventory_Line(HttpContext context)
        {
            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();

            string nganhhang = "";
            if (context.Request.QueryString["nganhhang"] != null)
                nganhhang = context.Request.QueryString["nganhhang"].ToString();

            string line = "";
            if (context.Request.QueryString["line"] != null)
                line = context.Request.QueryString["line"].ToString();

            string loaibaocao = "";
            if (context.Request.QueryString["loaibaocao"] != null)
                loaibaocao = context.Request.QueryString["loaibaocao"].ToString();

            string loailocation = "";
            if (context.Request.QueryString["loailocation"] != null)
                loailocation = context.Request.QueryString["loailocation"].ToString();

            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();
            string _exportContent = "";

            DataTable baocao = new DataTable("Inventory_Line");

            baocao.Columns.Add("Model", typeof(string));
            baocao.Columns.Add("Grade", typeof(string));
            baocao.Columns.Add("Line", typeof(string));
            baocao.Columns.Add("Quantity", typeof(double));
            baocao.Columns.Add("Booked", typeof(double));
            baocao.Columns.Add("Avai", typeof(double));

            ExecuteData xl = new ExecuteData();

            string condition = "";

            if (chungloai.Trim().Length > 3)
                condition += " and sp.chungloai_fk = '" + chungloai + "' ";

            if (nganhhang.Trim().Length > 3)
                condition += " and sp.nganhhang_fk = '" + nganhhang + "' ";

            if (line.Trim().Length > 3)
                condition += " and ct.line_fk = '" + line + "' ";


            if (loaibaocao.Trim().Equals("1"))
            {

            }
            else if (loaibaocao.Trim().Equals("2"))
            {
                condition += " AND ct.soluong = 0 ";
            }
            else if (loaibaocao.Trim().Equals("3"))
            {
                condition += " AND ct.soluong > 0 ";
            }

            if (loailocation.Trim().Length > 0)
            {
                condition += " AND ct.location_fk in (SELECT pk_seq FROM Location WHERE dasudung = '" + loailocation + "') ";
            }

            string sql = " SELECT sp.ma, sp.ten, dv.ma as donvi, line.ten AS line, " +
                "   ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') chungloai, " +
                 " 	ISNULL(SUM(ct.soluong), 0) as soluong, ISNULL(SUM(ct.booked), 0) as booked, ISNULL(SUM(ct.avai), 0) as  avai " +
                 " FROM SanPham sp  " +
                 " 		INNER JOIN DonViTinh dv on sp.dvt_fk = dv.pk_seq " +
                 " 		INNER JOIN Kho_SanPham_ChiTiet ct on sp.pk_seq = ct.sanpham_fk and ct.kho_fk = '" + kho + "' " + condition +
                 " 		INNER JOIN Line line on ct.line_fk = line.pk_seq " +
                 " WHERE ct.pk_seq > 0  " +
                 " GROUP BY sp.ma, sp.ten, dv.ma, line.ten, sp.chungloai_fk ";
            sql += " ORDER BY line.ten, sp.ten ASC ";

            DataTable dt = xl.ReadTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                
                string ma = dt.Rows[i]["ma"].ToString();
                string ten = dt.Rows[i]["ten"].ToString();
                string donvi = dt.Rows[i]["donvi"].ToString();
                chungloai = dt.Rows[i]["chungloai"].ToString();
                line = dt.Rows[i]["line"].ToString();

                string soluong = dt.Rows[i]["soluong"].ToString();
                string booked = dt.Rows[i]["booked"].ToString();
                string avai = dt.Rows[i]["avai"].ToString();

                DataRow dr = baocao.NewRow();


                dr[0] = ten;
                dr[1] = chungloai;
                dr[2] = line;
                dr[3] = FormatString.ForMatNumber(soluong);
                dr[4] = FormatString.ForMatNumber(booked);
                dr[5] = FormatString.ForMatNumber(avai);

                baocao.Rows.Add(dr);
            }

            string fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\Inventory_Line.xlsm";

            Workbook workbook = new Workbook(fileName);
            Worksheet worksheet = workbook.Worksheets[0];

            worksheet.Cells.ImportDataTable(baocao, true, "A4");
            worksheet.AutoFitColumns();

            worksheet.Cells["A1"].PutValue("INVENTORY - LINE");

            worksheet.Cells["A2"].PutValue("Created date");
            worksheet.Cells["C2"].PutValue(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));

            worksheet.Cells["A3"].PutValue("Username");
            worksheet.Cells["C3"].PutValue(context.Session["userName"].ToString());

            HttpResponse response = context.Response;
            workbook.Save(response, "Inventory_Line.xlsm", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsm));

            return _exportContent;
        }
        private string ExportToExcel_Inventory_Detail(HttpContext context)
        {
            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();

            string nganhhang = "";
            if (context.Request.QueryString["nganhhang"] != null)
                nganhhang = context.Request.QueryString["nganhhang"].ToString();

            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

            string solo = "";
            if (context.Request.QueryString["solo"] != null)
                solo = context.Request.QueryString["solo"].ToString();

            string plNo = "";
            if (context.Request.QueryString["plNo"] != null)
                plNo = context.Request.QueryString["plNo"].ToString();

            string lotNo = "";
            if (context.Request.QueryString["lotNo"] != null)
                lotNo = context.Request.QueryString["lotNo"].ToString();

            string location = "";
            if (context.Request.QueryString["location"] != null)
                location = context.Request.QueryString["location"].ToString();

            string pallet = "";
            if (context.Request.QueryString["pallet"] != null)
                pallet = context.Request.QueryString["pallet"].ToString();

            string loaihanghoa = "";
            if (context.Request.QueryString["loaihanghoa"] != null)
                loaihanghoa = context.Request.QueryString["loaihanghoa"].ToString();

            string sanpham = "";
            if (context.Request.QueryString["sanpham"] != null)
                sanpham = context.Request.QueryString["sanpham"].ToString();

            string ngaynhap = "";
            if (context.Request.QueryString["ngaynhap"] != null)
                ngaynhap = context.Request.QueryString["ngaynhap"].ToString();

            string loaibaocao = "";
            if (context.Request.QueryString["loaibaocao"] != null)
                loaibaocao = context.Request.QueryString["loaibaocao"].ToString();

            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();
            string _exportContent = "";

            ExecuteData xl = new ExecuteData();

            // check quyền
            string sql = "SELECT ISNULL(khachhang_fk, 0) FROM NhanVien WHERE pk_seq = '" + userId + "' ";
            object obj = xl.ExecuteScalarSQL(sql);
            if (int.Parse(obj.ToString()) > 0)
                khachhang = obj.ToString();

            string condition = "";

            if (chungloai.Trim().Length > 3)
                condition += " and sp.chungloai_fk = '" + chungloai + "' ";
            if (nganhhang.Trim().Length > 3)
                condition += " and sp.nganhhang_fk = '" + nganhhang + "' ";
            if (khachhang.Trim().Length > 3)
                condition += " and ct.khachhang_fk = '" + khachhang + "' ";
            if (sanpham.Trim().Length > 3)
                condition += " and ct.sanpham_fk = '" + sanpham + "' ";
            if (location.Trim().Length > 3)
                condition += " and ct.location_fk = '" + location + "' ";
            if (pallet.Trim().Length > 3)
                condition += " and ct.pallet_fk = '" + pallet + "' ";

            if (ngaynhap.Trim().Length > 0)
                condition += " and ct.ngaynhap = N'" + ngaynhap + "' ";
            if (solo.Trim().Length > 0)
                condition += " and ct.solo = N'" + solo + "' ";
            if (plNo.Trim().Length > 0)
                condition += " and ct.plNo = N'" + plNo + "' ";
            if (lotNo.Trim().Length > 0)
                condition += " and ct.lotNo = N'" + lotNo + "' ";

            if (loaibaocao.Trim().Equals("1"))
            {

            }
            else if (loaibaocao.Trim().Equals("2"))
            {
                condition += " AND ct.soluong = 0 ";
            }
            else if (loaibaocao.Trim().Equals("3"))
            {
                condition += " AND ct.soluong > 0 ";
            }


            sql = " SELECT ISNULL(MAX(soluong), 0) soluong " +
            " FROM " +
            " ( " +
            "	SELECT COUNT(*) soluong, mavach " +
            "	FROM Kho_SanPham_ChiTiet ct INNER JOIN SanPham sp ON ct.sanpham_fk = sp.pk_seq " +
            "	WHERE ct.pk_seq > 0 " + condition +
            "	GROUP BY ct.mavach " +
            " ) A ";
            obj = xl.ExecuteScalarSQL(sql);

            int maxQtySize = 0;

            if (obj != null)
                maxQtySize = int.Parse(obj.ToString());

            DataTable baocao = new DataTable("Inventory_Detail");

            //baocao.Columns.Add("Customer", typeof(string));
            baocao.Columns.Add("Date", typeof(string));
            baocao.Columns.Add("Contract No.", typeof(string));
            baocao.Columns.Add("Factory", typeof(string));            
            baocao.Columns.Add("Item No.", typeof(string));
            baocao.Columns.Add("Unit", typeof(string));
            baocao.Columns.Add("Barcode", typeof(string));
            baocao.Columns.Add("PL No.", typeof(string));
            baocao.Columns.Add("Color/Size", typeof(string));
            baocao.Columns.Add("Order No.", typeof(string));
            baocao.Columns.Add("Carton No", typeof(string));
            baocao.Columns.Add("LotNo", typeof(string));
            baocao.Columns.Add("NoOf", typeof(string));
            baocao.Columns.Add("Qty", typeof(string));
            baocao.Columns.Add("Net Weight", typeof(string));
            baocao.Columns.Add("Gross Weight", typeof(string));            
            baocao.Columns.Add("Location", typeof(string));

            sql = " SELECT ct.sanpham_fk, ct.ngaynhap, sp.ma AS masp, sp.codeEnglish, dv.ma as donvi, ct.mavach, ct.solo, ct.utcOrderNo AS factory, ct.plNo, ct.cartonNo, ct.lotNo, " +
                "   ct.netWeight, ct.grossWeight, ct.NoOf, ct.roll, ct.yards, ct.planShipping, " +
                "   ISNULL((SELECT ten FROM Location WHERE pk_seq = ct.location_fk), '') location, " +
                "   ISNULL((SELECT ten FROM Size WHERE pk_seq = ct.mausac_fk), '') size, " +
                " 	SUM(ct.soluong) soluong, SUM(ct.booked) booked, SUM(ct.avai) avai " +
                " FROM SanPham sp  " +
                " 		INNER JOIN DonViTinh dv on sp.dvt_fk = dv.pk_seq " +
                " 		INNER JOIN Kho_SanPham_ChiTiet ct on sp.pk_seq = ct.sanpham_fk " + condition +
                " WHERE ct.pk_seq > 0  " +
                " GROUP BY ct.sanpham_fk, ct.ngaynhap, sp.ma, sp.codeEnglish, dv.ma, ct.mavach, ct.solo, ct.utcOrderNo, ct.plNo, ct.cartonNo, ct.lotNo, ct.mausac_fk, ct.NoOf,  " +
                " ct.netWeight, ct.grossWeight, ct.roll, ct.yards, ct.planShipping, ct.location_fk, ct.sortBy ";


            sql += " ORDER BY CONVERT(datetime, ct.ngaynhap, 105), ct.solo, sp.ma, ct.sortBy ASC ";

            DataTable dt = xl.ReadTable(sql);

            double totalCarton = dt.Rows.Count;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = baocao.NewRow();

                ngaynhap = dt.Rows[i]["ngaynhap"].ToString();
                solo = dt.Rows[i]["solo"].ToString();                
                string factory = dt.Rows[i]["factory"].ToString();
                string masp = dt.Rows[i]["masp"].ToString();
                string donvi = dt.Rows[i]["donvi"].ToString();
                string mavach = dt.Rows[i]["mavach"].ToString();
                plNo = dt.Rows[i]["plNo"].ToString();
                string size = dt.Rows[i]["size"].ToString();
                string orderNo = dt.Rows[i]["planShipping"].ToString();
                string cartonNo = dt.Rows[i]["cartonNo"].ToString();
                lotNo = dt.Rows[i]["lotNo"].ToString();
                string NoOf = dt.Rows[i]["NoOf"].ToString();
                
                string soluong = FormatString.ForMatNumber(dt.Rows[i]["soluong"].ToString());
                string netWeight = FormatString.ForMatNumber(dt.Rows[i]["netWeight"].ToString());
                string grossWeight = FormatString.ForMatNumber(dt.Rows[i]["grossWeight"].ToString());
                
                location = dt.Rows[i]["location"].ToString();

                dr[0] = ngaynhap;
                dr[1] = solo;
                dr[2] = factory;
                dr[3] = masp;
                dr[4] = donvi;
                dr[5] = mavach;
                dr[6] = plNo;
                dr[7] = size;
                dr[8] = orderNo;
                dr[9] = cartonNo;
                dr[10] = lotNo;
                dr[11] = NoOf;
                dr[12] = soluong;
                dr[13] = netWeight;
                dr[14] = grossWeight;                
                dr[15] = location;

                baocao.Rows.Add(dr);
            }

            string fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\Inventory_Detail.xlsm";

            Workbook workbook = new Workbook(fileName);
            Worksheet worksheet = workbook.Worksheets[0];

            worksheet.Cells.ImportDataTable(baocao, true, "A4");
            
            worksheet.AutoFitColumns();

            worksheet.Cells["A1"].PutValue("INVENTORY - DETAIL");

            worksheet.Cells["A2"].PutValue("Created date");
            worksheet.Cells["C2"].PutValue(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));

            worksheet.Cells["A3"].PutValue("Username");
            worksheet.Cells["C3"].PutValue(context.Session["userName"].ToString());

            HttpResponse response = context.Response;
            workbook.Save(response, "Inventory_Detail.xlsm", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsm));

            return _exportContent;
        }
        private string ExportToExcel_Inventory_Partner_Total(HttpContext context)
        {
            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();

            string nganhhang = "";
            if (context.Request.QueryString["nganhhang"] != null)
                nganhhang = context.Request.QueryString["nganhhang"].ToString();

            string loaibaocao = "";
            if (context.Request.QueryString["loaibaocao"] != null)
                loaibaocao = context.Request.QueryString["loaibaocao"].ToString();

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
                    header.Text = "REPORT INVENTORY PARTNER ";
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

                    header.Text = "Created date: " + ngaythang;
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


                    string[] tieude = new string[] { "Partner", "Specification","PartCode", "Description", "Unit",
                        "Quantity", "Booked", "Avaiable" };

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

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();

                    string condition = "  ";
                    if (chungloai.Trim().Length > 3)
                        condition += " AND sp.chungloai_fk = '" + chungloai + "' ";
                    if (nganhhang.Trim().Length > 3)
                        condition += " AND sp.nganhhang_fk = '" + nganhhang + "' ";
                    if (khachhang.Trim().Length > 3)
                        condition += " AND ct.khachhang_fk = '" + khachhang + "' ";

                    if (loaibaocao.Trim().Equals("1"))
                    {

                    }
                    else if (loaibaocao.Trim().Equals("2"))
                    {
                        condition += " AND ct.soluong = 0 ";
                    }
                    else if (loaibaocao.Trim().Equals("3"))
                    {
                        condition += " AND ct.soluong > 0 ";
                    }

                    string sql = " SELECT ISNULL((SELECT ma FROM KhachHang WHERE pk_seq = ct.khachhang_fk), '') khachhang, sp.ma, sp.nameEnglish AS ten, (SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk) as donvi, " +
                    " 	ISNULL(SUM(ct.soluong), 0) as soluong, ISNULL(SUM(ct.booked), 0) as booked, ISNULL(SUM(ct.avai), 0) AS  avai,  " + 
                    "   ISNULL((SELECT ten FROM NganhHang WHERE pk_seq = sp.nganhhang_fk), '') nganhhang, " +
                    "   ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') chungloai " +
                    " FROM SanPham sp  " +
                    " 		INNER JOIN KhoKyGui_SanPham_ChiTiet ct on sp.pk_seq = ct.sanpham_fk " + condition +
                    " WHERE sp.trangthai = 1  " +
                    " GROUP BY ct.kho_fk, ct.khachhang_fk, sp.ma, sp.nameEnglish, sp.dvt_fk, sp.nganhhang_fk, sp.chungloai_fk " +
                    " ORDER BY sp.ma ASC ";

                    DataTable dt = xl.ReadTable(sql);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string ma = dt.Rows[i]["ma"].ToString();
                        string tensp = dt.Rows[i]["ten"].ToString();
                        string donvi = dt.Rows[i]["donvi"].ToString();
                        nganhhang = dt.Rows[i]["nganhhang"].ToString();
                        chungloai = dt.Rows[i]["chungloai"].ToString();
                        double tonkho = double.Parse(dt.Rows[i]["soluong"].ToString());
                        double booked = double.Parse(dt.Rows[i]["booked"].ToString());
                        double avai = double.Parse(dt.Rows[i]["avai"].ToString());

                        string[] data = new string[] { dt.Rows[i]["khachhang"].ToString(), chungloai, ma, tensp, donvi,
                            FormatString.ForMatNumber(tonkho.ToString()), FormatString.ForMatNumber(booked.ToString()), FormatString.ForMatNumber(avai.ToString()) };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            cell.VerticalAlign = VerticalAlign.Middle;

                            if (j == 3)
                                cell.Width = Unit.Parse("350");
                            else if (j == 0 || j == 1 || j == 2)
                                cell.Width = Unit.Parse("150");
                            else
                                cell.Width = Unit.Parse("100");
                            
                            if (j < 2 || j == 5)
                                cell.HorizontalAlign = HorizontalAlign.Center;
                            if (j >= 6)
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
        private string ExportToExcel_Inventory_Partner_LotNo(HttpContext context)
        {
            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();

            string nganhhang = "";
            if (context.Request.QueryString["nganhhang"] != null)
                nganhhang = context.Request.QueryString["nganhhang"].ToString();

            string loaibaocao = "";
            if (context.Request.QueryString["loaibaocao"] != null)
                loaibaocao = context.Request.QueryString["loaibaocao"].ToString();

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
                    header.Text = "REPORT INVENTORY PARTNER - LOTNO ";
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

                    header.Text = "Created date: " + ngaythang;
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


                    string[] tieude = new string[] { "Partner", "Specification", "PartCode", "Description", "Unit",
                        "LotNo", "Quantity", "Booked", "Avaiable" };

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

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();

                    string condition = "  ";
                    if (chungloai.Trim().Length > 3)
                        condition += " AND sp.chungloai_fk = '" + chungloai + "' ";
                    if (nganhhang.Trim().Length > 3)
                        condition += " AND sp.nganhhang_fk = '" + nganhhang + "' ";
                    if (khachhang.Trim().Length > 3)
                        condition += " AND ct.khachhang_fk = '" + khachhang + "' ";

                    if (loaibaocao.Trim().Equals("1"))
                    {

                    }
                    else if (loaibaocao.Trim().Equals("2"))
                    {
                        condition += " AND ct.soluong = 0 ";
                    }
                    else if (loaibaocao.Trim().Equals("3"))
                    {
                        condition += " AND ct.soluong > 0 ";
                    }

                    string sql = " SELECT ISNULL((SELECT ma FROM Khachhang WHERE pk_seq = ct.khachhang_fk), '') khachhang, sp.ma, sp.nameEnglish AS ten, (SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk) as donvi, " +
                    " 	ISNULL(SUM(ct.soluong), 0) as soluong, ISNULL(SUM(ct.booked), 0) as booked, ISNULL(SUM(ct.avai), 0) as  avai, ct.lotNo, " +
                    "   ISNULL((SELECT ten FROM NganhHang WHERE pk_seq = sp.nganhhang_fk), '') nganhhang, " +
                    "   ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') chungloai " +
                    " FROM SanPham sp  " +
                    " 		INNER JOIN KhoKyGui_SanPham_ChiTiet ct on sp.pk_seq = ct.sanpham_fk " + condition +
                    " WHERE sp.trangthai = 1  " +
                    " GROUP BY ct.kho_fk, ct.khachhang_fk, sp.ma, sp.nameEnglish, sp.dvt_fk, sp.nganhhang_fk, sp.chungloai_fk, ct.lotNo " +
                    " ORDER BY sp.ma, ct.lotNo ASC ";

                    DataTable dt = xl.ReadTable(sql);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string ma = dt.Rows[i]["ma"].ToString();
                        string tensp = dt.Rows[i]["ten"].ToString();
                        string donvi = dt.Rows[i]["donvi"].ToString();
                        nganhhang = dt.Rows[i]["nganhhang"].ToString();
                        chungloai = dt.Rows[i]["chungloai"].ToString();
                        string lotNo = dt.Rows[i]["lotNo"].ToString();
                        double tonkho = double.Parse(dt.Rows[i]["soluong"].ToString());
                        double booked = double.Parse(dt.Rows[i]["booked"].ToString());
                        double avai = double.Parse(dt.Rows[i]["avai"].ToString());

                        string[] data = new string[] { dt.Rows[i]["khachhang"].ToString(), chungloai, ma, tensp, donvi, lotNo,
                            FormatString.ForMatNumber(tonkho.ToString()), FormatString.ForMatNumber(booked.ToString()), FormatString.ForMatNumber(avai.ToString()) };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            cell.VerticalAlign = VerticalAlign.Middle;

                            if (j == 3)
                                cell.Width = Unit.Parse("350");
                            else if (j == 0 || j == 1 || j == 2)
                                cell.Width = Unit.Parse("150");
                            else
                                cell.Width = Unit.Parse("100");
                            
                            if (j < 2 || j == 5 || j == 6)
                                cell.HorizontalAlign = HorizontalAlign.Center;
                            if (j >= 7)
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
        private string ExportToExcel_Inventory_Partner_Detail(HttpContext context)
        {
            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();

            string nganhhang = "";
            if (context.Request.QueryString["nganhhang"] != null)
                nganhhang = context.Request.QueryString["nganhhang"].ToString();

            string loaibaocao = "";
            if (context.Request.QueryString["loaibaocao"] != null)
                loaibaocao = context.Request.QueryString["loaibaocao"].ToString();

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
                    header.Text = "REPORT INVENTORY DETAIL ";
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

                    header.Text = "Created date: " + ngaythang;
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


                    string[] tieude = new string[] { "Partner", "Specification", "PartCode", "Description",  "Unit",
                        "Serial No", "LotNo", "Product date", "Quantity", "Booked", "Avaiable" };

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

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();

                    string condition = "  ";
                    if (chungloai.Trim().Length > 3)
                        condition += " AND sp.chungloai_fk = '" + chungloai + "' ";
                    if (nganhhang.Trim().Length > 3)
                        condition += " AND sp.nganhhang_fk = '" + nganhhang + "' ";
                    if (khachhang.Trim().Length > 3)
                        condition += " AND ct.khachhang_fk = '" + khachhang + "' ";

                    if (loaibaocao.Trim().Equals("1"))
                    {

                    }
                    else if (loaibaocao.Trim().Equals("2"))
                    {
                        condition += " AND ct.soluong = 0 ";
                    }
                    else if (loaibaocao.Trim().Equals("3"))
                    {
                        condition += " AND ct.soluong > 0 ";
                    }

                    string sql = " SELECT ISNULL((SELECT ma FROM KhachHang WHERE pk_seq = ct.khachhang_fk), '') khachhang, sp.ma, sp.nameEnglish AS ten, dv.ten as donvi, " +
                    " 	ISNULL(ct.soluong, 0) as soluong, ISNULL(ct.booked, 0) as booked, ISNULL(ct.avai, 0) as  avai, ct.solo, ct.serialNo, ct.lotNo, " +
                    "   ISNULL((SELECT ten FROM NganhHang WHERE pk_seq = sp.nganhhang_fk), '') nganhhang, " +
                    "   ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') chungloai " +
                    " FROM SanPham sp  " +
                    " 		INNER JOIN DonViTinh dv on sp.dvt_fk = dv.pk_seq " +
                    " 		INNER JOIN KhoKyGui_SanPham_ChiTiet ct on sp.pk_seq = ct.sanpham_fk " + condition +
                    " WHERE sp.trangthai = 1  " +
                    " ORDER BY sp.ma, ct.serialNo, ct.lotNo ASC ";

                    DataTable dt = xl.ReadTable(sql);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string ma = dt.Rows[i]["ma"].ToString();
                        string tensp = dt.Rows[i]["ten"].ToString();
                        string donvi = dt.Rows[i]["donvi"].ToString();
                        nganhhang = dt.Rows[i]["nganhhang"].ToString();
                        chungloai = dt.Rows[i]["chungloai"].ToString();
                        string lotNo = dt.Rows[i]["lotNo"].ToString();
                        string serialNo = dt.Rows[i]["serialNo"].ToString();
                        string solo = dt.Rows[i]["solo"].ToString();
                        double tonkho = double.Parse(dt.Rows[i]["soluong"].ToString());
                        double booked = double.Parse(dt.Rows[i]["booked"].ToString());
                        double avai = double.Parse(dt.Rows[i]["avai"].ToString());

                        string[] data = new string[] { dt.Rows[i]["khachhang"].ToString(), chungloai, ma, tensp, donvi, serialNo, lotNo, solo,
                            FormatString.ForMatNumber(tonkho.ToString()), FormatString.ForMatNumber(booked.ToString()), FormatString.ForMatNumber(avai.ToString()) };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            cell.VerticalAlign = VerticalAlign.Middle;

                            if (j == 3)
                                cell.Width = Unit.Parse("350");
                            else if (j == 0 || j == 1 || j == 2)
                                cell.Width = Unit.Parse("150");
                            else
                                cell.Width = Unit.Parse("100");
                            

                            if (j < 2 || j == 7 || j == 8)
                                cell.HorizontalAlign = HorizontalAlign.Center;
                            if (j >= 9)
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
        private string ExportToExcel_Inventory_HSD(HttpContext context)
        {
            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            string location = "";
            if (context.Request.QueryString["location"] != null)
                location = context.Request.QueryString["location"].ToString();

            string nhanhang = "";
            if (context.Request.QueryString["nhanhang"] != null)
                nhanhang = context.Request.QueryString["nhanhang"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();

            string solieu = "";
            if (context.Request.QueryString["solieu"] != null)
                solieu = context.Request.QueryString["solieu"].ToString();

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
                    header.ColumnSpan = 12;
                    header.Text = "BÁO CÁO TỒN HIỆN TẠI - HẠN SỬ DỤNG";
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

                    header.Text = "Ngày tạo: " + ngaythang;
                    header.Font.Bold = true;
                    header.ColumnSpan = 12;
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


                    string[] tieude = new string[] { "Kho", "Mã tra cứu", "Mã final", "Barcode thùng", "Sản phẩm", "Đơn vị", "Thời hạn(ngày)", "HSD", "% HSD", "Quantity", "Booked", "Hiện hữu" };

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

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();

                    string ngayhientai = DateTime.Now.ToString("dd-MM-yyyy");
                   
                    string sql = "";

                    string condition = "";
                    if (kho.Trim().Length > 3)
                        condition += " AND ksp.kho_fk = '" + kho + "' ";
                    if (location.Trim().Length > 3)
                        condition += " AND ksp.location_fk = '" + location + "' ";
                    if (nhanhang.Trim().Length > 3)
                        condition += " and sp.nhanhang_fk = '" + nhanhang + "' ";
                    if (chungloai.Trim().Length > 0)
                        condition += " and sp.chungloai_fk = '" + chungloai + "' ";

                    condition += " AND ksp.kho_fk in (" + xl.getRole_Warehouse(userId) + ")";
                    //condition += " AND ksp.location_fk in (" + xl.getRole_Location(userId) + ")";

                    if (solieu.Trim().Equals("1"))
                    {

                    }
                    else if (solieu.Trim().Equals("2"))
                    {
                        condition += " AND ksp.soluong = 0 ";
                    }
                    else if (solieu.Trim().Equals("3"))
                    {
                        condition += " AND ksp.soluong > 0 ";
                    }

                    sql = " SELECT ISNULL(k.makho, '') makho, sp.matracuu, sp.barcodeThung, sp.ma AS masp, sp.nameEnglish AS tensp, dvt.ten AS donvi, ksp.solo AS hansudung, " +
                    " 	ISNULL((SELECT ma FROM Location WHERE pk_seq = ksp.location_fk), '') location, " +
                    " 	ISNULL((SELECT ma FROM Bin WHERE pk_seq = ksp.bin_fk), '') bin, " +
                    " 	ISNULL(ksp.giaban, 0) dongia, ksp.soluong, ksp.avai, ksp.booked, " +
                    " 	(SELECT DATEDIFF(DAY, CONVERT(datetime, '" + ngayhientai + "', 105), CONVERT(datetime, ksp.solo, 105))) thoigian, " +
                    "   ISNULL(sp.datebanhang, '0') datebanhang, (SELECT DATEDIFF(DAY, CONVERT(datetime, '" + ngayhientai + "', 105), CONVERT(datetime, ksp.solo, 105))) AS ngaysudung " +
                    " FROM Kho_SanPham_ChiTiet ksp INNER JOIN SanPham sp ON ksp.sanpham_fk = sp.pk_seq " + condition +
                    " 	INNER JOIN DonViTinh dvt ON sp.dvt_fk = dvt.pk_seq " +
                    "   INNER JOIN Kho k ON ksp.kho_fk = k.pk_seq " + 
                    " WHERE sp.trangthai = 1 " +
                    " ORDER BY sp.matracuu, sp.ma, CONVERT(datetime, ksp.solo, 105) ASC ";

                    DataTable dt = xl.ReadTable(sql);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string makho = dt.Rows[i]["makho"].ToString();
                        string masp = dt.Rows[i]["masp"].ToString();
                        string matracuu = dt.Rows[i]["matracuu"].ToString();
                        string barcodeThung = dt.Rows[i]["barcodeThung"].ToString() + " .";
                        string tensp = dt.Rows[i]["tensp"].ToString();
                        string donvi = dt.Rows[i]["donvi"].ToString();
                        string datebanhang = dt.Rows[i]["datebanhang"].ToString();
                        string hsd = dt.Rows[i]["hansudung"].ToString();                        
                        double tonkho = double.Parse(dt.Rows[i]["soluong"].ToString());
                        double booked = double.Parse(dt.Rows[i]["booked"].ToString());
                        double avai = double.Parse(dt.Rows[i]["avai"].ToString());
                        
                        string pt_hsd = "0";
                        if (dt.Rows[i]["datebanhang"].ToString().Equals("0"))
                        {
                            pt_hsd = "None";
                        }
                        else
                        {
                            pt_hsd = (Math.Round(double.Parse(dt.Rows[i]["ngaysudung"].ToString()) / double.Parse(dt.Rows[i]["datebanhang"].ToString()), 4) * 100).ToString();                           
                        }
                        
                        string[] data = new string[] { makho, matracuu, masp, barcodeThung, tensp, donvi, datebanhang, hsd, pt_hsd, 
                                                        FormatString.ForMatNumber(tonkho.ToString()), FormatString.ForMatNumber(booked.ToString()), FormatString.ForMatNumber(avai.ToString()) };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            cell.VerticalAlign = VerticalAlign.Middle;

                            if (j == 0 || j == 1 || j == 5 || j == 6 || j == 7)
                                cell.Width = Unit.Parse("100");
                            else if(j == 2 || j == 3)
                                cell.Width = Unit.Parse("150");
                            else if(j == 4)
                            {
                                cell.Width = Unit.Parse("400");
                            }
                            else
                                cell.Width = Unit.Parse("80");

                            if (j > 6)
                                cell.HorizontalAlign = HorizontalAlign.Right;
                            else if(j == 0 || j == 1 || j == 5 || j == 6 || j == 7)
                                cell.HorizontalAlign = HorizontalAlign.Center;
                          
                            //if (thoigian < 0)
                            //{
                            //    //cell.BackColor = Color.Black;
                            //}
                            //else if (thoigian <= 30.0)
                            //{
                            //    cell.BackColor = Color.Red;
                            //}
                            //else if (thoigian <= 70.0)
                            //{
                            //    cell.BackColor = Color.Yellow;
                            //}
                            //else
                            //{
                            //    cell.BackColor = Color.Green;
                            //}
                            
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
        private string ExportToExcel_CANHBAOHANGTONKHO(HttpContext context)
        {
            string nhomhang = "";
            if (context.Request.QueryString["nhomhang"] != null)
                nhomhang = context.Request.QueryString["nhomhang"].ToString();

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
                    header.ColumnSpan = 7;
                    header.Text = "Warning Stock Report";
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

                    header.Text = "Created date: " + ngaythang;
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


                    string[] tieude = new string[] { "Warehouse", "Partcode", "PartName", "Unit", "Warning stock", "Avaiable", "Adjustment" };

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

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();

                    string condition = "  ";
                    if (nhomhang.Trim().Length > 0)
                        condition += " AND a.nhomsanpham_fk = '" + nhomhang + "' ";
                    if (kho.Trim().Length > 3)
                        condition += " AND c.kho_fk = '" + kho + "' ";

                    string sql = " SELECT ISNULL((SELECT makho FROM Kho WHERE pk_seq = c.kho_fk), '') kho, a.pk_seq, a.ma, a.ten, a.trangthai, ISNULL(a.dinhmucton, 0)  as dinhmucton, SUM(c.avai) AS tonkho, ISNULL(d.ten, '') as donvi " +
                                 " FROM SanPham a INNER JOIN Kho_SanPham_ChiTiet c on a.pk_seq = c.sanpham_fk " + condition + 
                                 "  LEFT JOIN DonViTinh d on a.dvt_fk = d.pk_seq " +
                                 " WHERE a.pk_seq > 0  and a.trangthai = '1' " + condition +
                                 " GROUP BY c.kho_fk, a.pk_seq, a.ma, a.ten, a.trangthai, a.dinhmucton, d.ten " + 
                                 " ORDER BY a.ma ASC ";

                    DataTable dt = xl.ReadTable(sql);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        double tonkho = double.Parse(dt.Rows[i]["tonkho"].ToString());
                        double dinhmucton = double.Parse(dt.Rows[i]["dinhmucton"].ToString());
                        double chenhlech = tonkho - dinhmucton;


                        string[] data = new string[] { dt.Rows[i]["kho"].ToString(), dt.Rows[i]["ma"].ToString(), dt.Rows[i]["ten"].ToString(), dt.Rows[i]["donvi"].ToString(), 
                                                        FormatString.ForMatNumber(dinhmucton.ToString()), FormatString.ForMatNumber(tonkho.ToString()), FormatString.ForMatNumber(chenhlech.ToString())};

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            cell.VerticalAlign = VerticalAlign.Middle;

                            if(j == 1)
                                cell.Width = Unit.Parse("180");
                            else if (j == 2)
                                cell.Width = Unit.Parse("450");
                            else
                                cell.Width = Unit.Parse("100");
                            
                            if (j >= 4)
                                cell.HorizontalAlign = HorizontalAlign.Right;

                            if(chenhlech < 0)
                            {
                                cell.BackColor = Color.Red;
                            }

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
        private string BaoCaoThongTinSanPham(HttpContext context)
        {

            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();
            
            string sanpham = "";
            if (context.Request.QueryString["sanpham"] != null)
                sanpham = context.Request.QueryString["sanpham"].ToString();

            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            string xuatxu = "";
            if (context.Request.QueryString["xuatxu"] != null)
                xuatxu = context.Request.QueryString["xuatxu"].ToString();

            string location = "";
            if (context.Request.QueryString["location"] != null)
                location = context.Request.QueryString["location"].ToString();

            string bin = "";
            if (context.Request.QueryString["bin"] != null)
                bin = context.Request.QueryString["bin"].ToString();


            string ngaythang = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();

            string _exportContent = "";
            using (StringWriter sb = new StringWriter())
            {
                using (HtmlTextWriter htmlWriter = new HtmlTextWriter(sb))
                {
                    Table table = new Table();
                    table.GridLines = GridLines.Both;

                    ExecuteData xl = new ExecuteData();

                    //table.BorderWidth = new Unit(1);

                    //Tieu de
                    TableHeaderCell header = new TableHeaderCell();
                    header.RowSpan = 1;
                    header.ColumnSpan = 10;
                    header.Text = "BÁO CÁO THEO DÕI SẢN PHẨM";
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
                    header.ColumnSpan = 10;
                    header.Text = "Ngày: " + ngaythang;
                    header.Font.Bold = true;
                    //header.BackColor = Color.LightGray;
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);

                    table.Rows.Add(headerRow);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 10;
                    header.Text = "Thời gian từ : " + tungay + " đến ngày " + denngay;
                    header.Font.Bold = true;
                    //header.BackColor = Color.LightGray;
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 10;
                    header.Text = "Kho : " + xl.ExecuteScalarSQL("SELECT makho FROM Kho WHERE pk_seq = '" + kho + "' UNION ALL SELECT ma FROM NhomKhachHang WHERE pk_seq = '" + kho + "' ").ToString();
                    header.Font.Bold = true;
                    //header.BackColor = Color.LightGray;
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 10;
                    header.Text = "Sản phẩm: " + xl.ExecuteScalarSQL("SELECT ten FROM SanPham WHERE pk_seq = '" + sanpham + "'").ToString();
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

                    //LAY THONG TIN SAN PHAM
                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 10;
                    header.Text = "THÔNG TIN NHẬP HÀNG";
                    header.Font.Bold = true;
                    header.BackColor = Color.LightGreen;
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    //END THONG TIN SP

                    string condition = "";
                    string conditionNHAP = "";
                    string conditionDLB = "";

                    if (xuatxu.Trim().Length > 0)
                    {
                        condition += " and xx.pk_seq = '" + xuatxu + "' ";
                    }
                    if (location.Trim().Length > 0)
                    {
                        condition += " and loc.pk_seq = '" + location + "' ";
                    }
                    if (bin.Trim().Length > 0)
                    {
                        condition += " and bin.pk_seq = '" + bin + "' ";
                    }


                    // Thong tin nhap SP
                    string[] tieude = new string[] { "STT", "Nghiệp vụ", "Số nhập", "Ngày nhập", "Ngày chốt", "Trạng thái", "Mã đối tượng", "Tên đối tượng", "HSD", "Quantity" };
                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;
                        header.BackColor = Color.LightBlue;
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);


                    string sql = "  SELECT NHAP.nghiepvu, NHAP.sonhaphang, NHAP.ngaynghiepvu, NHAP.ngaychot, NHAP.manhacungcap, NHAP.nhacungcap, " +
                                 "  	 ISNULL(xx.ten, '') as xuatxu, ISNULL(loc.ten, '') as location, ISNULL(bin.ten, '') as bin, " +
                                 "  	 NHAP.solo, NHAP.soluong, NHAP.donvitinh, NHAP.soluongQUYDOI, NHAP.donviquydoi, NHAP.gianhap " +
                                 "  	 FROM (  " +
                                 "     SELECT N'Nhập hàng' as nghiepvu, nh.pk_seq sonhaphang, nh.ngaynhap as ngaynghiepvu, CONVERT(nvarchar(10), nh.ngaysua, 110) AS ngaychot, nh.kho_fk, ISNULL(ncc.ma, '') manhacungcap, ncc.ten as nhacungcap ,  " +
                                 "  		nh_sp_ct.xuatxu_fk, nh_sp_ct.location_fk, nh_sp_ct.bin_fk,      " +
                                 "      	nh_sp_ct.solo, nh_sp_ct.soluong , dvt.ten as donvitinh, nh_sp_ct.soluongQUYDOI,  " +
                                 "      	ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk), '') donviquydoi, ISNULL(nh_sp.gianhap, 0) gianhap  " +
                                 "      FROM NhapHang nh INNER JOIN NhapHang_SanPham nh_sp ON nh.pk_seq = nh_sp.nhaphang_fk  " +
                                 "      	INNER JOIN NhapHang_SanPham_ChiTiet nh_sp_ct ON nh_sp.nhaphang_fk = nh_sp_ct.nhaphang_fk AND nh_sp.sanpham_fk = nh_sp_ct.sanpham_fk  " +
                                 "      	INNER JOIN NhaCungCap ncc ON nh.ncc_fk = ncc.pk_seq  " +
                                 "      	INNER JOIN SanPham sp ON nh_sp_ct.sanpham_fk = sp.pk_seq  " +
                                 "      	INNER JOIN DonViTinh dvt ON nh_sp.DVT_FK = dvt.pk_seq  " +
                                 "     WHERE nh.pk_seq > 0  AND nh.trangthai = 1  and sp.pk_seq = '" + sanpham + "' " +
                                 "         and CONVERT(datetime, nh.ngaynhap, 105) >= CONVERT(datetime, '" + tungay + "', 105)   " +
                                 "         and CONVERT(datetime, nh.ngaynhap, 105) <= CONVERT(datetime, '" + denngay + "', 105)   " +                                 
                                 "    UNION ALL  " +
                                 "      SELECT N'Nhập khác' as nghiepvu, nh.pk_seq sonhaphang, nh.ngaynhap as ngaynghiepvu, CONVERT(nvarchar(10), nh.ngaysua, 110) AS ngaychot, nh.kho_fk as kho_fk, ISNULL(nh.madoituong, '') manhacungcap, ISNULL(nh.tendoituong, '') nhacungcap,   " +
                                 "      	nh_sp_ct.xuatxu_fk, nh_sp_ct.location_fk, nh_sp_ct.bin_fk, " +
                                 "      	nh_sp_ct.solo, nh_sp_ct.soluong , dvt.ten as donvitinh, nh_sp_ct.soluongQUYDOI,  " +
                                 "      	ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk), '') donviquydoi, ISNULL(nh_sp.dongia, 0) gianhap   " +
                                 "      FROM NhapKhac nh INNER JOIN NhapKhac_SanPham nh_sp ON nh.pk_seq = nh_sp.nhapkhac_fk AND nh.loainhap in (1, 2)  " +
                                 "      	INNER JOIN NhapKhac_SanPham_ChiTiet nh_sp_ct ON nh_sp.nhapkhac_fk = nh_sp_ct.nhapkhac_fk AND nh_sp.sanpham_fk = nh_sp_ct.sanpham_fk	  " +
                                 "      	INNER JOIN SanPham sp ON nh_sp_ct.sanpham_fk = sp.pk_seq  " +
                                 "      	INNER JOIN DonViTinh dvt ON nh_sp.DVT_FK = dvt.pk_seq  " +
                                 "      WHERE nh.pk_seq > 0  AND nh.trangthai = 1 and sp.pk_seq = '" + sanpham + "' " +
                                 "         and CONVERT(datetime, nh.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)   " +
                                 "         and CONVERT(datetime, nh.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105)   " +
                                 "    UNION ALL  " +
                                 "      SELECT N'Nhập khác - ký gửi' as nghiepvu, nh.pk_seq sonhaphang, nh.ngaynhap as ngaynghiepvu, CONVERT(nvarchar(10), nh.ngaysua, 110) AS ngaychot, nh.kho_fk as kho_fk, ISNULL(nh.madoituong, '') manhacungcap, ISNULL(nh.tendoituong, '') nhacungcap,   " +
                                 "      	nh_sp_ct.xuatxu_fk, nh_sp_ct.location_fk, nh_sp_ct.bin_fk, " +
                                 "      	nh_sp_ct.solo, nh_sp_ct.soluong , dvt.ten as donvitinh, nh_sp_ct.soluongQUYDOI,  " +
                                 "      	ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk), '') donviquydoi, ISNULL(nh_sp.dongia, 0) gianhap   " +
                                 "      FROM NhapKhac nh INNER JOIN NhapKhac_SanPham nh_sp ON nh.pk_seq = nh_sp.nhapkhac_fk AND nh.loainhap in (3)  " +
                                 "      	INNER JOIN NhapKhac_SanPham_ChiTiet nh_sp_ct ON nh_sp.nhapkhac_fk = nh_sp_ct.nhapkhac_fk AND nh_sp.sanpham_fk = nh_sp_ct.sanpham_fk	  " +
                                 "      	INNER JOIN SanPham sp ON nh_sp_ct.sanpham_fk = sp.pk_seq  " +
                                 "      	INNER JOIN DonViTinh dvt ON nh_sp.DVT_FK = dvt.pk_seq  " +
                                 "      WHERE nh.pk_seq > 0  AND nh.trangthai = 1 and sp.pk_seq = '" + sanpham + "' " +
                                 "         and CONVERT(datetime, nh.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)   " +
                                 "         and CONVERT(datetime, nh.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105)   " +
                                 "    UNION ALL  " +
                                 "      SELECT N'Nhập sản xuất' as nghiepvu, nh.pk_seq sonhaphang, nh.ngaynhap as ngaynghiepvu, CONVERT(nvarchar(10), nh.ngaysua, 110) AS ngaychot, nh.kho_fk, ISNULL(nh.madoituong, '') manhacungcap, ISNULL(nh.tendoituong, '') nhacungcap,   " +
                                 "      	nh_sp_ct.xuatxu_fk, nh_sp_ct.location_fk, nh_sp_ct.bin_fk,  " +
                                 "      	nh_sp_ct.solo, nh_sp_ct.soluong , dvt.ten as donvitinh, nh_sp_ct.soluongQUYDOI,  " +
                                 "      	ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk), '') donviquydoi, ISNULL(nh_sp.dongia, 0) gianhap   " +
                                 "      FROM NhapKhac nh INNER JOIN NhapKhac_SanPham nh_sp ON nh.pk_seq = nh_sp.nhapkhac_fk AND nh.loainhap in (4)  " +
                                 "      	INNER JOIN NhapKhac_SanPham_ChiTiet nh_sp_ct ON nh_sp.nhapkhac_fk = nh_sp_ct.nhapkhac_fk AND nh_sp.sanpham_fk = nh_sp_ct.sanpham_fk	   " +
                                 "      	INNER JOIN SanPham sp ON nh_sp_ct.sanpham_fk = sp.pk_seq  " +
                                 "      	INNER JOIN DonViTinh dvt ON nh_sp.DVT_FK = dvt.pk_seq  " +
                                 "      WHERE nh.pk_seq > 0  AND nh.trangthai = 1 and sp.pk_seq = '" + sanpham + "' " +
                                 "         and CONVERT(datetime, nh.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)   " +
                                 "         and CONVERT(datetime, nh.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                                 "    UNION ALL  " +
                                 "     SELECT N'Đổi lô bin' as nghiepvu, dh.pk_seq as sonhaphang, dh.ngaynhap as ngaynghiepvu, CONVERT(nvarchar(10), dh.ngaysua, 110) AS ngaychot, kho_fk as kho_fk ,  '' as manhacungcap, '' as nhacungcap,  " +
                                 "       	dh_sp_ct.xuatxu_new_fk, dh_sp_ct.location_new_fk, dh_sp_ct.bin_new_fk,  " +
                                 "       	dh_sp_ct.solo_new as solo,	dh_sp_ct.soluong, dvt.ten as donvi, dh_sp_ct.soluongQuyDoi, dvt2.ten as donviCHUAN, 0 as gianhap   " +
                                 "       FROM DoiLoBin dh INNER JOIN DoiLoBin_SanPham dh_sp ON dh.pk_seq = dh_sp.doilobin_fk     " +
                                 "          	INNER JOIN DoiLoBin_SanPham_ChiTiet dh_sp_ct ON dh_sp.doilobin_fk = dh_sp_ct.doilobin_fk AND dh_sp.sanpham_fk = dh_sp_ct.sanpham_fk     " +
                                 "          	INNER JOIN SanPham sp ON dh_sp_ct.sanpham_fk = sp.pk_seq     " +
                                 "          	LEFT JOIN DonViTinh dvt ON dh_sp.dvt_fk = dvt.pk_seq     " +
                                 "          	LEFT JOIN DonViTinh dvt2 ON sp.dvt_fk = dvt2.pk_seq       " +
                                 "      WHERE dh.pk_seq > 0  AND dh.trangthai = 1 and sp.pk_seq = '" + sanpham + "' " +
                                 "         and CONVERT(datetime, dh.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)   " +
                                 "         and CONVERT(datetime, dh.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                                 "      UNION ALL " +
                                 "         SELECT N'Điều chỉnh tồn kho' as nghiepvu, dc.pk_seq sonhaphang, dc.ngaydieuchinh as ngaynghiepvu, CONVERT(nvarchar(10), dc.ngaysua, 110) AS ngaychot, dc.kho_fk, '' manhacungcap, '' nhacungcap,   " +
                                 "      	dc_sp.xuatxu_fk, dc_sp.location_fk, dc_sp.bin_fk,  " +
                                 "      	dc_sp.solo, dc_sp.dieuchinh , dvt.ten as donvitinh, dc_sp.dieuchinh as soluongQuyDoi,  " +
                                 "      	ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk), '') donviquydoi, 0 gianhap   " +
                                 "      FROM DieuChinhTonKho dc INNER JOIN DieuChinhTonKho_SanPham dc_sp ON dc.pk_seq = dc_sp.dieuchinh_fk    	 " +
                                 "      	INNER JOIN SanPham sp ON dc_sp.sanpham_fk = sp.pk_seq  " +
                                 "      	INNER JOIN DonViTinh dvt ON sp.dvt_fk = dvt.pk_seq  " +
                                 "      WHERE dc.pk_seq > 0  AND dc.trangthai = 1 and sp.pk_seq = '" + sanpham + "' " +
                                 "         and CONVERT(datetime, dc.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)   " +
                                 "         and CONVERT(datetime, dc.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                                 //"  	 UNION ALL " +
                                 //"         SELECT N'Kiểm kho' as nghiepvu, kk.pk_seq sonhaphang, kk.ngaykiem as ngaynghiepvu, CONVERT(nvarchar(10), kk.ngaysua, 110) AS ngaychot, kk.kho_fk, '' manhacungcap, '' nhacungcap,   " +
                                 //"      	kk_sp.xuatxu_fk, kk_sp.location_fk, kk_sp.bin_fk,  " +
                                 //"      	kk_sp.SOLO, kk_sp.chenhlech as soluong , dvt.ten as donvitinh, kk_sp.chenhlech as soluongQuyDoi,  " +
                                 //"      	ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk), '') donviquydoi, 0 gianhap   " +
                                 //"      FROM KiemKho kk INNER JOIN KiemKho_SanPham kk_sp ON kk.pk_seq = kk_sp.kiemkho_fk    	 " +
                                 //"      	INNER JOIN SanPham sp ON kk_sp.sanpham_fk = sp.pk_seq  " +
                                 //"      	INNER JOIN DonViTinh dvt ON sp.dvt_fk = dvt.pk_seq  " +
                                 //"      WHERE kk.pk_seq > 0  AND kk.trangthai = 1 and sp.pk_seq = '" + sanpham + "' " +
                                 //"         and CONVERT(datetime, kk.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)   " +
                                 //"         and CONVERT(datetime, kk.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                                 "  	UNION ALL " +
                                 "         SELECT N'Khách hàng trả hàng' as nghiepvu, dth.pk_seq sonhaphang, dth.ngaydonhang as ngaynghiepvu, CONVERT(nvarchar(10), dth.ngaysua, 110) AS ngaychot, dth.kho_fk, '' manhacungcap, '' nhacungcap,   " +
                                 "      	dth_sp_ct.xuatxu_fk, dth_sp_ct.location_fk, dth_sp_ct.bin_fk,  " +
                                 "      	dth_sp_ct.SOLO, dth_sp_ct.soluong as soluong , dvt.ten as donvitinh, dth_sp_ct.soluongQuyDoi as soluongQuyDoi,  " +
                                 "      	ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk), '') donviquydoi, 0 gianhap   " +
                                 "      FROM DonTraHang dth INNER JOIN DonTraHang_SanPham dth_sp ON dth.pk_seq = dth_sp.dontrahang_fk   " +
                                 "  		INNER JOIN DonTraHang_SanPham_ChiTiet dth_sp_ct ON dth_sp.dontrahang_fk = dth_sp_ct.dontrahang_fk AND dth_sp.sanpham_fk = dth_sp_ct.sanpham_fk " +
                                 "      	INNER JOIN SanPham sp ON dth_sp.sanpham_fk = sp.pk_seq  " +
                                 "      	INNER JOIN DonViTinh dvt ON dth_sp.dvt_fk = dvt.pk_seq  " +
                                 "      WHERE dth.pk_seq > 0  AND dth.trangthai = 1 and sp.pk_seq = '" + sanpham + "' " +
                                 "         and CONVERT(datetime, dth.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)   " +
                                 "         and CONVERT(datetime, dth.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                                 "     ) NHAP     " +
                                 "  	LEFT JOIN XuatXu xx ON NHAP.xuatxu_fk = xx.pk_seq " +
                                 "  	LEFT JOIN Location loc ON NHAP.location_fk = loc.pk_seq " +
                                 "  	LEFT JOIN Bin bin ON NHAP.bin_fk = bin.pk_seq   " +
                                 "  WHERE NHAP.kho_fk = '" + kho + "' " + condition +
                                 "  ORDER BY NHAP.nghiepvu, CONVERT(datetime, NHAP.ngaynghiepvu, 105) asc, xx.ten, loc.ten, bin.ten ";

                    DataTable dt = xl.ReadTable(sql);

                    double totalSOLUONGNHAP = 0;
                    double totalSOLUONGNHAP_QuyDoi = 0;
                    double totalTIENNHAP = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string nghiepvu = dt.Rows[i]["nghiepvu"].ToString();
                        string sonhaphang = dt.Rows[i]["sonhaphang"].ToString();
                        string ngaynghiepvu = dt.Rows[i]["ngaynghiepvu"].ToString();
                        string ngaychot = dt.Rows[i]["ngaychot"].ToString();
                        string trangthai = "Đã nhập";
                        string manhacungcap = dt.Rows[i]["manhacungcap"].ToString();
                        string nhacungcap = dt.Rows[i]["nhacungcap"].ToString();
                        string tenkho = "";
                        string xuatxuNHAP = dt.Rows[i]["xuatxu"].ToString();
                        string locationNHAP = dt.Rows[i]["location"].ToString();
                        string binNHAP = dt.Rows[i]["bin"].ToString();
                        string soloNHAP = dt.Rows[i]["solo"].ToString();                        
                        string soluong = dt.Rows[i]["soluong"].ToString();
                        string donvitinh = dt.Rows[i]["donvitinh"].ToString();
                        string gianhap = dt.Rows[i]["gianhap"].ToString();
                        double thanhtien = double.Parse(soluong) * double.Parse(gianhap);
                        string soluongQUYDOI = dt.Rows[i]["soluongQUYDOI"].ToString();
                        string donviquydoi = dt.Rows[i]["donviquydoi"].ToString();

                        totalSOLUONGNHAP += double.Parse(soluong);
                        totalSOLUONGNHAP_QuyDoi += double.Parse(soluongQUYDOI);
                        totalTIENNHAP += thanhtien;

                        string[] data = new string[] { (i + 1).ToString(), nghiepvu, sonhaphang, ngaynghiepvu, ngaychot, trangthai, manhacungcap, nhacungcap,
                            soloNHAP, FormatString.ForMatNumber(soluong)};

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();

                            if (j == 0)
                                cell.Width = Unit.Parse("50");
                            else if (j == 7)
                                cell.Width = Unit.Parse("450");
                            else if (j == 8 || j == 9 || j == 10)
                                cell.Width = Unit.Parse("120");
                            else if (j == 1 || j == 6)
                                cell.Width = Unit.Parse("150");
                            else
                                cell.Width = Unit.Parse("80");

                            if (j < 5 || j == 8 || j == 9 || j == 10 || j == 11 || j == 14 || j == 18)
                                cell.HorizontalAlign = HorizontalAlign.Center;
                            if (j == 13 || j == 15 || j == 16 || j == 17)
                                cell.HorizontalAlign = HorizontalAlign.Right;

                            cell.Text = data[j];
                            row.Cells.Add(cell);
                        }

                        table.Rows.Add(row);
                    }

                    if (dt.Rows.Count >= 0)
                    {
                        string[] data = new string[] { "Total", FormatString.ForMatNumber(totalSOLUONGNHAP.ToString())};

                        TableRow row = new TableRow();
                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.Font.Bold = true;
                            cell.HorizontalAlign = HorizontalAlign.Right;
                            cell.BackColor = Color.LightGray;

                            if (j == 0)
                            {
                                cell.HorizontalAlign = HorizontalAlign.Center;
                                cell.ColumnSpan = 9;
                            }

                            cell.Text = data[j];

                            //cell.Attributes.Add("style", @"mso-number-format:\@;");

                            row.Cells.Add(cell);
                        }

                        table.Rows.Add(row);
                    }

                    //LAY THONG TIN SAN PHAM
                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 10;
                    header.Text = "THÔNG TIN XUẤT HÀNG";
                    header.Font.Bold = true;
                    header.BackColor = Color.LightGreen;
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    // Thong tin BAN SP
                    tieude = new string[] { "STT", "Nghiệp vụ", "Số xuất", "Ngày xuất", "Ngày chốt","Trạng thái", "Mã đối tượng", "Tên đối tượng",
                        "HSD", "Quantity" };
                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;
                        header.BackColor = Color.LightBlue;
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    sql = " SELECT XUAT.nghiepvu, XUAT.sodonhang, XUAT.ngaydonhang, XUAT.ngaychot, XUAT.kho_fk, XUAT.madoituong, XUAT.tendoituong,  " +
                             "  	xx.ten as xuatxu, loc.ten as location, bin.ten as bin,  " +
                             "  	XUAT.solo, XUAT.soluong, XUAT.donvi, XUAT.soluongQUYDOI,  XUAT.donviCHUAN, XUAT.dongia, XUAT.trangthai " +
                             "  FROM (  " +
                             "  	SELECT N'Bán hàng' as nghiepvu, dh.pk_seq as sodonhang, dh.ngaydonhang, CONVERT(nvarchar(10), dh.thoigianCHOT, 105) AS ngaychot, dh.kho_fk,   " +
                             "   	dh_sp_ct.soluong, dvt.ten as donvi, dh_sp_ct.soluongQuyDoi, dvt2.ten as donviCHUAN,  " +
                             "   	dh_sp_ct.xuatxu_fk, dh_sp_ct.location_fk, dh_sp_ct.bin_fk, dh_sp_ct.solo, dh_sp.dongia,  " +
                             "  		CASE WHEN dh.trangthai = 0 THEN N'Chưa xuất kho'            		 " +
                             "   			WHEN dh.trangthai in (1, 3, 4, 5, 6) THEN N'Đã xuất kho'   " +
                             "   			ELSE N'Trạng thái khác' END trangthai, kh.ma as madoituong, kh.hoten as tendoituong  " +
                             "   FROM DonHang dh INNER JOIN DonHang_SanPham dh_sp ON dh.pk_seq = dh_sp.donhang_fk    " +
                             "      	INNER JOIN DonHang_SanPham_ChiTiet dh_sp_ct ON dh_sp.donhang_fk = dh_sp_ct.donhang_fk AND dh_sp.sanpham_fk = dh_sp_ct.sanpham_fk    " +
                             "      	INNER JOIN SanPham sp ON dh_sp_ct.sanpham_fk = sp.pk_seq    " +
                             "      	LEFT JOIN DonViTinh dvt ON dh_sp.dvt_fk = dvt.pk_seq    " +
                             "      	LEFT JOIN DonViTinh dvt2 ON sp.dvt_fk = dvt2.pk_seq               	 " +
                             "      	INNER JOIN KhachHang kh ON dh.khachhang_fk = kh.pk_seq    " +
                             "   WHERE dh.pk_seq > 0 AND dh.trangthai not in (0, 2) and sp.pk_seq = '" + sanpham + "' " +
                             "            and CONVERT(datetime, dh.thoigianCHOT, 105) >= CONVERT(datetime, '" + tungay + "', 105)  " +
                             "            and CONVERT(datetime, dh.thoigianCHOT, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                             "  UNION ALL   " +
                             "  	SELECT N'Đổi lô bin' as nghiepvu, dh.pk_seq as sodonhang, dh.ngaynhap as ngaydonhang, CONVERT(nvarchar(10), dh.ngaysua, 105) AS ngaychot, ISNULL(khonhan_fk, 0) kho_fk,     " +
                             "     		dh_sp_ct.soluong, dvt.ten as donvi, dh_sp_ct.soluongQuyDoi, dvt2.ten as donviCHUAN,    " +
                             "  		dh_sp_ct.xuatxu_fk, dh_sp_ct.location_fk, dh_sp_ct.bin_fk, dh_sp_ct.solo, dh_sp.dongia,    " +
                             "   		N'Đã xuất kho' as trangthai, '' as madoituong, '' as tendoituong    " +
                             "     FROM DoiLoBin dh INNER JOIN DoiLoBin_SanPham dh_sp ON dh.pk_seq = dh_sp.doilobin_fk      " +
                             "        	INNER JOIN DoiLoBin_SanPham_ChiTiet dh_sp_ct ON dh_sp.doilobin_fk = dh_sp_ct.doilobin_fk AND dh_sp.sanpham_fk = dh_sp_ct.sanpham_fk      " +
                             "        	INNER JOIN SanPham sp ON dh_sp_ct.sanpham_fk = sp.pk_seq      " +
                             "        	LEFT JOIN DonViTinh dvt ON dh_sp.dvt_fk = dvt.pk_seq      " +
                             "        	LEFT JOIN DonViTinh dvt2 ON sp.dvt_fk = dvt2.pk_seq                   	  " +
                             "     WHERE dh.pk_seq > 0 AND dh.trangthai in (1) and sp.pk_seq = '" + sanpham + "' " +
                             "            and CONVERT(datetime, dh.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)  " +
                             "            and CONVERT(datetime, dh.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                             "  UNION ALL  " +
                             "  	SELECT N'Xuất khác' as nghiepvu, dh.pk_seq as sodonhang, dh.ngayxuat as ngaydonhang, CONVERT(nvarchar(10), dh.ngaysua, 105) AS ngaychot, dh.kho_fk,    " +
                             "     	dh_sp_ct.soluong, dvt.ten as donvi, dh_sp_ct.soluongQuyDoi, dvt2.ten as donviCHUAN,   " +
                             "      dh_sp_ct.xuatxu_fk, dh_sp_ct.location_fk,dh_sp_ct.bin_fk, dh_sp_ct.solo, dh_sp.dongia,   " +
                             "   	N'Đã xuất kho' as trangthai, '' as madoituong, '' as tendoituong   " +
                             "  	FROM XuatKhac dh INNER JOIN XuatKhac_SanPham dh_sp ON dh.pk_seq = dh_sp.xuatkhac_fk     " +
                             "        	INNER JOIN XuatKhac_SanPham_ChiTiet dh_sp_ct ON dh_sp.xuatkhac_fk = dh_sp_ct.xuatkhac_fk AND dh_sp.sanpham_fk = dh_sp_ct.sanpham_fk     " +
                             "        	INNER JOIN SanPham sp ON dh_sp_ct.sanpham_fk = sp.pk_seq     " +
                             "        	LEFT JOIN DonViTinh dvt ON dh_sp.dvt_fk = dvt.pk_seq     " +
                             "        	LEFT JOIN DonViTinh dvt2 ON sp.dvt_fk = dvt2.pk_seq                     " +
                             "     WHERE dh.pk_seq > 0 AND dh.trangthai in (1) and sp.pk_seq = '" + sanpham + "' " +
                             "            and CONVERT(datetime, dh.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)  " +
                             "            and CONVERT(datetime, dh.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                             "  UNION ALL " +
                             "  	SELECT N'Trả hàng NCC' as nghiepvu, dh.pk_seq as sodonhang, dh.ngaytra as ngaydonhang, CONVERT(nvarchar(10), dh.ngaysua, 105) AS ngaychot, ISNULL(kho_fk, 0) kho_fk,    " +
                             "     	dh_sp_ct.soluong, dvt.ten as donvi, dh_sp_ct.soluongQuyDoi, dvt2.ten as donviCHUAN,   " +
                             "      dh_sp_ct.xuatxu_fk, dh_sp_ct.location_fk,dh_sp_ct.bin_fk, dh_sp_ct.solo, dh_sp.giatra,   " +
                             "   	N'Đã xuất kho' as trangthai, '' as madoituong, '' as tendoituong   " +
                             "  	FROM TraHang dh INNER JOIN TraHang_SanPham dh_sp ON dh.pk_seq = dh_sp.trahang_fk     " +
                             "        	INNER JOIN TraHang_SanPham_ChiTiet dh_sp_ct ON dh_sp.trahang_fk = dh_sp_ct.trahang_fk AND dh_sp.sanpham_fk = dh_sp_ct.sanpham_fk     " +
                             "        	INNER JOIN SanPham sp ON dh_sp_ct.sanpham_fk = sp.pk_seq     " +
                             "        	LEFT JOIN DonViTinh dvt ON dh_sp.dvt_fk = dvt.pk_seq     " +
                             "        	LEFT JOIN DonViTinh dvt2 ON sp.dvt_fk = dvt2.pk_seq                     " +
                             "     WHERE dh.pk_seq > 0 AND dh.trangthai in (1) and sp.pk_seq = '" + sanpham + "' " +
                             "            and CONVERT(datetime, dh.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)  " +
                             "            and CONVERT(datetime, dh.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                             "  UNION ALL " +
                             "  	SELECT N'Tieu hao' as nghiepvu, dh.pk_seq as sodonhang, dh_sp.ngaytieuhao as ngaydonhang, CONVERT(nvarchar(10), dh.ngaysua, 105) AS ngaychot, ISNULL(dh_sp.kho_fk, 0) kho_fk,    " +
                             "     	dh_sp_ct.soluong, dvt.ten as donvi, dh_sp_ct.soluongQuyDoi, dvt.ten as donviCHUAN,   " +
                             "      dh_sp_ct.xuatxu_fk, dh_sp_ct.location_fk,dh_sp_ct.bin_fk, dh_sp_ct.solo, 0 as dongia,   " +
                             "   	N'Đã xuất kho' as trangthai, '' as madoituong, '' as tendoituong   " +
                             "  	FROM LenhSanXuat dh INNER JOIN LenhSanXuat_TieuHao dh_sp ON dh.pk_seq = dh_sp.lenhsanxuat_fk     " +
                             "        	INNER JOIN LenhSanXuat_TieuHao_ChiTiet dh_sp_ct ON dh_sp.lenhsanxuat_fk = dh_sp_ct.lenhsanxuat_fk  " +
                             "        	INNER JOIN SanPham sp ON dh_sp_ct.vattu_fk = sp.pk_seq     " +
                             "        	LEFT JOIN DonViTinh dvt ON sp.pk_seq = dvt.pk_seq                   " +
                             "     WHERE dh.pk_seq > 0 AND dh.trangthai in (1) and sp.pk_seq = '" + sanpham + "' " +
                             "            and CONVERT(datetime, dh_sp.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)  " +
                             "            and CONVERT(datetime, dh_sp.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                             "   ) XUAT  " +
                             "  	INNER JOIN XuatXu xx ON XUAT.xuatxu_fk = xx.pk_seq " +
                             "  	INNER JOIN Location loc ON XUAT.location_fk = loc.pk_seq " +
                             "  	INNER JOIN Bin bin ON XUAT.bin_fk = bin.pk_seq " +
                             "  WHERE XUAT.kho_fk = '" + kho + "' " + condition +
                             "  ORDER BY XUAT.nghiepvu, CONVERT(datetime,XUAT.ngaydonhang, 105), xx.ten, loc.ten, bin.ten ASC ";

                    dt = xl.ReadTable(sql);

                    double totalSOLUONG = 0;
                    double totalSOLUONG_QuyDoi = 0;
                    double totalTIEN = 0;


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        double soluong = double.Parse(dt.Rows[i]["soluong"].ToString());
                        double soluongQUYDOI = double.Parse(dt.Rows[i]["soluongQuyDoi"].ToString());

                        double dongia = double.Parse(dt.Rows[i]["dongia"].ToString());
                        double thanhtien = soluong * dongia;


                        totalSOLUONG += soluong;
                        totalSOLUONG_QuyDoi += soluongQUYDOI;
                        totalTIEN += thanhtien;

                        string sodonhang = dt.Rows[i]["sodonhang"].ToString();
                        string ngaydonhang = dt.Rows[i]["ngaydonhang"].ToString();
                        string ngaychot = dt.Rows[i]["ngaychot"].ToString();
                        string trangthaiDH = dt.Rows[i]["trangthai"].ToString();
                        string madoituong = dt.Rows[i]["madoituong"].ToString();
                        string tendoituong = dt.Rows[i]["tendoituong"].ToString();
                        string makho = "";
                        string xuatxuDH = dt.Rows[i]["xuatxu"].ToString();
                        string locationDH = dt.Rows[i]["location"].ToString();
                        string binDH = dt.Rows[i]["bin"].ToString();

                        string[] data = new string[] { (i + 1).ToString(), dt.Rows[i]["nghiepvu"].ToString(), sodonhang, ngaydonhang, ngaychot, trangthaiDH, madoituong, tendoituong,
                                                    dt.Rows[i]["solo"].ToString(), FormatString.ForMatNumber(soluong.ToString()) };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();

                            if (j == 0)
                                cell.Width = Unit.Parse("50");
                            else if (j == 7)
                                cell.Width = Unit.Parse("450");
                            else if (j == 8 || j == 9 || j == 10)
                                cell.Width = Unit.Parse("120");
                            else if (j == 1 || j == 6)
                                cell.Width = Unit.Parse("150");
                            else
                                cell.Width = Unit.Parse("80");

                            if (j < 5 || j == 8 || j == 9 || j == 10 || j == 11 || j == 14 || j == 18)
                                cell.HorizontalAlign = HorizontalAlign.Center;
                            if (j == 13 || j == 15 || j == 16 || j == 17)
                                cell.HorizontalAlign = HorizontalAlign.Right;

                            //if (i % 2 == 0)
                            //    cell.BackColor = Color.LightSkyBlue;  

                            cell.Text = data[j];
                            row.Cells.Add(cell);
                        }

                        table.Rows.Add(row);
                    }

                    if (dt.Rows.Count >= 0)
                    {
                        string[] data = new string[] { "Total", FormatString.ForMatNumber(totalSOLUONG.ToString()) };

                        TableRow row = new TableRow();
                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.Font.Bold = true;
                            cell.HorizontalAlign = HorizontalAlign.Right;
                            cell.BackColor = Color.LightGray;

                            if (j == 0)
                            {
                                cell.HorizontalAlign = HorizontalAlign.Center;
                                cell.ColumnSpan = 9;
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
        private string BaoCaoThongTinSanPham_HSD(HttpContext context)
        {

            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string ngayhethan = "";
            if (context.Request.QueryString["ngayhethan"] != null)
                ngayhethan = context.Request.QueryString["ngayhethan"].ToString();

            string sanpham = "";
            if (context.Request.QueryString["sanpham"] != null)
                sanpham = context.Request.QueryString["sanpham"].ToString();

            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            string xuatxu = "";
            if (context.Request.QueryString["xuatxu"] != null)
                xuatxu = context.Request.QueryString["xuatxu"].ToString();

            string location = "";
            if (context.Request.QueryString["location"] != null)
                location = context.Request.QueryString["location"].ToString();

            string bin = "";
            if (context.Request.QueryString["bin"] != null)
                bin = context.Request.QueryString["bin"].ToString();


            string ngaythang = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();

            string _exportContent = "";
            using (StringWriter sb = new StringWriter())
            {
                using (HtmlTextWriter htmlWriter = new HtmlTextWriter(sb))
                {
                    Table table = new Table();
                    table.GridLines = GridLines.Both;

                    ExecuteData xl = new ExecuteData();

                    //table.BorderWidth = new Unit(1);

                    //Tieu de
                    TableHeaderCell header = new TableHeaderCell();
                    header.RowSpan = 1;
                    header.ColumnSpan = 19;
                    header.Text = "BÁO CÁO THEO DÕI SẢN PHẨM";
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
                    header.ColumnSpan = 19;
                    header.Text = "Ngày: " + ngaythang;
                    header.Font.Bold = true;
                    //header.BackColor = Color.LightGray;
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);

                    table.Rows.Add(headerRow);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 19;
                    header.Text = "Thời gian từ : " + tungay + " đến ngày " + denngay;
                    header.Font.Bold = true;
                    //header.BackColor = Color.LightGray;
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 19;
                    header.Text = "Kho : " + xl.ExecuteScalarSQL("SELECT makho FROM Kho WHERE pk_seq = '" + kho + "' UNION ALL SELECT ma FROM NhomKhachHang WHERE pk_seq = '" + kho + "' ").ToString();
                    header.Font.Bold = true;
                    //header.BackColor = Color.LightGray;
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 19;
                    header.Text = "Sản phẩm: " + xl.ExecuteScalarSQL("SELECT ten FROM SanPham WHERE pk_seq = '" + sanpham + "'").ToString();
                    header.Font.Bold = true;
                    //header.BackColor = Color.LightGray;
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 19;
                    header.Text = "Hạn sử dụng: " + ngayhethan;
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

                    //LAY THONG TIN SAN PHAM
                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 19;
                    header.Text = "THÔNG TIN NHẬP HÀNG";
                    header.Font.Bold = true;
                    header.BackColor = Color.LightGreen;
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    //END THONG TIN SP

                    string condition = "";
                    string conditionNHAP = "";
                    string conditionDLB = "";

                    if (xuatxu.Trim().Length > 0)
                    {
                        condition += " and xx.pk_seq = '" + xuatxu + "' ";
                    }
                    if (location.Trim().Length > 0)
                    {
                        condition += " and loc.pk_seq = '" + location + "' ";
                    }
                    if (bin.Trim().Length > 0)
                    {
                        condition += " and bin.pk_seq = '" + bin + "' ";
                    }


                    // Thong tin nhap SP
                    string[] tieude = new string[] { "STT", "Nghiệp vụ", "Số nhập", "Ngày nhập", "Ngày chốt", "Trạng thái", "Mã đối tượng", "Tên đối tượng", "HSD", "Quantity" };
                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;
                        header.BackColor = Color.LightBlue;
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);


                    string sql = "  SELECT NHAP.nghiepvu, NHAP.sonhaphang, NHAP.ngaynghiepvu, NHAP.ngaychot, NHAP.manhacungcap, NHAP.nhacungcap, " +
                                 "  	 ISNULL(xx.ten, '') as xuatxu, ISNULL(loc.ten, '') as location, ISNULL(bin.ten, '') as bin, " +
                                 "  	 NHAP.solo, NHAP.soluong, NHAP.donvitinh, NHAP.soluongQUYDOI, NHAP.donviquydoi, NHAP.gianhap " +
                                 "  	 FROM (  " +
                                 "     SELECT N'Nhập hàng' as nghiepvu, nh.pk_seq sonhaphang, nh.ngaynhap as ngaynghiepvu, CONVERT(nvarchar(10), nh.ngaysua, 110) AS ngaychot, nh.kho_fk, ISNULL(ncc.ma, '') manhacungcap, ncc.ten as nhacungcap ,  " +
                                 "  		nh_sp_ct.xuatxu_fk, nh_sp_ct.location_fk, nh_sp_ct.bin_fk,      " +
                                 "      	nh_sp_ct.solo, nh_sp_ct.soluong , dvt.ten as donvitinh, nh_sp_ct.soluongQUYDOI,  " +
                                 "      	ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk), '') donviquydoi, ISNULL(nh_sp.gianhap, 0) gianhap  " +
                                 "      FROM NhapHang nh INNER JOIN NhapHang_SanPham nh_sp ON nh.pk_seq = nh_sp.nhaphang_fk  " +
                                 "      	INNER JOIN NhapHang_SanPham_ChiTiet nh_sp_ct ON nh_sp.nhaphang_fk = nh_sp_ct.nhaphang_fk AND nh_sp.sanpham_fk = nh_sp_ct.sanpham_fk  " +
                                 "      	INNER JOIN NhaCungCap ncc ON nh.ncc_fk = ncc.pk_seq  " +
                                 "      	INNER JOIN SanPham sp ON nh_sp_ct.sanpham_fk = sp.pk_seq  " +
                                 "      	INNER JOIN DonViTinh dvt ON nh_sp.DVT_FK = dvt.pk_seq  " +
                                 "     WHERE nh.pk_seq > 0  AND nh.trangthai = 1  and sp.pk_seq = '" + sanpham + "' and nh_sp_ct.solo = '" + ngayhethan + "'   " +
                                 "         and CONVERT(datetime, nh.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)   " +
                                 "         and CONVERT(datetime, nh.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105)   " +
                                 "    UNION ALL  " +
                                 " SELECT N'Nhập ký gửi từ ĐHB' as nghiepvu, dh.pk_seq sonhaphang, dh.ngaydonhang as ngaynghiepvu, CONVERT(nvarchar(10), dh.thoigianCHOT, 110) AS ngaychot, dh.doituongKG as kho_fk, N'KCT' manhacungcap, N'Kho công ty' as nhacungcap ,   " +
                                 "  	dh_sp_ct.xuatxu_fk, dh_sp_ct.location_fk, dh_sp_ct.bin_fk,       " +
                                 "     	dh_sp_ct.SOLO, dh_sp_ct.soluong , dvt.ten as donvitinh, dh_sp_ct.soluongQUYDOI,   " +
                                 "     	ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk), '') donviquydoi, ISNULL(dh_sp.dongiaSAUCHIA, 0) gianhap   " +
                                 "     FROM DonHang dh INNER JOIN DonHang_SanPham dh_sp ON dh.pk_seq = dh_sp.donhang_fk   " +
                                 "     	INNER JOIN DonHang_SanPham_ChiTiet dh_sp_ct ON dh_sp.donhang_fk = dh_sp_ct.donhang_fk AND dh_sp.sanpham_fk = dh_sp_ct.sanpham_fk     	 " +
                                 "     	INNER JOIN SanPham sp ON dh_sp_ct.sanpham_fk = sp.pk_seq   " +
                                 "     	INNER JOIN DonViTinh dvt ON dh_sp.DVT_FK = dvt.pk_seq   " +
                                 "    WHERE dh.pk_seq > 0  AND dh.trangthai in (2, 5, 6)  and sp.pk_seq = '" + sanpham + "' and dh.donhang_logistic = 0 and dh_sp_ct.solo = '" + ngayhethan + "'    " +
                                 "        and CONVERT(datetime, dh.thoigianCHOT, 105) >= CONVERT(datetime, '" + tungay + "', 105)    " +
                                 "        and CONVERT(datetime, dh.thoigianCHOT, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                                 "        and dh.tangkhoKYGUI = 1 " +
                                 "    UNION ALL  " +
                                 "      SELECT N'Nhập khác' as nghiepvu, nh.pk_seq sonhaphang, nh.ngaynhap as ngaynghiepvu, CONVERT(nvarchar(10), nh.ngaysua, 110) AS ngaychot, nh.kho_fk as kho_fk, ISNULL(nh.madoituong, '') manhacungcap, ISNULL(nh.tendoituong, '') nhacungcap,   " +
                                 "      	nh_sp_ct.xuatxu_fk, nh_sp_ct.location_fk, nh_sp_ct.bin_fk, " +
                                 "      	nh_sp_ct.solo, nh_sp_ct.soluong , dvt.ten as donvitinh, nh_sp_ct.soluongQUYDOI,  " +
                                 "      	ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk), '') donviquydoi, ISNULL(nh_sp.dongia, 0) gianhap   " +
                                 "      FROM NhapKhac nh INNER JOIN NhapKhac_SanPham nh_sp ON nh.pk_seq = nh_sp.nhapkhac_fk AND nh.loainhap in (1, 2)  " +
                                 "      	INNER JOIN NhapKhac_SanPham_ChiTiet nh_sp_ct ON nh_sp.nhapkhac_fk = nh_sp_ct.nhapkhac_fk AND nh_sp.sanpham_fk = nh_sp_ct.sanpham_fk	  " +
                                 "      	INNER JOIN SanPham sp ON nh_sp_ct.sanpham_fk = sp.pk_seq  " +
                                 "      	INNER JOIN DonViTinh dvt ON nh_sp.DVT_FK = dvt.pk_seq  " +
                                 "      WHERE nh.pk_seq > 0  AND nh.trangthai = 1 and sp.pk_seq = '" + sanpham + "' and nh_sp_ct.solo = '" + ngayhethan + "'  " +
                                 "         and CONVERT(datetime, nh.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)   " +
                                 "         and CONVERT(datetime, nh.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105)   " +
                                 "    UNION ALL  " +
                                 "      SELECT N'Nhập khác - ký gửi' as nghiepvu, nh.pk_seq sonhaphang, nh.ngaynhap as ngaynghiepvu, CONVERT(nvarchar(10), nh.ngaysua, 110) AS ngaychot, nh.kho_fk as kho_fk, ISNULL(nh.madoituong, '') manhacungcap, ISNULL(nh.tendoituong, '') nhacungcap,   " +
                                 "      	nh_sp_ct.xuatxu_fk, nh_sp_ct.location_fk, nh_sp_ct.bin_fk, " +
                                 "      	nh_sp_ct.solo, nh_sp_ct.soluong , dvt.ten as donvitinh, nh_sp_ct.soluongQUYDOI,  " +
                                 "      	ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk), '') donviquydoi, ISNULL(nh_sp.dongia, 0) gianhap   " +
                                 "      FROM NhapKhac nh INNER JOIN NhapKhac_SanPham nh_sp ON nh.pk_seq = nh_sp.nhapkhac_fk AND nh.loainhap in (3)  " +
                                 "      	INNER JOIN NhapKhac_SanPham_ChiTiet nh_sp_ct ON nh_sp.nhapkhac_fk = nh_sp_ct.nhapkhac_fk AND nh_sp.sanpham_fk = nh_sp_ct.sanpham_fk	  " +
                                 "      	INNER JOIN SanPham sp ON nh_sp_ct.sanpham_fk = sp.pk_seq  " +
                                 "      	INNER JOIN DonViTinh dvt ON nh_sp.DVT_FK = dvt.pk_seq  " +
                                 "      WHERE nh.pk_seq > 0  AND nh.trangthai = 1 and sp.pk_seq = '" + sanpham + "' and nh_sp_ct.solo = '" + ngayhethan + "'  " +
                                 "         and CONVERT(datetime, nh.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)   " +
                                 "         and CONVERT(datetime, nh.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105)   " +
                                 "    UNION ALL  " +
                                 "      SELECT N'Nhập sản xuất' as nghiepvu, nh.pk_seq sonhaphang, nh.ngaynhap as ngaynghiepvu, CONVERT(nvarchar(10), nh.ngaysua, 110) AS ngaychot, nh.kho_fk, ISNULL(nh.madoituong, '') manhacungcap, ISNULL(nh.tendoituong, '') nhacungcap,   " +
                                 "      	nh_sp_ct.xuatxu_fk, nh_sp_ct.location_fk, nh_sp_ct.bin_fk,  " +
                                 "      	nh_sp_ct.solo, nh_sp_ct.soluong , dvt.ten as donvitinh, nh_sp_ct.soluongQUYDOI,  " +
                                 "      	ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk), '') donviquydoi, ISNULL(nh_sp.dongia, 0) gianhap   " +
                                 "      FROM NhapKhac nh INNER JOIN NhapKhac_SanPham nh_sp ON nh.pk_seq = nh_sp.nhapkhac_fk AND nh.loainhap in (4)  " +
                                 "      	INNER JOIN NhapKhac_SanPham_ChiTiet nh_sp_ct ON nh_sp.nhapkhac_fk = nh_sp_ct.nhapkhac_fk AND nh_sp.sanpham_fk = nh_sp_ct.sanpham_fk	   " +
                                 "      	INNER JOIN SanPham sp ON nh_sp_ct.sanpham_fk = sp.pk_seq  " +
                                 "      	INNER JOIN DonViTinh dvt ON nh_sp.DVT_FK = dvt.pk_seq  " +
                                 "      WHERE nh.pk_seq > 0  AND nh.trangthai = 1 and sp.pk_seq = '" + sanpham + "' and nh_sp_ct.solo = '" + ngayhethan + "'  " +
                                 "         and CONVERT(datetime, nh.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)   " +
                                 "         and CONVERT(datetime, nh.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                                 "    UNION ALL  " +
                                 "     SELECT N'Đổi lô bin' as nghiepvu, dh.pk_seq as sonhaphang, dh.ngaynhap as ngaynghiepvu, CONVERT(nvarchar(10), dh.ngaysua, 110) AS ngaychot, khonhan_fk as kho_fk ,  '' as manhacungcap, '' as nhacungcap,  " +
                                 "       	dh_sp_ct.xuatxu_new_fk, dh_sp_ct.location_new_fk, dh_sp_ct.bin_new_fk,  " +
                                 "       	dh_sp_ct.solo_new as solo,	dh_sp_ct.soluong, dvt.ten as donvi, dh_sp_ct.soluongQuyDoi, dvt2.ten as donviCHUAN, 0 as gianhap   " +
                                 "       FROM DoiLoBin dh INNER JOIN DoiLoBin_SanPham dh_sp ON dh.pk_seq = dh_sp.doilobin_fk     " +
                                 "          	INNER JOIN DoiLoBin_SanPham_ChiTiet dh_sp_ct ON dh_sp.doilobin_fk = dh_sp_ct.doilobin_fk AND dh_sp.sanpham_fk = dh_sp_ct.sanpham_fk     " +
                                 "          	INNER JOIN SanPham sp ON dh_sp_ct.sanpham_fk = sp.pk_seq     " +
                                 "          	LEFT JOIN DonViTinh dvt ON dh_sp.dvt_fk = dvt.pk_seq     " +
                                 "          	LEFT JOIN DonViTinh dvt2 ON sp.dvt_fk = dvt2.pk_seq       " +
                                 "      WHERE dh.pk_seq > 0  AND dh.trangthai = 1 and sp.pk_seq = '" + sanpham + "' and dh_sp_ct.solo_new = '" + ngayhethan + "'  " +
                                 "         and CONVERT(datetime, dh.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)   " +
                                 "         and CONVERT(datetime, dh.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                                 "      UNION ALL " +
                                 "         SELECT N'Điều chỉnh tồn kho' as nghiepvu, dc.pk_seq sonhaphang, dc.ngaydieuchinh as ngaynghiepvu, CONVERT(nvarchar(10), dc.ngaysua, 110) AS ngaychot, dc.kho_fk, '' manhacungcap, '' nhacungcap,   " +
                                 "      	dc_sp.xuatxu_fk, dc_sp.location_fk, dc_sp.bin_fk,  " +
                                 "      	dc_sp.solo, dc_sp.dieuchinh , dvt.ten as donvitinh, dc_sp.dieuchinh as soluongQuyDoi,  " +
                                 "      	ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk), '') donviquydoi, 0 gianhap   " +
                                 "      FROM DieuChinhTonKho dc INNER JOIN DieuChinhTonKho_SanPham dc_sp ON dc.pk_seq = dc_sp.dieuchinh_fk    	 " +
                                 "      	INNER JOIN SanPham sp ON dc_sp.sanpham_fk = sp.pk_seq  " +
                                 "      	INNER JOIN DonViTinh dvt ON sp.dvt_fk = dvt.pk_seq  " +
                                 "      WHERE dc.pk_seq > 0  AND dc.trangthai = 1 and sp.pk_seq = '" + sanpham + "' and dc_sp.solo = '" + ngayhethan + "'  " +
                                 "         and CONVERT(datetime, dc.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)   " +
                                 "         and CONVERT(datetime, dc.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                                 //"  	 UNION ALL " +
                                 //"         SELECT N'Kiểm kho' as nghiepvu, kk.pk_seq sonhaphang, kk.ngaykiem as ngaynghiepvu, CONVERT(nvarchar(10), kk.ngaysua, 110) AS ngaychot, kk.kho_fk, '' manhacungcap, '' nhacungcap,   " +
                                 //"      	kk_sp.xuatxu_fk, kk_sp.location_fk, kk_sp.bin_fk,  " +
                                 //"      	kk_sp.SOLO, kk_sp.chenhlech as soluong , dvt.ten as donvitinh, kk_sp.chenhlech as soluongQuyDoi,  " +
                                 //"      	ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk), '') donviquydoi, 0 gianhap   " +
                                 //"      FROM KiemKho kk INNER JOIN KiemKho_SanPham kk_sp ON kk.pk_seq = kk_sp.kiemkho_fk    	 " +
                                 //"      	INNER JOIN SanPham sp ON kk_sp.sanpham_fk = sp.pk_seq  " +
                                 //"      	INNER JOIN DonViTinh dvt ON sp.dvt_fk = dvt.pk_seq  " +
                                 //"      WHERE kk.pk_seq > 0  AND kk.trangthai = 1 and sp.pk_seq = '" + sanpham + "' and kk_sp.solo = '" + ngayhethan + "'  " +
                                 //"         and CONVERT(datetime, kk.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)   " +
                                 //"         and CONVERT(datetime, kk.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                                 "  	UNION ALL " +
                                 "         SELECT N'Khách hàng trả hàng' as nghiepvu, dth.pk_seq sonhaphang, dth.ngaydonhang as ngaynghiepvu, CONVERT(nvarchar(10), dth.ngaysua, 110) AS ngaychot, dth.kho_fk, '' manhacungcap, '' nhacungcap,   " +
                                 "      	dth_sp_ct.xuatxu_fk, dth_sp_ct.location_fk, dth_sp_ct.bin_fk,  " +
                                 "      	dth_sp_ct.SOLO, dth_sp_ct.soluong as soluong , dvt.ten as donvitinh, dth_sp_ct.soluongQuyDoi as soluongQuyDoi,  " +
                                 "      	ISNULL((SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk), '') donviquydoi, 0 gianhap   " +
                                 "      FROM DonTraHang dth INNER JOIN DonTraHang_SanPham dth_sp ON dth.pk_seq = dth_sp.dontrahang_fk   " +
                                 "  		INNER JOIN DonTraHang_SanPham_ChiTiet dth_sp_ct ON dth_sp.dontrahang_fk = dth_sp_ct.dontrahang_fk AND dth_sp.sanpham_fk = dth_sp_ct.sanpham_fk " +
                                 "      	INNER JOIN SanPham sp ON dth_sp.sanpham_fk = sp.pk_seq  " +
                                 "      	INNER JOIN DonViTinh dvt ON dth_sp.dvt_fk = dvt.pk_seq  " +
                                 "      WHERE dth.pk_seq > 0  AND dth.trangthai = 1 and sp.pk_seq = '" + sanpham + "' and dth_sp_ct.solo = '" + ngayhethan + "'  " +
                                 "         and CONVERT(datetime, dth.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)   " +
                                 "         and CONVERT(datetime, dth.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                                 "     ) NHAP     " +
                                 "  	LEFT JOIN XuatXu xx ON NHAP.xuatxu_fk = xx.pk_seq " +
                                 "  	LEFT JOIN Location loc ON NHAP.location_fk = loc.pk_seq " +
                                 "  	LEFT JOIN Bin bin ON NHAP.bin_fk = bin.pk_seq   " +
                                 "  WHERE NHAP.kho_fk = '" + kho + "' " + condition +
                                 "  ORDER BY NHAP.nghiepvu, CONVERT(datetime, NHAP.ngaynghiepvu, 105) asc, xx.ten, loc.ten, bin.ten ";

                    DataTable dt = xl.ReadTable(sql);

                    double totalSOLUONGNHAP = 0;
                    double totalSOLUONGNHAP_QuyDoi = 0;
                    double totalTIENNHAP = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string nghiepvu = dt.Rows[i]["nghiepvu"].ToString();
                        string sonhaphang = dt.Rows[i]["sonhaphang"].ToString();
                        string ngaynghiepvu = dt.Rows[i]["ngaynghiepvu"].ToString();
                        string ngaychot = dt.Rows[i]["ngaychot"].ToString();
                        string trangthai = "Đã nhập";
                        string manhacungcap = dt.Rows[i]["manhacungcap"].ToString();
                        string nhacungcap = dt.Rows[i]["nhacungcap"].ToString();
                        string tenkho = "";
                        string xuatxuNHAP = dt.Rows[i]["xuatxu"].ToString();
                        string locationNHAP = dt.Rows[i]["location"].ToString();
                        string binNHAP = dt.Rows[i]["bin"].ToString();
                        string soloNHAP = dt.Rows[i]["solo"].ToString();
                        string ngayNHAP = dt.Rows[i]["ngaynhap"].ToString();
                        string soluong = dt.Rows[i]["soluong"].ToString();
                        string donvitinh = dt.Rows[i]["donvitinh"].ToString();
                        string gianhap = dt.Rows[i]["gianhap"].ToString();
                        double thanhtien = double.Parse(soluong) * double.Parse(gianhap);
                        string soluongQUYDOI = dt.Rows[i]["soluongQUYDOI"].ToString();
                        string donviquydoi = dt.Rows[i]["donviquydoi"].ToString();

                        totalSOLUONGNHAP += double.Parse(soluong);
                        totalSOLUONGNHAP_QuyDoi += double.Parse(soluongQUYDOI);
                        totalTIENNHAP += thanhtien;

                        string[] data = new string[] { (i + 1).ToString(), nghiepvu, sonhaphang, ngaynghiepvu, ngaychot, trangthai, manhacungcap, nhacungcap,
                            soloNHAP, FormatString.ForMatNumber(soluong)};

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();

                            if (j == 0)
                                cell.Width = Unit.Parse("50");
                            else if (j == 7)
                                cell.Width = Unit.Parse("450");
                            else if (j == 8 || j == 9 || j == 10)
                                cell.Width = Unit.Parse("120");
                            else if (j == 1 || j == 6)
                                cell.Width = Unit.Parse("150");
                            else
                                cell.Width = Unit.Parse("80");

                            if (j < 5 || j == 8 || j == 9 || j == 10 || j == 11 || j == 14 || j == 18)
                                cell.HorizontalAlign = HorizontalAlign.Center;
                            if (j == 13 || j == 15 || j == 16 || j == 17)
                                cell.HorizontalAlign = HorizontalAlign.Right;

                            cell.Text = data[j];
                            row.Cells.Add(cell);
                        }

                        table.Rows.Add(row);
                    }

                    if (dt.Rows.Count >= 0)
                    {
                        string[] data = new string[] { "Total", FormatString.ForMatNumber(totalSOLUONGNHAP.ToString()) };

                        TableRow row = new TableRow();
                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.Font.Bold = true;
                            cell.HorizontalAlign = HorizontalAlign.Right;
                            cell.BackColor = Color.LightGray;

                            if (j == 0)
                            {
                                cell.HorizontalAlign = HorizontalAlign.Center;
                                cell.ColumnSpan = 13;
                            }

                            cell.Text = data[j];

                            //cell.Attributes.Add("style", @"mso-number-format:\@;");

                            row.Cells.Add(cell);
                        }

                        table.Rows.Add(row);
                    }

                    //LAY THONG TIN SAN PHAM
                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 19;
                    header.Text = "THÔNG TIN XUẤT HÀNG";
                    header.Font.Bold = true;
                    header.BackColor = Color.LightGreen;
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    // Thong tin BAN SP
                    tieude = new string[] { "STT", "Nghiệp vụ", "Số xuất", "Ngày xuất", "Ngày chốt","Trạng thái", "Mã đối tượng", "Tên đối tượng",
                        "HSD", "Quantity" };
                    headerRow = new TableRow();
                    for (int i = 0; i < tieude.Length; i++)
                    {
                        //Tieu de
                        header = new TableHeaderCell();

                        header.Text = tieude[i];
                        header.Font.Bold = true;
                        header.BackColor = Color.LightBlue;
                        header.HorizontalAlign = HorizontalAlign.Center;
                        header.VerticalAlign = VerticalAlign.Middle;

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    sql = " SELECT XUAT.nghiepvu, XUAT.sodonhang, XUAT.ngaydonhang, XUAT.ngaychot, XUAT.kho_fk, XUAT.madoituong, XUAT.tendoituong,  " +
                             "  	xx.ten as xuatxu, loc.ten as location, bin.ten as bin,  " +
                             "  	XUAT.solo, XUAT.soluong, XUAT.donvi, XUAT.soluongQUYDOI,  XUAT.donviCHUAN, XUAT.dongia, XUAT.trangthai " +
                             "  FROM (  " +
                             "  	SELECT N'Bán hàng' as nghiepvu, dh.pk_seq as sodonhang, dh.ngaydonhang, CONVERT(nvarchar(10), dh.thoigianCHOT, 105) AS ngaychot,  " +
                             "  		CASE dh.DonHangKyGui WHEN 0 THEN ISNULL((SELECT pk_seq FROM Kho WHERE pk_seq = dh.kho_fk), '')    " +
                             "   			WHEN 1 THEN ISNULL((SELECT pk_seq FROM NhomKhachHang WHERE pk_seq = dh.doituongKG), '') END kho_fk,   " +
                             "   	dh_sp_ct.soluong, dvt.ten as donvi, dh_sp_ct.soluongQuyDoi, dvt2.ten as donviCHUAN,  " +
                             "   	dh_sp_ct.xuatxu_fk, dh_sp_ct.location_fk, dh_sp_ct.bin_fk, dh_sp_ct.solo, dh_sp.dongia,  " +
                             "  		CASE WHEN dh.trangthai = 0 THEN N'Chưa xuất kho'            		 " +
                             "   			WHEN dh.trangthai in (2, 5, 6) THEN N'Đã xuất kho'   " +
                             "   			ELSE N'Trạng thái khác' END trangthai, kh.ma as madoituong, kh.hoten as tendoituong  " +
                             "   FROM DonHang dh INNER JOIN DonHang_SanPham dh_sp ON dh.pk_seq = dh_sp.donhang_fk    " +
                             "      	INNER JOIN DonHang_SanPham_ChiTiet dh_sp_ct ON dh_sp.donhang_fk = dh_sp_ct.donhang_fk AND dh_sp.sanpham_fk = dh_sp_ct.sanpham_fk    " +
                             "      	INNER JOIN SanPham sp ON dh_sp_ct.sanpham_fk = sp.pk_seq    " +
                             "      	LEFT JOIN DonViTinh dvt ON dh_sp.dvt_fk = dvt.pk_seq    " +
                             "      	LEFT JOIN DonViTinh dvt2 ON sp.dvt_fk = dvt2.pk_seq               	 " +
                             "      	INNER JOIN KhachHang kh ON dh.khachhang_fk = kh.pk_seq    " +
                             "   WHERE dh.pk_seq > 0 AND dh.trangthai in (2, 5, 6) and sp.pk_seq = '" + sanpham + "' and dh_sp_ct.solo = '" + ngayhethan + "'   " +
                             "            and CONVERT(datetime, dh.thoigianCHOT, 105) >= CONVERT(datetime, '" + tungay + "', 105)  " +
                             "            and CONVERT(datetime, dh.thoigianCHOT, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                             "  UNION ALL   " +
                             "  	SELECT N'Đổi lô bin' as nghiepvu, dh.pk_seq as sodonhang, dh.ngaynhap as ngaydonhang, CONVERT(nvarchar(10), dh.ngaysua, 105) AS ngaychot, ISNULL(khonhan_fk, 0) kho_fk,     " +
                             "     		dh_sp_ct.soluong, dvt.ten as donvi, dh_sp_ct.soluongQuyDoi, dvt2.ten as donviCHUAN,    " +
                             "  		dh_sp_ct.xuatxu_fk, dh_sp_ct.location_fk, dh_sp_ct.bin_fk, dh_sp_ct.solo, dh_sp.dongia,    " +
                             "   		N'Đã xuất kho' as trangthai, '' as madoituong, '' as tendoituong    " +
                             "     FROM DoiLoBin dh INNER JOIN DoiLoBin_SanPham dh_sp ON dh.pk_seq = dh_sp.doilobin_fk      " +
                             "        	INNER JOIN DoiLoBin_SanPham_ChiTiet dh_sp_ct ON dh_sp.doilobin_fk = dh_sp_ct.doilobin_fk AND dh_sp.sanpham_fk = dh_sp_ct.sanpham_fk      " +
                             "        	INNER JOIN SanPham sp ON dh_sp_ct.sanpham_fk = sp.pk_seq      " +
                             "        	LEFT JOIN DonViTinh dvt ON dh_sp.dvt_fk = dvt.pk_seq      " +
                             "        	LEFT JOIN DonViTinh dvt2 ON sp.dvt_fk = dvt2.pk_seq                   	  " +
                             "     WHERE dh.pk_seq > 0 AND dh.trangthai in (1) and sp.pk_seq = '" + sanpham + "' and dh_sp_ct.solo = '" + ngayhethan + "'  " +
                             "            and CONVERT(datetime, dh.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)  " +
                             "            and CONVERT(datetime, dh.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                             "  UNION ALL  " +
                             "  	SELECT N'Xuất khác' as nghiepvu, dh.pk_seq as sodonhang, dh.ngayxuat as ngaydonhang, CONVERT(nvarchar(10), dh.ngaysua, 105) AS ngaychot, " +
                             "    		CASE dh.kygui WHEN 1 THEN ISNULL((SELECT pk_seq FROM NhomKhachHang WHERE pk_seq = dh.kho_fk), '')     " +
                             "     			ELSE ISNULL((SELECT pk_seq FROM Kho WHERE pk_seq = dh.kho_fk), '') END kho_fk,    " +
                             "     	dh_sp_ct.soluong, dvt.ten as donvi, dh_sp_ct.soluongQuyDoi, dvt2.ten as donviCHUAN,   " +
                             "      dh_sp_ct.xuatxu_fk, dh_sp_ct.location_fk,dh_sp_ct.bin_fk, dh_sp_ct.solo, dh_sp.dongia,   " +
                             "   	N'Đã xuất kho' as trangthai, '' as madoituong, '' as tendoituong   " +
                             "  	FROM XuatKhac dh INNER JOIN XuatKhac_SanPham dh_sp ON dh.pk_seq = dh_sp.xuatkhac_fk     " +
                             "        	INNER JOIN XuatKhac_SanPham_ChiTiet dh_sp_ct ON dh_sp.xuatkhac_fk = dh_sp_ct.xuatkhac_fk AND dh_sp.sanpham_fk = dh_sp_ct.sanpham_fk     " +
                             "        	INNER JOIN SanPham sp ON dh_sp_ct.sanpham_fk = sp.pk_seq     " +
                             "        	LEFT JOIN DonViTinh dvt ON dh_sp.dvt_fk = dvt.pk_seq     " +
                             "        	LEFT JOIN DonViTinh dvt2 ON sp.dvt_fk = dvt2.pk_seq                     " +
                             "     WHERE dh.pk_seq > 0 AND dh.trangthai in (1) and sp.pk_seq = '" + sanpham + "' and dh_sp_ct.solo = '" + ngayhethan + "'  " +
                             "            and CONVERT(datetime, dh.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)  " +
                             "            and CONVERT(datetime, dh.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105)  " +
                             "  UNION ALL " +
                             "  	SELECT N'Trả hàng NCC' as nghiepvu, dh.pk_seq as sodonhang, dh.ngaytra as ngaydonhang, CONVERT(nvarchar(10), dh.ngaysua, 105) AS ngaychot, ISNULL(kho_fk, 0) kho_fk,    " +
                             "     	dh_sp_ct.soluong, dvt.ten as donvi, dh_sp_ct.soluongQuyDoi, dvt2.ten as donviCHUAN,   " +
                             "      dh_sp_ct.xuatxu_fk, dh_sp_ct.location_fk,dh_sp_ct.bin_fk, dh_sp_ct.solo, dh_sp.giatra,   " +
                             "   	N'Đã xuất kho' as trangthai, '' as madoituong, '' as tendoituong   " +
                             "  	FROM TraHang dh INNER JOIN TraHang_SanPham dh_sp ON dh.pk_seq = dh_sp.trahang_fk     " +
                             "        	INNER JOIN TraHang_SanPham_ChiTiet dh_sp_ct ON dh_sp.trahang_fk = dh_sp_ct.trahang_fk AND dh_sp.sanpham_fk = dh_sp_ct.sanpham_fk     " +
                             "        	INNER JOIN SanPham sp ON dh_sp_ct.sanpham_fk = sp.pk_seq     " +
                             "        	LEFT JOIN DonViTinh dvt ON dh_sp.dvt_fk = dvt.pk_seq     " +
                             "        	LEFT JOIN DonViTinh dvt2 ON sp.dvt_fk = dvt2.pk_seq                     " +
                             "     WHERE dh.pk_seq > 0 AND dh.trangthai in (1) and sp.pk_seq = '" + sanpham + "' and dh_sp_ct.solo = '" + ngayhethan + "'  " +
                             "            and CONVERT(datetime, dh.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)  " +
                             "            and CONVERT(datetime, dh.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                             "  UNION ALL " +
                             "  	SELECT N'Tieu hao' as nghiepvu, dh.pk_seq as sodonhang, dh_sp.ngaytieuhao as ngaydonhang, CONVERT(nvarchar(10), dh.ngaysua, 105) AS ngaychot, ISNULL(dh_sp.kho_fk, 0) kho_fk,    " +
                             "     	dh_sp_ct.soluong, dvt.ten as donvi, dh_sp_ct.soluongQuyDoi, dvt.ten as donviCHUAN,   " +
                             "      dh_sp_ct.xuatxu_fk, dh_sp_ct.location_fk,dh_sp_ct.bin_fk, dh_sp_ct.solo, 0 as dongia,   " +
                             "   	N'Đã xuất kho' as trangthai, '' as madoituong, '' as tendoituong   " +
                             "  	FROM LenhSanXuat dh INNER JOIN LenhSanXuat_TieuHao dh_sp ON dh.pk_seq = dh_sp.lenhsanxuat_fk     " +
                             "        	INNER JOIN LenhSanXuat_TieuHao_ChiTiet dh_sp_ct ON dh_sp.lenhsanxuat_fk = dh_sp_ct.lenhsanxuat_fk  " +
                             "        	INNER JOIN SanPham sp ON dh_sp_ct.vattu_fk = sp.pk_seq     " +
                             "        	LEFT JOIN DonViTinh dvt ON sp.pk_seq = dvt.pk_seq                   " +
                             "     WHERE dh.pk_seq > 0 AND dh.trangthai in (1) and sp.pk_seq = '" + sanpham + "' and dh_sp_ct.solo = '" + ngayhethan + "'  " +
                             "            and CONVERT(datetime, dh_sp.ngaysua, 105) >= CONVERT(datetime, '" + tungay + "', 105)  " +
                             "            and CONVERT(datetime, dh_sp.ngaysua, 105) <= CONVERT(datetime, '" + denngay + "', 105) " +
                             "   ) XUAT  " +
                             "  	INNER JOIN XuatXu xx ON XUAT.xuatxu_fk = xx.pk_seq " +
                             "  	INNER JOIN Location loc ON XUAT.location_fk = loc.pk_seq " +
                             "  	INNER JOIN Bin bin ON XUAT.bin_fk = bin.pk_seq " +
                             "  WHERE XUAT.kho_fk = '" + kho + "' " + condition +
                             "  ORDER BY XUAT.nghiepvu, CONVERT(datetime,XUAT.ngaydonhang, 105), xx.ten, loc.ten, bin.ten ASC ";

                    dt = xl.ReadTable(sql);

                    double totalSOLUONG = 0;
                    double totalSOLUONG_QuyDoi = 0;
                    double totalTIEN = 0;


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        double soluong = double.Parse(dt.Rows[i]["soluong"].ToString());
                        double soluongQUYDOI = double.Parse(dt.Rows[i]["soluongQuyDoi"].ToString());

                        double dongia = double.Parse(dt.Rows[i]["dongia"].ToString());
                        double thanhtien = soluong * dongia;


                        totalSOLUONG += soluong;
                        totalSOLUONG_QuyDoi += soluongQUYDOI;
                        totalTIEN += thanhtien;

                        string sodonhang = dt.Rows[i]["sodonhang"].ToString();
                        string ngaydonhang = dt.Rows[i]["ngaydonhang"].ToString();
                        string ngaychot = dt.Rows[i]["ngaychot"].ToString();
                        string trangthaiDH = dt.Rows[i]["trangthai"].ToString();
                        string madoituong = dt.Rows[i]["madoituong"].ToString();
                        string tendoituong = dt.Rows[i]["tendoituong"].ToString();
                        string makho = "";
                        string xuatxuDH = dt.Rows[i]["xuatxu"].ToString();
                        string locationDH = dt.Rows[i]["location"].ToString();
                        string binDH = dt.Rows[i]["bin"].ToString();

                        string[] data = new string[] { (i + 1).ToString(), dt.Rows[i]["nghiepvu"].ToString(), sodonhang, ngaydonhang, ngaychot, trangthaiDH, madoituong, tendoituong,
                                                    dt.Rows[i]["solo"].ToString(), FormatString.ForMatNumber(soluong.ToString()) };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();

                            //if (j == 0)
                            //    cell.Width = Unit.Parse("50");
                            //else if (j == 6)
                            //    cell.Width = Unit.Parse("450");
                            //else if (j == 7 || j == 8 || j == 9)
                            //    cell.Width = Unit.Parse("120");
                            //else if (j == 1 || j == 5)
                            //    cell.Width = Unit.Parse("150");
                            //else
                            //    cell.Width = Unit.Parse("80");

                            //if (j < 5 || j == 7 || j == 8 || j == 9 || j == 10 || j == 13 || j == 17)
                            //    cell.HorizontalAlign = HorizontalAlign.Center;
                            //if (j == 12 || j == 14 || j == 15 || j == 16)
                            //    cell.HorizontalAlign = HorizontalAlign.Right;      

                            //if (i % 2 == 0)
                            //    cell.BackColor = Color.LightSkyBlue;  

                            cell.Text = data[j];
                            row.Cells.Add(cell);
                        }

                        table.Rows.Add(row);
                    }

                    if (dt.Rows.Count >= 0)
                    {
                        string[] data = new string[] { "Total", FormatString.ForMatNumber(totalSOLUONG.ToString()) };

                        TableRow row = new TableRow();
                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.Font.Bold = true;
                            cell.HorizontalAlign = HorizontalAlign.Right;
                            cell.BackColor = Color.LightGray;

                            if (j == 0)
                            {
                                cell.HorizontalAlign = HorizontalAlign.Center;
                                cell.ColumnSpan = 13;
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

        private string ExportToExcel_CapNhatLocation(HttpContext context)
        {
            
            string ngaythang = DateTime.Now.ToString();
            string _exportContent = "";

            DataTable baocao = new DataTable("Update_Location");

            //baocao.Columns.Add("Customer", typeof(string));
            baocao.Columns.Add("Date", typeof(string));
            baocao.Columns.Add("Invoice No.", typeof(string));
            baocao.Columns.Add("Item No.(JP)", typeof(string));
            baocao.Columns.Add("Item No.(VN)", typeof(string));
            baocao.Columns.Add("PL No.", typeof(string));
            baocao.Columns.Add("UTC ORDER NO.", typeof(string));
            baocao.Columns.Add("COLOR", typeof(string));
            baocao.Columns.Add("Carton No", typeof(string));
            baocao.Columns.Add("No of ", typeof(string));
            baocao.Columns.Add("Lot No.", typeof(string));
            baocao.Columns.Add("-1", typeof(string));
            baocao.Columns.Add("Qty 1", typeof(double));
            baocao.Columns.Add("Qty 2", typeof(double));
            baocao.Columns.Add("Qty 3", typeof(double));
            baocao.Columns.Add("Yards", typeof(string));

            baocao.Columns.Add("-2", typeof(string));
            baocao.Columns.Add("-3", typeof(string));
            baocao.Columns.Add("-4", typeof(string));
            baocao.Columns.Add("-5", typeof(string));

            baocao.Columns.Add("ROLL", typeof(double));
            baocao.Columns.Add("Total", typeof(double));
            baocao.Columns.Add("S/V (M)", typeof(string));
            baocao.Columns.Add("Net WEIGHT", typeof(double));
            baocao.Columns.Add("Gross WEIGHT", typeof(double));

            baocao.Columns.Add("Plan shipping", typeof(string));
            baocao.Columns.Add("Location", typeof(string));
            
            ExecuteData xl = new ExecuteData();

            string condition = "";

            string sql = " SELECT sp.ngaynhap, sp.masp, sp.codeEnglish, sp.masp AS tensp, sp.utcOrderNo, sp.PLNo, sp.lotNo, sp.cartonNo, sp.NoOf, sp.solo, sp.netWeight, sp.grossWeight, " +
            "   sp.planShipping, sp.yards, sp.roll, sp.soluong, sp.soluong1, sp.soluong2, sp.soluong3, sp.mavach, " +
            "   ISNULL((SELECT ten FROM MauSac WHERE pk_seq = sp.mausac_fk), '') color, " +
            "   sp.locationName " +
            " FROM CapNhat_Location sp  " +
            " WHERE sp.sanpham_fk > 0  ";
            sql += " ORDER BY sp.stt ";

            DataTable dt = xl.ReadTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string ngaynhap = dt.Rows[i]["ngaynhap"].ToString();
                string solo = dt.Rows[i]["solo"].ToString();
                string itemVN = dt.Rows[i]["masp"].ToString();
                string itemJP = dt.Rows[i]["codeEnglish"].ToString();
                string utcOrderNo = dt.Rows[i]["utcOrderNo"].ToString();
                string PLNo = dt.Rows[i]["PLNo"].ToString();
                string lotNo = dt.Rows[i]["lotNo"].ToString();
                string noOf = dt.Rows[i]["noOf"].ToString();
                string cartonNo = dt.Rows[i]["cartonNo"].ToString();
                string netWeight = dt.Rows[i]["netWeight"].ToString();
                string grossWeight = dt.Rows[i]["grossWeight"].ToString();
                string planShipping = dt.Rows[i]["planShipping"].ToString();
                string yards = dt.Rows[i]["yards"].ToString();
                string roll = dt.Rows[i]["roll"].ToString();
                string color = dt.Rows[i]["color"].ToString();
                string soluong1 = FormatString.ForMatNumber(dt.Rows[i]["soluong1"].ToString());
                string soluong2 = FormatString.ForMatNumber(dt.Rows[i]["soluong2"].ToString());
                string soluong3 = FormatString.ForMatNumber(dt.Rows[i]["soluong3"].ToString());
                string soluong = FormatString.ForMatNumber(dt.Rows[i]["soluong"].ToString());
                string location = dt.Rows[i]["locationName"].ToString();
                
                DataRow dr = baocao.NewRow();

                dr[0] = ngaynhap;
                dr[1] = solo;
                dr[2] = itemJP;
                dr[3] = itemVN;
                dr[4] = PLNo;
                dr[5] = utcOrderNo;
                dr[6] = color;
                dr[7] = cartonNo;
                dr[8] = noOf;
                dr[9] = lotNo;
                dr[10] = "";
                dr[11] = soluong1;
                dr[12] = soluong2;
                dr[13] = soluong3;
                dr[14] = yards;
                dr[15] = "";
                dr[16] = "";
                dr[17] = "";
                dr[18] = "";
                dr[19] = roll;
                dr[20] = soluong;
                dr[21] = "";
                dr[22] = FormatString.ForMatNumber(netWeight);
                dr[23] = FormatString.ForMatNumber(grossWeight);

                dr[24] = planShipping;
                dr[25] = location;
                
                baocao.Rows.Add(dr);
            }

            string fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\Update_Location.xlsm";

            Workbook workbook = new Workbook(fileName);
            Worksheet worksheet = workbook.Worksheets[0];

            worksheet.Cells.ImportDataTable(baocao, true, "A4");

            worksheet.AutoFitColumns();

            worksheet.Cells["A1"].PutValue("UPDATE LOCATION");

            worksheet.Cells["A2"].PutValue("Created date");
            worksheet.Cells["C2"].PutValue(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));

            worksheet.Cells["A3"].PutValue("Username");
            worksheet.Cells["C3"].PutValue(context.Session["userName"].ToString());

            HttpResponse response = context.Response;
            workbook.Save(response, "Update_Location.xlsm", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsm));

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