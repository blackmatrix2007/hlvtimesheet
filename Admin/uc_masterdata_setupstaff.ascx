<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_masterdata_setupstaff.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_masterdata_setupstaff" %>

<!-- Main content -->
<section class="content">

<input type="hidden" id="trangthaiSI" name="trangthaiSI" value="<%= trangthai %>" />
<div class="box box-primary">
    <div class="box-header with-border" style='background-color: #dd4b39 !important;'>
    <h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")){ %> Staff costs  <%} else if (language.Equals("2")) { %> Chi phí cho nhân viên <%} %> </b></h3>
    </div>
    <div class="box-body">

        <div class="row">
        <div class="col-xs-9" id='control01' style='text-align:left;' > 
            <a class="btn btn-primary" href="Index.aspx"><span class="glyphicon glyphicon-menu-left" aria-hidden="true"></span> 
                 <%if (language.Equals("1")){ %> Back <%} else if (language.Equals("2")){ %>Quay lại<%} %>
            </a> 
            
             <%-- <a class="btn btn-info"href="javascript:ExcelReport();"><span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span> 
                   <%if (language.Equals("1")){ %> Excel <%} else if (language.Equals("2")){ %> Excel <%} %>  
              </a>

             <a class="btn btn-info"href="javascript:loadPageReport();"><span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span> 
                  <%if (language.Equals("1")){ %> Report sale <%} else if (language.Equals("2")){ %> Báo cáo sale<%} %>  
             </a>          

            <% if (trangthai.Equals("0")){ %>
                <a class="btn btn-primary" href="javascript:SaveInfor(1);"><span class="glyphicon glyphicon-floppy-save" aria-hidden="true"></span> 
                    <%if (language.Equals("1")){ %> Save and Create new version <%} else if (language.Equals("2")) { %> Lưu và tạo mới phiên bản <%} %>
                </a>
            <% } %>--%>

            <% if (trangthai.Equals("0")){ %>
                <a class="btn btn-primary" href="javascript:SaveInfor(0);"><span class="glyphicon glyphicon-floppy-save" aria-hidden="true"></span> 
                    <%if (language.Equals("1")){ %> Save <%} else if (language.Equals("2")) { %> Lưu thông tin <%} %>
                </a>
            <% } %>

        </div>
        <div class="col-xs-1">
            <asp:DropDownList ID="ddlThang" runat="server" 
                AutoPostBack="True" onselectedindexchanged="ddlThang_SelectedIndexChanged" CssClass='form-control' ></asp:DropDownList>
        </div>
        <div class="col-xs-1">
            <asp:DropDownList ID="ddlNam" runat="server" 
                AutoPostBack="True" onselectedindexchanged="ddlNam_SelectedIndexChanged" CssClass='form-control' ></asp:DropDownList>
        </div>
        <div class="col-xs-1">
             <input type="text" class="form-control datepicker" id="txtNgayNhap" runat="server" placeholder="dd-MM-yyyy">
        </div>
    </div>

        <div class="row" style="margin-top:5px;">
        <div class="col-xs-12">
                <div id='loading01' class="box box-danger box-solid" style='display:none;' >
                    <div class="box-header">
                        <h3 class="box-title">
                            <%if (language.Equals("1")){ %> Processing <%} else if (language.Equals("2")){ %> Đang xử lý <%} %></h3>
                    </div>
                    <div class="box-body">
                        <%if (language.Equals("1")){ %> The system is processing your request. Please wait a moment. <%}
                       else if (language.Equals("2")){ %> Hệ thống đang xử lý yêu cầu. Vui lòng chờ đợi trong giây lát. <%} %>
                        
                    </div>
                    <div class="overlay">
                        <i class="fa fa-refresh fa-spin"></i>
                    </div>
                </div><!-- /.box -->
                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
        </div> 
    </div>

        <div class="row" style="margin-top:5px; font-size:smaller;">

            <div class="col-xs-2"></div>
           
            <div class="col-xs-2" style="display:none;">
                <asp:DropDownList ID="ddlPhienBan" runat="server" 
                    AutoPostBack="True" onselectedindexchanged="ddlPhienBan_SelectedIndexChanged" CssClass='form-control select2' ></asp:DropDownList>
            </div>

            <div class="col-xs-3">  
                <asp:DropDownList ID="ddlChiNhanh" runat="server" 
                    AutoPostBack="True" onselectedindexchanged="ddlChiNhanh_SelectedIndexChanged" CssClass='form-control select2' ></asp:DropDownList>
            </div> 
            
            <div class="col-xs-3">  
                <asp:DropDownList ID="ddlPhongBan" runat="server"
                    AutoPostBack="True" onselectedindexchanged="ddlPhongBan_SelectedIndexChanged" CssClass='form-control select2' ></asp:DropDownList>
            </div>

            <div class="col-xs-2" style="display:none;">
                    <button type="button" class="btn btn-success" data-toggle="modal" data-target="#exampleModalImport" data-whatever=""><span class="glyphicon glyphicon-import" aria-hidden="true"></span>
                        <%if (language.Equals("1")){ %> Imort File <%} else if (language.Equals("2")){%> Import file <%} %> 
                    </button>
            </div>

        </div>

        <div class="row" style="margin-top:5px; display:none;">
            <div class="col-xs-2" style="display:none;"><%if (language.Equals("1")){ %> Number <%} else if (language.Equals("2")){ %> Số phòng <%} %> </div>
            <div class="col-xs-10" style="display:none;">
                <input type="text" class="form-control" id="txtSoPhong"  runat="server" readonly>
            </div>
          </div>

          <div class="row" style="margin-top:10px; font-size:small;">               
                 <div style="height:auto; width:98%; overflow:auto; margin-bottom:-20px; margin-left:1%; background-color:white;" id="divDetail">
                    <table style="width:auto; font-size:small; overflow:scroll;">
                        <asp:Literal ID="ltInfor" runat="server"></asp:Literal>
                    </table>
                </div>            
          </div>
        
    </div><!-- /.box-body -->

      <div class="modal fade" id="exampleModalImport" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel">
          <div class="modal-dialog" role="document">
          <div class="modal-content">
           <div class="modal-header">
           <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
           <h4 class="modal-title" id="exampleModalLabel"><%if (language.Equals("1")){ %>Import file yearly sale <%} else if (language.Equals("2")) { %> Import file yearly sale  <%} %></h4>
           </div>
           <div class="modal-body">
           <form>		        
                <div class="form-group">			    
                    <input type="file" id="fileUpload" class="form-control" data-toggle="tooltip" title="Chọn file upload" onchange="fileSelected('fileUpload');" accept="image/*;capture=camera" >
                    <span id="fileUpload_details" style="display:none;" ></span>                               
                </div>            
                <div class="form-group" style="display:none;">
                <label for="recipient-name" class="control-label"><%if (language.Equals("1")){ %> Guid import file <%} else if (language.Equals("2")) { %> Hướng dẫn import file <%} %>: </label>			
                </div>

                <div class="form-group" style="display:none;">
                    <button type="button" class="btn btn-default" onclick="javascript:FileMauExcel();"><span class="glyphicon glyphicon-save" aria-hidden="true"></span> <%if (language.Equals("1")){ %> File template <%} else if (language.Equals("2")) { %> File mẫu <%} %></button>
                </div>
           </form>
           </div>
	
              <div id='control02' class="modal-footer">
               <button type="button" class="btn btn-default" data-dismiss="modal"><%if (language.Equals("1")){ %> Close <%} else if (language.Equals("2")) { %> Đóng lại <%} %></button>
                  <button type="button" class="btn btn-info" onclick="javascript:uploadFile('fileUpload', 0);" ><span class="fa fa-upload" aria-hidden="true"></span> Import File</button>		    
              </div>

              <div id='loading02' class="box box-danger box-solid" style='display:none;' >
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

