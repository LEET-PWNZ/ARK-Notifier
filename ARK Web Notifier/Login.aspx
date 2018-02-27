<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ARKWebNotifier.Login" MasterPageFile="~/Site.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table>
    <tr>
        <td>
            <span>Username or email address:</span>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtUsernameEmail"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <span>Password:</span>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtPassword" TextMode="Password"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:CheckBox runat="server" ID="chkRememberMe" Text="Remember Me" />
        </td>
    </tr>
        <tr>
            <td colspan="2">
                <span id="spanInfo" runat="server" style="color:#ee0000"></span>
            </td>
        </tr>
    <tr>
        <td colspan="2">
            <asp:Button runat="server" ID="btnLogin" Text="Submit" OnClick="btnLogin_Click" />
        </td>
    </tr>
    <tr>
        <td>
            <a href="Register.aspx">Register</a>
        </td>
        <td>
            <a href="ForgotPwd.aspx">Forgot Password</a>
        </td>
    </tr>
</table>
</asp:Content>