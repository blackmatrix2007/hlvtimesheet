# QC Test Report — Tích hợp HLVTimeSheet ↔ DeviceManager

**Version:** 1.0
**Ngày tạo:** 2026-03-27
**Người tạo:** Dev Team
**Phạm vi:** Module tích hợp máy chấm công nhận diện khuôn mặt

---

## Môi trường cần chuẩn bị

| Thành phần | Yêu cầu |
|------------|---------|
| HLVTimeSheet | Deploy lên IIS, có domain/IP public (để DeviceManager gọi webhook được) |
| DeviceManager Backend | Đang chạy tại `https://device.erp-x.com` (hoặc môi trường test) |
| SQL Server | `GDC_HLV_WorkSchedule` — có dữ liệu `DanhSachNhanSu`, `PhongBan` |
| Thiết bị Android | Máy chấm công đã activate và kết nối DeviceManager |
| Tài khoản test | DeviceManager: `admin@erpx.com / Admin@123456` |
| SMTP test | Gmail App Password hoặc Mailhog local |

---

## TC-01 — Cấu hình DeviceManagerSettings từ UI

**URL:** `/Admin/DeviceSettings.aspx`
**Mục tiêu:** Lưu cấu hình kết nối DeviceManager và SMTP vào bảng `DeviceManagerSettings`

| # | Bước thực hiện | Dữ liệu đầu vào | Kết quả kỳ vọng |
|---|---------------|-----------------|-----------------|
| 1 | Truy cập trang | — | Trang load thành công, form hiển thị |
| 2 | Để trống Base URL → nhấn Lưu | Base URL = rỗng | Hiện cảnh báo "Base URL và Customer ID là bắt buộc" |
| 3 | Nhập đủ thông tin hợp lệ → nhấn Lưu | BaseUrl, CustomerId, ApiKey, WebhookSecret hợp lệ | Alert xanh "Đã lưu cấu hình thành công" |
| 4 | Reload trang | — | Các field (trừ password) hiển thị lại đúng giá trị vừa lưu |
| 5 | Nhập ngưỡng face confidence = `1.5` → nhấn Lưu | threshold = 1.5 | Cảnh báo "Ngưỡng phải là số từ 0 đến 1" |
| 6 | Nhập đủ SMTP → nhấn "Gửi email test" | SMTP hợp lệ, email nhận hợp lệ | Alert xanh "Đã gửi email test thành công" + email thực sự đến hộp thư |
| 7 | SMTP sai host → nhấn "Gửi email test" | smtpHost = `invalid.host` | Alert đỏ hiển thị lỗi cụ thể |
| 8 | Bật DeviceManager → lưu → tắt → lưu lại | dangHoatDong toggle | Reload trang phản ánh đúng trạng thái checkbox |

**Kiểm tra DB sau TC-01:**
```sql
SELECT baseUrl, customerId, dangHoatDong, smtpHost, alertEmails, faceConfidenceThreshold
FROM DeviceManagerSettings WHERE laDinhChinh = 1;
```

---

## TC-02 — Đăng ký khuôn mặt nhân viên

**URL:** `/Admin/DeviceFaceRegister.aspx`
**Mục tiêu:** Upload ảnh khuôn mặt nhân viên lên DeviceManager API

| # | Bước thực hiện | Dữ liệu đầu vào | Kết quả kỳ vọng |
|---|---------------|-----------------|-----------------|
| 1 | Truy cập trang | — | Dropdown nhân viên load đúng danh sách từ `DanhSachNhanSu` |
| 2 | Không chọn NV → nhấn Đăng ký | NV = rỗng | Cảnh báo "Vui lòng chọn nhân viên" |
| 3 | Chọn NV, không chọn file → nhấn Đăng ký | file = rỗng | Cảnh báo "Vui lòng chọn file ảnh" |
| 4 | Upload file PDF → nhấn Đăng ký | file.pdf | Cảnh báo "Chỉ chấp nhận JPEG hoặc PNG" |
| 5 | Upload ảnh > 5MB | file 6MB | Cảnh báo "Ảnh vượt quá 5 MB" |
| 6 | Upload ảnh hợp lệ cho NV chưa đăng ký | JPEG rõ khuôn mặt, < 5MB | Alert xanh "Đăng ký khuôn mặt thành công" |
| 7 | Kiểm tra bảng danh sách sau upload | — | Cột "Khuôn mặt" của NV vừa upload chuyển từ đỏ → xanh "Đã đăng ký" |
| 8 | Upload ảnh cho NV đã có khuôn mặt | ảnh khác của cùng NV | API DeviceManager cập nhật, không lỗi |
| 9 | Preview ảnh | Chọn file bất kỳ | Ảnh preview hiển thị ngay dưới input trước khi upload |
| 10 | Upload khi DeviceManager offline | — | Alert đỏ "Lỗi kết nối DeviceManager: ..." |

