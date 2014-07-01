<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Settle.aspx.cs" Inherits="Accounting_System.Settle" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Settle</title>
    <script src="js/commonFunc.js"></script>
    <script src="js/jquery-1.9.1.min.js"></script>
    <link href="css/AccountingSystem.css" rel="stylesheet" />
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <link href="css/Css.css" rel="stylesheet" />
    <style type="text/css">
    body {
	background-color: #272727;
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
}
    </style>
    <script type="text/javascript">
        $(function() {
            $("#btnClose").click(function () {
                if (confirm("Do you really want to close?"))
                    return true;
                else {
                    return false;
                }
            });
            
            $("#btnReverse").click(function () {
                if (confirm("Do you really want to reverse?"))
                    return true;
                else {
                    return false;
                }
            });

        });
    </script>
<meta charset="utf-8">
</head>
<body>
    <form id="form1" runat="server"><br><br><center><label class="TE_wd04">Settle</label>
    <div style="width: 900px; border: none; text-align: center" class="a03_border"><br><br><br><div style="width: 100%; height: 670px; overflow-x: hidden; overflow-y: auto; ">
    <table align="center">
        <tr>
            <td >
                <asp:Button runat="server" ID="btnClose" Text="" title="Close" ClientIDMode="Static" CssClass="Close02" OnClick="btnClose_Click"/>
            </td>
            <td >
                <asp:Button runat="server" ID="btnReverse" Text="" title="Reverse" ClientIDMode="Static" CssClass="Reverse02" OnClick="btnReverse_Click"/>
            </td>
        </tr>
        <tr><td colspan="2" style="height: 20px">&nbsp;</td></tr>
        <tr style="color: #fff; font-family: Verdana; font-size: 14px; font-weight: bold">
            <td style="height: 36px" align="right" valign="middle">Current Period : </td>
            <td align="left" valign="middle"><asp:Label CssClass="a02_input_02_b" Width="120px" ID="lblCurrentPeriod" ClientIDMode="Static" runat="server"></asp:Label></td>
        </tr>
    </table></div><br><br></div></center>

    </form>
</body>
</html>
