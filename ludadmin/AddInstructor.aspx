<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddInstructor.aspx.cs" Inherits="OnlineLearningSystem.AddInstructor" %>
<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                            <div class="col-md-6">
                                <div class="add-new-course-objective margin-top-10 wraper-area">

                                    <div class="row">
                                        <div class="col-md-6">
                                            <h6>Instructors</h6>
                                        </div>
                                        <div class="col-md-6 text-right">
                                            <asp:Button ID="btnClearAll" CssClass="btn btn-custom-light btn-sm" runat="server" Text="Clear All" OnClick="btnClearAll_Click" />
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="text-right">
                                    </div>
                                    <div class="row margin-bottom-15">
                                        <div class="col-md-5">
                                            <asp:Label CssClass="sp-label" ID="LableCrouse" runat="server" Text="Select Instructor: "></asp:Label>
                                        </div>
                                        <div class="col-md-7">
                                            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListInstructor" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListInstructor_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="row margin-bottom-15">
                                        <div class="col-md-5">
                                            <strong>Or, </strong>
                                            <asp:Label CssClass="sp-label" ID="Label111" runat="server" Text="Add New Instructor"></asp:Label>
                                        </div>
                                        <div class="col-md-7 text-right">
                                            <asp:TextBox ID="TextBoxInstructorName" CssClass="tex-box form-control" runat="server" placeholder="Instructor Name"></asp:TextBox>
                                        </div>
                                    </div>


                                    <div class="text-right">
                                        <asp:Button ID="btnAddInstructor" runat="server" CssClass="btn btn-custom btn-sm" Text="Add" OnClick="btnAddInstructor_Click" />
                                        <asp:Button ID="btnUpdateInstructor" runat="server" CssClass="btn btn-custom btn-sm" Text="Update" OnClick="btnUpdateInstructor_Click" />
                                        <asp:Button ID="btnDeleteInstructor" runat="server" BackColor="Red" CssClass="btn btn-custom btn-sm" Text="Delete" OnClick="btnDeleteInstructor_Click" />
                                      
                                        <div class="margin-top-10">
                                            <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
                                            <asp:Label ID="lblErrorMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
                                        </div>
                                        
                                    </div>
                                </div>
                                <br />
                                <%-----------------------Instructor Contact Info ----------------%>
                                <asp:Panel CssClass=" wraper-area" ID="PanelInstructorContactInfo" runat="server" Visible="false">
                                    <div class="instructor-contact-info">
                                        <h6>Instructors Contact Information</h6>

                                        <div class="row margin-bottom-15">
                                            <div class="col-md-6">
                                                <asp:Label CssClass="sp-label" ID="Label2" runat="server" Text="Contact Information"></asp:Label>
                                            </div>
                                            <div class="col-md-6 text-right">
                                                <asp:TextBox ID="TextBoxContactInfo" TextMode="MultiLine" CssClass="font-size-13 form-control font-size-13" runat="server" placeholder=""></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row margin-bottom-15">
                                            <div class="col-md-6">
                                                <asp:Label CssClass="sp-label" ID="Label1" runat="server" Text="Preferred"></asp:Label>
                                                &nbsp;
                                                 <asp:CheckBox ID="CheckBoxPreferred" runat="server" />
                                                &nbsp;  &nbsp;  &nbsp;
                                                <asp:Label CssClass="sp-label" ID="Label3" runat="server" Text="Active"></asp:Label>
                                                &nbsp;
                                             <asp:CheckBox ID="CheckBoxContactInfoActive" runat="server" Checked="true" />
                                            </div>
                                            <div class="col-md-6 text-right">
                                                <asp:Button ID="btnAddContactInfo" runat="server" CssClass="btn btn-custom btn-sm" Text="Add Contact" OnClick="btnAddContactInfo_Click" />
                                            </div>
                                        </div>

                                    </div>

                                    <%-----------------------Instructor Course ----------------%>
                                    <hr />
                                    <div class="instructor-Course">
                                        <h6>Instructor Course Instance</h6>

                                        <div class="row margin-bottom-15">
                                            <div class="col-md-5">
                                                <asp:Label CssClass="sp-label" ID="Label5" runat="server" Text="Select Course Instance: "></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourse" runat="server" AutoPostBack="True"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="row margin-bottom-15">
                                            <div class="col-md-5">
                                                <asp:Label CssClass="sp-label" ID="Label4" runat="server" Text="Role"></asp:Label>
                                            </div>
                                            <div class="col-md-7 text-right">
                                                <asp:TextBox ID="TextBoxRole" CssClass="tex-box form-control" runat="server" placeholder=""></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row margin-bottom-15">
                                            <div class="col-md-6">
                                                <asp:Label CssClass="sp-label" ID="Label7" runat="server" Text="Active"></asp:Label>
                                                &nbsp;
                                                <asp:CheckBox ID="CheckBoxInstructorCourseActive" runat="server" Checked="true" />
                                            </div>
                                            <div class="col-md-6 text-right">
                                                <asp:Button ID="btnAddInstructorCourse" runat="server" CssClass="btn btn-custom btn-sm" Text="Submit" OnClick="btnAddInstructorCourse_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="col-md-6">
                                <div class="course-list margin-top-10  wraper-area">
                                    <h6>Instructor Contact Information List</h6>
                                    <hr />
                                    <div class="font-size-13" style="margin-right: 0">
                                        <asp:GridView CssClass="table table-bordered grid-table" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCancelingEdit="OnRowCancelingEdit" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting">
                                            <Columns>
                                                <asp:BoundField DataField="Id" ItemStyle-HorizontalAlign="Center" HeaderText="ID" ReadOnly="True" />
                                                <asp:TemplateField HeaderText="ContactInfo">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox2" CssClass="form-control font-size-13" runat="server" Text='<%# Bind("ContactInfo") %>' TextMode="MultiLine"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("ContactInfo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Preferred ">
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Preferred") %>' />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBo1" runat="server" Checked='<%# Bind("Preferred") %>' Enabled="false" />
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
                                                        <asp:LinkButton ID="LinkButtonDelete" ForeColor="Red" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <br />
                                    <h6>Instructor Course Instance List</h6>
                                    <hr />
                                    <div class="font-size-13" style="margin-right: 0">
                                        <asp:GridView CssClass="table table-bordered grid-table" ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="CourseInstanceId" OnRowCancelingEdit="GridView2_RowCancelingEdit" OnRowEditing="GridView2_RowEditing" OnRowUpdating="GridView2_RowUpdating" OnRowDataBound="GridView2_RowDataBound" OnRowDeleting="GridView2_RowDeleting">
                                            <Columns>
                                                <asp:BoundField DataField="CourseInstanceId" HeaderText="CourseInstanceId" ReadOnly="True" Visible="False" />
                                                <asp:BoundField DataField="Course" HeaderText="Course" ReadOnly="True" />
                                                <asp:TemplateField HeaderText="Role">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox2" CssClass="form-control  font-size-13" runat="server" Text='<%# Bind("Role") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("Role") %>'></asp:Label>
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
