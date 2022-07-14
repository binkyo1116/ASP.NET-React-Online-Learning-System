<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddSchool.aspx.cs" Inherits="OnlineLearningSystem.AddSchool" %>

<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<link rel="favicon icon" href="favicon.png" type="image/png">
<head runat="server">
    <title>Let's Use Data</title>
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
        <form id="form1" runat="server">

            <div class="add-course-area">
                <div class="add-school-area" style="margin: 0 60px;">
                    <div class="container-fluid">
                        <p align="right">
                            <uc1:Logout ID="ctlLogout" runat="server" />
                        </p>
                        <div class="row">
                            <div class="col-sm-8">
                                <div class="school-list">
                                    <h5>School List</h5>
                                    <hr />
                                    <div class="custome-overflow">
                                        <asp:GridView CssClass="table table-bordered grid-table" ID="grvSchoolList" runat="server" AutoGenerateColumns="False" DataKeyNames="SchoolId" OnRowDeleting="OnRowDeleting" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowCancelingEdit="OnRowCancelingEdit" OnRowDataBound="OnRowDataBound">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="School Id">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("SchoolId")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Name">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox2" CssClass="form-control font-size-13" runat="server" Text='<%# Eval("Name")%>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Academic Calendar">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox3" TextMode="MultiLine" CssClass="form-control font-size-13" runat="server" Text='<%# Eval("AcademicCalendar")%>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("AcademicCalendar") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Syllabus Message">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox4" TextMode="MultiLine" CssClass="form-control font-size-13" runat="server" Text='<%# Eval("SyllabusMessage")%>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label4" runat="server" Text='<%# Eval("SyllabusMessage") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:CommandField ShowEditButton="True" />
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButtonDelete" runat="server" ForeColor="Red" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="add-new-school">
                                    <div class="row">
                                        <div class="col-md-8">
                                             <h5>Add New</h5>
                                        </div>
                                        <div class="col-md-4">
                                             <asp:Button ID="btnClearAll" CssClass="btn btn-custom-light btn-sm" runat="server" Text="Clear All" OnClick="btnClearAll_Click" />
                                        </div>
                                    </div>
                                    <hr style="margin-top:5px" />
                                    <asp:Label CssClass="sp-label" ID="lblSchoolName" runat="server" Text="School Name: "></asp:Label>
                                    <asp:TextBox CssClass="tex-box form-control" ID="TextBoxSchoolName" runat="server"></asp:TextBox>
                                    <br />
                                    <asp:Label CssClass="sp-label" ID="Label3" runat="server" Text="Academic Calendar: "></asp:Label>
                                    <asp:TextBox CssClass="font-size-13 form-control" ID="TextBoxAcademicCalendar" runat="server" TextMode="MultiLine"></asp:TextBox>
                                    <br />
                                    <asp:Label CssClass="sp-label" ID="Label4" runat="server" Text="Syllabus Message: "></asp:Label>
                                    <asp:TextBox CssClass="font-size-13 form-control custome-textarea" ID="TextBoxSyllabusMessage" runat="server" TextMode="MultiLine"></asp:TextBox>
                                    <br />
                                    <asp:Button ID="AddNewSchool" runat="server" CssClass="btn btn-custom margin-bottom-5" Text="Add New" OnClick="AddSchool_Click" />
                                    <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
                                    <asp:Label ID="lblErrorMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label><br />
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
