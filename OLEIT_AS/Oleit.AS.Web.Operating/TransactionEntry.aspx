<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AmountTransfer.aspx.cs" Inherits="Accounting_System.AmountTransfer" %>
<!DOCTYPE html>
<html ng-app xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Transaction Entry</title>
    <script src="js/angular.min.js"></script>
    <script src="js/jquery-1.9.1.min.js"></script>
    <link href="css/AccountingSystem.css" rel="stylesheet" />
    <script src="js/jquery-1.9.1.min.js"></script>
    <script src="js/commonFunc.js"></script>
    <script src="js/jquery-ui-1.9.2.custom.min.js"></script>
    <script src="js/jquery.jstree.js"></script>
    <script src="js/jquery-ui-1.9.2.custom.min.js"></script>
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <link href="css/Css.css" rel="stylesheet" />
    <script src="js/jquery.watermark.js"></script>
    <script type="text/javascript">
        var _subMarkVal = 0;//whether the AmountTransfer is inserted
        var AMCollection = new Entity();
        function Entity() {
            this.entity = [];
        }
        function Record() {
            this.record = [];
        }
        $(function () {
            $("#divErrorMsg").dialog({
                autoOpen: false,
                title: "SubTotals is not confirmed!",
                width: '460px',
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                },
                position: 'top'
            });


            $("#showEntitiesBar")
                .jstree({
                    "json_data": {
                        "data": eval(<%=JsonEntityTreeString%>)
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
                    EntityClick(cls, id, name);
                });


            $(".dr-menu a").click(function () {
                var id = $(this).attr("id");
                $("#container").attr("src", id + ".aspx");
            });

            $("#btnSubTotal").click(function () {
                var _subTotalObj = $("#trBeforeTransferTitle").next();
                var _id = $(_subTotalObj).attr("entityid");
                var _entity = new entityObj();
                _entity.EntityId = _id;
                _entity.Currency = $("#currency" + _id).text();
                _entity.ER = $("#er" + _id).text();
                _entity.BeforeBaseAmount = $("#beforeBaseAmount" + _id).text();
                _entity.BeforeSGDAmount = $("#beforeSGDAmount" + _id).text();
                AMCollection.entity.push(_entity);
                AMCollection.UserId = '<%=UserId%>';

                $.ajax({
                    type: "POST",
                    async: false,
                    url: "AmountTransferAjax.aspx",
                    data: {
                        json: JSON.stringify(AMCollection),
                        type: "SubTotal"
                    },
                    success: function (data) {
                        if (data != "Fail!") {
                            clearTb();
                            disableFunc("btnSubTotal");
                            disableFunc("btnTransfer");
                            var _dataAry = data.split(',');
                            $("#trTransferFirst,#trBeforeFirst,#trResultFirst").remove();
                            $(_dataAry[0]).insertAfter($("#trBeforeTransferTitle"));
                            $(_dataAry[1]).insertAfter($("#trTransferTitle"));
                            $(_dataAry[2]).insertAfter($("#trResultTitle"));
                            $(_dataAry[3]).insertAfter($("#tbAftertransferTitle"));
                            $("#tdTotal").text(_dataAry[4]);
                            $("#tdTransferSGD").text(_dataAry[5]);
                            $("#tdTransferPnL").text(_dataAry[6]);
                            $("#tdResultSGD").text(_dataAry[7]);
                            $("#tdSubTotal").text(_dataAry[8]);
                            enableFunc("btnSave");
                            AMCollection = new Entity();
                        } else {

                        }
                    },
                    error: function () {
                        alert("Fail!");
                    }
                });
            });

            $("#btnTransfer").click(function () {
                var _entityId;
                $("#tbBeforeTransfer tr").each(function () {
                    if (this.id != "trBeforeTransferTitle" && this.id != "trBeforeTableName" && this.id != "" && this.id != "tbBeforeTransferHr") {
                        var _id = $(this).attr("entityid");
                        var _type = $(this).attr("entitytype");
                        var _entityAry = $(this).children();
                        var _entity = new entityObj();
                        _entity.EntityId = _id;
                        _entity.SumType = $("#" + _id).attr("sumtype");
                        _entity.EntityType = _type;
                        _entity.Currency = $(_entityAry[1]).text();
                        _entity.ER = $(_entityAry[2]).text();
                        _entity.BeforeBaseAmount = $(_entityAry[3]).text();
                        _entity.BeforeSGDAmount = $(_entityAry[4]).text();
                        if (this.id == "trBeforeFirst" + _id) {
                            _entity.ResultBaseAmount = $($("#trResultFirst" + _id).children()[0]).text();
                            _entity.ResultSGDAmount = $($("#trResultFirst" + _id).children()[1]).text();
                            _entityId = _id;
                        }
                        else {
                            _entity.TransferBaseAmount = $("#transferId" + _id).children().children().val();
                        }
                        AMCollection.entity.push(_entity);
                        AMCollection.UserId = '<%=UserId%>';
                    }
                });

                $.ajax({
                    type: "POST",
                    async: false,
                    url: "AmountTransferAjax.aspx",
                    data: {
                        json: JSON.stringify(AMCollection),
                        entityId: _entityId,
                        type: "Transfer"
                    },
                    success: function (data) {
                        var _dataAry = data.split(',');
                        if (_dataAry[0] != "Fail!") {
                            clearTb();
                            disableFunc("btnSubTotal");
                            disableFunc("btnTransfer");
                            var _dataAry = data.split(',');
                            $("#trTransferFirst,#trBeforeFirst,#trResultFirst").remove();
                            $(_dataAry[0]).insertAfter($("#trBeforeTransferTitle"));
                            $(_dataAry[1]).insertAfter($("#trTransferTitle"));
                            $(_dataAry[2]).insertAfter($("#trResultTitle"));
                            $(_dataAry[3]).insertAfter($("#tbAftertransferTitle"));
                            $("#tdTotal").text(_dataAry[4]);
                            $("#tdTransferSGD").text(_dataAry[5]);
                            $("#tdTransferPnL").text(_dataAry[6]);
                            $("#tdResultSGD").text(_dataAry[7]);
                            $("#tdSubTotal").text(_dataAry[8]);
                            enableFunc("btnSave");
                            AMCollection = new Entity();
                        }
                        else {
                            $("#divErrorMsg").dialog({
                                autoOpen: true
                            }).html(_dataAry[1]);
                        }
                    },
                    error: function () {
                        alert("Fail!");
                    }
                });

                AMCollection = new Entity();

            });

            $("#btnAdd").click(function () {
                if ($("#hfConfirmVal").val() == "1") {
                    $("#tbBeforeTransfer tr,#tbTransfer tr,#tbResult tr,#tbAftertransfer tr").each(function () {
                        var _entityid = $(this).attr("entityid");
                        if (_entityid)
                            $(this).remove();
                    });
                    $("#tdTotal").text("0");
                    $("#tdTransferSGD").text("0");
                    $("#tdTransferPnL").text("0");
                    $("#tdResultSGD").text("0");
                    $("#tdSubTotal").text("0");
                    $("#hfConfirmVal").val("0");
                    _subMarkVal = 0;
                }
                var id = $("#showEntitiesBar .jstree-clicked").parent().attr("id");
                var repeated = false;
                $("#tbBeforeTransfer tr").each(function () {
                    var entityId = $(this).attr("entityid");
                    if (entityId == id)
                        repeated = true;
                });//entityId
                if (id != null && !repeated) {
                    var _firstCheck = "true";
                    $("#tbBeforeTransfer tr").each(function () {
                        if (this.id != "trBeforeTableName" && this.id != "trBeforeTransferTitle" && this.id != "tbBeforeTransferHr" && this.id != "") {
                            _firstCheck = "false";
                        }
                    });
                    $.ajax({
                        type: "POST",
                        async: false,
                        url: "AmountTransferAjax.aspx",
                        data: {
                            entityId: id,
                            first: _firstCheck,
                            type: "Add"
                        },
                        success: function (data) {
                            if (data.indexOf("Fail!") == -1) {
                                var _ary = data.split(',');
                                $(_ary[0]).insertBefore($("#tbBeforeTransferHr"));
                                $(_ary[1]).insertBefore($("#tbTransferHr"));
                                _subMarkVal++;
                                enableFunc("btnSubTotal");
                                if ($("#" + id).attr("accountid") != "0") {
                                    disableFunc("btnSubTotal");
                                }
                                if (_subMarkVal >= 2) {
                                    enableFunc("btnTransfer");
                                    disableFunc("btnSubTotal");
                                }
                            }
                            else {
                                alert(data);
                            }
                        },
                        error: function () {
                            AMCollection = new Entity();
                            alert("Fail!");
                        }
                    });
                } else {
                    if (id == null)
                        alert("You don't choose anyone!");
                    else {
                        alert("Bad choose! You can't choose repeated.");
                    }
                }
            });

            $("#btnSave").click(function () {
                if ($("#tdSubTotal").text() != "0") {
                    alert("SubTotal must be \"0\"!");
                    return false;
                }
                var _entityId;
                $("#tbBeforeTransfer tr").each(function () {
                    if (this.id != "trBeforeTransferTitle" && this.id != "trBeforeTableName" && this.id != "" && this.id != "tbBeforeTransferHr") {
                        var _id = $(this).attr("entityid");
                        var _entityAry = $(this).children();
                        var _entity = new entityObj();
                        _entity.EntityId = _id;
                        _entity.Currency = $(_entityAry[1]).text();
                        _entity.ER = $(_entityAry[2]).text();
                        _entity.BeforeBaseAmount = $(_entityAry[3]).text();
                        _entity.BeforeSGDAmount = $(_entityAry[4]).text();
                        if (this.id == "trBeforeFirst" + _id) {
                            _entity.ResultBaseAmount = $($("#trResultFirst" + _id).children()[0]).text();
                            _entity.ResultSGDAmount = $($("#trResultFirst" + _id).children()[1]).text();
                            _entityId = _id;
                        }
                        else {
                            _entity.ResultBaseAmount = $($("#resultId" + _id).children()[0]).text();
                            _entity.ResultSGDAmount = $($("#resultId" + _id).children()[1]).text();
                            _entity.TransferBaseAmount = $("#transferId" + _id).children().children().val();
                            _entity.TransferSGDAmount = $($("#transferId" + _id).children()[1]).text();
                            _entity.PnL = $($("#transferId" + _id).children()[2]).text();
                        }
                        AMCollection.entity.push(_entity);
                        AMCollection.UserId = '<%=UserId%>';
                    }
                });
                recordFunc();
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "AmountTransferAjax.aspx",
                    data: {
                        json: JSON.stringify(AMCollection),
                        entityId: _entityId,
                        type: "Save"
                    },
                    success: function (data) {
                        var _dataAry = data.split(',');
                        enableFunc("btnConfirm");
                        disableFunc("btnSave");
                        alert(_dataAry[0]);
                        $("#hfRecordId").val(_dataAry[1]);
                    },
                    error: function () {
                        alert("Fail!");
                    }
                });
                AMCollection = new Entity();
            });
            $("#btnConfirm").click(function () {
                recordFunc();
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "AmountTransferAjax.aspx",
                    data: {
                        json: JSON.stringify(AMCollection),
                        recordId: $("#hfRecordId").val(),
                        type: "Confirm"
                    },
                    success: function (data) {
                        if (data != "Fail!") {
                            disableFunc("btnConfirm");
                            disableFunc("btnSave");
                            $("#hfConfirmVal").val("1");
                        }
                        alert(data);
                    },
                    error: function () {
                        alert("Fail!");
                    }
                });
                AMCollection = new Entity();

            });
        });

        function clearTb() {
            $("tr").each(function () {
                if ($(this).attr("entityid"))
                    $(this).remove();
            });
        }


        function recordFunc() {
            AMCollection.record = new Array();
            $("#tbAftertransfer tr").each(function () {
                if (this.id != "tbAftertransferTitle" && this.id != "") {
                    var _id = $(this).attr("entityid");
                    var _record = new recordObj();
                    _record.EntityId = _id;
                    _record.Currency = $($(this).children()[2]).text();
                    _record.ER = $($(this).children()[3]).text();
                    _record.BaseAmount = $($(this).children()[4]).text();
                    _record.SGDAmount = $($(this).children()[5]).text();
                    AMCollection.record.push(_record);
                }
            });
        }

        function entityObj(_entityId, _entityType, _currency, _er, _beforeBaseAmount, _beforeSGDAmount, _transferBaseAmount, _transferSGDAmount, _PnL, _resultBaseAmount, _resultSGDAmount) {
            this.EntityId = _entityId,
            this.EntityType = _entityType,
            this.Currency = _currency,
            this.ER = _er,
            this.BeforeBaseAmount = _beforeBaseAmount,
            this.BeforeSGDAmount = _beforeSGDAmount,
            this.TransferBaseAmount = _transferBaseAmount,
            this.TransferSGDAmount = _transferSGDAmount,
            this.PnL = _PnL,
            this.ResultBaseAmount = _resultBaseAmount,
            this.ResultSGDAmount = _resultSGDAmount;
        }

        function recordObj(_entityId, _currency, _er, _baseAmount, _SGDAmount) {
            this.EntityId = _entityId,
            this.Currency = _currency,
            this.ER = _er,
            this.BaseAmount = _baseAmount,
            this.SGDAmount = _SGDAmount;
        }

        //delete table tr (row)
        function deleteEntityCheck(obj) {
            $(obj).fadeOut(function () {
                var _deleteId = $(this).parent().parent().attr("entityid");
                $(this).parent().parent().remove();
                $("tr[entityid='" + _deleteId + "']").remove();
                var _hasEntity = 0;
                $("#tbBeforeTransfer tr").each(function () {
                    if (this.id.indexOf("Title") == -1 && $(this).attr("entityid"))
                        _hasEntity++;
                });
                if (_hasEntity <= 1) {

                    $("#btnSubTotal").removeAttr("disabled");
                    $("#btnSubTotal").attr("class", "btn01");
                    $("#subTotal").text('0');
                    $("#tdTransferSGD").text('0');
                    $("#tdTransferPnL").text('0');
                    $("#tdTotal").text('0');

                    disableFunc("btnSave");
                    disableFunc("btnTransfer");
                    AMCollection = new Entity();
                    _subMarkVal = _hasEntity;
                    if (_hasEntity == 0) {
                        clearTb();
                        disableFunc("btnSubTotal");
                    }
                }
            });

        }

        function disableFunc(_id) {
            $("#" + _id).attr("class", "disablebtn");
            $("#" + _id).prop("disabled", true);
        }

        function enableFunc(_id) {
            $("#" + _id).attr("class", "btn01");
            $("#" + _id).removeAttr("disabled");
        }

        function EntityClick(cls, id) {

        }
        var checkMark = false;
        var pageId;
        function changePage(id) {
            checkMark = true;
            pageId = id;
        }

    </script>
    <style type="text/css">
		.rfdRoundedCorners{
	border-radius: 12px;
	border: 1px solid #3a3732;
	font-family: Verdana, Geneva, sans-serif;
	font-size: 12px;
	font-style: normal;
	line-height: normal;
	font-weight: normal;
	font-variant: normal;
	text-transform: none;
	color: #bb9d67;
}
        .ui-icon-closethick:hover {
            background-image: url(css/images/ui-icons_cd0a0a_256x240.png);
            cursor: pointer;
        }
       
        #showEntitiesBar {
            margin-top: 0px;
            height: 100%;
		-webkit-border-radius: 5px;
	     border-radius: 5px;
		text-align: left;
        }

        #showEntitiesBar a:hover {
            color: #ed800c;
	background: #ccffff;
	font-weight: bold;
	line-height: 14px;
	font-family: Verdana, Geneva, sans-serif;
	font-size: 13px;
	font-style: normal;
	font-variant: normal;
	text-transform: none;
	text-decoration: none;
	border: 1px solid #04f9e5;
	-webkit-border-radius: 5px;
	border-radius: 5px;
	word-spacing: 2px;
	padding-top: 7px;
	height: 22px;
	padding-right: 3px;
	padding-left: 3px;
	text-align: left;
               
        }
            #showEntitiesBar a
            {
                line-height: normal;
	font-size: 13px;
	font-family: Verdana, Geneva, sans-serif;
	font-style: normal;
	font-variant: normal;
	text-transform: none;
	-webkit-border-radius: 5px;
	padding-top: 7px;
	height: 22px;
	padding-right: 3px;
	padding-left: 3px;
	border-radius: 5px;
	word-spacing: 2px;
	text-align: left;
            }
        .jstree-default .jstree-clicked
        {
	color: #bf0000;
            font-weight: bold;
        }
        

        .tableAT
        {
            padding: 5px 10px 5px 5px;
            border-spacing: 10px 10px;
            border: 1px solid #E0E0E0;
            border-collapse: collapse;
            background: #E0E0E0;
        }
            .tableAT tr td
            {
                border: 1px solid #fff;
                color: #4B4B4B;
                text-align: left;
            }
            .tableAT .tableName
            {
	color: #ffffff;
                font-weight: bold;
	font-size: 14px;
	font-family: Verdana, Geneva, sans-serif;
	font-style: normal;
	line-height: normal;
	font-variant: normal;
	text-transform: none;
	background-color: #d5e4ab;
            }
            .tableTransfer .tableName{
	font-family: Verdana, Geneva, sans-serif;
	font-size: 14px;
	font-style: normal;
	line-height: 15px;
	font-weight: bold;
	font-variant: normal;
	text-transform: none;
	color: #ffffff;
	text-decoration: none;
	background-color: #d5e4ab;
            }

            .tableAT a
            {
                color: #2f4f4f;
                text-decoration: none;
            }
                .tableAT a:hover
                {
                    color: #DC143c;
                    font-weight: bold;
                }
        #trBeforeTransferTitle td, #trTransferTitle td, #trResultTitle td,#tbAftertransferTitle td
        {
            text-align: center;

        }

        input {
            width: 80px;
        }
        #tbBeforeTransfer td,#tbTransfer td,#tbResult td {
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
    <form id="form1" runat="server"><br><br><center><label class="TE_wd04">Transaction Entry</label>
    <div style="width: 1300px; height: 765px; border: none; text-align: left" class="a03_border"><br><br>
