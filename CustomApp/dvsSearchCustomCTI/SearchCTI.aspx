<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchCTI.aspx.cs" Inherits="SearchCTI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; CHARSET=UTF-8" />
    <title>CustomCTI</title>
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

        .pageHide {
            display: none;
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
                if (row.rowIndex % 2 == 0)
                    row.style.backgroundColor = "rgb(175,238,238)";
                else {
                    row.style.backgroundColor = "rgb(224, 255, 255)"
                }
            }
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    if (obj.checked && inputs[i] != obj && inputs[i].checked) {
                        inputs[i].checked = false;
                        inputs[i].parentNode.parentNode.style.backgroundColor = "#FFFFFF";
                    }
                    //inputs[i].parentNode.parentNode.style.backgroundColor = "#FFFFFF";
                } //-- if (obj.checked && inputs[i] != obj && inputs[i].checked) --
            } //-- if (inputs[i].type == "checkbox") --
        } //-- for (var i = 0; i < inputs.length; i++) --        
        //-- function CheckOne(obj) --
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <label style="margin-right: 50px;">Search:&nbsp; </label>&nbsp;<asp:DropDownList ID="ddlSearchView" runat="server" Width="200" OnSelectedIndexChanged="ddlSearchView_SelectedIndexChanged"
                AutoPostBack="true">
                <asp:ListItem Value="1" Text="Citizen Id"></asp:ListItem>
                <asp:ListItem Value="2" Text="Firstname" Enabled="False"></asp:ListItem>
                <asp:ListItem Value="3" Text="Lastname" Enabled="False"></asp:ListItem>
                <asp:ListItem Value="4" Text="Fullname" Selected="True"></asp:ListItem>
                <asp:ListItem Value="5" Text="Telephone"></asp:ListItem>
                <asp:ListItem Value="6" Text="Policy Number"></asp:ListItem>
                <asp:ListItem Value="7" Text="Plate Number"></asp:ListItem>
                <asp:ListItem Value="8" Text="Chassis Number"></asp:ListItem> 
                <asp:ListItem Value="9" Text="Case Number"></asp:ListItem>
                <asp:ListItem Value="10" Text="เลขที่รับแจ้ง"></asp:ListItem>
                <asp:ListItem Value="11" Text="Claim Number"></asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="textSearch" runat="server" Width="300px"></asp:TextBox>
            &nbsp;
            <asp:Button ID="Button1" runat="server" Text="Search" OnClick="onBtn_Search_Click" style="margin-right:10px" />
            <asp:Button ID="Button2" runat="server" Text="Clear" OnClick="onBtn_Clear_Click" style="margin-right:10px"/>
            <asp:Button ID="btnNewRecord" runat="server" Text="New" OnClick="onBtn_New_Click" style="margin-right:10px"/>
            <asp:Button ID="btnUnknowndata" runat="server" TexT="ไม่พบข้อมูล" OnClick="onBtn_unknown_Click" />
            <br />
            Search Policy:
            <asp:DropDownList ID="ddlSearchProdGroup" runat="server" Width="250" AutoPostBack="true" Enabled="false">
                <asp:ListItem Value="99" Text="All" Selected="True"></asp:ListItem>
                <asp:ListItem Value="1" Text="ประกันภัยทางทะเล"></asp:ListItem>
                <asp:ListItem Value="2" Text="ประกันภัยเบ็ดเตล็ด"></asp:ListItem>
                <asp:ListItem Value="3" Text="ประกันภัยยานยนต์"></asp:ListItem>
                <asp:ListItem Value="4" Text="ประกันอัคคีภัย"></asp:ListItem>
            </asp:DropDownList>
            <asp:CheckBox ID="chkExpired" runat="server" Checked="true" Text="checkifexpire" Enabled="false" />
            <br />
            <%--<div align="right">
                
            </div>--%>
        </div>
        <%--Citizen ID:
        <asp:Label ID="lbCZID" runat="server" Text=" - " Width="20%"></asp:Label>
        <section style="height: 50px;"></section>--%>
        Calller ID:
        <asp:Label ID="lbCallerID" runat="server" Text=" - " Width="20%"></asp:Label>
        <section style="height: 50px;"></section>
        <asp:HiddenField id="hftelnum" runat="server"/>
        <asp:HiddenField ID="hfivrlink" runat="server"/>
        <asp:HiddenField ID="hfhandcode" runat="server" />
        <asp:HiddenField ID="hfvoiceurl" runat="server" />
        <div ID="CreateSection" visible="false" runat="server">
        Create 
        <br />
        <br />
        Firstname : 
        <asp:TextBox ID="textCreateName" runat="server" Width="300px" style="margin-right:150px;"></asp:TextBox>
        Lastname :
        <asp:TextBox ID="textCreateLastName" runat="server" Width="300px"></asp:TextBox>
        <br />
        <br />
        Citizen ID:
        <asp:TextBox ID="numberCreateCitizenID" type="number" runat="server" Width="300px" style="margin-right:150px;"></asp:TextBox>
        Phone Number:
        <asp:TextBox ID="numberCreatePhonenumber" type="number" runat="server" Width="300px"></asp:TextBox>
        <br />
        <div align="right">
            <asp:Button ID="btnCreateRecord" runat="server" OnClick="onBtn_CreateRecord_Click" Text="Create"/>
            <asp:Button ID="btnCancleCreate" runat="server" OnClick="onBtn_CancleCreate_Click" Text="Cancle"/>
        </div>
        </div>
        <asp:GridView ID="gvCZID" AutoGenerateColumns="false" runat="server" Width="100%" PageSize="10"
            GridLines="none" AllowPaging="true" EmptyDataText="No Contact records are available in this view"
            DataKeyNames="contactid" Visible="false" ShowHeader="true" CellPadding="-1" CssClass="gvCZID" HeaderStyle-BackColor="PaleTurquoise">
            <RowStyle BackColor="LightCyan"
                ForeColor="black" />
            <AlternatingRowStyle BackColor="PaleTurquoise"
                ForeColor="black" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="checkboxContact" runat="server" Enabled='<%# Eval("enabled") %>' onclick="CheckOne(this)" />
                    </ItemTemplate>
                </asp:TemplateField>
              <%--  <asp:TemplateField HeaderText="Contact Id">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDcontactid" runat="server" Text='<%#Eval("ContactId")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <%--<asp:TemplateField HeaderText="Agent Code">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDProducercode" runat="server" Text='<%#Eval("ProducerCode")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Person Id">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDPersonId" runat="server" Text='<%#Eval("PersonId")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fullname Thai">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDfullnameTH" runat="server" Text='<%#Eval("FullNameThai") %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="VIP">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDVIP" runat="server" Text='<%#Eval("VIP")  %>' />
                        </div>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="Language">
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
                <asp:TemplateField HeaderText="PhoneNumber">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCZIDPhoneNumber" runat="server" Text='<%#Eval("PhoneNumber")  %>' />
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
                </asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="PaleTurquoise"></HeaderStyle>
            <PagerSettings Visible="False" />
        </asp:GridView>
        <asp:GridView ID="gvPNUM" AutoGenerateColumns="false" runat="server" Width="100%" PageSize="10"
            GridLines="none" EmptyDataText="No Contact records are available in this view" 
            DataKeyNames="pfc_policyId" visible="false" ShowHeader="true" CssClass="gvCZID" CellPadding="-1"  HeaderStyle-BackColor="PaleTurquoise">
        <HeaderStyle BackColor="PaleTurquoise"></HeaderStyle>
            
            <RowStyle BackColor="LightCyan"
                ForeColor="black" />
            <AlternatingRowStyle BackColor="PaleTurquoise"
                ForeColor="black" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="checkboxPolicy" runat="server" Enabled='<%#Eval("enabled")%>' onclick="CheckOne(this)"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Policy Number">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMpolicynumber" runat="server" Text='<%#Eval("PolicyNumber")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Insurance Name">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMInsuredName" runat="server" Text='<%#Eval("InsuredName")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ProductType">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMproducttype" runat="server" Text='<%#Eval("ProdType")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Plate Province">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMplatejw" runat="server" Text='<%#Eval("RegProvince")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Plate Number">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMplatenumber" runat="server" Text='<%#Eval("RegNumber")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Start Date">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMstartdate" runat="server" Text='<%#Eval("Startdate")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="End Date">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMenddate" runat="server" Text='<%#Eval("Enddate")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Deduct">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMdeduct" runat="server" Text='<%#Eval("Deduct")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Risk">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMrisk" runat="server" Text='<%#Eval("RiskNumber")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ContactId" Visible="false">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMcontactid" runat="server" Text='<%#Eval("CustomerId")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ContactName" Visible="false">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMcontactname" runat="server" Text='<%#Eval("CustomerName")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Policy Additional" Visible="false">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMpolicyadd" runat="server" Text='<%#Eval("policyadd")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Policy Add Name" Visible="false">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMpolicyaddname" runat="server" Text='<%#Eval("policyaddname")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ContactType" Visible="false">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMcontacttype" runat="server" Text='<%#Eval("CustomerType")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:GridView ID="gvPlateNum" AutoGenerateColumns="false" runat="server" Width="100%" PageSize="10"
            GridLines="none" EmptyDataText="No Contact records are available in this view"
            DataKeyNames="contactid" visible="false" ShowHeader="true" CellPadding="-1"  HeaderStyle-BackColor="PaleTurquoise">
            <RowStyle BackColor="LightCyan"
                ForeColor="black" />
            <AlternatingRowStyle BackColor="PaleTurquoise"
                ForeColor="black" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="checkboxPlatenum" runat="server" Text='<%#Eval("enabled")%>' onclick="CheckOne(this)"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Policy Number">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMpolicynumber" runat="server" Text='<%#Eval(" ")  %>' />
                        </div>
                    </ItemTemplate>
               </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:GridView ID="gvCase" AutoGenerateColumns="false" runat="server" Width="100%" PageSize="10"
            GridLines="none" AllowPaging="true" EmptyDataText="No Contact records are available in this view"
            DataKeyNames="IncidentId" Visible="false" ShowHeader="true" CellPadding="-1" CssClass="gvCZID" HeaderStyle-BackColor="PaleTurquoise">
            <RowStyle BackColor="LightCyan"
                ForeColor="black" />
            <AlternatingRowStyle BackColor="PaleTurquoise"
                ForeColor="black" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="checkboxCase" runat="server" Enabled='<%#Eval("enabled")%>' onclick="CheckOne(this)" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Case Number">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCASEcasenumber" runat="server" Text='<%#Eval("CaseNumber")%>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Claim Notification">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCASEclaimnoti" runat="server" Text='<%#Eval("ClaimNoti")%>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Case Title">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCASEcasetitle" runat="server" Text='<%#Eval("CaseTitle")%>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Case Type">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCASEcasetype" runat="server" Text='<%#Eval("CaseType")%>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Category">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCASEcategory" runat="server" Text='<%#Eval("Category")%>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subcategory">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCASEsubcategory" runat="server" Text='<%#Eval("SubCategory")%>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Case VIP">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCASEcasevip" runat="server" Text='<%#Eval("CaseVIP")%>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Notification Date">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCASEnotidate" runat="server" Text='<%#Eval("NotiDate")%>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="PaleTurquoise"></HeaderStyle>
            <PagerSettings Visible="False" />
        </asp:GridView>
        <asp:GridView ID="gvClaim" AutoGenerateColumns="false" runat="server" Width="100%" PageSize="10"
            GridLines="none" EmptyDataText="No Contact records are available in this view" AllowPaging="true" 
            DataKeyNames="pfc_policyId" visible="false" ShowHeader="true" CssClass="gvCZID" CellPadding="-1"  HeaderStyle-BackColor="PaleTurquoise">
            <HeaderStyle BackColor="PaleTurquoise"></HeaderStyle>
            <PagerSettings Visible="False" />
            <RowStyle BackColor="LightCyan"
                ForeColor="black" />
            <AlternatingRowStyle BackColor="PaleTurquoise"
                ForeColor="black" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="checkboxClaim" runat="server" Text='<%#Eval("enabled")%>' onclick="CheckOne(this)"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Policy Number">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCLAIMpolicynumber" runat="server" Text='<%#Eval("PolicyNumber")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Insurance Name">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCLAIMinsurancename" runat="server" Text='<%#Eval("InsuredName")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ProductType">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCLAIMproducttype" runat="server" Text='<%#Eval("ProdType")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Plate Province">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCLAIMplatejw" runat="server" Text='<%#Eval("RegProvince")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Plate Number">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCLAIMplatenumber" runat="server" Text='<%#Eval("RegNumber")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Start Date">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCLAIMstartdate" runat="server" Text='<%#Eval("Startdate")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="End Date">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCLAIMenddate" runat="server" Text='<%#Eval("Enddate")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Claim Number">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCLAIMclaimnumber" runat="server" Text='<%#Eval("ClaimNumber")%>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Claim Open Date">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCLAIMclaimopendate" runat="server" Text='<%#Eval("ClaimOpenDate")%>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date of Loss">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbCLAIMdateofloss" runat="server" Text='<%#Eval("DateofLoss")%>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>


        <br />
        <br />
        <div>
            <asp:Label ID="lbTotal" runat="server" Text="1-50 of 500+ (1 select)" />
            <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
        </div>
        <div id="crm ageInfo" class="ms-crm-List-Paging" nowrap="nowrap">
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
        </div>
        <asp:HiddenField ID="hfTotal" runat="server" Value="0" />
        <asp:HiddenField ID="hiddenCTI" runat="server" />
        <asp:Button ID="btnOk" runat="server" Text="Submit" OnClick="onBtn_Ok_Click" />
        <asp:Button ID="btnCancle" runat="server" Text="Exit" />
    </form>

</body>
</html>
