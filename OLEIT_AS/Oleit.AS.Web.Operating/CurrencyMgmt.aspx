<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CurrencyMgmt.aspx.cs" Inherits="Accounting_System.CurrencyMgmt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="css/AccountingSystem.css" rel="stylesheet" />
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <link href="css/table.css" rel="stylesheet" />
    <link href="css/Css.css" rel="stylesheet" type="text/css">
    <style type="text/css">
    body {
	background-color: #272727;
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
}
    </style>
    <script src="js/jquery-1.9.1.min.js"></script>
    <script src="js/jquery-ui-1.9.2.custom.min.js"></script>
    <script src="js/commonFunc.js"></script>
    <title>Currency Management</title>
    <script type="text/javascript">
        $(function() {
            $("#btnAdd").click(function() {
                var repeated = false;
                $("#tbCurrency td").each(function() {
                    if ($(this).text() == $("#txtCurrencyName").val())
                        repeated = true;
                });
                if (repeated == false) {
                    var newCurrency = $("<tr><td style=\"color: black\">" + $("#txtCurrencyName").val() + "</td></tr>");
                    $("#tbCurrency").append(newCurrency);
                    return true;
                } else {
                    alert("Currency can't be repeated.");
                    return false;
                }
            });
        });
    </script>
<meta charset="utf-8">
</head>
<body>
    <form id="form1" runat="server"><br /><br /><center><label class="TE_wd04">Currency Management</label>
        <div style="width: 860px; height: 744px; border: none; text-align: center" class="a03_border">
        <br /><br />
                        <div style="width: 100%; height: 680px; overflow-x: hidden; overflow-y: auto; "><br /><input type="text" id="txtCurrencyName" maxlength="3" style="width: 120px; position: relative; top: -9px" class="a02_input_02_a" runat="server"/>&nbsp;<asp:Button runat="server" ID="btnAdd" ClientIDMode="Static" Text="" title="Add" CssClass="Add01" OnClick="btnAdd_Click"/>
            <br /><br />
        <table class="bordered" style="width: 35%" align="center" id="tbCurrency">
            <thead><tr><th>Currency List</th></tr></thead>
            
            <asp:Repeater ID="rptCurrency" runat="server">
                <ItemTemplate>
                    <tr>
                        <td><img src="img/Money.png" width="19px" height="19px" style="border: none" />&nbsp;:&nbsp;<%# Eval("CurrencyID") %></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
                        </div>
            <br /><br /></div></form>
</body>
</html>
