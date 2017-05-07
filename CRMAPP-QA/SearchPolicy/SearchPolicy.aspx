<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchPolicy.aspx.cs" Inherits="SearchPolicy" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; CHARSET=UTF-8" />
    <title>SearchPolicy</title>
    <style type="text/css">
        .gvCZID {
            border-spacing: 3px;
            border-collapse: separate;
        }

            .gvCZID > tbody > tr > th,
            .gvCZID > tbody > tr > td {
                border: 1px ridge black;
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
                <asp:ListItem Value="1" Text="Policy No"></asp:ListItem>
                <asp:ListItem Value="2" Text="Customer Name"></asp:ListItem>
                <asp:ListItem Value="3" Text="Plate Number"></asp:ListItem>
                <asp:ListItem Value="4" Text="Province"></asp:ListItem>
                <asp:ListItem Value="5" Text="Chasis Number"></asp:ListItem>
                <asp:ListItem Value="6" Text="Barcode"></asp:ListItem>
                <%--<asp:ListItem Value="8" Text="Policy Number"></asp:ListItem>
        <asp:ListItem Value="9" Text="Plate Number"></asp:ListItem>
        <asp:ListItem Value="10" Text="Chassis Number"></asp:ListItem>  --%>
            </asp:DropDownList>
            <asp:TextBox ID="textSearch" runat="server" Width="300px"></asp:TextBox>
            <asp:Button ID="btnSearchOk" runat="server" Text="Search" OnClick="onBtn_Search_Click" />
            <asp:Button ID="btnSearchCancle" runat="server" Text="Clear"  OnClientClick="return btnClearClick();"/>
            <br />
            <br />
            
        </div>
        <section style="height: 50px;"></section>

        <br />
        <asp:GridView ID="gvCZID" AutoGenerateColumns="False" runat="server" Width="100%" PageSize="10"
            GridLines="none" AllowPaging="True" EmptyDataText="No Contact records are available in this view"
            DataKeyNames="pfc_policyId" ShowHeader="true" CellPadding="-1" CssClass="gvCZID" HeaderStyle-BackColor="PaleTurquoise" EnableSortingAndPagingCallbacks="False">

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
                <asp:TemplateField HeaderText="Policy No">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDPolicyNo" runat="server" Text='<%#Eval("PolicyNo")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Customer Name">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDCustomerName" runat="server" Text='<%#Eval("CustomerName")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Plate Number">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDPlateNo" runat="server" Text='<%#Eval("PlateNo")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Province">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDProvince" runat="server" Text='<%#Eval("Province")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Chassis Number">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDChassisNo" runat="server" Text='<%#Eval("ChassisNo")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Barcode">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDBarcode" runat="server" Text='<%#Eval("Barcode")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Insurance Card">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDInsuranceCard" runat="server" Text='<%#Eval("InsuranceCard")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Driver Name">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDDriverName" runat="server" Text='<%#Eval("Driver1")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Effective Date">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDEffectiveDate" runat="server" Text='<%#Eval("EffectiveDate")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Expire Date">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDExpireDate" runat="server" Text='<%#Eval("ExpireDate")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Policy Additional Number">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDPolicyAddId" runat="server" Text='<%#Eval("PolicyAddId")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Policy Additional Name">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDPolicyAddName" runat="server" Text='<%#Eval("PolicyAddName")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>

               <%-- <asp:TemplateField HeaderText="CustomerSensitive">
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
        <asp:Label ID="lbTotal" runat="server" Text="" />
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
