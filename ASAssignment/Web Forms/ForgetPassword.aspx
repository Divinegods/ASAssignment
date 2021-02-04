<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgetPassword.aspx.cs" Inherits="ASAssignment.Web_Forms.ForgetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 25px;
        }
        .auto-style2 {
            height: 23px;
        }
        .auto-style3 {
            height: 25px;
            width: 158px;
            margin-left: 40px;
        }
        .auto-style4 {
            height: 25px;
            width: 158px;
            margin-left: 80px;
        }
        .auto-style5 {
            height: 23px;
            width: 158px;
        }
        .auto-style6 {
            height: 25px;
            width: 158px;
        }
        .auto-style7 {
            width: 100%;
        }
        .auto-style8 {
            height: 23px;
            width: 128px;
        }
        .auto-style9 {
            height: 25px;
            width: 128px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="auto-style7">
                <tr>
                    <td class="auto-style5">
                        <asp:Label ID="lbl_email" runat="server" Text="Email:"></asp:Label>
                    </td>
                    <td class="auto-style8">
                        <asp:TextBox ID="tb_email" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style2">
                        <asp:Label ID="lbl_emailchecker" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style6">
                        <asp:Label ID="lbl_oldpassword" runat="server" Text="Old Password:"></asp:Label>
                    </td>
                    <td class="auto-style9">
                        <asp:TextBox ID="tb_oldpassword" runat="server" TextMode="Password"></asp:TextBox>
                    </td>
                    <td class="auto-style1">
                        <asp:Label ID="lbl_oldpasswordchecker" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="lbl_newpassowrd" runat="server" Text="New Password:"></asp:Label>
                    </td>
                    <td class="auto-style9">
                        <asp:TextBox ID="tb_newpassword" runat="server" TextMode="Password"></asp:TextBox>
                    </td>
                    <td class="auto-style1">
                        <asp:Label ID="lbl_newpasswordchecker" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <asp:Label ID="lbl_cfmnewpassword" runat="server" Text="Confirm New Password:"></asp:Label>
                    </td>
                    <td class="auto-style9">
                        <asp:TextBox ID="tb_cfmnewpassword" runat="server" TextMode="Password"></asp:TextBox>
                    </td>
                    <td class="auto-style1">
                        <asp:Label ID="lbl_cfmnewpasswordchecker" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lbl_message" runat="server" Font-Bold="True"></asp:Label>
            <br />
            <asp:Button ID="lbl_reset" runat="server" OnClick="lbl_reset_Click" Text="Reset Password" />
            <br />
            <br />
            <asp:Button ID="lbl_return" runat="server" OnClick="lbl_return_Click" Text="Return to Login Page" />
        </div>
    </form>
</body>
</html>
