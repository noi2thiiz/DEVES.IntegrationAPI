<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="simulate_ClaimRegistration.aspx.cs" Inherits="DEVES.IntegrationAPI.WebApi.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test API</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    Test ClaimRegistration API
        <br />
        <br />
        <br />
        Add Guid in this text box to test:&nbsp;
            <asp:TextBox ID="txt_guid" runat="server" OnTextChanged="txt_guid_TextChanged"></asp:TextBox>
        <br />
        or<br />
        Add ClaimID in this text box to test:
        <asp:TextBox ID="txt_ClaimID" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="label_guid" runat="server" Text=" "></asp:Label>
        <br />
        <asp:Label ID="label_ClaimID" runat="server" Text=" "></asp:Label>
    </div>
        <p>
            <asp:Button ID="button_send" runat="server" OnClick="button_send_Click" Text="Send" />
        </p>
    </form>
</body>
</html>
