<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HintManagement.aspx.cs" Inherits="AdminPages.HintManagement" %>
<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Let's Use Data</title>
    <link rel="favicon icon" href="favicon.png" type="image/png" />
    <%: Styles.Render("~/Content/css") %>
    <%----------------------custome style-----------------------%>
    <style type="text/css">
        .question-img-area img {
            max-width: 100%;
        }

        .modal-body {
            word-wrap: break-word;
        }


        #mask {
            top: 0;
            position: fixed;
            width: 100%;
            height: 100%;
            background: #00000038;
            z-index: 999;
        }

        #loading {
            width: 100px;
            height: 100px;
            background: #0000008a;
            border-radius: 5%;
            border: 1px solid #ffffff29;
            top: 50%;
            left: 50%;
            position: absolute;
            transform: translate(-50%, -50%);
        }

            #loading img {
                width: 60px;
                height: 60px;
                position: absolute;
                top: 50%;
                left: 50%;
                transform: translate(-50%, -50%);
            }

        .select {
            min-height: 200px;
            width: 100%;
        }

        .question-img-area img {
            max-width: 100%;
            height: auto;
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
                        <div class="collapse navbar-collapse" id="navbarText">
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
            <div id="mask" class="collapse">
                <div id="loading">
                    <img src="https://i.imgur.com/zPT3qsO.gif" />
                </div>
            </div>

            <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>

            <div class="container">
                <p align="right">
                    <uc1:Logout ID="ctlLogout" runat="server" />
                </p>
                <div class="row">
                    <div class="col-md-12 margin-top-10">
                        <h5>Hints manager</h5>
                        <hr />
                    </div>
                    <div class="col-md-12">
                        <span>Filter by Course</span>
                        <hr />
                        <div class="row margin-bottom-10">
                            <div class="col-md-4">
                                <asp:Label CssClass="sp-label" ID="Label11" runat="server" Text="Course Instance"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <select id="courses" name="select" class="tex-box form-control"></select>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <span>Questions</span>
                        <hr />
                        <div id="questionsContainer">
                        </div>
                    </div>
                </div>
            </div>
        </form>

        <!-- Modal -->
        <div class="modal fade" id="hintSavedModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel">Hint saved</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        Changes were saved correctly.
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-dismiss="modal">OK</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Add Correct Answer -->
        <div class="modal fade" id="validAnswerSavedModal" tabindex="-1" aria-labelledby="validAnswerModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="validAnswerModalLabel">Added answer</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        A new valid answer has been added.
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-dismiss="modal" id="btn_add_answer">Ok</button>
                    </div>
                </div>
            </div>
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
    <%: Scripts.Render("~/bundles/hintManagement") %>
</body>
</html>
