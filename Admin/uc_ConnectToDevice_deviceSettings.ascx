<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_ConnectToDevice_deviceSettings.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_ConnectToDevice_deviceSettings" %>

<section class="content">

<div class="box box-primary">
    <div class="container-fluid" style="max-width:800px">
    <h4 class="mb-3">Cấu hình tích hợp DeviceManager</h4>

    <asp:Label ID="lblMsg" runat="server" />

    <!-- DeviceManager API -->
    <div class="card">
        <p class="section-title">DeviceManager API</p>
        <div class="form-group">
            <label>Base URL <span class="text-danger">*</span></label>
            <asp:TextBox ID="txtBaseUrl" runat="server" CssClass="form-control"
                         placeholder="https://device.erp-x.com/api" />
        </div>
        <div class="form-group">
            <label>Customer ID (UUID) <span class="text-danger">*</span></label>
            <asp:TextBox ID="txtCustomerId" runat="server" CssClass="form-control"
                         placeholder="578f3f6f-14db-4adf-9c02-fdad454273ea" />
        </div>
        <div class="form-group">
            <label>API Key <span class="text-danger">*</span></label>
            <asp:TextBox ID="txtApiKey" runat="server" CssClass="form-control"
                         placeholder="ck_..." TextMode="Password" />
            <small class="form-text text-muted">Giá trị hiện tại sẽ không hiển thị. Để trống nếu không đổi.</small>
        </div>
        <div class="form-group">
            <label>Webhook Secret (HMAC-SHA256)</label>
            <asp:TextBox ID="txtWebhookSecret" runat="server" CssClass="form-control"
                         TextMode="Password" placeholder="(tuỳ chọn)" />
            <small class="form-text text-muted">Khớp với secret đã cấu hình trên DeviceManager. Để trống nếu không dùng xác thực chữ ký.</small>
        </div>
        <div class="form-check mb-3">
            <asp:CheckBox ID="chkEnabled" runat="server" CssClass="form-check-input" Checked="true" />
            <label class="form-check-label" for='<%= chkEnabled.ClientID %>'>Bật tích hợp DeviceManager</label>
        </div>
    </div>

    <!-- SMTP Email alert -->
    <div class="card">
        <p class="section-title">Email thông báo bất thường</p>
        <div class="form-row">
            <div class="form-group col-md-6">
                <label>SMTP Host</label>
                <asp:TextBox ID="txtSmtpHost" runat="server" CssClass="form-control" placeholder="smtp.gmail.com" />
            </div>
            <div class="form-group col-md-3">
                <label>SMTP Port</label>
                <asp:TextBox ID="txtSmtpPort" runat="server" CssClass="form-control" placeholder="587" />
            </div>
            <div class="form-group col-md-3">
                <label>SSL</label><br />
                <asp:CheckBox ID="chkSmtpSsl" runat="server" CssClass="form-check-input mt-2" Checked="true" />
                <label class="form-check-label ml-1">Dùng SSL/TLS</label>
            </div>
        </div>
        <div class="form-row">
            <div class="form-group col-md-6">
                <label>Tài khoản gửi</label>
                <asp:TextBox ID="txtSmtpUser" runat="server" CssClass="form-control" placeholder="alert@company.com" />
            </div>
            <div class="form-group col-md-6">
                <label>Mật khẩu</label>
                <asp:TextBox ID="txtSmtpPass" runat="server" CssClass="form-control" TextMode="Password" />
            </div>
        </div>
        <div class="form-group">
            <label>Email nhận thông báo <small class="text-muted">(nhiều địa chỉ cách nhau bởi dấu phẩy)</small></label>
            <asp:TextBox ID="txtAlertEmails" runat="server" CssClass="form-control"
                         placeholder="hr@company.com,security@company.com" />
        </div>
        <div class="form-row">
            <div class="form-group col-md-4">
                <label>Ngưỡng độ tin cậy khuôn mặt</label>
                <div class="input-group">
                    <asp:TextBox ID="txtFaceThreshold" runat="server" CssClass="form-control" Text="0.6" />
                    <div class="input-group-append"><span class="input-group-text">0–1</span></div>
                </div>
                <small class="form-text text-muted">Cảnh báo khi confidence &lt; ngưỡng này</small>
            </div>
        </div>
        <div class="form-group">
            <asp:Button ID="btnTestEmail" runat="server" Text="Gửi email test"
                        CssClass="btn btn-sm btn-outline-secondary" OnClick="BtnTestEmail_Click" />
        </div>
    </div>

    <asp:Button ID="btnSave" runat="server" Text="Lưu cấu hình"
                CssClass="btn btn-primary" OnClick="BtnSave_Click" />
    <a href="DeviceSync.aspx" class="btn btn-secondary ml-2">Quay lại</a>
</div>

</div>
</section>