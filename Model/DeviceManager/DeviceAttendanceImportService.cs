using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

            var tkCtrl = new TimeKeepingController();

            foreach (DataRow row in rows.Rows)
            {
                result.Processed++;
                string mapNV      = row["mapNV"].ToString();
                string nhansuFk   = row["nhansuFk"].ToString();
                string phongbanFk = row["phongbanFk"].ToString();

                if (string.IsNullOrEmpty(nhansuFk))
                {
                    result.Skipped++;
                    result.Errors.Add($"[{mapNV}] Không tìm thấy nhân viên trong DanhSachNhanSu.");
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

                // Ngày dạng dd/MM/yyyy (đúng format INSERT_TimeKeeping_New)
                string ngayStr = ngay.ToString("dd/MM/yyyy");

                string kq = tkCtrl.INSERT_TimeKeeping_New(
                    ngaynhap:   ngayStr,
                    phongban_fk: phongbanFk,
                    nhansu_fk:   nhansuFk,
                    gioIn:       gioIn,
                    phutIn:      phutIn,
                    gioOut:      gioOut,
                    phutOut:     phutOut,
                    loai:        "1",
                    trangthai:   "1",
                    nguoitao:    nguoiTao);

                if (string.IsNullOrEmpty(kq))
                    result.Succeeded++;
                else
                    result.Errors.Add($"[{mapNV}] {kq}");
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
                        MAX(CASE WHEN cd.loai='check_out' AND cd.isValid=1 THEN cd.thoiGian END) AS gioRa
                    FROM ChamCong_Device cd
                    LEFT JOIN DanhSachNhanSu ns ON ns.ma = cd.mapNV AND ns.trangthai = 1
                    WHERE CAST(cd.thoiGian AS DATE) = @ngay
                    GROUP BY cd.mapNV, ns.pk_seq, ns.phongban_fk
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
