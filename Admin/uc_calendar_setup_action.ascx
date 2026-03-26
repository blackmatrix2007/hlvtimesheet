<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_calendar_setup_action.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_calendar_setup_action" %>

<style>
   .textboxStart { float:left; padding:0px;}
</style>

<style>
   .textboxEnd { float:left; margin-left: 4%; padding:0px;}
</style>


<!-- Main content -->
<link href="../Content/Styles/colorbox.css" rel="stylesheet" type="text/css" />
<script src="../Content/Scripts/jquery.colorbox.js" type="text/javascript"></script>

<script src="../Content/Scripts/ajax.js" type="text/javascript"></script>
<script src="Autocomplete/ajaxCtkm.js" type="text/javascript"></script>
<script src="Autocomplete/ajaxFilter.js" type="text/javascript"></script>

<section class="content">
<input type="hidden" id="language" name="language" value="<%= language %>" />
<input type="hidden" id="OffThang01" name="OffThang01" value="<%= OffThang01 %>" />
<input type="hidden" id="OffThang02" name="OffThang02" value="<%= OffThang02 %>" />
<input type="hidden" id="OffThang03" name="OffThang03" value="<%= OffThang03 %>" />
<input type="hidden" id="OffThang04" name="OffThang04" value="<%= OffThang04 %>" />
<input type="hidden" id="OffThang05" name="OffThang05" value="<%= OffThang05 %>" />
<input type="hidden" id="OffThang06" name="OffThang06" value="<%= OffThang06 %>" />
<input type="hidden" id="OffThang07" name="OffThang07" value="<%= OffThang07 %>" />
<input type="hidden" id="OffThang08" name="OffThang08" value="<%= OffThang08 %>" />
<input type="hidden" id="OffThang09" name="OffThang09" value="<%= OffThang09 %>" />
<input type="hidden" id="OffThang10" name="OffThang10" value="<%= OffThang10 %>" />
<input type="hidden" id="OffThang11" name="OffThang11" value="<%= OffThang11 %>" />
<input type="hidden" id="OffThang12" name="OffThang12" value="<%= OffThang12 %>" />

