<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddGradeScale.aspx.cs" Inherits="OnlineLearningSystem.AddGradeScale" %>

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
    <section class="content-area margin-top-15 margin-bottom-100 margin-left-right-30 ">
        <div class="add-poll-area">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-2 border-right">
                        <div class="poll-left-panel">
                            <div class="nav flex-column nav-pills" id="v-pills-tab" role="tablist" aria-orientation="vertical">
                                <a class="nav-link btn-outline-box" onclick="setActiveClass(this.id)" id="v-pills-AddGradeScale" data-toggle="pill" href="#v-pills-AddGradeScale-tab" role="tab" aria-controls="v-pills-profile" aria-selected="false"><i class="fa fa-plus fa-1x "></i>&nbsp;Grade Scale</a>
                               <%-- <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-AddGradeScaleToCourse" data-toggle="pill" href="#v-pills-AddGradeScaleToCourse-tab" role="tab" aria-controls="v-pills-AddGradeScaleToCourse-tab" aria-selected="true"><i class="fa fa-list fa-1x "></i>&nbsp; Course & Grade Scale</a>--%>

                            </div>
                        </div>
                    </div>
                    <div class="col-sm-10">
                        <div class="Poll-area margin-top-15 margin-left-right-30">
                            <form id="form1" runat="server">
                                <div class="row">
                                    <div class="col-md-12">
                                        <p align="right">
                                            <asp:Button ID="btnClearAll" CssClass="btn btn-custom-light btn-sm" runat="server" Text="Clear All" OnClick="btnClearAll_Click" />
                                            <uc1:Logout ID="ctlLogout" runat="server" />
                                        </p>
                                    </div>
                                </div>
                                <div class="tab-content  margin-top-10" id="v-pills-tabContent">
                                    <%--========================================Add Grade Scale Panel =====================================--%>
                                    <div class="tab-pane fade" id="v-pills-AddGradeScale-tab" role="tabpanel" aria-labelledby="v-pills-AddGradeScale">
                                        <div class="row">
                                            <div class="col-sm-5">
                                                <div class="add-grade-scale">
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <h6>Grade Scale Group</h6>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <asp:Label CssClass="sp-label" ID="Label5" runat="server" Text="Select Grade Scale Group: "></asp:Label>
                                                    <asp:DropDownList ID="DropDownListGradeScaleGroup" CssClass="tex-box form-control form-control-sm margin-bottom-10" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListGradeScaleGroup_SelectedIndexChanged"></asp:DropDownList>

                                                    <strong>OR,</strong>
                                                    <asp:Label ID="Label6" CssClass="sp-label" runat="server" Text="Add New Grade Scale Group"></asp:Label>
                                                    <div class="wraper-area">
                                                        <asp:TextBox CssClass="tex-box form-control form-control-sm margin-bottom-15" ID="TextBoxGroupTitle" runat="server" placeholder="Group Title"></asp:TextBox>
                                                        <asp:Button ID="AddNewGradeScaleGroup" runat="server" CssClass="btn btn-custom btn-sm" Text="Add New" OnClick="AddNewGradeScaleGroup_Click" />
                                                        &nbsp;
                                                    <asp:Button ID="btnGradeScaleGroupUpdate" runat="server" CssClass="btn btn-custom btn-sm" Text="Update" OnClick="btnGradeScaleGroupUpdate_Click" />
                                                        &nbsp;
                                                    <asp:Button ID="btnGradeScaleGroupDelete" OnClientClick="return confirm('Do you want to delete this?');" BackColor="Red" runat="server" CssClass="btn btn-custom btn-sm" Text="Delete" OnClick="btnGradeScaleGroupDelete_Click" />
                                                    &nbsp;
                                                    
                                                    </div>
                                                    <br />
                                                    <div>
                                                        <h6>Add New Grade Scale</h6>
                                                        <hr />
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <asp:TextBox CssClass="tex-box form-control form-control-sm margin-bottom-10" type="number" step="0.01" ID="TextBoxMaxNumber" runat="server" placeholder="Max Number in %"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:TextBox CssClass="tex-box form-control form-control-sm margin-bottom-10" type="number" step="0.01" ID="TextBoxMinNumber" runat="server" placeholder="Min Number in %"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <asp:TextBox CssClass="tex-box form-control form-control-sm" type="number" step="0.01" ID="TextBoxGPA" runat="server" placeholder="GPA"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Button ID="AddNewGradeScale" runat="server" CssClass="btn btn-custom btn-sm" Text="Add New" OnClick="AddNewGradeScale_Click" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="margin-top-10">
                                                        <asp:Label ID="lblMessage" CssClass="font-size-15" ForeColor="Green" runat="server" Text=""></asp:Label>
                                                        <asp:Label ID="lblErrorMessage" CssClass="font-size-15" ForeColor="Red" runat="server" Text=""></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="poll-type-list margin-left-30">
                                                    <h6>Grade Scale List</h6>
                                                    <hr />
                                                    <div class="margin-bottom-10">
                                                        <strong>Group: </strong>
                                                        <asp:Label ID="LabelGradeScaleGroup" CssClass="font-size-15" runat="server" Text=""></asp:Label>
                                                    </div>
                                                    <div class="custome-overflow">
                                                        <asp:GridView CssClass="table table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowDataBound="OnRowDataBound" OnRowCancelingEdit="OnRowCancelingEdit" OnRowDeleting="OnRowDeleting" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating">
                                                            <Columns>
                                                                <asp:BoundField DataField="Id" ItemStyle-HorizontalAlign="Center" HeaderText="Id" ReadOnly="True" />

                                                                <asp:TemplateField HeaderText="Max Number">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="TextBox2" type="number" CssClass="form-control" runat="server" Text='<%# Bind("MaxNumberInPercent") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("MaxNumberInPercent") %>'></asp:Label>&nbsp;<span>%</span>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Min Number In Percent">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="TextBox3" type="number" CssClass="form-control" runat="server" Text='<%# Bind("MinNumberInPercent") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("MinNumberInPercent") %>'></asp:Label>&nbsp;<span>%</span>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="GPA">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="TextBox4" CssClass="form-control" runat="server" Text='<%# Bind("GPA") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("GPA") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:CommandField ShowEditButton="True" />
                                                                <asp:TemplateField ShowHeader="True">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="LinkButtonDelete" ForeColor="Red" runat="server" CausesValidation="True" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%--========================================Poll Question Panel =====================================--%>
                                    <div class="tab-pane fade" id="v-pills-AddGradeScaleToCourse-tab" role="tabpanel" aria-labelledby="v-pills-AddGradeScaleToCourse">
                                        <div class="row">
                                            <div class="col-sm-4">
                                                <div class="add-grade-scale">
                                                    <h6>Add Grade Scale into Course</h6>
                                                    <hr />
                                                    <asp:Label CssClass="sp-label" ID="Label1" runat="server" Text="Select Course: "></asp:Label>
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourse" runat="server"></asp:DropDownList>

                                                    <asp:Label CssClass="sp-label" ID="Label9" runat="server" Text="Select Grade Scale: "></asp:Label>
                                                    <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListGradeScaleGroup2" runat="server"></asp:DropDownList>

                                                    <br />
                                                    <asp:Button ID="btnAddScaleInCourse" runat="server" CssClass="btn btn-custom btn-sm" Text="Add Grade Scale Into Course" OnClick="btnAddScaleInCourse_Click" />
                                                    <br />
                                                    <asp:Label ID="lblMessage2" ForeColor="Red" runat="server" Text=""></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-8">
                                                <div class="poll-type-list margin-left-30">
                                                    <h6>Grade Scale Course List</h6>
                                                    <hr />
                                                    <div class="custome-overflow">
                                                        <asp:GridView CssClass="table table-bordered" ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="CourseId" OnRowDeleting="GridView2_RowDeleting" OnRowDataBound="GridView2_RowDataBound">
                                                            <Columns>
                                                                <asp:BoundField DataField="GradeScaleGroupId" HeaderText="GradeScaleGroupId" ReadOnly="True" Visible="False" />

                                                                <asp:BoundField DataField="Course" HeaderText="Course" ReadOnly="True" />
                                                                <asp:BoundField DataField="GradeScale" HeaderText="Grade Scale" />
                                                                <asp:TemplateField ShowHeader="True">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ForeColor="Red" ID="LinkButtonDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="CourseId" HeaderText="CourseId" Visible="False" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
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
    <script src="Scripts/select2.min.js"></script>
    <script>
        $(document).ready(function () {
            //----------------Remove Previous active class--------------------
            if (sessionStorage.getItem('btnId') != null) {
                $(".nav-link").removeClass('active');
                $(".tab-pane").removeClass('active');
                $(".tab-pane").removeClass('show');

                //------------------Add active class------------------------------
                var id = sessionStorage.getItem('btnId')
                $("#" + id).addClass('active');
                $("#" + id + "-tab").addClass('active');
                $("#" + id + "-tab").addClass('show');
            }
            else {
                $("#v-pills-AddGradeScale").addClass('active');
                $("#v-pills-AddGradeScale-tab").addClass('active');
                $("#v-pills-AddGradeScale-tab").addClass('show');
            }
            //========================================================================
            $("#DropDownListCourse").select2();

        });
        function setActiveClass(id) {
            sessionStorage.setItem('btnId', id);
        }
    </script>

</body>
</html>
