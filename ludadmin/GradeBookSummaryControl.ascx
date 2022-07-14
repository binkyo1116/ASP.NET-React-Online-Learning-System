<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GradeBookSummaryControl.ascx.cs" Inherits="AdminPages.GradeBookSummaryControl" %>

<asp:Panel ID="Panel1" runat="server">
    <div class="wraper-area margin-bottom-15">
        <div class="course-static-area">
            <!-- -----------------------------calculate the students current and overall grade-------------------------------->
            <div class="row">
                <div class="col-md-6">
                    <div class="course-objective-intro">
                        <h4>Student Name: <asp:Label ID="lblStudentName" CssClass="result-span" runat="server" Text=""></asp:Label></h4>
                        <br />
                        <span>Course Current Grade: </span>
                        <asp:Label ID="lblTotalCurrentGrade" CssClass="result-span" runat="server" Text=""></asp:Label>
                        <br />
                        <%-- <span>Course Current GPA: </span>
                        <asp:Label ID="lblTotalCurrentGPA" runat="server" Text=""></asp:Label>
                        <br />--%>
                        <span>Course Predicted Grade: </span>
                        <asp:Label ID="lblTotalGrade" CssClass="result-span" runat="server" Text=""></asp:Label>
                        <br />
                       <%--<span>Course Predicted GPA: </span>
                        <asp:Label ID="lblTotalGPA" runat="server" Text=""></asp:Label>
                        <br />--%>

                        <asp:Label ID="lblTotalCompletion" CssClass="result-span" runat="server" Text=""></asp:Label>
                        <span> COMPLETED</span>
                        <br />
                        
                    </div>
                </div>
                <div class="col-md-6">
                    <div id="pnlOverallGrade" class="overall-grade margin-b-1">
                        <div>
                            <table class="table table-bordered font-size-13 box-bg-white">
                                <thead>
                                    <tr>
                                        <th></th>
                                        <th style="width: 30%; text-align: center">Completion</th>
                                        <th style="width: 15%; text-align: center">Weight</th>
                                        <th style="width: 15%; text-align: center">Grade</th>
                                        <th style="width: 15%; text-align: center">Total</th>

                                    </tr>
                                </thead>
                                <tbody>
                                    <tr id="assessmentRow">
                                        <td>Assessments</td>
                                        <td>
                                            <div class="progress">
                                                <div id="idAssessmentCompletionProgressBar" runat="server" class="progress-bar" role="progressbar" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                                <asp:Label CssClass="progress-bar-percentage" ID="lblAssessmentCompletion" runat="server" Text=""></asp:Label>
                                             <%--   <span class="progress-bar-percentage" id=""></span>--%>
                                            </div>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblAssessmentWeight" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblCurrentAssessmentGrade" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblWeightedAssessmentGrade" runat="server" Text=""></asp:Label>
                                        </td>

                                    </tr>
                                    <tr id="quizRow">
                                        <td>Quizzes</td>
                                        <td>
                                            <div class="progress">
                                                <div id="idQuizCompletionProgressBar" runat="server" class="progress-bar" role="progressbar" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                                 <asp:Label CssClass="progress-bar-percentage" ID="lblQuizCompletion" runat="server" Text=""></asp:Label>
                                            </div>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblQuizWeight" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblCurrentQuizGrade" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblWeightedQuizGrade" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                   <%-- <tr id="materialRow">
                                        <td>Materials</td>
                                        <td>
                                            <div class="progress">
                                                <div id="idMaterialCompletionProgressBar" runat="server" class="progress-bar" role="progressbar" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                                 <asp:Label CssClass="progress-bar-percentage" ID="lblMaterialCompletion" runat="server" Text=""></asp:Label>
                                            </div>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblMaterialWeight" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblCurrentMaterialGrade" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblWeightedMaterialGrade" runat="server" Text=""></asp:Label>
                                        </td>

                                    </tr>--%>
                                    <tr id="midtermRow">
                                        <td>Midterm</td>
                                        <td>
                                            <div class="progress">
                                                <div id="idMidtermCompletionProgressBar" runat="server" class="progress-bar" role="progressbar" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                                 <asp:Label CssClass="progress-bar-percentage" ID="lblMidtermCompletion" runat="server" Text=""></asp:Label>
                                            </div>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblMidtermWeight" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblCurrentMidtermGrade" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblWeightedMidtermGrade" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="finalRow">
                                        <td>Final</td>
                                        <td>
                                            <div class="progress">
                                                <div id="idFinalCompletionProgressBar" runat="server" class="progress-bar" role="progressbar" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                                 <asp:Label CssClass="progress-bar-percentage" ID="lblFinalCompletion" runat="server" Text=""></asp:Label>
                                            </div>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblFinalWeight" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblCurrentFinalGrade" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblWeightedFinalGrade" runat="server" Text=""></asp:Label>
                                        </td>

                                    </tr>
                                    <%--<tr id="discussionRow">
                                        <td>Discussion</td>
                                        <td>
                                            <div class="progress">
                                                <div id="idDiscussionCompletionProgressBar" runat="server" class="progress-bar" role="progressbar" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                                 <asp:Label CssClass="progress-bar-percentage" ID="lblDiscussionCompletion" runat="server" Text=""></asp:Label>
                                            </div>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblDiscussionWeight" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblCurrentDiscussionGrade" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblWeightedDiscussionGrade" runat="server" Text=""></asp:Label>
                                        </td>

                                    </tr>--%>
                                    <tr id="pollRow">
                                        <td>Poll</td>
                                        <td>
                                            <div class="progress">
                                                <div id="idPollCompletionProgressBar" runat="server" class="progress-bar" role="progressbar" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                                 <asp:Label CssClass="progress-bar-percentage" ID="lblPollCompletion" runat="server" Text=""></asp:Label>
                                            </div>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblPollWeight" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblCurrentPollGrade" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label ID="lblWeightedPollGrade" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>

                        </div>
                    </div>
                </div>
            </div>
            <!----------------------------------------------------------------------------------------------------------------------------->

            <%-- <div id="hr-tag"></div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div style="overflow-x: auto">
                                        <div id="headingQuiz"></div>
                                        <div id="my_dataviz"></div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div style="overflow-x: auto">
                                        <div id="headingAss"></div>
                                        <div id="my_assessment_chart"></div>
                                    </div>
                                </div>
                            </div>--%>
        </div>
    </div>
</asp:Panel>
