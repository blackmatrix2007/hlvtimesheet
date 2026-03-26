<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_masterdata_worktype_action.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_masterdata_worktype_action" %>

<!-- Main content -->
<section class="content">

<div class="box box-primary">
     <input type="hidden" id="sanpham_fk" name="sanpham_fk" value="<%= id %>" />
    <div class="box-header with-border" style='background-color: #dd4b39 !important;' >
    <h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")){ %>Information of item <%} else if (language.Equals("2")) { %> Thông tin sản phẩm <%} %></b></h3>
    </div>
    <div class="box-body">
    <div class="col-xs-12" >
	
        <div class="row">
            <div class="col-xs-12"  id='control01'> 
            <a class="btn btn-primary" href="MasterData.aspx?func=5"><span class="glyphicon glyphicon-menu-left" aria-hidden="true"></span> 
                <%if (language.Equals("1")){ %> Back <%} else if (language.Equals("2")) { %> Quay lại <%} %>
            </a>
             <a class="btn btn-primary" href="javascript:SaveProduction();"><span class="glyphicon glyphicon-floppy-save" aria-hidden="true"></span> 
                 <%if (language.Equals("1")){ %> Save <%} else if (language.Equals("2")) { %> Lưu thông tin <%} %>
             </a>
          
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

        <div class="row" style="margin-top:5px;">
            <div class="col-xs-12 col-sm-12 col-lg-2"><%if (language.Equals("1")){ %> Service code. (*) <%} else if (language.Equals("2")) { %> Mã (*) <%} %> </div>
            <div class="col-xs-12 col-sm-12 col-lg-4">
                <asp:TextBox ID="txtMa" runat="server" Width="100%" CssClass='form-control' ></asp:TextBox>
            </div>
           
             <div class="col-xs-12 col-sm-12 col-lg-2"><%if (language.Equals("1")){ %> Short code (*) <%} else if (language.Equals("2")) { %> Mã (*) <%} %> </div>
             <div class="col-xs-12 col-sm-12 col-lg-4">
                 <asp:TextBox ID="txtNameEnglish" runat="server" Width="100%" CssClass='form-control' ></asp:TextBox>
             </div>
        </div>
        
        <div class="row" style="margin-top:10px;">           
            <div class="col-xs-12 col-sm-12 col-lg-2"><%if (language.Equals("1")){ %> Detail name (*) <%} else if (language.Equals("2")) { %> Tên dịch vụ <%} %> </div>
            <div class="col-xs-12 col-sm-12 col-lg-10">
                <asp:TextBox ID="txtTen" runat="server" Width="100%" CssClass='form-control' ></asp:TextBox>
            </div>
        </div><!-- /.box-body -->

         <div class="row" style="margin-top:5px;"> 
            <div class="col-xs-12 col-sm-12 col-lg-2"><%if (language.Equals("1")){ %> Group <%} else if (language.Equals("2")) { %>Nhóm sản phẩm <%} %> </div>
            <div class="col-xs-12 col-sm-12 col-lg-4">
            <asp:DropDownList ID="ddlNhomSanPham" runat="server" Width="100%" CssClass='form-control'></asp:DropDownList>
            </div>    
            <div class="col-xs-12 col-sm-12 col-lg-2"><%if (language.Equals("1")){ %> Unit <%} else if (language.Equals("2")) { %> Đơn vị tính <%} %> </div>
            <div class="col-xs-12 col-sm-12 col-lg-4">
                <asp:DropDownList ID="ddlDonViTinh" runat="server" Width="100%" CssClass='form-control'></asp:DropDownList>
            </div>    
        </div>

        <div class="row" style="margin-top:5px;">           
            <div class="col-xs-12 col-sm-12 col-lg-2"> <%if (language.Equals("1")){ %> Cycle time (M) <%} else if (language.Equals("2")) { %> Chu kỳ thời gian (phút) <%} %> </div>
            <div class="col-xs-12 col-sm-12 col-lg-4" style="text-align:right;">
                 <input type="text" id="txtCycleTime" style="width:100%; text-align:right;" runat="server" class='form-control' autocomplete="off"/>
            </div>

             <div class="col-xs-12 col-sm-12 col-lg-2"> <%if (language.Equals("1")){ %> Cycle target (M) <%} else if (language.Equals("2")) { %> Chu kỳ mục tiêu (phút) <%} %> </div>
             <div class="col-xs-12 col-sm-12 col-lg-4" style="text-align:right;">
                  <input type="text" id="txtCycleTarget" style="width:100%; text-align:right;" runat="server" class='form-control' autocomplete="off"/>
             </div>
        </div>

         <div class="row" style="margin-top:5px;">
             <div class="col-xs-12 col-sm-12 col-lg-2"><%if (language.Equals("1")){ %> Price <%} else if (language.Equals("2")) { %> Đơn giá <%} %> </div>
             <div class="col-xs-12 col-sm-12 col-lg-4" style="text-align:right;">
                  <input type="text" id="txtDonGia" style="width:100%; text-align:right;" runat="server" class='form-control' autocomplete="off"/>
             </div>             
         </div>

         <div class="row" style="margin-top:5px;">
              <div class="col-xs-12 col-sm-12 col-lg-2"> <%if (language.Equals("1")){ %> Status <%} else if (language.Equals("2")) { %> Trạng thái <%} %> </div>
             <div class="col-xs-12 col-sm-12 col-lg-4">
                 <input type="checkbox" id="chkTrangThai" name="chkTrangThai" runat="server" class='minimal-red' /> <i> Action </i>
             </div>

             <div class="col-xs-12 col-sm-12 col-lg-2"> <%if (language.Equals("1")){ %> Customer list <%} else if (language.Equals("2")) { %> Hiển thị <%} %> </div>
              <div class="col-xs-12 col-sm-12 col-lg-4">
                 <label class='switch'><input type='checkbox' id="ckShowCustomer" name='ckShowCustomer' runat="server"><span class='slider round'></span></label>
             </div>
         </div>

        <div class="row" style="margin-top:10px; display:none;">
            <div class="col-md-12">
            <div class="box box-primary" style="font-size:small;">
            <div class="box-header with-border" style="display:none;">
                <h3 class="box-title">Setup</h3>
                <div class="box-tools pull-right">                    
                <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                </div>
            </div><!-- /.box-header -->

            <div class="box-body">
                <ul class="nav nav-tabs">                  
                    <li class="active"><a data-toggle="tab" href="#Material"><%if (language.Equals("1")){ %> Job list <%} else if (language.Equals("2")){%> Danh sách công việc <%} %></a></li>                
                </ul>

                <div class="tab-content">

                    <div id="Material" class="tab-pane in active">
                        <table  style="width:90%; margin-left:5%; font-size:small;" class="table table-hover table-bordered table-striped">
                            <tr>
                                <th style="text-align:center; width:5%;">#</th>
                                <th style="text-align:center; width:10%;"><%if (language.Equals("1")){ %> Service code <%} else if (language.Equals("2")){%> Service code  <%}%></th>
                                <th style="text-align:center; width:50%;"><%if (language.Equals("1")){ %> Job <%} else if (language.Equals("2")){%> Công việc  <%}%></th>
                                <th style="text-align:center; width:10%;"><%if (language.Equals("1")){ %> Unit <%} else if (language.Equals("2")){%> Đơn vị <%} %></th>
                                <th style="text-align:center; width:10%;"><%if (language.Equals("1")){ %> Cycle time (M) <%} else if (language.Equals("2")){%> Cycle time (M) <%} %></th>                            
                                <th style="text-align:center; width:10%;"><%if (language.Equals("1")){ %> Price <%} else if (language.Equals("2")){%> Giá mua  <%}%></th>                                                        
                            </tr>
                            <asp:Literal ID="ltSanPham" runat="server"></asp:Literal>
                        </table>
                    </div>

               
                </div>

            </div><!-- ./box-body -->
               
            </div><!-- /.box -->
        </div><!-- /.col -->      
        </div>

    </div><!-- /.box -->
    </div>
