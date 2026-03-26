using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Text;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using HLVTimeSheet.AcsessData;

namespace HLVTimeSheet.Admin.Hander
{
    /// <summary>
    /// Summary description for hdInPdf
    /// </summary>
    public class hdInPdf : IHttpHandler, System.Web.SessionState.IRequiresSessionState 
    {
        private string congty_chinhanh = "Honda Logicom Viet Nam Co., ltd";
        private string diachi_title = " Hoai Nam Building, No. 6, Hai Ba Trung Street, Hung Vuong Ward, Phuc Yen City, Vinh Phuc Province";
        private string pathImage = "../inetpub/wwwroot/Images/logoHondalogicom.png";
        //private string pathImage = "../Images/logoHondalogicom.png";

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string type = context.Request.QueryString["type"];
            if (type == null)
                type = "";

            if (type.Equals("donhangPDF"))
            {
                InDonHangPDF(context);
            }
            else if (type.Equals("pickupPrintPDF"))
            {
                PickupPrintPDF(context);
            }            
            else if (type.Equals("soanhang"))
            {
                InSoanHangPDF(context);
            }
            else if (type.Equals("xuatkhacPDF"))
            {
                InXuatKhacPDF(context);
            }
            else if (type.Equals("nhaphangPDF"))
            {
                InNhapHangPDF(context);
            }                                  
            else if (type.Equals("nhapkhacPDF"))
            {
                InNhapKhacPDF(context);
            }                       
            else if (type.Equals("InGiaoNhan"))
            {
                InPhieuGiaoNhan(context);
            }
            else if (type.Equals("doilobinPDF"))
            {
                DoiLocationBin(context);
            }                                    
            else if (type.Equals("dathangPDF"))
            {
                InDatHangPDF(context);
            }                        
            else if (type.Equals("dieuchuyenPDF"))
            {
                DieuChuyenPDF(context);
            }                        
        }

