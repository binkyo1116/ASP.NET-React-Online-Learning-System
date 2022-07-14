using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class ImageSubmission : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDownCourseInstance();
                //               BindGrid();
            }
        }
        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                if (DropDownListActivity.SelectedValue == "")
                {
                    return;
                }

                int codingProblemId = Convert.ToInt32(DropDownListActivity.SelectedValue);
                int courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);

                Solution.ImageUrl = @"data:image/png;base64," + db.CodingProblems.Where(x => x.Id == codingProblemId).FirstOrDefault().Solution;

                IQueryable<StudentGradable> model = from a in db.StudentGradables.Where(x => x.CodingProblemId == codingProblemId && x.CourseInstanceId == courseInstanceId) select a;
                if (CheckBoxNotCorrect.Checked)
                {
                    model = (from a in model.Where(x => x.Grade != 100) select a);
                }
                if (CheckBoxReviewed.Checked)
                {
                    model = (from a in model.Where(x => x.Student.Submissions.Where(y => y.CodingProblemId == codingProblemId && y.CourseInstanceId == courseInstanceId).OrderByDescending(s => s.Id).FirstOrDefault().Comment == null) select a);
                }

                var result = (from a in model
                              select new
                              {
                                  StGradableId = a.Id,
                                  SubmissionId = a.Student.Submissions.Where(x => x.CodingProblemId == codingProblemId && x.CourseInstanceId == courseInstanceId).OrderByDescending(s => s.Id).FirstOrDefault().Id,
                                  StudentName = a.Student.Name,
                                  StudentGrade = a.Grade,
                                  Comment = a.Student.Submissions.Where(x => x.CodingProblemId == codingProblemId && x.CourseInstanceId == courseInstanceId).OrderByDescending(s => s.Id).FirstOrDefault().Comment,
                                  StudentAnswer = @"data:image/png;base64," + a.Student.Submissions.Where(x => x.CodingProblemId == codingProblemId && x.CourseInstanceId == courseInstanceId).OrderByDescending(s => s.Id).FirstOrDefault().Code
                              }).ToList();

                GridView1.DataSource = result.OrderBy(y => y.StudentName).ToList();
                GridView1.DataBind();
            }
        }

        private void BindDropDownCourseInstance()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //---------------Coruse Droup Down list----------------
                IQueryable<CourseInstance> model = db.CourseInstances.Where(x => x.CourseInstanceActivities.Where(z => z.Activity.QuizQuestions.Where(y => y.QuizHints.Any()).Any()).Any());

                //DropDownListCourseInstance.DataSource = db.CourseInstances.Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourseInstance.DataSource = model.Select(x => new { x.Course.Name, Id = x.Id }).ToList();
                DropDownListCourseInstance.DataTextField = "Name";
                DropDownListCourseInstance.DataValueField = "Id";
                DropDownListCourseInstance.DataBind();
                DropDownListCourseInstance.Items.Insert(0, new ListItem("--Select Course Instance--", ""));
            }
        }
        protected void DropDownListCourseInstance_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCourseInstance.SelectedValue != "")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    Course course = db.CourseInstances.Find(Convert.ToInt32(DropDownListCourseInstance.SelectedValue)).Course;
                    //---------------Coruse Objective Droup Down list----------------
                    DropDownListCourseObjective.DataSource = course.CourseObjectives.Where(x => x.Active).Select(x => new { x.Id, x.Description }).OrderBy(y => y.Id).ToList();
                    DropDownListCourseObjective.DataTextField = "Description";
                    DropDownListCourseObjective.DataValueField = "Id";
                    DropDownListCourseObjective.DataBind();
                    DropDownListCourseObjective.Items.Insert(0, new ListItem("--Select Course Objective--", ""));
                }

                DropDownListModule.SelectedIndex = -1;
                DropDownListModuleObjective.SelectedIndex = -1;
                DropDownListActivity.SelectedIndex = -1;
                GridView1.DataSource = null;
                GridView1.DataBind();
            }
        }
        protected void DropDownListCourseObjective_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCourseObjective.SelectedValue != "")
            {
                int courseObjectiveId = Convert.ToInt32(DropDownListCourseObjective.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    //---------------Coruse Objective Droup Down list----------------
                    DropDownListModule.DataSource = db.CourseObjectives.Find(courseObjectiveId).Modules.Where(x => x.Active).Select(y => new { y.Id, y.Description }).OrderBy(x => x.Id).ToList();
                    DropDownListModule.DataTextField = "Description";
                    DropDownListModule.DataValueField = "Id";
                    DropDownListModule.DataBind();
                    DropDownListModule.Items.Insert(0, new ListItem("--Select Module--", ""));
                }

                DropDownListModuleObjective.SelectedIndex = -1;
                DropDownListActivity.SelectedIndex = -1;
                GridView1.DataSource = null;
                GridView1.DataBind();
            }
        }
        protected void DropDownListModule_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (DropDownListModule.SelectedValue != "")
            {
                int moduleId = Convert.ToInt32(DropDownListModule.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    //---------------Module Objective Droup Down list----------------
                    DropDownListModuleObjective.DataSource = db.Modules.Find(moduleId).ModuleObjectives.Where(x => x.Active).Select(y => new { y.Id, y.Description }).OrderBy(x => x.Id).ToList();
                    DropDownListModuleObjective.DataTextField = "Description";
                    DropDownListModuleObjective.DataValueField = "Id";
                    DropDownListModuleObjective.DataBind();
                    DropDownListModuleObjective.Items.Insert(0, new ListItem("--Select Module Objective--", ""));
                }

                DropDownListActivity.SelectedIndex = -1;
                GridView1.DataSource = null;
                GridView1.DataBind();

            }
        }
        protected void DropDownListModuleObjective_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownAssessment();
        }
        private void BindDropDownAssessment()
        {
            if (Convert.ToInt32(DropDownListModuleObjective.SelectedValue) > 0)
            {
                int moduleObjetiveId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    //---------------Coruse Objective Droup Down list----------------
                    int ciId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                    int moId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
                    IQueryable<CodingProblem> codingProblems = db.CourseInstanceCodingProblems.Where(x => x.CourseInstanceId == ciId && x.ModuleObjectiveId == moId && x.CodingProblem.Language == "Image").Select(y => y.CodingProblem);
                    DropDownListActivity.DataSource = codingProblems.Select(x => new { x.Id, x.Title }).OrderBy(x => x.Id).ToList();
                    DropDownListActivity.DataTextField = "Title";
                    DropDownListActivity.DataValueField = "Id";
                    DropDownListActivity.DataBind();
                    DropDownListActivity.Items.Insert(0, new ListItem("--Select One--", ""));
                }
                GridView1.DataSource = null;
                GridView1.DataBind();
            }
        }
        protected void DropDownListActivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListActivity.SelectedValue != "")
            {
                BindGrid();
            }
            else
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
            }
        }

        protected void CheckBoxNotCorrect_Changed(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void CheckBoxReviewed_Changed(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void SaveQuizHint(int rowIndex)
        {
            try
            {
                GridViewRow row = GridView1.Rows[rowIndex];
                int stGradableId = Convert.ToInt32(GridView1.DataKeys[rowIndex].Values[0]);
                int submissionId = Convert.ToInt32(GridView1.DataKeys[rowIndex].Values[1]);
                int grade = Convert.ToInt32((row.FindControl("TextBoxGrade") as TextBox).Text);
                string comment = (row.FindControl("TextBoxComment") as TextBox).Text;

                using (MaterialEntities db = new MaterialEntities())
                {
                    StudentGradable stGrade = db.StudentGradables.Find(stGradableId);
                    stGrade.Grade = grade;

                    Submission submission = db.Submissions.Find(submissionId);
                    submission.Comment = comment;

                    db.SaveChanges();
                    lblMessage.Text = "Save successfully!.";
                }
                if (grade == 100 && CheckBoxNotCorrect.Checked)
                {
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = 0;
            switch (e.CommandName)
            {
                case ("SubmitHint"):
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    SaveQuizHint(rowIndex);
                    BindGrid();
                    break;
            }
        }

        protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            BindGrid();
        }
        private void clearAll()
        {
            DropDownListCourseInstance.SelectedIndex = -1;
            DropDownListCourseObjective.SelectedIndex = -1;
            DropDownListModule.SelectedIndex = -1;
            DropDownListModuleObjective.SelectedIndex = -1;
            DropDownListActivity.SelectedIndex = -1;
            GridView1.DataSource = null;
            GridView1.DataBind();
        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            clearAll();
        }
    }
}