# Tích hợp HLVTimeSheet ↔ DeviceManager

## Tổng quan hệ thống

| Hệ thống | Công nghệ | Database | Vai trò |
|----------|-----------|----------|---------|
| **HLVTimeSheet** | ASP.NET WebForms, .NET 4.7.2, C# | SQL Server 2022 (`GDC_HLV_WorkSchedule`) | Quản lý HR, bảng công, lương |
| **DeviceManager** | NestJS, TypeScript, React, Android | PostgreSQL (`devicemanagement`) | Quản lý máy chấm công, nhận diện khuôn mặt |
| **Thiết bị Android** | Java, ArcSoft Face SDK v5 | Room (SQLite local) | Máy chấm công thực tế |

---

## Cấu trúc file đã implement

```
HLVTimeSheet/
├── Model/DeviceManager/
│   ├── DeviceManagerConfig.cs          ← Load config: DB trước, Web.config fallback
│   ├── DeviceManagerApiClient.cs       ← HTTP client, X-API-Key auth, timeout 30s
│   ├── DeviceManagerModels.cs          ← DTOs: DmEmployee, DmAttendanceLog, WebhookAttendanceDto, ...
│   ├── EmployeeSyncService.cs          ← Đăng ký / xóa / lấy danh sách khuôn mặt nhân viên
│   └── AttendanceSyncService.cs        ← PUSH webhook processor + PULL dự phòng
│
├── Admin/Hander/
│   └── hdAttendanceWebhook.ashx[.cs]   ← Endpoint nhận webhook PUSH từ DeviceManager
│
└── Admin/
    └── DeviceSync.aspx[.cs]            ← Trang quản trị: URL webhook, PULL thủ công, trạng thái
```

---

## Luồng A — Đăng ký nhân viên (HLVTimeSheet → DeviceManager)

```
[HR Admin - HLVTimeSheet]
        │  Upload ảnh khuôn mặt nhân viên
        ▼
[EmployeeSyncService.RegisterEmployeeFaceAsync()]
        │
        │  DeviceManagerConfig.Load()
        │    1. SELECT từ bảng DeviceManagerSettings (SQL Server)
        │    2. Fallback: Web.config appSettings
        │
        │  POST multipart/form-data
        │  Header: X-API-Key: {apiKey}
        ▼
[DeviceManager API]
  POST /employees/{customerId}/register-face
        │  Lưu face feature → PostgreSQL
        │  MQTT sync xuống tất cả thiết bị
        ▼
[Thiết bị Android]
        │  Nhận danh sách khuôn mặt qua MQTT
        │  Lưu vào Room DB (local) — ArcSoft SDK
        └─ Sẵn sàng nhận diện khuôn mặt
```

**API liên quan:**

| Hành động | Method | Endpoint |
|-----------|--------|----------|
| Đăng ký khuôn mặt | `POST` | `/employees/{customerId}/register-face` |
| Lấy danh sách NV | `GET` | `/employees/{customerId}?page=1&limit=200` |
| Lấy 1 NV | `GET` | `/employees/{customerId}/{employeeCode}` |
| Xóa NV | `DELETE` | `/employees/{customerId}/{employeeCode}` |
| Sync xuống thiết bị | `POST` | `/employees/{customerId}/sync-to-device/{deviceId}` |

---

## Luồng B — Chấm công thực tế (Thiết bị → HLVTimeSheet) ← Luồng chính

