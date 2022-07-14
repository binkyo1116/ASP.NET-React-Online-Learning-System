<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddCommunityStandard.ascx.cs" Inherits="OnlineLearningSystem.AddCommunityStandard" %>
<div class="row">
    <div class="col-sm-4">
        <div>
            <h6>Add Community Standard</h6>
            <hr />

            <asp:Label CssClass="sp-label" ID="Label10" runat="server" Text="Select a School "></asp:Label>
            <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListSchool" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListSchool_SelectedIndexChanged"></asp:DropDownList>

            <asp:Label CssClass="sp-label" ID="Label9" runat="server" Text="Sub Title "></asp:Label>
            <asp:TextBox CssClass="form-control tex-box margin-bottom-10" ID="TextBoxSubTitle" placeholder="Sub Title" runat="server"></asp:TextBox>

            <asp:Label CssClass="sp-label" ID="Label12" runat="server" Text="Description"></asp:Label>
            <asp:TextBox CssClass="form-control font-size-13 margin-bottom-10 custome-textarea" ID="TextBoxDescription" placeholder="Course Description" runat="server" TextMode="MultiLine"></asp:TextBox>

            <div>
                <asp:Label CssClass="sp-label" ID="Label4" runat="server" Text="Active"></asp:Label>
                &nbsp;
           <asp:CheckBox ID="CheckBoxActive" runat="server" Checked="true" />
                &nbsp;
                <asp:Button ID="btnAddCommunityStandard" CssClass="btn btn-custom btn-sm pull-right"  runat="server" Text="Add" OnClick="btnAddCommunityStandard_Click" />
            </div>
            <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
            <asp:Label ID="lblErrorMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </div>
    <div class="col-sm-8">
        <div class="margin-left-30">
            <h6>Community Standard List</h6>
            <hr />
            <div class="custome-overflow">
                <asp:GridView CssClass="table table-bordered grid-table" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCancelingEdit="OnRowCancelingEdit" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting">
                    <Columns>
                        <asp:BoundField DataField="Id" ItemStyle-HorizontalAlign="Center" HeaderText="ID" ReadOnly="True" />
                        <asp:TemplateField HeaderText="Subtitle">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" CssClass="form-control font-size-13" runat="server" TextMode="MultiLine" Text='<%# Bind("Subtitle") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Subtitle") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" CssClass="custome-textarea form-control" TextMode="MultiLine" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active">
                            <EditItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Active") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Active") %>' Enabled="false" />
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
