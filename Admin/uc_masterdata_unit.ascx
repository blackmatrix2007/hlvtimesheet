<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_masterdata_unit.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_masterdata_unit" %>
<!-- Main content -->
<section class="content">

<div class="box box-primary">
    <div class="box-header with-border" style='background-color: #dd4b39 !important;' >
    <h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")){ %>List of unit <%} else if (language.Equals("2")) { %> Danh sách đơn vị tính <%} %></b></h3>
    </div>
    <div class="box-body">
    <div class="col-xs-12" > 
            <div class="row">
                <div class="col-xs-10"> 
					<input type="text" class="form-control" id="txtTendonvi" placeholder="search follow unit" runat="server" >
				</div>
				
				 <div class="col-xs-2" style="text-align:right"> 
                     <asp:LinkButton ID="lbTimkiem" runat="server" CssClass='btn btn-primary' onclick="lbTimkiem_Click"> 
                         <%if (language.Equals("1")){ %>  Search <%} else if (language.Equals("2")){%> Tìm kiếm  <%} %>
                     </asp:LinkButton>
                    <% if (quyen[2].Equals("1"))
                       { %> 
                    <button type="button" class="btn btn-success" data-toggle="modal" data-target="#exampleModal" data-whatever=""> 
                        <%if (language.Equals("1")){ %>  Create new <%} else if (language.Equals("2")){%> Tạo mới <%} %>
                    </button> 
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
                <th style="text-align:center; width:10%;">Code </th>
                <th style="text-align:center; width:25%;">Name </th>
                <th style="text-align:center; width:8%;">Status</th>
                <th style="text-align:center; width:12%;">Updated date</th>
                <th style="text-align:center; width:14%;">Updated by</th>
			    <th style="text-align:center; width:8%;">Action</th>
                <% } else if (language.Equals("2")){  %>
                <th style="text-align:center; width:3%;">#</th>
                <th style="text-align:center; width:10%;">Mã </th>
                <th style="text-align:center; width:25%;">Đơn vị</th>                
                <th style="text-align:center; width:8%;">Trạng thái</th>
                <th style="text-align:center; width:12%;">Ngày sửa</th>
                <th style="text-align:center; width:14%;">Người sửa</th>
			    <th style="text-align:center; width:8%;">Quản trị</th>
                <%} %>

        </tr>

        <asp:Literal ID="ltData" runat="server"></asp:Literal>
       
        </table>
    </div><!-- /.box-body -->

	<div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel">
		<div class="modal-dialog" role="document">
		<div class="modal-content">
			<div class="modal-header">
			<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
			<h4 class="modal-title" id="exampleModalLabel">Information of unit</h4>
			</div>
			<div class="modal-body">
			<form>
				<div class="form-group">
				<label for="recipient-name" class="control-label">Code:</label>
				<input type="text" class="form-control" id="txtMa">
                <input type="hidden" class="form-control" id="txtId">
				</div>
				<div class="form-group">
				<label for="message-text" class="control-label">Name:</label>
                <input type="text" class="form-control" id="txtTen">
				</div>
                <div class="form-group">
				<label for="recipient-name" class="control-label">Status: &nbsp;&nbsp;</label>
				<input type="checkbox" id="txtTrangthai" > Action
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
				
    </div><!-- /.box-body -->

</section><!-- /.content -->

<script type="text/javascript" >
    function LuuThongTin() {

        var id = document.getElementById("txtId").value;
        var ma = document.getElementById("txtMa").value;
        var ten = document.getElementById("txtTen").value;
        var trangthai = 0;

        if (ten == '') {
            alert('Please you must be input name');
            return;
        }

        if (ma == '') {
            alert('Please you must be input code');
            return;
        }

        if (document.getElementById("txtTrangthai").checked == true)
            trangthai = 1;

        //Multi click
        var divControl = document.getElementById('control01');
        var divLoading = document.getElementById('loading01');

        divControl.style.display = 'none';
        divLoading.style.display = '';

        //alert('STT: ' + stt);
        //CALL AJAX
        $.post("Hander/hdMasterData.ashx?action=updateUnit&id=" + id + "&ma=" + ma + "&ten=" + ten + "&trangthai=" + trangthai, function (result) {

            if (result == '') {

                //HidePopup();
                if (id == '')
                    alert("Created new successfully");
                else
                    alert("Updated successfully");

                window.location.replace("MasterData.aspx?func=8");

            }
            else {

                //HidePopup();

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
        //LAY ID
        if (recipient == '') {
            document.getElementById("txtId").value = '';
        }
        else {
            document.getElementById("txtId").value = recipient;
            LayThongTin(recipient);
        }

        var modal = $(this)
    })
</script>


<script type="text/javascript" >
    function LayThongTin(id) {

        //CALL AJAX
        $.post("Hander/hdMasterData.ashx?action=getInfo_Unit&id=" + id, function (result) {

            if (result != '') {

                var arr = result.split(" -- ");

                document.getElementById("txtId").value = id;
                document.getElementById("txtMa").value = arr[0];
                document.getElementById("txtTen").value = arr[1];

                if (arr[2] == '1')
                    document.getElementById("txtTrangthai").checked = true;
                else
                    document.getElementById("txtTrangthai").checked = false;
            }
            else {

                //HidePopup();

                alert("Cannot load information of unit. Please try again! ");
            }

        });

    }

    function ResetInfor() {
        document.getElementById("txtId").value = "";
        document.getElementById("txtMa").value = "";
        document.getElementById("txtTen").value = "";
        document.getElementById("txtTrangthai").checked = true;
    }

</script>

