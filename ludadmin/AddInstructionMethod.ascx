<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddInstructionMethod.ascx.cs" Inherits="OnlineLearningSystem.AddInstructionMethod" %>
<div class="row">
    <div class="col-sm-4">
        <div class="add-poll-type">
            <h6>Instruction Method</h6>
            <hr />
            <asp:Label CssClass="sp-label" ID="LableCrouse" runat="server" Text="Select Course Instance: "></asp:Label>
            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseInstance" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseInstance_SelectedIndexChanged"></asp:DropDownList>
            <br /><br />
            <h6>Add New Instruction Method</h6>
            <asp:Label CssClass="sp-label" ID="Label1" runat="server" Text="Description: "></asp:Label>
            <asp:TextBox ID="TextBoxDescription" CssClass="custome-textarea form-control margin-bottom-10" runat="server" placeholder="" TextMode="MultiLine"></asp:TextBox>
            <div>
                <asp:Label CssClass="sp-label" ID="Label4" runat="server" Text="Active"></asp:Label>
                &nbsp;
           <asp:CheckBox ID="CheckBoxActive" runat="server" Checked="true" />
                &nbsp;
            <asp:Button ID="btnAddNewInstructionMethod" runat="server" CssClass="btn btn-custom btn-sm pull-right" Text="Add Prerequisite" OnClick="btnCoursePrerequisite_Click" />
            </div>
            <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
            <asp:Label ID="lblErrorMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </div>
    <div class="col-sm-7">
        <div class="margin-left-30">
            <h6>Instruction Method List</h6>
            <hr />
            <div class="font-size-13">
                <asp:GridView CssClass="table table-bordered grid-table" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCancelingEdit="OnRowCancelingEdit" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting">
                    <Columns>
                        <asp:BoundField DataField="Id" ItemStyle-HorizontalAlign="Center" HeaderText="Id" ReadOnly="True" />
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
