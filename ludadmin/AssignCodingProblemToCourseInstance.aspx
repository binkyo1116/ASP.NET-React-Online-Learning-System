<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssignCodingProblemToCourseInstance.aspx.cs" Inherits="OnlineLearningSystem.AssignCodingProblemToCourseInstance" ValidateRequest="false" %>
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
    <%----------------------custome style-------------------%>
    <style>
        .margin-bottom-8 {
            margin-bottom: 8px;
        }

        .text-center {
            text-align: center;
            display: block;
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
                                    <asp:Button ID="Button3" CssClass="btn btn-custom-light btn-sm" runat="server" PostBackUrl="~/AddCodingProblem.aspx" Text="Add Coding Problem" />&nbsp;
                                    <asp:Button ID="btnClearAll" CssClass="btn btn-custom-light btn-sm" runat="server" Text="Clear All" OnClick="btnClearAll_Click" />&nbsp;
                                    <uc1:Logout ID="ctlLogout" runat="server" />
                                </div>
                            </div>
                            <div class="col-md-12">
                                <h5>Assign Coding Problem to Course Instance</h5>
                                <hr />
                                <div class="row">
                                    <div class="col-md-6">
                                        <h6>Course Instance</h6>
                                        <div class="wraper-area margin-bottom-15">
                                            <div class="row margin-bottom-10">
                                                <div class="col-md-3">
                                                    <asp:Label CssClass="sp-label" ID="Label11" runat="server" Text="Course"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseFilter1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseFilter1_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row margin-bottom-10">
                                                <div class="col-md-3">
                                                    <asp:Label CssClass="sp-label" ID="Label13" runat="server" Text="Quarter"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListQuarter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListQuarter_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row margin-bottom-10">
                                                <div class="col-md-3">
                                                    <asp:Label CssClass="sp-label" ID="Label3" runat="server" Text="Course Instance"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseInstance" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseInstance_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <asp:Label CssClass="sp-label" ID="LabelModuleObjective" runat="server" Text="Module Objective"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListModuleObjective" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListModuleObjective_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div>
                                            <div class="row margin-bottom-10">
                                                <div class=" col-md-3 col-lg-2 padding-r-0">
                                                    <asp:Label CssClass="sp-label" ID="LabelDueDate" runat="server" Text="Due Date"></asp:Label>
                                                </div>
                                                <div class="col-md-9 col-lg-4 padding-r-0">
                                                    <div style="position: relative">
                                                        <asp:TextBox CssClass="tex-box form-control DatePicker margin-bottom-10" ID="TextBoxDueDate" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-lg-2 padding-r-0">
                                                    <asp:Label CssClass="sp-label" ID="Label7" runat="server" Text="Max Grade"></asp:Label>
                                                </div>
                                                <div class="col-md-9 col-lg-2 padding-r-0">
                                                    <asp:TextBox CssClass="tex-box form-control margin-bottom-8" ID="TextBoxCourseInstanceCPMaxGrade" type="number" Text="100" placeholder="Max Grade" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-3 col-lg-2 padding-r-0">
                                                    <asp:Label CssClass="sp-label" ID="Label8" runat="server" Text="Active"></asp:Label>
                                                    &nbsp;
                                                <asp:CheckBox ID="CheckBoxCourseInstanceCodingProblemActive" runat="server" Checked="true" />
                                                </div>
                                            </div>
                                            <div class="row margin-bottom-10">
                                                <div class="col-md-12 text-right">
                                                    <asp:Button ID="ButtonCourseInstanceCodingProblem" runat="server" CssClass="btn btn-custom btn-sm" Text="Submit" OnClick="ButtonCourseInstanceCodingProblem_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <h6>Coding Problem</h6>
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
                                                    <asp:Label ID="Label5" CssClass="sp-label" runat="server" Text="Select a Coding Problem"></asp:Label>
                                                </div>
                                                <div class="col-md">
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCodingProblem" runat="server" AutoPostBack="True"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
                                        <asp:Label ID="lblErrorMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <br />
                                <div class="wraper-area margin-bottom-50">
                                    <span>Course Instance & Coding Problem List</span>
                                    <div class="custome-overflow margin-top-10" style="max-height: 260px; height: auto">
                                        <asp:GridView CssClass="table table-bordered grid-table" ID="GridView1" runat="server" DataKeyNames="CourseInstanceId" OnRowCancelingEdit="OnRowCancelingEdit" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:TemplateField HeaderText="CodingProblemId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LabelCodingProblemId" runat="server" Text='<%# Bind("CodingProblemId") %>'></asp:Label>
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
                                                <asp:BoundField HeaderStyle-Width="20%" ItemStyle-Width="20%" FooterStyle-Width="20%" DataField="ModuleObjective" HeaderText="ModuleObjective" ReadOnly="True">
                                                    <FooterStyle Width="20%"></FooterStyle>

                                                    <HeaderStyle Width="20%"></HeaderStyle>

                                                    <ItemStyle Width="20%"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CodingProblem" HeaderText="CodingProblem" ReadOnly="True" />
                                                <asp:TemplateField HeaderText="MaxGrade">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBoxMaxGrade" CssClass="form-control" runat="server" Text='<%# Bind("MaxGrade") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label CssClass="text-center" ID="Label1" runat="server" Text='<%# Bind("MaxGrade") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Due Date">
                                                    <EditItemTemplate>
                                                        <div style="position: relative">
                                                            <asp:TextBox ID="TextBoxDueDate" CssClass="tex-box form-control DatePicker" runat="server" Text='<%# Eval("DueDate", "{0:MMM-dd-yyyy}") %>'></asp:TextBox>
                                                        </div>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label4" CssClass="text-center" runat="server" Text='<%# Eval("DueDate", "{0:MMM-dd-yyyy}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Active">
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="CheckBoxActive" runat="server" Checked='<%# Bind("Active") %>' />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox CssClass="text-center" ID="CheckBox1" runat="server" Checked='<%# Bind("Active") %>' Enabled="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:CommandField ShowEditButton="True" />
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButtonDelete" ForeColor="Red" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
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
    <%--<script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>--%>
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
            $("#DropDownListQuarter").select2();

            $("#DropDownListCourseInstance").select2();
            $("#DropDownListModuleObjective").select2();
            $("#DropDownListCourseInstanceFilter").select2();
            $("#DropDownListModuleObjectiveFilter").select2();
            $("#DropDownListCodingProblem").select2();
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
            //var today = new Date();
            //$("#TextBoxDueDate").val(today.setDate(9));

            //--------------------------------------------
            var srd = new Date();
            var srdnewdate = new Date();
            srdnewdate.setDate(srd.getDate() + 9);
            var dd = new Date(srdnewdate.getFullYear(), srdnewdate.getMonth(), srdnewdate.getDate())
            $("#TextBoxDueDate").datepicker("setDate", dd);

        })
    </script>
</body>
</html>
