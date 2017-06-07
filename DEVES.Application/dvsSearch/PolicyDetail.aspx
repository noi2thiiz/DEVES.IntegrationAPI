<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PolicyDetail.aspx.cs" Inherits="PolicyDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <nav id="page-nav-header" class="navbar navbar-default navbar-fixed-top">
    <div class="container">
        <div class="navbar-header">
            <a class="navbar-brand" href="#"><img src="/vendor-resource/images/theme_navbarlogo.png"></a>
        </div>
        <div class="pull-right" style="font-size: larger; margin-top: 15px; color: white;">
            Search Policy
        </div>

    </div>
</nav>
    <div id="main-container" class="container" >
<div class="row">
    <form id="form1" runat="server">
    <div>
            <div id="gridview">
            <asp:GridView ID="gvCZID" AutoGenerateColumns="false" runat="server" Width="100%"  AllowSorting="True"  PageSize="10"
                      GridLines="none" AllowPaging="true" EmptyDataText="No Contact records are available in this view"
                      DataKeyNames="pfc_policy_additionalId" Visible="false" ShowHeader="true" CellPadding="-1"  UseAccessibleHeader="true"
                      CssClass="table table-striped table-bordered table-hover "
            >

<PagerSettings Visible="False" />


            <Columns>
                <asp:TemplateField HeaderText="ชื่อผู้เอาประกัน">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: left;">
                            <asp:Label ID="lbPNUMFulltname" runat="server" Text='<%#Eval("insuredFullName")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ทะเบียนรถ">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMpolicycarregisterno" runat="server" Text='<%#Eval("policyCarRegisterNo")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="เลขที่กรมธรรม์">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMpolicynumber" runat="server" Text='<%#Eval("policyNo")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="เริ่ม">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMpolicyeffectivedate" runat="server" Text='<%#Eval("policyEffectiveDate")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="สิ้นสุด">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMpolicyexpirydate" runat="server" Text='<%#Eval("policyExpiryDate")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ประเภทกรมธรรม์">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMpolicycontracttype" runat="server" Text='<%#Eval("policyContractType")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Campaign">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMcampaign" runat="server" Text='<%#Eval("Campaign")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ฝ่ายที่ดูแล">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMhandledept" runat="server" Text='<%#Eval("handleDept")%>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="เจ้าหน้าที่ที่ดูแล">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMhandlefullname" runat="server" Text='<%#Eval("handleFullName")%>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Source">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMsource" runat="server" Text='<%#Eval("Source")%>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Agent">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMagent" runat="server" Text='<%#Eval("Agent")%>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Motor Add On">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMmotoraddon" runat="server" Text='<%#Eval("motorAddOn")%>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMstatus" runat="server" Text='<%#Eval("Status")%>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>               
                <asp:TemplateField HeaderText="customerid" Visible="false">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMcontactid" runat="server" Text='<%#Eval("insuredId")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="customertype" Visible="false">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMcontacttype" runat="server" Text='<%#Eval("insuredIdIdType")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="policyaddid" Visible="false" >
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMpolicyadd" runat="server" Text='<%#Eval("pfc_policy_additionalId")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="policyaddname" Visible="false">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMpolicyaddname" runat="server" Text='<%#Eval("pfc_policy_additional_name")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="policy status code" Visible="false">
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMpolicystatuscode" runat="server" Text='<%#Eval("pfc_policy_status")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="สถานะกรมธรรม์" >
                    <ItemTemplate>
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lbPNUMpolicystatus" runat="server" Text='<%#Eval("pfc_policy_statusName")  %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        <br />
            </div>
                    <div align ="right"> 
                    <asp:Button ID="btnCancle" runat="server" Text="Close" OnClientClick="window.close();" Width="95px" align="right"/>
                        </div>
    </div>
        <asp:Label ID="lbTotal" runat="server" Text="" /><br />
                <asp:HiddenField ID="hfTotal" runat="server" Value="0" />
                <asp:HiddenField ID="hiddenCTI" runat="server" />
    </form>
    </div>
            <div></div>
    </div>
</body>
</html>
