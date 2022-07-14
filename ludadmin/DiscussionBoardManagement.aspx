<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DiscussionBoardManagement.aspx.cs" Inherits="OnlineLearningSystem.DiscussionBoardManagement" %>
<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Let's Use Data</title>
    <link rel="favicon icon" href="favicon.png" type="image/png" />
    <link href="Content/jquery-ui.css" rel="stylesheet" />
    <link href="Content/select2.css" rel="stylesheet" />
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
    <section class="content-area margin-top-15 margin-bottom-100 ">
        <div class="discussion-management-area">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="discussion-area" style="margin: 0 60px;">
                            <form id="form1" runat="server">
                                <p align="right">
                                    <asp:Button ID="ButtonRefresh" runat="server" CssClass="btn btn-custom-light btn-sm font-size-13" Text="Clear All" OnClick="ButtonRefresh_Click" />
                                    &nbsp;
                                    <uc1:Logout ID="ctlLogout" runat="server" />
                                </p>
                                <div class="margin-top-15">
                                    <%--========================================Discussion Board add Panel =====================================--%>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h5>Course Instance & Discussion Board</h5>
                                            <hr />
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="row margin-bottom-10">
                                                        <div class="col-md-4">
                                                            <asp:Label CssClass="sp-label" ID="Label6" runat="server" Text="Course Instance"></asp:Label>
                                                        </div>
                                                        <div class="col-md">
                                                            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseInstance" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseInstance_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <h6>Filter Discussion Board</h6>
                                                    <div class="wraper-area margin-bottom-10">
                                                        <div class="row margin-bottom-10">
                                                            <div class="col-md-4">
                                                                <asp:Label CssClass="sp-label" ID="Label11" runat="server" Text="Course Instance Filter"></asp:Label>
                                                            </div>
                                                            <div class="col-md">
                                                                <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseInstanceFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseInstanceFilter_SelectedIndexChanged"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <asp:Label CssClass="sp-label" ID="Label12" runat="server" Text="Module Obj. Filter"></asp:Label>
                                                            </div>
                                                            <div class="col-md">
                                                                <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListModuleObjectiveFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListModuleObjectiveFilter_SelectedIndexChanged"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row margin-bottom-10">
                                                        <div class="col-md-4">
                                                            <asp:Label ID="Label8" CssClass="sp-label" runat="server" Text="Select a Discussion Board"></asp:Label>
                                                        </div>
                                                        <div class="col-md">
                                                            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListDiscussionBoard" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListDiscussionBoard_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-5">
                                                    <div class="row margin-bottom-10">
                                                        <div class="col-md-3">
                                                            <asp:Label CssClass="sp-label" ID="Label7" runat="server" Text="Module Obj."></asp:Label>
                                                        </div>
                                                        <div class="col-md">
                                                            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListModuleObjective" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListModuleObjective_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="row margin-bottom-10">
                                                        <div class="col-md-3">
                                                            <asp:Label CssClass="sp-label" ID="LabelDueDate" runat="server" Text="Due Date"></asp:Label>
                                                        </div>
                                                        <div class="col-md">
                                                            <div style="position: relative">
                                                                <asp:TextBox CssClass="tex-box form-control DatePicker margin-bottom-10" ID="TextBoxDueDate" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row margin-bottom-10">
                                                        <div class="col-md-6">
                                                            <asp:Label CssClass="sp-label" ID="Label10" runat="server" Text="Active"></asp:Label>
                                                            &nbsp;
                                                <asp:CheckBox ID="CheckBoxCourseInstanceDBActive" runat="server" Checked="true" />
                                                        </div>
                                                        <div class="col-md-6 text-right">
                                                            <asp:Button ID="ButtonCourseInstanceDiscussionBoard" runat="server" CssClass="btn btn-custom btn-sm" Text="Submit" OnClick="ButtonCourseInstanceDiscussionBoard_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row margin-bottom-15">
                                                <div class="col-md-12">
                                                    <h5>Or, Add New Discussion Board</h5>
                                                    <hr />
                                                    <asp:Label ID="Label13" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-5">
                                            <div class="add-discussion-board">
                                                <div class="wraper-area">
                                                    <h6>Discussion Board</h6>
                                                    <hr />
                                                    <asp:Label CssClass="sp-label" ID="Label9" runat="server" Text="Title"></asp:Label>
                                                    <asp:TextBox TextMode="MultiLine" CssClass="font-size-13 form-control margin-bottom-10" ID="TextBoxTitle" runat="server" placeholder="Discussion Board Title"></asp:TextBox>
                                                    <asp:Label CssClass="sp-label" ID="Label3" runat="server" Text="Active"></asp:Label>
                                                    &nbsp;
                                                    <asp:CheckBox ID="CheckBoxActive" runat="server" Checked="true" />
                                                    <br />
                                                    <asp:Button ID="AddNewDiscussionBoard" runat="server" CssClass="btn btn-custom btn-sm font-size-13" Text="Add New" OnClick="AddNewDiscussionBoard_Click" />
                                                    &nbsp;
                                                    <asp:Button ID="btnDiscussionBoardUpdate" runat="server" CssClass="btn btn-custom-light btn-sm font-size-13" Text="Update" OnClick="btnDiscussionBoardUpdate_Click" />
                                                    &nbsp;
                                                     <asp:Button ID="btnDiscussionBoardDelete" BackColor="Red" OnClientClick="return confirm('Do you want to delete this row ?');" runat="server" CssClass="btn btn-custom btn-sm font-size-13" Text="Delete" OnClick="btnDiscussionBoardDelete_Click" />
                                                    &nbsp;
                                                   <div class="margin-top-10">
                                                       <asp:Label ID="lblMessage" ForeColor="Green" CssClass="font-size-15" runat="server"></asp:Label>
                                                       <asp:Label ID="lblErrorMessage" ForeColor="Red" CssClass="text-danger font-size-15" runat="server"></asp:Label>
                                                   </div>
                                                </div>
                                                <br />
                                            </div>
                                        </div>
                                        <div class="col-md-7">
                                            <div class="wraper-area margin-left-30">
                                                <h6>Group Discussion</h6>
                                                <hr />
                                                <asp:Panel ID="PanelAddGroupDiscussion" runat="server" Visible="false">
                                                    <div class="margin-bottom-15">

                                                        <div class="">
                                                            <%--<asp:Label CssClass="sp-label" ID="LabelActivity" runat="server" Text="Title"></asp:Label>--%>
                                                            <asp:TextBox ID="TextBoxGroupDiscussionTitle" CssClass="tex-box form-control margin-bottom-10" runat="server" placeholder="Discussion Title"></asp:TextBox>

                                                            <%--<asp:Label CssClass="sp-label" ID="Label5" runat="server" Text="Description*"></asp:Label>--%>
                                                            <asp:TextBox ID="TextBoxGroupDiscussionDescription" CssClass="font-size-13 form-control margin-bottom-10" Height="140" runat="server" placeholder="Description" TextMode="MultiLine"></asp:TextBox>
                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <asp:Label CssClass="sp-label" ID="Label5" runat="server" Text="Active"></asp:Label>
                                                                    &nbsp;
                                                                    <asp:CheckBox ID="CheckBoxActiveDiscussion" runat="server" Checked="true" />
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="text-right">
                                                                        <asp:Button ID="btnAddDiscussion" runat="server" CssClass="btn btn-custom btn-sm" Text="Add New Discussion" OnClick="btnAddDiscussion_Click" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="wraper-area">
                                                <span>Course Instance & Discussion Board List</span>
                                                <div class="custome-overflow margin-top-10" style="max-height: 260px; height: auto">
                                                    <asp:GridView CssClass="table table-bordered table-fixed" ID="GridView2" runat="server" DataKeyNames="CourseInstanceId" OnRowCancelingEdit="OnRowCancelingEditCID" OnRowEditing="OnRowEditingCID" OnRowUpdating="OnRowUpdatingCID" OnRowDataBound="OnRowDataBoundCID" OnRowDeleting="OnRowDeletingCID" AutoGenerateColumns="False">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="DiscussionBoardId" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LabelDiscussionBoarId" runat="server" Text='<%# Bind("DiscussionBoardId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ModuleObjectiveId" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LabelModuleObjectiveId" runat="server" Text='<%# Bind("ModuleObjectiveId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="CourseInstanceId" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LabelCourseInstanceId" runat="server" Text='<%# Bind("CourseInstanceId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="CourseInstance" HeaderText="CourseInstance" ReadOnly="True" />
                                                            <asp:BoundField DataField="ModuleObjective" HeaderText="ModuleObjective" ReadOnly="True" />
                                                            <asp:BoundField DataField="DiscussionBoard" HeaderText="Discussion Board" ReadOnly="True" />
                                                            <asp:TemplateField HeaderText="Due Date">
                                                                <EditItemTemplate>
                                                                    <div style="position: relative">
                                                                        <asp:TextBox ID="TextBoxDueDate" CssClass="tex-box form-control DatePicker" runat="server" Text='<%# Eval("DueDate", "{0:MMM-dd-yyyy}") %>'></asp:TextBox>
                                                                    </div>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("DueDate", "{0:MMM-dd-yyyy}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Active">
                                                                <EditItemTemplate>
                                                                    <asp:CheckBox ID="CheckBoxActive" runat="server" Checked='<%# Bind("Active") %>' />
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Active") %>' Enabled="false" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:CommandField ShowEditButton="True" />
                                                            <asp:TemplateField ShowHeader="true">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="LinkButtonDelete" ForeColor="Red" runat="server" CausesValidation="true" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="wraper-area">
                                                <span>Group Discussion List</span>
                                                <div class="custome-overflow margin-top-10" style="max-height: 260px; height: auto">
                                                    <asp:GridView CssClass="table table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCancelingEdit="OnRowCancelingEdit" OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating">
                                                        <Columns>
                                                            <asp:BoundField DataField="CourseInstance" HeaderText="Course Instance" ReadOnly="True" />
                                                            <asp:BoundField DataField="DiscussionBoardId" HeaderText="Discussion Board Id" ReadOnly="True" Visible="False" />
                                                            <asp:BoundField DataField="Id" HeaderText="Group Discussion Id" ReadOnly="True" Visible="False" />
                                                            <asp:TemplateField HeaderText="Title">
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="TextBox2" CssClass="form-control font-size-13" runat="server" Text='<%# Bind("Title") %>'></asp:TextBox>
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
                                                            <asp:BoundField DataField="PublishedDate" HeaderText="Bublished Date" ReadOnly="True" />
                                                            <asp:TemplateField HeaderText="Active ">
                                                                <EditItemTemplate>
                                                                    <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("Active") %>' />
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CheckBo2" runat="server" Checked='<%# Bind("Active") %>' Enabled="false" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:CommandField ShowEditButton="True" />
                                                            <asp:TemplateField ShowHeader="true">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="LinkButtonDelete" ForeColor="Red" runat="server" CausesValidation="true" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%--========================================Poll Type Panel =====================================--%>
                                </div>
                            </form>
                        </div>
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
            $("#DropDownListCourseInstance").select2();
            $("#DropDownListModuleObjective").select2();
            $("#DropDownListCourseInstanceFilter").select2();
            $("#DropDownListModuleObjectiveFilter").select2();
            $("#DropDownListDiscussionBoard").select2();
        })
    </script>
</body>
</html>
