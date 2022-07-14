<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddNetiquette.aspx.cs" Inherits="AdminPages.AddNetiquette" %>

<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<link rel="favicon icon" href="favicon.png" type="image/png">
<head runat="server">
    <title>Let's Use Data</title>
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
    <section class="content-area margin-top-15 margin-bottom-100">
        <form id="form1" runat="server">

            <div class="add-poll-area" style="margin: 0 60px;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <p align="right">
                                <uc1:Logout ID="ctlLogout" runat="server" />
                            </p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2 border-right">
                            <div class="poll-left-panel">

                                <div class="nav flex-column nav-pills" id="v-pills-tab" role="tablist" aria-orientation="vertical">
                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-Netiquette" data-toggle="pill" href="#v-pills-Netiquette-tab" role="tab" aria-controls="v-pills-Netiquette" aria-selected="false"><i class="fa fa-list fa-1x "></i>&nbsp; Netiquette Description</a>
                                    <a class="nav-link" onclick="setActiveClass(this.id)" id="v-pills-NetiquetteLink" data-toggle="pill" href="#v-pills-NetiquetteLink-tab" role="tab" aria-controls="v-pills-NetiquetteLink-tab" aria-selected="true"><i class="fa fa-list fa-1x "></i>&nbsp; Netiquette Link</a>

                                </div>
                            </div>
                        </div>
                        <div class="col-sm-10">
                            <div class="Poll-area margin-top-10 margin-left-right-30">
                                <div class="tab-content" id="v-pills-tabContent">
                                    <%--======================================== =====================================--%>
                                    <div class="tab-pane fade" id="v-pills-Netiquette-tab" role="tabpanel" aria-labelledby="v-pills-Netiquette">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="course-instance ">
                                                    <div class="row">
                                                        <div class="col-md-8">
                                                            <h6>Netiquette</h6>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:Button ID="btnClearAll" CssClass="btn btn-custom-light btn-sm" runat="server" Text="Clear All" OnClick="btnClearAll_Click" />
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">
                                                        <div class="col-md-5">
                                                            <asp:Label CssClass="sp-label" ID="Label10" runat="server" Text="Select School"></asp:Label>
                                                        </div>
                                                        <div class="col-md-7">
                                                            <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListSchool" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListSchool_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <asp:Label CssClass="sp-label" ID="Label8" runat="server" Text="Select a Netiquette"></asp:Label>
                                                    <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListNetiquette" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListNetiquette_SelectedIndexChanged"></asp:DropDownList>

                                                    <strong>Or, </strong>
                                                    <asp:Label CssClass="sp-label" ID="Label11" runat="server" Text="Add New Netiquette"></asp:Label>
                                                    <br />

                                                    <asp:Label CssClass="sp-label" ID="Label9" runat="server" Text="Title "></asp:Label>
                                                    <asp:TextBox CssClass="form-control tex-box margin-bottom-10" ID="TextBoxSubTitle" placeholder="Title" runat="server"></asp:TextBox>

                                                    <asp:Label CssClass="sp-label" ID="Label12" runat="server" Text="Description"></asp:Label>
                                                    <asp:TextBox CssClass="form-control font-size-13 margin-bottom-10 custome-textarea" ID="TextBoxDescription" placeholder="Description" runat="server" TextMode="MultiLine"></asp:TextBox>

                                                    <asp:Label CssClass="sp-label" ID="Label4" runat="server" Text="Active"></asp:Label>
                                                    &nbsp;
                                                     <asp:CheckBox ID="CheckBoxActive" runat="server" Checked="true" />
                                                    <br />
                                                    <div class="text-right">
                                                        <asp:Button ID="btnAddNetiquette" runat="server" CssClass="btn btn-custom btn-sm" Text="Add" OnClick="btnAddNetiquette_Click" />
                                                        <asp:Button ID="btnUpdateNetiquette" runat="server" CssClass="btn btn-custom-light btn-sm" Text="Update" OnClick="btnUpdateNetiquette_Click" />
                                                        <asp:Button ID="btnDeleteNetiquette" OnClientClick="return confirm('Do you want to delete this?');" BackColor="Red" runat="server" CssClass="btn btn-custom btn-sm" Text="Delete" OnClick="btnDeleteNetiquette_Click" />
                                                        <br />
                                                        <div class="margin-top-10">
                                                          <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
                                                          <asp:Label ID="lblErrorMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-8">
                                                <div class="margin-left-30 ">
                                                    <h6>Add Course Policy Point</h6>
                                                    <hr />

                                                    <asp:Label CssClass="sp-label" ID="Label5" runat="server" Text="Description"></asp:Label>
                                                    <asp:TextBox CssClass="form-control font-size-13 margin-bottom-15 custome-textarea" ID="TextBoxPolicyPoint" placeholder="Course Description" runat="server" TextMode="MultiLine"></asp:TextBox>

                                                    <asp:Button ID="btnAddNetiquettePoint" runat="server" CssClass="btn btn-custom btn-sm" Text="Add Point" OnClick="btnAddNetiquettePoint_Click" />
                                                    <br />
                                                    <br />
                                                    <h6>Course Policy Point List</h6>
                                                    <hr />
                                                    <div class="custome-overflow">
                                                        <asp:Label ID="Label23" runat="server" Text="" Visible="false"></asp:Label>
                                                        <asp:GridView CssClass="table table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCancelingEdit="OnRowCancelingEdit" OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating">
                                                            <Columns>
                                                                <asp:BoundField DataField="Id" ItemStyle-HorizontalAlign="Center" HeaderText="ID" ReadOnly="True" />
                                                                <asp:TemplateField HeaderText="Description">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="TextBox1" CssClass="form-control custome-textarea" TextMode="MultiLine" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                                    </ItemTemplate>
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
                                    <%--============================================================================--%>
                                    <div class="tab-pane fade" id="v-pills-NetiquetteLink-tab" role="tabpanel" aria-labelledby="v-pills-NetiquetteLink">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="course-instance ">
                                                    <div class="row">
                                                        <div class="col-md-8">
                                                            <h6>Netiquette Link</h6>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">
                                                        <div class="col-md-5">
                                                            <asp:Label CssClass="sp-label" ID="Label6" runat="server" Text="Select School"></asp:Label>
                                                        </div>
                                                        <div class="col-md-7">
                                                            <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListSchoolForLink" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListSchoolForLink_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <asp:Label CssClass="sp-label" ID="Label3" runat="server" Text="Select a Netiquette"></asp:Label>
                                                    <asp:DropDownList CssClass="tex-box form-control margin-bottom-10" ID="DropDownListNetiquetteForLink" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListNetiquetteForLink_SelectedIndexChanged"></asp:DropDownList>

                                                    <asp:Label CssClass="sp-label" ID="Label7" runat="server" Text="Title "></asp:Label>
                                                    <asp:TextBox CssClass="form-control font-size-13 margin-bottom-10" ID="TextBoxLinkTitle" placeholder="Title" runat="server" TextMode="MultiLine"></asp:TextBox>

                                                    <asp:Label CssClass="sp-label" ID="Label13" runat="server" Text="Description"></asp:Label>
                                                    <asp:TextBox CssClass="form-control font-size-13 margin-bottom-10" ID="TextBoxLinkDescription" placeholder="Description" runat="server" TextMode="MultiLine"></asp:TextBox>

                                                    <asp:Label CssClass="sp-label" ID="Label2" runat="server" Text="Link"></asp:Label>
                                                    <asp:TextBox CssClass="form-control font-size-13 margin-bottom-10" ID="TextBoxLink" placeholder="Description" runat="server" TextMode="MultiLine"></asp:TextBox>

                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <asp:Label CssClass="sp-label" ID="Label14" runat="server" Text="Active"></asp:Label>
                                                            &nbsp;
                                                    <asp:CheckBox ID="CheckBoxLinkActive" runat="server" Checked="true" />
                                                        </div>
                                                        <div class="col-md-6 text-right">
                                                            <asp:Button ID="ButtonAddNetiquetteLink" runat="server" CssClass="btn btn-custom btn-sm" Text="Submit" OnClick="ButtonAddNetiquetteLink_Click" />
                                                        </div>
                                                    </div>
                                                    <div class="text-right margin-top-10">
                                                        <asp:Label ID="lblMessageLink" CssClass="font-size-15" runat="server" ForeColor="Green"></asp:Label>
                                                        <asp:Label ID="lblErrorMessageLink" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-8">
                                                <div class="margin-left-30 ">
                                                    <h6>Netiquette Link List</h6>
                                                    <hr />
                                                    <div class="custome-overflow">
                                                        <asp:Label ID="Label17" runat="server" Text="" Visible="false"></asp:Label>
                                                        <asp:GridView CssClass="table table-bordered" ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCancelingEdit="OnRowCancelingEditLink" OnRowDataBound="OnRowDataBoundLink" OnRowDeleting="OnRowDeletingLink" OnRowEditing="OnRowEditingLink" OnRowUpdating="OnRowUpdatingLink">
                                                            <Columns>
                                                                <asp:BoundField DataField="Id" ItemStyle-HorizontalAlign="Center" HeaderText="ID" ReadOnly="True" />
                                                                <asp:TemplateField HeaderText="Title">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="TextBox2" CssClass="form-control custome-textarea" TextMode="MultiLine" runat="server" Text='<%# Bind("Title") %>'></asp:TextBox>
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
                                                                <asp:TemplateField HeaderText="Link">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="TextBox3" CssClass="form-control custome-textarea" runat="server" TextMode="MultiLine" Text='<%# Bind("Link") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("Link") %>'></asp:Label>
                                                                    </ItemTemplate>
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
                                                                        <asp:LinkButton ForeColor="Red" ID="LinkButtonDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%--============================================================================--%>
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
    <script>
        $(document).ready(function () {
            //----------------Remove Previous active class--------------------
            if (sessionStorage.getItem('btnIdNetiquette') != null) {
                $(".nav-link").removeClass('active');
                $(".tab-pane").removeClass('active');
                $(".tab-pane").removeClass('show');

                //------------------Add active class------------------------------
                var id = sessionStorage.getItem('btnIdNetiquette')
                $("#" + id).addClass('active');
                $("#" + id + "-tab").addClass('active');
                $("#" + id + "-tab").addClass('show');
            }
            else {
                $("#v-pills-Netiquette").addClass('active');
                $("#v-pills-Netiquette-tab").addClass('active');
                $("#v-pills-Netiquette-tab").addClass('show');
            }
            //===============================================================
        });
        function setActiveClass(id) {
            sessionStorage.setItem('btnIdNetiquette', id);
        }


        $(document).ready(function () {

        });
    </script>

</body>
</html>
