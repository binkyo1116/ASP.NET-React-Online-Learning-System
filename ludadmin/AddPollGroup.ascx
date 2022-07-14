<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddPollGroup.ascx.cs" Inherits="OnlineLearningSystem.AddPollGroup" %>
<%--========================================Poll Group Panel =====================================--%>
<div class="tab-pane fade" id="v-pills-poll-group-tab" role="tabpanel" aria-labelledby="v-pills-poll-group">
    <div class="row">
        <div class="col-md-8">
            <h5>Course Instance & Poll Group</h5>
        </div>
        <div class="col-md-4 text-right">
            <asp:Button ID="ButtonRefresh" runat="server" CssClass="btn btn-custom-light btn-sm font-size-13" Text="Refresh" OnClick="ButtonRefresh_Click" />
        </div>
    </div>

    <hr />
    <div class="row">
        <div class="col-md-6">
            <div class="row margin-bottom-10">
                <div class="col-md-4">
                    <asp:Label CssClass="sp-label" ID="Label8" runat="server" Text="Course Instance"></asp:Label>
                </div>
                <div class="col-md">
                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseInstance" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseInstance_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>

            <h6>Poll Group Filter</h6>
            <div class="wraper-area  margin-bottom-10">
                <div class="row margin-bottom-10">
                    <div class="col-md-4">
                        <asp:Label CssClass="sp-label" ID="Label6" runat="server" Text="Course Instance Filter"></asp:Label>
                    </div>
                    <div class="col-md">
                        <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseInstanceFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseInstanceFilter_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <asp:Label CssClass="sp-label" ID="Label11" runat="server" Text="Module Obj. Filter"></asp:Label>
                    </div>
                    <div class="col-md">
                        <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListModuleObjectiveFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListModuleObjectiveFilter_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row margin-bottom-10">
                <div class="col-md-4">
                    <asp:Label CssClass="sp-label" ID="Label2" runat="server" Text="Select Group"></asp:Label>
                </div>
                <div class="col-md">
                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListPollGroup" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListPollGroup_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="row margin-bottom-10">
                <div class="col-md-4">
                    <asp:Label CssClass="sp-label" ID="Label9" runat="server" Text="Module Obj."></asp:Label>
                </div>
                <div class="col-md">
                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListModuleObjective" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListModuleObjective_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>
            <div class="row margin-bottom-10">
                <div class="col-md-4">
                    <asp:Label CssClass="sp-label" ID="LabelDueDate" runat="server" Text="Due Date"></asp:Label>
                </div>
                <div class="col-md">
                    <div style="position: relative">
                        <asp:TextBox CssClass="tex-box form-control DatePicker margin-bottom-10" ID="TextBoxDueDate" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row margin-bottom-10">
                <div class="col-md-6">
                    <asp:Label CssClass="sp-label" ID="Label19" runat="server" Text="Active"></asp:Label>
                    &nbsp;
                                                <asp:CheckBox ID="CheckBoxCourseInstanceCodingProblemActive" runat="server" Checked="true" />
                </div>
                <div class="col-md-6 text-right">
                    <asp:Button ID="ButtonCourseInstancePollGroup" runat="server" CssClass="btn btn-custom btn-sm" Text="Submit" OnClick="ButtonCourseInstancePollGroup_Click" />
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <br />
            <h5>Or, Add New Poll Group</h5>
            <hr />
        </div>
    </div>
    <div class="row">
        <div class="col-md-5">
            <div class="add-poll-group">
                <div class="wraper-area">
                    <h6>Poll Group</h6>
                    <div class="create-Group-area create-activity-area">
                        <%-- <div class="custome-form-group">
                            <asp:Label CssClass="sp-label" ID="Label11" runat="server" Text="Course"></asp:Label>
                            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourse" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourse_SelectedIndexChanged"></asp:DropDownList>
                            <br />
                        </div>
                        <div class="custome-form-group">
                            <asp:Label CssClass="sp-label" ID="Label12" runat="server" Text="Course Obj."></asp:Label>
                            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseObjective" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseObjective_SelectedIndexChanged"></asp:DropDownList>
                            <br />
                        </div>
                        <div class="custome-form-group">
                            <asp:Label CssClass="sp-label" ID="Label16" runat="server" Text="Module"></asp:Label>
                            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListModule" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListModule_SelectedIndexChanged"></asp:DropDownList>
                            <br />
                        </div>--%>
                        <%--   <div class="custome-form-group margin-bottom-10">
                            <asp:Label CssClass="sp-label" ID="LabelModuleObjective" runat="server" Text="Module Obj."></asp:Label>
                            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListModuleObjective" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListModuleObjective_SelectedIndexChanged"></asp:DropDownList>
                        </div>--%>

                        <asp:TextBox CssClass="tex-box form-control" ID="TextBoxTitle" runat="server" placeholder="Group Title"></asp:TextBox>

                        <asp:Label CssClass="sp-label" ID="Label5" runat="server" Text="Active"></asp:Label>
                        &nbsp;
                        <asp:CheckBox ID="CheckBoxActive" runat="server" Checked="true" />
                        <br />
                        <asp:Button ID="AddNewGroup" runat="server" CssClass="btn btn-custom btn-sm font-size-13" Text="Add New" OnClick="AddNewGroup_Click" />
                        &nbsp;
                                     <asp:Button ID="btnGroupUpdate" runat="server" CssClass="btn btn-custom-light btn-sm font-size-13" Text="Update" OnClick="btnGroupUpdate_Click" />
                        &nbsp;
                                     <asp:Button ID="btnGroupDelete" OnClientClick="return confirm('Do you want to delete this?');" BackColor="Red" runat="server" CssClass="btn btn-custom btn-sm font-size-13" Text="Delete" OnClick="btnGroupDelete_Click" />
                        &nbsp;
                    <br />
                        <asp:Label ID="lblMessage" CssClass="sp-label margin-top-10" runat="server" ForeColor="Green"></asp:Label>
                        <asp:Label ID="lblErrorMessage" CssClass="sp-label margin-top-10" runat="server" ForeColor="Red"></asp:Label><br />
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-7">
            <div class="margin-left-30">
                <div>
                    <div class="margin-bottom-15">
                        <h6>Add Poll into Group</h6>
                        <hr />
                        <asp:Label CssClass="sp-label" ID="Label3" runat="server" Text="Select Poll"></asp:Label>
                        <div class="row">
                            <div class="col-md-9">
                                <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListPollQuestion" runat="server"></asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <asp:Button ID="ButtonAddPollQuesInGroup" runat="server" CssClass="btn btn-custom btn-sm font-size-13" Text="Add New" OnClick="ButtonAddPollQuesInGroup_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <h6>Poll Question List</h6>
                <hr />
                <asp:Label CssClass="sp-label text-bold" ID="Label1" runat="server" Text="Title:"></asp:Label>
                <asp:Label CssClass="sp-label" ID="LabelPollGroupTitle" runat="server" Text=""></asp:Label>
                <asp:Label CssClass="sp-label" ID="LabelPollGroupId" runat="server" Text="" Visible="false"></asp:Label>
                <div class="custome-overflow margin-top-5" style="max-height: 260px; height: auto">
                    <asp:GridView CssClass="table table-bordered grid-table" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="PollQuestionId" OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting">
                        <Columns>
                            <asp:BoundField DataField="PollGroupId" HeaderText="PollGroupId" ReadOnly="True" Visible="False" />
                            <asp:BoundField DataField="PollQuestionId" HeaderText="PollQuestionId" ReadOnly="True" Visible="False" />
                            <asp:BoundField DataField="PollQuestion" HeaderText="Poll Question" />
                            <asp:CheckBoxField DataField="Active" HeaderText="Active" />
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonDelete"  ForeColor="Red" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <div class="wraper-area">
                <span>Course Instance & PollGroup</span>
                <div class="custome-overflow margin-top-10" style="max-height: 260px; height: auto">
                    <asp:GridView CssClass="table table-bordered grid-table table-fixed" ID="GridView2" runat="server" DataKeyNames="CourseInstanceId" OnRowCancelingEdit="OnRowCancelingEditCIPG" OnRowEditing="OnRowEditingCIPG" OnRowUpdating="OnRowUpdatingCIPG" OnRowDataBound="OnRowDataBoundCIPG" OnRowDeleting="OnRowDeletingCIPG" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField HeaderText="PollGroupId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="LabelPollGroupId" runat="server" Text='<%# Bind("PollGroupId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ModuleObjectiveId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="LabelModuleObjectiveId" runat="server" Text='<%# Bind("ModuleObjectiveId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CourseInstanceId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="LabelCourseInstanceId" runat="server" Text='<%# Bind("CourseInstanceId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CourseInstance" HeaderText="CourseInstance" ReadOnly="True" />
                            <asp:BoundField DataField="ModuleObjective" HeaderText="ModuleObjective" ReadOnly="True" />
                            <asp:BoundField DataField="PollGroup" HeaderText="PollGroup" ReadOnly="True" />
                            <asp:TemplateField HeaderText="Due Date">
                                <EditItemTemplate>
                                    <div style="position: relative">
                                        <asp:TextBox ID="TextBoxDueDate" CssClass="tex-box form-control DatePicker" runat="server" Text='<%# Eval("DueDate", "{0:MMM-dd-yyyy}") %>'></asp:TextBox>
                                    </div>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("DueDate", "{0:MMM-dd-yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Active">
                                <EditItemTemplate>
                                    <asp:CheckBox ID="CheckBoxActive" runat="server" Checked='<%# Bind("Active") %>' />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Active") %>' Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" />
                            <asp:TemplateField ShowHeader="False" HeaderText="PollGroupId">
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
<%--========================================My Poll Panel =====================================--%>
<div class="tab-pane fade" id="v-pills-my-poll-tab" role="tabpanel" aria-labelledby="v-pills-my-poll">
    <div class="row">
        <div class="col-md-6">
            <h6>Poll Response List</h6>
        </div>
        <div class="col-md-6">
            <asp:Label ID="Label13" CssClass="font-size-15" runat="server" Text="Filter By: "></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:RadioButton ID="RadioButton1" runat="server" GroupName="searchType" Checked="true" AutoPostBack="True" OnCheckedChanged="RadioButton1_CheckedChanged" />
            <asp:Label ID="Label14" CssClass="font-size-15" runat="server" Text="Course Instance & Module Objective"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:RadioButton ID="RadioButton2" runat="server" GroupName="searchType" AutoPostBack="True" OnCheckedChanged="RadioButton2_CheckedChanged" />
            <asp:Label ID="Label15" CssClass="font-size-15" runat="server" Text="All"></asp:Label>
        </div>
    </div>
    <hr />
    <div class="margin-bottom-15">
        <div class="row">
            <div class="col-md-4">
            </div>
            <div class="col-md-8 text-right">
            </div>
        </div>
        <%--        <div class="row">
            <div class="col-md-4">
                
            </div>
            <div class="col-md-8">
                <asp:Label CssClass="font-size-13" ID="LabelCourseAndModule" runat="server" Text=""></asp:Label>
            </div>
        </div>--%>
        <div class="row">
            <div class="col-md-4">
                <%-- <asp:Label ID="Label7" CssClass="sp-label" runat="server" Text="Select Course"></asp:Label>
                <asp:DropDownList ID="ddnCourses_PollList" runat="server" CssClass="form-control tex-box margin-bottom-10 input-sm" AutoPostBack="True" OnSelectedIndexChanged="ddnCourses_PollList_SelectedIndexChanged"></asp:DropDownList>--%>
                <asp:Label ID="Label7" CssClass="sp-label" runat="server" Text="Select Course"></asp:Label>
                <asp:DropDownList ID="ddnCourseInstance_myPollList" runat="server" CssClass="form-control tex-box margin-bottom-10 input-sm" AutoPostBack="True" OnSelectedIndexChanged="ddnCourseInstance_myPollList_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <%--<div class="col-md-4">
                <asp:Label ID="Label8" CssClass="sp-label" runat="server" Text="Select Course Objective"></asp:Label>
                <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownCourseObjective_PollList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseObjective_PollList_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="col-md-4">
                <asp:Label ID="Label9" CssClass="sp-label" runat="server" Text="Select Module"></asp:Label>
                <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListModule_PollList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListModule_PollList_SelectedIndexChanged"></asp:DropDownList>
            </div>--%>
            <div class="col-md-4">
                <asp:Label ID="Label10" CssClass="sp-label" runat="server" Text="Select Module Objective"></asp:Label>
                <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListModuleObjective_myPollList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListModuleObjective_PollList_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="col-md-4">
                <asp:Label CssClass="sp-label" ID="Label4" runat="server" Text="Select Group"></asp:Label>
                <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListPollGroup_myPollList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListPollGroup_myPollList_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="col-md-4">

                <div class="margin-top-10"></div>
                <asp:Button ID="ButtonDeleteAllResponse" runat="server" CssClass="btn btn-custom-light btn-sm font-size-13 margin-bottom-10" Text="Delete Group Response" OnClick="ButtonDeleteAllResponseOfTheGroup_Click" />
                <asp:Button ID="ButtonRefreshMyPollList" runat="server" CssClass="btn btn-custom-light btn-sm font-size-13 margin-bottom-10" Text="Reload" OnClick="ButtonRefreshMyPollList_Click" />
            </div>
        </div>
    </div>
    <%-----------------------------------------------------------------------------------------------------------------%>
    <div>
        <div>
            <asp:ListView ID="ListViewPollQuestion" runat="server" OnItemDataBound="ListView1_ItemDataBound" OnItemCommand="Delete_ItemCommand" DataKeyNames="PollQuestionId">
                <LayoutTemplate>
                    <table class="poll-response-list-view" width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <th width="15px"></th>
                            <th width="50%">Title</th>
                            <th width="15%">Poll Type</th>
                            <th width="7%">Response</th>
                            <th width="7%">Active Poll</th>
                            <th width="7%"></th>
                            <th width="10%"></th>
                        </tr>
                    </table>
                    <div runat="server" id="itemPlaceHolder"></div>
                </LayoutTemplate>
                <ItemTemplate>
                    <div class="SUBDIV" runat="server">
                        <table class="poll-response-list-view" width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="15px">
                                    <div class="btncolexp btn-collapse">
                                        &nbsp;
                                    </div>
                                    <%-- <a href="#" class="btncolexp collapse">click</a>--%>
                                </td>
                                <td width="50%"><%#Eval("Title") %></td>
                                <td width="15%"><%#Eval("PollType") %></td>
                                <td width="7%"><%#Eval("Response") %></td>
                                <td width="7%"><%#Eval("ActivePoll") %></td>
                                <td width="7%">
                                    <asp:LinkButton ID="lnkDeletePollQuestion" CommandName="PollDelete" runat="server" Text="Delete Poll" ToolTip="Delete the record" OnClientClick="javascript:return confirm('Are you sure to delete The Poll?')" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PollQuestionId") %>'></asp:LinkButton>
                                </td>
                                <td width="10%">
                                    <asp:LinkButton ID="btnDeleteResponse" runat="server" Text="Delete All Response" ToolTip="Delete all record" OnClientClick="javascript:return confirm('Are you sure to delete All Response?')" CommandName="ResponseDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PollQuestionId") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <div style="margin: 20px">
                                        <asp:GridView CssClass="table table-bordered table-fixed" ID="GridViewParticipantResponse" runat="server" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField DataField="PollAnswerId" HeaderText="PollAnswerId" ReadOnly="True" Visible="False" />
                                                <asp:BoundField DataField="StudentId" HeaderText="StudentId" ReadOnly="True" Visible="False" />
                                                <asp:BoundField DataField="PollQuestionId" HeaderText="PollQuestionId" ReadOnly="True" Visible="False" />
                                                <asp:BoundField DataField="StudentName" HeaderText="Participant Name" ReadOnly="True" />
                                                <asp:BoundField DataField="Response" HeaderText="Response" />
                                                <asp:BoundField DataField="EnlistedDate" HeaderText="EnlistedDate" />
                                                <%--   <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButtonDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
    <%--------------------------------------------------------------------------------------------------------------------------------%>
</div>
