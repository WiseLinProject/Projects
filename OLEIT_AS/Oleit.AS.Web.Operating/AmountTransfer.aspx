<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AmountTransfer.aspx.cs" Inherits="Accounting_System.AmountTransfer" %>
<!DOCTYPE html>
<html ng-app xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="js/angular.min.js"></script>
    <script src="js/jquery-1.9.1.min.js"></script>
    <link href="css/AccountingSystem.css" rel="stylesheet" />
    <script src="js/jquery-1.9.1.min.js"></script>
    <script src="js/commonFunc.js"></script>
    <script src="js/jquery-ui-1.9.2.custom.min.js"></script>
    <script src="js/jquery.jstree.js"></script>
    <script src="js/jquery-ui-1.9.2.custom.min.js"></script>
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <script src="js/jquery.watermark.js"></script>
    <script type="text/javascript">
        var _subMarkVal =0;//whether the AmountTransfer is inserted
        var  AMCollection = new Entity();
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
                    EntityClick(cls,id,name);
                });
            
            
            $(".dr-menu a").click(function () {
                var id = $(this).attr("id");
                $("#container").attr("src", id + ".aspx");
            });

            $("#btnSubTotal").click(function() {
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
                    success: function(data) {
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
                    error: function() {
                        alert("Fail!");
                    }
                });
            });

            $("#btnTransfer").click(function() {
                var _entityId;
                $("#tbBeforeTransfer tr").each(function() {
                    if(this.id!="trBeforeTransferTitle" && this.id!="trBeforeTableName" && this.id!="" && this.id!="tbBeforeTransferHr") {
                        var _id = $(this).attr("entityid");
                        var _type = $(this).attr("entitytype");
                        var _entityAry = $(this).children();
                        var _entity = new entityObj();
                        _entity.EntityId = _id;
                        _entity.SumType = $("#"+_id).attr("sumtype");
                        _entity.EntityType = _type;
                        _entity.Currency = $(_entityAry[1]).text();
                        _entity.ER = $(_entityAry[2]).text();
                        _entity.BeforeBaseAmount = $(_entityAry[3]).text();
                        _entity.BeforeSGDAmount = $(_entityAry[4]).text();
                        if(this.id=="trBeforeFirst"+_id) {
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
                        entityId:_entityId,
                        type:"Transfer"
                    },
                    success: function(data) {
                        var _dataAry = data.split(',');
                        if(_dataAry[0]!="Fail!") {
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
                    error: function() {
                        alert("Fail!");
                    }
                });

                AMCollection = new Entity();

            });

            $("#btnAdd").click(function() {
                if($("#hfConfirmVal").val()=="1")
                {
                    $("#tbBeforeTransfer tr,#tbTransfer tr,#tbResult tr,#tbAftertransfer tr").each(function(){
                        var _entityid= $(this).attr("entityid");
                        if(_entityid)
                        $(this).remove();
                    });
                    $("#tdTotal").text("0");
                    $("#tdTransferSGD").text("0");
                    $("#tdTransferPnL").text("0");
                    $("#tdResultSGD").text("0");
                    $("#tdSubTotal").text("0");
                    $("#hfConfirmVal").val("0");
                    _subMarkVal=0;
                }
                var id = $("#showEntitiesBar .jstree-clicked").parent().attr("id");
                var repeated = false;
                $("#tbBeforeTransfer tr").each(function () {
                    var entityId = $(this).attr("entityid");
                    if(entityId==id)
                        repeated = true;
                });//entityId
                if (id != null && !repeated) {
                    var _firstCheck = "true";
                    $("#tbBeforeTransfer tr").each(function() {
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
                                first:_firstCheck,
                                type:"Add"
                            },
                            success: function(data) {
                                if (data.indexOf("Fail!") ==-1) {
                                    var _ary = data.split(',');
                                    $(_ary[0]).insertBefore($("#tbBeforeTransferHr"));
                                    $(_ary[1]).insertBefore($("#tbTransferHr"));
                                    _subMarkVal++;
                                    enableFunc("btnSubTotal");
                                    if($("#"+id).attr("accountid")!="0") {
                                        disableFunc("btnSubTotal");
                                    }
                                    if(_subMarkVal>=2) {
                                        enableFunc("btnTransfer");
                                        disableFunc("btnSubTotal");
                                    }
                                }
                                else {
                                    alert(data);
                                }
                            },
                            error: function() {
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

            $("#btnSave").click(function() {
                if($("#tdSubTotal").text()!="0") {
                    alert("SubTotal must be \"0\"!");
                    return false;
                }
                var _entityId;
                $("#tbBeforeTransfer tr").each(function() {
                    if(this.id!="trBeforeTransferTitle" && this.id!="trBeforeTableName" && this.id!="" && this.id!="tbBeforeTransferHr") {
                        var _id = $(this).attr("entityid");
                        var _entityAry = $(this).children();
                        var _entity = new entityObj();
                        _entity.EntityId = _id;
                        _entity.Currency = $(_entityAry[1]).text();
                        _entity.ER = $(_entityAry[2]).text();
                        _entity.BeforeBaseAmount = $(_entityAry[3]).text();
                        _entity.BeforeSGDAmount = $(_entityAry[4]).text();
                        if(this.id=="trBeforeFirst"+_id) {
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
                        entityId:_entityId,
                        type:"Save"
                    },
                    success: function(data) {
                        var _dataAry = data.split(',');
                        enableFunc("btnConfirm");
                        disableFunc("btnSave");
                        alert(_dataAry[0]);
                        $("#hfRecordId").val(_dataAry[1]);
                    },
                    error: function() {
                        alert("Fail!");
                    }
                });
                AMCollection = new Entity();
            });
            $("#btnConfirm").click(function() {
                recordFunc();
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "AmountTransferAjax.aspx",
                    data: {
                        json: JSON.stringify(AMCollection),
                        recordId:$("#hfRecordId").val(),
                        type:"Confirm"
                    },
                    success: function(data) {
                        if(data!="Fail!") {
                            disableFunc("btnConfirm");
                            disableFunc("btnSave");
                            $("#hfConfirmVal").val("1");
                        }
                        alert(data);
                    },
                    error: function() {
                        alert("Fail!");
                    }
                });
                AMCollection = new Entity();
            
            });
        });

        function clearTb() {
            $("tr").each(function() {
                if ($(this).attr("entityid"))
                    $(this).remove();
            });
        }


        function recordFunc() {
            AMCollection.record = new Array();
            $("#tbAftertransfer tr").each(function() {
                if(this.id!="tbAftertransferTitle" && this.id!="") {
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
        
        function entityObj(_entityId,_entityType,_currency, _er,_beforeBaseAmount,_beforeSGDAmount,_transferBaseAmount,_transferSGDAmount,_PnL,_resultBaseAmount,_resultSGDAmount) {
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

        function recordObj(_entityId,_currency,_er,_baseAmount,_SGDAmount) {
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
                $("tr[entityid='" + _deleteId+"']").remove();
                var _hasEntity = 0;
                $("#tbBeforeTransfer tr").each(function() {
                    if(this.id.indexOf("Title")==-1 && $(this).attr("entityid"))
                        _hasEntity ++;
                });
                if(_hasEntity<=1) {
                    
                    $("#btnSubTotal").removeAttr("disabled");
                    $("#btnSubTotal").attr("class", "btn");
                    $("#subTotal").text('0');
                    $("#tdTransferSGD").text('0');
                    $("#tdTransferPnL").text('0');
                    $("#tdTotal").text('0');
                    
                    disableFunc("btnSave");
                    disableFunc("btnTransfer");
                    AMCollection = new Entity();
                    _subMarkVal = _hasEntity;
                    if(_hasEntity==0) {
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
            $("#" + _id).attr("class", "btn");
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
        .ui-icon-closethick:hover {
            background-image: url(css/images/ui-icons_cd0a0a_256x240.png);
            cursor: pointer;
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
                text-align: center;
                border: 0px;
                color: black;
                font-weight: bold;
                font-size: 22px;
            }
            .tableTransfer .tableName{
                
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
            border: 0px;
        }

        input {
            width: 80px;
        }
        #tbBeforeTransfer td,#tbTransfer td,#tbResult td {
            height: 26px;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <br><br><center>
        <div style="width: 1000px; border: none; text-align: left" class="a03_border"><br><br>
            <table class="tableAT">
                <tr>
                    <td style="text-align: center">
                        <input id="btnAdd" type="button" value="Add" class="btn" />
                    </td>
                    <td style="text-align:center;border: 0px"><span id="pageTitle" style="font-weight:bold;font-size:24px;color:black;" >Amount Transfer Calculation</span></td>
                </tr>
                <tr>
                    <td style="vertical-align: top;">
                        <div id="showEntitiesBar">
                            <script type="text/javascript">
                                $(function () {

                                });
                            </script>

                        </div>
                    </td>
                    <td style="vertical-align:top">

                        <table>
                            <tr>
                                <td style="border: 0px">
                                <input id="btnSubTotal" type="button" value="SubTotal" disabled="disabled" class="disablebtn" />&nbsp;&nbsp;&nbsp;
                                <input id="btnTransfer" type="button" value="Transfer" disabled="disabled" class="disablebtn" aria-grabbed="undefined" />&nbsp;&nbsp; 
                                <input id="btnSave" type="button" value="Save" disabled="disabled" class="disablebtn" />&nbsp;&nbsp;&nbsp;
                                <input id="btnConfirm" type="button" value="Confirm" disabled="disabled" class="disablebtn" />
                                <input type="hidden" id="hfConfirmVal" />
                                <input type="hidden" id="hfRecordId" />
                                </td>
                                <td style="border: 0px">&nbsp;&nbsp;&nbsp;</td>
                                <td style="border: 0px">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="3" style="border: 0">
                                    <!------------------------------------- Table Content --------------------------------------!-->
                                    <table>
                                        <tr style="vertical-align: top">
                                            <!------------------------------------- Before Transfer --------------------------------------!-->
                                            <td style="border: 0">
                                                <div id="BeforeTransfer">
                                                    <table id="tbBeforeTransfer" class="tableTransfer">
                                                        <tr id='trBeforeTableName'>
                                                            <td colspan="5" class="tableName">BeforeTransfer</td>
                                                        </tr>
                                                        <tr id="trBeforeTransferTitle">
                                                            <td>Entity</td>
                                                            <td>Currency</td>
                                                            <td>Rate</td>
                                                            <td>Base</td>
                                                            <td>SGD</td>
                                                            <td></td>
                                                        </tr>
                                                        <tr id="tbBeforeTransferHr">
                                                            <td  style="border: 0px" colspan="6"><hr/></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="font-weight: bold;color: black;border: 0px">Total</td>
                                                            <td colspan="3" style="border: 0px;font-weight: bold;color: black;"></td>
                                                            <td id="tdTotal" style="border: 0px;font-weight: bold;color: black;text-align:right"></td>
                                                            <td style="border: 0px"></td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                            <td style="border: 0px">&nbsp;&nbsp;</td>
                                            <!------------------------------------- Transfer --------------------------------------!-->
                                            <td style="border: 0">
                                                <div id="Transfer">
                                                    <table id="tbTransfer" class="">
                                                        <tr id='trTransferTableName'>
                                                            <td class="tableName" colspan="3">Transfer</td>
                                                        </tr>
                                                        <tr id="trTransferTitle">
                                                            <td>Base</td>
                                                            <td>SGD</td>
                                                            <td>Profit & Loss</td>
                                                        </tr>
                                                        <tr id="tbTransferHr"><td  colspan="3" style="border:0px"><hr/></td></tr>
                                                        <tr>
                                                            <td style="border: 0px"></td>
                                                            <td id="tdTransferSGD" style="text-align: right;font-weight: bold;color: black;border: 0px">0.00</td>
                                                            <td id="tdTransferPnL" style="text-align: right;font-weight: bold;color: black;border: 0px">0.00</td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </td>
                                            <td style="border: 0px">&nbsp;</td>
                                            <!------------------------------------- Result --------------------------------------!-->
                                            <td style="border: 0">
                                                <div id="Result">
                                                    <table id="tbResult" class="tableTransfer">
                                                        <tr id='trResultTableName'>
                                                            <td class="tableName" colspan="2">Result</td>
                                                        </tr>
                                                        <tr id="trResultTitle">
                                                            <td>Base</td>
                                                            <td>SGD</td>
                                                        </tr>
                                                        <tr id="tbResultHr"><td  colspan="2" style="border:0px"><hr/></td></tr>
                                                        <tr>
                                                            <td style="border: 0px"></td>
                                                            <td id="tdResultSGD" style="text-align: right;font-weight: bold;color: black;border: 0px">0.00</td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border: 0px">
                                                <br />
                                                <br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5" style="border: 0px">
                                                <div id="Aftertransfer">
                                                    <table id="tbAftertransfer" style="width: 100%">
                                                        <tr id="tbAftertransferTitle">
                                                            <td>Type</td>
                                                            <td>Entity</td>
                                                            <td>Currency</td>
                                                            <td>Rate</td>
                                                            <td>Base</td>
                                                            <td>SGD</td>
                                                        </tr>
                                                        <tr >
                                                            <td style="border: 0px" colspan="6"><hr/></td>
                                                        </tr>
                                                        <tr>
                                                            <td  colspan="4" style="border: 0px"></td>
                                                            <td style="font-weight: bold;color: black;border: 0px">Sub Total</td>
                                                            <td id="tdSubTotal" style="font-weight: bold;color: black;border: 0px;text-align: right" >0</td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>

                                    </table>


                                    <!------------------------------------- Table Content End--------------------------------------!-->

                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
            </table>
        <br><br></div>
        <div id="divErrorMsg"></div>
    </form>
</body>
</html>