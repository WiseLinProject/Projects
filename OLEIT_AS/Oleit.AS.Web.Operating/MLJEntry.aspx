<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MLJEntry.aspx.cs" Inherits="Accounting_System.MLJEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <!--<link href="css/AccountingSystem.css" rel="stylesheet" />
    <link href="css/Control.css" rel="stylesheet" />-->
    <link href="css/table.css" rel="stylesheet" />
    <link href="css/Css.css" rel="stylesheet" type="text/css">
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <script src="js/jquery-1.9.1.min.js"></script>
    <script src="js/jquery-ui-1.9.2.custom.min.js"></script>
    <title>MLJ Entry</title>
<style type="text/css">
        /*AutoComplete flyout */
        .completionList {
            border: solid 1px #444444;
            margin: 0px;
            padding: 2px;
            height: 100px;
            overflow: auto;
            background-color: #FFFFFF;
        }

        .listItem {
            color: #1C1C1C;
        }

        .itemHighlighted {
            background-color: #ffc0c0;
        }




        .classic {
            padding: 0.8em 1em;
        }

        .custom {
            padding: 0.5em 0.8em 0.8em 2em;
        }

        * html a:hover {
            background: transparent;
        }

        .info {
            background: #CCEEFF;
            border: 1px solid #2BB0D7;
        }


        .auto-style1 {
            width: 1171px;
        }

        .tooltip {
            color: #000000;
            outline: none;
            cursor: pointer;
            text-decoration: none;
            font-family: Verdana, Geneva, sans-serif;
            font-size: 14px;
            font-style: normal;
            line-height: 15px;
            font-weight: normal;
            font-variant: normal;
            text-transform: none;
        }

            .tooltip span {
                margin-left: -999em;
                position: absolute;
            }

            .tooltip:hover span {
                background: rgba(216, 216, 216, 0.95);
                border: 2px solid #ffffff;
                box-shadow: 4px 4px 3px rgba(0, 0, 0, 0.6);
                -webkit-border-radius: 15px;
                border-radius: 15px;
                font-family: Verdana, Geneva, sans-serif;
                position: absolute;
                left: 55px;
                top: 30px;
                z-index: 99;
                margin-left: 0;
                font-size: 14px;
                font-style: normal;
                line-height: 24px;
                font-weight: normal;
                font-variant: normal;
                text-transform: none;
                color: #333;
                width: 450px;
            }



            .tooltip:hover em {
                font-family: Verdana, Geneva, sans-serif;
                font-size: 15px;
                font-weight: bold;
                display: block;
                background-color: #57003d;
                font-style: normal;
                line-height: normal;
                font-variant: normal;
                text-transform: none;
                color: #FFF;
                -webkit-border-radius: 5px;
                border-radius: 5px;
                padding-top: 8px;
                padding-right: 8px;
                padding-bottom: 8px;
                padding-left: 11px;
                margin-bottom: 5px;
            }

            .tooltip:hover img {
                border: 0;
                margin: -10px 0 0 -55px;
                float: left;
                position: absolute;
            }
    </style>
    <style type="text/css">
        .modalBackground {
	background-color: Black;
	filter: alpha(opacity=90);
	opacity: 0.8;
        }

        .modalPopup {
	padding-top: 0px;
	padding-left: 0px;
	width: 900px;
	height: 600px;
	font-family: Verdana, Geneva, sans-serif;
	font-size: 13px;
	font-style: normal;
	line-height: 22px;
	font-weight: normal;
	font-variant: normal;
	text-transform: none;
	color: #c9c7c7;
	text-decoration: none;
	text-align: center;
	border: 2px solid #8a8a8a;
	box-shadow: 4px 4px 3px rgba(0, 0, 0, 0.5);
	-webkit-border-radius: 15px;
	border-radius: 15px;
	background-color: rgba(51,51,51,0.97);
        }

        .auto-style1 {
            height: 24px;
        }

        body {
            background-color: #282828;
            margin-left: 0px;
            margin-top: 0px;
            margin-right: 0px;
            margin-bottom: 0px;
        }
    </style>
    <script type="text/javascript">
        function txtKeyNumber() {
            if (!(((window.event.keyCode >= 48) && (window.event.keyCode <= 57)) ||
                (window.event.keyCode == 13) || (window.event.keyCode == 46) ||
                (window.event.keyCode == 45))) {
                event.returnValue = false;
            }
        }

    </script>
    <script language="javascript">
        function changeImg(obj, src) {
            obj.src = src;
        }

    </script>

    <meta charset="utf-8">
