<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeviceFaceRegister.aspx.cs" Inherits="HLVTimeSheet.Admin.DeviceFaceRegister" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>
<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Đăng ký khuôn mặt nhân viên</title>
    <link href="../Content/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body { padding: 20px; background: #f5f5f5; }
        .card { background: #fff; border-radius: 6px; padding: 20px; margin-bottom: 20px; box-shadow: 0 1px 3px rgba(0,0,0,.1); }
        .badge-ok  { background: #28a745; color: #fff; padding: 2px 8px; border-radius: 4px; font-size:.8em; }
        .badge-no  { background: #dc3545; color: #fff; padding: 2px 8px; border-radius: 4px; font-size:.8em; }
        .preview-img { max-width: 140px; max-height: 140px; border-radius: 6px; border: 2px solid #dee2e6; display:none; }
    </style>
</head>
<body>
<form id="form1" runat="server">
<div class="container-fluid">
    <h4 class="mb-3">Đăng ký khuôn mặt nhân viên — DeviceManager</h4>

    <!-- Form upload -->
    <div class="card">
        <h6>Upload ảnh khuôn mặt</h6>
        <div class="form-row align-items-end">
            <div class="form-group col-md-3">
                <label>Nhân viên <span class="text-danger">*</span></label>
                <asp:DropDownList ID="ddlNhanVien" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group col-md-4">
                <label>Ảnh khuôn mặt (JPEG/PNG, tối đa 5 MB) <span class="text-danger">*</span></label>
                <asp:FileUpload ID="fuAnh" runat="server" CssClass="form-control-file" accept="image/jpeg,image/png" />
                <img id="imgPreview" class="preview-img mt-2" alt="preview" />
            </div>
            <div class="form-group col-md-2">
                <asp:Button ID="btnUpload" runat="server" Text="Đăng ký lên DeviceManager"
                            CssClass="btn btn-primary btn-block" OnClick="BtnUpload_Click" />
            </div>
        </div>
        <asp:Label ID="lblResult" runat="server" />
    </div>

    <!-- Hidden field để truyền mã NV cần xóa -->
    <asp:HiddenField ID="hdnRemoveCode" runat="server" />
    <asp:Button ID="btnRemoveFace" runat="server" Text="" Style="display:none"
                OnClick="BtnRemoveFace_Click" />

    <!-- Danh sách đã đăng ký -->
    <div class="card">
        <div class="d-flex justify-content-between align-items-center mb-2">
            <h6 class="mb-0">Danh sách nhân viên &amp; trạng thái khuôn mặt</h6>
            <asp:Button ID="btnRefresh" runat="server" Text="Làm mới"
                        CssClass="btn btn-sm btn-secondary" OnClick="BtnRefresh_Click" />
        </div>
        <asp:Label ID="lblRemoveResult" runat="server" />
        <asp:Literal ID="litList" runat="server" />
    </div>
</div>
</form>

<script src="../Scripts/jquery-3.6.0.min.js"></script>
<script src="../Content/bootstrap/js/bootstrap.min.js"></script>
<script>
    function removeFace(code) {
        if (!confirm('Xóa khuôn mặt của ' + code + ' khỏi DeviceManager?')) return;
        document.getElementById('<%= hdnRemoveCode.ClientID %>').value = code;
        document.getElementById('<%= btnRemoveFace.ClientID %>').click();
    }

    // Preview ảnh trước khi upload
    document.getElementById('<%= fuAnh.ClientID %>').addEventListener('change', function () {
        var file = this.files[0];
        if (!file) return;
        var img = document.getElementById('imgPreview');
        img.src = URL.createObjectURL(file);
        img.style.display = 'block';
    });
</script>
</body>
</html>
