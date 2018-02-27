<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SteamID.aspx.cs" Inherits="ARKWebNotifier.SteamID" MasterPageFile="~/Site.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <span><br /><span runat="server" id="spanInfo">A Steam ID is required to view notifications. Please click the button below to sign in using Steam.</span><br /><br />
    <asp:Button style="text-align:center;margin-left:auto;margin-right:auto;display:block;" runat="server" ID="btnSteamSignIn" Text="Sign in with Steam" OnClick="btnSteamSignIn_Click" /></span>
</asp:Content>

