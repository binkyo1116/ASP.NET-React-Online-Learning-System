<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubmissionGrade.aspx.cs" Inherits="OnlineLearningSystem.SubmissionGrade" ValidateRequest="false" %>

<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Let's Use Data</title>
    <link rel="favicon icon" href="favicon.png" type="image/png" />
    <link href="Content/jquery-ui.css" rel="stylesheet" />
    <link href="Content/select2.css" rel="stylesheet" />
    <%--<link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />--%>
    <%: Styles.Render("~/Content/css") %>
    <%------------------------custome style------------------------%>
    <style>
        .margin-bottom-8 {
            margin-bottom: 8px;
        }

        #cke_1_contents {
            height: 150px !important;
        }

        #TextBoxExpectedOutput {
            height: 195px !important;
        }

        #TextBoxTestCode {
            height: 195px !important;
        }

        #cke_EditorInstruction {
            margin-bottom: 8px;
        }

        #cke_HighlightText .cke_top {
            display: none !important;
            visibility: hidden !important;
            font-size: 16px !important;
        }

        #cke_HighlightText {
            font-size: 13px !important;
            line-height: 21 !important;
        }

        body {
            font-family: 'Raleway', sans-serif;
            font-size: 16px;
            line-height: 21px;
            background-color: #fff;
        }

        div#HighlightText {
            border: 1px solid gainsboro;
            border-radius: 3px;
        }

            div#HighlightText:focus {
                color: #495057;
                background-color: #fff;
                border-color: #80bdff;
                outline: 0 !important;
                box-shadow: 0 0 0 0.2rem rgb(0 123 255 / 25%);
            }

        .txtCode {
            height: 250px !important;
        }

        .txtComment {
            height: 100px !important;
        }

        .result-span {
            color: #449ae8;
            font-size: 15px;
            margin-left: 5px;
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
                <div style="margin: 0 60px;">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="text-right margin-bottom-15">
                                    <asp:Button ID="btnClearAll" CssClass="btn btn-custom-light btn-sm" runat="server" Text="Clear All" OnClick="btnClearAll_Click" />&nbsp;
                                    <uc1:Logout ID="ctlLogout" runat="server" />
                                </div>
                            </div>
                            <div class="col-md-12">
                                <h5>Student Submission and Grade</h5>
                                <hr />
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="wraper-area margin-bottom-10">
                                            <div class="row margin-bottom-10">
                                                <div class="col-md-3">
                                                    <asp:Label CssClass="sp-label" ID="Label12" runat="server" Text="Course"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseFilter2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseFilter2_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row margin-bottom-10">
                                                <div class="col-md-3">
                                                    <asp:Label CssClass="sp-label" ID="Label14" runat="server" Text="Quarter"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListQuarterFilter2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListQuarterFilter2_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row margin-bottom-10">
                                                <div class="col-md-3">
                                                    <asp:Label CssClass="sp-label" ID="Label6" runat="server" Text="Course Instance"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseInstanceFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseInstanceFilter_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="wraper-area margin-bottom-10">
                                            <div class="row margin-bottom-10">
                                                <div class="col-md-3">
                                                    <asp:Label CssClass="sp-label" ID="Label9" runat="server" Text="Module Objective"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListModuleObjectiveFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListModuleObjectiveFilter_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="row margin-bottom-10">
                                                <div class="col-md-3">
                                                    <asp:Label ID="Label1" CssClass="sp-label" runat="server" Text="Select Student"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListStudent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListStudent_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <asp:Label ID="Label5" CssClass="sp-label" runat="server" Text="Select a Coding Problem"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCodingProblem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCodingProblem_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="text-right">
                                                 <asp:Button ID="btnSearchResult" runat="server" CssClass="btn btn-custom btn-sm" Text="Continue" OnClick="btnSearchResult_Click"  />
                                            </div>
                                               
                                        </div>
                                    </div>
                                </div>
                                <div class="row margin-bottom-15">
                                    <div class="col-md-12">
                                        <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="wraper-area">
                                    <asp:Panel ID="PanelSubmission" runat="server"></asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    </section>
    <%--===================================================================================--%>    <%--========================================footer area=================================--%>
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
    <%--=============================================================================--%>    <%: Scripts.Render("~/bundles/js") %>
    <script src="Scripts/jquery-ui.min.js"></script>
    <script src="Scripts/select2.min.js"></script>
    <script src="Scripts/ckeditor/ckeditor.js"></script>
    <script>
        CKEDITOR.replace('TextBoxInstruction');
    </script>
    <script>

        // $(function () {
        //     $("[id*=GridView1]").DataTable(
        //        {
        //            bLengthChange: true,
        //            //lengthMenu: [[5, 10, -1], [5, 10, "All"]],
        //            bFilter: true,
        //            bSort: true,
        //            bPaginate: false
        //        });
        //});

        $(document).ready(function () {
            $("#DropDownListCourseFilter1").select2();
            $("#DropDownListCourseFilter2").select2();
            $("#DropDownListQuarterFilter2").select2();
            $("#DropDownListCourseInstance").select2();
            $("#DropDownListModuleObjective").select2();
            $("#DropDownListCourseInstanceFilter").select2();
            $("#DropDownListModuleObjectiveFilter").select2();
            $("#DropDownListCodingProblem").select2();
            $("#DropDownListStudent").select2();
        });
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
        })
    </script>
</body>
</html>
