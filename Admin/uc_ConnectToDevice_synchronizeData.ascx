<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_ConnectToDevice_synchronizeData.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_ConnectToDevice_synchronizeData" %>

<section class="content">
<div class="box box-primary" style="text-align:center;">
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
</div>
</section>