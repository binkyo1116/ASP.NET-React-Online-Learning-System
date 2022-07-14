<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCourseRequirements.aspx.cs" Inherits="OnlineLearningSystem.AddCourseRequirements" %>

<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>
<%@ Register Src="~/AddTechnologyRequirement.ascx" TagPrefix="uc2" TagName="AddTechnologyRequirement" %>
<%@ Register Src="~/AddTextBook.ascx" TagPrefix="uc3" TagName="AddTextBook" %>
<%@ Register Src="~/AddRequiredTool.ascx" TagPrefix="uc4" TagName="AddRequiredTool" %>
<%@ Register Src="~/AddSupplie.ascx" TagPrefix="uc5" TagName="AddSupplie" %>
<%@ Register Src="~/AddCourseMaterialRequirement.ascx" TagPrefix="uc6" TagName="AddCourseMaterialRequirement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Let's Use Data</title>
    <link rel="favicon icon" href="favicon.png" type="image/png" />
    <link href="Content/select2.css" rel="stylesheet" />
    <%: Styles.Render("~/Content/css") %>
    <%----------------------custome style-------------------%>
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
                                    <h6>Course Requirement</h6>
                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-TechnologyRequirement" data-toggle="pill" href="#v-pills-TechnologyRequirement-tab" role="tab" aria-controls="v-pills-TechnologyRequirement" aria-selected="false"><i class="fa fa-list fa-1x "></i>&nbsp; Technology</a>
                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-Textbook" data-toggle="pill" href="#v-pills-Textbook-tab" role="tab" aria-controls="v-pills-Textbook-tab" aria-selected="true"><i class="fa fa-list fa-1x "></i>&nbsp; Text book</a>
                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-RequiredTool" data-toggle="pill" href="#v-pills-RequiredTool-tab" role="tab" aria-controls="v-pills-RequiredTool-tab" aria-selected="true"><i class="fa fa-list fa-1x "></i>&nbsp; Required Tool</a>
                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-Supplie" data-toggle="pill" href="#v-pills-Supplie-tab" role="tab" aria-controls="v-pills-Supplie-tab" aria-selected="true"><i class="fa fa-list fa-1x "></i>&nbsp;Supplies</a>
                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-Material" data-toggle="pill" href="#v-pills-Material-tab" role="tab" aria-controls="v-pills-Material" aria-selected="false"><i class="fa fa-list fa-1x "></i>&nbsp;Material Requirement</a>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-10">
                            <div class="Poll-area margin-top-15 margin-left-right-30">
                                <div class="tab-content" id="v-pills-tabContent">
                                    <%--========================================Technology Requirement Panel =====================================--%>
                                    <div class="tab-pane fade" id="v-pills-TechnologyRequirement-tab" role="tabpanel" aria-labelledby="v-pills-TechnologyRequirement">
                                        <uc2:AddTechnologyRequirement runat="server" ID="AddTechnologyRequirement" />
                                    </div>
                                    <%--========================================Text book Panel =====================================--%>
                                    <div class="tab-pane fade" id="v-pills-Textbook-tab" role="tabpanel" aria-labelledby="v-pills-Textbook">
                                        <uc3:AddTextBook runat="server" ID="AddTextBook" />
                                    </div>
                                    <%--========================================Required Tool Panel =====================================--%>
                                    <div class="tab-pane fade" id="v-pills-RequiredTool-tab" role="tabpanel" aria-labelledby="v-pills-RequiredTool">
                                        <uc4:AddRequiredTool runat="server" ID="AddRequiredTool" />
                                    </div>
                                    <%--======================================== Supplie Panel =====================================--%>
                                    <div class="tab-pane fade" id="v-pills-Supplie-tab" role="tabpanel" aria-labelledby="v-pills-Supplie">
                                        <uc5:AddSupplie runat="server" ID="AddSupplie" />
                                    </div>
                                    <%--======================================== Corse Material Requirement Panel =====================================--%>
                                    <div class="tab-pane fade" id="v-pills-Material-tab" role="tabpanel" aria-labelledby="v-pills-Material">
                                        <uc6:AddCourseMaterialRequirement runat="server" ID="AddCourseMaterialRequirement" />
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
        $(document).ready(function () {
            //----------------Remove Previous active class--------------------
            if (sessionStorage.getItem('btnIdCourseReq') != null) {
                $(".nav-link").removeClass('active');
                $(".tab-pane").removeClass('active');
                $(".tab-pane").removeClass('show');

                //------------------Add active class------------------------------
                var id = sessionStorage.getItem('btnIdCourseReq')
                $("#" + id).addClass('active');
                $("#" + id + "-tab").addClass('active');
                $("#" + id + "-tab").addClass('show');
            }
            else {
                $("#v-pills-TechnologyRequirement").addClass('active');
                $("#v-pills-TechnologyRequirement-tab").addClass('active');
                $("#v-pills-TechnologyRequirement-tab").addClass('show');
            }
            //===============================================================
            $("#AddTechnologyRequirement_DropDownListCourse").select2();
            $("#AddTextBook_DropDownListCourse").select2();
            $("#AddRequiredTool_DropDownListCourse").select2();
            $("#AddSupplie_DropDownListCourse").select2();
            $("#AddCourseMaterialRequirement_DropDownListCourse").select2();
        });
        function setActiveClass(id) {
            sessionStorage.setItem('btnIdCourseReq', id);
        }


        $(document).ready(function () {

        });
    </script>

</body>
</html>
