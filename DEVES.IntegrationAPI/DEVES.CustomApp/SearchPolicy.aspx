<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchPolicy.aspx.cs" Inherits="SearchPolicy" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; CHARSET=UTF-8" />
    <title>Search Policy</title>
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="/vendor-resource/font-awesome/css/font-awesome.min.css">

    <link rel="stylesheet" href="/vendor-resource/bootstrap/css/bootstrap.min.css" crossorigin="anonymous">

    <!-- Optional theme -->
    <link rel="stylesheet" href="/vendor-resource/bootstrap/css/bootstrap-theme.min.css"  crossorigin="anonymous">
    
    <link rel="stylesheet" href="/vendor-resource/css/main.css"  crossorigin="anonymous">

    <script src="/vendor-resource/jquery/dist/jquery.js"  type="text/javascript"></script>


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
        $(document).ready(function () {
            var resize = function () {
                $("#gridview").height($(window).innerHeight() - 250);
                $("#gridview").css("overflow-y", "scroll");
            }
            $(window).resize(resize);
            resize();
        });


        function btnClearClick() {
            document.getElementById("<%=textSearch.ClientID %>").value = "";
           return true;
        }
        
    </script>
    <style type="text/css">
        .col-xs-1 {
            margin-left: 17px;
        }
        .ratiospace input[type=radio] {
            margin-left: 8px;
            margin-right: 8px;
        }
    </style>
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
        <div class="navbar navbar-default form-action-bar" >
      <div class="row">
          <div class="col-xs-1">Search</div>
      <div class="col-xs-11">


            <asp:DropDownList ID="ddlSearchView" runat="server" Width="200"
                AutoPostBack="true">
                <asp:ListItem Value="1" Text="เลขที่กรมธรรม์"></asp:ListItem>
                <asp:ListItem Value="2" Text="เลขตัวถัง"></asp:ListItem>
                <asp:ListItem Value="3" Text="เลขทะเบียนรถ" Selected="True"></asp:ListItem>
                <asp:ListItem Value="4" Text="เลขประจำตัวประชาชน"  Enabled ="false"></asp:ListItem>
                <asp:ListItem Value="5" Text="เจ้าของกรมธรรม์"></asp:ListItem>
                <asp:ListItem Value="6" Text="เบอร์โทรศัพท์" Enabled ="false"></asp:ListItem>
                <asp:ListItem Value="7" Text="เลขหน้าตารางกรมธรรม์"></asp:ListItem>
                <asp:ListItem Value="8" Text="Insurance Card"></asp:ListItem>
                <%--<asp:ListItem Value="8" Text="Policy Number"></asp:ListItem>
        <asp:ListItem Value="9" Text="Plate Number"></asp:ListItem>
        <asp:ListItem Value="10" Text="Chassis Number"></asp:ListItem>  --%>
            </asp:DropDownList>
            <asp:TextBox ID="textSearch" runat="server" Width="300px"></asp:TextBox>
            <asp:Button ID="btnSearchOk" runat="server" Text="Search" OnClick="onBtn_Search_Click" />
            <asp:Button ID="btnSearchCancle" runat="server" Text="Clear"  OnClientClick="return btnClearClick();"/>
            </div> 
          <div class="col-xs-1"><asp:RadioButtonList ID="motorTypeValue"  runat="server" 
              OnSelectedIndexChanged="changMotorTypeauto" AutoPostBack ="true"
               CssClass =" ratiospace" Width="188px" style="margin-top: 12px" >
            <asp:ListItem Value ="0" Selected="True" >Motor</asp:ListItem>
            <asp:ListItem Value ="1" style="margin-right:20px">Non-Motor</asp:ListItem>
              </asp:RadioButtonList></div>
          <div class="col-xs-11">
              

          </div>

      </div>
            </div>
        

        <div></div>

        <br />
        <div id="gridview">
            <asp:GridView ID="gvCZID" AutoGenerateColumns="false" runat="server" Width="100%"  AllowSorting="True"  PageSize="10"
                      GridLines="none" AllowPaging="true" EmptyDataText="No Contact records are available in this view"  OnRowDataBound="ChangeRowColor"
                      DataKeyNames="pfc_policy_additionalId" Visible="false" ShowHeader="true" CellPadding="-1"  UseAccessibleHeader="true"
                      CssClass="table table-striped table-bordered table-hover " 
            >

<PagerSettings Visible="False" />


            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="checkboxPolicy" runat="server" Enabled='<%#Eval("enabled")%>' onclick="CheckOne(this)" />
                    </ItemTemplate>
                </asp:TemplateField>
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

</div></div>
    

</body>
</html>