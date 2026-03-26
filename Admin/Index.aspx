<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Main.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" EnableEventValidation="false" Inherits="HLVTimeSheet.Admin.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript" src="https://www.google.com/jsapi"></script>
<%--<script type="text/javascript" src="jquery.min.js"></script>--%>
<script type="text/javascript" src="../Content/Scripts/jquery.gvChart-0.1.min.js"></script>
<script type="text/javascript">
gvChartInit();
jQuery(document).ready(function(){		

    jQuery('#myTable1').gvChart({
		chartType: 'PieChart',
		gvSettings: {
			vAxis: {title: 'No of players'},
			hAxis: {title: 'Month'},
			width: 500,
			height: 350,
			}
	});
			
    jQuery('#myTable2').gvChart({
        chartType: 'PieChart',
        gvSettings: {
            vAxis: { title: 'No of players' },
            hAxis: { title: 'Month' },
            width: 325,
            height: 150,
        }
    });

    jQuery('#myTable3').gvChart({
        chartType: 'PieChart',
        gvSettings: {
            vAxis: { title: 'No of players' },
            hAxis: { title: 'Month' },
            width: 325,
            height: 150,
        }
    });

    jQuery('#myTable4').gvChart({
        chartType: 'PieChart',
        gvSettings: {
            vAxis: { title: 'No of players' },
            hAxis: { title: 'Month' },
            width: 325,
            height: 150,
        }
    });

    jQuery('#myTable5').gvChart({
        chartType: 'PieChart',
        gvSettings: {
            vAxis: { title: 'No of players' },
            hAxis: { title: 'Month' },
            width: 325,
            height: 150,
        }
    });
});

</script>

<!-- Content Header (Page header) -->
<section class="content-header">

<h1>
<%if (language.Equals("1")){ %> Overview <%} else if (language.Equals("2")) { %>  Tổng quan  <%} %>
<small> <%if (language.Equals("1")){ %> Information system <%} else if (language.Equals("2")) { %>  Thông tin hệ thống  <%} %> </small>
</h1>
<ol class="breadcrumb" style="display:none;">
<li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
<li class="active">System</li>
</ol>

</section>

