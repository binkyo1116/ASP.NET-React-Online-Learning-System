<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApproveRequestLogin.aspx.cs" Inherits="OnlineLearningSystem.ApproveRequestLogin" %>

<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

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
                                <uc1:Logout ID="ctlLogout" runat="server" />
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <h5>Approve Student's Login Request</h5>
                            <hr />
                        </div>
                        <div class="col-sm-12">
                            <div class="wraper-area margin-bottom-15">
                                <div class="col-md-12">
                                    <asp:RadioButton ID="RadioButtonPending" runat="server" GroupName="searchType" Checked="true" AutoPostBack="True" OnCheckedChanged="RadioButtonPending_CheckedChanged" />
                                    <asp:Label ID="Label3" CssClass="font-size-15 margin-right-15" runat="server" Text="Pending"></asp:Label>

                                    <asp:RadioButton ID="RadioButtonApproved" runat="server" GroupName="searchType" AutoPostBack="True" OnCheckedChanged="RadioButtonApproved_CheckedChanged" />
                                    <asp:Label ID="Label9" CssClass="font-size-15 margin-right-15" runat="server" Text="Approved"></asp:Label>

                                    <asp:RadioButton ID="RadioButtonReject" runat="server" GroupName="searchType" AutoPostBack="True" OnCheckedChanged="RadioButtonReject_CheckedChanged" />
                                    <asp:Label ID="Label6" CssClass="font-size-15" runat="server" Text="Reject"></asp:Label>
                                </div>
                                <div class="custome-overflow margin-top-15" style="max-height: 200px; height: auto">
                                    <asp:GridView CssClass="table table-bordered" ID="GridViewRequestLogin" runat="server" AutoGenerateColumns="False" DataKeyNames="RequestLoginId" OnRowCommand="GridViewRequestLogin_RowCommand" OnRowDataBound="GridViewRequestLogin_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="RequestLoginId" ItemStyle-HorizontalAlign="Center" HeaderText="ID" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Name" HeaderText="Student Name" />
                                            <asp:BoundField DataField="SchoolName" HeaderText="School Name" ReadOnly="True" />
                                            <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
                                            <asp:BoundField DataField="Email" HeaderText="Email" />
                                            <asp:TemplateField HeaderText="Time Stamp">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("TimeStamp", "{0:MMM-dd-yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Approval Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("ApprovalStatus") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Approve" ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButtonApprove" runat="server" CommandArgument='<%# Eval("RequestLoginId") %>' CausesValidation="false" CommandName="Approve" Text="Approve"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:ButtonField Text="Approve" CommandName="Approve" HeaderText="Approve" />--%>
                                            <asp:TemplateField HeaderText="Reject" ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButtonReject" runat="server" CommandArgument='<%# Eval("RequestLoginId") %>' CausesValidation="false" CommandName="Reject" Text="Reject"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Status" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-sm-6">
                                    <asp:Label CssClass="sp-label" ID="Label10" runat="server" Text="Search Student"></asp:Label>
                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListStudents" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListStudents_SelectedIndexChanged"></asp:DropDownList>
                               <br /><br />
                                    </div>
                            </div>
                        </div>

                        <div class="col-sm-9">
                            <asp:Panel ID="PanelAddStudent" runat="server" Visible="false">
                                <div class="wraper-area AddStudentcourse-list">
                                    <h6>Add New Student</h6>
                                    <div class="row margin-top-15">
                                        <div class="col-sm-6">
                                            <div>
                                                <%--<label class="control-label">Name:</label>--%>
                                                <div class="row">
                                                    <div class="col-sm-3">
                                                        <asp:Label CssClass="sp-label" ID="LabelCourseObjecctive" runat="server" Text="Name: "></asp:Label>
                                                    </div>
                                                    <div class="col-sm-9">
                                                        <asp:TextBox ID="TextBoxName" runat="server" CssClass="form-control tex-box" placeholder="Full Name"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name is required" ControlToValidate="TextBoxName" Font-Size="Small" ForeColor="Red" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-3">
                                                        <asp:Label CssClass="sp-label" ID="Label2" runat="server" Text="Email: "></asp:Label>
                                                    </div>
                                                    <div class="col-sm-9">
                                                        <asp:TextBox ID="TextBoxEmail" TextMode="Email" runat="server" CssClass="form-control tex-box" placeholder="E-mail"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Email is required" ControlToValidate="TextBoxEmail" Font-Size="Small" ForeColor="Red" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid Email format " ControlToValidate="TextBoxEmail" EnableTheming="True" Font-Size="Small" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="Insert"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-3">
                                                        <asp:Label CssClass="sp-label" ID="Label7" runat="server" Text="CanvasId: "></asp:Label>
                                                    </div>
                                                    <div class="col-sm-9">
                                                        <asp:TextBox ID="TextBoxCanvasId" type="number" runat="server" CssClass="form-control tex-box" placeholder="CanvasId"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="row">
                                                <div class="col-sm-3">
                                                    <asp:Label CssClass="sp-label" ID="Label8" runat="server" Text="Mark: "></asp:Label>
                                                </div>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TextBoxMark" type="number" runat="server" CssClass="form-control tex-box" placeholder="Mark"></asp:TextBox>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-sm-3">
                                                    <asp:Label CssClass="sp-label" ID="Label4" runat="server" Text="User Name: "></asp:Label>
                                                </div>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="txtUser" runat="server" CssClass="form-control tex-box" placeholder="User Name"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="User Name is required" ControlToValidate="txtUser" Font-Size="Small" ForeColor="Red" ValidationGroup="Insert"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-3">
                                                    <asp:Label CssClass="sp-label" ID="Label5" runat="server" Text="Password: "></asp:Label>
                                                </div>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control tex-box" placeholder="Password"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Password is required" ControlToValidate="txtPassword" Font-Size="Small" ForeColor="Red" ValidationGroup="Insert"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:Button ID="btnAddNewStudent" runat="server" CssClass="btn btn-custom btn-sm margin-bottom-15" Text="Add New Student" OnClick="btnAddNewStudent_Click" ValidationGroup="Insert" />
                                            &nbsp;
                                     <asp:Button ID="btnStudentUpdate" runat="server" CssClass="btn btn-custom btn-sm margin-bottom-15" Text="Update" OnClick="btnStudentUpdate_Click" />
                                            &nbsp;
                                   <%--  <asp:Button ID="btnStudentDelete" OnClientClick="return confirm('Do you want to delete this?');" runat="server" BackColor="Red" CssClass="btn btn-custom btn-sm margin-bottom-15" Text="Delete" OnClick="btnStudentDelete_Click" />
                                        &nbsp;--%>
                                            <asp:Label ID="Label1" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label><br />
                                        </div>
                                        <div class="col-sm-5 text-right">
                                            <asp:Button ID="Button4" CssClass="btn btn-custom-light btn-sm margin-bottom-15" runat="server" Text="Clear All" OnClick="btnClearAll_Click" />
                                            &nbsp;
                                        <asp:Button ID="Button5" CssClass="btn btn-custom-light btn-sm margin-bottom-15" runat="server" Text="Student List" OnClick="btnStudentListPage_Click" />
                                        </div>
                                        <asp:Label ID="lblMessage" ForeColor="Green" CssClass="sp-label margin-top-10" runat="server"></asp:Label>
                                    <asp:Label ID="lblErrorMessage" ForeColor="Red" CssClass="sp-label margin-top-10" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="col-sm-3">
                            <asp:Panel ID="PanelAddCourse" Visible="false" runat="server">
                                <div class="wraper-area add-new-course-objective">
                                    <h6>Add New Course</h6>
                                    <hr />
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <%--<asp:Label CssClass="sp-label text-bold" ID="Label10" runat="server" Text="Student ID:"></asp:Label>--%>
                                            <asp:Label ID="LabelStudentId" Visible="false" runat="server" Text=""></asp:Label>


                                            <asp:Label CssClass="sp-label text-bold" ID="Label11" runat="server" Text="Course Selected by Student:"></asp:Label>
                                            <asp:TextBox ID="TextBoxCourseByStudetn" runat="server" CssClass="form-control tex-box"></asp:TextBox>

                                            <asp:Label CssClass="sp-label" ID="Label12" runat="server" Text="Course"></asp:Label>
                                            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseFilter2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseFilter2_SelectedIndexChanged"></asp:DropDownList>

                                            <asp:Label CssClass="sp-label" ID="Label14" runat="server" Text="Quarter"></asp:Label>
                                            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListQuarterFilter2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListQuarterFilter2_SelectedIndexChanged"></asp:DropDownList>

                                            <asp:Label CssClass="sp-label" ID="LableCrouse" runat="server" Text="Course Instance: "></asp:Label>
                                            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseInstance" runat="server"></asp:DropDownList>

                                            <div class="text-right margin-top-15">
                                                <asp:Button ID="AddNewCourse" runat="server" CssClass="btn btn-custom btn-sm" Text="Add New Course" OnClick="AddNewCourse_Click" />

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="col-sm-6">
                            <asp:Panel ID="PanelCourseList" Visible="false" runat="server">
                                <div class="wraper-area margin-top-15">
                                    <h6>Course List</h6>
                                    <div class="custome-overflow margin-top-10" style="max-height: 400px; height: auto">
                                        <asp:GridView CssClass="table table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="CourseInstanceId" OnRowDeleting="OnRowDeleting" OnRowDataBound="OnRowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="CourseInstanceId" HeaderText="Id" ReadOnly="True" Visible="False" />
                                                <asp:BoundField DataField="Course" HeaderText="Course" ReadOnly="True" />
                                                <asp:TemplateField ShowHeader="true">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButtonDelete" ForeColor="Red" runat="server" CausesValidation="true" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </asp:Panel>
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
    <script src="Scripts/select2.min.js"></script>
    <script>
        $(document).ready(function () {
            $("#DropDownListStudents").select2();
            $("#DropDownListCourseInstance").select2();
            $("#DropDownListCourseFilter2").select2();
            $("#DropDownListQuarterFilter2").select2();
        });
    </script>
</body>
</html>

