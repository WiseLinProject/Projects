<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FuncPage.aspx.cs" Inherits="Accounting_System.FuncPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/slideMenu.js"></script>
    <link href="css/component.css" rel="stylesheet" />
    <link href="css/default.css" rel="stylesheet" />
    <script src="js/jquery-1.9.1.min.js"></script>
    <script src="js/commonFunc.js"></script>
    <script type="text/javascript">
        $(function() {
            $("li a").click(function() {
                var id = $(this).attr("id");
                $("container").attr("src", id + ".aspx");
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: left">
                <nav class="dr-menu">
                    <ul>
                        <li><a id="UserMgmt" onclick="Alert('test')"  href="#">User Management</a></li>
                        <li><a id="EntityMgmt"  href="#">Entity Management</a></li>
                        <li><a  href="#">Account Management</a></li>
                        <li><a  href="#">Transaction Entry</a></li>
                        <li><a  href="#">Amount Transfer</a></li>
                        <li><a  href="#">Exchange Rate Setting</a></li>
                        <li><a  href="#">Win/Loss Entry</a></li>
                        <li><a  href="#">Weekly Settle</a></li>
                        <li><a  href="#">Properties Setting</a></li>
                        <li><a  href="#">Set Period</a></li>
                        <li><a  href="#">Expenses/Bad Debt</a></li>
                        <li><a  href="#">Logout</a></li>
                    </ul>
                </nav>
         </div>
    </form>
</body>
</html>
