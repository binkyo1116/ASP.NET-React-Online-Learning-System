<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCourseInstance.aspx.cs" Inherits="OnlineLearningSystem.AddCourseInstance" %>

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
                        <div class="col-md-4">
                            <div class="course-instance">
                                <h5>Add New Course Instance</h5>
                                <hr />
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Label CssClass="sp-label" ID="LableCrouse" runat="server" Text="Course "></asp:Label>
                                    </div>
                                    <div class="col-md-9 margin-bottom-10">
                                        <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourse" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Label CssClass="sp-label" ID="Label16" runat="server" Text="Quarter "></asp:Label>
                                    </div>
                                    <div class="col-md-9 margin-bottom-10">
                                        <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListQuarter" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label CssClass="sp-label" ID="Label4" runat="server" Text="Active"></asp:Label>
                                        &nbsp;
                            <asp:CheckBox ID="CheckBoxActive" runat="server" Checked="true" />
                                    </div>
                                    <div class="col-md-8">
                                        <div class="text-right">
                                            <asp:Button ID="AddNewCourseInstance" runat="server" CssClass="btn btn-custom" Text="Submit" OnClick="AddNewCourseInstance_Click" />
                                            <br />
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <asp:Label ID="lblMessageCourseInstance" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
                                <asp:Label ID="lblErrorMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md">
                            <div class="margin-left-30 ">
                                <h5>Course Instance List</h5>
                                <hr />
                                <div class="row">
                                    <div class="col-md-9 margin-bottom-15">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:Label CssClass="sp-label" ID="Label11" runat="server" Text="Select a School"></asp:Label>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListSearchBySchool" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListSearchBySchool_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md margin-bottom-15 text-right">

                                        <asp:Button ID="ShowAllList" runat="server" CssClass="btn btn-sm btn-custom-light" Text="Show All" OnClick="ShowAllList_Click" />
                                    </div>
                                </div>
                                <div class="custome-overflow">
                                    <asp:GridView CssClass="table table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCancelingEdit="OnRowCancelingEdit" OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating">
                                        <Columns>
                                            <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" Visible="False" />
                                            <asp:TemplateField HeaderText="QuarterId" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("QuarterId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CourseId" HeaderText="CourseId" Visible="False" />
                                            <asp:BoundField DataField="CourseInstance" HeaderText="Course Instance" ReadOnly="True" />
                                            <asp:BoundField DataField="Course" HeaderText="Course" ReadOnly="True" />
                                            <asp:BoundField DataField="School" HeaderText="School" ReadOnly="True" />
                                            <asp:TemplateField HeaderText="Quarter">
                                                <EditItemTemplate>
                                                    <asp:DropDownList CssClass="form-control font-size-13 textbox-width" ID="DropDownListQuarter" runat="server"></asp:DropDownList>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Quarter") %>'></asp:Label>
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
            $("#DropDownListCourse").select2();
            $("#DropDownListQuarter").select2();
        });
    </script>
</body>
</html>
