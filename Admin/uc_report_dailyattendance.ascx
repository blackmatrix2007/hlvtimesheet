<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_report_dailyattendance.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_report_dailyattendance" %>

<section class="content">

<div class="box box-primary">
     <div class="box-body">
    <div class="row">
        <div class="col-xs-10" id='control01' style='text-align:left;' > 
            <a class="btn btn-primary" href="Report.aspx"><span class="glyphicon glyphicon-menu-left" aria-hidden="true"></span> 
                    <%if (language.Equals("1")){ %> Back <%} else if (language.Equals("2")){ %>Quay lại<%} %>
            </a>

             <a class="btn btn-info"href="javascript:ExcelReport();"><span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span> 
                 <%if (language.Equals("1")){ %> Excel <%} else if (language.Equals("2")){ %> Excel <%} %>  
             </a>
        </div>
         
    </div>
         
    <div class="row" style="margin-top:5px; font-size:smaller;">
           
            <div class="col-xs-4"></div>

             <div class="col-xs-1">
                 <asp:TextBox ID="txtTuNgay" runat="server" CssClass='form-control datepicker'  AutoPostBack="true" ontextchanged="txtTuNgay_TextChanged" autocomplete="off"></asp:TextBox>
             </div>

              <div class="col-xs-1">
                  <asp:TextBox ID="txtDenNgay" runat="server" CssClass='form-control datepicker'  AutoPostBack="true" ontextchanged="txtDenNgay_TextChanged" autocomplete="off"></asp:TextBox>
              </div>
           
            <div class="col-xs-2">  
                <asp:DropDownList ID="ddlPhongBan" runat="server" AutoPostBack="True" onselectedindexchanged="ddlPhongBan_SelectedIndexChanged" CssClass='form-control select2' ></asp:DropDownList>
            </div>       
                   
            <div class="col-xs-4"></div>
        
        </div>

    <div class="row" style="margin-top:5px; font-size:small;">
        <div style="width:99%; margin-left:0.5%; margin-top:0%;" class="box-body table-responsive no-padding" id="divDetail">
        <table class="table table-hover table-bordered table-striped" style="font-size:smaller;">
            <asp:Literal ID="ltInfor" runat="server"></asp:Literal>
        </table>
    </div>            
    </div>
  </div>
</div><!-- /.box -->

<script type="text/javascript">

    function ExcelReport() {
        var tungay = document.getElementById("<%= txtTuNgay.ClientID %>").value;
        var denngay = document.getElementById("<%= txtDenNgay.ClientID %>").value;        
        var phongban = document.getElementById("<%= ddlPhongBan.ClientID %>").value;

        var url = 'Hander/hdReport.ashx?action=dailyattendanceReport&tungay=' + tungay + "&denngay=" + denngay + "&phongban=" + phongban;
        window.location.assign(url);

    }
</script>

</section><!-- /.content -->
