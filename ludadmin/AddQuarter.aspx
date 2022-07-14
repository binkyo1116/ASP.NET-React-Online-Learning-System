<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddQuarter.aspx.cs" Inherits="OnlineLearningSystem.AddQuarter" %>

<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>
<%@ Register Src="~/AddCoursePrerequisite.ascx" TagPrefix="uc1" TagName="AddCoursePrerequisite" %>
<%@ Register Src="~/AddInstructionMethod.ascx" TagPrefix="uc1" TagName="AddInstructionMethod" %>
<%@ Register Src="~/AddNonAcademicDay.ascx" TagPrefix="uc1" TagName="AddNonAcademicDay" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Let's Use Data</title>
    <link rel="favicon icon" href="favicon.png" type="image/png" />
    <link href="Content/select2.css" rel="stylesheet" />
    <%: Styles.Render("~/Content/css") %>

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
    <style type="text/css">
     /*   .btn-sm {
            height: 26px;
        }*/
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
            <div style="margin: 0 60px;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <p align="right">
                                <uc1:Logout ID="ctlLogout" runat="server" />
                            </p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2 border-right">
                            <div class="poll-left-panel">
                                <div class="nav flex-column nav-pills" id="v-pills-tab" role="tablist" aria-orientation="vertical">
                                    <asp:Button ID="btnClearAll" CssClass="btn btn-custom-light btn-sm margin-bottom-10" runat="server" Text="Reload page" OnClick="btnClearAll_Click" />

                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-quarter" data-toggle="pill" href="#v-pills-quarter-tab" role="tab" aria-controls="v-pills-quarter-tab" aria-selected="true"><i class="fa fa-list fa-1x "></i>&nbsp; Quarter</a>
                                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-NonAcademicDay" data-toggle="pill" href="#v-pills-NonAcademicDay-tab" role="tab" aria-controls="v-pills-NonAcademicDay-tab" aria-selected="true"><i class="fa fa-list fa-1x "></i>&nbsp;Non Academic Day</a>
                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-session" data-toggle="pill" href="#v-pills-session-tab" role="tab" aria-controls="v-pills-session-tab" aria-selected="true"><i class="fa fa-list fa-1x "></i>&nbsp;Session</a>
                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-CourseRequisite" data-toggle="pill" href="#v-pills-CourseRequisite-tab" role="tab" aria-controls="v-pills-CourseRequisite-tab" aria-selected="true"><i class="fa fa-list fa-1x "></i>&nbsp;Course Requisite</a>
                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-InstructionMethod" data-toggle="pill" href="#v-pills-InstructionMethod-tab" role="tab" aria-controls="v-pills-InstructionMethod-tab" aria-selected="true"><i class="fa fa-list fa-1x "></i>&nbsp;Instruction Method</a>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-10">
                            <div class="Poll-area margin-left-right-30">
                                <div class="tab-content" id="v-pills-tabContent">
                                    <%--========================================Quarter Panel =====================================--%>
                                    <div class="tab-pane fade" id="v-pills-quarter-tab" role="tabpanel" aria-labelledby="v-pills-quarter">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="quarter-area">
                                                    <h6>Add New Quarter</h6>
                                                    <hr />
                                                    <asp:Label CssClass="sp-label" ID="Label8" runat="server" Text="School "></asp:Label>
                                                    <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListSchool" runat="server"></asp:DropDownList>

                                                    <asp:Label CssClass="sp-label" ID="lblName" runat="server" Text="Name: "></asp:Label>
                                                    <asp:TextBox CssClass="tex-box form-control margin-bottom-10" ID="TextBoxName" runat="server"></asp:TextBox>

                                                    <asp:Label CssClass="sp-label" ID="Label9" runat="server" Text="Start Date "></asp:Label>
                                                    <div style="position: relative">
                                                        <asp:TextBox CssClass="tex-box form-control DatePicker margin-bottom-10" ID="TextBoxStartDate" placeholder="Start Date" runat="server"></asp:TextBox>
                                                    </div>

                                                    <asp:Label CssClass="sp-label" ID="Label17" runat="server" Text="End Date "></asp:Label>
                                                    <div style="position: relative">
                                                        <asp:TextBox CssClass="tex-box form-control DatePicker margin-bottom-10" ID="TextBoxEndDate" placeholder="End Date" runat="server"></asp:TextBox>
                                                    </div>

                                                    <asp:Label CssClass="sp-label" ID="Label18" runat="server" Text="Withdraw  Date "></asp:Label>
                                                    <div style="position: relative">
                                                        <asp:TextBox CssClass="tex-box form-control DatePicker margin-bottom-10" ID="TextBoxWithdrawDate" placeholder="Withdraw Date" runat="server"></asp:TextBox>
                                                    </div>

                                                    <asp:Label CssClass="sp-label" ID="Label14" runat="server" Text="Active"></asp:Label>
                                                    &nbsp;
                                    <asp:CheckBox ID="CheckBoxQuarterActive" runat="server" Checked="true" />
                                                    <br />
                                                    <div class="text-right">
                                                        <asp:Button ID="btnAddQuarter" runat="server" CssClass="btn btn-custom btn-sm" Text="Add" OnClick="btnAddQuarter_Click" />
                                                        <%-- <asp:Button ID="btnUpdateQuarter" runat="server" CssClass="btn btn-custom-light btn-sm" Text="Update" OnClick="btnUpdateQuarter_Click" />
                                                    <asp:Button ID="btnDeleteQuarter" runat="server" CssClass="btn btn-custom-light btn-sm" Text="Delete" OnClick="btnDeleteQuarter_Click" />--%>
                                                        <br />
                                                        <asp:Label ID="lblMessageQuarter" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
                                                        <asp:Label ID="lblErrorMessageQuarter" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-9">
                                                <div class="margin-left-30 ">
                                                    <h6>Quarter List</h6>
                                                    <hr />
                                                    <div class="custome-overflow">
                                                        <asp:GridView CssClass="table table-bordered grid-table" ID="GridViewQuarter" runat="server" AutoGenerateColumns="False" DataKeyNames="QuarterId" OnRowCancelingEdit="GridViewQuarter_RowCancelingEdit" OnRowDataBound="GridViewQuarter_RowDataBound" OnRowDeleting="GridViewQuarter_RowDeleting" OnRowEditing="GridViewQuarter_RowEditing" OnRowUpdating="GridViewQuarter_RowUpdating">
                                                            <Columns>
                                                                <asp:BoundField DataField="QuarterId" ItemStyle-HorizontalAlign="Center" HeaderText="ID" ReadOnly="True" />
                                                                <asp:TemplateField HeaderText="SchoolId" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("SchoolId") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="School">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListSchoolGV" runat="server"></asp:DropDownList>

                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("School") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Name">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="StartDate">
                                                                    <EditItemTemplate>

                                                                        <div style="position: relative">
                                                                            <asp:TextBox ID="TextBox1" CssClass="tex-box form-control DatePicker input-text-fix-160" runat="server" Text='<%# Eval("StartDate", "{0:MMM-dd-yyyy}") %>'></asp:TextBox>
                                                                        </div>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("StartDate", "{0:MMM-dd-yyyy}") %>'></asp:Label>

                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="EndDate">
                                                                    <EditItemTemplate>
                                                                        <div style="position: relative">
                                                                            <asp:TextBox ID="TextBox2" CssClass="tex-box form-control DatePicker input-text-fix-160" runat="server" Text='<%# Eval("EndDate", "{0:MMM-dd-yyyy}") %>'></asp:TextBox>
                                                                        </div>

                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("EndDate", "{0:MMM-dd-yyyy}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="WithdrawDate">
                                                                    <EditItemTemplate>
                                                                        <div style="position: relative">
                                                                            <asp:TextBox ID="TextBox3" CssClass="tex-box form-control DatePicker input-text-fix-160" runat="server" Text='<%# Eval("WithdrawDate", "{0:MMM-dd-yyyy}") %>'></asp:TextBox>
                                                                        </div>

                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("WithdrawDate", "{0:MMM-dd-yyyy}") %>'></asp:Label>
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
                                    <%--========================================Session Panel =====================================--%>
                                    <div class="tab-pane fade" id="v-pills-session-tab" role="tabpanel" aria-labelledby="v-pills-session">
                                        <h5>Course Instance & Session</h5>
                                        <hr />
                                        <div class="row">
                                            <div class="col-md-3">
                                                <asp:Label CssClass="sp-label" ID="Label10" runat="server" Text="Select a Course Instance "></asp:Label>
                                                <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListCourseInstance" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseInstance_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:Label CssClass="sp-label" ID="Label2" runat="server" Text="Select a Session "></asp:Label>
                                                <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListSession" runat="server" AutoPostBack="True"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:CheckBox ID="CheckBoxActive" runat="server" Checked="true" />
                                                &nbsp;
                                            <asp:Label CssClass="sp-label" ID="Label4" runat="server" Text="Active"></asp:Label>
                                                <br />
                                                <asp:Button ID="btnAddCourseInstanceSession" runat="server" CssClass="btn btn-custom btn-sm" Text="Submit" OnClick="btnAddCourseInstanceSession_Click" />
                                            </div>
                                            <div class="col-md-3">
                                                <asp:Label ID="lblMessageSession" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
                                                <asp:Label ID="lblErrorMessageSession" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="quarter-area wraper-area">
                                                    <h5>Or, Add New Session</h5>
                                                    <hr />
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <asp:Label CssClass="sp-label" ID="Label12" runat="server" Text="Lecture Day"></asp:Label>
                                                        </div>
                                                        <div class="col-md-8">
                                                            <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListLectureDay" runat="server"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <asp:Label CssClass="sp-label" ID="Label13" runat="server" Text="Start Time "></asp:Label>
                                                            <div style="position: relative">
                                                                <asp:TextBox CssClass="tex-box form-control  margin-bottom-10" type="time" ID="TextBoxStartTime" placeholder="Start Date" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label CssClass="sp-label" ID="Label15" runat="server" Text="End Time "></asp:Label>
                                                            <div style="position: relative">
                                                                <asp:TextBox CssClass="tex-box form-control margin-bottom-10" type="time" ID="TextBoxEndTime" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <asp:Label CssClass="sp-label" ID="Label22" runat="server" Text="Description "></asp:Label>
                                                    <asp:TextBox CssClass="form-control font-size-13 margin-bottom-10" ID="TextBoxLocationDescription" runat="server" TextMode="MultiLine"></asp:TextBox>

                                                    <asp:Label CssClass="sp-label" ID="Label19" runat="server" Text="Location "></asp:Label>
                                                    <asp:TextBox CssClass="form-control font-size-13 margin-bottom-10" ID="TextBoxLocation" runat="server" TextMode="MultiLine"></asp:TextBox>

                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <asp:Label CssClass="sp-label" ID="Label20" runat="server" Text="Active"></asp:Label>
                                                            &nbsp;
                                                <asp:CheckBox ID="CheckBoxSessionActive" runat="server" Checked="true" />
                                                        </div>
                                                        <div class="col-md-6 text-right">
                                                            <asp:Button ID="btnAddSession" runat="server" CssClass="btn btn-custom btn-sm" Text="Add Session" OnClick="btnAddSession_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-8">
                                                <div class="margin-bottom-10 wraper-area">
                                                    <h5>Course Instance Session List</h5>
                                                    <hr />
                                                    <div class="custome-overflow margin-top-5" style="max-height: 260px; height: auto">
                                                        <asp:Label ID="Label23" runat="server" Text="" Visible="false"></asp:Label>
                                                        <asp:GridView CssClass="table table-bordered grid-table" ID="GridViewCourseInstanceSession" runat="server" AutoGenerateColumns="False" DataKeyNames="SessionId" OnRowCancelingEdit="GridViewCourseInstanceSession_RowCancelingEdit" OnRowDataBound="GridViewCourseInstanceSession_RowDataBound" OnRowDeleting="GridViewCourseInstanceSession_RowDeleting" OnRowEditing="GridViewCourseInstanceSession_RowEditing" OnRowUpdating="GridViewCourseInstanceSession_RowUpdating">
                                                            <Columns>
                                                                <asp:BoundField DataField="SessionId" HeaderText="SessionId" ReadOnly="True" Visible="False" />
                                                                <asp:TemplateField HeaderText="LectureDay">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("LectureDay") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="StartTime">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("StartTime") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="EndTime">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("EndTime") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Location">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
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
                                                <div class="wraper-area">
                                                    <h5>Session List</h5>
                                                    <hr />
                                                    <div class="custome-overflow margin-top-5" style="max-height: 260px; height: auto">
                                                        <asp:Label ID="LabelSelectedCourseId" runat="server" Text="" Visible="false"></asp:Label>
                                                        <asp:GridView CssClass="table table-bordered grid-table" ID="GridViewSession" runat="server" AutoGenerateColumns="False" DataKeyNames="SessionId" OnRowCancelingEdit="GridViewSession_RowCancelingEdit" OnRowDataBound="GridViewSession_RowDataBound" OnRowDeleting="GridViewSession_RowDeleting" OnRowEditing="GridViewSession_RowEditing" OnRowUpdating="GridViewSession_RowUpdating">
                                                            <Columns>
                                                                <asp:BoundField DataField="SessionId" HeaderText="ID" ReadOnly="True" />
                                                                <asp:TemplateField HeaderText="LectureDay">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="TextBox1" CssClass="form-control font-size-13" runat="server" Text='<%# Bind("LectureDay") %>'></asp:TextBox>

                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("LectureDay") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="StartTime">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="TextBox2" type="time" CssClass="form-control font-size-13" runat="server" Text='<%# Bind("StartTime") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("StartTime") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="EndTime">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="TextBox3" type="time" CssClass="form-control font-size-13" runat="server" Text='<%# Bind("EndTime") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("EndTime") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Location">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="TextBox4" CssClass="form-control font-size-13" runat="server" TextMode="MultiLine" Text='<%# Bind("Location") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Description">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="TextBox5" CssClass="form-control font-size-13" runat="server" TextMode="MultiLine" Text='<%# Bind("Description") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
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

                                    <%--========================================Course Requisite Panel =====================================--%>
                                      <div class="tab-pane fade" id="v-pills-NonAcademicDay-tab" role="tabpanel" aria-labelledby="v-pills-NonAcademicDay">
                                        <uc1:AddNonAcademicDay runat="server" id="AddNonAcademicDay" />
                                    </div>
                                    <div class="tab-pane fade" id="v-pills-CourseRequisite-tab" role="tabpanel" aria-labelledby="v-pills-session">
                                        <uc1:AddCoursePrerequisite runat="server" ID="AddCoursePrerequisite" />
                                    </div>
                                    <div class="tab-pane fade" id="v-pills-InstructionMethod-tab" role="tabpanel" aria-labelledby="v-pills-session">
                                        <uc1:AddInstructionMethod runat="server" ID="AddInstructionMethod" />
                                    </div>
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
        });
        $(document).ready(function () {
            //----------------Remove Previous active class--------------------
            if (sessionStorage.getItem('btnIdQuarter') != null) {
                $(".nav-link").removeClass('active');
                $(".tab-pane").removeClass('active');
                $(".tab-pane").removeClass('show');

                //------------------Add active class------------------------------
                var id = sessionStorage.getItem('btnIdQuarter')
                $("#" + id).addClass('active');
                $("#" + id + "-tab").addClass('active');
                $("#" + id + "-tab").addClass('show');
            }
            else {
                $("#v-pills-quarter").addClass('active');
                $("#v-pills-quarter-tab").addClass('active');
                $("#v-pills-quarter-tab").addClass('show');
            }
            //========================================================================
            $("#DropDownListCourseInstance").select2();
            $("#AddCoursePrerequisite_DropDownListCourse").select2();
            $("#AddCoursePrerequisite_DropDownListCoursePrerequisite").select2();
            $("#AddCoursePrerequisite_DropDownListCourseCorequisite").select2();

            $("#AddInstructionMethod_DropDownListCourseInstance").select2();
            $("#AddCoursePrerequisite_DropDownListCourseCorequisite").select2();
            //===============================================================
        });
        function setActiveClass(id) {
            sessionStorage.setItem('btnIdQuarter', id);
        }
    </script>
    <script>

</script>
</body>
</html>
