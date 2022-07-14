<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddCoursePrerequisite.ascx.cs" Inherits="OnlineLearningSystem.AddCoursePrerequisite" %>
<div class="row">
    <div class="col-sm-4">
        <div class="add-poll-type">
            <h6>Course Requisite</h6>
            <hr />
            <asp:Label CssClass="sp-label" ID="LableCrouse" runat="server" Text="Select Course: "></asp:Label>
            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourse" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourse_SelectedIndexChanged"></asp:DropDownList>
            <br /><br />
            <h6>Add Course Prerequisite</h6>
            <hr />
            <asp:Label CssClass="sp-label" ID="Label1" runat="server" Text="Course Prerequisite: "></asp:Label>
            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCoursePrerequisite" runat="server"></asp:DropDownList>

            <div class="margin-top-10">
                <asp:Label CssClass="sp-label" ID="Label4" runat="server" Text="Active"></asp:Label>
                &nbsp;
           <asp:CheckBox ID="CheckBoxActivePrerequisite" runat="server" Checked="true" />
                &nbsp;
            <asp:Button ID="btnCoursePrerequisite" runat="server" CssClass="btn btn-custom btn-sm pull-right" Text="Add Prerequisite" OnClick="btnCoursePrerequisite_Click" />
            </div>

            <br />
            <br />
            <h6>Add Course Corequisite</h6>
            <hr />
            <asp:Label CssClass="sp-label" ID="Label2" runat="server" Text="Course Corequisite: "></asp:Label>
            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseCorequisite" runat="server"></asp:DropDownList>
            <div class="margin-top-10">
                <asp:Label CssClass="sp-label" ID="Label3" runat="server" Text="Active"></asp:Label>
                &nbsp;
           <asp:CheckBox ID="CheckBoxActiveCorequisite" runat="server" Checked="true" />
                &nbsp;
           <asp:Button ID="btnCourseCorequisite" runat="server" CssClass="btn btn-custom btn-sm pull-right" Text="Add Corequisite" OnClick="btnCourseCorequisite_Click" />
            </div>

            <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
            <asp:Label ID="lblErrorMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </div>
    <div class="col-sm-6">
        <div class="margin-left-30">
            <h6>Course Prerequisite List</h6>
            <hr />
            <div class="font-size-13">
                <asp:GridView CssClass="table table-bordered grid-table" ID="GridViewCoursePrerequisite" runat="server" AutoGenerateColumns="False" DataKeyNames="PrerequisiteCourseId" OnRowDataBound="GridViewCoursePrerequisite_RowDataBound" OnRowDeleting="GridViewCoursePrerequisite_RowDeleting" OnRowCancelingEdit="GridViewCoursePrerequisite_RowCancelingEdit" OnRowEditing="GridViewCoursePrerequisite_RowEditing" OnRowUpdating="GridViewCoursePrerequisite_RowUpdating">
                    <Columns>
                        <asp:BoundField DataField="PrerequisiteCourseId" ItemStyle-HorizontalAlign="Center" HeaderText="ID" ReadOnly="True" />
                        <asp:TemplateField HeaderText="Course Prerequisite">

                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("PrerequisiteCourse") %>'></asp:Label>
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
            <br />
            <h6>Course Corequisite List</h6>
            <hr />
            <div class="font-size-13">
                <asp:GridView CssClass="table table-bordered grid-table" ID="GridViewCourseCorequisite" runat="server" AutoGenerateColumns="False" DataKeyNames="CorequisiteCourseId" OnRowDataBound="GridViewCourseCorequisite_RowDataBound" OnRowDeleting="GridViewCourseCorequisite_RowDeleting" OnRowCancelingEdit="GridViewCourseCorequisite_RowCancelingEdit" OnRowEditing="GridViewCourseCorequisite_RowEditing" OnRowUpdating="GridViewCourseCorequisite_RowUpdating">
                    <Columns>
                        <asp:BoundField DataField="CorequisiteCourseId" ItemStyle-HorizontalAlign="Center" HeaderText="ID" ReadOnly="True" />
                        <asp:TemplateField HeaderText="Course Corequisite">
                        
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("CorequisiteCourse") %>'></asp:Label>
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
