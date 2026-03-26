using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using HLVTimeSheet.AcsessData;

namespace HLVTimeSheet.Model
{
    public class PhanQuyen
    {
        public string Update_UngDung(string nhanvien_fk, string xemIds, string xoaIds, string taomoiIds, string chinhsuaIds, string nguoisua)
        {
            int kq = -1;

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            try
            {

                string sql = "DELETE NhanVien_PhanQuyen_UngDung WHERE nhanvien_fk = '" + nhanvien_fk + "' ";
                kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                sql = "Insert NhanVien_PhanQuyen_UngDung ( nhanvien_fk, ungdung_fk, nguoisua ) SELECT '" + nhanvien_fk + "', maungdung, '" + nguoisua + "' FROM UngDung ";
                kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                if (kq <= 0)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Error! Cannot updated data... NhanVien_PhanQuyen_UngDung " + sql;
                }

                if (xemIds != null)
                {
                    sql = "UPDATE NhanVien_PhanQuyen_UngDung SET xem = 1 WHERE nhanvien_fk = '" + nhanvien_fk + "' and ungdung_fk in (" + xemIds + ")  ";

                    kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                    if (kq <= 0)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Error! Cannot updated data... NhanVien_PhanQuyen_UngDung " + sql;
                    }
                }

                if (xoaIds != null)
                {
                    sql = "UPDATE NhanVien_PhanQuyen_UngDung SET xoa = 1 WHERE nhanvien_fk = '" + nhanvien_fk + "' and ungdung_fk in (" + xoaIds + ")";

                    kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                    if (kq <= 0)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Error! Cannot updated data... NhanVien_PhanQuyen_UngDung " + sql;
                    }
                }

