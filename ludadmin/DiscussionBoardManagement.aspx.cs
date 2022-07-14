using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class DiscussionBoardManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDownCourseInstance();
                BindDropDownDiscussionBoard();
            }
        }

        #region---------------------Dropdown List---------------------------
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
                DropDownListCourseInstanceFilter.Items.Insert(0, new ListItem("--Select Course Instance--", ""));
            }
        }
        private void BindDropDownDiscussionBoard()
        {

            using (MaterialEntities db = new MaterialEntities())
            {
                var model = db.DiscussionBoards.Select(x => new { Id = x.Id, Title = x.Title }).ToList();
                //--------------- Droup Down list----------------
                DropDownListDiscussionBoard.DataSource = model;
                DropDownListDiscussionBoard.DataTextField = "Title";
                DropDownListDiscussionBoard.DataValueField = "Id";
                DropDownListDiscussionBoard.DataBind();
                DropDownListDiscussionBoard.Items.Insert(0, new ListItem("--Select Discussion Board--", ""));
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
        protected void DropDownListCourseInstanceFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterDiscussion();
            BindDropDownModuleObjectiveFilter();
        }
        protected void DropDownListModuleObjectiveFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterDiscussion();
        }

        private void filterDiscussion()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                if (DropDownListModuleObjectiveFilter.SelectedValue != "" && DropDownListCourseInstanceFilter.SelectedValue != "")
                {
                    int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjectiveFilter.SelectedValue);
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    DropDownListDiscussionBoard.DataSource = db.CourseInstanceDiscussionBoards.Where(x => x.ModuleObjectiveId == moduleObjectiveId && x.CourseInstanceId == courseInstanceId).Select(y => y.DiscussionBoard).OrderBy(x => x.Id).ToList();
                }
                else if (DropDownListCourseInstanceFilter.SelectedValue != "")
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    DropDownListDiscussionBoard.DataSource = db.CourseInstanceDiscussionBoards.Where(x => x.CourseInstanceId == courseInstanceId).Select(y => y.DiscussionBoard).OrderBy(x => x.Id).ToList();
                }
                else if (DropDownListModuleObjectiveFilter.SelectedValue != "")
                {
                    int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjectiveFilter.SelectedValue);
                    DropDownListDiscussionBoard.DataSource = db.CourseInstanceDiscussionBoards.Where(x => x.ModuleObjectiveId == moduleObjectiveId).Select(y => y.DiscussionBoard).OrderBy(x => x.Id).ToList();
                }
                else
                {
                    DropDownListDiscussionBoard.DataSource = db.DiscussionBoards.Select(x => new { Id = x.Id, Title = x.Title }).ToList();
                }

                //--------------- Droup Down list----------------
                DropDownListDiscussionBoard.DataTextField = "Title";
                DropDownListDiscussionBoard.DataValueField = "Id";
                DropDownListDiscussionBoard.DataBind();
                DropDownListDiscussionBoard.Items.Insert(0, new ListItem("--Select Discussion Board--", ""));
            }
        }

        protected void DropDownListDiscussionBoard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListDiscussionBoard.SelectedValue != "")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int discussionBoardId = Convert.ToInt32(DropDownListDiscussionBoard.SelectedValue);
                    DiscussionBoard model = db.DiscussionBoards.Find(discussionBoardId);
                    TextBoxTitle.Text = model.Title;
                    CheckBoxActive.Checked = model.Active;

                    AddNewDiscussionBoard.Attributes["disabled"] = "disabled";
                    AddNewDiscussionBoard.Style["pointer-events"] = "none";
                    //---------Load Grid--------------------
                    BindGridGroupDiscussion();
                    PanelAddGroupDiscussion.Visible = true;
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
        #endregion----------------------------------------------------------
        private void clearText()
        {
            TextBoxGroupDiscussionTitle.Text = "";
            TextBoxGroupDiscussionDescription.Text = "";
        }
        private void clearAll()
        {
            TextBoxDueDate.Text = "";

            TextBoxTitle.Text = "";
            DropDownListDiscussionBoard.SelectedIndex = -1;
            CheckBoxActive.Checked = true;
            CheckBoxActiveDiscussion.Checked = true;
            CheckBoxCourseInstanceDBActive.Checked = true;

            AddNewDiscussionBoard.Attributes.Remove("disabled");
            AddNewDiscussionBoard.Style["pointer-events"] = "visible";

            clearText();

            GridView1.DataSource = null;
            GridView1.DataBind();
            PanelAddGroupDiscussion.Visible = false;
            BindGrid();
        }
        protected void ButtonRefresh_Click(object sender, EventArgs e)
        {
            clearAll();
            lblMessage.Text = "";
            lblErrorMessage.Text = "";
        }

        #region ===============CourseInstance & Discussion Board============================================
        protected void ButtonCourseInstanceDiscussionBoard_Click(object sender, EventArgs e)
        {
            if (CourseInstanceDsicussionBoardValidation())
            {
                int courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
                int discussiBoardId = Convert.ToInt32(DropDownListDiscussionBoard.SelectedValue);

                using (MaterialEntities db = new MaterialEntities())
                {
                    bool exist = db.CourseInstanceDiscussionBoards.Where(x => x.CourseInstanceId == courseInstanceId
                                                                     && x.ModuleObjectiveId == moduleObjectiveId
                                                                     && x.DiscussionBoardId == discussiBoardId).Any();
                    if (!exist)
                    {
                        CourseInstanceDiscussionBoard model = new CourseInstanceDiscussionBoard()
                        {
                            CourseInstanceId = courseInstanceId,
                            ModuleObjectiveId = moduleObjectiveId,
                            DiscussionBoardId = discussiBoardId,
                            DueDate = Convert.ToDateTime(TextBoxDueDate.Text),
                            Active = CheckBoxCourseInstanceDBActive.Checked
                        };
                        db.CourseInstanceDiscussionBoards.Add(model);
                        db.SaveChanges();
                        lblMessage.Text = "Save Successfully!";
                        lblErrorMessage.Text = "";
                        clearAll();
                        BindDropDownDiscussionBoard();
                    }
                    else
                    {
                        lblErrorMessage.Text = "Sorry! Already Exist!";
                        lblMessage.Text = "";
                    }
                }

            }
        }
        protected bool CourseInstanceDsicussionBoardValidation()
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
            if (string.IsNullOrWhiteSpace(DropDownListDiscussionBoard.SelectedValue))
            {
                fieldName += " DiscussionBoard -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxDueDate.Text))
            {
                fieldName += " Due Date -";
                result = false;
            }
            if (!result)
            {
                lblMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblErrorMessage.Text = "";
            }
            return result;
        }
        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                IQueryable<CourseInstanceDiscussionBoard> model = from a in db.CourseInstanceDiscussionBoards select a;
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
                                      a.DiscussionBoardId,
                                      CourseInstance = a.CourseInstance.Course.Name,
                                      ModuleObjective = a.ModuleObjective.Description,
                                      DiscussionBoard = a.DiscussionBoard.Title,
                                      a.Active,
                                      a.DueDate
                                  }).ToList();
                GridView2.DataSource = gridResult;
                GridView2.DataBind();
            }
        }
        protected void OnRowCancelingEditCID(object sender, GridViewCancelEditEventArgs e)
        {
            GridView2.EditIndex = -1;
            BindGrid();
        }
        protected void OnRowDataBoundCID(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView2.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }
        protected void OnRowDeletingCID(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView2.Rows[e.RowIndex];
                int courseInstanceid = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
                int moduleObjectiveId = Convert.ToInt32((row.FindControl("LabelModuleObjectiveId") as Label).Text);
                int discussionBoardId = Convert.ToInt32((row.FindControl("LabelDiscussionBoarId") as Label).Text);

                using (MaterialEntities db = new MaterialEntities())
                {
                    CourseInstanceDiscussionBoard model = db.CourseInstanceDiscussionBoards.Where(x => x.CourseInstanceId == courseInstanceid && x.ModuleObjectiveId == moduleObjectiveId && x.DiscussionBoardId == discussionBoardId).FirstOrDefault();
                    db.CourseInstanceDiscussionBoards.Remove(model);
                    db.SaveChanges();
                }
                GridView2.EditIndex = -1;
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
                    lblErrorMessage.Text = ex.InnerException.Message;
                }
                lblMessage.Text = "";
            }
           
        }
        protected void OnRowEditingCID(object sender, GridViewEditEventArgs e)
        {
            GridView2.EditIndex = e.NewEditIndex;
            BindGrid();
        }
        protected void OnRowUpdatingCID(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView2.Rows[e.RowIndex];
            int courseInstanceid = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
            int moduleObjectiveId = Convert.ToInt32((row.FindControl("LabelModuleObjectiveId") as Label).Text);
            int discussionBoardId = Convert.ToInt32((row.FindControl("LabelDiscussionBoarId") as Label).Text);
            bool active = (row.FindControl("CheckBoxActive") as CheckBox).Checked;
            DateTime dueDate = Convert.ToDateTime((row.FindControl("TextBoxDueDate") as TextBox).Text);

            using (MaterialEntities db = new MaterialEntities())
            {
                CourseInstanceDiscussionBoard model = db.CourseInstanceDiscussionBoards.Where(x => x.CourseInstanceId == courseInstanceid && x.ModuleObjectiveId == moduleObjectiveId && x.DiscussionBoardId == discussionBoardId).FirstOrDefault();
                model.DueDate = dueDate;
                model.Active = active;
                db.SaveChanges();
            }
            GridView2.EditIndex = -1;
            BindGrid();
            lblMessage.Text = "Update Successfully!";
            lblErrorMessage.Text = "";
        }
        #endregion =====================================================================================
        #region ===============Discussion Board=========================================================
        protected void AddNewDiscussionBoard_Click(object sender, EventArgs e)
        {
            if (TextBoxTitle.Text != "")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    DiscussionBoard board = new DiscussionBoard()
                    {
                        Title = TextBoxTitle.Text,
                        Active = CheckBoxActive.Checked
                    };

                    db.DiscussionBoards.Add(board);
                    db.SaveChanges();
                    BindDropDownDiscussionBoard();
                    DropDownListDiscussionBoard.SelectedValue = Convert.ToString(board.Id);
                }
                lblMessage.Text = "Save Successfully";
                lblErrorMessage.Text = "";
                AddNewDiscussionBoard.Attributes["disabled"] = "disabled";
                AddNewDiscussionBoard.Style["pointer-events"] = "none";
                PanelAddGroupDiscussion.Visible = true;
            }
            else
            {
                lblErrorMessage.Text = "Title is required!";
                lblMessage.Text = "";
            }
        }
        protected void btnDiscussionBoardUpdate_Click(object sender, EventArgs e)
        {
            if (DropDownListDiscussionBoard.SelectedValue != "")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int discussionBoardId = Convert.ToInt32(DropDownListDiscussionBoard.SelectedValue);
                    DiscussionBoard board = db.DiscussionBoards.Find(discussionBoardId);
                    board.Title = TextBoxTitle.Text.Trim();
                    board.Active = CheckBoxActive.Checked;
                    db.SaveChanges();
                    BindDropDownDiscussionBoard();
                    DropDownListDiscussionBoard.SelectedValue = Convert.ToString(board.Id);
                }
                lblMessage.Text = "Update Successfully";
                lblErrorMessage.Text = "";
            }
            else
            {
                lblErrorMessage.Text = "Please add an Discussion Board first";
                lblMessage.Text = "";
            }
        }
        protected void btnDiscussionBoardDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (DropDownListDiscussionBoard.SelectedValue != "")
                {
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        var discussionBoardId = Convert.ToInt32(DropDownListDiscussionBoard.SelectedValue);
                        DiscussionBoard board = db.DiscussionBoards.Find(discussionBoardId);
                        db.DiscussionBoards.Remove(board);
                        db.SaveChanges();
                        clearAll();

                    }
                    lblMessage.Text = "Deleted Successfully";
                    lblErrorMessage.Text = "";
                    BindDropDownDiscussionBoard();
                }
                else
                {
                    lblErrorMessage.Text = "Please add an Discussion Board first";
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
                    lblErrorMessage.Text = ex.InnerException.Message;
                }
                lblMessage.Text = "";
            }
         
        }
        #endregion =====================================================================================
        #region =============== Group Discussion=========================================================
        protected void btnAddDiscussion_Click(object sender, EventArgs e)
        {
            if (DsicussionValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int boardId = Convert.ToInt32(DropDownListDiscussionBoard.SelectedValue);
                    int studentid = ((Student)Session["Student"]).StudentId;

                    GroupDiscussion discussion = new GroupDiscussion
                    {
                        DiscussionBoardId = boardId,
                        Title = TextBoxGroupDiscussionTitle.Text.Trim(),
                        Description = TextBoxGroupDiscussionDescription.Text.Trim(),
                        StudentId = studentid,
                        PublishedDate = DateTime.Now,
                        LastUpdateDate = DateTime.Now,
                        Active = CheckBoxActiveDiscussion.Checked,
                        CourseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue)
                    };

                    db.GroupDiscussions.Add(discussion);
                    db.SaveChanges();

                    clearText();
                    BindGridGroupDiscussion();
                }
            }

        }
        protected bool DsicussionValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(DropDownListCourseInstance.SelectedValue))
            {
                fieldName += " Course Instance -";
                result = false;
            }

            if (string.IsNullOrWhiteSpace(TextBoxGroupDiscussionDescription.Text))
            {
                fieldName += " Description -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessage.Text = "";
            }
            return result;
        }
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGridGroupDiscussion();
        }
        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGridGroupDiscussion();
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);

            string title = (row.FindControl("TextBox2") as TextBox).Text;
            string description = (row.FindControl("TextBox1") as TextBox).Text;
            bool active = (row.FindControl("CheckBox2") as CheckBox).Checked;
            using (MaterialEntities db = new MaterialEntities())
            {
                GroupDiscussion discussion = db.GroupDiscussions.Find(id);
                discussion.Title = title;
                discussion.Description = description;
                discussion.Active = active;
                db.SaveChanges();
            }
            lblMessage.Text = "Update Successfully";
            lblErrorMessage.Text = "";
            GridView1.EditIndex = -1;
            BindGridGroupDiscussion();
        }
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                using (MaterialEntities db = new MaterialEntities())
                {
                    GroupDiscussion discussion = db.GroupDiscussions.Find(id);
                    //TODO: Make sure this is correct
                    db.GroupDiscussions.Remove(discussion);
                    db.SaveChanges();
                }
                lblMessage.Text = "Deleted Successfully";
                lblErrorMessage.Text = "";
                BindGridGroupDiscussion();
            }
            catch (Exception ex)
            {

                if (ex.InnerException == null)
                {
                    lblErrorMessage.Text = ex.Message;
                }
                else
                {
                    lblErrorMessage.Text = ex.InnerException.Message;
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
        private void BindGridGroupDiscussion()
        {
            if (DropDownListDiscussionBoard.SelectedValue != "")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int boardId = Convert.ToInt32(DropDownListDiscussionBoard.SelectedValue);
                    IQueryable<GroupDiscussion> model = db.GroupDiscussions.Where(x => x.DiscussionBoardId == boardId);
                    GridView1.DataSource = (from a in model
                                            select new { CourseInstance = a.CourseInstance.Course.Name, a.DiscussionBoardId, a.Id, a.Title, a.Description, PublishedBy = a.Student.Name, a.PublishedDate, a.Active }).ToList();
                    GridView1.DataBind();
                }
            }
        }
        #endregion =====================================================================================

    }
}