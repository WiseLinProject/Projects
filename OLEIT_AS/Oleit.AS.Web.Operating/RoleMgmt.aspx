<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleMgmt.aspx.cs" Inherits="Accounting_System.RoleMgmt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Role Management</title>
    <link href="css/table.css" rel="stylesheet" />
    <link href="css/a_style.css" rel="stylesheet" />
    <link href="css/jquery-ui.css" rel="stylesheet" />
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
    <!--<link href="css/AccountingSystem.css" rel="stylesheet" />-->

    <script src="js/jquery-1.9.1.min.js"></script>
    <script src="js/jquery-ui-1.9.2.custom.min.js"></script>

    <script src="js/commonFunc.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#divEditRole").dialog({
                autoOpen: false,
                title: "Edit Role",
                width: '460px'
            });

            $("#divAddRole").dialog({
                autoOpen: false,
                title: "Add Role",
                width: '460px'
            });

            $("#btnAddRole").click(function () {
                $("#divAddRole").dialog({
                    autoOpen: true
                });
                $("#divAddRole").parent().appendTo($("form:first"));
            });

            $("#btnAdd").click(function () {
                //$("#divEditRole").dialog({
                //    autoOpen: true
                //});
                return false;
            });

            $("#btnEditRole").click(function () {
                if (checkRoleName($("#txtRoleName").val(), "edit")) {
                    var _roleId = $("#txtRoleName").attr("roleid");
                    var _roleName = $("#txtRoleName").val();
                    $.ajax({
                        type: "POST",
                        url: "RoleAjax.aspx",
                        data: {
                            RoleName: _roleName,
                            RoleId: _roleId,
                            Type: "update"
                        },
                        success: function (data) {
                            alert(data);
                            if (data == "Success") {
                                $("#roleId" + _roleId).text(_roleName).attr("title", "Edit－" + _roleName);
                            }
                            else {
                                if (data == "Session has expired.")
                                    window.parent.location.reload();
                            }
                        },
                        error: function () {
                            alert("Fail!");
                        }
                    });
                    $("#divEditRole").dialog("close");
                }
            });

            $("#btnAddNewRole").click(function () {
                if (checkRoleName($("#txtNewRoleName").val())) {
                    $.ajax({
                        type: "POST",
                        url: "RoleAjax.aspx",
                        data: {
                            RoleName: $("#txtNewRoleName").val(),
                            Type: "insert"
                        },
                        success: function (data) {
                            alert(data);
                            if (data == "Success") {
                                location.reload();
                            }
                            else {
                                if (data == "Session has expired.")
                                    window.parent.location.reload();
                            }
                        },
                        error: function () {
                            alert("Fail!");
                        }
                    });
                }
            });

            $("#btnDelete").click(function () {
                if (confirm("Do you really want to delete?")) {
                    var _deleteStr = "";
                    $("input[id^='cbRole']").each(function () {
                        var _id = $(this).attr("id");
                        if ($(this).prop("checked"))
                            _deleteStr += _id.substring(6) + ",";
                    });
                    _deleteStr = _deleteStr.substring(0, _deleteStr.length - 1);
                    $.ajax({
                        type: "POST",
                        url: "RoleAjax.aspx",
                        data: {
                            roleIdAry: _deleteStr,
                            Type: "delete"
                        },
                        success: function (data) {
                            alert(data);
                            if (data == "Success") {
                                location.reload();
                            }
                            else {
                                if (data == "Session has expired.")
                                    window.parent.location.reload();
                            }
                        },
                        error: function () {
                            alert("Fail!");
                        }
                    });
                }
            });
        });

        function editRole(id) {
            var _roleName = $("#" + id).text();
            $("#txtRoleName").val(_roleName).attr("roleid",id.substring(6));
            
            $("#hfOriRoleName").val(_roleName);
            $("#divEditRole").dialog({
                autoOpen: true
            });
        }

        function checkRoleName(roleName,edit) {
            if (roleName != "") {
                var _sameName = false;
                $("a").each(function() {
                    var _id = $(this).attr("id");
                    var _roleName = $("#" + _id).text();
                    if (_roleName == roleName && _roleName!=$("#hfOriRoleName").val())
                        {
                        alert("There is already the same "+roleName+" name here!");
                        _sameName = true;
                        return false;
                    }
                });
                return !_sameName;//if there is the same name return false
            } else {
                alert("Role Name can't be null!");
                $("#txtRoleName").focus();
                return false;
            }
  
        }
        
        function checkRoleName(roleName) {
            if (roleName != "") {
                var _sameName = false;
                $("a").each(function () {
                    var _id = $(this).attr("id");
                    var _roleName = $("#" + _id).text();
                    if (_roleName == roleName) {
                        alert("There is already the same " + roleName + " name here!");
                        _sameName = true;
                        return false;
                    }
                });
                return !_sameName;//if there is the same name return false
            } else {
                alert("Role Name can't be null!");
                $("#txtRoleName").focus();
                return false;
            }

        }

        function checkCB() {
            var _checked=false;
            $("input[id^='cbRole']").each(function () {
                if ($(this).prop("checked") == true) {
                    _checked = true;
                    return false;
                }
            });
            if (_checked)
                $("#btnDelete").attr("class", "DeleteBtn");
            else {
                $("#btnDelete").attr("class", "DeleteBtn");
            }
                
        }

        function selectAllCB() {
            var _checked = $("#cbSelectAll").prop("checked");
            $("input[id^='cbRole']").each(function() {
                $(this).prop("checked", _checked);
            });
            if (_checked)
                $("#btnDelete").attr("class", "DeleteBtn");
            else {
                $("#btnDelete").attr("class", "DeleteBtn");
            }
        }

        function deleteRole() {
            
  
        }

    </script>