<div class="box box-primary"  style="font-size:small;">    
    <div class="box-header with-border" style='background-color: #dd4b39 !important; display:none;' >
    <h3 class="box-title" ><b style='color: #FFF;' >Information of item</b></h3>
    </div>
    <div class="box-body">
             
        <div class="row">
        <div class="col-xs-6" id='control01' style='text-align:left;'> 
            <a class="btn btn-primary" href="Index.aspx"><span class="glyphicon glyphicon-menu-left" aria-hidden="true"></span> <%if (language.Equals("1")){ %>  Back <%} else if (language.Equals("2")){%> Quay lại  <%} %> </a>            
        </div>               

        <div class="col-xs-2">
            <asp:DropDownList ID="ddlPhongBan" runat="server" AutoPostBack="True" onselectedindexchanged="ddlPhongBan_SelectedIndexChanged" CssClass='form-control' ></asp:DropDownList>
        </div>    
        <div class="col-xs-2">
            <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="True" onselectedindexchanged="ddlYear_SelectedIndexChanged" CssClass='form-control' ></asp:DropDownList>
        </div>    
        <div class="col-xs-2">
            <asp:TextBox ID="txtNgayNhap" runat="server" CssClass='form-control datepicker'></asp:TextBox>
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
        
        <div class="col-xs-12 col-sm-12 col-lg-6"> 

            <div class="row" style="margin-top:5px;">
                <div class="col-xs-3"><%if (language.Equals("1")){ %> Sun off<%} else if (language.Equals("2")){ %> Chủ nhật<%} %>: <asp:Label ID="lblSoNgayChuNhat" runat="server" ForeColor="Red" Font-Size="Medium" Font-Bold="true"></asp:Label></div>
                <div class="col-xs-3"><%if (language.Equals("1")){ %> National holiday<%} else if (language.Equals("2")){ %> Nghỉ lễ<%} %>: <asp:Label ID="lbltSoNgayNghiLe" runat="server" ForeColor="Red" Font-Size="Medium" Font-Bold="true"></asp:Label></div>
                <div class="col-xs-3"><%if (language.Equals("1")){ %> Company holiday<%} else if (language.Equals("2")){ %> Ngày nghỉ công ty<%} %>: <asp:Label ID="lblSoNgayNghiCongTy" runat="server" ForeColor="Orange" Font-Size="Medium" Font-Bold="true"></asp:Label></div>
                <div class="col-xs-3"><%if (language.Equals("1")){ %> Orther<%} else if (language.Equals("2")){ %> Khác<%} %>: <asp:Label ID="lblSoNgayNghiKhac" runat="server" ForeColor="Green" Font-Size="Medium" Font-Bold="true"></asp:Label></div>
            </div>

        </div>

         <div class="col-xs-12 col-sm-12 col-lg-6"> 
             <div class="box-body table-responsive no-padding" style="margin-top:5px;">
                <table class="table table-hover table-bordered table-striped" style="font-size:smaller">
                <tr>
                     <%if (language.Equals("1")){ %>			    
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">JAN</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">FEB</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">MAR</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">APR</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">MAY</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">JUN</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">JUL</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">AUG</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">SEP</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">OCT</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">NOV</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">DEC</th>
                        <th style="text-align:center; border:1px solid black;  width:7%;">Total (day)</th>
                        <th style="text-align:center; border:1px solid black;  width:7%;">Holiday</th>
                        <th style="text-align:center; border:1px solid black;  width:7%;">Total (H)</th>
                        <% } else if (language.Equals("2")){  %>                
		                <th style="text-align:center; border:1px solid black;  width:6.5%;">JAN</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">FEB</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">MAR</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">APR</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">MAY</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">JUN</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">JUL</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">AUG</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">SEP</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">OCT</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">NOV</th>
                        <th style="text-align:center; border:1px solid black;  width:6.5%;">DEC</th>
                        <th style="text-align:center; border:1px solid black;  width:7%;">Total (day)</th>
                        <th style="text-align:center; border:1px solid black;  width:7%;">Holiday</th>
                        <th style="text-align:center; border:1px solid black;  width:7%;">Total (H)</th>
                        <%} %>
                </tr>

                <asp:Literal ID="ltYear" runat="server"></asp:Literal>
   
                </table>
            </div><!-- /.box-body -->
        </div>


    </div><!-- /.box-body -->

    <div class="row">
        <div class="col-md-12">
            <div class="box box-primary" style="font-size:small;">
            <div class="box-header with-border" style="display:none;">
                <h3 class="box-title">Setup</h3>
                <div class="box-tools pull-right">                    
                <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                </div>
            </div><!-- /.box-header -->

            <div class="box-body">
                            
                <div class="col-xs-12 col-sm-6 col-lg-3" style="padding:0px;"> 
                    <div class="col-xs-12"><%if (language.Equals("1")){ %> JAN <%} else if (language.Equals("2")){ %> Tháng 01<%} %></div>
                    <div class="col-xs-12" style="width:100%; overflow:auto;" id="divThang01"></div>
                </div>
                <div class="col-xs-12 col-sm-6 col-lg-3" style="padding:0px;"> 
                    <div class="col-xs-12"><%if (language.Equals("1")){ %> FEB <%} else if (language.Equals("2")){ %> Tháng 02<%} %></div>
                    <div class="col-xs-12" style="width:100%; overflow:auto;" id="divThang02"></div>
                </div>
                <div class="col-xs-12 col-sm-6 col-lg-3" style="padding:0px;"> 
                    <div class="col-xs-12"><%if (language.Equals("1")){ %> MAR <%} else if (language.Equals("2")){ %> Tháng 03<%} %></div>
                    <div class="col-xs-12" style="width:100%; overflow:auto;" id="divThang03"></div>
                </div>
                <div class="col-xs-12 col-sm-6 col-lg-3" style="padding:0px;"> 
                    <div class="col-xs-12"><%if (language.Equals("1")){ %> APR <%} else if (language.Equals("2")){ %> Tháng 04<%} %></div>
                    <div class="col-xs-12" style="width:100%; overflow:auto;" id="divThang04"></div>
                </div>

                <div class="col-xs-12 col-sm-6 col-lg-3" style="padding:0px;"> 
                    <div class="col-xs-12"><%if (language.Equals("1")){ %> MAY <%} else if (language.Equals("2")){ %> Tháng 05<%} %></div>
                    <div class="col-xs-12" style="width:100%; overflow:auto;" id="divThang05"></div>
                </div>
                <div class="col-xs-12 col-sm-6 col-lg-3" style="padding:0px;"> 
                    <div class="col-xs-12"><%if (language.Equals("1")){ %> JUN <%} else if (language.Equals("2")){ %> Tháng 06<%} %></div>
                    <div class="col-xs-12" style="width:100%; overflow:auto;" id="divThang06"></div>
                </div>
                <div class="col-xs-12 col-sm-6 col-lg-3" style="padding:0px;"> 
                    <div class="col-xs-12"><%if (language.Equals("1")){ %> JUL <%} else if (language.Equals("2")){ %> Tháng 07<%} %></div>
                    <div class="col-xs-12" style="width:100%; overflow:auto;" id="divThang07"></div>
                </div>
                <div class="col-xs-12 col-sm-6 col-lg-3" style="padding:0px;"> 
                    <div class="col-xs-12"><%if (language.Equals("1")){ %> AUG <%} else if (language.Equals("2")){ %> Tháng 08<%} %></div>
                    <div class="col-xs-12" style="width:100%; overflow:auto;" id="divThang08"></div>
                </div>

                <div class="col-xs-12 col-sm-6 col-lg-3" style="padding:0px;"> 
                    <div class="col-xs-12"><%if (language.Equals("1")){ %> SEP <%} else if (language.Equals("2")){ %> Tháng 09<%} %></div>
                    <div class="col-xs-12" style="width:100%; overflow:auto;" id="divThang09"></div>
                </div>
                <div class="col-xs-12 col-sm-6 col-lg-3" style="padding:0px;"> 
                    <div class="col-xs-12"><%if (language.Equals("1")){ %> OCT <%} else if (language.Equals("2")){ %> Tháng 10<%} %></div>
                    <div class="col-xs-12" style="width:100%; overflow:auto;" id="divThang10"></div>
                </div>
                <div class="col-xs-12 col-sm-6 col-lg-3" style="padding:0px;"> 
                    <div class="col-xs-12"><%if (language.Equals("1")){ %> NOV <%} else if (language.Equals("2")){ %> Tháng 11<%} %></div>
                    <div class="col-xs-12" style="width:100%; overflow:auto;" id="divThang11"></div>
                </div>
                <div class="col-xs-12 col-sm-6 col-lg-3" style="padding:0px;"> 
                    <div class="col-xs-12"><%if (language.Equals("1")){ %> DEC <%} else if (language.Equals("2")){ %> Tháng 12<%} %></div>
                    <div class="col-xs-12" style="width:100%; overflow:auto;" id="divThang12"></div>
                </div>

            </div><!-- ./box-body -->
                
            </div><!-- /.box -->
        </div><!-- /.col -->         
    </div>

     <div class="row">
          <div class="col-xs-12">
               <div class="col-xs-3"></div>
               
              <div class="col-xs-2"> <%if (language.Equals("1")){ %> Working time <%} else if (language.Equals("2")) { %> Thời gian làm việc <%} %> </div>
              <div class="col-xs-2">
                    <asp:DropDownList ID="ddlGioStart" runat="server" Width="40%" CssClass='form-control textboxEnd' ></asp:DropDownList>
                    <asp:DropDownList ID="ddlPhutStart" runat="server" Width="40%" CssClass='form-control textboxStart' ></asp:DropDownList>
              </div>
             
               <div class="col-xs-2">
                   <asp:DropDownList ID="ddlGioEnd" runat="server" Width="40%" CssClass='form-control textboxEnd' ></asp:DropDownList>
                   <asp:DropDownList ID="ddlPhutEnd" runat="server" Width="40%" CssClass='form-control textboxStart' ></asp:DropDownList>
             </div>

              <div class="col-xs-3"></div>
          </div> 
    </div>

     <div class="row">
      <div class="col-xs-12">
           <div class="col-xs-3"></div>
           
          <div class="col-xs-2"> <%if (language.Equals("1")){ %> Break 01 <%} else if (language.Equals("2")) { %> Thời gian nghỉ 01 <%} %> </div>
          <div class="col-xs-2">
                <asp:DropDownList ID="ddlBreakOneHourStart" runat="server" Width="40%" CssClass='form-control textboxEnd' ></asp:DropDownList>
                <asp:DropDownList ID="ddlBreakOneMinuteStart" runat="server" Width="40%" CssClass='form-control textboxStart' ></asp:DropDownList>
          </div>
         
           <div class="col-xs-2">
               <asp:DropDownList ID="ddlBreakOneHourEnd" runat="server" Width="40%" CssClass='form-control textboxEnd' ></asp:DropDownList>
               <asp:DropDownList ID="ddlBreakOneMinuteEnd" runat="server" Width="40%" CssClass='form-control textboxStart' ></asp:DropDownList>
         </div>

          <div class="col-xs-3"></div>
      </div> 
     </div>

    <div class="row">
    <div class="col-xs-12">
        <div class="col-xs-3"></div>
      
        <div class="col-xs-2"> <%if (language.Equals("1")){ %> Lunch break <%} else if (language.Equals("2")) { %> Giờ nghỉ trưa <%} %> </div>
        <div class="col-xs-2">
            <asp:DropDownList ID="ddlLunchBreakHourStart" runat="server" Width="40%" CssClass='form-control textboxEnd' ></asp:DropDownList>
            <asp:DropDownList ID="ddlLunchBreakMinuteStart" runat="server" Width="40%" CssClass='form-control textboxStart' ></asp:DropDownList>
        </div>
    
        <div class="col-xs-2">
            <asp:DropDownList ID="ddlLunchBreakHourEnd" runat="server" Width="40%" CssClass='form-control textboxEnd' ></asp:DropDownList>
            <asp:DropDownList ID="ddlLunchBreakMinuteEnd" runat="server" Width="40%" CssClass='form-control textboxStart' ></asp:DropDownList>
    </div>

        <div class="col-xs-3"></div>
    </div> 
    </div>

     <div class="row">
      <div class="col-xs-12">
           <div class="col-xs-3"></div>
           
          <div class="col-xs-2"> <%if (language.Equals("1")){ %> Break 02 <%} else if (language.Equals("2")) { %> Thời gian nghỉ 02 <%} %> </div>
          <div class="col-xs-2">
                <asp:DropDownList ID="ddlBreakTwoHourStart" runat="server" Width="40%" CssClass='form-control textboxEnd' ></asp:DropDownList>
                <asp:DropDownList ID="ddlBreakTwoMinuteStart" runat="server" Width="40%" CssClass='form-control textboxStart' ></asp:DropDownList>
          </div>
         
           <div class="col-xs-2">
               <asp:DropDownList ID="ddlBreakTwoHourEnd" runat="server" Width="40%" CssClass='form-control textboxEnd' ></asp:DropDownList>
               <asp:DropDownList ID="ddlBreakTwoMinuteEnd" runat="server" Width="40%" CssClass='form-control textboxStart' ></asp:DropDownList>
         </div>

          <div class="col-xs-3"></div>
      </div> 
    </div>

     <div class="row">
      <div class="col-xs-12">
           <div class="col-xs-3"></div>
           
          <div class="col-xs-2"> <%if (language.Equals("1")){ %> Over time <%} else if (language.Equals("2")) { %> Tăng ca <%} %> </div>
          <div class="col-xs-2">
                <asp:DropDownList ID="ddlOverTimeHourStart" runat="server" Width="40%" CssClass='form-control textboxEnd' ></asp:DropDownList>
                <asp:DropDownList ID="ddlOverTimeMinuteStart" runat="server" Width="40%" CssClass='form-control textboxStart' ></asp:DropDownList>
          </div>
         
           <div class="col-xs-2">
               <asp:DropDownList ID="ddlOverTimeHourEnd" runat="server" Width="40%" CssClass='form-control textboxEnd' ></asp:DropDownList>
               <asp:DropDownList ID="ddlOverTimeMinuteEnd" runat="server" Width="40%" CssClass='form-control textboxStart' ></asp:DropDownList>
         </div>

          <div class="col-xs-3"></div>
      </div> 
    </div>

     <div class="row" style="margin-bottom:10px;">
      <div class="col-xs-12">
           <div class="col-xs-3"></div>
       
          <div class="col-xs-6">
              <asp:LinkButton ID="lbLuuLai" runat="server" CssClass="btn btn-primary" onclick="lbLuuLai_Click"><span class="glyphicon glyphicon-floppy-save" aria-hidden="true"></span> <%if (language.Equals("1")){ %> Save <%} else if (language.Equals("2")) { %> Lưu thông tin <%} %> </asp:LinkButton>
          </div>
          
          <div class="col-xs-3"></div>
      </div> 
    </div>

    <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel">
	<div class="modal-dialog" role="document">
	<div class="modal-content">
		<div class="modal-header">
		<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
		<h4 class="modal-title" id="exampleModalLabel"><%if (language.Equals("1")){ %>Information detail <%} else if (language.Equals("2")) { %> Thông tin chi tiết <%} %></h4>
		</div>
		<div class="modal-body">
		<form>
			<div class="form-group">
			<label for="recipient-name" class="control-label"><%if (language.Equals("1")){ %> Date <%} else if (language.Equals("2")) { %> Ngày <%} %> :</label>
			<input type="text" class="form-control" id="txtThoiGian" runat="server" readonly="readonly">            
			</div>
			
            <div class="form-group">
			<label for="message-text" class="control-label"><%if (language.Equals("1")){ %> Type <%} else if (language.Equals("2")) { %>Loại <%} %>:</label>
            <asp:DropDownList ID="ddlLoaiNgayNghi" runat="server" Width="100%" CssClass='form-control' ></asp:DropDownList>
			</div>
           
		</form>
		</div>
		
        <div id='control02' class="modal-footer">
             <div class="col-xs-12 col-sm-6 col-lg-6" style="text-align:left;"> 
		        <button type="button" class="btn btn-danger" onclick='javascript:DeleteDatetime();' id='btnLuuthongtinDelete' ><%if (language.Equals("1")){ %> Delete <%} else if (language.Equals("2")) { %> Xóa thông tin <%} %></button>
            </div>
             <div class="col-xs-12 col-sm-6 col-lg-6" style="text-align:right;"> 
                <button type="button" class="btn btn-default" data-dismiss="modal"><%if (language.Equals("1")){ %> Close <%} else if (language.Equals("2")) { %> Đóng lại <%} %></button>
		        <button type="button" class="btn btn-primary" onclick='javascript:SaveInfoDatetime();' id='btnLuuthongtin' ><%if (language.Equals("1")){ %> Save <%} else if (language.Equals("2")) { %> Lưu thông tin <%} %></button>
            </div>
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
</section><!-- /.content -->


