<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_masterdata_worktype.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_masterdata_worktype" %>


       <style>
        .switch {
          position: relative;
          display: inline-block;
          width: 45px;
          height: 20px;
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
          height: 12px;
          width: 12px;
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

<script type="text/javascript" >

    //PHAN TRANG MOI
    function moveTO(pageId) {

        //alert('Page ID: ' + pageId);
        window.location.replace("MasterData.aspx?func=5&pageNumber=" + pageId + "&search=" + getSearch() );
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
        window.location.replace("MasterData.aspx?func=5&pageNumber=" + pageId + "&search=" + getSearch() );
    }

    function getSearch() {

        var search = '';
        try {

            search += 'sp__' + document.getElementById("<%= txtSanPham.ClientID %>").value + ";;";
            search += 'gr__' + document.getElementById("<%= ddlNhomSanPham.ClientID %>").value + ";;";
            search += 'st__' + document.getElementById("<%= ddlTrangThai.ClientID %>").value + ";;";
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
    <h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")){ %>List of item<%} else if (language.Equals("2")) { %> Danh sách sản phẩm <%} %></b></h3>
    </div>

    <div class="box-body">

    <div class="col-xs-12 col-sm-12 col-lg-12"> 
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-lg-3">  
                    <asp:TextBox ID="txtSanPham" runat="server" placeholder="search follow item" CssClass='form-control' ToolTip="search follow item"  ></asp:TextBox>
			    </div>

                <div class="col-xs-12 col-sm-6 col-lg-2"> 
					<asp:DropDownList  ID="ddlNhomSanPham" runat="server" CssClass='form-control select2' data-toggle="tooltip" title="Search follow group"  ></asp:DropDownList>
				</div>

                 <div class="col-xs-12 col-sm-6 col-lg-2"> 
					<asp:DropDownList  ID="ddlTrangThai" runat="server" CssClass='form-control' data-toggle="tooltip" title="Search follow status"  ></asp:DropDownList>
				</div>

				<div class="col-xs-12 col-sm-12 col-lg-5" style="text-align:right">  
                    <asp:LinkButton ID="lbTimkiem" runat="server" CssClass='btn btn-primary' 
                        onclick="lbTimkiem_Click" ><span class="glyphicon glyphicon-search" aria-hidden="true"></span> 
                        <%if (language.Equals("1")){ %>Search <%} else if (language.Equals("2")) { %> Tìm kiếm <%} %>
                    </asp:LinkButton>
                    <button type="button" class="btn btn-info" onclick='javascript:XuatFileExcel();' ><span class="glyphicon glyphicon-save" aria-hidden="true"></span> Excel</button>

                     <button type="button" class="btn btn-success" data-toggle="modal" data-target="#exampleModal" data-whatever=""> 
                         <%if (language.Equals("1")){ %> New group <%} else if (language.Equals("2")){%> Tạo nhóm <%} %>
                     </button> 

                    <% if (quyen[2].Equals("1"))
                       { %>
                    <a class="btn btn-success" href="MasterData.aspx?func=5&action=taomoi"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span> 
                        <%if (language.Equals("1")){ %>Create new<%} else if (language.Equals("2")) { %> Tạo mới <%} %>
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
                <th style="text-align:center; width:6%;">Group</th>            
                <th style="text-align:center; width:8%;">Short name</th>      
                <th style="text-align:center; width:25%;">Detail name</th>            
                <th style="text-align:center; width:8%;">Code</th>
                <th style="text-align:center; width:6%;">Unit</th>            
                <th style="text-align:center; width:6%;">Cycle time</th>
                <th style="text-align:center; width:6%;">Cycle target</th>
                <th style="text-align:center; width:6%;">Price</th> 
                <th style="text-align:center; width:8%;">Customer list</th> 
			    <th style="text-align:center; width:12%;">Action</th>
            <% } else if (language.Equals("2")){  %>                
                <th style="text-align:center; width:3%;">#</th>                
                <th style="text-align:center; width:6%;">Group</th>            
                <th style="text-align:center; width:8%;">Short name</th>      
                <th style="text-align:center; width:25%;">Detail name</th>            
                <th style="text-align:center; width:8%;">Code</th>
                <th style="text-align:center; width:6%;">Unit</th>            
                <th style="text-align:center; width:6%;">Cycle time</th>
                <th style="text-align:center; width:6%;">Cycle target</th>
                <th style="text-align:center; width:6%;">Price</th> 
                <th style="text-align:center; width:8%;">Customer list</th> 
                <th style="text-align:center; width:12%;">Action</th>
            <%} %>

        </tr>

        <asp:Literal ID="ltData" runat="server"></asp:Literal>
       
        </table>
    </div><!-- /.box-body -->

    <div class="box-footer clearfix">
        
        <asp:Literal ID="ltPhanTrang" runat="server"></asp:Literal>

    </div>	

    <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel">
	    <div class="modal-dialog" role="document">
	<div class="modal-content">
		<div class="modal-header">
		<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
		<h4 class="modal-title" id="exampleModalLabel">Information of group</h4>
		</div>
		<div class="modal-body">
		<form>
            <div class="row" style="margin-top:10px;">			 
                <div class="col-xs-3 col-sm-3 col-lg-3"> 
			        Code:
                </div>
                <div class="col-xs-9 col-sm-9 col-lg-9"> 
			        <input type="text" class="form-control" id="txtMa" runat="server" readonly="readonly">
                    <input type="hidden" class="form-control" id="txtId">
                </div>			
            </div>

             <div class="row" style="margin-top:10px;display:none;">     
                <div class="col-xs-3 col-sm-3 col-lg-3"> 
                    Name:
                </div>
                <div class="col-xs-9 col-sm-9 col-lg-9"> 
                    <input type="text" class="form-control" id="txtTen" runat="server">
                </div>       
            </div>

            <div class="row" style="margin-top:10px;">               
                <div class="col-xs-3 col-sm-3 col-lg-3"> 
                    Bảng mã màu:
                </div>
                <div class="col-xs-9 col-sm-9 col-lg-9"> 
                    <asp:DropDownList  ID="ddlBangmaMau" runat="server" CssClass='form-control select2' data-toggle="tooltip" title="Select color" Width="100%" ></asp:DropDownList>
                </div>           
            </div>

             <div class="row" style="margin-top:10px;">                
                 <div class="col-xs-3 col-sm-3 col-lg-3"> 
                     Status:
                 </div>
                 <div class="col-xs-9 col-sm-9 col-lg-9"> 
                     <input type="checkbox" id="txtTrangthai" runat="server"> Action
                 </div>       
            </div>
		</form>
		</div>
			
        <div id='control01' class="modal-footer">
			<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
			<button type="button" class="btn btn-primary" onclick='javascript:LuuThongTin();' id='btnLuuthongtin' >Save</button>
        </div>

        <div id='loading01' class="box box-danger box-solid" style='display:none;' >
            <div class="box-header">
                <h3 class="box-title">Processing request</h3>
            </div>
            <div class="box-body">
                The system is processing your request. Please wait a moment.
            </div><!-- /.box-body -->
            <!-- Loading (remove the following to stop the loading)-->
            <div class="overlay">
                <i class="fa fa-refresh fa-spin"></i>
            </div>
            <!-- end loading -->
        </div><!-- /.box -->

	</div>
	</div>
    </div>
    
    <div class="modal fade" id="exampleModalAddGroup" tabindex="-1" role="dialog" aria-labelledby="exampleModalAddGroupLabel">
        <div class="modal-dialog" role="document">
        <div class="modal-content">
	    <div class="modal-header">
	    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
	    <h4 class="modal-title" id="exampleModalAddGroupLabel">Add of group</h4>
	    </div>
	    <div class="modal-body">
	    <form>           

             <div class="row" style="margin-top:10px;">			 
                 <div class="col-xs-3 col-sm-3 col-lg-3"> 
	                    Short name:
                 </div>
                 <div class="col-xs-9 col-sm-9 col-lg-9"> 
	                    <input type="text" class="form-control" id="txtShortName" runat="server" readonly="readonly">                     
                 </div>			
             </div>

             <div class="row" style="margin-top:10px;">			 
                 <div class="col-xs-3 col-sm-3 col-lg-3"> 
	                    Detail name
                 </div>
                 <div class="col-xs-9 col-sm-9 col-lg-9"> 
	                    <input type="text" class="form-control" id="txtDetailName" runat="server" readonly="readonly">                     
                 </div>			
             </div>

             <div class="row" style="margin-top:10px;">			 
                 <div class="col-xs-3 col-sm-3 col-lg-3"> 
	                    Code:
                 </div>
                 <div class="col-xs-9 col-sm-9 col-lg-9"> 
	                    <input type="text" class="form-control" id="txtCode" runat="server" readonly="readonly">                     
                 </div>			
             </div>

            <div class="row" style="margin-top:10px;">               
                <div class="col-xs-3 col-sm-3 col-lg-3"> 
                    Group item
                </div>
                <div class="col-xs-9 col-sm-9 col-lg-9"> 
                    <asp:DropDownList  ID="ddlAddGroup" runat="server" CssClass='form-control select2' data-toggle="tooltip" title="Select color" Width="100%" ></asp:DropDownList>
                    <input type="hidden" class="form-control" id="txtIdSP">
                </div>           
            </div>            
	    </form>
	    </div>
		
        <div id='control02' class="modal-footer">
		    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
		    <button type="button" class="btn btn-primary" onclick='javascript:LuuThongTinAddGroup();' id='btnLuuthongtinAddGroup' >Save</button>
        </div>

        <div id='loading02' class="box box-danger box-solid" style='display:none;' >
            <div class="box-header">
                <h3 class="box-title">Processing request</h3>
            </div>
            <div class="box-body">
                The system is processing your request. Please wait a moment.
            </div><!-- /.box-body -->
            <!-- Loading (remove the following to stop the loading)-->
            <div class="overlay">
                <i class="fa fa-refresh fa-spin"></i>
            </div>
            <!-- end loading -->
        </div><!-- /.box -->

        </div>
        </div>
    </div>

</div><!-- /.box -->
</section><!-- /.content -->


<script type="text/javascript" >
    function LuuThongTin() {

        var id = "0";
        var ma = document.getElementById("<%= txtMa.ClientID %>").value;
        var ten = document.getElementById("<%= txtTen.ClientID %>").value;
        var bangmamau = document.getElementById("<%= ddlBangmaMau.ClientID %>").value;
        var trangthai = 0;

        if (ten == '') {
            alert('Please you must be input name');
            return;
        }

        if (ma == '') {
            alert('Please you must be input code');
            return;
        }


        if (bangmamau.length < 3) {
            alert('Please you must be input color');
            return;
        }

        if (document.getElementById("<%= txtTrangthai.ClientID %>").checked == true)
            trangthai = 1;

        //Multi click
        var divControl = document.getElementById('control01');
        var divLoading = document.getElementById('loading01');

        divControl.style.display = 'none';
        divLoading.style.display = '';

        //alert('STT: ' + stt);
        //CALL AJAX
        $.post("Hander/hdMasterData.ashx?action=saveGroupItem&ma=" + ma + "&ten=" + ten + "&bangmamau=" + bangmamau + "&trangthai=" + trangthai, function (result) {

            if (result == '') {

                //HidePopup();
                if (id == '')
                    alert("Created new successfully");
                else
                    alert("Updated successfully");

                window.location.replace("MasterData.aspx?func=5");

            }
            else {

                //HidePopup();

                divControl.style.display = '';
                divLoading.style.display = 'none';

                alert(result);
            }

        });

    }

    function LuuThongTinAddGroup() {

        var sanpham_fk = document.getElementById("txtIdSP").value;        
        var nhomsanpham_fk = document.getElementById("<%= ddlAddGroup.ClientID %>").value;
        
        //Multi click
        var divControl = document.getElementById('control02');
        var divLoading = document.getElementById('loading02');

        divControl.style.display = 'none';
        divLoading.style.display = '';

        $.post("Hander/hdMasterData.ashx?action=saveAddGroupItem&sanpham_fk=" + sanpham_fk + "&nhomsanpham_fk=" + nhomsanpham_fk, function (result) {

            if (result == '') {

                alert("Updated successfully");
                window.location.replace("MasterData.aspx?func=5");

            }
            else {

                divControl.style.display = '';
                divLoading.style.display = 'none';

                alert(result);
            }
        });

    }

</script>

<script type="text/javascript" >
    $('#exampleModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget) // Button that triggered the modal
        var recipient = button.data('whatever') // Extract info FROM data-* attributes
        ResetInfor();
       
        var modal = $(this)
    })

    $('#exampleModalAddGroup').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget) // Button that triggered the modal
        var recipient = button.data('whatever') // Extract info FROM data-* attributes

        //LAY ID
        if (recipient == '') {
            document.getElementById("txtIdSP").value = '';
        }
        else {
            document.getElementById("txtIdSP").value = recipient;
        }

        loadInforItem_AddGroup(recipient);

        var modal = $(this)
    })
