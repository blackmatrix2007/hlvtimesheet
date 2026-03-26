<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTopMenu.ascx.cs" Inherits="HLVTimeSheet.Admin.UserControls.ucTopMenu" %>

<header class="main-header">
    <a href="Index.aspx" class="logo"><b>HLV-TimeSheet</b></a>
    <!-- Header Navbar: style can be found in header.less -->
    <nav class="navbar navbar-static-top" role="navigation">
          
		<!-- Sidebar toggle button-->
        <a href="#" class="sidebar-toggle" data-toggle="offcanvas" role="button">
        <span class="sr-only">Toggle navigation</span>
        <span class="icon-bar"></span>
        <span class="icon-bar"></span>
        <span class="icon-bar"></span>
        </a>
          
		<!-- Collect the nav links, forms, and other content for toggling -->
        <div class="collapse navbar-collapse" id="navbar-collapse">
        <ul class="nav navbar-nav">
            
            <li><a href="Homepage.aspx"><%if (language.Equals("1")){ %> Home page <%} else if (language.Equals("2")){ %> Trang chủ <%} %></a></li>
            
            <asp:Literal ID="ltTopmenu" runat="server"></asp:Literal>

        </ul>

        <ul class="nav navbar-nav navbar-right">
      
            <!-- Notifications: style can be found in dropdown.less -->
            <li class="dropdown notifications-menu" style="display:none;">
            <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                <i class="fa fa-bell-o"></i>
                <span class="label label-warning">10</span>
            </a>
            <ul class="dropdown-menu" style="display:none;">
                <%--<li class="header">Hệ thống có 10 sản phẩm sắp hết tồn</li>--%>
                <asp:Literal ID="ltSosphetton" runat="server"></asp:Literal>
                <li>
                <ul class="menu">
                    <%--<li>
                        <a href="Report.aspx?id=26"><i class="fa fa-users text-aqua"></i> Hàng hóa test 01</a>
                    </li>--%>
                    <asp:Literal ID="ltHetton" runat="server"></asp:Literal>
                </ul>
                </li>
                <li class="footer"><a href="Report.aspx?func=26">view all</a></li>
            </ul>
            </li>
			  
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
			
            <li class="dropdown user user-menu">
            <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                <img src="../Images/logoHLV.ico" class="user-image"/>
                <span class="hidden-xs">Hi: <%= Session["userName"].ToString()%></span> &nbsp;&nbsp;
            </a>
            <ul class="dropdown-menu" role="menu" style='max-width:120px;' >
                <li><a href="Login.aspx">Log out</a></li>
                <li class="divider"></li>
                <li><a href="Administrator.aspx?func=92"><%if (language.Equals("1")){ %> Information of account <%} else if (language.Equals("2")) { %> Thông tin tài khoản<%} %></a></li>
                <li><a href="Administrator.aspx?func=99"><%if (language.Equals("1")){ %> Change password <%} else if (language.Equals("2")) { %> Đổi mật khẩu<%} %></a></li> 
            </ul>
            </li>
			  
        </ul>
        </div><!-- /.navbar-collapse -->
          
    </nav>
</header>

<script type="text/javascript" >
    //var url = "";
    //function getURL() {
    //    url = window.location.href;
    //    setTimeout(getURL, 300);
    //}

    //$(document).ready(function () {
    //    getURL();
    //});
</script>