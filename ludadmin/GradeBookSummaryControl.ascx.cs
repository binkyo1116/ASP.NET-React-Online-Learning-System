using AdminPages.Services;
using EFModel;
using LMSLibrary;
using System;
using System.Linq;

namespace AdminPages
{
    public partial class GradeBookSummaryControl : System.Web.UI.UserControl
    {
        private readonly MaterialEntities data = new MaterialEntities();
        public CourseInstance CourseInstance { get; set; }
        public Student Student { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadPage();
        }
        private void LoadPage()
        {
            lblStudentName.Text = Student.Name;

            GradeScale gradeScale = CourseInstance.Course.GradeScaleGroup.GradeScales.FirstOrDefault();

            Gradebook courseGradebook = GradeBookService.GetGradebook(data, Student, CourseInstance);
            int totalGrade = (int)Math.Round(courseGradebook.CalculateWeightedGrade());
            int totalCompletion = (int)Math.Round(courseGradebook.CalculateTotalCompletion());
            double totalGPA = GradeBookService.GetGPAByPercent(totalGrade, gradeScale);
            int totalWeight = courseGradebook.GetTotalWeight();
            double totalWeightedGrade = GradingHelper.CalculateWeightedGrade(totalGrade, totalWeight);
            int totalActualGrade = GradeBookService.GetCurrentGrade(data, Student, CourseInstance);


            lblAssessmentWeight.Text = courseGradebook.Assessment.Weight.ToString() + "%";
            lblCurrentAssessmentGrade.Text = courseGradebook.Assessment.Grade.ToString() + "%";
            lblWeightedAssessmentGrade.Text = GradingHelper.CalculateWeightedGrade(courseGradebook.Assessment.Grade, courseGradebook.Assessment.Weight).ToString() + "%";

            int AddessmentCompletion = courseGradebook.Assessment.Completion;
            lblAssessmentCompletion.Text = AddessmentCompletion.ToString() + "%";
            idAssessmentCompletionProgressBar.Style.Clear();
            idAssessmentCompletionProgressBar.Style.Add("width", AddessmentCompletion + "%");
            idAssessmentCompletionProgressBar.Attributes.Add("aria-valuenow", AddessmentCompletion + "");

            //var AssessmentGPA = GradeBookService.GetGPAByPercent(courseGradebook.Assessment.Grade, gradeScale);

            //--------------------------------------------
            lblQuizWeight.Text = courseGradebook.Quiz.Weight.ToString() + "%";
            lblCurrentQuizGrade.Text = courseGradebook.Quiz.Grade.ToString() + "%";
            lblWeightedQuizGrade.Text = GradingHelper.CalculateWeightedGrade(courseGradebook.Quiz.Grade, courseGradebook.Quiz.Weight).ToString() + "%";

            int QuizCompletion = courseGradebook.Quiz.Completion;
            lblQuizCompletion.Text = QuizCompletion.ToString() + "%";
            idQuizCompletionProgressBar.Style.Clear();
            idQuizCompletionProgressBar.Style.Add("width", QuizCompletion + "%");
            idQuizCompletionProgressBar.Attributes.Add("aria-valuenow", QuizCompletion + "");


            //var QuizGPA = GradeBookService.GetGPAByPercent(courseGradebook.Quiz.Grade, gradeScale);

            //--------------------------------------------------
            //lblMaterialWeight.Text = courseGradebook.Material.Weight.ToString()+"%";
            //lblCurrentMaterialGrade.Text = courseGradebook.Material.Grade.ToString()+"%";
            //lblWeightedMaterialGrade.Text = GradingHelper.CalculateWeightedGrade(courseGradebook.Material.Grade, courseGradebook.Material.Weight).ToString()+"%";

            //var MaterialCompletion = courseGradebook.Material.Completion;
            //lblMaterialCompletion.Text = MaterialCompletion.ToString() + "%";
            //idMaterialCompletionProgressBar.Style.Clear();
            //idMaterialCompletionProgressBar.Style.Add("width", MaterialCompletion + "%");
            //idMaterialCompletionProgressBar.Attributes.Add("aria-valuenow", MaterialCompletion + "");


            //var MaterialGPA = GradeBookService.GetGPAByPercent(courseGradebook.Material.Grade, gradeScale);

            //----------------------------------------------------
            lblMidtermWeight.Text = courseGradebook.Midterm.Weight.ToString() + "%";
            lblCurrentMidtermGrade.Text = courseGradebook.Midterm.Grade.ToString() + "%";
            lblWeightedMidtermGrade.Text = GradingHelper.CalculateWeightedGrade(courseGradebook.Midterm.Grade, courseGradebook.Midterm.Weight).ToString() + "%";

            int MidtermCompletion = courseGradebook.Midterm.Completion;
            lblMidtermCompletion.Text = MidtermCompletion.ToString() + "%";
            idMidtermCompletionProgressBar.Style.Clear();
            idMidtermCompletionProgressBar.Style.Add("width", MidtermCompletion + "%");
            idMidtermCompletionProgressBar.Attributes.Add("aria-valuenow", MidtermCompletion + "");

            //var MidtermGPA = GradeBookService.GetGPAByPercent(courseGradebook.Midterm.Grade, gradeScale);

            //----------------------------------------------------
            lblFinalWeight.Text = courseGradebook.Final.Weight.ToString() + "%";
            lblCurrentFinalGrade.Text = courseGradebook.Final.Grade.ToString() + "%";
            lblWeightedFinalGrade.Text = GradingHelper.CalculateWeightedGrade(courseGradebook.Final.Grade, courseGradebook.Final.Weight).ToString() + "%";

            int FinalCompletion = courseGradebook.Final.Completion;
            lblFinalCompletion.Text = FinalCompletion.ToString() + "%";
            idFinalCompletionProgressBar.Style.Clear();
            idFinalCompletionProgressBar.Style.Add("width", FinalCompletion + "%");
            idFinalCompletionProgressBar.Attributes.Add("aria-valuenow", FinalCompletion + "");


            //var FinalGPA = GradeBookService.GetGPAByPercent(courseGradebook.Final.Grade, gradeScale);

            //---------------------------------------------------
            lblPollWeight.Text = courseGradebook.Poll.Weight.ToString() + "%";
            lblCurrentPollGrade.Text = courseGradebook.Poll.Grade.ToString() + "%";
            lblWeightedPollGrade.Text = GradingHelper.CalculateWeightedGrade(courseGradebook.Poll.Grade, courseGradebook.Poll.Weight).ToString() + "%";

            int PollCompletion = courseGradebook.Poll.Completion;
            lblPollCompletion.Text = PollCompletion.ToString() + "%";
            idPollCompletionProgressBar.Style.Clear();
            idPollCompletionProgressBar.Style.Add("width", PollCompletion + "%");
            idPollCompletionProgressBar.Attributes.Add("aria-valuenow", PollCompletion + "");


            //var PollGPA = GradeBookService.GetGPAByPercent(courseGradebook.Poll.Grade, gradeScale);

            //---------------------------------------------------
            //lblDiscussionWeight.Text = courseGradebook.Discussion.Weight.ToString()+"%";
            //lblCurrentDiscussionGrade.Text = courseGradebook.Discussion.Grade.ToString()+"%";
            //lblWeightedDiscussionGrade.Text = GradingHelper.CalculateWeightedGrade(courseGradebook.Discussion.Grade, courseGradebook.Discussion.Weight).ToString()+"%";

            //var DiscussionCompletion = courseGradebook.Discussion.Completion;
            //lblDiscussionCompletion.Text = DiscussionCompletion.ToString() + "%";
            //idDiscussionCompletionProgressBar.Style.Clear();
            //idDiscussionCompletionProgressBar.Style.Add("width", DiscussionCompletion + "%");
            //idDiscussionCompletionProgressBar.Attributes.Add("aria-valuenow", DiscussionCompletion + "");


            //var DiscussionGPA = GradeBookService.GetGPAByPercent(courseGradebook.Discussion.Grade, gradeScale);

            //var TotalWeight = totalWeight;
            lblTotalGrade.Text = totalGrade.ToString() + "%";
            //var TotalWeightedGrade = 10;
            lblTotalCompletion.Text = totalCompletion.ToString() + "%";
            //lblTotalGPA.Text = totalGPA.ToString();
            lblTotalCurrentGrade.Text = totalActualGrade.ToString() + "%";
            //lblTotalCurrentGPA.Text = GradeBookService.GetGPAByPercent(totalGrade, gradeScale).ToString();
        }
    }
}