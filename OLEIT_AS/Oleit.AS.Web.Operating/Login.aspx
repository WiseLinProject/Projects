<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Accounting_System.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
        <title>Accounting System Login Page</title>
        <!--<link rel="shortcut icon" href="../favicon.ico"> -->
    <!--<link rel="stylesheet" type="text/css" href="css/LoginStyle.css" />
    <link href="css/font-awesome.css" rel="stylesheet" />
    <link href="css/demo.css" rel="stylesheet" /> -->
		<script src="js/modernizr.custom.63321.js"></script>
		<!--[if lte IE 7]><style>.main{display:none;} .support-note .note-ie{display:block;}</style><![endif]-->
    <script src="js/jquery-1.9.1.min.js"></script>
 
    <script type="text/javascript">
$(function () {
            $("#btnLogin").click(function () {
                if ($("#txtUsername").val() == "") {
                    alert("Please enter Username.");
                    $("#txtUsername").focus();
                    return false;
                }
                else if ($("#txtPassword").val() == "") {
                    alert("Please enter Password.");
                    $("#txtPassword").focus();
                    return false;
                } else {
                    return true;

                }

            });
        })
        function MM_effectAppearFade(targetElement, duration, from, to, toggle) {
            Spry.Effect.DoFade(targetElement, { duration: duration, from: from, to: to, toggle: toggle });
}
function MM_popupMsg(msg) { //v1.0
  alert(msg);
}
    </script>

<style type="text/css">
body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
	background-image: url(img/index.gif);
	background-repeat: no-repeat;
	background-position: center;
	background-position: top;
	height: 1260px;
	background-color: #000;
    font-family: Verdana, Geneva, sans-serif;
	font-size: 13px;
	font-style: normal;
	line-height: 24px;
	font-weight: normal;
	font-variant: normal;
	text-transform: none;
	color: #FFF;
}

.a_input {
	font-family: Verdana, Geneva, sans-serif;
	font-size: 13px;
	font-style: normal;
	line-height: 14px;
	font-weight: normal;
	font-variant: normal;
	text-transform: none;
	color: #FFF;
	background-color: transparent;
	height: 23px;
	width: 167px;
	border-top-width: 0px;
	border-right-width: 0px;
	border-bottom-width: 0px;
	border-left-width: 0px;
	border-top-style: none;
	border-right-style: none;
	border-bottom-style: none;
	border-left-style: none;
    cursor: pointer;
}




</style>
<link href="css/Css.css" rel="stylesheet" type="text/css">
<meta charset="utf-8">
</head>
<body>
<center>
<div style="width: 990px; height: 1260px; border: none; text-align: left">			
			<section style="width: 263px; height: 135px; border: none; position: relative; top: 137px; left: 682px">
			    <form id="form1" runat="server">
					<input id="txtUsername" type="text" runat="server" name="login" placeholder="Username" class="a_input" style="position: relative; left: 93px; background-color: transparent"  title="Please enter your Usernane to login."/><br />
					<input id="txtPassword" runat="server" type="password" name="password" placeholder="Password" class="a_input" style="position: relative; left: 93px; top: 22px; background-color: transparent"  title="Please enter your Password to login."/><br />

                    <asp:Literal ClientIDMode="Static" ID="ltrFailText" runat="server"></asp:Literal><br />
                  <center><asp:Button ID="btnLogin" type="submit" CssClass="GObtn" name="test" runat="server" OnClick="btnLogin_Click" style="position: relative; top: 9px" title="GO!!!"></asp:Button></center>
					
                     </form>
			</section>
    <img src="img/logo.gif" width="487" height="152" style="border: none; position: relative; top: 738px; left: 483px" />
   
</div>
</center>
</body>
</html>
