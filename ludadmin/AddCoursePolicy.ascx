<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddCoursePolicy.ascx.cs" Inherits="OnlineLearningSystem.AddCoursePolicy" %>

<div class="row">
    <div class="col-md-4">
        <div class="course-instance ">
            <div class="row">
                <div class="col-md-8">
                      <h6>Course Policy</h6>
                </div>
                 <div class="col-md-4">
                      <asp:Button ID="btnClearAll" CssClass="btn btn-custom-light btn-sm" runat="server" Text="Clear All" OnClick="btnClearAll_Click" />
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col-md-5">
                    <asp:Label CssClass="sp-label" ID="Label10" runat="server" Text="Select School"></asp:Label>
                </div>
                 <div class="col-md-7">
                      <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListSchool" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListSchool_SelectedIndexChanged" ></asp:DropDownList>
                </div>
            </div>
  
            <asp:Label CssClass="sp-label" ID="Label8" runat="server" Text="Select a Course Policy "></asp:Label>
            <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListCoursePolicy" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCoursePolicy_SelectedIndexChanged" ></asp:DropDownList>

            <strong>Or, </strong>
            <asp:Label CssClass="sp-label" ID="Label11" runat="server" Text="Add New Course CoursePolicy"></asp:Label>
            <br />

             <asp:Label CssClass="sp-label" ID="Label9" runat="server" Text="Sub Title "></asp:Label>
             <asp:TextBox CssClass="form-control tex-box margin-bottom-10" ID="TextBoxSubTitle" placeholder="Sub Title" runat="server"></asp:TextBox>

             <asp:Label CssClass="sp-label" ID="Label12" runat="server" Text="Description"></asp:Label>
             <asp:TextBox CssClass="form-control font-size-13 margin-bottom-10 custome-textarea" ID="TextBoxDescription" placeholder="Course Description" runat="server" TextMode="MultiLine"></asp:TextBox>
 
            <asp:Label CssClass="sp-label" ID="Label4" runat="server" Text="Active"></asp:Label>
            &nbsp;
            <asp:CheckBox ID="CheckBoxActive" runat="server" Checked="true" />
            <br />
            <div class="text-right">
                <asp:Button ID="btnAddCoursePolicy" runat="server" CssClass="btn btn-custom btn-sm" Text="Add" OnClick="btnAddCoursePolicy_Click"/>
                <asp:Button ID="btnUpdateCoursePolicy" runat="server" CssClass="btn btn-custom-light btn-sm" Text="Update" OnClick="btnUpdateCoursePolicy_Click"/>
                <asp:Button ID="btnDeleteCoursePolicy" OnClientClick="return confirm('Do you want to delete this?');" BackColor="Red" runat="server" CssClass="btn btn-custom btn-sm" Text="Delete" OnClick="btnDeleteCoursePolicy_Click"/>
               <div class="margin-top-10">
                   <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
                   <asp:Label ID="lblErrorMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
               </div>
                
            </div>
        </div>
    </div>
    <div class="col-md-8">
        <div class="margin-left-30 ">
            <h6>Add Course Policy Point</h6>
            <hr />
             <asp:Label CssClass="sp-label" ID="Label5" runat="server" Text="Description"></asp:Label>
             <asp:TextBox CssClass="form-control font-size-13 margin-bottom-15 custome-textarea" ID="TextBoxPolicyPoint" placeholder="Course Description" runat="server" TextMode="MultiLine"></asp:TextBox>
           <div class="row">
               <div class="col-md-6">
                   <asp:Label CssClass="sp-label" ID="Label2" runat="server" Text="Active"></asp:Label>
            &nbsp;
            <asp:CheckBox ID="CheckBoxPolicyPointActive" runat="server" Checked="true" />
               </div>
               <div class="col-md-6 text-right">
                   <asp:Button ID="btnAddCoursePolicyPoint" runat="server" CssClass="btn btn-custom btn-sm" Text="Add Point" OnClick="btnAddCoursePolicyPoint_Click"/>
               </div>
           </div>
            
            <br />
            <br />
            <h6>Course Policy Point List</h6>
            <hr />
            <div class="custome-overflow">
                <asp:Label ID="Label23" runat="server" Text="" Visible="false"></asp:Label>
                <asp:GridView CssClass="table table-bordered grid-table" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCancelingEdit="OnRowCancelingEdit" OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating">
                    <Columns>
                        <asp:BoundField DataField="Id" ItemStyle-HorizontalAlign="Center" HeaderText="ID" ReadOnly="True" />
                        <asp:TemplateField HeaderText="Description">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" CssClass="form-control custome-textarea" TextMode="MultiLine" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                            </EditItemTemplate>
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
                                <asp:LinkButton ID="LinkButtonDelete" runat="server" ForeColor="Red" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</div>