<script type="text/javascript">

    function FileMauExcel() {
        var url = 'Hander/hdDownloadFileTemplate.ashx?action=fileTemplateYearlySale';
        window.location.assign(url);
    }

    function loadPageReport() {   
        <%--var nam = document.getElementById("<%= ddlNam.ClientID %>").value;
        var sanpham = document.getElementById("<%= ddlSanPham.ClientID %>").value;
        var khachhang = document.getElementById("<%= ddlKhachHang.ClientID %>").value;

        window.open("Report.aspx?func=122&ye=" + nam + "&it=" + sanpham + "&cus=" + khachhang);--%>
    }


    function ExcelReport() {        
      <%--  var nam = document.getElementById("<%= ddlNam.ClientID %>").value;
        var sanpham = document.getElementById("<%= ddlSanPham.ClientID %>").value;
        var khachhang = document.getElementById("<%= ddlKhachHang.ClientID %>").value;
        var phienban = document.getElementById("<%= ddlPhienBan.ClientID %>").value;

        var url = 'Hander/hdReport.ashx?action=yearlySaleMCT&nam=' + nam + "&sanpham=" + sanpham + "&khachhang=" + khachhang + "&phienban=" + phienban;

        window.location.assign(url);--%>
    }


    function loadPageAddNew() {
       <%-- var nam = document.getElementById("<%= ddlNam.ClientID %>").value;
        var sanpham = document.getElementById("<%= ddlSanPham.ClientID %>").value;
        var khachhang = document.getElementById("<%= ddlKhachHang.ClientID %>").value;

        window.open("MaterialControl.aspx?func=305&ye=" + nam + "&it=" + sanpham + "&cus=" + khachhang);--%>
    }

    function SaveInfor(ver) {
        var thang = document.getElementById("<%= ddlThang.ClientID %>").value;       
        var nam = document.getElementById("<%= ddlNam.ClientID %>").value;       
        var ngaynhap = document.getElementById("<%= txtNgayNhap.ClientID %>").value;
        var sophong = document.getElementById("<%= txtSoPhong.ClientID %>").value;
        var nhansu_fk = document.getElementsByName("nhansu_fk");

        var nhansu = "";
        var phongban = "";
        var chiphi = "";
        
        for (var j = 0; j < nhansu_fk.length; j++) {

            var flag = false;
            var chiphiTemp = "";

            if (nhansu_fk.item(j).value.length > 3) {                
                for (var i = 0; i < sophong; i++) {

                    var t = i.toString();
                    if (i < 10)
                        t = "0" + i.toString();
                   
                    var _chiphiOLD = document.getElementsByName("chiphiOLD" + t + "");
                    var _chiphi = document.getElementsByName("chiphi" + t + "");

                    var cpOLD = _chiphiOLD.item(j).value.replace(/,/g, "");
                    var cp = _chiphi.item(j).value.replace(/,/g, "");

                    if (cpOLD == "")
                        cpOLD = "0";
                    if (cp == "")
                        cp = "0";

                    if (cp != cpOLD)
                        flag = true;

                    chiphiTemp += cp + "_";
                    
                }

                if (chiphiTemp.length > 1)
                    chiphiTemp = chiphiTemp.substring(0, chiphiTemp.length - 1);

                if (flag == true) {
                    nhansu = nhansu + nhansu_fk.item(j).value + ";";
                    chiphi = chiphi + chiphiTemp + ";";                    
                }              
            }
        }

        for (var i = 0; i < sophong; i++) {

            var t = i.toString();
            if (i < 10)
                t = "0" + i.toString();

            var _phongban_fk = document.getElementsByName("phongban_fk" + t + "");           
            var pb = _phongban_fk.item(i).value;            
            phongban += pb + ";";

        }

        if (nhansu.length > 1)
            nhansu = nhansu.substring(0, nhansu.length - 1);
        if (phongban.length > 1)
            phongban = phongban.substring(0, phongban.length - 1);
        if (chiphi.length > 1)
            chiphi = chiphi.substring(0, chiphi.length - 1);
       
        //Multi click
        var divControl = document.getElementById('control01');
        var divLoading = document.getElementById('loading01');

        divControl.style.display = 'none';
        divLoading.style.display = '';

        $.post("Hander/hdMasterData.ashx?action=saveCostStaff&nhansu=" + nhansu + "&phongban=" + phongban + "&chiphi=" + chiphi + "&sophong=" + sophong + "&nam=" + nam +
            "&thang=" + thang + "&ngaynhap=" + ngaynhap, function (result) {
                if (result.length < 10) {

            <%if (language.Equals("1")){ %> alert('Save successfully'); <%} else if (language.Equals("2")){ %> alert('Lưu thành công'); <%} %>   
                    window.location.replace("Staff.aspx?func=9");

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
    $('#exampleModalImport').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget) // Button that triggered the modal
        var recipient = button.data('whatever') // Extract info FROM data-* attributes

        //LAY ID
        //if (recipient == '') {
        //    document.getElementById("txtId").value = '';
        //}
        //else {
        //    document.getElementById("txtId").value = recipient;
        //    LayThongTin(recipient);
        //}

        var modal = $(this)
    })
</script>

<script type="text/javascript">

    function fileSelected(id) {

        var count = document.getElementById(id).files.length;
        document.getElementById(id + '_details').innerHTML = "";

        for (var index = 0; index < count; index++) {

            var file = document.getElementById(id).files[index];
            var fileSize = 0;

            if (file.size > 1024 * 1024)
                fileSize = (Math.round(file.size * 100 / (1024 * 1024)) / 100).toString() + 'MB';
            else
                fileSize = (Math.round(file.size * 100 / 1024) / 100).toString() + 'KB';

            document.getElementById(id + '_details').innerHTML += 'Name: ' + file.name + ', Size: ' + fileSize + ', Type: ' + file.type;
            //document.getElementById('details').innerHTML += '<p>';
        }
    }

    function uploadFile(id, stt) {

        var fd = new FormData();

        var count = document.getElementById(id).files.length;

        for (var index = 0; index < count; index++) {
            var file = document.getElementById(id).files[index];
            fd.append(file.name, file);
        }

        var xhr = new XMLHttpRequest();

        var uploadType = "";

        document.getElementById('control02').style.display = 'none';
        document.getElementById('loading02').style.display = '';

        xhr.upload.addEventListener(id + "_progress", uploadProgress, false);
        xhr.addEventListener("load", uploadComplete, false);
        xhr.addEventListener("error", uploadFailed, false);
        xhr.addEventListener("abort", uploadCanceled, false);

        uploadType = "YearlySale";

        xhr.open("POST", "SaveToFile.aspx?uploadForder=Files&uploadType=" + uploadType);
        xhr.send(fd);

    }

    function uploadProgress(evt) {

        if (evt.lengthComputable) {

            var percentComplete = Math.round(evt.loaded * 100 / evt.total);
            document.getElementById('progress').innerHTML = "Waiting for uploading: " + percentComplete.toString() + '%';
        }

        else {

            document.getElementById('progress').innerHTML = 'unable to compute';
        }

    }

    function uploadComplete(evt) {

        document.getElementById('control02').style.display = '';
        document.getElementById('loading02').style.display = 'none';

        var msg = evt.target.responseText;
        var pos = parseInt(msg.indexOf("<!DOCTYPE"));
        msg = msg.substring(0, pos);

        if (msg.length > 10)
            alert(msg);

        window.location.replace("MaterialControl.aspx?func=304");
    }

    function uploadFailed(evt) {

        document.getElementById('control02').style.display = '';
        document.getElementById('loading02').style.display = 'none';

        alert("There was an error attempting to upload the file.");
    }

    function uploadCanceled(evt) {

        document.getElementById('control02').style.display = '';
        document.getElementById('loading02').style.display = 'none';

        alert("The upload has been canceled by the user or the browser dropped the connection.");

    }

</script>

</section><!-- /.content -->
