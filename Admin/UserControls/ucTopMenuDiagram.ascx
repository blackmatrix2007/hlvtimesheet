<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTopMenuDiagram.ascx.cs" Inherits="HLVTimeSheet.Admin.UserControls.ucTopMenuDiagram" %>
<header class="main-header">
    <a href="Index.aspx" class="logo"><b>HLV-TimeSheet</b></a>
    <!-- Header Navbar: style can be found in header.less -->
    <nav class="navbar navbar-static-top hidden-xs hidden-sm" role="navigation">
          
        <div class="collapse navbar-collapse" id="navbar-collapse">
        <ul class="nav navbar-nav">
           <%-- <asp:Label ID="lblWorkTime" runat="server" ForeColor="White" Font-Bold="true"></asp:Label>--%>
        </ul>

        <ul class="nav navbar-nav navbar-right" style="margin-right:10px;">
      	  
			<!-- Tasks: style can be found in dropdown.less -->
            <%if (language.Equals("1"))
                { %>
            <li class="dropdown tasks-menu">
            <a href="#" class="dropdown-toggle languageGDC" data-toggle="dropdown">
               <img src="../Images/UnitedKingdomFlag.png" style="width:24px; height:18px;"/>
            </a>
            <ul class="dropdown-menu" style="width:25px;">
                <li>
                    <a href="<%=path %>&lang=2">
                        <img src="../Images/VietNamFlag.png" style="width:24px; height:18px;"/>
                        <span class="hidden-xs">Vietnamese</span>
                    </a>
                </li>
               <li>
                    <a href="<%=path %>&lang=1">
                        <img src="../Images/UnitedKingdomFlag.png" style="width:24px; height:18px;"/>
                        <span class="hidden-xs">English</span>
                    </a>
                </li>
            </ul>
            </li>
              <%}
            else if (language.Equals("2"))
            { %>
                <li class="dropdown tasks-menu">
            <a href="#" class="dropdown-toggle languageGDC" data-toggle="dropdown">
               <img src="../Images/VietNamFlag.png" style="width:24px; height:18px;"/>
            </a>
            <ul class="dropdown-menu" style="width:25px;">
                <li>
                    <a href="<%=path %>&lang=2">
                        <img src="../Images/VietNamFlag.png" style="width:24px; height:18px;"/>
                        <span class="hidden-xs">Vietnamese</span>
                    </a>
                </li>
               <li>
                    <a href="<%=path %>&lang=1">
                        <img src="../Images/UnitedKingdomFlag.png" style="width:24px; height:18px;"/>
                        <span class="hidden-xs">English</span>
                    </a>
                </li>
            </ul>
            </li>
            <%} %>
			  
        </ul>
        </div><!-- /.navbar-collapse -->
          
    </nav>
</header>
