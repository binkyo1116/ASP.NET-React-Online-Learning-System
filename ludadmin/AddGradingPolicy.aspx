<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddGradingPolicy.aspx.cs" Inherits="OnlineLearningSystem.AddGradingPolicy" %>

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
                            <div class="col-sm-5">
                                <div class="margin-top-10 wraper-area">
                                    <h6>Add New Grading Policy</h6>
                                    <hr />

                                    <asp:Label CssClass="sp-label" ID="Label5" runat="server" Text="Select School: "></asp:Label>
                                    <asp:DropDownList CssClass="tex-box form-control margin-bottom-15" ID="DropDownListSchool" runat="server" AutoPostBack="True"></asp:DropDownList>

                                    <asp:Label CssClass="sp-label" ID="Label1" runat="server" Text="Description: "></asp:Label>
                                    <asp:TextBox ID="TextBoxDescription" CssClass="form-control margin-bottom-15 custome-textarea" runat="server" placeholder="" TextMode="MultiLine"></asp:TextBox>
                                    <div>
                                        <asp:Label CssClass="sp-label" ID="Label4" runat="server" Text="Active"></asp:Label>
                                        &nbsp;
                                          <asp:CheckBox ID="CheckBoxActive" runat="server" Checked="true" />
                                        &nbsp;
                                           <asp:Button ID="btnAddGradingPolicy" runat="server" CssClass="btn btn-custom btn-sm pull-right" Text="Add Grading" OnClick="btnAddGradingPolicy_Click" />
                                    </div>
                                    <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
                                    <asp:Label ID="lblErrorMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
                                    <br />
                                    <br />
                                </div>
                                <br />

                            </div>
                            <div class="col-md-7">
                                <div class="margin-top-10 margin-left-30">
                                    <h6>Grading Policy List</h6>
                                    <hr />
                                    <div class="font-size-13">
                                        <asp:GridView CssClass="table table-bordered grid-table" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCancelingEdit="OnRowCancelingEdit" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting">
                                            <Columns>
                                                <asp:BoundField DataField="Id" ItemStyle-HorizontalAlign="Center" HeaderText="ID" ReadOnly="True" />
                                                <asp:TemplateField HeaderText="SchoolId" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="SchoolId" runat="server" Text='<%# Bind("SchoolId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="School">
                                                    <EditItemTemplate>
                                                        <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListSchoolGV" runat="server"></asp:DropDownList>

                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("School") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" CssClass="custome-textarea form-control" TextMode="MultiLine" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>

                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Active">
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Active") %>' />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Active") %>' Enabled="false" />
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
