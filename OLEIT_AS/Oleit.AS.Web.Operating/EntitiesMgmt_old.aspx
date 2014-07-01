<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EntitiesMgmt_old.aspx.cs" Inherits="Accounting_System.EntityMgmt" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
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
        $(function() {
            //var test=PageMethods.SetName();
            $("#showEntitiesBar")
                .jstree({
                    "json_data": {
                        "data": <%=JsonEntityTreeString%>
                    },
                    "plugins": [
                        "themes", "json_data", "ui", "crrm"
                    ],
                    "themes": { "themes": "default-rtl", "dots": false, "icons": false }
                }).bind("select_node.jstree", function() {
                    var clicked = $.jstree._focused().get_selected();
                    var cls = clicked[0].className;
                    var id = clicked[0].id;
                    var name = clicked[0].attributes.entityName.value;
                    EntityClick(cls, id, name);
                });

            $("#divRelation")
                .jstree({
                    "json_data": {
                        "data": <%=JsonRelationTreeString%>
                    },
                    "plugins": [
                        "themes", "json_data", "ui", "crrm"
                    ],
                    "themes": { "themes": "default-rtl", "dots": false, "icons": false }
                }).bind("select_node.jstree", function() {
                    if ($("#txtRelationValue").val() != "") {
                        //var clicked = $.jstree._focused().get_selected();
                        //var cls = clicked[0].className;
                        //var id = clicked[0].id;
                        var relation = $("#ddlRelation :selected").text();
                        var relationValue = $("#ddlRelation").val();
                        var to = $("#divRelation .jstree-clicked").text().substring(1, $("#divRelation .jstree-clicked").text().length);
                        var value = $("#txtRelationValue").val();
                        var toId = $("#divRelation .jstree-clicked").parent().attr("id");
                        addRelation(relation, relationValue, to, toId, value);
                    } else {
                        alert("Please enter a \"Value\".");
                    }
                });
            $("#btnAddRelation").click(function() {
                if ($("#tmpRelation").text().replace(" ", "") != "" && $("#tmpTarget").text().replace(" ", "") != "" && $("#tmpValue").text().replace(" ", "") != "") {
                    $.ajax({
                        type: "POST",
                        url: "RelationAjax.aspx",
                        data: {
                            RelationValue: $("#tmpRelation").attr("relationValue"),
                            EntityId: $("#showEntitiesBar .jstree-clicked").parent().attr("id"),
                            Target: $("#tmpTarget").text(),
                            TargetId: $("#tmpTarget").attr("entityId"),
                            Value: $("#tmpValue").text(),
                            EntityClass:$("#showEntitiesBar .jstree-clicked").parent().attr("class"),
                            Type:"add"
                        },
                        success: function(data) {
                            if (data == "Success!") {
                                /*
                                var newRelation = $("<tr><td>" + $("#tmpRelation").text() + "</td><td>" + $("#tmpTarget").text() + "</td><td>" + $("#tmpValue").text() + "</td></tr>");
                                var _cls = $("#showEntitiesBar .jstree-clicked").parent().attr("class");
                                if(_cls.indexOf("MainEntity")==-1)
                                    $("#tbRelation").append(newRelation);
                                else {
                                    $("#tbMainEntityRelation").append(newRelation);
                                }*/
                                $("#divRelationDialog").dialog("close");
                                alert(data);
                                location.reload();
                            }
                            else
                            {
                                alert(data);
                                if(data=="Session has expired.")
                                    window.parent.location.reload();
                            }
                        },
                        error: function(data) {
                            if (data != null)
                                alert(data);
                            else {
                                alert("Faled!");
                            }
                        }
                    });

                } else {
                    alert("Please add a relation first.");
                }

            });

            

            $("#divRelationDialog").dialog({
                autoOpen: false,
                title: "Entity Relation",
                width: '460px'
            });

            $("#btnNodeRelation ,#btnSummeryRelation").click(function() {
                var _id = $(this).attr("id");
                if (_id =="btnSummeryRelation") {
                    $("#ddlRelation").children().each(function (){
                        if($(this).val()!="5")
                            $(this).css("display","none");
                    });
                    $("#ddlRelation").val("5");
                } else {
                    $("#ddlRelation").val("1");
                    $("#ddlRelation").children().each(function (){
                        $(this).removeAttr("style");
                    });
                }
                $("#divRelationDialog").dialog({
                    autoOpen: true
                });
            });

            $("#btnLastEntity").click(function() {
                if (SaveCheck(this.id)) {

                }
            });

            $("#btnNewCashMainEntity").click(function() {
                if (SaveCheck(this.id) && checkLimit($("#txtNewCreditLimit").val())) {
                    return true;
                } else {
                    return false;
                }
            });

            $("#btnNewMainEntity").click(function() {
                if (SaveCheck(this.id)) {
                    //$("#showEntitiesBar").jstree("create", $("#showEntitiesBar"), "last", { "attr": { "id": $("#txtMainEntityName").val().replace(" ", "") + "_" + $("#SelectRole").find(":selected").text(), "href": "#", "class": "MainEntity" }, "data": $("#txtMainEntityName").val() + "(" + $("#SelectRole").find(":selected").text() + ")" }, false, true);
                    return true;
                } else {
                    return false;
                }
            });

            $("#btnNodeEntity").click(function() {
                if ((SaveCheck(this.id)&&$("#cbAccount").prop("checked")&&checkLimit($("#txtAccBetLimit").val()))||(SaveCheck(this.id)&&!$("#cbAccount").prop("checked"))) {
                    $("#showEntitiesBar .jstree-clicked").text($("#txtNodeEntityName").val());
                    return true;   
                }
                else
                {
                    return false;
                }
            });

            $("#btnMainEntity").click(function (){
                if (SaveCheck(this.id)) {


                    return true;
                }
                else{
                    return false;
                }
            });

            $("#btnNewEntity").click(function() {
                if (SaveCheck(this.id)) {

                    return true;
                }
                else{
                    return false;
                }
            });

            $("#btnNewNodeEntity").click(function() {
                var limit = true;
                if ($("#cbNewAccount").prop("checked"))
                    limit = checkLimit($("#txtNewBettingLimit").val());
                if (SaveCheck(this.id) && limit) {
                    var Id = $("#showEntitiesBar .jstree-clicked").parent().attr("id");
                    var entityType;
                    if ($("#cbNewAccount").prop("checked") == true)
                        entityType = "Account";
                    else {
                        $("input[name='rbNewNodeEntityLast']").each(function() {
                            if ($("#rbNewNodeEntityLastLVY").prop("checked") == true)
                                entityType = "LastNodeEntity";
                            else {
                                entityType = "NodeEntity";
                            }
                        });
                    }
                    //$("#showEntitiesBar").jstree("create", $("#" + Id), "last", { "attr": { "id": $("#txtNewNodeEntityName").val().replace(" ", ""), "href": "#", "class": entityType }, "data": $("#txtNewNodeEntityName").val() }, false, true);
                    return true;
                } else {
                    return false;
                }
            });


            $("#btnCurrencyER").click(function() {
                if ($("#txtCurrencyER").val() == "") {
                    alert("Please enter a Exchange Rate!");
                    $("#txtCurrencyER").focus();
                    return false;
                } else {
                    var id = $("#showEntitiesBar .jstree-clicked").parent().attr("id");
                    var Currency = $("#ddlCurrency").val();
                    var ER = $("#txtCurrencyER").val();
                    $("#" + id).attr("Currency", Currency).attr("ER", ER);
                    return true;
                }
            });
            $("#btnCashMainEntity").click(function() {
                if (SaveCheck(this.id)) {
                    //$("#showEntitiesBar").jstree("create", $("#showEntitiesBar"), "last", { "attr": { "id": $("#txtCashMainEntityName").val().replace(" ", "") + "_" + $("#SelectRole").find(":selected").text(), "href": "#", "class": "MainEntity" }, "data": $("#txtCashMainEntityName").val() + "(" + $("#SelectRole").find(":selected").text() + ")" }, false, true);
                    return true;
                } else {
                    return false;
                }
            });                
            

            $('#SelectRole').change(function() {
                if ($(this).val() == 'Cash') {
                    $("#divNewCashMainEntity").show();
                    $('#divNewMainEntity').hide();
                } else {
                    $('#divNewMainEntity').show();
                    $("#divNewCashMainEntity").hide();
                }
            });

            //edit last level radio button event
            $("input[name='rbNodeEntityLast']").each(function() {
                $(this).click(function() {
                    if (this.id == "rbNodeEntityLastLVY") {
                        //$("#tbAccount").hide();
                        $("#cbAccount").removeAttr("checked");
                        $("#cbAccount").removeAttr("disabled");
                    } else {
                        $("#cbAccount").attr("disabled", "disabled");
                        $("#tbAccount").hide();
                        $("#cbAccount").removeAttr("checked");
                    }
                });
            });

            //edit account checkbox check event
            $("#cbAccount").click(function() {
                if ($(this).prop("checked") == true)
                    $("#tbAccount").show();
                else {
                    $("#tbAccount").hide();
                }
            });

            //new last level radio button event
            $("input[name='rbNewNodeEntityLast']").each(function() {
                $(this).click(function() {
                    if (this.id == "rbNewNodeEntityLastLVY") {
                        //$("#tbAccount").hide();
                        $("#cbNewAccount").removeAttr("checked");
                        $("#cbNewAccount").removeAttr("disabled");
                    } else {
                        $("#cbNewAccount").attr("disabled", "disabled");
                        $("#tbNewAccount").hide();
                        $("#cbNewAccount").removeAttr("checked");
                    }
                });
            });

            //add account checkbox check event
            $("#cbNewAccount").click(function() {
                if ($(this).prop("checked") == true)
                    $("#tbNewAccount").show();
                else {
                    $("#tbNewAccount").hide();
                }
            });
            $("#txtCashMainEntityCurrency, #txtCashMainEntityER,#txtEntityCurrency,#txtEntityER,#txtMainEntityCurrency,#txtMainEntityER").prop('readonly', true);

            $("#btnRemoveEntity").click(function() {
                var removeName = $("#showEntitiesBar .jstree-clicked").text();
                var clickedId=$("#showEntitiesBar .jstree-clicked").parent().attr("id");
                if (clickedId) {
                    if (confirm("Do you really want to delete \"" + removeName + "\" Entity?")) {
                        $(".Entity").hide();
                        $("#" + id).show();
                        RemoveEntity(clickedId);
                        $("#showEntitiesBar").jstree("remove", "#" + clickedId);
                        //$("#requiredHint").hide();
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    alert("You don't choose any Entity to delete.");
                    return false;
                }
            });

            $("#btnBadDebt").click(function() {
                var _badDebtName = $("#showEntitiesBar .jstree-clicked").text();
                if(confirm("Do you really want to change \""+_badDebtName+"\" to BadDebt Entity?")) 
                    return true;
                 else 
                    return false;
            });
        });

        function deleteRelation(obj) {
            var _relationName = $($(obj).parent().parent().children()[0]).text();
            var _entityId = $(obj).parent().parent().attr("id").substring(3);
            var _targetEntityId = $(obj).parent().parent().attr("targetEntityId");
            var _entityType = $(obj).parent().parent().attr("rtype");
            if(confirm("Do you really want to delete this \""+_relationName+"\" Relation?")) {
                $.ajax({
                    type: "POST",
                    url: "RelationAjax.aspx",
                    data: {
                        EntityType: _entityType,
                        EntityId: _entityId,
                        TargetId:_targetEntityId,
                        Type:"remove"
                    },
                    success: function (data) {
                        alert(data);
                        if (data == "Success!") {
                            $(obj).fadeOut(function() {
                                $(this).parent().parent().remove();
                            });
                        }
                        else{                            
                            if(data=="Session has expired.")
                                window.parent.location.reload();
                        }
                    },
                    error:function () {
                        alert("Fail!");
                    }
                });
            }
        };

        function checkLimit(value) {
            if(parseFloat(value)>9999999999999.99 || parseFloat(value)<-9999999999999.99 || parseFloat(value).toString()=="NaN"){
                alert("Betting Limit can't be more than \"9999999999999.99\" or less than \"-9999999999999.99\"");
                return false;
            } else {
                return true;
            }
        }

            function refreshTree() {
                $("#showEntitiesBar").jstree("refresh");
            }

            function addRelation(relation,relationValue,to,toId,value) {
                if($("#"+toId).attr("sumtype")=="1") {
                    $("#tmpRelation").text(relation.replace(" ", ""));
                    $("#tmpTarget").text(to.replace(" ", ""));
                    $("#tmpValue").text(value);
                    $("#tmpTarget").attr("entityId", toId);
                    $("#tmpRelation").attr("relationValue", relationValue);
                } else {
                    alert("You must choose \"Transaction\" type! ");
  
                }
            }

            //save check
            function SaveCheck(btnId) {
                var id ="txt"+ btnId.substring(btnId.indexOf("btn")+3, btnId.length)+"Name";
                if ($("#" + id).val() == "") {
                    alert("Please enter an Entity Name!");
                    $("#" + id).focus();
                    return false;
                } else {
                    return true;
                }
            }

            var checkMark;
            var pageId;
            function changeEntities(id) {
                checkMark = true;
                pageId = id;
            }

        
            //entities click event
            function EntityClick(cls,id,name) {
                $("#btnTest").click();
                btnWork($("#" + id)[0].attributes.sumtype.value);
                clearEntityShow();
                clearAllInput();
                $("#hfChoseEntity").val(id);
                var _entityType = $("#" + id)[0].attributes.entitytype.value;
                if (cls.indexOf("Account") != -1) { //account
                    //$("#hfNewNodeEntityParentType").val($("#"+clickedId)[0].attributes.entitytype.value);
                    $("#ddlAccCompany").val($("#" + id)[0].attributes.company.value);
                    $("#txtAccName").val($("#" + id)[0].attributes.accountname.value);
                    $("#txtAccPwd").val($("#" + id)[0].attributes.password.value);
                    $("#ddlAccType").val($("#" + id)[0].attributes.accounttype.value);
                    $("#txtAccBetLimit").val($("#" + id)[0].attributes.bettinglimit.value);
                    $("#ddlAccStatus").val($("#" + id)[0].attributes.status.value);
                    
                    $("#txtDateOpen").val($("#" + id)[0].attributes.dateopen.value);
                    $("#txtPersonnel").val($("#" + id)[0].attributes.Personnel.value);
                    $("#txtIP").val($("#" + id)[0].attributes.ip.value);
                    $("#txtOdds").val($("#" + id)[0].attributes.odds.value);
                    $("#txtIssuesConditions").val($("#" + id)[0].attributes.IssuesConditions.value);
                    $("#txtRemarks").val($("#" + id)[0].attributes.remarks.value);
                    $("#txtFactor").val($("#" + id)[0].attributes.factor.value);
                    $("#txtPerBet").val($("#" + id)[0].attributes.perBet.value);
                    
                    $("#hfNodeEntityType").val($("#" + id)[0].attributes.entitytype.value);
                    $("#hfNodeEntityParentId").val($("#" + id)[0].attributes.parentid.value);
                    $("#hfNodeEntityAccountID").val($("#" + id)[0].attributes.accountid.value);
                    $("#hfNodeEntityId").val(id);
                    $("#trSumType").hide();
                    $("#rbNodeEntityLastLVY").prop("checked", true);
                    $("#cbAccount").prop("disabled",true).prop("checked", true);
                    $("#tbAccount").show();
                    entitiesInitial(name,"node",_entityType);
                } else if (cls.indexOf("LastNodeEntity") != -1) { //last entity
                    
                    $("#ddlAccCompany").val('');
                    $("#txtAccName").val('');
                    $("#txtAccPwd").val('');
                    $("#ddlAccType").val('');
                    $("#txtAccBetLimit").val('');
                    $("#ddlAccStatus").val('');
                    
                    $("#hfNodeEntityType").val($("#" + id)[0].attributes.entitytype.value);
                    $("#hfNodeEntityParentId").val($("#" + id)[0].attributes.parentid.value);
                    $("#hfNodeEntityId").val(id);
                    $("#rbNodeEntityLastLVY").prop("checked", true);
                    $("#trSumType").hide();
                    $("#cbAccount").removeAttr("checked").removeAttr("disabled");
                    $("#tbAccount").hide();
                    entitiesInitial(name, "node",_entityType);
                } else if (cls.indexOf("NodeEntity") != -1) {// entity
                    $("#rbNodeEntityLastLVN").prop("checked", "checked");
                    $("#trSumType").show();
                    $("#ddlSumType").val($("#" + id)[0].attributes.sumtype.value);
                    
                    $("#ddlAccCompany").val('');
                    $("#txtAccName").val('');
                    $("#txtAccPwd").val('');
                    $("#ddlAccType").val('');
                    $("#txtAccBetLimit").val('');
                    $("#ddlAccStatus").val('');

                    $("#hfNodeEntityType").val($("#" + id)[0].attributes.entitytype.value);
                    $("#hfNodeEntityParentId").val($("#" + id)[0].attributes.parentid.value);
                    $("#hfNodeEntityId").val(id);
                    $("#cbAccount").removeAttr("checked").attr("disabled", "disabled");
                    $("#tbAccount").hide();
                    entitiesInitial(name, "node",_entityType);
                }
                else if(cls.indexOf("CashMainEntity") != -1) {
                    $("#trCashMainEntityER").show();
                    $("#txtCashMainEntityName").val(name);
                    $("#divCashMainEntity").show();
                    $("#hfCashEntityId").val(id);//sTest
                    $("#txtTest").val($("#" + id)[0].attributes.currency.value);
                    $("#sTest").text($("#" + id)[0].attributes.currency.value);
                    $("#txtCashMainEntityCurrency").val($("#" + id)[0].attributes.currency.value);
                    $("#txtCashMainEntityER").val($("#" + id)[0].attributes.er.value);
                    $("#txtContactNumber").val($("#" + id)[0].attributes.contractnumber.value);
                    $("#txtTallyName").val($("#" + id)[0].attributes.tallyname.value);
                    $("#txtTallyNumber").val($("#" + id)[0].attributes.tallynumber.value);
                    $("#txtSettlementName").val($("#" + id)[0].attributes.settlementname.value);
                    $("#txtSettlementNumber").val($("#" + id)[0].attributes.settlementnumber.value);
                    $("#txtRecommendedby").val($("#" + id)[0].attributes.recommendedby.value);
                    $("#txtSkype").val($("#" + id)[0].attributes.skype.value);
                    $("#txtQQ").val($("#" + id)[0].attributes.qq.value);
                    $("#txtEmail").val($("#" + id)[0].attributes.email.value);
                    $("#txtCreditLimit").val($("#" + id)[0].attributes.creditlimit.value);
                    entitiesInitial(name, "Cash",_entityType);
                }
                else { //main entity
                    if(_entityType=="PnL"){
                        $("#PnLRelationButton").css("display","block");
                        $("#PnLRelation").css("display","block");
                    }
                    else {
                        $("#PnLRelationButton").css("display","none");
                        $("#PnLRelation").css("display","none");
                    }
                    $("#hfMainEntityId").val(id);
                    $("#hfMainEntityType").val(_entityType);
                    $("#trSumType").show();
                    $("#divMainEntity").show();
                    $("#trMainEntityER").show();
                    $("#txtMainEntityName").val(name);
                    entitiesInitial(name, "main",_entityType);
                }
            }

            function entitiesInitial(entityName, nodeType,_entityType) {
                var Id = $("#showEntitiesBar .jstree-clicked").parent().attr("id");
                if($("#" + Id)[0].attributes.sumtype.value>0)
                    $("#cbAccount").attr("disabled", "disabled");
                if (nodeType == "node") {
                    $("#txtEntityCurrency").val('');
                    $("#txtEntityER").val('');
                    $("#divNodeEntity").show();
                    $("#txtNodeEntityName").val(entityName);
                    $("#txtEntityCurrency").val($("#" + Id).attr("Currency"));
                    $("#txtEntityER").val($("#" + Id).attr("ER"));
                    $("#tbRelation").html("<table><tr><td style='text-align:center;'><img src='img/ajax-loader-dark.gif' /></td></tr></table>");
                    $.ajax({
                        type: "POST",
                        url: "RelationAjax.aspx",
                        data: {
                            Relation: $("#tmpRelation").text(),
                            RelationValue:$("#tmpRelation").attr("relationValue"),
                            EntityId: Id,
                            Target: $("#tmpTarget").text(),
                            TargetId:$("#tmpTarget").attr("entityId"),
                            Value: $("#tmpValue").text(),
                            RelationLoad:"true",
                            Type:"load"
                        },
                        success: function (data) {
                            if(data!="Session has expired.")
                            {
                                $("#tbRelation").html('');
                                $("#tbRelation").append(data);
                            }
                            else
                            {
                                alert(data);
                                window.parent.location.reload();
                            }
                        }
                    });
                }
                else if(nodeType=="Cash") {
                    $("#txtCashMainEntityCurrency").val($("#" + Id).attr("Currency"));
                    $("#txtCashMainEntityER").val($("#" + Id).attr("ER"));
                }
                else {
                    $("#txtMainEntityCurrency").val($("#" + Id).attr("Currency"));
                    $("#txtMainEntityER").val($("#" + Id).attr("ER"));
                    if(_entityType=="PnL") {
                        $.ajax({
                            type: "POST",
                            url: "RelationAjax.aspx",
                            data: {
                                Relation: $("#tmpRelation").text(),
                                RelationValue:$("#tmpRelation").attr("relationValue"),
                                EntityId: $("#showEntitiesBar .jstree-clicked").parent().attr("id"),
                                Target: $("#tmpTarget").text(),
                                TargetId:$("#tmpTarget").attr("entityId"),
                                Value: $("#tmpValue").text(),
                                RelationLoad:"true",
                                Type:"load"
                            },
                            success: function (data) {
                                if(data!="Session has expired.")
                                {
                                    $("#tbMainEntityRelation").html('');
                                    $("#tbMainEntityRelation").append(data);
                                }
                                else
                                {
                                    alert(data);                            
                                    window.parent.location.reload();
                                }
                            }
                        });  
                    }
                }
            }

            //clear all input
            function clearAllInput() {
                $("#txtNodeEntityName").val("");
                $("#txtNewNodeEntityName").val("");
            }

            //hide all entities
            function clearEntityShow() {
                $("#requiredHint").show();
                $(".Entity").hide();
            }


            function btnWork(sumtype) {
                $("#btnAddNewEntity").attr("class", "btn").removeAttr("disabled");
                $("#btnRemoveEntity").attr("class", "btn").removeAttr("disabled");
                if(sumtype!="0" && sumtype!="2")
                    $("#btnSetExchangeRate").attr("class", "btn").removeAttr("disabled");
                else {
                    $("#btnSetExchangeRate").attr("class", "disablebtn").attr("disabled","disabled");
                }
                $("#btnBadDebt").attr("class", "btn").removeAttr("disabled");
            }

            //
            function ShowHideDiv(id) {
                var clickedId= $("#showEntitiesBar .jstree-clicked").parent().attr("id");
                if (id != "") {
                    $(".Entity").hide();
                    $("#" + id).show();
                    $("#requiredHint").show();
                    clearAllInput();
                    $("#rbNodeEntityLastLVN,#rbNewNodeEntityLastLVN").prop("checked", "checked");
                    $("#cbAccount,#cbNewAccount").removeAttr("checked").attr("disabled", "disabled");
                    $("#tbAccount,#tbNewAccount").hide();
                    //$("#rbNodeEntityLastLVY").removeAttr("checked");
                    if (id == "divSetExchangeRate") {
                        $("#hfCurrencyEntityId").val(clickedId);
                        $("#ddlCurrency").val($("#"+clickedId).attr("currency"));
                        $("#txtCurrencyER").val($("#"+clickedId).attr("er"));
                        $("#EREntityName").text($("#showEntitiesBar .jstree-clicked").text());
                    }
                }
            
                if(id=='divNewNodeEntity') {
                    $("#hfNewNodeEntityParentId").val(clickedId);
                    $("#hfNewNodeEntityParentType").val($("#"+clickedId)[0].attributes.entitytype.value);
                    $("#txtNewNodeEntityName").val('');
                    $("#txtNewAccName").val('');
                    $("#txtNewBettingLimit").val('');
                }
            
                if (id == 'divNewMainEntity') {
                    $("#divSelectRole,#divNewMainEntity").show();
                    $("#txtNewMainEntityName").val("");
                    $("#SelectRole").children().each(function() {
                        if ($(this).text() == "P&L") {
                            $(this).attr("selected", true);
                        }
                    });
                }
            
            }

            //remove entities
            //function RemoveEntity(removeId) {
            //    $.ajax({
            //        type: "POST",
            //        async: false,
            //        url: "",
            //        data: {},
            //        success: function () {
            //            alert("Success!");
            //            $("#showEntitiesBar").jstree("remove", "#" + removeId);
            //        },
            //        error: function () {
                    
            //        }
            //    });
            //}
    </script>
    <style type="text/css">
        .ui-icon-closethick:hover {
            background-image: url(css/images/ui-icons_cd0a0a_256x240.png);
            cursor: pointer;
        }
        #requiredHint {
            display: none;
        }
        #divSelectRole table td{
            
        }
        .Entity {
            display: none;
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
        .erStyle {
            font-weight: bold;
            line-height: 40px; 
            color: black;
            background-color:#E0E0E0;
            border-style:None;
        }
        .relationTitle {
            font-weight: bold;
            color: black;
        }
        #PnLRelation,#PnLRelationButton {
            display: none;
        }
    </style>
