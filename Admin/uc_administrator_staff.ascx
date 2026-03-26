<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_administrator_staff.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_administrator_staff" %>

<script type="text/javascript" >

    //PHAN TRANG MOI
    function moveTO(pageId) {

        //alert('Page ID: ' + pageId);
        window.location.replace("Administrator.aspx?func=95&pageNumber=" + pageId + "&search=" + getSearch() );
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
        window.location.replace("Administrator.aspx?func=95&pageNumber=" + pageId + "&search=" + getSearch() );
    }

        function getSearch() {

        var search = '';
        try {
            search += 'br__' + document.getElementById("<%= ddlChiNhanh.ClientID %>").value + ";;";
            search += 'off__' + document.getElementById("<%= ddlPhongBan.ClientID %>").value + ";;";
            search += 'pos__' + document.getElementById("<%= ddlChucVu.ClientID %>").value + ";;";
            search += 'cus__' + document.getElementById("<%= txtKhachHang.ClientID %>").value + ";;";
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
    <h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")){ %>List of staff <%} else if (language.Equals("2")) { %> Danh sách nhân sự <%} %></b></h3>
    </div>
    <div class="box-body">

     <div class="col-xs-12 col-sm-12 col-lg-12" style="text-align:center;"> 
        <img src="../Images/StaffInWorkType.jpg" />
    </div>

    <div class="col-xs-12 col-sm-12 col-lg-12" style="display:none;">
            <div class="row"> 
                  <div class="col-xs-6 col-sm-6 col-lg-3"> 
					<asp:DropDownList ID="ddlChiNhanh" runat="server" Width="100%" CssClass='form-control' ToolTip="search follow branch" 
                        AutoPostBack="True" onselectedindexchanged="ddlChiNhanh_SelectedIndexChanged" ></asp:DropDownList>
				</div>
                <div class="col-xs-6 col-sm-6 col-lg-3"> 
					<asp:DropDownList ID="ddlPhongBan" runat="server" Width="100%" CssClass='form-control' ToolTip="search follow department" 
                        AutoPostBack="True" onselectedindexchanged="ddlPhongBan_SelectedIndexChanged" ></asp:DropDownList>
				</div>
                 <div class="col-xs-6 col-sm-6 col-lg-3"> 
					<asp:DropDownList ID="ddlChucVu" runat="server" Width="100%" CssClass='form-control' ToolTip="search follow position" 
                        AutoPostBack="True" onselectedindexchanged="ddlChucVu_SelectedIndexChanged" ></asp:DropDownList>
				</div>                
                 <div class="col-xs-6 col-sm-6 col-lg-3"> 
					<input type="text" class="form-control" id="txtKhachHang" placeholder="search follow code, name or address of staff " runat="server" >
				</div>
            </div>
            <div class="row" style="margin-top:5px;">
                <div class="col-xs-12 col-sm-6 col-lg-3">  
					<asp:DropDownList ID="ddlTrangThai" runat="server" Width="100%" CssClass='form-control' ToolTip="search follow status" 
                        AutoPostBack="True" onselectedindexchanged="ddlTrangThai_SelectedIndexChanged" ></asp:DropDownList>
				</div>

				<div class="col-xs-12 col-sm-6 col-lg-9" style="text-align:right;" > 
                    <asp:LinkButton ID="lbTimkiem" runat="server" CssClass='btn btn-primary' onclick="lbTimkiem_Click" > 
                        <%if (language.Equals("1")){ %>  Search <%} else if (language.Equals("2")){%> Tìm kiếm  <%} %>
                    </asp:LinkButton>
                    <% if (quyen[2].Equals("1"))
                       { %> 
                    <a class="btn btn-success" href="Administrator.aspx?func=95&action=taomoi">
                        <%if (language.Equals("1")){ %>  Create new <%} else if (language.Equals("2")){%> Tạo mới <%} %>
                    </a>
                    <% } %>
                </div>
			</div>
        </div>
   </div>
				
	<div class="box-body table-responsive no-padding"  style="display:none;">
        <table class="table table-hover table-bordered table-striped" style="font-size:small">
        <tr>
            <%if (language.Equals("1")){ %>
			    <th style="text-align:center; width:3%;">#</th>
               <%-- <th style="text-align:center; width:8%;">Code </th>--%>
                <th style="text-align:center; width:12%;">Position</th>
                <th style="text-align:center; width:15%;">Name </th>
                <th style="text-align:center; width:10%;">Account </th>
                <th style="text-align:center; width:6%;">Branch </th>
                <th style="text-align:center; width:8%;">Department</th>
                <th style="text-align:center; width:8%;">Phone</th>
                <th style="text-align:center; width:6%;">Status</th>
			    <th style="text-align:center; width:8%;">Action</th>
                <% } else if (language.Equals("2")){  %>
                <th style="text-align:center; width:3%;">#</th>
                <%--<th style="text-align:center; width:8%;">Mã </th>--%>
                <th style="text-align:center; width:12%;">Chức vụ</th>
                <th style="text-align:center; width:15%;">Nhân sự</th>
                <th style="text-align:center; width:10%;">Tài khoản </th>
                <th style="text-align:center; width:6%;">Chi nhánh</th>
                <th style="text-align:center; width:8%;">Phòng ban</th>                
                <th style="text-align:center; width:8%;">Điện thoại</th>
                <th style="text-align:center; width:6%;">Trạng thái</th>
			    <th style="text-align:center; width:8%;">Quản trị</th>
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
