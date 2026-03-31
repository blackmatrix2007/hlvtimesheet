<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_ConnectToDevice_faceRegistration.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_ConnectToDevice_faceRegistration" %>

<section class="content">    
<div class="box box-primary">
      
     <div class="box-header with-border" style='background-color: #dd4b39 !important;' >
        <h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")){ %> Face Registration <%} else if (language.Equals("2")) { %> Đăng ký khuôn mặt nhân viên <%} %></b></h3>
    </div>

    <!-- Form upload -->
    <div class="box-body">        
        <div class="col-xs-12 col-sm-12 col-lg-12">
            <div class="row">
                <div class="col-xs-6 col-sm-4 col-lg-3">                      
                    <asp:DropDownList ID="ddlNhanVien" runat="server" CssClass="form-control" />
                </div>
                <div class="col-xs-6 col-sm-4 col-lg-3"> 
                    <label style="display:none;">Ảnh khuôn mặt (JPEG/PNG, tối đa 5 MB) <span class="text-danger">*</span></label>
                    <asp:FileUpload ID="fuAnh" runat="server" CssClass="form-control-file" accept="image/jpeg,image/png" />
                    <img id="imgPreview" class="preview-img mt-2" alt="preview" style="width:30%;"/>
                </div>
                <div class="col-xs-6 col-sm-4 col-lg-3"> 
                    
                </div>
                <div class="col-xs-6 col-sm-4 col-lg-3" style="text-align:right;">
                    <asp:Button ID="btnUpload" runat="server" Width="30%" Text="Đăng ký" CssClass="btn btn-primary" OnClick="BtnUpload_Click" /> &nbsp&nbsp&nbsp
                    <asp:Button ID="btnRefresh" runat="server" Text="Làm mới" CssClass="btn btn-info" OnClick="BtnRefresh_Click" />
                </div>
            </div>            
        </div>
        <div class="col-xs-12 col-sm-12 col-lg-12">
            <div class="row" style="margin-top:10px;">
                <asp:Label ID="lblResult" runat="server" />
             </div>
        </div>
        <div class="col-xs-12 col-sm-12 col-lg-12">
            <div class="row" style="margin-top:10px;">
                <!-- Hidden field để truyền mã NV cần xóa -->
                    <asp:HiddenField ID="hdnRemoveCode" runat="server" />
                    <asp:Button ID="btnRemoveFace" runat="server" Text="" Style="display:none" OnClick="BtnRemoveFace_Click" />
            </div>
        </div>
    </div>

    <!-- Danh sách đã đăng ký -->
    <div class="box-body table-responsive no-padding">        
          <asp:Label ID="lblRemoveResult" runat="server" />
           <asp:Literal ID="litList" runat="server" />          
    </div>

</div>
</section>

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