<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_administrator_changepassword.ascx.cs" Inherits="HLVTimeSheet.Admin.uc_administrator_changepassword" %>

<!-- Main content -->
<section class="content">

<div class="box box-primary">
    <div class="box-header with-border" style='background-color: #dd4b39 !important;' >
    <h3 class="box-title" ><b style='color: #FFF;' ><%if (language.Equals("1")){ %> Change password <%} else if (language.Equals("2")) { %> Đổi mật khẩu <%} %></b></h3>
    </div>
    <div class="box-body">
		<table style='width:98%; margin:auto;' >
			<tr>
                <td style='width:150px;'><%if (language.Equals("1")){ %> New password <%} else if (language.Equals("2")) { %> Mật khẩu mới <%} %></td>
                <td >
					<input type="password" name="passNew" runat="server" id="txtPassword" CssClass='form-control' style="width:200px;" />
				</td>
            </tr>
            <tr><td colspan="2" >&nbsp;</td></tr>
            <tr  >
                <td><%if (language.Equals("1")){ %> Confrim password <%} else if (language.Equals("2")) { %> Xác nhận mật khẩu <%} %></td>
                <td >
					<input type="password" name="passNew2" runat="server" id="txtPassword2" CssClass='form-control' style="width:200px;" />
				</td>
			</tr>
             <tr><td colspan="2" >&nbsp;</td></tr>
            <tr style="line-height:50px;"  >
                <td></td>
				<td style="text-align:left;"  > 
                    <asp:LinkButton ID="lbCapnhat" runat="server" CssClass='btn btn-success' OnClick="lbCapnhat_Click"  ><%if (language.Equals("1")){ %> Save <%} else if (language.Equals("2")) { %> Cập nhật <%} %></asp:LinkButton>

                    <br />
                    <asp:Label ID="lbMsg" runat="server" Text=""></asp:Label>
                </td>
			</tr>
		</table>
    </div><!-- /.box -->
</div>

</section><!-- /.content -->