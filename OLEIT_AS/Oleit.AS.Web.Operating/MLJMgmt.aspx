<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MLJMgmt.aspx.cs" Inherits="Accounting_System.MLJMgmt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MLJ Management</title>
    <link href="css/AccountingSystem.css" rel="stylesheet" />
    <link href="css/Css.css" rel="stylesheet" type="text/css">
    <link href="css/table.css" rel="stylesheet" />
    <style type="text/css">
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
        <center><label class="TE_wd04">MLJ Management</label>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        
            <br />
            <div style="background-color: #191919; width: 920px; height: 700px; border: none" class="a03_border"><br /><br />
            <div style="width: 100%; height: 680px; overflow-x: hidden; overflow-y: auto; "><table border="0" cellspacing="0" cellpadding="0" align="center">
  <tr>
    <td width="592px" align="center" valign="middle"><asp:UpdatePanel ID="Update_User" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>  
                    <table border="0" cellpadding="0" cellspacing="0" id="0">
                        <tr>
                            <td width="286px" class="a02_border_wd" align="center" valign="middle"><br />
                            MLJ Role :
                            <asp:DropDownList ID="DDL_MLJRole" runat="server" AutoPostBack="True" CssClass="a02_input_02" OnSelectedIndexChanged="DDL_MLJRole_SelectedIndexChanged">
                            </asp:DropDownList>
                            <br />
                            <asp:ListBox ID="Listb_User" runat="server" CssClass="a01_select" style="position: relative; top: 29px" Height="380px" Width="220px" AutoPostBack="True" OnSelectedIndexChanged="Listb_User_SelectedIndexChanged"></asp:ListBox></td>
                            
                            <td width="306px" align="center" valign="middle"><br />
                         <div style="text-align: center; vertical-align: middle; height: 32px" class="a02_border_wd">MLJ :&nbsp;<asp:DropDownList ID="DDL_MLJ" runat="server" CssClass="a02_input_02" AutoPostBack="True" OnSelectedIndexChanged="DDL_MLJ_SelectedIndexChanged">
                            <asp:ListItem Value="0">All</asp:ListItem>
                            <asp:ListItem Value="1">Customers</asp:ListItem>
                            <asp:ListItem Value="2">Accounts</asp:ListItem>
                        </asp:DropDownList>&nbsp;<asp:Button ID="Btn_SaveUser" CssClass="Save01"  runat="server" title="Save" Text=""  OnClick="Btn_SaveUser_Click" /></div><br />
                        <asp:Panel ID="Panel_MLJ" runat="server" Height="367px" Width="220px" CssClass="a02_select" style="position: relative; top: 16px"  ScrollBars="Vertical" ClientIDMode="AutoID">
                        <asp:CheckBoxList ID="CBL_MLJ" runat="server" ClientIDMode="AutoID" OnSelectedIndexChanged="CBL_MLJ_SelectedIndexChanged" AutoPostBack="True">
                      </asp:CheckBoxList></asp:Panel></td></tr>
                    </table>
        </ContentTemplate>
                           <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Btn_SaveUser" />
            </Triggers>  
                </asp:UpdatePanel></td>
    <td width="273px" align="center" valign="middle" class="a02_border_wd"><asp:UpdatePanel ID="Udate_Color" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>                                
                        Status Color :&nbsp;<asp:Button ID="Btn_SaveColor" CssClass="SaveColor"  runat="server" title="SaveColor" Text="" OnClick="Btn_SaveColor_Click" /><br />
                        <asp:GridView ID="gv_StatusColor" style="position: relative; top: 32px" CssClass="bc" runat="server" AutoGenerateColumns="False"  EmptyDataText="No data !" Height="380px" Width="220px" OnRowDataBound="gv_StatusColor_RowDataBound" DataKeyNames="MLJStatus">
                    <Columns>
                        <asp:TemplateField HeaderText="Status">                           
                            <ItemTemplate>
                                <asp:Label ID="Lb_Color" runat="server" Text='<%# Bind("MLJStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pick Color">                            
                            <ItemTemplate>
                                <asp:TextBox ID="Tx_Color" Width="70px" runat="server" Text='<%# Bind("MLJColor") %>' ></asp:TextBox>
                                <cc1:ColorPickerExtender  ID="ColorPickerExtender1" runat="server"  TargetControlID="Tx_Color" SampleControlID="Tx_Color"   ></cc1:ColorPickerExtender>
                            </ItemTemplate>
                            <ItemStyle Width="500px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </ContentTemplate>
                           <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Btn_SaveColor" />
        </Triggers>  
    </asp:UpdatePanel></td>
  </tr>
</table>
            </div></div>       
    </form>
</body>
</html>
