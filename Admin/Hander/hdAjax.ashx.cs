using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using HLVTimeSheet.Model;
using HLVTimeSheet.AcsessData;

namespace HLVTimeSheet.Admin.Hander
{
    /// <summary>
    /// Summary description for hdAjax
    /// </summary>
    public class hdAjax : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string type = context.Request.QueryString["type"];
            if (type == null)
                type = "";
            
            if (type.Equals("viewDetailItemStockOut"))
            {
                ViewDetailItem_StockOut(context);
            }
            else if (type.Equals("viewDetailItemStockIn"))
            {
                viewDetailItem_StockIn(context);
            }            
            else if (type.Equals("loadLocation"))
            {
                //loadInfo_Location(context);
            }                       
            else if (type.Equals("CheckLocation"))
            {
                CheckLocation(context);
            }
            else if (type.Equals("CheckColor"))
            {
                CheckColor(context);
            }
            else if (type.Equals("loadInforItemStockIn"))
            {
                LoadInforItemStockIn(context);
            }
            else if (type.Equals("printerLabel"))
            {
                PrinterLabel(context);
            }
        }
        
        private void ViewDetailItem_StockOut(HttpContext context)
        {
            string donhang_fk = "";
            //string location_fk = "";
            string sanpham_fk = "";
            string mausac_fk = "";
            string plNo = "";
            string solo = "";
            string info = "";
            string trangthai = "0";
            string scanByPDA = "0";

            ExecuteData xl = new ExecuteData();

            if (context.Request.QueryString["donhang_fk"] != null)
                donhang_fk = context.Request.QueryString["donhang_fk"].ToString();

            //if (context.Request.QueryString["location_fk"] != null)
            //    location_fk = context.Request.QueryString["location_fk"].ToString();

            if (context.Request.QueryString["sanpham_fk"] != null)
                sanpham_fk = context.Request.QueryString["sanpham_fk"].ToString();

            if (context.Request.QueryString["plNo"] != null)
                plNo = context.Request.QueryString["plNo"].ToString();

            if (context.Request.QueryString["solo"] != null)
                solo = context.Request.QueryString["solo"].ToString();

            if (context.Request.QueryString["info"] != null)
                info = context.Request.QueryString["info"].ToString();

            if (context.Request.QueryString["mausac_fk"] != null)
                mausac_fk = context.Request.QueryString["mausac_fk"].ToString();

            if (context.Request.QueryString["trangthai"] != null)
                trangthai = context.Request.QueryString["trangthai"].ToString();

            if (context.Request.QueryString["scanByPDA"] != null)
                scanByPDA = context.Request.QueryString["scanByPDA"].ToString();

            string userId = "";
            if (context.Session["userId"].ToString() != null)
                userId = context.Session["userId"].ToString();

            string msg = "<table style='width:100%; font-size:small;' cellpadding='2' cellspacing='1'> " +
            " <tr> " +
            " 	<th style='text-align:center; width:4%;'>No</th> " +
            " 	<th style='text-align:center; width:10%;'>Location</th> " +
            " 	<th style='text-align:center; width:10%;'>Order No.</th> " +
            " 	<th style='text-align:center; width:7%;'>Carton No.</th> " +
            " 	<th style='text-align:center; width:12%;'>Size</th> " +
            " 	<th style='text-align:center; width:15%;'>Barcode</th> " +
            " 	<th style='text-align:center; width:7%;'>Order</th> " +
            " 	<th style='text-align:center; width:7%;'>Suggest</th> " +
            " 	<th style='text-align:center; width:18%;'></th> " +
            " </tr> "; 
           
            string query = "";
            if (donhang_fk.Trim().Length <= 0)
                donhang_fk = "0";

            string condition = "";
            string conditionK = "";

            // Get scanByPDA
            query = "SELECT scanByPDA, trangthai FROM DonHang WHERE pk_seq = '" + donhang_fk + "' ";
            DataTable dt = xl.ReadTable(query);
            if (dt.Rows.Count > 0)
            {
                scanByPDA = dt.Rows[0]["scanByPDA"].ToString();
                trangthai = dt.Rows[0]["trangthai"].ToString();
            }

            if (donhang_fk.Length > 3)
                condition += " AND dh.donhang_fk = " + donhang_fk + " ";
            //if (location_fk.Length > 3)
            //    condition += " AND dh.location_fk = " + location_fk + " ";
            if (sanpham_fk.Length > 3)
            {
                condition += " AND dh.sanpham_fk = " + sanpham_fk + " ";
            }
            if (plNo.Length > 0)
            {
                condition += " AND dh.plNo = N'" + plNo + "' ";
                //conditionK += " AND kh.plNo = N'" + plNo + "' ";
            }
            if (solo.Length > 0)
            {
                condition += " AND dh.solo = N'" + solo + "' ";
                //conditionK += " AND kh.solo = N'" + solo + "' ";
            }
            if (mausac_fk.Length > 3)
            {
                condition += " AND dh.mausac_fk = N'" + mausac_fk + "' ";
                conditionK += " AND kh.mausac_fk = N'" + mausac_fk + "' ";
            }

            if (scanByPDA.Equals("0") || scanByPDA.Equals("1") || trangthai.Equals("0"))
            {
                query = "SELECT * FROM (" + 
                " SELECT '1' AS stt, dh.sortBy, dh.mavach, dh.solo, dh.lotNo, dh.cartonNo, dh.plNo, dh.location_fk, dh.mausac_fk AS size_fk, dh.soluong, dh.soluong1, dh.soluong2, dh.soluong3, dh.soluongGOC, dh.scanByPDA, " +
                "  ISNULL((SELECT ten FROM Location WHERE pk_seq = dh.location_fk), '') location, " +
                "  ISNULL((SELECT ten FROM Size WHERE pk_seq = dh.mausac_fk), '') size " +
                " FROM DonHang_SanPham_ChiTiet dh " +
                " WHERE dh.pk_seq > 0 " + condition +                 
                " UNION ALL " +
                " SELECT '2' AS stt, kh.sortBy, kh.mavach, kh.solo, kh.lotNo, kh.cartonNo, kh.plNo, kh.location_fk, kh.mausac_fk AS size_fk, kh.avai AS soluong, kh.soluong1, kh.soluong2, kh.soluong3, 0 AS soluongGoc, 0 AS scanByPDA,  " +
                " ISNULL((SELECT ten FROM Location WHERE pk_seq = kh.location_fk), '') location,  " +
                " ISNULL((SELECT ten FROM Size WHERE pk_seq = kh.mausac_fk), '') size  " +
                " FROM Kho_SanPham_ChiTiet kh  " +
                " WHERE kh.sanpham_fk = " + sanpham_fk + " AND kh.avai > 0  " + conditionK +
                " ) A ORDER BY A.location, A.lotNo, A.sortBy ASC ";

            }
            else
            {
                query = "SELECT * FROM ( SELECT '1' AS stt, dh.sortBy, dh.mavach, dh.solo, dh.lotNo, dh.cartonNo, dh.plNo, dh.location_fk, dh.mausac_fk AS size_fk, dh.soluong, dh.soluong1, dh.soluong2, dh.soluong3, dh.soluongGOC, dh.scanByPDA, " +
                "  ISNULL((SELECT ten FROM Location WHERE pk_seq = dh.location_fk), '') location, " +
                "  ISNULL((SELECT ten FROM Size WHERE pk_seq = dh.mausac_fk), '') size " +
                " FROM DonHang_SanPham_ChiTiet dh " +
                " WHERE dh.pk_seq > 0 " + condition + 
                " ) A ORDER BY A.location, A.lotNo, A.sortBy ASC ";
            }
            dt = xl.ReadTable(query);
            string sp = "";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i % 2 == 0)
                    sp += " <tr style='background-color:WhiteSmoke;'> ";
                else
                    sp += " <tr style='background-color:#FFF;'> ";

                string style = "";
                string soluongGOC = dt.Rows[i]["soluongGOC"].ToString();
                
                string soluong = dt.Rows[i]["soluong"].ToString();

                if (soluongGOC.Equals("0"))
                    style = " color:blue; font-weight:bold;";
                else if (double.Parse(soluongGOC) < double.Parse(soluong))
                    style = " color:green; font-weight:bold;";
                else if (double.Parse(soluongGOC) > double.Parse(soluong))
                    style = " color:red; font-weight:bold;";
                else
                    style = "font-weight:bold;";


                sp += " <td style='text-align:center;'>" + (i + 1).ToString() + "</td> ";
                sp += " <td style='text-align:center;'>" + dt.Rows[i]["location"].ToString() + "</td> ";
                sp += " <td style='text-align:center;'>" + dt.Rows[i]["lotNo"].ToString() + "</td> ";                
                sp += " <td style='text-align:center;'>" + dt.Rows[i]["cartonNo"].ToString() + "</td> ";                
                sp += " <td style='text-align:center;'>" + dt.Rows[i]["size"].ToString() + "</td> ";
                sp += " <td style='text-align:center;'>" + dt.Rows[i]["mavach"].ToString() + "</td> ";

                sp += " <td style='text-align:center; display:none;'><input type='text' name='soloCT' style='text-align:center;' value='" + dt.Rows[i]["solo"].ToString() + "' readonly /></td> ";
                sp += " <td style='text-align:center; display:none;'><input type='text' name='plNoCT' style='text-align:center;' value='" + dt.Rows[i]["solo"].ToString() + "' readonly /></td> ";
                sp += " <td style='text-align:center; display:none;'><input type='text' name='scanByPDACT' style='text-align:center;' value='" + dt.Rows[i]["scanByPDA"].ToString() + "'/></td> ";                
                sp += " <td style='text-align:center; display:none;'><input type='text' name='mavachCT' style='text-align:center;' value='" + dt.Rows[i]["mavach"].ToString() + "'  readonly /></td> ";
                sp += " <td style='text-align:center; display:none;'><input type='text' name='cartonNoCT' style='text-align:center;' value='" + dt.Rows[i]["cartonNo"].ToString() + "' readonly /></td> ";
                sp += " <td style='text-align:center; display:none;'><input type='text' name='locationCT_fk' style='text-align:center;' value='" + dt.Rows[i]["location_fk"].ToString() + "' readonly /></td> ";
                sp += " <td style='text-align:center; display:none;'><input type='text' name='sizeCT_fk' style='text-align:center;' value='" + dt.Rows[i]["size_fk"].ToString() + "' readonly /></td> ";

                sp += " <td style='text-align:right;" + style + "'><input type='text' name='soluonggocCT' style='text-align:right;' class='form-control' value='" + FormatString.ForMatNumber(soluongGOC) + "' onkeyup='AutoCalculatorQuantity();' readonly/></td> ";
                sp += " <td style='text-align:right;" + style + "'><input type='text' name='soluongCT' style='text-align:right;' class='form-control' value='" + FormatString.ForMatNumber(soluong) + "' readonly/></td> ";

                if (trangthai.Equals("0"))
                {
                    sp += " <td style='text-align:center;'>" +
                     //scanBYPDA 0
                    " <a href='javascript:CancelPallet(" + i + "," + trangthai + ", " + dt.Rows[i]["scanByPDA"].ToString() + ")' title='Cancel' class='cancelPalletCSS" + i + "'><span class='fa fa-times' aria-hidden='true' style='color:red; font-size:smaller;'> Hủy</span></a> &nbsp;&nbsp;" +
                    " <a href='javascript:ApprovelItem(" + i + "," + trangthai + ", " + dt.Rows[i]["scanByPDA"].ToString() + ")' title='Choose item' class='approvelItemCSS" + i + "'><span class='fa fa-check' aria-hidden='true' style='font-size:smaller;'> Chọn pickup</span></a> &nbsp;&nbsp;" +
                    " <a href='javascript:ConfirmPickup(" + i + "," + trangthai + ", " + dt.Rows[i]["scanByPDA"].ToString() + ")' title='Checked' class='confirmItemCSS" + i + "'><span class='fa fa-gavel' aria-hidden='true' style='font-size:smaller;'> Checked </span></a> &nbsp;&nbsp;" +

                    // ScanBYPDA 1 (hủy pickup, xác nhận checkout
                    " <a href='javascript:CancelPickUp(" + i + "," + trangthai + ", " + dt.Rows[i]["scanByPDA"].ToString() + ")' title='Cancel pickup' class='cancelPickupItemCSS" + i + "'><span class='fa fa-trash' aria-hidden='true' style='color:red; font-size:smaller;'> Hủy pickup</span></a> &nbsp;&nbsp;" +
                    " <a href='javascript:ConfirmCheckOut(" + i + "," + trangthai + ", " + dt.Rows[i]["scanByPDA"].ToString() + ")' title='Confirm Checkout' class='ConfirmCheckOutItemCSS" + i + "'><span class='fa fa-gavel' aria-hidden='true' style='font-size:smaller;'> Check out</span></a> &nbsp;&nbsp;" +
                    // scanBYPDA 2 (chủy checkout)      
                    //" <a href='javascript:CheckOut(" + i + "," + trangthai + ", " + dt.Rows[i]["scanByPDA"].ToString() + ")' title='Check out' class='checkOutItemCSS" + i + "'><span class='fa fa-ban' aria-hidden='true' style='font-size:smaller;'> Check out</span></a> &nbsp;&nbsp; " +
                    " <a href='javascript:UnCheckOut(" + i + "," + trangthai + ", " + dt.Rows[i]["scanByPDA"].ToString() + ")' title=' cancel Check out' class='unCheckOutItemCSS" + i + "'><span class='fa fa-unlock' aria-hidden='true' style='color:red; font-size:smaller;'> Hủy Checkout</span></a> " +
                    " </td> ";
                }
                else
                {
                    sp += " <td style='text-align:center;'></td>";
                }
                sp += " </tr> ";
            }

            msg += sp;
            msg += " </table> ";
            
            dt.Clone();
            context.Response.Write(msg);
        }
        private void viewDetailItem_StockIn(HttpContext context)
        {
            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string nhaphang_fk = "";
            if (context.Request.QueryString["nhaphang_fk"] != null)
                nhaphang_fk = context.Request.QueryString["nhaphang_fk"].ToString();

            string sanpham_fk = "";
            if (context.Request.QueryString["sanpham_fk"] != null)
                sanpham_fk = context.Request.QueryString["sanpham_fk"].ToString();

            string solo = "";
            if (context.Request.QueryString["solo"] != null)
                solo = context.Request.QueryString["solo"].ToString();

            string plNo = "";
            if (context.Request.QueryString["plNo"] != null)
                plNo = context.Request.QueryString["plNo"].ToString();

            //string utcOrderNo = "";
            //if (context.Request.QueryString["utcOrderNo"] != null)
            //    utcOrderNo = context.Request.QueryString["utcOrderNo"].ToString();

            string lotNo = "";
            if (context.Request.QueryString["lotNo"] != null)
                lotNo = context.Request.QueryString["lotNo"].ToString();

            string size_fk = "";
            if (context.Request.QueryString["size_fk"] != null)
                size_fk = context.Request.QueryString["size_fk"].ToString();

            string location_fk = "";
            if (context.Request.QueryString["location_fk"] != null)
                location_fk = context.Request.QueryString["location_fk"].ToString();

            string content = "<table class='table table-hover table-bordered table-striped' style='font-size:small;'>" +
            " <tr> " +
            " 	<th style='text-align:center; width: 3%;'>No</th> " +            
            " 	<th style='text-align:center; width: 10%;'>Carton No.</th> " +
            " 	<th style='text-align:center; width: 6%;'>No of</th> " +
            " 	<th style='text-align:center; width: 6%;'>Roll</th> " +
            " 	<th style='text-align:center; width: 10%;'>Net Weight</th> " +
            " 	<th style='text-align:center; width: 10%;'>Gross Weight</th> " +
            " 	<th style='text-align:center; width: 18%;'>Barcode</th> " +
            " 	<th style='text-align:center; width: 15%;'>Size</th> " +
            " 	<th style='text-align:center; width: 6%;'>Total</th> " +            
            " 	<th style='text-align:center; width: 8%;'></th> " +
            " </tr> ";

            string sql = "";
            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                if (nhaphang_fk.Length > 3)
                {
                    ExecuteData xl = new ExecuteData();
                    // AND b.PLNo = N'" + plNo + "'

                    sql = " SELECT b.mavach, b.cartonNo, b.lotNo, b.solo, b.location_fk, b.Noof, b.yards, b.roll, b.mavach, ISNULL((b.flag), 0) flag, " +
                    "	ISNULL((SELECT ten FROM Pallet WHERE pk_seq = b.pallet_fk), '') pallet, b.NoOf, b.roll, b.netWeight, b.grossWeight, " +
                    "	ISNULL((SELECT ten FROM Size WHERE pk_seq = b.mausac_fk), '') size, mausac_fk AS size_fk, " +
                    "   b.pallet_fk, b.soluong1, b.soluong2, b.soluong3, b.soluongQuyDoi AS soluong " +
                    //"   ISNULL((SELECT COUNT(*) FROM NhapHang_SanPham_ChiTiet nh WHERE nh.nhaphang_fk = b.nhaphang_fk AND nh.sanpham_fk = b.sanpham_fk AND nh.mausac_fk = b.mausac_fk AND nh.location_fk = b.location_fk AND nh.solo = b.solo AND nh.lotNo = b.lotNo AND nh.cartonNo = b.cartonNo GROUP BY nh.cartonNo), 0) AS checkDuplicate " + 
                    " FROM NhapHang a INNER JOIN NhapHang_SanPham_ChiTiet b ON a.pk_seq = b.nhaphang_fk AND b.nhaphang_fk = '" + nhaphang_fk + "' AND b.sanpham_fk = '" + sanpham_fk + "' " +
                    " WHERE b.solo = N'" + solo + "' AND b.plNo = N'" + plNo + "' AND b.location_fk = '" + location_fk + "' AND b.lotNo = N'" + lotNo + "' AND b.mausac_fk = '" + size_fk + "' " +
                    " ORDER BY b.stt ";

                    DataTable dt = xl.ReadTable(sql);

                    if (dt.Rows.Count <= 0)
                    {
                        location_fk = "0";

                        sql = " SELECT b.mavach, b.cartonNo, b.lotNo, b.solo, b.location_fk, b.Noof, b.yards, b.roll, b.mavach, ISNULL((b.flag), 0) flag, " +
                           "	ISNULL((SELECT ten FROM Pallet WHERE pk_seq = b.pallet_fk), '') pallet, b.NoOf, b.roll, b.netWeight, b.grossWeight, " +
                           "	ISNULL((SELECT ten FROM Size WHERE pk_seq = b.mausac_fk), '') size, mausac_fk AS size_fk, " +
                           "   b.pallet_fk, b.soluong1, b.soluong2, b.soluong3, b.soluongQuyDoi AS soluong " +
                           //"   ISNULL((SELECT COUNT(*) FROM NhapHang_SanPham_ChiTiet nh WHERE nh.nhaphang_fk = b.nhaphang_fk AND nh.sanpham_fk = b.sanpham_fk AND nh.mausac_fk = b.mausac_fk AND nh.location_fk = b.location_fk AND nh.solo = b.solo AND nh.lotNo = b.lotNo AND nh.cartonNo = b.cartonNo GROUP BY nh.cartonNo), 0) AS checkDuplicate " + 
                           " FROM NhapHang a INNER JOIN NhapHang_SanPham_ChiTiet b ON a.pk_seq = b.nhaphang_fk AND b.nhaphang_fk = '" + nhaphang_fk + "' AND b.sanpham_fk = '" + sanpham_fk + "' " +
                           " WHERE b.solo = N'" + solo + "' AND b.plNo = N'" + plNo + "' AND b.location_fk = '" + location_fk + "' AND b.lotNo = N'" + lotNo + "' AND b.mausac_fk = '" + size_fk + "' " +
                           " ORDER BY b.stt ";
                        dt = xl.ReadTable(sql);
                    }    

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i % 2 == 0)
                            content += " <tr style='background-color:WhiteSmoke;' > ";
                        else
                            content += " <tr style='background-color:#FFF;' > ";

                        int flag = int.Parse(dt.Rows[i]["flag"].ToString());
                        //int checkDuplicate = int.Parse(dt.Rows[i]["checkDuplicate"].ToString());

                        string style = "";
                        //string styleDuplicate = "";

                        if (flag > 0)
                            style = " color: red; font-weight: bold; ";
                        else
                            style = "";

                        //if(checkDuplicate > 1)
                        //    styleDuplicate = " color: red; font-weight: bold; ";
                        //else
                        //    styleDuplicate = "";

                        content += " <td style='display:none;'><input type='text' class='form-control' name='mavach1' value='" + dt.Rows[i]["mavach"].ToString() + "'></td> ";
                        content += " <td style='display:none;'><input type='text' class='form-control' name='solo1' value='" + dt.Rows[i]["solo"].ToString() + "'></td> ";
                        content += " <td style='display:none;'><input type='text' class='form-control' name='lotNo1' value='" + dt.Rows[i]["lotNo"].ToString() + "'></td> ";
                        content += " <td style='display:none;'><input type='text' class='form-control' name='location1_fk' value='" + dt.Rows[i]["location_fk"].ToString() + "'></td> ";
                        content += " <td style='display:none;'><input type='text' class='form-control' name='pallet1_fk' value='" + dt.Rows[i]["pallet_fk"].ToString() + "'></td> ";
                        content += " <td style='display:none;'><input type='text' class='form-control' name='size1_fk' value='" + dt.Rows[i]["size_fk"].ToString() + "'></td> ";
                        content += " <td style='width:3%; text-align:center;'>" + (i + 1).ToString() + "</td> ";                        
                        content += " <td style='width:9.9%; text-align:center; " + style + "' >" + dt.Rows[i]["cartonNo"].ToString() + "</td> ";
                        content += " <td style='width:9.9%; text-align:center; " + style + "' >" + dt.Rows[i]["NoOf"].ToString() + "</td> ";
                        content += " <td style='width:9.9%; text-align:center; " + style + "' >" + dt.Rows[i]["roll"].ToString() + "</td> ";
                        content += " <td style='width:9.9%; text-align:center; " + style + "' >" + dt.Rows[i]["netWeight"].ToString() + "</td> ";
                        content += " <td style='width:9.9%; text-align:center; " + style + "' >" + dt.Rows[i]["grossWeight"].ToString() + "</td> ";
                        content += " <td style='width:5.9%; text-align:center; " + style + "' >" + dt.Rows[i]["mavach"].ToString() + "</td> ";
                        content += " <td style='width:9.9%; text-align:center; " + style + "' >" + dt.Rows[i]["size"].ToString() + "</td> ";

                        content += " <td style='width:11.9%; text-align:right; " + style + "' >" + FormatString.ForMatNumber(dt.Rows[i]["soluong"].ToString()) + "</td> ";

                        if (dt.Rows[i]["location_fk"].ToString().Length > 3)
                            content += " <td style='width:6.9%; text-align:center;' ><a href='javascript:CancelPallet(" + i + ")' title='Cannel Pallet' ><span class='fa fa-times' aria-hidden='true' style='color:red'></span></a></td> ";
                        else
                            content += " <td style='width:6.9%; text-align:center;' ><a href='javascript:DeleteItem(" + i + ")' title='Delete item' ><span class='fa fa-trash-o' aria-hidden='true' style='color:red'></span></a></td> ";

                        content += " </tr> ";
                    }
                }
                else
                {
                    content += "<td style='text-align:center;' colspan='7'>No data</td>";
                }
            }

            content += "</table>";

            context.Response.Write(content);
        }

        private void CheckLocation(HttpContext context)
        {
            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string location = "";
            if (context.Request.QueryString["location"] != null)
                location = context.Request.QueryString["location"].ToString();

            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            string khachhang = "0";
            if (context.Request.QueryString["khachhang"] != null)
                khachhang = context.Request.QueryString["khachhang"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                string msg = "NA";

                ExecuteData xl = new ExecuteData();
                
                string sql = "";

                string condition = "";
                //if (khachhang.Trim().Length > 3)
                    condition += " AND a.khachhang_fk = '" + khachhang + "' ";
                if (kho.Trim().Length > 0)
                    condition += " AND a.kho_fk = N'" + kho + "'  ";

                sql = "SELECT TOP(1) a.pk_seq, a.ma, a.ten " +
                     " FROM Location a " +
                     " WHERE a.trangthai = '1' AND a.dasudung = 0 AND a.ma = N'" + location + "'  " + condition;
                sql += " ORDER BY a.khoangcach ASC ";

                DataTable dt = xl.ReadTable(sql);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    msg = dt.Rows[i]["pk_seq"].ToString() + " -- " + dt.Rows[i]["ma"].ToString() + " -- " + dt.Rows[i]["ten"].ToString();
                }
                dt.Clone();

                context.Response.Write(msg);
            }
        }
        private void CheckColor(HttpContext context)
        {
            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();

            string color = "";
            if (context.Request.QueryString["color"] != null)
                color = context.Request.QueryString["color"].ToString();

            string kho = "";
            if (context.Request.QueryString["kho"] != null)
                kho = context.Request.QueryString["kho"].ToString();

            if (userId.Trim().Length <= 0)
            {
                context.Response.Write("If you want to be continue. Please, log in again!");
            }
            else
            {
                string msg = "NA";

                ExecuteData xl = new ExecuteData();

                string sql = "";

                sql = "SELECT a.pk_seq, a.ma, a.ten " +
                     " FROM MauSac a " +
                     " WHERE a.trangthai = '1' AND a.ma = N'" + color + "'  ";
                sql += " ORDER BY a.ma ASC ";

                DataTable dt = xl.ReadTable(sql);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    msg = dt.Rows[i]["pk_seq"].ToString() + " -- " + dt.Rows[i]["ma"].ToString() + " -- " + dt.Rows[i]["ten"].ToString();
                }
                dt.Clone();

                context.Response.Write(msg);
            }
        }

        private void LoadInforItemStockIn(HttpContext context)
        {
            string userId = "";
            if (context.Session["userId"] != null)
                userId = context.Session["userId"].ToString();
            
            string cartonNo = "";
            if (context.Request.QueryString["cartonNo"] != null)
                cartonNo = context.Request.QueryString["cartonNo"].ToString();

            string lotNo = "";
            if (context.Request.QueryString["lotNo"] != null)
                lotNo = context.Request.QueryString["lotNo"].ToString();

            string msg = "";
            string condition = "";
            if (lotNo.Trim().Length > 0)
                condition += " AND nh_ct.lotNo = N'" + lotNo + "' ";
            if (cartonNo.Trim().Length > 0)
                condition += " AND nh_ct.cartonNo = N'" + cartonNo + "' ";

            ExecuteData xl = new ExecuteData();

            string sql = " SELECT TOP(1) nh_ct.nhaphang_fk, nh.ngaynhap, nh_ct.mavach, nh_ct.sanpham_fk AS itemId, sp.ma AS itemCode, sp.codeEnglish AS itemJapan, nh_ct.plNo, nh_ct.lotNo, nh_ct.yards, nh_ct.soluongQuyDoi AS soluong, " +
            " ISNULL((SELECT ten FROM MauSac WHERE pk_seq = nh_ct.mausac_fk), '') color " +
            " FROM NhapHang nh INNER JOIN NhapHang_SanPham_ChiTiet nh_ct ON nh.pk_seq = nh_ct.nhaphang_fk " + condition +
            " 	INNER JOIN SanPham sp ON nh_ct.sanpham_fk = sp.pk_seq " +
            " WHERE nh.trangthai = 0 AND pallet_fk = 0 ";

            DataTable dt = xl.ReadTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                msg = dt.Rows[0]["nhaphang_fk"].ToString() + " -- " + dt.Rows[0]["itemCode"].ToString() + " (" + dt.Rows[0]["itemJapan"].ToString() + ")" + " -- " + dt.Rows[0]["mavach"].ToString() + " -- " +
                    dt.Rows[0]["itemCode"].ToString() + " -- " + dt.Rows[0]["itemJapan"].ToString() + " -- " + dt.Rows[0]["plNo"].ToString() + " -- " +
                    dt.Rows[0]["lotNo"].ToString() + " -- " + dt.Rows[0]["color"].ToString() + " -- " + dt.Rows[0]["soluong"].ToString() + " -- " +
                    dt.Rows[0]["yards"].ToString() + " -- " + dt.Rows[0]["ngaynhap"].ToString();
            }
            dt.Clone();

            context.Response.Write(msg);
        }

        private void PrinterLabel(HttpContext context)
        {
            string nhaphang_fk = "";
            if (context.Request.QueryString["nhaphang_fk"] != null)
                nhaphang_fk = context.Request.QueryString["nhaphang_fk"].ToString();

            string location_fk = "";
            if (context.Request.QueryString["location_fk"] != null)
                location_fk = context.Request.QueryString["location_fk"].ToString();

            string printerName = "";
            if (context.Request.QueryString["printerName"] != null)
                printerName = context.Request.QueryString["printerName"].ToString();

            string barcode = "";
            if (context.Request.QueryString["barcode"] != null)
                barcode = context.Request.QueryString["barcode"].ToString();

            string itemName = "";
            if (context.Request.QueryString["itemName"] != null)
                itemName = context.Request.QueryString["itemName"].ToString();

            string solo = "";
            if (context.Request.QueryString["solo"] != null)
                solo = context.Request.QueryString["solo"].ToString();

            string plNo = "";
            if (context.Request.QueryString["plNo"] != null)
                plNo = context.Request.QueryString["plNo"].ToString();

            string lotNo = "";
            if (context.Request.QueryString["lotNo"] != null)
                lotNo = context.Request.QueryString["lotNo"].ToString();

            string cartonNo = "";
            if (context.Request.QueryString["cartonNo"] != null)
                cartonNo = context.Request.QueryString["cartonNo"].ToString();

            string color = "";
            if (context.Request.QueryString["color"] != null)
                color = context.Request.QueryString["color"].ToString();

            string qty = "";
            if (context.Request.QueryString["qty"] != null)
                qty = context.Request.QueryString["qty"].ToString();

            string yard = "";
            if (context.Request.QueryString["yard"] != null)
                yard = context.Request.QueryString["yard"].ToString();

            string rDate = "";
            if (context.Request.QueryString["rDate"] != null)
                rDate = context.Request.QueryString["rDate"].ToString();

            string userID = "100000";

            string msg = "";

            //PrinterControl printerControl = new PrinterControl();
            //printerControl.runPrinter(nhaphang_fk, location_fk, printerName, barcode, itemName, solo, plNo, lotNo, cartonNo, color, qty, yard, rDate, userID);
            context.Response.Write(msg);
        }

        private string getLocation(DataTable tbDVT, string dvtId, int pos)
        {
            string size = "<SELECT style='width:100%; padding: 4px 0px; ' name = 'location_fk'>";

            size += " <option value=''  > </option> ";
            for (int i = 0; i < tbDVT.Rows.Count; i++)
            {
                if (tbDVT.Rows[i]["pk_seq"].ToString().Equals(dvtId))
                    size += " <option value='" + tbDVT.Rows[i]["pk_seq"].ToString() + "' selected  readonly>" + tbDVT.Rows[i]["ten"].ToString() + "</option> ";
                else
                    size += " <option value='" + tbDVT.Rows[i]["pk_seq"].ToString() + "' readonly >" + tbDVT.Rows[i]["ten"].ToString() + "</option> ";
            }

            size += " </select> ";
            return size;
        }
        private string getPallet(DataTable tbDVT, string dvtId, int pos)
        {
            string size = "<SELECT style='width:100%; padding: 4px 0px; ' name = 'pallet_fk' >";

            size += " <option value=''  > </option> ";
            for (int i = 0; i < tbDVT.Rows.Count; i++)
            {
                if (tbDVT.Rows[i]["pk_seq"].ToString().Equals(dvtId))
                    size += " <option value='" + tbDVT.Rows[i]["pk_seq"].ToString() + "' selected  readonly>" + tbDVT.Rows[i]["ten"].ToString() + "</option> ";
                else
                    size += " <option value='" + tbDVT.Rows[i]["pk_seq"].ToString() + "' readonly >" + tbDVT.Rows[i]["ten"].ToString() + "</option> ";
            }

            size += " </select> ";
            return size;
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