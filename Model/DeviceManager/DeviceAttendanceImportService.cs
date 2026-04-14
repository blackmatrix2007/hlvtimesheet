using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using HLVTimeSheet.AcsessData;
using HLVTimeSheet.Model;

namespace HLVTimeSheet.Model.DeviceManager
{
    /// <summary>
    /// Đọc dữ liệu từ ChamCong_Device (máy chấm công khuôn mặt)
    /// và ghi vào bảng ChamCong / ChamCong_ChiTiet (TimeKeeping của HLVTimeSheet).
    ///
    /// Luồng:
    ///   ChamCong_Device (isValid=1)
    ///     → JOIN DanhSachNhanSu ON mapNV  (lấy nhansu_fk, phongban_fk)
    ///     → TimeKeepingController.INSERT_TimeKeeping_New()
    ///         → INSERT/UPDATE ChamCong + ChamCong_ChiTiet
    /// </summary>
    public class DeviceAttendanceImportService
    {
        public class ImportResult
        {
            public int Processed  { get; set; }
            public int Succeeded  { get; set; }
            public int Skipped    { get; set; }     // NV không tồn tại trong HLV
            public List<string> Errors { get; set; } = new List<string>();
        }

        /// <summary>
        /// Import chấm công từ ChamCong_Device vào bảng ChamCong cho một ngày.
        /// Gọi từ trang quản trị hoặc scheduled task.
        /// </summary>
        /// <param name="ngay">Ngày cần import</param>
        /// <param name="nguoiTao">Tên tài khoản thực hiện (ghi vào nguoitao)</param>
        public ImportResult ImportDay(DateTime ngay, string nguoiTao = "device-import")
        {
            var result = new ImportResult();
            var rows   = GetDailyCheckInOutFromDevice(ngay);

            Debug.WriteLine($"[Import] ImportDay {ngay:dd/MM/yyyy}: {rows.Rows.Count} rows từ ChamCong_Device");

            var tkCtrl = new TimeKeepingController();

            foreach (DataRow row in rows.Rows)
            {
                result.Processed++;
                string mapNV      = row["mapNV"].ToString();
                string nhansuFk   = row["nhansuFk"].ToString();
                string phongbanFk = row["phongbanFk"].ToString();
                string loaiCheck = row["loai"].ToString();
                string deviceId = row["deviceId"].ToString();

                Debug.WriteLine($"[Import] Row: mapNV={mapNV}, nhansuFk={nhansuFk}, phongbanFk={phongbanFk}, gioVao={row["gioVao"]}, gioRa={row["gioRa"]}");

                if (string.IsNullOrEmpty(nhansuFk))
                {
                    result.Skipped++;
                    result.Errors.Add($"[{mapNV}] Không tìm thấy nhân viên trong DanhSachNhanSu.");
                    Debug.WriteLine($"[Import] SKIP {mapNV}: nhansuFk rỗng");
                    continue;
                }

                // Tách giờ/phút vào–ra
                string gioIn  = "", phutIn  = "";
                string gioOut = "", phutOut = "";

                if (row["gioVao"] != DBNull.Value)
                {
                    var gv = (DateTime)row["gioVao"];
                    gioIn  = gv.Hour.ToString("D2");
                    phutIn = gv.Minute.ToString("D2");
                }

                if (row["gioRa"] != DBNull.Value)
                {
                    var gr = (DateTime)row["gioRa"];
                    gioOut  = gr.Hour.ToString("D2");
                    phutOut = gr.Minute.ToString("D2");
                }

                string mayCheckIn = "";
                string mayCheckOut = "";
            
                switch (loaiCheck)
                {
                    case "check_in":
                        loaiCheck = "1";
                        mayCheckIn = deviceId;
                        break;
                    case "check_out":
                        loaiCheck = "2";
                        mayCheckOut = deviceId;
                        break;
                    default:
                        loaiCheck = "1";
                        break;

                }    

                // Ngày dạng dd-MM-yyyy (format lưu trong ChamCong)
                string ngayStr = ngay.ToString("dd-MM-yyyy");

                if (string.IsNullOrEmpty(phongbanFk) || phongbanFk == "0")
                {
                    result.Skipped++;
                    result.Errors.Add($"[{mapNV}] Nhân viên chưa có phòng ban (phongban_fk=0) — cần cấu hình trong DanhSachNhanSu.");
                    Debug.WriteLine($"[Import] SKIP {mapNV}: phongban_fk rỗng/0, không tìm được GioLamViec");
                    continue;
                }

                Debug.WriteLine($"[Import] Calling INSERT_TimeKeeping_New: nhansu={nhansuFk}, phongban={phongbanFk}, ngay={ngayStr}, in={gioIn}:{phutIn}, out={gioOut}:{phutOut}");
                DeviceManagerLogger.Log("IMPORT", $"Calling INSERT_TimeKeeping_New: mapNV={mapNV}, nhansu={nhansuFk}, phongban={phongbanFk}, ngay={ngayStr}, in={gioIn}:{phutIn}, out={gioOut}:{phutOut}, loai={loaiCheck}, idIn={mayCheckIn}, idOut={mayCheckOut}");

                try
                {
                    string kq = tkCtrl.INSERT_TimeKeeping_New(
                        ngaynhap: ngayStr,
                        phongban_fk: phongbanFk,
                        nhansu_fk: nhansuFk,
                        gioIn: gioIn,
                        phutIn: phutIn,
                        gioOut: gioOut,
                        phutOut: phutOut,
                        loai: loaiCheck,
                        trangthai: "1",
                        hinhanhIn: "",
                        hinhanhOut: "",
                        idMayCheckIn: mayCheckIn,
                        idMayCheckOut: mayCheckOut,
                        nguoitao: nguoiTao);

                    if (string.IsNullOrEmpty(kq))
                    {
                        result.Succeeded++;
                        Debug.WriteLine($"[Import] OK: {mapNV}");
                        // Đánh dấu đã đồng bộ vào ChamCong
                        MarkAsSynced(mapNV, ngay);
                    }
                    else
                    {
                        result.Errors.Add($"[{mapNV}] {kq}");
                        Debug.WriteLine($"[Import] FAIL: {mapNV} → {kq}");
                    }
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"[{mapNV}] Exception: {ex.Message}");
                    Debug.WriteLine($"[Import] EXCEPTION {mapNV}: {ex.Message}");
                    DeviceManagerLogger.LogError("IMPORT", $"EXCEPTION {mapNV} (nhansu={nhansuFk}, phongban={phongbanFk}, ngay={ngayStr}, idIn={mayCheckIn}): {ex.Message}", ex);
                }
            }