                if (taomoiIds != null)
                {
                    sql = "UPDATE NhanVien_PhanQuyen_UngDung SET taomoi = 1 WHERE nhanvien_fk = '" + nhanvien_fk + "' and ungdung_fk in (" + taomoiIds + ")";

                    kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                    if (kq <= 0)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Error! Cannot updated data... NhanVien_PhanQuyen_UngDung " + sql;
                    }
                }

                if (chinhsuaIds != null)
                {
                    sql = "UPDATE NhanVien_PhanQuyen_UngDung SET sua = 1 WHERE nhanvien_fk = '" + nhanvien_fk + "' and ungdung_fk in (" + chinhsuaIds + ")";

                    kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                    if (kq <= 0)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Error! Cannot updated data... NhanVien_PhanQuyen_UngDung " + sql;
                    }
                }


                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Lỗi khi cập nhật quyền: " + ex.Message;
            }

            connect.Close();
            return "";
        }


        public string NhomQuyen_Update_UngDung(string nhomquyen_fk, string ma, string diengiai, string trangthai, string groupXemIds, string groupXoaIds, string groupTaomoiIds, string groupCapnhatIds, string groupChotIds, string groupHuyChotIds,
                string xemIds, string xoaIds, string taomoiIds, string chinhsuaIds, string chotIds, string huychotIds, string userId)
        {
            int kq = -1;

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            try
            {
                string sql = "";

                // Kiểm tra  mã 
                sql = " SELECT COUNT(*) FROM NhomQuyen WHERE maquyen = N'" + ma + "' ";
                if (nhomquyen_fk.Length > 3)
                    sql += " AND pk_seq != '" + nhomquyen_fk + "' ";

                object obj = new SqlCommand(sql, connect, transaction).ExecuteScalar();
                if (obj != null)
                {
                    if (int.Parse(obj.ToString()) > 0)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Code is duplicate! " + ma + " Please input again ";
                    }
                }

                if (nhomquyen_fk.Trim().Length > 0)
                {
                    sql = "UPDATE NHOMQUYEN SET maquyen = N'" + ma + "', tenquyen = N'" + diengiai + "', trangthai = '" + trangthai + "', nguoisua = '" + userId + "'  WHERE pk_seq = '" + nhomquyen_fk + "' ";
                    kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();
                    if (kq <= 0)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Error! Cannot updated data... NHOMQUYEN " + sql;
                    }
                }
                else
                {
                    sql = "INSERT NHOMQUYEN ( MAQUYEN, TENQUYEN, TRANGTHAI, NGUOITAO, NGUOISUA ) values( N'" + ma + "', N'" + diengiai + "', '" + trangthai + "', '" + userId + "', '" + userId + "' ) ";
                    kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();
                    if (kq <= 0)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Error! Cannot updated data... NHOMQUYEN " + sql;
                    }

                    nhomquyen_fk = new SqlCommand("SELECT IDENT_CURRENT('NHOMQUYEN')", connect, transaction).ExecuteScalar().ToString();
                }

                sql = "DELETE NhomQuyen_GroupChucNang WHERE nhomquyen_fk = '" + nhomquyen_fk + "' ";
                kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                sql = "DELETE NhomQuyen_ChucNang WHERE nhomquyen_fk = '" + nhomquyen_fk + "' ";
                kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                sql = "DELETE NhomQuyen_ChucNang_ChiTiet WHERE nhomquyen_fk = '" + nhomquyen_fk + "' ";
                kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();


                //GROUP CHUC NANG
                sql = "insert NhomQuyen_GroupChucNang(nhomquyen_fk, group_fk, chkALL_XEM, chkALL_XOA, chkALL_TAOMOI, chkALL_CAPNHAT, chkALL_CHOT, chkALL_HUYCHOT) " +
                      "  SELECT '" + nhomquyen_fk + "', pk_seq, 0, 0, 0, 0, 0, 0 FROM Group_ChucNang  ";
                kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Error! Cannot updated data... NhomQuyen_GroupChucNang " + sql;
                }

                //GROUP CHUC NANG CHECK ALL
                if (groupXemIds.Trim().Length > 0)
                {
                    sql = "UPDATE NhomQuyen_GroupChucNang SET chkALL_XEM = 1 WHERE nhomquyen_fk = '" + nhomquyen_fk + "' and group_fk in (" + groupXemIds.Trim().Substring(0, groupXemIds.Trim().Length - 1) + ")  ";
                    kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Error! Cannot updated data... NhomQuyen_GroupChucNang " + sql;
                    }
                }

                if (groupXoaIds.Trim().Length > 0)
                {
                    sql = "UPDATE NhomQuyen_GroupChucNang SET chkALL_XOA = 1 WHERE nhomquyen_fk = '" + nhomquyen_fk + "' and group_fk in (" + groupXoaIds.Trim().Substring(0, groupXoaIds.Trim().Length - 1) + ")  ";
                    kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Error! Cannot updated data... NhomQuyen_GroupChucNang " + sql;
                    }
                }

                if (groupTaomoiIds.Trim().Length > 0)
                {
                    sql = "UPDATE NhomQuyen_GroupChucNang SET chkALL_TAOMOI = 1 WHERE nhomquyen_fk = '" + nhomquyen_fk + "' and group_fk in (" + groupTaomoiIds.Trim().Substring(0, groupTaomoiIds.Trim().Length - 1) + ")  ";
                    kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Error! Cannot updated data... NhomQuyen_GroupChucNang " + sql;
                    }
                }

                if (groupCapnhatIds.Trim().Length > 0)
                {
                    sql = "UPDATE NhomQuyen_GroupChucNang SET chkALL_CAPNHAT = 1 WHERE nhomquyen_fk = '" + nhomquyen_fk + "' and group_fk in (" + groupCapnhatIds.Trim().Substring(0, groupCapnhatIds.Trim().Length - 1) + ")  ";
                    kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Error! Cannot updated data... NhomQuyen_GroupChucNang " + sql;
                    }
                }

                if (groupChotIds.Trim().Length > 0)
                {
                    sql = "UPDATE NhomQuyen_GroupChucNang SET chkALL_CHOT = 1 WHERE nhomquyen_fk = '" + nhomquyen_fk + "' and group_fk in (" + groupChotIds.Trim().Substring(0, groupChotIds.Trim().Length - 1) + ")  ";
                    kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Error! Cannot updated data... NhomQuyen_GroupChucNang " + sql;
                    }
                }

                if (groupHuyChotIds.Trim().Length > 0)
                {
                    sql = "UPDATE NhomQuyen_GroupChucNang SET chkALL_HUYCHOT = 1 WHERE nhomquyen_fk = '" + nhomquyen_fk + "' and group_fk in (" + groupHuyChotIds.Trim().Substring(0, groupHuyChotIds.Trim().Length - 1) + ")  ";
                    kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Error! Cannot updated data... NhomQuyen_GroupChucNang " + sql;
                    }
                }

                //UNG DUNG CHI TIET
                sql = "insert NhomQuyen_ChucNang_ChiTiet(nhomquyen_fk, chucnang_fk, xem, xoa, taomoi, sua, chot, huychot) " +
                      "  SELECT '" + nhomquyen_fk + "', pk_seq, 0, 0, 0, 0, 0, 0 FROM ChucNang  ";
                kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();
                if (kq < 1)
                {
                    transaction.Rollback();
                    connect.Close();
                    return "Error! Cannot updated data... NhomQuyen_ChucNang_ChiTiet " + sql;
                }

                if (xemIds.Trim().Length > 0)
                {
                    sql = "UPDATE NhomQuyen_ChucNang_ChiTiet SET xem = 1 WHERE nhomquyen_fk = '" + nhomquyen_fk + "' and chucnang_fk in (" + xemIds.Trim().Substring(0, xemIds.Trim().Length - 1) + ")  ";
                    kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Error! Cannot updated data... NhomQuyen_ChucNang_ChiTiet " + sql;
                    }
                }

                if (xoaIds.Trim().Length > 0)
                {
                    sql = "UPDATE NhomQuyen_ChucNang_ChiTiet SET xoa = 1 WHERE nhomquyen_fk = '" + nhomquyen_fk + "' and chucnang_fk in (" + xoaIds.Trim().Substring(0, xoaIds.Trim().Length - 1) + ")  ";
                    kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Error! Cannot updated data... NhomQuyen_ChucNang_ChiTiet " + sql;
                    }
                }

                if (taomoiIds.Trim().Length > 0)
                {
                    sql = "UPDATE NhomQuyen_ChucNang_ChiTiet SET taomoi = 1 WHERE nhomquyen_fk = '" + nhomquyen_fk + "' and chucnang_fk in (" + taomoiIds.Trim().Substring(0, taomoiIds.Trim().Length - 1) + ")  ";
                    kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Error! Cannot updated data... NhomQuyen_ChucNang_ChiTiet " + sql;
                    }
                }

                if (chinhsuaIds.Trim().Length > 0)
                {
                    sql = "UPDATE NhomQuyen_ChucNang_ChiTiet SET sua = 1 WHERE nhomquyen_fk = '" + nhomquyen_fk + "' and chucnang_fk in (" + chinhsuaIds.Trim().Substring(0, chinhsuaIds.Trim().Length - 1) + ")  ";
                    kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Error! Cannot updated data... NhomQuyen_ChucNang_ChiTiet " + sql;
                    }
                }

                if (chotIds.Trim().Length > 0)
                {
                    sql = "UPDATE NhomQuyen_ChucNang_ChiTiet SET chot = 1 WHERE nhomquyen_fk = '" + nhomquyen_fk + "' and chucnang_fk in (" + chotIds.Trim().Substring(0, chotIds.Trim().Length - 1) + ")  ";
                    kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Error! Cannot updated data... NhomQuyen_ChucNang_ChiTiet " + sql;
                    }
                }

                if (huychotIds.Trim().Length > 0)
                {
                    sql = "UPDATE NhomQuyen_ChucNang_ChiTiet SET huychot = 1 WHERE nhomquyen_fk = '" + nhomquyen_fk + "' and chucnang_fk in (" + huychotIds.Trim().Substring(0, huychotIds.Trim().Length - 1) + ")  ";
                    kq = new SqlCommand(sql, connect, transaction).ExecuteNonQuery();

                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connect.Close();
                        return "Error! Cannot updated data... NhomQuyen_ChucNang_ChiTiet " + sql;
                    }
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                connect.Close();
                return "Lỗi khi cập nhật quyền: " + ex.Message;
            }

            connect.Close();

            return "OK";

        }
    }
}