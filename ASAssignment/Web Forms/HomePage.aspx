<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="ASAssignment.Web_Forms.HomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label ID="lbl_message" runat="server" Font-Bold="True" Font-Size="XX-Large"></asp:Label>
        <br />
        <br />
        <asp:Button ID="btn_logout" runat="server" OnClick="btn_logout_Click" Text="Logout" Visible="false"/>
    </form>
</body>
</html>
