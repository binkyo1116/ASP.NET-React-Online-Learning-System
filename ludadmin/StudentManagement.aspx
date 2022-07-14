<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentManagement.aspx.cs" Inherits="OnlineLearningSystem.StudentManagement" %>
<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Let's Use Data</title>
    <link rel="favicon icon" href="favicon.png" type="image/png" />
    <link href="Content/select2.css" rel="stylesheet" />
    <%: Styles.Render("~/Content/css") %>
    <style>
        a.cell-custome {
            display: block;
            text-align: center;
        }

        a.cell-width-90 {
            width: 90px !important;
        }

        a.cell-width-60 {
            width: 60px !important;
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
                            <p align="right">
                                <asp:Button ID="btnAddCourse" CssClass="btn btn-custom btn-sm" runat="server" Text="Add Student & Course" OnClick="btnAddCourse_Click" />
                                &nbsp;
                                <%-- <button type="button" class="btn btn-custom show-modal" data-toggle="modal" data-target="#exampleModal">Add New Student</button>--%>
                                <uc1:Logout ID="ctlLogout" runat="server" />
                            </p>
                        </div>
                        <div class="col-sm-12">
                            <div class="student-list">
                                <h4>Student List</h4>
                                <hr />
                                <div class="row margin-bottom-15">
                                    <div class="col-md-1">
                                        <asp:Label CssClass="font-size-15" ID="Label1" runat="server" Text="Student"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList CssClass="tex-box form-control" ID="ddStudents" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddStudents_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <asp:Label ID="lblMessage" runat="server" Style="font: bold 12px sans-serif"></asp:Label>

                                <div class="custome-overflow">
                                    <asp:GridView CssClass="table table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="StudentId" OnRowCancelingEdit="OnRowCancelingEdit" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowCommand="GridView1_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="StudentId" ItemStyle-HorizontalAlign="Center" HeaderText="Student ID" ReadOnly="True" />
                                            <asp:TemplateField HeaderText="Name">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Email">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("Email") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Canvas ID">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("CanvasId") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("CanvasId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mark">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("Mark") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("Mark") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="User Name">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("UserName") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Password">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("Password") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("Password") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField ControlStyle-CssClass="cell-custome cell-width-60" ShowEditButton="True" />
                                            <%--    <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButtonDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:ButtonField ControlStyle-CssClass="cell-custome cell-width-90" CommandName="Select" Text="Add Course" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--=============================Modal============================================--%>
            <!-- Modal -->
            <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true" style="">
                <div class="modal-dialog" role="document" style="max-width: 730px;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="exampleModalLabel">Add New Material</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <%-----------------------------------------%>
                            <div class="container">
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div>
                                            <%--<label class="control-label">Name:</label>--%>
                                            <asp:Label CssClass="sp-label" ID="LabelCourseObjecctive" runat="server" Text="Name: "></asp:Label>
                                            <asp:TextBox ID="TextBoxName" runat="server" CssClass="form-control tex-box" placeholder="Full Name"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name is required" ControlToValidate="TextBoxName" Font-Size="Small" ForeColor="Red" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                            <br />
                                            <%--<label class="control-label">Email:</label>--%>
                                            <asp:Label CssClass="sp-label" ID="Label2" runat="server" Text="Email: "></asp:Label>
                                            <asp:TextBox ID="TextBoxEmail" TextMode="Email" runat="server" CssClass="form-control tex-box" placeholder="E-mail"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Email is required" ControlToValidate="TextBoxEmail" Font-Size="Small" ForeColor="Red" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid Email format " ControlToValidate="TextBoxEmail" EnableTheming="True" Font-Size="Small" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="Insert"></asp:RegularExpressionValidator>
                                            <br />
                                            <%--<label class="control-label">Mark:</label>--%>
                                            <%-- <asp:Label CssClass="sp-label" ID="Label3" runat="server" Text="Confirm E-mail: "></asp:Label>
                                             <asp:TextBox ID="TextBoxConfirmEmail" runat="server" CssClass="form-control tex-box" placeholder="Confirm E-mail"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Confirm E-mail is required" ControlToValidate="TextBoxConfirmEmail" Font-Size="Small" ForeColor="Red" ></asp:RequiredFieldValidator>
                                             <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="E-mail does not match" ControlToCompare="TextBoxEmail" ControlToValidate="TextBoxConfirmEmail" Font-Size="Small" ForeColor="Red"></asp:CompareValidator>
                                             <br />--%>

                                            <asp:Label CssClass="sp-label" ID="Label7" runat="server" Text="CanvasId: "></asp:Label>
                                            <asp:TextBox ID="TextBoxCanvasId" runat="server" CssClass="form-control tex-box" placeholder="CanvasId"></asp:TextBox>
                                            <br />

                                            <asp:Button ID="AddNewStudent" runat="server" CssClass="btn btn-custom" Text="Submit" OnClick="AddNewStudent_Click" ValidationGroup="Insert" />

                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:Label CssClass="sp-label" ID="Label8" runat="server" Text="Mark: "></asp:Label>
                                        <asp:TextBox ID="TextBoxMark" runat="server" CssClass="form-control tex-box" placeholder="Mark"></asp:TextBox>
                                        <br />
                                        <%--<label class="control-label">User Name:</label>--%>
                                        <asp:Label CssClass="sp-label" ID="Label4" runat="server" Text="User Name: "></asp:Label>
                                        <asp:TextBox ID="txtUser" runat="server" CssClass="form-control tex-box" placeholder="User Name"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="User Name is required" ControlToValidate="txtUser" Font-Size="Small" ForeColor="Red" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                        <br />
                                        <%--<label class="control-label">Password:</label>--%>
                                        <asp:Label CssClass="sp-label" ID="Label5" runat="server" Text="Password: "></asp:Label>
                                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control tex-box" placeholder="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Password is required" ControlToValidate="txtPassword" Font-Size="Small" ForeColor="Red" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                        <br />
                                        <%--<label class="control-label">Confirm Password:</label>--%>
                                        <%--         <asp:Label CssClass="sp-label" ID="Label6" runat="server" Text="Confirm Password: "></asp:Label>
                                            <asp:TextBox ID="TextBoxConfirmPassword" runat="server" TextMode="Password" CssClass="form-control tex-box" placeholder="Confirm Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Confirm Password is required" ControlToValidate="TextBoxConfirmPassword" Font-Size="Small" ForeColor="Red"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Password does not match" ControlToCompare="txtPassword" ControlToValidate="TextBoxConfirmPassword" Font-Size="Small" ForeColor="Red"></asp:CompareValidator>--%>
                                    </div>
                                </div>
                            </div>
                            <%-----------------------------------------%>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-light" data-dismiss="modal">Close</button>
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
            $(".show-modal").click(function () {
                $("#exampleModal").modal({
                    backdrop: 'static',
                    keyboard: false
                });
            });
            $("#ddStudents").select2();
        });
    </script>
</body>
</html>

