<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddPollQuestionType.ascx.cs" Inherits="OnlineLearningSystem.AddPollQuestionType" %>
<div class="row">
    <div class="col-sm-3">
        <div class="add-poll-type">
            <h6>Add New Poll Type</h6>
            <hr />
            <asp:Label CssClass="sp-label" ID="lblTitle" runat="server" Text="Title: "></asp:Label>
            <asp:TextBox CssClass="tex-box form-control" ID="TextBoxTitle" runat="server"></asp:TextBox>
            <br />
             <asp:Label CssClass="sp-label" ID="LabelOpton" runat="server" Text="Allow Poll Option: "></asp:Label>&nbsp;
             <asp:CheckBox ID="CheckBoxOption" runat="server" />
            <br />
            <asp:Button ID="AddNewPollType" runat="server" CssClass="btn btn-custom btn-sm" Text="Add New" OnClick="AddNewPollType_Click"  />
            <br />
             <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
             <asp:Label ID="lblErrorMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </div>
        <div class="col-sm-6">
        <div class="poll-type-list margin-left-30">
            <h6>Poll Type List</h6>
            <hr />
            <div class="custome-overflow">
                <asp:GridView CssClass="table table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="PollTypeId" OnRowDataBound="OnRowDataBound" OnRowCancelingEdit="OnRowCancelingEdit" OnRowDeleting="OnRowDeleting" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating">
                    <Columns>
                        <asp:BoundField DataField="PollTypeId" ItemStyle-HorizontalAlign="Center" HeaderText="ID" ReadOnly="True" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Type Title">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" CssClass="form-control" runat="server" Text='<%# Bind("TypeTitle") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("TypeTitle") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Poll Option">
                            <EditItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("PollOption") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("PollOption") %>' Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButtonDelete" runat="server" ForeColor="Red" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

        </div>
    </div>
</div>