<!-- Main content -->
<div class="content">
    
    <% if (roleSys.Equals("1"))
        {  %>
    <div class="row" style="display:none;">
        <div class="col-xs-6 col-sm-6 col-lg-3">
        <!-- small box -->
        <div class="small-box bg-aqua">
        <div class="inner">
            <%--<h3>15</h3>--%>
            <asp:Literal ID="ltDiagramWarehouse" runat="server"></asp:Literal> 
                <p><%if (language.Equals("1")) { %>Layout Bonded Warehouse  <%} else if (language.Equals("2")) { %> Sơ đồ kho <%} %></p>
        </div>
        <div class="icon">
            <i class="ion ion-ios-grid-view"></i>
        </div>
        <a href="LayoutWarehouse.aspx" class="small-box-footer">
            
            <%if (language.Equals("1"))
                { %>Show details  <%}
                      else if (language.Equals("2"))
                      { %> Chi tiết <%} %>

            <i class="fa fa-arrow-circle-right"></i></a>
        </div>
    </div><!-- ./col -->
       
        <div class="col-xs-6 col-sm-6 col-lg-3" style="display:none;">
            <!-- small box -->
            <div class="small-box bg-aqua">
            <div class="inner">
                <%--<h3>15</h3>--%>
                <asp:Literal ID="Literal8" runat="server"></asp:Literal> 
                    <p><%if (language.Equals("1"))
                            { %>Layout Two Warehouse  <%}
                            else if (language.Equals("2"))
                            { %> Sơ đồ kho ngoại quan và kho thường <%} %></p>
            </div>
            <div class="icon">
                <i class="ion ion-ios-grid-view"></i>
            </div>
            <a href="LayoutBondedAndNormalWarehouse.aspx" class="small-box-footer">
        
                <%if (language.Equals("1"))
                    { %>Show details  <%}
                    else if (language.Equals("2"))
                    { %> Chi tiết <%} %>

                <i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div><!-- ./col -->

        <div class="col-xs-6 col-sm-6 col-lg-3">
            <!-- small box -->
            <div class="small-box bg-aqua">
            <div class="inner">
                <%--<h3>5,500</h3>--%>
                <asp:Literal ID="Literal9" runat="server"></asp:Literal>
                <p><%if (language.Equals("1")) { %> Layout Warehouse and General plan <%} else if (language.Equals("2")) { %> Sơ đồ kho và Theo dõi kế hoạch <%} %></p>
            </div>
            <div class="icon">
                <i class="ion ion-log-out"></i>
            </div>
            <a href="LayoutBondedAndGeneralPlan.aspx" class="small-box-footer">
               <%if (language.Equals("1"))
                   { %>Show details  <%}
                      else if (language.Equals("2"))
                      { %> Chi tiết <%} %>
                <i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div><!-- ./col -->

        <div class="col-xs-6 col-sm-6 col-lg-3">
            <!-- small box -->
            <div class="small-box bg-aqua">
            <div class="inner">
                <%--<h3>5,500</h3>--%>
                <asp:Literal ID="ltDiagramStockOut" runat="server"></asp:Literal>
                <p><%if (language.Equals("1"))
                       { %> General plan <%}
                      else if (language.Equals("2"))
                      { %> Theo dõi kế hoạch <%} %></p>
            </div>
            <div class="icon">
                <i class="ion ion-log-out"></i>
            </div>
            <a href="GeneralPlan.aspx" class="small-box-footer">
               <%if (language.Equals("1"))
                   { %>Show details  <%}
                      else if (language.Equals("2"))
                      { %> Chi tiết <%} %>
                <i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div><!-- ./col -->

        <div class="col-xs-6 col-sm-6 col-lg-3">
            <!-- small box -->
            <div class="small-box bg-aqua">
            <div class="inner">
                <%--<h3>5,500</h3>--%>
                <asp:Literal ID="Literal10" runat="server"></asp:Literal>
                <p><%if (language.Equals("1")){ %> Print label <%} else if (language.Equals("2")) { %> In tem <%} %></p>
            </div>
            <div class="icon">
                <i class="icon-qrcode"></i>
            </div>
            <a href="RePrinterLabel.aspx" class="small-box-footer">
               <%if (language.Equals("1")){ %>Show details  <%} else if (language.Equals("2")) { %> Chi tiết <%} %>
                <i class="fa fa-arrow-circle-right"></i></a>
            </div>
            </div><!-- ./col -->

        <div class="col-xs-6 col-sm-6 col-lg-3" style="display:none;">
        <!-- small box -->
        <div class="small-box bg-aqua">
        <div class="inner">
            <%--<h3>5,500</h3>--%>
            <asp:Literal ID="ltTrucking" runat="server"></asp:Literal>
            <p>Diagram Trucking</p>
        </div>
        <div class="icon">
            <i class="ion ion-ios-car-outline"></i>
        </div>
        <a href="DiagramTrucking.aspx" class="small-box-footer">
            <%if (language.Equals("1"))
                { %>Show details  <%}
                      else if (language.Equals("2"))
                      { %> Chi tiết <%} %>
            <i class="fa fa-arrow-circle-right"></i></a>
        </div>
        </div><!-- ./col -->

        <div class="col-xs-6 col-sm-6 col-lg-3" style="display:none;">
        <!-- small box -->
        <div class="small-box bg-aqua">
        <div class="inner">
            <%--<h3>5,500</h3>--%>
            <asp:Literal ID="Literal3" runat="server"></asp:Literal>
            <p> <%if (language.Equals("1"))
                    { %> StockOut Plan <%}
                       else if (language.Equals("2"))
                       { %> Kế hoạch xuất kho <%} %> </p>
        </div>
        <div class="icon">
            <i class="ion ion-ios-browsers-outline"></i>
        </div>
        <a href="StockOutPlan.aspx" class="small-box-footer">
            <%if (language.Equals("1"))
                { %>Show details  <%}
                      else if (language.Equals("2"))
                      { %> Chi tiết <%} %>
            <i class="fa fa-arrow-circle-right"></i></a>
        </div>
        </div><!-- ./col -->

    </div><!-- /.row --> 

    <div class="row" style="display:none;">

        <div class="col-xs-6 col-sm-6 col-lg-3"> </div>

        <div class="col-xs-6 col-sm-6 col-lg-3">
        <!-- small box -->
        <div class="small-box bg-aqua">
        <div class="inner">
            <%--<h3>15</h3>--%>
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            <p> <%if (language.Equals("1"))
                    { %> General plan <%}
                      else if (language.Equals("2"))
                      { %> Kế hoạch chung <%} %> </p>
        </div>
        <div class="icon">
            <i class="ion ion-ios-paper-outline"></i>
        </div>
        <a href="GeneralPlan.aspx" class="small-box-footer">
          <%if (language.Equals("1"))
              { %>Show details  <%}
                      else if (language.Equals("2"))
                      { %> Chi tiết <%} %>   
         <i class="fa fa-arrow-circle-right"></i></a>
        </div>
    </div><!-- ./col -->
       
        <div class="col-xs-6 col-sm-6 col-lg-3">
        <!-- small box -->
        <div class="small-box bg-aqua">
        <div class="inner">
            <asp:Literal ID="Literal2" runat="server"></asp:Literal>
            <p> <%if (language.Equals("1"))
                    { %> Delivery <%}
                  else if (language.Equals("2"))
                  { %> Danh sách giao hàng <%} %> </p>
        </div>
        <div class="icon">
            <i class="ion ion-ios-crop"></i>
        </div>
        <a href="DiagramDelivery.aspx" class="small-box-footer">
          <%if (language.Equals("1"))
              { %>Show details  <%}
                      else if (language.Equals("2"))
                      { %> Chi tiết <%} %>
         <i class="fa fa-arrow-circle-right"></i></a>
        </div>
    </div><!-- ./col -->
    
    </div><!-- /.row -->  

    <div class="row" style="display:none;">
        <div class="col-xs-12 col-sm-12 col-lg-6">
            <div class="box box-info">
            <div class="box-header with-border">
                 <%if (language.Equals("1"))
                     { %>
                <h3 class="box-title">Inventory</h3>
                 <%}
                     else if (language.Equals("2"))
                     { %>
                    <h3 class="box-title">Tổng kho</h3>
                 <%} %>
                <div class="box-tools pull-right">
                <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                </div>
            </div><!-- /.box-header -->
            <div class="box-body" >
			      <table id='myTable1'>
                      <%if (language.Equals("1"))
                          { %>
                        <caption>% Rate of customer car</caption>
                         <%}
                             else if (language.Equals("2"))
                             { %>
                            <caption>% Tỉ lệ bãi xe theo khách hàng</caption>
                         <%} %>
				    
				        <thead>
					        <tr>
						        <th></th>
                                <asp:Literal ID="ltTonKhoKhachHang" runat="server"></asp:Literal>
					        </tr>
				        </thead>
					    <tbody>
					    <tr>
						    <th></th>
                            <asp:Literal ID="ltTonKhoSoLuong" runat="server"></asp:Literal>
					    </tr>
				    </tbody>
			    </table>
            </div><!-- ./box-body -->
            
            </div><!-- /.box -->
        </div><!-- /.col -->
        <div class="col-xs-12 col-sm-12 col-lg-6">
            <div class="col-xs-12 col-sm-12 col-lg-6">
                <div class="box box-info">
                <div class="box-header with-border">
                    <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    </div>
                </div><!-- /.box-header -->
                <div class="box-body" >
			          <table id='myTable2'>
				        <caption>% Total</caption>
				            <thead>
					            <tr>
						            <th></th>
                                    <asp:Literal ID="ltTyLeKhachHangTrongKhoBai" runat="server"></asp:Literal>
					            </tr>
				            </thead>
					        <tbody>
					        <tr>
						        <th></th>
                                <asp:Literal ID="ltGiaTriTong" runat="server"></asp:Literal>
					        </tr>
				        </tbody>
			        </table>
                </div><!-- ./box-body -->
            
            </div>
            </div><!-- /.box -->
            <div class="col-xs-12 col-sm-12 col-lg-6">
                <div class="box box-info">
                <div class="box-header with-border">
                    <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    </div>
                </div><!-- /.box-header -->
                <div class="box-body" >
			          <table id='myTable3'>
				        <caption>% Hoang Nguyen</caption>
				            <thead>
					            <tr>
						            <th></th>
                                    <asp:Literal ID="ltKhachHang1" runat="server"></asp:Literal>
					            </tr>
				            </thead>
					        <tbody>
					        <tr>
						        <th></th>
                                <asp:Literal ID="ltTonKhoKH1" runat="server"></asp:Literal>
					        </tr>
				        </tbody>
			        </table>
                </div><!-- ./box-body -->
            
            </div>
            </div><!-- /.box -->
            <div class="col-xs-12 col-sm-12 col-lg-6">
                 <div class="box box-info">
                <div class="box-header with-border">
                    <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    </div>
                </div><!-- /.box-header -->
                <div class="box-body" >
			          <table id='myTable4'>
				        <caption>% Phuong Anh</caption>
				            <thead>
					            <tr>
						            <th></th>
                                    <asp:Literal ID="ltKhachHang2" runat="server"></asp:Literal>
					            </tr>
				            </thead>
					        <tbody>
					        <tr>
						        <th></th>
                                <asp:Literal ID="ltTonKhoKH2" runat="server"></asp:Literal>
					        </tr>
				        </tbody>
			        </table>
                </div><!-- ./box-body -->
            </div>
            </div>
            <div class="col-xs-12 col-sm-12 col-lg-6">
                 <div class="box box-info">
                <div class="box-header with-border">
                    <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    </div>
                </div><!-- /.box-header -->
                <div class="box-body" >
			          <table id='myTable5'>
				        <caption>% TMV</caption>
				            <thead>
					            <tr>
						            <th></th>
                                    <asp:Literal ID="ltKhachHang3" runat="server"></asp:Literal>
					            </tr>
				            </thead>
					        <tbody>
					        <tr>
						        <th></th>
                                <asp:Literal ID="ltTonKhoKH3" runat="server"></asp:Literal>
					        </tr>
				        </tbody>
			        </table>
                </div><!-- ./box-body -->
            </div>
            </div>
        </div>
    </div><!-- /.row -->

    <div class="row" style="display:none;">
        <div class="col-xs-6 col-sm-6 col-lg-3">
            <!-- small box -->
            <div class="small-box bg-teal">
            <div class="inner">
                <asp:Literal ID="Literal4" runat="server"></asp:Literal>
                <p> <%if (language.Equals("1"))
                        { %> Receiving and Shipping plan <%}
                                     else if (language.Equals("2"))
                                     { %> Kế hoạch xuất-nhập <%} %> </p>
            </div>
            <div class="icon">
                <i class="ion ion-ios-crop"></i>
            </div>
            <a href="Administrator.aspx?func=60" class="small-box-footer">
              <%if (language.Equals("1"))
                  { %>Show details  <%}
                      else if (language.Equals("2"))
                      { %> Chi tiết <%} %>
             <i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div><!-- ./col -->
        <div class="col-xs-6 col-sm-6 col-lg-3">
            <!-- small box -->
            <div class="small-box bg-teal">
            <div class="inner">
                <asp:Literal ID="Literal5" runat="server"></asp:Literal>
                <p> <%if (language.Equals("1"))
                        { %> Receiving <%}
                   else if (language.Equals("2"))
                   { %> Nhập hàng <%} %> </p>
            </div>
            <div class="icon">
                <i class="ion ion-ios-crop"></i>
            </div>
            <a href="StockIn.aspx?func=61" class="small-box-footer">
              <%if (language.Equals("1"))
                  { %>Show details  <%}
                      else if (language.Equals("2"))
                      { %> Chi tiết <%} %>
             <i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div><!-- ./col -->
        <div class="col-xs-6 col-sm-6 col-lg-3">
            <!-- small box -->
            <div class="small-box bg-teal">
            <div class="inner">
                <asp:Literal ID="Literal6" runat="server"></asp:Literal>
                <p> <%if (language.Equals("1"))
                        { %> Shipping <%}
                  else if (language.Equals("2"))
                  { %> Xuất hàng <%} %> </p>
            </div>
            <div class="icon">
                <i class="ion ion-ios-crop"></i>
            </div>
            <a href="StockOut.aspx?func=71" class="small-box-footer">
              <%if (language.Equals("1"))
                  { %>Show details  <%}
                      else if (language.Equals("2"))
                      { %> Chi tiết <%} %>
             <i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div><!-- ./col -->
        <div class="col-xs-6 col-sm-6 col-lg-3">
            <!-- small box -->
            <div class="small-box bg-teal">
            <div class="inner">
                <asp:Literal ID="Literal7" runat="server"></asp:Literal>
                <p> <%if (language.Equals("1"))
                        { %> Master controller <%}
                           else if (language.Equals("2"))
                           { %> Bảng điều khiển chính <%} %> </p>
            </div>
            <div class="icon">
                <i class="ion ion-ios-crop"></i>
            </div>
            <a href="Administrator.aspx?func=93" class="small-box-footer">
              <%if (language.Equals("1"))
                  { %>Show details  <%}
                      else if (language.Equals("2"))
                      { %> Chi tiết <%} %>
             <i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div><!-- ./col -->
    </div>

    <%} %>
</div>

<script type="text/javascript">
    $(window).on('load', function () {
        $('#renewModal').modal('show');
    });

</script>

</asp:Content>

