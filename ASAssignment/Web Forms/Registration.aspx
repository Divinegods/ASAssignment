<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="ASAssignment.Web_Forms.Registration1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="JQuery/jQuery-3.5.1.min.js"></script>
    <script>
        //Date Picker
        /*$(document).ready(function () {
            $("#tb_dob").datepicker();
        });*/
        //Password Valication
        function validate() {
            var str = document.getElementById('<%=tb_password.ClientID%>').value;
            var password = document.getElementById("tb_password").value;
            var confirm_password = document.getElementById("tb_cfmpassword").value;
            if (str.length == 0) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password cannot be empty";
                document.getElementById("lbl_pwdchecker").style.color = "Black";
                return ("empty");
            }
            else if (str.length < 8) {
                //document.getElementById("lbl_pwdchecker").innerHTML = password + confirm_password;
                document.getElementById("lbl_pwdchecker").innerHTML = "Password Length Must be at least 8 Characters";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("too_short");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 number";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_number");
            }
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 LowerCase";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_lowercase");
            }
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 UpperCase";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_uppercase");
            }
            else if (str.search(/[^a-zA-Z0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 Special Character";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_specialcharacter");
            }
            document.getElementById("lbl_pwdchecker").innerHTML = "Password is Secure!";
            document.getElementById("lbl_pwdchecker").style.color = "Green";

            if (password != confirm_password) {
                document.getElementById("lbl_cfmpwdchecker").innerHTML = "Password does not match!";
                document.getElementById("lbl_cfmpwdchecker").style.color = "Red";
                return ("no_match");
            }
            document.getElementById("lbl_cfmpwdchecker").innerHTML = "Password matches!";
            document.getElementById("lbl_cfmpwdchecker").style.color = "Green";
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 207px;
            height: 26px;
        }
        .auto-style2 {
            width: 55px;
            height: 26px;
        }
        .auto-style3 {
            height: 26px;
        }
        .auto-style4 {
            width: 207px;
            height: 25px;
        }
        .auto-style5 {
            width: 55px;
            height: 25px;
        }
        .auto-style6 {
            height: 25px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width:100%;">
                <tr>
                    <td class="modal-lg" style="width: 207px">
                        <asp:Label ID="lbl_fname" runat="server" Text="First Name:"></asp:Label>
                    </td>
                    <td class="modal-sm" style="width: 55px">
                        <asp:TextBox ID="tb_fname" runat="server" Width="144px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_fnamechecker" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="modal-lg" style="width: 207px">
                        <asp:Label ID="lbl_lname" runat="server" Text="Last Name:"></asp:Label>
                    </td>
                    <td class="modal-sm" style="width: 55px">
                        <asp:TextBox ID="tb_lname" runat="server" Width="144px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_lnamechecker" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="modal-lg" style="width: 207px">
                        <asp:Label ID="lbl_dob" runat="server" Text="Birth Date:"></asp:Label>
                    </td>
                    <td class="modal-sm" style="width: 55px">
                        <asp:TextBox ID="tb_dob" runat="server" type="date" Width="147px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_dobchecker" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="lbl_email" runat="server" Text="Email Address:"></asp:Label>
                    </td>
                    <td class="auto-style2">
                        <asp:TextBox ID="tb_email" runat="server" Width="144px"></asp:TextBox>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="lbl_emailchecker" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="modal-lg" style="width: 207px">
                        <asp:Label ID="lbl_password" runat="server" Text="Password:"></asp:Label>
                    </td>
                    <td class="modal-sm" style="width: 55px">
                        <asp:TextBox ID="tb_password" runat="server" onkeyup="javascript:validate()" TextMode="Password" Width="144px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_pwdchecker" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <asp:Label ID="lbl_cfmpassword" runat="server" Text="Confirm Password:"></asp:Label>
                    </td>
                    <td class="auto-style5">
                        <asp:TextBox ID="tb_cfmpassword" runat="server" onkeyup="javascript:validate()" TextMode="Password" Width="144px"></asp:TextBox>
                    </td>
                    <td class="auto-style6">
                        <asp:Label ID="lbl_cfmpwdchecker" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="modal-lg" style="width: 207px">
                        <asp:Label ID="lbl_ccno" runat="server" Text="Credit Card Number:"></asp:Label>
                    </td>
                    <td class="modal-sm" style="width: 55px">
                        <asp:TextBox ID="tb_ccno" runat="server" style="margin-left: 0" Width="144px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_ccnochecker" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="modal-lg" style="width: 207px; height: 20px">
                        <asp:Label ID="lbl_ccexpdate" runat="server" Text="Credit Card Expiry Date:"></asp:Label>
                    </td>
                    <td style="height: 20px; width: 55px">
                        <asp:TextBox ID="tb_ccexpdate" runat="server" type="date" Width="147px"></asp:TextBox>
                    </td>
                    <td style="height: 20px">
                        <asp:Label ID="lbl_ccexpdatechecker" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="modal-lg" style="width: 207px; height: 20px">
                        <asp:Label ID="lbl_cccvv" runat="server" Text="Credit Card CVV:"></asp:Label>
                    </td>
                    <td style="height: 20px; width: 55px">
                        <asp:TextBox ID="tb_cccvv" runat="server" Width="144px"></asp:TextBox>
                    </td>
                    <td style="height: 20px">
                        <asp:Label ID="lbl_cccvvchceker" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lbl_checker" runat="server" Font-Bold="True"></asp:Label>
            <br />
            <asp:Button ID="btn_createacc" runat="server" Text="Create Account" OnClick="btn_createacc_Click" />
        </div>
    </form>
</body>
</html>
