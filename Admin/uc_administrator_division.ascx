<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_administrator_division.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_administrator_division" %>

<!-- Main content -->
<section class="content">

<div class="box box-primary">
    <div class="box-header with-border" style='background-color: #dd4b39 !important;' >
    <h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")){ %>Information of division <%} else if (language.Equals("2")) { %> Thông tin bộ phận <%} %></b></h3>
    </div>
    <div class="box-body">
                
	<div class="col-xs-12" > 
            <div class="row">
                <div class="col-xs-10">      
					<input type="text" class="form-control" id="txtTenLocation" placeholder="Seach follow department " runat="server" >
				</div>
                
                <div class="col-xs-12 col-sm-4 col-lg-2" style="text-align:right;" > 
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
                <th style="text-align:center; width:8%;">Code</th>               
                <th style="text-align:center; width:16%;">Division</th>
                <th style="text-align:center; width:6%;">Status</th>
                <th style="text-align:center; width:10%;">Updated date</th>
                <th style="text-align:center; width:12%;">Updated by</th>
			    <th style="text-align:center; width:12%;">Action</th>
                <% } else if (language.Equals("2")){  %>                
			    <th style="text-align:center; width:3%;">#</th>
                <th style="text-align:center; width:8%;">Mã</th> 
                <th style="text-align:center; width:16%;">Bộ phận</th> 
                <th style="text-align:center; width:6%;">Trạng thái</th>
                <th style="text-align:center; width:10%;">Ngày sửa</th>
                <th style="text-align:center; width:12%;">Người sửa</th>
			    <th style="text-align:center; width:12%;">Quản trị</th>
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
			<h4 class="modal-title" id="exampleModalLabel"><%if (language.Equals("1")){ %>Information of department detail <%} else if (language.Equals("2")) { %> Thông tin chi tiết <%} %></h4>
			</div>
			<div class="modal-body">
			<form>
				<div class="form-group">
				<label for="recipient-name" class="control-label"><%if (language.Equals("1")){ %> Code <%} else if (language.Equals("2")) { %> Mã <%} %> :</label>
				<input type="text" class="form-control" id="txtMa">
                <input type="hidden" class="form-control" id="txtId">
				</div>
				<div class="form-group">
				<label for="message-text" class="control-label"><%if (language.Equals("1")){ %> Division <%} else if (language.Equals("2")) { %> Bộ phận <%} %>:</label>
                <input type="text" class="form-control" id="txtTen">
				</div>
                <div class="form-group">
				<label for="recipient-name" class="control-label"><%if (language.Equals("1")){ %> Status <%} else if (language.Equals("2")) { %> Trạng thái <%} %>: &nbsp;&nbsp;</label>
				<input type="checkbox" id="txtTrangthai" > <%if (language.Equals("1")){ %> Action <%} else if (language.Equals("2")) { %> Hoạt động <%} %>
				</div>
			</form>
			</div>
			
            <div id='control01' class="modal-footer">
			    <button type="button" class="btn btn-default" data-dismiss="modal"><%if (language.Equals("1")){ %> Close <%} else if (language.Equals("2")) { %> Đóng lại <%} %></button>
			    <button type="button" class="btn btn-primary" onclick='javascript:LuuThongTin();' id='btnLuuthongtin' ><%if (language.Equals("1")){ %> Save <%} else if (language.Equals("2")) { %> Lưu thông tin <%} %></button>
            </div>

            <div id='loading01' class="box box-danger box-solid" style='display:none;' >
                <div class="box-header">
                    <h3 class="box-title">Processing request</h3>
                </div>
                <div class="box-body">
                    The system was processing your request. Please wait at the moment.
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

        var id = document.getElementById("txtId").value;
        var ma = document.getElementById("txtMa").value;
        var ten = document.getElementById("txtTen").value;
        var trangthai = 0;

        if (ma == '') {
            <%if (language.Equals("1")){ %> alert('Please, you must to input code!'); <%} else if (language.Equals("2")){ %> alert('Vui lòng nhập mã'); <%} %>
            return;
        }

        if (ten == '') {
            <%if (language.Equals("1")){ %> alert('Please, you must to input department!'); <%} else if (language.Equals("2")){ %> alert('Vui lòng nhập chi nhánh'); <%} %>
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
        $.post("Hander/hdToChuc.ashx?action=UpdateDivision&id=" + id + "&ma=" + ma + "&ten=" + ten + "&trangthai=" + trangthai, function (result) {

            if (result == '') {

                if (id == '') {
                    <%if (language.Equals("1")){ %> alert('Created successfully'); <%} else if (language.Equals("2")){ %> alert('Tạo mới chi nhánh thành công!'); <%} %>
                }
                else {
                    <%if (language.Equals("1")){ %> alert('Updated successfully'); <%} else if (language.Equals("2")){ %> alert('Cập nhật chi nhánh thành công!'); <%} %>
                }
                    
                window.location.replace("Administrator.aspx?func=99");

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

        $.post("Hander/hdToChuc.ashx?action=GetInforDivision&id=" + id, function (result) {

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

                <%if (language.Equals("1")){ %>
                alert("Cannot load information of division. Please try again! ");
                 <%}
                else if (language.Equals("2")){ %>
                alert("Không thể tải thông tin bộ phận. Vui lòng thử lại! ");
                 <%} %>
            }

        });

    }
</script>

