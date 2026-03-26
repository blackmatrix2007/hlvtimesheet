<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_masterdata_staff.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_masterdata_staff" %>

<!-- Main content -->
<section class="content">

<div class="box box-primary">
    <div class="box-header with-border" style='background-color: #dd4b39 !important;' >
    <h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")){ %>Information of work type <%} else if (language.Equals("2")) { %> Thông tin sản phẩm <%} %></b></h3>
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
            <div class="col-xs-12 col-sm-12 col-lg-2"><%if (language.Equals("1")){ %> Date <%} else if (language.Equals("2")) { %> Ngày nhập <%} %> </div>
            <div class="col-xs-12 col-sm-12 col-lg-4">
                <asp:TextBox ID="txtNgayNhap" runat="server" Width="100%" CssClass='form-control' ></asp:TextBox>
            </div>            
        </div>
     

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
                <li class="active"><a data-toggle="tab" href="#Material"><%if (language.Equals("1")){ %> Staff list <%} else if (language.Equals("2")){%> Danh sách nhân sự <%} %></a></li>                
            </ul>

            <div class="tab-content">

                <div id="Material" class="tab-pane in active">
                    <table  style="width:100%; font-size:small;" class="table table-hover table-bordered table-striped">
                        <tr>
                            <th style="text-align:center; width:3%;">#</th>
                            <th style="text-align:center; width:6%;"><%if (language.Equals("1")){ %> Branch <%} else if (language.Equals("2")){%> Chi nhánh  <%}%></th>
                            <th style="text-align:center; width:10%;"><%if (language.Equals("1")){ %> Department <%} else if (language.Equals("2")){%> Phòng ban  <%}%></th>
                            <th style="text-align:center; width:12%;"><%if (language.Equals("1")){ %> Position <%} else if (language.Equals("2")){%> Chức vụ <%} %></th>
                            <th style="text-align:center; width:8%;"><%if (language.Equals("1")){ %> Code <%} else if (language.Equals("2")){%> Mã <%} %></th> 
                            <th style="text-align:center; width:20%;"><%if (language.Equals("1")){ %> Name <%} else if (language.Equals("2")){%> Nhân viên <%} %></th> 
                            <th style="text-align:center; width:8%;"><%if (language.Equals("1")){ %> Result <%} else if (language.Equals("2")){%> Result  <%}%></th>
                            <th style="text-align:center; width:8%;"><%if (language.Equals("1")){ %> Normal over <%} else if (language.Equals("2")){%> Normal over  <%}%></th>
                            <th style="text-align:center; width:8%;"><%if (language.Equals("1")){ %> Night <%} else if (language.Equals("2")){%> Night  <%}%></th>
                            <th style="text-align:center; width:8%;"><%if (language.Equals("1")){ %> Night over <%} else if (language.Equals("2")){%> Night over  <%}%></th>
                            <th style="text-align:center; width:8%;"><%if (language.Equals("1")){ %> Holiday <%} else if (language.Equals("2")){%> Holiday  <%}%></th>
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
        
        var nhansu_fk = document.getElementsByName("nhansu_fk");
        var result = document.getElementsByName("result");
        var normalOver = document.getElementsByName("normalOver");
        var night = document.getElementsByName("night");
        var nightOver = document.getElementsByName("nightOver");
        var holiday = document.getElementsByName("holiday");
       
        var nhansu = "";
        for (var i = 0; i < nhansu_fk.length; i++) {

            var _nhansu_fk = nhansu_fk.item(i).value;
    
            var _result = result.item(i).value.replace(/,/g, "");
            var _normalOver = normalOver.item(i).value.replace(/,/g, "");
            var _night = night.item(i).value.replace(/,/g, "");
            var _nightOver = nightOver.item(i).value.replace(/,/g, "");
            var _holiday = holiday.item(i).value.replace(/,/g, "");

            if (_result == "")
                _result = "0";
            if (_normalOver == "")
                _normalOver = "0";

            if (_night == "")
                _night = "0";
            if (_nightOver == "")
                _nightOver = "0";
            if (_holiday == "")
                _holiday = "0";

            if (_nhansu_fk != "" && _nhansu_fk.length > 1) {
                nhansu += _nhansu_fk + '_' + _result + '_' + _normalOver + '_' + _night + '_' + _nightOver + '_' + _holiday + ';';
            }
        }
        
      

        //Multi click
        var divControl = document.getElementById('control01');
        var divLoading = document.getElementById('loading01');

        divControl.style.display = 'none';
        divLoading.style.display = '';

        $.post("Hander/hdMasterData.ashx?action=saveStaff&nhansu=" + nhansu, function (result) {
                if (result.length < 10) {

            <%if (language.Equals("1")){ %> alert('Save successfully'); <%} else if (language.Equals("2")){ %> alert('Lưu thành công'); <%} %>   
                    window.location.replace("MasterData.aspx?func=6");

                }
                else {

                    divControl.style.display = '';
                    divLoading.style.display = 'none';

                    alert(result);
                }

            });
    }

</script>
