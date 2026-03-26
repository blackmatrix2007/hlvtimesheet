<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_masterdata_department_action.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_masterdata_department_action" %>
<!-- Main content -->
<section class="content">

<div class="box box-primary">
     <input type="hidden" id="sanpham_fk" name="sanpham_fk" value="<%= id %>" />
    <div class="box-header with-border" style='background-color: #dd4b39 !important;' >
    <h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")){ %>Information of work type <%} else if (language.Equals("2")) { %> Thông tin phòng ban <%} %></b></h3>
    </div>
    <div class="box-body">
    <div class="col-xs-12" >
	
        <div class="row">
            <div class="col-xs-12"  id='control01'> 
            <a class="btn btn-primary" href="MasterData.aspx?func=7"><span class="glyphicon glyphicon-menu-left" aria-hidden="true"></span> 
                <%if (language.Equals("1")){ %> Back <%} else if (language.Equals("2")) { %> Quay lại <%} %>
            </a>
             <asp:LinkButton ID="lbLuuLai" runat="server" CssClass="btn btn-primary" onclick="lbLuuLai_Click" ><span class="glyphicon glyphicon-floppy-save" aria-hidden="true"></span> 
                    <%if (language.Equals("1")){ %> Save <%} else if (language.Equals("2")) { %> Lưu thông tin <%} %> 
                </asp:LinkButton>
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
            <div class="col-xs-12 col-sm-12 col-lg-2"> <%if (language.Equals("1")){ %> Status <%} else if (language.Equals("2")) { %> Trạng thái <%} %> </div>
            <div class="col-xs-12 col-sm-12 col-lg-4">
                <input type="checkbox" id="chkTrangThai" name="chkTrangThai" runat="server" class='minimal-red' /> <i> Action </i>
            </div>
        </div>
        
        <div class="row" style="margin-top:10px;">           
            <div class="col-xs-12 col-sm-12 col-lg-2"><%if (language.Equals("1")){ %> Department (*) <%} else if (language.Equals("2")) { %> Phòng ban <%} %> </div>
            <div class="col-xs-12 col-sm-12 col-lg-10">
                <asp:TextBox ID="txtTen" runat="server" Width="100%" CssClass='form-control' ></asp:TextBox>
            </div>
        </div><!-- /.box-body -->

      
    <div class="row" style="margin-top:10px;">
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
                <li class="active"><a data-toggle="tab" href="#WorkType"><%if (language.Equals("1")){ %> Job list <%} else if (language.Equals("2")){%> Danh sách công việc <%} %></a></li>                
                <li><a data-toggle="tab" href="#Staff"><%if (language.Equals("1")){ %> Staff <%} else if (language.Equals("2")){%> Danh sách nhân sự <%} %></a></li>                
            </ul>

            <div class="tab-content">

                <div id="WorkType" class="tab-pane in active">
                    <table  style="width:90%; margin-left:5%; font-size:small;" class="table table-hover table-bordered table-striped">
                        <tr>
                            <th style="text-align:center; width:5%;">#</th>
                            <th style="text-align:center; width:10%;"><%if (language.Equals("1")){ %> Service code <%} else if (language.Equals("2")){%> Service code  <%}%></th>
                            <th style="text-align:center; width:50%;"><%if (language.Equals("1")){ %> Job <%} else if (language.Equals("2")){%> Công việc  <%}%></th>
                            <th style="text-align:center; width:10%;"><%if (language.Equals("1")){ %> Unit <%} else if (language.Equals("2")){%> Đơn vị <%} %></th>
                            <th style="text-align:center; width:10%;"><%if (language.Equals("1")){ %> Cycle time (M) <%} else if (language.Equals("2")){%> Cycle time (M) <%} %></th>                            
                            <th style="text-align:center; width:10%;"><%if (language.Equals("1")){ %> Price <%} else if (language.Equals("2")){%> Giá mua  <%}%></th>
                            <th style="text-align:center; width:10%;"><%if (language.Equals("1")){ %> Choose <%} else if (language.Equals("2")){%> Chọn  <%}%></th>
                        </tr>
                        <asp:Literal ID="ltSanPham" runat="server"></asp:Literal>
                    </table>
                </div>

                <div id="Staff" class="tab-pane">
                     <table  style="width:90%; margin-left:5%; font-size:small;" class="table table-hover table-bordered table-striped">
                         <tr>
                             <th style="text-align:center; width:5%;">#</th>
                             <th style="text-align:center; width:6%;"><%if (language.Equals("1")){ %> Branch <%} else if (language.Equals("2")){%> Chi nhánh  <%}%></th>
                             <th style="text-align:center; width:10%;"><%if (language.Equals("1")){ %> Department <%} else if (language.Equals("2")){%> Phòng ban  <%}%></th>
                             <th style="text-align:center; width:12%;"><%if (language.Equals("1")){ %> Position <%} else if (language.Equals("2")){%> Chức vụ <%} %></th>
                             <th style="text-align:center; width:8%;"><%if (language.Equals("1")){ %> Code <%} else if (language.Equals("2")){%> Mã <%} %></th> 
                             <th style="text-align:center; width:20%;"><%if (language.Equals("1")){ %> Name <%} else if (language.Equals("2")){%> Nhân viên <%} %></th> 
                             <th style="text-align:center; width:10%;"><%if (language.Equals("1")){ %> Choose <%} else if (language.Equals("2")){%> Chọn  <%}%></th>
                         </tr>
                         <asp:Literal ID="ltStaff" runat="server"></asp:Literal>
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

        alert("Chưa lưu được đâu bạn ơi")
        return;

        var sanpham_fk = document.getElementById("sanpham_fk").value;
        var ma = document.getElementById("<%= txtMa.ClientID %>").value;
        var ten = document.getElementById("<%= txtTen.ClientID %>").value;
        
        if (document.getElementById("<%= chkTrangThai.ClientID %>").checked)
            trangthai = "1";

        //var sanpham_fk = document.getElementsByName("sanpham_fk");
        //var masp = document.getElementsByName("masp");
        //var tensp = document.getElementsByName("tensp");
        //var dvt_fk = document.getElementsByName("unitId");
        //var cycletime = document.getElementsByName("cycletime");
        //var giaban = document.getElementsByName("giaban");
       
        //var sanpham = "";
        //for (var i = 0; i < masp.length; i++) {

        //    var _sanpham_fk = sanpham_fk.item(i).value;
        //    var _masp = masp.item(i).value;
        //    var _tensp = tensp.item(i).value;
        //    var _dvt_fk = dvt_fk.item(i).value;
        //    var _cycletime = cycletime.item(i).value.replace(/,/g, "");
        //    var _giaban = giaban.item(i).value.replace(/,/g, "");

        //    if (_cycletime == "")
        //        _cycletime = "0";
        //    if (_giaban == "")
        //        _giaban = "0";

        //    if (_masp != "" && _tensp.length > 1) {
        //        sanpham += _sanpham_fk + '_' + _masp + '_' + _tensp + '_' + _dvt_fk + '_' + _cycletime + '_' + _giaban + ';';
        //    }
        //}
        
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

        $.post("Hander/hdMasterData.ashx?action=saveProduction&sanpham_fk=" + sanpham_fk + "&ma=" + ma + "&ten=" + ten + 
            "&trangthai=" + trangthai, function (result) {
                if (result.length < 10) {

            <%if (language.Equals("1")){ %> alert('Save successfully'); <%} else if (language.Equals("2")){ %> alert('Lưu thành công'); <%} %>   
                    window.location.replace("MasterData.aspx?func=7");

                }
                else {

                    divControl.style.display = '';
                    divLoading.style.display = 'none';

                    alert(result);
                }

            });
    }

</script>
