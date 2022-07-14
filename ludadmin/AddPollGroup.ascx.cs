using EFModel;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddPollGroup : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDownCourseInstance_myPollList();
                BindDropDownCourseInstance();
                BindDropDownPollGroup();
            }
        }

        private void BindGridPollGroupPollQuestion()
        {
            if (DropDownListPollGroup.SelectedValue != "")
            {
                int pollGroupId = Convert.ToInt32(DropDownListPollGroup.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    var model = (from a in db.PollGroupPollQuestions
                                 where a.PollGroupId == pollGroupId
                                 select new { a.PollGroupId, a.PollQuestionId, PollQuestion = a.PollQuestion.Title, a.Active }).ToList();
                    GridView1.DataSource = model;
                    GridView1.DataBind();
                }
            }
        }
        private void clearAll()
        {
            DropDownListPollQuestion.Items.Clear();
            TextBoxDueDate.Text = "";
            CheckBoxCourseInstanceCodingProblemActive.Checked = true;

            TextBoxTitle.Text = "";
            CheckBoxActive.Checked = true;
            DropDownListPollGroup.SelectedIndex = -1;
            //DropDownListCourseObjective.SelectedIndex = -1;
            //DropDownListModule.SelectedIndex = -1;

            LabelPollGroupTitle.Text = "";
            LabelPollGroupId.Text = "";


            AddNewGroup.Attributes.Remove("disabled");
            AddNewGroup.Style["pointer-events"] = "visible";
            lblMessage.Text = "";
            lblErrorMessage.Text = "";
            GridView1.DataSource = null;
            GridView1.DataBind();
            BindGrid();
        }
        protected void ButtonRefresh_Click(object sender, EventArgs e)
        {
            clearAll();
        }

        #region======================================Poll Group=================================
        protected void AddNewGroup_Click(object sender, EventArgs e)
        {
            if (PollGroupValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    PollGroup group = new PollGroup()
                    {
                        Title = TextBoxTitle.Text,
                        Active = CheckBoxActive.Checked
                    };

                    db.PollGroups.Add(group);
                    db.SaveChanges();

                    BindDropDownPollGroup();
                    DropDownListPollGroup.SelectedValue = Convert.ToString(group.Id);
                }
                lblMessage.Text = "Save Successfully";
                lblErrorMessage.Text = "";
                AddNewGroup.Attributes["disabled"] = "disabled";
                AddNewGroup.Style["pointer-events"] = "none";

                BindDropDownPoll();
            }
        }
        protected void btnGroupUpdate_Click(object sender, EventArgs e)
        {
            if (DropDownListPollGroup.SelectedValue != "")
            {
                int groupId = Convert.ToInt32(DropDownListPollGroup.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    //-------------Group table---------------------
                    PollGroup group = db.PollGroups.Where(x => x.Id == groupId).FirstOrDefault();
                    group.Title = TextBoxTitle.Text;
                    group.Active = CheckBoxActive.Checked;
                    db.SaveChanges();
                    BindDropDownPollGroup();
                    DropDownListPollGroup.SelectedValue = Convert.ToString(group.Id);
                }

                lblMessage.Text = "Update Successfully";
                lblErrorMessage.Text = "";
            }
            else
            {
                lblErrorMessage.Text = "Please add an activity first";
                lblMessage.Text = "";
            }
        }
        protected void btnGroupDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (DropDownListPollGroup.SelectedValue != "")
                {
                    int groupId = Convert.ToInt32(DropDownListPollGroup.SelectedValue);
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        IQueryable<PollGroupPollQuestion> pollMaping = db.PollGroupPollQuestions.Where(x => x.PollGroupId == groupId);
                        //TODO: Make sure this is correct
                        if (pollMaping.Any()) {
                            db.PollGroupPollQuestions.RemoveRange(pollMaping);
                        }
                        //-----------------------------------------------------------------------------------
                        PollGroup group = db.PollGroups.Where(x => x.Id == groupId).FirstOrDefault();
                        //TODO: Make sure this is correct
                        db.PollGroups.Remove(group);
                        db.SaveChanges();
                    }
                    clearAll();
                    lblMessage.Text = "Deleted Successfully";
                    lblErrorMessage.Text = "";
                }
                else
                {
                    lblMessage.Text = "Please add an activity first";
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
        protected void DropDownListPollGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListPollGroup.SelectedValue != "")
            {
                int pollGroupId = Convert.ToInt32(DropDownListPollGroup.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    PollGroup model = db.PollGroups.Find(pollGroupId);
                    TextBoxTitle.Text = model.Title;
                    CheckBoxActive.Checked = model.Active;
                    AddNewGroup.Attributes["disabled"] = "disabled";
                    AddNewGroup.Style["pointer-events"] = "none";
                    //---------Load Grid--------------------
                    BindDropDownPoll();
                    BindGridPollGroupPollQuestion();
                }
                lblMessage.Text = "";
                lblErrorMessage.Text = "";
            }
        }

        #endregion=============================================================================

        #region====================================== Add  Poll Question========================
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                int pollGroupId = Convert.ToInt32(DropDownListPollGroup.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    PollGroupPollQuestion pollMaping = db.PollGroupPollQuestions.Where(x => x.PollQuestionId == id && x.PollGroupId == pollGroupId).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.PollGroupPollQuestions.Remove(pollMaping);
                    db.SaveChanges();
                }
                BindGridPollGroupPollQuestion();
                //this.BindMyPollGrid();
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
        protected void ButtonAddPollQuesInGroup_Click(object sender, EventArgs e)
        {
            if (DropDownListPollGroup.SelectedValue != "" && DropDownListPollQuestion.SelectedValue != "")
            {
                int pollGroupId = Convert.ToInt32(DropDownListPollGroup.SelectedValue);
                int pollQuesId = Convert.ToInt32(DropDownListPollQuestion.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    try
                    {
                        bool exist = db.PollGroupPollQuestions.Where(x => x.PollGroupId == pollGroupId && x.PollQuestionId == pollQuesId).Any();
                        if (!exist)
                        {
                            PollGroupPollQuestion pollMaping = new PollGroupPollQuestion()
                            {
                                PollGroupId = pollGroupId,
                                PollQuestionId = pollQuesId,
                                Active = true
                            };
                            db.PollGroupPollQuestions.Add(pollMaping);
                            db.SaveChanges();
                            //--------------------------------------
                            lblMessage.Text = "Save Successfully";
                            lblErrorMessage.Text = "";
                            DropDownListPollQuestion.SelectedIndex = -1;
                            BindGridPollGroupPollQuestion();
                            //BindMyPollGrid();
                        }
                        else
                        {
                            lblErrorMessage.Text = "Sorry! Already Exist!";
                            lblMessage.Text = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        lblErrorMessage.Text = ex.Message;
                        lblMessage.Text = "";
                    }
                }
            }
            else
            {
                lblErrorMessage.Text = "Please Select an Poll Question";
                lblMessage.Text = "";
            }
        }
        #endregion=============================================================================
        private void BindDropDownPoll()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //---------------Poll Droup Down list----------------
                DropDownListPollQuestion.DataSource = db.PollQuestions.Select(a => new { a.Id, a.Title }).OrderBy(x => x.Id).ToList();
                DropDownListPollQuestion.DataTextField = "Title";
                DropDownListPollQuestion.DataValueField = "Id";
                DropDownListPollQuestion.DataBind();
                DropDownListPollQuestion.Items.Insert(0, new ListItem("--Select Poll--", ""));
            }
        }
        //=======================================================================================================
        private void BindDropDownCourseInstance()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //---------------Coruse Droup Down list----------------
                DropDownListCourseInstance.DataSource = db.CourseInstances.Where(x => x.Active).Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourseInstance.DataTextField = "Name";
                DropDownListCourseInstance.DataValueField = "Id";
                DropDownListCourseInstance.DataBind();
                DropDownListCourseInstance.Items.Insert(0, new ListItem("--Select Course Instance--", ""));

                DropDownListCourseInstanceFilter.DataSource = db.CourseInstances.Where(x => x.Active).Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourseInstanceFilter.DataTextField = "Name";
                DropDownListCourseInstanceFilter.DataValueField = "Id";
                DropDownListCourseInstanceFilter.DataBind();
                DropDownListCourseInstanceFilter.Items.Insert(0, new ListItem("--Select Course Instance--", ""));
            }
        }
        private void BindDropDownPollGroup()
        {

            using (MaterialEntities db = new MaterialEntities())
            {
                var model = db.PollGroups.Select(x => new { Id = x.Id, Title = x.Title }).ToList();
                //--------------- Droup Down list----------------
                DropDownListPollGroup.DataSource = model;
                DropDownListPollGroup.DataTextField = "Title";
                DropDownListPollGroup.DataValueField = "Id";
                DropDownListPollGroup.DataBind();
                DropDownListPollGroup.Items.Insert(0, new ListItem("--Select Poll Group--", ""));
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
            filterPollGroup();
            BindDropDownModuleObjectiveFilter();
        }
        protected void DropDownListModuleObjectiveFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterPollGroup();
        }

        private void filterPollGroup()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                if (DropDownListModuleObjectiveFilter.SelectedValue != "" && DropDownListCourseInstanceFilter.SelectedValue != "")
                {
                    int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjectiveFilter.SelectedValue);
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    DropDownListPollGroup.DataSource = db.CourseInstancePollGroups.Where(x => x.ModuleObjectiveId == moduleObjectiveId && x.CourseInstanceId == courseInstanceId).Select(y => y.PollGroup).OrderBy(x => x.Id).ToList();
                }
                else if (DropDownListCourseInstanceFilter.SelectedValue != "")
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    DropDownListPollGroup.DataSource = db.CourseInstancePollGroups.Where(x => x.CourseInstanceId == courseInstanceId).Select(y => y.PollGroup).OrderBy(x => x.Id).ToList();
                }
                else if (DropDownListModuleObjectiveFilter.SelectedValue != "")
                {
                    int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjectiveFilter.SelectedValue);
                    DropDownListPollGroup.DataSource = db.CourseInstancePollGroups.Where(x => x.ModuleObjectiveId == moduleObjectiveId).Select(y => y.PollGroup).OrderBy(x => x.Id).ToList();
                }
                else
                {
                    DropDownListPollGroup.DataSource = db.PollGroups.Select(x => new { Id = x.Id, Title = x.Title }).ToList();
                }

                //--------------- Droup Down list----------------
                DropDownListPollGroup.DataTextField = "Title";
                DropDownListPollGroup.DataValueField = "Id";
                DropDownListPollGroup.DataBind();
                DropDownListPollGroup.Items.Insert(0, new ListItem("--Select Poll Group--", ""));
            }
        }


        #region ===============CourseInstance & Poll Group============================================
        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                IQueryable<CourseInstancePollGroup> model = from a in db.CourseInstancePollGroups select a;
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
                                      a.PollGroupId,
                                      CourseInstance = a.CourseInstance.Course.Name,
                                      ModuleObjective = a.ModuleObjective.Description,
                                      PollGroup = a.PollGroup.Title,
                                      a.Active,
                                      a.DueDate
                                  }).ToList();
                GridView2.DataSource = gridResult;
                GridView2.DataBind();
            }
        }
        protected void ButtonCourseInstancePollGroup_Click(object sender, EventArgs e)
        {
            if (CourseInstancePollGroupValidation())
            {
                int courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
                int PollGroupId = Convert.ToInt32(DropDownListPollGroup.SelectedValue);

                using (MaterialEntities db = new MaterialEntities())
                {
                    bool exist = db.CourseInstancePollGroups.Where(x => x.CourseInstanceId == courseInstanceId
                                                                     && x.ModuleObjectiveId == moduleObjectiveId
                                                                     && x.PollGroupId == PollGroupId).Any();
                    if (!exist)
                    {
                        CourseInstancePollGroup model = new CourseInstancePollGroup()
                        {
                            CourseInstanceId = courseInstanceId,
                            ModuleObjectiveId = moduleObjectiveId,
                            PollGroupId = PollGroupId,
                            DueDate = Convert.ToDateTime(TextBoxDueDate.Text),
                            Active = CheckBoxCourseInstanceCodingProblemActive.Checked
                        };
                        db.CourseInstancePollGroups.Add(model);
                        db.SaveChanges();
                        lblMessage.Text = "Save Successfully!";
                        lblErrorMessage.Text = "";
                        clearAll();
                        BindDropDownPollGroup();
                    }
                    else
                    {
                        lblErrorMessage.Text = "Sorry! Already Exist!";
                        lblMessage.Text = "";

                    }
                }

            }
        }
        protected void OnRowEditingCIPG(object sender, GridViewEditEventArgs e)
        {
            GridView2.EditIndex = e.NewEditIndex;
            BindGrid();
        }
        protected void OnRowCancelingEditCIPG(object sender, GridViewCancelEditEventArgs e)
        {
            GridView2.EditIndex = -1;
            BindGrid();
        }
        protected void OnRowUpdatingCIPG(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = GridView2.Rows[e.RowIndex];
                int courseInstanceid = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
                int moduleObjectiveId = Convert.ToInt32((row.FindControl("LabelModuleObjectiveId") as Label).Text);
                int pollGroupId = Convert.ToInt32((row.FindControl("LabelPollGroupId") as Label).Text);
                bool active = (row.FindControl("CheckBoxActive") as CheckBox).Checked;
                DateTime dueDate = Convert.ToDateTime((row.FindControl("TextBoxDueDate") as TextBox).Text);

                using (MaterialEntities db = new MaterialEntities())
                {
                    CourseInstancePollGroup model = db.CourseInstancePollGroups.Where(x => x.CourseInstanceId == courseInstanceid && x.ModuleObjectiveId == moduleObjectiveId && x.PollGroupId == pollGroupId).FirstOrDefault();
                    model.DueDate = dueDate;
                    model.Active = active;
                    db.SaveChanges();
                }
                GridView2.EditIndex = -1;
                BindGrid();
                lblMessage.Text = "Update Successfully!";
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
        protected void OnRowDeletingCIPG(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView2.Rows[e.RowIndex];
                int courseInstanceid = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
                int moduleObjectiveId = Convert.ToInt32((row.FindControl("LabelModuleObjectiveId") as Label).Text);
                int pollGroupId = Convert.ToInt32((row.FindControl("LabelPollGroupId") as Label).Text);
               
                using (MaterialEntities db = new MaterialEntities())
                {
                    CourseInstancePollGroup model = db.CourseInstancePollGroups.Where(x => x.CourseInstanceId == courseInstanceid && x.ModuleObjectiveId == moduleObjectiveId && x.PollGroupId == pollGroupId).FirstOrDefault();
                    db.CourseInstancePollGroups.Remove(model);
                    db.SaveChanges();
                }
                GridView2.EditIndex = -1;
                this.BindGrid();
                lblMessage.Text = "Deleted Successfully!";
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
        protected void OnRowDataBoundCIPG(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView2.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }
        protected bool CourseInstancePollGroupValidation()
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
            if (string.IsNullOrWhiteSpace(DropDownListPollGroup.SelectedValue))
            {
                fieldName += " Poll Group -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxDueDate.Text))
            {
                fieldName += " Due Date -";
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

        #region ======================Dropdown for My Poll List==============================
        private void BindDropDownCourseInstance_myPollList()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                System.Collections.Generic.List<CourseInstance> model = (from a in db.CourseInstances.Where(x => x.CourseInstancePollGroups.Any())
                                                                         select a).ToList();

                ddnCourseInstance_myPollList.DataSource = model.Select(x => new { x.Course.Name, x.Id }).OrderBy(x => x.Id).ToList();
                ddnCourseInstance_myPollList.DataTextField = "Name";
                ddnCourseInstance_myPollList.DataValueField = "Id";
                ddnCourseInstance_myPollList.DataBind();
                ddnCourseInstance_myPollList.Items.Insert(0, new ListItem("--Select Course Instance--", ""));
            }
            DropDownListPollGroup_myPollList.Items.Clear();
            DropDownListModuleObjective_myPollList.Items.Clear();
            ListViewPollQuestion.DataSource = null;
            ListViewPollQuestion.DataBind();
        }
        private void BindDropDownModuleObjective_myPollList()
        {
            if (ddnCourseInstance_myPollList.SelectedValue != "")
            {
                int ciId = Convert.ToInt32(ddnCourseInstance_myPollList.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    IQueryable<ModuleObjective> model = from a in db.ModuleObjectives.Where(x => x.CourseInstancePollGroups.Where(y => y.CourseInstanceId == ciId).Any()) select a;

                    DropDownListModuleObjective_myPollList.DataSource = model.Select(x => new { x.Id, x.Description }).OrderBy(y => y.Id).ToList();
                    DropDownListModuleObjective_myPollList.DataTextField = "Description";
                    DropDownListModuleObjective_myPollList.DataValueField = "Id";
                    DropDownListModuleObjective_myPollList.DataBind();
                    DropDownListModuleObjective_myPollList.Items.Insert(0, new ListItem("--Select Module Objective--", "0"));
                }
                DropDownListPollGroup_myPollList.Items.Clear();
                ListViewPollQuestion.DataSource = null;
                ListViewPollQuestion.DataBind();
            }
        }
        private void BindDropDownPollGroup_myPollList()
        {
            if (ddnCourseInstance_myPollList.SelectedValue != "" && DropDownListModuleObjective_myPollList.SelectedValue != "")
            {
                int ciId = Convert.ToInt32(ddnCourseInstance_myPollList.SelectedValue);
                int moId = Convert.ToInt32(DropDownListModuleObjective_myPollList.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    var model = db.CourseInstancePollGroups.Where(x => x.CourseInstanceId == ciId && x.ModuleObjectiveId == moId)
                                                                   .Select(y => y.PollGroup).Select(z => new { z.Id, z.Title }).ToList();
                    DropDownListPollGroup_myPollList.DataSource = model;
                    DropDownListPollGroup_myPollList.DataTextField = "Title";
                    DropDownListPollGroup_myPollList.DataValueField = "Id";
                    DropDownListPollGroup_myPollList.DataBind();
                    DropDownListPollGroup_myPollList.Items.Insert(0, new ListItem("--Select Group--", ""));
                }
                ListViewPollQuestion.DataSource = null;
                ListViewPollQuestion.DataBind();
            }
        }
        protected void ddnCourseInstance_myPollList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddnCourseInstance_myPollList.SelectedValue != "")
            {
                BindDropDownModuleObjective_myPollList();
            }
        }
        protected void DropDownListModuleObjective_PollList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(DropDownListModuleObjective_myPollList.SelectedValue) > 0)
            {
                BindDropDownPollGroup_myPollList();
            }
        }
        protected void DropDownListPollGroup_myPollList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindListView();
        }
        protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            DropDownListPollGroup_myPollList.Items.Clear();
            ListViewPollQuestion.DataSource = null;
            ListViewPollQuestion.DataBind();
        }
        protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //---------------Poll Droup Down list 2----------------
                DropDownListPollGroup_myPollList.DataSource = db.CourseInstancePollGroups.Select(x => x.PollGroup).Select(y => new { y.Id, y.Title }).OrderBy(x => x.Id).ToList();
                DropDownListPollGroup_myPollList.DataTextField = "Title";
                DropDownListPollGroup_myPollList.DataValueField = "Id";
                DropDownListPollGroup_myPollList.DataBind();
                DropDownListPollGroup_myPollList.Items.Insert(0, new ListItem("--Select Group--", ""));
            }
            ListViewPollQuestion.DataSource = null;
            ListViewPollQuestion.DataBind();
        }
        #endregion =====================================================================

        #region ===============================List View===============================================
        private void BindListView()
        {
            if (DropDownListPollGroup_myPollList.SelectedValue != "")
            {
                int ciId = Convert.ToInt32(ddnCourseInstance_myPollList.SelectedValue);
                int moId = Convert.ToInt32(DropDownListModuleObjective_myPollList.SelectedValue);
                int groupId = Convert.ToInt32(DropDownListPollGroup_myPollList.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {

                    IQueryable<ResponseListItem> model = (from a in db.PollGroupPollQuestions
                                                          where a.PollGroupId == groupId
                                                          select new ResponseListItem
                                                          {
                                                              PollQuestionId = a.PollQuestionId,
                                                              PollGroupId = a.PollGroupId,
                                                              Title = a.PollQuestion.Title,
                                                              PollType = a.PollQuestion.PollQuestionType.TypeTitle,
                                                              Response = a.PollQuestion.PollParticipantAnswers.Where(x => x.PollGroupId == groupId && x.ModuleObjectiveId == moId && x.CourseInstanceId == ciId).Count(),
                                                              ActivePoll = a.Active
                                                          });

                    ListViewPollQuestion.DataSource = model.OrderBy(x => x.PollQuestionId).ToList();
                    ListViewPollQuestion.DataBind();
                }
            }
        }
        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem lvItem = (ListViewDataItem)e.Item;
                ResponseListItem rlistItem = (ResponseListItem)lvItem.DataItem;

                if (rlistItem != null)
                {
                    GridView gv1 = (GridView)e.Item.FindControl("GridViewParticipantResponse");
                    if (gv1 != null)
                    {
                        using (MaterialEntities db = new MaterialEntities())
                        {
                            int ciId = Convert.ToInt32(ddnCourseInstance_myPollList.SelectedValue);
                            int moId = Convert.ToInt32(DropDownListModuleObjective_myPollList.SelectedValue);

                            PollGroupPollQuestion poll = db.PollGroupPollQuestions.Where(x => x.PollGroupId == rlistItem.PollGroupId && x.PollQuestionId == rlistItem.PollQuestionId).FirstOrDefault();
                            if (poll.PollQuestion.PollQuestionType.PollOption)
                            {
                                gv1.DataSource = (from a in db.PollParticipantAnswers
                                                  join b in db.PollQuestionOptions on a.PollOptionId equals b.PollOptionId
                                                  where a.PollGroupId == poll.PollGroupId && a.PollQuestionId == poll.PollQuestionId &&
                                                  a.ModuleObjectiveId == moId && a.CourseInstanceId == ciId
                                                  select new { a.PollAnswerId, a.StudentId, a.PollQuestionId, StudentName = a.Student.Name, Response = b.Title, a.EnlistedDate }).ToList();
                            }
                            else
                            {
                                gv1.DataSource = (from a in db.PollParticipantAnswers
                                                  where a.PollGroupId == poll.PollGroupId && a.PollQuestionId == poll.PollQuestionId &&
                                                     a.ModuleObjectiveId == moId && a.CourseInstanceId == ciId
                                                  select new { a.PollAnswerId, a.StudentId, a.PollQuestionId, StudentName = a.Student.Name, Response = a.TextAnswer, a.EnlistedDate }).ToList();
                            }
                            gv1.DataBind();
                        }

                    }

                }
            }
        }
        private void ItemDeleting_PollQuestion(int pollQid)
        {

            //int pollQid = Convert.ToInt32(ListViewPollQuestion.DataKeys[e.ItemIndex].Value);
            //Delete record based on Id
            int ciId = Convert.ToInt32(ddnCourseInstance_myPollList.SelectedValue);
            int moId = Convert.ToInt32(DropDownListModuleObjective_myPollList.SelectedValue);
            int pollGroupId = Convert.ToInt32(DropDownListPollGroup_myPollList.SelectedValue);
            using (MaterialEntities db = new MaterialEntities())
            {
                //-----------------------Delete Answer-----------------

                IQueryable<PollParticipantAnswer> pollAns = db.PollParticipantAnswers.Where(x => x.PollQuestionId == pollQid && x.PollGroupId == pollGroupId && x.ModuleObjectiveId == moId && x.CourseInstanceId == ciId);
                //TODO: Make sure this is correct
                //db.PollParticipantAnswers.RemoveRange(pollAns);
                //db.SaveChanges();
                //-----------------------Delete PollQuestion-----------------
                PollGroupPollQuestion pollMaping = db.PollGroupPollQuestions.Where(x => x.PollQuestionId == pollQid && x.PollGroupId == pollGroupId).FirstOrDefault();
                //TODO: Make sure this is correct
                //db.PollGroupPollQuestions.Remove(pollMaping);
                //db.SaveChanges();
            }
            BindListView();
        }
        private void ItemDeleting_PollResponse(int pollQid)
        {

            //int pollQid = Convert.ToInt32(ListViewPollQuestion.DataKeys[e.ItemIndex].Value);
            //Delete record based on Id
            int ciId = Convert.ToInt32(ddnCourseInstance_myPollList.SelectedValue);
            int moId = Convert.ToInt32(DropDownListModuleObjective_myPollList.SelectedValue);
            int pollGroupId = Convert.ToInt32(DropDownListPollGroup_myPollList.SelectedValue);
            using (MaterialEntities db = new MaterialEntities())
            {
                //-----------------------Delete Answer-----------------
                IQueryable<PollParticipantAnswer> pollAns = db.PollParticipantAnswers.Where(x => x.PollQuestionId == pollQid && x.PollGroupId == pollGroupId && x.ModuleObjectiveId == moId && x.CourseInstanceId == ciId);
                //TODO: Make sure this is correct
                //db.PollParticipantAnswers.RemoveRange(pollAns);
                //db.SaveChanges();
            }
            BindListView();
        }
        protected void Delete_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            int PollId = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case ("ResponseDelete"):
                    ItemDeleting_PollResponse(PollId);
                    break;
                case ("PollDelete"):
                    ItemDeleting_PollQuestion(PollId);
                    break;
            }
        }
        protected void ButtonDeleteAllResponseOfTheGroup_Click(object sender, EventArgs e)
        {
            if (DropDownListPollGroup_myPollList.SelectedValue != "")
            {
                int ciId = Convert.ToInt32(ddnCourseInstance_myPollList.SelectedValue);
                int moId = Convert.ToInt32(DropDownListModuleObjective_myPollList.SelectedValue);
                int groupId = Convert.ToInt32(DropDownListPollGroup_myPollList.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    IQueryable<PollParticipantAnswer> answers = db.PollParticipantAnswers.Where(x => x.PollGroupId == groupId && x.ModuleObjectiveId == moId && x.CourseInstanceId == ciId);
                    //TODO: Make sure this is correct
                    //db.PollParticipantAnswers.RemoveRange(answers);
                    db.SaveChanges();
                }
                BindListView();
            }
        }
        protected void ButtonRefreshMyPollList_Click(object sender, EventArgs e)
        {
            BindListView();
        }
        #endregion ====================================================================================

        #region--------------------Validation---------------------------
        protected bool PollGroupValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(TextBoxTitle.Text))
            {
                fieldName += " Title -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessage.Text = "";
            }
            return result;
        }
        #endregion=============================================================================

    }

    internal class ResponseListItem
    {
        public int PollQuestionId { get; set; }
        public int PollGroupId { get; set; }
        public string Title { get; set; }
        public string PollType { get; set; }
        public int Response { get; set; }
        public bool ActivePoll { get; set; }

    }
}