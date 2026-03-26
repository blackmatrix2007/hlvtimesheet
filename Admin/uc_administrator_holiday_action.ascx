<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_administrator_holiday_action.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_administrator_holiday_action" %>


<!-- Main content -->
<section class="content">

<div class="box box-primary">
    <div class="box-header with-border" style='background-color: #dd4b39 !important;' >
    <h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")){ %>Information of holiday <%} else if (language.Equals("2")) { %> Thông tin ngày nghỉ lễ <%} %></b></h3>
    </div>
    <div class="box-body">
    <div class="col-xs-12" >
	
        <div class="row">
        <div class="col-xs-12"> 
        <a class="btn btn-primary" href="Administrator.aspx?func=98"><span class="glyphicon glyphicon-menu-left" aria-hidden="true"></span> 
            <%if (language.Equals("1")){ %> Back <%} else if (language.Equals("2")) { %> Quay lại <%} %>
        </a>
        <asp:LinkButton ID="lbLuuLai" runat="server" CssClass="btn btn-primary" onclick="lbLuuLai_Click"><span class="glyphicon glyphicon-floppy-save" aria-hidden="true"></span> 
            <%if (language.Equals("1")){ %>Save <%} else if (language.Equals("2")) { %> Lưu thông tin <%} %>
        </asp:LinkButton>
        <br /><br />
            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
        </div>
        </div>

        <div class="row" style="margin-top:5px;">
            <div class="col-xs-2"><%if (language.Equals("1")){ %>From (*) <%} else if (language.Equals("2")) { %> Từ ngày (*) <%} %> </div>
            <div class="col-xs-4">
                <asp:TextBox ID="txtTuNgay" runat="server" Width="100%" CssClass='form-control datepicker' ></asp:TextBox>
            </div>
             <div class="col-xs-2"><%if (language.Equals("1")){ %>To (*) <%} else if (language.Equals("2")) { %> Đến ngày (*) <%} %> </div>
            <div class="col-xs-4">
                <asp:TextBox ID="txtDenNgay" runat="server" Width="100%" CssClass='form-control datepicker' ></asp:TextBox>
            </div>        
        </div>
     
        <div class="row" style="margin-top:5px;">
            <div class="col-xs-2"><%if (language.Equals("1")){ %>Description (*) <%} else if (language.Equals("2")) { %> Nội dung <%} %> </div>
            <div class="col-xs-10">
                <asp:TextBox ID="txtNoiDung" runat="server" Width="100%" CssClass='form-control' ></asp:TextBox>
            </div>
        </div>

        <div class="row" style="margin-top:5px;">
            <div class="col-xs-2">Type </div>
            <div class="col-xs-4">
                <asp:DropDownList ID="ddlLoai" runat="server" Width="100%" CssClass='form-control' ></asp:DropDownList>
            </div>
            <div class="col-xs-2"> <%if (language.Equals("1")){ %> Status <%} else if (language.Equals("2")) { %> Trạng thái <%} %> </div>
            <div class="col-xs-4">
                <input type="checkbox" id="chkTrangThai" name="chkTrangThai" runat="server" class='minimal-red' /> <i> Action </i>
            </div>
        </div>

        
    </div>
    </div><!-- /.box-body -->
</div><!-- /.box -->
</section><!-- /.content -->


<script type="text/javascript" >

</script>
