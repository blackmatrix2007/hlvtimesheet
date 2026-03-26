<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_administrator_account_action.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_administrator_account_action" %>

<!-- Main content -->
<section class="content">

<div class="box box-primary">
    <div class="box-header with-border" style='background-color: #dd4b39 !important;' >
    <h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")) { %> Information of user <% } else if (language.Equals("2")) { %> Thông tin tài khoản <%} %></b></h3>
    </div>
    <div class="box-body">
    
        <div class="row">
            <div class="col-xs-12"> 
               <a class="btn btn-primary" href="Administrator.aspx?func=92"><span class="glyphicon glyphicon-menu-left" aria-hidden="true"></span> 
                   <%if (language.Equals("1")){ %> Back <%} else if (language.Equals("2")){ %>Quay lại<%} %>
               </a>
                <asp:LinkButton ID="lbLuuLai" runat="server" CssClass="btn btn-primary" onclick="lbLuuLai_Click" ><span class="glyphicon glyphicon-floppy-save" aria-hidden="true"></span> 
                    <%if (language.Equals("1")){ %> Save <%} else if (language.Equals("2")) { %> Lưu thông tin <%} %> 
                </asp:LinkButton>
            </div>
        </div>

        <div class="row" style="margin-top:5px;">
            <div class="col-xs-12">
                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
            </div>
        </div>

        <div class="row" style="margin-top:5px;">
            <div class="col-xs-2" style="display:none;"> <%if (language.Equals("1")){ %> Code(*) <%} else if (language.Equals("2")) { %> Mã (*) <%} %> </div>
            <div class="col-xs-4" style="display:none;"> 
                <input type="text" class="form-control" id="txtMa" runat="server" >
            </div>

            <div class="col-xs-2"> <%if (language.Equals("1")){ %> Name (*) <%} else if (language.Equals("2")) { %> Tên(*) <%} %> </div>
            <div class="col-xs-4">
                <input type="text" class="form-control" id="txtTen" runat="server" >
            </div>
        </div>

         <div class="row" style="margin-top:5px;">
            <div class="col-xs-2"> <%if (language.Equals("1")){ %> Phone <%} else if (language.Equals("2")) { %> Điện thoại <%} %> </div>
            <div class="col-xs-4"> 
                <asp:TextBox ID="txtDienThoai" runat="server" class="form-control" ></asp:TextBox>
            </div>

            <div class="col-xs-2"> <%if (language.Equals("1")){ %> Address <%} else if (language.Equals("2")) { %> Địa chỉ <%} %> </div>
            <div class="col-xs-4">
                <asp:TextBox ID="txtDiaChi" runat="server" class="form-control" ></asp:TextBox>
            </div>
        </div>

        
         <div class="row" style="margin-top:5px;">
            <div class="col-xs-2" style="display:none;"> <%if (language.Equals("1")){ %> Role permission <%} else if (language.Equals("2")) { %> Nhóm quyền <%} %> </div>
            <div class="col-xs-4" style="display:none;"> 
                <asp:DropDownList ID="ddlNhomQuyen" runat="server" class="form-control"></asp:DropDownList>
            </div>

             <div class="col-xs-2"> <%if (language.Equals("1")){ %> Language default <%} else if (language.Equals("2")) { %> Ngôn ngữ mặc định <%} %> </div>
            <div class="col-xs-4"> 
                <asp:DropDownList ID="ddlNgonNgu" runat="server" class="form-control"></asp:DropDownList>
            </div>

            <div class="col-xs-2"> <%if (language.Equals("1")){ %> Status <%} else if (language.Equals("2")) { %> Trạng thái <%} %> </div>
            <div class="col-xs-4">
                <input type="checkbox" id='chkTrangThai' runat="server" /> <i><%if (language.Equals("1")){ %> Action <%} else if (language.Equals("2")) { %> Hoạt động <%} %></i>
            </div>
        </div>

        <div class="row" style="margin-top:5px;">
            <div class="col-xs-2"> Type </div>
            <div class="col-xs-4"> 
                <asp:DropDownList ID="ddlPhanLoai" runat="server" class="form-control" 
                    AutoPostBack="True" onselectedindexchanged="ddlPhanLoai_SelectedIndexChanged" ></asp:DropDownList>
            </div>

              <% if (ddlPhanLoai.SelectedValue.Equals("0")){ %>
                <div class="col-xs-2"> Customer </div>
                <div class="col-xs-4">
                    <asp:DropDownList ID="ddlKhachHang" runat="server" class="form-control"  ></asp:DropDownList>
                </div>
            <% } %>
        </div>

        <div class="row" style="margin-top:5px;">
            <div class="col-xs-2"> <%if (language.Equals("1")){ %> Username (*) <%} else if (language.Equals("2")) { %> Đăng nhập (*) <%} %> </div>
            <div class="col-xs-4"> 
                <input type="text" id="txtDangNhap" runat="server" class="form-control"  />
            </div>

            <div class="col-xs-2"> <%if (language.Equals("1")){ %> Password (*) <%} else if (language.Equals("2")) { %> Mật khẩu (*) <%} %> </div>
            <div class="col-xs-4">
                <input type="password" id="txtMatkhau" runat="server" class="form-control"  />
            </div>
        </div>

         <div class="row" style="margin-top:5px; display:none;">
            <div class="col-xs-2"> Mail (*) </div>
            <div class="col-xs-4"> 
                <input type="text" id="txtMail" runat="server" class="form-control"  />
            </div>

        </div>

          <div class="row" style="margin-top:10px;">
            <div class="col-xs-12"> 
                
                        <ul class="nav nav-tabs">
                          <li class="active" ><a data-toggle="tab" href="#nhomquyen"> <%if (language.Equals("1")){ %> Role permission <%} else if (language.Equals("2")) { %> Nhóm quyền <%} %></a></li>
                            <li style="display:none;"><a data-toggle="tab" href="#location">Location</a></li>
                            <li style="display:none;"><a data-toggle="tab" href="#khuvuc">Quyền Khu vực</a></li>
                            <li style="display:none;"><a data-toggle="tab" href="#kho">Kho</a></li>
                            <li><a data-toggle="tab" href="#system"><%if (language.Equals("1")){ %> System <%} else if (language.Equals("2")) { %> Hệ thống <%} %></a></li>
                        </ul>

                        <div class="tab-content">

                          <div id="nhomquyen" class="tab-pane fade in active">
                    
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

                          <div id="location" class="tab-pane fade">                    
                                <table class="table table-hover table-bordered table-striped" style="font-size:small">
                                    <tr>
			                            <th style="text-align:center;">#</th>
                                        <th style="text-align:center;">Mã Location</th>
                                        <th style="text-align:center;">Tên Location</th>
                                        <th style="text-align:center;">Ghi chú</th>
                                        <th style="text-align:center;">Chọn </th>
                                    </tr>
                                    <asp:Literal ID="ltLocation" runat="server"></asp:Literal>
                                </table>
                          </div>

                          <div id="kho" class="tab-pane fade">
                    
                            <table class="table table-hover table-bordered table-striped" style="font-size:small">
                                <tr>
			                        <th style="text-align:center;">#</th>
                                    <th style="text-align:center;">Code</th>
                                    <th style="text-align:center;">Name</th>                                
                                    <th style="text-align:center;">Choose</th>
                                </tr>
                                <asp:Literal ID="ltKho" runat="server"></asp:Literal>
                            </table>
                            </div>

                          <div id="system" class="tab-pane fade">
                    
                            <table class="table table-hover table-bordered table-striped" style="font-size:small">
                                <tr>
			                        <th style="text-align:center;">#</th>
                                    <th style="text-align:center;">Code</th>
                                    <th style="text-align:center;">System</th>
                                    <th style="text-align:center;">Choose</th>
                                </tr>
                                <asp:Literal ID="ltSystem" runat="server"></asp:Literal>
                            </table>
                            </div>

                         <% if (ddlPhanLoai.SelectedValue.Equals("1"))
                        { %>
                            
                          <div id="khuvuc" class="tab-pane fade">
                    
                                <table class="table table-hover table-bordered table-striped" style="font-size:small">
                                    <tr>
			                            <th style="text-align:center;">#</th>
                                        <th style="text-align:center;">Mã </th>
                                        <th style="text-align:center;">Tên </th>
                                        <th style="text-align:center;">Chọn </th>
                                    </tr>

                                    <asp:Literal ID="ltKhuvuc" runat="server"></asp:Literal>

                                </table>

                          </div>

                        <% } %>

                        </div>
            </div>
        </div>

    </div><!-- /.box-body -->

</div><!-- /.box -->

</section><!-- /.content -->