```
[Nhân viên đứng trước máy chấm công]
        │  Camera chụp khuôn mặt
        ▼
[Thiết bị Android]
        │  ArcSoft FaceEngine.detectFaces()
        │  So khớp face DB local → tìm employeeCode
        │  confidence_score: 0.0 – 1.0
        │
        ├─ MQTT publish  → DeviceManager (real-time)
        └─ HTTP POST     → DeviceManager (fallback)
        ▼
[DeviceManager Backend - NestJS :3008]
        │  Lưu attendance log → PostgreSQL
        │
        │  Gọi Webhook đã cấu hình:
        │  POST https://{domain}/Admin/Hander/hdAttendanceWebhook.ashx
        │
        │  Payload JSON:
        │  {
        │    "event":         "attendance.checkin",
        │    "employeeCode":  "NV001",
        │    "checkingTime":  "2026-03-26T08:30:00+07:00",
        │    "faceConfidence": 0.92,
        │    "latitude":      10.7769,
        │    "longitude":     106.7009,
        │    "gpsAccuracy":   5.0,
        │    "deviceName":    "Camera-Lobby",
        │    "source":        "arcface-device",
        │    "signature":     "hmac-sha256-hex"
        │  }
        ▼
[hdAttendanceWebhook.ashx]
        │  Validate: Method = POST
        │  Deserialize JSON → WebhookAttendanceDto
        │  Validate bắt buộc: employeeCode, checkingTime, event
        ▼
[AttendanceSyncService.ProcessWebhook()]
        │
        │  Bước 1 — Xác thực chữ ký HMAC-SHA256
        │    payload = "{event}:{employeeCode}:{checkingTime}"
        │    HMAC(webhookSecret) != dto.Signature → trả 422
        │
        │  Bước 2 — Parse thời gian
        │    DateTime.TryParse(checkingTime) → ToLocalTime()
        │
        │  Bước 3 — Xác định loại
        │    "attendance.checkin"  → loai = "check_in"
        │    "attendance.checkout" → loai = "check_out"
        │
        │  Bước 4 — Tính attemptNumber
        │    SELECT MAX(attemptNumber) + 1
        │    FROM ChamCong_Device
        │    WHERE mapNV = @mapNV AND ngay = @ngay
        │    attemptNo > 1 → isDuplicate = true, isValid = false
        │
        │  Bước 5 — INSERT vào ChamCong_Device
        │
        ▼
[SQL Server — GDC_HLV_WorkSchedule]
  Bảng ChamCong_Device:
  ┌──────────┬───────────┬──────────┬─────────┬─────────┬─────────┬───────────────────────┐
  │ mapNV    │ loai      │ thoiGian │ diemTin │ isValid │ attempt │ rejectionReason        │
  ├──────────┼───────────┼──────────┼─────────┼─────────┼─────────┼───────────────────────┤
  │ NV001    │ check_in  │ 08:30:00 │ 0.92    │ 1       │ 1       │ NULL                  │
  │ NV001    │ check_out │ 17:45:00 │ 0.88    │ 1       │ 2       │ NULL                  │
  │ NV001    │ check_in  │ 08:31:00 │ 0.85    │ 0       │ 3       │ Chấm công trùng lặp   │ ← audit
  └──────────┴───────────┴──────────┴─────────┴─────────┴─────────┴───────────────────────┘
        │
        │  HTTP 200 → DeviceManager dừng retry
        │  HTTP 500 → DeviceManager tự retry
        ▼
[TimeKeepingController - HLVTimeSheet]
        │  AttendanceSyncService.GetDailyCheckInOut()
        │  SELECT MIN(gioVao), MAX(gioRa) WHERE isValid = 1
        ▼
[Bảng công hàng tháng]
  NV001 | 26/03 | Vào: 08:30 | Ra: 17:45 | Số giờ: 9.25h
```

---

## Luồng C — Dự phòng PULL (khi webhook bị mất)

```
[Admin - DeviceSync.aspx]
        │  Chọn khoảng ngày → nhấn "Kéo dữ liệu"
        ▼
[AttendanceSyncService.PullAndSaveAsync()]
        │
        │  GET /attendance/external/check-ins
        │      ?companyId={customerId}&startDate=...&endDate=...
        │  Header: X-API-Key
        ▼
[DeviceManager API]
        │  Trả danh sách attendance logs (JSON)
        ▼
[MERGE INTO ChamCong_Device]
        │  Dùng dmLogId để tránh trùng (UPSERT)
        │  source = 'pull'   ← phân biệt với webhook
        ▼
[SQL Server - ChamCong_Device]  ✓ đồng bộ xong
```

---

## Schema bảng SQL Server

### ChamCong_Device
Tự tạo lần đầu khi gọi webhook hoặc pull.

```sql
CREATE TABLE ChamCong_Device (
    pk_seq          INT IDENTITY(1,1) PRIMARY KEY,
    dmLogId         INT,               -- ID từ DeviceManager (dùng cho PULL UPSERT)
    mapNV           NVARCHAR(50)  NOT NULL,
    loai            NVARCHAR(20)  NOT NULL,   -- 'check_in' / 'check_out'
    thoiGian        DATETIME      NOT NULL,
    deviceId        NVARCHAR(100),
    deviceName      NVARCHAR(200),
    diemTin         FLOAT,                    -- face confidence 0.0–1.0

    -- GPS
    latitude        FLOAT,
    longitude       FLOAT,
    gpsAccuracy     FLOAT,

    -- Audit trail
    isValid         BIT  DEFAULT 1,           -- bản ghi chính thức
    isDuplicate     BIT  DEFAULT 0,
    attemptNumber   INT  DEFAULT 1,           -- lần thứ mấy trong ngày
    rejectionReason NVARCHAR(500),
    source          NVARCHAR(100) DEFAULT 'device-manager',  -- 'webhook' / 'pull'

    ngaySync        DATETIME DEFAULT GETDATE(),

    CONSTRAINT UQ_ChamCong_Device_DmLogId UNIQUE (dmLogId)
);

CREATE INDEX IX_ChamCong_Device_mapNV_thoiGian ON ChamCong_Device (mapNV, thoiGian);
CREATE INDEX IX_ChamCong_Device_Valid           ON ChamCong_Device (mapNV, thoiGian, isValid, loai);
```

