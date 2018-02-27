<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="ARKWebNotifier.Register" MasterPageFile="~/Site.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table>
     <tr>
        <td>
            <span>Username:</span>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtUsername"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <span>Email Address:</span>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtEmailAddress"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <span>Confirm Email Address:</span>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtConfirmEmail"></asp:TextBox>
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
        <td>
            <span>Confirm Password:</span>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtPasswordConfirm" TextMode="Password"></asp:TextBox>
        </td>
    </tr>
        <tr>
            <td colspan="2">
                <span id="spanInfo" style="color:#ee0000" runat="server"></span>
            </td>
        </tr>
    <tr>
        <td colspan="2">
            <asp:Button runat="server" ID="btnRegister" Text="Register" OnClick="btnRegister_Click" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <a href="ForgotPwd.aspx">Forgot Password</a>
        </td>
    </tr>
</table>
</asp:Content>

