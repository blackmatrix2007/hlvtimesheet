<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_administrator_teamwork.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_administrator_teamwork" %>

<script type="text/javascript" >

    //PHAN TRANG MOI
    function moveTO(pageId) {

        //alert('Page ID: ' + pageId);
        window.location.replace("Administrator.aspx?func=97&pageNumber=" + pageId + "&search=" + getSearch() );
    }

    function moveTO2(flag1, flag2) {

        var pagedropdownNUMBER = document.getElementById('pagedropdownNUMBER');
        var pageId = pagedropdownNUMBER.value;

        //alert('FLAG 1: ' + flag1 + '  -- FLAG 2: ' + flag2 );

        if (flag1 == '-1') {  //PREV + FIST

            if (flag2 == '-1') //FIST
                pageId = 1;
            else
                pageId = parseInt(pageId) - 1;
        }
        else {   //NEXT + LAST

            if (flag2 == '-1') //NEXT
                pageId = parseInt(pageId) + 1;
            else   //TIM PAGE LAST
                pageId = document.getElementById('pagedropdownNUMBER_MAX').value;
        }

        //alert('Page ID: ' + pageId);
        window.location.replace("Administrator.aspx?func=97&pageNumber=" + pageId + "&search=" + getSearch() );
    }

        function getSearch() {

        var search = '';
        try {
            search += 'staff__' + document.getElementById("<%= ddlNhanSu.ClientID %>").value + ";;";
            search += 'name__' + document.getElementById("<%= txtTen.ClientID %>").value + ";;";
        }
        catch (err) {

            search = '';
        }

        return search;
    }

</script>

<!-- Main content -->
<section class="content">

<div class="box box-primary">
    <div class="box-header with-border" style='background-color: #dd4b39 !important;' >
    <h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")){ %>List of teamwork <%} else if (language.Equals("2")) { %> Danh sách nhóm làm việc <%} %></b></h3>
    </div>
    <div class="box-body">
    <div class="col-xs-12 col-sm-12 col-lg-12">
            <div class="row">                  
                 <div class="col-xs-6 col-sm-6 col-lg-4"> 
					<asp:DropDownList ID="ddlNhanSu" runat="server" Width="100%" CssClass='form-control' ToolTip="search follow staff" 
                        AutoPostBack="True" onselectedindexchanged="ddlNhanSu_SelectedIndexChanged" ></asp:DropDownList>
				</div>                
                 <div class="col-xs-6 col-sm-6 col-lg-4"> 
					<input type="text" class="form-control" id="txtTen" placeholder="search follow name " runat="server" >
				</div>
                <div class="col-xs-12 col-sm-12 col-lg-4" style="text-align:right;" > 
                    <asp:LinkButton ID="lbTimkiem" runat="server" CssClass='btn btn-primary' onclick="lbTimkiem_Click" > 
                        <%if (language.Equals("1")){ %>  Search <%} else if (language.Equals("2")){%> Tìm kiếm  <%} %>
                    </asp:LinkButton>
                    <% if (quyen[2].Equals("1"))
                       { %> 
                    <a class="btn btn-success" href="Administrator.aspx?func=97&action=taomoi">
                        <%if (language.Equals("1")){ %>  Create new <%} else if (language.Equals("2")){%> Tạo mới <%} %>
                    </a>
                    <% } %>
                </div>
            </div>           
        </div>
   </div>
				
	<div class="box-body table-responsive no-padding">
        <table class="table table-hover table-bordered table-striped" style="font-size:small">
        <tr>
            <%if (language.Equals("1")){ %>
			    <th style="text-align:center; width:3%;">#</th>
                <%--<th style="text-align:center; width:10%;">Code </th>--%>
                <th style="text-align:center; width:35%;">Name </th>
                <th style="text-align:center; width:25%;">Admin </th>
                <th style="text-align:center; width:8%;">Status</th>
			    <th style="text-align:center; width:12%;">Action</th>
                <% } else if (language.Equals("2")){  %>
                <th style="text-align:center; width:3%;">#</th>
                <%--<th style="text-align:center; width:10%;">Mã </th>--%>
                <th style="text-align:center; width:35%;">Tên</th>
                <th style="text-align:center; width:25%;">Người quản trị</th>
                <th style="text-align:center; width:8%;">Trạng thái</th>
			    <th style="text-align:center; width:12%;">Quản trị</th>
                <%} %>

        </tr>

        <asp:Literal ID="ltData" runat="server"></asp:Literal>
       
        </table>
    </div><!-- /.box-body -->

     <div class="box-footer clearfix">        
        <asp:Literal ID="ltPhanTrang" runat="server"></asp:Literal>
    </div>
    
    </div><!-- /.box-body -->

<script type="text/javascript" >
    
</script>

</section>

