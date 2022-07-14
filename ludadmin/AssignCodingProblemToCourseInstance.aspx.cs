using EFModel;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AssignCodingProblemToCourseInstance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindDropDownCourse();
                BindDropDownCourse();
                BindDropDownCodingProblem();
                BindGrid();
            }
        }
        private void BindDropDownCourse()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //---------------Coruse Droup Down list----------------
                DropDownListCourseFilter1.DataSource = db.Courses.Select(x => new { x.Id, x.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourseFilter1.DataTextField = "Name";
                DropDownListCourseFilter1.DataValueField = "Id";
                DropDownListCourseFilter1.DataBind();
                DropDownListCourseFilter1.Items.Insert(0, new ListItem("--Select Course--", ""));

                DropDownListCourseFilter2.DataSource = db.Courses.Select(x => new { x.Id, x.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourseFilter2.DataTextField = "Name";
                DropDownListCourseFilter2.DataValueField = "Id";
                DropDownListCourseFilter2.DataBind();
                DropDownListCourseFilter2.Items.Insert(0, new ListItem("--Select Course--", ""));
            }
        }
        private void BindDropDownCourseInstance()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //---------------Coruse Instance Droup Down list----------------
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
        private void BindDropDownCodingProblem()
        {

            using (MaterialEntities db = new MaterialEntities())
            {
                var model = db.CodingProblems.Select(x => new { Id = x.Id, Title = x.Title }).ToList();
                //--------------- Droup Down list----------------
                DropDownListCodingProblem.DataSource = model;
                DropDownListCodingProblem.DataTextField = "Title";
                DropDownListCodingProblem.DataValueField = "Id";
                DropDownListCodingProblem.DataBind();
                DropDownListCodingProblem.Items.Insert(0, new ListItem("--Select Coding Problem--", ""));
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
                    //List<DropDownListItem> model = new List<DropDownListItem>();
                    //model = (from a in db.ModuleObjectives
                    //         where a.Active
                    //         select new DropDownListItem { Id = a.Id, Title = a.Description }).OrderBy(y => y.Id).ToList();
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
                    //List<DropDownListItem> model = new List<DropDownListItem>();
                    //model = (from a in db.ModuleObjectives
                    //         where a.Active
                    //         select new DropDownListItem { Id = a.Id, Title = a.Description }).OrderBy(y => y.Id).ToList();
                    //--------------------Bind Dropdown--------------------------
                    DropDownListModuleObjectiveFilter.DataSource = modelObjectives;
                    DropDownListModuleObjectiveFilter.DataTextField = "Title";
                    DropDownListModuleObjectiveFilter.DataValueField = "Id";
                    DropDownListModuleObjectiveFilter.DataBind();
                    DropDownListModuleObjectiveFilter.Items.Insert(0, new ListItem("--Select Module--", ""));
                }
            }
        }
        protected void btnCodingProblemListPage_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "OpenWindow", "window.open('QuizQuestionList.aspx','_newtab');", true);
        }
        private void clearAll()
        {
            DropDownListCourseFilter1.SelectedIndex = -1;
            DropDownListCourseFilter2.SelectedIndex = -1;
            DropDownListQuarter.Items.Clear();
            DropDownListQuarterFilter2.Items.Clear();
            DropDownListCourseInstance.Items.Clear();
            DropDownListCourseInstanceFilter.Items.Clear();

            CheckBoxCourseInstanceCodingProblemActive.Checked = true;

            DropDownListCodingProblem.SelectedIndex = -1;
            DropDownListModuleObjective.SelectedIndex = -1;
            lblErrorMessage.Text = "";
            lblMessage.Text = "";
            BindDropDownCodingProblem();
            BindGrid();
        }
        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            clearAll();
        }
        protected void btnAssessmentListPage_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "OpenWindow", "window.open('CodingProblemList.aspx','_newtab');", true);
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
            filterCodingProblem();
            BindDropDownModuleObjectiveFilter();
        }
        protected void DropDownListModuleObjectiveFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterCodingProblem();
        }
        private void filterCodingProblem()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                if (DropDownListModuleObjectiveFilter.SelectedValue != "" && DropDownListCourseInstanceFilter.SelectedValue != "")
                {
                    int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjectiveFilter.SelectedValue);
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    DropDownListCodingProblem.DataSource = db.CourseInstanceCodingProblems.Where(x => x.ModuleObjectiveId == moduleObjectiveId && x.CourseInstanceId == courseInstanceId && x.Active).Select(y => y.CodingProblem).OrderBy(x => x.Id).ToList();
                }
                else if (DropDownListCourseInstanceFilter.SelectedValue != "")
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    DropDownListCodingProblem.DataSource = db.CourseInstanceCodingProblems.Where(x => x.CourseInstanceId == courseInstanceId && x.Active).Select(y => y.CodingProblem).OrderBy(x => x.Id).ToList();
                }
                else if (DropDownListModuleObjectiveFilter.SelectedValue != "")
                {
                    int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjectiveFilter.SelectedValue);
                    DropDownListCodingProblem.DataSource = db.CourseInstanceCodingProblems.Where(x => x.ModuleObjectiveId == moduleObjectiveId && x.Active).Select(y => y.CodingProblem).OrderBy(x => x.Id).ToList();
                }
                else
                {
                    DropDownListCodingProblem.DataSource = db.CodingProblems.Where(x => x.Active).Select(x => new { Id = x.Id, Title = x.Title }).ToList();
                }

                //--------------- Droup Down list----------------
                DropDownListCodingProblem.DataTextField = "Title";
                DropDownListCodingProblem.DataValueField = "Id";
                DropDownListCodingProblem.DataBind();
                DropDownListCodingProblem.Items.Insert(0, new ListItem("--Select Coding Problem--", ""));
            }
        }
        #region ===============CourseInstanceCodingProblem============================================
        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                IQueryable<CourseInstanceCodingProblem> model = from a in db.CourseInstanceCodingProblems select a;
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
                                      a.CodingProblemId,
                                      CourseInstance = a.CourseInstance.Course.Name,
                                      ModuleObjective = a.ModuleObjective.Description,
                                      CodingProblem = a.CodingProblem.Title,
                                      a.Active,
                                      a.MaxGrade,
                                      a.DueDate
                                  }).ToList();
                GridView1.DataSource = gridResult;
                GridView1.DataBind();
            }
        }
        protected void ButtonCourseInstanceCodingProblem_Click(object sender, EventArgs e)
        {
            if (CourseInstanceCodingProblemValidation())
            {
                int courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
                int codingProblemId = Convert.ToInt32(DropDownListCodingProblem.SelectedValue);

                using (MaterialEntities db = new MaterialEntities())
                {
                    bool exist = db.CourseInstanceCodingProblems.Where(x => x.CourseInstanceId == courseInstanceId
                                                                     && x.ModuleObjectiveId == moduleObjectiveId
                                                                     && x.CodingProblemId == codingProblemId).Any();
                    if (!exist)
                    {
                       TimeSpan time = new TimeSpan(23, 59, 59);
                        var dueDate = Convert.ToDateTime(TextBoxDueDate.Text).Add(time);
                       CourseInstanceCodingProblem model = new CourseInstanceCodingProblem()
                        {
                            CourseInstanceId = courseInstanceId,
                            ModuleObjectiveId = moduleObjectiveId,
                            CodingProblemId = codingProblemId,
                            DueDate = dueDate,
                            Active = CheckBoxCourseInstanceCodingProblemActive.Checked,
                            MaxGrade = Convert.ToInt32(TextBoxCourseInstanceCPMaxGrade.Text.Trim()),
                        };
                        db.CourseInstanceCodingProblems.Add(model);
                        db.SaveChanges();
                        lblMessage.Text = "Save Successfully!";
                        lblErrorMessage.Text = "";
                        DropDownListCodingProblem.SelectedIndex = -1;
                        BindGrid();
                        //BindDropDownCodingProblem();
                    }
                    else
                    {
                        lblErrorMessage.Text = "Sorry! Already Exist!";
                        lblMessage.Text = "";
                    }
                }

            }
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
            int courseInstanceid = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            int moduleObjectiveId = Convert.ToInt32((row.FindControl("LabelModuleObjectiveId") as Label).Text);
            int codingProblemId = Convert.ToInt32((row.FindControl("LabelCodingProblemId") as Label).Text);
            bool active = (row.FindControl("CheckBoxActive") as CheckBox).Checked;
            int maxGrade = Convert.ToInt32((row.FindControl("TextBoxMaxGrade") as TextBox).Text);
            TimeSpan time = new TimeSpan(23, 59, 59);
            DateTime dueDate = Convert.ToDateTime((row.FindControl("TextBoxDueDate") as TextBox).Text).Add(time);

            using (MaterialEntities db = new MaterialEntities())
            {
                CourseInstanceCodingProblem model = db.CourseInstanceCodingProblems.Where(x => x.CourseInstanceId == courseInstanceid && x.ModuleObjectiveId == moduleObjectiveId && x.CodingProblemId == codingProblemId).FirstOrDefault();
                model.DueDate = dueDate;
                model.MaxGrade = maxGrade;
                model.Active = active;
                db.SaveChanges();
            }
            GridView1.EditIndex = -1;
            BindGrid();
            lblMessage.Text = "Updated Successfully!";
            lblErrorMessage.Text = "";
        }
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int courseInstanceid = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                int moduleObjectiveId = Convert.ToInt32((row.FindControl("LabelModuleObjectiveId") as Label).Text);
                int codingProblemId = Convert.ToInt32((row.FindControl("LabelCodingProblemId") as Label).Text);
                using (MaterialEntities db = new MaterialEntities())
                {
                    CourseInstanceCodingProblem model = db.CourseInstanceCodingProblems.Where(x => x.CourseInstanceId == courseInstanceid && x.ModuleObjectiveId == moduleObjectiveId && x.CodingProblemId == codingProblemId).FirstOrDefault();
                    db.CourseInstanceCodingProblems.Remove(model);
                    db.SaveChanges();
                }
                GridView1.EditIndex = -1;
                this.BindGrid();
                lblMessage.Text = "Deleted Successfully!";
                lblErrorMessage.Text = "";
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
            ////check if the row is the header row
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    //add the thead and tbody section programatically
            //    e.Row.TableSection = TableRowSection.TableHeader;
            //}
        }
        protected bool CourseInstanceCodingProblemValidation()
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
            if (string.IsNullOrWhiteSpace(DropDownListCodingProblem.SelectedValue))
            {
                fieldName += " Coding Problem -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxDueDate.Text))
            {
                fieldName += " Due Date -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxCourseInstanceCPMaxGrade.Text))
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
        #endregion =====================================================================================
        protected void DropDownListCourseFilter1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string i = DropDownListCourseFilter1.SelectedValue;
            DropDownListCourseInstance.Items.Clear();
            if (i != "")
            {

                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseId = Convert.ToInt32(i);
                    IQueryable<CourseInstance> courseIns = db.CourseInstances.Where(ci => ci.Course.Id == courseId && ci.Active);

                    var list = courseIns.Select(x => new { Name = x.Quarter.StartDate + " TO " + x.Quarter.EndDate, Id = x.Id }).OrderBy(x => x.Id).ToList();
                    //---------------Coruse Instance Droup Down list----------------
                    DropDownListQuarter.DataSource = list;
                    DropDownListQuarter.DataTextField = "Name";
                    DropDownListQuarter.DataValueField = "Id";
                    DropDownListQuarter.DataBind();
                    //--------------------------------------------------------------------------------
                    if (courseIns.Count() == 1)
                    {
                        BindDropDownListsCourseInstance(courseIns.FirstOrDefault().Id);
                        //----------------------------------------------------------
                        BindGrid();
                        BindDropDownModuleObjective();
                    }
                    else
                    {
                        DropDownListQuarter.Items.Insert(0, new ListItem("--Select One--", ""));
                        DropDownListCourseInstance.Items.Clear();
                        DropDownListModuleObjective.Items.Clear();
                        BindGrid();
                    }
                }
            }
        }
        protected void DropDownListQuarter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string i = DropDownListQuarter.SelectedValue;
            if (i != "")
            {
                int courseInstanceId = Convert.ToInt32(i);
                BindDropDownListsCourseInstance(courseInstanceId);
                //------------------------------------------------
                BindGrid();
                BindDropDownModuleObjective();
            }
        }
        protected void BindDropDownListsCourseInstance(int courseInstanceId)
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var list = db.CourseInstances.Where(ci => ci.Id == courseInstanceId && ci.Active).Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
                //---------------Coruse Instance Droup Down list----------------
                DropDownListCourseInstance.DataSource = list;
                DropDownListCourseInstance.DataTextField = "Name";
                DropDownListCourseInstance.DataValueField = "Id";
                DropDownListCourseInstance.DataBind();
            }
        }
        protected void DropDownListCourseFilter2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string i = DropDownListCourseFilter2.SelectedValue;
            DropDownListCourseInstanceFilter.Items.Clear();
            if (i != "")
            {

                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseId = Convert.ToInt32(i);
                    IQueryable<CourseInstance> courseIns = db.CourseInstances.Where(ci => ci.Course.Id == courseId && ci.Active);

                    var list = courseIns.Select(x => new { Name = x.Quarter.StartDate + " TO " + x.Quarter.EndDate, Id = x.Id }).OrderBy(x => x.Id).ToList();
                    //---------------Coruse Instance Droup Down list----------------
                    DropDownListQuarterFilter2.DataSource = list;
                    DropDownListQuarterFilter2.DataTextField = "Name";
                    DropDownListQuarterFilter2.DataValueField = "Id";
                    DropDownListQuarterFilter2.DataBind();
                    //--------------------------------------------------------------------------------
                    if (courseIns.Count() == 1)
                    {
                        BindDropDownListsCourseInstanceFilter(courseIns.FirstOrDefault().Id);
                        //--------------------------------------------
                        filterCodingProblem();
                        BindDropDownModuleObjectiveFilter();
                    }
                    else
                    {
                        DropDownListQuarterFilter2.Items.Insert(0, new ListItem("--Select One--", ""));
                    }
                }
            }
        }
        protected void DropDownListQuarterFilter2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string i = DropDownListQuarterFilter2.SelectedValue;
            if (i != "")
            {
                int courseInstanceId = Convert.ToInt32(i);
                BindDropDownListsCourseInstanceFilter(courseInstanceId);
                //---------------------------------------
                filterCodingProblem();
                BindDropDownModuleObjectiveFilter();
            }
        }
        protected void BindDropDownListsCourseInstanceFilter(int courseInstanceId)
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var list = db.CourseInstances.Where(ci => ci.Id == courseInstanceId && ci.Active).Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
                //---------------Coruse Instance Droup Down list----------------
                DropDownListCourseInstanceFilter.DataSource = list;
                DropDownListCourseInstanceFilter.DataTextField = "Name";
                DropDownListCourseInstanceFilter.DataValueField = "Id";
                DropDownListCourseInstanceFilter.DataBind();
            }
        }
    }
}