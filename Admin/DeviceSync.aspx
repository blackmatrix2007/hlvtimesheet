<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeviceSync.aspx.cs" Inherits="HLVTimeSheet.Admin.DeviceSync" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>
<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Đồng bộ máy chấm công (DeviceManager)</title>
    <link href="../Content/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body { padding: 20px; background: #f5f5f5; }
        .card { background: #fff; border-radius: 6px; padding: 20px; margin-bottom: 20px; box-shadow: 0 1px 3px rgba(0,0,0,.1); }
        .badge-success { background: #28a745; color: #fff; padding: 2px 7px; border-radius: 4px; }
        .badge-danger  { background: #dc3545; color: #fff; padding: 2px 7px; border-radius: 4px; }
        .badge-warning { background: #ffc107; color: #212529; padding: 2px 7px; border-radius: 4px; }
        .webhook-url   { background: #e9ecef; padding: 8px 12px; border-radius: 4px; font-family: monospace; font-size: .85em; word-break: break-all; }
    </style>
</head>
<body>
<form id="form1" runat="server">
<div class="container-fluid">
    <h4 class="mb-3">Đồng bộ máy chấm công (DeviceManager)</h4>

    <!-- Thông tin Webhook -->
    <div class="card">
        <h6>Cấu hình Webhook (PUSH) — Dán URL này vào DeviceManager</h6>
        <p class="text-muted mb-1" style="font-size:.85em">
            DeviceManager sẽ tự động POST dữ liệu chấm công mỗi khi nhân viên check-in / check-out.
        </p>
        <div class="webhook-url">
            <asp:Literal ID="litWebhookUrl" runat="server" />
        </div>
    </div>

    <!-- Xem danh sách chấm công từ DeviceManager -->
    <div class="card">
        <h6>Xem danh sách chấm công từ DeviceManager</h6>
        <div class="form-inline mb-2">
            <label class="mr-2">Từ ngày:</label>
            <asp:TextBox ID="txtViewFrom" runat="server" CssClass="form-control mr-3" TextMode="Date" />
            <label class="mr-2">Đến ngày:</label>
            <asp:TextBox ID="txtViewTo"   runat="server" CssClass="form-control mr-3" TextMode="Date" />
            <asp:Button ID="btnViewLogs" runat="server" Text="Xem danh sách"
                        CssClass="btn btn-info" OnClick="BtnViewLogs_Click" />
        </div>
        <asp:Literal ID="litViewLogs" runat="server" />
    </div>

    <!-- Kéo dữ liệu (PULL dự phòng) -->
    <div class="card">
        <h6>Kéo dữ liệu thủ công (PULL — dự phòng khi webhook bị mất)</h6>
        <div class="form-inline mb-2">
            <label class="mr-2">Từ ngày:</label>
            <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control mr-3" TextMode="Date" />
            <label class="mr-2">Đến ngày:</label>
            <asp:TextBox ID="txtTo"   runat="server" CssClass="form-control mr-3" TextMode="Date" />
            <asp:Button ID="btnPull" runat="server" Text="Kéo dữ liệu"
                        CssClass="btn btn-primary" OnClick="BtnPull_Click" />
        </div>
        <asp:Label ID="lblPullResult" runat="server" />
    </div>

    <!-- Tổng hợp hôm nay -->
    <div class="card">
        <h6>Tổng hợp chấm công hôm nay (từ DeviceManager API)</h6>
        <asp:Button ID="btnToday" runat="server" Text="Làm mới"
                    CssClass="btn btn-sm btn-secondary mb-2" OnClick="BtnToday_Click" />
        <asp:Literal ID="litToday" runat="server" />
    </div>

    <!-- Import vào bảng công HLVTimeSheet -->
    <div class="card">
        <h6>Import vào bảng công HLVTimeSheet (ChamCong)</h6>
        <p class="text-muted mb-2" style="font-size:.85em">
            Đọc giờ vào/ra từ <code>ChamCong_Device</code> và ghi vào bảng <code>ChamCong / ChamCong_ChiTiet</code>.
        </p>
        <div class="form-inline mb-2">
            <label class="mr-2">Từ ngày:</label>
            <asp:TextBox ID="txtImportFrom" runat="server" CssClass="form-control mr-3" TextMode="Date" />
            <label class="mr-2">Đến ngày:</label>
            <asp:TextBox ID="txtImportTo" runat="server" CssClass="form-control mr-3" TextMode="Date" />
            <asp:Button ID="btnImport" runat="server" Text="Import vào bảng công"
                        CssClass="btn btn-success" OnClick="BtnImport_Click" />
        </div>
        <asp:Label ID="lblImportResult" runat="server" />
    </div>

    <!-- Debug: Kiểm tra mapping mã NV -->
    <div class="card">
        <h6>Kiểm tra mapping mã NV (Debug)</h6>
        <p class="text-muted mb-2" style="font-size:.85em">
            So sánh mã nhân viên trong <code>ChamCong_Device</code> với bảng <code>DanhSachNhanSu</code>.
            Mã phải khớp chính xác để Import thành công.
        </p>
        <asp:Button ID="btnCheckMapping" runat="server" Text="Kiểm tra mapping"
                    CssClass="btn btn-sm btn-info mb-2" OnClick="BtnCheckMapping_Click" />
        <asp:Literal ID="litMapping" runat="server" />

        <hr/>
        <h6 class="mt-2">Danh sách nhân viên trong DanhSachNhanSu (mã <code>ma</code>)</h6>
        <p class="text-muted mb-2" style="font-size:.85em">
            Mã NV trên DeviceManager phải trùng với cột <code>ma</code> bên dưới.
        </p>
        <asp:Button ID="btnShowHlvStaff" runat="server" Text="Xem danh sách nhân viên HLV"
                    CssClass="btn btn-sm btn-secondary mb-2" OnClick="BtnShowHlvStaff_Click" />
        <asp:Literal ID="litHlvStaff" runat="server" />
    </div>

    <!-- Nhân viên đã đăng ký khuôn mặt -->
    <div class="card">
        <div class="d-flex justify-content-between align-items-center mb-2">
            <h6 class="mb-0">Nhân viên đã đăng ký khuôn mặt trên DeviceManager</h6>
            <a href="DeviceFaceRegister.aspx" class="btn btn-sm btn-primary">&#128247; Quản lý khuôn mặt</a>
        </div>
        <asp:Button ID="btnListEmp" runat="server" Text="Tải danh sách"
                    CssClass="btn btn-sm btn-secondary mb-2" OnClick="BtnListEmp_Click" />
        <asp:Literal ID="litEmployees" runat="server" />
    </div>
</div>
<script src="../Scripts/jquery-3.6.0.min.js"></script>
<script src="../Content/bootstrap/js/bootstrap.min.js"></script>
</form>
</body>
</html>
