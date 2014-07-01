<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EntitiesMgmt.aspx.cs" Inherits="Accounting_System.EntityMgmt2" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Entities Management</title>
    <script src="js/jquery-1.9.1.min.js"></script>    
    <script src="js/commonFunc.js"></script>
    <script src="js/jquery-ui-1.9.2.custom.min.js"></script>
    <script src="js/jquery.jstree.js"></script>
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <link href="css/table.css" rel="stylesheet" />
    <link href="css/AccountingSystem.css" rel="stylesheet" />
    <link href="css/Css.css" rel="stylesheet" />
    <script src="js/Entities.js"></script>
    <link href="css/Entities.css" rel="stylesheet" />
    <script type="text/javascript">
        var Entity = new Object();
        var NewEntity = new Object();
        function Account(company, accountname, password, accounttype, bettinglimit, dateopen, personnel, ip, odds, issuesconditions, remarksacc, factor, perbet, status) {
            this.Company = company,
            this.AccountName = accountname,
            this.Password = password,
            this.AccountType = accounttype,
            this.BettingLimit = bettinglimit,
            this.DateOpen = dateopen,
            this.Personnel = personnel,
            this.IP = ip,
            this.Odds = odds,
            this.IssuesConditions = issuesconditions,
            this.RemarksAcc = remarksacc,
            this.Factor = factor,
            this.PerBet = perbet,
            this.Status = status
        }

        $(function () {
            var _mainEntityStr = '<%=MainEntityString%>';
        var _mainEntityAry = _mainEntityStr.split(',');
        $("#txtMainEntitySearch,#txtRelationMainEntitySearch").autocomplete({
            source: _mainEntityAry
        });
    });
            $(function () {
                //Main Entity left side tree list
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
                    var _id = clicked[0].id;
                    var _name = clicked[0].attributes.entityName.value;
                    entityClick(_id, _name);
                }).bind("loaded.jstree", function (event, data) {
                    if ($("#hfReload").val() != "") {
                        var _reloadAry = $("#hfReload").val().split(',');
                        entityClick(_reloadAry[0], _reloadAry[1]);
                        $("#" + _reloadAry[0] + " >a").attr("class", "jstree-clicked");
                        $("#hfReload").val('');
                    }
                });

            //relation tree
            $("#divRelation")
                .jstree({
                    "json_data": {
                        "data": eval(<%=JsonRelationTreeString%>)
                    },
                    "plugins": [
                        "themes", "json_data", "ui", "crrm"
                    ],
                    "themes": { "themes": "default-rtl", "dots": false, "icons": false }
                }).bind("select_node.jstree", function () {
                    if ($("#txtRelationValue").val() != "" || $("#ddlRelation").val() == "5") {
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
                        $("#tmpRelation").text("");
                        $("#tmpTarget").text("");
                        $("#tmpValue").text("");
                        $("#tmpTarget").attr("entityId", "");
                        $("#tmpRelation").attr("relationValue", "");
                        $("#divRelation a[class*='jstree-clicked']").removeClass("jstree-clicked");
                        alert("Please enter a \"Value\".");
                    }
                });

            //add button in relation tree
            $("#btnAddRelation").click(function () {
                var _entityID = $("#hfNodeEntityId").val();
                if (($("#tmpRelation").text().replace(" ", "") != "" && $("#tmpTarget").text().replace(" ", "") != "" && $("#tmpValue").text().replace(" ", "") != "") || $("#ddlRelation").val() == "5" && ($("#tmpRelation").text().replace(" ", "") != "" && $("#tmpTarget").text().replace(" ", "") != "") || $("#ddlRelation").val() == "3" && ($("#tmpRelation").text().replace(" ", "") != "" && $("#tmpTarget").text().replace(" ", "") != "")) {
                    $.ajax({
                        type: "POST",
                        url: "RelationAjax.aspx",
                        async: false,
                        data: {
                            RelationValue: $("#tmpRelation").attr("relationValue"),
                            EntityId: _entityID,
                            Target: $("#tmpTarget").text(),
                            TargetId: $("#tmpTarget").attr("entityId"),
                            Value: $("#tmpValue").text(),
                            EntityClass: $("#divRelation .jstree-clicked").parent().attr("class"),
                            Type: "add"
                        },
                        success: function (data) {
                            if (data == "Success!") {
                                $("#divRelationDialog").dialog("close");
                                alert(data);
                                $.ajax({
                                    type: "POST",
                                    url: "RelationAjax.aspx",
                                    async: false,
                                    data: {
                                        EntityId: _entityID,
                                        Type: "load"
                                    },
                                    success: function (data) {
                                        if (data != "Session has expired.") {
                                            if ($("#" + _entityID).attr("parentid") == "0") {
                                                $("#tbMainEntityRelation").html('');
                                                $("#tbMainEntityRelation").append(data);
                                            }
                                            else {
                                                $("#tbRelation").html('');
                                                $("#tbRelation").append(data);
                                            }
                                        }
                                        else {
                                            alert(data);
                                            window.parent.location.reload();
                                        }
                                    }
                                });
                            }
                            else {
                                alert(data);
                                if (data == "Session has expired.")
                                    window.parent.location.reload();
                            }
                        },
                        error: function (data) {
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

            //Change Rate button
            $("#btnSetExchangeRate").click(function () {
                var _entityID = $("#hfNodeEntityId").val();
                $("#fsRelation").hide();
                $("#ddlCurrency").val($("#" + _entityID).attr("currency"));
                $("#txtCurrencyER").val($("#" + _entityID).attr("er"));
            });

            //New Entity button
            $("#btnAddNewEntity").click(function () {
                if (Entity.ParentID == "0") {//Main
                    $("#ddlNewSumType").val("1").children().each(function () {
                        if ($(this).val() != "1")
                            $(this).hide();
                    });
                    $("#cbAccount,#cbNewAccount").removeAttr("checked").attr("disabled", "disabled");
                    $("#rbNewNodeEntityLastLVN").prop("checked", true);
                    $("#divNewNotLastLevel,#ddlNewCurrency").show();
                    $("#txtNewNodeEntityName,#tbAccount,#tbNewAccount").hide();
                    $("input[name='rbNewNodeEntityLast']").attr("disabled", "disabled");
                }
                else if (Entity.SumType == "1") {//Transaction
                    $("#ddlNewSumType").val("2").children().each(function () {
                        if ($(this).val() != "2")
                            $(this).hide();
                    });
                    $("#cbAccount,#cbNewAccount").removeAttr("checked").attr("disabled", "disabled");
                    $("#rbNewNodeEntityLastLVN").prop("checked", true);
                    $("#divNewNotLastLevel,#txtNewNodeEntityName").show();
                    $("#ddlNewCurrency,#tbAccount,#tbNewAccount").hide();
                    $("input[name='rbNewNodeEntityLast']").attr("disabled", "disabled");
                }
                else if (Entity.SumType == "5") {//Agent
                    $("#ddlNewSumType").val("0").children().each(function () {
                        if (parseInt($(this).val()) > parseInt(Entity.SumType) || $(this).val() == "0")
                            $(this).show();
                        else
                            $(this).hide();
                    });
                    if (!NewEntity.IsAccount)
                        $("#cbAccount,#cbNewAccount").removeAttr("checked");
                    $("#rbNewNodeEntityLastLVY").prop("checked", true);
                    $("#ddlNewCurrency,#divNewNotLastLevel").hide();
                    $("#cbNewAccount").removeAttr("disabled");
                    $("#txtNewNodeEntityName").show();
                    $("input[name='rbNewNodeEntityLast']").removeAttr("disabled");
                }
                else {//SubTotal, Super, Master, Account
                    $("#ddlNewSumType").val(parseInt(Entity.SumType) + 1).children().each(function () {
                        if (parseInt($(this).val()) > parseInt(Entity.SumType) || $(this).val() == "0")
                            $(this).show();
                        else
                            $(this).hide();
                    });
                    $("#cbAccount,#cbNewAccount").removeAttr("checked").attr("disabled", "disabled");
                    $("#rbNewNodeEntityLastLVN").prop("checked", true);
                    $("#divNewNotLastLevel,#txtNewNodeEntityName").show();
                    $("#ddlNewCurrency,#tbAccount,#tbNewAccount").hide();
                    $("input[name='rbNewNodeEntityLast']").removeAttr("disabled");
                }
                $("#fsRelation").hide();
            });

            //New Main Entity button
            $("#btnNewMain").click(function () {
                $("#divAccount,#fsRelation,#divSubEntities").hide();
                $("#SelectRole").val('0');
                $(".jstree-clicked").removeClass("jstree-clicked");
                $("#btnSetExchangeRate,#btnAddNewEntity,#btnRemoveEntity,#btnBadDebt").attr("class", "btn01").attr("disabled", "disabled");
            });

            //Relation dialog
            $("#divRelationDialog").dialog({
                autoOpen: false,
                title: "Entity Relation",
                width: '460px'
            });

            //Relation Function open
            $("#btnNodeRelation,#btnSummeryRelation").click(function () {
                var _btnId = $(this).attr("id");
                if (_btnId == "btnSummeryRelation") {
                    $("#ddlRelation").children().each(function () {
                        if ($(this).val() != "5")
                            $(this).css("display", "none");
                    });
                    $("#ddlRelation").val("5");
                    $("#tdRelationValTitle,#tdRelationVal,#tdTmpRelationVal,#tdTmpRelationValTitle").hide();

                } else {
                    $("#ddlRelation").children().each(function () {
                        if ($(this).val() == "5")
                            $(this).css("display", "none");
                    });
                    $("#ddlRelation").val("1");
                    $("#ddlRelation").children().each(function () {
                        $(this).removeAttr("style");
                    });
                    $("#tdRelationValTitle,#tdRelationVal,#tdTmpRelationVal,#tdTmpRelationValTitle").show();
                }
                $("#divRelationDialog").dialog({
                    autoOpen: true
                });
            });

            $("#btnLastEntity").click(function () {
                if (SaveCheck(this.id)) {

                }
            });

            //Add new Cash Main Entity
            $("#btnNewCashMainEntity").click(function () {
                if (SaveCheck(this.id) && checkLimit($("#txtNewCreditLimit").val())) {
                    return true;
                } else {
                    return false;
                }
            });

            //Add Main Entity besides the Cash Entity
            $("#btnNewMainEntity").click(function () {
                if (SaveCheck(this.id)) {
                    //$("#showEntitiesBar").jstree("create", $("#showEntitiesBar"), "last", { "attr": { "id": $("#txtMainEntityName").val().replace(" ", "") + "_" + $("#SelectRole").find(":selected").text(), "href": "#", "class": "MainEntity" }, "data": $("#txtMainEntityName").val() + "(" + $("#SelectRole").find(":selected").text() + ")" }, false, true);
                    return true;
                } else {
                    return false;
                }
            });

            //Save Node Entity
            $("#btnSaveNodeEntity").click(function () {
                if ((SaveCheck(this.id) && $("#cbAccount").prop("checked") && checkLimit($("#txtAccBetLimit").val())) || (SaveCheck(this.id) && !$("#cbAccount").prop("checked"))) {
                    Entity.EntityID = $("#hfNodeEntityId").val();
                    Entity.SumType = $("#ddlNewSumType").val();
                    Entity.ParentID = $("#hfNodeEntityParentId").val();
                    Entity.EntityName = $("#txtNodeEntityName").val();
                    Entity.EntityType = $("#hfNodeEntityType").val();
                    Entity.IsLastLevel = $("#rbNodeEntityLastLVY").prop("checked");
                    Entity.IsAccount = $("#cbAccount").prop("checked");
                    Entity.Currency = $("#txtEntityCurrency").val();
                    Entity.ER = $("#txtEntityER").val();
                    if (!$("#cbAccount").prop("checked")) {
                        Entity.Account = null;
                    }
                    else {
                        var _perbet;
                        if ($("#txtPerBet").val() == "")
                            _perbet = 0;
                        else
                            _perbet = $("#txtPerBet").val();
                        Entity.Account = new Account();
                        Entity.Account.Company = $("#ddlAccCompany").val();
                        Entity.Account.AccountName = $("#txtAccName").val();
                        Entity.Account.Password = $("#txtAccPwd").val();
                        Entity.Account.AccountType = $("#ddlAccType").val();
                        Entity.Account.BettingLimit = $("#txtBettingLimit").val();
                        Entity.Account.DateOpen = $("#txtDateOpen").val();
                        Entity.Account.IP = $("#txtIP").val();
                        Entity.Account.Odds = $("#txtOdds").val();
                        Entity.Account.IssuesConditions = $("#txtIssuesConditions").val();
                        Entity.Account.RemarksAcc = $("#txtRemarks").val();
                        Entity.Account.Factor = $("#txtFactor").val();
                        Entity.Account.PerBet = _perbet;
                        Entity.Account.Status = $("#ddlAccStatus").val();
                        Entity.Account.Personnel = $("#txtPersonnel").val();
                    }
                    $.ajax({
                        type: "POST",
                        url: "EntitiesAjax.aspx",
                        data: {
                            json: JSON.stringify(Entity),
                            type: "update"
                        },
                        success: function (data) {
                            if (data == "Success!") {
                                refreshTree();
                                //entityClick($("#hfMainEntityId").val(),$("#"+$("#hfMainEntityId").val()).attr("entityname"));
                                //loadEntities($("#hfMainEntityId").val());
                                //clickSubEntities(Entity.ParentID);
                            }
                            else {

                            }
                            //Entity = new Object();
                            alert(data);
                        },
                        error: function () {
                            alert("Fail!");
                        }
                    });
                }
                else {
                    return false;
                }
            });

            //Save Main Entity
            $("#btnMainEntity").click(function () {
                if (SaveCheck(this.id)) {


                    return true;
                }
                else {
                    return false;
                }
            });

            //Add a new Main Entity
            $("#btnNewEntity").click(function () {
                if (SaveCheck(this.id)) {

                    return true;
                }
                else {
                    return false;
                }
            });

            //Add a new node Entity
            $("#btnNewNodeEntity").click(function () {
                var _limitbool = true;
                if ($("#cbNewAccount").prop("checked"))
                    if (!checkLimit($("#txtNewBettingLimit").val()))
                        return false;
                if (SaveCheck(this.id)) {

                    NewEntity.ParentID = Entity.EntityID;
                    NewEntity.IsLastLevel = $("#rbNewNodeEntityLastLVY").prop("checked");
                    NewEntity.IsAccount = $("#cbNewAccount").prop("checked");
                    NewEntity.EntityType = Entity.EntityType;
                    NewEntity.SumType = $("#ddlNewSumType").val();
                    if (NewEntity.SumType == "1") {
                        NewEntity.EntityName = $("#ddlNewCurrency").val();
                        NewEntity.Currency = NewEntity.EntityName;
                    }
                    else {
                        NewEntity.EntityName = $("#txtNewNodeEntityName").val();
                        NewEntity.Currency = Entity.Currency;
                    }

                    if (!$("#cbNewAccount").prop("checked")) {
                        NewEntity.Account = null;
                    }
                    else {
                        var _perbet;
                        if ($("#txtPerBet").val() == "")
                            _perbet = 0;
                        NewEntity.Account = new Account();
                        NewEntity.Account.Company = $("#ddlNewAccCompany").val();
                        NewEntity.Account.AccountName = $("#txtNewAccName").val();
                        NewEntity.Account.Password = $("#txtNewAccPwd").val();
                        NewEntity.Account.AccountType = $("#ddlNewAccType").val();
                        NewEntity.Account.BettingLimit = $("#txtNewBettingLimit").val();
                        NewEntity.Account.DateOpen = $("#txtNewDateOpen").val();
                        NewEntity.Account.IP = $("#txtNewIP").val();
                        NewEntity.Account.Odds = $("#txtNewOdds").val();
                        NewEntity.Account.IssuesConditions = $("#txtNewIssuesConditions").val();
                        NewEntity.Account.RemarksAcc = $("#txtNewRemarks").val();
                        NewEntity.Account.Factor = $("#txtNewFactor").val();
                        NewEntity.Account.PerBet = _perbet;
                        NewEntity.Account.Status = $("#ddlNewAccStatus").val();
                        NewEntity.Account.Personnel = "";
                    }

                    $.ajax({
                        type: "POST",
                        url: "EntitiesAjax.aspx",
                        async: false,
                        data: {
                            json: JSON.stringify(NewEntity),
                            type: "insert"
                        },
                        success: function (data) {
                            var _oldEntity = Entity;
                            if (data == "Success!") {
                                refreshTree();
                                $('#btnAddNewEntity').click();

                                //alert(data);
                                //loadSubEntities(Entity.ParentID);
                            }
                            else if (data == "Session has expired.") {
                                alert(data);
                                window.parent.location.reload();
                            }
                            else
                                alert(data);
                            //Entity = new Object();
                            //alert(data);
                            Entity = _oldEntity;
                        }
                    });
                }
            });

            //Set Currency and ExchangeRate
            $("#btnCurrencyER").click(function () {
                if ($("#txtCurrencyER").val() == "") {
                    alert("Please enter a Exchange Rate!");
                    $("#txtCurrencyER").focus();
                    return false;
                } else {
                    var _currency = $("#ddlCurrency").val();
                    var _er = $("#txtCurrencyER").val();
                    $.ajax({
                        type: "POST",
                        url: "EntitiesAjax.aspx",
                        data: {
                            entityID: Entity.EntityID,
                            currency: _currency,
                            er: _er,
                            Type: "er"
                        },
                        success: function (data) {
                            if (data != "Faled!") {
                                refreshTree();
                            }
                            alert(data);
                        },
                        error: function () {
                            alert("Fail!");
                        }
                    });
                }
            });

            //Save Cash Entity
            $("#btnCashMainEntity").click(function () {
                if (SaveCheck(this.id)) {
                    //$("#showEntitiesBar").jstree("create", $("#showEntitiesBar"), "last", { "attr": { "id": $("#txtCashMainEntityName").val().replace(" ", "") + "_" + $("#SelectRole").find(":selected").text(), "href": "#", "class": "MainEntity" }, "data": $("#txtCashMainEntityName").val() + "(" + $("#SelectRole").find(":selected").text() + ")" }, false, true);
                    return true;
                } else {
                    return false;
                }
            });

            $("#ddlRelation").change(function () {
                if ($(this).val() != "3") {
                    $("#tdRelationValTitle,#tdRelationVal,#tdTmpRelationVal,#tdTmpRelationValTitle").show();
                }
                else {
                    $("#tdRelationValTitle,#tdRelationVal,#tdTmpRelationVal,#tdTmpRelationValTitle").hide();
                }

            });

            //Selete the Role
            $('#SelectRole').change(function () {
                if ($(this).val() == '2') {
                    $("#divNewCashMainEntity").show();
                    $('#divNewMainEntity').hide();
                } else {
                    $('#divNewMainEntity').show();
                    $("#divNewCashMainEntity").hide();
                }
            });

            //edit last level radio button event
            $("input[name='rbNodeEntityLast']").each(function () {
                $(this).click(function () {
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

            //Edit account checkbox check event
            $("#cbAccount").click(function () {
                if ($(this).prop("checked") == true)
                    $("#tbAccount").show();
                else {
                    $("#tbAccount").hide();
                }
            });

            //New last level radio button event
            $("input[name='rbNewNodeEntityLast']").each(function () {
                $(this).click(function () {
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

            //Add an account checkbox check event
            $("#cbNewAccount").click(function () {
                if ($(this).prop("checked") == true)
                    $("#tbNewAccount").show();
                else {
                    $("#tbNewAccount").hide();
                }
            });


            $("#txtCashMainEntityCurrency, #txtCashMainEntityER,#txtEntityCurrency,#txtEntityER,#txtMainEntityCurrency,#txtMainEntityER").prop('readonly', true);

            //Remove the Entity function
            $("#btnRemoveEntity").click(function () {
                var removeName = $("#hfNodeEntityName").val();
                var _entityMainID = $("#hfMainEntityId").val();
                var _entityName = $("#" + Entity.EntityID).attr("entityName");
                if (Entity.EntityID) {
                    if (confirm("Do you really want to delete \"" + removeName + "\" Entity?")) {
                        $(".Entity").hide();
                        $.ajax({
                            type: "POST",
                            url: "EntitiesAjax.aspx",
                            data: {
                                entityID: Entity.EntityID,
                                Type: "remove"
                            },
                            success: function (data) {
                                if (data != "Faled!") {
                                    if (Entity.ParentID != "0") {
                                        entityClick(Entity.EntityID, Entity.EntityName);
                                        refreshTree();
                                    }
                                    else
                                        window.location.reload();
                                }
                                alert(data);
                            },
                            error: function () {
                                alert("Fail!");
                            }
                        });
                    }
                } else {
                    alert("You don't choose any Entity to delete.");
                    return false;
                }
            });

            //Save the Entity as Entity
            $("#btnBadDebt").click(function () {
                var _badDebtName = $("#hfNodeEntityName").val();
                if (confirm("Do you really want to change \"" + _badDebtName + "\" to BadDebt Entity?")) {
                    $.ajax({
                        type: "POST",
                        url: "EntitiesAjax.aspx",
                        async: false,
                        data: {
                            entityID: Entity.EntityID,
                            Type: "baddebt"
                        },
                        success: function (data) {
                            alert(data);
                            if (data == "Success!") {
                                entityClick(Entity.EntityID, _badDebtName);
                                refreshTree();
                            }
                        },
                        error: function () {
                            alert("Fail!");
                        }
                    });

                }
                else {
                    return false;
                }
            });

            //Search the relation Entity name
            $("#btnRelationEntitySearch").click(function () {
                //
                $.ajax({
                    type: "POST",
                    url: "EntitiesAjax.aspx",
                    data: {
                        entityType: $("#ddlRelationEntityType").val(),
                        entityName: $("#txtRelationMainEntitySearch").val(),
                        type: "search"
                    },
                    success: function (data) {

                        if (data != "Faled!") {
                            $("#divRelation")
                                .jstree({
                                    "json_data": {
                                        "data": eval(data)
                                    },
                                    "plugins": [
                                        "themes", "json_data", "ui", "crrm"
                                    ],
                                    "themes": { "themes": "default-rtl", "dots": false, "icons": false }
                                }).bind("select_node.jstree", function () {
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
                                        $("#tmpRelation").text("");
                                        $("#tmpTarget").text("");
                                        $("#tmpValue").text("");
                                        $("#tmpTarget").attr("entityId", "");
                                        $("#tmpRelation").attr("relationValue", "");
                                        $("#divRelation a[class*='jstree-clicked']").removeClass("jstree-clicked");
                                        alert("Please enter a \"Value\".");
                                    }
                                });
                        }
                    },
                    error: function () {
                        alert("Fail!");
                    }
                });
            });

        });

            //Delete the Entity relation function
            function deleteRelation(obj) {
                var _relationName = $($(obj).parent().parent().children()[0]).text();
                var _entityId = $(obj).parent().parent().attr("id").substring(3);
                var _targetEntityId = $(obj).parent().parent().attr("targetEntityId");
                var _entityType = $(obj).parent().parent().attr("rtype");
                if (confirm("Do you really want to delete this \"" + _relationName + "\" Relation?")) {
                    $.ajax({
                        type: "POST",
                        url: "RelationAjax.aspx",
                        data: {
                            EntityType: _entityType,
                            EntityId: _entityId,
                            TargetId: _targetEntityId,
                            Type: "remove"
                        },
                        success: function (data) {
                            alert(data);
                            if (data == "Success!") {
                                $(obj).fadeOut(function () {
                                    $(this).parent().parent().remove();
                                });
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
            };

            //Check the limit funciton
            function checkLimit(value) {
                if (parseFloat(value) > 9999999999999.99 || parseFloat(value) < -9999999999999.99 || parseFloat(value).toString() == "NaN") {
                    alert("Betting Limit can't be more than \"9999999999999.99\" or less than \"-9999999999999.99\"");
                    $("#txtNewBettingLimit").focus();
                    return false;
                } else {
                    return true;
                }
            }

            //Refresh the SubEntities tree and the Relation tree
            function refreshTree() {
                $("#divSubEntities").jstree("refresh", $("#divSubEntities"));
                $("#divRelation").jstree("refresh", $("#divRelation"));
            }

            //Add a relation function
            function addRelation(relation, relationValue, to, toId, value) {
                if ($("#ddlRelation").val() == "5" && $("#" + toId).attr("sumtype") != "1") {
                    $("#tmpRelation").text(relation.replace(" ", ""));
                    $("#tmpTarget").text(to.replace(" ", ""));
                    $("#tmpValue").text(value);
                    $("#tmpTarget").attr("entityId", toId);
                    $("#tmpRelation").attr("relationValue", $("#ddlRelation").val());
                }
                else if ($("#ddlRelation").val() == "5" && $("#" + toId).attr("sumtype") == "1") {
                    $("#divRelation a[class*='jstree-clicked']").removeClass("jstree-clicked");
                    $("#tmpRelation").text("");
                    $("#tmpTarget").text("");
                    $("#tmpValue").text("");
                    $("#tmpTarget").attr("entityId", "");
                    $("#tmpRelation").attr("relationValue", "");
                    alert("You must choose \"Main Entity\" type! ");
                }
                else if ($("#" + toId).attr("sumtype") == "1") {
                    $("#tmpRelation").text(relation.replace(" ", ""));
                    $("#tmpTarget").text(to.replace(" ", ""));
                    $("#tmpValue").text(value);
                    $("#tmpTarget").attr("entityId", toId);
                    $("#tmpRelation").attr("relationValue", relationValue);
                } else {
                    $("#divRelation a[class*='jstree-clicked']").removeClass("jstree-clicked");
                    $("#tmpRelation").text("");
                    $("#tmpTarget").text("");
                    $("#tmpValue").text("");
                    $("#tmpTarget").attr("entityId", "");
                    $("#tmpRelation").attr("relationValue", "");
                    alert("You must choose \"Transaction\" type! ");
                }
            }

            //Save check function
            function SaveCheck(btnId) {
                var id = "txt" + btnId.substring(btnId.indexOf("btn") + 3, btnId.length) + "Name";
                if ($("#" + id).val() == "" && Entity.ParentID != "0") {
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

            //Load the Sub Entities tree function
            function loadSubEntities(entityID) {
                $("#divSubEntities")
                    .jstree({
                        "json_data": {
                            "ajax": {
                                type: "POST",
                                url: "EntitiesAjax.aspx",
                                async: false,
                                data: {
                                    entityID: entityID,
                                    type: "loadSubEntities"
                                },
                                success: function (data) {
                                    if (data == "Session has expired.") {
                                        alert(data);
                                        window.parent.location.reload();
                                    }
                                },
                                error: function (data) {
                                    if (data.responseText == "Session has expired.") {
                                        alert(data.responseText);
                                        window.parent.location.reload();
                                    }
                                }
                            }
                        },
                        "plugins": [
                            "themes", "json_data", "ui", "crrm"
                        ],
                        "themes": { "themes": "default-rtl", "dots": false, "icons": false }
                    }).bind("select_node.jstree", function () {
                        var clicked = $.jstree._focused().get_selected();
                        var _id = clicked[0].id;
                        var _name = clicked[0].attributes.entityName.value;
                        entityClick(_id, _name);
                    });
            }

            //Entities click event function
            function entityClick(id, name) {
                Entity.EntityName = name;
                Entity.EntityID = id;
                Entity.Currency = $("#" + Entity.EntityID).attr("currency");
                $("#hfNodeEntityName").val(Entity.EntityName);

                $("#hfNodeEntityId").val(Entity.EntityID);
                Entity.ParentID = $("#" + Entity.EntityID).attr("parentid");

                if (Entity.ParentID == "0") {
                    $("#divSubEntities").show();
                    $("#hfMainEntityId").val(Entity.EntityID);
                    loadSubEntities(Entity.EntityID);
                    Entity.ParentType = $("#" + Entity.EntityID).attr("entitytype");
                }
                else {
                    Entity.ParentType = $("#" + Entity.ParentID).attr("entitytype");
                }
                Entity.ER = $("#" + Entity.EntityID).attr("er");
                Entity.EntityType = $("#" + Entity.EntityID).attr("entitytype");
                $("#hfMainEntityType").val(Entity.EntityType);
                Entity.SumType = $("#" + Entity.EntityID).attr("sumtype");
                btnWork(id);
                //loadEntities(id);
                $("#btnTest").click();
                clearEntityShow();
                clearAllInput();
                $("#hfChoseEntity").val(Entity.EntityID);
                $("#hfNodeEntityParentId").val(Entity.ParentID);

                $("#hfNewNodeEntityParentType").val(Entity.ParentType);

                if (Entity.ParentID != 0 && Entity.SumType == "0")
                    Entity.IsLastLevel = true;
                else
                    Entity.IsLastLevel = false;

                if ($("#" + Entity.EntityID).attr("accountid") != "0")
                    Entity.IsAccount = true;
                else
                    Entity.IsAccount = false;

                if (Entity.IsAccount == true) { //account
                    //$("#hfNewNodeEntityParentType").val($("#"+clickedId)[0].attributes.entitytype.value);
                    $("#tbAccount,#fsRelation").show();
                    $("#trSumType").hide();
                    $("#ddlAccCompany").val($("#" + Entity.EntityID).attr("company"));
                    $("#txtAccName").val($("#" + Entity.EntityID).attr("accountname"));
                    $("#txtAccPwd").val($("#" + Entity.EntityID).attr("password"));
                    $("#ddlAccType").val($("#" + Entity.EntityID).attr("accounttype"));
                    $("#txtAccBetLimit").val($("#" + Entity.EntityID).attr("bettinglimit"));
                    $("#ddlAccStatus").val($("#" + Entity.EntityID).attr("status"));

                    $("#txtDateOpen").val($("#" + Entity.EntityID).attr("dateopen"));
                    $("#txtPersonnel").val($("#" + Entity.EntityID).attr("Personnel"));
                    $("#txtIP").val($("#" + Entity.EntityID).attr("ip"));
                    $("#txtOdds").val($("#" + Entity.EntityID).attr("odds"));
                    $("#txtIssuesConditions").val($("#" + Entity.EntityID).attr("IssuesConditions"));
                    $("#txtRemarks").val($("#" + Entity.EntityID).attr("remarks"));
                    $("#txtFactor").val($("#" + Entity.EntityID).attr("factor"));
                    $("#txtPerBet").val($("#" + Entity.EntityID).attr("perBet"));

                    $("#hfNodeEntityType").val($("#" + Entity.EntityID).attr("entitytype"));

                    $("#hfNodeEntityAccountID").val($("#" + Entity.EntityID).attr("accountid"));
                    $("#hfNodeEntityId").val(id);

                    $("#rbNodeEntityLastLVY").prop("checked", true);
                    $("#cbAccount").prop("disabled", true).prop("checked", true);

                    entitiesInitial(Entity.SumType, id, Entity.EntityName, "account", Entity.ParentType);
                }
                    //} else if (cls.indexOf("LastNodeEntity") != -1) { //last entity

                    //    $("#ddlAccCompany").val('');
                    //    $("#txtAccName").val('');
                    //    $("#txtAccPwd").val('');
                    //    $("#ddlAccType").val('');
                    //    $("#txtAccBetLimit").val('');
                    //    $("#ddlAccStatus").val('');

                    //    $("#hfNodeEntityType").val($("#" + id)[0].attributes.entitytype.value);
                    //    $("#hfNodeEntityParentId").val($("#" + id)[0].attributes.parentid.value);
                    //    $("#hfNodeEntityId").val(id);
                    //    $("#trSumType").hide();
                    //    $("#tbAccount").hide();
                    //    entitiesInitial(id,name, "node",_entityType);
                    //} 
                else if (parseInt(Entity.SumType) > 0 || (!Entity.IsAccount && Entity.SumType == "0" && Entity.ParentID != "0")) {// entity
                    if (Entity.SumType == "1" || Entity.SumType == "2")
                        $("input[name='rbNodeEntityLast']").prop("disabled", true);
                    else
                        $("input[name='rbNodeEntityLast']").removeAttr("disabled", true);
                    $("#trSumType").show();
                    $("#tbAccount,#fsRelation").hide();
                    $("#rbNodeEntityLastLVN").prop("checked", "checked");
                    $("#ddlNewSumType").val($("#" + Entity.EntityID).attr("sumtype"));

                    $("#ddlAccCompany").val('');
                    $("#txtAccName").val('');
                    $("#txtAccPwd").val('');
                    $("#ddlAccType").val('');
                    $("#txtAccBetLimit").val('');
                    $("#ddlAccStatus").val('');

                    $("#hfNodeEntityType").val($("#" + Entity.EntityID).attr("entitytype"));
                    $("#hfNodeEntityParentId").val($("#" + Entity.EntityID).attr("parentid"));
                    $("#hfNodeEntityId").val(id);

                    entitiesInitial(Entity.SumType, id, Entity.EntityName, "node", Entity.ParentType);
                }
                else if (Entity.ParentType == "Cash") {
                    $("#trCashMainEntityER,#divCashMainEntity").show();
                    $("#divMainEntityRelation,#divMainEntity,#fsRelation").hide();
                    $("#txtCashMainEntityName").val(Entity.EntityName);
                    $("#hfCashEntityId").val(id);//sTest
                    $("#txtTest").val($("#" + Entity.EntityID).attr("currency"));
                    $("#sTest").text($("#" + Entity.EntityID).attr("currency"));
                    $("#txtCashMainEntityCurrency").val($("#" + Entity.EntityID).attr("currency"));
                    $("#txtCashMainEntityER").val($("#" + Entity.EntityID).attr("er"));
                    $("#txtContactNumber").val($("#" + Entity.EntityID).attr("contractnumber"));
                    $("#txtTallyName").val($("#" + Entity.EntityID).attr("tallyname"));
                    $("#txtTallyNumber").val($("#" + Entity.EntityID).attr("tallynumber"));
                    $("#txtSettlementName").val($("#" + Entity.EntityID).attr("settlementname"));
                    $("#txtSettlementNumber").val($("#" + Entity.EntityID).attr("settlementnumber"));
                    $("#txtRecommendedby").val($("#" + Entity.EntityID).attr("recommendedby"));
                    $("#txtSkype").val($("#" + Entity.EntityID).attr("skype"));
                    $("#txtQQ").val($("#" + Entity.EntityID).attr("qq"));
                    $("#txtEmail").val($("#" + Entity.EntityID).attr("email"));
                    $("#txtCreditLimit").val($("#" + Entity.EntityID).attr("creditlimit"));

                    entitiesInitial(Entity.SumType, id, Entity.EntityName, "Cash", Entity.ParentType);

                }
                else if (Entity.ParentType == "PAndL") {
                    $("#trSumType,#divMainEntity,#trMainEntityER,#tbMainEntityRelation,#divMainEntityRelation,#fsRelation").show();
                    $("#hfMainEntityType").val(Entity.ParentType);
                    $("#txtMainEntityName").val(Entity.EntityName);
                    entitiesInitial(Entity.SumType, id, Entity.EntityName, "main", Entity.ParentType);
                }
                else {
                    $("#trSumType,#divMainEntity,#trMainEntityER").show();
                    $("#hfMainEntityType").val(Entity.ParentType);
                    $("#txtMainEntityName").val(Entity.EntityName);
                    entitiesInitial(Entity.SumType, id, Entity.EntityName, "main", Entity.ParentType);
                }

            }


            function clickCss() {
                $('#divNotSet > ul > li > a,#divTransactionEntities > ul > li > a, #divSubTotalEntities > ul > li > a, #divNodeEntities2 > ul > li > a, #divAccount > ul > li > a, #divNodeEntities1 > ul > li > a').click(function () {
                    $('#divNotSet li,#divTransactionEntities li, #divSubTotalEntities li, #divNodeEntities2 li, #divAccount li, #divNodeEntities1 li').removeClass('active');
                    $(this).closest('li').addClass('active');
                    var checkElement = $(this).next();
                    if ((checkElement.is('ul')) && (checkElement.is(':visible'))) {
                        $(this).closest('li').removeClass('active');
                        checkElement.slideUp('normal');
                    }
                    if ((checkElement.is('ul')) && (!checkElement.is(':visible'))) {
                        $('#divNotSet ul ul:visible,#divTransactionEntities ul ul:visible, #divSubTotalEntities ul ul:visible, #divNodeEntities2 ul ul:visible, #divNodeEntities1 ul ul:visible, #divAccount ul ul:visible').slideUp('normal');
                        checkElement.slideDown('normal');
                    }
                    if ($(this).closest('li').find('ul').children().length == 0) {
                        return true;
                    } else {
                        return false;
                    }
                });
            }

            //function loadEntities(entityID)
            //{

            //    //$("#divTransactionEntities,#divSubTotalEntities,#divAccount,#divNodeEntities1,#divNodeEntities2").css("display","");
            //    $.ajax({
            //        type: "POST",
            //        url: "EntitiesAjax.aspx",
            //        async:false,
            //        data: {
            //            EntityID:entityID,
            //            Type:"load"
            //        },
            //        success: function (data) {                        
            //            if(data!="Session has expired.")
            //            {
            //                $("#divTransactionEntities").html('');
            //                $("#divNodeEntities1").html('');
            //                $("#divSubTotalEntities").html('');
            //                $("#divNodeEntities2").html('');
            //                $("#divAccount").html('');
            //                var _dataAry = data.split(',');
            //                if(_dataAry[0]=="")
            //                    $("#divTransactionEntities").css("display","none");
            //                else
            //                {
            //                    $("#divTransactionEntities").html('');
            //                    $("#divTransactionEntities").css("display","").html(_dataAry[0]);
            //                }

            //                if(_dataAry[1]=="")
            //                    $("#divNodeEntities1").css("display","none");
            //                else
            //                    $("#divNodeEntities1").css("display","").html(_dataAry[1]);

            //                if(_dataAry[2]=="")
            //                    $("#divSubTotalEntities").css("display","none");
            //                else
            //                    $("#divSubTotalEntities").css("display","").html(_dataAry[2]);

            //                if(_dataAry[3]=="")
            //                    $("#divNodeEntities2").css("display","none");
            //                else
            //                    $("#divNodeEntities2").css("display","").html(_dataAry[3]);

            //                if(_dataAry[4]=="")
            //                    $("#divAccount").css("display","none");
            //                else
            //                    $("#divAccount").css("display","").html(_dataAry[4]);

            //                if(_dataAry[5]=="")
            //                {
            //                    $("#divNotSet").css("display","none");
            //                    $("#tdNotSet").css("display","none");
            //                }
            //                else
            //                {
            //                    $("#tdNotSet").css("display","");
            //                    $("#divNotSet").css("display","").html(_dataAry[5]);
            //                }

            //                if(_dataAry[1]=="" && _dataAry[3]=="")
            //                {
            //                    $("#tdSubTotal").attr("colspan","2");
            //                    $("#tdAcc").attr("colspan","2");
            //                    $("#tdNode1").css("display","none");
            //                    $("#tdNode2").css("display","none");

            //                }
            //                else if(_dataAry[1]=="")
            //                {

            //                    $("#tdSubTotal").attr("colspan","1");
            //                    $("#tdAcc").attr("colspan","1");
            //                    $("#tdNode1").css("display","none");
            //                    $("#tdNode2").css("display","").attr("colspan","2");
            //                }
            //                else
            //                {
            //                    $("#tdSubTotal").attr("colspan","2");
            //                    $("#tdAcc").attr("colspan","1");
            //                    $("#tdNode1").css("display","").attr("colspan","1");
            //                    $("#tdNode2").css("display","none");
            //                }
            //                clickCss();
            //            }
            //            else
            //            {
            //                alert(data);
            //                window.parent.location.reload();
            //            }

            //        }
            //    });
            //}


            function entitiesInitial(sumTypeValue, entityID, entityName, nodeType, entityType) {
                //if($("#" + Id)[0].attributes.sumtype.value>0)
                //    $("#cbAccount").attr("disabled", "disabled");
                $("#ddlAccCompany,#txtAccName,#txtAccPwd,#ddlAccType,#txtAccBetLimit,#ddlAccStatus").val('');

                //$("#rbNodeEntityLastLVN").prop("checked", "checked");
                $("#trSumType").show();
                $("#ddlNewSumType").val(sumTypeValue);
                $("#hfNodeEntityType").val($("#" + entityID).attr("entitytype"));
                $("#hfNodeEntityParentId").val($("#" + entityID).attr("parentid"));
                $("#hfNodeEntityId").val(entityID);

                if (nodeType == "node") {
                    $("#divNodeEntity").show();
                    $("#divMainEntityRelation,#tbAccount").hide();
                    $("#cbAccount").removeAttr("checked").attr("disabled", "disabled");
                    $("#txtEntityCurrency,#txtEntityER").val('');

                    if (Entity.IsLastLevel) {
                        $("#rbNodeEntityLastLVY").prop("checked", true);
                        $("#divNotLastLevel").hide();
                    }
                    else {
                        $("#rbNodeEntityLastLVN").prop("checked", true);
                        $("#divNotLastLevel").show();
                    }
                    $("#cbAccount").prop("disabled", true);
                    $("#txtNodeEntityName").val(entityName);

                    if (Entity.SumType == "1" || Entity.SumType == "2")
                        $("#txtNodeEntityName").attr("disabled", "disabled");
                    else
                        $("#txtNodeEntityName").removeAttr("disabled");

                    $("#txtEntityCurrency").val($("#" + entityID).attr("Currency"));
                    $("#txtEntityER").val($("#" + entityID).attr("ER"));
                    $("#tbRelation").html("<table><tr><td style='text-align:center;'><img src='img/ajax-loader-dark.gif' /></td></tr></table>");
                    //$.ajax({
                    //    type: "POST",
                    //    url: "RelationAjax.aspx",
                    //    data: {
                    //        Relation: $("#tmpRelation").text(),
                    //        RelationValue:$("#tmpRelation").attr("relationValue"),
                    //        EntityId: entityID,
                    //        Target: $("#tmpTarget").text(),
                    //        TargetId:$("#tmpTarget").attr("entityId"),
                    //        Value: $("#tmpValue").text(),
                    //        RelationLoad:"true",
                    //        Type:"load"
                    //    },
                    //    success: function (data) {
                    //        if(data!="Session has expired.")
                    //        {
                    //            $("#tbRelation").html('');
                    //            $("#tbRelation").append(data);
                    //        }
                    //        else
                    //        {
                    //            alert(data);
                    //            window.parent.location.reload();
                    //        }                            
                    //    }
                    //});
                }
                else if (nodeType == "account") {
                    $("input[name='rbNodeEntityLast']").each(function () {
                        $(this).removeAttr("disabled");
                    });
                    $("#fsRelation,#divEntityRelation,#divNodeEntity,#tbAccount").show();
                    $("#divMainEntityRelation,#divNotLastLevel").hide();
                    $("#txtEntityCurrency,#txtEntityER").val('');
                    $("#rbNodeEntityLastLVY").prop("checked", true);
                    $("#cbAccount").prop("disabled", true).prop("checked", true);

                    $("#ddlAccCompany").val($("#" + entityID).attr("company"));
                    $("#txtAccName").val($("#" + entityID).attr("accountname"));
                    $("#txtAccPwd").val($("#" + entityID).attr("password"));
                    $("#ddlAccType").val($("#" + entityID).attr("accounttype"));
                    $("#txtAccBetLimit").val($("#" + entityID).attr("bettinglimit"));
                    $("#ddlAccStatus").val($("#" + entityID).attr("status"));


                    $("#txtDateOpen").val($("#" + entityID).attr("dateopen"));
                    $("#txtPersonnel").val($("#" + entityID).attr("Personnel"));
                    $("#txtIP").val($("#" + entityID).attr("ip"));
                    $("#txtOdds").val($("#" + entityID).attr("odds"));
                    $("#txtIssuesConditions").val($("#" + entityID).attr("IssuesConditions"));
                    $("#txtRemarks").val($("#" + entityID).attr("remarks"));
                    $("#txtFactor").val($("#" + entityID).attr("factor"));
                    $("#txtPerBet").val($("#" + entityID).attr("perBet"));

                    $("#hfNodeEntityAccountID").val($("#" + entityID).attr("accountid"));

                    $("#txtNodeEntityName").val(entityName);
                    $("#txtEntityCurrency").val($("#" + entityID).attr("Currency"));
                    $("#txtEntityER").val($("#" + entityID).attr("ER"));
                    $("#tbRelation").html("<table><tr><td style='text-align:center;'><img src='img/ajax-loader-dark.gif' /></td></tr></table>");
                    $.ajax({
                        type: "POST",
                        url: "RelationAjax.aspx",
                        data: {
                            Relation: $("#tmpRelation").text(),
                            RelationValue: $("#tmpRelation").attr("relationValue"),
                            EntityId: entityID,
                            Target: $("#tmpTarget").text(),
                            TargetId: $("#tmpTarget").attr("entityId"),
                            Value: $("#tmpValue").text(),
                            RelationLoad: "true",
                            Type: "load"
                        },
                        success: function (data) {
                            if (data != "Session has expired.") {
                                $("#tbRelation").html('');
                                $("#tbRelation").append(data);
                            }
                            else {
                                alert(data);
                                window.parent.location.reload();
                            }

                        }
                    });
                }
                else if (entityType == "Cash") {
                    $("#divMainEntityRelation,#divEntityRelation,#fsRelation").hide();

                    $("#txtCashMainEntityCurrency").val($("#" + entityID).attr("Currency"));
                    $("#txtCashMainEntityER").val($("#" + entityID).attr("ER"));
                }
                    //else if (Entity.EntityType == "PAndL") {//P&L Main
                    //    $("#txtMainEntityCurrency").val($("#" + entityID).attr("Currency"));
                    //    $("#txtMainEntityER").val($("#" + entityID).attr("ER"));
                    //    $("#divMainEntityRelation,#fsRelation").show();
                    //    $("#divEntityRelation").hide();

                    //    if (entityType == "PAndL") {
                    //        $("#divMainEntityRelation,#tbMainEntityRelation,#fsRelation").show();
                    //        $.ajax({
                    //            type: "POST",
                    //            url: "RelationAjax.aspx",
                    //            data: {
                    //                Relation: $("#tmpRelation").text(),
                    //                RelationValue: $("#tmpRelation").attr("relationValue"),
                    //                EntityId: entityID,
                    //                Target: $("#tmpTarget").text(),
                    //                TargetId: $("#tmpTarget").attr("entityId"),
                    //                Value: $("#tmpValue").text(),
                    //                RelationLoad: "true",
                    //                Type: "load"
                    //            },
                    //            success: function (data) {
                    //                if (data != "Session has expired.") {
                    //                    $("#tbMainEntityRelation").html('');
                    //                    $("#tbMainEntityRelation").append(data);
                    //                }
                    //                else {
                    //                    alert(data);
                    //                    window.parent.location.reload();
                    //                }
                    //            }
                    //        });
                    //    }
                    //}
                else {//P&L Main
                    $("#txtMainEntityCurrency").val($("#" + entityID).attr("Currency"));
                    $("#txtMainEntityER").val($("#" + entityID).attr("ER"));
                    $("#divMainEntityRelation,#tbMainEntityRelation,#fsRelation").hide();
                    $("#divEntityRelation").hide();
                    if (entityType == "PAndL") {
                        $("#divMainEntityRelation,#tbMainEntityRelation,#fsRelation").show();
                        $.ajax({
                            type: "POST",
                            url: "RelationAjax.aspx",
                            data: {
                                Relation: $("#tmpRelation").text(),
                                RelationValue: $("#tmpRelation").attr("relationValue"),
                                EntityId: entityID,
                                Target: $("#tmpTarget").text(),
                                TargetId: $("#tmpTarget").attr("entityId"),
                                Value: $("#tmpValue").text(),
                                RelationLoad: "true",
                                Type: "load"
                            },
                            success: function (data) {
                                if (data != "Session has expired.") {

                                    $("#tbMainEntityRelation").html('');
                                    $("#tbMainEntityRelation").append(data);
                                }
                                else {
                                    alert(data);
                                    window.parent.location.reload();
                                }
                            }
                        });
                    }
                }
                $('html, body').scrollTop(0);
            }

            //clear all input
            function clearAllInput() {
                $("#txtNodeEntityName,#txtNewNodeEntityName").val("");
            }

            //hide all entities
            function clearEntityShow() {
                $("#requiredHint").show();
                $(".Entity").hide();
            }


            function btnWork(id) {
                var _sumType = $("#" + id).attr("sumtype");
                $("#btnAddNewEntity").attr("class", "btn01").removeAttr("disabled");
                $("#btnRemoveEntity").attr("class", "btn01").removeAttr("disabled");
                $("#btnBadDebt").attr("class", "btn01").removeAttr("disabled");
                if (_sumType == "Subtotal")
                    $("#btnSetExchangeRate").attr("class", "btn01").removeAttr("disabled");
                else
                    $("#btnSetExchangeRate").attr("class", "disablebtn").attr("disabled", "disabled");
            }

            //
            function ShowHideDiv(id) {
                var clickedId = $("#showEntitiesBar .jstree-clicked").parent().attr("id");
                if (id != "") {
                    $(".Entity").hide();
                    $("#" + id).show();
                    $("#requiredHint").show();
                    clearAllInput();
                    //$("#cbAccount,#cbNewAccount").removeAttr("checked").attr("disabled", "disabled");

                    //$("#rbNodeEntityLastLVY").removeAttr("checked");
                    if (id == "divSetExchangeRate") {
                        $("#hfCurrencyEntityId").val(Entity.EntityID);
                        $("#ddlCurrency").val(Entity.Currency);
                        $("#txtCurrencyER").val(Entity.ER);
                        $("#EREntityName").text(Entity.EntityName);
                    }


                    if (id == 'divNewNodeEntity') {
                        $("#hfNewNodeEntityParentId").val(Entity.EntityID);
                        $("#hfNewNodeEntityParentType").val(Entity.EntityType);
                        $("#txtNewNodeEntityName,#txtNewAccName,#txtNewBettingLimit").val('');
                    }

                    if (id == 'divNewMainEntity') {
                        $("#divSelectRole,#divNewMainEntity").show();
                        $("#txtNewMainEntityName").val("");
                        $("#SelectRole").children().each(function () {
                            if ($(this).text() == "PAndL") {
                                $(this).attr("selected", true);
                            }
                        });

                    }
                }
            }
    </script>
    <style type="text/css">
        .ui-icon-closethick:hover {
            background-image: url(css/images/ui-icons_cd0a0a_256x240.png);

        }
        .erStyle {
            font-weight: bold;            
            color: ffffff;
            background-color:#E0E0E0;
            border-style:None;
            border:none;
            box-shadow:none;
        }
       ? fieldset.rfdRoundedCorners
        {
            background-position: 0 0;
        }
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
*+html


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
		
		.relation {
            display: none;
        }
        #showEntitiesBar {
            margin-top: 0px;
            height: 100%;
	width: 190px;
	text-align: left;
	-webkit-border-radius: 5px;
	border-radius: 5px;
        }

        #showEntitiesBar a:hover {
	font-weight: normal;
	line-height: 14px;
	font-size: 13px;
	color: #ffffff;
	font-family: Verdana, Geneva, sans-serif;
	font-style: normal;
	font-variant: normal;
	text-transform: none;
	text-decoration: none;
	border: 1px solid #7e7e7e;
	width: 190px;
	-webkit-border-radius: 5px;
	border-radius: 5px;
	word-spacing: 2px;
        }
            #showEntitiesBar a
            {
	font-weight: normal;
	line-height: normal;
	font-size: 13px;
	color: #f2cdac;
	font-family: Verdana, Geneva, sans-serif;
	font-style: normal;
	font-variant: normal;
	text-transform: none;
	text-decoration: none;
	background-color: transparent;
	height: 27px;
	width: 190px;
	background-image: url(img/item_a.gif);
	background-repeat: no-repeat;
	border-top-width: 0px;
	border-right-width: 0px;
	border-bottom-width: 0px;
	border-left-width: 0px;
	border-top-style: none;
	border-right-style: none;
	border-bottom-style: none;
	border-left-style: none;
	padding-left: 33px;
	padding-top: 7px;
	cursor: pointer;
	-webkit-border-radius: 5px;
	border-radius: 5px;
	word-spacing: 2px;
            }
			
        .jstree-default .jstree-clicked
        {
	color: #bf0000;
            font-weight: bold;
        }
        .relationTitle {
            font-weight: bold;
	color: #FFF;
        }
        #PnLRelation,#PnLRelationButton {
            display: none;
        }
        .auto-style1
        {
            width: 176px;
        }
        a:hover
        {

            cursor: pointer;
        }
.textbox {
	font-family: Verdana, Geneva, sans-serif;
	background-color: #28251a;
	height: 22px;
	border: 1px solid #414246;
	font-size: 13px;
	font-style: normal;
	line-height: 14px;
	font-weight: bold;
	font-variant: normal;
	text-transform: none;
	color: #9495A0;
	padding-left: 5px;
	width: 150px;
	background-image: url(img/bg03.png);
	background-repeat: repeat-x;
}

.textbox02 {
	font-family: Verdana, Geneva, sans-serif;
	background-color: #28251a;
	height: 26px;
	border: 1px solid #414246;
	font-size: 13px;
	font-style: normal;
	line-height: 14px;
	font-weight: bold;
	font-variant: normal;
	text-transform: none;
	color: #9495A0;
	padding-left: 3px;
	background-image: url(img/bg03.png);
	background-repeat: repeat-x;
}

.textbox03 {
	font-family: Verdana, Geneva, sans-serif;
	background-color: #28251a;
	height: 22px;
	border: 1px solid #414246;
	font-size: 13px;
	font-style: normal;
	line-height: 14px;
	font-weight: bold;
	font-variant: normal;
	text-transform: none;
	color: #9495A0;
	padding-left: 5px;
	background-image: url(img/bg03.png);
	background-repeat: repeat-x;
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
    
    <form id="form1" runat="server"><br><br><center><label class="TE_wd04">Entities Management</label>
        <div style="width: 1050px; border: none; text-align: left" class="a03_border"><br><br><div style="width: 100%; height: 690px; overflow-x: hidden; overflow-y: auto; ">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<table border="0" cellspacing="0" cellpadding="6px" align="center">
  <tr>
    <td width="248px" rowspan="2" align="right" valign="top"><table width="240px" border="0" cellpadding="0" cellspacing="3" align="center" >
                                <tr>
                                    <td height="15px" colspan="2" align="center" valign="middle"><input class="NewMain" id="btnNewMain" type="button" value="" title="New Main Entity" onclick="ShowHideDiv('divNewMainEntity');" />&nbsp;<select class="textbox02" id="ddlEntityType" runat="server" style="width: 95px; position: relative; top: -10px">
                                            <option value="0">AnyType</option>
                                            <option value="1">P&amp;L</option>
                                            <option value="2">Cash</option>
                                            <option value="3">Expence</option>
                                            <option value="4">BadDebt</option>
                                            <option value="5">MLJ</option>
                                    </select><hr class="a_line"></hr></td>
                                </tr>
                                <tr>
                                <td height="34" colspan="2" align="center" valign="middle" style="border:0px"><input type="text" placeholder="Enter Entity Name" id="txtMainEntitySearch" runat="server" class="a01_input" style="width: 134px; position: relative; top: -8px"/>&nbsp;<asp:Button ID="btnSearchMainEntity" runat="server" CssClass="Search02" Text="" title="Search" OnClick="btnSearchMainEntity_Click"  /><hr style="position: relative; top: -2px" class="a_line"></hr></td> 
                                </tr>
                                <tr>
                                  <td colspan="2" align="center" valign="top" style="border:0px"><div id="showEntitiesBar" style="width: 238px; position: relative; top: 6px; left: -12px">
                            
                            <script type="text/javascript">
                                $(function () {

                                });
                            </script>

                        </div></td>
                              </tr>
    </table></td>
    <td width="710px" colspan="2" align="left" valign="top"><fieldset class="rfdRoundedCorners" id="fs1" style="width: 749px; position: relative; top: -16px" runat="server" name="fs1">
                                        <legend class="legend_wd"> Functions </legend>
                                        <div style="text-align: left"><input class="disablebtn" id="btnAddNewEntity" disabled="disabled" type="button" value="New Entity" title="New Entity" onclick="ShowHideDiv('divNewNodeEntity');" />&nbsp;
                                                        <input type="button" class="disablebtn" value="Remove Entity" title="Remove Entity" disabled="disabled" id="btnRemoveEntity" />&nbsp;
                                <input class="disablebtn" id="btnSetExchangeRate" type="button" disabled="disabled" title="Change Rate" value="Change Rate" onclick="ShowHideDiv('divSetExchangeRate');" />&nbsp;
                                <asp:Button runat="server" ID="btnBadDebt" CssClass="disablebtn" Enabled="False" ClientIDMode="Static" Text="Save as BadDebt" title="Save as BadDebt" />
                                                        <input type="hidden" runat="server" id="hfChoseEntity" /></div>

          </fieldset></td>
  </tr>
  <tr>
    <td align="left" valign="top"><fieldset style="background-color:# FF0; height: 880px; width: 321px; position: relative; top: -32px" id="Fieldset1" runat="server" class="rfdRoundedCorners">
                                        <legend align="top" class="legend_wd">SubEntities</legend>
                                        <div id="divSubEntities">
                                            
                                        </div>
                                    </fieldset></td>
    <td align="left" valign="top"><fieldset id="fsEntityAttr" runat="server" class="rfdRoundedCorners" style="height: 880px; width: 392px; position: relative; top: -33px">
                                        <legend align="top" class="legend_wd">Attributes</legend>
                                        
                                            <div id="divSelectRole" class="Entity">
                                                <table  width="100%" border="0" cellspacing="4" cellpadding="4">
                                                    <tr>
                                                        <td align="left" valign="middle" colspan="2">Role :&nbsp;<select id="SelectRole" runat="server" class="textbox02">
                                                                <option selected="selected" value="1">P&amp;L</option>
                                                                <option value="2">Cash</option>
                                                                <option value="3">Expense</option>
                                                                <option value="5">MLJ</option>
                                                            </select>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div><br>
                                            <div id="requiredHint" style="height: 22px; text-align: left; vertical-align: middle">&nbsp;<span class="required" style="font-weight: bold">● Required Field</span></div>
                                            <div id="divCashMainEntity" class="Entity">
                                                <table  width="100%" border="0" cellspacing="4" cellpadding="4">
                                                    <tr>
                                                        <td align="left" valign="middle" colspan="3"><span class="required">*</span> Name :&nbsp;<input id="txtCashMainEntityName" class="textbox" type="text" runat="server" /></td>
                                                    </tr>
                                                    <tr id="trCashMainEntityER">
                                                        <td align="left" valign="middle" colspan="3">Exchange Rate :&nbsp;<input type="text" id="txtCashMainEntityCurrency" runat="server" style="width: 113px" class="textbox03"/>&nbsp;<input type="text" id="txtCashMainEntityER" runat="server" style="width: 60px" class="textbox03"/></td>
                                                    </tr>

                                                    <tr>
                                                        <td align="right" valign="middle">Contact Number :</td>
                                                        <td colspan="2">
                                                            <input id="txtContactNumber" class="textbox" type="text" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle">Tally Name :</td>
                                                        <td colspan="2">
                                                            <input id="txtTallyName" class="textbox" type="text" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle">Tally Number :</td>
                                                        <td colspan="2">
                                                            <input id="txtTallyNumber" class="textbox" type="text" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle">Settlement Name :</td>
                                                        <td colspan="2">
                                                            <input id="txtSettlementName" class="textbox" type="text" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle">Settlement Number :</td>
                                                        <td colspan="2">
                                                            <input id="txtSettlementNumber" class="textbox" type="text" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle">Recommended by :</td>
                                                        <td colspan="2">
                                                            <input id="txtRecommendedby" class="textbox" type="text" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle">Skype :</td>
                                                        <td colspan="2">
                                                            <input id="txtSkype" type="text" class="textbox" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle">QQ :</td>
                                                        <td colspan="2">
                                                            <input id="txtQQ" type="text" class="textbox" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle">Email :</td>
                                                        <td colspan="2">
                                                            <input id="txtEmail" type="text" class="textbox" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle"><span class="required">*</span>Credit Limit :</td>
                                                        <td colspan="2">
                                                            <input id="txtCreditLimit" maxlength="15" class="textbox" type="text" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                    
                                                        <td colspan="3" align="center" valign="middle" height="44px">
                                                            <input type="hidden" id="hfCashEntityId" runat="server" />
                                                            <asp:Button runat="server" ClientIDMode="Static" CssClass="Save01" Text="" title="Save" ID="btnCashMainEntity" OnClick="btnCashMainEntity_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="divMainEntity" class="Entity">
                                                <table  width="100%" border="0" cellspacing="4" cellpadding="4">
                                                    <tr>
                                                        <td align="left" valign="middle" colspan="3"><span class="required">*</span> Name :&nbsp;<input id="txtMainEntityName" runat="server" type="text" /></td>
                                                    </tr>
                                                    <tr id="trMainEntityER">
                                                        <td align="left" valign="middle" colspan="3">Exchange Rate :&nbsp;<input type="text" id="txtMainEntityCurrency" style="width: 113px" class="textbox03" runat="server"  />&nbsp;<input type="text" id="txtMainEntityER" style="width: 60px" class="textbox03" runat="server"/></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" align="center" valign="middle" height="44px">
                                                            <input type="hidden" id="hfMainEntityId" runat="server" />
                                                            <input type="hidden" id="hfMainEntityType" runat="server" />
                                                            <asp:Button runat="server" ClientIDMode="Static" CssClass="Save01" ID="btnMainEntity" title="Save" Text="" OnClick="btnMainEntity_Click" />
                                                        </td>
                                                    </tr>
                                                    
                                                </table>
                                            </div>

                                            <div id="divNewCashMainEntity" class="Entity">
                                                <table  width="100%" border="0" cellspacing="4" cellpadding="4">
                                                    <tr>
                                                        <td align="left" valign="middle" colspan="3"><span class="required">*</span> Name :&nbsp;<input id="txtNewCashMainEntityName" placeholder="Enter Entity name" runat="server" type="text" /></td>
                                                    </tr>

                                                    <tr>
                                                        <td align="right" valign="middle">Contact Number :</td>
                                                        <td>
                                                            <input id="txtNewContactNumber" type="text" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle">Tally Name :</td>
                                                        <td>
                                                            <input id="txtNewTallyName" type="text" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle">Tally Number :</td>
                                                        <td>
                                                            <input id="txtNewTallyNumber" type="text" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle">Settlement Name :</td>
                                                        <td>
                                                            <input id="txtNewSettlementName" type="text" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle">Settlement Number :</td>
                                                        <td>
                                                            <input id="txtNewSettlementNumber" type="text" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle">Recommended by :</td>
                                                        <td>
                                                            <input id="txtNewRecommendedby" type="text" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle">Skype :</td>
                                                        <td>
                                                            <input id="txtNewSkype" type="text" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle">QQ :</td>
                                                        <td>
                                                            <input id="txtNewQQ" type="text" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle">Email :</td>
                                                        <td>
                                                            <input id="txtNewEmail" type="text" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle"><span class="required">*</span>Credit Limit :</td>
                                                        <td>
                                                            <input id="txtNewCreditLimit" maxlength="15" type="text" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="center" valign="middle" height="44px">
                                                            <asp:Button runat="server" ClientIDMode="Static" CssClass="Save01" ID="btnNewCashMainEntity" Text="" title="Save" OnClick="btnNewCashMainEntity_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                            <div id="divNewMainEntity" class="Entity">
                                                <table  width="100%" border="0" cellspacing="4" cellpadding="4">
                                                    <tr>
                                                        <td align="left" valign="middle" colspan="3"><span class="required">*</span> Name :&nbsp;<input id="txtNewMainEntityName" placeholder="Enter Entity name" runat="server" type="text" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="center" valign="middle" height="44px">
                                                            <asp:Button runat="server" ClientIDMode="Static" CssClass="Add01" ID="btnNewMainEntity" Text="" title="Add" OnClick="btnNewMainEntity_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                            <div id="divNewEntity" class="Entity">
                                                <table  width="100%" border="0" cellspacing="4" cellpadding="4">
                                                    <tr>
                                                        <td align="left" valign="middle" colspan="3"><span class="required">*</span> Name :&nbsp;<input id="txtNewEntityName" type="text" /></td> 
                                                    </tr>
                                                    <tr>
                                                    <td colspan="3" align="left" valign="middle" style="text-align: left; font-weight: bold; color: #dac592"><input id="rbNewEntityLastN" checked="checked" name="rbLast" type="radio" />Not last level</td>
                                                    </tr>
                                                    <tr><td colspan="3" align="left" valign="middle" style="text-align: left; font-weight: bold; color: #dac592"><input id="rbNewEntityLastY" name="rbLast" type="radio" />Last Level Entity&nbsp;&nbsp;&nbsp;<input id="cbNewEntityAccount" type="checkbox" />Account</td></tr>
                                                    <tr>
                                                        <td colspan="3" align="center" valign="middle" height="44px">
                                                            <input id="btnNewEntity" class="Save01" type="button" title="Save" value=""/></td>

                                                    </tr>
                                                </table>
                                            </div>

                                            <div id="divSetExchangeRate" class="Entity">
                                                <table  width="100%" border="0" cellspacing="4" cellpadding="4">
                                                    <tr>
                                                        <td align="right" valign="middle" colspan="2">Entity Name : <span style="font-weight: bold; line-height: 40px; height: 40px; color: black;" id="EREntityName"></span></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <select id="ddlCurrency" style="width: 100px" runat="server">
                                                            </select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle">
                                                            <span class="required">*</span><input id="txtCurrencyER" type="text" runat="server" /></td>
                                                        <td align="left" valign="middle">&nbsp;
                                                            <input type="hidden" id="hfCurrencyEntityId" runat="server" />
                                                            <input type="button" id="btnCurrencyER"  class="Set" value="" title="Set" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>



                                            <div id="divNodeEntity" class="Entity">
                                                <table  width="100%" border="0" cellspacing="4" cellpadding="4">
                                                    <tr>
                                                        <td align="left" valign="middle" colspan="3"><span class="required">*</span> Name :&nbsp;<input id="txtNodeEntityName" type="text" runat="server" /></td>
                                                    </tr>
                                                    <tr><td colspan="3" align="left" valign="middle"><div style="text-align: left; font-weight: bold; color: #dac592" id="divNotLastLevel"><input id="rbNodeEntityLastLVN" name="rbNodeEntityLast" type="radio" runat="server"  disabled="True" />Not last level</div></td></tr>
                                                    
                                                    <tr><td colspan="3" align="left" valign="middle" style="font-weight: bold; color: #dac592"><input id="rbNodeEntityLastLVY" name="rbNodeEntityLast" type="radio" runat="server" disabled="True" />Last Level Entity&nbsp;&nbsp;&nbsp;<input id="cbAccount" disabled="True" type="checkbox" runat="server" />Account</td></tr>

                                                    <tr>
                                                        <td align="left" valign="middle" colspan="3">Exchange Rate :&nbsp;<input type="text" style="width: 113px" class="textbox03" id="txtEntityCurrency"  runat="server" />&nbsp;<input type="text" id="txtEntityER" style="width: 60px" class="textbox03" runat="server" /></td>
                                                    </tr>
                                                    

                                                    <tr id="tbAccount" style="display: none">
                                                        <td colspan="3">
                                                            <table  width="100%" border="0" cellspacing="4" cellpadding="4">
                                                                <tr>
                                                                    <td colspan="3" >Account Attributes</td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">Company :</td>
                                                                    <td colspan="2" align="left" valign="middle"><select id="ddlAccCompany" runat="server" class="textbox02" style="width: 158px">
                                                                            <option value="1">Company1</option>
                                                                            <option value="2">Company2</option>
                                                                            <option value="3">Company3</option>
                                                                            <option value="4">Company4</option>
                                                                            <option value="5">Company5</option>
                                                                            <option value="6">Company6</option>
                                                                        </select></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">Account Name :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtAccName" type="text" runat="server" style="font-family: Verdana, Geneva, sans-serif; background-color: #28251a; height: 22px; border: 1px solid #414246; font-size: 13px; font-style: normal; line-height: 14px; font-weight: bold; font-variant: normal; text-transform: none; color: #9495A0; padding-left: 5px; width: 150px; background-image: url(img/bg03.png); background-repeat: repeat-x;"/></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">Password :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtAccPwd" type="password" runat="server" style="font-family: Verdana, Geneva, sans-serif; background-color: #28251a; height: 22px; border: 1px solid #414246; font-size: 13px; font-style: normal; line-height: 14px; font-weight: bold; font-variant: normal; text-transform: none; color: #9495A0; padding-left: 5px; width: 150px; background-image: url(img/bg03.png); background-repeat: repeat-x;"/></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">Account Type :</td>
                                                                    <td colspan="2" align="left" valign="middle"><select style="width: 158px" id="ddlAccType" runat="server" class="textbox02">
                                                                            <option value="1">SuperSenior</option>
                                                                            <option value="2">Senior</option>
                                                                            <option value="3">Master</option>
                                                                            <option value="4">Agent</option>
                                                                            <option value="5">Members</option>
                                                                        </select></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle"><span class="required">*</span>Betting Limit :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtAccBetLimit" class="textbox" type="text" onkeydown="return checkNum(event);" runat="server" /></td>
                                                                </tr>

                                                                <tr>
                                                                    <td align="right" valign="middle">DateOpen :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtDateOpen" class="textbox" type="text" runat="server" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">Personnel :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtPersonnel" class="textbox" disabled="True" type="text" runat="server" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">IP :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtIP" type="text" class="textbox" runat="server" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">Odds :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtOdds" type="text" class="textbox" runat="server" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">Issues/Conditions :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtIssuesConditions" class="textbox" type="text" runat="server" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">Remarks :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtRemarks" type="text" class="textbox" runat="server" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">Factor :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtFactor" type="text" class="textbox" runat="server" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">Per Bet :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtPerBet" type="text" class="textbox" onkeydown="return checkNum(event);" runat="server" /></td>
                                                                </tr>

                                                                <tr>
                                                                    <td align="right" valign="middle">Status :</td>
                                                                    <td colspan="2" align="left" valign="middle"><select id="ddlAccStatus" runat="server" class="textbox02">
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
                                                        <td align="center" valign="middle" colspan="3" height="44px">
                                                            <input id="hfNodeEntityId" type="hidden" runat="server" />
                                                            <input id="hfNodeEntityName" type="hidden" runat="server" />
                                                            <input id="hfNodeEntityType" type="hidden" runat="server" />
                                                            <input type="hidden" id="hfNodeEntityParentId" runat="server" />
                                                            <input type="hidden" id="hfNodeEntityAccountID" runat="server" />
                                                            <input type="button" class="Save01" id="btnNodeEntity" value="" title="Save" />

                                                        </td>
                                                    </tr>
                                                    
                                                </table>
                                            </div>
                                            <div id="divNewNodeEntity" class="Entity">
                                                <table  width="100%" border="0" cellspacing="4" cellpadding="4">
                                                   <tr>
                                                        <td align="left" valign="middle" colspan="3"><span class="required">*</span> Name :&nbsp;<input id="txtNewNodeEntityName" placeholder="Enter Entity name" type="text" runat="server" />
                                                            <select id="ddlNewCurrency" runat="server" class="textbox02">
                                                            </select>
                                                        </td>
                                                        <tr>
                                                            
                                                        <td align="left" valign="middle" colspan="3">
                                                            <div id="divNewNotLastLevel" style="text-align: left; font-weight: bold; color: #dac592"><input id="rbNewNodeEntityLastLVN" name="rbNewNodeEntityLast" runat="server" type="radio" />Not last level</div></td>
                                                        </tr>
                                                        
                                                        <tr>
                                                            
                                                        <td align="left" valign="middle" colspan="3" style="text-align: left; font-weight: bold; color: #dac592"><input id="rbNewNodeEntityLastLVY" name="rbNewNodeEntityLast" runat="server" type="radio" />Last Level Entity&nbsp;&nbsp;&nbsp;<input id="cbNewAccount" type="checkbox" runat="server" />Account                                               
                                                        </td>
                                                    </tr> 

                                                  </tr> 

                                                    <tr id="trSumType">
                                                        <td align="left" valign="middle" colspan="3">Sum Type :&nbsp;<select id="ddlNewSumType" runat="server" class="textbox02">
                                                                <option value="0">Not</option>                                                                
                                                                <option value="1">Transaction</option>
                                                                <option value="2">Subtotal</option>
                                                                <option value="3">Super</option>
                                                                <option value="4">Master</option>
                                                                <option value="5">Agent</option>
                                                            </select>
                                                        </td>
                                                    </tr>

                                                    <tr id="tbNewAccount" style="display: none">
                                                        <td colspan="3">
                                                            <table  width="100%" border="0" cellspacing="4" cellpadding="4">
                                                                <tr>
                                                                    <td colspan="3">Account Attributes</td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">Company :</td>
                                                                    <td colspan="2" align="left" valign="middle"><select id="ddlNewAccCompany" runat="server" class="textbox02" style="width: 158px">
                                                                            <option value="1">Company1</option>
                                                                            <option value="2">Company2</option>
                                                                            <option value="3">Company3</option>
                                                                            <option value="4">Company4</option>
                                                                            <option value="5">Company5</option>
                                                                            <option value="6">Company6</option>
                                                                        </select></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">Account Name :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtNewAccName" class="textbox" type="text" runat="server" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">Password :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtNewAccPwd" class="textbox" type="password" runat="server" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">Account Type :</td>
                                                                    <td colspan="2" align="left" valign="middle">
                                                                        <select id="ddlNewAccType" runat="server" class="textbox02" style="width: 158px">
                                                                            <option value="1">SuperSenior</option>
                                                                            <option value="2">Senior</option>
                                                                            <option value="3">Master</option>
                                                                            <option value="4">Agent</option>
                                                                            <option value="5">Members</option>
                                                                        </select></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle"><span class="required">*</span>Betting Limit :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtNewBettingLimit" class="textbox" type="text" maxlength="15" onkeydown="return checkNum(event);" runat="server" /></td>
                                                                </tr>

                                                                <tr>
                                                                    <td align="right" valign="middle">DateOpen :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtNewDateOpen" class="textbox" type="text" runat="server" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">IP :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtNewIP" class="textbox" type="text" runat="server" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">Odds :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtNewOdds" class="textbox" type="text" runat="server" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">Issues/Conditions :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtNewIssuesConditions" class="textbox" type="text" runat="server" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">Remarks :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtNewRemarks" class="textbox" type="text" runat="server" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">Factor :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtNewFactor" class="textbox" type="text" runat="server" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="middle">Per Bet :</td>
                                                                    <td colspan="2">
                                                                        <input id="txtNewPerBet" class="textbox" type="text" onkeydown="return checkNum(event);" runat="server" /></td>
                                                                </tr>

                                                                <tr>
                                                                    <td align="right" valign="middle">Status :</td>
                                                                    <td colspan="2" align="left" valign="middle"><select id="ddlNewAccStatus" runat="server" class="textbox02">
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
                                                        <td colspan="3" align="center" valign="middle" height="44px">
                                                            <input type="hidden" id="hfNewNodeEntityParentId" runat="server" />
                                                            <input type="hidden" id="hfNewNodeEntityParentType" runat="server" />
                                                            <input type="button" class="Add01" id="btnNewNodeEntity" value="" title="Add" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        <fieldset class="rfdRoundedCorners" style="display:none" runat="server" id="fsRelation">
                                            <legend>Entity Relations</legend>
                                            <div id="divMainEntityRelation" class="relation">
                                                <table  width="100%" border="0" cellspacing="4" cellpadding="4">
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="6px" id="tbMainEntityRelation">
                                                </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" valign="top" height="36px"> <input id="btnSummeryRelation"  class="Set" type="button" value="" title="Set Summary Entity" /></td>
                                                    </tr>
                                                </table>
                                                
                                               
                                            </div>
                                            <div id="divEntityRelation" class="relation">
                                                <table  width="100%" border="0" cellspacing="4" cellpadding="4">
                                                    <tr>
                                                        <td>
                                                            <table id="tbRelation" width="100%" border="0" cellspacing="0" cellpadding="5">
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="44px" align="center" valign="middle" colspan="3">
                                                            <input id="btnNodeRelation" class="Add01" type="btn" value="" title="Add Relation" /></td>
                                                    </tr>
                                                </table>


                                            </div>
                                            <div id="divAddRelation">
                                                
                                            </div>
                                            <div id="divRelationDialog">
                                                <table width="430px" border="0" cellspacing="4" cellpadding="4">
                                                    <tr>
                                                        <td align="left" valign="middle" width="207px">
                                                            <input class="textbox03" style="width: 180px" type="text" placeholder="Enter Entity Name" id="txtRelationMainEntitySearch" runat="server" />
                                                        </td>
                                                        <td align="left" valign="middle" width="108px">
                                                            <select id="ddlRelationEntityType" runat="server" class="textbox02" style="width: 100px">
                                                                <option value="0">AnyType</option>
                                                                <option value="1">P&amp;L</option>
                                                                <option value="2">Cash</option>
                                                                <option value="3">Expence</option>
                                                                <option value="4">BadDebt</option>
                                                                <option value="5">MLJ</option>
                                                            </select>
                                                        </td>
                                                        <td align="left" valign="middle" width="115px"><input style="background-color: transparent; background-image: url(../img/SearchBtn.png); height: 28px; width: 79px; border-top-width: 0px; border-right-width: 0px; border-bottom-width: 0px; border-left-width: 0px; border-top-style: none;	border-right-style: none; border-bottom-style: none; border-left-style: none; cursor: pointer;" type="button" value="" title="Search" id="btnRelationEntitySearch" /></td>
                                                    </tr>
                                                </table>
                                                <table width="430px" border="0" cellspacing="4" cellpadding="4" >
                                                    <tr>
                                                        <td align="left" valign="middle" colspan="2" width="207px">Relation :&nbsp;<select id="ddlRelation" class="textbox02">
                                                                <option value="1">Allocate</option>
                                                                <option value="2">Position</option>
                                                                <option value="3">Commission</option>
                                                                <option value="4">Follow Bet</option>
                                                                <option value="5">P&amp;L Sum</option>
                                                            </select>
                                                        </td>
                                                        <td id="tdRelationReq" align="right" valign="middle" width="60px"><span class="required">*</span>Value :</td>
                                                        <td id="tdRelationVal" align="left" valign="middle" width="163px">&nbsp;<input type="text" class="textbox03" style="width: 50px" onkeydown="return checkNum(event);" id="txtRelationValue" /></td>
                                                    </tr>
                                                </table>

                                                <br />
                                                <div id="divRelation"></div>

                                                <br />
                                                <table style="color: #ffffff; font-weight: bold; font-family: Verdana, Geneva, sans-serif; font-size: 13px; word-spacing: 2px; border-color: #ffffff; border-collapse:collapse" width="100%" border="1px" cellspacing="0px" cellpadding="0px" >
                                                    <tr>
                                                        <td height="26" align="center" valign="middle" bgcolor="#2e9e7b" style="background-image:url(img/bg03.png); background-repeat: repeat-x">Relation</td>
                                                        <td align="center" valign="middle" bgcolor="#2e9e7b" style="background-image:url(img/bg03.png); background-repeat: repeat-x">To</td>
                                                        <td align="center" valign="middle" bgcolor="#2e9e7b" id="tdRelationValTitle" style="background-image:url(img/bg03.png); background-repeat: repeat-x">Value</td>
                                                        <td bgcolor="#2e9e7b" style="background-image:url(img/bg03.png); background-repeat: repeat-x">&nbsp;</td>
  </tr>
                                                    <tr style="color: #F30; font-weight: bold; background-color: rgba(255, 255, 255, 0.8)" id="trRelation">
                                                        <td height="36" align="center" valign="middle"><span style="color: #F30;" id="tmpRelation"></span></td>
                                                        <td align="center" valign="middle"><span style="color: #F30;" id="tmpTarget" entityid=""></span></td>
                                                        <td align="center" valign="middle" id="tdTmpRelationVal"><span style="color: #F30;" id="tmpValue"></span></td>
                                                        <td height="44px" align="center" valign="middle" ><input id="btnAddRelation" type="button" value="" title="Add" style="background-color: transparent; background-image: url(../img/Add01.png); height: 28px; width: 68px; border-top-width: 0px; border-right-width: 0px; border-bottom-width: 0px;
	border-left-width: 0px; border-top-style: none; border-right-style: none; border-bottom-style: none; border-left-style: none; cursor: pointer"/></td>
                                                    </tr>
                                                </table>
                                            </div>

                                        </fieldset>
                                    </fieldset></td>
  </tr>
</table></div><br><br></div>
        <input type="hidden" id="hfReload" runat="server" />
    </center></form>
</body>
</html>
