<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestCode2.aspx.cs" Inherits="Accounting_System.TestCode2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title><script type="text/javascript">

                       function demo() {
                           $("#divZone").append("<div>New div</div>")
                           .children("div").css({ backgeoundColor: "blue", color: "white" });
                       }
                       var plLoader = new ActiveXObject("Oleit.AS.Service.DataObject");
                       var pageId = 'Button12';
                       __doPostBack('Button12', RaisePostBackEvent);

    </script>
    <style type="text/css"> 
        .modalBackground 
        { 
            background-color: Gray; 
        } 
        .modalPopup 
        { 
            background-color: #EAFDFF; 
            border-width: 3px; 
            border-style: solid; 
            border-color: Gray; 
            padding: 3px; 
            width: 250px; 
        } 
        .auto-style1 {
            border-collapse: collapse;
            font-size: 12.0pt;
            font-family: Calibri, sans-serif;
            border: 1.0pt solid windowtext;
        }
    </style>
    <%--<script type="text/javascript">

        var session = ['<%=Session["userid"]%>'];

        alert('<%=aa%>');

</script>--%>
     <link href="css/AccountingSystem.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="button" id="Button12"/>
        <input type="button" id="kk" onclick="demo();" />
        <div id="divZone">1111</div>
        <asp:Button ID="Button1" runat="server" Text="Get AD UserList" OnClick="Button1_Click" />
    &nbsp;&nbsp;
        <br />
        <br />
                   
                    <asp:TreeView ID="Tree_Entity" runat="server" ImageSet="Arrows" BorderWidth="0" BackColor="#E0E0E0" BorderStyle="None" >
                        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                        <NodeStyle Font-Names="Verdana" Font-Size="12pt" BackColor="#E0E0E0" ForeColor="Black" HorizontalPadding="8px" NodeSpacing="3px" VerticalPadding="0px" />
                        <ParentNodeStyle Font-Bold="False" />
                        <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
                    </asp:TreeView>
                   
              
                   
                <br />
        <br />
        <asp:Button ID="Button7" runat="server" Text="AddUser" OnClick="Button7_Click" />
        <br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ID :<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <br />
        Password:<asp:TextBox ID="TextBox2" runat="server" TextMode="Password"></asp:TextBox>
        <br />
        <br /><asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Login" />

        <br />
        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="GetCurrency" />
    &nbsp;<br />
        <br />
        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="Button4" runat="server" OnClick="Button4_Click" Text="DelCurrency" />
        <br />
        <br />
        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="Button5" runat="server" OnClick="Button5_Click" Text="Add Period" />
        <br />
        <br />
        <br />
        <asp:Button ID="Button6" runat="server" OnClick="Button6_Click" Text="Property" />
        <br />
        <br />
        <asp:Button ID="Button8" runat="server" OnClick="Button8_Click" Text="Add Entity" />
    &nbsp;<asp:Button ID="Button9" runat="server" OnClick="Button9_Click" Text="Call Entity" style="margin-bottom: 0px" />
        <br />
        <br />
        <asp:Button ID="Button10" runat="server" OnClick="Button10_Click" Text="Able Entity" />
        <br />
        <br />
        <asp:Button ID="Button11" runat="server" OnClick="Button11_Click" Text="Set Relation" />
    &nbsp;<br />
        <br />
        <asp:Button ID="Button13" runat="server" OnClick="Button13_Click" Text="Calculate" />
        <br />
        <br />
        <asp:Button ID="Button14" runat="server" OnClick="Button14_Click" Text="ColsedReverse" />
        <br />
        <br />
        <asp:Button ID="Button15" runat="server" OnClick="Button15_Click" Text="Add Record" />
        <br />
        <br />
        <asp:Button ID="Button16" runat="server" OnClick="Button16_Click" Text="Confirm" />
        <br />
        <br />
        <asp:Button ID="Button17" runat="server" OnClick="Button17_Click" Text="Add Transfer" />
        <br />
        <br />
        <asp:Button ID="Button18" runat="server" OnClick="Button18_Click" Text="Transaction" />
        </div>

        <div>
        <p>Say bye-bey to Postbacks.</p>

        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>
        <br />
        <asp:TextBox ID="TextBox11" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="BtnTest" runat="server" Text="Go" OnClientClick="Test(); return false;" />
            <br />
    </div>

           <asp:UpdatePanel ID="UpdatePanel1"  runat="server">
                <ContentTemplate>
         <asp:Button ID="Button21" runat="server" Text="Button" OnClick="Button21_Click" />
                    <br />
                    <br />
                    <br />
          </ContentTemplate>
                </asp:UpdatePanel>
         <asp:Button id="btnShowPopup" runat="server" style="display:none" />
                    
            		<cc1:ModalPopupExtender ID="mdlPopup" runat="server" 
            		    TargetControlID="btnShowPopup" PopupControlID="pnlPopup" 
            		    BackgroundCssClass="modalBackground"
            		/>
                <asp:Panel  ID="PanelData"  style="display:none"  runat="server" CssClass="" Width="600px">   
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
        <table style="width:400px">
             <thead>
    <tr>           
        <th colspan="2" style="text-align:center">Edit Transaction</th>       
    </tr>
    </thead><tr><td>From:</td><td>                   
                   <asp:Label ID="lb_FromEntity" runat="server"></asp:Label>
                    <asp:Label ID="lb_FromEntityID" runat="server" Visible="false"></asp:Label>
                   <asp:Button ID="btn_showFrom" runat="server" Text="Select"  CssClass="btn"/>
                   <asp:Panel ID="Panel1" runat="server" Visible="false">
                    <asp:TreeView ID="Tree_FromEntity" runat="server" PopulateNodesFromClient="true" ImageSet="Arrows" BorderWidth="0" BackColor="#E0E0E0" BorderStyle="None" ExpandDepth="0" >
                        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                        <NodeStyle Font-Names="Verdana" Font-Size="12pt" BackColor="#E0E0E0" ForeColor="Black" HorizontalPadding="8px" NodeSpacing="3px" VerticalPadding="0px" />
                        <ParentNodeStyle Font-Bold="False" />
                        <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
                    </asp:TreeView>
                       </asp:Panel>
                 </td></tr>
               <tr><td>To:</td><td>
                   <asp:Label ID="lb_ToEntity" runat="server"></asp:Label>
                    <asp:Label ID="lb_ToEntityID" runat="server" Visible="false"></asp:Label>
                    <asp:Button ID="btn_showTo" runat="server" Text="Select" CssClass="btn" />
                    <asp:Panel ID="Panel2" runat="server" Visible="false">
                    <asp:TreeView ID="Tree_ToEntity" runat="server" ImageSet="Arrows" BorderWidth="0" BackColor="#E0E0E0" BorderStyle="None" >
                        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                        <NodeStyle Font-Names="Verdana" Font-Size="12pt" BackColor="#E0E0E0" ForeColor="Black" HorizontalPadding="8px" NodeSpacing="3px" VerticalPadding="0px" />
                        <ParentNodeStyle Font-Bold="False" />
                        <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
                    </asp:TreeView>
                    </asp:Panel>               
                   </td></tr>
               <tr><td class="auto-style24">Amount:</td><td>
                <input id="txAmount" type="text" /></td></tr>
            <tr><td colspan="2" style="text-align:center"><input id="btnAddConfirm" type="button" value="Confirm" class="btn"/>&nbsp;&nbsp;&nbsp;
                    <input id="btnAddCancel" type="button" value="Cancel"  class="btn"/></td></tr>
        </table>

                        </ContentTemplate>
                    </asp:UpdatePanel>         
         </asp:Panel>
             
        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="OpportunityDetailID,FunnelProductID1">
                                <Columns>
                                    <asp:BoundField DataField="ProductCategory" HeaderText="Product Category"
                                        SortExpression="ProductCategory"  />
                                    <asp:BoundField DataField="Total" HeaderText="Total $" SortExpression="Total"
                                        DataFormatString="{0:c}"/>
                                    <asp:BoundField DataField="Percentage" HeaderText="% Probability"
                                        SortExpression="Percentage" DataFormatString="{0:p0}" />
                                    <asp:TemplateField>
                                        <ItemTemplate>                                            
                                            <cc1:CollapsiblePanelExtender
                                                ID="CollapsiblePanelExtender11"
                                                runat="server"
                                                TargetControlID="Panel8"
                                                ExpandControlID="Panel7"
                                                CollapseControlID="Panel7"
                                                TextLabelID="Label5"
                                                CollapsedText="BM Extra Info"
                                                ExpandedText="Hide Dialog"
                                                ImageControlID="Image5"
                                                ExpandedImage="/Content/images/collapse_blue.jpg"
                                                CollapsedImage="/Content/images/expand_blue.jpg"
                                                Collapsed="True"
                                                SuppressPostBack="true"
                                                ExpandDirection="Horizontal">
                                            </cc1:CollapsiblePanelExtender>
                                            <asp:Panel ID="Panel7" runat="server" CssClass="collapsePanelHeader">
                                                <asp:ImageButton ID="ImageButton1" runat="server" CommandName="Select" />
                                               
                                                <asp:Label ID="Label5" runat="server" Text="BM"></asp:Label>
                                            </asp:Panel>
                                            <asp:Panel ID="Panel8" runat="server" CssClass="collapsePanel">
                                                <table>
                                                    <tr><td>Expected RFQ Rec'd Date</td>    <td><asp:TextBox ID="TextBox78" Text='<%# DataBinder.Eval(Container.DataItem, "ExpectedRFQDate") %>' runat="server" /></td></tr>
                                                
                                                </table>
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                   
                                 
                                </Columns>
                            </asp:GridView>
    </form>
</body>
</html>
