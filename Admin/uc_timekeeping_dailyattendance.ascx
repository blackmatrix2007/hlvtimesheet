<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_timekeeping_dailyattendance.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_timekeeping_dailyattendance" %>


<section class="content">


<div class="box box-primary">
    <div class="box-header with-border" style='background-color: #dd4b39 !important; display:none;'>
    <h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")){ %> Information of Receiving and Shipping plan  <%} else if (language.Equals("2")) { %> Thông tin kế hoạch xuất-nhập <%} %> </b></h3>
    </div>
    <div class="box-body">

        <div class="row">
        <div class="col-xs-3" id='control01' style='text-align:left;' > 
            <a class="btn btn-primary" href="Index.aspx"><span class="glyphicon glyphicon-menu-left" aria-hidden="true"></span> 
                 <%if (language.Equals("1")){ %> Back <%} else if (language.Equals("2")){ %>Quay lại<%} %>
            </a>

             <a class="btn btn-info" href="javascript:ExportExcel();" style="display:none;"><span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span> 
                  <%if (language.Equals("1")){ %> Report Production plan <%} else if (language.Equals("2")){ %> Báo cáo kế hoạch sản xuất <%} %>
             </a>

        </div>
         <div class="col-xs-4"> 
             <div style="text-align:center; float:left; width:20%; height:30px; border:0.1px solid black; font-size:smaller; padding-top:5px; background-color: lightgreen;"> Fulltime</div>
             <div style="text-align:center; float:left; width:20%; height:30px; border:0.1px solid black; font-size:smaller; padding-top:5px; background-color: yellow;"> Hafltime</div>
             <div style="text-align:center; float:left; width:20%; height:30px; border:0.1px solid black; font-size:smaller; padding-top:5px; background-color: orange;"> Approved leave</div>
             <div style="text-align:center; float:left; width:20%; height:30px; border:0.1px solid black; font-size:smaller; padding-top:5px; background-color: orangered;"> AB</div>
             <div style="text-align:center; float:left; width:20%; height:30px; border:0.1px solid black; font-size:smaller; padding-top:5px; background-color: lightgray;"> Late arrival</div>
         </div>
          <div class="col-xs-1" style="display:none;">
              <input type="text" class="form-control datepicker" id="txtTuNgay"  runat="server" data-toggle="tooltip" title="From date" autocomplete="off">
          </div>
       
          <div class="col-xs-1"  style="display:none;">
              <input type="text" class="form-control datepicker" id="txtDenNgay"  runat="server" data-toggle="tooltip" title="To date" autocomplete="off">
          </div>

            <div class="col-xs-1" style="display:none;">
                <asp:DropDownList ID="ddlPhienBan" runat="server" 
                    AutoPostBack="True" onselectedindexchanged="ddlPhienBan_SelectedIndexChanged" CssClass='form-control select2' ></asp:DropDownList>
            </div>

          <div class="col-xs-2">
              <asp:DropDownList ID="ddlPhongBan" runat="server" 
                    AutoPostBack="True" onselectedindexchanged="ddlPhongBan_SelectedIndexChanged" CssClass='form-control select2' ></asp:DropDownList>
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

    <div class="row" style="margin-top:5px; display:none;">
        <div class="col-xs-2" style="display:none;"><%if (language.Equals("1")){ %> Days in Month <%} else if (language.Equals("2")){ %> Số ngày trong tháng <%} %> </div>
        <div class="col-xs-10" style="display:none;">
            <input type="text" class="form-control" id="txtSoNgay"  runat="server" readonly>
        </div>
        </div>

        <div class="row" style="margin-top:10px;  margin-left:0.5%; font-size:small;">               
            <div style="height:auto; overflow:auto; margin-bottom:-20px; background-color:white;" id="divDetail">
            <table style="width:99%; max-height:auto; font-size:smaller; overflow:scroll;">
                <asp:Literal ID="ltInfor" runat="server"></asp:Literal>
            </table>
        </div>
        <div class="box-footer clearfix"  style="margin-top:25px; text-align:center;">
            <asp:Literal ID="ltPhanTrang" runat="server"></asp:Literal>
        </div>
     </div>
        
    </div><!-- /.box-body -->

     <div class="modal fade showInforStaff" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel">
        <input type="hidden" id="timenow" name="timenow" value=""/>
	    <div class="modal-dialog" role="document" style="width: 800px;">
	    <div class="modal-content">
		    <div class="modal-header modal-primary">
		    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
		    <h4 class="modal-title" id="exampleModalLabel">Information detail</h4>
		    </div>

		    <div class="row modal-body">
                <div class="col-xs-12 col-sm-12 col-lg-12">
                   <div class="col-xs-12 col-sm-4 col-lg-3">
                    <div class="row" style="text-align:center;">
                        <div class="col-xs-12 col-sm-12 col-lg-12">
                            <div style="text-align:center; height:110px; width:100px; " id="divHinhUpLoad" runat="server"></div>
                        </div>
                    </div>
                  </div>
                   <div class="col-xs-12 col-sm-8 col-lg-9">
                        <div class="row">
                            <div class="col-xs-12 col-sm-3 col-lg-2"  style="margin-top:10px;"><%if (language.Equals("1")){ %>Staff <%} else if (language.Equals("2")) { %> Nhân viên <%} %></div>
                             <div class="col-xs-12 col-sm-9 col-lg-10"  style="margin-top:10px;"> 
                                 <asp:TextBox ID="txtTen" runat="server" Width="100%" CssClass='form-control clearBoder' ReadOnly="true"></asp:TextBox>
                                 <input type="hidden" class="form-control" id="txtNhanSu" runat="server" readonly="readonly" />
                              </div>
                        </div>
                       <div class="row">
                            <div class="col-xs-12 col-sm-3 col-lg-2" style="margin-top:10px;"><%if (language.Equals("1")){ %> Branch <%} else if (language.Equals("2")) { %> Chi nhánh <%} %></div>
                            <div class="col-xs-12 col-sm-9 col-lg-10" style="margin-top:10px;"> 
                                <asp:DropDownList ID="ddlChiNhanh" runat="server" Width="100%" CssClass='form-control clearBoder' readonly="readonly"></asp:DropDownList>
                            </div>
                        </div>

                         <div class="row">
                            <div class="col-xs-12 col-sm-3 col-lg-2" style="margin-top:10px;"><%if (language.Equals("1")){ %> Position <%} else if (language.Equals("2")) { %> Chức vụ <%} %></div>
                            <div class="col-xs-12 col-sm-9 col-lg-10" style="margin-top:10px;"> 
                                <asp:DropDownList ID="ddlChucVu" runat="server" Width="100%" CssClass='form-control clearBoder' readonly="readonly"></asp:DropDownList>
                            </div>
                        </div>

                         <div class="row">
                            <div class="col-xs-12 col-sm-3 col-lg-2" style="margin-top:10px;"><%if (language.Equals("1")){ %> Department <%} else if (language.Equals("2")) { %> Phòng ban <%} %></div>
                            <div class="col-xs-12 col-sm-9 col-lg-10" style="margin-top:10px;">  
                                <asp:DropDownList ID="ddlPhongBanShow" runat="server" Width="100%" CssClass='form-control clearBoder' readonly="readonly"></asp:DropDownList>
                            </div>               
                        </div>                     
                       <div class="row" style="display:none;">
                            <div class="col-xs-12 col-sm-3 col-lg-2" style="margin-top:10px;"><%if (language.Equals("1")){ %> Start date <%} else if (language.Equals("2")) { %> Ngày bắt đầu làm <%} %></div>
                            <div class="col-xs-12 col-sm-9 col-lg-10" style="margin-top:10px;">                                 
                                <asp:TextBox ID="txtNgayBatDauLam" runat="server" Width="100%" CssClass='form-control clearBoder' ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        
                         <div class="row showSupport" style="display:none;">
                            <div class="col-xs-12 col-sm-3 col-lg-2" style="margin-top:10px;"><%if (language.Equals("1")){ %> Support <%} else if (language.Equals("2")) { %> Hỗ trợ <%} %></div>
                            <div class="col-xs-12 col-sm-9 col-lg-10" style="margin-top:10px;">                                  
                                <select id="ddlPhongBanSupport" class='form-control clearBoder' runat="server" style="width:100%;" onchange="ChangeStatus();"></select>
                            </div>               
                        </div>
                        
                       <div class="row">
                             <div class="col-xs-12 col-sm-3 col-lg-2" style="margin-top:10px;"><%if (language.Equals("1")){ %> Date <%} else if (language.Equals("2")) { %> Ngày nhập <%} %></div>
                             <div class="col-xs-12 col-sm-9 col-lg-10" style="margin-top:10px;"> 
                                 <asp:TextBox ID="txtNgayNhapShow" runat="server" Width="100%" CssClass='form-control' ReadOnly="true"></asp:TextBox>
                             </div>
                         </div>

                       <div class="row">
                           <div class="col-xs-12 col-sm-3 col-lg-2" style="margin-top:10px;"><%if (language.Equals("1")){ %> Check in <%} else if (language.Equals("2")) { %> Check in <%} %></div>
                           <div class="col-xs-12 col-sm-9 col-lg-10" style="margin-top:10px;">  
                                <asp:DropDownList ID="ddlGioStart" style="color:blue; float:left; padding:0px;" runat="server" Width="16%" CssClass='form-control' ></asp:DropDownList>
                                <asp:DropDownList ID="ddlPhutStart" style="color:blue; float:left; padding:0px;" runat="server" Width="16%" CssClass='form-control' ></asp:DropDownList>
                                <input type="text" class="form-control" id="txtSpace" runat="server" readonly="readonly" style="border:0px; float:left; text-align:center; width:35.5%;">
                                <asp:DropDownList ID="ddlGioEnd" style="color:red; float:left; padding:0px;" runat="server" Width="16%" CssClass='form-control' ></asp:DropDownList>
                                <asp:DropDownList ID="ddlPhutEnd" style="color:red; padding:0px;" runat="server" Width="16%" CssClass='form-control' ></asp:DropDownList>
                           </div>
                       </div>
                       
                  </div>
                </div>
                <div class="col-xs-12 col-sm-12 col-lg-12"> 
                    <div class="col-xs-12 col-sm-6 col-lg-6" style="margin-top:10px;"> 
                        <div style="text-align:center; height:250px; width:70%; margin-left:15%;" id="divImageCheckIn" runat="server"></div>
                        <div class="overlay-text" style="text-align:center;">Check in </div>
                    </div>

                    <div class="col-xs-12 col-sm-6 col-lg-6" style="margin-top:10px;"> 
                        <div style="text-align:center; height:250px; width:70%; margin-left:15%;" id="divImageCheckOut" runat="server"></div>
                        <div class="overlay-text" style="text-align:center;">Check out</div>
                    </div>
                </div>
		    </div>

            <div id='control02' class="modal-footer" style="margin-top:10px;">
		        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
		        <button type="button" class="btn btn-primary" onclick='javascript:SaveInforStaff();' id='btnLuuthongtin' >Save</button>
            </div>

            <div id='loading02' class="box box-danger box-solid" style='display:none;' >
                <div class="box-header">
                    <h3 class="box-title">The system is processing your request. Please wait a moment.</h3>
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