</script>


<script type="text/javascript" >
  
    function ResetInfor() {
        
        document.getElementById("<%= txtTrangthai.ClientID %>").checked = true;
    }

    function loadInforItem_AddGroup(sanpham_fk) {

        //CALL AJAX
        $.post("Hander/hdMasterData.ashx?action=getInfoItem_AddGroup&sanpham_fk=" + sanpham_fk, function (result) {

            if (result != '') {

                var arr = result.split(" -- ");

                document.getElementById("<%= txtShortName.ClientID %>").value = arr[0];
                document.getElementById("<%= txtDetailName.ClientID %>").value = arr[1];
                document.getElementById("<%= txtCode.ClientID %>").value = arr[2];
                document.getElementById("<%= ddlAddGroup.ClientID %>").value = arr[3];
            }
            else {

                //HidePopup();

                alert("Cannot load information of unit. Please try again! ");
            }

        });
    }

    function saveChangeCuctomer(pos) {

        var sanpham_fk = document.getElementsByName("sanpham_fk").item(pos).value;

        var showCustomer = "0";
        if (document.getElementsByName("ckCustom").item(pos).checked == true)
            showCustomer = "1";

        $.post("Hander/hdMasterData.ashx?action=saveShowCustomerItem&sanpham_fk=" + sanpham_fk + "&showCustomer=" + showCustomer, function (result) {

            if (result == '') {

                window.location.replace("MasterData.aspx?func=5");

            }
            else {

                alert(result);
            }
        });

    }
</script>


