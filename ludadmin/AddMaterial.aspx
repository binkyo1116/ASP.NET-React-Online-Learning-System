<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddMaterial.aspx.cs" Inherits="OnlineLearningSystem.AddMaterial" %>

<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Let's Use Data</title>
    <link rel="favicon icon" href="favicon.png" type="image/png" />
    <link href="Content/jquery-ui.css" rel="stylesheet" />
    <link href="Content/select2.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />
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
                <div style="margin: 0 60px;">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <p align="right">
                                    <uc1:Logout ID="ctlLogout" runat="server" />
                                </p>
                            </div>
                            <div class="col-md-12">
                                <h5>Course Instance & Material</h5>
                                <hr />
                            </div>
                            <div class="col-md-5">
                                <div class="margin-right-15">
                                    <span>Select a Module Objective</span>
                                    <hr />
                                    <div class="row margin-bottom-10">
                                        <div class="col-md-4">
                                            <asp:Label CssClass="sp-label" ID="Label11" runat="server" Text="Course Instance"></asp:Label>
                                        </div>
                                        <div class="col-md-8">
                                            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseInstance" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseInstance_SelectedIndexChanged"></asp:DropDownList>
                                            <asp:Label ID="lblCourseId" runat="server" Text="" Visible="false"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row margin-bottom-10">
                                        <div class="col-md-4">
                                            <asp:Label CssClass="sp-label" ID="Label3" runat="server" Text="Module Objective: "></asp:Label>
                                        </div>
                                        <div class="col-md-8">
                                            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListModuleObjective" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListModuleObjective_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row margin-bottom-15">
                                        <div class="col-md-4">
                                            <asp:Label CssClass="sp-label" ID="LabelDueDate" runat="server" Text="Due Date"></asp:Label>
                                        </div>
                                        <div class="col-md-8">
                                            <div style="position: relative">
                                                <asp:TextBox CssClass="tex-box form-control DatePicker margin-bottom-10" ID="TextBoxDueDate" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>


                                </div>
                                <div class="mrgin-bottom-15">
                                    <strong>Or</strong> &nbsp; <span>Add New Material</span>
                                    <hr />
                                    <div class="row">
                                        <div class="col-md-3">
                                            <asp:Label CssClass="sp-label" ID="LabelTitle" runat="server" Text="Title"></asp:Label>
                                        </div>
                                        <div class="col-md-9">
                                            <asp:TextBox CssClass="tex-box form-control margin-bottom-10" ID="TextBoxTitle" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <asp:Label CssClass="sp-label" ID="Label5" runat="server" Text="Description"></asp:Label>
                                    <asp:TextBox CssClass="custome-textarea form-control margin-bottom-15" ID="TextBoxDescription" runat="server" TextMode="MultiLine"></asp:TextBox>

                                    <div class="row margin-bottom-10">
                                        <div class="col-md-3">
                                            <asp:Label CssClass="sp-label" ID="Label4" runat="server" Text="Active"></asp:Label>
                                            &nbsp;
                                    <asp:CheckBox ID="CheckBoxActive" runat="server" Checked="true" />
                                        </div>
                                        <div class="col-md-9 text-right">
                                            <asp:Button ID="AddNewMaterial" runat="server" CssClass="btn btn-custom btn-sm" Text="Add Material" OnClick="AddNewMaterial_Click" />
                                            
                                                <asp:Button ID="btnMaterialUpdate" runat="server" CssClass="btn btn-custom btn-sm" Text="Update" OnClick="btnMaterialUpdate_Click" />
                                         
                                        <asp:Button ID="btnMaterialDelete" BackColor="Red" OnClientClick="return confirm('Do you want to delete the material?');" ForeColor="White" runat="server" CssClass="btn btn-custom btn-sm" Text="Delete" OnClick="btnMaterialDelete_Click" />
                                    
                                        </div>
                                    </div>
                                    <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
                                    <asp:Label ID="lblErrorMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label><br />
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="margin-left-15 mrgin-bottom-15">
                                    <span>Filter & Select a Material</span>
                                    <hr />
                                    <div class="wraper-area margin-bottom-15">
                                        <div class="row margin-bottom-10">
                                            <div class="col-md-4">
                                                <asp:Label CssClass="sp-label" ID="Label6" runat="server" Text="Course Instance Filter:"></asp:Label>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseInstanceFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseInstanceFilter_SelectedIndexChanged"></asp:DropDownList>
                                                <asp:Label ID="Label7" runat="server" Text="" Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row margin-bottom-10">
                                            <div class="col-md-4">
                                                <asp:Label CssClass="sp-label" ID="Label8" runat="server" Text="Module Objective Filter: "></asp:Label>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListModuleObjectiveFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListModuleObjectiveFilter_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row margin-bottom-15">
                                        <div class="col-md-2">
                                            <asp:Label CssClass="sp-label" ID="Label10" runat="server" Text="Material : "></asp:Label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:DropDownList CssClass="tex-box form-control" ID="ddlMaterial" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMaterial_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="text-right">
                                        <asp:Button ID="btnClearAll" CssClass="btn btn-custom-light btn-sm" runat="server" Text="Clear All" OnClick="btnClearAll_Click" />
                                        &nbsp;
                                        <asp:Button ID="ButtonSubmitCourseInstanceMaterial" runat="server" CssClass="btn btn-custom " Text="Submit" OnClick="ButtonSubmitCourseInstanceMaterial_Click" />
                                   
                                        </div>

                                </div>
                            </div>
                            <div class="col-sm-12">
                                <div class="">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h5>Course Instance And Material List &nbsp;
                                                <asp:Button ID="Button2" runat="server" CssClass="btn btn-sm btn-custom-header" Text="Show All" OnClick="ShowAllModuleList_Click" /></h5>
                                        </div>

                                    </div>
                                    <hr style="margin-top: 8px;" />
                                    <div class="custome-overflow margin-top-10" style="max-height: 400px; height: auto">
                                        <asp:GridView CssClass="table table-bordered table-fixed grid-table" ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowCancelingEdit="OnRowCancelingEdit" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting">
                                            <Columns>
                                                <asp:TemplateField HeaderText="CourseInstanceId" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("CourseInstanceId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="MaterialId" Visible="False">

                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("MaterialId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ModuleObjectiveId" Visible="False">

                                                    <ItemTemplate>
                                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("ModuleObjectiveId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CourseInstance" HeaderText="Course Instance" ReadOnly="True" />
                                                <asp:BoundField DataField="Material" HeaderText="Material" ReadOnly="True" />
                                                <asp:BoundField DataField="ModuleObjective" HeaderText="Module Objective" ReadOnly="True" />
                                                <asp:TemplateField HeaderText="Due Date">
                                                    <EditItemTemplate>
                                                        <div style="position: relative">
                                                            <asp:TextBox ID="TextBox2" CssClass="tex-box form-control DatePicker" runat="server" Text='<%# Eval("DueDate", "{0:MMM-dd-yyyy}") %>'></asp:TextBox>
                                                        </div>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label4" runat="server" Text='<%# Eval("DueDate", "{0:MMM-dd-yyyy}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:CommandField ShowEditButton="True" />
                                                 <asp:TemplateField ShowHeader="true" Visible="true">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButtonDelete" runat="server" ForeColor="Red" CausesValidation="true" CommandName="Delete" Text="Delete"></asp:LinkButton>
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
    <script src="Scripts/jquery-ui.min.js"></script>
    <script src="Scripts/select2.min.js"></script>
    <%--<script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>--%>
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
            $("#ddlMaterial").select2();

            //$("[id*=GridView1]").DataTable(
            //    {
            //        bLengthChange: true,
            //        //lengthMenu: [[5, 10, -1], [5, 10, "All"]],
            //        bFilter: true,
            //        bSort: true,
            //        bPaginate: false
            //    });
        })
    </script>
</body>
</html>