<script type="text/javascript" >

    function GetInfoMonth01() {

        var thang = "01";
        var language = document.getElementById("language").value;
        var nam = document.getElementById("<%= ddlYear.ClientID %>").value;
        var phongban = document.getElementById("<%= ddlPhongBan.ClientID %>").value;

        $.post("Hander/hdDiagramSystem.ashx?action=monthlyDivDetail&thang=" + thang + "&nam=" + nam + "&phongban=" + phongban + "&language=" + language,
            function (result) {
                document.getElementById("divThang01").innerHTML = result;
            });
    }

    function GetInfoMonth02() {

        var thang = "02";
        var language = document.getElementById("language").value;
        var nam = document.getElementById("<%= ddlYear.ClientID %>").value;
        var phongban = document.getElementById("<%= ddlPhongBan.ClientID %>").value;

        $.post("Hander/hdDiagramSystem.ashx?action=monthlyDivDetail&thang=" + thang + "&nam=" + nam + "&phongban=" + phongban + "&language=" + language,
            function (result) {
                document.getElementById("divThang02").innerHTML = result;
            });
    }

    function GetInfoMonth03() {

        var thang = "03";
        var language = document.getElementById("language").value;
        var nam = document.getElementById("<%= ddlYear.ClientID %>").value;
        var phongban = document.getElementById("<%= ddlPhongBan.ClientID %>").value;

        $.post("Hander/hdDiagramSystem.ashx?action=monthlyDivDetail&thang=" + thang + "&nam=" + nam + "&phongban=" + phongban + "&language=" + language,
            function (result) {
                document.getElementById("divThang03").innerHTML = result;
            });
    }

    function GetInfoMonth04() {

        var thang = "04";
        var language = document.getElementById("language").value;
        var nam = document.getElementById("<%= ddlYear.ClientID %>").value;
        var phongban = document.getElementById("<%= ddlPhongBan.ClientID %>").value;

        $.post("Hander/hdDiagramSystem.ashx?action=monthlyDivDetail&thang=" + thang + "&nam=" + nam + "&phongban=" + phongban + "&language=" + language,
            function (result) {
                document.getElementById("divThang04").innerHTML = result;
            });
    }

    function GetInfoMonth05() {

        var thang = "05";
        var language = document.getElementById("language").value;
        var nam = document.getElementById("<%= ddlYear.ClientID %>").value;
        var phongban = document.getElementById("<%= ddlPhongBan.ClientID %>").value;

        $.post("Hander/hdDiagramSystem.ashx?action=monthlyDivDetail&thang=" + thang + "&nam=" + nam + "&phongban=" + phongban + "&language=" + language,
            function (result) {
                document.getElementById("divThang05").innerHTML = result;
            });
    }

    function GetInfoMonth06() {

        var thang = "06";
        var language = document.getElementById("language").value;
        var nam = document.getElementById("<%= ddlYear.ClientID %>").value;
        var phongban = document.getElementById("<%= ddlPhongBan.ClientID %>").value;

        $.post("Hander/hdDiagramSystem.ashx?action=monthlyDivDetail&thang=" + thang + "&nam=" + nam + "&phongban=" + phongban + "&language=" + language,
            function (result) {
                document.getElementById("divThang06").innerHTML = result;
            });
    }

    function GetInfoMonth07() {

        var thang = "07";
        var language = document.getElementById("language").value;
        var nam = document.getElementById("<%= ddlYear.ClientID %>").value;
        var phongban = document.getElementById("<%= ddlPhongBan.ClientID %>").value;

        $.post("Hander/hdDiagramSystem.ashx?action=monthlyDivDetail&thang=" + thang + "&nam=" + nam + "&phongban=" + phongban + "&language=" + language,
            function (result) {
                document.getElementById("divThang07").innerHTML = result;
            });
    }

    function GetInfoMonth08() {

        var thang = "08";
        var language = document.getElementById("language").value;
        var nam = document.getElementById("<%= ddlYear.ClientID %>").value;
        var phongban = document.getElementById("<%= ddlPhongBan.ClientID %>").value;

        $.post("Hander/hdDiagramSystem.ashx?action=monthlyDivDetail&thang=" + thang + "&nam=" + nam + "&phongban=" + phongban + "&language=" + language,
            function (result) {
                document.getElementById("divThang08").innerHTML = result;
            });
    }


    function GetInfoMonth09() {

        var thang = "09";
        var language = document.getElementById("language").value;
        var nam = document.getElementById("<%= ddlYear.ClientID %>").value;
        var phongban = document.getElementById("<%= ddlPhongBan.ClientID %>").value;

        $.post("Hander/hdDiagramSystem.ashx?action=monthlyDivDetail&thang=" + thang + "&nam=" + nam + "&phongban=" + phongban + "&language=" + language,
            function (result) {
                document.getElementById("divThang09").innerHTML = result;
            });
    }

    function GetInfoMonth10() {

        var thang = "10";
        var language = document.getElementById("language").value;
        var nam = document.getElementById("<%= ddlYear.ClientID %>").value;
        var phongban = document.getElementById("<%= ddlPhongBan.ClientID %>").value;

        $.post("Hander/hdDiagramSystem.ashx?action=monthlyDivDetail&thang=" + thang + "&nam=" + nam + "&phongban=" + phongban + "&language=" + language,
            function (result) {
                document.getElementById("divThang10").innerHTML = result;
            });
    }

    function GetInfoMonth11() {

        var thang = "11";
        var language = document.getElementById("language").value;
        var nam = document.getElementById("<%= ddlYear.ClientID %>").value;
        var phongban = document.getElementById("<%= ddlPhongBan.ClientID %>").value;

        $.post("Hander/hdDiagramSystem.ashx?action=monthlyDivDetail&thang=" + thang + "&nam=" + nam + "&phongban=" + phongban + "&language=" + language,
            function (result) {
                document.getElementById("divThang11").innerHTML = result;
            });
    }

    function GetInfoMonth12() {

        var thang = "12";
        var language = document.getElementById("language").value;
        var nam = document.getElementById("<%= ddlYear.ClientID %>").value;
        var phongban = document.getElementById("<%= ddlPhongBan.ClientID %>").value;

        $.post("Hander/hdDiagramSystem.ashx?action=monthlyDivDetail&thang=" + thang + "&nam=" + nam + "&phongban=" + phongban + "&language=" + language,
            function (result) {
                document.getElementById("divThang12").innerHTML = result;
            });
    }

    $(document).ready(function () {
        GetInfoMonth01();
        GetInfoMonth02();
        GetInfoMonth03();
        GetInfoMonth04();
        GetInfoMonth05();
        GetInfoMonth06();
        GetInfoMonth07();
        GetInfoMonth08();
        GetInfoMonth09();
        GetInfoMonth10();
        GetInfoMonth11();
        GetInfoMonth12();
    });

