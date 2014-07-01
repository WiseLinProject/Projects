<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tally.aspx.cs" Inherits="Accounting_System.Tally" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Tally</title>
    <link href="css/AccountingSystem.css" rel="stylesheet" />
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <script src="js/commonFunc.js"></script>
    <script src="js/jquery-1.9.1.min.js"></script>
    <script src="js/jquery-ui-1.9.2.custom.min.js"></script>
    <script src="js/jquery.jstree.js"></script>
    <script src="js/jquery.watermark.js"></script>
    <link href="css/Css.css" rel="stylesheet" type="text/css">
<script type="text/javascript">
        var _confirmId;
        $(function() {
            
            $("#btnSave").click(function() {
                var TallyEntities = "";
                $("li[class*='jstree-checked']").each(function() {
                    TallyEntities += $(this).attr("id") + ",";
                });
                $("#hfTallyEntities").val(TallyEntities.substring(0, TallyEntities.length - 1));
                alert(TallyEntities.substring(0, TallyEntities.length - 1));
                return true;
            });
            $("#ddlMainEntity").change(function() {
                EntityClick($(this).val());
                
            });
            
        });

        function cssAdjust() {
            //$($("li[sumtype='1']").children()[2]).prepend('<table><tr><td style="height:10px;border:0px"></td></tr></table>');
            $("li[sumtype='0']").each(function () {
                $($(this).children()[1]).css("height","27px");
            });
        }

        function EntityClick(_id) {
            $("#showTallyTree")
                .jstree({
                    "json_data": {
                        "ajax": {
                            "url": "TallyAjax.aspx",
                            "data": { "entityId": _id, "type": "load", "periodID": $("#ddlYear").val() + "." + $("#ddlMonth").val() + "." + $("#ddlWeek").val() }
                        }
                    },
                    "core": { "html_titles": true },
                    "plugins": [
                        "themes", "json_data", "ui", "crrm"
                    ],
                    "themes": { "themes": "default-rtl", "dots": false, "icons": false }
                    
                }).bind('loaded.jstree', function (e, data) {
                    // invoked after jstree has loaded
                    cssAdjust();
                }).on('loaded.jstree', function () {
                    $("#showTallyTree").jstree('open_all');
                });;
            
        }
        function tallyClick() {
            
        }

        function confirmCheck(_id) {
            if (confirm("Do you really want to \"Confrim\"?")) {
                var entityId = _id.substring(9, _id.length);
                $.ajax({
                    type: "POST",
                    aysnc: false,
                    url: "TallyAjax.aspx",
                    dataType: "json",
                    data: { entityId: entityId, type: "confirm" },
                    success: function (data) {
                        for (var i = data.length-1; i >= 0; i--) {
                            var _updateId = data[i].Entity.EntityID;
                            if (data[i].Entity.SumType == 2) {
                                $("#BaseTransfer" + _updateId).val(data[i].BaseTransfer);
                                $("#preBalance" + _updateId).val(data[i].BasePrevBalance);
                                $("#transaction" + _updateId).val(data[i].BaseTransaction);
                                $("#balance" + _updateId).val(data[i].BaseBalance);
                            }
                            else {
                                $("#winAndLoss" + _updateId).text(data[i].BaseWinAndLoss);
                            }
                            $("#cbConfirm" + _updateId).attr("id", "cbConfirm" + _updateId + "1");
                            var _newInput = "<input type='checkbox' id='cbConfirm" + _updateId + "' name='confirm' disabled checked>";
                            $(_newInput).insertAfter($("#cbConfirm" + _updateId + "1"));
                            $("#cbConfirm" + _updateId + "1").remove();
                        }
                        alert("Success!");
                    },
                    error: function () {
                        alert("Internal Error! Please try again or contact server administrator.");
                    }
                });
            }
        }

    </script>
    <style type="text/css">
        
        #where a:link{
        color:#8E63B7;
        text-decoration: none;
        font-size:13px;
}
#where a:visited{color:#8E63B7; text-decoration: none; }
#where a:hover{color:#8E63B7; text-decoration: underline; }
#where a:active{
        color:#8E63B7;
        text-decoration: none;
}

