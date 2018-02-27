<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailNotVerified.aspx.cs" Inherits="ARKWebNotifier.EmailNotVerified" MasterPageFile="~/Site.Master" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <span style="text-align:center;width:100%;"><br />The email address for this account has not yet been verified. Please follow the link in the verification email that was sent, or click the button below to have the email sent again.<br /><br />
    <asp:Button runat="server" style="text-align:center;margin-left:auto;margin-right:auto;display:block;" ID="btnSendAgain" OnClick="btnSendAgain_Click" Text="Resend Email" /></span>
</asp:Content>