</script>

<script type="text/javascript" >
    function SaveInfoDatetime() {

        var phongban = document.getElementById("<%= ddlPhongBan.ClientID %>").value;
        var thoigian = document.getElementById("<%= txtThoiGian.ClientID %>").value;
        var loaingaynghi = document.getElementById("<%= ddlLoaiNgayNghi.ClientID %>").value;
        var trangthai = 1;

        //Multi click
        var divControl = document.getElementById('control02');
        var divLoading = document.getElementById('loading02');

        divControl.style.display = 'none';
        divLoading.style.display = '';

        //alert('STT: ' + stt);
        //CALL AJAX
        $.post("Hander/hdCalendar.ashx?action=updateSetDate&phongban=" + phongban + "&thoigian=" + thoigian + "&loaingaynghi=" + loaingaynghi + "&trangthai=" + trangthai, function (result) {

            if (result == '') {

                    <%if (language.Equals("1")){ %> alert('Save successfully'); <%} else if (language.Equals("2")){ %> alert('Lưu thông tin thành công!'); <%} %>
                window.location.replace("Calendar.aspx?func=331&action=capnhat&depa=" + phongban);

            }             
            else {

                divControl.style.display = '';
                divLoading.style.display = 'none';

                alert(result);
            }

        });

    }

    function DeleteDatetime() {

        var phongban = document.getElementById("<%= ddlPhongBan.ClientID %>").value;
        var thoigian = document.getElementById("<%= txtThoiGian.ClientID %>").value;
        
        //Multi click
        var divControl = document.getElementById('control02');
        var divLoading = document.getElementById('loading02');

        divControl.style.display = 'none';
        divLoading.style.display = '';

        //alert('STT: ' + stt);
        //CALL AJAX
        $.post("Hander/hdCalendar.ashx?action=deleteSetDate&phongban=" + phongban + "&thoigian=" + thoigian, function (result) {

            if (result == '') {

                <%if (language.Equals("1")){ %> alert('Save successfully'); <%} else if (language.Equals("2")){ %> alert('Lưu thông tin thành công!'); <%} %>
                window.location.replace("Calendar.aspx?func=331&action=capnhat&depa=" + phongban);

            }
            else {

                divControl.style.display = '';
                divLoading.style.display = 'none';

                alert(result);
            }

        });

    }

    function autoCalculatorDay() {
       
        var songayCN = 0;
        var songayThang = 0;
        var songayCN_Thang = 0;
        var totalSoNgay = 0;
        for (var i = 1; i <= 12; i++) {

            var t = "";
            if (i < 10)
                t = "0" + i.toString();
            else
                t = i.toString();

            songayThang = 0;
            songayCN_Thang = 0;

            var OffThang = document.getElementById("OffThang" + t + "").value;
            if (OffThang == "")
                OffThang = "0";

            var thang = document.getElementsByName("thang" + t + "");
            
            for (var j = 0; j < thang.length; j++) {

                var _thang = thang.item(j).value;

                if (_thang == "6") {
                    songayCN = parseFloat(songayCN) + 1;
                    songayCN_Thang = parseFloat(songayCN_Thang) + 1;
                }

                songayThang = parseFloat(songayThang) + 1;
            }

            songayThang = parseFloat(songayThang) - parseFloat(songayCN_Thang) - parseFloat(OffThang);

            totalSoNgay = parseFloat(totalSoNgay) + parseFloat(songayThang);

            document.getElementsByName("totalThang" + t + "").item(0).value = songayThang;

        }

        var hourDay = document.getElementsByName("hourDay").item(0).value;

        document.getElementsByName("totalYear").item(0).value = totalSoNgay;
        document.getElementsByName("totalHourWork").item(0).value = formatNumber(parseFloat(totalSoNgay) * parseFloat(hourDay));

        document.getElementById("<%= lblSoNgayChuNhat.ClientID %>").innerHTML = songayCN;

        setTimeout(autoCalculatorDay, 5000);
    }
    autoCalculatorDay();

