<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNavitor.ascx.cs" Inherits="HLVTimeSheet.Admin.UserControls.ucNavitor" %>

<section class="content-header">
    <h1>
        <%--Tổng quan<small>Thông tin hệ thống</small>--%>
        <asp:Literal ID="ucNvparent" runat="server"></asp:Literal>
    </h1>
    <ol class="breadcrumb" style="display:none;">
    <li><a href="javascript:void(0);"><i class="fa fa-laptop"></i> <asp:Literal ID="ucNvparentLevel2" runat="server"></asp:Literal> </a></li>
    <li class="active"><asp:Literal ID="ucNvaction" runat="server"></asp:Literal></li>
    </ol>
</section>