<meta charset="utf-8">
</head>
<body>
    <form id="form1" runat="server"><br /><br />
        <center><label class="TE_wd04">Role Management</label>
        <div style="width: 800px; border: none; text-align: left" class="a03_border">
        <br /><br /><div style="width: 100%; height: 690px; overflow-x: hidden; overflow-y: auto; "><input id="btnAddRole" class="Add-Role02" type="button" title="Add Role" value="" style="position: relative; left: 105px"/><br /><br />
        <table align="center">
            <tr>
                <td align="center">
                        <asp:GridView ID="gvRole" AutoGenerateColumns="False" runat="server" Width="600px" GridLines="None" CssClass="bordered" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                
                                <asp:TemplateField ItemStyle-Width="100px">
                                    <HeaderTemplate>
                                        <input type="checkbox" onclick="selectAllCB();" id='cbSelectAll' />&nbsp;Select all
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <center><input type="checkbox" id='cbRole<%# Eval("ID") %>' onclick="checkCB();" /></center>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RoleName">
                                    <ItemTemplate>
                                        <a id="roleId<%# Eval("ID") %>" title="Edit－<%# Eval("RoleName") %>" onclick="editRole(this.id)" href="#"><%# Eval("RoleName") %></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>
                    
                    <div id="divEditRole" style="background-color: rgba(25, 25, 25, 0.95); color: #fff; font-family: Verdana, Geneva, sans-serif; font-size: 14px">
                        <table style="width: 372px" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="height: 36px; width: 94px" align="right" valign="middle">
                                    Role Name :
                                </td>
                                <td align="center" style="width: 176px" valign="middle">

                                    <input style="width: 160px" class="a01_input" type="text" value="" id="txtRoleName"/>
                                </td>
                                <td style="width: 102px" valign="middle" align="left">
                                    <input type="hidden" id="hfOriRoleName"/>
                                    <input id="btnEditRole" type="button" value="" title="Edit Role" style="position: relative; top: 1px"  class="Add01"/>
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="divAddRole" style="background-color: rgba(25, 25, 25, 0.95); color: #fff; font-family: Verdana, Geneva, sans-serif; font-size: 14px">
                        <table style="width: 372px" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="height: 36px; width: 94px" align="right" valign="middle">
                                    Role Name :
                                </td>
                                <td align="center" style="width: 176px" valign="middle">
                                    <input type="text" style="width: 160px" class="a01_input" value="" id="txtNewRoleName" runat="server"/>
                                </td>
                                <td style="width: 102px" valign="middle" align="left">
                                    <input type="button" value="" title="Add" class="Add01" id="btnAddNewRole"/>
                                </td>
                            </tr>
                        </table>
                    </div>

                </td>
            </tr>
            <tr><td style="height: 70px; text-align: left; vertical-align: middle" ><input type="button" id="btnDelete" class="DeleteBtn" title="Delete" value="" style="position: relative; left: 65px"/>
                </td></tr>
        </table>
            </div><br /><br /></div>
    </center></form>
</body>
</html>