.wherehightlight {
        color:#eb0000;
        text-decoration: none;
        font-size:24px;
}
        .ui-icon-closethick:hover {
            background-image: url(css/images/ui-icons_cd0a0a_256x240.png);
            cursor: pointer;
        }

        #requiredHint {
            display: none;
        }

        #showTallyTree
        {
	margin-top: 0px;
;
        }
		

            #showTallyTree a
            {
	height: 62px;
	border-top-width: 0px;
	border-right-width: 0px;
	border-bottom-width: 0px;
	border-left-width: 0px;
	border-top-style: none;
	border-right-style: none;
	border-bottom-style: none;
	border-left-style: none;
            }
            #showTallyTree .jstree-clicked
            {
	-webkit-border-radius: 0px;
	border-radius: 0px;
	background-color: transparent;
	border: 0px none transparent;
            }
            #showTallyTree .jstree-hovered
            {
	border: 0px;
	padding: 0;
            }
        input[type=text]{
	width: 120px;
	font-family: Verdana, Geneva, sans-serif;
	background-image: url(../img/bg03.png);
	background-color: #28251a;
	height: 20px;
	border: 1px inset #414246;
	font-size: 15px;
	font-style: normal;
	line-height: 16px;
	font-weight: normal;
	font-variant: normal;
	text-transform: none;
	color: #ffbf00;
	padding-left: 5px;
	text-decoration: none;
        }
		
        #tbTransaction td {
	height: 28px;
	font-weight: bold;
	color: #FFF;
	font-family: Verdana, Geneva, sans-serif;
	font-size: 13px;
	font-style: normal;
	line-height: normal;
	font-variant: normal;
	text-transform: none;
	width: 100%;
	text-align: center;
	border-top-width: 0px;
	border-right-width: 0px;
	border-bottom-width: 0px;
	border-left-width: 0px;
	border-top-style: none;
	border-right-style: none;
	border-bottom-style: none;
	border-left-style: none;
	padding-top: 0px;
	padding-right: 2px;
	padding-bottom: 0px;
	padding-left: 2px;
	background-image: url(img/bg09.gif);
	background-repeat: repeat-x;
	word-spacing: 1px;
	margin: 0px;
        }
		
		        #tbSubtotal td {
	height: 28px;
	font-weight: bold;
	color: #5a8d01;
	font-family: Verdana, Geneva, sans-serif;
	font-size: 13px;
	font-style: normal;
	line-height: normal;
	font-variant: normal;
	text-transform: none;
	text-align: center;
	background-color: #e7f6bc;
	border-top-width: 0px;
	border-right-width: 0px;
	border-bottom-width: 0px;
	border-left-width: 0px;
	border-top-style: none;
	border-right-style: none;
	border-bottom-style: none;
	border-left-style: none;
	width: 100%;
	padding-top: 0px;
	padding-right: 2px;
	padding-bottom: 0px;
	padding-left: 2px;
	word-spacing: 1px;
	margin: 0px;
        }
		

		
		
        #showTallyTree a {
	font-weight: normal;
	color: #FFF;
	font-family: Verdana, Geneva, sans-serif;
	font-size: 13px;
	font-style: normal;
	line-height: normal;
	font-variant: normal;
	text-transform: none;
        }
        .tbAcc td{
	height: 28px;
	font-weight: bold;
	color: #a21301;
	font-family: Verdana, Geneva, sans-serif;
	font-size: 13px;
	font-style: normal;
	line-height: normal;
	font-variant: normal;
	text-transform: none;
	text-align: center;
	padding-right: 8px;
	padding-left: 8px;
	word-spacing: 1px;
	border-top-width: 0px;
	border-right-width: 0px;
	border-bottom-width: 0px;
	border-left-width: 0px;
	border-top-style: none;
	border-right-style: none;
	border-bottom-style: none;
	border-left-style: none;
	background-color: #f8d4cb;
	margin: 0px;
        }

    body,td,th {
	font-size: 13px;
}
    body {
	background-color: #272727;
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
}
    </style>
<meta charset="utf-8">
</head>
<body>
    <form id="form1" runat="server"><br><br><center><label class="TE_wd04">Tally</label>
    <div style="width: 900px; border: none; text-align: center" class="a03_border"><br><br>
        <div style="width: 100%; height: 670px; overflow-x: hidden; overflow-y: auto; "><table width="95%" style="height: 500px" align="center">
            
            <tr >
                 
                <!-- background: -webkit-linear-gradient(left, blue 25%, red 50% ,  yellow 25%, green );-->          
                <td  style="vertical-align:top">
                    <table style="border:0px; background-color: #FFF;  width:100%; color: #000000; font-family: Verdana; font-size: 12px; font-weight: bold" align="center">
                        <tr>
                            <td colspan="3" align="left" valign="middle">&nbsp;&nbsp;Cash Main Entity List : <select id="ddlMainEntity" runat="server" class="a02_input_02_a"></select></td>
                            <td colspan="2">Period : <asp:DropDownList ID="ddlYear" runat="server" CssClass="a02_input_02_a" Width="95px"></asp:DropDownList>
                            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="a02_input_02_a" Width="50px">
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                              <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>7</asp:ListItem>
                                <asp:ListItem>8</asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
                              <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                <asp:ListItem>13</asp:ListItem>
                                </asp:DropDownList>
                            <asp:DropDownList ID="ddlWeek" runat="server" CssClass="a02_input_02_a" Width="50px">
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                </asp:DropDownList>
                            
                                </td>
                            <td align="right" valign="middle">Export all Cash Entities : </td>
                            <td align="left" valign="middle"><asp:ImageButton ImageUrl="~/img/excel_img.gif" ToolTip="Export to excel" ID="imgbtnExcel" runat="server" OnClick="imgbtnExcel_Click" /></td>
                        </tr>
                    </table>
                    
                <br/>
                <br/>
                    <div id="showTallyTree" style="width: 770px; text-align: left"></div>
                </td>

            </tr>
        </table></div>
<br><br></div></center>
    </form>
</body>
</html>
