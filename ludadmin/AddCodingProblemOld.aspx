<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCodingProblemOld.aspx.cs" Inherits="OnlineLearningSystem.AddCodingProblemOld" ValidateRequest="false" %>

<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Let's Use Data</title>
    <link rel="favicon icon" href="favicon.png" type="image/png" />
    <link href="Content/jquery-ui.css" rel="stylesheet" />
    <link href="Content/select2.css" rel="stylesheet" />
    <%--<link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />--%>
    <%: Styles.Render("~/Content/css") %>
    <style>
        .margin-bottom-8 {
            margin-bottom: 8px;
        }
    </style>
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
            <div class="add-course-area">
                <div style="margin: 0 60px;">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="text-right margin-bottom-15">
                                    <asp:Button ID="btnClearAll" CssClass="btn btn-custom-light btn-sm" runat="server" Text="Clear All" OnClick="btnClearAll_Click" />&nbsp;
                                    <uc1:Logout ID="ctlLogout" runat="server" />
                                </div>
                            </div>
                            <div class="col-md-12">
                                <h5>Course Instance & Coding Problem</h5>
                                <hr />
                                <div class="row">
                                    <div class="col-md-6">
                                        <h6>Course Instance</h6>
                                        <div class="wraper-area margin-bottom-15">
                                            <div class="row margin-bottom-10">
                                                <div class="col-md-3">
                                                    <asp:Label CssClass="sp-label" ID="Label11" runat="server" Text="Course"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseFilter1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseFilter1_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                                  <div class="row margin-bottom-10">
                                                <div class="col-md-3">
                                                    <asp:Label CssClass="sp-label" ID="Label13" runat="server" Text="Quarter"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListQuarter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListQuarter_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <asp:Label CssClass="sp-label" ID="Label3" runat="server" Text="Course Instance"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseInstance" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseInstance_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div>
                                            <div class="row margin-bottom-10">
                                                <div class="col-md-3">
                                                    <asp:Label CssClass="sp-label" ID="LabelModuleObjective" runat="server" Text="Module Objective"></asp:Label>
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
                                                    <asp:Label CssClass="sp-label" ID="Label7" runat="server" Text="Max Grade"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:TextBox CssClass="tex-box form-control margin-bottom-8" ID="TextBoxCourseInstanceCPMaxGrade" type="number" placeholder="Max Grade" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row margin-bottom-10">
                                                <div class="col-md-6">
                                                    <asp:Label CssClass="sp-label" ID="Label8" runat="server" Text="Active"></asp:Label>
                                                    &nbsp;
                                                <asp:CheckBox ID="CheckBoxCourseInstanceCodingProblemActive" runat="server" Checked="true" />
                                                </div>
                                                <div class="col-md-6 text-right">
                                                    <asp:Button ID="ButtonCourseInstanceCodingProblem" runat="server" CssClass="btn btn-custom btn-sm" Text="Submit" OnClick="ButtonCourseInstanceCodingProblem_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6">
                                        <h6>Coding Problem</h6>
                                        <div class="wraper-area margin-bottom-10">
                                            <div class="row margin-bottom-10">
                                                <div class="col-md-3">
                                                    <asp:Label CssClass="sp-label" ID="Label12" runat="server" Text="Course"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseFilter2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseFilter2_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                                    <div class="row margin-bottom-10">
                                                <div class="col-md-3">
                                                    <asp:Label CssClass="sp-label" ID="Label14" runat="server" Text="Quarter"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListQuarterFilter2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListQuarterFilter2_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row margin-bottom-10">
                                                <div class="col-md-3">
                                                    <asp:Label CssClass="sp-label" ID="Label6" runat="server" Text="Course Instance"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseInstanceFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseInstanceFilter_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="row margin-bottom-10">
                                                <div class="col-md-3">
                                                    <asp:Label CssClass="sp-label" ID="Label9" runat="server" Text="Module Objective"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListModuleObjectiveFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListModuleObjectiveFilter_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="row margin-bottom-10">
                                                <div class="col-md-3">
                                                    <asp:Label ID="Label5" CssClass="sp-label" runat="server" Text="Select a Coding Problem"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCodingProblem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCodingProblem_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="row margin-bottom-15">
                                    <div class="col-md-12">
                                        <hr />
                                        <h5>Or, Add New Coding Problem</h5>
                                        <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-9">
                                <div class="wraper-area">
                                    <div class="custome-form-group">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <h6 class="margin-bottom-10">Coding Problem</h6>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="create-activity-area">
                                                <%--<span class="display-block">Search Module Objective</span>
                                                <div class="custome-form-group margin-top-10">
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
                                                <%--    <div class="custome-form-group">
                                                    <asp:Label CssClass="sp-label" ID="LabelModuleObjective" runat="server" Text="Module Obj."></asp:Label>
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListModuleObjective" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListModuleObjective_SelectedIndexChanged"></asp:DropDownList>
                                                </div>--%>                                                <%--<hr />--%>                                                <%--  <div>
                                                    <asp:Label ID="Label5" runat="server" Text="Select a Coding Problem"></asp:Label>
                                                    <asp:DropDownList CssClass="tex-box form-control margin-top-10" ID="DropDownListCodingProblem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCodingProblem_SelectedIndexChanged"></asp:DropDownList>
                                                    <br />
                                                </div>--%>

                                                <asp:TextBox CssClass="custome-textarea form-control" ID="TextBoxInstruction" runat="server" placeholder="Instruction" TextMode="MultiLine"></asp:TextBox>
                                                <asp:TextBox CssClass="form-control font-size-13" ID="TextBoxScript" runat="server" placeholder="Script" TextMode="MultiLine"></asp:TextBox>
                                                <asp:TextBox CssClass="form-control font-size-13" ID="TextBoxSolution" placeholder="Solution" runat="server" TextMode="MultiLine"></asp:TextBox>

                                                <div>
                                                    <h5>Excel Solution:</h5>
                                                    <br />
                                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                                    <br />
                                                    <br />
                                                    <asp:Button ID="btnUploadFile" CssClass="btn btn-custom btn-sm" runat="server" OnClick="btnUploadFile_Click" Text="Upload" />
                                                    <br />
                                                    <asp:Label ID="lblmessageFile" ForeColor="Red" runat="server" /><br />
                                                </div>

                                                <asp:TextBox CssClass="tex-box form-control" ID="TextBoxClassName" placeholder="Class Name" runat="server"></asp:TextBox>
                                                <asp:TextBox CssClass="tex-box form-control" ID="TextBoxMethodName" placeholder="Method Name" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="create-activity-area">
                                                <asp:TextBox CssClass="tex-box form-control" ID="TextBoxParameterTypes" placeholder="Parameter Types" runat="server"></asp:TextBox>
                                                <%--<asp:TextBox CssClass="tex-box form-control" ID="TextBoxLanguage" placeholder="Language" runat="server"></asp:TextBox>--%>
                                                <div class="row">
                                                <div class="col-md-4">
                                                    <asp:Label CssClass="sp-label" ID="Label17" runat="server" Text="Language"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListLanguage" runat="server" AutoPostBack="True"></asp:DropDownList>
                                                </div>
                                            </div>
                                                <asp:TextBox CssClass="tex-box form-control" ID="TextBoxTestCaseClass" placeholder="Test Case Class" runat="server"></asp:TextBox>
                                                <asp:TextBox CssClass="form-control font-size-13" ID="TextBoxBefore" placeholder="Before" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                <asp:TextBox CssClass="form-control font-size-13" ID="TextBoxAfter" placeholder="After" runat="server" TextMode="MultiLine"></asp:TextBox>

                                                <asp:TextBox CssClass="tex-box form-control" ID="TextBoxTitle" placeholder="Title" runat="server"></asp:TextBox>
                                                <asp:TextBox CssClass="tex-box form-control margin-bottom-8" ID="TextBoxMaxGrade" type="number" placeholder="Max Grade" runat="server"></asp:TextBox>
                                              <%--  <asp:TextBox CssClass="tex-box form-control" ID="TextBoxType" placeholder="Type" runat="server"></asp:TextBox>--%>
                                                <div>
                                                    <asp:Label CssClass="sp-label" ID="Label15" runat="server" Text="Type: "></asp:Label>
                                                     &nbsp; &nbsp;
                                                    <asp:CheckBox ID="CheckBoxTypeCode" runat="server" />
                                                    <asp:Label ID="Label16" runat="server" Text="code"></asp:Label>
                                                </div>
                                                <asp:TextBox CssClass="tex-box form-control margin-bottom-8" ID="TextBoxAttempts" type="number" placeholder="Attempts" runat="server"></asp:TextBox>
                                                <asp:TextBox CssClass="tex-box form-control margin-bottom-8" ID="TextBoxRole" type="number" placeholder="Role" runat="server"></asp:TextBox>
                                                <div class="margin-bottom-8 margin-top-10">
                                                    <div class="row">
                                                        <div class="col-md-3">
                                                            <asp:Label CssClass="sp-label" ID="Label4" runat="server" Text="Active"></asp:Label>
                                                    &nbsp;   <asp:CheckBox ID="CheckBoxActive" runat="server" Checked="true" />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label CssClass="sp-label" ID="Label10" runat="server" Text="Uses Hint"></asp:Label>
                                                    <asp:CheckBox ID="CheckboxUsesHint" runat="server" />
                                                        </div>
                                                    </div>
                                                    
                                              
                                                </div>
                                            <div class="margin-top-10">
                                                <asp:Button ID="btnAddNewCodingProblem" runat="server" CssClass="btn btn-custom btn-sm" Text="Add Coding" OnClick="btnAddNewCodingProblem_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnUpdateCodingProblem" runat="server" CssClass="btn btn-custom btn-sm" Text="Update Coding" OnClick="btnUpdateCodingProblem_Click" />
                                                &nbsp;
                                            </div>
                                            </div>


                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="wraper-area">

                                    <h6>Add New Test</h6>
                                    <%-------------------------------------%>
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="create-test-area">
                                                <br />
                                                <%--<asp:Label CssClass="sp-label" ID="LabelScript" runat="server" Text="Script"></asp:Label>--%>
                                                <asp:TextBox CssClass="form-control font-size-13" ID="TextBoxExpectedOutput" runat="server" placeholder="Expected Output" TextMode="MultiLine"></asp:TextBox>

                                                <asp:TextBox CssClass=" tex-box form-control " ID="TextBoxOutputType" runat="server" placeholder="Output Type"></asp:TextBox>

                                                <%--<asp:Label CssClass="sp-label " ID="LabelInstruction" runat="server" Text="Instruction"></asp:Label>--%>
                                                <asp:TextBox CssClass="form-control tex-box" ID="TextBoxParameterValues" runat="server" placeholder="Parameter Values"></asp:TextBox>

                                                <asp:Label CssClass="sp-label" ID="LabelOutPutException" runat="server" Text="Output Exception"></asp:Label>
                                                &nbsp;
                                                 <asp:CheckBox ID="CheckBoxOutPutException" runat="server" />
                                                <br />
                                                <asp:Label CssClass="sp-label" ID="LabelAutoGenerated" runat="server" Text="Auto Generated"></asp:Label>
                                                &nbsp;
                                                <asp:CheckBox ID="CheckBoxAutoGenerated" runat="server" />

                                                <div class="margin-top-10">
                                                    <asp:Button ID="AddNewTest" runat="server" CssClass="btn btn-custom btn-sm" Text="Add New Test" OnClick="AddNewTest_Click" />

                                                    <br />
                                                    <br />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <br />
                                <div class="wraper-area">
                                    <span>Course Instance & Coding Problem List</span>
                                    <div class="custome-overflow margin-top-10" style="max-height: 260px; height: auto">
                                        <asp:GridView CssClass="table table-bordered table-fixed" ID="GridView1" runat="server" DataKeyNames="CourseInstanceId" OnRowCancelingEdit="OnRowCancelingEdit" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:TemplateField HeaderText="CodingProblemId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LabelCodingProblemId" runat="server" Text='<%# Bind("CodingProblemId") %>'></asp:Label>
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
                                                <asp:BoundField DataField="CodingProblem" HeaderText="CodingProblem" ReadOnly="True" />
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
                                                        <asp:LinkButton ID="LinkButtonDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <br />
                                <div class="wraper-area">
                                    <span>Test List</span>
                                    <div class="custome-overflow margin-top-5" style="max-height: 260px; height: auto">
                                        <div>
                                            <asp:GridView CssClass="table table-bordered table-fixed" ID="GridViewTest" runat="server" DataKeyNames="Id" OnRowCancelingEdit="OnRowCancelingEditTest" OnRowEditing="OnRowEditingTest" OnRowUpdating="OnRowUpdatingTest" OnRowDataBound="OnRowDataBoundTest" OnRowDeleting="OnRowDeletingTest" AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" />
                                                    <asp:TemplateField HeaderText="Parameter Values">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox1" CssClass="wrape-text-area  form-control custome-textarea" runat="server" Text='<%# Bind("ParameterValues") %>' TextMode="MultiLine"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label1" CssClass="wrape-text-area " runat="server" Text='<%# Bind("ParameterValues") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Expected Output">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox2" CssClass="wrape-text-area form-control custome-textarea" runat="server" Text='<%# Bind("ExpectedOutput") %>' TextMode="MultiLine"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label2" CssClass="wrape-text-area" runat="server" Text='<%# Bind("ExpectedOutput") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="OutputException">
                                                        <EditItemTemplate>
                                                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("OutputException") %>' />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("OutputException") %>' Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Autogenerated ">
                                                        <EditItemTemplate>
                                                            <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("Autogenerated") %>' />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBo2" runat="server" Checked='<%# Bind("Autogenerated") %>' Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField ShowEditButton="True" />
                                                    <asp:TemplateField ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButtonDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
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
                </div>
            </div>
        </form>
    </section>
    <%--===================================================================================--%>    <%--========================================footer area=================================--%>
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
    <%--=============================================================================--%>    <%: Scripts.Render("~/bundles/js") %>
    <script src="Scripts/jquery-ui.min.js"></script>
    <script src="Scripts/select2.min.js"></script>
    <%--<script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>--%>
    <script>

        // $(function () {
        //     $("[id*=GridView1]").DataTable(
        //        {
        //            bLengthChange: true,
        //            //lengthMenu: [[5, 10, -1], [5, 10, "All"]],
        //            bFilter: true,
        //            bSort: true,
        //            bPaginate: false
        //        });
        //});

        $(document).ready(function () {
            $("#DropDownListCourseFilter1").select2();
            $("#DropDownListCourseFilter2").select2();
            $("#DropDownListQuarter").select2();

            $("#DropDownListCourseInstance").select2();
            $("#DropDownListModuleObjective").select2();
            $("#DropDownListCourseInstanceFilter").select2();
            $("#DropDownListModuleObjectiveFilter").select2();
            $("#DropDownListCodingProblem").select2();
        });
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
        })
    </script>
</body>
</html>
