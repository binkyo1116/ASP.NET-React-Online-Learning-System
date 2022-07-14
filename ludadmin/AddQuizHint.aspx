<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddQuizHint.aspx.cs" Inherits="OnlineLearningSystem.AddQuizHint" %>

<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Let's Use Data</title>
    <link rel="favicon icon" href="favicon.png" type="image/png" />
    <link href="Content/select2.css" rel="stylesheet" />
    <%: Styles.Render("~/Content/css") %>
    <%------------------------custome style-------------------------%>
    <style type="text/css">
        .table-layout-fixed {
            margin-right: 0px;
        }

        .student-answer-td {
            display: block;
            width: 210px;
        }

        table tbody tr th {
            text-align: center;
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
            <div class="add-poll-area" style="margin: 0 60px;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <p align="right">
                                <uc1:Logout ID="ctlLogout" runat="server" />
                            </p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="margin-top-10">
                                <div>
                                    <%--===========================================DropDown List=================================--%>
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div>
                                                <h5>Quiz Hint</h5>
                                                <hr />
                                                <div class="create-activity-area">
                                                    <div class="row">
                                                        <div class="col-md-2">
                                                            <div>
                                                                <br />
                                                                <div class="margin-bottom-10">
                                                                    <asp:RadioButton ID="RadioButton1" runat="server" GroupName="searchType" Checked="true" AutoPostBack="True" OnCheckedChanged="RadioButton1_CheckedChanged" />
                                                                    <asp:Label ID="Label5" CssClass="font-size-15" runat="server" Text="Not Reviewed"></asp:Label>
                                                                </div>
                                                                <div class="margin-bottom-15">
                                                                    <asp:RadioButton ID="RadioButton2" runat="server" GroupName="searchType" AutoPostBack="True" OnCheckedChanged="RadioButton2_CheckedChanged" />
                                                                    <asp:Label ID="Label8" CssClass="font-size-15" runat="server" Text="Find for a Quiz"></asp:Label>
                                                                </div>
                                                                <br />

                                                            </div>
                                                        </div>
                                                        <div class="col-md-10">
                                                            <div class="row">
                                                                <div class="col-md-4">
                                                                    <div>
                                                                        <asp:Label CssClass="sp-label" ID="Label11" runat="server" Text="Course Instance"></asp:Label>
                                                                        <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseInstance" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseInstance_SelectedIndexChanged"></asp:DropDownList>

                                                                        <asp:Label CssClass="sp-label" ID="Label12" runat="server" Text="Course Obj."></asp:Label>
                                                                        <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseObjective" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseObjective_SelectedIndexChanged"></asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div>
                                                                        <asp:Label CssClass="sp-label" ID="Label16" runat="server" Text="Module"></asp:Label>
                                                                        <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListModule" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListModule_SelectedIndexChanged"></asp:DropDownList>

                                                                        <asp:Label CssClass="sp-label" ID="LabelModuleObjective" runat="server" Text="Module Obj."></asp:Label>
                                                                        <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListModuleObjective" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListModuleObjective_SelectedIndexChanged"></asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div>
                                                                        <asp:Label CssClass="sp-label" ID="Label13" runat="server" Text="Select Activity"></asp:Label>
                                                                        <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListActivity" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListActivity_SelectedIndexChanged"></asp:DropDownList>
                                                                        <br />
                                                                        <br />
                                                                        <asp:Button ID="btnClearAll" CssClass="btn btn-custom-light btn-sm" runat="server" Text="Refresh" OnClick="btnClearAll_Click" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%--========================================Add Panel =====================================--%>
                                    <div>
                                        <div class="row">
                                            <div class="col-sm-12 margin-top-10">
                                                <div>
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <span>Add Quiz Hint</span>
                                                        </div>
                                                        <div class="col-md-8">
                                                            <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
                                                        </div>
                                                        <%-- <div class="col-md-6">
                                                            <asp:Label ID="Label4" CssClass="font-size-15" runat="server" Text="Filter By: "></asp:Label>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:RadioButton ID="RadioButton1" runat="server" GroupName="searchType" Checked="true" AutoPostBack="True" OnCheckedChanged="RadioButton1_CheckedChanged" />
                                                            <asp:Label ID="Label5" CssClass="font-size-15" runat="server" Text="Empty"></asp:Label>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                                             <asp:RadioButton ID="RadioButton2" runat="server" GroupName="searchType" AutoPostBack="True" OnCheckedChanged="RadioButton2_CheckedChanged" />
                                                            <asp:Label ID="Label8" CssClass="font-size-15" runat="server" Text="All"></asp:Label>
                                                        </div>--%>
                                                    </div>
                                                    <hr />
                                                    <div class="custome-overflow">
                                                        <asp:GridView CssClass="table table-bordered table-fixed" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="GridView1_RowCommand">
                                                            <Columns>
                                                                <%-- <asp:CommandField ShowEditButton="True" />--%>
                                                                <asp:BoundField DataField="Id" HeaderText="Id" Visible="False" />
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Question Id">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("QuestionId") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="StudentAnswer">
                                                                    <ItemTemplate>
                                                                        <asp:Label CssClass="student-answer-td" ID="LabelIncorrectAns" runat="server" Text='<%# Bind("StudentAnswer") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Expected" HeaderText="Expected" ReadOnly="True" />
                                                                <asp:TemplateField HeaderText="QuizQuestion">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="TextBox6" CssClass="form-control custome-textarea" runat="server" Text='<%# Bind("QuizQuestion") %>' TextMode="multiline"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("QuizQuestion") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Correct">
                                                                    <ItemTemplate>
                                                                        <span class="text-center display-block">
                                                                            <asp:CheckBox ID="CheckBoxCorrect" runat="server" Checked='<%# Bind("Correct") %>' />
                                                                        </span>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Hint">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="TextBoxHint" CssClass="form-control input-text-fix" runat="server" Text='<%# Bind("Hint") %>' TextMode="MultiLine"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Submit" ShowHeader="False">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="LinkButton1" CssClass="btn btn-custom btn-sm margin-top-10" runat="server" CausesValidation="false" CommandName="SubmitHint" CommandArgument='<%# Container.DataItemIndex %>' Text="Submit"></asp:LinkButton>
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
        $(function () {
            $("#DropDownListCourseInstance").select2();
            $("#DropDownListCourseObjective").select2();
            $("#DropDownListModule").select2();
            $("#DropDownListModuleObjective").select2();
            $("#DropDownListActivity").select2();

        })
    </script>
</body>
</html>