### DeviceManagerSettings
Cấu hình per-company, tự tạo khi khởi động.

```sql
CREATE TABLE DeviceManagerSettings (
    pk_seq        INT IDENTITY(1,1) PRIMARY KEY,
    chiNhanhId    NVARCHAR(50),          -- NULL = áp dụng toàn công ty
    baseUrl       NVARCHAR(500) NOT NULL, -- https://device.erp-x.com/api
    customerId    NVARCHAR(100) NOT NULL, -- UUID từ DeviceManager
    apiKey        NVARCHAR(255) NOT NULL, -- X-API-Key header
    apiSecret     NVARCHAR(255),
    webhookSecret NVARCHAR(255),          -- HMAC-SHA256 secret
    laDinhChinh   BIT DEFAULT 1,
    dangHoatDong  BIT DEFAULT 1,
    ngayTao       DATETIME DEFAULT GETDATE(),
    nguoiTao      NVARCHAR(100)
);
```

**Insert cấu hình ban đầu:**
```sql
INSERT INTO DeviceManagerSettings (baseUrl, customerId, apiKey, webhookSecret, laDinhChinh)
VALUES (
    'https://device.erp-x.com/api',
    '578f3f6f-14db-4adf-9c02-fdad454273ea',
    'ck_a49fbf00754cc51f3b20d3eb719fffe6',
    'your-webhook-secret-here',
    1
);
```

---

## Cấu hình DeviceManager (Webhook URL)

Trong giao diện DeviceManager Dashboard → Settings → Webhook:

```
Webhook URL:    https://{domain}/Admin/Hander/hdAttendanceWebhook.ashx
Webhook Secret: (khớp với DeviceManagerSettings.webhookSecret)
Events:         attendance.checkin, attendance.checkout
```

---

## Web.config fallback (nếu chưa có bảng DB)

```xml
<appSettings>
  <add key="DeviceManager_BaseUrl"       value="https://device.erp-x.com/api" />
  <add key="DeviceManager_CustomerId"    value="578f3f6f-14db-4adf-9c02-fdad454273ea" />
  <add key="DeviceManager_ApiKey"        value="ck_a49fbf00754cc51f3b20d3eb719fffe6" />
  <add key="DeviceManager_WebhookSecret" value="" />
</appSettings>
```

---

## Thứ tự triển khai

1. **Deploy HLVTimeSheet** lên server có domain/IP public
2. **Cấu hình DeviceManagerSettings** trong SQL Server (hoặc để Web.config fallback)
3. **Cấu hình Webhook URL** trong DeviceManager Dashboard trỏ về `hdAttendanceWebhook.ashx`
4. **Upload ảnh khuôn mặt** nhân viên qua `EmployeeSyncService.RegisterEmployeeFaceAsync()`
5. **DeviceManager sync MQTT** xuống thiết bị Android
6. **Nhân viên chấm công** → thiết bị nhận diện → webhook → `ChamCong_Device`
7. **Lập bảng công** từ `AttendanceSyncService.GetDailyCheckInOut()`
8. **Dùng DeviceSync.aspx** để kiểm tra trạng thái và PULL dự phòng nếu cần

---

## Điểm còn thiếu / cần làm tiếp

| Hạng mục | Trạng thái | Ghi chú |
|----------|-----------|---------|
| UI upload ảnh khuôn mặt nhân viên | ❌ Chưa có | Cần trang cho HR upload ảnh từng NV |
| UI cấu hình DeviceManagerSettings | ❌ Chưa có | Hiện phải INSERT thẳng vào DB |
| Nối `TimeKeepingController` ← `ChamCong_Device` | ❌ Chưa nối | Cần gọi `GetDailyCheckInOut()` khi lập bảng công |
| Kiểm tra `mapNV` tồn tại trong `DanhSachNhanSu` | ❌ Chưa có | Webhook chưa validate NV có trong HLV không |
| Thông báo chấm công bất thường | ❌ Chưa có | face confidence thấp, GPS ngoài vùng |
| Geofence validation | ❌ Chưa có | Kiểm tra latitude/longitude trong bán kính cho phép |
