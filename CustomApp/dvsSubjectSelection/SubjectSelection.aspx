<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SubjectSelection.aspx.cs" Inherits="SubjectSelection.SubjectSelection" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Subject Information</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css"/>
    <link href="Styles/template.css" rel="stylesheet" type="text/css"/>
    <link href="Styles/lookup.css" rel="stylesheet" type="text/css"/>
    <style type="text/css">
        .auto-style1 {
            height: 22px;
        }
        .auto-style2 {
            height: 10px;
        }
    </style>
</head>
<body style="background-color:whitesmoke">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="smLookup" runat="server" EnableScriptLocalization="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="uPnl_Lookup" runat="server">
        <ContentTemplate>
            <center>               
                <table>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Label ID="lbTitle" runat="server" Text="Subject Information" Font-Bold="true"
                                Font-Underline="false" Font-Size="Medium" style="color:black"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 1px; font-size: 1px; border-bottom-color: #cccccc; border-bottom-width: 1px;
                            border-bottom-style: solid;" colspan="3">
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <asp:RadioButtonList ID="rbCaller" runat="server" OnSelectedIndexChanged="callerOnCheck" RepeatDirection="Horizontal"> 
                            <asp:ListItem Text="Outgoing"></asp:ListItem>
                            <asp:ListItem Text="Incoming"></asp:ListItem>
                        </asp:RadioButtonList>
                        </td>                       
                    </tr>
                    <tr>                        
                        <td colspan="3" class="auto-style2" />
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbCallTypeC" runat="server" Text="Call Type" Font-Bold="true" Font-Underline="true"
                                Font-Size="Small" style="color:black"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbCategoryC" runat="server" Text="Category" Font-Bold="true" Font-Underline="true"
                                Font-Size="Small" style="color:black"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbSubCategoryC" runat="server" Text="Sub-Category" Font-Bold="true"
                                Font-Underline="true" Font-Size="Small" style="color:black"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ListBox ID="lbCallType" runat="server" Height="250px" Width="265px" OnSelectedIndexChanged="lbCallType_SelectedIndexChanged"
                                AutoPostBack="true" ToolTip="Call Type" BackColor="White"></asp:ListBox>
                        </td>
                        <td>
                            <asp:ListBox ID="lbCategory" runat="server" Height="250px" Width="265px" AutoPostBack="true"
                                OnSelectedIndexChanged="lbCategory_SelectedIndexChanged" ToolTip="Category" BackColor="White">
                            </asp:ListBox>
                        </td>
                        <td>
                            <asp:ListBox ID="lbSubCategory" runat="server" Height="250px" Width="265px" OnSelectedIndexChanged="lbSubCategory_SelectedIndexChanged"
                                AutoPostBack="true" ToolTip="Sub-Category" BackColor="White"></asp:ListBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left" class="auto-style1">
                            <asp:Label ID="lbMoreInformationC" runat="server" Text="More Information" Font-Bold="true"
                                Font-Underline="true" Font-Size="Small" style="color:black"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 20px">
                        <td colspan="3" align="left">
                            &nbsp;&nbsp;<asp:HyperLink ID="hplMoreInformation" runat="server"></asp:HyperLink>
                            <%-- <asp:TextBox ID="txtMoreInformation" runat="server" Width="580px"></asp:TextBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left">
                            <asp:Label ID="lbScript" runat="server" Text="Scripting" Font-Bold="true" Font-Underline="true"
                                Font-Size="Small" style="color:black"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:TextBox ID="txtScript" runat="server" Width="798px" TextMode="MultiLine" Height="60px"
                                ReadOnly="true" ToolTip="Scripting"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left">
                            <asp:Label ID="lbNote" runat="server" Text="Notes" Font-Bold="true" Font-Underline="true"
                                Font-Size="Small" style="color:black"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:TextBox ID="txtNote" runat="server" Width="798px" TextMode="MultiLine" Height="60px"
                                ToolTip="Notes" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="height: 10px">
                        <td colspan="3">
                            <asp:HiddenField ID="hfMaxWoringDay" runat="server" />
                            <asp:HiddenField ID="hfetc" runat="server" />
                            <asp:HiddenField ID="hfdrc" runat="server" />
                            <asp:HiddenField ID="hfurl" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            
                            <asp:Button  ID="btnOk" runat="server" Text="Ok" CssClass="Button ms-crm-Button" OnClick="btnOk_Click" Height="25px" />
                            &nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="Button ms-crm-Button"
                                OnClick="btnCancel_Click" Height="25px" />
                        </td>
                    </tr>
                </table>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
