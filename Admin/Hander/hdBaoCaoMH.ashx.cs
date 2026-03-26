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

namespace HLVTimeSheet.Admin.Hander
{
    /// <summary>
    /// Summary description for hdBaoCaoMH
    /// </summary>
    public class hdBaoCaoMH : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;

            string action = "";
            if (context.Request.QueryString["action"] != null)
                action = context.Request.QueryString["action"].ToString();

            if (action.Equals("nhaphangExcel"))
            {
                string solieu = "";
                if (context.Request.QueryString["solieu"] != null)
                    solieu = context.Request.QueryString["solieu"].ToString();

                if (solieu.Equals("1"))
                {
                    ExportToExcel_StockIn_New(context);
                }
                else if (solieu.Equals("2"))
                {
                    ExportToExcel_StockIn_Color(context);

                }
                else if (solieu.Equals("3"))
                {
                    ExportToExcel_StockIn_Line(context);
                }
                else if (solieu.Equals("4"))
                {
                    ExportToExcel_StockIn_Detail(context);
                }
                else if (solieu.Equals("5"))
                {
                    
                }
            }
            else if (action.Equals("fileTemplateStockIn"))
            {
                string exportContent = ExportToExcel_FileTemplateStockIn(context);
            }
            else if (action.Equals("nhapPartnerExcel"))
            {
                string solieu = "";
                if (context.Request.QueryString["solieu"] != null)
                    solieu = context.Request.QueryString["solieu"].ToString();

                if (solieu.Equals("1"))
                {
                    context.Response.AddHeader("content-disposition", "attachment; filename=StockInPartner_Total.xls");
                    context.Response.ContentType = "application/ms-excel";

                    HttpRequest request = context.Request;
                    HttpResponse response = context.Response;
                    string exportContent = ExportToExcel_NhapPartner_Total(context);
                    response.Write(exportContent);
                }
                else if (solieu.Equals("2"))
                {
                    context.Response.AddHeader("content-disposition", "attachment; filename=StockInPartner_LotNo.xls");
                    context.Response.ContentType = "application/ms-excel";

                    HttpRequest request = context.Request;
                    HttpResponse response = context.Response;
                    string exportContent = ExportToExcel_NhapPartner_LotNo(context);
                    response.Write(exportContent);
                }
                else if (solieu.Equals("3"))
                {
                    context.Response.AddHeader("content-disposition", "attachment; filename=StockInPartner_Detail.xls");
                    context.Response.ContentType = "application/ms-excel";

                    HttpRequest request = context.Request;
                    HttpResponse response = context.Response;
                    string exportContent = ExportToExcel_NhapPartner_ChiTiet(context);
                    response.Write(exportContent);
                }
            }
            else if (action.Equals("trahangExel"))
            {
                string exportContent = ExportToExcel_TRAHANG(context);
            }
            else if (action.Equals("theodoiPRExel"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=TheoDoiPR.xls");
                context.Response.ContentType = "application/ms-excel";

                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = ExportToExcel_THEODOIPR(context);
                response.Write(exportContent);
            }
            else if (action.Equals("theodoiPOExel"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=TheoDoiPO.xls");
                context.Response.ContentType = "application/ms-excel";

                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = ExportToExcel_THEODOIPO(context);
                response.Write(exportContent);
            }
            else if (action.Equals("id_nhaphangExcel"))
            {
                StockIn_ID_EXCEL(context);
            }
            else if (action.Equals("nhapkhacExcel"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=StockIn.xls");
                context.Response.ContentType = "application/ms-excel";
                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = NhapKhac_ID_EXCEL(context);
                response.Write(exportContent);
            }
            else if (action.Equals("nhaphangchiphiExcel"))
            {
                //context.Response.AddHeader("content-disposition", "attachment; filename=NhapHangChiPhi.xls");
                //context.Response.ContentType = "application/ms-excel";
                //HttpRequest request = context.Request;
                //HttpResponse response = context.Response;
                //string exportContent = NhapHang_ChiPhi_ID_EXCEL(context);
                //response.Write(exportContent);
            }
            else if (action.Equals("kiemkho_ID_Excel"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=KiemKho.xls");
                context.Response.ContentType = "application/ms-excel";
                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = KiemKho_ID_EXCEL(context);
                response.Write(exportContent);
            }
            else if (action.Equals("kiemkhoExcel"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=KiemKho.xls");
                context.Response.ContentType = "application/ms-excel";
                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = KiemKho_EXCEL(context);
                response.Write(exportContent);
            }
            else if (action.Equals("id_nhapPartnerExcel"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=StockIn_Partner.xls");
                context.Response.ContentType = "application/ms-excel";
                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = NhapKhac_ID_EXCEL(context);
                response.Write(exportContent);
            }
            else if (action.Equals("id_doilobinExcel"))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=TransferLocation.xls");
                context.Response.ContentType = "application/ms-excel";
                HttpRequest request = context.Request;
                HttpResponse response = context.Response;
                string exportContent = ExportToExcel_TransferID_Detail(context);
                response.Write(exportContent);

                //string exportContent = ExportToExcel_TransferID_Detail(context);
            }
        }

        private string ExportToExcel_THEODOIPO(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

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
                    header.ColumnSpan = 9;
                    header.Text = "THEO DÕI ĐƠN MUA HÀNG ( PO )";
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

                    string[] tieude = new string[] { "STT", "Số PO", "Ngày mua", "Nhà cung cấp", "Ngành hàng", "Nhãn hàng", "Chủng loại", "Mã sản phẩm", "Tên sản phẩm", "Đơn vị", "Số lượng mua", "Số lượng nhận", "Đơn giá mua", "Thành tiền" };

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
                    if (tungay.Trim().Length > 0)
                        condition += " AND convert( datetime, dh.ngaynhap, 105) >= convert( datetime, '" + tungay + "', 105) ";
                    if (denngay.Trim().Length > 0)
                        condition += " AND convert( datetime, dh.ngaynhap, 105) <= convert( datetime, '" + denngay + "', 105) ";

                    string sql = " SELECT dh.pk_seq sodonhang, REPLACE(CONVERT(varchar(11), CONVERT(datetime, dh.ngaynhap, 105), 113), ' ', '-') ngaynhap, ncc.ma as nccMa, ncc.ten as nccTEN,   " +
                                    " 		ISNULL(nh.ten, '') as nganhHANG, ISNULL(nhan.ten, '') as nhanhang, ISNULL(cl.ten, '') as chungloai, sp.ma as spMA, sp.ten as spTEN, dvt.ten as donvi,      " +
                                    " 		dh_sp.soluong as soluong, dh_sp.gianhap as dongia, dh_sp.soluong * dh_sp.gianhap as thanhtien, " +
                                    " 		ISNULL( ( " +
                                    " 			SELECT sum( nh_sp.soluong )   " +
                                    " 			FROM NHAPHANG nh INNER JOIN NHAPHANG_SANPHAM nh_sp on nh.pk_seq = nh_sp.nhaphang_fk " +
                                    " 			WHERE nh.muahang_fk = dh.pk_seq and nh.trangthai != 2 and nh_sp.sanpham_fk = dh_sp.sanpham_fk " +
                                    " 		 ), 0 ) as soluongNHAP " +
                                    " FROM MUAHANG dh INNER JOIN MuaHang_SanPham dh_sp on dh.pk_seq = dh_sp.muahang_fk     " +
                                    " 	INNER JOIN SanPham sp on dh_sp.sanpham_fk = sp.pk_seq        " +
                                    " 	INNER JOIN NGANHHANG nh on sp.NGANHHANG_FK = nh.pk_seq            " +
                                    " 	INNER JOIN NhaCungCap ncc on dh.ncc_fk = ncc.pk_seq        " +
                                    " 	INNER JOIN DONVITINH dvt on dh_sp.dvt_fk = dvt.pk_seq   " +
                                    " 	LEFT JOIN NHANHANG nhan on sp.NHANHANG_FK = nhan.pk_seq  " +
                                    " 	LEFT JOIN CHUNGLOAI cl on sp.CHUNGLOAI_FK = cl.pk_seq  " +
                                    " WHERE dh.trangthai in ( 1, 2 ) " + condition +
                                    " ORDER BY convert( datetime, dh.ngaynhap, 105) asc ";

                    DataTable dt = xl.ReadTable(sql);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string maNCC = dt.Rows[i]["nccMa"].ToString();
                        string tenNCC = dt.Rows[i]["nccTEN"].ToString();

                        string sodonhang = dt.Rows[i]["sodonhang"].ToString();
                        string ngaydonhang = dt.Rows[i]["ngaynhap"].ToString();

                        string nganhHANG = dt.Rows[i]["nganhHANG"].ToString();
                        string nhanHANG = dt.Rows[i]["nhanhang"].ToString();
                        string chungLOAI = dt.Rows[i]["chungloai"].ToString();
                        string spMA = dt.Rows[i]["spMA"].ToString();
                        string spTEN = dt.Rows[i]["spTEN"].ToString();
                        string donvi = dt.Rows[i]["donvi"].ToString();

                        double soluong = double.Parse(dt.Rows[i]["soluong"].ToString());
                        double soluongNHAN = double.Parse(dt.Rows[i]["soluongNHAP"].ToString());

                        double dongia = double.Parse(dt.Rows[i]["dongia"].ToString());
                        double thanhtien = double.Parse(dt.Rows[i]["thanhtien"].ToString());

                        string[] data = new string[] { ( i + 1).ToString(), sodonhang, ngaydonhang, tenNCC, 
                                                    nganhHANG, nhanHANG, chungLOAI, spMA, spTEN, donvi,
                                                    FormatString.ForMatNumber(soluong.ToString()), FormatString.ForMatNumber(soluongNHAN.ToString()), FormatString.ForMatNumber(dongia.ToString()), FormatString.ForMatNumber(thanhtien.ToString()) };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            cell.VerticalAlign = VerticalAlign.Middle;

                            if (j == 0)
                                cell.Width = Unit.Parse("50");
                            else if (j == 3 || j == 8)
                                cell.Width = Unit.Parse("250");
                            else if (j == 4 || j == 5 || j == 6 || j == 7)
                                cell.Width = Unit.Parse("150");
                            else
                                cell.Width = Unit.Parse("100");

                            if (j >= 10)
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

        private string ExportToExcel_THEODOIPR(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

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
                    header.ColumnSpan = 9;
                    header.Text = "THEO DÕI ĐỀ NGHỊ MUA HÀNG ( PR )";
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

                    string[] tieude = new string[] { "STT", "Số đề nghị", "Ngày mua", "Nhà cung cấp", "Ngành hàng", "Nhãn hàng", "Chủng loại", "Mã sản phẩm", "Tên sản phẩm", "Đơn vị", "Số lượng đề nghi", "Số lượng mua", "Đơn giá", "Thành tiền" };

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
                    if (tungay.Trim().Length > 0)
                        condition += " AND convert( datetime, dh.ngaynhap, 105) >= convert( datetime, '" + tungay + "', 105) ";
                    if (denngay.Trim().Length > 0)
                        condition += " AND convert( datetime, dh.ngaynhap, 105) <= convert( datetime, '" + denngay + "', 105) ";

                    string sql =  " SELECT dh.pk_seq sodonhang, dh.ngaynhap, ncc.ma as nccMa, ncc.ten as nccTEN,   " +
                                    " 		ISNULL(nh.ten, '') as nganhHANG, ISNULL(nhan.ten, '') as nhanhang, ISNULL(cl.ten, '') as chungloai, sp.ma as spMA, sp.ten as spTEN, dvt.ten as donvi,      " +
                                    " 		dh_sp.soluong as soluong, dh_sp.gianhap as dongia, dh_sp.soluong * dh_sp.gianhap as thanhtien, " +
                                    " 		ISNULL( ( " +
                                    " 			SELECT sum( nh_sp.soluong )   " +
                                    " 			FROM MUAHANG nh INNER JOIN MUAHANG_SANPHAM nh_sp on nh.pk_seq = nh_sp.muahang_fk " +
                                    " 			WHERE nh.dathang_fk = dh.pk_seq and nh.trangthai != 3 and nh_sp.sanpham_fk = dh_sp.sanpham_fk " +
                                    " 		 ), 0 ) as soluongMUA " +
                                    " FROM DENGHIMUAHANG dh INNER JOIN DENGHIMUAHANG_SanPham dh_sp on dh.pk_seq = dh_sp.denghi_fk     " +
                                    " 	INNER JOIN SanPham sp on dh_sp.sanpham_fk = sp.pk_seq        " +
                                    " 	INNER JOIN NGANHHANG nh on sp.NGANHHANG_FK = nh.pk_seq            " +
                                    " 	INNER JOIN NhaCungCap ncc on dh.ncc_fk = ncc.pk_seq        " +
                                    " 	INNER JOIN DONVITINH dvt on dh_sp.dvt_fk = dvt.pk_seq   " +
                                    " 	LEFT JOIN NHANHANG nhan on sp.NHANHANG_FK = nhan.pk_seq  " +
                                    " 	LEFT JOIN CHUNGLOAI cl on sp.CHUNGLOAI_FK = cl.pk_seq  " +
                                    " WHERE dh.trangthai in ( 1, 2 ) " + condition +
                                    " ORDER BY convert( datetime, dh.ngaynhap, 105) asc ";

                    DataTable dt = xl.ReadTable(sql);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string maNCC = dt.Rows[i]["nccMa"].ToString();
                        string tenNCC = dt.Rows[i]["nccTEN"].ToString();

                        string sodonhang = dt.Rows[i]["sodonhang"].ToString();
                        string ngaydonhang = dt.Rows[i]["ngaynhap"].ToString();

                        string nganhHANG = dt.Rows[i]["nganhHANG"].ToString();
                        string nhanHANG = dt.Rows[i]["nhanhang"].ToString();
                        string chungLOAI = dt.Rows[i]["chungloai"].ToString();
                        string spMA = dt.Rows[i]["spMA"].ToString();
                        string spTEN = dt.Rows[i]["spTEN"].ToString();
                        string donvi = dt.Rows[i]["donvi"].ToString();

                        double soluong = double.Parse(dt.Rows[i]["soluong"].ToString());
                        double soluongMUA = double.Parse(dt.Rows[i]["soluongMUA"].ToString());

                        double dongia = double.Parse(dt.Rows[i]["dongia"].ToString());
                        double thanhtien = double.Parse(dt.Rows[i]["thanhtien"].ToString());

                        string[] data = new string[] { ( i + 1).ToString(), sodonhang, ngaydonhang, tenNCC, 
                                                    nganhHANG, nhanHANG, chungLOAI, spMA, spTEN, donvi,
                                                    FormatString.ForMatNumber(soluong.ToString()), FormatString.ForMatNumber(soluongMUA.ToString()), FormatString.ForMatNumber(dongia.ToString()), FormatString.ForMatNumber(thanhtien.ToString()) };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            cell.VerticalAlign = VerticalAlign.Middle;

                            if (j == 0)
                                cell.Width = Unit.Parse("50");
                            else if (j == 3 || j == 8)
                                cell.Width = Unit.Parse("250");
                            else if (j == 4 || j == 5 || j == 6 || j == 7)
                                cell.Width = Unit.Parse("150");
                            else
                                cell.Width = Unit.Parse("100");

                            if (j >= 10)
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

            string layDACHOT = "0";
            if (context.Request.QueryString["layDACHOT"] != null)
                layDACHOT = context.Request.QueryString["layDACHOT"].ToString();

            string nccId = "";
            if (context.Request.QueryString["nccId"] != null)
                nccId = context.Request.QueryString["nccId"].ToString();

            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();

            string _exportContent = "";
            ExecuteData xl = new ExecuteData();

            string condition = "";
            if (tungay.Trim().Length > 0)
                condition += " and convert( datetime, dh.ngaytra, 105) >= convert( datetime, '" + tungay + "', 105) ";
            if (denngay.Trim().Length > 0)
                condition += " and convert( datetime, dh.ngaytra, 105) <= convert( datetime, '" + denngay + "', 105) ";
            if (nccId.Trim().Length > 0)
                condition += " and dh.ncc_fk = '" + nccId + "' ";

            DataTable baocao = new DataTable("TraHangVeNCC");

            baocao.Columns.Add("MaNCC", typeof(string));
            baocao.Columns.Add("TenNCC", typeof(string));
            baocao.Columns.Add("SoChungTu", typeof(string));
            baocao.Columns.Add("NgayNhap", typeof(string));

            baocao.Columns.Add("NganhHang", typeof(string));
            baocao.Columns.Add("NhanHang", typeof(string));
            baocao.Columns.Add("ChungLoai", typeof(string));

            baocao.Columns.Add("MaSanPham", typeof(string));
            baocao.Columns.Add("TenSanPham", typeof(string));
            baocao.Columns.Add("DonVi", typeof(string));

            baocao.Columns.Add("SoLuong", typeof(double));
            baocao.Columns.Add("DonGia", typeof(double));
            baocao.Columns.Add("ThanhTien", typeof(double));

            string sql = "SELECT dh.pk_seq sodonhang, dh.ngaytra, ncc.ma as nccMa, ncc.ten as nccTEN,  " +
                         "		ISNULL(nh.ten, '') as nganhHANG, ISNULL(nhan.ten, '') as nhanhang, ISNULL(cl.ten, '') as chungloai, sp.ma as spMA, sp.ten as spTEN, dvt.ten as donvi,     " +
                         "		dh_sp.soluong as soluong, dh_sp.giatra as dongia, dh_sp.soluong * dh_sp.giatra as thanhtien " +
                         "FROM TraHang dh INNER JOIN TraHang_SanPham dh_sp on dh.pk_seq = dh_sp.trahang_fk    " +
                         "	INNER JOIN SanPham sp on dh_sp.sanpham_fk = sp.pk_seq       " +
                         "	INNER JOIN NGANHHANG nh on sp.NGANHHANG_FK = nh.pk_seq           " +
                         "	INNER JOIN NhaCungCap ncc on dh.ncc_fk = ncc.pk_seq       " +
                         "	INNER JOIN DONVITINH dvt on dh_sp.dvt_fk = dvt.pk_seq  " +
                         "	LEFT JOIN NHANHANG nhan on sp.NHANHANG_FK = nhan.pk_seq " +
                         "	LEFT JOIN CHUNGLOAI cl on sp.CHUNGLOAI_FK = cl.pk_seq " +
                         "WHERE dh.trangthai = 1  " + condition;

            DataTable dt = xl.ReadTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string maKH = dt.Rows[i]["nccMa"].ToString();
                string tenKH = dt.Rows[i]["nccTEN"].ToString();

                string sodonhang = dt.Rows[i]["sodonhang"].ToString();
                string ngaydonhang = dt.Rows[i]["ngaytra"].ToString();

                string nganhHANG = dt.Rows[i]["nganhHANG"].ToString();
                string nhanHANG = dt.Rows[i]["nhanhang"].ToString();
                string chungLOAI = dt.Rows[i]["chungloai"].ToString();
                string spMA = dt.Rows[i]["spMA"].ToString();
                string spTEN = dt.Rows[i]["spTEN"].ToString();
                string donvi = dt.Rows[i]["donvi"].ToString();

                double soluong = double.Parse(dt.Rows[i]["soluong"].ToString());
                double dongia = double.Parse(dt.Rows[i]["dongia"].ToString());
                double thanhtien = double.Parse(dt.Rows[i]["thanhtien"].ToString());

                DataRow dr = baocao.NewRow();

                dr[0] = maKH;
                dr[1] = tenKH;

                dr[2] = sodonhang;
                dr[3] = ngaydonhang;

                dr[4] = nganhHANG;
                dr[5] = nhanHANG;
                dr[6] = chungLOAI;

                dr[7] = spMA;
                dr[8] = spTEN;
                dr[9] = donvi;

                dr[10] = soluong;
                dr[11] = dongia;
                dr[12] = thanhtien;

                baocao.Rows.Add(dr);
            }

            string fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\DoanhSoNhapHang.xlsm";

            Workbook workbook = new Workbook(fileName);
            Worksheet worksheet = workbook.Worksheets[0];


            worksheet.Cells.ImportDataTable(baocao, true, "S1");
            worksheet.AutoFitColumns();

            worksheet.Cells["A1"].PutValue("Báo cáo trả hàng về NCC ");

            //worksheet.Cells["A3"].PutValue("Thời gian tạo ");
            //worksheet.Cells["B3"].PutValue(tungay + " đến " + denngay);

            worksheet.Cells["A2"].PutValue("NgàyTạo");
            worksheet.Cells["B2"].PutValue(DateTime.Now.ToString("dd-MM-yyyy"));

            worksheet.Cells["A3"].PutValue("Người Tạo");
            worksheet.Cells["B3"].PutValue(context.Session["userName"].ToString());

            HttpResponse response = context.Response;
            workbook.Save(response, "TraHangVeNCC.xlsm", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsm));

            return _exportContent;
        }
        
        private string ExportToExcel_StockIn(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

            string nhacungcap = "";
            if (context.Request.QueryString["nhacungcap"] != null)
                nhacungcap = context.Request.QueryString["nhacungcap"].ToString();

            string nganhhang = "";
            if (context.Request.QueryString["nganhhang"] != null)
                nganhhang = context.Request.QueryString["nganhhang"].ToString();

            string nhanhang = "";
            if (context.Request.QueryString["nhanhang"] != null)
                nhanhang = context.Request.QueryString["nhanhang"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();

            string line = "";
            if (context.Request.QueryString["line"] != null)
                line = context.Request.QueryString["line"].ToString();

            string layDACHOT = "0";
            if (context.Request.QueryString["layDACHOT"] != null)
                layDACHOT = context.Request.QueryString["layDACHOT"].ToString();

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

                    //Tieu de
                    TableHeaderCell header = new TableHeaderCell();
                    header.RowSpan = 1;
                    header.ColumnSpan = 9;
                    header.Text = "REPORT STOCK IN - TOTAL";
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
                        header.Text = "Datetime: FROM " + tungay + " to " + denngay;

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
                    header.Text = "Created date:  " + ngaythang;
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

                    string[] tieude = new string[] { "No", "Date", "ID", "Types", "Customer", "Note",
                        "Model", "Grade", "Qty"};

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

                    string condition = "";
                    string conditionCL = "";

                    if (tungay.Trim().Length > 0)
                        condition += " and convert( datetime, dh.ngaynhap, 105) >= convert( datetime, '" + tungay + "', 105) ";
                    if (denngay.Trim().Length > 0)
                        condition += " and convert( datetime, dh.ngaynhap, 105) <= convert( datetime, '" + denngay + "', 105) ";
                    if (nhacungcap.Trim().Length > 3)
                        condition += " and dh.ncc_fk = '" + nhacungcap + "' ";
                    if (kho.Trim().Length > 3)
                        condition += " and dh.kho_fk = '" + kho + "' ";
                    if (khachhang.Trim().Length > 3)
                        condition += " and dh.khachhang_fk = '" + khachhang + "' ";
                    if (line.Trim().Length > 3)
                        condition += " and dh_sp.line_fk = '" + line + "' ";

                    if (chungloai.Trim().Length > 3)
                        conditionCL += " and sp.chungloai_fk = '" + chungloai + "' ";

                    string sql = "SELECT dh.pk_seq AS nhaphang_fk, ISNULL(dh.sohopdong, '') sohopdong, dh.ngaynhap, k.makho, ISNULL(dh.ghichu, '') ghichu, " +
                        "  CASE dh.loainhap WHEN 0 THEN N'Unknow' " +
                        "                   WHEN 1 THEN N'Into storage' " +
                        "                   WHEN 2 THEN N'Temporary location' ELSE 'Unknnow' END loainhap, " +
                        "	ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') AS chungloai, " +
                        "	ISNULL((SELECT hoten FROM KhachHang WHERE pk_seq = dh.khachhang_fk), '') AS khachhang, " +
                        "   sp.ma AS masp, sp.ten AS tensp, SUM(dh_sp.soluong) AS soluong " +
                        "FROM NhapHang dh INNER JOIN NhapHang_SanPham_ChiTiet dh_sp on dh.pk_seq = dh_sp.nhaphang_fk    " + condition +
                        "  INNER JOIN Kho k ON dh.kho_fk = k.pk_seq " +
                        "   INNER JOIN SanPham sp on dh_sp.sanpham_fk = sp.pk_seq " + conditionCL +
                        "WHERE dh.pk_seq > 0 AND dh.trangthai in (1) " +
                        " GROUP BY dh.pk_seq, dh.ngaynhap, dh.sohopdong, dh.khachhang_fk, k.makho, dh.ghichu, dh.loainhap, sp.chungloai_fk, sp.ma, sp.ten, dh_sp.dvt_fk " +
                        " ORDER BY k.makho, CONVERT(datetime, dh.ngaynhap, 105), sp.ten ";

                    DataTable dt = xl.ReadTable(sql);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();
                        string nhaphang_fk = dt.Rows[i]["nhaphang_fk"].ToString();
                        //string makho = dt.Rows[i]["makho"].ToString();
                        string ghichu = dt.Rows[i]["ghichu"].ToString();
                        khachhang = dt.Rows[i]["khachhang"].ToString();
                        string loainhap = dt.Rows[i]["loainhap"].ToString();

                        string sohopdong = dt.Rows[i]["sohopdong"].ToString();
                        string ngaynhap = dt.Rows[i]["ngaynhap"].ToString();

                        chungloai = dt.Rows[i]["chungloai"].ToString();
                        string masp = dt.Rows[i]["masp"].ToString();
                        string tensp = dt.Rows[i]["tensp"].ToString();

                        double soluong = double.Parse(dt.Rows[i]["soluong"].ToString());

                        string[] data = new string[] { (i + 1).ToString(), ngaynhap, nhaphang_fk, loainhap,
                            khachhang, ghichu, tensp, chungloai, FormatString.ForMatNumber(soluong.ToString()) };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            cell.VerticalAlign = VerticalAlign.Middle;

                            if (j == 0)
                                cell.Width = Unit.Parse("60");
                            else if (j == 2 || j == 6)
                                cell.Width = Unit.Parse("150");
                            else if (j == 3 || j == 4 || j == 5)
                                cell.Width = Unit.Parse("350");
                            else
                                cell.Width = Unit.Parse("100");

                            if(j == 0 || j == 1 || j == 2)
                                cell.HorizontalAlign = HorizontalAlign.Center;
                            if (j > 7)
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

        private string ExportToExcel_StockIn_New(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

            string nhacungcap = "";
            if (context.Request.QueryString["nhacungcap"] != null)
                nhacungcap = context.Request.QueryString["nhacungcap"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();

            string line = "";
            if (context.Request.QueryString["line"] != null)
                line = context.Request.QueryString["line"].ToString();

            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();
            string _exportContent = "";

            DataTable baocao = new DataTable("ReportStockIn_Total");

            baocao.Columns.Add("Date", typeof(string));
            baocao.Columns.Add("ID", typeof(string));
            baocao.Columns.Add("Customer", typeof(string));
            baocao.Columns.Add("Option", typeof(string));
            baocao.Columns.Add("Note", typeof(string));

            baocao.Columns.Add("InvoiceNo.", typeof(string));
            baocao.Columns.Add("ItemJP", typeof(string));
            baocao.Columns.Add("ItemVN", typeof(string));

            baocao.Columns.Add("Quantity", typeof(double));

            ExecuteData xl = new ExecuteData();

            string condition = "";
            string conditionCL = "";

            if (tungay.Trim().Length > 0)
                condition += " and convert( datetime, dh.ngaynhap, 105) >= convert( datetime, '" + tungay + "', 105) ";
            if (denngay.Trim().Length > 0)
                condition += " and convert( datetime, dh.ngaynhap, 105) <= convert( datetime, '" + denngay + "', 105) ";
            if (nhacungcap.Trim().Length > 3)
                condition += " and dh.ncc_fk = '" + nhacungcap + "' ";
            if (kho.Trim().Length > 3)
                condition += " and dh.kho_fk = '" + kho + "' ";
            if (khachhang.Trim().Length > 3)
                condition += " and dh.khachhang_fk = '" + khachhang + "' ";
            if (line.Trim().Length > 3)
                condition += " and dh_sp.line_fk = '" + line + "' ";

            if (chungloai.Trim().Length > 3)
                conditionCL += " and sp.chungloai_fk = '" + chungloai + "' ";

            string sql = "SELECT dh.pk_seq AS nhaphang_fk, ISNULL(dh.sohopdong, '') sohopdong, dh.ngaynhap, k.makho, ISNULL(dh.ghichu, '') ghichu, " +
                "  CASE dh.loainhap WHEN 0 THEN N'Unknow' " +
                "                   WHEN 1 THEN N'Into storage' " +
                "                   WHEN 2 THEN N'Temporary location' ELSE 'Unknnow' END loainhap, " +
                "	ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') AS chungloai, " +
                "	ISNULL((SELECT hoten FROM KhachHang WHERE pk_seq = dh.khachhang_fk), '') AS khachhang, " +
                "   sp.ma AS masp, sp.codeEnglish, dh_sp.solo, sp.ten AS tensp, SUM(dh_sp.soluong) AS soluong " +
                "FROM NhapHang dh INNER JOIN NhapHang_SanPham_ChiTiet dh_sp on dh.pk_seq = dh_sp.nhaphang_fk    " + condition +
                "  INNER JOIN Kho k ON dh.kho_fk = k.pk_seq " +
                "   INNER JOIN SanPham sp on dh_sp.sanpham_fk = sp.pk_seq " + conditionCL +
                "WHERE dh.pk_seq > 0 AND dh.trangthai in (1) " +
                " GROUP BY dh.pk_seq, dh.ngaynhap, dh.sohopdong, dh.khachhang_fk, dh_sp.solo, k.makho, dh.ghichu, dh.loainhap, sp.chungloai_fk, sp.ma, sp.codeEnglish, sp.ten, dh_sp.dvt_fk " +
                " ORDER BY k.makho, CONVERT(datetime, dh.ngaynhap, 105), sp.ten ";

            DataTable dt = xl.ReadTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string nhaphang_fk = dt.Rows[i]["nhaphang_fk"].ToString();
                //string makho = dt.Rows[i]["makho"].ToString();
                string ghichu = dt.Rows[i]["ghichu"].ToString();
                khachhang = dt.Rows[i]["khachhang"].ToString();
                string loainhap = dt.Rows[i]["loainhap"].ToString();
                string itemJP = dt.Rows[i]["codeEnglish"].ToString();
                string itemVN = dt.Rows[i]["masp"].ToString();
                string ngaynhap = dt.Rows[i]["ngaynhap"].ToString();
                string solo = dt.Rows[i]["solo"].ToString();

                double soluong = double.Parse(dt.Rows[i]["soluong"].ToString());
                DataRow dr = baocao.NewRow();

                dr[0] = ngaynhap;
                dr[1] = nhaphang_fk;
                dr[2] = khachhang;
                dr[3] = loainhap;
                dr[4] = ghichu;
                dr[5] = solo;
                dr[6] = itemJP;
                dr[7] = itemVN;                
                dr[8] = FormatString.ForMatNumber(soluong.ToString());

                baocao.Rows.Add(dr);
            }

            string fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\ReportStockIn_Total.xlsm";

            Workbook workbook = new Workbook(fileName);
            Worksheet worksheet = workbook.Worksheets[0];

            worksheet.Cells.ImportDataTable(baocao, true, "A5");
            worksheet.AutoFitColumns();

            worksheet.Cells["A1"].PutValue("REPORT STOCK IN - TOTAL");

            worksheet.Cells["A2"].PutValue("Created date");
            worksheet.Cells["C2"].PutValue(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));

            worksheet.Cells["A3"].PutValue("Username");
            worksheet.Cells["C3"].PutValue(context.Session["userName"].ToString());

            worksheet.Cells["A4"].PutValue("From date");
            worksheet.Cells["C4"].PutValue(tungay + " to " + denngay);


            HttpResponse response = context.Response;
            workbook.Save(response, "ReportStockIn_Total.xlsm", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsm));

            return _exportContent;
        }

        private string ExportToExcel_StockIn_Color(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

            string nhacungcap = "";
            if (context.Request.QueryString["nhacungcap"] != null)
                nhacungcap = context.Request.QueryString["nhacungcap"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();

            string line = "";
            if (context.Request.QueryString["line"] != null)
                line = context.Request.QueryString["line"].ToString();

            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();
            string _exportContent = "";

            DataTable baocao = new DataTable("ReportStockIn_Color");

            baocao.Columns.Add("Date", typeof(string));
            baocao.Columns.Add("ID", typeof(string));
            baocao.Columns.Add("Customer", typeof(string));
            baocao.Columns.Add("Option", typeof(string));
            baocao.Columns.Add("Note", typeof(string));
            baocao.Columns.Add("InvoiceNo.", typeof(string));
            baocao.Columns.Add("ItemJP", typeof(string));
            baocao.Columns.Add("ItemVN", typeof(string));
            baocao.Columns.Add("Color", typeof(string));
            baocao.Columns.Add("Quantity", typeof(double));

            ExecuteData xl = new ExecuteData();

            string condition = "";
            string conditionCL = "";

            if (tungay.Trim().Length > 0)
                condition += " and convert( datetime, dh.ngaynhap, 105) >= convert( datetime, '" + tungay + "', 105) ";
            if (denngay.Trim().Length > 0)
                condition += " and convert( datetime, dh.ngaynhap, 105) <= convert( datetime, '" + denngay + "', 105) ";
            if (nhacungcap.Trim().Length > 3)
                condition += " and dh.ncc_fk = '" + nhacungcap + "' ";
            if (kho.Trim().Length > 3)
                condition += " and dh.kho_fk = '" + kho + "' ";
            if (khachhang.Trim().Length > 3)
                condition += " and dh.khachhang_fk = '" + khachhang + "' ";
            if (line.Trim().Length > 3)
                condition += " and dh_sp.line_fk = '" + line + "' ";

            if (chungloai.Trim().Length > 3)
                conditionCL += " and sp.chungloai_fk = '" + chungloai + "' ";

            string sql = "SELECT dh.pk_seq AS nhaphang_fk, ISNULL(dh.sohopdong, '') sohopdong, dh.ngaynhap, k.makho, ISNULL(dh.ghichu, '') ghichu, " +
                "  CASE dh.loainhap WHEN 0 THEN N'Unknow' " +
                "                   WHEN 1 THEN N'Into storage' " +
                "                   WHEN 2 THEN N'Temporary location' ELSE 'Unknnow' END loainhap, " +
                "	ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') AS chungloai, " +
                "	ISNULL((SELECT ten FROM MauSac WHERE pk_seq = dh_sp.mausac_fk), '') AS mausac, " +
                "	ISNULL((SELECT hoten FROM KhachHang WHERE pk_seq = dh.khachhang_fk), '') AS khachhang, " +
                "   sp.ma AS masp, sp.codeEnglish, sp.ten AS tensp, dh_sp.solo, SUM(dh_sp.soluong) AS soluong " +
                "FROM NhapHang dh INNER JOIN NhapHang_SanPham_ChiTiet dh_sp on dh.pk_seq = dh_sp.nhaphang_fk    " + condition +
                "  INNER JOIN Kho k ON dh.kho_fk = k.pk_seq " +
                "   INNER JOIN SanPham sp on dh_sp.sanpham_fk = sp.pk_seq " + conditionCL +
                "WHERE dh.pk_seq > 0 AND dh.trangthai in (1) " +
                " GROUP BY dh.pk_seq, dh.ngaynhap, dh.sohopdong, dh.khachhang_fk, dh_sp.solo, k.makho, dh.ghichu, dh.loainhap, sp.chungloai_fk, sp.ma, sp.codeEnglish, sp.ten, dh_sp.mausac_fk " +
                " ORDER BY k.makho, CONVERT(datetime, dh.ngaynhap, 105), sp.ten ";

            DataTable dt = xl.ReadTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string nhaphang_fk = dt.Rows[i]["nhaphang_fk"].ToString();
                //string makho = dt.Rows[i]["makho"].ToString();
                string ghichu = dt.Rows[i]["ghichu"].ToString();
                khachhang = dt.Rows[i]["khachhang"].ToString();
                string loainhap = dt.Rows[i]["loainhap"].ToString();
                string itemJP = dt.Rows[i]["codeEnglish"].ToString();
                string itemVN = dt.Rows[i]["masp"].ToString();
                string ngaynhap = dt.Rows[i]["ngaynhap"].ToString();
                string solo = dt.Rows[i]["solo"].ToString();
                string mausac = dt.Rows[i]["mausac"].ToString();

                double soluong = double.Parse(dt.Rows[i]["soluong"].ToString());
                DataRow dr = baocao.NewRow();


                dr[0] = ngaynhap;
                dr[1] = nhaphang_fk;
                dr[2] = khachhang;
                dr[3] = loainhap;
                dr[4] = ghichu;
                dr[5] = solo;
                dr[6] = itemJP;
                dr[7] = itemVN;                
                dr[8] = mausac;
                dr[9] = FormatString.ForMatNumber(soluong.ToString());

                baocao.Rows.Add(dr);
            }

            string fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\ReportStockIn_Color.xlsm";

            Workbook workbook = new Workbook(fileName);
            Worksheet worksheet = workbook.Worksheets[0];

            worksheet.Cells.ImportDataTable(baocao, true, "A5");
            worksheet.AutoFitColumns();

            worksheet.Cells["A1"].PutValue("REPORT STOCK IN - TOTAL");

            worksheet.Cells["A2"].PutValue("Created date");
            worksheet.Cells["C2"].PutValue(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));

            worksheet.Cells["A3"].PutValue("Username");
            worksheet.Cells["C3"].PutValue(context.Session["userName"].ToString());

            worksheet.Cells["A4"].PutValue("From date");
            worksheet.Cells["C4"].PutValue(tungay + " to " + denngay);

            HttpResponse response = context.Response;
            workbook.Save(response, "ReportStockIn_Color.xlsm", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsm));

            return _exportContent;
        }

        private string ExportToExcel_StockIn_Line(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

            string nhacungcap = "";
            if (context.Request.QueryString["nhacungcap"] != null)
                nhacungcap = context.Request.QueryString["nhacungcap"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();

            string line = "";
            if (context.Request.QueryString["line"] != null)
                line = context.Request.QueryString["line"].ToString();

            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();
            string _exportContent = "";

            DataTable baocao = new DataTable("ReportStockIn_Line");

            baocao.Columns.Add("Date", typeof(string));
            baocao.Columns.Add("ID", typeof(string));
            baocao.Columns.Add("Customer", typeof(string));
            baocao.Columns.Add("Option", typeof(string));
            baocao.Columns.Add("Note", typeof(string));

            baocao.Columns.Add("Model", typeof(string));
            baocao.Columns.Add("Grade", typeof(string));
            baocao.Columns.Add("Line", typeof(string));
            baocao.Columns.Add("Quantity", typeof(double));

            ExecuteData xl = new ExecuteData();

            string condition = "";
            string conditionCL = "";

            if (tungay.Trim().Length > 0)
                condition += " and convert( datetime, dh.ngaynhap, 105) >= convert( datetime, '" + tungay + "', 105) ";
            if (denngay.Trim().Length > 0)
                condition += " and convert( datetime, dh.ngaynhap, 105) <= convert( datetime, '" + denngay + "', 105) ";
            if (nhacungcap.Trim().Length > 3)
                condition += " and dh.ncc_fk = '" + nhacungcap + "' ";
            if (kho.Trim().Length > 3)
                condition += " and dh.kho_fk = '" + kho + "' ";
            if (khachhang.Trim().Length > 3)
                condition += " and dh.khachhang_fk = '" + khachhang + "' ";
            if (line.Trim().Length > 3)
                condition += " and dh_sp.line_fk = '" + line + "' ";

            if (chungloai.Trim().Length > 3)
                conditionCL += " and sp.chungloai_fk = '" + chungloai + "' ";

            string sql = "SELECT dh.pk_seq AS nhaphang_fk, ISNULL(dh.sohopdong, '') sohopdong, dh.ngaynhap, k.makho, ISNULL(dh.ghichu, '') ghichu, " +
                "  CASE dh.loainhap WHEN 0 THEN N'Unknow' " +
                "                   WHEN 1 THEN N'Into storage' " +
                "                   WHEN 2 THEN N'Temporary location' ELSE 'Unknnow' END loainhap, " +
                "	ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') AS chungloai, " +
                "	ISNULL((SELECT ten FROM Line WHERE pk_seq = dh_sp.line_fk), '') AS line, " +
                "	ISNULL((SELECT hoten FROM KhachHang WHERE pk_seq = dh.khachhang_fk), '') AS khachhang, " +
                "   sp.ma AS masp, sp.ten AS tensp, SUM(dh_sp.soluong) AS soluong " +
                "FROM NhapHang dh INNER JOIN NhapHang_SanPham_ChiTiet dh_sp on dh.pk_seq = dh_sp.nhaphang_fk    " + condition +
                "  INNER JOIN Kho k ON dh.kho_fk = k.pk_seq " +
                "   INNER JOIN SanPham sp on dh_sp.sanpham_fk = sp.pk_seq " + conditionCL +
                "WHERE dh.pk_seq > 0 AND dh.trangthai in (1) " +
                " GROUP BY dh.pk_seq, dh.ngaynhap, dh.sohopdong, dh.khachhang_fk, k.makho, dh.ghichu, dh.loainhap, sp.chungloai_fk, sp.ma, sp.ten, dh_sp.line_fk " +
                " ORDER BY k.makho, CONVERT(datetime, dh.ngaynhap, 105), sp.ten ";

            DataTable dt = xl.ReadTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string nhaphang_fk = dt.Rows[i]["nhaphang_fk"].ToString();
                //string makho = dt.Rows[i]["makho"].ToString();
                string ghichu = dt.Rows[i]["ghichu"].ToString();
                khachhang = dt.Rows[i]["khachhang"].ToString();
                string loainhap = dt.Rows[i]["loainhap"].ToString();
                string sohopdong = dt.Rows[i]["sohopdong"].ToString();
                string ngaynhap = dt.Rows[i]["ngaynhap"].ToString();

                chungloai = dt.Rows[i]["chungloai"].ToString();
                string masp = dt.Rows[i]["masp"].ToString();
                string tensp = dt.Rows[i]["tensp"].ToString();
                line = dt.Rows[i]["line"].ToString();

                double soluong = double.Parse(dt.Rows[i]["soluong"].ToString());
                DataRow dr = baocao.NewRow();


                dr[0] = ngaynhap;
                dr[1] = nhaphang_fk;
                dr[2] = khachhang;
                dr[3] = loainhap;
                dr[4] = ghichu;
                dr[5] = tensp;
                dr[6] = chungloai;
                dr[7] = line;
                dr[8] = FormatString.ForMatNumber(soluong.ToString());

                baocao.Rows.Add(dr);
            }

            string fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\ReportStockIn_Line.xlsm";

            Workbook workbook = new Workbook(fileName);
            Worksheet worksheet = workbook.Worksheets[0];

            worksheet.Cells.ImportDataTable(baocao, true, "A5");
            worksheet.AutoFitColumns();

            worksheet.Cells["A1"].PutValue("REPORT STOCK IN - TOTAL");

            worksheet.Cells["A2"].PutValue("Created date");
            worksheet.Cells["C2"].PutValue(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));

            worksheet.Cells["A3"].PutValue("Username");
            worksheet.Cells["C3"].PutValue(context.Session["userName"].ToString());

            worksheet.Cells["A4"].PutValue("From date");
            worksheet.Cells["C4"].PutValue(tungay + " to " + denngay);

            HttpResponse response = context.Response;
            workbook.Save(response, "ReportStockIn_Line.xlsm", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsm));