</script>

<script type="text/javascript" >
    $('#exampleModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget) // Button that triggered the modal
        var recipient = button.data('whatever') // Extract info FROM data-* attributes

        ResetInfor();
       
        //LAY ID
        if (recipient == '') {
            document.getElementById("<%= txtThoiGian.ClientID %>").value = '';
        }
        else {
            document.getElementById("<%= txtThoiGian.ClientID %>").value = recipient;
            GetInfoDatetime(recipient);
        }

        var modal = $(this)
    })
</script>

<script type="text/javascript" >
    function GetInfoDatetime(thoigian) {

        var phongban = document.getElementById("<%= ddlPhongBan.ClientID %>").value;

        $.post("Hander/hdCalendar.ashx?action=getInforDatetime&thoigian=" + thoigian + "&phongban=" + phongban, function (result) {

            if (result != '') {

                var arr = result.split(" -- ");;
                document.getElementById("<%= txtThoiGian.ClientID %>").value = arr[1];
                document.getElementById("<%= ddlLoaiNgayNghi.ClientID %>").value = arr[2];
            }
            else {

                document.getElementById("<%= txtThoiGian.ClientID %>").value = thoigian;
            }
        });
    }

    function ResetInfor() {
       
        document.getElementById("<%= ddlLoaiNgayNghi.ClientID %>").value = "0";
        document.getElementById("<%= txtThoiGian.ClientID %>").value = "";        
    }




</script>
