<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_administrator_teamwork_action.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_administrator_teamwork_action" %>

<style>
.switch {
  position: relative;
  display: inline-block;
  width: 60px;
  height: 34px;
}

.switch input { 
  opacity: 0;
  width: 0;
  height: 0;
}

.slider {
  position: absolute;
  cursor: pointer;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: #ccc;
  -webkit-transition: .4s;
  transition: .4s;
}

.slider:before {
  position: absolute;
  content: "";
  height: 26px;
  width: 26px;
  left: 4px;
  bottom: 4px;
  background-color: white;
  -webkit-transition: .4s;
  transition: .4s;
}

input:checked + .slider {
  background-color: #2196F3;
}

input:focus + .slider {
  box-shadow: 0 0 1px #2196F3;
}

input:checked + .slider:before {
  -webkit-transform: translateX(26px);
  -ms-transform: translateX(26px);
  transform: translateX(26px);
}

/* Rounded sliders */
.slider.round {
  border-radius: 34px;
}

.slider.round:before {
  border-radius: 50%;
}
</style>

<!-- Main content -->
<section class="content">
<input type="hidden" id="donhang_fk" name="donhang_fk" value="<%= id %>" />
<div class="box box-primary">
    <div class="box-header with-border" style='background-color: #dd4b39 !important;' >
    <h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")){ %>Information of teamwork <%} else if (language.Equals("2")) { %> Thông tin nhóm làm việc <%} %></b></h3>
    </div>
    <div class="box-body">
    <div class="col-xs-12" >
	
        <div class="row">
            <div class="col-xs-12"> 
                <a class="btn btn-primary" href="MonthlyWorkSchedule.aspx"><span class="glyphicon glyphicon-menu-left" aria-hidden="true"></span> 
                    <%if (language.Equals("1")){ %> Back <%} else if (language.Equals("2")) { %> Quay lại <%} %>
                </a>
                
                  <asp:LinkButton ID="lbLuuLai" runat="server" CssClass="btn btn-primary" onclick="lbLuuLai_Click"><span class="glyphicon glyphicon-floppy-save" aria-hidden="true"></span> 
                    <%if (language.Equals("1")){ %>Save <%} else if (language.Equals("2")) { %> Lưu thông tin <%} %>
                </asp:LinkButton>

                <br /><br />
                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
            </div>
        </div>
       
        <div class="row" style="margin-top:5px; display:none;">
            <div class="col-xs-2"><%if (language.Equals("1")){ %> Code <%} else if (language.Equals("2")) { %> Mã <%} %> (*) </div>
            <div class="col-xs-4">
                <asp:TextBox ID="txtMa" runat="server" Width="100%" CssClass='form-control'></asp:TextBox>
            </div>            
        </div>

        <div class="row" style="margin-top:5px;">
            <div class="col-xs-2"><%if (language.Equals("1")){ %> Admin <%} else if (language.Equals("2")) { %> Quản lý <%} %> (*) </div>
            <div class="col-xs-4">
                <asp:DropDownList ID="ddlQuanLy" runat="server" Width="100%" CssClass='form-control select2'></asp:DropDownList>
            </div>
            <div class="col-xs-2"> <label class='switch'><input type='checkbox' name='ckStatus' runat="server" id="ckStatus"><span class='slider round'></span></label></div>
        </div>

         <div class="row" style="margin-top:5px;">
            <div class="col-xs-2"><%if (language.Equals("1")){ %> Name <%} else if (language.Equals("2")) { %> Tên nhóm <%} %> (*) </div>
            <div class="col-xs-10">
                <asp:TextBox ID="txtTenNhom" runat="server" Width="100%" CssClass='form-control'></asp:TextBox>
            </div>
        </div>
        
         <div class="row" style="margin-top:5px;">
            <div class="col-xs-2"><%if (language.Equals("1")){ %> Member (*) <%} else if (language.Equals("2")) { %> Thành viên (*) <%} %> </div>
            <div class="col-xs-10">                
                <select id="ddlNhanSu" class='form-control select2 leaderMultiSelctdropdown' multiple runat="server" style="width:100%;"></select>
                <input type="hidden" class="form-control" id="txtNhanSu" runat="server" readonly="readonly" >
            </div>
        </div>

         <div class="row" style="margin-top:5px;">
            <div class="col-xs-2"><%if (language.Equals("1")){ %> Descriptions <%} else if (language.Equals("2")) { %> Mô tả <%} %> </div>
            <div class="col-xs-10">
                <asp:TextBox ID="txtGhiChu" runat="server" Width="100%" CssClass='form-control'></asp:TextBox>
            </div>
        </div>

    </div>
    </div><!-- /.box-body -->
</div><!-- /.box -->
</section><!-- /.content -->


<script type="text/javascript" >


    $(".leaderMultiSelctdropdown").val([<%= listNhanSu %>]);

    function GetInfoNhanSu() {
        document.getElementById("<%= txtNhanSu.ClientID %>").value = $(".leaderMultiSelctdropdown").val();
        setTimeout(GetInfoNhanSu, 3000);
    }
    GetInfoNhanSu();


    $(document).ready(function () {
        GetInfoNhanSu();
    });

</script>