<script type="text/javascript" >

    $('#exampleModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget) // Button that triggered the modal        
        var recipient = button.data('whatever') // Extract info FROM data-* attributes

        var ngaynhap = recipient.split('--')[0];
        var nhansu = recipient.split('--')[1];

        GetInfoStaff(nhansu, ngaynhap);
        
    })

    
    
    function GetInfoStaff(nhansu, ngaynhap) {
        if (typeof nhansu !== 'undefined') {

            var action = "viewStaff";
            $.post("Hander/hdTimeKeeping.ashx?action=" + action + "&nhansu=" + nhansu + "&ngaynhap=" + ngaynhap, function (result) {

                if (result.length > 10) {

                    var arr = result.split(" -- ");
                    document.getElementById("<%= txtNgayNhapShow.ClientID %>").value = ngaynhap;

                    document.getElementById("<%= txtNhanSu.ClientID %>").value = arr[0];
                    document.getElementById("<%= txtTen.ClientID %>").value = arr[1];

                    document.getElementById("<%= txtNgayBatDauLam.ClientID %>").value = arr[7];
                    
                    document.getElementById("<%= ddlChiNhanh.ClientID %>").value = arr[9];
                    document.getElementById("<%= ddlPhongBanShow.ClientID %>").value = arr[10];
                    document.getElementById("<%= ddlPhongBanSupport.ClientID %>").value = arr[11];
                    document.getElementById("<%= ddlChucVu.ClientID %>").value = arr[12];

                    var gioIn = arr[13];
                    var phutIn = arr[14];
                    var gioOut = arr[15];
                    var phutOut = arr[16];
                    var hinhanhIn = arr[18];
                    var hinhanhOut = arr[19];

                    if (gioIn.length < 2)
                        gioIn = "0" + gioIn;
                    if (phutIn.length < 2)
                        phutIn = "0" + phutIn;
                    if (gioOut.length < 2)
                        gioOut = "0" + gioOut;
                    if (phutOut.length < 2)
                        phutOut = "0" + phutOut;

                    if (hinhanhIn.length < 3)
                        hinhanhIn = "avatardefault.png";
                    if (hinhanhOut.length < 3)
                        hinhanhOut = "avatardefault.png";

                    document.getElementById("<%= ddlGioStart.ClientID %>").value = gioIn;
                    document.getElementById("<%= ddlPhutStart.ClientID %>").value = phutIn;
                    document.getElementById("<%= ddlGioEnd.ClientID %>").value = gioOut;
                    document.getElementById("<%= ddlPhutEnd.ClientID %>").value = phutOut;

                    document.getElementById("<%= divHinhUpLoad.ClientID %>").innerHTML = "<img src='https://hlv-ws-ssl.giangdc.company/Admin/Avatar/" + arr[17] + "' style='max-height:110px; max-width:110px' />";

                    document.getElementById("<%= divImageCheckIn.ClientID %>").innerHTML = "<img src='https://hlv-ws-ssl.giangdc.company/Admin/Avatar/" + hinhanhIn + "' style='max-height:100%; max-width:100%' />";
                    document.getElementById("<%= divImageCheckOut.ClientID %>").innerHTML = "<img src='https://hlv-ws-ssl.giangdc.company/Admin/Avatar/" + hinhanhOut + "' style='max-height:100%; max-width:100%' />";
                }
            });
        }
    }

    function SaveInforStaff() {

        var nhansu = document.getElementById("<%= txtNhanSu.ClientID %>").value;        
        var phongban = document.getElementById("<%= ddlPhongBanShow.ClientID %>").value;
        var ngaynhap = document.getElementById("<%= txtNgayNhapShow.ClientID %>").value;
        var gioIn = document.getElementById("<%= ddlGioStart.ClientID %>").value;
        var phutIn = document.getElementById("<%= ddlPhutStart.ClientID %>").value;
        var gioOut = document.getElementById("<%= ddlGioEnd.ClientID %>").value;
        var phutOut = document.getElementById("<%= ddlPhutEnd.ClientID %>").value;
        
        $.post("Hander/hdTimeKeeping.ashx?action=saveInforCheckIn&nhansu=" + nhansu + "&ngaynhap=" + ngaynhap + "&phongban=" + phongban +
            "&gioIn=" + gioIn + "&phutIn=" + phutIn + "&gioOut=" + gioOut + "&phutOut=" + phutOut,
            function (result) {
                if (result.length < 10) {

                    $('.showInforStaff').css("display", "none");

                    window.location("TimeSheet.aspx?func=301&action=capnhat&depa=" + phongban);

                }
                else
                    alert(result);
            });
    }


    $(document).ready(function () {
        
    });

</script>

</section><!-- /.cont