<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SupportTicketManagement.aspx.cs" Inherits="OnlineLearningSystem.SupportTicketManagement" %>
<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Let's Use Data</title>
    <link rel="favicon icon" href="favicon.png" type="image/png" />
    <link href="Content/select2.css" rel="stylesheet" />
    <%: Styles.Render("~/Content/css") %>
    <%-------------------------custome style------------------------------%>
    <style>
        .sp-label {
            margin-left: 5px;
            margin-right: 5px;
            font-weight: 500 !important;
        }

     /*   span.select2-dropdown {
            width: 700px !important;
        }*/

        .support-ticket-message-area {
            font-size: 15px;
        }

        .image-circle {
            height: 50px;
            width: 50px;
            border-radius: 50%;
        }

        .support-ticket-person span {
            font-size: 13px;
            color: #59595a;
            margin-left: 10px;
        }

        .support-ticket-person .support-ticket-time {
            font-size: 13px;
            color: #59595a;
            margin-left: 10px;
        }

        .support-ticket-person .support-ticket-person {
            font-size: 13px;
            color: #2c2c2c;
            margin-left: 10px;
            display: block;
        }

        .message-area .text-area {
            margin: 0 64px;
            padding: 1rem;
            border: 1px solid #e1e2e4;
        }

            .message-area .text-area p {
                font-size: 15px;
                margin: 0;
            }

        .support-ticket-person {
            display: flex;
        }

        .margin-top-5 {
            margin-top: 5px;
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
    <section class="content-area">
        <form id="form1" runat="server">
            <div class="add-course-area" style="margin: 0 60px;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <p align="right">
                                <uc1:Logout ID="Logout1" runat="server" />
                            </p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="course-list">
                                <h5>Support Ticket List</h5>
                                <hr />

                                <div class="row margin-bottom-10">
                                    <div class="col-md-3">
                                        <asp:Label CssClass="sp-label" ID="Label11" runat="server" Text="Course Instance"></asp:Label>
                                    </div>
                                    <div class="col-md">
                                        <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListCourseInstance" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCourseInstance_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row margin-bottom-10">
                                    <div class="col-md-3">
                                        <asp:Label CssClass="sp-label" ID="Label2" runat="server" Text="Student"></asp:Label>
                                    </div>
                                    <div class="col-md">
                                        <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListStudent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListStudent_SelectedIndexChanged"></asp:DropDownList>
                                    </div>

                                </div>
                                <div class="row margin-bottom-10">
                                    <div class="col-md-2">
                                        <asp:Label CssClass="sp-label" ID="Label6" runat="server" Text="Priority"></asp:Label>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListPriority" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListPriority_SelectedIndexChanged"></asp:DropDownList>

                                    </div>
                                    <div class="col-md-4">
                                        <asp:RadioButton ID="RadioButtonOpen" runat="server" GroupName="searchType" Checked="true" AutoPostBack="True" OnCheckedChanged="RadioButtonOpen_CheckedChanged" />
                                        <asp:Label ID="Label5" CssClass="font-size-15 margin-right-15" runat="server" Text="Open"></asp:Label>

                                        <asp:RadioButton ID="RadioButtonClose" runat="server" GroupName="searchType" AutoPostBack="True" OnCheckedChanged="RadioButtonClose_CheckedChanged" />
                                        <asp:Label ID="Label3" CssClass="font-size-15" runat="server" Text="Closed"></asp:Label>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Button ID="ShowAllList" runat="server" CssClass="btn btn-sm btn-custom-light" Text="Show All" OnClick="ShowAllList_Click" />
                                    </div>
                                </div>

                                <div class="custome-overflow margin-top-10" style="max-height: 400px; height: auto">
                                    <%--<asp:Label ID="LabelSelectedCourseId" runat="server" Text="" Visible="false"></asp:Label>--%>
                                    <asp:GridView CssClass="table table-bordered" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="GridView1_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" Visible="False" />
                                            <asp:TemplateField HeaderText="Token No">
                                                <EditItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("TokenNo") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("TokenNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Open Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("OpenedDate", "{0:MMM-dd-yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="StudentName" HeaderText="Student Name" ReadOnly="True" />
                                            <asp:BoundField DataField="OpeneStatus" HeaderText="Open Status" />
                                            <asp:BoundField DataField="Title" HeaderText="Title" ReadOnly="True" />
                                            <asp:BoundField DataField="Priority" HeaderText="Priority" ReadOnly="True" />
                                            <asp:BoundField DataField="CourseInstance" HeaderText="Course Instance" />
                                            <asp:BoundField DataField="UnreadMessage" HeaderText="Unread Message" />
                                            <asp:ButtonField Text="Show Detail" CommandName="ShowDetail" HeaderText="Action" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="margin-left-30">
                                <h5>Support Ticket Details</h5>
                                <hr />
                                <asp:Panel ID="TicketDetailArea" runat="server" Visible="false">
                                    <div class="margin-b-1">
                                        <div class="row">
                                            <div class="col-md-8">
                                                <asp:Label CssClass="sp-label" ID="lblSupportTicketId" runat="server" Text="" Visible="false"></asp:Label>
                                                <span class="sp-label">Subject: </span>
                                                <asp:Label CssClass="font-size-15" ID="lblTitle" runat="server" Text=""></asp:Label><br />
                                                <span class="sp-label">Course Name: </span>
                                                <asp:Label CssClass="font-size-15" ID="lblCourseInstance" runat="server" Text=""></asp:Label><br />
                                            </div>
                                            <div class="col-md-4">
                                                <span class="sp-label">Token No: </span>
                                                <asp:Label CssClass="font-size-13" ID="lblTokenNo" runat="server" Text=""></asp:Label><br />
                                                <span class="sp-label">Priority: </span>
                                                <asp:Label CssClass="font-size-13" ID="lblPriority" runat="server" Text=""></asp:Label><br />

                                            </div>
                                            <div class="col-md-3">
                                                <span class="sp-label">Status: </span>
                                                <asp:Label CssClass="sp-label" ID="lblOpenStatus" runat="server" Text=""></asp:Label><br />
                                            </div>
                                            <div class="col-md-5">
                                                <span class="sp-label">Open Date: </span>
                                                <asp:Label CssClass="sp-label" ID="lblOpenDate" runat="server" Text=""></asp:Label><br />
                                            </div>
                                            <div class="col-md-4 ">
                                                <asp:Button ID="btnCloseTicket" runat="server" CssClass="btn btn-custom btn-sm" Text="Close Ticket" Visible="false" OnClick="btnCloseTicket_Click" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="message-area custome-overflow wraper-area margin-top-10" style="max-height: 280px; height: auto">
                                        <asp:Panel ID="pnlSupportTicketDetail" runat="server">
                                        </asp:Panel>
                                    </div>
                                    <div id="TextMessagingArea" class="text-message-area margin-top-15">

                                        <asp:TextBox CssClass="form-control font-size-13 margin-bottom-15" Rows="2" ID="txtMessage" runat="server" TextMode="MultiLine" placeholder="Description"></asp:TextBox>
                                        <div class="row">
                                            <div class="col-md-6">

                                                <asp:FileUpload ID="fileUploadImage" CssClass="form-control font-size-13" runat="server" />
                                                <div class="support-ticket-image-area margin-t-1">
                                                    <asp:Image ID="ImageView" Width="300" runat="server" />
                                                </div>
                                            </div>

                                            <div class="col-md-6 text-right">
                                                <asp:Button ID="btnSubmitMessage" runat="server" CssClass="btn btn-custom btn-sm" Text="Submit" OnClick="btnSubmitMessage_Click" />
                                            </div>
                                        </div>
                                    </div>
                                    <asp:Label ID="lblMessage" CssClass="font-size-15 margin-bottom-15" runat="server" ForeColor="Red"></asp:Label>
                                </asp:Panel>
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
        $(document).ready(function () {
            $("#DropDownListCourseInstance").select2();
            $("#DropDownListStudent").select2();
        });
    </script>
</body>
</html>
