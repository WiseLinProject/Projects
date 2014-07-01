<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Expenses.aspx.cs" Inherits="Accounting_System.Expenses" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/commonFunc.js"></script>
    <script src="js/jquery-1.9.1.min.js"></script>
    <script src="js/jquery-ui-1.9.2.custom.min.js"></script>
    <script src="js/jquery.jstree.js"></script>
    <script src="js/jquery.watermark.js"></script>
    <link href="css/AccountingSystem.css" rel="stylesheet" />
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript">
        $(function () {
            $("#showEntitiesBar")
                .jstree({
                    "json_data": {
                        "data": <%=JsonEntityTreeString%>
                        },
                    "plugins": [
                        "themes", "json_data", "ui", "crrm"
                    ],
                    "themes": { "themes": "default-rtl", "dots": false, "icons": false }
                }).bind("select_node.jstree", function () {
                    var clicked = $.jstree._focused().get_selected();
                    var cls = clicked[0].className;
                    var id = clicked[0].id;
                    var name = clicked[0].attributes.entityName.value;
                    EntityClick(cls,id,name);
                });

            $("#btnAdd").click(function () {
                var id = $("#showEntitiesBar .jstree-clicked").parent().attr("id");
                if ($("#trExpenses").children().first().text() == "") {
                    if ($("#" + id).attr("entitytype") == "Cash") {
                        $("#trExpenses").children().first().text($("#showEntitiesBar .jstree-clicked").text());
                        $("#txtBalanceBefore").val("10000");
                    } else {
                        alert("Please choose \"Cash Entity\" first.");
                    }
                } else {
                    if ($("#" + id).attr("entitytype") == "Expenses") {
                        var repeated = false;
                        $(".expBalance").each(function () {
                            if ($(this).parent().children().first().text() == $("#showEntitiesBar .jstree-clicked").text())
                                repeated = true;
                        });
                        if (repeated == false) {
                            var Balance = $("#" + id).attr("Balance");
                            var BalanceBefore = $("#txtBalanceBefore").val();
                            var BalanceAfter;
                            var Amount = 0;
                            var expRow = "<tr><td>" + $("#showEntitiesBar .jstree-clicked").text() + "</td><td></td><td class='expBalance'>" + Balance + "</td><td></td><td><span title='delete' onclick='deleteEntity(this);' class='ui-button-icon-primary ui-icon ui-icon-closethick'></span></td></tr>";
                            $(expRow).insertAfter($("#trExpenses"));
                            $(".expBalance").each(function() {
                                Amount = parseFloat(Amount) - parseFloat($(this).text());
                            });
                            BalanceAfter = parseFloat(BalanceBefore) + Amount;
                            $("#txtBalanceAfter").val(BalanceAfter);
                            $("#txtAmount").val(Amount);
                        } else {
                            alert("You have repeated to choose \"Expense Entity\"!");
  
                        }
                    } else {
                        alert("Please choose \"Expenses Entities\".");
                    }
                }
                
            });

            $("#btnClear").click(function() {
                $("#trExpenses").children().first().text('');
                $("#txtBalanceBefore, #txtAmount, #txtBalanceAfter").val('');
                
            });

            $("#btnSave").click(function() {
                alert("Success!");
            });
        });
        function EntityClick(cls, id) {
            
        }

    </script>
    <style type="text/css">
        .ui-icon-closethick:hover {
            background-image: url(css/images/ui-icons_cd0a0a_256x240.png);
            cursor: pointer;
        }
        .normalInput {
            width: 80px;
        }

        input {
            width: 100px;
            font-weight: 700;
        }
        #showEntitiesBar {
            margin-top: 0px;
            height: 100%;
        }

        #showEntitiesBar a:hover {
            color: #a52a2a;
                background: #beebff;
                font-weight: bolder;
                line-height: 40px;
                height: 40px;
               
        }
            #showEntitiesBar a
            {
                font-weight: bolder;
                line-height: 40px;
                height: 40px;
                font-size: 22px;
            }
        .jstree-default .jstree-clicked
        {
            background: #beebff;
            border: 1px solid #99defd;
            padding: 0 2px 0 1px;
            font-weight: bold;
            color: #a52a2a;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="tableS">
            <tr>
                <td style="text-align: center"><input type="button" id="btnAdd" class="btn" value="Add" style="width: 50px"/></td>
                <td></td>
                <td>Period<input type="text" class="normalInput" id="Text1" readonly="readonly"/></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <div  id="showEntitiesBar" class=""></div>
                </td>
                <td style="background: white"></td>
                <td style="vertical-align: top">
                    <table id="tbExpenses">
                        <tr>
                            <td>Entity Name</td>
                            <td>Balance before</td>
                            <td>Amout</td>
                            <td>Balance after</td>
                            <td></td>
                        </tr>
                        <tr id="trExpenses">
                            <td></td>
                            <td><input type="text" class="normalInput" id="txtBalanceBefore" readonly="readonly" /></td>
                            <td><input type="text" class="normalInput" id="txtAmount" readonly="readonly" /></td>
                            <td><input type="text" class="normalInput" id="txtBalanceAfter" readonly="readonly" /></td>
                            <td><input type="button" id="btnClear" style="width:70px" value="Clear" class="btn" /></td>
                        </tr>
                    </table>
                    <table style="width: 460px; ">
                        <tr>
                            <td style="text-align: right"><input type="button" value="Save" id="btnSave" style="width: 60px" class="btn"/></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
