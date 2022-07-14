<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuizQuestionList.aspx.cs" Inherits="OnlineLearningSystem.QuizQuestionList" %>

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
                        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarText" aria-controls="navbarText" aria-expanded="false" aria-label="Toggle navigation">
                            <span class="navbar-toggler-icon"></span>
                        </button>
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
            <div class="">
                <div class="container">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="">
                                <div class="text-right">
                                    <asp:Button ID="btnAddActivity" CssClass="btn btn-custom btn-sm" runat="server" Text="Add New" OnClick="btnAddActivity_Click" />
                                    <asp:Label ID="lblMessage" runat="server" Style="font: bold 12px sans-serif"></asp:Label>
                                </div>
                                <h5>Activity List</h5>
                                <hr />
                                <div class="custome-overflow">
                                    <asp:Label ID="LabelSelectedCourseId" runat="server" Text="" Visible="false"></asp:Label>
                                    <asp:Label ID="LabelSelectedCourseObjectiveId" runat="server" Text="" Visible="false"></asp:Label>
                                    <asp:Label ID="LabelSelectedModuleId" runat="server" Text="" Visible="false"></asp:Label>
                                    <asp:Label ID="LabelSelectedModuleObjectiveId" runat="server" Text="" Visible="false"></asp:Label>
                                    <asp:GridView CssClass="table table-bordered table-fixed" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="ActivityId" OnRowCancelingEdit="OnRowCancelingEdit" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting">
                                        <Columns>
                                            <asp:BoundField DataField="ActivityId" HeaderText="Activity ID" ReadOnly="True" />
                                            <asp:BoundField DataField="QuizId" HeaderText="QuizId" ReadOnly="True" />
                                            <asp:TemplateField HeaderText="Course ID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelCourseId" runat="server" Text='<%# Eval("CourseId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CourseObjectiveId" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelCourseObjectiveId" runat="server" Text='<%# Eval("CourseObjectiveId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ModuleId" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelModuleId" runat="server" Text='<%# Eval("ModuleId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ModuleObjectiveId" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelModuleObjectiveId" runat="server" Text='<%# Eval("ModuleObjectiveId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Course">
                                                <EditItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Course") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("Course") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:Label ID="LabeHeaderCourseddl" runat="server" Text="Course"></asp:Label>
                                                    <asp:DropDownList ID="DropDownListSortCourse" CssClass="form-control header-drop-down" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListSortCourse_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Course Objective">
                                                <EditItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("CourseObjective") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("CourseObjective") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:Label ID="LabeHeaderCourseObjectiveddl" runat="server" Text="Course Objective"></asp:Label>
                                                    <asp:DropDownList ID="DropDownListSortCourseObjective" CssClass="form-control header-drop-down" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListSortCourseObjective_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Module">
                                                <EditItemTemplate>
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("Module") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("Module") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:Label ID="LabeHeaderModule" runat="server" Text="Module"></asp:Label>
                                                    <asp:DropDownList ID="DropDownListSortModule" CssClass="form-control header-drop-down" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListSortModule_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Module Objective">
                                                <EditItemTemplate>
                                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("ModuleObjective") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label7" runat="server" Text='<%# Bind("ModuleObjective") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:Label ID="LabeHeaderModuleObjective" runat="server" Text="Module Objective"></asp:Label>
                                                    <asp:DropDownList ID="DropDownListSortModuleObjective" CssClass="form-control header-drop-down" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListSortModuleObjective_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Title">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox2" CssClass="form-control custome-textarea" runat="server" Text='<%# Bind("Title") %>' TextMode="multiline"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("Title") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox1" CssClass="form-control custome-textarea" runat="server" Text='<%# Bind("Type") %>' TextMode="multiline"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Type") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Max Grade">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox3" CssClass="form-control custome-textarea" runat="server" Text='<%# Bind("MaxGrade") %>' TextMode="multiline"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("MaxGrade") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField ShowEditButton="True" />
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButtonDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
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
