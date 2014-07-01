<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleUser.aspx.cs" Inherits="Accounting_System.RoleUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Role and User</title>
    <link href="css/table.css" rel="stylesheet" />
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
    <script src="js/jquery-1.9.1.min.js"></script>

    <script type="text/javascript">
        $(function () {
            $("input[type='checkbox']").click(function () {
                var _id = $(this).attr("id");
                var _userId = _id.substring(0, _id.indexOf("_"));
                var _roleId = _id.substring(_id.indexOf("_") + 1);
                $.ajax({
                    type: "POST",
                    url: "RoleAjax.aspx",
                    data: {
                        UserId: _userId,
                        RoleId: _roleId,
                        Type: "roleuser",
                        Checked: $(this).prop("checked")
                    },
                    success: function (data) {
                        if (data == "Session has expired.")
                            window.parent.location.reload();
                    }
                });
            });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
<br /><br />
        <center><label class="TE_wd04">Role and User</label><div style="width: 95%; height: 732px; overflow-x: hidden; overflow: auto; "><asp:GridView ID="gvRoleUser" AutoGenerateColumns="False"  runat="server" DataKeyNames="UserID" OnRowDataBound="gvRoleUser_RowDataBound" GridLines="None" CssClass="bj" AlternatingRowStyle-CssClass="alt">
            <Columns>
                <asp:BoundField DataField="UserName" HeaderText="User" />
            </Columns>         
        </asp:GridView>
</div></center></form>
</body>
</html>
