using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddQuizQuestion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDownCourseInstance();
                BindDropDownActivity();
                //BindDropDownModuleObjective();
            }
        }
        private void BindDropDownCourseInstance()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //---------------Coruse Droup Down list----------------
                DropDownListCourseInstance.DataSource = db.CourseInstances.Where(ci => ci.Active).Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourseInstance.DataTextField = "Name";
                DropDownListCourseInstance.DataValueField = "Id";
                DropDownListCourseInstance.DataBind();
                DropDownListCourseInstance.Items.Insert(0, new ListItem("--Select Course Instance--", ""));

                DropDownListCourseInstanceFilter.DataSource = db.CourseInstances.Where(ci => ci.Active).Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourseInstanceFilter.DataTextField = "Name";
                DropDownListCourseInstanceFilter.DataValueField = "Id";
                DropDownListCourseInstanceFilter.DataBind();
                DropDownListCourseInstanceFilter.Items.Insert(0, new ListItem("--Select Course Instance Filter--", ""));
            }
        }
        private void BindDropDownActivity()
        {

            using (MaterialEntities db = new MaterialEntities())
            {
                var model = db.Activities.Select(x => new { Id = x.Id, Title = x.Title }).ToList();
                //--------------- Droup Down list----------------
                DropDownListActivity.DataSource = model;
                DropDownListActivity.DataTextField = "Title";
                DropDownListActivity.DataValueField = "Id";
                DropDownListActivity.DataBind();
                DropDownListActivity.Items.Insert(0, new ListItem("--Select Activity--", ""));
            }
        }
        private void BindDropDownModuleObjective()
        {
            if (DropDownListCourseInstance.SelectedValue != "")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                    int courseId = db.CourseInstances.Find(courseInstanceId).CourseId;
                    var modelObjectives = (from a in db.ModuleObjectives.Where(x => x.Active && x.Modules.Where(m => m.Active && m.CourseObjectives.Where(co => co.Active && co.Courses.Where(c => c.Id == courseId).Any()).Any()).Any())
                                           select new { Id = a.Id, Title = a.Description }).OrderBy(y => y.Id).ToList();

                    //--------------------Bind Dropdown--------------------------
                    DropDownListModuleObjective.DataSource = modelObjectives;
                    DropDownListModuleObjective.DataTextField = "Title";
                    DropDownListModuleObjective.DataValueField = "Id";
                    DropDownListModuleObjective.DataBind();
                    DropDownListModuleObjective.Items.Insert(0, new ListItem("--Select Module--", ""));
                }
            }
        }
        private void BindDropDownModuleObjectiveFilter()
        {
            if (DropDownListCourseInstanceFilter.SelectedValue != "")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    int courseId = db.CourseInstances.Find(courseInstanceId).CourseId;
                    var modelObjectives = (from a in db.ModuleObjectives.Where(x => x.Active && x.Modules.Where(m => m.Active && m.CourseObjectives.Where(co => co.Active && co.Courses.Where(c => c.Id == courseId).Any()).Any()).Any())
                                           select new { Id = a.Id, Title = a.Description }).OrderBy(y => y.Id).ToList();

                    //--------------------Bind Dropdown--------------------------
                    DropDownListModuleObjectiveFilter.DataSource = modelObjectives;
                    DropDownListModuleObjectiveFilter.DataTextField = "Title";
                    DropDownListModuleObjectiveFilter.DataValueField = "Id";
                    DropDownListModuleObjectiveFilter.DataBind();
                    DropDownListModuleObjectiveFilter.Items.Insert(0, new ListItem("--Select Module--", ""));
                }
            }
        }

        private void BindGridQuizQuestion()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                int activityId = Convert.ToInt32(DropDownListActivity.SelectedValue);
                var model = from a in db.QuizQuestions.Where(x => x.ActivityId1 == activityId)
                            select new
                            {
                                a.Id,
                                a.Prompt1,
                                a.Prompt2,
                                a.Answer,
                                a.Source,
                                a.MaxGrade,
                                a.CaseSensitive,
                                a.PositionX,
                                a.PositionY,
                                a.Height,
                                a.Width,
                                a.Type,
                                a.VideoTimestamp,
                                a.VideoSource,
                                a.EmbedAction
                            };

                GridView1.DataSource = model.OrderBy(x => x.Id).ToList();
                GridView1.DataBind();
            }
        }
        protected void DropDownListActivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListActivity.SelectedValue != "")
            {
                int activityId = Convert.ToInt32(DropDownListActivity.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    Activity model = db.Activities.Find(activityId);
                    TextBoxTitle.Text = model.Title;
                    TextBoxType.Text = model.Type;
                    TextBoxActivityMaxGrade.Text = Convert.ToString(model.MaxGrade);
                    CheckBoxActive.Checked = model.Active;
                    TextBoxRole.Text = Convert.ToString(model.Role);
                    AddNewActivity.Attributes["disabled"] = "disabled";
                    AddNewActivity.Style["pointer-events"] = "none";
                    //---------Load Grid--------------------
                    BindGridQuizQuestion();
                }
            }
        }
        protected void DropDownListCourseInstance_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
            BindDropDownModuleObjective();
        }
        protected void DropDownListModuleObjective_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }
        protected void DropDownListCourseInstanceFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterActivity();
            BindDropDownModuleObjectiveFilter();
        }
        protected void DropDownListModuleObjectiveFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterActivity();
        }

        private void filterActivity()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                if (DropDownListModuleObjectiveFilter.SelectedValue != "" && DropDownListCourseInstanceFilter.SelectedValue != "")
                {
                    int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjectiveFilter.SelectedValue);
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    DropDownListActivity.DataSource = db.CourseInstanceActivities.Where(x => x.ModuleObjectiveId == moduleObjectiveId && x.CourseInstanceId == courseInstanceId).Select(y => y.Activity).OrderBy(x => x.Id).ToList();
                }
                else if (DropDownListCourseInstanceFilter.SelectedValue != "")
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    DropDownListActivity.DataSource = db.CourseInstanceActivities.Where(x => x.CourseInstanceId == courseInstanceId).Select(y => y.Activity).OrderBy(x => x.Id).ToList();
                }
                else if (DropDownListModuleObjectiveFilter.SelectedValue != "")
                {
                    int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjectiveFilter.SelectedValue);
                    DropDownListActivity.DataSource = db.CourseInstanceActivities.Where(x => x.ModuleObjectiveId == moduleObjectiveId).Select(y => y.Activity).OrderBy(x => x.Id).ToList();
                }
                else
                {
                    DropDownListActivity.DataSource = db.Activities.Select(x => new { Id = x.Id, Title = x.Title }).ToList();
                }

                //--------------- Droup Down list----------------
                DropDownListActivity.DataTextField = "Title";
                DropDownListActivity.DataValueField = "Id";
                DropDownListActivity.DataBind();
                DropDownListActivity.Items.Insert(0, new ListItem("--Select Activity--", ""));
            }
        }
        private void crearText()
        {
            TextBoxPrompt1.Text = "";
            TextBoxPrompt2.Text = "";
            TextBoxAnswer.Text = "";
            TextBoxSource.Text = "";
            TextBoxMaxGrade.Text = "";
            CheckBoxCaseSensitive.Checked = true;
            TextBoxPositionX.Text = "";
            TextBoxPositionY.Text = "";
            TextBoxHeight.Text = "";
            TextBoxWidth.Text = "";
            TextBoxType.Text = "";
            TextBoxVideoTimestamp.Text = "";
            TextBoxVideoSource.Text = "";
            CheckBoxEmbedAction.Checked = true;
        }
        private void clearAll()
        {
            TextBoxCourseInstanceActivityMaxGrade.Text = "";
            TextBoxDueDate.Text = "";
            CheckBoxCourseInstanceCodingProblemActive.Checked = true;

            TextBoxTitle.Text = "";
            TextBoxType.Text = "";
            DropDownListActivity.SelectedIndex = -1;
            CheckBoxActive.Checked = true;
            TextBoxRole.Text = "";

            TextBoxActivityMaxGrade.Text = "";

            TextBoxPrompt1.Text = "";
            TextBoxPrompt2.Text = "";
            TextBoxAnswer.Text = "";
            TextBoxSource.Text = "";
            TextBoxMaxGrade.Text = "";
            CheckBoxCaseSensitive.Checked = true;
            TextBoxPositionX.Text = "";
            TextBoxPositionY.Text = "";
            TextBoxHeight.Text = "";
            TextBoxWidth.Text = "";
            TextBoxType.Text = "";
            TextBoxVideoTimestamp.Text = "";
            TextBoxVideoSource.Text = "";
            CheckBoxEmbedAction.Checked = true;

            AddNewActivity.Attributes.Remove("disabled");
            AddNewActivity.Style["pointer-events"] = "visible";

            GridView1.DataSource = null;
            GridView1.DataBind();
            BindGrid();
           
        }
        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            clearAll();
            lblMessage.Text = "";
            lblErrorMessage.Text = "";
            BindDropDownCourseInstance();
            DropDownListModuleObjective.Items.Clear();
            DropDownListModuleObjectiveFilter.Items.Clear();
            BindDropDownActivity();
            GridView2.DataSource = null;
            GridView2.DataBind();
        }

        #region =========================================Quiz Question=============================
        protected void AddNewQuiz_Click(object sender, EventArgs e)
        {
            if (DropDownListActivity.SelectedValue != "")
            {
                if (QuizQuestionValidation())
                {
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        QuizQuestion quizQuestion = new QuizQuestion()
                        {
                            ActivityId1 = Convert.ToInt32(DropDownListActivity.SelectedValue),
                            Prompt1 = TextBoxPrompt1.Text.Trim(),
                            Prompt2 = TextBoxPrompt2.Text.Trim(),
                            Answer = TextBoxAnswer.Text.Trim(),
                            Source = TextBoxSource.Text.Trim(),
                            MaxGrade = Convert.ToInt32(TextBoxMaxGrade.Text.Trim()),
                            CaseSensitive = CheckBoxCaseSensitive.Checked,
                            PositionX = Convert.ToInt32(TextBoxPositionX.Text.Trim()),
                            PositionY = Convert.ToInt32(TextBoxPositionY.Text.Trim()),
                            Height = Convert.ToInt32(TextBoxHeight.Text.Trim()),
                            Width = Convert.ToInt32(TextBoxWidth.Text.Trim()),
                            Type = TextBoxType.Text.Trim(),
                            VideoTimestamp = Convert.ToInt32(TextBoxVideoTimestamp.Text.Trim()),
                            VideoSource = TextBoxVideoSource.Text.Trim(),
                            EmbedAction = CheckBoxEmbedAction.Checked
                        };
                        db.QuizQuestions.Add(quizQuestion);
                        db.SaveChanges();
                    }
                    lblMessage.Text = "Save Successfully";
                    lblErrorMessage.Text = "";
                    crearText();
                    BindGridQuizQuestion();
                }
            }
            else
            {
                lblErrorMessage.Text = "Please Add Acticity and Quiz first";
                lblMessage.Text = "";
            }

        }
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGridQuizQuestion();
        }
        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGridQuizQuestion();
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);

            string prompt1 = (row.FindControl("TextBox7") as TextBox).Text.Trim();
            string prompt2 = (row.FindControl("TextBox6") as TextBox).Text.Trim();
            string answer = (row.FindControl("TextBox5") as TextBox).Text.Trim();
            string source = (row.FindControl("TextBox4") as TextBox).Text.Trim();
            int maxGrade = Convert.ToInt32((row.FindControl("TextBox3") as TextBox).Text.Trim());
            bool caseSensitive = (row.FindControl("CheckBoxCaseSensitive") as CheckBox).Checked;

            int positionX = Convert.ToInt32((row.FindControl("TextBoxPositionX") as TextBox).Text.Trim());
            int positionY = Convert.ToInt32((row.FindControl("TextBoxPositionY") as TextBox).Text.Trim());
            int height = Convert.ToInt32((row.FindControl("TextBoxHeight") as TextBox).Text.Trim());
            int width = Convert.ToInt32((row.FindControl("TextBoxTextBoxWidth") as TextBox).Text.Trim());
            string type = (row.FindControl("TextBoxTextBoxType") as TextBox).Text.Trim();
            int vidioTimestamp = Convert.ToInt32((row.FindControl("TextBoxVideoTimestamp") as TextBox).Text.Trim());
            string vidioSource = (row.FindControl("TextBoxVideoSource") as TextBox).Text.Trim();
            bool embedAction = (row.FindControl("CheckBoxEmbedAction") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                QuizQuestion quizQuestion = db.QuizQuestions.Find(id);
                quizQuestion.Prompt1 = prompt1;
                quizQuestion.Prompt2 = prompt2;
                quizQuestion.Answer = answer;
                quizQuestion.Source = source;
                quizQuestion.MaxGrade = maxGrade;
                quizQuestion.CaseSensitive = caseSensitive;
                quizQuestion.PositionX = positionX;
                quizQuestion.PositionY = positionY;
                quizQuestion.Height = height;
                quizQuestion.Width = width;
                quizQuestion.Type = type;
                quizQuestion.VideoTimestamp = vidioTimestamp;
                quizQuestion.VideoSource = vidioSource;
                quizQuestion.EmbedAction = embedAction;

                db.SaveChanges();
            }
            lblMessage.Text = "Update Successfully";
            lblErrorMessage.Text = "";
            GridView1.EditIndex = -1;
            BindGridQuizQuestion();
        }
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                using (MaterialEntities db = new MaterialEntities())
                {
                    QuizQuestion quizQuestion = db.QuizQuestions.Find(id);
                    ////TODO: Make sure this is correct
                    db.QuizQuestions.Remove(quizQuestion);
                    db.SaveChanges();
                }
                lblMessage.Text = "Delete Successfully";
                lblErrorMessage.Text = "";
                BindGridQuizQuestion();
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    lblErrorMessage.Text = ex.Message;
                }
                else
                {
                    lblErrorMessage.Text = ex.InnerException.InnerException.Message;
                }
                lblMessage.Text = "";
            }
        
        }
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }

        #endregion ========================================================================================

        #region =========================================Course Instance & Activity=============================
        protected void ButtonCourseInstanceActivity_Click(object sender, EventArgs e)
        {
            if (CourseInstanceActivityValidation())
            {
                int courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
                int activityId = Convert.ToInt32(DropDownListActivity.SelectedValue);

                using (MaterialEntities db = new MaterialEntities())
                {
                    bool exist = db.CourseInstanceActivities.Where(x => x.CourseInstanceId == courseInstanceId
                                                                     && x.ModuleObjectiveId == moduleObjectiveId
                                                                     && x.ActivityId == activityId).Any();
                    if (!exist)
                    {
                        CourseInstanceActivity model = new CourseInstanceActivity()
                        {
                            CourseInstanceId = courseInstanceId,
                            ModuleObjectiveId = moduleObjectiveId,
                            ActivityId = activityId,
                            DueDate = Convert.ToDateTime(TextBoxDueDate.Text),
                            Active = CheckBoxCourseInstanceCodingProblemActive.Checked,
                            MaxGrade = Convert.ToInt32(TextBoxCourseInstanceActivityMaxGrade.Text.Trim()),
                        };
                        db.CourseInstanceActivities.Add(model);
                        db.SaveChanges();
                        lblMessage.Text = "Save Successfully!";
                        lblErrorMessage.Text = "";
                        clearAll();
                        BindDropDownActivity();
                    }
                    else
                    {
                        lblErrorMessage.Text = "Sorry! Already Exist!";
                        lblMessage.Text = "";

                    }
                }

            }
        }
        protected bool CourseInstanceActivityValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(DropDownListCourseInstance.SelectedValue))
            {
                fieldName += " Course Instance -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListModuleObjective.SelectedValue))
            {
                fieldName += " Module Objective -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListActivity.SelectedValue))
            {
                fieldName += " Activity -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxDueDate.Text))
            {
                fieldName += " Due Date -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxCourseInstanceActivityMaxGrade.Text))
            {
                fieldName += " Max Grade -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessage.Text = "";
            }
            return result;
        }
        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                IQueryable<CourseInstanceActivity> model = from a in db.CourseInstanceActivities select a;
                if (DropDownListCourseInstance.SelectedValue != "")
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                    model = from a in model
                            where a.CourseInstanceId == courseInstanceId
                            select a;
                }
                if (DropDownListModuleObjective.SelectedValue != "")
                {
                    int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
                    model = from a in model
                            where a.ModuleObjectiveId == moduleObjectiveId
                            select a;
                }
                var gridResult = (from a in model
                                  select new
                                  {
                                      a.CourseInstanceId,
                                      a.ModuleObjectiveId,
                                      a.ActivityId,
                                      CourseInstance = a.CourseInstance.Course.Name,
                                      ModuleObjective = a.ModuleObjective.Description,
                                      Activity = a.Activity.Title,
                                      a.Active,
                                      a.MaxGrade,
                                      a.DueDate
                                  }).ToList();
                GridView2.DataSource = gridResult;
                GridView2.DataBind();
            }
        }
        protected void OnRowCancelingEditCIA(object sender, GridViewCancelEditEventArgs e)
        {
            GridView2.EditIndex = -1;
            BindGrid();
        }

        protected void OnRowDataBoundCIA(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView2.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }

        protected void OnRowDeletingCIA(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView2.Rows[e.RowIndex];
                int courseInstanceid = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
                int moduleObjectiveId = Convert.ToInt32((row.FindControl("LabelModuleObjectiveId") as Label).Text);
                int activityId = Convert.ToInt32((row.FindControl("LabelActivityId") as Label).Text);
                if (courseInstanceid != 0 && moduleObjectiveId != 0 && activityId != 0)
                {
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        CourseInstanceActivity model = db.CourseInstanceActivities.Where(x => x.CourseInstanceId == courseInstanceid && x.ModuleObjectiveId == moduleObjectiveId && x.ActivityId == activityId).FirstOrDefault();
                        db.CourseInstanceActivities.Remove(model);
                        db.SaveChanges();
                    }
                    GridView2.EditIndex = -1;
                    this.BindGrid();
                    lblMessage.Text = "Delete Successfully!";
                    lblErrorMessage.Text = "";
                }
                else {
                    lblErrorMessage.Text = "Sorry! Selection process is not correct.";
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    lblErrorMessage.Text = ex.Message;
                }
                else
                {
                    lblErrorMessage.Text = ex.InnerException.InnerException.Message;
                }
                lblMessage.Text = "";
            }
         
        }

        protected void OnRowEditingCIA(object sender, GridViewEditEventArgs e)
        {
            GridView2.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void OnRowUpdatingCIA(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView2.Rows[e.RowIndex];
            int courseInstanceid = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
            int moduleObjectiveId = Convert.ToInt32((row.FindControl("LabelModuleObjectiveId") as Label).Text);
            int activityId = Convert.ToInt32((row.FindControl("LabelActivityId") as Label).Text);
            bool active = (row.FindControl("CheckBoxActive") as CheckBox).Checked;
            int maxGrade = Convert.ToInt32((row.FindControl("TextBoxMaxGrade") as TextBox).Text);
            DateTime dueDate = Convert.ToDateTime((row.FindControl("TextBoxDueDate") as TextBox).Text);

            using (MaterialEntities db = new MaterialEntities())
            {
                CourseInstanceActivity model = db.CourseInstanceActivities.Where(x => x.CourseInstanceId == courseInstanceid && x.ModuleObjectiveId == moduleObjectiveId && x.ActivityId == activityId).FirstOrDefault();
                model.DueDate = dueDate;
                model.MaxGrade = maxGrade;
                model.Active = active;
                db.SaveChanges();
            }
            GridView2.EditIndex = -1;
            BindGrid();
            lblMessage.Text = "Update Successfully!";
            lblErrorMessage.Text = "";
        }
        #endregion ========================================================================================

        #region =========================================Activity==========================================
        protected void AddNewActivity_Click(object sender, EventArgs e)
        {

            if (ActivityValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    Activity activity = new Activity()
                    {
                        Title = TextBoxTitle.Text,
                        Type = TextBoxType.Text,
                        MaxGrade = Convert.ToInt32(TextBoxActivityMaxGrade.Text),
                        Active = CheckBoxActive.Checked,
                        Role = Convert.ToInt32(TextBoxRole.Text)
                    };
                    db.Activities.Add(activity);
                    db.SaveChanges();
                    BindDropDownActivity();
                    DropDownListActivity.SelectedValue = Convert.ToString(activity.Id);
                }
                lblMessage.Text = "Save Successfully";
                lblErrorMessage.Text = "";
                AddNewActivity.Attributes["disabled"] = "disabled";
                AddNewActivity.Style["pointer-events"] = "none";

            }

        }
        protected void btnActivityUpdate_Click(object sender, EventArgs e)
        {
            if (DropDownListActivity.SelectedValue != "")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    Activity activity = db.Activities.Find(Convert.ToInt32(DropDownListActivity.SelectedValue));
                    activity.Title = TextBoxTitle.Text;
                    activity.Type = TextBoxType.Text;
                    activity.MaxGrade = Convert.ToInt32(TextBoxActivityMaxGrade.Text);
                    activity.Active = CheckBoxActive.Checked;
                    activity.Role = Convert.ToInt32(TextBoxRole.Text);
                    db.SaveChanges();
                    BindDropDownActivity();
                    DropDownListActivity.SelectedValue = Convert.ToString(activity.Id);
                }
                lblMessage.Text = "Update Successfully";
                lblErrorMessage.Text = "";
            }
            else
            {
                lblErrorMessage.Text = "Please add an Activity first";
                lblMessage.Text = "";
            }
        }
        protected void btnActivityDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (DropDownListActivity.SelectedValue != "")
                {
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        Activity activity = db.Activities.Find(Convert.ToInt32(DropDownListActivity.SelectedValue));
                        db.Activities.Remove(activity);
                        db.SaveChanges();
                        clearAll();
                        BindDropDownActivity();
                        lblMessage.Text = "Deleted Successfully!";
                        lblErrorMessage.Text = "";
                    }
                }
                else
                {
                    lblErrorMessage.Text = "Please add an Activity first";
                    lblMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    lblErrorMessage.Text = ex.Message;
                }
                else
                {
                    lblErrorMessage.Text = ex.InnerException.InnerException.Message;
                }
                lblMessage.Text = "";
            }
        }
        #endregion ========================================================================================

        #region--------------------Validation---------------------------
        protected bool ActivityValidation()
        {
            bool result = true;
            string fieldName = "";

            if (string.IsNullOrWhiteSpace(TextBoxTitle.Text))
            {
                fieldName += " Title -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxType.Text))
            {
                fieldName += " Type -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxActivityMaxGrade.Text))
            {
                fieldName += " Max Grade -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxRole.Text))
            {
                fieldName += " Role -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessage.Text = "";
            }
            return result;
        }
        protected bool QuizQuestionValidation()
        {
            bool result = true;
            string fieldName = "";

            if (string.IsNullOrWhiteSpace(TextBoxPrompt1.Text))
            {
                fieldName += " Prompt1 -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxPrompt2.Text))
            {
                fieldName += " Prompt2 -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxSource.Text))
            {
                fieldName += " Source -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxAnswer.Text))
            {
                fieldName += " Answer -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxMaxGrade.Text))
            {
                fieldName += " Max Grade -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessage.Text = "";
            }
            return result;
        }


        #endregion
    }
}