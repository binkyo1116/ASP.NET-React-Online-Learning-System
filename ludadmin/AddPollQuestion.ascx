<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddPollQuestion.ascx.cs" Inherits="OnlineLearningSystem.AddPollQuestion" %>
<%--========================================Add Poll Panel =====================================--%>
<div class="tab-pane fade" id="v-pills-create-poll-tab" role="tabpanel" aria-labelledby="v-pills-create-poll">
    <div class="add-poll-question-area">
        <h6>Add New Poll</h6>
        <hr />
        <div class="row">
            <div class="col-sm-6">
                <div class="add-poll-question">

                    <asp:Label CssClass="sp-label" ID="LabelPollType" runat="server" Text="Select Poll Type: "></asp:Label>
                    <div class="row">
                        <div class="col-md-9">
                            <asp:DropDownList ID="DropDownListPollType" CssClass="tex-box form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListPollType_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:Button ID="ButtonRefressTypeList" runat="server" CssClass="btn btn-custom-light btn-sm font-size-13" Text="Refresh" OnClick="ButtonRefressTypeList_Click" />
                        </div>
                    </div>

                    <br />
                    <div class="">
                        <h6>Poll Question</h6>
                        <asp:TextBox CssClass="form-control" ID="TextBoxQuestionTitle" runat="server" placeholder="Title" TextMode="MultiLine"></asp:TextBox>
                        <br />
                        <div>
                            <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox2_CheckedChanged" />
                            <asp:Label ID="Label2" CssClass="sp-label" runat="server" Text="Label">Add Poll Group</asp:Label>
                            
                            <asp:Panel ID="PanelAddPollGroup" runat="server" Visible="false">
                                <asp:DropDownList ID="DropDownListGroupList" CssClass="tex-box form-control" runat="server"></asp:DropDownList>
                            </asp:Panel>
                        </div>
                        <br />
                        <asp:Button ID="AddNewPollQuestion" runat="server" CssClass="btn btn-custom btn-sm font-size-13" Text="Add Question" OnClick="AddNewPollQuestion_Click" />
                        &nbsp;
                    <asp:Button ID="btnPollQuestionUpdate" runat="server" CssClass="btn btn-custom-light btn-sm font-size-13" Text="Update" OnClick="btnPollQuestionUpdate_Click" />
                        &nbsp;
                    <asp:Button ID="btnPollQuestionDelete" BackColor="Red" runat="server" CssClass="btn btn-custom btn-sm font-size-13" Text="Delete" OnClick="btnPollQuestionDelete_Click" />
                        &nbsp;
                    </div>

                    <hr />
                    <asp:Panel ID="PanelPollOption" runat="server" Visible="false">
                        <div class="add-pool-option">
                            <br />
                            <h6>Poll Option</h6>
                            <div class="row">
                                <div class="col-md-1">
                                    <asp:CheckBox ID="CheckBoxCorrectAns" runat="server" />
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox CssClass="tex-box form-control" ID="TextBoxPollOption" runat="server" placeholder="Option"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:Button runat="server" CssClass="btn btn-custom btn-sm font-size-13" ID="ButtonAddPollOption" Text="Add Option" OnClick="ButtonAddPollOption_Click" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <br />
                   <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
                   <asp:Label ID="lblErrorMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label><br />

                </div>
            </div>
            <div class="col-sm-6">
                <div class="margin-left-30">
                    <strong>Question: </strong>
                    <asp:Label ID="LabelPollQuestionTitle" runat="server" Text=""></asp:Label><br />
                    <asp:Label ID="LabelPollQuestionId" runat="server" Text="Label" Visible="false"></asp:Label>
                    <br />
                    <div class="custome-overflow">
                        <asp:GridView CssClass="table table-bordered table-fixed" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="PollOptionId" OnRowCancelingEdit="OnRowCancelingEdit" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting">
                            <Columns>
                                <asp:BoundField DataField="PollOptionId" HeaderText="PollOptionId" ReadOnly="True" Visible="False" />
                                <asp:BoundField DataField="PollQuestionId" HeaderText="PollQuestionId" ReadOnly="True" Visible="False" />
                                <asp:BoundField DataField="Identity" HeaderText="Option" ReadOnly="True" />
                                <asp:TemplateField HeaderText="Title">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" CssClass="form-control" runat="server" Text='<%# Bind("Title") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Title") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Correct Answer">
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("CorrectAnswer") %>' />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("CorrectAnswer") %>' Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowEditButton="True" />
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonDelete" ForeColor="Red" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<%--========================================Poll List Panel =====================================--%>
<div class="tab-pane fade" id="v-pills-poll-list-tab" role="tabpanel" aria-labelledby="v-pills-poll-list">
    <h6>Poll List</h6>
    <hr />
    <div class="custome-overflow">
        <asp:Label ID="lblListMessage" runat="server" Style="font: bold 12px sans-serif" Font-Size="Small" ForeColor="Red"></asp:Label>
        <asp:GridView CssClass="table table-bordered grid-table table-fixed" ID="GridViewMyPoll" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowDeleting="GridViewMyPoll_RowDeleting" OnRowCommand="GridViewMyPoll_RowCommand" OnRowDataBound="GridViewMyPoll_RowDataBound">
            <Columns>
                <asp:BoundField DataField="Id" ItemStyle-HorizontalAlign="Center" HeaderText="Id" ReadOnly="True" />
                <asp:TemplateField HeaderText="Title">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" CssClass="form-control" runat="server" Text='<%# Bind("Title") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Title") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PollType" HeaderText="Poll Type" />
                <asp:BoundField DataField="EnlistedDate" HeaderText="Enlisted Date" />
                <asp:ButtonField Text="Edit" CommandName="PollEdit" />
               <asp:ButtonField Text="Delete" CommandName="PollDelete" />
             <%--   <asp:TemplateField ShowHeader="true">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButtonDelete" ForeColor="Red" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>--%>
            </Columns>
            <SelectedRowStyle BackColor="#CCFFCC" />
        </asp:GridView>
    </div>
</div>


