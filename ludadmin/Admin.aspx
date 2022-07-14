<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="OnlineLearningSystem.Admin" %>
<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Let's Use Data</title>
    <link rel="favicon icon" href="favicon.png" type="image/png" />
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
        <div class="add-course-area">
            <form id="form1" runat="server">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-sm-2 border-right">
                            <div class="admin-left-panel" style="margin: 10px; font-size: 15px">
                                <div class="nav flex-column nav-pills" id="v-pills-tab" role="tablist" aria-orientation="vertical">
                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-home" data-toggle="pill" href="#v-pills-home-tab" role="tab" aria-controls="v-pills-home-tab" aria-selected="true">Add and List Page</a>
                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-profile" data-toggle="pill" href="#v-pills-profile-tab" role="tab" aria-controls="v-pills-profile-tab" aria-selected="false">Syllabus</a>
                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-coding-problem" data-toggle="pill" href="#v-pills-coding-problem-tab" role="tab" aria-controls="v-pills-coding-broblem-tab" aria-selected="false">Coding Problem</a>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-9">
                            <div class="Admin-area margin-top-10 margin-left-right-30">
                                <div class="add-course-area">
                                    <h5>Admin Panel</h5>
                                    <hr />
                                    <div class="tab-content" id="v-pills-tabContent">
                                        <div class="tab-pane fade" id="v-pills-home-tab" role="tabpanel" aria-labelledby="v-pills-home">
                                            <asp:Button ID="Button4" runat="server" PostBackUrl="~/AddCourse.aspx" Text="Add Course" />
                                            <asp:Button ID="Button5" runat="server" PostBackUrl="~/AddCourseObjective.aspx" Text="Add Course Objective" />
                                            <asp:Button ID="Button7" runat="server" PostBackUrl="~/AddMaterial.aspx" Text="Add Material" />
                                            <asp:Button ID="Button8" runat="server" PostBackUrl="~/AddModule.aspx" Text="Add Module" />
                                            <asp:Button ID="Button9" runat="server" PostBackUrl="~/AddModuleObjective.aspx" Text="Add Module Objective" />
                                            <asp:Button ID="Button16" runat="server" PostBackUrl="~/AddCourseInstance.aspx" Text="Add Course Instance" />
                                            <asp:Button ID="Button11" runat="server" PostBackUrl="~/AddSchool.aspx" Text="Add School" />

                                            <asp:Button ID="Button12" runat="server" PostBackUrl="~/AddQuizQuestion.aspx" Text="Add Quiz Question" />
                                            <asp:Button ID="Button13" runat="server" PostBackUrl="~/AddStudentCourse.aspx" Text="Add Student Course" />
                                            <asp:Button ID="Button33" runat="server" PostBackUrl="~/ApproveRequestLogin.aspx" Text="Approve Request Login" />
                                            <asp:Button ID="Button6" runat="server" PostBackUrl="~/DiscussionBoardManagement.aspx" Text="Discussion Board" />
                                            <asp:Button ID="Button10" runat="server" PostBackUrl="~/AddAnnouncement.aspx" Text="Add Announcement" />
                                            <asp:Button ID="Button18" runat="server" PostBackUrl="~/AddGradeScale.aspx" Text="Add Grade Scale" />
                                            <asp:Button ID="Button1" runat="server" PostBackUrl="~/GradeBook.aspx" Text="Gradebook" />
                                            <asp:Button ID="Button29" runat="server" PostBackUrl="~/GradeBookSummary.aspx" Text="Gradebook Summary" />
                                            <asp:Button ID="Button32" runat="server" PostBackUrl="~/SubmissionGrade.aspx" Text="Student Submission" />

                                            <asp:Button ID="Button14" runat="server" PostBackUrl="~/AddPoll.aspx" Text="Add Poll" />
                                            <asp:Button ID="Button2" runat="server" PostBackUrl="~/AddQuizHint.aspx" Text="Quiz Hinting System" />
                                            <asp:Button ID="Button15" runat="server" PostBackUrl="~/ImageSubmission.aspx" Text="Image Submission" />

                                            <asp:Button ID="Button19" runat="server" PostBackUrl="~/StudentManagement.aspx" Text="Student List" />
                                            <asp:Button ID="Button25" runat="server" PostBackUrl="~/StudentSubmittedFile.aspx" Text="Excel Files" />
                                            <asp:Button ID="Button26" runat="server" PostBackUrl="~/HintManagement.aspx" Text="Hint Managment" />
                                            <asp:Button ID="Button27" runat="server" PostBackUrl="~/SupportTicketManagement.aspx" Text="Ticketing System Managment" />
                                             <asp:Button ID="Button34" runat="server" PostBackUrl="~/FeedbackManagement.aspx" Text="Feedback Managment" />
                                            <asp:Button ID="Button28" runat="server" PostBackUrl="~/HtmlHintManagement.aspx" Text="HTML Hint Managment" />

                                        </div>
                                        <%--===============================Panel Syllabus===============================================================--%>
                                        <div class="tab-pane fade" id="v-pills-profile-tab" role="tabpanel" aria-labelledby="v-pills-profile">
                                            <asp:Button ID="Button24" runat="server" PostBackUrl="~/AddQuarter.aspx" Text="Quarter, Non Academic Day, Session, Course Requisite, Instruction Method" />
                                            <asp:Button ID="Button17" runat="server" PostBackUrl="~/AddInstructor.aspx" Text="Add Instructor" />
                                            <asp:Button ID="Button20" runat="server" PostBackUrl="~/AddCourseRequirements.aspx" Text="Add Course Requirements" />
                                            <asp:Button ID="Button21" runat="server" PostBackUrl="~/AddGradingPolicy.aspx" Text="Add Grading Policy" />
                                            <asp:Button ID="Button22" runat="server" PostBackUrl="~/AddSchoolPolicyAndOthers.aspx" Text="Course Policy, Community Standard, Campus Public Safty, Support Service" />
                                            <asp:Button ID="Button23" runat="server" PostBackUrl="~/AddNetiquette.aspx" Text="Add Netiquette" />
                                        </div>
                                        <%--===============================Panel Coding Problelm===============================================================--%>
                                        <div class="tab-pane fade" id="v-pills-coding-problem-tab" role="tabpanel" aria-labelledby="v-pills-coding-problem">
                                            <asp:Button ID="Button3" runat="server" PostBackUrl="~/AddCodingProblem.aspx" Text="Add Coding Problem" />
                                            <asp:Button ID="Button31" runat="server" PostBackUrl="~/UpdateCodingProblem.aspx" Text="Update Coding Problem" />
                                            <asp:Button ID="Button30" runat="server" PostBackUrl="~/AssignCodingProblemToCourseInstance.aspx" Text="Assign Coding Problem to Course Instance" />
                                        </div>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>

                </div>
            </form>
        </div>
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
    <script>
        $(document).ready(function () {
            //----------------Remove Previous active class--------------------
            if (sessionStorage.getItem('btnIdAdmin') != null) {
                $(".nav-link").removeClass('active');
                $(".tab-pane").removeClass('active');
                $(".tab-pane").removeClass('show');

                //------------------Add active class------------------------------
                var id = sessionStorage.getItem('btnIdAdmin')
                $("#" + id).addClass('active');
                $("#" + id + "-tab").addClass('active');
                $("#" + id + "-tab").addClass('show');
            }
            else {
                $("#v-pills-home").addClass('active');
                $("#v-pills-home-tab").addClass('active');
                $("#v-pills-home-tab").addClass('show');
            }
        });
        function setActiveClass(id) {
            sessionStorage.setItem('btnIdAdmin', id);
        }


        $(document).ready(function () {

        });
    </script>
</body>
</html>
