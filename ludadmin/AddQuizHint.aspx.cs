using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddQuizHint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDownCourseInstance();
                BindGrid();
            }
        }
        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                IQueryable<QuizHint> model = from a in db.QuizHints select a;
                if (RadioButton1.Checked)
                {
                    model = (from a in model.Where(x => x.Reviewed == false) select a);
                }
                if (DropDownListActivity.SelectedValue != "")
                {
                    int activityId = Convert.ToInt32(DropDownListActivity.SelectedValue);
                    model = from a in model.Where(x => x.QuizQuestion.ActivityId1 == activityId) select a;
                }
                var result = (from a in model
                              select new { a.Id, a.QuestionId, StudentAnswer = a.StudentAnswer, Expected = a.QuizQuestion.Answer, QuizQuestion = a.QuizQuestion.Prompt1 + " ________ " + a.QuizQuestion.Prompt2, a.Correct, a.Hint }).ToList();

                GridView1.DataSource = result.OrderBy(y => y.QuestionId).ToList();
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
            BindDropDownActivity();
        }
        private void BindDropDownActivity()
        {
            if (Convert.ToInt32(DropDownListModuleObjective.SelectedValue) > 0)
            {
                int moduleObjetiveId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    //---------------Coruse Objective Droup Down list----------------
                    int ciId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                    int moId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
                    IQueryable<Activity> activities = db.CourseInstanceActivities.Where(x => x.CourseInstanceId == ciId && x.ModuleObjectiveId == moId).Select(y => y.Activity);
                    DropDownListActivity.DataSource = activities.Select(x => new { x.Id, x.Title }).OrderBy(x => x.Id).ToList();
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
            if (Convert.ToInt32(DropDownListActivity.SelectedValue) > 0)
            {
                BindGrid();
            }
        }

        private void SaveQuizHint(int rowIndex)
        {
            try
            {
                GridViewRow row = GridView1.Rows[rowIndex];
                int id = Convert.ToInt32(GridView1.DataKeys[rowIndex].Values[0]);
                string hint = (row.FindControl("TextBoxHint") as TextBox).Text;
                bool correct = (row.FindControl("CheckBoxCorrect") as CheckBox).Checked;

                using (MaterialEntities db = new MaterialEntities())
                {
                    QuizHint quizHind = db.QuizHints.Find(id);
                    quizHind.Hint = hint;
                    quizHind.Correct = correct;
                    quizHind.Reviewed = true;
                    db.SaveChanges();
                    lblMessage.Text = "Save successfully!.";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void UpdateQuizHint(int rowIndex)
        {
            //try
            //{
            //    int questionId = Convert.ToInt32(GridView2.DataKeys[rowIndex].Values[0]);

            //    GridViewRow row = GridView2.Rows[rowIndex];
            //    string hint = (row.FindControl("TextBoxHint") as TextBox).Text;
            //    if (!string.IsNullOrWhiteSpace(hint))
            //    {
            //        int hintId = Convert.ToInt32((row.FindControl("Label3") as Label).Text == "" ? "0" : (row.FindControl("Label3") as Label).Text);
            //        int quizId = Convert.ToInt32((row.FindControl("Label1") as Label).Text);

            //        var courseId = Convert.ToInt32(DropDownListCourse.SelectedValue);
            //        var courseObjectiveId = Convert.ToInt32(DropDownListCourseObjective.SelectedValue);
            //        var moduleId = Convert.ToInt32(DropDownListModule.SelectedValue);
            //        var moduleObjetiveId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
            //        var activityId = Convert.ToInt32(DropDownListActivity.SelectedValue);

            //        using (MaterialEntities db = new MaterialEntities())
            //        {
            //            if (hintId != 0)
            //            {
            //                var model = db.QuizHints.Where(x =>
            //                x.QuestionId == questionId
            //                && x.Id == hintId).FirstOrDefault();

            //                model.Hint = hint;
            //                db.SaveChanges();
            //                lblMessage.Text = "Update successfully!.";
            //            }
            //        }
            //    }
            //    else
            //    {
            //        lblMessage.Text = "Sorry! The Hint is required.";
            //    }

            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
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

        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = 0;
            switch (e.CommandName)
            {
                case ("UpdateHint"):
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    UpdateQuizHint(rowIndex);
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
            BindGrid();
        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            clearAll();
        }
    }
}