<table border="0" cellspacing="0" cellpadding="0px" style="position: relative; left: 20px">
                <tr>
    <td align="left" valign="top" width="265px"><input id="btnAdd" type="button" value="" title="Add" class="Add_a" /><div style="width: 100%; height: 670px; overflow: auto"><br><div id="showEntitiesBar" style="position: relative; top: 9px">
                            <script type="text/javascript">
                                $(function () {

                                });
                            </script>

                        </div></div></td>
    <td colspan="2" align="left" valign="top"><div style="width: 100%; height: 701px; overflow: auto; position: relative; left: 14px"><fieldset class="rfdRoundedCorners" style="width: 962px; position: relative; top: -7px">
                                        <legend class="legend_wd"><span id="pageTitle" class="legend_wd">Amount Transfer Calculation</span></legend>
<input id="btnSubTotal" type="button" value="SubTotal" disabled="disabled" class="disablebtn" />&nbsp;<input id="btnTransfer" type="button" value="Transfer" disabled="disabled" class="disablebtn" aria-grabbed="undefined" />&nbsp;<input id="btnSave" type="button" value="Save" disabled="disabled" class="disablebtn" />&nbsp;<input id="btnConfirm" type="button" value="Confirm" disabled="disabled" class="disablebtn" />
                                <input type="hidden" id="hfConfirmVal" />
                                <input type="hidden" id="hfRecordId" />
        <hr class="a_line"></hr>
        <table border="0" cellpadding="1px" cellspacing="0">
                            <tr>
    <td align="left" valign="top" width="510px"><div id="BeforeTransfer"><table id="tbBeforeTransfer" border="1" cellpadding="0px"  cellspacing="0px" style="color: #ffffff; font-weight: bold; font-family: Verdana, Geneva, sans-serif; font-size: 13px; word-spacing: 2px; border-color: #696969; border-collapse:collapse">
                                                        <tr id='trBeforeTableName'>
                                                            <td height="32" colspan="6" align="center" valign="middle" style="font-family: Verdana, Geneva, sans-serif;
	font-size: 14px;
	font-style: normal;
	line-height: 15px;
	font-weight: bold;
	font-variant: normal;
	text-transform: none;
	color: #4b7b03;
	text-decoration: none;
	background-color: #d5e4ab;">BeforeTransfer</td>
                                                        </tr>
                                                        <tr id="trBeforeTransferTitle">
                                                          <td height="30" align="center" valign="middle" bgcolor="#5fa11f" style="background-image:url(img/bg09.gif); background-repeat: repeat-x; border-color: #696969; width: 250px">Entity</td>
                                                            <td align="center" valign="middle" bgcolor="#5fa11f" style="background-image:url(img/bg09.gif); background-repeat: repeat-x; padding-left: 7px; padding-right: 7px; border-color: #696969">Currency</td>
                                                            <td align="center" valign="middle" bgcolor="#5fa11f" style="background-image:url(img/bg09.gif); background-repeat: repeat-x; border-color: #696969; width: 86px">Rate</td>
                                                            <td align="center" valign="middle" bgcolor="#5fa11f" style="background-image:url(img/bg09.gif); background-repeat: repeat-x; border-color: #696969; width: 87px">Base</td>
                                                            <td align="center" valign="middle" bgcolor="#5fa11f" style="background-image:url(img/bg09.gif); background-repeat: repeat-x; border-color: #696969; width: 86px">SGD</td>
                                                            <td align="center" valign="middle" bgcolor="#5fa11f" style="background-image:url(img/bg09.gif); background-repeat: repeat-x; padding-left: 7px; padding-right: 7px; border-color: #696969"></td>
                                                            
                                                        </tr>
                                                        <tr id="tbBeforeTransferHr">
                                                        </tr>
                                                        <tr style="color: #F30; font-weight: bold; background-color: #696969">
                      <td  height="26" align="right" valign="middle" style="color: #ffffff">Total : &nbsp;</td><td colspan="5" align="left" valign="middle" id="tdTotal" style="color: #ffffff"></td></tr>
                </table></div></td>
    <td align="left" valign="top"><div id="Transfer"><table id="tbTransfer" border="1" cellpadding="0px"  cellspacing="0px" style="color: #ffffff; font-weight: bold; font-family: Verdana, Geneva, sans-serif; font-size: 13px; word-spacing: 2px; border-color: #696969; border-collapse:collapse">
                                                        <tr id='trTransferTableName'>
                                                            <td height="32" colspan="3" align="center" valign="middle" style="font-family: Verdana, Geneva, sans-serif; font-size: 14px; font-style: normal; line-height: 15px; font-weight: bold; font-variant: normal; text-transform: none; color: #4b7b03; text-decoration: none; background-color: #d5e4ab;">Transfer</td>
                                                        </tr>
                                                        <tr id="trTransferTitle">
                                                            <td height="30px" align="center" valign="middle" bgcolor="#5fa11f" style="background-image:url(img/bg09.gif); background-repeat: repeat-x; border-color: #696969; width: 87px">Base</td>
                                                            <td align="center" valign="middle" bgcolor="#5fa11f" style="background-image:url(img/bg09.gif); background-repeat: repeat-x; border-color: #696969; width: 86px">SGD</td>
                                                            <td align="center" valign="middle" bgcolor="#5fa11f" style="background-image:url(img/bg09.gif); background-repeat: repeat-x; border-color: #696969; width: 122px">Profit & Loss</td>
                                                        </tr>
                                                        <tr id="tbTransferHr"></tr>
                                                        <tr style="color: #F30; font-weight: bold; background-color: #696969">
                                                          <td height="26" align="right" valign="middle">&nbsp;</td>
                                                            <td align="right" valign="middle" id="tdTransferSGD" style="color: #ffffff; padding-right: 4px">0.00</td>
                                                            <td align="right" valign="middle" id="tdTransferPnL" style="color: #ffffff; padding-right: 4px">0.00</td>
                                                        </tr>

                </table></div></td>
    <td align="right" valign="top"><div id="Result"><table id="tbResult" border="1" cellpadding="0px"  cellspacing="0px" style="color: #ffffff; font-weight: bold; font-family: Verdana, Geneva, sans-serif; font-size: 13px; word-spacing: 2px; border-color: #696969; border-collapse:collapse">
                                                        <tr id='trResultTableName'>
                                                            <td height="32" colspan="2" align="center" valign="middle" style="font-family: Verdana, Geneva, sans-serif;
	font-size: 14px;
	font-style: normal;
	line-height: 15px;
	font-weight: bold;
	font-variant: normal;
	text-transform: none;
	color: #4b7b03;
	text-decoration: none;
	background-color: #d5e4ab;">Result</td>
                                                        </tr>
                      <tr style="background-image:url(img/bg09.gif); background-repeat: repeat-x" id="trResultTitle">
                                                            <td height="30px" align="center" valign="middle" bgcolor="#5fa11f" style="background-image:url(img/bg09.gif); background-repeat: repeat-x; border-color: #696969; width: 87px">Base</td>
                                                            <td align="center" valign="middle" bgcolor="#5fa11f" style="background-image:url(img/bg09.gif); background-repeat: repeat-x; border-color: #696969; width: 86px">SGD</td>
                                                        </tr>
                                                        <tr id="tbResultHr"></tr>
                                                        <tr style="color: #F30; font-weight: bold; background-color: #696969">
                                                          <td style="height: 26px" align="center" valign="middle">&nbsp;</td>
                                                            <td align="right" valign="middle" id="tdResultSGD" style="color: #ffffff; padding-right: 4px">0.00</td>
                                        </tr>
                </table></div></td>
                                                        </tr>
                                        <tr>
    <td colspan="3"><br><div id="Aftertransfer"><table width="100%" id="tbAftertransfer" border="1" cellpadding="0px"  cellspacing="0px" style="color: #ffffff; font-weight: bold; font-family: Verdana, Geneva, sans-serif; font-size: 13px; word-spacing: 2px; border-color: #696969; border-collapse:collapse">
                                                        <tr id="tbAftertransferTitle" style="background-image:url(img/bg09.gif); background-repeat: repeat-x">
                                                            <td height="30" align="center" valign="middle">Type</td>
                                                            <td align="center" valign="middle">Entity</td>
                                                            <td align="center" valign="middle">Currency</td>
                                                            <td align="center" valign="middle">Rate</td>
                                                            <td align="center" valign="middle">Base</td>
                                                            <td align="center" valign="middle">SGD</td>
                                                        </tr>
                                                        <tr style="color: #F30; font-weight: bold; background-color: #696969">
                                                            <td height="26" colspan="4" style="border: 0px"></td>
                                                            <td align="right" valign="middle" style="color: #ffffff">Sub Total : </td>
                                                            <td align="left" valign="middle" id="tdSubTotal" style="color: #ffffff; padding-left: 4px">0</td>
                                        </tr>
                </table></div></td>
                            </tr>
</table>

</fieldset></div><br /></td>
                </tr>
</table><br><div id="divErrorMsg"></div></div></center>
        
    </form>
</body>
</html>