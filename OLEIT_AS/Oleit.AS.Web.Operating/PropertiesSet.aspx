<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PropertiesSet.aspx.cs" Inherits="Accounting_System.PropertiesSet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!--<link href="css/AccountingSystem.css" rel="stylesheet" />-->
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <link href="css/table.css" rel="stylesheet" />
    <link href="css/Css.css" rel="stylesheet" type="text/css">
    <link href="css/a_style.css" rel="stylesheet" type="text/css">
    <script src="js/jquery-1.9.1.min.js"></script>
    <script src="js/jquery-ui-1.9.2.custom.min.js"></script>
    <script src="js/jquery.watermark.js"></script>
    <script src="js/commonFunc.js"></script>
    <title>Properties Setting</title>
    <script type="text/javascript">
        $(function () {
            $("#addNewProperty,#editNewProperty").dialog({
                autoOpen: false,
                width: '390px'
            });

            $("#btnCancelAdd").click(function () {
                $("#addNewProperty").dialog("close");
                $("#propertyName").val("");
                $("#propertyValue").val("");
            });
            
            $("#btnCancelEdit").click(function () {
                $("#editNewProperty").dialog("close");
                $("#propertyName").val("");
                $("#propertyValue").val("");
            });
            
        });
        
        //Add Function
        function addFunc() {
            if (repeatedCheck($("#txtPropertyNameAdd").val(),"add") == true) {
                if ($("#propertyName").val() != "" && $("#txtPropertyValueAdd").val() != "") {
                    var append = $("#addNewProperty").dialog("close");//prevent jquery dialog moves it outside the form
                    append.parent().appendTo($("#form1"));//prevent jquery dialog moves it outside the form
                    alert("Add success!");
                    return true;
                } else {
                    alert("Please enter \"Property Name\" and \"Property Value\"!");
                    return false;
                }
            } else {
                return false;
            }
        }

        //Edit function
        function editFunc() {
            if (repeatedCheck($("#txtPropertyNameEdit").val(),"edit") == true) {
                if ($("#txtPropertyNameEdit").val() != "" && $("#txtPropertyValueEdit").val() != "") {
                    var hfEditValueId = $("#hfEditValueId").val();
                    $("#" + hfEditValueId).text($("#txtPropertyNameEdit").val());
                    $("#" + hfEditValueId + "Value").text($("#txtPropertyValueEdit").val());
                    $("#" + hfEditValueId).attr("id", $("#txtPropertyNameEdit").val());
                    $("#" + hfEditValueId + "Value").attr("id", $("#txtPropertyNameEdit").val() + "Value");
                    var append = $("#editNewProperty").dialog("close");//prevent jquery dialog moves it outside the form
                    append.parent().appendTo($("#form1"));//prevent jquery dialog moves it outside the form
                    alert("Edit success!");
                    return true;
                } else {
                    alert("Please enter \"Property Name\" and \"Property Value\"!");
                    return false;
                }
            } else {
                return false;
            }
        }

        //check the property name whether is repeated
        function repeatedCheck(properName,type) {
            var repeated = false;
            var count = 0;
            if (type == "add") {
                $(".Properties").each(function () {
                    if (properName == $(this).attr("id")) {
                        repeated = true;
                        return false;
                    }
                });
            } else {
                if (properName != $("#hfEditValueId").val()) {
                    $(".Properties").each(function() {
                        if (properName == $(this).attr("id")) {
                            count++;
                        }
                    });
                    if (count >= 1)
                        repeated = true;
                }
            }
            if (repeated == false)
                return true;//not repeated
            else {
                alert("Do not set repeated name.");
                return false;//repeated
            }
        }

        function editProperty(obj) {
            var propertyName = $(obj).attr("id");
            var propertyValue = $("#" + propertyName+"Value").text();
            $("#txtPropertyNameEdit").val(propertyName);
            $("#txtPropertyValueEdit").val(propertyValue);
            $("#hfEditValueId").val(propertyName);
            $("#editNewProperty").dialog({
                position: { my: "left top", at: "left bottom", of: "#" + propertyName },
                autoOpen: true,
                title: "Edit property " + propertyName
            });
            //editNewProperty
        }

        function addPropertyFunc(obj) {
            $("#addNewProperty").dialog({
                position: { my: "left top", at: "left bottom", of: "#" + $(obj).attr("id") },
                autoOpen: true,
                title: "Add a new property"
            });
        }

    </script>
    <style type="text/css">
        
    </style>
    
