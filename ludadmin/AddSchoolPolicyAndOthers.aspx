<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddSchoolPolicyAndOthers.aspx.cs" Inherits="OnlineLearningSystem.AddSchoolPolicyAndOthers" %>

<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>
<%@ Register Src="~/AddCoursePolicy.ascx" TagPrefix="uc1" TagName="AddCoursePolicy" %>
<%@ Register Src="~/AddCommunityStandard.ascx" TagPrefix="uc2" TagName="AddCommunityStandard" %>
<%@ Register Src="~/AddCampusPublicSafety.ascx" TagPrefix="uc3" TagName="AddCampusPublicSafety" %>
<%@ Register Src="~/AddSupportService.ascx" TagPrefix="uc4" TagName="AddSupportService" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<link rel="favicon icon" href="favicon.png" type="image/png">
<head runat="server">
    <title>Let's Use Data</title>
    <%: Styles.Render("~/Content/css") %>
    <%-----------------custome style----------------------%>
    <style type="text/css">
        table {
            border: solid 1px #dee2e6;
        }

            table td {
                border-right: solid 1px #dee2e6;
                border-bottom: solid 1px #dee2e6;
            }

            table th {
                border-bottom: solid 1px #dee2e6;
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
    <section class="content-area margin-top-15 margin-bottom-100 ">
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
                        <div class="col-sm-2 border-right">
                            <div class="poll-left-panel">

                                <div class="nav flex-column nav-pills" id="v-pills-tab" role="tablist" aria-orientation="vertical">
                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-CoursePolicy" data-toggle="pill" href="#v-pills-CoursePolicy-tab" role="tab" aria-controls="v-pills-CoursePolicy" aria-selected="false"><i class="fa fa-list fa-1x "></i>&nbsp; Course Policy</a>
                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-CommunityStandard" data-toggle="pill" href="#v-pills-CommunityStandard-tab" role="tab" aria-controls="v-pills-CommunityStandard-tab" aria-selected="true"><i class="fa fa-list fa-1x "></i>&nbsp; Community Standard</a>
                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-CampusPublicSafety" data-toggle="pill" href="#v-pills-CampusPublicSafety-tab" role="tab" aria-controls="v-pills-CampusPublicSafety-tab" aria-selected="true"><i class="fa fa-list fa-1x "></i>&nbsp; Campus Public Safety</a>
                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-SupportService" data-toggle="pill" href="#v-pills-SupportService-tab" role="tab" aria-controls="v-pills-SupportService-tab" aria-selected="true"><i class="fa fa-list fa-1x "></i>&nbsp; SupportService</a>

                                </div>
                            </div>
                        </div>
                        <div class="col-sm-10">
                            <div class="Poll-area margin-top-10 margin-left-right-30">
                                <div class="tab-content" id="v-pills-tabContent">
                                    <%--======================================== =====================================--%>
                                    <div class="tab-pane fade" id="v-pills-CoursePolicy-tab" role="tabpanel" aria-labelledby="v-pills-CoursePolicy">
                                        <uc1:AddCoursePolicy runat="server" ID="AddCoursePolicy" />
                                    </div>
                                    <%--============================================================================--%>
                                    <div class="tab-pane fade" id="v-pills-CommunityStandard-tab" role="tabpanel" aria-labelledby="v-pills-CommunityStandard">
                                        <uc2:AddCommunityStandard runat="server" ID="AddCommunityStandard" />
                                    </div>
                                    <%--============================================================================--%>
                                    <div class="tab-pane fade" id="v-pills-CampusPublicSafety-tab" role="tabpanel" aria-labelledby="v-pills-CampusPublicSafety">
                                        <uc3:AddCampusPublicSafety runat="server" ID="AddCampusPublicSafety" />
                                    </div>
                                    <%--============================================================================--%>
                                    <div class="tab-pane fade" id="v-pills-SupportService-tab" role="tabpanel" aria-labelledby="v-pills-SupportService">
                                        <uc4:AddSupportService runat="server" ID="AddSupportService" />
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
    <script>
        $(document).ready(function () {
            //----------------Remove Previous active class--------------------
            if (sessionStorage.getItem('btnIdCoursePolicy') != null) {
                $(".nav-link").removeClass('active');
                $(".tab-pane").removeClass('active');
                $(".tab-pane").removeClass('show');

                //------------------Add active class------------------------------
                var id = sessionStorage.getItem('btnIdCoursePolicy')
                $("#" + id).addClass('active');
                $("#" + id + "-tab").addClass('active');
                $("#" + id + "-tab").addClass('show');
            }
            else {
                $("#v-pills-CoursePolicy").addClass('active');
                $("#v-pills-CoursePolicy-tab").addClass('active');
                $("#v-pills-CoursePolicy-tab").addClass('show');
            }
            //===============================================================
        });
        function setActiveClass(id) {
            sessionStorage.setItem('btnIdCoursePolicy', id);
        }


        $(document).ready(function () {

        });
    </script>

</body>
</html>
