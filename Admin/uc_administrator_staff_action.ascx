<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_administrator_staff_action.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_administrator_staff_action" %>

<style>

   .clearBoder {
        border-top: 0;
        border-left: 0;
        border-right: 0;
    }

</style>
<!-- Main content -->
<section class="content">

<script type="text/javascript">

     function fileSelected(id) {

         uploadFile(id, 0);

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

         uploadType = "Avatar";

         xhr.open("POST", "SaveFileUpload.aspx?Avatar=Avatar&uploadForder=Avatar&uploadType=" + uploadType);
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

         var name = evt.target.responseText;
         var pos = parseInt(name.indexOf("<!DOCTYPE"));

         if (pos > 0) {
             var arr = name.split("<!DOCTYPE");
             pos = parseInt(name.indexOf("."));
             name = arr[0].substring(0, pos + 4);
             
             document.getElementById("<%= divHinhUpLoad.ClientID %>").innerHTML = "<img src='/Admin/Avatar/" + name + "' style='max-height:150px; max-width:100px' />";
             document.getElementById("<%= txtHinhAnh.ClientID%>").value = name;
         }
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


<div class="box box-primary">
    <div class="box-header with-border" style='background-color: #dd4b39 !important;' >
    <h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")){ %>Information of staff <%} else if (language.Equals("2")) { %> Thông tin nhân viên <%} %></b></h3>
    </div>

    <div class="box-body">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-lg-4"  style="margin-top:10px;">
                <a class="btn btn-primary" href="Administrator.aspx?func=95"><span class="glyphicon glyphicon-menu-left" aria-hidden="true"></span> <%if (language.Equals("1")){ %> Back <%} else if (language.Equals("2")) { %> Quay về <%} %></a>
                <asp:LinkButton ID="lbLuuLai" runat="server" CssClass="btn btn-primary" onclick="lbLuuLai_Click"><span class="glyphicon glyphicon-floppy-save" aria-hidden="true"></span> <%if (language.Equals("1")){ %> Save <%} else if (language.Equals("2")) { %> Lưu thông tin <%} %> </asp:LinkButton>
                <br /><br />
          </div>
         
            <div class="col-xs-12 col-sm-12 col-lg-6" style="margin-top:10px;"> </div>
               
            <div class="col-xs-12 col-sm-12 col-lg-2" style="margin-top:10px; text-align:right">
                    <asp:DropDownList ID="ddlChiNhanh" runat="server" CssClass='form-control clearBoder' ClientIDMode="Static" data-toggle="tooltip" title="Chi nhánh"></asp:DropDownList>
                </div>

            <div class="col-xs-12 col-sm-12 col-lg-12">
              <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
          </div>

        </div>

        <div class="col-xs-12 col-sm-12 col-lg-9">
            <div class="row">
                   <div class="col-xs-12 col-sm-2 col-lg-2" style="margin-top:10px;"><%if (language.Equals("1")){ %> Code <%} else if (language.Equals("2")) { %> Mã nhân viên <%} %>(*)</div>
                 <div class="col-xs-12 col-sm-10 col-lg-5" style="margin-top:10px;"> 
                     <input type="text" id="txtMa" style="width:100%;" runat="server" class='form-control clearBoder'/>
                  </div>

                <div class="col-xs-12 col-sm-2 col-lg-2" style="margin-top:10px;"><%if (language.Equals("1")){ %> Show work schedule week <%} else if (language.Equals("2")) { %> Hiển thị lịch tuần làm việc <%} %></div>
                <div class="col-xs-12 col-sm-10 col-lg-3" style="margin-top:10px;">  
                    <asp:DropDownList ID="ddlHienThi" runat="server" Width="100%" CssClass='form-control select2 clearBoder'></asp:DropDownList>
                </div>

            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-2 col-lg-2"  style="margin-top:10px;"><%if (language.Equals("1")){ %>Staff <%} else if (language.Equals("2")) { %> Nhân viên <%} %>(*)</div>
                 <div class="col-xs-12 col-sm-10 col-lg-5"  style="margin-top:10px;"> 
                     <asp:TextBox ID="txtTen" runat="server" Width="100%" CssClass='form-control clearBoder' ></asp:TextBox>
                  </div>
              
                 <div class="col-xs-12 col-sm-2 col-lg-2" style="margin-top:10px;"><%if (language.Equals("1")){ %> Short name (*) <%} else if (language.Equals("2")) { %> Tên thường gọi (*) <%} %></div>
                <div class="col-xs-12 col-sm-10 col-lg-3" style="margin-top:10px;">  
                    <asp:TextBox ID="txtTenHienThi" runat="server" Width="100%" CssClass='form-control clearBoder' ></asp:TextBox>
                </div>

            </div>
            
             <div class="row">
                <div class="col-xs-12 col-sm-2 col-lg-2" style="margin-top:10px;"><%if (language.Equals("1")){ %> Department <%} else if (language.Equals("2")) { %> Phòng ban <%} %></div>
                <div class="col-xs-12 col-sm-10 col-lg-5" style="margin-top:10px;">  
                    <asp:DropDownList ID="ddlPhongBan" runat="server" Width="100%" CssClass='form-control clearBoder'></asp:DropDownList>
                </div>               
                <div class="col-xs-12 col-sm-2 col-lg-2" style="margin-top:10px;"><%if (language.Equals("1")){ %> Position <%} else if (language.Equals("2")) { %> Chức vụ <%} %> (*) </div>
                <div class="col-xs-12 col-sm-10 col-lg-3" style="margin-top:10px;"> 
                    <asp:DropDownList ID="ddlChucVu" runat="server" Width="100%" CssClass='form-control clearBoder'
                        AutoPostBack="True" onselectedindexchanged="ddlChucVu_SelectedIndexChanged" ></asp:DropDownList>
                    <input type="hidden" id="txtCapBac" runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="col-xs-12 col-sm-2 col-lg-2"  style="margin-top:10px;"><%if (language.Equals("1")){ %> Address <%} else if (language.Equals("2")) { %> Địa chỉ <%} %></div>
                    <div class="col-xs-12 col-sm-10 col-lg-5"  style="margin-top:10px;">
                    <asp:TextBox ID="txtDiaChi" runat="server" Width="100%" CssClass='form-control clearBoder' ></asp:TextBox>
                </div>
            
                <div class="col-xs-12 col-sm-2 col-lg-2"  style="margin-top:10px;"><%if (language.Equals("1")){ %> Phone <%} else if (language.Equals("2")) { %> Di động <%} %></div>
                <div class="col-xs-12 col-sm-10 col-lg-3"  style="margin-top:10px;"> 
                    <input type="text" id="txtDienThoai" onKeyPress="return isNumberKey(event)" style="width:100%;" runat="server" class='form-control clearBoder'/>
                </div>
            </div>
           
            <div class="row"> 
                 <div class="col-xs-12 col-sm-2 col-lg-2" style="margin-top:10px;">Mail</div>
                <div class="col-xs-12 col-sm-10 col-lg-5" style="margin-top:10px;">  
                    <asp:TextBox ID="txtMail" runat="server" Width="100%" CssClass='form-control clearBoder' ></asp:TextBox>
                </div>

                 <div class="col-xs-12 col-sm-2 col-lg-2" style="margin-top:10px;"><%if (language.Equals("1")){ %> Gender <%} else if (language.Equals("2")) { %> Giới tính <%} %></div>
                <div class="col-xs-12 col-sm-10 col-lg-3" style="margin-top:10px;">
                    <asp:DropDownList ID="ddlGioiTinh" runat="server" Width="100%" CssClass='form-control clearBoder'></asp:DropDownList>
                </div>

            </div>

            <div class="row">
                <div class="col-xs-12 col-sm-2 col-lg-2" style="margin-top:10px;"><%if (language.Equals("1")){ %> Birthday <%} else if (language.Equals("2")) { %> Ngày sinh <%} %></div>
                <div class="col-xs-12 col-sm-10 col-lg-5" style="margin-top:10px;">  
                    <asp:TextBox ID="txtNgaySinh" runat="server" Width="100%" CssClass='form-control clearBoder datepicker' placeholder="dd-mm-yyyy"></asp:TextBox>
                </div>               
                
            </div>

             <div class="row">
                <div class="col-xs-12 col-sm-2 col-lg-2" style="margin-top:10px;"><%if (language.Equals("1")){ %> Start date <%} else if (language.Equals("2")) { %> Ngày bắt đầu làm <%} %></div>
                <div class="col-xs-12 col-sm-10 col-lg-5" style="margin-top:10px;">  
                    <asp:TextBox ID="txtNgayBatDauLam" runat="server" Width="100%" CssClass='form-control clearBoder datepicker' placeholder="dd-mm-yyyy"></asp:TextBox>
                </div>               
               <div class="col-xs-12 col-sm-2 col-lg-2" style="margin-top:10px;"><%if (language.Equals("1")){ %> Status <%} else if (language.Equals("2")) { %> Trạng thái <%} %></div>
                <div class="col-xs-12 col-sm-10 col-lg-3" style="margin-top:10px;"> 
                    <asp:DropDownList ID="ddlTrangThai" runat="server" Width="100%" CssClass='form-control clearBoder'
                        AutoPostBack="True" onselectedindexchanged="ddlTrangThai_SelectedIndexChanged" ></asp:DropDownList>
                </div>
            </div>

             <div class="row showSetting">
                <div class="col-xs-12 col-sm-2 col-lg-2" style="margin-top:10px;"><%if (language.Equals("1")){ %> From <%} else if (language.Equals("2")) { %> Từ ngày <%} %></div>
                <div class="col-xs-12 col-sm-10 col-lg-5" style="margin-top:10px;">  
                    <asp:TextBox ID="txtTuNgay" runat="server" Width="100%" CssClass='form-control clearBoder datepicker' placeholder="dd-mm-yyyy"></asp:TextBox>
                </div>               
                <div class="col-xs-12 col-sm-2 col-lg-2" style="margin-top:10px;"><%if (language.Equals("1")){ %> To <%} else if (language.Equals("2")) { %> Đến ngày <%} %></div>
                <div class="col-xs-12 col-sm-10 col-lg-3" style="margin-top:10px;"> 
                    <asp:TextBox ID="txtDenNgay" runat="server" Width="100%" CssClass='form-control clearBoder datepicker' placeholder="dd-mm-yyyy"></asp:TextBox>
                </div>
            </div>

             <div class="row">
                <div class="col-xs-12 col-sm-2 col-lg-2 showSetting" style="margin-top:10px;"><%if (language.Equals("1")){ %> Reason <%} else if (language.Equals("2")) { %> Lý do <%} %></div>
                <div class="col-xs-12 col-sm-10 col-lg-5 showSetting" style="margin-top:10px;">  
                    <asp:TextBox ID="txtLyDo" runat="server" Width="100%" CssClass='form-control clearBoder'></asp:TextBox>
                </div>
                <div class="col-xs-12 col-sm-2 col-lg-2 showSupport" style="margin-top:10px;"><%if (language.Equals("1")){ %> Support department <%} else if (language.Equals("2")) { %> Hỗ trợ bộ phận <%} %></div>
                <div class="col-xs-12 col-sm-10 col-lg-3 showSupport" style="margin-top:10px;">                    
                    <select id="ddlPhongBanSupport" class='form-control clearBoder' runat="server" style="width:100%;" onchange="ChangeStatus();"></select>
                </div>   
            </div>

             <div class="row">
                <div class="col-xs-12 col-sm-2 col-lg-2" style="margin-top:10px; display:none;"><%if (language.Equals("1")){ %> Account <%} else if (language.Equals("2")) { %> Tài khoản truy cập <%} %></div>
                <div class="col-xs-12 col-sm-10 col-lg-5" style="margin-top:10px; display:none;">  
                    <asp:DropDownList ID="ddlTaiKhoan" runat="server" Width="100%" CssClass='form-control select2 clearBoder'></asp:DropDownList>
                </div>
                
                 <div class="col-xs-12 col-sm-2 col-lg-2" style="margin-top:10px;"> <%if (language.Equals("1")){ %> Username <%} else if (language.Equals("2")) { %> Đăng nhập <%} %> </div>
                <div class="col-xs-12 col-sm-10 col-lg-5" style="margin-top:10px;"> 
                    <input type="text" id="txtDangNhap" runat="server" class="form-control clearBoder"  />
                    <input type="hidden" id="txtToken" runat="server" class="form-control"  />
                </div>

                <div class="col-xs-12 col-sm-2 col-lg-2" style="margin-top:10px;"> <%if (language.Equals("1")){ %> Password <%} else if (language.Equals("2")) { %> Mật khẩu <%} %> </div>
                <div class="col-xs-12 col-sm-10 col-lg-3" style="margin-top:10px;">
                    <input type="password" id="txtMatkhau" runat="server" class="form-control clearBoder"  />
                </div>

            </div>

        </div>
        
        <div class="col-xs-12 col-sm-12 col-lg-3">
             <div class="row" style="margin-top:10px;">
                     <div class="col-xs-12 col-sm-2 col-lg-2" style="display:none;"><%if (language.Equals("1")){ %> Avatar <%} else if (language.Equals("2")) { %> Hình ảnh <%} %> </div>
                     <div class="col-xs-12 col-sm-4 col-lg-12">
                            <input type="file" id="fileUpload" class="form-control" data-toggle="tooltip" title="Chọn file upload" onchange="fileSelected('fileUpload');" accept="image/*;capture=camera" >
                            <span id="fileUpload_details" style="display:none;" ></span>
                    </div>
                     <div class="col-xs-12 col-sm-2 col-lg-4">
                        <div id='control02' style="display:none;"> 
                            <div id='loading02' class="box box-danger box-solid" style="display:none;"  >
                                <div class="box-header">
                                    <h3 class="box-title">Đang xử lý yêu cầu</h3>
                                </div>
                        
                                <div class="overlay">
                                    <i class="fa fa-refresh fa-spin"></i>
                                </div>
                            </div>
                        </div>
                        &nbsp; <button type="button" class="btn btn-success" onclick="javascript:uploadFile('fileUpload', 0);" style="display:none;"><span class="fa fa-upload" aria-hidden="true"></span> Import File</button>
                    </div>
                </div>

             <div class="col-xs-12 col-sm-12 col-lg-12">
                <div style="padding: 10px; text-align:center;" id="divHinhUpLoad" runat="server"></div>
                    <input type="hidden" class="form-control" id="txtHinhAnh" runat="server">
                </div>
        </div>

        <div class="row" style="margin-top:10px;">
            <div class="col-md-12">
            
                <ul class="nav nav-tabs">
                    <li class="active"><a data-toggle="tab" href="#phongban"> <%if (language.Equals("1")){ %> Department in charge <%} else if (language.Equals("2")) { %> Phòng phụ trách <%} %></a></li>
                    <li><a data-toggle="tab" href="#nhansu"> <%if (language.Equals("1")){ %> Staff <%} else if (language.Equals("2")) { %> Nhân sự <%} %></a></li>
                    <li><a data-toggle="tab" href="#nhomquyen"> <%if (language.Equals("1")){ %> Role permission <%} else if (language.Equals("2")) { %> Nhóm quyền <%} %></a></li>
                </ul>

                <div class="tab-content" style="margin-top:5px;">

                    <div id="phongban" class="tab-pane fade in active">
                        <table class="table table-hover table-bordered table-striped" style=" font-size:small">
                            <tr>
			                    <th style="text-align:center; width:3%;">#</th>
                                <th style="text-align:center; width:8%;"><%if (language.Equals("1")){ %> Code  <%} else if (language.Equals("2")) { %> Mã <%} %></th>
                                <th style="text-align:center; width:30%;"><%if (language.Equals("1")){ %> Name  <%} else if (language.Equals("2")) { %> Tên <%} %></th>                                
                                <th style="text-align:center; width:5%;">Chọn</th>
                            </tr>
                            <asp:Literal ID="ltData" runat="server"></asp:Literal>
                        </table>
                    </div>

                     <div id="nhansu" class="tab-pane fade in">
                        <table class="table table-hover table-bordered table-striped" style=" font-size:small">
                            <tr>
			                    <th style="text-align:center; width:3%;">#</th>
                                <th style="text-align:center; width:8%;"><%if (language.Equals("1")){ %> Code  <%} else if (language.Equals("2")) { %> Mã <%} %></th>
                                <th style="text-align:center; width:30%;"><%if (language.Equals("1")){ %> Name  <%} else if (language.Equals("2")) { %> Tên <%} %></th>
                                <th style="text-align:center; width:25%;"><%if (language.Equals("1")){ %> Department  <%} else if (language.Equals("2")) { %> Phòng/ban - Bộ phận <%} %></th>
                                <th style="text-align:center; width:25%;"><%if (language.Equals("1")){ %> Position  <%} else if (language.Equals("2")) { %> Chức vụ <%} %></th>
                                <th style="text-align:center; width:5%;">Chọn</th>
                            </tr>
                            <asp:Literal ID="ltNhanSu" runat="server"></asp:Literal>
                        </table>
                    </div>

                     <div id="nhomquyen" class="tab-pane fade in">                    
                        <table class="table table-hover table-bordered table-striped" style="font-size:small">
                            <tr>
			                    <th style="text-align:center;">#</th>
                                <th style="text-align:center;"><%if (language.Equals("1")){ %> Code <%} else if (language.Equals("2")) { %> Mã <%} %></th>
                                <th style="text-align:center;"><%if (language.Equals("1")){ %> Name <%} else if (language.Equals("2")) { %> Tên nhóm quyền <%} %></th>                                        
                                <th style="text-align:center;"><%if (language.Equals("1")){ %> Choose <%} else if (language.Equals("2")) { %> Chọn <%} %> </th>
                            </tr>
                            <asp:Literal ID="ltNhomQuyen" runat="server"></asp:Literal>
                        </table>
                    </div>


                </div>

            </div><!-- /.col -->
        </div>

    </div><!-- /.box-body -->

</div><!-- /.box -->
</section><!-- /.content -->

<script lang="javascript" type="text/javascript">

    function ChangeStatus() {

        var trangthai = document.getElementById("<%= ddlTrangThai.ClientID %>").value;
           $(".showSetting").show();
           $(".showSupport").show();
           document.getElementById("<%= txtTuNgay.ClientID %>").disabled = false;
           document.getElementById("<%= txtDenNgay.ClientID %>").disabled = false;
           document.getElementById("<%= txtLyDo.ClientID %>").disabled = false;
           document.getElementById("<%= ddlPhongBanSupport.ClientID %>").disabled = false;

           if (trangthai == "1") {
               document.getElementById("<%= txtTuNgay.ClientID %>").value = "";
            document.getElementById("<%= txtDenNgay.ClientID %>").value = "";
            document.getElementById("<%= txtLyDo.ClientID %>").value = "";
            document.getElementById("<%= ddlPhongBanSupport.ClientID %>").value = "0";

            document.getElementById("<%= txtTuNgay.ClientID %>").disabled = true;
            document.getElementById("<%= txtDenNgay.ClientID %>").disabled = true;
            document.getElementById("<%= txtLyDo.ClientID %>").disabled = true;
            document.getElementById("<%= ddlPhongBanSupport.ClientID %>").disabled = true;

            $('.showSetting').css("display", "none");
            $('.showSupport').css("display", "none");
        }
        else if (trangthai == "2") {
            document.getElementById("<%= ddlPhongBanSupport.ClientID %>").value = "0";
            document.getElementById("<%= ddlPhongBanSupport.ClientID %>").disabled = true;
               $('.showSupport').css("display", "none");
           }
       }

    $(document).ready(function () {
        ChangeStatus();
    });

</script>