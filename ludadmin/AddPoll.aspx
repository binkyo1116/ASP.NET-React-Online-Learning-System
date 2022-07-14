<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddPoll.aspx.cs" Inherits="OnlineLearningSystem.AddPoll" %>

<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>
<%@ Register Src="AddPollQuestionType.ascx" TagPrefix="uc2" TagName="AddPollQuestionType" %>
<%@ Register Src="~/AddPollQuestion.ascx" TagPrefix="uc3" TagName="AddPollQuestion" %>
<%@ Register Src="~/AddPollGroup.ascx" TagPrefix="ucPollGroup" TagName="AddPollGroup" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Let's Use Data</title>
    <link rel="favicon icon" href="favicon.png" type="image/png" />
    <link href="Content/select2.css" rel="stylesheet" />
    <link href="Content/jquery-ui.css" rel="stylesheet" />
    <%: Styles.Render("~/Content/css") %>
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
    <section class="content-area margin-top-15 margin-bottom-100 margin-left-right-30 ">
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
            <div class="add-poll-area">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-sm-2 border-right">
                            <div class="poll-left-panel">
                                <div class="nav flex-column nav-pills" id="v-pills-tab" role="tablist" aria-orientation="vertical">
                                    <a class="nav-link btn-outline-box" onclick="setActiveClass(this.id)" id="v-pills-create-poll" data-toggle="pill" href="#v-pills-create-poll-tab" role="tab" aria-controls="v-pills-profile" aria-selected="false"><i class="fa fa-plus fa-1x "></i>&nbsp; Create Poll</a>
                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-poll-list" data-toggle="pill" href="#v-pills-poll-list-tab" role="tab" aria-controls="v-pills-poll-list-tab" aria-selected="true"><i class="fa fa-list fa-1x "></i>&nbsp;Poll List</a>

                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-poll-group" data-toggle="pill" href="#v-pills-poll-group-tab" role="tab" aria-controls="v-pills-poll-group-tab" aria-selected="true"><i class="fa fa-list fa-1x "></i>&nbsp; Create Group</a>
                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-my-poll" data-toggle="pill" href="#v-pills-my-poll-tab" role="tab" aria-controls="v-pills-home" aria-selected="true"><i class="fa fa-list fa-1x "></i>&nbsp; Poll Response</a>

                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-create-polltype" data-toggle="pill" href="#v-pills-create-polltype-tab" role="tab" aria-controls="v-pills-profile" aria-selected="false"><i class="fa fa-list fa-1x "></i>&nbsp; Add Poll Type</a>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-10">
                            <div class="Poll-area margin-top-15 margin-left-right-30">
                                <div class="tab-content" id="v-pills-tabContent">
                                    <%--========================================Poll Question Panel =====================================--%>
                                    <uc3:AddPollQuestion runat="server" ID="AddPollQuestion" />
                                    <%--========================================Poll Group Panel =====================================--%>
                                    <ucPollGroup:AddPollGroup runat="server" ID="AddPollGroup" />
                                    <%--========================================Poll Type Panel =====================================--%>
                                    <div class="tab-pane fade" id="v-pills-create-polltype-tab" role="tabpanel" aria-labelledby="v-pills-create-polltype">
                                        <uc2:AddPollQuestionType runat="server" ID="AddPollQuestionType" />
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
        });
        $(document).ready(function () {
            //----------------Remove Previous active class--------------------
            if (sessionStorage.getItem('btnIdPoll') != null) {
                $(".nav-link").removeClass('active');
                $(".tab-pane").removeClass('active');
                $(".tab-pane").removeClass('show');

                //------------------Add active class------------------------------
                var id = sessionStorage.getItem('btnIdPoll')
                $("#" + id).addClass('active');
                $("#" + id + "-tab").addClass('active');
                $("#" + id + "-tab").addClass('show');
            }
            else {
                $("#v-pills-my-poll").addClass('active');
                $("#v-pills-my-poll-tab").addClass('active');
                $("#v-pills-my-poll-tab").addClass('show');
            }
            //========================================================================
            // THIS IS FOR HIDE ALL DETAILS ROW
            $(".SUBDIV table tr:not(:first-child)").not("tr tr").hide();
            $(".SUBDIV .btncolexp").click(function () {
                $(this).closest('tr').next('tr').toggle();
                //this is for change img of btncolexp button
                if ($(this).attr('class').toString() == "btncolexp btn-collapse") {
                    $(this).addClass('btn-expand');
                    $(this).removeClass('btn-collapse');
                }
                else {
                    $(this).removeClass('btn-expand');
                    $(this).addClass('btn-collapse');
                }
            });
            //===============================================================
            $("#AddPollGroup_DropDownListCourseInstance").select2();
            $("#AddPollGroup_DropDownListModuleObjective").select2();
            $("#AddPollGroup_DropDownListCourseInstanceFilter").select2();
            $("#AddPollGroup_DropDownListModuleObjectiveFilter").select2();
            $("#AddPollGroup_DropDownListPollGroup").select2();
            $("#AddPollGroup_DropDownListPollQuestion").select2();

            $("#AddPollQuestion_DropDownListGroupList").select2();
            $("#AddPollGroup_ddnCourseInstance_myPollList").select2();
            $("#AddPollGroup_DropDownListModuleObjective_myPollList").select2();
            $("#AddPollGroup_DropDownListPollGroup_myPollList").select2();



        });
        function setActiveClass(id) {
            sessionStorage.setItem('btnIdPoll', id);
        }

    </script>
</body>
</html>
