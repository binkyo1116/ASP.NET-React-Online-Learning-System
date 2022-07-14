<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCourse.aspx.cs" Inherits="OnlineLearningSystem.AddCourse" %>

<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<link rel="favicon icon" href="favicon.png" type="image/png">
<head runat="server">
    <title>Let's Use Data</title>
    <%: Styles.Render("~/Content/css") %>
    <%----------------------Custome Style-------------------------%>
    <style>
        .grid-table {
            max-width: 1150px !important;
        }

            .grid-table tbody tr th {
                text-align: center;
            }

        .description-cell {
            /*width:385px  !important;*/
        }

        .grid-textarea {
            height: 300px !important;
            font-size: 13px;
        }

        .grid-text-box {
            font-size: 13px;
        }

        .textbox-width {
            width: 130px !important;
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
                        <p align="right">
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
                                <uc1:Logout ID="ctlLogout" runat="server" />
                            </p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 ">
                            <div class="add-new-school">
                                <h5>Add New Course</h5>
                                <hr />
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Label CssClass="font-size-15" ID="lblCourseName" runat="server" Text="Name: "></asp:Label>
                                    </div>
                                    <div class="col-md-9">
                                        <asp:TextBox CssClass="tex-box form-control margin-bottom-10" ID="TextBoxCourseName" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Label CssClass="font-size-15" ID="LabelSchool" runat="server" Text="School"></asp:Label>
                                    </div>
                                    <div class="col-md-9">
                                        <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListSchool" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Label CssClass="font-size-15" ID="Label8" runat="server" Text="Grade"></asp:Label>
                                    </div>
                                    <div class="col-md-9">
                                        <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListGradeScale" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Label CssClass="font-size-15" ID="Label5" runat="server" Text="Department"></asp:Label>
                                    </div>
                                    <div class="col-md-9">
                                        <asp:TextBox CssClass="tex-box form-control margin-bottom-10" ID="TextBoxDepartment" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Label CssClass="font-size-15" ID="Label2" runat="server" Text="Credits: "></asp:Label>
                                    </div>
                                    <div class="col-md-9">
                                        <asp:TextBox CssClass="tex-box form-control margin-bottom-10" type="number" ID="TextBoxCredits" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Label CssClass="font-size-15" ID="Labe0l3" runat="server" Text="Number: "></asp:Label>
                                    </div>
                                    <div class="col-md-9">
                                        <asp:TextBox CssClass="tex-box form-control margin-bottom-10" type="number" ID="TextBoxNumber" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Label CssClass="font-size-15 margin-bottom-5 display-block " ID="Label4" runat="server" Text="Description: "></asp:Label>
                                    </div>
                                    <div class="col-md-12">
                                        <asp:TextBox CssClass="custome-textarea form-control margin-bottom-15" TextMode="MultiLine" ID="TextBoxDescription" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="update-profile-image margin-bottom-15">
                                    <asp:FileUpload ID="FileUploadImage" CssClass="form-control font-size-13" runat="server" />
                                </div>

                                <asp:Button ID="AddNewCourse" runat="server" CssClass="btn btn-custom margin-bottom-10 btn-sm" Text="Add New" OnClick="AddNewCourse_Click" />
                                <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label><br />
                                 <asp:Label ID="lblErrorMessage1" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label><br />
                            </div>
                        </div>
                        <div class="col-sm-9 ">
                            <div class="course-list margin-left-30">
                                <div class="row">
                                    <div class="col-md-2">
                                         <h5>Course List</h5>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="lblErrorMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
                                          <asp:Label ID="lblSuccessMessage" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
                                    </div>
                                    <div class="col-md-2 text-right">
                                        <asp:Button ID="ShowAllList" runat="server" CssClass="btn btn-sm btn-custom-light" Text="Show All" OnClick="ShowAllList_Click" />
                                    </div>
                                </div>
                               
                                <hr style="margin-top:5px" />
                                <div class="row">
                                    <div class="col-md-6 margin-bottom-15">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:Label CssClass="sp-label" ID="Label11" runat="server" Text="Select a School"></asp:Label>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListSearchBySchool" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListSearchBySchool_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 margin-bottom-15 text-right">

                                        
                                    </div>
                                </div>
                                <div class="custome-overflow">
                                    <asp:GridView CssClass="table table-bordered grid-table" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="CourseId" OnRowDataBound="OnRowDataBound" OnRowCancelingEdit="OnRowCancelingEdit" OnRowDeleting="OnRowDeleting" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating">
                                        <Columns>
                                            <asp:BoundField ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" DataField="CourseId" HeaderText="Course ID" ReadOnly="True" />
                                            <asp:TemplateField HeaderText="Course Name">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox1" TextMode="MultiLine" CssClass="form-control grid-text-box" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SchoolId" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label7" runat="server" Text='<%# Bind("SchoolId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="School">
                                                <EditItemTemplate>
                                                    <asp:DropDownList CssClass="form-control font-size-13 textbox-width" ID="DropDownListSchoolGV" runat="server"></asp:DropDownList>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("School") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="GradeScaleGroupId" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label8" runat="server" Text='<%# Bind("GradeScaleGroupId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="GradeScaleGroup">
                                                <EditItemTemplate>
                                                    <asp:DropDownList CssClass="form-control font-size-13 textbox-width" ID="DropDownListGradeScaleGroup" runat="server"></asp:DropDownList>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label19" runat="server" Text='<%# Bind("GradeScaleGroup") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Credits">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox2" CssClass="form-control grid-text-box" runat="server" Text='<%# Bind("Credits") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("Credits") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Department">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox3" CssClass="form-control grid-text-box" runat="server" Text='<%# Bind("Department") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("Department") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Number">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox4" CssClass="form-control grid-text-box" runat="server" Text='<%# Bind("Number") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("Number") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="description-cell">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox5" CssClass="form-control grid-textarea" TextMode="MultiLine" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                </ItemTemplate>

                                                <HeaderStyle CssClass="description-cell"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Picture">
                                                <EditItemTemplate>
                                                    <asp:FileUpload ID="FileUploadImage" CssClass="form-control grid-text-box textbox-width " runat="server" />
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Image ID="CourseImageView" ImageUrl='<%# "data:image;base64," + Convert.ToBase64String((byte[])Eval("Picture")) %>' Width="50" runat="server" />
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
