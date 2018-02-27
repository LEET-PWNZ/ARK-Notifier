<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NotificationControl.ascx.cs" Inherits="ARKWebNotifier.NotificationControl" %>
<div class="NotificationContainer">
    <div class="NotificationHeader">Notification<span runat="server" id="SpanDate"></span></div>
    <div class="NotificationBody">
        <table>
            <tr>
                <td>
                    Server:
                </td>
                <td>
                    <span runat="server" id="SpanServer"></span>
                </td>
            </tr>
            <tr>
                <td>
                    Title:
                </td>
                <td>
                    <span runat="server" id="SpanTitle"></span>
                </td>
            </tr>
            <tr>
                <td>
                    Message:
                </td>
                <td>
                    <span runat="server" id="SpanMessage"></span>
                </td>
            </tr>
        </table>
    </div>
</div>