**Kiểm tra sau TC-02:**
```
DeviceManager Dashboard → Employees → Tìm mã NV vừa đăng ký → Có ảnh khuôn mặt
```

---

## TC-03 — Webhook PUSH nhận chấm công

**URL Webhook:** `/Admin/Hander/hdAttendanceWebhook.ashx`
**Mục tiêu:** Nhận sự kiện check-in/check-out từ DeviceManager, ghi vào `ChamCong_Device`

### TC-03a — Happy path

| # | Bước thực hiện | Payload | Kết quả kỳ vọng |
|---|---------------|---------|-----------------|
| 1 | POST JSON hợp lệ — check-in | `event=attendance.checkin`, NV tồn tại, checkingTime hợp lệ | HTTP 200, `{"success":true,"isValid":true}` |
| 2 | POST JSON hợp lệ — check-out | `event=attendance.checkout`, cùng NV, cùng ngày | HTTP 200, `{"success":true,"isValid":true}` |

**Kiểm tra DB:**
```sql
SELECT mapNV, loai, thoiGian, diemTin, isValid, isDuplicate, attemptNumber, source
FROM ChamCong_Device
WHERE mapNV = 'NV_TEST' AND CAST(thoiGian AS DATE) = CAST(GETDATE() AS DATE)
ORDER BY thoiGian;
```

### TC-03b — Validation

| # | Bước thực hiện | Payload | Kết quả kỳ vọng |
|---|---------------|---------|-----------------|
| 3 | GET thay vì POST | — | HTTP 405 Method Not Allowed |
| 4 | Body rỗng | `{}` | HTTP 400, message "employeeCode và checkingTime là bắt buộc" |
| 5 | Event không hợp lệ | `event=unknown.event` | HTTP 400, message "event không được hỗ trợ" |
| 6 | checkingTime sai format | `checkingTime=abc` | HTTP 422, message "checkingTime không hợp lệ" |
| 7 | Mã NV không tồn tại trong DanhSachNhanSu | `employeeCode=NV_GHOST` | HTTP 200, `{"isValid":false, "rejectionReason":"Mã nhân viên không tồn tại..."}` |
| 8 | Chấm công lần 2 trong ngày (duplicate) | Gửi lại payload giống TC-03a#1 | HTTP 200, `{"isValid":false, "isDuplicate":true, "attemptNumber":2}` |

### TC-03c — Xác thực chữ ký HMAC

| # | Bước thực hiện | Điều kiện | Kết quả kỳ vọng |
|---|---------------|-----------|-----------------|
| 9 | Gửi webhook có `signature` sai | WebhookSecret đã cấu hình | HTTP 422, "Chữ ký webhook không hợp lệ" |
| 10 | Gửi webhook có `signature` đúng | HMAC-SHA256(`event:employeeCode:checkingTime`, secret) | HTTP 200 thành công |
| 11 | Không gửi `signature` khi chưa cấu hình secret | webhookSecret = rỗng | HTTP 200 thành công (bỏ qua xác thực) |

**Curl test mẫu:**
```bash
curl -X POST https://{domain}/Admin/Hander/hdAttendanceWebhook.ashx \
  -H "Content-Type: application/json" \
  -d '{
    "event": "attendance.checkin",
    "employeeCode": "NV001",
    "checkingTime": "2026-03-27T08:30:00+07:00",
    "faceConfidence": 0.92,
    "deviceName": "Camera-Lobby",
    "source": "test"
  }'
```

---

## TC-04 — Email cảnh báo bất thường

**Mục tiêu:** Gửi email khi phát hiện bất thường, không block luồng chính