</head>
<body>
    
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <table class="tableS">
            <tr>
                <td style="vertical-align: top">
                    <div id="showEntitiesBar" class="">
                        <script type="text/javascript">
                            $(function () {

                            });
                        </script>

                    </div>
                </td>
                <td style="border: 0; background-color: white; width: 5px"></td>
                <td style="vertical-align: top">
                    <table>
                        <tr>
                            <td style="border: 0px; vertical-align: central; height: 50px;">
                                <input class="btn" id="btnNewMain" type="button" value="New Main Entity" onclick="ShowHideDiv('divNewMainEntity');" />&nbsp;
                                <input class="disablebtn" id="btnAddNewEntity" disabled="disabled" type="button" value="New Entity" onclick="ShowHideDiv('divNewNodeEntity');" />&nbsp;
                                <asp:Button runat="server" ID="btnRemoveEntity" CssClass="disablebtn" Enabled="False" ClientIDMode="Static" Text="Remove Entity" OnClick="btnRemoveEntity_Click" />&nbsp;
                                <input class="disablebtn" id="btnSetExchangeRate" type="button" disabled="disabled" value="Set Exchange Rate" onclick="ShowHideDiv('divSetExchangeRate');" />&nbsp;
                                <asp:Button runat="server" ID="btnBadDebt" CssClass="disablebtn" Enabled="False" ClientIDMode="Static" Text="Save To BadDebt" OnClick="btnBadDebt_Click" />
                                <input type="hidden" runat="server" id="hfChoseEntity" />
                            </td>
                        </tr>
                    </table>

                    <div id="divSelectRole" class="Entity">
                        <table>
                            <tr>
                                <td>Role:</td>
                                <td>
                                    <select id="SelectRole" runat="server">
                                        <option selected="selected" value="PnL">P&L</option>
                                        <option value="Cash">Cash</option>
                                        <option value="EXP">EXP</option>
                                    </select>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="requiredHint"><span class="required">*Required Field</span></div>
                    <div id="divCashMainEntity" class="Entity">
                        <table style="border: 0px">
                            <tr>
                                <td><span class="required">*</span>Name:</td>
                                <td colspan="2">
                                    <input id="txtCashMainEntityName" type="text" runat="server" /></td>
                            </tr>
                            <tr id="trCashMainEntityER">
                                <td>Exchange Rate:</td>
                                <td>
                                    <input type="text" id="txtCashMainEntityCurrency" runat="server" class="erStyle" /></td>
                                <td>
                                    <input type="text" id="txtCashMainEntityER" runat="server" class="erStyle" /></td>
                            </tr>

                            <tr>
                                <td>Contact Number:</td>
                                <td colspan="2">
                                    <input id="txtContactNumber" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Tally Name:</td>
                                <td colspan="2">
                                    <input id="txtTallyName" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Tally Number:</td>
                                <td colspan="2">
                                    <input id="txtTallyNumber" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Settlement Name:</td>
                                <td colspan="2">
                                    <input id="txtSettlementName" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Settlement Number:</td>
                                <td colspan="2">
                                    <input id="txtSettlementNumber" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Recommended by:</td>
                                <td colspan="2">
                                    <input id="txtRecommendedby" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Skype:</td>
                                <td colspan="2">
                                    <input id="txtSkype" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>QQ:</td>
                                <td colspan="2">
                                    <input id="txtQQ" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Email:</td>
                                <td colspan="2">
                                    <input id="txtEmail" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><span class="required">*</span>Credit Limit:</td>
                                <td colspan="2">
                                    <input id="txtCreditLimit" maxlength="15" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td colspan="3" style="border: 0px; text-align: right">
                                    <input type="hidden" id="hfCashEntityId" runat="server" />
                                    <asp:Button runat="server" ClientIDMode="Static" CssClass="btn" Text="Save" ID="btnCashMainEntity" OnClick="btnCashMainEntity_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div id="divMainEntity" class="Entity">
                        <table>
                            <tr>
                                <td><span class="required">*</span>Name:</td>
                                <td colspan="2">
                                    <input id="txtMainEntityName" runat="server" type="text" /></td>
                            </tr>
                            <tr id="trMainEntityER">
                                <td>Exchange Rate:</td>
                                <td>
                                    <input type="text" id="txtMainEntityCurrency" runat="server" class="erStyle" /></td>
                                <td>
                                    <input type="text" id="txtMainEntityER" runat="server" class="erStyle" /></td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: right; border: 0px">
                                    <input type="hidden" id="hfMainEntityId" runat="server" />
                                    <input type="hidden" id="hfMainEntityType" runat="server" />
                                    <asp:Button runat="server" ClientIDMode="Static" CssClass="btn" ID="btnMainEntity" Text="Save" OnClick="btnMainEntity_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table id="tbMainEntityRelation" style="width: 100%">
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="border: 0px; text-align: right" colspan="3">
                                    <input id="btnSummeryRelation" class="btn" type="button" value="Set Summary Entity" />
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div id="divNewCashMainEntity" class="Entity">
                        <table>
                            <tr>
                                <td><span class="required">*</span>Name:</td>
                                <td colspan="2">
                                    <input id="txtNewCashMainEntityName" placeholder="Enter Entity name" runat="server" type="text" /></td>
                            </tr>

                            <tr>
                                <td>Contact Number:</td>
                                <td>
                                    <input id="txtNewContactNumber" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Tally Name:</td>
                                <td>
                                    <input id="txtNewTallyName" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Tally Number:</td>
                                <td>
                                    <input id="txtNewTallyNumber" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Settlement Name:</td>
                                <td>
                                    <input id="txtNewSettlementName" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Settlement Number:</td>
                                <td>
                                    <input id="txtNewSettlementNumber" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Recommended by:</td>
                                <td>
                                    <input id="txtNewRecommendedby" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Skype:</td>
                                <td>
                                    <input id="txtNewSkype" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>QQ:</td>
                                <td>
                                    <input id="txtNewQQ" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Email:</td>
                                <td>
                                    <input id="txtNewEmail" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><span class="required">*</span>Credit Limit:</td>
                                <td>
                                    <input id="txtNewCreditLimit" maxlength="15" type="text" runat="server" /></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: right; border: 0px">
                                    <asp:Button runat="server" ClientIDMode="Static" CssClass="btn" ID="btnNewCashMainEntity" Text="Save" OnClick="btnNewCashMainEntity_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div id="divNewMainEntity" class="Entity">
                        <table>
                            <tr>
                                <td><span class="required">*</span>Name:</td>
                                <td colspan="2">
                                    <input id="txtNewMainEntityName" placeholder="Enter Entity name" runat="server" type="text" /></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: right; border: 0px">
                                    <asp:Button runat="server" ClientIDMode="Static" CssClass="btn" ID="btnNewMainEntity" Text="Save" OnClick="btnNewMainEntity_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div id="divNewEntity" class="Entity">
                        <table>
                            <tr>
                                <td><span class="required">*</span>Name:</td>
                                <td>
                                    <input id="txtNewEntityName" type="text" /></td>
                                <td>
                                    <input id="rbNewEntityLastN" checked="checked" name="rbLast" type="radio" />Not last level<br />
                                    <input id="rbNewEntityLastY" name="rbLast" type="radio" />Last Level Entity<br />
                                    <input id="cbNewEntityAccount" type="checkbox" />Account
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: right; border: 0px">
                                    <input id="btnNewEntity" class="btn" type="button" value="Save" style="width: 100px" /></td>

                            </tr>
                        </table>
                    </div>

                    <div id="divSetExchangeRate" class="Entity">
                        <table>
                            <tr>
                                <td colspan="2">Entity Name: <span style="font-weight: bold; line-height: 40px; height: 40px; color: black;" id="EREntityName"></span></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <select id="ddlCurrency" style="width: 100px" runat="server">
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="required">*</span><input id="txtCurrencyER" type="text" runat="server" /></td>
                                <td>
                                    <input type="hidden" id="hfCurrencyEntityId" runat="server" />
                                    <asp:Button runat="server" ID="btnCurrencyER" OnClick="btnCurrencyER_Click" CssClass="btn" Text="OK" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divNodeEntity" class="Entity">
                        <table>
                            <tr>
                                <td><span class="required">*</span>Name:</td>
                                <td>
                                    <input id="txtNodeEntityName" type="text" runat="server" /></td>
                                <td>
                                    <input id="rbNodeEntityLastLVN" name="rbNodeEntityLast" type="radio" runat="server" disabled="True" />Not last level<br />
                                    <input id="rbNodeEntityLastLVY" name="rbNodeEntityLast" type="radio" runat="server" disabled="True" />Last Level Entity<br />
                                    <input id="cbAccount" disabled="True" type="checkbox" runat="server" />Account                                               
                                </td>
                            </tr>

                            <tr>
                                <td>Exchange Rate:</td>
                                <td>
                                    <input type="text" class="erStyle" id="txtEntityCurrency" runat="server" /></td>
                                <td>
                                    <input type="text" class="erStyle" id="txtEntityER" runat="server" /></td>
                            </tr>
                            <tr id="trSumType">
                                <td>Sum Type:</td>
                                <td colspan="2">
                                    <select id="ddlSumType" runat="server">
                                        <option value="0">Not</option>
                                        <option value="1">Subtotal</option>
                                        <option value="2">Transaction</option>
                                    </select>
                                </td>
                            </tr>

                            <tr id="tbAccount" style="display: none">
                                <td colspan="3">
                                    <table>
                                        <tr>
                                            <td colspan="3">Account Attributes</td>
                                        </tr>
                                        <tr>
                                            <td>Company:</td>
                                            <td colspan="2">
                                                <select id="ddlAccCompany" runat="server">
                                                    <option value="1">Company1</option>
                                                    <option value="2">Company2</option>
                                                    <option value="3">Company3</option>
                                                    <option value="4">Company4</option>
                                                    <option value="5">Company5</option>
                                                    <option value="6">Company6</option>
                                                </select></td>
                                        </tr>
                                        <tr>
                                            <td>Account Name:</td>
                                            <td colspan="2">
                                                <input id="txtAccName" type="text" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>Password:</td>
                                            <td colspan="2">
                                                <input id="txtAccPwd" type="password" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>Account Type:</td>
                                            <td colspan="2">
                                                <select id="ddlAccType" runat="server">
                                                    <option value="1">SuperSenior</option>
                                                    <option value="2">Senior</option>
                                                    <option value="3">Master</option>
                                                    <option value="4">Agent</option>
                                                    <option value="5">Members</option>
                                                </select></td>
                                        </tr>
                                        <tr>
                                            <td><span class="required">*</span>Betting Limit:</td>
                                            <td colspan="2">
                                                <input id="txtAccBetLimit" type="text" onkeydown="return checkNum(event);" runat="server" /></td>
                                        </tr>

                                        <tr>
                                            <td>DateOpen:</td>
                                            <td colspan="2">
                                                <input id="txtDateOpen" type="text" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>Personnel:</td>
                                            <td colspan="2">
                                                <input id="txtPersonnel" disabled="True" type="text" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>IP:</td>
                                            <td colspan="2">
                                                <input id="txtIP" type="text" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>Odds:</td>
                                            <td colspan="2">
                                                <input id="txtOdds" type="text" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>Issues/Conditions:</td>
                                            <td colspan="2">
                                                <input id="txtIssuesConditions" type="text" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>Remarks:</td>
                                            <td colspan="2">
                                                <input id="txtRemarks" type="text" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>Factor:</td>
                                            <td colspan="2">
                                                <input id="txtFactor" type="text" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>Per Bet:</td>
                                            <td colspan="2">
                                                <input id="txtPerBet" type="text" onkeydown="return checkNum(event);" runat="server" /></td>
                                        </tr>

                                        <tr>
                                            <td>Status:</td>
                                            <td colspan="2">
                                                <select id="ddlAccStatus" runat="server">
                                                    <option value="1">Open</option>
                                                    <option value="2">Suspended</option>
                                                    <option value="3">Closed</option>
                                                    <option value="4">NoFight</option>
                                                    <option value="5">FollowBet</option>
                                                    <option value="6">LousyOdds</option>
                                                    <option value="7">NeedToOpenBack</option>
                                                </select></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="border: 0px; text-align: right" colspan="3">
                                    <input id="hfNodeEntityId" type="hidden" runat="server" />
                                    <input id="hfNodeEntityType" type="hidden" runat="server" />
                                    <input type="hidden" id="hfNodeEntityParentId" runat="server" />
                                    <input type="hidden" id="hfNodeEntityAccountID" runat="server" />
                                    <asp:Button runat="server" ID="btnNodeEntity" ClientIDMode="Static" CssClass="btn" Text="Save" OnClick="btnNodeEntity_Click" />

                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table id="tbRelation" style="width: 100%">
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="border: 0px; text-align: right" colspan="3">
                                    <input id="btnNodeRelation" class="btn" type="button" value="Add Relation" />
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div id="divNewNodeEntity" class="Entity">
                        <table>
                            <tr>
                                <td><span class="required">*</span>Name:</td>
                                <td>
                                    <input id="txtNewNodeEntityName" placeholder="Enter Entity name" type="text" runat="server" /></td>
                                <td>
                                    <input id="rbNewNodeEntityLastLVN" name="rbNewNodeEntityLast" runat="server" type="radio" />Not last level<br />
                                    <input id="rbNewNodeEntityLastLVY" name="rbNewNodeEntityLast" runat="server" type="radio" />Last Level Entity<br />
                                    <input id="cbNewAccount" type="checkbox" runat="server" />Account                                               
                                </td>
                            </tr>


                            <tr id="tbNewAccount" style="display: none">
                                <td colspan="3">
                                    <table>
                                        <tr>
                                            <td colspan="3">Account Attributes</td>
                                        </tr>
                                        <tr>
                                            <td>Company:</td>
                                            <td colspan="2">
                                                <select id="ddlNewAccCompany" runat="server">
                                                    <option value="1">Company1</option>
                                                    <option value="2">Company2</option>
                                                    <option value="3">Company3</option>
                                                    <option value="4">Company4</option>
                                                    <option value="5">Company5</option>
                                                    <option value="6">Company6</option>
                                                </select></td>
                                        </tr>
                                        <tr>
                                            <td>Account Name:</td>
                                            <td colspan="2">
                                                <input id="txtNewAccName" type="text" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>Password:</td>
                                            <td colspan="2">
                                                <input id="txtNewAccPwd" type="password" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>Account Type:</td>
                                            <td colspan="2">
                                                <select id="ddlNewAccType" runat="server">
                                                    <option value="1">SuperSenior</option>
                                                    <option value="2">Senior</option>
                                                    <option value="3">Master</option>
                                                    <option value="4">Agent</option>
                                                    <option value="5">Members</option>
                                                </select></td>
                                        </tr>
                                        <tr>
                                            <td><span class="required">*</span>Betting Limit:</td>
                                            <td colspan="2">
                                                <input id="txtNewBettingLimit" type="text" maxlength="15" onkeydown="return checkNum(event);" runat="server" /></td>
                                        </tr>

                                        <tr>
                                            <td>DateOpen:</td>
                                            <td colspan="2">
                                                <input id="txtNewDateOpen" type="text" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>IP:</td>
                                            <td colspan="2">
                                                <input id="txtNewIP" type="text" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>Odds:</td>
                                            <td colspan="2">
                                                <input id="txtNewOdds" type="text" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>Issues/Conditions:</td>
                                            <td colspan="2">
                                                <input id="txtNewIssuesConditions" type="text" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>Remarks:</td>
                                            <td colspan="2">
                                                <input id="txtNewRemarks" type="text" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>Factor:</td>
                                            <td colspan="2">
                                                <input id="txtNewFactor" type="text" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>Per Bet:</td>
                                            <td colspan="2">
                                                <input id="txtNewPerBet" type="text" onkeydown="return checkNum(event);" runat="server" /></td>
                                        </tr>

                                        <tr>
                                            <td>Status:</td>
                                            <td colspan="2">
                                                <select id="ddlNewAccStatus" runat="server">
                                                    <option value="1">Opened</option>
                                                    <option value="2">Suspended</option>
                                                    <option value="3">Closed</option>
                                                    <option value="4">NoFight</option>
                                                    <option value="5">FollowBet</option>
                                                    <option value="6">LousyOdds</option>
                                                    <option value="7">NeedToOpenBack</option>
                                                </select></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>


                            <tr>
                                <td style="border: 0px; text-align: right" colspan="3">
                                    <input type="hidden" id="hfNewNodeEntityParentId" runat="server" />
                                    <input type="hidden" id="hfNewNodeEntityParentType" runat="server" />
                                    <asp:Button runat="server" ID="btnNewNodeEntity" ClientIDMode="Static" CssClass="btn" Text="Save" OnClick="btnNewNodeEntity_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
        <div id="divRelationDialog">
            <table class="tableS">
                <tr>
                    <td>Relation</td>
                    <td>
                        <select id="ddlRelation">
                            <option value="1">Allocate</option>
                            <option value="2">Position</option>
                            <option value="3">Commission</option>
                            <option value="4">Follow Bet</option>
                            <option value="5">PnL Sum</option>
                        </select>
                    </td>
                    <td><span class="required">*</span>Value</td>
                    <td>
                        <input type="text" style="width: 50px" id="txtRelationValue" /></td>
                    <td></td>

                </tr>

            </table>
            <br />
            <div id="divRelation"></div>

            <br />
            <table class="tableS">
                <tr>
                    <td>Relation</td>
                    <td>To</td>
                    <td>Value</td>
                </tr>
                <tr id="trRelation">
                    <td><span id="tmpRelation"></span></td>
                    <td><span id="tmpTarget" entityid=""></span></td>
                    <td><span id="tmpValue"></span></td>
                    <td>
                        <input id="btnAddRelation" type="button" value="Add" class="btn" /></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
