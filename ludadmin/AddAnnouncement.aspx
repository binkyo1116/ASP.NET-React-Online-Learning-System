<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddAnnouncement.aspx.cs" Inherits="OnlineLearningSystem.AddAnnouncement" %>

<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Let's Use Data</title>
    <link rel="favicon icon" href="favicon.png" type="image/png" />
    <link href="Content/select2.css" rel="stylesheet" />
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
                <div class="add-course-area" style="margin: 0 60px;">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <p align="right">
                                    <uc1:Logout ID="ctlLogout" runat="server" />
                                </p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="add-new-course-objective">
                                   <h5>Add New Announcement</h5>
                                    <hr />
                                    <asp:Label CssClass="sp-label" ID="LableCrouse" runat="server" Text="Course Isntance: "></asp:Label>
                                    <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListCourseInstance" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseInstance_SelectedIndexChanged"></asp:DropDownList>

                                    <asp:Label CssClass="sp-label" ID="Label111" runat="server" Text="Title"></asp:Label>
                                    <asp:TextBox ID="TextBoxTitle" CssClass="tex-box form-control margin-bottom-10" runat="server" placeholder="Announcement Title"></asp:TextBox>

                                    <asp:Label CssClass="sp-label" ID="Label5" runat="server" Text="Description"></asp:Label>
                                    <asp:TextBox ID="TextBoxDescription" CssClass="font-size-13 form-control margin-bottom-10" Height="140" runat="server" placeholder="Description" TextMode="MultiLine"></asp:TextBox>

                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label CssClass="sp-label" ID="Label4" runat="server" Text="Active"></asp:Label>
                                            &nbsp;
                                    <asp:CheckBox ID="CheckBoxActive" runat="server" Checked="true" />
                                        </div>
                                        <div class="col-md-6 text-right">
                                            <asp:Button ID="btnAddAnnouncement" runat="server" CssClass="btn btn-custom btn-sm" Text="Submit" OnClick="btnAddAnnouncement_Click" />
                                        </div>
                                    </div>
                                    <div class="text-right margin-top-10">
                                        <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
                                        <asp:Label ID="lblErrorMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label><br />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-8">
                                <div class="course-list margin-left-30">
                                        <div class="row">
                                        <div class="col-md-6">
                                                <h5>Announcement List</h5>
                                        </div>
                                        <div class="col-md-6 text-right">
                                             <asp:Button ID="btnClearAll" CssClass="btn btn-custom-light btn-sm" runat="server" Text="Clear All" OnClick="btnClearAll_Click" />
                                        </div>
                                    </div>
                                 
                                    <hr style="margin-top:5px" />
                                    <div class="custome-overflow" style="margin-right: 0">
                                        <asp:GridView CssClass="table table-bordered grid-table" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCancelingEdit="OnRowCancelingEdit" OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating">
                                            <Columns>
                                                <asp:BoundField DataField="Id" ItemStyle-HorizontalAlign="Center" HeaderText="Id" ReadOnly="True" />
                                                <asp:BoundField DataField="CourseInstance" HeaderText="CourseInstance" ReadOnly="True" />
                                                <asp:TemplateField HeaderText="Title">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox2" CssClass="form-control custome-textarea th-width-announcement" runat="server" Text='<%# Bind("Title") %>' TextMode="MultiLine"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("Title") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" CssClass="form-control custome-textarea" TextMode="MultiLine" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="PublishedBy" HeaderText="Published By" ReadOnly="True" />
                                                <asp:TemplateField HeaderText="Bublished Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("PublishedDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Active ">
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("Active") %>' />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBo2" runat="server" Checked='<%# Bind("Active") %>' Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:CommandField ShowEditButton="True" />
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButtonDelete" ForeColor="Red"  runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
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
        $(function () {
            $("#DropDownListCourseInstance").select2();

        })
    </script>
</body>
</html>
