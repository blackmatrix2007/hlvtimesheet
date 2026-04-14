using HLVTimeSheet.AcsessData;
using HLVTimeSheet.Model.DeviceManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace HLVTimeSheet.Model
{
    public class TimeKeepingController
    {       
        public string INSERT_TimeKeeping_New(string ngaynhap, string phongban_fk, string nhansu_fk, string gioIn, string phutIn, string gioOut, string phutOut, string loai, string hinhanhIn, string hinhanhOut, string idMayCheckIn, string idMayCheckOut, string trangthai, string nguoitao)
        {
            Debug.WriteLine($"[TimeKeeping] INSERT_NEW: nhansu_fk={nhansu_fk}, phongban_fk={phongban_fk}, ngay={ngaynhap}, in={gioIn}:{phutIn}, out={gioOut}:{phutOut}");

            string chamcong_fk = "0";
            string thang = "";
            string nam = "";
            string thoigian = "";
            int kq = 0;
            bool flag = false;

            // Parse thang/nam từ ngaynhap (dd-MM-yyyy) để INSERT ChamCong
            if (DateTime.TryParseExact(ngaynhap, "dd-MM-yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime ngayParsed))
            {
                thang = ngayParsed.Month.ToString();
                nam   = ngayParsed.Year.ToString();
            }
            Debug.WriteLine($"[TimeKeeping] thang={thang}, nam={nam}");

            string sql = "SELECT pk_seq FROM ChamCong WHERE nhansu_fk = N'" + nhansu_fk + "' AND ngaynhap = '" + ngaynhap + "' ";
            ConnectionDatabase conn = new ConnectionDatabase();
            SqlTransaction transaction;            
            using (var connection = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", "")))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                command.Transaction = transaction; // bắt buộc khi connection có active transaction
                // thực hiện truy vấn
                object obj = command.ExecuteScalar();
                Debug.WriteLine($"[TimeKeeping] ChamCong lookup: nhansu_fk={nhansu_fk}, ngay={ngaynhap} → existing_pk={obj}");
                if (obj != null && obj.ToString().Length > 3)
                {
                    flag = true;
                    chamcong_fk = obj.ToString();
                    Debug.WriteLine($"[TimeKeeping] UPDATE existing ChamCong pk={chamcong_fk}");
                    sql = "UPDATE ChamCong SET ngaysua = GETDATE(), nguoisua = '" + nguoitao + "' WHERE pk_seq = '" + chamcong_fk + "' ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connection.Close();
                        return "2.Error! Cannot Update.";
                    }
                }
                else
                {
                    Debug.WriteLine($"[TimeKeeping] INSERT new ChamCong: ngay={ngaynhap}, phongban={phongban_fk}, nhansu={nhansu_fk}");
                    sql = "INSERT ChamCong(ngaynhap, phongban_fk, nhansu_fk, thoigianIn, thoigianOut, thang, nam, trangthai, nguoitao, nguoisua) " +
                    " SELECT N'" + ngaynhap + "', N'" + phongban_fk + "', N'" + nhansu_fk + "', N'', N'', '" + thang + "', N'" + nam + "', '" + trangthai + "', '" + nguoitao + "', '" + nguoitao + "' ";
                    Debug.WriteLine($"[TimeKeeping] SQL ChamCong: {sql}");
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connection.Close();
                        Debug.WriteLine($"[TimeKeeping] FAILED INSERT ChamCong: nhansu={nhansu_fk}, phongban={phongban_fk}");
                        DeviceManagerLogger.Log("IMPORT", $"FAIL INSERT ChamCong: nhansu={nhansu_fk}, phongban={phongban_fk}, ngay={ngaynhap}");
                        return "2.Error! FAIL INSERT ChamCong nhansu=" + nhansu_fk;
                    }

                    if (chamcong_fk.Length < 3)
                    {
                        sql = "SELECT IDENT_CURRENT('ChamCong')";
                        command.CommandTimeout = int.MaxValue;
                        command.CommandText = sql;
                        chamcong_fk = command.ExecuteScalar().ToString();
                        Debug.WriteLine($"[TimeKeeping] New ChamCong pk={chamcong_fk}");
                    }
                }

                // ghi nhan cham cong
                // 1 - In
                // 2 - Out
                thoigian = gioIn + ":" + phutIn;
                Debug.WriteLine($"[TimeKeeping] thoigianIn='{thoigian}' (len={thoigian.Length}), will insert={thoigian.Length > 3}");
                if (thoigian.Length > 3)
                {
                    loai = "1";

                    if(flag)
                    {
                        sql = "INSERT ChamCong_ChiTiet_LichSu(chamcong_fk, ngaynhap, phongban_fk, nhansu_fk, thoigian, gio, phut, thang, nam, loai, gioStart, phutStart, gioEnd, phutEnd, hinhanh, idMayChamCong, trangthai, nguoitao, nguoisua)" +
                            " SELECT chamcong_fk, ngaynhap, phongban_fk, nhansu_fk, thoigian, gio, phut, thang, nam, loai, gioStart, phutStart, gioEnd, phutEnd, hinhanh, idMayChamCong, trangthai, '" + nguoitao + "', '" + nguoitao + "' " +
                            " FROM ChamCong_ChiTiet WHERE chamcong_fk = '" + chamcong_fk + "' AND loai = 1 ";                        
                        command.CommandTimeout = int.MaxValue;
                        command.CommandText = sql;
                        kq = command.ExecuteNonQuery();

                        sql = "DELETE ChamCong_ChiTiet WHERE chamcong_fk = '" + chamcong_fk + "' AND loai = 1 ";
                        command.CommandTimeout = int.MaxValue;
                        command.CommandText = sql;
                        kq = command.ExecuteNonQuery();

                    }    

                    Debug.WriteLine($"[TimeKeeping] INSERT ChiTiet loai=1 (In): chamcong={chamcong_fk}, phongban={phongban_fk}, gio={gioIn}:{phutIn}");
                    sql = "INSERT ChamCong_ChiTiet(chamcong_fk, ngaynhap, phongban_fk, nhansu_fk, thoigian, gio, phut, thang, nam, loai, gioStart, phutStart, gioEnd, phutEnd, hinhanh, idMayChamCong, trangthai, nguoitao, nguoisua) " +
                    " SELECT '" + chamcong_fk + "', N'" + ngaynhap + "', N'" + phongban_fk + "', N'" + nhansu_fk + "', N'" + thoigian + "', N'" + gioIn + "', '" + phutIn + "', '" + thang + "', N'" + nam + "', '" + loai + "', gioStart, phutStart, gioEnd, phutEnd, N'" + hinhanhIn + "', N'" + idMayCheckIn + "', '" + trangthai + "', '" + nguoitao + "', '" + nguoitao + "' " +
                    " FROM GioLamViec WHERE loai = 1 AND trangthai = 1 AND phongban_fk = '" + phongban_fk + "' ";
                    DeviceManagerLogger.Log("IMPORT", $"SQL ChiTiet loai=1: chamcong_fk={chamcong_fk}, phongban={phongban_fk}, nhansu={nhansu_fk}, ngay={ngaynhap}, gio={gioIn}:{phutIn}, idMayIn={idMayCheckIn}");
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                    Debug.WriteLine($"[TimeKeeping] INSERT ChiTiet loai=1 result: kq={kq} (0=FAIL=GioLamViec không có phongban_fk={phongban_fk})");
                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connection.Close();
                        DeviceManagerLogger.Log("IMPORT", $"FAIL INSERT ChiTiet loai=1: GioLamViec không có phongban_fk={phongban_fk}, nhansu={nhansu_fk}, ngay={ngaynhap}");
                        return "2.Error! FAIL ChiTiet loai=1 (GioLamViec phongban=" + phongban_fk + ")";
                    }
                }

                thoigian = "";
                thoigian = gioOut + ":" + phutOut;
                Debug.WriteLine($"[TimeKeeping] thoigianOut='{thoigian}' (len={thoigian.Length}), will insert={thoigian.Length > 3}");
                if (thoigian.Length > 3)
                {
                    loai = "2";

                    if (flag)
                    {
                        sql = "INSERT ChamCong_ChiTiet_LichSu(chamcong_fk, ngaynhap, phongban_fk, nhansu_fk, thoigian, gio, phut, thang, nam, loai, gioStart, phutStart, gioEnd, phutEnd, hinhanh, idMayChamCong, trangthai, nguoitao, nguoisua)" +
                           " SELECT chamcong_fk, ngaynhap, phongban_fk, nhansu_fk, thoigian, gio, phut, thang, nam, loai, gioStart, phutStart, gioEnd, phutEnd, hinhanh, idMayChamCong, trangthai, '" + nguoitao + "', '" + nguoitao + "' " +
                           " FROM ChamCong_ChiTiet WHERE chamcong_fk = '" + chamcong_fk + "' AND loai = 2 ";                        
                        command.CommandTimeout = int.MaxValue;
                        command.CommandText = sql;
                        kq = command.ExecuteNonQuery();

                        sql = "DELETE ChamCong_ChiTiet WHERE chamcong_fk = '" + chamcong_fk + "' AND loai = 2 ";
                        command.CommandTimeout = int.MaxValue;
                        command.CommandText = sql;
                        kq = command.ExecuteNonQuery();

                    }

                    Debug.WriteLine($"[TimeKeeping] INSERT ChiTiet loai=2 (Out): chamcong={chamcong_fk}, phongban={phongban_fk}, gio={gioOut}:{phutOut}");
                    sql = "INSERT ChamCong_ChiTiet(chamcong_fk, ngaynhap, phongban_fk, nhansu_fk, thoigian, gio, phut, thang, nam, loai, gioStart, phutStart, gioEnd, phutEnd, hinhanh, idMayChamCong, trangthai, nguoitao, nguoisua) " +
                    " SELECT '" + chamcong_fk + "', N'" + ngaynhap + "', N'" + phongban_fk + "', N'" + nhansu_fk + "', N'" + thoigian + "', N'" + gioOut + "', '" + phutOut + "', '" + thang + "', N'" + nam + "', '" + loai + "', gioStart, phutStart, gioEnd, phutEnd, N'" + hinhanhOut + "', N'" + idMayCheckOut + "', '" + trangthai + "', '" + nguoitao + "', '" + nguoitao + "' " +
                    " FROM GioLamViec WHERE loai = 1 AND trangthai = 1 AND phongban_fk = '" + phongban_fk + "' ";
                    command.CommandTimeout = int.MaxValue;
                    command.CommandText = sql;
                    kq = command.ExecuteNonQuery();
                    Debug.WriteLine($"[TimeKeeping] INSERT ChiTiet loai=2 result: kq={kq} (0=FAIL=GioLamViec không có phongban_fk={phongban_fk})");
                    if (kq < 1)
                    {
                        transaction.Rollback();
                        connection.Close();
                        DeviceManagerLogger.Log("IMPORT", $"FAIL INSERT ChiTiet loai=2: GioLamViec không có phongban_fk={phongban_fk}, nhansu={nhansu_fk}, ngay={ngaynhap}");
                        return "2.Error! FAIL ChiTiet loai=2 (GioLamViec phongban=" + phongban_fk + ")";
                    }
                }

                Debug.WriteLine($"[TimeKeeping] COMMIT OK: nhansu={nhansu_fk}, chamcong={chamcong_fk}");
                transaction.Commit();
                connection.Close();
            }
            
            return "";

        }

        public string GET_InformationStaff_CheckIn(string nhansu_fk, string ngaynhap)
        {
            string info = "";

            ConnectionDatabase conn = new ConnectionDatabase();
            SqlConnection connect = new SqlConnection(conn.ReturnConnectionDatabase("", "", "", ""));
            SqlTransaction transaction;

            connect.Open();
            transaction = connect.BeginTransaction();

            string sql = " SELECT pk_seq, ma, ten, B.trangthai, diachi, dienthoai, gioitinh, mail, ngaysinh, chinhanh_fk, phongbanGoc_fk, phongbanSupport_fk, chucvu_fk, " +
            " ISNULL(ngaybatdaulam, '') ngaybatdaulam, ISNULL(A.hinhanh, 'avatardefault.png') hinhanh, ISNULL(B.hinhanh, 'avatardefault.png') hinhanhIn, ISNULL(C.hinhanh, 'avatardefault.png') hinhanhOut, ISNULL(B.idMayChamCong, '') idMayCheckIn, " +
            " ISNULL((B.gioIn), 0) gioIn, ISNULL((B.phutIn), 0) phutIn, ISNULL((C.gioOut), 0) gioOut, ISNULL((C.phutOut), 0) phutOut, ISNULL(B.idMayChamCong, '') idMayCheckOut " +
            " FROM DanhSachNhanSu A LEFT JOIN " +
            " ( " +
            "	SELECT TOP(1) a.trangthai, b.nhansu_fk, b.gio AS gioIn, b.phut AS phutIn, b.hinhanh, b.idMayChamCong " +
            "	FROM ChamCong a INNER JOIN ChamCong_ChiTiet b ON a.pk_seq = b.chamcong_fk AND b.nhansu_fk = '" + nhansu_fk + "' AND b.ngaynhap = '" + ngaynhap + "' AND b.loai = '1' " +
            "	ORDER BY b.gio, b.phut DESC " +
            " ) B ON A.pk_seq = B.nhansu_fk LEFT JOIN " +
            " ( " +
            "	SELECT TOP(1) a.trangthai, b.nhansu_fk, b.gio AS gioOut, b.phut AS phutOut, b.hinhanh, b.idMayChamCong " +
            "	FROM ChamCong a INNER JOIN ChamCong_ChiTiet b ON a.pk_seq = b.chamcong_fk AND b.nhansu_fk = '" + nhansu_fk + "' AND b.ngaynhap = '" + ngaynhap + "' AND b.loai = '2' " +
            "	ORDER BY b.gio, b.phut DESC " +
            " ) C ON A.pk_seq = C.nhansu_fk " +
            " WHERE A.pk_seq = '" + nhansu_fk + "' ";
            SqlCommand command = new SqlCommand(sql, connect, transaction);
            command.CommandTimeout = int.MaxValue;
            command.CommandText = sql;
            SqlDataReader obj = command.ExecuteReader();
            if (obj != null)
            {
                DataTable dt = new DataTable();
                dt.Load(obj);

                info = dt.Rows[0]["pk_seq"].ToString() + " -- " + dt.Rows[0]["ten"].ToString() + " -- " + dt.Rows[0]["diachi"].ToString() + " -- " +
                    dt.Rows[0]["dienthoai"].ToString() + " -- " + dt.Rows[0]["gioitinh"].ToString() + " -- " + dt.Rows[0]["mail"].ToString() + " -- " +
                    dt.Rows[0]["ngaysinh"].ToString() + " -- " + dt.Rows[0]["ngaybatdaulam"].ToString() + " -- " + dt.Rows[0]["trangthai"].ToString() + " -- " +
                    dt.Rows[0]["chinhanh_fk"].ToString() + " -- " + dt.Rows[0]["phongbanGoc_fk"].ToString() + " -- " + dt.Rows[0]["phongbanSupport_fk"].ToString() + " -- " +
                    dt.Rows[0]["chucvu_fk"].ToString() + " -- " + dt.Rows[0]["gioIn"].ToString() + " -- " + dt.Rows[0]["phutIn"].ToString() + " -- " + dt.Rows[0]["gioOut"].ToString() + " -- " + dt.Rows[0]["phutOut"].ToString() + " -- " + 
                    dt.Rows[0]["hinhanh"].ToString() + " -- " + dt.Rows[0]["hinhanhIn"].ToString() + " -- " + dt.Rows[0]["hinhanhOut"].ToString() + " -- " + dt.Rows[0]["idMayCheckIn"].ToString() +" -- " + dt.Rows[0]["idMayCheckOut"].ToString();

                dt.Clone();
                dt.Clear();
            }

            return info;
        }
    }
}