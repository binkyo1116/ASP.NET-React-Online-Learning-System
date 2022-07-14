<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StatisticsPage.aspx.cs" Inherits="OnlineLearningSystem.StatisticsPage" %>

<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<link rel="favicon icon" href="favicon.png" type="image/png">
<head runat="server">
    <title>Let's Use Data</title>
    <%: Styles.Render("~/Content/css") %>
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
                        <a href="CourseSelection.aspx" class="navbar-brand padding-left-15">Online Learning System</a>
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
    <%--=====================================================================================--%>
    <%--========================================Content area=================================--%>
    <section class="content-area margin-top-15 margin-left-right-30">
        <form id="form1" runat="server">
            <div class="container">
                <div class="row">
                    <div class="col-md-12">
                        <p align="right">
                            <uc1:Logout ID="ctlLogout" runat="server" />
                        </p>
                    </div>
                </div>
            </div>
            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="margin-top-15">
                            <div>
                                <%--========================================Quiz Grade =====================================--%>
                                <div class="quiz-grade-chart-area">
                                    <div>
                                        <h5>Online Grading Statistics </h5>
                                        <hr />

                                        <div class="row">
                                            <div class="col-md-6">
                                                <%------------------------------------Course List-----------------------------------------%>
                                                <asp:Panel ID="PanelCourseList" runat="server">
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <asp:Label ID="Label1" CssClass="sp-label" runat="server" Text="Select Course"></asp:Label>
                                                        </div>
                                                        <div class="col-md-8">
                                                            <asp:DropDownList ID="ddnCourses" runat="server" CssClass="form-control tex-box" AutoPostBack="True" OnSelectedIndexChanged="ddnCourses_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                <%---------------------------------------------------------------------------------------%>
                                            </div>
                                            <div class="col-md-6">
                                                <%------------------------------------For Admin-----------------------------------------%>
                                                <asp:Panel ID="PanelStudentList" runat="server" Visible="false">
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <asp:Label ID="Label2" CssClass="sp-label" runat="server" Text="Select Student"></asp:Label>
                                                        </div>
                                                        <div class="col-md-8">
                                                            <asp:DropDownList ID="DropDownListStudent" runat="server" CssClass="form-control tex-box" AutoPostBack="True" OnSelectedIndexChanged="DropDownListStudent_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                <%---------------------------------------------------------------------------------------%>
                                            </div>
                                        </div>
                                        <br />
                                    </div>
                                    <div class="course-static-area">
                                        <%-- -----------------------------calculate the students current and overall grade--------------------------------%>
                                        <asp:Panel ID="PanelOverallGrade" Visible="false" runat="server">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <div>
                                                        <h6>Student Current and Overall Grade</h6>
                                                        <table class="table table-bordered font-size-13 margin-top-15">
                                                            <thead>
                                                                <tr>
                                                                    <th></th>
                                                                    <th>Overall Grade</th>
                                                                    <th>Current Grade</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <tr>
                                                                    <td>Assessments</td>
                                                                    <td>45%</td>
                                                                    <td>
                                                                        <asp:Label ID="LabelCurrentAssessmentGrade" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Quizzes</td>
                                                                    <td>10%</td>
                                                                    <td>
                                                                        <asp:Label ID="LabelCurrentQuizGrade" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Midterm</td>
                                                                    <td>
                                                                        <asp:Label ID="LabelOverallMidterm" runat="server" Text=""></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="LabelCurrentMidtermGrade" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Final</td>
                                                                    <td>
                                                                        <asp:Label ID="LabelOverallFinal" runat="server" Text=""></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label ID="LabelCurrentFinalGrade" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr class="bg-info">
                                                                    <td><strong>Total</strong></td>
                                                                    <td>100%</td>
                                                                    <td>
                                                                        <asp:Label ID="LabelTotalGrade" runat="server" Text=""></asp:Label>
                                                                        &nbsp&nbsp (GPA &nbsp
                                                                        <asp:Label ID="LabelGPA" runat="server" Text=""></asp:Label>)
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <%-----------------------------------------------------------------------------------------------------------------------------%>
                                        <%-- --------------------------------------------------------------------------------%>
                                        <hr />
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div style="overflow-x: auto">
                                                    <div id="headingQuiz"></div>
                                                    <div id="my_dataviz"></div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div style="overflow-x: auto">
                                                    <div id="headingAss"></div>
                                                    <div id="my_assessment_chart"></div>
                                                </div>
                                            </div>
                                        </div>
                                        <%-- --------------------------------------------------------------------------------%>
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
    <!-- Load d3.js -->
    <script src="https://d3js.org/d3.v4.js"></script>
    <script src="Scripts/d3_Js/d3_statistics.js"></script>

</body>
</html>
