<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddStudentCourse.aspx.cs" Inherits="OnlineLearningSystem.AddStudentCourse" %>

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
                              <h5>Student & Course</h5>
                                    <hr />
                        </div>
                        <div class="col-sm-9">
                            <div class="wraper-area AddStudentcourse-list">
                                <div class="row">
                                    <div class="col-sm-6">
                                        <asp:Label CssClass="sp-label" ID="Label11" runat="server" Text="Select Student"></asp:Label>
                                        <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListStudents" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListStudents_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <hr />
                                <h6>Or Add New Student</h6>
                                <div class="row">
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
                                        <asp:Button ID="btnStudentDelete" OnClientClick="return confirm('Do you want to delete this?');" runat="server" BackColor="Red" CssClass="btn btn-custom btn-sm margin-bottom-15" Text="Delete" OnClick="btnStudentDelete_Click" />
                                        &nbsp;
                                        <asp:Label ID="Label1" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label><br />
                                    </div>
                                    <div class="col-sm-5 text-right">
                                        <asp:Button ID="Button4" CssClass="btn btn-custom-light btn-sm margin-bottom-15" runat="server" Text="Clear All" OnClick="btnClearAll_Click" />
                                        &nbsp;
                                        <asp:Button ID="Button5" CssClass="btn btn-custom-light btn-sm margin-bottom-15" runat="server" Text="Student List" OnClick="btnStudentListPage_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="wraper-area add-new-course-objective">
                                <h6>Add New Course</h6>
                                <hr />
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Label CssClass="sp-label text-bold" ID="Label10" runat="server" Text="Student ID:"></asp:Label>
                                        <asp:Label ID="LabelStudentId" runat="server" Text=""></asp:Label>
                                        <br />
                                        <asp:Label CssClass="sp-label" ID="LableCrouse" runat="server" Text="Course Instance: "></asp:Label>
                                        <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourse" runat="server"></asp:DropDownList>
                                        <br />
                                        <br />
                                        <div class="text-right margin-bottom-30">
                                            <asp:Button ID="AddNewCourse" runat="server" CssClass="btn btn-custom btn-sm" Text="Add New Course" OnClick="AddNewCourse_Click" />
                                            <asp:Label ID="lblMessage" ForeColor="Green" CssClass="sp-label margin-top-10" runat="server"></asp:Label>
                                            <asp:Label ID="lblErrorMessage" ForeColor="Red" CssClass="sp-label margin-top-10" runat="server"></asp:Label>
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="wraper-area margin-top-15">
                                <h6>Course List</h6>
                                <div class="custome-overflow margin-top-10" style="max-height: 400px; height: auto">
                                    <asp:GridView CssClass="table table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="CourseInstanceId" OnRowDeleting="OnRowDeleting" OnRowDataBound="OnRowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="CourseInstanceId" HeaderText="Id" ReadOnly="True" Visible="False" />
                                            <asp:BoundField DataField="Course" HeaderText="Course" ReadOnly="True" />
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
            $("#DropDownListCourse").select2();

        });
    </script>
</body>
</html>

