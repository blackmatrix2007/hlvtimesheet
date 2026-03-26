<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeviceSync.aspx.cs" Inherits="HLVTimeSheet.Admin.DeviceSync" %>
<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Đồng bộ máy chấm công</title>
    <link href="../Content/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body { padding: 20px; background: #f5f5f5; }
        .card { background: #fff; border-radius: 6px; padding: 20px; margin-bottom: 20px; box-shadow: 0 1px 3px rgba(0,0,0,.1); }
        .badge-success { background: #28a745; color: #fff; }
        .badge-danger  { background: #dc3545; color: #fff; }
    </style>
</head>
<body>
<div class="container-fluid">
    <h4 class="mb-3"><i class="fa fa-sync"></i> Đồng bộ máy chấm công (DeviceManager)</h4>

    <!-- Kéo dữ liệu chấm công -->
    <div class="card">
        <h6>Kéo dữ liệu chấm công về HLVTimeSheet</h6>
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

    <!-- Trạng thái hôm nay -->
    <div class="card">
        <h6>Tổng hợp chấm công hôm nay</h6>
        <asp:Button ID="btnToday" runat="server" Text="Làm mới"
                    CssClass="btn btn-sm btn-secondary mb-2" OnClick="BtnToday_Click" />
        <asp:Literal ID="litToday" runat="server" />
    </div>

    <!-- Danh sách nhân viên trên DeviceManager -->
    <div class="card">
        <h6>Nhân viên đã đăng ký trên DeviceManager</h6>
        <asp:Button ID="btnListEmp" runat="server" Text="Tải danh sách"
                    CssClass="btn btn-sm btn-secondary mb-2" OnClick="BtnListEmp_Click" />
        <asp:Literal ID="litEmployees" runat="server" />
    </div>
</div>
<script src="../Scripts/jquery-3.6.0.min.js"></script>
<script src="../Content/bootstrap/js/bootstrap.min.js"></script>
</body>
</html>
