<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ASAssignment.Web_Forms.Login1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://www.google.com/recaptcha/api.js?render=6LfTH0UaAAAAAEw6h0HZjOWmx_K9bru7JvPIFHhL"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width:100%;">
                <tr>
                    <td class="modal-sm" style="height: 20px; width: 88px">
                        <asp:Label ID="lbl_email" runat="server" Text="Email:"></asp:Label>
                    </td>
                    <td style="height: 20px">
                        <asp:TextBox ID="tb_email" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="modal-sm" style="width: 88px">
                        <asp:Label ID="lbl_password" runat="server" Text="Password"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tb_password" runat="server" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                </table>
            <br />
            <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
            <asp:label ID="lbl_message" runat="server" Font-Bold="True" Visible="False"></asp:label>
            <br />
                        <asp:Button ID="btn_login" runat="server" OnClick="btn_login_Click" Text="Login" />
                    <br />
                    <br />
            <asp:Button ID="btn_forgotpassword" runat="server" OnClick="btn_forgotpassword_Click" Text="Forgot Password" />
            <br />
            <br />
            <asp:Button ID="btn_register" runat="server" OnClick="lbl_register_Click" Text="Register" />
            <br />
            <%--<asp:Label ID="lbl_gScore" runat="server" Text="Label" Visible="false"></asp:Label>--%>
            <br />
        </div>
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LfTH0UaAAAAAEw6h0HZjOWmx_K9bru7JvPIFHhL', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            })
        })
    </script>
</body>
</html>
