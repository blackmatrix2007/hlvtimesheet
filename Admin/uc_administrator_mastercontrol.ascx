<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_administrator_mastercontrol.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_administrator_mastercontrol" %>

<section>
<div class="box box-primary">
<div class="box-header with-border" style='background-color: #dd4b39 !important;' >
<h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")){ %> Master controller <%} else if (language.Equals("2")) { %> Bảng điều khiển chính <%} %></b></h3>
</div>
<div class="box-body">
    <div class="row" style="margin-top:5px;">
        <div class="col-xs-12">
            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </div>

    <div class="row">
    <div class="col-xs-12 col-sm-12 col-lg-12"> 
        <div class="box box-info collapsed-box">
            <div class="box-header with-border">
            <h3 class="box-title"><%if (language.Equals("1")){ %> Configuration customer use location <%} else if (language.Equals("2")) { %> Thiết lập khách hàng sử dụng vị trí <%} %></h3>
            <div class="box-tools pull-right">
            <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-plus"></i></button>
            </div>
            </div><!-- /.box-header -->
             <div class="box-body">            
                  <div class="row" style="margin-top:5px;">
                       <div style="width:100%; overflow:auto;" id="divLocation" >
                        <table style="width:100%" cellpadding="2" cellspacing="1" border="2px"></table>
                        </div>
                  </div>
                 
             </div>
        </div>
        </div>
    </div>
</div>
</div>
<script type="text/javascript" > 

    function SaveConfigurationLocation(pallet_fk, line, sanpham_fk, loai) {
        $.post("Hander/hdToChuc.ashx?action=UpdateLocationConfigPallet&line=" + line + "&pallet=" + pallet + "&sanpham_fk=" + sanpham_fk + "&loai=" + loai, function (result) {
            if (result.length < 10) {              
            }
            else {
                alert(result);
                
            }

            GetInfoLocation();
        });
    }

    function GetInfoLocation() {

        $.post("Hander/hdDiagramSystem.ashx?action=GetInfoLocation", function (result) {
            document.getElementById("divLocation").innerHTML = result;
        });
    }

    $(document).ready(function () {

        GetInfoLocation();
    });

</script>

</section>