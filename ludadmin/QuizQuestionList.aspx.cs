using EFModel;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class QuizQuestionList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }
        private void BindGrid()
        {
            //using (MaterialEntities db = new MaterialEntities())
            //{
            //    var model = from a in db.Quizs
            //                select new { ActivityId = a.ActivityId, QuizId = a.QuizId, CourseId = a.CourseId, CourseObjectiveId = a.CourseObjectiveId, ModuleId = a.ModuleId, ModuleObjectiveId = a.ModuleObjectiveId, Title = a.Activity.Title, Type = a.Activity.Type, MaxGrade = a.Activity.MaxGrade, ModuleObjective = a.Activity.ModuleObjective.Description, Module = a.Activity.ModuleObjective.Module.Description, CourseObjective = a.Activity.ModuleObjective.Module.CourseObjective.Description, Course = a.Activity.ModuleObjective.Module.CourseObjective.Course.Name };
            //    GridView1.DataSource = model.OrderBy(x => x.ActivityId);
            //    GridView1.DataBind();
            //}
        }
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            string title = (row.FindControl("TextBox2") as TextBox).Text;
            string type = (row.FindControl("TextBox1") as TextBox).Text;
            int activityMaxGrade = Convert.ToInt32((row.FindControl("TextBox3") as TextBox).Text);

            int courseId = Convert.ToInt32((row.FindControl("LabelCourseId") as Label).Text);
            int courseObjectiveId = Convert.ToInt32((row.FindControl("LabelCourseObjectiveId") as Label).Text);
            int moduleId = Convert.ToInt32((row.FindControl("LabelModuleId") as Label).Text);
            int moduleObjectiveId = Convert.ToInt32((row.FindControl("LabelModuleObjectiveId") as Label).Text);

            using (MaterialEntities db = new MaterialEntities())
            {
                //-------------Activity table---------------------
                Activity activity = db.Activities.FirstOrDefault(x => x.Id == id);
                activity.Title = title;
                activity.Type = type;
                activity.MaxGrade = activityMaxGrade;
                db.SaveChanges();
            }
            lblMessage.Text = "Update Successfully";
            GridView1.EditIndex = -1;
            BindGrid();
        }
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int courseId = Convert.ToInt32((row.FindControl("LabelCourseId") as Label).Text);
            int courseObjectiveId = Convert.ToInt32((row.FindControl("LabelCourseObjectiveId") as Label).Text);
            int moduleId = Convert.ToInt32((row.FindControl("LabelModuleId") as Label).Text);
            int moduleObjectiveId = Convert.ToInt32((row.FindControl("LabelModuleObjectiveId") as Label).Text);

            using (MaterialEntities db = new MaterialEntities())
            {

                //var quizQuestions = db.QuizQuestions.Where(x => x.CourseId == courseId
                //                                       && x.CourseObjectiveId == courseObjectiveId
                //                                       && x.ModuleId == moduleId
                //                                       && x.ModuleObjectiveId == moduleObjectiveId
                //                                       && x.ActivityId == id);
                //TODO: Make sure this is correct
                //db.QuizQuestions.RemoveRange(quizQuestions);

                //Quiz quiz = db.Quizs.Where(x => x.CourseId == courseId
                //                                       && x.CourseObjectiveId == courseObjectiveId
                //                                       && x.ModuleId == moduleId
                //                                       && x.ModuleObjectiveId == moduleObjectiveId
                //                                       && x.ActivityId == id).FirstOrDefault();
                //TODO: Make sure this is correct
                //db.Quizs.Remove(quiz);

                //Activity activity = quiz.Activity;
                //TODO: Make sure this is correct
                //db.Activities.Remove(activity);
                db.SaveChanges();
            }
            lblMessage.Text = "Delete Successfully";
            BindGrid();
        }
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    //-----------------Grid Sorting course----------------
                    DropDownList DropDownListSortCourse = (DropDownList)e.Row.FindControl("DropDownListSortCourse");
                    DropDownListSortCourse.DataSource = db.Courses.OrderBy(x => x.Id);
                    DropDownListSortCourse.DataTextField = "Name";
                    DropDownListSortCourse.DataValueField = "Id";
                    DropDownListSortCourse.DataBind();
                    DropDownListSortCourse.Items.Insert(0, new ListItem("--Select One--"));
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row with ID = " + DataBinder.Eval(e.Row.DataItem, "ActivityId") + "?');";
            }
        }

        protected void btnAddActivity_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "OpenWindow", "window.open('AddQuizQuestion.aspx','_newtab');", true);
        }

        #region --------------------------Grid view Shorting-----------------------------------
        protected void DropDownListSortCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            LabelSelectedCourseId.Text = (sender as DropDownList).SelectedValue;
            LabelSelectedCourseObjectiveId.Text = "";
            LabelSelectedModuleId.Text = "";
            LabelSelectedModuleObjectiveId.Text = "";

            if (LabelSelectedCourseId.Text != "")
            {
                BindGrid();
                BindHeaderDropDownCourseObjective();
            }
        }
        protected void DropDownListSortCourseObjective_SelectedIndexChanged(object sender, EventArgs e)
        {
            LabelSelectedCourseObjectiveId.Text = (sender as DropDownList).SelectedValue;
            LabelSelectedModuleId.Text = "";
            LabelSelectedModuleObjectiveId.Text = "";
            if (LabelSelectedCourseId.Text != "" && LabelSelectedCourseObjectiveId.Text != "")
            {
                BindGrid();
                BindHeaderDropDownCourseObjective();
                BindHeaderDropDownModule();
            }
        }
        protected void DropDownListSortModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            LabelSelectedModuleId.Text = (sender as DropDownList).SelectedValue;
            LabelSelectedModuleObjectiveId.Text = "";
            if (LabelSelectedCourseId.Text != "" && LabelSelectedCourseObjectiveId.Text != "" && LabelSelectedModuleId.Text != "")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    BindGrid();
                    BindHeaderDropDownCourseObjective();
                    BindHeaderDropDownModule();
                    BindHeaderDropDownModuleObjective();
                }
            }
        }
        protected void DropDownListSortModuleObjective_SelectedIndexChanged(object sender, EventArgs e)
        {
            LabelSelectedModuleObjectiveId.Text = (sender as DropDownList).SelectedValue;
            if (LabelSelectedCourseId.Text != "" && LabelSelectedCourseObjectiveId.Text != "" && LabelSelectedModuleId.Text != ""
                        && LabelSelectedModuleObjectiveId.Text != "")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    BindGrid();
                    BindHeaderDropDownCourseObjective();
                    BindHeaderDropDownModule();
                    BindHeaderDropDownModuleObjective();
                }
            }
        }

        //private void BindGrid()
        //{
        //    using (MaterialEntities db = new MaterialEntities())
        //    {
        //        if (LabelSelectedCourseId.Text == "")
        //        {
        //            var model = db.Quizs;
        //            SetDatainGrid(model, db);
        //        }
        //        else if (LabelSelectedCourseId.Text != "" && LabelSelectedCourseObjectiveId.Text == "")
        //        {
        //            var model = db.Quizs.Where(a => a.CourseId == Convert.ToInt32(LabelSelectedCourseId.Text));
        //            SetDatainGrid(model, db);
        //        }
        //        else if (LabelSelectedCourseObjectiveId.Text != "" && LabelSelectedModuleId.Text == "")
        //        {
        //            var model = db.Quizs.Where(a => a.CourseId == Convert.ToInt32(LabelSelectedCourseId.Text)
        //                        && a.CourseObjectiveId == Convert.ToInt32(LabelSelectedCourseObjectiveId.Text));
        //            SetDatainGrid(model, db);
        //        }
        //        else if (LabelSelectedModuleId.Text != "" && LabelSelectedModuleObjectiveId.Text == "")
        //        {
        //            var model = db.Quizs.Where(a => a.CourseId == Convert.ToInt32(LabelSelectedCourseId.Text)
        //                        && a.CourseObjectiveId == Convert.ToInt32(LabelSelectedCourseObjectiveId.Text)
        //                        && a.ModuleId == Convert.ToInt32(LabelSelectedModuleId.Text));
        //            SetDatainGrid(model, db);
        //        }
        //        else if (LabelSelectedModuleObjectiveId.Text != "")
        //        {
        //            var model = db.Quizs.Where(a => a.CourseId == Convert.ToInt32(LabelSelectedCourseId.Text)
        //                        && a.CourseObjectiveId == Convert.ToInt32(LabelSelectedCourseObjectiveId.Text)
        //                        && a.ModuleId == Convert.ToInt32(LabelSelectedModuleId.Text)
        //                        && a.ModuleObjectiveId == Convert.ToInt32(LabelSelectedModuleObjectiveId.Text));
        //            SetDatainGrid(model, db);
        //        }
        //    }
        //}
        //private void SetDatainGrid(IQueryable<Quiz> quizes, MaterialEntities db )
        //{
        //    //var model = from a in quizes 

        //    //            select new { ActivityId = a.ActivityId, QuizId = a.QuizId, CourseId = a.CourseId, 
        //    //                CourseObjectiveId = a.CourseObjectiveId, ModuleId = a.ModuleId, ModuleObjectiveId = a.ModuleObjectiveId, 
        //    //                Title = a.Activity.Title, Type = a.Activity.Type, MaxGrade = a.Activity.MaxGrade, ModuleObjective = mo.Description, 
        //    //                Module = m.Description, CourseObjective = co.Description, Course = c.Name };

        //    List<Quiz> model = new List<Quiz>();
        //    if (model.Any())
        //    {
        //        GridView1.DataSource = model.OrderBy(x => x.ActivityId);
        //        GridView1.DataBind();
        //    }
        //    else
        //    {
        //        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Sorry! No data found.');", true);
        //    }
        //}

        private void BindHeaderDropDownCourseObjective()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //--------------------Bind Dropdown--------------------------
                DropDownList DropDownListSortCourseObjective = GridView1.HeaderRow.FindControl("DropDownListSortCourseObjective") as DropDownList;
                DropDownListSortCourseObjective.DataSource = db.CourseObjectives.OrderBy(y => y.Id);
                DropDownListSortCourseObjective.DataValueField = "CourseObjectiveId";
                DropDownListSortCourseObjective.DataTextField = "Description";
                DropDownListSortCourseObjective.DataBind();
                DropDownListSortCourseObjective.Items.Insert(0, new ListItem("--Select One--"));
            }
        }
        private void BindHeaderDropDownModule()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //-----------------Grid Sorting Module ----------------
                DropDownList DropDownListSortModule = GridView1.HeaderRow.FindControl("DropDownListSortModule") as DropDownList;
                //[Sohel] Fix this
                //DropDownListSortModule.DataSource = db.Modules.Where(y => y.CourseId == Convert.ToInt32(LabelSelectedCourseId.Text)
                //                            && y.x_CourseObjectiveId == Convert.ToInt32(LabelSelectedCourseObjectiveId.Text)).OrderBy(x => x.x_ModuleId);
                DropDownListSortModule.DataTextField = "Description";
                DropDownListSortModule.DataValueField = "ModuleId";
                DropDownListSortModule.DataBind();
                DropDownListSortModule.Items.Insert(0, new ListItem("--Select One--"));
            }
        }
        private void BindHeaderDropDownModuleObjective()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //-----------------Grid Sorting Module Objective ----------------
                DropDownList DropDownListSortModuleObjective = GridView1.HeaderRow.FindControl("DropDownListSortModuleObjective") as DropDownList;
                //[Sohel] Fix this
                //DropDownListSortModuleObjective.DataSource = db.ModuleObjectives.Where(y => y.x_CourseId == Convert.ToInt32(LabelSelectedCourseId.Text)
                //                                             && y.x_CourseObjectiveId == Convert.ToInt32(LabelSelectedCourseObjectiveId.Text)
                //                                             && y.x_ModuleId == Convert.ToInt32(LabelSelectedModuleId.Text)).OrderBy(x => x.Id);

                DropDownListSortModuleObjective.DataTextField = "Description";
                DropDownListSortModuleObjective.DataValueField = "Id";
                DropDownListSortModuleObjective.DataBind();
                DropDownListSortModuleObjective.Items.Insert(0, new ListItem("--Select One--"));
            }
        }
        #endregion
    }
}