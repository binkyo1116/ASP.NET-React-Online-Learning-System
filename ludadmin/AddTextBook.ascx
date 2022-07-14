<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddTextBook.ascx.cs" Inherits="OnlineLearningSystem.AddTextBook" %>
<div class="row">
    <div class="col-sm-4">
        <div>
            <h6>Add New Text Book</h6>
            <hr />
            <%--  <asp:Label CssClass="sp-label" ID="LableCrouse" runat="server" Text="Select Course Instance: "></asp:Label>
            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseInstance" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseInstance_SelectedIndexChanged"></asp:DropDownList>
            <br />--%>

            <asp:Label CssClass="sp-label" ID="Label1" runat="server" Text="Description: "></asp:Label>
            <asp:TextBox ID="TextBoxDescription" CssClass="form-control margin-bottom-15 custome-textarea" runat="server" placeholder="" TextMode="MultiLine"></asp:TextBox>
            <div>
                <asp:Label CssClass="sp-label" ID="Label4" runat="server" Text="Active"></asp:Label>
                &nbsp;
           <asp:CheckBox ID="CheckBoxActive" runat="server" Checked="true" />
                &nbsp;
            <asp:Button ID="btnAddTextBook" runat="server" CssClass="btn btn-custom btn-sm pull-right" Text="Add Technology" OnClick="btnAddTextBook_Click" />
            </div>
            <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
            <asp:Label ID="lblErrorMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
        </div>
        <br />
        <div>
            <h6>Text Book List</h6>
            <hr />
            <div class="font-size-13">
                <asp:GridView CssClass="table table-bordered grid-table" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="TextbookId" OnRowCancelingEdit="OnRowCancelingEdit" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting">
                    <Columns>
                        <asp:BoundField DataField="TextbookId" HeaderText="ID" ReadOnly="True" />
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
    <div class="col-sm-8">
        <div class="margin-left-30">
            <div>
                <h6>Course Text Book</h6>
                <hr />
                <div class="row">
                    <div class="col-md-6">
                        <asp:Label CssClass="sp-label" ID="LableCrouse" runat="server" Text="Select Course: "></asp:Label>
                        <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourse" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourse_SelectedIndexChanged"></asp:DropDownList>

                    </div>
                    <div class="col-md-6">
                        <asp:Label CssClass="sp-label" ID="Label3" runat="server" Text="Select Text Book: "></asp:Label>
                        <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListTextBook" runat="server" AutoPostBack="True"></asp:DropDownList>

                    </div>
                </div>
                <div class="margin-top-15">
                    <div class="row">
                        <div class="col-md-4">
                            <asp:Label CssClass="sp-label" ID="Label2" runat="server" Text="Active"></asp:Label>
                            &nbsp;
                           <asp:CheckBox ID="CheckBoxCourseTextBookActive" runat="server" Checked="true" />
                        </div>
                        <div class="col-md text-right">
                            <asp:Button ID="btnClearAll" CssClass="btn btn-custom-light btn-sm" runat="server" Text="Clear All" OnClick="btnClearAll_Click" />&nbsp;
                            <asp:Button ID="addCourseTextBook" runat="server" CssClass="btn btn-custom btn-sm pull-right" Text="Add Course Technology" OnClick="addCourseTextBook_Click" />
                        </div>
                    </div>
                </div>
                <asp:Label ID="lblMessageCourseTextBook" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
                <asp:Label ID="lblErrorMessageCourseTextBook" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
            </div>
            <br />
            <div>
                <h6>Course Text Book List</h6>
                <hr />
                <div class="font-size-13">
                    <asp:GridView CssClass="table table-bordered grid-table" ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="TextbookId" OnRowCancelingEdit="GridView2_RowCancelingEdit" OnRowEditing="GridView2_RowEditing" OnRowUpdating="GridView2_RowUpdating" OnRowDataBound="GridView2_RowDataBound" OnRowDeleting="GridView2_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="TextbookId" HeaderText="TextbookId" ReadOnly="True" Visible="False" />
                            <asp:BoundField DataField="Course" HeaderText="Course" ReadOnly="True" />
                            <asp:TemplateField HeaderText="CourseId" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="LabelCourseId" runat="server" Text='<%# Bind("CourseId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Text Book">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                </ItemTemplate>
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
</div>
