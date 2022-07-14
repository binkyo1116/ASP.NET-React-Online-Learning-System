<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GradeBook.aspx.cs" Inherits="OnlineLearningSystem.GradeBook" %>
<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Let's Use Data</title>
    <link rel="favicon icon" href="favicon.png" type="image/png" />
    <link href="Content/select2.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />
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
                        <p align="right">
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
    <%--==================================================================================--%>    <%--========================================Content area=================================--%>
    <section class="content-area margin-top-15 margin-bottom-100">
        <form id="form1" runat="server">
            <div class="add-course-area" style="margin: 0 60px;">
                <div class="container-fluid">
                    <p align="right">
                        <uc1:Logout ID="ctlLogout" runat="server" />
                    </p>
                    <h5>Grade Book</h5>
                    <hr />
                    <div class="row">
                        <div class="col-md-5 margin-bottom-10">
                            <div class="row">
                                <div class="col-md-4">
                                    <asp:Label CssClass="font-size-15" ID="Label8" runat="server" Text="Course Instance"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="ddCourses" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddCourses_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-5 margin-bottom-10" style="margin-left: 60px;">
                            <div class="row">
                                <div class="col-md-2">
                                    <asp:Label CssClass="font-size-15" ID="Label1" runat="server" Text="Student"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="ddStudents" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddStudents_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="row">
                                <div class="col-md-4">
                                    <asp:Label CssClass="font-size-15" ID="Label2" runat="server" Text="Module Objective"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListModuleObjective" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListModuleObjective_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-5">
                            <h5>Activity Grade</h5>
                            <hr />
                            <div class="row">
                                <div class="col-md-2">
                                    <asp:Label CssClass="font-size-15" ID="Label3" runat="server" Text="Activity"></asp:Label>
                                </div>
                                <div class="col-md-5">
                                    <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListActivity" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListActivity_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="custome-overflow">
                                <asp:GridView ID="grdActivites" CssClass="table table-bordered grid-table" OnRowDataBound="GridView1_RowDataBound" runat="server"></asp:GridView>

                            </div>
                        </div>
                        <div class="col-md-5">
                            <h5>Assessment Grade</h5>
                            <hr />
                            <div class="row">
                                <div class="col-md-2">
                                    <asp:Label CssClass="font-size-15" ID="Label4" runat="server" Text="Assessment"></asp:Label>
                                </div>
                                <div class="col-md-5">
                                    <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListAssessment" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListAssessment_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="custome-overflow">
                                <asp:GridView CssClass="table table-bordered grid-table" OnRowDataBound="GridView1_RowDataBound" ID="grdAssessments" runat="server"></asp:GridView>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <h5>Student Total Grade</h5>
                            <hr />
                            <br />
                            <div class="custome-overflow">
                                <asp:GridView CssClass="table table-bordered grid-table" OnRowDataBound="GridView1_RowDataBound" ID="grdTotalGrade" runat="server"></asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    </section>
    <%--===================================================================================--%><%--========================================footer area=================================--%>
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
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
    <script>
        $(document).ready(function () {
            $("#ddCourses").select2();
            $("#ddStudents").select2();
            $("#DropDownListModuleObjective").select2();
            $("#DropDownListModuleObjectiveFilter").select2();
            $("#DropDownListCodingProblem").select2();

         <%--   $('#<%= grdActivites.ClientID %>').DataTable()--%>
            $("[id*=grdActivites]").DataTable(
                {
                    bLengthChange: true,
                    //lengthMenu: [[5, 10, -1], [5, 10, "All"]],
                    bFilter: true,
                    bSort: true,
                    bPaginate: false
                });
            $("[id*=grdAssessments]").DataTable(
                {
                    bLengthChange: true,
                    //lengthMenu: [[5, 10, -1], [5, 10, "All"]],
                    bFilter: true,
                    bSort: true,
                    bPaginate: false
                });
            $("[id*=grdTotalGrade]").DataTable(
                {
                    bLengthChange: true,
                    //lengthMenu: [[5, 10, -1], [5, 10, "All"]],
                    bFilter: true,
                    bSort: true,
                    bPaginate: false
                });
        });
    </script>
</body>
</html>
