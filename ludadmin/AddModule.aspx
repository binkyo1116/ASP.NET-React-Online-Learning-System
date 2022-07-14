<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddModule.aspx.cs" Inherits="OnlineLearningSystem.AddModule" %>

<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Let's Use Data</title>
    <link rel="favicon icon" href="favicon.png" type="image/png" />
    <link href="Content/select2.css" rel="stylesheet" />
    <link href="Content/jquery-ui.css" rel="stylesheet" />
    <%: Styles.Render("~/Content/css") %>
    <style>
        span.select2-dropdown {
            width: 700px !important;
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
            <div class="add-course-area">
                <div class="add-course-area" style="margin: 0 60px;">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <p align="right">
                                    <uc1:Logout ID="ctlLogout" runat="server" />
                                </p>
                            </div>
                        </div>
                        <div class="add-new-module">
                            <div class="row">
                                <div class="col-md-5">
                                    <div class="margin-bottom-10">
                                        <h5>Course Objective & Module</h5>
                                        <hr />
                                        <div class="wraper-area margin-bottom-10">
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <asp:Label CssClass="font-size-15" ID="Label11" runat="server" Text="Course"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourse" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourse_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row margin-bottom-10">
                                            <div class="col-md-3">
                                                <asp:Label CssClass="font-size-15" ID="LabelCourseObjecctive" runat="server" Text="Course Obj.: "></asp:Label>
                                            </div>
                                            <div class="col-md">
                                                <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseObjective" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseObjective_SelectedIndexChanged1"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="row margin-bottom-15">
                                            <div class="col-md-3">
                                                <asp:Label CssClass="font-size-15" ID="Label2" runat="server" Text="Module: "></asp:Label>
                                            </div>
                                            <div class="col-md">
                                                <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListModule" runat="server"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="text-right">
                                            <asp:Button ID="btnClearAll" CssClass="btn btn-custom-light btn-sm" runat="server" Text="Clear All" OnClick="btnClearAll_Click" />
                                            &nbsp;
                                            <asp:Button ID="ButtonCourseObjectiveModule" runat="server" CssClass="btn btn-custom btn-sm " Text="Submit" OnClick="ButtonCourseObjectiveModule_Click" />
                                        </div>
                                    </div>
                                    <div>
                                        <h5>Add New Module</h5>
                                        <hr />
                                        <div class="row margin-bottom-10">
                                            <div class="col-md-4">
                                                <asp:Label CssClass="font-size-15" ID="LabelDueDate" runat="server" Text="Due Date"></asp:Label>
                                            </div>
                                            <div class="col-md">
                                                <div style="position: relative">
                                                    <asp:TextBox CssClass="tex-box form-control DatePicker" ID="TextBoxDueDate" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:TextBox CssClass=" form-control margin-bottom-15 font-size-15" ID="TextBoxDescription" runat="server" placeholder="Description" TextMode="MultiLine"></asp:TextBox>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <asp:Label CssClass="font-size-15" ID="Label4" runat="server" Text="Active"></asp:Label>
                                                &nbsp;
                                         <asp:CheckBox ID="CheckBoxActive" runat="server" Checked="true" />
                                            </div>
                                            <div class="col-md-6 text-right">
                                                <asp:Button ID="AddNewModule" runat="server" CssClass="btn btn-custom btn-sm" Text="Add New" OnClick="AddNewModule_Click" />
                                            </div>
                                        </div>
                                        <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
                                         <asp:Label ID="lblErrorMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label><br />
                                    </div>

                                </div>
                                <div class="col-md">
                                    <div class="course-list margin-left-30">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <h5>Module List</h5>
                                            </div>
                                            <div class="col-md-6 text-right">
                                                <asp:Button ID="ShowAllModuleList" runat="server" CssClass="btn btn-sm btn-custom-light" Text="Show All" OnClick="ShowAllModuleList_Click" />
                                            </div>
                                        </div>
                                        <hr style="margin-top:5px" />
                                        <div class="custome-overflow">
                                            <asp:GridView CssClass="table table-bordered grid-table" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowDeleting="OnRowDeleting" OnRowCancelingEdit="OnRowCancelingEdit" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowDataBound="OnRowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Id" ReadOnly="True" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Description">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Description") %>' TextMode="MultiLine" CssClass="form-control custome-textarea"></asp:TextBox>
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
                                                    <asp:TemplateField HeaderText="Due Date">
                                                        <EditItemTemplate>
                                                            <div style="position: relative">
                                                                <asp:TextBox ID="TextBox2" CssClass="tex-box form-control DatePicker" runat="server" Text='<%# Eval("DueDate", "{0:MMM-dd-yyyy}") %>'></asp:TextBox>
                                                            </div>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("DueDate", "{0:MMM-dd-yyyy}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField ShowEditButton="True" />
                                                    <asp:TemplateField ShowHeader="True" Visible="true">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButtonDelete" runat="server" ForeColor="Red" CausesValidation="true" CommandName="Delete" Text="Delete"></asp:LinkButton>
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
            $(document).ready(function () {
                $("#DropDownListCourse").select2();
                $("#DropDownListCourseObjective").select2();
                $("#DropDownListModule").select2();
            });
        })
    </script>
</body>
</html>
