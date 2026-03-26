<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLeftMenu.ascx.cs" Inherits="HLVTimeSheet.Admin.UserControls.ucLeftMenu" %>

<aside class="main-sidebar">

    <section class="sidebar">
         
        <ul class="sidebar-menu">

            <li class="header"></li>
            
            <li>
                <a href="Index.aspx">
                     <%if (language.Equals("1"))
                        { %>
                        <i class="fa fa-calendar"></i> <span>Overview</span>
                      <%}
                    else if (language.Equals("2"))
                    { %>
                        <i class="fa fa-calendar"></i> <span>Tổng quan</span>
                    <%} %>
                
                </a>
            </li>

            <asp:Literal ID="ltLeftmenu" runat="server"></asp:Literal>
            
        </ul>
        
    </section>
</aside>