        private void InPhieuGiaoNhan(HttpContext context)
        {
            context.Response.ContentType = "application/pdf";

            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            //context.Response.AddHeader("content-disposition", "attachment;filename=PhieuXuatKho_" + id + ".pdf");
            context.Response.AddHeader("content-disposition", "inline;filename=PhieuXuatKhoTong_" + id + ".pdf");
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            context.Response.Charset = Encoding.Unicode.ToString();
            context.Response.ContentEncoding = Encoding.UTF8;

            Document pdfDoc = new Document(PageSize.A4, 5.0f, 5.0f, 5.0f, 5.0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, context.Response.OutputStream);

            pdfDoc.Open();

            try
            {
                ExecuteData xl = new ExecuteData();

                BaseFont bf = BaseFont.CreateFont("c:\\windows\\fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font times = new Font(bf, 13.0f, Font.BOLD);

                Paragraph pxk = new Paragraph("PHIẾU XUẤT KHO TỔNG", times);
                pxk.Alignment = 1;
                pxk.SpacingBefore = 1.0f;
                pxk.SpacingAfter = 1.0f;
                pdfDoc.Add(pxk);

                string trangthai = xl.ExecuteScalarSQL("SELECT trangthai FROM PhieuXuatKhoNVGN WHERE pk_seq = '" + id + "'").ToString();

                Paragraph dh = new Paragraph("No: " + id + " - Ngày xuất: " + xl.ExecuteScalarSQL("SELECT ngayxuatkho FROM PhieuXuatKhoNVGN WHERE pk_seq = '" + id + "'"), new Font(bf, 7.0f, Font.ITALIC));
                dh.SpacingAfter = 1.0f;
                dh.Alignment = Element.ALIGN_CENTER;
                pdfDoc.Add(dh);

                dh = new Paragraph("Thông tin sản phẩm", new Font(bf, 10.0f, Font.UNDERLINE));
                dh.SpacingAfter = 1.0f;
                pdfDoc.Add(dh);

                //content
                Paragraph content = new Paragraph();

                iTextSharp.text.Table aTable = new iTextSharp.text.Table(5);
                aTable.AutoFillEmptyCells = true;
                aTable.Alignment = Element.ALIGN_MIDDLE;
                aTable.Cellpadding = 1.0f;
                aTable.Cellspacing = 1.0f;
                aTable.Widths = new float[] { 6.0f, 8.0f, 50.0f, 8.0f, 8.0f };

                Cell cell = new Cell(new Phrase("No", new Font(bf, 8.0f, Font.BOLD)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("Mã", new Font(bf, 8.0f, Font.BOLD)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("Part name", new Font(bf, 8.0f, Font.BOLD)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                aTable.AddCell(cell);
              

                cell = new Cell(new Phrase("ĐVT", new Font(bf, 8.0f, Font.BOLD)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("Số lượng", new Font(bf, 8.0f, Font.BOLD)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                aTable.AddCell(cell);

                //INIT PHIEU XUAT KHO
                string query = "";

                if (trangthai.Equals("1"))
                {
                    query = " SELECT b.ma, b.ten, e.ten as donvitinh, a.solo, N'Hàng bán' as loai, cast(a.SOLUONGQUYDOI as varchar(20)) as soluong, '' as SCHEME, 0 as STT, " +
                            "       isnull( a.quycach, 1 )  AS quyCACH " +
                            " 	FROM DonHang_SanPham a INNER JOIN  SanPham b on a.SANPHAM_FK = b.pk_seq " +
                            "       INNER JOIN  Donhang f ON f.pk_seq = a.donhang_fk " +
                            "           INNER JOIN  PhieuXuatKhoNVGN_DonHang d ON d.donhang_fk = a.donhang_fk " +
                            "           INNER JOIN  DonViTinh e ON e.pk_seq = a.dvt_fk "+
                            " 	WHERE d.phieuxuatkhoNVGN_fk = '" + id + "'" +
                            " union all" +
                            " 	SELECT b.ma, b.ten, f.ten as donvitinh, a.solo, N'Hàng khuyến mại' as loai, cast(a.SOLUONG as varchar(20)) as soluong, d.scheme as SCHEME, 1 as STT,  " +
                            "       isnull( a.quycach, 1 )  AS quyCACH " +
                            " 	FROM DonHang_CTKM_TraKM a INNER JOIN  SanPham b on a.SPMA = b.ma " +
                            "       INNER JOIN  Donhang dh ON dh.pk_seq = a.donhang_fk " +                           
                            " 		INNER JOIN  ChuongTrinhKhuyenMai d on a.CTKM_FK = d.pk_seq" +
                            "       INNER JOIN  PhieuXuatKhoNVGN_DonHang e ON e.donhang_fk = a.donhang_fk " +
                             "      INNER JOIN  DonViTinh f ON f.pk_seq = b.dvt_fk " +
                            " 	WHERE e.phieuxuatkhoNVGN_fk = '" + id + "' and a.LOAI = '0' " +
                            " union all" +
                            " 	SELECT '' as ma, '' as ten, '' as donvitinh, '' as solo, N'Tiền khuyến mại' as loai, cast(a.TONGGIATRI as varchar(20)) as soluong, b.scheme as SCHEME, 2 as STT, 0 as QuyCach  " +
                            " 	FROM DonHang_CTKM_TraKM a INNER JOIN  ChuongTrinhKhuyenMai b on a.CTKM_FK = b.pk_seq" +
                            "           INNER JOIN  PhieuXuatKhoNVGN_DonHang e ON e.donhang_fk = a.donhang_fk " +
                            " 	WHERE e.phieuxuatkhoNVGN_fk = '" + id + "'" +
                            " ORDER BY STT asc";
                }
                else
                {
                    query = " 	SELECT c.ma, c.ten, f.ten as donvitinh, b.solo, N'Hàng bán' as loai, cast( SUM(b.soluongQUYDOI) as varchar(20)) as soluong , '' as SCHEME, 0 as STT,  " +
                            "       isnull( b.quycach, 1 )  AS quyCACH " +
                            " 	FROM DONHANG a INNER JOIN  DonHang_SanPham b on a.pk_seq = b.donhang_fk   " +
                            " 			INNER JOIN  SanPham c on b.sanpham_fk = c.pk_seq " +
                            "           INNER JOIN  DonViTinh f ON f.pk_seq = b.dvt_fk " +
                            " 	WHERE a.pk_seq in ( SELECT donhang_fk FROM PhieuXuatKhoNVGN_DONHANG WHERE phieuxuatkhoNVGN_fk = '" + id + "' )   " +
                            " 	group by c.ma, c.ten, b.solo, c.pk_seq, c.dvt_fk, f.ten, b.quycach  " +
                            " 	having sum(b.SOLUONG) > 0  " +
                            " union ALL " +
                            " 	SELECT d.ma, d.ten, f.ten as deonvitinh, b.SOLO, N'Tiền khuyến mại' as loai,CAST( SUM(b.soluong)  as varchar(20)) as soluong, c.scheme, 1 as STT,   " +
                            "       isnull(b.quycach, 1 )  AS quyCACH " +
                            "     FROM DONHANG a INNER JOIN  DONHANG_CTKM_TRAKM b on a.pk_seq = b.donhang_fk   " +
                            "             INNER JOIN  ChuongTrinhKhuyenMai c on b.CTKM_FK = c.pk_seq   " +
                            "             INNER JOIN  SanPham d on b.SPMA = d.ma   " +
                             "           INNER JOIN  DonViTinh f ON f.pk_seq = d.dvt_fk " +
                            "     WHERE a.pk_seq in ( SELECT donhang_fk FROM PhieuXuatKhoNVGN_DONHANG WHERE phieuxuatkhoNVGN_fk = '" + id + "' ) and b.loai = '0'  " +
                            "     group by d.ma, d.ten, b.SOLO, c.scheme, d.pk_seq, d.dvt_fk, f.ten, b.quycach " +
                            "     having sum(b.SOLUONG) > 0 " +
                            " union ALL " +
                            " 	SELECT '', '', '', '', N'Tiền khuyến mại' as loai, CAST( SUM(TONGGIATRI) as varchar(20)) as soluong, b.scheme, 2 as STT, 0 as QuyCach   " +
                            "     FROM DONHANG_CTKM_TRAKM a INNER JOIN  ChuongTrinhKhuyenMai b on a.CTKM_FK = b.pk_seq  " +
                            "     WHERE DONHANG_FK in ( SELECT donhang_fk FROM PhieuXuatKhoNVGN_DONHANG WHERE phieuxuatkhoNVGN_fk = '" + id + "' )  and len(ISNULL(SPMA, '')) = 0   " +
                            "     group by b.scheme ";
                }

                DataTable dtSP = xl.ReadTable(query);
                if (dtSP.Rows.Count > 0)
                {
                    double quycach = 0;
                    double soluong = 0;
                    double sothung = 0;
                    double soluongLe = 0;

                    for (int i = 0; i < dtSP.Rows.Count; i++)
                    {
                        Font fo = new Font(bf, 8.0f, Font.NORMAL);
                        if (dtSP.Rows[i]["SCHEME"].ToString().Trim().Length > 0)
                            fo = new Font(bf, 8.0f, Font.ITALIC, Color.RED);

                        quycach = double.Parse(dtSP.Rows[i]["quycach"].ToString());
                        soluong = double.Parse(dtSP.Rows[i]["soluong"].ToString());

                        //if (quycach <= 0)
                        //{
                        //    sothung = 0;
                        //    soluongLe = soluong;
                        //}
                        //else
                        //{
                        //    sothung = Math.Round(soluong / quycach, 0);
                        //    soluongLe = Math.Round((soluong - (sothung * quycach)), 3);
                        //}

                        cell = new Cell(new Phrase((i + 1).ToString(), fo));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        aTable.AddCell(cell);
                      
                        cell = new Cell(new Phrase(dtSP.Rows[i]["ma"].ToString(), fo));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase(dtSP.Rows[i]["ten"].ToString(), fo));
                        cell.SetHorizontalAlignment("LEFT");
                        cell.SetVerticalAlignment("middle");
                        aTable.AddCell(cell);

                        //cell = new Cell(new Phrase(sothung.ToString() + " ", fo));
                        //cell.SetHorizontalAlignment("RIGHT");
                        //cell.SetVerticalAlignment("middle");
                        //aTable.AddCell(cell);

                        //cell = new Cell(new Phrase(soluongLe.ToString() + " ", fo));
                        //cell.SetHorizontalAlignment("RIGHT");
                        //cell.SetVerticalAlignment("middle");
                        //aTable.AddCell(cell);

                        cell = new Cell(new Phrase(dtSP.Rows[i]["donvitinh"].ToString(), fo));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase(FormatString.ForMatNumber(soluong.ToString()) + " ", fo));
                        cell.SetHorizontalAlignment("RIGHT");
                        cell.SetVerticalAlignment("middle");
                        aTable.AddCell(cell);
                      
                    }

                }
                dtSP.Clone();


                content.Add(aTable);


                //KHACH HANG
                dh = new Paragraph("Khách hàng", new Font(bf, 10.0f, Font.UNDERLINE, Color.BLACK));
                dh.SpacingAfter = 1.0f;
                content.Add(dh);

                aTable = new iTextSharp.text.Table(4);
                aTable.AutoFillEmptyCells = true;
                aTable.Alignment = Element.ALIGN_MIDDLE;
                aTable.Cellpadding = 1.0f;
                aTable.Cellspacing = 1.0f;
                aTable.Widths = new float[] { 7.0f, 18.0f, 60.0f, 15.0f};

                cell = new Cell(new Phrase("No", new Font(bf, 8.0f, Font.BOLD)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                aTable.AddCell(cell);               

                cell = new Cell(new Phrase("Tên khách hàng", new Font(bf, 8.0f, Font.BOLD)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("Địa chỉ giao hàng", new Font(bf, 8.0f, Font.BOLD)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("Đơn hàng", new Font(bf, 8.0f, Font.BOLD)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                aTable.AddCell(cell);

                //cell = new Cell(new Phrase("Tổng tiền", new Font(bf, 8.0f, Font.BOLD)));
                //cell.SetHorizontalAlignment("CENTER");
                //cell.SetVerticalAlignment("middle");
                //aTable.AddCell(cell);             

                //query = " SELECT xk.sodonhang, xk.MA, xk.hoten, xk.diachi, xk.chon, cast ( ( xk.bVAT - ( ( xk.bVAT - xk.KM ) *  xk.pt_chietkhau / 100 ) - xk.KM ) as numeric(18 , 0) ) as TONGSOTIEN_SAUKM  " +
                //        " FROM " +
                //        " ( " +
                //        " 	SELECT a.pk_seq as sodonhang, b.ma, b.hoten, b.diachi, 1 as chon,   " +
                //        " 			( SELECT SUM(soluong * isnull( dongiaSAUCHIA, dongia) - chietkhau ) FROM DonHang_SanPham WHERE donhang_fk = a.pk_seq ) bVAT, " +
                //        " 			isnull( ( SELECT SUM(TONGGIATRI) FROM DONHANG_CTKM_TRAKM WHERE SPMA is null and donhang_fk = a.pk_seq and loai = '0' ), 0 ) KM, a.pt_chietkhau " +
                //        " 	FROM DONHANG a INNER JOIN  KHACHHANG b on a.khachhang_fk = b.pk_seq   " +
                //        " 	WHERE a.pk_seq in ( SELECT DONHANG_FK FROM PhieuXuatKhoNVGN_DONHANG WHERE phieuxuatkhoNVGN_fk = '" + id + "' ) " +
                //        " ) xk ";

                query = " SELECT k.ma, k.hoten, ISNULL(k.diachigiaohang, k.diachi) diachigiaohang, a.donhang_fk " +
                        " FROM PhieuXuatKhoNVGN_DonHang a INNER JOIN  DonHang b ON a.donhang_fk = b.pk_seq " +
                        "	INNER JOIN  KhachHang k ON b.khachhang_fk = k.pk_seq " +
                        " WHERE a.phieuxuatkhoNVGN_fk = '" + id + "' ";
                DataTable dtKhachHang = xl.ReadTable(query);
                if (dtKhachHang.Rows.Count > 0)
                {
                    for (int i = 0; i < dtKhachHang.Rows.Count; i++)
                    {
                        Font fo = new Font(bf, 8.0f, Font.NORMAL);

                        cell = new Cell(new Phrase((i + 1).ToString(), fo));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        aTable.AddCell(cell);                     

                        cell = new Cell(new Phrase(dtKhachHang.Rows[i]["hoten"].ToString(), fo));
                        cell.SetHorizontalAlignment("LEFT");
                        cell.SetVerticalAlignment("middle");
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase(dtKhachHang.Rows[i]["diachigiaohang"].ToString(), fo));
                        cell.SetHorizontalAlignment("LEFT");
                        cell.SetVerticalAlignment("middle");
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase(dtKhachHang.Rows[i]["donhang_fk"].ToString(), fo));
                        cell.SetHorizontalAlignment("LEFT");
                        cell.SetVerticalAlignment("middle");
                        aTable.AddCell(cell);

                        //cell = new Cell(new Phrase(FormatString.ForMatNumber(dtKhachHang.Rows[i]["TONGSOTIEN_SAUKM"].ToString()) + " ", fo));
                        //cell.SetHorizontalAlignment("RIGHT");
                        //cell.SetVerticalAlignment("middle");
                        //aTable.AddCell(cell);
                    }
                }

                content.Add(aTable);

                pdfDoc.Add(content);

            }
            catch (Exception ex)
            {
                StringWriter sw = new StringWriter();

                string str = "Error when created file PDF...." + ex.Message;
                StringReader sr = new StringReader(str);

                htmlparser.Parse(sr);
            }

            pdfDoc.Close();
            context.Response.Write(pdfDoc);
            context.Response.End();
        }
       
        private void InDonHangPDF(HttpContext context)
        {
            context.Response.ContentType = "application/pdf";
            
            DataTable dtInfo = new DataTable();
            DataTable dtSP = new DataTable();

            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string loaiFile = "";
            if (context.Request.QueryString["loaiFile"] != null)
                loaiFile = context.Request.QueryString["loaiFile"].ToString();

            context.Response.AddHeader("content-disposition", "inline;filename=StockOut_" + id + ".pdf");
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            context.Response.Charset = Encoding.Unicode.ToString();
            context.Response.ContentEncoding = Encoding.UTF8;

            Document pdfDoc = new Document();

            string dinhdang = context.Request.QueryString["dinhdang"];
            if (dinhdang == null)
                dinhdang = "0";

            if (dinhdang.Equals("1"))
                pdfDoc = new Document(PageSize.A4, 20.0f, 20.0f, 0.0f, 0.0f);
            else
                pdfDoc = new Document(PageSize.A5, 20.0f, 20.0f, 0.0f, 0.0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, context.Response.OutputStream);

            pdfDoc.Open();

            int sodongDH = 0;
            int checkSoDong = 0;
            int defineLine = 0;
            double countPDF = 1;
            double totalBox = 0;
            double totalQty = 0;

            try
            {
                ExecuteData xl = new ExecuteData();

                //TẠM THỜI ĐỂ XỬ LÝ NHỮNG ĐƠN CŨ
                //xl.updateTONGGIATRI(id);

                BaseFont bf = BaseFont.CreateFont("c:\\windows\\fonts\\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                Font times = new Font(bf, 10.0f, Font.BOLD);
                Cell cell = new Cell();

                Paragraph content = new Paragraph();

                //string kho = "";
                //string khuvuc = "";
                for (int loop = 0; loop < 1; loop++)
                { 
                    if (loop == 0)
                    {
                        for (int count = 0; count < countPDF; count++)
                        {

                            string query = "SELECT a.pk_seq, ISNULL(a.dathang_fk, 0) dathang_fk, a.trangthai, a.ngaydonhang, ISNULL(a.ngaygiaohang, '') ngaygiaohang, " +
                            "  a.ghichu, ISNULL(a.sophieu, '') sophieu, ISNULL((SELECT hoten FROM KhachHang WHERE pk_seq = a.khachhang_fk), '') khachhang," +
                            "  ISNULL((SELECT tenkho FROM Kho WHERE pk_seq = a.kho_fk), '') kho " +
                            " FROM DonHang a " +
                            " WHERE a.pk_seq = '" + id + "'";

                            dtInfo = xl.ReadTable(query);

                            //kho = dtInfo.Rows[0]["kho_fk"].ToString();
                            //khuvuc = dtInfo.Rows[0]["khuvuc"].ToString();

                            iTextSharp.text.Table aTable = new iTextSharp.text.Table(3);
                            aTable.AutoFillEmptyCells = true;
                            aTable.Alignment = Element.ALIGN_LEFT;
                            aTable.Width = 100.0f;
                            aTable.Cellpadding = 0.0f;
                            aTable.Cellspacing = 0.0f;
                            aTable.Widths = new float[] { 30.0f, 100.0f, 100.0f };
                            aTable.BorderWidth = 0.0f;

                            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(pathImage);
                            jpg.ScaleToFit(70.0f, 100.0f);

                            cell = new Cell(jpg);
                            cell.Rowspan = 3;
                            cell.BorderWidth = 0;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase(congty_chinhanh, new Font(bf, 9.0f, Font.NORMAL)));
                            cell.BorderWidth = 0;
                            cell.Colspan = 2;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase(diachi_title, new Font(bf, 10.0f, Font.NORMAL)));
                            cell.BorderWidth = 0;
                            cell.Colspan = 2;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase(" Department: ", new Font(bf, 10.0f, Font.NORMAL)));
                            cell.BorderWidth = 0;
                            cell.Colspan = 2;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase("", new Font(bf, 10.0f, Font.NORMAL)));
                            cell.BorderWidth = 0;
                            //cell.Colspan = 2;
                            aTable.AddCell(cell);

                            pdfDoc.Add(aTable);

                            aTable = new iTextSharp.text.Table(3);
                            aTable.AutoFillEmptyCells = true;
                            aTable.Alignment = Element.ALIGN_LEFT;
                            aTable.Width = 100.0f;
                            aTable.Cellpadding = 0.5f;
                            aTable.Cellspacing = 0.5f;
                            aTable.Widths = new float[] { 30.0f, 30.0f, 30.0f };
                            aTable.BorderWidth = 0.0f;

                            cell = new Cell(new Phrase("DELIVERY BILL", new Font(bf, 16.0f, Font.BOLD)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0;
                            cell.Colspan = 3;
                            aTable.AddCell(cell);

                            string tt = dtInfo.Rows[0]["trangthai"].ToString();
                            string trangthaiDH = "";
                            if (tt.Equals("0"))
                                trangthaiDH = "Processing";
                            else if (tt.Equals("1"))
                                trangthaiDH = "Approved";
                            else if (tt.Equals("2"))
                                trangthaiDH = "Cannel";

                            cell = new Cell(new Phrase("Status: " + trangthaiDH, new Font(bf, 10.0f, Font.ITALIC)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("small");
                            cell.BorderWidth = 0f;
                            cell.Colspan = 3;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase("No: " + dtInfo.Rows[0]["sophieu"].ToString() + " - Date: " + dtInfo.Rows[0]["ngaydonhang"].ToString() + " - Delivery date: " + dtInfo.Rows[0]["ngaygiaohang"].ToString(), new Font(bf, 10.0f, Font.ITALIC)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("small");
                            cell.BorderWidth = 0f;
                            cell.Colspan = 3;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase("From  : " + dtInfo.Rows[0]["kho"].ToString(), new Font(bf, 11.0f, Font.NORMAL)));
                            cell.SetHorizontalAlignment("LEFT");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0f;
                            cell.Colspan = 3;
                            aTable.AddCell(cell);

                            //cell = new Cell(new Phrase("Type     : " + dtInfo.Rows[0]["loaihanghoa"].ToString(), new Font(bf, 11.0f, Font.NORMAL)));
                            //cell.SetHorizontalAlignment("LEFT");
                            //cell.SetVerticalAlignment("middle");
                            //cell.BorderWidth = 0f;
                            //cell.Colspan = 1;
                            //aTable.AddCell(cell);

                            cell = new Cell(new Phrase("To      : " + dtInfo.Rows[0]["khachhang"].ToString(), new Font(bf, 11.0f, Font.NORMAL)));
                            cell.SetHorizontalAlignment("LEFT");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0f;
                            cell.Colspan = 3;
                            aTable.AddCell(cell);

                            //cell = new Cell(new Phrase("By order: " + dtInfo.Rows[0]["dathang_fk"].ToString(), new Font(bf, 11.0f, Font.NORMAL)));
                            //cell.SetHorizontalAlignment("LEFT");
                            //cell.SetVerticalAlignment("middle");
                            //cell.BorderWidth = 0f;
                            //cell.Colspan = 1;
                            //aTable.AddCell(cell);

                            //cell = new Cell(new Phrase("Option: " + dtInfo.Rows[0]["tuychon"].ToString(), new Font(bf, 11.0f, Font.NORMAL)));
                            //cell.SetHorizontalAlignment("LEFT");
                            //cell.SetVerticalAlignment("middle");
                            //cell.BorderWidth = 0f;
                            //cell.Colspan = 3;
                            //aTable.AddCell(cell);

                            cell = new Cell(new Phrase("Note   : " + dtInfo.Rows[0]["ghichu"].ToString(), new Font(bf, 11.0f, Font.NORMAL)));
                            cell.SetHorizontalAlignment("LEFT");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0f;
                            cell.Colspan = 3;
                            aTable.AddCell(cell);

                            // VIP
                            content.Add(aTable);

                            //if (loaiFile.Equals("1"))
                            //{
                            //    #region Xuat kho hang ban
                            //    aTable = new iTextSharp.text.Table(9);
                            //    aTable.AutoFillEmptyCells = true;
                            //    aTable.Alignment = Element.ALIGN_LEFT;
                            //    aTable.Width = 100.0f;
                            //    aTable.Cellpadding = 1.0f;
                            //    aTable.Cellspacing = 1.0f;
                            //    aTable.Widths = new float[] { 5.0f, 12.0f, 20.0f, 10.0f, 9.0f, 6.0f, 7.0f, 6.0f, 8.0f };
                            //    aTable.BorderWidth = 0.01f;

                            //    cell = new Cell(new Phrase("No", new Font(bf, 9.0f, Font.BOLD)));
                            //    cell.SetHorizontalAlignment("CENTER");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    aTable.AddCell(cell);

                            //    cell = new Cell(new Phrase("Part Code", new Font(bf, 9.0f, Font.BOLD)));
                            //    cell.SetHorizontalAlignment("CENTER");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    aTable.AddCell(cell);

                            //    cell = new Cell(new Phrase("Part Name", new Font(bf, 9.0f, Font.BOLD)));
                            //    cell.SetHorizontalAlignment("CENTER");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    aTable.AddCell(cell);
                                
                            //    cell = new Cell(new Phrase("Lot No", new Font(bf, 9.0f, Font.BOLD)));
                            //    cell.SetHorizontalAlignment("CENTER");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    aTable.AddCell(cell);

                            //    cell = new Cell(new Phrase("Qty/Packed", new Font(bf, 9.0f, Font.BOLD)));
                            //    cell.SetHorizontalAlignment("CENTER");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    aTable.AddCell(cell);

                            //    cell = new Cell(new Phrase("Boxs", new Font(bf, 9.0f, Font.BOLD)));
                            //    cell.SetHorizontalAlignment("CENTER");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    aTable.AddCell(cell);

                            //    cell = new Cell(new Phrase("Quantity", new Font(bf, 9.0f, Font.BOLD)));
                            //    cell.SetHorizontalAlignment("CENTER");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    aTable.AddCell(cell);

                            //    cell = new Cell(new Phrase("Unit", new Font(bf, 9.0f, Font.BOLD)));
                            //    cell.SetHorizontalAlignment("CENTER");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    aTable.AddCell(cell);

                            //    cell = new Cell(new Phrase("Exchange", new Font(bf, 9.0f, Font.BOLD)));
                            //    cell.SetHorizontalAlignment("CENTER");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    aTable.AddCell(cell);

                            //    #region truy van SQL

                            //    //CHIẾT KHẤU VÀ KM ĐÃ TRỪ VÀO ĐƠN GIÁ CHƯA VAT
                            //    query = " 	SELECT COUNT(*) box, sp.ma, sp.ten, dh_sp.solo, dh_sp.lotNo, dv.ten as dvt," +
                            //    "       '' AS location, '' AS xuatxu, sp.soluongtieuchuan, ISNULL((SELECT TOP(1) soluong2 FROM QuyCach WHERE sanpham_fk = dh_sp.sanpham_fk), 1) quycach, SUM(dh_sp.soluong) soluong  " +
                            //    " 	FROM DonHang_SanPham_ChiTiet dh_sp INNER JOIN  SanPham sp on dh_sp.sanpham_fk = sp.pk_seq  AND dh_sp.donhang_fk = '" + id + "' " +
                            //    //"       INNER JOIN XuatXu xx ON dh_sp.xuatxu_fk = xx.pk_seq " +
                            //    //"       LEFT JOIN Location loc ON dh_sp.location_fk = loc.pk_seq " +
                            //    " 		INNER JOIN  DonViTinh dv on dh_sp.dvt_fk = dv.pk_seq " +
                            //    " 	WHERE sp.pk_seq > 0 " +
                            //    "   GROUP BY dh_sp.sanpham_fk, sp.ma, sp.ten, dh_sp.solo, sp.soluongtieuchuan, dh_sp.lotNo, dv.ten " + 
                            //    "   ORDER BY sp.ma, dh_sp.lotNo ASC ";
                            //    dtSP = xl.ReadTable(query);
                              
                            //    double quycach = 0;
                            //    #endregion

                            //    if (sodongDH == 0)
                            //    {
                            //        sodongDH = dtSP.Rows.Count;
                            //        // tinh so lan lap
                            //        countPDF = (Math.Round(sodongDH / 18.0 + 0.499));
                            //    }

                            //    defineLine = dtSP.Rows.Count;

                            //    if (defineLine > 0)
                            //    {
                            //        Font fo = new Font(bf, 9.0f, Font.NORMAL);
                            //        Font fo_ten = new Font(bf, 9.0f, Font.NORMAL);

                            //        #region moi lan in trang, in 18 dong - don hang ban

                            //        sodongDH = defineLine - count * 18;
                            //        if (sodongDH < 18)
                            //        {
                            //            #region In don hang
                            //            for (int i = 0; i < sodongDH; i++)
                            //            {
                            //                totalBox += double.Parse(dtSP.Rows[checkSoDong]["box"].ToString());
                            //                totalQty += double.Parse(dtSP.Rows[checkSoDong]["soluong"].ToString());
                            //                quycach = double.Parse(dtSP.Rows[checkSoDong]["soluong"].ToString()) / double.Parse(dtSP.Rows[checkSoDong]["quycach"].ToString());

                            //                cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                            //                cell.SetHorizontalAlignment("CENTER");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["ma"].ToString(), fo));
                            //                cell.SetHorizontalAlignment("CENTER");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["ten"].ToString(), fo_ten));
                            //                cell.SetHorizontalAlignment("LEFT");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);
                                            
                            //                cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["lotNo"].ToString(), fo));
                            //                cell.SetHorizontalAlignment("CENTER");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(FormatString.ForMatNumber(dtSP.Rows[checkSoDong]["soluongtieuchuan"].ToString()) + " ", fo));
                            //                cell.SetHorizontalAlignment("RIGHT");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(FormatString.ForMatNumber(dtSP.Rows[checkSoDong]["box"].ToString()) + " ", fo));
                            //                cell.SetHorizontalAlignment("RIGHT");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(FormatString.ForMatNumber(dtSP.Rows[checkSoDong]["soluong"].ToString()) + " ", fo));
                            //                cell.SetHorizontalAlignment("RIGHT");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["dvt"].ToString(), fo));
                            //                cell.SetHorizontalAlignment("CENTER");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(FormatString.ForMatNumber((quycach.ToString())) + " ", fo));
                            //                cell.SetHorizontalAlignment("RIGHT");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                checkSoDong++;
                            //            }
                            //            #endregion
                            //        }
                            //        else
                            //        {
                            //            #region In don hang
                            //            for (int i = 0; i < 18; i++)
                            //            {
                            //                totalBox += double.Parse(dtSP.Rows[checkSoDong]["box"].ToString());
                            //                totalQty += double.Parse(dtSP.Rows[checkSoDong]["soluong"].ToString());
                            //                quycach = double.Parse(dtSP.Rows[checkSoDong]["soluong"].ToString()) / double.Parse(dtSP.Rows[checkSoDong]["quycach"].ToString());

                            //                cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                            //                cell.SetHorizontalAlignment("CENTER");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["ma"].ToString(), fo));
                            //                cell.SetHorizontalAlignment("CENTER");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["ten"].ToString(), fo_ten));
                            //                cell.SetHorizontalAlignment("LEFT");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["lotNo"].ToString(), fo));
                            //                cell.SetHorizontalAlignment("CENTER");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(FormatString.ForMatNumber(dtSP.Rows[checkSoDong]["soluongtieuchuan"].ToString()) + " ", fo));
                            //                cell.SetHorizontalAlignment("RIGHT");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(FormatString.ForMatNumber(dtSP.Rows[checkSoDong]["box"].ToString()) + " ", fo));
                            //                cell.SetHorizontalAlignment("RIGHT");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(FormatString.ForMatNumber(dtSP.Rows[checkSoDong]["soluong"].ToString()) + " ", fo));
                            //                cell.SetHorizontalAlignment("RIGHT");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["dvt"].ToString(), fo));
                            //                cell.SetHorizontalAlignment("CENTER");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(FormatString.ForMatNumber((quycach.ToString())) + " ", fo));
                            //                cell.SetHorizontalAlignment("RIGHT");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                checkSoDong++;
                            //            }
                            //            #endregion
                            //        }
                            //        #endregion

                            //        if(checkSoDong == defineLine)
                            //        {
                            //            cell = new Cell(new Phrase("Total", new Font(bf, 9.0f, Font.BOLD)));
                            //            cell.SetHorizontalAlignment("CENTER");
                            //            cell.SetVerticalAlignment("middle");
                            //            cell.BorderWidth = 0.01f;
                            //            cell.Colspan = 5;
                            //            aTable.AddCell(cell);

                            //            cell = new Cell(new Phrase(FormatString.ForMatNumber(totalBox.ToString()) + " ", new Font(bf, 9.0f, Font.BOLD)));
                            //            cell.SetHorizontalAlignment("RIGHT");
                            //            cell.SetVerticalAlignment("middle");
                            //            cell.BorderWidth = 0.01f;
                            //            aTable.AddCell(cell);

                            //            cell = new Cell(new Phrase(FormatString.ForMatNumber(totalQty.ToString()) + " ", new Font(bf, 9.0f, Font.BOLD)));
                            //            cell.SetHorizontalAlignment("RIGHT");
                            //            cell.SetVerticalAlignment("middle");
                            //            cell.BorderWidth = 0.01f;
                            //            aTable.AddCell(cell);

                            //            cell = new Cell(new Phrase("", new Font(bf, 9.0f, Font.BOLD)));
                            //            cell.SetHorizontalAlignment("CENTER");
                            //            cell.SetVerticalAlignment("middle");
                            //            cell.BorderWidth = 0.01f;
                            //            aTable.AddCell(cell);
                            //        }
                            //    }

                            //    content.Add(aTable);
                                
                            //    #region Thong tin duoi
                            //    if (checkSoDong == defineLine)
                            //    {

                            //        aTable = new iTextSharp.text.Table(3);
                            //        aTable.AutoFillEmptyCells = true;
                            //        aTable.Alignment = Element.ALIGN_LEFT;
                            //        aTable.Width = 100.0f;
                            //        aTable.Cellpadding = 0.5f;
                            //        aTable.Cellspacing = 0.5f;
                            //        aTable.Widths = new float[] { 20.0f, 20.0f, 20.0f };
                            //        aTable.Border = 0;

                            //        ////////////////////////////////
                            //        ////THEM THONG TIN O DUOI///////
                            //        ////////////////////////////////


                            //        cell = new Cell(new Phrase(dtInfo.Rows[0]["khoxuat"].ToString() + ", " + " Day " + DateTime.Now.ToString("dd") + " Month " + DateTime.Now.ToString("MM") + " Year " + DateTime.Now.Year.ToString(), new Font(bf, 11.0f, Font.ITALIC)));
                            //        cell.SetHorizontalAlignment("RIGHT");
                            //        cell.SetVerticalAlignment("middle");
                            //        cell.BorderWidth = 0;
                            //        cell.Colspan = 5;
                            //        aTable.AddCell(cell);

                            //        cell = new Cell(new Phrase("Delivery", new Font(bf, 10.0f, Font.BOLD)));
                            //        cell.SetHorizontalAlignment("CENTER");
                            //        cell.SetVerticalAlignment("middle");
                            //        cell.BorderWidth = 0;
                            //        aTable.AddCell(cell);

                            //        cell = new Cell(new Phrase("Receiptent", new Font(bf, 10.0f, Font.BOLD)));
                            //        cell.SetHorizontalAlignment("CENTER");
                            //        cell.SetVerticalAlignment("middle");
                            //        cell.BorderWidth = 0;
                            //        aTable.AddCell(cell);

                            //        cell = new Cell(new Phrase("Supervisor", new Font(bf, 10.0f, Font.BOLD)));
                            //        cell.SetHorizontalAlignment("CENTER");
                            //        cell.SetVerticalAlignment("middle");
                            //        cell.BorderWidth = 0;
                            //        aTable.AddCell(cell);

                            //        cell = new Cell(new Phrase("(Signature)", new Font(bf, 10.0f, Font.ITALIC)));
                            //        cell.SetHorizontalAlignment("CENTER");
                            //        cell.SetVerticalAlignment("middle");
                            //        cell.BorderWidth = 0;
                            //        aTable.AddCell(cell);

                            //        cell = new Cell(new Phrase("(Signature)", new Font(bf, 10.0f, Font.ITALIC)));
                            //        cell.SetHorizontalAlignment("CENTER");
                            //        cell.SetVerticalAlignment("middle");
                            //        cell.BorderWidth = 0;
                            //        aTable.AddCell(cell);

                            //        cell = new Cell(new Phrase("(Signature)", new Font(bf, 10.0f, Font.ITALIC)));
                            //        cell.SetHorizontalAlignment("CENTER");
                            //        cell.SetVerticalAlignment("middle");
                            //        cell.BorderWidth = 0;
                            //        aTable.AddCell(cell);

                            //        content.Add(aTable);

                            //    }
                            //    pdfDoc.Add(content);

                            //    #endregion

                            //    // Xoa data in, tao content trong
                            //    content.Clear();
                            //    content.Clone();

                            //    pdfDoc.NewPage();

                            //    #endregion
                            //}
                            //else if (loaiFile.Equals("2"))
                            //{
                            //    #region Xuat kho hang ban
                            //    aTable = new iTextSharp.text.Table(7);
                            //    aTable.AutoFillEmptyCells = true;
                            //    aTable.Alignment = Element.ALIGN_LEFT;
                            //    aTable.Width = 100.0f;
                            //    aTable.Cellpadding = 1.0f;
                            //    aTable.Cellspacing = 1.0f;
                            //    aTable.Widths = new float[] { 5.0f, 12.0f, 20.0f, 8.0f, 10.0f, 10.0f, 8.0f };
                            //    aTable.BorderWidth = 0.01f;

                            //    cell = new Cell(new Phrase("No", new Font(bf, 9.0f, Font.BOLD)));
                            //    cell.SetHorizontalAlignment("CENTER");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    aTable.AddCell(cell);

                            //    cell = new Cell(new Phrase("Part Code", new Font(bf, 9.0f, Font.BOLD)));
                            //    cell.SetHorizontalAlignment("CENTER");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    aTable.AddCell(cell);

                            //    cell = new Cell(new Phrase("Part Name", new Font(bf, 9.0f, Font.BOLD)));
                            //    cell.SetHorizontalAlignment("CENTER");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    aTable.AddCell(cell);

                            //    cell = new Cell(new Phrase("Unit", new Font(bf, 9.0f, Font.BOLD)));
                            //    cell.SetHorizontalAlignment("CENTER");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    aTable.AddCell(cell);

                            //    cell = new Cell(new Phrase("Location", new Font(bf, 9.0f, Font.BOLD)));
                            //    cell.SetHorizontalAlignment("CENTER");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    aTable.AddCell(cell);

                            //    cell = new Cell(new Phrase("Lot No", new Font(bf, 9.0f, Font.BOLD)));
                            //    cell.SetHorizontalAlignment("CENTER");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    aTable.AddCell(cell);

                            //    //cell = new Cell(new Phrase("Qty/Packed", new Font(bf, 9.0f, Font.BOLD)));
                            //    //cell.SetHorizontalAlignment("CENTER");
                            //    //cell.SetVerticalAlignment("middle");
                            //    //cell.BorderWidth = 0.01f;
                            //    //aTable.AddCell(cell);

                            //    //cell = new Cell(new Phrase("Boxs", new Font(bf, 9.0f, Font.BOLD)));
                            //    //cell.SetHorizontalAlignment("CENTER");
                            //    //cell.SetVerticalAlignment("middle");
                            //    //cell.BorderWidth = 0.01f;
                            //    //aTable.AddCell(cell);

                            //    cell = new Cell(new Phrase("Quantity", new Font(bf, 9.0f, Font.BOLD)));
                            //    cell.SetHorizontalAlignment("CENTER");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    aTable.AddCell(cell);
                                
                            //    //cell = new Cell(new Phrase("Exchange", new Font(bf, 9.0f, Font.BOLD)));
                            //    //cell.SetHorizontalAlignment("CENTER");
                            //    //cell.SetVerticalAlignment("middle");
                            //    //cell.BorderWidth = 0.01f;
                            //    //aTable.AddCell(cell);

                            //    #region truy van SQL

                            //    //CHIẾT KHẤU VÀ KM ĐÃ TRỪ VÀO ĐƠN GIÁ CHƯA VAT
                            //    query = " 	SELECT COUNT(*) box, sp.ma, sp.ten, dh_sp.lotNo, dv.ten as dvt," +
                            //    "       loc.ma AS location, '' AS xuatxu, sp.soluongtieuchuan, ISNULL((SELECT TOP(1) soluong2 FROM QuyCach WHERE sanpham_fk = dh_sp.sanpham_fk), 1) quycach, SUM(dh_sp.soluong) soluong  " +
                            //    " 	FROM DonHang_SanPham_ChiTiet dh_sp INNER JOIN  SanPham sp on dh_sp.sanpham_fk = sp.pk_seq  AND dh_sp.donhang_fk = '" + id + "' " +
                            //    //"       INNER JOIN XuatXu xx ON dh_sp.xuatxu_fk = xx.pk_seq " +
                            //    "       LEFT JOIN Location loc ON dh_sp.location_fk = loc.pk_seq " +
                            //    " 		INNER JOIN  DonViTinh dv on dh_sp.dvt_fk = dv.pk_seq " +
                            //    " 	WHERE sp.pk_seq > 0 " +
                            //    "   GROUP BY dh_sp.sanpham_fk, sp.ma, sp.ten, dh_sp.lotNo, sp.soluongtieuchuan, dv.ten, loc.ma " +
                            //    "   ORDER BY sp.ma, loc.ma ASC ";
                            //    dtSP = xl.ReadTable(query);

                            //    double quycach = 0;
                            //    #endregion

                            //    if (sodongDH == 0)
                            //    {
                            //        sodongDH = dtSP.Rows.Count;
                            //        // tinh so lan lap
                            //        countPDF = (Math.Round(sodongDH / 18.0 + 0.499));
                            //    }

                            //    defineLine = dtSP.Rows.Count;

                            //    if (defineLine > 0)
                            //    {
                            //        Font fo = new Font(bf, 9.0f, Font.NORMAL);
                            //        Font fo_ten = new Font(bf, 9.0f, Font.NORMAL);

                            //        #region moi lan in trang, in 18 dong - don hang ban

                            //        sodongDH = defineLine - count * 18;
                            //        if (sodongDH < 18)
                            //        {
                            //            #region In don hang
                            //            for (int i = 0; i < sodongDH; i++)
                            //            {
                            //                totalBox += double.Parse(dtSP.Rows[checkSoDong]["box"].ToString());
                            //                totalQty += double.Parse(dtSP.Rows[checkSoDong]["soluong"].ToString());
                            //                quycach = double.Parse(dtSP.Rows[checkSoDong]["soluong"].ToString()) / double.Parse(dtSP.Rows[checkSoDong]["quycach"].ToString());

                            //                cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                            //                cell.SetHorizontalAlignment("CENTER");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["ma"].ToString(), fo));
                            //                cell.SetHorizontalAlignment("CENTER");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["ten"].ToString(), fo_ten));
                            //                cell.SetHorizontalAlignment("LEFT");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["dvt"].ToString(), fo));
                            //                cell.SetHorizontalAlignment("CENTER");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["location"].ToString(), fo));
                            //                cell.SetHorizontalAlignment("CENTER");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["lotNo"].ToString(), fo));
                            //                cell.SetHorizontalAlignment("CENTER");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(FormatString.ForMatNumber(dtSP.Rows[checkSoDong]["soluong"].ToString()) + " ", fo));
                            //                cell.SetHorizontalAlignment("RIGHT");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);
                                            
                            //                checkSoDong++;
                            //            }
                            //            #endregion
                            //        }
                            //        else
                            //        {
                            //            #region In don hang
                            //            for (int i = 0; i < 18; i++)
                            //            {
                            //                totalBox += double.Parse(dtSP.Rows[checkSoDong]["box"].ToString());
                            //                totalQty += double.Parse(dtSP.Rows[checkSoDong]["soluong"].ToString());
                            //                quycach = double.Parse(dtSP.Rows[checkSoDong]["soluong"].ToString()) / double.Parse(dtSP.Rows[checkSoDong]["quycach"].ToString());

                            //                cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                            //                cell.SetHorizontalAlignment("CENTER");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["ma"].ToString(), fo));
                            //                cell.SetHorizontalAlignment("CENTER");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["ten"].ToString(), fo_ten));
                            //                cell.SetHorizontalAlignment("LEFT");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["dvt"].ToString(), fo));
                            //                cell.SetHorizontalAlignment("CENTER");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["location"].ToString(), fo));
                            //                cell.SetHorizontalAlignment("CENTER");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["lotNo"].ToString(), fo));
                            //                cell.SetHorizontalAlignment("CENTER");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                cell = new Cell(new Phrase(FormatString.ForMatNumber(dtSP.Rows[checkSoDong]["soluong"].ToString()) + " ", fo));
                            //                cell.SetHorizontalAlignment("RIGHT");
                            //                cell.SetVerticalAlignment("middle");
                            //                cell.BorderWidth = 0.01f;
                            //                aTable.AddCell(cell);

                            //                checkSoDong++;
                            //            }
                            //            #endregion
                            //        }
                            //        #endregion

                            //        if (checkSoDong == defineLine)
                            //        {
                            //            cell = new Cell(new Phrase("Total", new Font(bf, 9.0f, Font.BOLD)));
                            //            cell.SetHorizontalAlignment("CENTER");
                            //            cell.SetVerticalAlignment("middle");
                            //            cell.BorderWidth = 0.01f;
                            //            cell.Colspan = 6;
                            //            aTable.AddCell(cell);

                            //            //cell = new Cell(new Phrase(FormatString.ForMatNumber(totalBox.ToString()) + " ", new Font(bf, 9.0f, Font.BOLD)));
                            //            //cell.SetHorizontalAlignment("RIGHT");
                            //            //cell.SetVerticalAlignment("middle");
                            //            //cell.BorderWidth = 0.01f;
                            //            //aTable.AddCell(cell);

                            //            cell = new Cell(new Phrase(FormatString.ForMatNumber(totalQty.ToString()) + " ", new Font(bf, 9.0f, Font.BOLD)));
                            //            cell.SetHorizontalAlignment("RIGHT");
                            //            cell.SetVerticalAlignment("middle");
                            //            cell.BorderWidth = 0.01f;
                            //            aTable.AddCell(cell);

                            //            //cell = new Cell(new Phrase("", new Font(bf, 9.0f, Font.BOLD)));
                            //            //cell.SetHorizontalAlignment("CENTER");
                            //            //cell.SetVerticalAlignment("middle");
                            //            //cell.BorderWidth = 0.01f;
                            //            //aTable.AddCell(cell);
                            //        }
                            //    }

                            //    content.Add(aTable);

                            //    #region Thong tin duoi
                            //    if (checkSoDong == defineLine)
                            //    {

                            //        aTable = new iTextSharp.text.Table(3);
                            //        aTable.AutoFillEmptyCells = true;
                            //        aTable.Alignment = Element.ALIGN_LEFT;
                            //        aTable.Width = 100.0f;
                            //        aTable.Cellpadding = 0.5f;
                            //        aTable.Cellspacing = 0.5f;
                            //        aTable.Widths = new float[] { 20.0f, 20.0f, 20.0f };
                            //        aTable.Border = 0;

                            //        ////////////////////////////////
                            //        ////THEM THONG TIN O DUOI///////
                            //        ////////////////////////////////


                            //        cell = new Cell(new Phrase(dtInfo.Rows[0]["khoxuat"].ToString() + ", " + " Day " + DateTime.Now.ToString("dd") + " Month " + DateTime.Now.ToString("MM") + " Year " + DateTime.Now.Year.ToString(), new Font(bf, 11.0f, Font.ITALIC)));
                            //        cell.SetHorizontalAlignment("RIGHT");
                            //        cell.SetVerticalAlignment("middle");
                            //        cell.BorderWidth = 0;
                            //        cell.Colspan = 5;
                            //        aTable.AddCell(cell);

                            //        cell = new Cell(new Phrase("Delivery", new Font(bf, 10.0f, Font.BOLD)));
                            //        cell.SetHorizontalAlignment("CENTER");
                            //        cell.SetVerticalAlignment("middle");
                            //        cell.BorderWidth = 0;
                            //        aTable.AddCell(cell);

                            //        cell = new Cell(new Phrase("Receiptent", new Font(bf, 10.0f, Font.BOLD)));
                            //        cell.SetHorizontalAlignment("CENTER");
                            //        cell.SetVerticalAlignment("middle");
                            //        cell.BorderWidth = 0;
                            //        aTable.AddCell(cell);

                            //        cell = new Cell(new Phrase("Supervisor", new Font(bf, 10.0f, Font.BOLD)));
                            //        cell.SetHorizontalAlignment("CENTER");
                            //        cell.SetVerticalAlignment("middle");
                            //        cell.BorderWidth = 0;
                            //        aTable.AddCell(cell);

                            //        cell = new Cell(new Phrase("(Signature)", new Font(bf, 10.0f, Font.ITALIC)));
                            //        cell.SetHorizontalAlignment("CENTER");
                            //        cell.SetVerticalAlignment("middle");
                            //        cell.BorderWidth = 0;
                            //        aTable.AddCell(cell);

                            //        cell = new Cell(new Phrase("(Signature)", new Font(bf, 10.0f, Font.ITALIC)));
                            //        cell.SetHorizontalAlignment("CENTER");
                            //        cell.SetVerticalAlignment("middle");
                            //        cell.BorderWidth = 0;
                            //        aTable.AddCell(cell);

                            //        cell = new Cell(new Phrase("(Signature)", new Font(bf, 10.0f, Font.ITALIC)));
                            //        cell.SetHorizontalAlignment("CENTER");
                            //        cell.SetVerticalAlignment("middle");
                            //        cell.BorderWidth = 0;
                            //        aTable.AddCell(cell);

                            //        content.Add(aTable);

                            //    }
                            //    pdfDoc.Add(content);

                            //    #endregion

                            //    // Xoa data in, tao content trong
                            //    content.Clear();
                            //    content.Clone();

                            //    pdfDoc.NewPage();

                            //    #endregion
                            //}
                            //else
                            {
                                #region Xuat kho hang ban
                                aTable = new iTextSharp.text.Table(8);
                                aTable.AutoFillEmptyCells = true;
                                aTable.Alignment = Element.ALIGN_LEFT;
                                aTable.Width = 100.0f;
                                aTable.Cellpadding = 1.0f;
                                aTable.Cellspacing = 1.0f;
                                aTable.Widths = new float[] { 5.0f, 18.0f, 15.0f, 10.0f, 10.0f, 10.0f, 10.0f, 8.0f };
                                aTable.BorderWidth = 0.01f;

                                cell = new Cell(new Phrase("No", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Item JP", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Item VN", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);
                                
                                cell = new Cell(new Phrase("Contract No.", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Color", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Sewing No", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Lot No", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Qty", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);

                                #region truy van SQL

                                //CHIẾT KHẤU VÀ KM ĐÃ TRỪ VÀO ĐƠN GIÁ CHƯA VAT
                                query = "SELECT dh_sp_ct.sanpham_fk, sp.ma, sp.codeEnglish, sp.ten, dh_sp_ct.solo, dh_sp_ct.sewingNo, dh_sp_ct.lotNoOrder, SUM(dh_sp_ct.soluong) soluong, " +                                
                                "   ISNULL((SELECT ten FROM MauSac WHERE pk_seq = dh_sp_ct.mausac_fk), '') mausac " +
                                "FROM DonHang_SanPham_ChiTiet dh_sp_ct INNER JOIN SanPham sp on dh_sp_ct.sanpham_fk = sp.pk_seq AND dh_sp_ct.donhang_fk = '" + id + "' " +                                
                                " WHERE dh_sp_ct.donhang_fk in (  " + id + "  ) AND dh_sp_ct.soluong > 0  " +
                                " GROUP BY dh_sp_ct.sanpham_fk, sp.ma, sp.codeEnglish, sp.ten, dh_sp_ct.solo, dh_sp_ct.sewingNo, dh_sp_ct.lotNoOrder, dh_sp_ct.mausac_fk " +
                                " ORDER BY sp.codeEnglish, dh_sp_ct.solo, dh_sp_ct.mausac_fk, dh_sp_ct.lotNoOrder ASC ";

                                dtSP = xl.ReadTable(query);

                                #endregion

                                if (sodongDH == 0)
                                {
                                    sodongDH = dtSP.Rows.Count;
                                    // tinh so lan lap
                                    countPDF = (Math.Round(sodongDH / 18.0 + 0.499));
                                }

                                defineLine = dtSP.Rows.Count;

                                if (defineLine > 0)
                                {
                                    Font fo = new Font(bf, 9.0f, Font.NORMAL);
                                    Font fo_ten = new Font(bf, 9.0f, Font.NORMAL);

                                    #region moi lan in trang, in 18 dong - don hang ban

                                    sodongDH = defineLine - count * 18;
                                    if (sodongDH < 18)
                                    {

                                        #region In don hang

                                        for (int i = 0; i < sodongDH; i++)
                                        {

                                            cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.01f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["codeEnglish"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.01f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["ma"].ToString(), fo_ten));
                                            cell.SetHorizontalAlignment("LEFT");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.01f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["solo"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.01f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["mausac"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.01f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["sewingNo"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.01f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["lotNoOrder"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.01f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(FormatString.ForMatNumber(dtSP.Rows[checkSoDong]["soluong"].ToString()), fo));
                                            cell.SetHorizontalAlignment("RIGHT");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.01f;
                                            aTable.AddCell(cell);

                                            checkSoDong++;
                                        }
                                        #endregion

                                    }
                                    else
                                    {
                                        #region In don hang

                                        for (int i = 0; i < 18; i++)
                                        {
                                            cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.01f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["codeEnglish"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.01f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["ma"].ToString(), fo_ten));
                                            cell.SetHorizontalAlignment("LEFT");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.01f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["solo"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.01f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["mausac"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.01f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["sewingNo"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.01f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["lotNoOrder"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.01f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(FormatString.ForMatNumber(dtSP.Rows[checkSoDong]["soluong"].ToString()), fo));
                                            cell.SetHorizontalAlignment("RIGHT");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.01f;
                                            aTable.AddCell(cell);

                                            checkSoDong++;
                                        }
                                        #endregion
                                    }
                                    #endregion

                                }

                                content.Add(aTable);


                                #region Thong tin duoi
                                if (checkSoDong == defineLine)
                                {

                                    aTable = new iTextSharp.text.Table(3);
                                    aTable.AutoFillEmptyCells = true;
                                    aTable.Alignment = Element.ALIGN_LEFT;
                                    aTable.Width = 100.0f;
                                    aTable.Cellpadding = 0.5f;
                                    aTable.Cellspacing = 0.5f;
                                    aTable.Widths = new float[] { 20.0f, 20.0f, 20.0f };
                                    aTable.Border = 0;

                                    ////////////////////////////////
                                    ////THEM THONG TIN O DUOI///////
                                    ////////////////////////////////


                                    cell = new Cell(new Phrase(dtInfo.Rows[0]["kho"].ToString() + ", " + " Day " + DateTime.Now.ToString("dd") + " Month " + DateTime.Now.ToString("MM") + " Year " + DateTime.Now.Year.ToString(), new Font(bf, 11.0f, Font.ITALIC)));
                                    cell.SetHorizontalAlignment("RIGHT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0;
                                    cell.Colspan = 5;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase("Delivery", new Font(bf, 10.0f, Font.BOLD)));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase("Receiptent", new Font(bf, 10.0f, Font.BOLD)));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase("Supervisor", new Font(bf, 10.0f, Font.BOLD)));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase("(Signature)", new Font(bf, 10.0f, Font.ITALIC)));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase("(Signature)", new Font(bf, 10.0f, Font.ITALIC)));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase("(Signature)", new Font(bf, 10.0f, Font.ITALIC)));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0;
                                    aTable.AddCell(cell);

                                    content.Add(aTable);

                                }
                                pdfDoc.Add(content);

                                #endregion

                                // Xoa data in, tao content trong
                                content.Clear();
                                content.Clone();

                                pdfDoc.NewPage();

                                #endregion
                            }
                            
                        }
                    }
                }
                dtInfo.Clear();
                dtSP.Clear();

                dtInfo.Clone();
                dtSP.Clone();
            }
            catch (Exception ex)
            {
                StringWriter sw = new StringWriter();

                string str = "Error when created file PDF...." + ex.Message;
                StringReader sr = new StringReader(str);

                htmlparser.Parse(sr);
            }

            pdfDoc.Close();
            context.Response.Write(pdfDoc);
            context.Response.End();

        }
       
        private void InSoanHangPDF(HttpContext context)
        {
            context.Response.ContentType = "application/pdf";

            string id = "";
            string loaiFile = "1";

            DataTable dtInfo = new DataTable();
            DataTable dtHD = new DataTable();
            DataTable dtSP = new DataTable();
            DataTable dtKM = new DataTable();
            DataTable dtSDH = new DataTable();

            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();
            if (context.Request.QueryString["loaiFile"] != null)
                loaiFile = context.Request.QueryString["loaiFile"].ToString();

            context.Response.AddHeader("content-disposition", "inline;filename=Pickup_" + id + ".pdf");
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            context.Response.Charset = Encoding.Unicode.ToString();
            context.Response.ContentEncoding = Encoding.UTF8;


            Document pdfDoc = new Document(PageSize.A4, 20.0f, 20.0f, 0.0f, 0.0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, context.Response.OutputStream);

            pdfDoc.Open();

            try
            {
                ExecuteData xl = new ExecuteData();

                BaseFont bf = BaseFont.CreateFont("c:\\windows\\fonts\\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                Font times = new Font(bf, 15.0f, Font.BOLD);
                Cell cell = new Cell();

                Paragraph content = new Paragraph();
                
                int sodongDH = 0;
                double countPDF = 1;
                int checkSoDong = 0;
                int defineLine = 0;
                int maxQtySize = 1;
                string query = " SELECT ISNULL(MAX(soluong), 0) soluong " +
                " FROM " +
                " ( " +
                "	SELECT COUNT(*) soluong, mavach " +
                "	FROM DonHang_SanPham_ChiTiet ct INNER JOIN SanPham sp ON ct.sanpham_fk = sp.pk_seq " +
                "	WHERE ct.donhang_fk = '" + id + "' AND mavach in (SELECT DISTINCT mavach FROM DonHang_SanPham_ChiTiet ct INNER JOIN SanPham sp ON ct.sanpham_fk = sp.pk_seq  WHERE (ct.pk_seq > 0 AND ct.soluong > 0 )) " +
                "	GROUP BY ct.mavach " +
                ") A ";

                object obj = xl.ExecuteScalarSQL(query);

                if (obj != null)
                    maxQtySize = int.Parse(obj.ToString());
                
                int colTabel = 7 + (2 * maxQtySize);

                for (int count = 0; count < countPDF; count++)
                {    
                    #region Khung hoa don
                    query = "SELECT a.pk_seq, ISNULL(a.dathang_fk, 0) dathang_fk, a.trangthai, a.ngaydonhang, ISNULL(a.ngaygiaohang, '') ngaygiaohang, " +
                    "  a.ghichu, ISNULL(a.sophieu, '') sophieu, ISNULL((SELECT hoten FROM KhachHang WHERE pk_seq = a.khachhang_fk), '') khachhang," +
                    "  ISNULL((SELECT tenkho FROM Kho WHERE pk_seq = a.kho_fk), '') kho " + 
                    " FROM DonHang a " +
                    " WHERE a.pk_seq = '" + id + "'";
                    
                    dtInfo = xl.ReadTable(query);

                    //iTextSharp.text.Table aTable = new iTextSharp.text.Table(3);
                    //aTable.AutoFillEmptyCells = true;
                    //aTable.Alignment = Element.ALIGN_LEFT;
                    //aTable.Width = 100.0f;
                    //aTable.Cellpadding = 0.0f;
                    //aTable.Cellspacing = 0.0f;
                    //aTable.Widths = new float[] { 30.0f, 100.0f, 100.0f };
                    //aTable.BorderWidth = 0.0f;

                    //iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(pathImage);
                    //jpg.ScaleToFit(70.0f, 100.0f);

                    //cell = new Cell(jpg);
                    //cell.Rowspan = 3;
                    //cell.BorderWidth = 0;
                    //aTable.AddCell(cell);

                    //cell = new Cell(new Phrase(congty_chinhanh, new Font(bf, 9.0f, Font.NORMAL)));
                    //cell.BorderWidth = 0;
                    //cell.Colspan = 2;
                    //aTable.AddCell(cell);

                    //cell = new Cell(new Phrase(diachi_title, new Font(bf, 10.0f, Font.NORMAL)));
                    //cell.BorderWidth = 0;
                    //cell.Colspan = 2;
                    //aTable.AddCell(cell);

                    //cell = new Cell(new Phrase("", new Font(bf, 10.0f, Font.NORMAL)));
                    //cell.BorderWidth = 0;
                    //cell.Colspan = 2;
                    //aTable.AddCell(cell);

                    //cell = new Cell(new Phrase("", new Font(bf, 10.0f, Font.NORMAL)));
                    //cell.BorderWidth = 0;
                    ////cell.Colspan = 2;
                    //aTable.AddCell(cell);

                    //pdfDoc.Add(aTable);

                    iTextSharp.text.Table aTable = new iTextSharp.text.Table(3);
                    aTable.AutoFillEmptyCells = true;
                    aTable.Alignment = Element.ALIGN_LEFT;
                    aTable.Width = 100.0f;
                    aTable.Cellpadding = 0.5f;
                    aTable.Cellspacing = 0.5f;
                    aTable.Widths = new float[] { 50.0f, 50.0f, 50.0f };
                    aTable.BorderWidth = 0.0f;

                    cell = new Cell(new Phrase("PICK UP", new Font(bf, 11.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    aTable.AddCell(cell);                                     

                    cell = new Cell(new Phrase("No: " + dtInfo.Rows[0]["sophieu"].ToString() + " - Date : " + dtInfo.Rows[0]["ngaydonhang"].ToString() + " - Delivery date : " + dtInfo.Rows[0]["ngaygiaohang"].ToString(), new Font(bf, 9.0f, Font.ITALIC)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    aTable.AddCell(cell);
                    
                    //cell = new Cell(new Phrase("From  : " + dtInfo.Rows[0]["kho"].ToString(), new Font(bf, 11.0f, Font.NORMAL)));
                    //cell.SetHorizontalAlignment("LEFT");
                    //cell.SetVerticalAlignment("middle");
                    //cell.BorderWidth = 0f;
                    //cell.Colspan = 3;
                    //aTable.AddCell(cell);

                    //cell = new Cell(new Phrase("To      : " + dtInfo.Rows[0]["khachhang"].ToString(), new Font(bf, 11.0f, Font.NORMAL)));
                    //cell.SetHorizontalAlignment("LEFT");
                    //cell.SetVerticalAlignment("middle");
                    //cell.BorderWidth = 0f;
                    //cell.Colspan = 3;
                    //aTable.AddCell(cell);

                    //cell = new Cell(new Phrase("By order: " + id, new Font(bf, 11.0f, Font.NORMAL)));
                    //cell.SetHorizontalAlignment("LEFT");
                    //cell.SetVerticalAlignment("middle");
                    //cell.BorderWidth = 0f;
                    //cell.Colspan = 1;
                    //aTable.AddCell(cell);

                    //cell = new Cell(new Phrase("Note   : " + dtInfo.Rows[0]["ghichu"].ToString(), new Font(bf, 11.0f, Font.NORMAL)));
                    //cell.SetHorizontalAlignment("LEFT");
                    //cell.SetVerticalAlignment("middle");
                    //cell.BorderWidth = 0f;
                    //cell.Colspan = 3;
                    //aTable.AddCell(cell);

                    // VIP
                    content.Add(aTable);

                    #endregion

                    if (loaiFile.Equals("1"))
                    {
                        //iTextSharp.text.Table 
                        aTable = new iTextSharp.text.Table(colTabel);
                        aTable.AutoFillEmptyCells = true;
                        aTable.Alignment = Element.ALIGN_LEFT;
                        aTable.Width = 100.0f;
                        aTable.Cellpadding = 1.0f;
                        aTable.Cellspacing = 1.0f;

                        if (maxQtySize == 1)
                            aTable.Widths = new float[] { 5.0f, 12.0f, 12.0f, 12.0f, 5.0f, 5.0f, 5.0f, 7.0f, 7.0f };
                        else if (maxQtySize == 2)
                            aTable.Widths = new float[] { 5.0f, 12.0f, 12.0f, 12.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 7.0f, 7.0f };
                        else if (maxQtySize == 3)
                            aTable.Widths = new float[] { 5.0f, 12.0f, 12.0f, 12.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 7.0f, 7.0f };
                        else if (maxQtySize == 4)
                            aTable.Widths = new float[] { 5.0f, 12.0f, 12.0f, 12.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 7.0f, 7.0f };
                        else if (maxQtySize == 5)
                            aTable.Widths = new float[] { 5.0f, 12.0f, 12.0f, 12.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 7.0f, 7.0f };
                        else 
                            aTable.Widths = new float[] { 5.0f, 12.0f, 12.0f, 12.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 7.0f, 7.0f };

                        aTable.BorderWidth = 0.01f;

                        cell = new Cell(new Phrase("No", new Font(bf, 8.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.01f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Order No", new Font(bf, 8.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.01f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Contract No.", new Font(bf, 8.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.01f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Style No", new Font(bf, 8.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.01f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Carton", new Font(bf, 8.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.01f;
                        aTable.AddCell(cell);

                        for (int i = 0; i < maxQtySize; i++)
                        {
                            cell = new Cell(new Phrase("Size", new Font(bf, 8.0f, Font.BOLD)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.01f;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase("Qty", new Font(bf, 8.0f, Font.BOLD)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.01f;
                            aTable.AddCell(cell);
                        }

                        cell = new Cell(new Phrase("Date", new Font(bf, 8.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.01f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Location", new Font(bf, 8.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.01f;
                        aTable.AddCell(cell);

                        #region truy van SQL

                        query = "SELECT * FROM ( " + 
                        " SELECT DISTINCT dh_sp_ct.sanpham_fk, dh_sp_ct.mavach, sp.ma, sp.codeEnglish, sp.ten, dh_sp_ct.plNo, dh_sp_ct.cartonNo, dh_sp_ct.lotNo, dh_sp_ct.dongia, " +
                        "   dh_sp_ct.solo, dh_sp_ct.ngaynhap, ISNULL(loc.ten, '') location " +                        
                        "FROM DonHang_SanPham_ChiTiet dh_sp_ct INNER JOIN SanPham sp on dh_sp_ct.sanpham_fk = sp.pk_seq  AND  dh_sp_ct.donhang_fk = '" + id + "' " +
                        "  LEFT JOIN Location loc ON dh_sp_ct.location_fk = loc.pk_seq " +
                        " WHERE dh_sp_ct.donhang_fk in (  " + id + "  ) AND dh_sp_ct.soluong > 0  " +
                        " ) A " +
                        " ORDER BY A.location, A.ma, A.plNo, A.lotNo, A.cartonNo ASC ";
                        
                        dtSP = xl.ReadTable(query);

                        #endregion

                        if (sodongDH == 0)
                        {
                            sodongDH = dtSP.Rows.Count;
                            // tinh so lan lap
                            countPDF = (Math.Round(sodongDH / 50.0 + 0.499));
                        }

                        defineLine = dtSP.Rows.Count;

                        if (defineLine > 0)
                        {
                            sodongDH = defineLine - count * 50;

                            Font fo = new Font(bf, 6.5f, Font.NORMAL);
                            Font fo_ten = new Font(bf, 6.5f, Font.NORMAL);
                            double dongia = 0;

                            #region moi lan in trang, in 50 dong - don hang ban

                            if (sodongDH < 50)
                            {
                                #region In don hang
                                for (int i = 0; i < sodongDH; i++)
                                {
                                    query = "SELECT soluong, ISNULL((SELECT ten FROM Size WHERE pk_seq = mausac_fk), '') size" +
                                        " FROM DonHang_SanPham_ChiTiet " +
                                        " WHERE donhang_fk = '" + id + "' AND mavach = N'" + dtSP.Rows[checkSoDong]["mavach"].ToString() + "' " +
                                        " AND sanpham_fk = '" + dtSP.Rows[checkSoDong]["sanpham_fk"].ToString() + "'  " +
                                        " AND plNo = N'" + dtSP.Rows[checkSoDong]["plNo"].ToString() + "'  " +
                                        " AND solo = '" + dtSP.Rows[checkSoDong]["solo"].ToString() + "'  " +
                                        " AND ngaynhap = '" + dtSP.Rows[checkSoDong]["ngaynhap"].ToString() + "'  ";

                                    DataTable dtCT = xl.ReadTable(query);

                                    cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.01f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["lotNo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.01f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["solo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.01f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["ma"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.01f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["cartonNo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.01f;
                                    aTable.AddCell(cell);

                                    for (int j = 0; j < dtCT.Rows.Count; j++)
                                    {
                                        ///
                                        cell = new Cell(new Phrase(dtCT.Rows[j]["size"].ToString(), fo));
                                        cell.SetHorizontalAlignment("CENTER");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);

                                        cell = new Cell(new Phrase(FormatString.ForMatNumber(dtCT.Rows[j]["soluong"].ToString()), fo));
                                        cell.SetHorizontalAlignment("RIGHT");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);
                                        //
                                    }

                                    for (int j = 0; j < maxQtySize - dtCT.Rows.Count; j++)
                                    {
                                        ///
                                        cell = new Cell(new Phrase("", fo));
                                        cell.SetHorizontalAlignment("CENTER");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);

                                        cell = new Cell(new Phrase("", fo));
                                        cell.SetHorizontalAlignment("RIGHT");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);
                                        //
                                    }

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["ngaynhap"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.01f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["location"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.01f;
                                    aTable.AddCell(cell);

                                    checkSoDong++;

                                }
                                #endregion
                            }
                            else
                            {
                                #region In don hang
                                for (int i = 0; i < 50; i++)
                                {
                                    query = "SELECT soluong, ISNULL((SELECT ten FROM Size WHERE pk_seq = mausac_fk), '') size" +
                                        " FROM DonHang_SanPham_ChiTiet " +
                                        " WHERE donhang_fk = '" + id + "' AND mavach = N'" + dtSP.Rows[checkSoDong]["mavach"].ToString() + "' " +
                                        " AND sanpham_fk = '" + dtSP.Rows[checkSoDong]["sanpham_fk"].ToString() + "'  " +
                                        " AND plNo = N'" + dtSP.Rows[checkSoDong]["plNo"].ToString() + "'  " +
                                        " AND solo = '" + dtSP.Rows[checkSoDong]["solo"].ToString() + "'  " +
                                        " AND ngaynhap = '" + dtSP.Rows[checkSoDong]["ngaynhap"].ToString() + "'  ";

                                    DataTable dtCT = xl.ReadTable(query);

                                    cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.01f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["lotNo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.01f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["solo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.01f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["ma"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.01f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["cartonNo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.01f;
                                    aTable.AddCell(cell);

                                    for (int j = 0; j < dtCT.Rows.Count; j++)
                                    {
                                        ///
                                        cell = new Cell(new Phrase(dtCT.Rows[j]["size"].ToString(), fo));
                                        cell.SetHorizontalAlignment("CENTER");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);

                                        cell = new Cell(new Phrase(FormatString.ForMatNumber(dtCT.Rows[j]["soluong"].ToString()), fo));
                                        cell.SetHorizontalAlignment("RIGHT");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);
                                        //
                                    }

                                    for (int j = 0; j < maxQtySize - dtCT.Rows.Count; j++)
                                    {
                                        ///
                                        cell = new Cell(new Phrase("", fo));
                                        cell.SetHorizontalAlignment("CENTER");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);

                                        cell = new Cell(new Phrase("", fo));
                                        cell.SetHorizontalAlignment("RIGHT");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);
                                        //
                                    }

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["ngaynhap"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.01f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["location"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.01f;
                                    aTable.AddCell(cell);
                                   
                                    checkSoDong++;
                                }
                                #endregion
                            }

                            #endregion
                        }

                        content.Add(aTable);
                    }
                  
                    #region Thong tin duoi
                    if (checkSoDong == defineLine)
                    {
                        aTable = new iTextSharp.text.Table(4);
                        aTable.AutoFillEmptyCells = true;
                        aTable.Alignment = Element.ALIGN_LEFT;
                        aTable.Width = 100.0f;
                        aTable.Cellpadding = 0.5f;
                        aTable.Cellspacing = 0.5f;
                        aTable.Widths = new float[] { 25.0f, 25.0f, 25.0f, 25.0f };
                        aTable.Border = 0;

                        ////////////////////////////////
                        ////THEM THONG TIN O DUOI///////
                        ////////////////////////////////


                        cell = new Cell(new Phrase(dtInfo.Rows[0]["kho"].ToString() + ", Day " + DateTime.Now.ToString("dd") + " Month " + DateTime.Now.ToString("MM") + " Year " + DateTime.Now.Year.ToString(), new Font(bf, 11.0f, Font.ITALIC)));
                        cell.SetHorizontalAlignment("RIGHT");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0;
                        cell.Colspan = 4;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Compiler", new Font(bf, 10.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0;
                        aTable.AddCell(cell);
                        
                        cell = new Cell(new Phrase("Supervisor", new Font(bf, 10.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Receiver", new Font(bf, 10.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Stocker", new Font(bf, 10.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0;
                        aTable.AddCell(cell);

                        content.Add(aTable);
                    }

                    
                    pdfDoc.Add(content);

                    #endregion

                    // Xoa data in, tao content trong
                    content.Clear();
                    content.Clone();
                    
                    pdfDoc.NewPage();
                }

                dtHD.Clear();
                dtInfo.Clear();
                dtKM.Clear();
                dtSP.Clear();

                dtHD.Clone();
                dtInfo.Clone();
                dtKM.Clone();
                dtSP.Clone();
            }
            catch (Exception ex)
            {
                StringWriter sw = new StringWriter();

                string str = "Error when created file PDF...." + ex.Message;
                StringReader sr = new StringReader(str);

                htmlparser.Parse(sr);
            }

            pdfDoc.Close();
            context.Response.Write(pdfDoc);
            context.Response.End();

        }

        private void PickupPrintPDF(HttpContext context)
        {
            context.Response.ContentType = "application/pdf";

            string loaiFile = "1";

            if (context.Request.QueryString["loaiFile"] != null)
                loaiFile = context.Request.QueryString["loaiFile"].ToString();

            context.Response.AddHeader("content-disposition", "inline;filename=Pickup.pdf");
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            context.Response.Charset = Encoding.Unicode.ToString();
            context.Response.ContentEncoding = Encoding.UTF8;


            Document pdfDoc = new Document(PageSize.A4, 5.0f, 5.0f, 0.0f, 0.0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, context.Response.OutputStream);

            pdfDoc.Open();

            try
            {
                ExecuteData xl = new ExecuteData();

                BaseFont bf = BaseFont.CreateFont("c:\\windows\\fonts\\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                Font times = new Font(bf, 15.0f, Font.BOLD);
                Cell cell = new Cell();

                Paragraph content = new Paragraph();

                string query = " SELECT DISTINCT b.location_fk, ISNULL(loc.ten, '') tenLocation, b.PLNo, b.sewingNo, b.solo, " +
                " ISNULL((SELECT ten FROM MauSac WHERE pk_seq = b.mausac_fk), '') color " +
                " FROM DonHang a INNER JOIN DonHang_SanPham_ChiTiet b ON a.pk_seq = b.donhang_fk " +
                " LEFT JOIN Location loc ON b.location_fk = loc.pk_seq " +
                " WHERE a.trangthai in (0, 1) AND b.scanByPDA = 0 " +
                " ORDER BY tenLocation, b.PLNo ";

                DataTable dt = xl.ReadTable(query);

                iTextSharp.text.Table aTable = new iTextSharp.text.Table(10);
                aTable.AutoFillEmptyCells = true;
                aTable.Alignment = Element.ALIGN_LEFT;
                aTable.Width = 100.0f;
                aTable.Cellpadding = 0.0f;
                aTable.Cellspacing = 0.0f;
                aTable.Widths = new float[] { 8.0f, 8.0f, 8.0f, 8.0f, 30.0f, 5.0f, 6.0f, 5.0f, 8.0f, 8.0f };
                aTable.BorderWidth = 0.0f;

                #region Title
                cell = new Cell(new Phrase("PICK UP", new Font(bf, 18.0f, Font.BOLD)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0;
                cell.Colspan = 10;
                aTable.AddCell(cell);

                if (dt.Rows.Count > 0)
                {
                    cell = new Cell(new Phrase("Contract No: " + dt.Rows[0]["solo"].ToString(), new Font(bf, 9.0f, Font.NORMAL)));
                    cell.SetHorizontalAlignment("LEFT");
                    cell.SetVerticalAlignment("middle");
                    aTable.Cellpadding = 0.5f;
                    aTable.Cellspacing = 0.5f;
                    cell.BorderWidthBottom = 0.1f;
                    cell.Colspan = 10;
                    aTable.AddCell(cell);
                }
                else
                {
                    cell = new Cell(new Phrase("Contract No: ", new Font(bf, 9.0f, Font.NORMAL)));
                    cell.SetHorizontalAlignment("LEFT");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidthBottom = 0.1f;
                    cell.Colspan = 10;
                    aTable.AddCell(cell);
                }

                cell = new Cell(new Phrase("", new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("Carton", new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("No of", new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("Lot", new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("Color", new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("", new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("Qty", new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("S/V", new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("NET", new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("GROSS", new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                aTable.AddCell(cell);

                //
                cell = new Cell(new Phrase("", new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("No", new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("Carton", new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("No.", new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("", new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("ROLL", new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("(M)", new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("(M)", new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("WEIGHT", new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                cell.Colspan = 2;
                aTable.AddCell(cell);

                #endregion

                content.Add(aTable);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //
                    #region Title PLNo
                    cell = new Cell(new Phrase("", new Font(bf, 9.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidthTop = 0f;                    
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase(dt.Rows[i]["PLNo"].ToString(), new Font(bf, 9.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 3;
                    aTable.AddCell( cell);

                    cell = new Cell(new Phrase(dt.Rows[i]["color"].ToString() + "(" + dt.Rows[i]["sewingNo"].ToString() + ")", new Font(bf, 9.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("LEFT");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidthBottom = 0.1f;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("", new Font(bf, 9.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 5;
                    aTable.AddCell(cell);

                    #endregion
                    query = " SELECT  a.ngaydonhang, b.sewingNo, b.PLNo, b.lotNo, b.cartonNo, b.solo, b.yards, b.roll, b.soluong, b.mavach, b.netWeight, b.grossWeight, " +
                    " ISNULL((SELECT ten FROM Location WHERE pk_seq = b.location_fk), '') location, " +
                    " ISNULL((SELECT ten FROM Pallet WHERE pk_seq = b.pallet_fk), '') pallet, " +
                    " ISNULL((SELECT ten FROM MauSac WHERE pk_seq = b.mausac_fk), '') color " +
                    " FROM DonHang a INNER JOIN DonHang_SanPham_ChiTiet b ON a.pk_seq = b.donhang_fk AND b.location_fk = '" + dt.Rows[i]["location_fk"].ToString() + "' AND b.PLNo = N'" + dt.Rows[i]["PLNo"].ToString() + "' " +
                    //" INNER JOIN SanPham s ON b.sanpham_fk = s.pk_seq " +
                    " WHERE a.trangthai in (0, 1) AND b.scanByPDA = 0 " +
                    " ORDER BY b.stt, b.lotNo, b.cartonNo ";

                    DataTable dtDetail = xl.ReadTable(query);

                    double NoofTOTAL = 0;
                    double rollTOTAL = 0;
                    double qtyTOTAL = 0;
                    double netWeightTOTAL = 0;
                    double grossWeightTOTAL = 0;

                    for (int j = 0; j < dtDetail.Rows.Count; j++)
                    {
                        NoofTOTAL += 1;
                        rollTOTAL += double.Parse(dtDetail.Rows[j]["roll"].ToString());
                        qtyTOTAL += double.Parse(dtDetail.Rows[j]["soluong"].ToString());
                        netWeightTOTAL += double.Parse(dtDetail.Rows[j]["netWeight"].ToString());
                        grossWeightTOTAL += double.Parse(dtDetail.Rows[j]["grossWeight"].ToString());

                        if (j > 0 && j < dtDetail.Rows.Count && dtDetail.Rows[j]["location"].ToString().Equals(dtDetail.Rows[j - 1]["location"].ToString()))
                            cell = new Cell(new Phrase("", new Font(bf, 9.0f, Font.NORMAL)));
                        else if (j == dtDetail.Rows.Count && dtDetail.Rows[j - 1]["location"].ToString().Equals(dtDetail.Rows[j]["location"].ToString()))
                            cell = new Cell(new Phrase("", new Font(bf, 9.0f, Font.NORMAL)));
                        else
                            cell = new Cell(new Phrase(dtDetail.Rows[j]["location"].ToString(), new Font(bf, 9.0f, Font.NORMAL)));

                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase(dtDetail.Rows[j]["cartonNo"].ToString(), new Font(bf, 9.0f, Font.NORMAL)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("1", new Font(bf, 9.0f, Font.NORMAL)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0f;
                        aTable.AddCell(cell);

                        if (j > 0 && j < dtDetail.Rows.Count && dtDetail.Rows[j]["lotNo"].ToString().Equals(dtDetail.Rows[j - 1]["lotNo"].ToString()))
                            cell = new Cell(new Phrase("", new Font(bf, 9.0f, Font.NORMAL)));
                        else if(j == dtDetail.Rows.Count && dtDetail.Rows[j - 1]["lotNo"].ToString().Equals(dtDetail.Rows[j]["lotNo"].ToString()))
                            cell = new Cell(new Phrase("", new Font(bf, 9.0f, Font.NORMAL)));
                        else
                            cell = new Cell(new Phrase(dtDetail.Rows[j]["lotNo"].ToString(), new Font(bf, 9.0f, Font.NORMAL)));

                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase(FormatString.ForMatNumber(dtDetail.Rows[j]["soluong"].ToString()), new Font(bf, 9.0f, Font.NORMAL)));
                        cell.SetHorizontalAlignment("LEFT");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase(dtDetail.Rows[j]["roll"].ToString(), new Font(bf, 9.0f, Font.NORMAL)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase(FormatString.ForMatNumber(dtDetail.Rows[j]["soluong"].ToString()), new Font(bf, 9.0f, Font.NORMAL)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("", new Font(bf, 9.0f, Font.NORMAL)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase(FormatString.ForMatNumber(dtDetail.Rows[j]["netWeight"].ToString()), new Font(bf, 9.0f, Font.NORMAL)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase(FormatString.ForMatNumber(dtDetail.Rows[j]["grossWeight"].ToString()), new Font(bf, 9.0f, Font.NORMAL)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0f;
                        aTable.AddCell(cell);
                    }

                    #region Total PLNo
                    if (dtDetail.Rows.Count > 0)
                    {
                        cell = new Cell(new Phrase("", new Font(bf, 9.0f, Font.NORMAL)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidthTop = 0.1f;
                        cell.BorderWidthBottom = 0.1f;
                        cell.BackgroundColor = Color.LIGHT_GRAY;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidthTop = 0.1f;
                        cell.BorderWidthBottom = 0.1f;
                        cell.BackgroundColor = Color.LIGHT_GRAY;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase(FormatString.ForMatNumber(NoofTOTAL.ToString()), new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidthTop = 0.1f;
                        cell.BorderWidthBottom = 0.1f;
                        cell.BackgroundColor = Color.LIGHT_GRAY;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidthTop = 0.1f;
                        cell.BorderWidthBottom = 0.1f;
                        cell.BackgroundColor = Color.LIGHT_GRAY;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("S.TOTAL", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidthTop = 0.1f;
                        cell.BorderWidthBottom = 0.1f;
                        cell.BackgroundColor = Color.LIGHT_GRAY;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidthTop = 0.1f;
                        cell.BorderWidthBottom = 0.1f;
                        cell.BackgroundColor = Color.LIGHT_GRAY;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase(FormatString.ForMatNumber(qtyTOTAL.ToString()), new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidthTop = 0.1f;
                        cell.BorderWidthBottom = 0.1f;
                        cell.BackgroundColor = Color.LIGHT_GRAY;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase(FormatString.ForMatNumber(rollTOTAL.ToString()), new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidthTop = 0.1f;
                        cell.BorderWidthBottom = 0.1f;
                        cell.BackgroundColor = Color.LIGHT_GRAY;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase(FormatString.ForMatNumber(netWeightTOTAL.ToString()), new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidthTop = 0.1f;
                        cell.BorderWidthBottom = 0.1f;
                        cell.BackgroundColor = Color.LIGHT_GRAY;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase(FormatString.ForMatNumber(grossWeightTOTAL.ToString()), new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidthTop = 0.1f;
                        cell.BorderWidthBottom = 0.1f;
                        cell.BackgroundColor = Color.LIGHT_GRAY;
                        aTable.AddCell(cell);
                    }
                    #endregion

                }

                pdfDoc.Add(content);
                // Xoa data in, tao content trong
                content.Clear();
                content.Clone();

                dt.Clear();
                dt.Clone();

                pdfDoc.NewPage();
            }
            catch (Exception ex)
            {
                StringWriter sw = new StringWriter();

                string str = "Error when created file PDF...." + ex.Message;
                StringReader sr = new StringReader(str);

                htmlparser.Parse(sr);
            }

            pdfDoc.Close();
            context.Response.Write(pdfDoc);
            context.Response.End();

        }

        private void InXuatKhacPDF(HttpContext context)
        {         
            context.Response.ContentType = "application/pdf";

            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string loaiFile = "";
            if (context.Request.QueryString["loaiFile"] != null)
                loaiFile = context.Request.QueryString["loaiFile"].ToString();

            context.Response.AddHeader("content-disposition", "inline;filename=StockOut_" + id + ".pdf");

            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            context.Response.Charset = Encoding.Unicode.ToString();
            context.Response.ContentEncoding = Encoding.UTF8;
            
            Document pdfDoc = new Document(PageSize.A4, 20.0f, 20.0f, 0.0f, 0.0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, context.Response.OutputStream);

            pdfDoc.Open();

            try
            {
                ExecuteData xl = new ExecuteData();

                BaseFont bf = BaseFont.CreateFont("c:\\windows\\fonts\\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                Font times = new Font(bf, 15.0f, Font.BOLD);
                Cell cell = new Cell();

                Paragraph content = new Paragraph();

                string khoXUAT = "";
                string khuvuc = "";
                string query = " SELECT a.pk_seq, a.trangthai, a.ngayxuat, a.ghichu, a.loaixuat, ISNULL(a.sophieu, '') sophieu, " +
                "  	CASE a.loaikho WHEN 0 THEN (SELECT ma FROM KhachHang WHERE pk_seq = a.khachhang_fk)  " +
                "  				 ELSE  (SELECT makho FROM Kho WHERE pk_seq = a.khonhan_fk) END khoxuat, '' AS khuvuc,  " +
                "  	CASE a.loaixuat WHEN 2 THEN (SELECT ma FROM KhachHang WHERE pk_seq = a.khachhangnhan_fk)  " +
                "  				 ELSE  (SELECT makho FROM Kho WHERE pk_seq = a.khonhan_fk) END khonhan,   " +
                "  	CASE a.loaixuat WHEN 1 THEN N'Company' " +
                "  				 WHEN 2 THEN N'Partner' " +
                "  				 ELSE N'Unknow' END tenloaixuat, a.ScanByPDA " +
                "  FROM XuatKhac a  " +
                "  WHERE a.pk_seq = '" + id + "' ";

                DataTable dtInfo = xl.ReadTable(query);
                double totaltTT = 0;

                khuvuc = dtInfo.Rows[0]["khuvuc"].ToString();

                iTextSharp.text.Table aTable = new iTextSharp.text.Table(3);
                aTable.AutoFillEmptyCells = true;
                aTable.Alignment = Element.ALIGN_LEFT;
                aTable.Width = 100.0f;
                aTable.Cellpadding = 0.0f;
                aTable.Cellspacing = 0.0f;
                aTable.Widths = new float[] { 30.0f, 100.0f, 100.0f };
                aTable.BorderWidth = 0.0f;

                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(pathImage);
                //iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(@"E:\ISO-Data\Project\ISO-IPP\ISO\Images\HLVTimeSheet.png");
                jpg.ScaleToFit(70.0f, 70.0f);

                cell = new Cell(jpg);
                cell.Rowspan = 3;
                cell.BorderWidth = 0;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase(congty_chinhanh, new Font(bf, 9.0f, Font.NORMAL)));
                cell.BorderWidth = 0;
                cell.Colspan = 2;
                aTable.AddCell(cell);

               
                cell = new Cell(new Phrase(diachi_title, new Font(bf, 10.0f, Font.NORMAL)));
                cell.BorderWidth = 0;
                cell.Colspan = 2;
                aTable.AddCell(cell);

              
                cell = new Cell(new Phrase("Department: ", new Font(bf, 10.0f, Font.NORMAL)));
                cell.BorderWidth = 0;
                cell.Colspan = 2;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("", new Font(bf, 10.0f, Font.NORMAL)));
                cell.BorderWidth = 0;
                //cell.Colspan = 2;
                aTable.AddCell(cell);

                pdfDoc.Add(aTable);


                aTable = new iTextSharp.text.Table(4);
                aTable.AutoFillEmptyCells = true;
                aTable.Alignment = Element.ALIGN_LEFT;
                aTable.Width = 100.0f;
                aTable.Cellpadding = 0.5f;
                aTable.Cellspacing = 0.5f;
                aTable.Widths = new float[] { 20.0f, 20.0f, 20.0f, 20.0f };
                aTable.BorderWidth = 0.0f;
                
                cell = new Cell(new Phrase("DELIVERY NOTE", new Font(bf, 16.0f, Font.BOLD)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0;
                cell.Colspan = 4;
                aTable.AddCell(cell);

                if (dtInfo.Rows[0]["loaixuat"].ToString().Equals("1"))
                    cell = new Cell(new Phrase("(COMPANY)", new Font(bf, 11.0f, Font.NORMAL)));
                else if (dtInfo.Rows[0]["loaixuat"].ToString().Equals("2"))
                    cell = new Cell(new Phrase("(PARTNER)", new Font(bf, 11.0f, Font.NORMAL)));
                else 
                    cell = new Cell(new Phrase("(UNKNOW)", new Font(bf, 11.0f, Font.NORMAL)));
              
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0;
                cell.Colspan = 4;
                aTable.AddCell(cell);


                cell = new Cell(new Phrase("No: " + dtInfo.Rows[0]["sophieu"].ToString() + " - Date: " + dtInfo.Rows[0]["ngayxuat"].ToString(), new Font(bf, 9.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0;
                cell.Colspan = 4;
                aTable.AddCell(cell);               
               
                for (int j = 0; j < 5; j++)
                {
                    cell = new Cell(new Phrase("", new Font(bf, 11.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("LEFT");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 4;
                    aTable.AddCell(cell);
                }

                cell = new Cell(new Phrase("Receiver: ....................................................................................................................................................................", new Font(bf, 11.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("LEFT");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                cell.Colspan = 4;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("Address/Department: ..................................................................................................................................................", new Font(bf, 11.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("LEFT");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                cell.Colspan = 4;
                aTable.AddCell(cell);
                
                khoXUAT =  dtInfo.Rows[0]["khoxuat"].ToString();
                cell = new Cell(new Phrase("From: " + khoXUAT, new Font(bf, 11.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("LEFT");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                cell.Colspan = 2;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("To: " + dtInfo.Rows[0]["khonhan"].ToString(), new Font(bf, 11.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("LEFT");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                cell.Colspan = 2;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("Note: " + dtInfo.Rows[0]["ghichu"].ToString(), new Font(bf, 11.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("LEFT");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                cell.Colspan = 4;
                aTable.AddCell(cell);

                //////
                content.Add(aTable);

                if (loaiFile.Equals("1"))
                {
                    iTextSharp.text.Table
                    aTable_2 = new iTextSharp.text.Table(6);
                    aTable_2.AutoFillEmptyCells = true;
                    aTable_2.Alignment = Element.ALIGN_LEFT;
                    aTable_2.Width = 100.0f;
                    aTable_2.Cellpadding = 1.0f;
                    aTable_2.Cellspacing = 1.0f;
                    aTable_2.Widths = new float[] { 5.0f, 15.0f, 35.0f, 10.0f, 10.0f, 8.0f };
                    aTable_2.BorderWidth = 0.01f;

                    cell = new Cell(new Phrase("No", new Font(bf, 9.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0.1f;
                    //cell.Rowspan = 2;
                    aTable_2.AddCell(cell);

                    cell = new Cell(new Phrase("PartNo", new Font(bf, 9.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0.1f;
                    //cell.Rowspan = 2;
                    aTable_2.AddCell(cell);

                    cell = new Cell(new Phrase("Part name", new Font(bf, 9.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0.1f;
                    //cell.Rowspan = 2;
                    aTable_2.AddCell(cell);

                    cell = new Cell(new Phrase("LotNo", new Font(bf, 9.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0.1f;
                    aTable_2.AddCell(cell);

                    //cell = new Cell(new Phrase("SerialNo", new Font(bf, 9.0f, Font.BOLD)));
                    //cell.SetHorizontalAlignment("CENTER");
                    //cell.SetVerticalAlignment("middle");
                    //cell.BorderWidthRight = 0.1f;
                    //aTable_2.AddCell(cell);

                    cell = new Cell(new Phrase("Prod date", new Font(bf, 9.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidthRight = 0.1f;
                    aTable_2.AddCell(cell);

                    cell = new Cell(new Phrase("Qty", new Font(bf, 9.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0.1f;
                    aTable_2.AddCell(cell);


                    query = "SELECT xk_ct.xuatkhac_fk, sp.ma, sp.ten, (SELECT ten FROM DonViTinh WHERE pk_seq = xk_ct.dvt_fk) as donvitinh, SUM(xk_ct.soluong) soluong, ISNULL(xk_ct.dongia, 0) dongia, " +
                        "   xk_ct.solo, ISNULL(loc.ten, '') as location, ISNULL(bin.ten, '') bin, xk_ct.lotNo, ISNULL(xk_ct.quycach, 1) as quycach " +
                        " FROM XuatKhac_SanPham_ChiTiet xk_ct INNER JOIN SanPham sp ON xk_ct.sanpham_fk = sp.pk_seq " +
                            " LEFT JOIN Location loc ON xk_ct.location_fk = loc.pk_seq " +
                            " LEFT JOIN Bin bin ON xk_ct.bin_fk = bin.pk_seq " +
                        " WHERE xk_ct.xuatkhac_fk = '" + id + "' AND xk_ct.soluong > 0 " +
                        " GROUP BY xk_ct.xuatkhac_fk, sp.ma, sp.ten, xk_ct.dvt_fk, xk_ct.dongia, xk_ct.solo, loc.ten, bin.ten, xk_ct.lotNo, xk_ct.quycach ";

                    DataTable dtSP = xl.ReadTable(query);

                    if (dtSP.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtSP.Rows.Count; i++)
                        {
                            Font fo = new Font(bf, 9.0f, Font.NORMAL);
                            Font fo_ten = new Font(bf, 9.0f, Font.NORMAL);
                            //bool flag = false;

                            double totalSP = 0;
                            double soluong = double.Parse(dtSP.Rows[i]["SoLuong"].ToString());
                            double dongia = double.Parse(dtSP.Rows[i]["dongia"].ToString());

                            double quyCACH = double.Parse(dtSP.Rows[i]["quyCACH"].ToString());

                            totalSP = soluong * dongia;
                            totaltTT += totalSP;

                            cell = new Cell(new Phrase((i + 1).ToString(), fo));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.1f;
                            aTable_2.AddCell(cell);

                            cell = new Cell(new Phrase(dtSP.Rows[i]["ma"].ToString(), fo));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.1f;
                            aTable_2.AddCell(cell);

                            cell = new Cell(new Phrase(dtSP.Rows[i]["ten"].ToString(), fo_ten));
                            cell.SetHorizontalAlignment("LEFT");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.1f;
                            aTable_2.AddCell(cell);

                            //cell = new Cell(new Phrase(dtSP.Rows[i]["donvitinh"].ToString(), fo));
                            //cell.SetHorizontalAlignment("CENTER");
                            //cell.SetVerticalAlignment("middle");
                            //cell.BorderWidth = 0.1f;
                            //aTable_2.AddCell(cell);

                            cell = new Cell(new Phrase(dtSP.Rows[i]["lotNo"].ToString(), fo));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.1f;
                            aTable_2.AddCell(cell);

                            //cell = new Cell(new Phrase(dtSP.Rows[i]["serialNo"].ToString(), fo));
                            //cell.SetHorizontalAlignment("CENTER");
                            //cell.SetVerticalAlignment("middle");
                            //cell.BorderWidth = 0.1f;
                            //aTable_2.AddCell(cell);

                            cell = new Cell(new Phrase(dtSP.Rows[i]["solo"].ToString(), fo));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.1f;
                            aTable_2.AddCell(cell);

                            //cell = new Cell(new Phrase(dtSP.Rows[i]["ngaynhap"].ToString(), fo));
                            //cell.SetHorizontalAlignment("CENTER");
                            //cell.SetVerticalAlignment("middle");
                            //cell.BorderWidth = 0.1f;
                            //aTable_2.AddCell(cell);

                            cell = new Cell(new Phrase(soluong.ToString() + " ", fo));
                            cell.SetHorizontalAlignment("RIGHT");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.1f;
                            aTable_2.AddCell(cell);
                        }

                    }

                    dtSP.Clear();
                    dtSP.Clone();

                    content.Add(aTable_2);
                }
                else
                { 
                    iTextSharp.text.Table aTable_2 = new iTextSharp.text.Table(7);
                    aTable_2.AutoFillEmptyCells = true;
                    aTable_2.Alignment = Element.ALIGN_LEFT;
                    aTable_2.Width = 100.0f;
                    aTable_2.Cellpadding = 1.0f;
                    aTable_2.Cellspacing = 1.0f;
                    aTable_2.Widths = new float[] { 5.0f, 15.0f, 35.0f, 10.0f, 10.0f, 10.0f, 8.0f };
                    aTable_2.BorderWidth = 0.01f;

                    cell = new Cell(new Phrase("No", new Font(bf, 9.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0.1f;
                    //cell.Rowspan = 2;
                    aTable_2.AddCell(cell);

                    cell = new Cell(new Phrase("PartNo", new Font(bf, 9.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0.1f;
                    //cell.Rowspan = 2;
                    aTable_2.AddCell(cell);

                    cell = new Cell(new Phrase("Part name", new Font(bf, 9.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0.1f;
                    //cell.Rowspan = 2;
                    aTable_2.AddCell(cell);

                    cell = new Cell(new Phrase("LotNo", new Font(bf, 9.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0.1f;
                    aTable_2.AddCell(cell);

                    cell = new Cell(new Phrase("SerialNo", new Font(bf, 9.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidthRight = 0.1f;
                    aTable_2.AddCell(cell);

                    cell = new Cell(new Phrase("Prod date", new Font(bf, 9.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidthRight = 0.1f;
                    aTable_2.AddCell(cell);

                    cell = new Cell(new Phrase("Qty", new Font(bf, 9.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0.1f;
                    aTable_2.AddCell(cell);


                    query = "SELECT xk_ct.xuatkhac_fk, sp.ma, sp.ten, (SELECT ten FROM DonViTinh WHERE pk_seq = xk_ct.dvt_fk) as donvitinh, SUM(xk_ct.soluong) soluong, ISNULL(xk_ct.dongia, 0) dongia, " +
                        "   xk_ct.solo, ISNULL(loc.ten, '') as location, ISNULL(bin.ten, '') bin, xk_ct.lotNo, xk_ct.serialNo, ISNULL(xk_ct.quycach, 1) as quycach " +
                        " FROM XuatKhac_SanPham_ChiTiet xk_ct INNER JOIN SanPham sp ON xk_ct.sanpham_fk = sp.pk_seq " +
                            " LEFT JOIN Location loc ON xk_ct.location_fk = loc.pk_seq " +
                            " LEFT JOIN Bin bin ON xk_ct.bin_fk = bin.pk_seq " +
                        " WHERE xk_ct.xuatkhac_fk = '" + id + "' AND xk_ct.soluong > 0 " +
                        " GROUP BY xk_ct.xuatkhac_fk, sp.ma, sp.ten, xk_ct.dvt_fk, xk_ct.dongia, xk_ct.solo, xk_ct.serialNo, loc.ten, bin.ten, xk_ct.lotNo, xk_ct.quycach ";

                    DataTable dtSP = xl.ReadTable(query);

                    if (dtSP.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtSP.Rows.Count; i++)
                        {
                            Font fo = new Font(bf, 9.0f, Font.NORMAL);
                            Font fo_ten = new Font(bf, 9.0f, Font.NORMAL);
                            //bool flag = false;

                            double totalSP = 0;
                            double soluong = double.Parse(dtSP.Rows[i]["SoLuong"].ToString());
                            double dongia = double.Parse(dtSP.Rows[i]["dongia"].ToString());

                            double quyCACH = double.Parse(dtSP.Rows[i]["quyCACH"].ToString());

                            totalSP = soluong * dongia;
                            totaltTT += totalSP;

                            cell = new Cell(new Phrase((i + 1).ToString(), fo));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.1f;
                            aTable_2.AddCell(cell);

                            cell = new Cell(new Phrase(dtSP.Rows[i]["ma"].ToString(), fo));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.1f;
                            aTable_2.AddCell(cell);

                            cell = new Cell(new Phrase(dtSP.Rows[i]["ten"].ToString(), fo_ten));
                            cell.SetHorizontalAlignment("LEFT");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.1f;
                            aTable_2.AddCell(cell);

                            //cell = new Cell(new Phrase(dtSP.Rows[i]["donvitinh"].ToString(), fo));
                            //cell.SetHorizontalAlignment("CENTER");
                            //cell.SetVerticalAlignment("middle");
                            //cell.BorderWidth = 0.1f;
                            //aTable_2.AddCell(cell);

                            cell = new Cell(new Phrase(dtSP.Rows[i]["lotNo"].ToString(), fo));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.1f;
                            aTable_2.AddCell(cell);

                            cell = new Cell(new Phrase(dtSP.Rows[i]["serialNo"].ToString(), fo));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.1f;
                            aTable_2.AddCell(cell);

                            cell = new Cell(new Phrase(dtSP.Rows[i]["solo"].ToString(), fo));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.1f;
                            aTable_2.AddCell(cell);

                            //cell = new Cell(new Phrase(dtSP.Rows[i]["ngaynhap"].ToString(), fo));
                            //cell.SetHorizontalAlignment("CENTER");
                            //cell.SetVerticalAlignment("middle");
                            //cell.BorderWidth = 0.1f;
                            //aTable_2.AddCell(cell);

                            cell = new Cell(new Phrase(soluong.ToString() + " ", fo));
                            cell.SetHorizontalAlignment("RIGHT");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.1f;
                            aTable_2.AddCell(cell);
                        }

                    }
                    dtSP.Clear();
                    dtSP.Clone();

                    content.Add(aTable_2);
                }
                
                aTable = new iTextSharp.text.Table(4);
                aTable.AutoFillEmptyCells = true;
                aTable.Alignment = Element.ALIGN_LEFT;
                aTable.Width = 100.0f;
                aTable.Cellpadding = 0.5f;
                aTable.Cellspacing = 0.5f;
                aTable.Widths = new float[] { 25.0f, 25.0f, 25.0f, 25.0f };
                aTable.Border = 0;

                cell = new Cell(new Phrase("Number of accompanying original documents:............................................................................................................", new Font(bf, 11.0f, Font.NORMAL)));
                cell.SetHorizontalAlignment("LEFT");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0f;
                cell.Colspan = 4;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase(khoXUAT + ", Date " + DateTime.Now.Day.ToString() + " Month " + DateTime.Now.Month.ToString() + " Year " + DateTime.Now.Year.ToString(), new Font(bf, 11.0f, Font.ITALIC)));
                cell.SetHorizontalAlignment("RIGHT");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0;
                cell.Colspan = 4;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("Manager", new Font(bf, 11.0f, Font.BOLD)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("Inventory management", new Font(bf, 11.0f, Font.BOLD)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("Stocker", new Font(bf, 11.0f, Font.BOLD)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("Receiver", new Font(bf, 11.0f, Font.BOLD)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("(Signature)", new Font(bf, 11.0f, Font.ITALIC)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("(Signature)", new Font(bf, 11.0f, Font.ITALIC)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                aTable.AddCell(cell);

                cell = new Cell(new Phrase("(Signature)", new Font(bf, 11.0f, Font.ITALIC)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                aTable.AddCell(cell);
              
                cell = new Cell(new Phrase("(Signature)", new Font(bf, 11.0f, Font.ITALIC)));
                cell.SetHorizontalAlignment("CENTER");
                cell.SetVerticalAlignment("middle");
                cell.BorderWidth = 0;
                cell.Colspan = 1;
                aTable.AddCell(cell);
                
                dtInfo.Clear();
                dtInfo.Clone();

                content.Add(aTable);
                pdfDoc.Add(content);

            }
            catch (Exception ex)
            {
                StringWriter sw = new StringWriter();

                string str = "Error when created file PDF...." + ex.Message;
                StringReader sr = new StringReader(str);

                htmlparser.Parse(sr);
            }

            pdfDoc.Close();
            context.Response.Write(pdfDoc);
            context.Response.End();
        }

        private void InNhapHangPDF(HttpContext context)
        {
            context.Response.ContentType = "application/pdf";

            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string loaiFile = "";
            if (context.Request.QueryString["loaiFile"] != null)
                loaiFile = context.Request.QueryString["loaiFile"].ToString();

            context.Response.AddHeader("content-disposition", "inline;filename=NhapHang_" + id + ".pdf");
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            context.Response.Charset = Encoding.Unicode.ToString();
            context.Response.ContentEncoding = Encoding.UTF8;


            Document pdfDoc = new Document(PageSize.A4, 20.0f, 20.0f, 10.0f, 10.0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, context.Response.OutputStream);

            pdfDoc.Open();            

            try
            {
                ExecuteData xl = new ExecuteData();
                DataTable dtSP = new DataTable();
                BaseFont bf = BaseFont.CreateFont("c:\\windows\\fonts\\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                Font times = new Font(bf, 15.0f, Font.BOLD);
                Cell cell = new Cell();

                Paragraph content = new Paragraph();

                string khoNHAP = "";
                string ngaynhap = "";
                
                for (int loop = 0; loop < 1; loop++)
                {

                    int sodongDH = 0;
                    int checkSoDong = 0;
                    int defineLine = 0;
                    double countPDF = 1;
                    bool checkPage = true;
                    double totalBox = 0;
                    double totalQty = 0;

                    for (int count = 0; count < countPDF; count++)
                    {
                        string query = "SELECT a.pk_seq, a.kho_fk, a.trangthai, a.ngaynhap, a.ghichu, ISNULL(a.sophieu, '') sophieu, " +
                        " CASE a.loainhap WHEN 0 THEN N'Unknow' " +
                        "				WHEN 1 THEN N'Into storage' " +
                        "				WHEN 2 THEN N'Temporary location' ELSE N'Unknow' END loainhap, " +
                        " ISNULL((SELECT makho FROM Kho WHERE pk_seq = a.kho_fk), '') as khonhap, " +
                        " ISNULL((SELECT hoten FROM KhachHang WHERE pk_seq = a.khachhang_fk), '') as khachhang, " +
                        " ISNULL((SELECT ten FROM NhaCungCap WHERE pk_seq = a.nhacungcap_fk), '') as nhacungcap " +
                        " FROM NhapHang a " +
                        " WHERE a.pk_seq = '" + id + "' ";

                        DataTable dtInfo = xl.ReadTable(query);

                        ngaynhap = dtInfo.Rows[0]["ngaynhap"].ToString();

                        #region Noi dung chinh
                        iTextSharp.text.Table aTable = new iTextSharp.text.Table(3);
                        aTable.AutoFillEmptyCells = true;
                        aTable.Alignment = Element.ALIGN_LEFT;
                        aTable.Width = 100.0f;
                        aTable.Cellpadding = 0.0f;
                        aTable.Cellspacing = 0.0f;
                        aTable.Widths = new float[] { 30.0f, 100.0f, 100.0f };
                        aTable.BorderWidth = 0.0f;

                        iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(pathImage);
                        jpg.ScaleToFit(70.0f, 70.0f);

                        cell = new Cell(jpg);
                        cell.Rowspan = 3;
                        cell.BorderWidth = 0;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase(congty_chinhanh, new Font(bf, 9.0f, Font.NORMAL)));
                        cell.BorderWidth = 0;
                        cell.Colspan = 2;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase(diachi_title, new Font(bf, 10.0f, Font.NORMAL)));
                        cell.BorderWidth = 0;
                        cell.Colspan = 2;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase(". ", new Font(bf, 1.0f, Font.NORMAL)));
                        cell.BorderWidth = 0;
                        cell.Colspan = 2;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("", new Font(bf, 10.0f, Font.NORMAL)));
                        cell.BorderWidth = 0;
                        //cell.Colspan = 2;
                        aTable.AddCell(cell);

                        pdfDoc.Add(aTable);

                        aTable = new iTextSharp.text.Table(5);
                        aTable.AutoFillEmptyCells = true;
                        aTable.Alignment = Element.ALIGN_LEFT;
                        aTable.Width = 100.0f;
                        aTable.Cellpadding = 0.5f;
                        aTable.Cellspacing = 0.5f;
                        aTable.Widths = new float[] { 20.0f, 20.0f, 20.0f, 20.0f, 20.0f };
                        aTable.BorderWidth = 0.0f;

                        cell = new Cell(new Phrase("RECEIPT NOTE", new Font(bf, 16.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0;
                        cell.Colspan = 5;
                        aTable.AddCell(cell);
                        
                        cell = new Cell(new Phrase("No: " + dtInfo.Rows[0]["sophieu"].ToString() + " - Date: " + ngaynhap, new Font(bf, 9.0f, Font.ITALIC)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("small");
                        cell.BorderWidth = 0f;
                        cell.Colspan = 5;
                        aTable.AddCell(cell);

                        /////////////////////////////////////////////

                        cell = new Cell(new Phrase("From: " + dtInfo.Rows[0]["khachhang"].ToString(), new Font(bf, 11.0f, Font.NORMAL)));
                        cell.SetHorizontalAlignment("LEFT");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0f;
                        cell.Colspan = 5;
                        aTable.AddCell(cell);
                        
                        khoNHAP = dtInfo.Rows[0]["khonhap"].ToString();
                        cell = new Cell(new Phrase("To      : " + khoNHAP, new Font(bf, 11.0f, Font.NORMAL)));
                        cell.SetHorizontalAlignment("LEFT");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0f;
                        cell.Colspan = 5;
                        aTable.AddCell(cell);

                        //cell = new Cell(new Phrase("Type  : " + dtInfo.Rows[0]["loainhap"].ToString(), new Font(bf, 11.0f, Font.NORMAL)));
                        //cell.SetHorizontalAlignment("LEFT");
                        //cell.SetVerticalAlignment("middle");
                        //cell.BorderWidth = 0f;
                        //cell.Colspan = 5;
                        //aTable.AddCell(cell);
                        
                        cell = new Cell(new Phrase("Note  : " + dtInfo.Rows[0]["ghichu"].ToString(), new Font(bf, 11.0f, Font.NORMAL)));
                        cell.SetHorizontalAlignment("LEFT");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0f;
                        cell.Colspan = 5;
                        aTable.AddCell(cell);

                        ////VIP
                        content.Add(aTable);

                        #endregion

                        if (loaiFile.Equals("1"))
                        {
                            if (loop == 0)
                            {
                                #region Phieu nhap kho
                                //iTextSharp.text.Table 
                                aTable = new iTextSharp.text.Table(4);
                                aTable.AutoFillEmptyCells = true;
                                aTable.Alignment = Element.ALIGN_LEFT;
                                aTable.Width = 100.0f;
                                aTable.Cellpadding = 1.0f;
                                aTable.Cellspacing = 1.0f;
                                aTable.Widths = new float[] { 5.0f, 20.0f, 20.0f, 8.0f };
                                aTable.BorderWidth = 1.0f;

                                cell = new Cell(new Phrase("No", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Rowspan = 1;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Item JP", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Rowspan = 1;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Item VN", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Rowspan = 1;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Qty", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Rowspan = 1;
                                aTable.AddCell(cell);

                                dtInfo.Clear();
                                dtInfo.Clone();

                                query = "SELECT COUNT(*) box, nh_ct.nhaphang_fk, sp.ma, sp.codeEnglish, dvt.ten as donvitinh, ISNULL(SUM(nh_ct.soluong), 0) soluong " +
                                " FROM SanPham sp INNER JOIN  NhapHang_SanPham_ChiTiet nh_ct ON sp.pk_seq= nh_ct.sanpham_fk  AND nh_ct.nhaphang_fk = '" + id + "' " +
                                "     INNER JOIN  DonViTinh dvt ON nh_ct.dvt_fk = dvt.pk_seq " +
                                " WHERE nh_ct.soluong > 0 " +
                                " GROUP BY nh_ct.nhaphang_fk, sp.ma, sp.codeEnglish, dvt.ten " +
                                " ORDER BY sp.codeEnglish ASC ";

                                dtSP = xl.ReadTable(query);
                              
                                if (dtSP.Rows.Count > 0)
                                {
                                    Font fo = new Font(bf, 9.0f, Font.NORMAL);
                                    Font fo_ten = new Font(bf, 9.0f, Font.NORMAL);

                                    if (sodongDH == 0)
                                    {
                                        sodongDH = dtSP.Rows.Count;
                                        // tinh so lan lap
                                        countPDF = (Math.Round(sodongDH / 22.0 + 0.499));
                                    }

                                    defineLine = dtSP.Rows.Count;

                                    sodongDH = defineLine - count * 22;

                                    if (sodongDH < 22)
                                    {
                                        #region Less 22 line
                                        for (int i = 0; i < sodongDH; i++)
                                        {

                                            double soluong = double.Parse(dtSP.Rows[checkSoDong]["soLuong"].ToString());
                                            totalBox += double.Parse(dtSP.Rows[checkSoDong]["box"].ToString());
                                            totalQty += soluong;
                                           
                                            cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["codeEnglish"].ToString(), fo_ten));
                                            cell.SetHorizontalAlignment("LEFT");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["ma"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(FormatString.ForMatNumber(soluong.ToString()) + " ", fo));
                                            cell.SetHorizontalAlignment("RIGHT");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            checkSoDong++;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region More 22 line
                                        for (int i = 0; i < 22; i++)
                                        {

                                            double soluong = double.Parse(dtSP.Rows[checkSoDong]["soLuong"].ToString());
                                            totalBox += double.Parse(dtSP.Rows[checkSoDong]["box"].ToString());
                                            totalQty += soluong;

                                            cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["codeEnglish"].ToString(), fo_ten));
                                            cell.SetHorizontalAlignment("LEFT");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["ma"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(FormatString.ForMatNumber(soluong.ToString()) + " ", fo));
                                            cell.SetHorizontalAlignment("RIGHT");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            checkSoDong++;
                                        }
                                        #endregion
                                    }

                                    if(checkSoDong == defineLine)
                                    {
                                        cell = new Cell(new Phrase("Total", new Font(bf, 9.0f, Font.BOLD)));
                                        cell.SetHorizontalAlignment("CENTER");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.1f;
                                        cell.Colspan = 3;
                                        aTable.AddCell(cell);

                                        cell = new Cell(new Phrase(FormatString.ForMatNumber(totalQty.ToString()) + " ", new Font(bf, 9.0f, Font.NORMAL)));
                                        cell.SetHorizontalAlignment("RIGHT");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.1f;
                                        aTable.AddCell(cell);
                                    }
                                }

                                #endregion
                            }
                        }
                        else if (loaiFile.Equals("2"))
                        {
                            if (loop == 0)
                            {
                                #region Phieu nhap kho
                                //iTextSharp.text.Table 
                                aTable = new iTextSharp.text.Table(6);
                                aTable.AutoFillEmptyCells = true;
                                aTable.Alignment = Element.ALIGN_LEFT;
                                aTable.Width = 100.0f;
                                aTable.Cellpadding = 1.0f;
                                aTable.Cellspacing = 1.0f;
                                aTable.Widths = new float[] { 5.0f, 15.0f, 20.0f, 10.0f, 10.0f, 8.0f };
                                aTable.BorderWidth = 1.0f;

                                cell = new Cell(new Phrase("No", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Rowspan = 1;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Item JP", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Rowspan = 1;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Item VN", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Rowspan = 1;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Location", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Rowspan = 1;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Pallet", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Rowspan = 1;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Quantity", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Colspan = 1;
                                aTable.AddCell(cell);

                                dtInfo.Clear();
                                dtInfo.Clone();

                                query = "SELECT COUNT(*) box, nh_ct.nhaphang_fk, sp.ma, sp.codeEnglish, dvt.ten as donvitinh, ISNULL(SUM(nh_ct.soluong), 0) soluong, " +
                                "   ISNULL((SELECT ten FROM Location WHERE pk_seq = nh_ct.location_fk), '') location, " +
                                "   ISNULL((SELECT ten FROM Pallet WHERE pk_seq = nh_ct.pallet_fk), '') pallet " +
                                " FROM SanPham sp INNER JOIN  NhapHang_SanPham_ChiTiet nh_ct ON sp.pk_seq= nh_ct.sanpham_fk  AND nh_ct.nhaphang_fk = '" + id + "' " +
                                "     INNER JOIN  DonViTinh dvt ON nh_ct.dvt_fk = dvt.pk_seq " +
                                " WHERE nh_ct.soluong > 0 " +
                                " GROUP BY nh_ct.nhaphang_fk, sp.ma, sp.codeEnglish, dvt.ten, nh_ct.location_fk, nh_ct.pallet_fk " +
                                " ORDER BY sp.codeEnglish, nh_ct.location_fk, nh_ct.pallet_fk ASC ";

                                dtSP = xl.ReadTable(query);

                                if (dtSP.Rows.Count > 0)
                                {
                                    Font fo = new Font(bf, 9.0f, Font.NORMAL);
                                    Font fo_ten = new Font(bf, 9.0f, Font.NORMAL);

                                    if (sodongDH == 0)
                                    {
                                        sodongDH = dtSP.Rows.Count;
                                        // tinh so lan lap
                                        countPDF = (Math.Round(sodongDH / 22.0 + 0.499));
                                    }

                                    defineLine = dtSP.Rows.Count;

                                    sodongDH = defineLine - count * 22;

                                    if (sodongDH < 22)
                                    {
                                        #region Less 22 line
                                        for (int i = 0; i < sodongDH; i++)
                                        {

                                            double soluong = double.Parse(dtSP.Rows[checkSoDong]["SoLuong"].ToString());
                                            totalBox += double.Parse(dtSP.Rows[checkSoDong]["box"].ToString());
                                            totalQty += soluong;

                                            cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["codeEnglish"].ToString(), fo_ten));
                                            cell.SetHorizontalAlignment("LEFT");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["ma"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["location"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["pallet"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(FormatString.ForMatNumber(soluong.ToString()) + " ", fo));
                                            cell.SetHorizontalAlignment("RIGHT");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            checkSoDong++;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region More 22 line
                                        for (int i = 0; i < 22; i++)
                                        {

                                            double soluong = double.Parse(dtSP.Rows[checkSoDong]["SoLuong"].ToString());
                                            totalBox += double.Parse(dtSP.Rows[checkSoDong]["box"].ToString());
                                            totalQty += soluong;

                                            cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["codeEnglish"].ToString(), fo_ten));
                                            cell.SetHorizontalAlignment("LEFT");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["ma"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["location"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["pallet"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(FormatString.ForMatNumber(soluong.ToString()) + " ", fo));
                                            cell.SetHorizontalAlignment("RIGHT");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            checkSoDong++;
                                        }
                                        #endregion
                                    }

                                }

                                if (checkSoDong == defineLine)
                                {
                                    cell = new Cell(new Phrase("Total", new Font(bf, 9.0f, Font.BOLD)));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    cell.Colspan = 5;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(FormatString.ForMatNumber(totalQty.ToString()) + " ", new Font(bf, 9.0f, Font.NORMAL)));
                                    cell.SetHorizontalAlignment("RIGHT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            if (loop == 0)
                            {
                                #region Phieu nhap kho
                                //iTextSharp.text.Table 
                                aTable = new iTextSharp.text.Table(10);
                                aTable.AutoFillEmptyCells = true;
                                aTable.Alignment = Element.ALIGN_LEFT;
                                aTable.Width = 100.0f;
                                aTable.Cellpadding = 1.0f;
                                aTable.Cellspacing = 1.0f;
                                aTable.Widths = new float[] { 5.0f, 12.0f, 12.0f, 20.0f, 10.0f, 10.0f, 8.0f, 8.0f, 8.0f, 8.0f };
                                aTable.BorderWidth = 1.0f;

                                cell = new Cell(new Phrase("No", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Rowspan = 1;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Item JP", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Rowspan = 1;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Item VN", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Rowspan = 1;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("PL No.", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Rowspan = 1;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Carton No.", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Rowspan = 1;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Lot No.", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Rowspan = 1;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Color", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Colspan = 1;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Location", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Rowspan = 1;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Pallet", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Rowspan = 1;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Qty", new Font(bf, 9.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.1f;
                                cell.Rowspan = 1;
                                aTable.AddCell(cell);

                                dtInfo.Clear();
                                dtInfo.Clone();

                                query = "SELECT nh_ct.nhaphang_fk, sp.ma, sp.codeEnglish, dvt.ten as donvitinh, nh_ct.mavach, ISNULL(nh_ct.soluong, 0) soluong, " +
                                "   ISNULL(nh_ct.plNo, '') plNo, ISNULL(nh_ct.cartonNo, '') cartonNo, ISNULL(nh_ct.lotNo, '') lotNo, " +
                                "   ISNULL((SELECT ten FROM Location WHERE pk_seq = nh_ct.location_fk), '') location, " +
                                "   ISNULL((SELECT ten FROM Pallet WHERE pk_seq = nh_ct.pallet_fk), '') pallet, " +
                                "   ISNULL((SELECT ten FROM MauSac WHERE pk_seq = nh_ct.mausac_fk), '') mausac " +
                                " FROM SanPham sp INNER JOIN  NhapHang_SanPham_ChiTiet nh_ct ON sp.pk_seq= nh_ct.sanpham_fk  AND nh_ct.nhaphang_fk = '" + id + "' " +
                                "     INNER JOIN  DonViTinh dvt ON nh_ct.dvt_fk = dvt.pk_seq " +
                                " WHERE nh_ct.soluong > 0 " +
                                " ORDER BY sp.codeEnglish, nh_ct.plNo, nh_ct.cartonNo, nh_ct.lotNo ASC ";

                                dtSP = xl.ReadTable(query);
                                
                                if (dtSP.Rows.Count > 0)
                                {
                                    Font fo = new Font(bf, 9.0f, Font.NORMAL);
                                    Font fo_ten = new Font(bf, 9.0f, Font.NORMAL);

                                    if (sodongDH == 0)
                                    {
                                        sodongDH = dtSP.Rows.Count;
                                        // tinh so lan lap
                                        countPDF = (Math.Round(sodongDH / 22.0 + 0.499));
                                    }

                                    defineLine = dtSP.Rows.Count;

                                    sodongDH = defineLine - count * 22;

                                    if (sodongDH < 22)
                                    {
                                        #region Less 22 line
                                        for (int i = 0; i < sodongDH; i++)
                                        {

                                            double soluong = double.Parse(dtSP.Rows[checkSoDong]["SoLuong"].ToString());
                                            totalQty += soluong;

                                            cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["codeEnglish"].ToString(), fo_ten));
                                            cell.SetHorizontalAlignment("LEFT");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["ma"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["plNo"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["cartonNo"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["lotNo"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["mausac"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["location"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["pallet"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(FormatString.ForMatNumber(dtSP.Rows[checkSoDong]["soluong"].ToString()), fo));
                                            cell.SetHorizontalAlignment("RIGHT");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);
                                            
                                            checkSoDong++;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region More 22 line
                                        for (int i = 0; i < 22; i++)
                                        {

                                            double soluong = double.Parse(dtSP.Rows[checkSoDong]["SoLuong"].ToString());
                                            totalQty += soluong;

                                            cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["codeEnglish"].ToString(), fo_ten));
                                            cell.SetHorizontalAlignment("LEFT");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["ma"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["plNo"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["cartonNo"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["lotNo"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["mausac"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["location"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["pallet"].ToString(), fo));
                                            cell.SetHorizontalAlignment("CENTER");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            cell = new Cell(new Phrase(FormatString.ForMatNumber(dtSP.Rows[checkSoDong]["soluong"].ToString()), fo));
                                            cell.SetHorizontalAlignment("RIGHT");
                                            cell.SetVerticalAlignment("middle");
                                            cell.BorderWidth = 0.1f;
                                            aTable.AddCell(cell);

                                            checkSoDong++;
                                        }
                                        #endregion
                                    }

                                }

                                if (checkSoDong == defineLine)
                                {
                                    cell = new Cell(new Phrase("Total", new Font(bf, 9.0f, Font.BOLD)));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    cell.Colspan = 9;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(FormatString.ForMatNumber(totalQty.ToString()) + " ", new Font(bf, 9.0f, Font.NORMAL)));
                                    cell.SetHorizontalAlignment("RIGHT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);
                                }
                                #endregion
                            }
                        }

                        content.Add(aTable);

                        #region Phan cuoi
                        if (checkSoDong == dtSP.Rows.Count)
                        {
                            aTable = new iTextSharp.text.Table(3);
                            aTable.AutoFillEmptyCells = true;
                            aTable.Alignment = Element.ALIGN_LEFT;
                            aTable.Width = 100.0f;
                            aTable.Cellpadding = 0.5f;
                            aTable.Cellspacing = 0.5f;
                            aTable.Widths = new float[] { 25.0f, 25.0f, 25.0f};
                            aTable.Border = 0;

                            ////////////////////////////////
                            ////THEM THONG TIN O DUOI///////
                            ////////////////////////////////

                            //cell = new Cell(new Phrase(khoNHAP + ", Day " + DateTime.Now.ToString("dd") + " Month " + DateTime.Now.ToString("MM") + " Year " + DateTime.Now.Year.ToString(), new Font(bf, 11.0f, Font.ITALIC)));
                            //cell.SetHorizontalAlignment("RIGHT");
                            //cell.SetVerticalAlignment("middle");
                            //cell.BorderWidth = 0;
                            //cell.Colspan = 3;
                            //aTable.AddCell(cell);

                            cell = new Cell(new Phrase("", new Font(bf, 11.0f, Font.ITALIC)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0;
                            cell.Colspan = 2;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase(khoNHAP, new Font(bf, 11.0f, Font.ITALIC)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0;
                            cell.Colspan = 1;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase("Delivery", new Font(bf, 11.0f, Font.BOLD)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0;
                            cell.Colspan = 1;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase("Recipient", new Font(bf, 11.0f, Font.BOLD)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0;
                            cell.Colspan = 1;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase("Supervisor", new Font(bf, 11.0f, Font.BOLD)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0;
                            cell.Colspan = 1;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase("(Signature)", new Font(bf, 11.0f, Font.ITALIC)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0;
                            cell.Colspan = 1;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase("(Signature)", new Font(bf, 11.0f, Font.ITALIC)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0;
                            cell.Colspan = 1;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase("(Signature)", new Font(bf, 11.0f, Font.ITALIC)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0;
                            cell.Colspan = 1;
                            aTable.AddCell(cell);

                            content.Add(aTable);                                               
                        }
                        #endregion
                        
                        pdfDoc.Add(content);

                        content.Clear();
                        pdfDoc.NewPage();

                        dtSP.Clear();
                        dtSP.Clone();


                    }
                }
            }
            catch (Exception ex)
            {
                StringWriter sw = new StringWriter();

                string str = "Error when created file PDF...." + ex.Message;
                StringReader sr = new StringReader(str);

                htmlparser.Parse(sr);
            }

            pdfDoc.Close();
            context.Response.Write(pdfDoc);
            context.Response.End();

        }

        private void InNhapKhacPDF(HttpContext context)
        {
            context.Response.ContentType = "application/pdf";

            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string loaiFile = "";
            if (context.Request.QueryString["loaiFile"] != null)
                loaiFile = context.Request.QueryString["loaiFile"].ToString();

            context.Response.AddHeader("content-disposition", "inline;filename=StockOut_" + id + ".pdf");
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            context.Response.Charset = Encoding.Unicode.ToString();
            context.Response.ContentEncoding = Encoding.UTF8;


            Document pdfDoc = new Document(PageSize.A4, 20.0f, 20.0f, 0.0f, 0.0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, context.Response.OutputStream);

            pdfDoc.Open();

            int sodongDH = 0;
            double countPDF = 1;
            int checkSoDong = 0;
            int defineLine = 0;

            try
            {
                ExecuteData xl = new ExecuteData();

                BaseFont bf = BaseFont.CreateFont("c:\\windows\\fonts\\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                Font times = new Font(bf, 15.0f, Font.BOLD);
                Cell cell = new Cell();

                Paragraph content = new Paragraph();

                string khoNHAP = "";
                string makhoNHAP = "";
                string khochuyen = "";

                for (int count = 0; count < countPDF; count++)
                {
                    string query = "SELECT a.pk_seq, a.trangthai, a.loaikho, a.ngaynhap, a.ghichu, a.loainhap, ISNULL(a.kho_fk, 0) kho_fk, ISNULL(a.khochuyen_fk, 0) khochuyen_fk, ISNULL(a.khachhang_fk, 0) khachhang_fk, " +
                    "  CASE a.loaikho WHEN 0 THEN ISNULL((SELECT ma FROM KhachHang WHERE pk_seq = a.khachhang_fk), '') " +
                    "                  ELSE ISNULL((SELECT makho FROM Kho WHERE pk_seq = a.khochuyen_fk), '') END khonhap,  " + 
                    "  ISNULL((SELECT tenkho FROM Kho WHERE pk_seq = a.khochuyen_fk), '') khochuyen, " +
                    "  ISNULL((SELECT hoten FROM KhachHang WHERE pk_seq = a.khachhangchuyen_fk), '') khachhangchuyen, " +
                    "  ISNULL((SELECT sophieu FROM XuatKhac WHERE pk_seq = a.xuatkhac_fk), 0) sophieuXUAT, ISNULL(a.sophieu, '') sophieu, '' AS khuvuc " +
                    " FROM NhapKhac a WHERE a.pk_seq = '" + id + "'";

                    DataTable dtInfo = xl.ReadTable(query);

                    iTextSharp.text.Table aTable = new iTextSharp.text.Table(3);
                    aTable.AutoFillEmptyCells = true;
                    aTable.Alignment = Element.ALIGN_LEFT;
                    aTable.Width = 100.0f;
                    aTable.Cellpadding = 0.0f;
                    aTable.Cellspacing = 0.0f;
                    aTable.Widths = new float[] { 30.0f, 100.0f, 100.0f };
                    aTable.BorderWidth = 0.0f;

                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(pathImage);
                    jpg.ScaleToFit(70.0f, 70.0f);

                    cell = new Cell(jpg);
                    cell.Rowspan = 3;
                    cell.BorderWidth = 0;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase(congty_chinhanh, new Font(bf, 9.0f, Font.NORMAL)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase(diachi_title, new Font(bf, 10.0f, Font.NORMAL)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase(". ", new Font(bf, 1.0f, Font.NORMAL)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("", new Font(bf, 10.0f, Font.NORMAL)));
                    cell.BorderWidth = 0;
                    //cell.Colspan = 2;
                    aTable.AddCell(cell);

                    pdfDoc.Add(aTable);

                    aTable = new iTextSharp.text.Table(4);
                    aTable.AutoFillEmptyCells = true;
                    aTable.Alignment = Element.ALIGN_LEFT;
                    aTable.Width = 100.0f;
                    aTable.Cellpadding = 0.5f;
                    aTable.Cellspacing = 0.5f;
                    aTable.Widths = new float[] { 20.0f, 20.0f, 20.0f, 20.0f};
                    aTable.BorderWidth = 0.0f;

                    cell = new Cell(new Phrase("RECEIPT NOTE", new Font(bf, 16.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0;
                    cell.Colspan = 4;
                    aTable.AddCell(cell);

                    makhoNHAP = dtInfo.Rows[0]["khonhap"].ToString();
                    khochuyen = dtInfo.Rows[0]["khochuyen"].ToString();
                    if(khochuyen.Length < 2)
                    {
                        khochuyen = dtInfo.Rows[0]["khachhangchuyen"].ToString();
                    }

                    if (dtInfo.Rows[0]["loaikho"].ToString().Equals("0"))
                    {
                        cell = new Cell(new Phrase("(PARTNER)", new Font(bf, 12.0f, Font.NORMAL)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0;
                        cell.Colspan = 4;
                        aTable.AddCell(cell);
                    }
                    else
                    {
                        cell = new Cell(new Phrase("(COMPANY)", new Font(bf, 12.0f, Font.NORMAL)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0;
                        cell.Colspan = 4;
                        aTable.AddCell(cell);
                    }
                   
                    cell = new Cell(new Phrase("No: " + dtInfo.Rows[0]["sophieu"].ToString() + " - Date: " + dtInfo.Rows[0]["ngaynhap"].ToString(), new Font(bf, 9.0f, Font.ITALIC)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("small");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 4;
                    aTable.AddCell(cell);

                    /////////////////////////////////////////////

                    cell = new Cell(new Phrase("From: " + khochuyen, new Font(bf, 11.0f, Font.NORMAL)));
                    cell.SetHorizontalAlignment("LEFT");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 3;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("No: " + dtInfo.Rows[0]["sophieuXuat"].ToString(), new Font(bf, 11.0f, Font.NORMAL)));
                    cell.SetHorizontalAlignment("LEFT");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 1;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("To  : " + makhoNHAP, new Font(bf, 11.0f, Font.NORMAL)));
                    cell.SetHorizontalAlignment("LEFT");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 4;
                    aTable.AddCell(cell);
                   
                    cell = new Cell(new Phrase("Note: " + dtInfo.Rows[0]["ghichu"].ToString(), new Font(bf, 11.0f, Font.NORMAL)));
                    cell.SetHorizontalAlignment("LEFT");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 4;
                    aTable.AddCell(cell);

                    ////VIP
                    content.Add(aTable);

                    if (loaiFile.Equals("1"))
                    {
                        aTable = new iTextSharp.text.Table(6);
                        aTable.AutoFillEmptyCells = true;
                        aTable.Alignment = Element.ALIGN_LEFT;
                        aTable.Width = 100.0f;
                        aTable.Cellpadding = 1.0f;
                        aTable.Cellspacing = 1.0f;
                        aTable.Widths = new float[] { 5.0f, 15.0f, 35.0f, 10.0f, 10.0f, 8.0f };
                        aTable.BorderWidth = 1.0f;

                        cell = new Cell(new Phrase("No", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("PartNo", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("PartName", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("LotNo", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);

                        //cell = new Cell(new Phrase("SerialNo", new Font(bf, 9.0f, Font.BOLD)));
                        //cell.SetHorizontalAlignment("CENTER");
                        //cell.SetVerticalAlignment("middle");
                        //cell.BorderWidth = 0.1f;
                        //aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Prod date", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Qty", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);

                        content.Add(aTable);
                        //pdfDoc.Add(aTable);               

                        query = "SELECT sp.ma, sp.ten, (SELECT ten FROM DonViTinh WHERE pk_seq = nk_ct.dvt_fk) dvt, nk_ct.quycach, " +
                            "   nk_ct.lotNo, SUM(nk_ct.soluong) soluong, ISNULL(nk_ct.dongia, 0) dongia, nk_ct.solo, ISNULL(loc.ten, '') location, ISNULL(bin.ten, '') bin " +
                            " FROM NhapKhac_SanPham_ChiTiet nk_ct INNER JOIN SanPham sp ON nk_ct.sanpham_fk = sp.pk_seq AND nk_ct.nhapkhac_fk = '" + id + "' " +
                            " LEFT JOIN Location loc ON nk_ct.location_fk = loc.pk_seq  " +
                            " LEFT JOIN BIN bin ON nk_ct.bin_fk = bin.pk_seq  " +
                            " WHERE nk_ct.nhapkhac_fk = '" + id + "'" +
                            " GROUP BY sp.ma, sp.ten, nk_ct.dvt_fk, nk_ct.quycach, nk_ct.lotNo, nk_ct.dongia, nk_ct.solo, loc.ten, bin.ten " +
                            " ORDER BY sp.ma ASC ";

                        DataTable dtSP = xl.ReadTable(query);

                        if (sodongDH == 0)
                        {
                            sodongDH = dtSP.Rows.Count;
                            // tinh so lan lap
                            countPDF = (Math.Round(sodongDH / 12.0 + 0.4999));
                        }

                        defineLine = dtSP.Rows.Count;
                        if (dtSP.Rows.Count > 0)
                        {
                            sodongDH = defineLine - count * 12;
                            if (sodongDH < 12)
                            {

                                for (int i = 0; i < sodongDH; i++)
                                {
                                    Font fo = new Font(bf, 9.0f, Font.NORMAL);
                                    Font fo_ten = new Font(bf, 9.0f, Font.NORMAL);

                                    double soluong = double.Parse(dtSP.Rows[checkSoDong]["soluong"].ToString());

                                    cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["ma"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["ten"].ToString(), fo_ten));
                                    cell.SetHorizontalAlignment("LEFT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["LotNo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    //cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["serialNo"].ToString(), fo));
                                    //cell.SetHorizontalAlignment("CENTER");
                                    //cell.SetVerticalAlignment("middle");
                                    //cell.BorderWidth = 0.1f;
                                    //aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["solo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(FormatString.ForMatNumber(soluong.ToString()) + " ", fo));
                                    cell.SetHorizontalAlignment("RIGHT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    checkSoDong++;

                                }
                            }
                            else
                            {
                                for (int i = 0; i < 12; i++)
                                {
                                    Font fo = new Font(bf, 9.0f, Font.NORMAL);
                                    Font fo_ten = new Font(bf, 9.0f, Font.NORMAL);

                                    double soluong = double.Parse(dtSP.Rows[checkSoDong]["SoLuong"].ToString());

                                    cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);
                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["ma"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["ten"].ToString(), fo_ten));
                                    cell.SetHorizontalAlignment("LEFT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["LotNo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    //cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["serialNo"].ToString(), fo));
                                    //cell.SetHorizontalAlignment("CENTER");
                                    //cell.SetVerticalAlignment("middle");
                                    //cell.BorderWidth = 0.1f;
                                    //aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["solo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(FormatString.ForMatNumber(soluong.ToString()) + " ", fo));
                                    cell.SetHorizontalAlignment("RIGHT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    checkSoDong++;

                                }
                            }

                            string arrDVT = "";

                            //if (checkSoDong == defineLine)
                            //{
                            //    {
                            //        query = "SELECT SUM(soluong) soluong, b.ten " +
                            //                    " FROM NhapKhac_SanPham a INNER JOIN  SanPham c ON a.sanpham_fk = c.pk_seq INNER JOIN  DonViTinh b ON a.DVT_FK = b.pk_seq " +
                            //                    " WHERE a.nhapkhac_fk = '" + id + "'GROUP BY b.ten";

                            //        DataTable dtDVT = xl.ReadTable(query);
                            //        if (dtDVT.Rows.Count > 0)
                            //        {
                            //            for (int i = 0; i < dtDVT.Rows.Count; i++)
                            //            {
                            //                arrDVT += dtDVT.Rows[i]["soluong"].ToString() + " " + dtDVT.Rows[i]["ten"].ToString() + ", ";
                            //            }
                            //        }
                            //    }

                            //    cell = new Cell(new Phrase("Tổng: " + arrDVT, new Font(bf, 10.0f, Font.BOLD)));
                            //    cell.SetHorizontalAlignment("RIGHT");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    cell.Colspan = 8;
                            //    aTable.AddCell(cell);

                            //}
                        }

                        dtInfo.Clear();
                        dtInfo.Clone();

                        dtSP.Clear();
                        dtSP.Clone();

                    }
                    else
                    {
                        aTable = new iTextSharp.text.Table(7);
                        aTable.AutoFillEmptyCells = true;
                        aTable.Alignment = Element.ALIGN_LEFT;
                        aTable.Width = 100.0f;
                        aTable.Cellpadding = 1.0f;
                        aTable.Cellspacing = 1.0f;
                        aTable.Widths = new float[] { 5.0f, 15.0f, 30.0f, 10.0f, 10.0f, 10.0f, 8.0f };
                        aTable.BorderWidth = 1.0f;

                        cell = new Cell(new Phrase("No", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("PartNo", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("PartName", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("LotNo", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("SerialNo", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Prod date", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Qty", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);

                        content.Add(aTable);
                        //pdfDoc.Add(aTable);               

                        query = "SELECT sp.ma, sp.ten, (SELECT ten FROM DonViTinh WHERE pk_seq = nk_ct.dvt_fk) dvt, nk_ct.quycach, " +
                            "   nk_ct.lotNo, SUM(nk_ct.soluong) soluong, ISNULL(nk_ct.dongia, 0) dongia, nk_ct.serialNo, nk_ct.solo, ISNULL(loc.ten, '') location, ISNULL(bin.ten, '') bin " +
                            " FROM NhapKhac_SanPham_ChiTiet nk_ct INNER JOIN SanPham sp ON nk_ct.sanpham_fk = sp.pk_seq AND nk_ct.nhapkhac_fk = '" + id + "' " +
                            " LEFT JOIN Location loc ON nk_ct.location_fk = loc.pk_seq  " +
                            " LEFT JOIN BIN bin ON nk_ct.bin_fk = bin.pk_seq  " +
                            " WHERE nk_ct.nhapkhac_fk = '" + id + "'" +
                            " GROUP BY sp.ma, sp.ten, nk_ct.dvt_fk, nk_ct.quycach, nk_ct.lotNo, nk_ct.dongia, nk_ct.solo, nk_ct.serialNo, loc.ten, bin.ten " +
                            " ORDER BY sp.ma ASC ";

                        DataTable dtSP = xl.ReadTable(query);

                        if (sodongDH == 0)
                        {
                            sodongDH = dtSP.Rows.Count;
                            // tinh so lan lap
                            countPDF = (Math.Round(sodongDH / 12.0 + 0.4999));
                        }

                        defineLine = dtSP.Rows.Count;
                        if (dtSP.Rows.Count > 0)
                        {
                            sodongDH = defineLine - count * 12;
                            if (sodongDH < 12)
                            {

                                for (int i = 0; i < sodongDH; i++)
                                {
                                    Font fo = new Font(bf, 9.0f, Font.NORMAL);
                                    Font fo_ten = new Font(bf, 9.0f, Font.NORMAL);

                                    double soluong = double.Parse(dtSP.Rows[checkSoDong]["soluong"].ToString());

                                    cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["ma"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["ten"].ToString(), fo_ten));
                                    cell.SetHorizontalAlignment("LEFT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["LotNo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["serialNo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["solo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(FormatString.ForMatNumber(soluong.ToString()) + " ", fo));
                                    cell.SetHorizontalAlignment("RIGHT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    checkSoDong++;

                                }
                            }
                            else
                            {
                                for (int i = 0; i < 12; i++)
                                {
                                    Font fo = new Font(bf, 9.0f, Font.NORMAL);
                                    Font fo_ten = new Font(bf, 9.0f, Font.NORMAL);

                                    double soluong = double.Parse(dtSP.Rows[checkSoDong]["SoLuong"].ToString());

                                    cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);
                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["ma"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["ten"].ToString(), fo_ten));
                                    cell.SetHorizontalAlignment("LEFT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["LotNo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["serialNo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["solo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(FormatString.ForMatNumber(soluong.ToString()) + " ", fo));
                                    cell.SetHorizontalAlignment("RIGHT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    checkSoDong++;

                                }
                            }

                            string arrDVT = "";

                            //if (checkSoDong == defineLine)
                            //{
                            //    {
                            //        query = "SELECT SUM(soluong) soluong, b.ten " +
                            //                    " FROM NhapKhac_SanPham a INNER JOIN  SanPham c ON a.sanpham_fk = c.pk_seq INNER JOIN  DonViTinh b ON a.DVT_FK = b.pk_seq " +
                            //                    " WHERE a.nhapkhac_fk = '" + id + "'GROUP BY b.ten";

                            //        DataTable dtDVT = xl.ReadTable(query);
                            //        if (dtDVT.Rows.Count > 0)
                            //        {
                            //            for (int i = 0; i < dtDVT.Rows.Count; i++)
                            //            {
                            //                arrDVT += dtDVT.Rows[i]["soluong"].ToString() + " " + dtDVT.Rows[i]["ten"].ToString() + ", ";
                            //            }
                            //        }
                            //    }

                            //    cell = new Cell(new Phrase("Tổng: " + arrDVT, new Font(bf, 10.0f, Font.BOLD)));
                            //    cell.SetHorizontalAlignment("RIGHT");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    cell.Colspan = 8;
                            //    aTable.AddCell(cell);

                            //}
                        }

                        dtInfo.Clear();
                        dtInfo.Clone();

                        dtSP.Clear();
                        dtSP.Clone();
                    }
                    //content.Add(aTable);

                    aTable = new iTextSharp.text.Table(3);
                    aTable.AutoFillEmptyCells = true;
                    aTable.Alignment = Element.ALIGN_LEFT;
                    aTable.Width = 100.0f;
                    aTable.Cellpadding = 0.5f;
                    aTable.Cellspacing = 0.5f;
                    aTable.Widths = new float[] { 33.0f, 34.0f, 33.0f };
                    aTable.Border = 0;

                    ////////////////////////////////
                    ////THEM THONG TIN O DUOI///////
                    ////////////////////////////////

                    cell = new Cell(new Phrase("Number of accompanying original documents:........................................................................................................... ", new Font(bf, 11.0f, Font.NORMAL)));
                    cell.SetHorizontalAlignment("LEFT");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 3;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase(makhoNHAP + ", Date " + DateTime.Now.Day.ToString() + " Month " + DateTime.Now.Month.ToString() + " Year " + DateTime.Now.Year.ToString(), new Font(bf, 11.0f, Font.ITALIC)));
                    cell.SetHorizontalAlignment("RIGHT");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0;
                    cell.Colspan = 3;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("Manager", new Font(bf, 11.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("Inventory management", new Font(bf, 11.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("Stocker", new Font(bf, 11.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    aTable.AddCell(cell);


                    cell = new Cell(new Phrase("(Signature)", new Font(bf, 11.0f, Font.ITALIC)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("(Signature)", new Font(bf, 11.0f, Font.ITALIC)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("(Signature)", new Font(bf, 11.0f, Font.ITALIC)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    aTable.AddCell(cell);

                    content.Add(aTable);
                    pdfDoc.Add(content);

                    content.Clear();
                    pdfDoc.NewPage();
                }

            }
            catch (Exception ex)
            {
                StringWriter sw = new StringWriter();

                string str = "Error when created file PDF...." + ex.Message;
                StringReader sr = new StringReader(str);

                htmlparser.Parse(sr);
            }

            pdfDoc.Close();
            context.Response.Write(pdfDoc);
            context.Response.End();
        }

        private void DoiLocationBin(HttpContext context)
        {
            context.Response.ContentType = "application/pdf";

            string id = "";
            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            string loaiFile = "1";
            if (context.Request.QueryString["loaiFile"] != null)
                loaiFile = context.Request.QueryString["loaiFile"].ToString();

            context.Response.AddHeader("content-disposition", "inline;filename=DoiLoBin_" + id + ".pdf");
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            context.Response.Charset = Encoding.Unicode.ToString();
            context.Response.ContentEncoding = Encoding.UTF8;


            Document pdfDoc = new Document(PageSize.A4, 20.0f, 20.0f, 0.0f, 0.0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, context.Response.OutputStream);

            pdfDoc.Open();

            int sodongDH = 0;           
            int checkSoDong = 0;
            int defineLine = 0;
            double countPDF = 1;
            bool checkPage = true;

            try
            {
                ExecuteData xl = new ExecuteData();

                BaseFont bf = BaseFont.CreateFont("c:\\windows\\fonts\\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                Font times = new Font(bf, 15.0f, Font.BOLD);
                Cell cell = new Cell();

                Paragraph content = new Paragraph();
                
                string makhoNHAP = "";

                for (int count = 0; count < countPDF; count++)
                {
                    string query = " SELECT  a.pk_seq, a.ngaynhap, a.ghichu, a.sophieu, a.loainhap, ISNULL((SELECT makho FROM Kho WHERE pk_seq = a.kho_fk), '') makho, " +
                    " CASE a.loainhap WHEN 1 THEN N'A litte item in pallet' " +
                    "                 WHEN 2 THEN N'All in pallet' ELSE N'Unknow' END tenloainhap,  " + 
                    " ISNULL((SELECT hoten FROM KhachHang WHERE pk_seq = a.khachhang_fk), '') khachhang, " +
                    " ISNULL((SELECT ten FROM Location WHERE pk_seq = a.location_fk), '') location, " +
                    " ISNULL((SELECT ten FROM Location WHERE pk_seq = a.location_new_fk), '') locationNew " +
                    " FROM DoiLoBin a  " +
                    " WHERE a.pk_seq = '" + id + "' ";

                    DataTable dtInfo = xl.ReadTable(query);

                    #region Tittle

                    iTextSharp.text.Table aTable = new iTextSharp.text.Table(3);
                    aTable.AutoFillEmptyCells = true;
                    aTable.Alignment = Element.ALIGN_LEFT;
                    aTable.Width = 100.0f;
                    aTable.Cellpadding = 0.0f;
                    aTable.Cellspacing = 0.0f;
                    aTable.Widths = new float[] { 30.0f, 100.0f, 100.0f };
                    aTable.BorderWidth = 0.0f;

                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(pathImage);
                    jpg.ScaleToFit(70.0f, 70.0f);

                    cell = new Cell(jpg);
                    cell.Rowspan = 3;
                    cell.BorderWidth = 0;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase(congty_chinhanh, new Font(bf, 9.0f, Font.NORMAL)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase(diachi_title, new Font(bf, 10.0f, Font.NORMAL)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase(". ", new Font(bf, 1.0f, Font.NORMAL)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("", new Font(bf, 10.0f, Font.NORMAL)));
                    cell.BorderWidth = 0;
                    //cell.Colspan = 2;
                    aTable.AddCell(cell);

                    pdfDoc.Add(aTable);
                    #endregion

                    #region Content 
                    aTable = new iTextSharp.text.Table(5);
                    aTable.AutoFillEmptyCells = true;
                    aTable.Alignment = Element.ALIGN_LEFT;
                    aTable.Width = 100.0f;
                    aTable.Cellpadding = 0.5f;
                    aTable.Cellspacing = 0.5f;
                    aTable.Widths = new float[] { 20.0f, 20.0f, 20.0f, 20.0f, 20.0f };
                    aTable.BorderWidth = 0.0f;

                    cell = new Cell(new Phrase("RECEIPT NOTE", new Font(bf, 16.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0;
                    cell.Colspan = 5;
                    aTable.AddCell(cell);


                    cell = new Cell(new Phrase("(Transfer location)", new Font(bf, 12.0f, Font.ITALIC)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0;
                    cell.Colspan = 5;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("No: " + id +  " - Date: " + dtInfo.Rows[0]["ngaynhap"].ToString(), new Font(bf, 9.0f, Font.ITALIC)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("small");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 5;
                    aTable.AddCell(cell);

                    /////////////////////////////////////////////

                    makhoNHAP = dtInfo.Rows[0]["makho"].ToString();
                    
                    cell = new Cell(new Phrase("Customer    : " + dtInfo.Rows[0]["khachhang"].ToString(), new Font(bf, 10.0f, Font.NORMAL)));
                    cell.SetHorizontalAlignment("LEFT");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 5;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("Location    : " + dtInfo.Rows[0]["location"].ToString(), new Font(bf, 10.0f, Font.NORMAL)));
                    cell.SetHorizontalAlignment("LEFT");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 3;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("New location    : " + dtInfo.Rows[0]["locationNew"].ToString(), new Font(bf, 10.0f, Font.NORMAL)));
                    cell.SetHorizontalAlignment("LEFT");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 2;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("Note           :  " + dtInfo.Rows[0]["ghichu"].ToString(), new Font(bf, 10.0f, Font.NORMAL)));
                    cell.SetHorizontalAlignment("LEFT");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 5;
                    aTable.AddCell(cell);

                    ////VIP
                    content.Add(aTable);

                    #endregion

                    if (loaiFile.Equals("1"))
                    {
                        #region Information of Products

                        aTable = new iTextSharp.text.Table(7);
                        aTable.AutoFillEmptyCells = true;
                        aTable.Alignment = Element.ALIGN_LEFT;
                        aTable.Width = 100.0f;
                        aTable.Cellpadding = 1.0f;
                        aTable.Cellspacing = 1.0f;
                        aTable.Widths = new float[] { 5.0f, 12.0f, 12.0f, 18.0f, 12.0f, 12.0f, 8.0f };
                        aTable.BorderWidth = 1.0f;

                        cell = new Cell(new Phrase("No", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        //cell.Rowspan = 2;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Item JP", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        //cell.Rowspan = 2;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Item VN", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        //cell.Rowspan = 2;
                        aTable.AddCell(cell);
                        
                        cell = new Cell(new Phrase("Color", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        //cell.Colspan = 2;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Pallet", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        //cell.Colspan = 2;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("New Pallet", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        //cell.Colspan = 2;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Qty", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        //cell.Rowspan = 2;
                        aTable.AddCell(cell);

                     
                        content.Add(aTable);
                        //pdfDoc.Add(aTable);               

                        #endregion

                        query = " SELECT sp.ma as masp, sp.codeEnglish as codeEnglish, SUM(dlb.soluong) soluong, " +
                        "   ISNULL((SELECT ma FROM MauSac WHERE pk_seq = dlb.mausac_fk), '') mausac, " +
                        "   ISNULL((SELECT ten FROM Pallet WHERE pk_seq = dlb.pallet_fk), '') pallet, " +
                        "   ISNULL((SELECT ten FROM Pallet WHERE pk_seq = dlb.pallet_new_fk), '') palletNew " +
                        " FROM DoiLoBin_SanPham_ChiTiet dlb INNER JOIN SanPham sp ON dlb.sanpham_fk = sp.pk_seq AND dlb.doilobin_fk = '" + id + "' " +
                        " WHERE dlb.doilobin_fk = '" + id + "'  " +
                        " GROUP BY sp.ma, sp.codeEnglish, dlb.mausac_fk, dlb.pallet_fk, dlb.pallet_new_fk " +
                        " ORDER BY sp.codeEnglish, dlb.mausac_fk ASC ";

                        DataTable dtSP = xl.ReadTable(query);

                        if (sodongDH == 0)
                        {
                            sodongDH = dtSP.Rows.Count;
                            // tinh so lan lap
                            countPDF = (Math.Round(sodongDH / 22.0 + 0.4999));
                        }

                        defineLine = dtSP.Rows.Count;

                        if (dtSP.Rows.Count > 0)
                        {
                            sodongDH = defineLine - count * 22;
                            if (sodongDH < 22)
                            {
                                #region less 22 line
                                for (int i = 0; i < sodongDH; i++)
                                {
                                    Font fo = new Font(bf, 8.0f, Font.NORMAL);
                                    Font fo_ten = new Font(bf, 8.0f, Font.NORMAL);

                                    double soluong = double.Parse(dtSP.Rows[checkSoDong]["SoLuong"].ToString());

                                    cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["codeEnglish"].ToString(), fo_ten));
                                    cell.SetHorizontalAlignment("LEFT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["masp"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["mausac"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["pallet"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["palletNew"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(FormatString.ForMatNumber(soluong.ToString()) + " ", fo));
                                    cell.SetHorizontalAlignment("RIGHT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    checkSoDong++;

                                }

                                #endregion

                                checkPage = false;
                            }
                            else
                            {
                                #region more 22 line
                                for (int i = 0; i < 22; i++)
                                {
                                    Font fo = new Font(bf, 8.0f, Font.NORMAL);
                                    Font fo_ten = new Font(bf, 8.0f, Font.NORMAL);

                                    double soluong = double.Parse(dtSP.Rows[checkSoDong]["SoLuong"].ToString());

                                    cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["codeEnglish"].ToString(), fo_ten));
                                    cell.SetHorizontalAlignment("LEFT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["masp"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["mausac"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["pallet"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["palletNew"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(FormatString.ForMatNumber(soluong.ToString()) + " ", fo));
                                    cell.SetHorizontalAlignment("RIGHT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    checkSoDong++;

                                }
                                #endregion

                                checkPage = true;

                            }
                            
                            //string arrDVT = "";
                            //if (defineLine == checkSoDong)
                            //{
                            //    {
                            //        query = "SELECT SUM(soluong) soluong, b.ten " +
                            //        " FROM DoiLoBin_SanPham a INNER JOIN  SanPham c ON a.sanpham_fk = c.pk_seq INNER JOIN  DonViTinh b ON a.dvt_fk = b.pk_seq " +
                            //        " WHERE a.doilobin_fk = '" + id + "'GROUP BY b.ten";

                            //        DataTable dtDVT = xl.ReadTable(query);
                            //        if (dtDVT.Rows.Count > 0)
                            //        {
                            //            for (int i = 0; i < dtDVT.Rows.Count; i++)
                            //            {
                            //                arrDVT += FormatString.ForMatNumber(dtDVT.Rows[i]["soluong"].ToString()) + " " + dtDVT.Rows[i]["ten"].ToString() + ", ";
                            //            }
                            //        }
                            //    }

                            //    cell = new Cell(new Phrase("Total: " + arrDVT, new Font(bf, 9.0f, Font.NORMAL)));
                            //    cell.SetHorizontalAlignment("RIGHT");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    cell.Colspan = 5;
                            //    aTable.AddCell(cell);

                            //}
                        }
                        dtSP.Clear();
                        dtSP.Clone();
                    }
                    else if (loaiFile.Equals("2"))
                    {
                        #region Information of Products

                        aTable = new iTextSharp.text.Table(10);
                        aTable.AutoFillEmptyCells = true;
                        aTable.Alignment = Element.ALIGN_LEFT;
                        aTable.Width = 100.0f;
                        aTable.Cellpadding = 1.0f;
                        aTable.Cellspacing = 1.0f;
                        aTable.Widths = new float[] { 4.0f, 12.0f, 12.0f, 15.0f, 12.0f, 12.0f, 12.0f, 12.0f, 12.0f, 8.0f };
                        aTable.BorderWidth = 1.0f;

                        cell = new Cell(new Phrase("No", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        //cell.Rowspan = 2;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Item JP", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        //cell.Rowspan = 2;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Item VN", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        //cell.Rowspan = 2;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("PL No.", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        //cell.Rowspan = 2;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Carton No", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        //cell.Colspan = 2;
                        aTable.AddCell(cell);

                        //// 
                        cell = new Cell(new Phrase("Lot No", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);


                        cell = new Cell(new Phrase("Color", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Pallet", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("New Pallet", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Qty", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);

                        content.Add(aTable);
                        //pdfDoc.Add(aTable);               

                        #endregion

                        query = " SELECT sp.ma as masp, sp.codeEnglish as codeEnglish, dlb.plNo, dlb.cartonNo, dlb.lotNo, SUM(dlb.soluong) soluong, " +
                        "   ISNULL((SELECT ma FROM MauSac WHERE pk_seq = dlb.mausac_fk), '') mausac, " +
                        "   ISNULL((SELECT ten FROM Pallet WHERE pk_seq = dlb.pallet_fk), '') pallet, " +
                        "   ISNULL((SELECT ten FROM Pallet WHERE pk_seq = dlb.pallet_new_fk), '') palletNew " +
                        " FROM DoiLoBin_SanPham_ChiTiet dlb INNER JOIN SanPham sp ON dlb.sanpham_fk = sp.pk_seq AND dlb.doilobin_fk = '" + id + "' " +
                        " WHERE dlb.doilobin_fk = '" + id + "'  " +
                        " GROUP BY sp.ma, sp.codeEnglish, dlb.plNo, dlb.cartonNo, dlb.lotNo, dlb.mausac_fk, dlb.pallet_fk, dlb.pallet_new_fk " +
                        " ORDER BY sp.codeEnglish, dlb.mausac_fk ASC ";

                        DataTable dtSP = xl.ReadTable(query);

                        if (sodongDH == 0)
                        {
                            sodongDH = dtSP.Rows.Count;
                            // tinh so lan lap
                            countPDF = (Math.Round(sodongDH / 22.0 + 0.4999));
                        }

                        defineLine = dtSP.Rows.Count;

                        if (dtSP.Rows.Count > 0)
                        {
                            sodongDH = defineLine - count * 22;
                            if (sodongDH < 22)
                            {
                                #region less 22 line
                                for (int i = 0; i < sodongDH; i++)
                                {
                                    Font fo = new Font(bf, 8.0f, Font.NORMAL);
                                    Font fo_ten = new Font(bf, 8.0f, Font.NORMAL);

                                    double soluong = double.Parse(dtSP.Rows[checkSoDong]["SoLuong"].ToString());

                                    cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["codeEnglish"].ToString(), fo_ten));
                                    cell.SetHorizontalAlignment("LEFT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["masp"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["plNo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["cartonNo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["lotNo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["mausac"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["pallet"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["palletNew"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(FormatString.ForMatNumber(soluong.ToString()) + " ", fo));
                                    cell.SetHorizontalAlignment("RIGHT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    checkSoDong++;

                                }

                                #endregion

                                checkPage = false;
                            }
                            else
                            {
                                #region more 22 line
                                for (int i = 0; i < 22; i++)
                                {
                                    Font fo = new Font(bf, 8.0f, Font.NORMAL);
                                    Font fo_ten = new Font(bf, 8.0f, Font.NORMAL);

                                    double soluong = double.Parse(dtSP.Rows[checkSoDong]["SoLuong"].ToString());

                                    cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["codeEnglish"].ToString(), fo_ten));
                                    cell.SetHorizontalAlignment("LEFT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["masp"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["plNo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["cartonNo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["lotNo"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["mausac"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["pallet"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["palletNew"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(FormatString.ForMatNumber(soluong.ToString()) + " ", fo));
                                    cell.SetHorizontalAlignment("RIGHT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    checkSoDong++;

                                }

                                #endregion

                                checkPage = true;

                            }

                            //string arrDVT = "";
                            //if (defineLine == checkSoDong)
                            //{
                            //    {
                            //        query = "SELECT SUM(soluong) soluong, b.ten " +
                            //        " FROM DoiLoBin_SanPham a INNER JOIN  SanPham c ON a.sanpham_fk = c.pk_seq INNER JOIN  DonViTinh b ON a.DVT_FK = b.pk_seq " +
                            //        " WHERE a.doilobin_fk = '" + id + "'GROUP BY b.ten";

                            //        DataTable dtDVT = xl.ReadTable(query);
                            //        if (dtDVT.Rows.Count > 0)
                            //        {
                            //            for (int i = 0; i < dtDVT.Rows.Count; i++)
                            //            {
                            //                arrDVT += FormatString.ForMatNumber(dtDVT.Rows[i]["soluong"].ToString()) + " " + dtDVT.Rows[i]["ten"].ToString() + ", ";
                            //            }
                            //        }
                            //    }

                            //    cell = new Cell(new Phrase("Tổng: " + arrDVT, new Font(bf, 9.0f, Font.NORMAL)));
                            //    cell.SetHorizontalAlignment("RIGHT");
                            //    cell.SetVerticalAlignment("middle");
                            //    cell.BorderWidth = 0.01f;
                            //    cell.Colspan = 7;
                            //    aTable.AddCell(cell);

                            //}
                        }
                        dtSP.Clear();
                        dtSP.Clone();
                    }
                    else
                    {
                        #region Information of Products

                        aTable = new iTextSharp.text.Table(9);
                        aTable.AutoFillEmptyCells = true;
                        aTable.Alignment = Element.ALIGN_LEFT;
                        aTable.Width = 100.0f;
                        aTable.Cellpadding = 1.0f;
                        aTable.Cellspacing = 1.0f;
                        aTable.Widths = new float[] { 5.0f, 18.0f, 12.0f, 9.0f, 9.0f, 9.0f, 9.0f, 9.0f, 9.0f };
                        aTable.BorderWidth = 1.0f;

                        cell = new Cell(new Phrase("No", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        cell.Rowspan = 2;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Vin No", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        cell.Rowspan = 2;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Grade", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        cell.Rowspan = 2;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Model", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        cell.Rowspan = 2;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Frame", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        cell.Rowspan = 2;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Engine", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        cell.Rowspan = 2;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Color", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        cell.Rowspan = 2;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Location", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        cell.Colspan = 2;
                        aTable.AddCell(cell);

                        //// 
                        cell = new Cell(new Phrase("Before", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);


                        cell = new Cell(new Phrase("After", new Font(bf, 9.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0.1f;
                        aTable.AddCell(cell);

                        content.Add(aTable);
                        //pdfDoc.Add(aTable);               

                        #endregion

                        query = " SELECT dlb.mavach, sp.ma as masp, sp.ten as tensp, dlb.sokhung, dlb.dongco, " +
                        "   ISNULL((SELECT ten FROM ChungLoai WHERE pk_seq = sp.chungloai_fk), '') chungloai, " +
                        "   ISNULL((SELECT ma FROM MauSac WHERE pk_seq = dlb.mausac_fk), '') mausac, " +
                        " 	loc.ten as location, loc_new.ten as location_NEW, dlb.soluong " +
                        " FROM DoiLoBin_SanPham_ChiTiet dlb INNER JOIN SanPham sp ON dlb.sanpham_fk = sp.pk_seq AND dlb.doilobin_fk = '" + id + "' " +
                        "   INNER JOIN Location loc ON dlb.location_fk = loc.pk_seq  " +
                        "   INNER JOIN Location loc_new ON dlb.location_new_fk = loc_new.pk_seq  " +
                        " WHERE dlb.doilobin_fk = '" + id + "'  " +
                        " ORDER BY sp.ma, CONVERT(datetime, dlb.solo, 105), loc.ten ASC ";

                        DataTable dtSP = xl.ReadTable(query);

                        if (sodongDH == 0)
                        {
                            sodongDH = dtSP.Rows.Count;
                            // tinh so lan lap
                            countPDF = (Math.Round(sodongDH / 15.0 + 0.4999));
                        }

                        defineLine = dtSP.Rows.Count;

                        if (dtSP.Rows.Count > 0)
                        {
                            sodongDH = defineLine - count * 15;
                            if (sodongDH < 15)
                            {
                                #region less 15 line
                                for (int i = 0; i < sodongDH; i++)
                                {
                                    Font fo = new Font(bf, 8.0f, Font.NORMAL);
                                    Font fo_ten = new Font(bf, 8.0f, Font.NORMAL);

                                    double soluong = double.Parse(dtSP.Rows[checkSoDong]["SoLuong"].ToString());

                                    cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["mavach"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["chungloai"].ToString(), fo_ten));
                                    cell.SetHorizontalAlignment("LEFT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["tensp"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["sokhung"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["dongco"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["mausac"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);


                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["location"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["location_new"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    checkSoDong++;

                                }

                                #endregion

                                checkPage = false;
                            }
                            else
                            {
                                #region more 15 line
                                for (int i = 0; i < 15; i++)
                                {
                                    Font fo = new Font(bf, 8.0f, Font.NORMAL);
                                    Font fo_ten = new Font(bf, 8.0f, Font.NORMAL);

                                    double soluong = double.Parse(dtSP.Rows[checkSoDong]["SoLuong"].ToString());

                                    cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["mavach"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["chungloai"].ToString(), fo_ten));
                                    cell.SetHorizontalAlignment("LEFT");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["tensp"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["sokhung"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["dongco"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["mausac"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);


                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["location"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["location_new"].ToString(), fo));
                                    cell.SetHorizontalAlignment("CENTER");
                                    cell.SetVerticalAlignment("middle");
                                    cell.BorderWidth = 0.1f;
                                    aTable.AddCell(cell);

                                    checkSoDong++;

                                }

                                #endregion

                                checkPage = true;

                            }
                            string arrDVT = "";

                            if (defineLine == checkSoDong)
                            {
                                {
                                    query = "SELECT SUM(soluong) soluong, b.ten " +
                                    " FROM DoiLoBin_SanPham a INNER JOIN  SanPham c ON a.sanpham_fk = c.pk_seq INNER JOIN  DonViTinh b ON a.DVT_FK = b.pk_seq " +
                                    " WHERE a.doilobin_fk = '" + id + "'GROUP BY b.ten";

                                    DataTable dtDVT = xl.ReadTable(query);
                                    if (dtDVT.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtDVT.Rows.Count; i++)
                                        {
                                            arrDVT += FormatString.ForMatNumber(dtDVT.Rows[i]["soluong"].ToString()) + " " + dtDVT.Rows[i]["ten"].ToString() + ", ";
                                        }
                                    }
                                }

                                cell = new Cell(new Phrase("Tổng: " + arrDVT, new Font(bf, 9.0f, Font.NORMAL)));
                                cell.SetHorizontalAlignment("RIGHT");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                cell.Colspan = 9;
                                aTable.AddCell(cell);

                            }
                        }
                        dtSP.Clear();
                        dtSP.Clone();
                    }

                    dtInfo.Clear();
                    dtInfo.Clone();
                    //content.Add(aTable);

                    #region Information 
                    aTable = new iTextSharp.text.Table(2);
                    aTable.AutoFillEmptyCells = true;
                    aTable.Alignment = Element.ALIGN_LEFT;
                    aTable.Width = 100.0f;
                    aTable.Cellpadding = 0.5f;
                    aTable.Cellspacing = 0.5f;
                    aTable.Widths = new float[] { 25.0f, 25.0f};
                    aTable.Border = 0;

                    ////////////////////////////////
                    ////THEM THONG TIN O DUOI///////
                    ////////////////////////////////

                    cell = new Cell(new Phrase("Number of accompanying original documents:........................................................................................................................ ", new Font(bf, 10.0f, Font.NORMAL)));
                    cell.SetHorizontalAlignment("LEFT");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 4;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase(makhoNHAP + ", Date " + DateTime.Now.Day.ToString() + " Month " + DateTime.Now.Month.ToString() + " Year " + DateTime.Now.Year.ToString(), new Font(bf, 10.0f, Font.ITALIC)));
                    cell.SetHorizontalAlignment("RIGHT");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0;
                    cell.Colspan = 4;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("CheckBy", new Font(bf, 10.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("Manager", new Font(bf, 10.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    aTable.AddCell(cell);
                    
                    cell = new Cell(new Phrase("(Signature)", new Font(bf, 10.0f, Font.ITALIC)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("(Signature)", new Font(bf, 10.0f, Font.ITALIC)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0;
                    cell.Colspan = 1;
                    aTable.AddCell(cell);
                    
                    #endregion

                    content.Add(aTable);
                    pdfDoc.Add(content);

                    //#region In so trang

                    //Paragraph para = new Paragraph();

                    //if (checkPage)
                    //{
                    //    if (count == 0)
                    //    {
                    //        for (int i = 0; i < 4; i++)
                    //        {
                    //            para = new Paragraph(" ", new Font(Font.ITALIC, 9));
                    //            para.Alignment = Element.ALIGN_RIGHT;
                    //            pdfDoc.Add(para);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        for (int i = 0; i < 3; i++)
                    //        {
                    //            para = new Paragraph(" ", new Font(Font.ITALIC, 9));
                    //            para.Alignment = Element.ALIGN_RIGHT;
                    //            pdfDoc.Add(para);
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    for (int i = 0; i < (14 - sodongDH) * 2; i++)
                    //    {
                    //        para = new Paragraph(" ", new Font(Font.ITALIC, 9));
                    //        para.Alignment = Element.ALIGN_RIGHT;
                    //        pdfDoc.Add(para);
                    //    }

                    //    if (count == 0)
                    //    {
                    //        for (int i = 0; i < 4; i++)
                    //        {
                    //            para = new Paragraph(" ", new Font(Font.ITALIC, 9));
                    //            para.Alignment = Element.ALIGN_RIGHT;
                    //            pdfDoc.Add(para);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        for (int i = 0; i < 3; i++)
                    //        {
                    //            para = new Paragraph(" ", new Font(Font.ITALIC, 9));
                    //            para.Alignment = Element.ALIGN_RIGHT;
                    //            pdfDoc.Add(para);
                    //        }
                    //    }
                    //}
                    //para = new Paragraph("Trang " + (count + 1).ToString() + "/" + countPDF.ToString(), new Font(Font.ITALIC, 9));
                    //para.Alignment = Element.ALIGN_RIGHT;
                    //pdfDoc.Add(para);

                    //#endregion

                    content.Clear();
                    pdfDoc.NewPage();

                }
            }
            catch (Exception ex)
            {
                StringWriter sw = new StringWriter();

                string str = "Error when created file PDF...." + ex.Message;
                StringReader sr = new StringReader(str);

                htmlparser.Parse(sr);
            }

            pdfDoc.Close();
            context.Response.Write(pdfDoc);
            context.Response.End();

        }

        private void InDatHangPDF(HttpContext context)
        {
            context.Response.ContentType = "application/pdf";

            string id = "";

            DataTable dtInfo = new DataTable();
            DataTable dtSP = new DataTable();

            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            context.Response.AddHeader("content-disposition", "inline;filename=Order_" + id + ".pdf");
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            context.Response.Charset = Encoding.Unicode.ToString();
            context.Response.ContentEncoding = Encoding.UTF8;

            Document pdfDoc = new Document();

            string dinhdang = context.Request.QueryString["dinhdang"];
            if (dinhdang == null)
                dinhdang = "0";

            if (dinhdang.Equals("1"))
                pdfDoc = new Document(PageSize.A4, 20.0f, 20.0f, 0.0f, 0.0f);
            else
                pdfDoc = new Document(PageSize.A5, 20.0f, 20.0f, 0.0f, 0.0f);

            //Document pdfDoc = new Document(PageSize.A4, 20.0f, 20.0f, 0.0f, 0.0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, context.Response.OutputStream);

            pdfDoc.Open();

            int sodongDH = 0;
            double countPDF = 1;
            int checkSoDong = 0;
            int defineLine = 0;

            try
            {
                ExecuteData xl = new ExecuteData();

                BaseFont bf = BaseFont.CreateFont("c:\\windows\\fonts\\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                Font times = new Font(bf, 10.0f, Font.BOLD);
                Cell cell = new Cell();

                Paragraph content = new Paragraph();

                for (int count = 0; count < countPDF; count++)
                {

                    #region Khung hoa don

                    string query = " SELECT a.pk_seq, a.ngaydathang, a.ngaygiaohang, ISNULL(a.ghichu, '') ghichu, a.trangthai, " +                                     
                    "   ISNULL(a.diachigiaohang, '') diachigiaohang, " +
                    "   ISNULL((SELECT makho FROM Kho WHERE pk_seq = a.kho_fk), '') khoxuat, " +
                    "  	CASE a.loaikho WHEN 0 THEN (SELECT ma FROM KhachHang WHERE pk_seq = a.khachhang_fk)  " +
                    "  				 ELSE  (SELECT makho FROM Kho WHERE pk_seq = a.khonhan_fk) END khonhan, '' AS khuvuc,  " +
                    "  	CASE a.loaixuat WHEN 1 THEN N'Company' " +
                    "  				 WHEN 2 THEN N'Partner' " +
                    "  				 ELSE N'Unknow' END tenloaixuat " +
                    " FROM DatHang a " +
                    " WHERE a.pk_seq in (" + id + ") ";

                    dtInfo = xl.ReadTable(query);                   
                   
                    iTextSharp.text.Table aTable = new iTextSharp.text.Table(3);
                    aTable.AutoFillEmptyCells = true;
                    aTable.Alignment = Element.ALIGN_LEFT;
                    aTable.Width = 100.0f;
                    aTable.Cellpadding = 0.0f;
                    aTable.Cellspacing = 0.0f;
                    aTable.Widths = new float[] { 30.0f, 100.0f, 100.0f };
                    aTable.BorderWidth = 0.0f;

                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(pathImage);
                    jpg.ScaleToFit(70.0f, 100.0f);

                    cell = new Cell(jpg);
                    cell.Rowspan = 3;
                    cell.BorderWidth = 0;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase(congty_chinhanh, new Font(bf, 9.0f, Font.NORMAL)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    aTable.AddCell(cell);


                    cell = new Cell(new Phrase(diachi_title, new Font(bf, 10.0f, Font.NORMAL)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase(" Department: ", new Font(bf, 10.0f, Font.NORMAL)));
                    cell.BorderWidth = 0;
                    cell.Colspan = 2;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("", new Font(bf, 10.0f, Font.NORMAL)));
                    cell.BorderWidth = 0;
                    aTable.AddCell(cell);

                    pdfDoc.Add(aTable);

                    aTable = new iTextSharp.text.Table(5);
                    aTable.AutoFillEmptyCells = true;
                    aTable.Alignment = Element.ALIGN_LEFT;
                    aTable.Width = 100.0f;
                    aTable.Cellpadding = 0.5f;
                    aTable.Cellspacing = 0.5f;
                    aTable.Widths = new float[] { 20.0f, 20.0f, 20.0f, 20.0f, 20.0f };
                    aTable.BorderWidth = 0.0f;

                    cell = new Cell(new Phrase("ORDER", new Font(bf, 16.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0;
                    cell.Colspan = 5;
                    aTable.AddCell(cell);

                    string tt = dtInfo.Rows[0]["trangthai"].ToString();
                    string trangthaiDH = "";
                    if (tt.Equals("0"))
                        trangthaiDH = "Proccesing";
                    else if (tt.Equals("1"))
                        trangthaiDH = "Approved";
                    else if (tt.Equals("2"))
                        trangthaiDH = "Canceled";
                    else if (tt.Equals("3"))
                        trangthaiDH = "Đã xác nhận";
                    else if (tt.Equals("4"))
                        trangthaiDH = "Moved stock out";
                    else if (tt.Equals("5"))
                        trangthaiDH = "Done";                   

                    cell = new Cell(new Phrase("ID: " + dtInfo.Rows[0]["pk_seq"].ToString(), new Font(bf, 10.0f, Font.ITALIC)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("small");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 5;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("Date: " + dtInfo.Rows[0]["ngaydathang"].ToString() + " - Delivery date: " + dtInfo.Rows[0]["ngaygiaohang"].ToString(), new Font(bf, 10.0f, Font.ITALIC)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("small");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 5;
                    aTable.AddCell(cell);
                    
                    cell = new Cell(new Phrase("From:           : " + dtInfo.Rows[0]["khoxuat"].ToString(), new Font(bf, 10.0f, Font.NORMAL)));
                    cell.SetHorizontalAlignment("LEFT");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 3;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("To:           : " + dtInfo.Rows[0]["khonhan"].ToString(), new Font(bf, 10.0f, Font.NORMAL)));
                    cell.SetHorizontalAlignment("LEFT");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 2;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("Type:           : " + dtInfo.Rows[0]["tenloaixuat"].ToString(), new Font(bf, 10.0f, Font.NORMAL)));
                    cell.SetHorizontalAlignment("LEFT");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0f;
                    cell.Colspan = 5;
                    aTable.AddCell(cell);

                    if (dtInfo.Rows[0]["ghichu"].ToString().Trim().Length > 0)
                    {
                        cell = new Cell(new Phrase("Ghi chú                  : " + dtInfo.Rows[0]["ghichu"].ToString(), new Font(bf, 10.0f, Font.NORMAL)));
                        cell.SetHorizontalAlignment("LEFT");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0f;
                        cell.Colspan = 5;
                        aTable.AddCell(cell);
                    }

                    #endregion
                    // VIP
                    content.Add(aTable);

                    #region Xuat kho hang ban
                    aTable = new iTextSharp.text.Table(7);
                    aTable.AutoFillEmptyCells = true;
                    aTable.Alignment = Element.ALIGN_LEFT;
                    aTable.Width = 100.0f;
                    aTable.Cellpadding = 1.0f;
                    aTable.Cellspacing = 1.0f;
                    aTable.Widths = new float[] { 6.0f, 15.0f, 40.0f, 8.0f, 8.0f, 8.0f, 8.0f };
                    aTable.BorderWidth = 0.01f;

                    cell = new Cell(new Phrase("No", new Font(bf, 10.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0.01f;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("PartNo", new Font(bf, 10.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0.01f;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("Part name", new Font(bf, 10.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0.01f;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("Unit", new Font(bf, 10.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0.01f;
                    aTable.AddCell(cell);                   

                    cell = new Cell(new Phrase("Qty", new Font(bf, 10.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0.01f;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("Qty delivered", new Font(bf, 10.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0.01f;
                    aTable.AddCell(cell);

                    cell = new Cell(new Phrase("Qty received", new Font(bf, 10.0f, Font.BOLD)));
                    cell.SetHorizontalAlignment("CENTER");
                    cell.SetVerticalAlignment("middle");
                    cell.BorderWidth = 0.01f;
                    aTable.AddCell(cell); 

                    #region truy van SQL


                    query = " SELECT c.ma as masp, c.ten as tensp, (SELECT ten FROM DonViTinh WHERE pk_seq = b.dvt_fk) as donvitinh, b.soluong as soluongDAT,  " +
                             " 	ISNULL(GIAO.soluongGIAO, 0) soluongGIAO, ISNULL(TRA.soluongTRA, 0) soluongTRA " +
                             " FROM DatHang a INNER JOIN  DatHang_SanPham b ON a.pk_seq = b.dathang_fk " +
                             " 	INNER JOIN  SanPham c ON b.sanpham_fk = c.pk_seq " +                            
                             " 	LEFT JOIN " +
                             " 	( " +
                             " 		SELECT dathang_fk, sanpham_fk, SUM(soluong) as soluongGIAO " +
                             " 		FROM DatHang_SanPham_GiaoHang " +
                             " 		WHERE dathang_fk = '"+ id +"' " +
                             " 		GROUP BY dathang_fk, sanpham_fk " +
                             " 	)GIAO ON b.dathang_fk = GIAO.dathang_fk AND b.sanpham_fk = GIAO.sanpham_fk " +
                             " 	LEFT JOIN  " +
                             " 	( " +
                             " 		SELECT d.dathang_fk, b.sanpham_fk, c.dvt_fk, c.soluong as soluongTRA " +
                             " 		FROM DonTraHang a INNER JOIN  DonTraHang_SanPham_ChiTiet b ON a.pk_seq = b.dontrahang_fk " +
                             " 			INNER JOIN  DonTraHang_SanPham c ON b.dontrahang_fk = c.dontrahang_fk AND b.sanpham_fk = c.sanpham_fk " +
                             " 			INNER JOIN  DonHang d ON a.donhang_fk = d.pk_seq " +
                             " 		WHERE d.dathang_fk = '"+ id +"' AND a.trangthai = 1 " +
                             " 	)TRA ON b.dathang_fk = TRA.dathang_fk AND b.sanpham_fk = TRA.sanpham_fk AND b.dvt_fk = TRA.dvt_fk " +
                             " WHERE a.pk_seq = '"+ id +"' ";

                    dtSP = xl.ReadTable(query);


                    #endregion

                    if (sodongDH == 0)
                    {
                        sodongDH = dtSP.Rows.Count;
                        // tinh so lan lap
                        countPDF = (Math.Round(sodongDH / 20.0 + 0.499));
                    }

                    defineLine = dtSP.Rows.Count;

                    if (defineLine > 0)
                    {
                        
                        Font fo = new Font(bf, 9.0f, Font.NORMAL);
                        Font fo_ten = new Font(bf, 9.0f, Font.NORMAL);
                        double dongia = 0;

                        #region moi lan in trang, in 20 dong - don hang ban

                        sodongDH = defineLine - count * 20;
                        if (sodongDH < 20)
                        {

                            #region In don hang

                            for (int i = 0; i < sodongDH; i++)
                            {
                                double soluong = double.Parse(dtSP.Rows[checkSoDong]["soLuongdat"].ToString());
                                double soluongGIAO = double.Parse(dtSP.Rows[checkSoDong]["soluongGIAO"].ToString());
                                double soluongTRA = double.Parse(dtSP.Rows[checkSoDong]["soluongTRA"].ToString());
                                double soluongNHAN = soluongGIAO - soluongTRA;
                                                                                             

                                cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["masp"].ToString(), fo));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["tensp"].ToString(), fo_ten));
                                cell.SetHorizontalAlignment("LEFT");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);


                                cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["donvitinh"].ToString(), fo));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);


                                cell = new Cell(new Phrase(FormatString.ForMatNumber(soluong.ToString()) + " ", fo));
                                cell.SetHorizontalAlignment("RIGHT");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase(" ", fo));
                                cell.SetHorizontalAlignment("RIGHT");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase(" ", fo));
                                cell.SetHorizontalAlignment("RIGHT");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);

                                checkSoDong++;
                            }
                            #endregion

                        }
                        else
                        {
                            #region In don hang

                            for (int i = 0; i < 20; i++)
                            {

                                double soluong = double.Parse(dtSP.Rows[checkSoDong]["soLuongdat"].ToString());
                                double soluongGIAO = double.Parse(dtSP.Rows[checkSoDong]["soluongGIAO"].ToString());
                                double soluongTRA = double.Parse(dtSP.Rows[checkSoDong]["soluongTRA"].ToString());
                                double soluongNHAN = soluongGIAO - soluongTRA;

                                cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["masp"].ToString(), fo));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["tensp"].ToString(), fo_ten));
                                cell.SetHorizontalAlignment("LEFT");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);


                                cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["donvitinh"].ToString(), fo));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);


                                cell = new Cell(new Phrase(FormatString.ForMatNumber(soluong.ToString()) + " ", fo));
                                cell.SetHorizontalAlignment("RIGHT");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);

                               
                                cell = new Cell(new Phrase( " ", fo));
                                cell.SetHorizontalAlignment("RIGHT");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase(" ", fo));
                                cell.SetHorizontalAlignment("RIGHT");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0.01f;
                                aTable.AddCell(cell);

                                checkSoDong++;
                            }
                            #endregion
                        }
                        #endregion

                        //string arrDVT = "";

                        //if (checkSoDong == defineLine)
                        //{
                        //    query = "SELECT SUM(soluong) soluong, b.ten FROM DatHang_SanPham a INNER JOIN  DonViTinh b ON a.DVT_FK = b.pk_seq " +
                        //                " WHERE a.dathang_fk = '" + id + "'GROUP BY b.ten";                                    
                        //    DataTable dtDVT = xl.ReadTable(query);
                        //    if (dtDVT.Rows.Count > 0)
                        //    {
                        //        for (int i = 0; i < dtDVT.Rows.Count; i++)
                        //        {
                        //            arrDVT += dtDVT.Rows[i]["soluong"].ToString() + " " + dtDVT.Rows[i]["ten"].ToString() + ", ";
                        //        }
                        //    }
                        //}

                        //cell = new Cell(new Phrase("            Tổng số lượng đặt hàng: " + arrDVT, new Font(bf, 10.0f, Font.NORMAL)));
                        //cell.SetHorizontalAlignment("LEFT");
                        //cell.SetVerticalAlignment("middle");
                        //cell.BorderWidth = 0.01f;
                        //cell.Colspan = 7;
                        //aTable.AddCell(cell);                       
                       
                    }
                
                    #endregion
                    
                    content.Add(aTable);                    
                    pdfDoc.Add(content);

                    if (checkSoDong == dtSP.Rows.Count)
                    {
                        #region Thong tin duoi
                        aTable = new iTextSharp.text.Table(3);
                        aTable.AutoFillEmptyCells = true;
                        aTable.Alignment = Element.ALIGN_LEFT;
                        aTable.Width = 100.0f;
                        aTable.Cellpadding = 0.5f;
                        aTable.Cellspacing = 0.5f;
                        aTable.Widths = new float[] { 30.0f, 30.0f, 30.0f };
                        aTable.Border = 0;

                        ////////////////////////////////
                        ////THEM THONG TIN O DUOI///////
                        ////////////////////////////////


                        cell = new Cell(new Phrase(dtInfo.Rows[0]["khoxuat"].ToString() + ", " + " Date " + DateTime.Now.Day.ToString() + " Month " + DateTime.Now.Month.ToString() + " Year " + DateTime.Now.Year.ToString(), new Font(bf, 11.0f, Font.ITALIC)));
                        cell.SetHorizontalAlignment("RIGHT");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0;
                        cell.Colspan = 3;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Orderer", new Font(bf, 10.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Roundsman", new Font(bf, 10.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0;
                        aTable.AddCell(cell);

                        cell = new Cell(new Phrase("Receiver", new Font(bf, 10.0f, Font.BOLD)));
                        cell.SetHorizontalAlignment("CENTER");
                        cell.SetVerticalAlignment("middle");
                        cell.BorderWidth = 0;
                        aTable.AddCell(cell);

                        content.Add(aTable);
                        pdfDoc.Add(content);
                        #endregion
                    }
                    // Xoa data in, tao content trong
                    content.Clear();
                    content.Clone();

                    pdfDoc.NewPage();
                }

                dtInfo.Clear();             
                dtSP.Clear();               
                dtInfo.Clone();               
                dtSP.Clone();
            }
            catch (Exception ex)
            {
                StringWriter sw = new StringWriter();

                string str = "Error when created file PDF...." + ex.Message;
                StringReader sr = new StringReader(str);

                htmlparser.Parse(sr);
            }

            pdfDoc.Close();
            context.Response.Write(pdfDoc);
            context.Response.End();

        }

        private void DieuChuyenPDF(HttpContext context)
        {
            context.Response.ContentType = "application/pdf";

            string id = "";

            DataTable dtInfo = new DataTable();
            DataTable dtSP = new DataTable();

            if (context.Request.QueryString["id"] != null)
                id = context.Request.QueryString["id"].ToString();

            context.Response.AddHeader("content-disposition", "inline;filename=DieuChuyen_" + id + ".pdf");
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            context.Response.Charset = Encoding.Unicode.ToString();
            context.Response.ContentEncoding = Encoding.UTF8;

            Document pdfDoc = new Document();

            string dinhdang = context.Request.QueryString["dinhdang"];
            if (dinhdang == null)
                dinhdang = "0";

            if (dinhdang.Equals("1"))
                pdfDoc = new Document(PageSize.A4, 20.0f, 20.0f, 0.0f, 0.0f);
            else
                pdfDoc = new Document(PageSize.A5, 20.0f, 20.0f, 0.0f, 0.0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, context.Response.OutputStream);

            pdfDoc.Open();

            int sodongDH = 0;
            int checkSoDong = 0;
            int defineLine = 0;
            double countPDF = 1;
            try
            {
                ExecuteData xl = new ExecuteData();

                //TẠM THỜI ĐỂ XỬ LÝ NHỮNG ĐƠN CŨ
                //xl.updateTONGGIATRI(id);

                BaseFont bf = BaseFont.CreateFont("c:\\windows\\fonts\\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                Font times = new Font(bf, 10.0f, Font.BOLD);
                Cell cell = new Cell();

                Paragraph content = new Paragraph();

                string kho = "";
                string khuvuc = "";
                for (int loop = 0; loop < 2; loop++)
                {

                    if (loop == 0)
                    {
                        for (int count = 0; count < countPDF; count++)
                        {

                            string query = "SELECT a.pk_seq, a.trangthai, a.ngaynhap, ISNULL(a.kho_fk, 0) kho_fk, 0 thueVAT, 0 sohoadon, " +
                                           "    a.ghichu, ISNULL(b.makho, '') tenkho, ISNULL(b.diachi, '') diachi, 0 AS khuvuc " +                                           
                                           " FROM DoiLoBin a INNER JOIN Kho b on a.kho_fk = b.pk_seq " +
                                           " WHERE a.pk_seq = '" + id + "'";

                            dtInfo = xl.ReadTable(query);

                            kho = dtInfo.Rows[0]["kho_fk"].ToString();
                            khuvuc = dtInfo.Rows[0]["khuvuc"].ToString();

                            iTextSharp.text.Table aTable = new iTextSharp.text.Table(3);
                            aTable.AutoFillEmptyCells = true;
                            aTable.Alignment = Element.ALIGN_LEFT;
                            aTable.Width = 100.0f;
                            aTable.Cellpadding = 0.0f;
                            aTable.Cellspacing = 0.0f;
                            aTable.Widths = new float[] { 30.0f, 100.0f, 100.0f };
                            aTable.BorderWidth = 0.0f;

                            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(pathImage);
                            jpg.ScaleToFit(70.0f, 100.0f);

                            cell = new Cell(jpg);
                            cell.Rowspan = 3;
                            cell.BorderWidth = 0;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase(congty_chinhanh, new Font(bf, 9.0f, Font.NORMAL)));
                            cell.BorderWidth = 0;
                            cell.Colspan = 2;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase(diachi_title, new Font(bf, 10.0f, Font.NORMAL)));
                            cell.BorderWidth = 0;
                            cell.Colspan = 2;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase(" Department: ", new Font(bf, 10.0f, Font.NORMAL)));
                            cell.BorderWidth = 0;
                            cell.Colspan = 2;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase("", new Font(bf, 10.0f, Font.NORMAL)));
                            cell.BorderWidth = 0;
                            //cell.Colspan = 2;
                            aTable.AddCell(cell);

                            pdfDoc.Add(aTable);

                            aTable = new iTextSharp.text.Table(5);
                            aTable.AutoFillEmptyCells = true;
                            aTable.Alignment = Element.ALIGN_LEFT;
                            aTable.Width = 100.0f;
                            aTable.Cellpadding = 0.5f;
                            aTable.Cellspacing = 0.5f;
                            aTable.Widths = new float[] { 30.0f, 20.0f, 20.0f, 20.0f, 10.0f };
                            aTable.BorderWidth = 0.0f;

                            cell = new Cell(new Phrase("PHIẾU ĐIỀU CHUYỂN", new Font(bf, 16.0f, Font.BOLD)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0;
                            cell.Colspan = 5;
                            aTable.AddCell(cell);

                            string tt = dtInfo.Rows[0]["trangthai"].ToString();
                            string trangthaiDH = "";
                            if (tt.Equals("0"))
                                trangthaiDH = "Đang xử lý";
                            else if (tt.Equals("1"))
                                trangthaiDH = "Đã xử lý";
                            else if (tt.Equals("2"))
                                trangthaiDH = "Đã hủy";
                            
                            cell = new Cell(new Phrase("Trạng thái: " + trangthaiDH, new Font(bf, 10.0f, Font.ITALIC)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("small");
                            cell.BorderWidth = 0f;
                            cell.Colspan = 5;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase("Ngày điều chuyển: " + dtInfo.Rows[0]["ngaynhap"].ToString(), new Font(bf, 10.0f, Font.ITALIC)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("small");
                            cell.BorderWidth = 0f;
                            cell.Colspan = 5;
                            aTable.AddCell(cell);
                            
                            //cell = new Cell(new Phrase("Điện thoại: " + dtInfo.Rows[0]["dienthoai"].ToString(), new Font(bf, 10.0f, Font.NORMAL)));
                            //cell.SetHorizontalAlignment("LEFT");
                            //cell.SetVerticalAlignment("middle");
                            //cell.BorderWidth = 0f;
                            //cell.Colspan = 2;
                            //aTable.AddCell(cell);

                            cell = new Cell(new Phrase("Điều chuyển tại kho  : " + dtInfo.Rows[0]["tenkho"].ToString() + "                        Số điều chuyển:       " + dtInfo.Rows[0]["pk_seq"].ToString(), new Font(bf, 10.0f, Font.NORMAL)));
                            cell.SetHorizontalAlignment("LEFT");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0f;
                            cell.Colspan = 5;
                            aTable.AddCell(cell);

                            if (dtInfo.Rows[0]["ghichu"].ToString().Trim().Length > 0)
                            {
                                cell = new Cell(new Phrase("Ghi chú         : " + dtInfo.Rows[0]["ghichu"].ToString(), new Font(bf, 10.0f, Font.NORMAL)));
                                cell.SetHorizontalAlignment("LEFT");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0f;
                                cell.Colspan = 5;
                                aTable.AddCell(cell);
                            }

                            // VIP
                            content.Add(aTable);

                            #region Xuat kho hang ban
                            aTable = new iTextSharp.text.Table(7);
                            aTable.AutoFillEmptyCells = true;
                            aTable.Alignment = Element.ALIGN_LEFT;
                            aTable.Width = 100.0f;
                            aTable.Cellpadding = 1.0f;
                            aTable.Cellspacing = 1.0f;
                            aTable.Widths = new float[] { 6.0f, 20.0f, 35.0f, 8.0f, 15.0f, 15.0f, 8.0f };
                            aTable.BorderWidth = 0.01f;

                            cell = new Cell(new Phrase("No", new Font(bf, 10.0f, Font.BOLD)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.01f;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase("Mã", new Font(bf, 10.0f, Font.BOLD)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.01f;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase("Part name", new Font(bf, 10.0f, Font.BOLD)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.01f;
                            aTable.AddCell(cell);


                            cell = new Cell(new Phrase("DVT", new Font(bf, 10.0f, Font.BOLD)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.01f;
                            aTable.AddCell(cell);

                            //cell = new Cell(new Phrase("Xuất xứ", new Font(bf, 10.0f, Font.BOLD)));
                            //cell.SetHorizontalAlignment("CENTER");
                            //cell.SetVerticalAlignment("middle");
                            //cell.BorderWidth = 0.01f;
                            //aTable.AddCell(cell);

                            cell = new Cell(new Phrase("Location", new Font(bf, 10.0f, Font.BOLD)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.01f;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase("Location mới", new Font(bf, 10.0f, Font.BOLD)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.01f;
                            aTable.AddCell(cell);

                            cell = new Cell(new Phrase("Qty", new Font(bf, 10.0f, Font.BOLD)));
                            cell.SetHorizontalAlignment("CENTER");
                            cell.SetVerticalAlignment("middle");
                            cell.BorderWidth = 0.01f;
                            aTable.AddCell(cell);

                            #region truy van SQL

                            //CHIẾT KHẤU VÀ KM ĐÃ TRỪ VÀO ĐƠN GIÁ CHƯA VAT
                            query = " 	SELECT sp.ma, sp.ten, dh_sp.solo, dv.ten as dvt," +
                            "       ISNULL((SELECT ma FROM Location WHERE pk_seq = dh_sp.location_new_fk), '') locationNEW, " + 
                            "       loc.ma AS location, '' AS xuatxu, dh_sp.soluong  " +
                            " 	FROM DoiLoBin_SanPham_ChiTiet dh_sp INNER JOIN  SanPham sp on dh_sp.sanpham_fk = sp.pk_seq  AND dh_sp.doilobin_fk = '" + id + "' " +                            
                            "       INNER JOIN Location loc ON dh_sp.location_fk = loc.pk_seq " +
                            " 		INNER JOIN  DonViTinh dv on dh_sp.dvt_fk = dv.pk_seq " +
                            " 	WHERE sp.pk_seq > 0 ";
                            dtSP = xl.ReadTable(query);


                            #endregion

                            if (sodongDH == 0)
                            {
                                sodongDH = dtSP.Rows.Count;
                                // tinh so lan lap
                                countPDF = (Math.Round(sodongDH / 18.0 + 0.499));
                            }

                            defineLine = dtSP.Rows.Count;

                            if (defineLine > 0)
                            {
                                Font fo = new Font(bf, 9.0f, Font.NORMAL);
                                Font fo_ten = new Font(bf, 9.0f, Font.NORMAL);

                                #region moi lan in trang, in 18 dong - don hang ban

                                sodongDH = defineLine - count * 18;
                                if (sodongDH < 18)
                                {

                                    #region In don hang

                                    for (int i = 0; i < sodongDH; i++)
                                    {

                                        cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                        cell.SetHorizontalAlignment("CENTER");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);

                                        cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["ma"].ToString(), fo));
                                        cell.SetHorizontalAlignment("CENTER");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);

                                        cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["ten"].ToString(), fo_ten));
                                        cell.SetHorizontalAlignment("LEFT");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);

                                        cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["dvt"].ToString(), fo));
                                        cell.SetHorizontalAlignment("CENTER");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);

                                        //cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["xuatxu"].ToString(), fo));
                                        //cell.SetHorizontalAlignment("CENTER");
                                        //cell.SetVerticalAlignment("middle");
                                        //cell.BorderWidth = 0.01f;
                                        //aTable.AddCell(cell);

                                        cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["location"].ToString(), fo));
                                        cell.SetHorizontalAlignment("CENTER");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);

                                        cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["locationNEW"].ToString() + " ", fo));
                                        cell.SetHorizontalAlignment("CENTER");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);

                                        cell = new Cell(new Phrase(FormatString.ForMatNumber(dtSP.Rows[checkSoDong]["soluong"].ToString()) + " ", fo));
                                        cell.SetHorizontalAlignment("RIGHT");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);

                                        checkSoDong++;
                                    }
                                    #endregion

                                }
                                else
                                {
                                    #region In don hang

                                    for (int i = 0; i < 18; i++)
                                    {
                                        cell = new Cell(new Phrase((checkSoDong + 1).ToString(), fo));
                                        cell.SetHorizontalAlignment("CENTER");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);

                                        cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["ma"].ToString(), fo));
                                        cell.SetHorizontalAlignment("CENTER");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);

                                        cell = new Cell(new Phrase(" " + dtSP.Rows[checkSoDong]["ten"].ToString(), fo_ten));
                                        cell.SetHorizontalAlignment("LEFT");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);

                                        cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["dvt"].ToString(), fo));
                                        cell.SetHorizontalAlignment("CENTER");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);

                                        //cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["xuatxu"].ToString(), fo));
                                        //cell.SetHorizontalAlignment("CENTER");
                                        //cell.SetVerticalAlignment("middle");
                                        //cell.BorderWidth = 0.01f;
                                        //aTable.AddCell(cell);

                                        cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["location"].ToString(), fo));
                                        cell.SetHorizontalAlignment("CENTER");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);


                                        cell = new Cell(new Phrase(dtSP.Rows[checkSoDong]["locationNEW"].ToString() + " ", fo));
                                        cell.SetHorizontalAlignment("CENTER");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);

                                        cell = new Cell(new Phrase(FormatString.ForMatNumber(dtSP.Rows[checkSoDong]["soluong"].ToString()) + " ", fo));
                                        cell.SetHorizontalAlignment("RIGHT");
                                        cell.SetVerticalAlignment("middle");
                                        cell.BorderWidth = 0.01f;
                                        aTable.AddCell(cell);

                                        checkSoDong++;
                                    }
                                    #endregion
                                }
                                #endregion

                            }

                            content.Add(aTable);


                            #region Thong tin duoi
                            if (checkSoDong == defineLine)
                            {

                                aTable = new iTextSharp.text.Table(5);
                                aTable.AutoFillEmptyCells = true;
                                aTable.Alignment = Element.ALIGN_LEFT;
                                aTable.Width = 100.0f;
                                aTable.Cellpadding = 0.5f;
                                aTable.Cellspacing = 0.5f;
                                aTable.Widths = new float[] { 20.0f, 20.0f, 20.0f, 20.0f, 20.0f };
                                aTable.Border = 0;

                                ////////////////////////////////
                                ////THEM THONG TIN O DUOI///////
                                ////////////////////////////////


                                cell = new Cell(new Phrase(dtInfo.Rows[0]["tenkho"].ToString() + ", " + " Ngày " + DateTime.Now.Day.ToString() + " Month " + DateTime.Now.Month.ToString() + " Year " + DateTime.Now.Year.ToString(), new Font(bf, 11.0f, Font.ITALIC)));
                                cell.SetHorizontalAlignment("RIGHT");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0;
                                cell.Colspan = 5;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Người nhận", new Font(bf, 10.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Stocker", new Font(bf, 10.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Người lập", new Font(bf, 10.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Inventory management ", new Font(bf, 10.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("Manager", new Font(bf, 10.0f, Font.BOLD)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("(Signature)", new Font(bf, 10.0f, Font.ITALIC)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("(Signature)", new Font(bf, 10.0f, Font.ITALIC)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("(Signature)", new Font(bf, 10.0f, Font.ITALIC)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("(Signature)", new Font(bf, 10.0f, Font.ITALIC)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0;
                                aTable.AddCell(cell);

                                cell = new Cell(new Phrase("(Signature)", new Font(bf, 10.0f, Font.ITALIC)));
                                cell.SetHorizontalAlignment("CENTER");
                                cell.SetVerticalAlignment("middle");
                                cell.BorderWidth = 0;
                                aTable.AddCell(cell);

                                content.Add(aTable);

                            }
                            pdfDoc.Add(content);

                            #endregion

                            // Xoa data in, tao content trong
                            content.Clear();
                            content.Clone();

                            pdfDoc.NewPage();
                        }
                        #endregion

                    }
                }
                dtInfo.Clear();
                dtSP.Clear();

                dtInfo.Clone();
                dtSP.Clone();
            }
            catch (Exception ex)
            {
                StringWriter sw = new StringWriter();

                string str = "Error when created file PDF...." + ex.Message;
                StringReader sr = new StringReader(str);

                htmlparser.Parse(sr);
            }

            pdfDoc.Close();
            context.Response.Write(pdfDoc);
            context.Response.End();

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
             
        //DOC TIEN
        private static string Chu(string gNumber)
        {
            string result = "";
            switch (gNumber)
            {
                case "0":
                    result = "không";
                    break;
                case "1":
                    result = "một";
                    break;
                case "2":
                    result = "hai";
                    break;
                case "3":
                    result = "ba";
                    break;
                case "4":
                    result = "bốn";
                    break;
                case "5":
                    result = "năm";
                    break;
                case "6":
                    result = "sáu";
                    break;
                case "7":
                    result = "bảy";
                    break;
                case "8":
                    result = "tám";
                    break;
                case "9":
                    result = "chín";
                    break;
            }
            return result;
        }

        private static string Donvi(string so)
        {
            string Kdonvi = "";

            if (so.Equals("1"))
                Kdonvi = "";
            if (so.Equals("2"))
                Kdonvi = "nghìn";
            if (so.Equals("3"))
                Kdonvi = "triệu";
            if (so.Equals("4"))
                Kdonvi = "tỷ";
            if (so.Equals("5"))
                Kdonvi = "nghìn tỷ";
            if (so.Equals("6"))
                Kdonvi = "triệu tỷ";
            if (so.Equals("7"))
                Kdonvi = "tỷ tỷ";

            return Kdonvi;
        }

        private static string Tach(string tach3)
        {
            string Ktach = "";
            if (tach3.Equals("000"))
                return "";
            if (tach3.Length == 3)
            {
                string tr = tach3.Trim().Substring(0, 1).ToString().Trim();
                string ch = tach3.Trim().Substring(1, 1).ToString().Trim();
                string dv = tach3.Trim().Substring(2, 1).ToString().Trim();
                if (tr.Equals("0") && ch.Equals("0"))
                    Ktach = " không trăm lẻ " + Chu(dv.ToString().Trim()) + " ";
                if (!tr.Equals("0") && ch.Equals("0") && dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm ";
                if (!tr.Equals("0") && ch.Equals("0") && !dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm lẻ " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (tr.Equals("0") && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm mười " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("0"))
                    Ktach = " không trăm mười ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("5"))
                    Ktach = " không trăm mười lăm ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười " + Chu(dv.Trim()).Trim() + " ";

                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười lăm ";

            }


            return Ktach;

        }

        public static string So_chu(double gNum)
        {
            if (gNum == 0)
                return "Không đồng";

            string lso_chu = "";
            string tach_mod = "";
            string tach_conlai = "";
            double Num = Math.Round(gNum, 0);
            string gN = Convert.ToString(Num);
            int m = Convert.ToInt32(gN.Length / 3);
            int mod = gN.Length - m * 3;
            string dau = "[+]";

            // Dau [+ , - ]
            if (gNum < 0)
                dau = "[-]";
            dau = "";

            // Tach hang lon nhat
            if (mod.Equals(1))
                tach_mod = "00" + Convert.ToString(Num.ToString().Trim().Substring(0, 1)).Trim();
            if (mod.Equals(2))
                tach_mod = "0" + Convert.ToString(Num.ToString().Trim().Substring(0, 2)).Trim();
            if (mod.Equals(0))
                tach_mod = "000";
            // Tach hang con lai sau mod :
            if (Num.ToString().Length > 2)
                tach_conlai = Convert.ToString(Num.ToString().Trim().Substring(mod, Num.ToString().Length - mod)).Trim();

            ///don vi hang mod
            int im = m + 1;
            if (mod > 0)
                lso_chu = Tach(tach_mod).ToString().Trim() + " " + Donvi(im.ToString().Trim());
            /// Tach 3 trong tach_conlai

            int i = m;
            int _m = m;
            int j = 1;
            string tach3 = "";
            string tach3_ = "";

            while (i > 0)
            {
                tach3 = tach_conlai.Trim().Substring(0, 3).Trim();
                tach3_ = tach3;
                lso_chu = lso_chu.Trim() + " " + Tach(tach3.Trim()).Trim();
                m = _m + 1 - j;
                if (!tach3_.Equals("000"))
                    lso_chu = lso_chu.Trim() + " " + Donvi(m.ToString().Trim()).Trim();
                tach_conlai = tach_conlai.Trim().Substring(3, tach_conlai.Trim().Length - 3);

                i = i - 1;
                j = j + 1;
            }
            if (lso_chu.Trim().Substring(0, 1).Equals("k"))
                lso_chu = lso_chu.Trim().Substring(10, lso_chu.Trim().Length - 10).Trim();
            if (lso_chu.Trim().Substring(0, 1).Equals("l"))
                lso_chu = lso_chu.Trim().Substring(2, lso_chu.Trim().Length - 2).Trim();
            if (lso_chu.Trim().Length > 0)
                lso_chu = dau.Trim() + " " + lso_chu.Trim().Substring(0, 1).Trim().ToUpper() + lso_chu.Trim().Substring(1, lso_chu.Trim().Length - 1).Trim() + " đồng chẵn.";

            return lso_chu.ToString().Trim();

        }


    }
}