</head>
<body>
    <form id="form1" runat="server"><br /><br />
        <center><label class="TE_wd04">Properties Setting</label>
        <div style="width: 860px; border: none; text-align: left" class="a03_border">
        <br /><br /><div style="width: 100%; height: 670px; overflow-x: hidden; overflow-y: auto; "><input type="button" id="btnAddProperty" onclick="addPropertyFunc(this);" class="AddProperty02" title="AddProperty" value="" style="position: relative; left: 129px"/>
        <br /><br />
        <table id="propertyTable" style="width: 600px" class="bordered" align="center">
           <thead>
             <tr >
                <th>Property Name</th>
                <th>Property Value</th>
            </tr>
               </thead>
            <asp:Repeater ID="propertyRPT" runat="server" >
                <ItemTemplate>
                    <tr>
                        <td style="background-image: url(../img/bg03.png); background-repeat: repeat-x"><a href="#" id='<%# Eval("PropertyName") %>' onclick="editProperty(this);"><%# Eval("PropertyName") %></a></td>
                        <td id='<%# Eval("PropertyName")+"Value" %>'><%# Eval("PropertyValue") %></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <div id="addNewProperty" runat="server">
            <table style="width: 371px" border="0" cellspacing="0" cellpadding="8">
                <thead>
                <tr>
                    <td style="height: 32px" width="157px" align="right" valign="middle">
                        <asp:Label ID="propertyNamelblAdd" Text="Property Name :" ClientIDMode="Static" runat="server" CssClass="h01_wd"></asp:Label>
                    </td>
                    <td width="206px" align="left" valign="middle">
                        <input id="txtPropertyNameAdd" runat="server" type="text" class="a01_input" style="width: 198px"/>
                    </td>
                </tr>
                <tr>
                    <td style="height: 32px" align="right" valign="middle">
                        <asp:Label ID="propertyValuelblAdd" Text="Property Value :" ClientIDMode="Static" runat="server" CssClass="h01_wd"></asp:Label>
                    </td>
                    <td align="left" valign="middle">
                        <input type="text" runat="server" id="txtPropertyValueAdd" class="a01_input" style="width: 198px"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center" valign="middle" height="50px">
                        <asp:Button ID="btnAdd" CssClass="Add01" ClientIDMode="Static" Text="" title="Add" runat="server" OnClientClick="return addFunc();" OnClick="btnAdd_Click" />
                        <input type="button" id="btnCancelAdd" class="Cancel" value="" style="background-color: transparent; background-image: url(../img/Cancel.png); background-repeat: no-repeat; border-top-width: 0px; border-right-width: 0px; border-bottom-width: 0px; border-left-width: 0px; border-top-style: none; border-right-style: none; border-bottom-style: none; border-left-style: none; height: 28px; width: 68px; cursor: pointer;"/>
                    </td>
                </tr>
                    </thead>
            </table>
        </div>
        <div id="editNewProperty">
            <table style="width: 371px" border="0" cellspacing="0" cellpadding="8">
              <thead>
                <tr> 
                    <td style="height: 32px" width="157px" align="right" valign="middle">
                        <asp:Label ID="propertyNamelblEdit" Text="Property Name :" ClientIDMode="Static" runat="server"></asp:Label>
                    </td>
                    <td <td width="206px" align="left" valign="middle"><input type="text" id="txtPropertyNameEdit" runat="server" class="a01_input" style="width: 198px"/>
                    </td>
                </tr>
                
                <tr>
                    <td style="height: 32px" align="right" valign="middle">
                        <asp:Label ID="propertyValuelblEdit" Text="Property Value :" ClientIDMode="Static" runat="server"></asp:Label>
                    </td>
                    <td align="left" valign="middle"><input type="text" id="txtPropertyValueEdit" runat="server" class="a01_input" style="width: 198px"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center" valign="bottom" height="50px">
                        <asp:Button runat="server" ID="btnEdit" ClientIDMode="Static" CssClass="Edit" OnClientClick="return editFunc();" Text="" title="Edit" OnClick="btnEdit_Click"/>
                        <input type="button" id="btnCancelEdit" class="Cancel" value="" title="Cancel" style="background-color: transparent; background-image: url(../img/Cancel.png); background-repeat: no-repeat; border-top-width: 0px; border-right-width: 0px; border-bottom-width: 0px; border-left-width: 0px; border-top-style: none; border-right-style: none; border-bottom-style: none; border-left-style: none; height: 28px; width: 68px; cursor: pointer;" />
                        <input type="hidden" id="hfEditValueId" value="" runat="server"/>
                    </td> 
                </tr>
                  </thead>
            </table>
        </div></div><br /><br /></div>
    </form>
</body>
</html>