            return result;
        }

        /// <summary>
        /// Import nhiều ngày liên tiếp.
        /// </summary>
        public ImportResult ImportRange(DateTime tuNgay, DateTime denNgay, string nguoiTao = "device-import")
        {
            var total = new ImportResult();
            for (var d = tuNgay.Date; d <= denNgay.Date; d = d.AddDays(1))
            {
                var r = ImportDay(d, nguoiTao);
                total.Processed += r.Processed;
                total.Succeeded += r.Succeeded;
                total.Skipped   += r.Skipped;
                total.Errors.AddRange(r.Errors);
            }
            return total;
        }

        // ─── Debug: Kiểm tra mapping mã NV ──────────────────────────────────────

        /// <summary>
        /// Trả về danh sách mã NV duy nhất trong ChamCong_Device
        /// cùng thông tin nhân viên tương ứng từ DanhSachNhanSu (nếu có).
        /// </summary>
        public DataTable GetMappingDebug()
        {
            var conn = new ConnectionDatabase();
            using (var sqlConn = new System.Data.SqlClient.SqlConnection(conn.ReturnConnectionDatabaseWS()))
            {
                sqlConn.Open();
                AttendanceSyncService.EnsureTableExists(sqlConn);

                const string sql = @"
                    SELECT DISTINCT
                        cd.mapNV,
                        ISNULL(CAST(ns.pk_seq AS NVARCHAR(20)), '')     AS pk_seq,
                        ISNULL(ns.ten, '')                               AS ten,
                        ISNULL((SELECT pb.ten FROM PhongBan pb WHERE pb.pk_seq = ns.phongban_fk), '') AS phongban,
                        ISNULL(CAST(ns.trangthai AS NVARCHAR(5)), '')    AS trangthai
                    FROM ChamCong_Device cd
                    LEFT JOIN DanhSachNhanSu ns ON ns.ma = cd.mapNV
                    ORDER BY cd.mapNV";

                using (var cmd = new System.Data.SqlClient.SqlCommand(sql, sqlConn))
                using (var adapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        /// <summary>
        /// Trả về danh sách nhân viên trong DanhSachNhanSu để đối chiếu với mã DeviceManager.
        /// </summary>
        public DataTable GetHlvStaffList()
        {
            var conn = new ConnectionDatabase();
            using (var sqlConn = new System.Data.SqlClient.SqlConnection(conn.ReturnConnectionDatabaseWS()))
            {
                sqlConn.Open();
                const string sql = @"
                    SELECT
                        ns.ma,
                        ns.ten,
                        ISNULL((SELECT pb.ten FROM PhongBan pb WHERE pb.pk_seq = ns.phongban_fk), '') AS phongban,
                        ns.trangthai
                    FROM DanhSachNhanSu ns
                    WHERE ns.trangthai = 1
                    ORDER BY ns.ma";

                using (var cmd = new System.Data.SqlClient.SqlCommand(sql, sqlConn))
                using (var adapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        // ─── Private: Đánh dấu đã đồng bộ ──────────────────────────────────────

        private static void MarkAsSynced(string mapNV, DateTime ngay)
        {
            try
            {
                var conn = new ConnectionDatabase();
                using (var sqlConn = new SqlConnection(conn.ReturnConnectionDatabaseWS()))
                {
                    sqlConn.Open();
                    const string sql = @"
                        UPDATE ChamCong_Device
                        SET    daDongBo   = 1,
                               ngayDongBo = GETDATE()
                        WHERE  mapNV = @mapNV
                          AND  CAST(thoiGian AS DATE) = @ngay
                          AND  daDongBo = 0";
                    using (var cmd = new SqlCommand(sql, sqlConn))
                    {
                        cmd.Parameters.AddWithValue("@mapNV", mapNV);
                        cmd.Parameters.AddWithValue("@ngay",  ngay.Date);
                        int rows = cmd.ExecuteNonQuery();
                        Debug.WriteLine($"[Import] MarkAsSynced: {mapNV} {ngay:dd/MM/yyyy} → {rows} rows updated");
                    }
                }
            }
            catch (Exception ex)
            {
                // Không ném ngoại lệ — import đã thành công, chỉ cần log
                Debug.WriteLine($"[Import] MarkAsSynced FAIL {mapNV}: {ex.Message}");
            }
        }

        // ─── Private: Đọc ChamCong_Device + join DanhSachNhanSu ─────────────────

        private static DataTable GetDailyCheckInOutFromDevice(DateTime ngay)
        {
            var conn = new ConnectionDatabase();
            using (var sqlConn = new SqlConnection(conn.ReturnConnectionDatabaseWS()))
            {
                sqlConn.Open();
                AttendanceSyncService.EnsureTableExists(sqlConn);

                // Giờ vào = check_in đầu tiên hợp lệ
                // Giờ ra  = check_out cuối cùng hợp lệ
                // JOIN DanhSachNhanSu để lấy pk_seq (nhansuFk) và phongban_fk (phongbanFk)
                const string sql = @"
                    SELECT
                        cd.mapNV,
                        ns.pk_seq                                                           AS nhansuFk,
                        ISNULL(CAST(ns.phongban_fk AS NVARCHAR(50)), '')                    AS phongbanFk,
                        MIN(CASE WHEN cd.loai='check_in'  AND cd.isValid=1 THEN cd.thoiGian END) AS gioVao,
                        MAX(CASE WHEN cd.loai='check_out' AND cd.isValid=1 THEN cd.thoiGian END) AS gioRa, 
                        cd.deviceId, cd.loai 
                    FROM ChamCong_Device cd
                    LEFT JOIN DanhSachNhanSu ns ON ns.ma = cd.mapNV AND ns.trangthai = 1 AND CAST(cd.thoiGian AS DATE) = @ngay 
                    WHERE CAST(cd.thoiGian AS DATE) = @ngay
                    GROUP BY cd.mapNV, cd.deviceId, cd.loai, ns.pk_seq, ns.phongban_fk
                    ORDER BY cd.mapNV";

                using (var cmd = new SqlCommand(sql, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@ngay", ngay.Date);
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }
}
