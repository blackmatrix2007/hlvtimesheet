<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_administrator_rolepermission.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_administrator_rolepermission" %>

<!-- Main content -->
<section class="content">

<div class="box box-primary">
    <div class="box-header with-border" style='background-color: #dd4b39 !important;' >
    
    <h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")){ %>List of role <%} else if (language.Equals("2")) { %> Danh sách nhóm quyền <%} %></b></h3>
    </div>

    <div class="box-body">
     <div class="col-xs-12 col-sm-12 col-lg-12"> 
            <div class="row">
                <div class="col-xs-12 col-sm-8 col-lg-10">  
					<input type="text" class="form-control" id="txtTendonvi" placeholder="search follow role permission" runat="server" >
				</div>
				
				<div class="col-xs-12 col-sm-4 col-lg-2" style="text-align:right;" > 
                     <asp:LinkButton ID="lbTimkiem" runat="server" CssClass='btn btn-primary' 
                        onclick="lbTimkiem_Click" ><span class="glyphicon glyphicon-search" aria-hidden="true"></span> 
                        <%if (language.Equals("1")){ %>  Search <%} else if (language.Equals("2")){%> Tìm kiếm  <%} %>
                    </asp:LinkButton>
                    <% if (quyen[2].Equals("1"))
                       { %> 
                    <a class="btn btn-success" href="Administrator.aspx?func=91&action=taomoi"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span> 
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
                <th style="text-align:center; width:15%;">Code</th>
                <th style="text-align:center; width:22%;">Name</th>
                <th style="text-align:center; width:8%;">Status</th>           
                <th style="text-align:center; width:12%;">Updated date</th>
                <th style="text-align:center; width:14%;">Updated by</th>
			    <th style="text-align:center; width:12%;">Action</th>
                <% } else if (language.Equals("2")){  %>
                    <th style="text-align:center; width:3%;">#</th>
                    <th style="text-align:center; width:15%;">Mã</th>
                    <th style="text-align:center; width:22%;">Nhóm quyền</th>
                    <th style="text-align:center; width:8%;">Trạng thái</th>           
                    <th style="text-align:center; width:12%;">Ngày sửa</th>
                    <th style="text-align:center; width:14%;">Ngày cập nhật</th>
			        <th style="text-align:center; width:12%;">Quản trị</th>
                <%} %>
        </tr>

        <asp:Literal ID="ltData" runat="server"></asp:Literal>
       
        </table>
    </div><!-- /.box-body -->

</div>	
</section><!-- /.content -->

