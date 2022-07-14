<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentSubmittedFile.aspx.cs" Inherits="OnlineLearningSystem.StudentSubmittedFile" %>

<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<link rel="favicon icon" href="favicon.png" type="image/png">
<head runat="server">
    <title>Let's Use Data</title>
    <%: Styles.Render("~/Content/css") %>
    <%-----------------------custome style--------------------------%>
    <style>
        input#ctlLogout_btnLogout, #ctlLogout_btnComment, .btn-profile {
            border-radius: 18px;
            padding: 4px 25px;
            border: solid 1px #2c79bf;
            font-size: 13px;
            background: #fff;
            margin-bottom: 10px;
            line-height: 18px;
            display: inline-block;
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
    <section class="content-area">
        <form id="form1" runat="server">
            <div class="add-course-area" style="margin: 0 60px;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <p align="right">
                                <uc1:Logout ID="Logout1" runat="server" />
                            </p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-7">
                            <div class="course-list margin-top-10">
                                <h5>Student Submitted Excel File </h5>
                                <hr />

                                <div class="custome-overflow">
                                    <%--<asp:Label ID="LabelSelectedCourseId" runat="server" Text="" Visible="false"></asp:Label>--%>
                                    <asp:GridView CssClass="table table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="GridView1_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" />
                                            <asp:BoundField DataField="CodingProblem" HeaderText="Coding Problem" />
                                            <asp:BoundField DataField="Student" HeaderText="Student" />
                                            <asp:ButtonField Text="Download Excel File" CommandName="DownloadExcelFile" HeaderText="Excel File" />
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
</body>
</html>
