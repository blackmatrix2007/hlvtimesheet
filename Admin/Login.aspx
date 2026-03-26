<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HLVTimeSheet.Admin.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8">
    <title>Hondalogicom VietNam</title>
    <link rel="shortcut icon" href="../Images/logoHLV.ico" /> 

    <meta content='width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no' name='viewport'>
    <!-- Bootstrap 3.3.2 -->
    <link href="../Content/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <!-- Font Awesome Icons -->
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <!-- Theme style -->
    <link href="../Content/dist/css/AdminLTE.min.css" rel="stylesheet" type="text/css" />
    <!-- iCheck -->
    <link href="../Content/plugins/iCheck/square/blue.css" rel="stylesheet" type="text/css" />

</head>

<body class="login-page" style="background-color: mediumaquamarine; " >
<form id="form1" runat="server">
    
    <div class="login-box">
      <div class="login-logo">
          <%--<a href="javascript:void(0);" style='color:White;' ><b>Hondalogicom VietNam</b></a>--%>
          <a href="javascript:void(0);" style='color:White;' > <img src="../Images/logoHondalogicom.png" style="width:300px;"/></a>
      </div><!-- /.login-logo -->
      <div class="login-box-body" style="background-color: mediumaquamarine; " >
        
          <div class="row">
              <div class="col-xs-12">
                  <asp:Label ID="lblError" runat="server" ForeColor="Black"></asp:Label>
               </div>
           </div>

          <div class="form-group has-feedback">
            <asp:TextBox ID="txtUserName" runat="server" placeholder="Username" Width="100%" CssClass='form-control' ></asp:TextBox>
            <span class="glyphicon glyphicon-user form-control-feedback"></span>
          </div>
        
          <div class="form-group has-feedback">
               <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" placeholder="Password" Width="100%" CssClass='form-control' 
                AutoPostBack="true" OnTextChanged="txtPassword_TextChanged"></asp:TextBox>
            <span class="glyphicon glyphicon-lock form-control-feedback"></span>
          </div>
        
          <div class="row">
            <div class="col-xs-8">    
              <div class="checkbox icheck ">
                <label>
                    
                </label>
              </div>                        
            </div><!-- /.col -->

            <div class="col-xs-4">
              
               <asp:LinkButton ID="lblDangNhap" runat="server" CssClass="btn btn-primary" onclick="lbDangNhap_Click" Width="100%" > 
                   <span class="btn-block btn-flat" aria-hidden="true"></span> Login    
               </asp:LinkButton>
            </div><!-- /.col -->
          </div>

      </div><!-- /.login-box-body -->
    </div><!-- /.login-box -->

<script type="text/javascript" >
    //function DangNhap() {

    //    var Username = document.getElementById("txtUsername").value;
    //    var Password = document.getElementById("txtPassword").value;

    //    if (Username == '') {

    //        alert('Please you must be input username!');
    //        return;
    //    }

    //    if (Password == '') {

    //        alert('Please you must be input password!');
    //        return;
    //    }

    //    //CALL AJAX
    //    $.post("Hander/hdThanhvien.ashx?action=dangnhap&Username=" + Username + "&Password=" + Password, function (result) {
            
    //        if (result == 'OK') {
    //             window.location.replace("Index.aspx");

    //        }
    //        else {
    //            alert( result );
    //        }

    //    });
    //}
    
    function doClick(buttonName, e) {
        //the purpose of this function is to allow the enter key to 
        //point to the correct button to click.
        var ev = e || window.event;
        var key = ev.keyCode;

        if (key == 13) {
            //Get the button the user wants to have clicked
            var btn = document.getElementById(buttonName);
            if (btn != null) {
                //If we find the button click it
                btn.click();
                ev.preventDefault();
            }
        }
    }

</script>

</form>
</body>
</html>
