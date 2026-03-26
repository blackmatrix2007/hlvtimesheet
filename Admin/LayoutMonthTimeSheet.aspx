<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Main.Master" AutoEventWireup="true" CodeBehind="LayoutMonthTimeSheet.aspx.cs" Inherits="HLVTimeSheet.Admin.LayoutMonthTimeSheet" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="box box-primary">
    <div class="box-header with-border" style='background-color: #dd4b39 !important;display: none;' >
    <h3 class="box-title" ><b style='color: #FFF;' >Bảng theo dõi hàng ngày</b></h3>
    </div>

    <div class="box-body">
    
        <div class="row" style="margin-left:0.1px;">
        	<div class="box-body table-responsive no-padding">
            <div style="width:99.6%; overflow:auto;" id="divGeneral" >
                <table style="width:99.6%" cellpadding="2" cellspacing="1" border="2px"></table>
            </div>
             </div><!-- /.box-body -->
        </div>
     </div>
</div><!-- /.box -->

<script type="text/javascript" >
  
    function GetInfoGeneral() {

        $.post("Hander/hdDiagramSystem.ashx?action=layoutMonthTimeSheet", function (result) {
            document.getElementById("divGeneral").innerHTML = result;
        });

        //setTimeout(GetInfoGeneral, 50000);
    }


    function autoLoad() {
        GetInfoGeneral();
        
        //setTimeout(autoLoad, 120000);
        setTimeout(autoLoad, 500000000);        
    }

    autoLoad();
   
    $(document).ready(function () {
        autoLoad();        
    });

</script>

</asp:Content>

