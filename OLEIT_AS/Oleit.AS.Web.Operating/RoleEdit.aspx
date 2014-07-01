<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleEdit.aspx.cs" Inherits="Accounting_System.RoleEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/table.css" rel="stylesheet" />
    <link href="css/AccountingSystem.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:DetailsView ID="dvRoleEdit" DefaultMode="Edit" AutoGenerateRows="False" CssClass="bordered" runat="server" Height="50px" Width="125px">
            <Fields>
                <asp:BoundField DataField="RoleName" HeaderText="RoleName" />
            </Fields>
        </asp:DetailsView>
    </div>
    </form>
</body>
</html>
