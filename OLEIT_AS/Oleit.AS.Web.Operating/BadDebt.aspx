<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BadDebt.aspx.cs" Inherits="Accounting_System.BadDebt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery-1.9.1.min.js"></script>
    <script src="js/commonFunc.js"></script>
    <script src="js/jquery-ui-1.9.2.custom.min.js"></script>
    <script src="js/jquery.jstree.js"></script>
    <script src="js/jquery.watermark.js"></script>
    <link href="css/AccountingSystem.css" rel="stylesheet" />
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript">
        $(function() {

            $("#btnQuery").click(function() {
                $("#allList").dialog({
                    position: { my: "right top", at: "right bottom", of: "#btnQuery" },
                    autoOpen: true
                });
            });

            $("#allList").dialog({
                autoOpen: false,
                title: "Entity Amount",
                width: '240px'
            });

            $("input[name='btnAddBD']").click(function() {
                $("#tbEntityChoose input[type='checkbox']").each(function() {
                    if ($(this).prop("checked") == true) {
                        var obj = $(this).parent().parent();
                        var row = "<tr><td>" + $(obj.children()[1]).text() + "</td><td>" + $(obj.children()[2]).text() + "</td><td><input type='text' onkeydown='return checkNum(event);' style='width:110px' id='txt" + $(obj.children()[1]).attr("id") + "' /></td><td><span title='delete' onclick='deleteEntity(this);' class='ui-button-icon-primary ui-icon ui-icon-closethick'></span></td></tr>";
                        $("#tbBadDebt").append($(row));
                    }
                });
                $("#allList").dialog("close");
            });
        });

        function confirmFunc() {
            if(confirm("Are you sure to confirm?")) {
                if ($("#tbBadDebt").children().children().last().attr("id") == "tableTitle")
                    alert("You don't choose any Bad Debt.");
            }
        }
    </script>
    <style type="text/css">
        .ui-icon-closethick:hover {
            background-image: url(css/images/ui-icons_cd0a0a_256x240.png);
            cursor: pointer;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td>Period From<input type="text" class="normalInput" id="txtPeriodFrom" readonly="readonly" /></td>
                    <td>
                        <input id="btnQuery" type="button" class="btn" value="Query" /></td>
                </tr>
            </table>
            <br/>
            <table id="tbBadDebt" class="tableS">
                <tr id="tableTitle">
                    <td>Entities</td>
                    <td>Balance</td>
                    <td>Bad Debt Amount</td>
                    <td></td>
                </tr>
            </table>
            <table style="width: 300px">
                <tr>
                    <td colspan="4" style="text-align: right"><input name="btnConfirm" type="button" class="btn" onclick="confirmFunc();" value="Confirm" /></td>
                </tr>
            </table>
            
            <div id="allList">
                <table>
                    <tr>
                        <td colspan="3" style="text-align: right">
                            <input type="button" value="Add" class="btn" name="btnAddBD" /></td>
                    </tr>
                    <tr>
                        <td>
                            <table id="tbEntityChoose">
                                <tr>
                                    <td></td>
                                    <td>Entity Name</td>
                                    <td>Balance</td>
                                </tr>
                                <tr>
                                    <td>
                                        <input type="checkbox" /></td>
                                    <td id="entityId1">EntityA</td>
                                    <td id="entityId1Amount">1000</td>
                                </tr>
                                <tr>
                                    <td>
                                        <input type="checkbox" /></td>
                                    <td id="entityId2">EntityB</td>
                                    <td id="entityId2Amount">200</td>
                                </tr>
                                <tr>
                                    <td>
                                        <input type="checkbox" /></td>
                                    <td id="entityId3">EntityC</td>
                                    <td id="entityId3Amount">500</td>
                                </tr>
                                <tr>
                                    <td>
                                        <input type="checkbox" /></td>
                                    <td id="entityId4">EntityD</td>
                                    <td id="entityId4Amount">800</td>
                                </tr>
                                <tr>
                                    <td>
                                        <input type="checkbox" /></td>
                                    <td id="entityId5">EntityE</td>
                                    <td id="entityId5Amount">900</td>
                                </tr>
                                <tr>
                                    <td>
                                        <input type="checkbox" /></td>
                                    <td id="entityId6">EntityF</td>
                                    <td id="entityId6Amount">300</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: right">
                            <input type="button" value="Add" class="btn" name="btnAddBD" /></td>
                    </tr>
                </table>

            </div>
        </div>
    </form>
</body>
</html>