            return _exportContent;

        }

        private string ExportToExcel_StockIn_Detail(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

            string nhacungcap = "";
            if (context.Request.QueryString["nhacungcap"] != null)
                nhacungcap = context.Request.QueryString["nhacungcap"].ToString();

            string chungloai = "";
            if (context.Request.QueryString["chungloai"] != null)
                chungloai = context.Request.QueryString["chungloai"].ToString();

            string line = "";
            if (context.Request.QueryString["line"] != null)
                line = context.Request.QueryString["line"].ToString();

            string userId = "-1";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string ngaythang = DateTime.Now.ToString();
            string _exportContent = "";

            DataTable baocao = new DataTable("ReportStockIn_Detail");

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
            baocao.Columns.Add("Pallet", typeof(string));

            ExecuteData xl = new ExecuteData();

            string condition = "";
            string conditionCL = "";

            if (tungay.Trim().Length > 0)
                condition += " and convert( datetime, dh.ngaynhap, 105) >= convert( datetime, '" + tungay + "', 105) ";
            if (denngay.Trim().Length > 0)
                condition += " and convert( datetime, dh.ngaynhap, 105) <= convert( datetime, '" + denngay + "', 105) ";
            if (nhacungcap.Trim().Length > 3)
                condition += " and dh.ncc_fk = '" + nhacungcap + "' ";
            if (kho.Trim().Length > 3)
                condition += " and dh.kho_fk = '" + kho + "' ";
            if (khachhang.Trim().Length > 3)
                condition += " and dh.khachhang_fk = '" + khachhang + "' ";
            if (line.Trim().Length > 3)
                condition += " and dh_sp.line_fk = '" + line + "' ";

            if (chungloai.Trim().Length > 3)
                conditionCL += " and sp.chungloai_fk = '" + chungloai + "' ";

           string sql = "SELECT dh.ngaynhap, sp.ma AS masp, sp.nameEnglish, sp.ten AS tensp, dh_sp.utcOrderNo, dh_sp.PLNo, dh_sp.lotNo, dh_sp.cartonNo, dh_sp.NoOf, dh_sp.solo, dh_sp.netWeight, dh_sp.grossWeight, " +
                 "   dh_sp.planShipping, dh_sp.yards, dh_sp.roll, dh_sp.soluong, dh_sp.soluong1, dh_sp.soluong2, dh_sp.soluong3, dh_sp.mavach, " +
                 "   ISNULL((SELECT ten FROM MauSac WHERE pk_seq = dh_sp.mausac_fk), '') color, " +
                 "   ISNULL((SELECT ten FROM Location WHERE pk_seq = dh_sp.location_fk), '') location, " +
                 "   ISNULL((SELECT ten FROM Pallet WHERE pk_seq = dh_sp.pallet_fk), '') pallet " +
                 " FROM NhapHang dh INNER JOIN NhapHang_SanPham_ChiTiet dh_sp ON dh.pk_seq = dh_sp.nhaphang_fk " + condition + 
                 " INNER JOIN SanPham sp on dh_sp.sanpham_fk = sp.pk_seq  " + conditionCL + 
                 " WHERE dh_sp.pk_seq > 0 " +
                 " ORDER BY CONVERT(datetime, dh.ngaynhap, 105), dh_sp.solo, dh_sp.stt ";

            DataTable dt = xl.ReadTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string ngaynhap = dt.Rows[i]["ngaynhap"].ToString();
                string solo = dt.Rows[i]["solo"].ToString();
                string itemVN = dt.Rows[i]["masp"].ToString();
                string itemJP = dt.Rows[i]["nameEnglish"].ToString();
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
                string location = dt.Rows[i]["location"].ToString();
                string pallet = dt.Rows[i]["pallet"].ToString();


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
                dr[26] = pallet;

                baocao.Rows.Add(dr);
            }

            string fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\ReportStockIn_Detail.xlsm";

            Workbook workbook = new Workbook(fileName);
            Worksheet worksheet = workbook.Worksheets[0];

            worksheet.Cells.ImportDataTable(baocao, true, "A5");
            worksheet.AutoFitColumns();

            worksheet.Cells["A1"].PutValue("REPORT STOCK IN - DETAIL");

            worksheet.Cells["A2"].PutValue("Created date");
            worksheet.Cells["C2"].PutValue(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));

            worksheet.Cells["A3"].PutValue("Username");
            worksheet.Cells["C3"].PutValue(context.Session["userName"].ToString());

            worksheet.Cells["A4"].PutValue("From date");
            worksheet.Cells["C4"].PutValue(tungay + " to " + denngay);

            HttpResponse response = context.Response;
            workbook.Save(response, "ReportStockIn_Detail.xlsm", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsm));

            return _exportContent;
        }

        private string ExportToExcel_NhapPartner_Total(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();
            
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

            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

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
                    header.Text = "REPORT STOCK IN PARTNER - TOTAL";
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
                        header.Text = "Datetime: FROM " + tungay + " to " + denngay;

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
                    header.Text = "Created date:  " + ngaythang;
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
                    
                    string[] tieude = new string[] { "No", "Partner", "Specification", "Date", "ID", "Datetime", "Warehouse",
                        "PartCode", "Description", "Unit", "Qty"};

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

                    string condition = "";
                    if (tungay.Trim().Length > 0)
                        condition += " and convert( datetime, dh.ngaynhap, 105) >= convert( datetime, '" + tungay + "', 105) ";
                    if (denngay.Trim().Length > 0)
                        condition += " and convert( datetime, dh.ngaynhap, 105) <= convert( datetime, '" + denngay + "', 105) ";
                    if (khachhang.Trim().Length > 3)
                        condition += " and dh.khachhang_fk = '" + khachhang + "' ";
                    if (kho.Trim().Length > 3)
                        condition += " and dh.khochuyen_fk = '" + kho + "' ";

                    string sql = "SELECT dh.pk_seq sodonhang, dh.ngaynhap, ISNULL(kh.ma, '') as makh, ISNULL(kh.hoten, '') as khachhang, k.makho, " +
                         "  REPLACE(CONVERT(varchar(11), dh.thoigianCHOT, 113), ' ', '-') AS thoigianCHOT, SUBSTRING(CONVERT(nvarchar(10), dh.thoigianCHOT, 105), 1, 2) AS ngay, SUBSTRING(CONVERT(nvarchar(10), dh.thoigianCHOT, 105), 4, 2) AS thang, " +
                         "	ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') as chungloai, " +
                         "  sp.ma as masp, sp.ten as tensp, (SELECT ten FROM DonViTinh WHERE pk_seq = dh_sp.dvt_fk) as donvi,     " +
                         "	SUM(dh_sp.soluong) as soluong, dh_sp.dongia, SUM(dh_sp.soluong * dh_sp.dongia) as thanhtien " +
                         "FROM NhapKhac dh INNER JOIN NhapKhac_SanPham dh_sp on dh.pk_seq = dh_sp.nhapkhac_fk    " + condition +
                         "  LEFT JOIN Kho k ON dh.khochuyen_fk = k.pk_seq " +
                         "	INNER JOIN SanPham sp on dh_sp.sanpham_fk = sp.pk_seq " +
                         "	LEFT JOIN KhachHang kh on dh.khachhang_fk = kh.pk_seq " +
                         "WHERE dh.pk_seq > 0 AND dh.trangthai in (1) " + condition +
                         " GROUP BY dh.pk_seq, dh.ngaynhap, kh.ma, kh.hoten, k.makho, dh.thoigianCHOT, sp.chungloai_fk, sp.ma, sp.ten, dh_sp.dvt_fk, dh_sp.dongia " +
                         " ORDER BY kh.ma, CONVERT(datetime, dh.ngaynhap, 105), sp.ma, k.makho ";

                    DataTable dt = xl.ReadTable(sql);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string makho = dt.Rows[i]["makho"].ToString();
                        string makh = dt.Rows[i]["makh"].ToString();
                        khachhang = dt.Rows[i]["khachhang"].ToString();

                        string sodonhang = dt.Rows[i]["sodonhang"].ToString();
                        string ngaynhap = dt.Rows[i]["ngaynhap"].ToString();
                        string thoigianChot = dt.Rows[i]["thoigianChot"].ToString();
                        string ngay = dt.Rows[i]["ngay"].ToString();
                        string thang = dt.Rows[i]["thang"].ToString();

                        chungloai = dt.Rows[i]["chungloai"].ToString();
                        string masp = dt.Rows[i]["masp"].ToString();
                        string tensp = dt.Rows[i]["tensp"].ToString();
                        string donvi = dt.Rows[i]["donvi"].ToString();

                        double soluong = double.Parse(dt.Rows[i]["soluong"].ToString());

                        double dongia = double.Parse(dt.Rows[i]["dongia"].ToString());
                        double thanhtien = double.Parse(dt.Rows[i]["thanhtien"].ToString());

                        string[] data = new string[] { (i + 1).ToString(), makh, chungloai, ngaynhap, sodonhang, thoigianChot, makho,
                                                    masp, tensp, donvi, FormatString.ForMatNumber(soluong.ToString()) };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            cell.VerticalAlign = VerticalAlign.Middle;

                            if (j == 0)
                                cell.Width = Unit.Parse("60");
                            else if (j == 1 || j == 2 || j == 7)
                                cell.Width = Unit.Parse("150");
                            else if (j == 8)
                                cell.Width = Unit.Parse("350");
                            else
                                cell.Width = Unit.Parse("100");

                            if (j < 8)
                                cell.HorizontalAlign = HorizontalAlign.Center;
                            if (j >= 10)
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

        private string ExportToExcel_NhapPartner_LotNo(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

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
                    header.Text = "REPORT STOCK IN PARTNER - LOTNO ";
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
                        header.Text = "Datetime: FROM " + tungay + " to " + denngay;

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
                    header.Text = "Created date:  " + ngaythang;
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

                    // SUBSTRING(CONVERT(nvarchar(10), dh.thoigianCHOT, 105), 1, 2) AS ngay, SUBSTRING(CONVERT(nvarchar(10), dh.thoigianCHOT, 105), 4, 2) AS thang,

                    string[] tieude = new string[] { "No", "Partner", "Specification", "Date", "ID", "Datetime", "Warehouse",
                        "PartCode", "Description", "Unit", "LotNo", "Qty"};

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

                    string condition = "";
                    if (tungay.Trim().Length > 0)
                        condition += " and convert( datetime, dh.ngaynhap, 105) >= convert( datetime, '" + tungay + "', 105) ";
                    if (denngay.Trim().Length > 0)
                        condition += " and convert( datetime, dh.ngaynhap, 105) <= convert( datetime, '" + denngay + "', 105) ";
                    if (khachhang.Trim().Length > 3)
                        condition += " and dh.khachhang_fk = '" + khachhang + "' ";
                    if (kho.Trim().Length > 3)
                        condition += " and dh.khochuyen_fk = '" + kho + "' ";

                    string sql = "SELECT dh.pk_seq sodonhang, dh.ngaynhap, ISNULL(kh.ma, '') as makh, ISNULL(kh.hoten, '') as khachhang, k.makho, " +
                         "  REPLACE(CONVERT(varchar(11), dh.thoigianCHOT, 113), ' ', '-') AS thoigianCHOT, SUBSTRING(CONVERT(nvarchar(10), dh.thoigianCHOT, 105), 1, 2) AS ngay, SUBSTRING(CONVERT(nvarchar(10), dh.thoigianCHOT, 105), 4, 2) AS thang, " +
                         "	ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') chungloai, " +
                         "  sp.ma as masp, sp.ten as tensp, (SELECT ten FROM DonViTinh WHERE pk_seq = sp.dvt_fk) as donvi, dh_sp.lotNo, " +
                         "	SUM(dh_sp.soluong) as soluong, dh_sp.dongia, SUM(dh_sp.soluong * dh_sp.dongia) as thanhtien " +
                         "FROM NhapKhac dh INNER JOIN NhapKhac_SanPham_ChiTiet dh_sp on dh.pk_seq = dh_sp.nhapkhac_fk    " + condition +
                         "	INNER JOIN SanPham sp on dh_sp.sanpham_fk = sp.pk_seq " +
                         "  LEFT JOIN Kho k ON dh.khochuyen_fk = k.pk_seq " +
                         "	LEFT JOIN KhachHang kh on dh.khachhang_fk = kh.pk_seq " +
                         "WHERE dh.pk_seq > 0 AND dh.trangthai in (1) " + condition +
                         " GROUP BY dh.pk_seq, dh.ngaynhap, kh.ma,  kh.hoten, k.makho, dh.thoigianCHOT, sp.chungloai_fk, sp.ma, sp.ten, sp.dvt_fk, dh_sp.dongia, dh_sp.lotNo " +
                         " ORDER BY kh.ma, CONVERT(datetime, dh.ngaynhap, 105), k.makho, sp.ma ";

                    DataTable dt = xl.ReadTable(sql);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string makho = dt.Rows[i]["makho"].ToString();
                        string makh = dt.Rows[i]["makh"].ToString();
                        khachhang = dt.Rows[i]["khachhang"].ToString();

                        string sodonhang = dt.Rows[i]["sodonhang"].ToString();
                        string ngaynhap = dt.Rows[i]["ngaynhap"].ToString();
                        string thoigianChot = dt.Rows[i]["thoigianChot"].ToString();
                        string ngay = dt.Rows[i]["ngay"].ToString();
                        string thang = dt.Rows[i]["thang"].ToString();

                        chungloai = dt.Rows[i]["chungloai"].ToString();
                        string masp = dt.Rows[i]["masp"].ToString();
                        string tensp = dt.Rows[i]["tensp"].ToString();
                        string donvi = dt.Rows[i]["donvi"].ToString();
                        //string hansudung = dt.Rows[i]["solo"].ToString();
                        string lotNo = dt.Rows[i]["lotNo"].ToString();
                        //string serialNo = dt.Rows[i]["serialNo"].ToString();

                        double soluong = double.Parse(dt.Rows[i]["soluong"].ToString());

                        double dongia = double.Parse(dt.Rows[i]["dongia"].ToString());
                        double thanhtien = double.Parse(dt.Rows[i]["thanhtien"].ToString());

                        string[] data = new string[] { (i + 1).ToString(), makh, chungloai, ngaynhap, sodonhang, thoigianChot, makho,
                                                    masp, tensp, donvi, lotNo, FormatString.ForMatNumber(soluong.ToString())};

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            cell.VerticalAlign = VerticalAlign.Middle;

                            if (j == 0)
                                cell.Width = Unit.Parse("60");
                            else if (j == 1 || j == 2 || j == 7)
                                cell.Width = Unit.Parse("150");
                            else if (j == 8)
                                cell.Width = Unit.Parse("350");
                            else
                                cell.Width = Unit.Parse("100");

                            if (j < 8)
                                cell.HorizontalAlign = HorizontalAlign.Center;
                            if (j >= 11)
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

        private string ExportToExcel_NhapPartner_ChiTiet(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            string khachhang = "";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

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
                    header.ColumnSpan = 14;
                    header.Text = "REPORT STOCK IN PARTNER - DETAIL ";
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
                        header.Text = "Datetime: FROM " + tungay + " to " + denngay;

                    header.Font.Bold = true;
                    header.ColumnSpan = 14;
                    //header.BackColor = Color.LightGray;
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    //Tieu de
                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.Text = "Created date:  " + ngaythang;
                    header.Font.Bold = true;
                    header.ColumnSpan = 14;
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

                    // SUBSTRING(CONVERT(nvarchar(10), dh.thoigianCHOT, 105), 1, 2) AS ngay, SUBSTRING(CONVERT(nvarchar(10), dh.thoigianCHOT, 105), 4, 2) AS thang,

                    string[] tieude = new string[] { "No", "Partner", "Specification", "Date", "ID", "Datetime", "Warehouse",
                        "PartCode", "Descripton", "Unit", "LotNo", "SerialNo", "ProductDate", "Qty"};

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

                    string condition = "";
                    if (tungay.Trim().Length > 0)
                        condition += " and convert( datetime, dh.ngaynhap, 105) >= convert( datetime, '" + tungay + "', 105) ";
                    if (denngay.Trim().Length > 0)
                        condition += " and convert( datetime, dh.ngaynhap, 105) <= convert( datetime, '" + denngay + "', 105) ";
                    if (khachhang.Trim().Length > 3)
                        condition += " and dh.khachhang_fk = '" + khachhang + "' ";
                    if (kho.Trim().Length > 3)
                        condition += " and dh.khochuyen_fk = '" + kho + "' ";

                    string sql = "SELECT dh.pk_seq sodonhang, dh.ngaynhap, ISNULL(kh.ma, '') as makh, ISNULL(kh.hoten, '') as khachhang, k.makho, " +
                         "  REPLACE(CONVERT(varchar(11), dh.thoigianCHOT, 113), ' ', '-') AS thoigianCHOT, SUBSTRING(CONVERT(nvarchar(10), dh.thoigianCHOT, 105), 1, 2) AS ngay, SUBSTRING(CONVERT(nvarchar(10), dh.thoigianCHOT, 105), 4, 2) AS thang, " +
                         "	ISNULL((SELECT ma FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') as chungloai, " +
                         "  sp.ma as masp, sp.ten as tensp, (SELECT ten FROM DonViTinh WHERE pk_seq = dh_sp.dvt_fk) as donvi, dh_sp.solo, dh_sp.lotNo, dh_sp.serialNo,   " +
                         "	dh_sp.soluong as soluong, dh_sp.dongia, dh_sp.soluong * dh_sp.dongia as thanhtien " +
                         "FROM NhapKhac dh INNER JOIN NhapKhac_SanPham_ChiTiet dh_sp on dh.pk_seq = dh_sp.nhapkhac_fk    " + condition +
                         "  LEFT JOIN Kho k ON dh.khochuyen_fk = k.pk_seq " +
                         "	INNER JOIN SanPham sp on dh_sp.sanpham_fk = sp.pk_seq " +
                         "	LEFT JOIN KhachHang kh on dh.khachhang_fk = kh.pk_seq " +
                         "WHERE dh.pk_seq > 0 AND dh.trangthai in (1) " + condition +
                         " ORDER BY k.makho, CONVERT(datetime, dh.ngaynhap, 105), sp.ma, dh_sp.lotNo, dh_sp.serialNo ";

                    DataTable dt = xl.ReadTable(sql);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string makho = dt.Rows[i]["makho"].ToString();
                        string makh = dt.Rows[i]["makh"].ToString();
                        khachhang = dt.Rows[i]["khachhang"].ToString();

                        string sodonhang = dt.Rows[i]["sodonhang"].ToString();
                        string ngaynhap = dt.Rows[i]["ngaynhap"].ToString();
                        string thoigianChot = dt.Rows[i]["thoigianChot"].ToString();
                        string ngay = dt.Rows[i]["ngay"].ToString();
                        string thang = dt.Rows[i]["thang"].ToString();

                        chungloai = dt.Rows[i]["chungloai"].ToString();
                        string masp = dt.Rows[i]["masp"].ToString();
                        string tensp = dt.Rows[i]["tensp"].ToString();
                        string donvi = dt.Rows[i]["donvi"].ToString();
                        string hansudung = dt.Rows[i]["solo"].ToString();
                        string lotNo = dt.Rows[i]["lotNo"].ToString();
                        string serialNo = dt.Rows[i]["serialNo"].ToString();

                        double soluong = double.Parse(dt.Rows[i]["soluong"].ToString());

                        double dongia = double.Parse(dt.Rows[i]["dongia"].ToString());
                        double thanhtien = double.Parse(dt.Rows[i]["thanhtien"].ToString());

                        string[] data = new string[] { (i + 1).ToString(), makh, chungloai, ngaynhap, sodonhang, thoigianChot, makho,
                                                    masp, tensp, donvi, lotNo, serialNo, hansudung, FormatString.ForMatNumber(soluong.ToString()) };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            cell.VerticalAlign = VerticalAlign.Middle;

                            if (j == 0)
                                cell.Width = Unit.Parse("60");
                            else if (j == 1 || j == 2 || j == 7)
                                cell.Width = Unit.Parse("150");
                            else if (j < 8)
                                cell.Width = Unit.Parse("100");
                            else if (j == 8)
                                cell.Width = Unit.Parse("350");
                            else
                                cell.Width = Unit.Parse("100");

                            if (j < 8 || j == 11 || j == 12)
                                cell.HorizontalAlign = HorizontalAlign.Center;
                            if (j >= 13)
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

        private string StockIn_ID_EXCEL(HttpContext context)
        {
            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string _exportContent = "";

            DataTable baocao = new DataTable("ReportStockIn_ID");

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
            baocao.Columns.Add("Pallet", typeof(string));

            string sql = "";
            ExecuteData xl = new ExecuteData();

            if (id.Length < 3)
                id = "0";

            sql = "SELECT dh.ngaynhap, sp.ma AS masp, sp.nameEnglish, sp.ten AS tensp, dh_sp.utcOrderNo, dh_sp.PLNo, dh_sp.lotNo, dh_sp.cartonNo, dh_sp.NoOf, dh_sp.solo, dh_sp.netWeight, dh_sp.grossWeight, " +
                "   dh_sp.planShipping, dh_sp.yards, dh_sp.roll, dh_sp.soluong, dh_sp.soluong1, dh_sp.soluong2, dh_sp.soluong3, dh_sp.mavach, " +
                "   ISNULL((SELECT ten FROM MauSac WHERE pk_seq = dh_sp.mausac_fk), '') color, " +
                "   ISNULL((SELECT ten FROM Location WHERE pk_seq = dh_sp.location_fk), '') location, " +
                "   ISNULL((SELECT ten FROM Pallet WHERE pk_seq = dh_sp.pallet_fk), '') pallet " +
                " FROM NhapHang dh INNER JOIN NhapHang_SanPham_ChiTiet dh_sp ON dh.pk_seq = dh_sp.nhaphang_fk AND dh_sp.nhaphang_fk = '" + id + "' " + 
                " INNER JOIN SanPham sp on dh_sp.sanpham_fk = sp.pk_seq  " +
                " WHERE dh_sp.pk_seq > 0 " +
                " ORDER BY dh_sp.stt ";

            DataTable dt = xl.ReadTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string ngaynhap = dt.Rows[i]["ngaynhap"].ToString();
                string solo = dt.Rows[i]["solo"].ToString();
                string itemVN = dt.Rows[i]["masp"].ToString();
                string itemJP = dt.Rows[i]["nameEnglish"].ToString();
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
                string location = dt.Rows[i]["location"].ToString();
                string pallet = dt.Rows[i]["pallet"].ToString();


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
                dr[26] = pallet;

                baocao.Rows.Add(dr);
            }

            string fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\ReportStockIn_ID.xlsm";

            Workbook workbook = new Workbook(fileName);
            Worksheet worksheet = workbook.Worksheets[0];

            worksheet.Cells.ImportDataTable(baocao, true, "A10");
            worksheet.AutoFitColumns();


            sql = "SELECT a.ngaynhap, ISNULL(k.makho, '') makho, ISNULL(kh.ma, '') khachhang, ISNULL(ncc.ten, '') nhacungcap, " +
            " ISNULL(a.sohopdong, '') sohopdong, ISNULL(a.sophieu, '') sophieu, ISNULL(a.ghichu, '') ghichu, " +
            "  CASE a.loainhap WHEN 0 THEN N'Unknow' " +
            "                   WHEN 1 THEN N'Into storage' " +
            "                   WHEN 2 THEN N'Temporary location' ELSE 'Unknnow' END loainhap " +
            " FROM NhapHang a INNER JOIN kho k ON a.kho_fk = k.pk_seq " +
            " LEFT JOIN khachhang kh ON a.khachhang_fk = kh.pk_seq " +
            " LEFT JOIN NhaCungCap ncc ON a.nhacungcap_fk = ncc.pk_seq " +
            " WHERE a.pk_seq = '" + id + "'";
            dt = xl.ReadTable(sql);
            if(dt.Rows.Count > 0)
            {
                string ngaynhap = dt.Rows[0]["ngaynhap"].ToString();
                string makho = dt.Rows[0]["makho"].ToString();
                string khachhang = dt.Rows[0]["khachhang"].ToString();
                string nhacungcap = dt.Rows[0]["nhacungcap"].ToString();
                string sohopdong = dt.Rows[0]["sohopdong"].ToString();
                string sophieu = dt.Rows[0]["sophieu"].ToString();
                string loainhap = dt.Rows[0]["loainhap"].ToString();
                string ghichu = dt.Rows[0]["ghichu"].ToString();

                worksheet.Cells["A5"].PutValue("No: " + sophieu + " - Date: " + ngaynhap);
                worksheet.Cells["A6"].PutValue("Customer: ");
                worksheet.Cells["C6"].PutValue(khachhang);

                worksheet.Cells["A7"].PutValue("Supplier: ");
                worksheet.Cells["C7"].PutValue(nhacungcap);

                worksheet.Cells["A8"].PutValue("Note: ");
                worksheet.Cells["C8"].PutValue(ghichu);

            }

            HttpResponse response = context.Response;
            workbook.Save(response, "ReportStockIn_ID.xlsm", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsm));

            return _exportContent;
        }

        private string NhapKhac_ID_EXCEL(HttpContext context)
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
                    header.Text = "INFORMATION IMPORT";
                    header.Font.Bold = true;
                    header.Font.Size = FontUnit.Point(15);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();
                    string query = "SELECT a.ngaynhap, ISNULL(a.ghichu, '') ghichu, ISNULL(a.xuatkhac_fk, 0) xuatkhac_fk, ISNULL(a.donhang_fk, 0) donhang_fk, " +
                        " CASE a.loaikho WHEN 0 THEN ISNULL((SELECT ma FROM KhachHang WHERE pk_seq = a.khachhang_fk), '') " +
                        "                 ELSE ISNULL((SELECT makho FROM Kho WHERE pk_seq = a.kho_fk), '') END makho, " +
                        " CASE a.loainhap WHEN 2 THEN ISNULL((SELECT ma FROM KhachHang WHERE pk_seq = a.khachhangchuyen_fk), '') " +
                        "                 ELSE ISNULL((SELECT makho FROM Kho WHERE pk_seq = a.khochuyen_fk), '') END khochuyen, " +
                        " CASE a.loainhap WHEN 1 THEN N'Local' " +
                        "                 WHEN 2 THEN N'Partner' " +
                        "                 WHEN 3 THEN N'Orther' " +
                        "                 WHEN 4 THEN N'Demolished' ELSE N'Unknow'  END loainhap " +
                        " FROM NhapKhac a " +
                        " WHERE a.pk_seq = '" + id + "'";
                    DataTable dtINFO = xl.ReadTable(query);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 6;
                    header.Text = " No: " + id + "  - Date: " + dtINFO.Rows[0]["ngaynhap"].ToString();
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
                    header.Text = "To: " + dtINFO.Rows[0]["makho"].ToString();
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
                    header.Text = "From: " + dtINFO.Rows[0]["khochuyen"].ToString() + " ............................................... No. stock out: " + dtINFO.Rows[0]["donhang_fk"].ToString();
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
                    header.Text = "Stock in type: " + dtINFO.Rows[0]["loainhap"].ToString();
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

                    string[] tieude = new string[] { "No", "Part Code", "Name", "Unit", "Lot No", "Serial No", "Product date", "Location", "Quantity" };

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
                            else if (i == 3 || i == 4 || i == 5 || i == 6 || i == 7)
                                header.Width = Unit.Parse("120");
                            else
                                header.Width = Unit.Parse("90");
                        }

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    query = " SELECT sp.ma, sp.ten, (SELECT ten FROM DonViTinh WHERE pk_seq = b.dvt_fk) AS donvi, b.solo, b.lotNo, b.serialNo, b.soluong, b.saiso,  " +
                        "   ISNULL((SELECT ma FROM Location WHERE pk_seq = b.location_fk), '') location, " +
                        "   ISNULL(sp.datebanhang, '0') datebanhang  " +
                        " FROM NhapKhac_SanPham_ChiTiet b INNER JOIN SanPham sp on b.sanpham_fk = sp.pk_seq  AND b.nhapkhac_fk = '" + id + "' " +                        
                        " WHERE b.nhapkhac_fk = " + id + "  " +                        
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
                        string location = dt.Rows[i]["location"].ToString();
                        string soluong = dt.Rows[i]["soluong"].ToString();
                        string saiso = dt.Rows[i]["saiso"].ToString();

                        string pt_hsd = "0";
                        //if (dt.Rows[i]["datebanhang"].ToString().Equals("0"))
                        //{
                        //    pt_hsd = "None";
                        //}
                        //else
                        //{
                        //    pt_hsd = (Math.Round(double.Parse(dt.Rows[i]["ngaysudung"].ToString()) / double.Parse(dt.Rows[i]["datebanhang"].ToString()), 2) * 100).ToString();
                        //}

                        string[] data = new string[] { (i + 1).ToString(), masp, tensp, donvi,lotNo, serialNo, solo, location, FormatString.ForMatNumber(soluong) };

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();

                            if (j > 7)
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

        private string KiemKho_ID_EXCEL(HttpContext context)
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
                    header.ColumnSpan = 8;
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
                    header.ColumnSpan = 8;
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
                    header.ColumnSpan = 8;
                    header.Text = "INFORMATION OF RESTOCK";
                    header.Font.Bold = true;
                    header.Font.Size = FontUnit.Point(15);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();
                    string query = "SELECT a.ngaykiem, ISNULL(b.makho, '') +  ', ' + ISNULL(b.tenkho, '')  AS kho, ISNULL(a.ghichu, '') ghichu, loaikiem, " +
                        " CASE loaikiem WHEN 1 THEN N'Follow Location' " +
                        "               WHEN 2 THEN N'Follow LotNo' END loaikiemText " + 
                        " FROM KiemKho a INNER JOIN Kho b on a.kho_fk = b.pk_seq  AND a.pk_seq = '" + id + "' " +
                        " WHERE a.pk_seq = '" + id + "'";
                    DataTable dtINFO = xl.ReadTable(query);

                    string loaikiem = dtINFO.Rows[0]["loaikiem"].ToString();

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 8;
                    header.Text = " No: " + id + "  - Date: " + dtINFO.Rows[0]["ngaykiem"].ToString();
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
                    header.ColumnSpan = 8;
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
                    header.ColumnSpan = 8;
                    header.Text = "Type of restock: " + dtINFO.Rows[0]["loaikiemText"].ToString();
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

                    string[] tieude = new string[] { "No.", "PartCode",
                        "Description", "Unit", "Location", "LotNo", "Quantity", "Booked", "Avaiable", "Restock", "Adjustment", "Scan by PDA" };

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

                        if (i == 6)
                            header.Width = Unit.Parse("450");
                        else
                        {
                            if (i == 3 || i == 4 || i == 5)
                                header.Width = Unit.Parse("150");
                            else if (i == 0)
                                header.Width = Unit.Parse("60");
                            else
                                header.Width = Unit.Parse("100");
                        }

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    string condition = "";
                    if (loaikiem.Equals("1"))
                        condition = ", CONVERT(datetime, b.solo, 105) ASC ";
                    query = " SELECT sp.ma, sp.ten, dvt.ten AS donvi, b.solo, b.LotNo,  " +
                    " 	'' AS xuatxu, ISNULL(loc.ma, '') AS location, b.ton, b.avai, b.booked, b.soluongkiem, b.chenhlech, b.ScanByPDA  " +
                    " FROM KiemKho a INNER JOIN KiemKho_SanPham b on a.pk_seq = b.kiemkho_fk  AND a.pk_seq = '" + id + "'  " +
                    " 	INNER JOIN SanPham sp on b.sanpham_fk = sp.pk_seq  " +
                    " 	INNER JOIN DonViTinh dvt on sp.dvt_fk = dvt.pk_seq  " +
                    //" 	LEFT JOIN XuatXu xx ON b.xuatxu_fk = xx.pk_seq  " +
                    " 	LEFT JOIN Location loc ON b.location_fk = loc.pk_seq  " +
                    " WHERE a.pk_seq = " + id + "  " +
                    " ORDER BY sp.ma ASC  " + condition;

                    DataTable dt = xl.ReadTable(query);
                    
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();
                        
                        string masp = dt.Rows[i]["ma"].ToString();
                        string tensp = dt.Rows[i]["ten"].ToString();
                        string donvi = dt.Rows[i]["donvi"].ToString();
                        string location = dt.Rows[i]["location"].ToString();
                        string lotNo = dt.Rows[i]["lotNo"].ToString();
                        string ton = dt.Rows[i]["ton"].ToString();
                        string avai = dt.Rows[i]["avai"].ToString();
                        string booked = dt.Rows[i]["booked"].ToString();

                        string soluong = dt.Rows[i]["soluongkiem"].ToString();
                        string chenhlech = dt.Rows[i]["chenhlech"].ToString();
                        string ScanByPDA = dt.Rows[i]["ScanByPDA"].ToString();

                        if (ScanByPDA.Equals("0"))
                        {
                            chenhlech = "-" + soluong;
                            soluong = "0";                            
                        }
                        string[] data = new string[] { (i + 1).ToString(), masp, tensp, donvi, location, lotNo, 
                            FormatString.ForMatNumber(ton), FormatString.ForMatNumber(avai), FormatString.ForMatNumber(booked), FormatString.ForMatNumber(soluong), FormatString.ForMatNumber(chenhlech), ScanByPDA};

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();

                            if (j > 7)
                                cell.HorizontalAlign = HorizontalAlign.Right;

                            if (j == 0 || j == 1 || j == 2 || j == 5|| j == 7)
                                cell.HorizontalAlign = HorizontalAlign.Center;

                            cell.Text = data[j];
                            cell.Font.Size = FontUnit.Point(10);
                            cell.Font.Name = "Arial";

                            //cell.Attributes.Add("style", @"mso-number-format:\@;");

                            row.Cells.Add(cell);
                        }

                        table.Rows.Add(row);
                    }

                    #region Note 

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 8;
                    header.Text = "Note: ";
                    header.Font.Italic = true;
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
                    header.Text = "1. Cột Tồn kho: Số lượng đang có trong kho trên hệ thống";
                    header.Font.Italic = true;
                    header.Font.Bold = false;
                    header.Font.Size = FontUnit.Point(9);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 8;
                    header.Text = "2. Cột hiện hữu: Số lượng còn có thể xuất kho";
                    header.Font.Italic = true;
                    header.Font.Bold = false;
                    header.Font.Size = FontUnit.Point(9);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 8;
                    header.Text = "3. Cột Booked: Số lượng đã được lên đơn hàng, chờ xuất kho";
                    header.Font.Italic = true;
                    header.Font.Bold = false;
                    header.Font.Size = FontUnit.Point(9);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 8;
                    header.Text = "4. Kiểm kho: Số lượng kiểm kho";
                    header.Font.Italic = true;
                    header.Font.Bold = false;
                    header.Font.Size = FontUnit.Point(9);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 8;
                    header.Text = "5. ScanByPDA: Số lần kiểm kho bằng PDA";
                    header.Font.Italic = true;
                    header.Font.Bold = false;
                    header.Font.Size = FontUnit.Point(9);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    #endregion 

                    table.RenderControl(htmlWriter);
                    _exportContent = sb.ToString();
                }
            }
            return _exportContent;
        }

        private string KiemKho_EXCEL(HttpContext context)
        {
            string tungay = "";
            if (context.Request.QueryString["tungay"] != null)
                tungay = context.Request.QueryString["tungay"].ToString();

            string denngay = "";
            if (context.Request.QueryString["denngay"] != null)
                denngay = context.Request.QueryString["denngay"].ToString();

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
                    header.Text = "THÔNG TIN KIỂM KHO";
                    header.Font.Bold = true;
                    header.Font.Size = FontUnit.Point(15);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 3;
                    header.Text = "Từ ngày " + tungay + "  đến ngày " + denngay;
                    header.Font.Italic = false;
                    header.Font.Bold = true;
                    header.Font.Size = FontUnit.Point(9);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Left;
                    header.VerticalAlign = VerticalAlign.Middle;

                    ExecuteData xl = new ExecuteData();
                    
                    //Chen 2 row khoang cach
                    for (int i = 0; i < 1; i++)
                    {
                        TableRow rowS = new TableRow();
                        table.Rows.Add(rowS);
                    }


                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    string[] tieude = new string[] { "STT", "Date", "ID", "Item VN", "Item JP", "Name", "Unit", 
                        "PLNo", "Color", "LotNo", "CartonNo", "Qty1", "Qty2", "Qty3", "Total", "Location", "Location scan" };

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

                        //if (i == 5)
                        //    header.Width = Unit.Parse("450");
                        //else
                        //{
                        //    if (i == 1)
                        //        header.Width = Unit.Parse("150");
                        //    else if (i == 0)
                        //        header.Width = Unit.Parse("60");
                        //    else
                        //        header.Width = Unit.Parse("100");
                        //}

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    string condition = "";
                    if (tungay.Trim().Length > 0)
                        condition += " AND CONVERT(datetime, a.ngaykiem, 105) >= CONVERT(datetime, '" + tungay + "', 105) ";
                    if (denngay.Trim().Length > 0)
                        condition += " AND CONVERT(datetime, a.ngaykiem, 105) <= CONVERT(datetime, '" + denngay + "', 105) ";
                    if (id.Trim().Length > 3)
                        condition += " AND a.pk_seq = '" + id + "' ";

                    string query = " SELECT (SELECT makho FROM Kho WHERE pk_seq = a.kho_fk) kho, a.ngaykiem, a.pk_seq idKiemKho, sp.ma, sp.codeEnglish, sp.ten, dvt.ten AS donvi, b.solo, " +
                    "	 b.plNo, b.lotNo, b.cartonNo, " +
                    "	 ISNULL((SELECT ten FROM MauSac WHERE pk_seq = b.mausac_fk), '') mausac, " +
                    "	 ISNULL((SELECT ten FROM Location WHERE pk_seq = b.locationScan_fk), '') locationScan, " +
                    "	 ISNULL((SELECT ten FROM Location WHERE pk_seq = b.location_fk), '') location, " +
                    "	 b.ton, b.soluongkiem, b.soluong, b.soluong1, b.soluong2, b.soluong3, b.chenhlech, b.ScanByPDA " +
                    " FROM KiemKho a INNER JOIN KiemKho_SanPham b on a.pk_seq = b.kiemkho_fk  AND a.trangthai in (0, 1) " + condition + 
                    "	INNER JOIN SanPham sp on b.sanpham_fk = sp.pk_seq  " +
                    "	INNER JOIN DonViTinh dvt on sp.dvt_fk = dvt.pk_seq " +
                    " WHERE a.trangthai in (0, 1) " + condition + 
                    " ORDER BY CONVERT(datetime, a.ngaykiem, 105), sp.ma, b.plNo, b.lotNo, b.sortBy ASC  ";

                    DataTable dt = xl.ReadTable(query);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string kho = dt.Rows[i]["kho"].ToString();
                        string idKiemKho = dt.Rows[i]["idKiemKho"].ToString();
                        string ngaykiem = dt.Rows[i]["ngaykiem"].ToString();
                        string itemVN = dt.Rows[i]["ma"].ToString();
                        string itemJP = dt.Rows[i]["codeEnglish"].ToString();
                        string tensp = dt.Rows[i]["ten"].ToString();
                        string donvi = dt.Rows[i]["donvi"].ToString();
                        string mausac = dt.Rows[i]["mausac"].ToString();
                        string solo = dt.Rows[i]["solo"].ToString();
                        string location = dt.Rows[i]["location"].ToString();
                        string locationScan = dt.Rows[i]["locationScan"].ToString();
                        string plNo = dt.Rows[i]["plNo"].ToString();
                        string lotNo = dt.Rows[i]["lotNo"].ToString();
                        string cartonNo = dt.Rows[i]["cartonNo"].ToString();
                        string ton = dt.Rows[i]["ton"].ToString();
                        string soluong = dt.Rows[i]["soluongkiem"].ToString();
                        string soluong1 = dt.Rows[i]["soluong1"].ToString();
                        string soluong2 = dt.Rows[i]["soluong2"].ToString();
                        string soluong3 = dt.Rows[i]["soluong3"].ToString();
                        string chenhlech = dt.Rows[i]["chenhlech"].ToString();
                        string ScanByPDA = dt.Rows[i]["ScanByPDA"].ToString();

                        if (ScanByPDA.Equals("0"))
                        {
                            chenhlech = "-" + soluong;
                            soluong = "0";                            
                        }

                        //"STT", "Date", "ID", "Item VN", "Item JP", "Name", "Unit", 
                        //"PLNo", "Color", "LotNo", "CartonNo", "Qty1", "Qty2", "Qty3", "Total", "Location", "Location scan"

                        string[] data = new string[] { (i + 1).ToString(), ngaykiem, idKiemKho, itemVN, itemJP, tensp, donvi,
                            plNo, mausac, lotNo, cartonNo, 
                            FormatString.ForMatNumber(soluong1), FormatString.ForMatNumber(soluong2), FormatString.ForMatNumber(soluong3), FormatString.ForMatNumber(soluong),
                            location, locationScan};

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();

                            if (j > 10 && j < 15)
                                cell.HorizontalAlign = HorizontalAlign.Right;

                            if (j == 0 || j == 1 || j == 2 || j == 6 || j == 15 || j == 16)
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

        private string ExportToExcel_TransferID_Detail(HttpContext context)
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
                    header.Text = "INFORMATION TRANSFER LOCATION";
                    header.Font.Bold = true;
                    header.Font.Size = FontUnit.Point(15);
                    header.Font.Name = "Arial";
                    header.HorizontalAlign = HorizontalAlign.Center;
                    header.VerticalAlign = VerticalAlign.Middle;

                    headerRow.Cells.Add(header);
                    table.Rows.Add(headerRow);

                    ExecuteData xl = new ExecuteData();
                    string query = "SELECT a.ngaynhap, ISNULL(a.ghichu, '') ghichu, " +
                        " ISNULL((SELECT makho FROM Kho WHERE pk_seq = a.kho_fk), '') makho, " +
                        " ISNULL((SELECT hoten FROM KhachHang WHERE pk_seq = a.khachhang_fk), '') khachhang " +
                        " FROM DoiLoBin a " +
                        " WHERE a.pk_seq = '" + id + "'";
                    DataTable dtINFO = xl.ReadTable(query);

                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 6;
                    header.Text = " No: " + id + "  - Date: " + dtINFO.Rows[0]["ngaynhap"].ToString();
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
                    //header.ColumnSpan = 6;
                    //header.Text = "Warehouse: " + dtINFO.Rows[0]["makho"].ToString();
                    //header.Font.Italic = false;
                    //header.Font.Bold = true;
                    //header.Font.Size = FontUnit.Point(10);
                    //header.Font.Name = "Arial";
                    //header.HorizontalAlign = HorizontalAlign.Left;
                    //header.VerticalAlign = VerticalAlign.Middle;

                    //headerRow.Cells.Add(header);
                    //table.Rows.Add(headerRow);
                    
                    headerRow = new TableRow();
                    header = new TableHeaderCell();
                    header.ColumnSpan = 6;
                    header.Text = "Customer: " + dtINFO.Rows[0]["khachhang"].ToString();
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

                    string[] tieude = new string[] { "No", "Vin No", "Grade", "Mode", "Frame", "Engine", "Color", "Location", " New Location"};

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
                            header.Width = Unit.Parse("250");
                        else
                        {

                            header.Width = Unit.Parse("150");
                        }

                        headerRow.Cells.Add(header);
                    }

                    table.Rows.Add(headerRow);

                    query = " SELECT b.mavach, sp.ma, sp.ten, (SELECT ten FROM DonViTinh WHERE pk_seq = b.dvt_fk) AS donvi, b.solo, b.sokhung, b.dongco, b.soluong, " +
                        "   ISNULL((SELECT ma FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') chungloai, " +
                        "   ISNULL((SELECT ma FROM MauSac WHERE pk_seq = b.mausac_fk), '') mausac, " +
                        "   ISNULL((SELECT ma FROM Location WHERE pk_seq = b.location_fk), '') location, " +
                        "   ISNULL((SELECT ma FROM Location WHERE pk_seq = b.location_new_fk), '') locationNew " +
                        " FROM DoiLoBin_SanPham_ChiTiet b INNER JOIN SanPham sp on b.sanpham_fk = sp.pk_seq  AND b.doilobin_fk = '" + id + "' " +
                        " WHERE b.doilobin_fk = " + id + "  " +
                        " ORDER BY b.mavach ASC ";

                    DataTable dt = xl.ReadTable(query);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow row = new TableRow();

                        string masp = dt.Rows[i]["ma"].ToString();
                        string tensp = dt.Rows[i]["ten"].ToString();
                        string mavach = dt.Rows[i]["mavach"].ToString();
                        string chungloai = dt.Rows[i]["chungloai"].ToString();
                        string sokhung = dt.Rows[i]["sokhung"].ToString();
                        string dongco = dt.Rows[i]["dongco"].ToString();
                        string mausac = dt.Rows[i]["mausac"].ToString();
                        string location = dt.Rows[i]["location"].ToString();
                        string locationNew = dt.Rows[i]["locationNew"].ToString();
                        string soluong = dt.Rows[i]["soluong"].ToString();
                        
                        string[] data = new string[] { (i + 1).ToString(), mavach, chungloai, tensp, sokhung, dongco, mausac, location, locationNew};

                        for (int j = 0; j < data.Length; j++)
                        {
                            TableCell cell = new TableCell();

                            if (j > 7)
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

        private string ExportToExcel_FileTemplateStockIn(HttpContext context)
        {

            string _exportContent = "";
            string fileName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Admin\\Files\\FileTemplateStockIn.xlsm";
            Workbook workbook = new Workbook(fileName);
            HttpResponse response = context.Response;
            workbook.Save(response, "FileTemplateStockIn.xlsm", ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsm));

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