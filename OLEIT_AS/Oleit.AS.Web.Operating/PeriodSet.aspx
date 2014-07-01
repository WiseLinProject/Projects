<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeriodSet.aspx.cs" Inherits="Accounting_System.PeriodSet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="css/AccountingSystem.css" rel="stylesheet" />
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <link href="css/table.css" rel="stylesheet" />
    <link href="css/Css.css" rel="stylesheet" type="text/css">
    <script src="js/jquery-1.9.1.min.js"></script>
    <script src="js/jquery-ui-1.9.2.custom.min.js"></script>
    <script src="js/jquery.watermark.js"></script>
    <script src="js/commonFunc.js"></script>
    <title>Period Setting</title>
    <script type="text/javascript">
        $(function() {
            $("#yearSet").watermark("please enter AD year");
        });


    </script>
    <style type="text/css">
        
        #yearPeriod {
            margin-top: 20px;
        }
        .tablePeriod
        {
            border-spacing: 10px;
            border: 1px solid #E0E0E0;
            border-collapse:collapse;
            background: #E0E0E0;
        }
            .tablePeriod tr td
            {
                border: 2px solid #fff;
                color: black;
                font-weight: 300;
                font-size: 20px;
                text-align: left;
                width: 150px
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
    <form id="form1" runat="server"><br /><br />
        <center><label class="TE_wd04">Period Setting</label>
        <div style="width: 860px; border: none; text-align: left" class="a03_border">
                <br /><br /><div style="width: 100%; height: 670px; overflow-x: hidden; overflow-y: auto; ">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Year&nbsp;:&nbsp;<asp:TextBox CssClass="a01_input" ID="yearSet" runat="server" onkeydown="return checkNum(event);"></asp:TextBox>&nbsp;<asp:Button runat="server" ClientIDMode="Static" CssClass="Set" title="Set" Text="" ID="btnSet" OnClick="btnSet_Click" />
       <br /><br />
        <table class="bordered" style="width: 75%" align="center">
            <thead>
            <tr>
                <th>Period No.</th>
                <th>Start Date</th>
                <th>End Date</th>
            </tr>
                </thead>
            <asp:Repeater runat="server" ID="rptPeriod">
                <ItemTemplate>
                    <tr>
                        <td style="text-align: left; color: #c0c7c1; font-weight: bold; background-image: url(../img/bg03.png); background-repeat: repeat-x">&nbsp;<%# Eval("PeriodNo") %></td>
                        <td style="text-align: center"><%# ((DateTime)Eval("StartDate")).ToString("yyyy/MM/dd") %></td>
                        <td style="text-align: center"><%# ((DateTime)Eval("EndDate")).ToString("yyyy/MM/dd") %></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div><br /><br /></div></center>
    </form>
</body>
</html>
