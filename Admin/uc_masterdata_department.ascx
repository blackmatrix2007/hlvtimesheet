<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_masterdata_department.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_masterdata_department" %>

<script type="text/javascript" >

    //PHAN TRANG MOI
    function moveTO(pageId) {

        //alert('Page ID: ' + pageId);
        window.location.replace("MasterData.aspx?func=7&pageNumber=" + pageId + "&search=" + getSearch() );
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
        window.location.replace("MasterData.aspx?func=7&pageNumber=" + pageId + "&search=" + getSearch() );
    }

    function getSearch() {

        var search = '';
        try {

            search += 'sp__' + document.getElementById("<%= txtSanPham.ClientID %>").value + ";;";
            search += 'cate__' + document.getElementById("<%= ddlNganhHang.ClientID %>").value + ";;";
            search += 'brand__' + document.getElementById("<%= ddlChungLoai.ClientID %>").value + ";;";
        }
        catch (err) {

            search = '';
        }

        return search;
    }

    function XuatFileExcel() {

        var url = 'Hander/hdToChuc.ashx?action=danhmuchanghoa';
        window.location.assign(url);
    }
    
</script>


<!-- Main content -->
<section class="content">

<div class="box box-primary">
    <div class="box-header with-border" style='background-color: #dd4b39 !important;' >
    <h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")){ %>List of department <%} else if (language.Equals("2")) { %> Danh sách phòng ban <%} %></b></h3>
    </div>

    <div class="box-body">

    <div class="col-xs-12 col-sm-12 col-lg-12"> 
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-lg-9">  
                    <asp:TextBox ID="txtSanPham" runat="server" 
                        placeholder="search follow department" CssClass='form-control' ToolTip="search follow department"  ></asp:TextBox>
			    </div>

                <div class="col-xs-12 col-sm-6 col-lg-3" style="display:none;"> 
					<asp:DropDownList  ID="ddlNganhHang" runat="server" CssClass='form-control' data-toggle="tooltip" title="Search follow department"  ></asp:DropDownList>
				</div>

                 <div class="col-xs-12 col-sm-6 col-lg-3" style="display:none;"> 
					<asp:DropDownList  ID="ddlChungLoai" runat="server" CssClass='form-control' data-toggle="tooltip" title="Search follow brand"  ></asp:DropDownList>
				</div>

				<div class="col-xs-12 col-sm-12 col-lg-3" style="text-align:right">  
                    <asp:LinkButton ID="lbTimkiem" runat="server" CssClass='btn btn-primary' 
                        onclick="lbTimkiem_Click" ><span class="glyphicon glyphicon-search" aria-hidden="true"></span> 
                        <%if (language.Equals("1")){ %>Search <%} else if (language.Equals("2")) { %> Tìm kiếm <%} %>
                    </asp:LinkButton>
                    <button type="button" class="btn btn-info" onclick='javascript:XuatFileExcel();' ><span class="glyphicon glyphicon-save" aria-hidden="true"></span> Excel</button>
                    <% if (quyen[2].Equals("1"))
                       { %>
                   <%-- <a class="btn btn-success" href="MasterData.aspx?func=7&action=taomoi"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span> 
                        <%if (language.Equals("1")){ %>Create new<%} else if (language.Equals("2")) { %> Tạo mới <%} %>
                    </a>--%>
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
            <th style="text-align:center; width:8%;">Code</th>            
            <th style="text-align:center; width:40%;">Department</th>
            <th style="text-align:center; width:8%;">Status</th>
            <th style="text-align:center; width:12%;">Updated date</th>
            <th style="text-align:center; width:12%;">Updated by</th>
			<th style="text-align:center; width:10%;">Action</th>
            <% } else if (language.Equals("2")){  %>                
			<th style="text-align:center; width:3%;">#</th>
            <th style="text-align:center; width:8%;">Mã</th>
            <th style="text-align:center; width:40%;">Phòng ban</th>
            <th style="text-align:center; width:8%;">Trạng thái</th>
            <th style="text-align:center; width:12%;">Ngày sửa</th>
            <th style="text-align:center; width:12%;">Người sửa</th>
			<th style="text-align:center; width:10%;">Quản trị</th>
            <%} %>

        </tr>

        <asp:Literal ID="ltData" runat="server"></asp:Literal>
       
        </table>
    </div><!-- /.box-body -->

    <div class="box-footer clearfix">
        
        <asp:Literal ID="ltPhanTrang" runat="server"></asp:Literal>

    </div>	

</div><!-- /.box -->
</section><!-- /.content -->


<script type="text/javascript" >

  

</script>