</head>
<body>
    <form id="form1" runat="server">
        <br>
        <br>
        <center><label class="TE_wd04">MLJ Entry</label>
     
        
        
        <div style="width: 1000px; border: none; text-align: center" class="a03_border">
                <br /><asp:Button ID="btn_Approve" runat="server" class="ApproveBtn" Text="" title="Approve" OnClick="btn_Approve_Click" Visible="False" style="padding-left: 50px"/>
            <hr style="width: 980px" class="a_line"></hr>
<div style="width: 100%; height: 630px; overflow-x: hidden; overflow-y: auto"><input type="hidden" id="control1" value="0" />
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        
    <asp:UpdatePanel ID="up_gvGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <br>
        <table  style="border:0px; grid-cell:none; column-span:none; width:990px" align="center">
                    <tr>
                      <td style="vertical-align: middle; text-align: center; height: 32px" class="a02_border_wd">&nbsp;&nbsp;Period：<asp:TextBox ID="tx_Period" runat="server" Width="85px" CssClass="a01_input" MaxLength="10"></asp:TextBox>&nbsp;&nbsp; Name：
                          <asp:TextBox  CssClass="a01_input" ID="tx_EntityName" runat="server" Width="85px" MaxLength="50"></asp:TextBox>
                          
                          &nbsp;User：<asp:DropDownList ID="DDL_User" runat="server" ClientIDMode="AutoID" CssClass="a02_input_02">
                          </asp:DropDownList>
                          
                          &nbsp;Status : <asp:DropDownList ID="DDL_Status" runat="server" CssClass="a02_input_02" AutoPostBack="false" OnSelectedIndexChanged="DDL_Status_SelectedIndexChanged">
                          </asp:DropDownList>
                          
                        <asp:AutoCompleteExtender  runat="server" 
                BehaviorID="AutoCompleteEx"
                ID="autoComplete1" 
                TargetControlID="tx_EntityName"               
                ServiceMethod="SearchCustomers"
                MinimumPrefixLength="1" 
                CompletionInterval="200"
                EnableCaching="true"
                CompletionSetCount="20"
                CompletionListCssClass="completionList" 
                CompletionListItemCssClass="listItem" 
                CompletionListHighlightedItemCssClass="itemHighlighted"
                DelimiterCharacters=";, :"
                ShowOnlyCurrentWordInCompletionListItem="true" >
                <Animations>
                    <OnShow>
                        <Sequence>                            
                            <OpacityAction Opacity="0" />
                            <HideAction Visible="true" /> 
                            <Parallel Duration=".4">
                                <FadeIn />
                                <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx')._height" />
                            </Parallel>
                        </Sequence>
                    </OnShow>
                     <OnHide>                     
                        <Parallel Duration=".4">
                            <FadeOut />
                            <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx')._height" EndValue="0" />
                        </Parallel>
                    </OnHide>
                </Animations>
