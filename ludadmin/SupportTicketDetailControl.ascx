<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupportTicketDetailControl.ascx.cs" Inherits="AdminPages.SupportTicketDetailControl" %>

<asp:Panel ID="Panel1" runat="server">
    <div class="message-box">
        <div class="support-ticket-person">
            <asp:Image ID="PersonImage" ImageUrl="Content/images/photo.jpg" runat="server" class="image-circle" />

            <div class="margin-top-5">
                <asp:Label ID="PersonName" class="support-ticket-person" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div class="text-area">
            <asp:Label ID="lblTextMessage" class="font-size-13" runat="server" Text=""></asp:Label>
            <div class="support-ticket-image-area">
                <asp:Image ID="imgScreenShort" Width="300" runat="server" class="box-bg-white margin-t-1" />
            </div>
        </div>
    </div>
</asp:Panel>
