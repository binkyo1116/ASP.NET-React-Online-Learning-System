<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Logout.ascx.cs" Inherits="OnlineLearningSystem.Logout" %>
<span class="pull-left">
<asp:LinkButton ID="btnHome" CssClass="btn-profile" runat="server" PostBackUrl="~/Admin.aspx"><i class="fa fa-home" style="font-size:18px"></i> Home</asp:LinkButton>
</span>
<asp:Button ID="btnLogout" CssClass="btn-profile" runat="server" Text="Logout" OnClick="btnLogout_Click" />