| # | Tình huống | Điều kiện | Kết quả kỳ vọng |
|---|-----------|-----------|-----------------|
| 1 | Face confidence thấp | `faceConfidence=0.3` (ngưỡng=0.6), NV tồn tại, SMTP đã cấu hình | Webhook trả 200 bình thường + email cảnh báo đến hộp thư trong ≤ 30s |
| 2 | Face confidence đủ ngưỡng | `faceConfidence=0.85` | Webhook trả 200, **không** gửi email |
| 3 | Mã NV không tồn tại | `employeeCode=NV_GHOST` | Webhook trả 200 + email cảnh báo "mã NV không tồn tại" |
| 4 | SMTP chưa cấu hình | Bảng DeviceManagerSettings chưa có smtpHost | Webhook trả 200 bình thường, **không** crash, không gửi email |
| 5 | SMTP sai password | credentials sai | Webhook trả 200 bình thường (email gửi lỗi bị bỏ qua), **không** crash |

**Kiểm tra nội dung email cảnh báo confidence thấp:**
- Subject: `[Cảnh báo] Chấm công không đủ tin cậy — NV001`
- Body: Bảng thông tin NV, thời gian, thiết bị, confidence hiển thị màu đỏ

---

## TC-05 — PULL dữ liệu thủ công

**URL:** `/Admin/DeviceSync.aspx`
**Mục tiêu:** Kéo dữ liệu chấm công từ DeviceManager API, upsert vào `ChamCong_Device`

| # | Bước thực hiện | Dữ liệu | Kết quả kỳ vọng |
|---|---------------|---------|-----------------|
| 1 | Chọn ngày hợp lệ → nhấn "Kéo dữ liệu" | Khoảng ngày có dữ liệu | Alert xanh "Đã lưu/cập nhật X bản ghi" |
| 2 | Pull lại cùng khoảng ngày lần 2 | — | Không tạo bản ghi trùng (UPSERT theo `dmLogId`) |
| 3 | Kiểm tra cột `source` | — | Records từ PULL có `source='pull'`, từ webhook có `source='device-manager'` |
| 4 | Pull khi DeviceManager offline | — | Alert đỏ, **không** crash server |

---

## TC-06 — Import vào bảng công (ChamCong)

**URL:** `/Admin/DeviceSync.aspx` → phần "Import vào bảng công"
**Mục tiêu:** Đọc `ChamCong_Device` → ghi vào `ChamCong` / `ChamCong_ChiTiet`

**Điều kiện tiên quyết:** Đã có dữ liệu `isValid=1` trong `ChamCong_Device` cho ngày test

| # | Bước thực hiện | Dữ liệu | Kết quả kỳ vọng |
|---|---------------|---------|-----------------|
| 1 | Chọn ngày có dữ liệu → nhấn "Import vào bảng công" | Ngày có check-in/out hợp lệ | Alert xanh, hiển thị số lượng thành công |
| 2 | NV có trong `ChamCong_Device` nhưng không có trong `DanhSachNhanSu` | mapNV = `NV_GHOST` | Hiển thị "Bỏ qua: 1 — Không tìm thấy nhân viên" |
| 3 | Kiểm tra `ChamCong` sau import | — | Có bản ghi với `nhansu_fk`, `thoigianIn`, `thoigianOut` đúng |
| 4 | Kiểm tra `ChamCong_ChiTiet` | — | Có bản ghi loai=1 (vào) và loai=2 (ra) khớp với `GioLamViec` |
| 5 | Import lại cùng ngày (idempotent) | — | `ChamCong` update `ngaysua`, **không** tạo bản ghi trùng |
| 6 | Ngày không có dữ liệu trong `ChamCong_Device` | — | Hiển thị "Đã xử lý 0 bản ghi" |

**Kiểm tra DB sau TC-06:**
```sql
SELECT cc.ngaynhap, cc.nhansu_fk, cc.thoigianIn, cc.thoigianOut
FROM ChamCong cc
JOIN DanhSachNhanSu ns ON ns.pk_seq = cc.nhansu_fk
WHERE cc.ngaynhap = '27/03/2026';

SELECT cct.nhansu_fk, cct.thoigian, cct.loai
FROM ChamCong_ChiTiet cct
WHERE cct.ngaynhap = '27/03/2026';
```

