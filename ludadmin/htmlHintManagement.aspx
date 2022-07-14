<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="htmlHintManagement.aspx.cs" Inherits="AdminPages.htmlHintManagement" ValidateRequest="false" %>
<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
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
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
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
            <div class="container">
                <p align="right">
                    <uc1:Logout ID="ctlLogout" runat="server" />
                </p>
                <div class="row">
                    <div class="col-lg-12 margin-top-10">
                        <h5>HTML Hints manager</h5>
                        <hr />
                    </div>
                    <div class="col-md-12">
                        <span>Filter by Coding Problem</span>
                        <hr />
                        <div class="row margin-bottom-10">
                            <div class="col-md-4">
                                <asp:Label CssClass="sp-label" ID="Label11" runat="server" Text="Coding Problem"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <select id="codSelector" name="D1" class="tex-box form-control"></select>
                            </div>
                        </div>
                    </div>
                    <div id="codingProblemContainer" class="col-md-12 d-none">
                        <div class="row">
                            <div class="col-md-12 margin-top-10">
                                <span id="Label4" class="sp-label">Solution </span>
                                <textarea id="viewHTMLSelected" name="S1" rows="15" class="font-size-13 form-control custome-textarea"></textarea>
                            </div>
                            <div class="col-md-12 margin-top-10">
                                <span>Filter by Answers</span>
                                <hr />
                                <div class="row margin-bottom-10">
                                    <div class="col-md-4">
                                        <asp:Label CssClass="sp-label" ID="Label12" runat="server" Text="Answer"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <select id="ansSelector" name="D1" class="tex-box form-control"></select>
                                    </div>
                                </div>
                            </div>
                            <div id="viewAnsContainer" class="col-md-12 margin-top-10 d-none">
                                <span id="Label14" class="sp-label">Answer </span>
                                <textarea id="viewAnsSelected" name="S1" rows="15" class="font-size-13 form-control custome-textarea"></textarea>
                            </div>
                            <div id="viewBlockContainer" class="col-md-12 margin-top-10 wraper-area box-bg-white d-none">
                                <div class="margin-bottom-10">
                                    <span id="Label5" class="sp-label">HTML Block</span>
                                    <textarea id="inputHTMLBlock" class="font-size-13 form-control custome-textarea" rows="5"></textarea>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <span id="Label15" class="sp-label">Line: </span>
                                        <input id="inputLine" type="number" min="-1" />
                                    </div>
                                    <div class="col-md-6" style="text-align: right">
                                        <button id="btnSubmitHTMLBlock" type="button" class="btn btn-custom mr-3">Add Html Block</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="hintsContainer" class="col-md-12 margin-top-10">
                    </div>
                    <div>
                        <div id="newHintContainer">
                        </div>
                    </div>
                </div>
                <div class="col-lg-3">
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
    <%: Scripts.Render("~/bundles/htmlHintManagement") %>
</body>
</html>