</div>
</section><!-- /.content -->


<script type="text/javascript" >

    function SaveProduction() {

        var sanpham_fk = document.getElementById("sanpham_fk").value;
        var ma = document.getElementById("<%= txtMa.ClientID %>").value;
        var ten = document.getElementById("<%= txtTen.ClientID %>").value;
        var nameEnglish = document.getElementById("<%= txtNameEnglish.ClientID %>").value;
        var donvi = document.getElementById("<%= ddlDonViTinh.ClientID %>").value;
        var cycletime = document.getElementById("<%= txtCycleTime.ClientID %>").value.replace(/,/g, "");
        var cycletarget = document.getElementById("<%= txtCycleTarget.ClientID %>").value.replace(/,/g, "");
        var dongia = document.getElementById("<%= txtDonGia.ClientID %>").value;
        var nhomsanpham = document.getElementById("<%= ddlNhomSanPham.ClientID %>").value;

        var trangthai = "0";
        if (document.getElementById("<%= chkTrangThai.ClientID %>").checked)
            trangthai = "1";

        var showCustomer = "0";
        if (document.getElementById("<%= ckShowCustomer.ClientID %>").checked)
            showCustomer = "1";
       
        if (ma.length <= 0) {
         <%if (language.Equals("1")){ %> alert('Please, you must to part no!'); <%} else if (language.Equals("2")){ %> alert('Vui lòng nhập mã sản phẩm'); <%} %>

            return;
        }

        if (ten.length <= 0) {
        <%if (language.Equals("1")){ %> alert('Please, you must to part name!'); <%} else if (language.Equals("2")){ %> alert('Vui lòng nhập tên sản phẩm'); <%} %>

            return;
        }

        //Multi click
        var divControl = document.getElementById('control01');
        var divLoading = document.getElementById('loading01');

        divControl.style.display = 'none';
        divLoading.style.display = '';

        $.post("Hander/hdMasterData.ashx?action=saveProduction&sanpham_fk=" + sanpham_fk + "&ma=" + ma + "&ten=" + ten + "&donvi=" + donvi + "&nameEnglish=" + nameEnglish + "&nhomsanpham=" + nhomsanpham +
            "&trangthai=" + trangthai + "&showCustomer=" + showCustomer + "&cycletarget=" + cycletarget + "&cycletime=" + cycletime + "&dongia=" + dongia, function (result) {
                if (result.length < 10) {

            <%if (language.Equals("1")){ %> alert('Save successfully'); <%} else if (language.Equals("2")){ %> alert('Lưu thành công'); <%} %>   
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