---

## TC-07 — Tổng hợp hôm nay & danh sách NV

**URL:** `/Admin/DeviceSync.aspx`

| # | Bước thực hiện | Kết quả kỳ vọng |
|---|---------------|-----------------|
| 1 | Nhấn "Làm mới" trong phần Tổng hợp hôm nay | Bảng hiển thị đúng: tổng check-in, check-out, NV duy nhất, danh sách gần đây |
| 2 | Nhấn "Tải danh sách" trong phần NV đã đăng ký | Bảng NV hiển thị đúng số ảnh khuôn mặt mỗi người |
| 3 | URL Webhook hiển thị | — | URL đúng dạng `https://{domain}/Admin/Hander/hdAttendanceWebhook.ashx` |

---

## TC-08 — Cấu hình load ưu tiên DB trước Web.config

**Mục tiêu:** Xác nhận thứ tự ưu tiên cấu hình

| # | Bước thực hiện | Điều kiện | Kết quả kỳ vọng |
|---|---------------|-----------|-----------------|
| 1 | Xóa hàng trong `DeviceManagerSettings` | Bảng rỗng | Hệ thống dùng giá trị từ `Web.config appSettings` |
| 2 | Thêm hàng vào `DeviceManagerSettings` với giá trị khác Web.config | — | Hệ thống dùng giá trị từ DB (override Web.config) |

---

## TC-09 — Tự tạo bảng DB khi chưa tồn tại

**Mục tiêu:** Các bảng được tự tạo, không cần chạy script DDL thủ công

| # | Bảng | Kiểm tra |
|---|------|---------|
| 1 | `ChamCong_Device` | Xóa bảng → gọi webhook → bảng được tạo lại với đầy đủ cột và index |
| 2 | `DeviceManagerSettings` | Xóa bảng → vào `DeviceSettings.aspx` → bảng được tạo lại |

---

## TC-10 — End-to-end flow thực tế

**Mục tiêu:** Kiểm tra toàn bộ luồng từ đăng ký đến bảng công

```
Bước 1: DeviceSettings.aspx → cấu hình API + Webhook URL + SMTP
Bước 2: DeviceFaceRegister.aspx → upload ảnh NV001
Bước 3: DeviceManager sync MQTT xuống máy chấm công
Bước 4: NV001 đứng trước máy → máy nhận diện → DeviceManager nhận log
Bước 5: DeviceManager POST webhook → hdAttendanceWebhook.ashx
Bước 6: Kiểm tra ChamCong_Device có bản ghi isValid=1
Bước 7: DeviceSync.aspx → nhấn "Import vào bảng công"
Bước 8: Kiểm tra ChamCong / ChamCong_ChiTiet có dữ liệu
```

**Kết quả kỳ vọng:** Toàn bộ 8 bước thành công, không cần can thiệp thủ công.

---

## Regression — Các tính năng cũ không được ảnh hưởng

| Module | Kiểm tra |
|--------|---------|
| Đăng nhập HLVTimeSheet | Vẫn hoạt động bình thường |
| Nhập bảng công thủ công (`TimeKeeping.aspx`) | Vẫn hoạt động, không bị override |
| Quản lý nhân viên (`DanhSachNhanSu`) | Vẫn CRUD bình thường |
| Báo cáo lương | Không bị ảnh hưởng bởi module mới |

---

## Lưu ý cho QC

1. **Webhook cần IP public** — Không test được trên localhost trừ khi dùng ngrok hoặc tương tự
2. **Fire-and-forget email** — Email alert gửi async, không block response. Test bằng cách kiểm tra hộp thư sau ~30s
3. **Duplicate logic** — `attemptNumber` tính theo **tổng lượt trong ngày** (check_in + check_out), không phân biệt loại
4. **PULL idempotent** — Chạy nhiều lần cùng khoảng ngày không tạo bản ghi trùng (dùng `dmLogId` UNIQUE)
5. **Import bảng công** — Chỉ lấy bản ghi `isValid=1`. NV bị `isValid=0` (audit) sẽ bị bỏ qua
6. **API Key** — Trường ApiKey trong `DeviceSettings.aspx` dạng password, không hiển thị lại sau khi lưu. Để trống khi save nếu không muốn đổi
