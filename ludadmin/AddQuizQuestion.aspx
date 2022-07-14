<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddQuizQuestion.aspx.cs" Inherits="OnlineLearningSystem.AddQuizQuestion" %>

<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Let's Use Data</title>
    <link rel="favicon icon" href="favicon.png" type="image/png" />
    <link href="Content/jquery-ui.css" rel="stylesheet" />
    <link href="Content/select2.css" rel="stylesheet" />
    <%: Styles.Render("~/Content/css") %>
    <%----------------------custome style--------------------------%>
    <style type="text/css">
        .table-layout-fixed {
            margin-right: 0px;
        }

        .break-text {
            display: inline-block;
            width: 150px;
            word-break: break-word;
        }

        .break-text200 {
            display: inline-block;
            width: 200px;
            word-break: break-word;
        }
    </style>

    <!---------------- Global site tag (gtag.js) - Google Analytics ------------------>
    <script async src="https://www.googletagmanager.com/gtag/js?id=UA-196054626-1">
    </script>
    <script>
        window.dataLayer = window.dataLayer || [];
        function gtag() { dataLayer.push(arguments); }
        gtag('js', new Date());

        gtag('config', 'UA-196054626-1');
    </script>
    <!------------------------------Close Google Analytics---------------------------->
</head>
<body>
    <%--========================================Top Navbar=================================--%>
    <section class="top-navbar-area">
        <div class="container">
            <div class="row">
                <div class="col-sm-12">
                    <nav class="navbar navbar-expand-lg navbar-light">
                        <div class="brand-logo">
                            <img src="Content/images/element2.png" />
                        </div>
                        <a class="navbar-brand padding-left-15">Online Learning System</a>
                        <!--<button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarText" aria-controls="navbarText" aria-expanded="false" aria-label="Toggle navigation">
                            <span class="navbar-toggler-icon"></span>
                        </button>-->
                        <div class="collapse navbar-collapse" id="navbarText">
                            <%--             <ul class="navbar-nav ml-auto ">
                          <li class="nav-item active">
                            <a class="nav-link" href="#">Home <span class="sr-only">(current)</span></a>
                          </li>
                          <li class="nav-item">
                            <a class="nav-link" href="#">Tutorial <span class="sr-only">(current)</span></a>
                          </li>
                          <li class="nav-item">
                            <a class="nav-link" href="#">Contact <span class="sr-only">(current)</span></a>
                          </li>
                        </ul>--%>
                        </div>
                    </nav>
                </div>
            </div>
        </div>
    </section>
    <%--==================================================================================--%>

    <%--========================================Content area=================================--%>
    <section class="content-area margin-top-15 margin-bottom-100">
        <form id="form1" runat="server">
            <div class="add-course-area" style="margin: 0 60px;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="text-right margin-bottom-15">
                                <asp:Button ID="btnClearAll" CssClass="btn btn-custom-light btn-sm" runat="server" Text="Clear All" OnClick="btnClearAll_Click" />
                                <uc1:Logout ID="ctlLogout" runat="server" />
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-md-3"><h5>Course Instance & Activity</h5></div>
                                 <div class="col-md-9">
                                      <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
                                    <asp:Label ID="lblErrorMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
                                 </div>
                            </div>
                            
                            <hr />
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="row margin-bottom-10">
                                        <div class="col-md-4">
                                            <asp:Label CssClass="sp-label" ID="Label3" runat="server" Text="Course Instance"></asp:Label>
                                        </div>
                                        <div class="col-md">
                                            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseInstance" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseInstance_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <h6>Filter activity</h6>
                                    <div class="wraper-area margin-bottom-10">
                                        <div class="row margin-bottom-10">
                                            <div class="col-md-4">
                                                <asp:Label CssClass="sp-label" ID="Label10" runat="server" Text="Course Instance Filter"></asp:Label>
                                            </div>
                                            <div class="col-md">
                                                <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseInstanceFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseInstanceFilter_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:Label CssClass="sp-label" ID="Label24" runat="server" Text="Module Obj. Filter"></asp:Label>
                                            </div>
                                            <div class="col-md">
                                                <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListModuleObjectiveFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListModuleObjectiveFilter_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row margin-bottom-10">
                                        <div class="col-md-4">
                                            <asp:Label ID="Label5" CssClass="sp-label" runat="server" Text="Select an activity"></asp:Label>
                                        </div>
                                        <div class="col-md">
                                            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListActivity" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListActivity_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="row margin-bottom-10">
                                        <div class="col-md-3">
                                            <asp:Label CssClass="sp-label" ID="Label19" runat="server" Text="Module Obj."></asp:Label>
                                        </div>
                                        <div class="col-md">
                                            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListModuleObjective" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListModuleObjective_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row margin-bottom-10">
                                        <div class="col-md-3">
                                            <asp:Label CssClass="sp-label" ID="LabelDueDate" runat="server" Text="Due Date"></asp:Label>
                                        </div>
                                        <div class="col-md">
                                            <div style="position: relative">
                                                <asp:TextBox CssClass="tex-box form-control DatePicker margin-bottom-10" ID="TextBoxDueDate" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row margin-bottom-10">
                                        <div class="col-md-3">
                                            <asp:Label CssClass="sp-label" ID="Label20" runat="server" Text="Max Grade"></asp:Label>
                                        </div>
                                        <div class="col-md">
                                            <asp:TextBox CssClass="tex-box form-control margin-bottom-8" ID="TextBoxCourseInstanceActivityMaxGrade" type="number" placeholder="Max Grade" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row margin-bottom-10">
                                        <div class="col-md-6">
                                            <asp:Label CssClass="sp-label" ID="Label21" runat="server" Text="Active"></asp:Label>
                                            &nbsp;
                                                <asp:CheckBox ID="CheckBoxCourseInstanceCodingProblemActive" runat="server" Checked="true" />
                                        </div>
                                        <div class="col-md-6 text-right">
                                            <asp:Button ID="ButtonCourseInstanceActivity" runat="server" CssClass="btn btn-custom btn-sm" Text="Submit" OnClick="ButtonCourseInstanceActivity_Click" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="row margin-bottom-15">
                                <div class="col-md-12">
                                    <h5>Or, Add New Activity</h5>
                                    <hr />
                                    <asp:Label ID="Label22" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-5">
                            <div class="wraper-area">
                                <div class="create-activity-area">
                                    <h6>Activity</h6>
                                    <asp:TextBox CssClass="tex-box form-control" ID="TextBoxTitle" runat="server" placeholder="Title"></asp:TextBox>
                                    <asp:TextBox CssClass="tex-box form-control" ID="TextBoxType" runat="server" placeholder="Type"></asp:TextBox>
                                    <asp:TextBox CssClass="tex-box form-control margin-bottom-10" ID="TextBoxRole" type="number" runat="server" placeholder="Role"></asp:TextBox>
                                    <div class="row margin-bottom-10">
                                        <div class="col-md-6">
                                            <asp:TextBox CssClass="tex-box form-control" ID="TextBoxActivityMaxGrade" type="number" runat="server" placeholder="Max Grade"></asp:TextBox>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label CssClass="sp-label" ID="Label4" runat="server" Text="Active"></asp:Label>
                                            &nbsp;
                                            <asp:CheckBox ID="CheckBoxActive" runat="server" Checked="true" />
                                        </div>
                                    </div>
                                    <asp:Button ID="AddNewActivity" runat="server" CssClass="btn btn-custom btn-sm" Text="Add New" OnClick="AddNewActivity_Click" />
                                    &nbsp;
                                     <asp:Button ID="btnActivityUpdate" runat="server" CssClass="btn btn-custom btn-sm" Text="Update" OnClick="btnActivityUpdate_Click" />
                                    &nbsp;
                                     <asp:Button ID="btnActivityDelete" OnClientClick="return confirm('Do you want to delete this?');" runat="server" BackColor="Red" CssClass="btn btn-custom btn-sm" Text="Delete" OnClick="btnActivityDelete_Click" />
                                    &nbsp;
                                    <br />
                                    <br />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-7">
                            <div class="wraper-area">
                                <h6>Add New Quiz Question</h6>
                                <%-------------------------------------%>
                                <div class="row">
                                    <div class="col-sm-8">
                                        <asp:Label CssClass="sp-label" ID="Label6" runat="server" Text="Prompt 1"></asp:Label>
                                        <asp:TextBox CssClass="form-control font-size-13" ID="TextBoxPrompt1" runat="server" TextMode="MultiLine"></asp:TextBox>

                                        <asp:Label CssClass="sp-label" ID="Label7" runat="server" Text="Prompt 2"></asp:Label>
                                        <asp:TextBox CssClass="form-control font-size-13" ID="TextBoxPrompt2" runat="server" TextMode="MultiLine"></asp:TextBox>

                                        <asp:Label CssClass="sp-label" ID="Label14" runat="server" Text="Source"></asp:Label>
                                        <asp:TextBox CssClass="form-control font-size-13" ID="TextBoxSource" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <asp:Label CssClass="sp-label" ID="Label9" runat="server" Text="Height"></asp:Label>
                                                <asp:TextBox CssClass="tex-box form-control" type="number" ID="TextBoxHeight" runat="server"></asp:TextBox>

                                                <asp:Label CssClass="sp-label" ID="Label11" runat="server" Text="Width"></asp:Label>
                                                <asp:TextBox CssClass="tex-box form-control" type="number" ID="TextBoxWidth" runat="server"></asp:TextBox>

                                                <asp:Label CssClass="sp-label" ID="Label18" runat="server" Text="Case Sensitive"></asp:Label>
                                                &nbsp;
                                                <asp:CheckBox ID="CheckBoxCaseSensitive" runat="server" Checked="true" />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label CssClass="sp-label" ID="Label16" runat="server" Text="Video Timestamp"></asp:Label>
                                                <asp:TextBox CssClass="tex-box form-control" type="number" ID="TextBoxVideoTimestamp" runat="server"></asp:TextBox>

                                                <asp:Label CssClass="sp-label" ID="Label17" runat="server" Text="Video Source"></asp:Label>
                                                <asp:TextBox CssClass="tex-box form-control" ID="TextBoxVideoSource" runat="server"></asp:TextBox>


                                                <asp:Label CssClass="sp-label" ID="Label23" runat="server" Text="Embed Action"></asp:Label>
                                                &nbsp;
                                               <asp:CheckBox ID="CheckBoxEmbedAction" runat="server" Checked="true" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Label CssClass="sp-label" ID="Label12" runat="server" Text="Type"></asp:Label>
                                        <asp:TextBox CssClass="tex-box form-control" ID="TextBoxQuizType" runat="server"></asp:TextBox>

                                        <asp:Label CssClass="sp-label" ID="Label13" runat="server" Text="Answer"></asp:Label>
                                        <asp:TextBox CssClass="tex-box form-control" ID="TextBoxAnswer" runat="server"></asp:TextBox>

                                        <asp:Label CssClass="sp-label" ID="Label15" runat="server" Text="Max Grade"></asp:Label>
                                        <asp:TextBox CssClass="tex-box form-control" type="number" ID="TextBoxMaxGrade" runat="server"></asp:TextBox>

                                        <asp:Label CssClass="sp-label" ID="Label2" runat="server" Text="Position X"></asp:Label>
                                        <asp:TextBox CssClass="tex-box form-control" type="number" ID="TextBoxPositionX" runat="server"></asp:TextBox>

                                        <asp:Label CssClass="sp-label" ID="Label8" runat="server" Text="Position Y"></asp:Label>
                                        <asp:TextBox CssClass="tex-box form-control" type="number" ID="TextBoxPositionY" runat="server"></asp:TextBox>
                                        <br />
                                        <asp:Button ID="AddNewQuiz" runat="server" CssClass="btn btn-custom btn-sm" Text="Add New Question" OnClick="AddNewQuiz_Click" />
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="wraper-area">
                                <span>Course Instance & Activity List</span>
                                <div class="custome-overflow margin-top-10" style="max-height: 260px; height: auto">
                                    <asp:GridView CssClass="table table-bordered grid-table table-fixed" ID="GridView2" runat="server" DataKeyNames="CourseInstanceId" OnRowCancelingEdit="OnRowCancelingEditCIA" OnRowEditing="OnRowEditingCIA" OnRowUpdating="OnRowUpdatingCIA" OnRowDataBound="OnRowDataBoundCIA" OnRowDeleting="OnRowDeletingCIA" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField ShowHeader="False" HeaderText="ActivityId" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelActivityId" runat="server" Text='<%# Bind("ActivityId") %>'></asp:Label>
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
                                            <asp:BoundField DataField="Activity" HeaderText="Activity" ReadOnly="True" />
                                            <asp:TemplateField HeaderText="MaxGrade">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBoxMaxGrade" CssClass="form-control" runat="server" Text='<%# Bind("MaxGrade") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("MaxGrade") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
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
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButtonDelete" runat="server" ForeColor="Red" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <br />
                            <div class="wraper-area">
                                <span>Question List</span>
                                <div class="custome-overflow margin-top-10" style="max-height: 260px; height: auto">
                                    <asp:GridView CssClass="table table-bordered grid-table table-fixed" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCancelingEdit="OnRowCancelingEdit" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting">
                                        <Columns>
                                            <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" Visible="False" />
                                            <asp:TemplateField HeaderText="Prompt1">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox7" CssClass="form-control" runat="server" Text='<%# Bind("Prompt1") %>' TextMode="multiline"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label7" CssClass="break-text200" runat="server" Text='<%# Bind("Prompt1") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Prompt2">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox6" CssClass="form-control" runat="server" Text='<%# Bind("Prompt2") %>' TextMode="multiline"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label6" CssClass="break-text200" runat="server" Text='<%# Bind("Prompt2") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Answer">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox5" CssClass="form-control" runat="server" Text='<%# Bind("Answer") %>' TextMode="multiline"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("Answer") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Source">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox4" CssClass="form-control" runat="server" Text='<%# Bind("Source") %>' TextMode="multiline"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label4" CssClass="break-text" runat="server" Text='<%# Bind("Source") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Max Grade">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox3" CssClass="form-control" runat="server" Text='<%# Bind("MaxGrade") %>' TextMode="multiline"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("MaxGrade") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CaseSensitive">
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="CheckBoxCaseSensitive" runat="server" Checked='<%# Bind("CaseSensitive") %>' />
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CheckBoxCaseSensitive2" runat="server" Checked='<%# Bind("CaseSensitive") %>' Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Position X">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBoxPositionX" runat="server" CssClass="form-control" Text='<%# Bind("PositionX") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label15" runat="server" Text='<%# Bind("PositionX") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Position Y">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBoxPositionY" CssClass="form-control" runat="server" Text='<%# Bind("PositionY") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label14" runat="server" Text='<%# Bind("PositionY") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Height">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBoxHeight" CssClass="form-control" runat="server" Text='<%# Bind("Height") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label13" runat="server" Text='<%# Bind("Height") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Width">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBoxWidth" CssClass="form-control" runat="server" Text='<%# Bind("Width") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label12" runat="server" Text='<%# Bind("Width") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBoxType" CssClass="form-control" TextMode="MultiLine" runat="server" Text='<%# Bind("Type") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label11" runat="server" Text='<%# Bind("Type") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Video Timestamp">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBoxVideoTimestamp" CssClass="form-control" runat="server" Text='<%# Bind("VideoTimestamp") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label9" runat="server" Text='<%# Bind("VideoTimestamp") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Video Source">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBoxVideoSource" CssClass="form-control" runat="server" Text='<%# Bind("VideoSource") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label8" runat="server" Text='<%# Bind("VideoSource") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Embed Action">
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="CheckBoxEmbedAction" runat="server" Checked='<%# Bind("EmbedAction") %>' />
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CheckBoxEmbedAction2" runat="server" Checked='<%# Bind("EmbedAction") %>' Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField ShowEditButton="True" />
                                            <asp:TemplateField ShowHeader="true">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButtonDelete" ForeColor="Red" runat="server" CausesValidation="true" CommandName="Delete" Text="Delete"></asp:LinkButton>
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
        </form>

    </section>
    <%--===================================================================================--%>
    <%--========================================footer area=================================--%>
    <section class="footer-area">
        <div class="container">
            <div class="row">
                <div class="col-sm-12">
                    <div class="footer-content">
                    </div>
                </div>
            </div>
        </div>
    </section>
    <%--=============================================================================--%>
    <%: Scripts.Render("~/bundles/js") %>
    <script src="Scripts/jquery-ui.min.js"></script>
    <script src="Scripts/select2.min.js"></script>
    <script>
        $(function () {
            $(".DatePicker").datepicker({
                dateFormat: "d MM, yy",
                changeMonth: true,
                changeYear: true,
                yearRange: "-100:+50",
                minDate: new Date(1920, 0, 1),
                maxDate: new Date(2050, 0, 1),
                showAnim: "blind",
                showOn: "both",
                buttonText: "<i class='fa fa-calendar'></i>"

            });

            $("#DropDownListCourseInstance").select2();
            $("#DropDownListModuleObjective").select2();
            $("#DropDownListCourseInstanceFilter").select2();
            $("#DropDownListModuleObjectiveFilter").select2();
            $("#DropDownListActivity").select2();
        })
    </script>
</body>
</html>
