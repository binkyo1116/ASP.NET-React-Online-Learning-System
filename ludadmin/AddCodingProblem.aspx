<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="AddCodingProblem.aspx.cs" Inherits="OnlineLearningSystem.AddCodingProblem" ValidateRequest="false" %>

<%@ Register Src="Logout.ascx" TagName="Logout" TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Let's Use Data</title>
    <link rel="favicon icon" href="favicon.png" type="image/png" />
    <link href="Content/jquery-ui.css" rel="stylesheet" />
    <link href="Content/select2.css" rel="stylesheet" />

    <%--<link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />--%><%: Styles.Render("~/Content/css") %><%----------------------Custome Style-------------------------%>
    <style>
        .margin-bottom-8 {
            margin-bottom: 8px;
        }

        #cke_1_contents {
            height: 150px !important;
        }

        #TextBoxExpectedOutput {
            height: 195px !important;
        }

        #TextBoxTestCode {
            height: 195px !important;
        }

        #TextBoxTestCodeForStudent {
            height: 195px !important;
        }

        #cke_EditorInstruction {
            margin-bottom: 8px;
        }

        #cke_HighlightText .cke_top {
            display: none !important;
            visibility: hidden !important;
            font-size: 16px !important;
        }

        #cke_HighlightText {
            font-size: 13px !important;
            line-height: 21 !important;
        }

        body {
            font-family: 'Raleway', sans-serif;
            font-size: 16px;
            line-height: 21px;
            /* color: #3b4047; */
            background-color: #fff;
        }

        div#HighlightText {
            border: 1px solid gainsboro;
            border-radius: 3px;
        }

        div#HighlightText:focus {
            color: #495057;
            background-color: #fff;
            border-color: #80bdff;
            outline: 0 !important;
            box-shadow: 0 0 0 0.2rem rgb(0 123 255 / 25%);
        }

        .modal {
            margin-top: 76px;
            padding-bottom: 50px;
        }

        .hintsPicture {
            width: 70%;
        }


    </style>

    <!---------------- Global site tag (gtag.js) - Google Analytics ------------------>
    <script async src="https://www.googletagmanager.com/gtag/js?id=UA-196054626-1">
    </script>
    <script src='https://kit.fontawesome.com/a076d05399.js' crossorigin='anonymous'></script>
    <script>
        window.dataLayer = window.dataLayer || [];
        function gtag() { dataLayer.push(arguments); }
        gtag('js', new Date());

        gtag('config', 'UA-196054626-1');
    </script>
    <!------------------------------Close Google Analytics---------------------------->
    <!------------------------------Hints Modal -------------------------------------->
    <script>
        var req = new XMLHttpRequest();
        req.open('GET', '/files/hints.csv', true);
        req.onreadystatechange = function (aEvt) {
            if (req.readyState == 4) {
                if (req.status == 200) {
                    info = addHints(req.responseText);
                    document.getElementById("accordion").innerHTML = info;
                }
                else
                    document.getElementById("accordion").innerHTML = "<p>Hints file is not available</p>";
            }
        };
        req.send(null);

        function addHints(allText) {
            var allTextLines = allText.split(/\r\n|\n/);
            var headers = allTextLines[0].split(';');
            var lines = "";

            for (var i = 1; i < allTextLines.length; i++) {
                var data = allTextLines[i].split(';');
                if (data.length == headers.length) {
                    var accordionElement = `<div class="card">
                        <div class="card-header" id="heading`+ i + `">
                            <h2 class="mb-0">
                                <button class="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#collapse`+ i + `" aria-expanded="false" aria-controls="collapse` + i + `">
                                    <b>`+ data[1] + `: </b>` + data[0] + `
                                </button>
                            </h2>
                        </div>

                        <div id="collapse`+ i + `" class="collapse" aria-labelledby="heading` + i + `" data-parent="#accordion">
                            <div class="card-body">
                               <p><b>Syntax: `+ data[0] + `</b></p>
                               <p>`+ data[3] + `</p>
                               <p>Example:</p>
                               <img class="hintsPicture" src="/pictures/`+ data[2] + `">
                            </div>
                        </div>
                    </div>`;
                    lines += accordionElement;
                }
            }
            return lines;
        }
    </script>
    <!------------------------------Close Modal -------------------------------------->
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
    <%--==================================================================================--%><%--========================================Content area=================================--%>
    <section class="content-area margin-top-15 margin-bottom-100">
        <!-- Modal -->
        <div class="modal fade" id="hintsModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
          <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLabel">Metalanguage Hints</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div class="modal-body">
                <div class="accordion" id="accordion">
                  
                </div>
              </div>
            </div>
          </div>
        </div>
        <form id="form1" runat="server">
            <div class="add-course-area">
                <div style="margin: 0 60px;">
                    <div class="container-fluid">
                        <div id="modal"></div>
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="text-right margin-bottom-15">
                                    <button type="button" id="openHints" class="btn btn-custom-light btn-sm" data-toggle="modal" data-target="#hintsModal">Hints</button>
                                    <asp:Button ID="Button30" CssClass="btn btn-custom-light btn-sm" runat="server" PostBackUrl="~/AssignCodingProblemToCourseInstance.aspx" Text="Assign Coding Problem to Course Instance" />&nbsp;
                                    <asp:Button ID="btnClearAll" CssClass="btn btn-custom-light btn-sm" runat="server" Text="Clear All" OnClick="btnClearAll_Click" />&nbsp;
                                    <uc1:Logout ID="ctlLogout" runat="server" />
                                </div>
                            </div>
                            <div class="col-lg-12">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <h5>Add New Coding Problem</h5>
                                        <hr />
                                        <table id="SameRow" runat="server">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblMessage" CssClass="font-size-15" runat="server" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="btnMessageHelp"  runat="server" Visible ="false"><i id ="errorCompLink" href ="#errorCompDiv" style='font-size:24px;color:red' class='fas'>&#xf071;</i></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="wraper-area">
                                    <div class="custome-form-group">
                                        <div class="row">
                                            <div class="col-lg-4">
                                                <h6 class="margin-bottom-10">Coding Problem</h6>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="create-activity-area">
                                        <div class="row">
                                            <div class="col-sm-5">
                                                <div class="row">
                                                    <div class="col-lg-3">
                                                        <asp:Label ID="Label3" CssClass="sp-label" runat="server" Text="Title"></asp:Label>
                                                    </div>
                                                    <div class="col-lg">
                                                        <asp:TextBox CssClass="tex-box form-control" ID="TextBoxTitle" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-3 padding-r-0">
                                                        <asp:Label ID="Label7" CssClass="sp-label" runat="server" Text="Max Grade"></asp:Label>
                                                    </div>
                                                    <div class="col-lg-3 padding-r-0">
                                                        <asp:TextBox CssClass="tex-box form-control margin-bottom-8" ID="TextBoxMaxGrade" Text="100" type="number" runat="server"></asp:TextBox>
                                                    </div>
                                                    <div class="col-lg-2 padding-r-0">
                                                        <asp:Label ID="Label22" CssClass="sp-label" runat="server" Text="Type"></asp:Label>
                                                    </div>
                                                    <div class="col-lg-4">
                                                        <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListType" runat="server" OnSelectedIndexChanged="DropDownListType_SelectedIndexChanged" AutoPostBack="True">
                                                            <asp:ListItem Value="">--Select Type--</asp:ListItem>
                                                            <asp:ListItem Value="code">code</asp:ListItem>
                                                            <asp:ListItem Value="file">file</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-12">
                                                        <asp:Label ID="Label8" CssClass="sp-label" runat="server" Text="Instructions"></asp:Label>
                                                    </div>
                                                    <div class="col-lg">
                                                        <textarea style="border: none" id="EditorInstruction" runat="server"></textarea>
                                                        <script src="Scripts/ckeditor/ckeditor.js"></script>
                                                        <script>
                                                            CKEDITOR.replace('EditorInstruction');
                                                        </script>
                                                    </div>
                                                </div>
                                                <asp:Panel ID="PanelGrid" Visible="false" runat="server">
	                                                    <div class="row">
		                                                <div class="col-lg-12">
			                                                <asp:Label ID="Label5" CssClass="sp-label" runat="server" Text="Variables"></asp:Label>
		                                                </div>
		                                                <div class="col-lg">
			                                                <asp:Table ID ="TableValues" CssClass="table table-condensed table-striped" runat="server">
				                                                <asp:TableRow>
					                                                <asp:TableCell id="Cell11" Text="Name" />
					                                                <asp:TableCell id="Cell12" Text="Values" />
				                                                </asp:TableRow>
			                                                </asp:Table>
		                                                </div>
		                                                <div class="col-md-12">
			                                                <div class="margin-top-10" style="text-align: right;">
				                                                <asp:Button ID="btnAddVariable" runat="server" CssClass="btn btn-custom btn-sm" Text="Add new variable" OnClick="AddVariable" AutoPostBack="True"/>
                                                                <asp:Button ID="btnDelVariable" runat="server" CssClass="btn btn-custom btn-sm" Text="Delete last variable" OnClick="DelVariable" AutoPostBack="True"/>
				                                                &nbsp;
			                                                </div>
		                                                </div>
	                                                </div>
                                                </asp:Panel>
                                                <asp:HiddenField runat ="server" ID ="IndicateStart"/>
                                                <asp:Panel ID="PanelCodeInstance" Visible="false" runat="server">
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <asp:Label ID="Label11" CssClass="sp-label" runat="server" Text="Script"></asp:Label>
                                                        </div>
                                                        <div class="col-lg">
                                                            <asp:TextBox CssClass="form-control font-size-13" ID="TextBoxScript" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                            <asp:Button ID="setStartCode" CssClass="btn btn-custom btn-sm" runat="server" OnClientClick="ObtainFocusTextBoxScript()" OnClick="set_StartCode" Text="Set code limits" />
                                                            <%--<textarea class="form-control" id="HighlightText" oninput="HighlightSyntax()"></textarea>--%>
                                                            <%-- <script>
                                                                 CKEDITOR.replace('HighlightText');
                                                             </script>--%>
                                                            <%--<div style="height:100px" id="HighlightText" contenteditable="true" oninput="HighlightSyntax()"></div>--%>

                                                            <div id="ScriptTextShow"></div>
                                                        </div>
                                                    </div>
                                                    <div class="row margin-bottom-10">
                                                        <div class="col-lg-12">
                                                            <asp:Label ID="Label13" CssClass="sp-label" runat="server" Text="Solution"></asp:Label>
                                                        </div>
                                                        <div class="col-lg">
                                                            <asp:TextBox CssClass="form-control font-size-13" ID="TextBoxSolution" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                <asp:Panel ID="PanelTestCase" Visible="false" runat="server">
                                                    <div class="row">
                                                        <div class="col-lg-4">
                                                            <asp:Label ID="Label18" CssClass="sp-label" runat="server" Text="Test Case Class"></asp:Label>
                                                        </div>
                                                        <div class="col-lg">
                                                            <asp:TextBox CssClass="tex-box form-control" ID="TextBoxTestCaseClass" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-sm">
                                                <asp:Panel ID="PanelLanguage" Visible="true" runat="server">
                                                    <div class="row">
                                                        <div class="col-lg-4">
                                                            <asp:Label Visible="true" CssClass="sp-label" ID="LabelProgram" runat="server" Text="Program"></asp:Label>
                                                        </div>
                                                        <div class="col-lg">
                                                            <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListLanguage" runat="server" OnSelectedIndexChanged="DropDownListLanguage_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                <asp:Panel ID="PanelExcelFile" Visible="false" runat="server">
                                                    <div class="wraper-area margin-bottom-8">
                                                        <h6 class="margin-bottom-10 margin-top-10">Expected Output:</h6>
                                                        <div class="row">
                                                            <div class="col-lg-6">
                                                                <asp:FileUpload ID="FileUpload2" runat="server" />
                                                            </div>
                                                            <div class="col-lg-6 text-right">
                                                                <asp:Button ID="Button1" CssClass="btn btn-custom btn-sm" runat="server" OnClick="btnUploadExpectedOutputFile_Click" Text="Upload" />
                                                            </div>
                                                        </div>
                                                        <asp:Label ID="lblmessageFile2" ForeColor="Red" runat="server" />
                                                    </div>
                                                </asp:Panel>

                                                <asp:Panel ID="PanelCodeInstance2" Visible="false" runat="server">
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <asp:Label ID="Label20" CssClass="sp-label" runat="server" Text="Before"></asp:Label>
                                                        </div>
                                                        <div class="col-lg">
                                                            <asp:TextBox CssClass="form-control font-size-13" ID="TextBoxBefore" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row margin-bottom-10">
                                                        <div class="col-lg-12">
                                                            <asp:Label ID="Label21" CssClass="sp-label" runat="server" Text="After"></asp:Label>
                                                        </div>
                                                        <div class="col-lg">
                                                            <asp:TextBox CssClass="form-control font-size-13" ID="TextBoxAfter" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                <div class="row">
                                                    <div class="col-lg-3">
                                                        <asp:Label ID="Label24" CssClass="sp-label" runat="server" Text="Role"></asp:Label>
                                                    </div>
                                                    <div class="col-lg">
                                                        <asp:DropDownList CssClass="tex-box form-control" ID="DropDownListRole" runat="server">
                                                            <asp:ListItem Value="">--Select Role--</asp:ListItem>
                                                            <asp:ListItem Value="0">Assessment</asp:ListItem>
                                                            <asp:ListItem Value="1">Final</asp:ListItem>
                                                            <asp:ListItem Value="2">Midterm</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>

                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-5">
                                                        <asp:Label CssClass="sp-label" ID="Label4" runat="server" Text="Active"></asp:Label>
                                                        &nbsp;  
                                                            <asp:CheckBox ID="CheckBoxActive" runat="server" Checked="true" />
                                                    </div>
                                                    <div class="col-lg-3">
                                                        <asp:Label ID="Label23" CssClass="sp-label" runat="server" Text="Attempts"></asp:Label>
                                                    </div>
                                                    <div class="col-lg-4">
                                                        <asp:TextBox CssClass="tex-box form-control margin-bottom-8" ID="TextBoxAttempts" Text="100" type="number" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <asp:Panel ID="PanelFileSubmit" Visible="false" runat="server">
                                                    <div class="row text-left">
                                                        <div class="col-md-12">
                                                            <div class="margin-top-10">
                                                                <asp:Button ID="btnAddNewFileUploadProblem" runat="server" CssClass="btn btn-custom btn-sm" Text="Submit Coding Problem" OnClick="btnAddNewCodingProblem_Click" />                                                              
                                                                &nbsp;
                                                                <asp:Label ID="Label2" Visible="false" runat="server" Text="Label"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-sm">
                                                <asp:Panel ID="PanelExpectedOutput" Visible="false" runat="server">
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <asp:Label ID="Label12" CssClass="sp-label" runat="server" Text="Expected Output"></asp:Label>
                                                        </div>
                                                        <div class="col-lg">
                                                            <asp:TextBox CssClass="form-control font-size-13" ID="TextBoxExpectedOutput" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                            <asp:Button ID="SetGrades" CssClass="btn btn-custom btn-sm" runat="server" OnClick="SetGrades_Click" Text="Set Grades" />
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                <asp:Panel ID="PanelTestCode" Visible="false" runat="server">
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <asp:Label ID="Label1" CssClass="sp-label" runat="server" Text="Test Code"></asp:Label>
                                                        </div>
                                                        <div class="col-lg">
                                                            <asp:TextBox CssClass="form-control font-size-13" ID="TextBoxTestCode" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-12">
                                                            <asp:Label ID="Label6" CssClass="sp-label" runat="server" Text="Test Code For Student"></asp:Label>
                                                        </div>
                                                        <div class="col-lg">
                                                            <asp:TextBox CssClass="form-control font-size-13" ID="TextBoxTestCodeForStudent" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </asp:Panel>

                                                <asp:Panel ID="PanelCodeSubmit" Visible="false" runat="server">
                                                    <div class="row text-left">
                                                        <div class="col-md-12">
                                                            <div class="margin-top-10">
                                                                <asp:Button ID="btnAddNewCodingProblem" runat="server" CssClass="btn btn-custom btn-sm" Text="Submit Coding Problem" OnClick="btnAddNewCodingProblem_Click" />
                                                                <asp:Button ID="btnTestCodingProblem" runat="server" CssClass="btn btn-custom btn-sm" Text="Test Coding Problem" OnClick="btnTestCodingProblem_Click" Width="206px" />                                                                
                                                                &nbsp;
                                                                <asp:Label ID="lblCodingProblemId" Visible="false" runat="server" Text="Label"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                        <asp:Panel ID="ErrorCompilation" Visible="false" runat="server">
                                            <div id ="errorCompDiv" class="row">
                                                <div class="col-lg-12">
                                                    <asp:Label ID="CodeLbl" CssClass="sp-label" runat="server" ForeColor ="Red" Text="Compilation error"></asp:Label>
                                                </div>
                                                <div class="col-lg">
                                                    <div class="row">
                                                        <div class="col-lg">
                                                            <asp:TextBox CssClass="form-control font-size-13" ID="TextBoxAllCode" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg">
                                                            <asp:TextBox CssClass="form-control font-size-13" ID="TextBoxErrorComp" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    </section>
    <%--===================================================================================--%><%--========================================footer area=================================--%>
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
    <%--=============================================================================--%><%: Scripts.Render("~/bundles/js") %>
    <script src="Scripts/jquery-ui.min.js"></script>
    <script src="Scripts/select2.min.js"></script>
    <%--<script src="Scripts/ckeditor/ckeditor.js"></script>--%><%--<script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>--%>
    <script>
        CKEDITOR.replace('EditorInstruction');
   </script>
    <script>
        $(document).ready(function () {
            $("#DropDownListCourseFilter1").select2();
            $("#DropDownListCourseFilter2").select2();
            $("#DropDownListQuarterFilter2").select2();
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
        })
    </script>
    <%-- -----------------------------------------%>
    <script>
        var keywords = "for,return,foreach,static,struct,this,while,switch,abstract,as,base,bool,break,byte,case,catch,char,checked,class,const,continue,decimal,default,delegate,do,double,else,enum,event,explicit,extern,false,finally,fixed,float,goto,if,implicit,in,int,interface,internal,is,lock,long,namespace,new,null,object,operator,out,override,params,private,protected,public,readonly,ref,sbyte,sealed,short,sizeof,stackalloc,string,throw,true,try,typeof,uint,ulong,unchecked,unsafe,ushort,using,virtual,void,volatile";
        var tabSize = 4;
        var tab = "";
        var language = "C#";
        function Setup() {

            //for (let i = 0; i < tabSize; i++) tab += "\u0020";

            var elem = document.getElementById('HighlightText');
            elem.spellcheck = false;
            elem.focus();
            elem.blur();
            if (browserSupport()) {
                BuildNewHTML(elem);
                elem.addEventListener('input', HighlightSyntax);
            } else {
                //elem.addEventListener('input', NumerateLines);
            }


            return false;
        }
        //function NumerateLines() {
        //    var elem = document.getElementById('HighlightText');
        //    //var divHeight = elem.offsetHeight;
        //    var divHeight = elem.scrollHeight -
        //        parseFloat(window.getComputedStyle(elem, null).getPropertyValue('padding-top')) -
        //        parseFloat(window.getComputedStyle(elem, null).getPropertyValue('padding-bottom'));

        //    //var linesDiv = document.getElementById('lines');
        //    //linesDiv.innerHTML = ""; //remove previous lines

        //    var firstLine = AddNewLine(linesDiv, 1);
        //    var lines = elem.innerHTML.split('\n');
        //    var numberOfLines = lines.length;

        //    //var numberOfLines = Math.floor(divHeight / firstLine.offsetHeight);
        //    for (var i = 2; i <= numberOfLines; i++) {
        //        AddNewLine(linesDiv, i);
        //    }
        //    //linesDiv.scrollTop = elem.scrollTop;

        //    ShowErrors();
        //    UnderlineErrorLines(elem);
        //}
        function browserSupport() {
            // Get the user-agent string 
            var userAgentString =
                navigator.userAgent;

            // Detect Chrome 
            var chromeAgent =
                userAgentString.indexOf("Chrome") > -1;

            // Detect Internet Explorer 
            var IExplorerAgent =
                userAgentString.indexOf("MSIE") > -1 ||
                userAgentString.indexOf("rv:") > -1;

            // Detect Firefox 
            var firefoxAgent =
                userAgentString.indexOf("Firefox") > -1;

            // Detect Safari 
            var safariAgent =
                userAgentString.indexOf("Safari") > -1;

            // Discard Safari since it also matches Chrome 
            if ((chromeAgent) && (safariAgent))
                safariAgent = false;

            // Detect Opera 
            var operaAgent =
                userAgentString.indexOf("OP") > -1;

            // Discard Chrome since it also matches Opera      
            if ((chromeAgent) && (operaAgent))
                chromeAgent = false;

            return (chromeAgent || safariAgent);
            // return false;
        }

        function HighlightSyntax() {
            var elem = document.getElementById('HighlightText');
            ;

            var positionArr = getCaretPosition(elem);
            if (positionArr == undefined) {
                return;
            }
            var position = positionArr[0];

            //AdjustErrorList(elem, position);

            BuildNewHTML(elem);

            SetCaretPosition(elem, position);

        }
        function node_walk(node, func) {
            var result = func(node);
            for (node = node.firstChild; result !== false && node; node = node.nextSibling)
                result = node_walk(node, func);
            return result;
        };
        function SetCaretPosition(el, pos) {

            // Loop through all child nodes
            for (var node of el.childNodes) {
                if (node.nodeType == 3) { // we have a text node
                    if (node.length >= pos) {
                        // finally add our range
                        var range = document.createRange(),
                            sel = window.getSelection();
                        range.setStart(node, pos);
                        range.collapse(true);
                        sel.removeAllRanges();
                        sel.addRange(range);
                        return -1; // we are done
                    } else {
                        pos -= node.length;
                    }
                } else {
                    pos = SetCaretPosition(node, pos);
                    if (pos == -1) {
                        return -1; // no need to finish the for loop
                    }
                }
            }
            return pos; // needed because of recursion stuff
        }
        function BuildNewHTML(elem) {
            var words = splitIntoWords(elem);
            var newHTML = "";

            console.log(words);
            words.forEach(word => {

                if (style = getStyle(word)) {
                    word = word.replace('<', '&lt;').replace('>', '&gt;');
                    newHTML += "<span style='" + style + "'>" + word + "</span>";
                }
                else {
                    word = word.replace('<', '&lt;').replace('>', '&gt;');
                    newHTML += word;
                }

            });
            console.log(newHTML);
            //document.getElementById("ScriptTextShow").innerHTML = newHTML;

            elem.innerHTML = newHTML;
            //oldText = elem.textContent;
            //NumerateLines();
        }
        function getStyle(word) {

            var keywordsArray = keywords.split(",");
            var style = "";
            if ((word.length > 1) && IsOpeningComment(word[1], word[0])) {
                style = "color: rgb(0, 128, 0);";
            }
            else if ((word.length > 1) && (word[0] === '"')) {
                style = "color: rgb(163, 21, 21); ";
            } else if (keywordsArray.includes(word)) {
                style = "color: rgb(0, 0, 255); font-weight: bold;";
            }
            return style;
        }

        function splitIntoWords(el) {
            var text = el.textContent;
            var result = [];
            var endWordSymbols = ['\u00a0', '\u0020', '\u0009', '\n', '\t', ' ', '(', '{', '[', ')', '}', ']', '=', '+', '-', '/', '*', '<', '>', '!', ',', ';', ':', '?'];
            var currentWord = "";
            var isString = false;
            var isComment = false;
            var isBlockComment = false;

            //console.log(endWordSymbols.includes(" "));
            //console.log(endWordSymbols.includes(' '));

            function pushWordAndSymbol(symbol) {
                if (currentWord !== "") {
                    result.push(currentWord);
                    currentWord = "";
                }
                result.push(symbol);
            }
            function pushWordWithSymbol(symbol) {
                result.push(currentWord + symbol);
                currentWord = "";
            }
            function pushWordAddSymbol(symbol) {
                if (currentWord !== "") {
                    result.push(currentWord);
                }
                currentWord = symbol;
            }

            for (var i = 0; i < text.length; i++) {

                var symbol = text[i];

                if ((symbol === '\n') && !isBlockComment) {
                    isString = false;
                    isComment = false;
                    pushWordAndSymbol(symbol);
                } else if (symbol === '"' && isString && !isComment) {
                    isString = false;
                    pushWordWithSymbol(symbol);
                } else if (symbol === '"' && !isString && !isComment) {
                    isString = true;
                    pushWordAddSymbol(symbol);
                } else if (IsOpeningComment(symbol, text[i - 1]) && !isString) {
                    isComment = true;
                    currentWord += symbol;
                    isBlockComment = IsBlockComment(symbol, text[i - 1]);
                } else if (CommentStarts(symbol) && !isString && !isComment) {
                    pushWordAddSymbol(symbol);
                } else if (CommentEnds(symbol) && isComment && !isString) {
                    currentWord += symbol;
                } else if (IsClosingComment(symbol, text[i - 1]) && isBlockComment && !isString) {
                    isComment = false;
                    isBlockComment = false;
                    pushWordWithSymbol(symbol);
                } else if (endWordSymbols.includes(symbol) && !isComment && !isString) {
                    pushWordAndSymbol(symbol);
                } else {
                    currentWord += symbol;
                }

            }

            if (currentWord !== "") {
                result.push(currentWord);
            }

            return result;
        }
        //------------------------------------

        function CommentStarts(symbol) {
            switch (language) {
                case "C#":
                    if (symbol === '/') return true;
                    break;
                case "Java":
                    if (symbol === '/') return true;
                    break;
                case "Python":
                    if (symbol === '#') return true;
                    break;
                case "R":
                    if (symbol === '#') return true;
                    break;
                case "SQL":
                    if (symbol === '/') return true;
                    break;
            }
            return false;

        }

        function CommentEnds(symbol) {
            switch (language) {
                case "C#":
                    if (symbol === '*') return true;
                    break;
                case "Java":
                    if (symbol === '*') return true;
                    break;
                case "SQL":
                    if (symbol === '*') return true;
                    break;
            }
            return false;
        }

        function IsBlockComment(current, previous) {
            switch (language) {
                case "C#":
                    if ((previous === '/') && (current === '*')) return true;
                    break;
                case "Java":
                    if ((previous === '/') && (current === '*')) return true;
                    break;
                case "SQL":
                    if ((previous === '/') && (current === '*')) return true;
                    break;
            }
            return false;
        }

        function IsOpeningComment(current, previous) {
            switch (language) {
                case "C#":
                    if ((previous === '/') && ((current === '*') || (current === '/'))) return true;
                    break;
                case "Java":
                    if ((previous === '/') && ((current === '*') || (current === '/'))) return true;
                    break;
                case "Python":
                    if ((previous === '#')) return true;
                    break;
                case "R":
                    if ((previous === '#')) return true;
                    break;
                case "SQL":
                    if ((previous === '/') && (current === '*')) return true;
                    break;
            }
            return false;
        }

        function IsClosingComment(current, previous) {
            switch (language) {
                case "C#":
                    if ((previous === '*') && (current === '/')) return true;
                    break;
                case "Java":
                    if ((previous === '*') && (current === '/')) return true;
                    break;
                case "SQL":
                    if ((previous === '*') && (current === '/')) return true;
                    break;
            }
            return false;
        }
        function getCaretPosition(elem) {
            var sel = window.getSelection();
            var cum_length = [0, 0];

            if (sel.anchorNode == elem)
                cum_length = [sel.anchorOffset, sel.extentOffset];
            else {
                var nodes_to_find = [sel.anchorNode, sel.extentNode];
                if (!elem.contains(sel.anchorNode) || !elem.contains(sel.extentNode))
                    return undefined;
                else {
                    var found = [0, 0];
                    var i;
                    node_walk(elem, function (node) {
                        for (i = 0; i < 2; i++) {
                            if (node == nodes_to_find[i]) {
                                found[i] = true;
                                if (found[i == 0 ? 1 : 0])
                                    return false; // all done
                            }
                        }

                        if (node.textContent && !node.firstChild) {
                            for (i = 0; i < 2; i++) {
                                if (!found[i])
                                    cum_length[i] += node.textContent.length;
                            }
                        }
                    });
                    cum_length[0] += sel.anchorOffset;
                    cum_length[1] += sel.extentOffset;
                }
            }

            if (cum_length[0] <= cum_length[1])
                return cum_length;
            return [cum_length[1], cum_length[0]];
        }

    </script>
    <script type="text/javascript">
        function addRow() {
            var table = document.getElementById('gridVar');
            var rowCount = table.rows.length;
            var cellCount = table.rows[0].cells.length;
            var row = table.insertRow(rowCount);
            row.style = "background-color: white;";
            for (var i = 0; i <= cellCount; i++) {
                    var cell = 'col' + i;
                    cell = row.insertCell(i);
                    var copycel = document.getElementById('col' + i).innerHTML;
                    cell.innerHTML = copycel;
            }
        }

        function deleteRow() {
            var table = document.getElementById('gridVar');
            var rowCount = table.rows.length;
            if (rowCount > '2') {
                var row = table.deleteRow(rowCount - 1);
                rowCount--;
            }
            else {
                alert('There should be atleast one row');
            }
        }

        $('#errorCompLink').on('click', function (e) {
            e.preventDefault();
            $("html, body").animate({ scrollTop: $('#errorCompDiv').offset().top }, 1000);
        });
        
        //Obtain focus TextBoxString
        function ObtainFocusTextBoxScript() {
            var textBoxScriptElement = document.getElementById("TextBoxScript");
            var start = textBoxScriptElement.selectionStart;

            var textStringSel = textBoxScriptElement.textContent.substr(0, start);
            var cantEnter = (textStringSel.match(/\n/gm) || '').length + 1;

            start = parseInt(start) + parseInt(cantEnter);

            IndicateStart.value = start;
        }
    </script>
</body>
</html>

