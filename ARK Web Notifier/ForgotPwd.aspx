<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPwd.aspx.cs" Inherits="ARKWebNotifier.ForgotPwd" MasterPageFile="~/Site.Master" %>
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
        <td colspan="2">
            <span id="spanInfo" runat="server" style="color:#ee0000"></span>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button runat="server" ID="btnSubmit" Text="Submit" OnClick="btnSubmit_Click" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <a href="Register.aspx">Register</a>
        </td>
    </tr>
</table>
</asp:Content>