</asp:AutoCompleteExtender>
                          &nbsp;<asp:Panel Style="display:inline" ID="Panel_Type" runat="server" Visible="False" Width="179px" Wrap="False">
                          Type：<asp:DropDownList ID="DDL_Type" runat="server" ClientIDMode="AutoID" CssClass="a02_input_02">
                              <asp:ListItem Value="0">Account</asp:ListItem>
                              <asp:ListItem Value="1">Customer</asp:ListItem>
                              <asp:ListItem Value="2">All</asp:ListItem>
                          </asp:DropDownList></asp:Panel><asp:Button ID="btn_Search" runat="server" Text="" title="Search" class="Search" OnClick="btn_Search_Click" UseSubmitBehavior="False" />&nbsp;<asp:Button ID="btn_Save" runat="server" class="Save01" title="Save" Text="" OnClick="btn_Save_Click" UseSubmitBehavior="False" />
                          <asp:ImageButton ID="IB_Excel" runat="server" ImageUrl="~/img/excel.png" OnClick="IB_Excel_Click" AlternateText="Export to Excel" ToolTip="Export to Excel" CssClass="excel"/>

                      </td>                 
                    </tr>
            <tr><td align="center"><hr style="width: 980px" class="a_line"></hr>
                </td></tr>
                </table>
                <br><center>
                <asp:GridView ID="gv_MLJ" CssClass="bg" runat="server" AutoGenerateColumns="False" DataKeyNames="ID,RecordID,Status,Company,Account_Name,Password,Betting_Limit,Factor,Perbet,DateOpen,Personnel,IP,Odds,IssuesConditions,RemarksAcc,EntityName,Accountid,Color,EntityID" EmptyDataText="No data !" OnRowDataBound="gv_MLJ_RowDataBound" ShowFooter="True" OnRowCommand="gv_MLJ_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Name">                           
                            <ItemTemplate>
                                <asp:ImageButton ID="IB_Detail" runat="server" CommandName="ShowDetail" Height="25" Width="25" ImageUrl="img/Add03.png" onmouseout="changeImg(this, 'img/Add03.png')" onmouseover="changeImg(this, 'img/Add03_on.png')" />
                                <asp:Literal ID="Literal_ToolTip" runat="server"></asp:Literal>                               
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mon">
                            <EditItemTemplate>
                               
                            </EditItemTemplate>
                            <ItemTemplate>
                              <asp:TextBox ID="tx_Mon" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" AutoPostBack="false" OnTextChanged="Tx_OnTextChanged" runat="server"  CssClass="a01_input_02"   Text='<%# DataBinder.Eval(Container.DataItem, "Mon", "{0:#,0}") %>' ></asp:TextBox>
                               <asp:Label ID="Lb_Mon" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Mon", "{0:#,0}") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tue">
                            <EditItemTemplate>
                               
                            </EditItemTemplate>
                            <ItemTemplate>
                               <asp:TextBox ID="tx_Tue" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" AutoPostBack="false" OnTextChanged="Tx_OnTextChanged"  runat="server"  CssClass="a01_input_02"  Text='<%# DataBinder.Eval(Container.DataItem, "Tue", "{0:#,0}") %>'></asp:TextBox>
                                <asp:Label ID="Lb_Tue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tue", "{0:#,0}") %>' Visible="false"></asp:Label>

                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Wed">
                            <EditItemTemplate>
                             
                            </EditItemTemplate>
                            <ItemTemplate> 
                             <asp:TextBox ID="tx_Wed" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" AutoPostBack="false" OnTextChanged="Tx_OnTextChanged"  runat="server"  CssClass="a01_input_02"  Text='<%# DataBinder.Eval(Container.DataItem, "Wed", "{0:#,0}") %>'></asp:TextBox>
                                <asp:Label ID="Lb_Wed" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Wed", "{0:#,0}") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Thu">
                            <EditItemTemplate>
                               
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="tx_Thu" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" AutoPostBack="false" OnTextChanged="Tx_OnTextChanged"  runat="server" CssClass="a01_input_02"  Text='<%# DataBinder.Eval(Container.DataItem, "Thu", "{0:#,0}") %>'></asp:TextBox>
                                <asp:Label ID="Lb_Thu" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Thu", "{0:#,0}") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fri">
                            <EditItemTemplate>
                               
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox  ID="tx_Fri" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" AutoPostBack="false"  OnTextChanged="Tx_OnTextChanged"  runat="server"  CssClass="a01_input_02"  Text='<%# DataBinder.Eval(Container.DataItem, "Fri", "{0:#,0}") %>'></asp:TextBox>
                                <asp:Label ID="Lb_Fri" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Fri", "{0:#,0}") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sat">
                            <EditItemTemplate>
                              
                            </EditItemTemplate>
                            <ItemTemplate>
                               <asp:TextBox ID="tx_Sat" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" AutoPostBack="false" OnTextChanged="Tx_OnTextChanged"  runat="server"  CssClass="a01_input_02"  Text='<%# DataBinder.Eval(Container.DataItem, "Sat", "{0:#,0}") %>'></asp:TextBox>
                                <asp:Label ID="Lb_Sat" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Sat", "{0:#,0}") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sun">
                            <EditItemTemplate>
                              
                            </EditItemTemplate>
                            <ItemTemplate>
                               <asp:TextBox ID="tx_Sun" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" AutoPostBack="false" OnTextChanged="Tx_OnTextChanged"  runat="server"  CssClass="a01_input_02"  Text='<%# DataBinder.Eval(Container.DataItem, "Sun", "{0:#,0}") %>'></asp:TextBox>
                                <asp:Label ID="Lb_Sun" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Sun", "{0:#,0}") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="EntityTotal" ItemStyle-HorizontalAlign="Right" HeaderText="Total">
                        <ItemStyle HorizontalAlign="Right" CssClass="bg_wd" />
                        </asp:BoundField>
                        
                    </Columns>
                    <FooterStyle CssClass="bg_wd02"/>
                </asp:GridView></center>
       </ContentTemplate>
          
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btn_Search" />         
            <asp:PostBackTrigger ControlID="IB_Excel" />
        </Triggers>       
        </asp:UpdatePanel></div>

        <asp:UpdatePanelAnimationExtender ID="UpdateAn_MLJ" runat="server" TargetControlID="up_gvGrid">
                            <Animations>
                <OnUpdating>                    
                    <Sequence>                       
                        <%-- Disable all the controls --%>
                        <Parallel duration="0">
                           <EnableAction AnimationTarget="Btn_SaveColor" Enabled="false" />
                            <EnableAction AnimationTarget="IB_Excel" Enabled="false" />
                            <EnableAction AnimationTarget="btn_Approve" Enabled="false" />
                            <EnableAction AnimationTarget="gv_MLJ" Enabled="false" />
                        </Parallel>
                        <StyleAction Attribute="overflow" Value="hidden" />                        
                        <%-- Do each of the selected effects --%>
                        <Parallel duration=".25" Fps="30">                           
                                <FadeOut AnimationTarget="up_gvGrid" minimumOpacity=".2" />
                        </Parallel>
                    </Sequence>                       
                </OnUpdating>
                <OnUpdated>                   
                    <Sequence>
                        <%-- Do each of the selected effects --%>
                        <Parallel duration=".25" Fps="30">                           
                                <FadeIn AnimationTarget="up_gvGrid" minimumOpacity=".2" />                                               
                        </Parallel>
                        <%-- Enable all the controls --%>
                        <Parallel duration="0">
                            <EnableAction AnimationTarget="Btn_SaveColor" Enabled="true" />
                            <EnableAction AnimationTarget="IB_Excel" Enabled="true" />
                            <EnableAction AnimationTarget="btn_Approve" Enabled="true" />
                            <EnableAction AnimationTarget="gv_MLJ" Enabled="true" />                     
                        </Parallel>                            
                    </Sequence>                       
                </OnUpdated>
            </Animations>
                        </asp:UpdatePanelAnimationExtender>
      <asp:Button ID="btnShow" runat="server" Text="Show Modal Popup" style="display:none" />
        <asp:ModalPopupExtender ID="mp1" runat="server" PopupControlID="pnl_Add" CancelControlID="IB_Close" TargetControlID="btnShow" BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>
        <asp:Panel ID="pnl_Add" runat="server" CssClass="modalPopup" align="center" style = "display:normal; overflow: auto">
            <asp:UpdatePanel ID="up_Edit" runat="server" UpdateMode="Conditional"><ContentTemplate>
         <table style="width: 100%; border: none">
            
             <tr>
                 <td style="text-align: left; vertical-align: middle">&nbsp;&nbsp;&nbsp;Name：<asp:Label ID="LB_DetailName" runat="server"></asp:Label>
                      &nbsp;&nbsp; Status：<asp:DropDownList CssClass="a02_input_02" ID="DDL_DetailStatus" runat="server"></asp:DropDownList>
                      &nbsp; Remark：<asp:TextBox ID="Tx_DetailRemark" runat="server" CssClass="a01_input" MaxLength="500" Width="305px"></asp:TextBox>
                      <asp:Button ID="btn_SaveAccount" runat="server" class="Save01" OnClick="btn_SaveAccount_Click" Text="" title="Save" UseSubmitBehavior="False" />
                      &nbsp;<asp:Label ID="LB_ID" runat="server" Visible="False" ForeColor="#ffffff" Font-Size="13px" Font-Bold="false"></asp:Label>
                 </td>
                 <td style="text-align: left; vertical-align: middle">
                     <asp:ImageButton ID="IB_Close" runat="server" Height="24px" ImageUrl="img/Xbtn_on.png" OnClick="IB_Close_Click" Width="24px" />
                 </td>
             </tr>
              <tr>
                 <td colspan="2" style="text-align: center"><br />
                     <center><asp:Panel ID="Panel_Status" runat="server" ScrollBars="Vertical">
                     <asp:GridView ID="gv_StatusRecord" runat="server" AutoGenerateColumns="False" CssClass="bg" EmptyDataText="No Data !">
                         <Columns>
                             <asp:BoundField HeaderText="Updater" DataField="UserAccount">
                             <ItemStyle Width="96px"/>
                             </asp:BoundField>
                             <asp:BoundField DataField="Update_Date" HeaderText="Time">
                             <ItemStyle Width="208px"/>
                             </asp:BoundField>
                             <asp:BoundField DataField="StatusName" HeaderText="Status">
                             <ItemStyle Width="160px"/>
                             </asp:BoundField>
                             <asp:BoundField DataField="RemarksAcc" HeaderText="Remark"/>
                         </Columns>
                     </asp:GridView>
                         </asp:Panel></center>
                 <br /></td>
             </tr>
             <tr>
                 <td colspan="2" style="text-align: center"><br />
                      <center><asp:Panel ID="Panel_AccountRecord" runat="server" ScrollBars="Vertical">
                     <asp:GridView ID="gv_AccountRecord" CssClass="bg" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data !!">
                         <Columns>
                             <asp:BoundField DataField="UserAccount" HeaderText="Updater" ItemStyle-Width="96px"/>
                             <asp:BoundField DataField="UpdateTime" HeaderText="Time" ItemStyle-Width="208px"/>
                             <asp:BoundField DataField="Mon" HeaderText="Mon" DataFormatString="{0:#,0}" />
                             <asp:BoundField DataField="Tue" HeaderText="Tue" DataFormatString="{0:#,0}" />
                             <asp:BoundField DataField="Wed" HeaderText="Wed" DataFormatString="{0:#,0}" />
                             <asp:BoundField DataField="Thu" HeaderText="Thu" DataFormatString="{0:#,0}" />
                             <asp:BoundField DataField="Fri" HeaderText="Fri" DataFormatString="{0:#,0}" />
                             <asp:BoundField DataField="Sat" HeaderText="Sat" DataFormatString="{0:#,0}" />
                             <asp:BoundField DataField="Sun" HeaderText="Sun" DataFormatString="{0:#,0}" />
                         </Columns>
                     </asp:GridView>
                     </asp:Panel></center>
                 <br /></td>
             </tr>
         </table>   
            </ContentTemplate></asp:UpdatePanel>    
        </asp:Panel>
    <br /><br /></div><br /></center>
    </form>
</body>
</html>
