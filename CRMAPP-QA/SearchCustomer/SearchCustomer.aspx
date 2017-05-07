<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchCustomer.aspx.cs" Inherits="SearchCustomer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; CHARSET=UTF-8" />
    <title>Search Customer</title>
    <style type="text/css">
        .gvCZID {
            border-spacing: 3px;
            border-collapse: separate;
        }

            .gvCZID > tbody > tr > th,
            .gvCZID > tbody > tr > td {
                border: 1px ridge black;
                border-color: #bababa;
                padding: 3px;
            }
    </style>
    <script type="text/javascript">
        function CheckOne(obj) {//debugger;
            var grid = obj.parentNode.parentNode.parentNode;
            var row = obj.parentNode.parentNode;
            var inputs = grid.getElementsByTagName("input");
            if (obj.checked) { document.getElementById("btnOk").disabled = false; row.style.backgroundColor = "rgb(167, 205, 240)"; }
            else {
                document.getElementById("btnOk").disabled = true;
                
                    row.style.backgroundColor = "rgb(224, 255, 255)"
              
            }
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    if (obj.checked && inputs[i] != obj && inputs[i].checked) {
                        inputs[i].checked = false;
                        
                            inputs[i].parentNode.parentNode.style.backgroundColor = "rgb(224, 255, 255)";
                        
                    }
                    //inputs[i].parentNode.parentNode.style.backgroundColor = "#FFFFFF";
                } //-- if (obj.checked && inputs[i] != obj && inputs[i].checked) --
            } //-- if (inputs[i].type == "checkbox") --
        } //-- for (var i = 0; i < inputs.length; i++) --        
        //-- function CheckOne(obj) --

        function btnClearClick() {
            document.getElementById("<%=textSearch.ClientID %>").value = "";
           return true;
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <label style="margin-right: 50px;">Search</label>
            <asp:DropDownList ID="ddlSearchView" runat="server" Width="200"
                AutoPostBack="true">
                <asp:ListItem Value="1" Text="Customer Name"></asp:ListItem>
                <asp:ListItem Value="2" Text="Customer Client Number"></asp:ListItem>
                <%--<asp:ListItem Value="8" Text="Policy Number"></asp:ListItem>
        <asp:ListItem Value="9" Text="Plate Number"></asp:ListItem>
        <asp:ListItem Value="10" Text="Chassis Number"></asp:ListItem>  --%>
            </asp:DropDownList>
            <asp:TextBox ID="textSearch" runat="server" Width="300px"></asp:TextBox>
            <asp:Button ID="btnSearchOk" runat="server" Text="Search" OnClick="onBtn_Search_Click" />
                <asp:Button  ID="btnSearchCancle" runat="server" Text="Clear" OnClientClick="return btnClearClick();" />
            <br />
            <br />
            <div align="right">
                
                
            </div>
        </div>
        <section style="height: 50px;"></section>
        <div></div>

            <br />
        <asp:GridView ID="gvCZID" AutoGenerateColumns="false" runat="server" Width="100%"  AllowSorting="True" PageSize="10"
            GridLines="none" AllowPaging="True" EmptyDataText="No Contact records are available in this view" 
            DataKeyNames="ContactId" ShowHeader="true" CellPadding="-1" CssClass="gvCZID" HeaderStyle-BackColor="PaleTurquoise" OnSelectedIndexChanged="gvCZID_SelectedIndexChanged"
            >

<HeaderStyle BackColor="PaleTurquoise"></HeaderStyle>

            <PagerSettings Visible="False" />
            <RowStyle BackColor="LightCyan"
                ForeColor="black" />
            
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkCZIDAuthorizedPerson" runat="server" Enabled='<%# Eval("enabled") %>' onclick="CheckOne(this)" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Customer Name">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDCustomerName" runat="server" Text='<%#Eval("CustomerName")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Customer Client ID">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDCustomerClientID" runat="server" Text='<%#Eval("CustomerClientNo")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CItizen ID.">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDCitizenID" runat="server" Text='<%#Eval("CitizenID") %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Customer VIP">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDVIP" runat="server" Text='<%#Eval("VIP")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Customer Privilege">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDPrivilege" runat="server" Text='<%#Eval("Privilege")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Customer Sensitive">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDSensitive" runat="server" Text='<%#Eval("Sensitive")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
               
                <%--<asp:TemplateField HeaderText="Language">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDLanguage" runat="server" Text='<%#Eval("Language")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Citizen Id">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDcitizenId" runat="server" Text='<%#Eval("CitizenID")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CustomerSensitive">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDCustomerSensitive" runat="server" Text='<%#Eval("CustomerSensitive")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CustomerPrivilege">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDCustomerPrivilege" runat="server" Text='<%#Eval("CustomerPrivilege")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PolisyClientID">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDPolisyClientID" runat="server" Text='<%#Eval("PolisyClientID")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PolisySecurityNum">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDPolisySecurityNum" runat="server" Text='<%#Eval("PolisySecurityNum")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>--%>
            </Columns>
        </asp:GridView>
        <br />
        <br />
        <asp:Label ID="lbTotal" runat="server" Text="" /><br />
        <asp:HiddenField ID="hfTotal" runat="server" Value="0" />
        <asp:HiddenField ID="hiddenCTI" runat="server" />
        <div align="Left">
              <asp:ImageButton ID="imgGrid_FirstPage" runat="server" ToolTip="Load First Page"
          ImageUrl="~/_imgs/page_FL0.gif" ImageAlign="AbsMiddle" CssClass="toolbarbutton"
            Style="width: 8px; cursor: pointer;" OnClick="imgGrid_FirstPage_Click" />
           <asp:ImageButton ID="imgGrid_Previous" runat="server" ToolTip="Previous Page" ImageUrl="~/_imgs/page_L0.gif"
           ImageAlign="AbsMiddle" CssClass="toolbarbutton" Style="cursor: pointer;" OnClick="imgGrid_Previous_Click" />
           &nbsp;Page
            <asp:Label ID="lbGrid_PageIndex" runat="server" Text="1" />
               &nbsp;<asp:ImageButton ID="imgGrid_Next" runat="server" ToolTip="Next Page" ImageUrl="~/_imgs/page_R0.gif"
            ImageAlign="AbsMiddle" CssClass="toolbarbutton" Style="cursor: pointer;" OnClick="imgGrid_Next_Click" />
            &nbsp;&nbsp;
            <asp:Button ID="btnOk" runat="server" Text="Submit" OnClick="onBtn_Ok_Click" />
            <asp:Button ID="btnCancle" runat="server" Text="Exit" OnClientClick="window.close();"/>

            
        </div>
    </form>

</body>
</html>
