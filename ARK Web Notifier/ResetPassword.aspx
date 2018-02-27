<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="ARKWebNotifier.ResetPassword" MasterPageFile="~/Site.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <span runat="server" id="spanInfo" style="color:#ee0000;"></span>
    <table runat="server" id="tblPasswords">
        <tr>
            <td>
                <span>New password:</span>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>Confirm new password:</span>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtConfirmPassword" TextMode="Password" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="btnSubmit" Text="Submit" OnClick="btnSubmit_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
