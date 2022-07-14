<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubmissionControl.ascx.cs" Inherits="AdminPages.SubmissionControl" %>

<asp:Panel ID="Panel1" runat="server" DefaultButton="btnSubmitGrade">
    <div class="wraper-area margin-bottom-15">
        <div>
            <!-- -----------------------------calculate the students current and overall grade-------------------------------->
            <div class="row">
                <div class="col-md-6">
                    <div>
                        <div class="row margin-bottom-10">
                            <div class="col-lg-3">
                                <asp:Label ID="Label1" CssClass="sp-label" runat="server" Text="Student Name"></asp:Label>
                            </div>
                            <div class="col-lg">
                                <asp:Label ID="lblStudentName" CssClass="result-span" runat="server" Text=""></asp:Label></h4>
                            </div>
                        </div>
                        <div class="row margin-bottom-10">
                            <div class="col-lg-3">
                                <asp:Label ID="Label2" CssClass="sp-label" runat="server" Text="Time Stamp"></asp:Label>
                            </div>
                            <div class="col-lg">
                                <asp:Label ID="lblTimeStamp" CssClass="sp-label" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row margin-bottom-10">
                            <div class="col-lg-3">
                                <asp:Label ID="Label4" CssClass="sp-label" runat="server" Text="Grade"></asp:Label>
                            </div>
                            <div class="col-lg-4">
                                <asp:TextBox CssClass="tex-box form-control" type="number" ID="TextBoxGrade" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row margin-bottom-10">
                            <div class="col-lg-3">
                                <asp:Label ID="Label5" CssClass="sp-label" runat="server" Text="Comment"></asp:Label>
                            </div>
                            <div class="col-lg">
                                <asp:TextBox CssClass="form-control font-size-13 txtComment" ID="TextBoxComment" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                        </div>
                        <div class="row margin-bottom-10">
                            <div class="col-lg-6">
                                <asp:Label ID="lblMessage" CssClass="sp-label text-danger" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-lg text-right">
                                <asp:Button ID="btnSubmitGrade" runat="server" CssClass="btn btn-custom btn-sm" Text="Submit" OnClick="btnSubmitGrade_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:Label ID="Label20" CssClass="sp-label" runat="server" Text="Code"></asp:Label>
                        </div>
                        <div class="col-lg">
                            <asp:TextBox CssClass="form-control font-size-13 txtCode" ID="TextBoxCode" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <!----------------------------------------------------------------------------------------------------------------------------->
        </div>
    </div>
</asp:Panel>
