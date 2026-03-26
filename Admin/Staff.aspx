<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Main.Master" AutoEventWireup="true" CodeBehind="Staff.aspx.cs" Inherits="HLVTimeSheet.Admin.Staff" %>
<%@ Register src="../Admin/UserControls/ucNavitor.ascx" tagname="ucNavitor" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- NAVITOR -->
    <uc1:ucNavitor ID="ucNavitor1" runat="server" />

    <asp:Panel ID="panelControl" runat="server">
    </asp:Panel>

</asp:Content>
