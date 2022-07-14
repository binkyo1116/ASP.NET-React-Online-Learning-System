using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddRequiredTool : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridRequiredTool();
                BindDropDownCourse();
                BindDropDownRequiredTool();
                BindGridCourseRequiredTool();
            }
        }
        private void BindDropDownCourse()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                DropDownListCourse.DataSource = db.Courses.Select(x => new { x.Id, x.Name }).OrderBy(x => x.Id).ToList(); ;
                DropDownListCourse.DataTextField = "Name";
                DropDownListCourse.DataValueField = "Id";
                DropDownListCourse.DataBind();
                DropDownListCourse.Items.Insert(0, new ListItem("--Select One--", ""));
                //----------------------Prerequisite-----------------------------------
            }
        }
        private void BindDropDownRequiredTool()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                System.Collections.Generic.List<RequiredTool> model = db.RequiredTools.OrderBy(x => x.RequiredToolId).ToList();
                DropDownListRequiredTool.DataSource = model;
                DropDownListRequiredTool.DataTextField = "Description";
                DropDownListRequiredTool.DataValueField = "RequiredToolId";
                DropDownListRequiredTool.DataBind();
                DropDownListRequiredTool.Items.Insert(0, new ListItem("--Select One--", ""));
                //----------------------Prerequisite-----------------------------------
            }
        }
        private void BindGridRequiredTool()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //var courseInstance = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                var model = from a in db.RequiredTools
                            select new { a.RequiredToolId, a.Description, a.Active };

                GridView1.DataSource = model.ToList();
                GridView1.DataBind();
            }
        }
        private void BindGridCourseRequiredTool()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                IQueryable<CourseRequiredTool> model = from a in db.CourseRequiredTools select a;
                if (DropDownListCourse.SelectedValue != "")
                {
                    int courseId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                    model = from a in model
                            where a.CourseId == courseId
                            select a;
                }
                var result = (from a in model
                              select new { Course = a.Course.Name, a.CourseId, a.RequiredToolId, a.RequiredTool.Description, a.Active }).ToList();
                GridView2.DataSource = result;
                GridView2.DataBind();
            }
        }

        protected void btnAddRequiredTool_Click(object sender, EventArgs e)
        {
            if (RequiredToolValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    RequiredTool requiredTool = new RequiredTool()
                    {
                        Description = TextBoxDescription.Text.Trim(),
                        Active = CheckBoxActive.Checked
                    };
                    db.RequiredTools.Add(requiredTool);
                    db.SaveChanges();
                    lblMessage.Text = "Save Successfully!";
                    lblErrorMessage.Text = "";
                }
                BindDropDownRequiredTool();
                BindGridRequiredTool();
                CheckBoxActive.Checked = true;
                TextBoxDescription.Text = "";
            }
        }

        protected bool RequiredToolValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(TextBoxDescription.Text))
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
            BindGridRequiredTool();
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            string description = (row.FindControl("TextBox1") as TextBox).Text.Trim();

            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                RequiredTool requiredTool = db.RequiredTools.Where(x => x.RequiredToolId == id).FirstOrDefault();

                requiredTool.Description = description;
                requiredTool.Active = active;

                db.SaveChanges();
            }
            GridView1.EditIndex = -1;
            BindGridRequiredTool();
            BindDropDownRequiredTool();
            lblMessage.Text = "Update Successfully!";
            lblErrorMessage.Text = "";
        }
        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGridRequiredTool();
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);

                using (MaterialEntities db = new MaterialEntities())
                {
                    RequiredTool requiredTool = db.RequiredTools.Where(x => x.RequiredToolId == id).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.RequiredTools.Remove(requiredTool);
                    db.SaveChanges();
                }
                this.BindGridRequiredTool();
                BindDropDownRequiredTool();
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
        }

        //===================================Course Technology Requirement=========================================
        protected void addCourseRequiredTool_Click(object sender, EventArgs e)
        {
            if (CourseRequiredToolValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                    int requiredToolId = Convert.ToInt32(DropDownListRequiredTool.SelectedValue);
                    if (!db.CourseRequiredTools.Where(x => x.CourseId == courseId && x.RequiredToolId == requiredToolId).Any())
                    {
                        CourseRequiredTool courseRequiredTool = new CourseRequiredTool()
                        {
                            CourseId = courseId,
                            RequiredToolId = requiredToolId,
                            Active = CheckBoxCourseRequiredToolActive.Checked
                        };
                        db.CourseRequiredTools.Add(courseRequiredTool);
                        db.SaveChanges();
                        lblMessageCourseRequiredTool.Text = "Save Successfully!";
                        lblErrorMessageCourseRequiredTool.Text = "";
                    }
                    else
                    {
                        lblErrorMessageCourseRequiredTool.Text = "Already Exist!";
                        lblMessageCourseRequiredTool.Text = "";
                    }

                }

                BindGridCourseRequiredTool();
                CheckBoxCourseRequiredToolActive.Checked = true;
                DropDownListRequiredTool.SelectedIndex = -1;
            }
        }
        protected bool CourseRequiredToolValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(DropDownListCourse.SelectedValue))
            {
                fieldName += " Course Instance -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListRequiredTool.SelectedValue))
            {
                fieldName += " Required Tool -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessageCourseRequiredTool.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessageCourseRequiredTool.Text = "";
            }
            return result;
        }

        protected void DropDownListCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCourse.SelectedValue != "")
            {
                BindGridCourseRequiredTool();
            }
        }

        protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView2.EditIndex = -1;
            BindGridCourseRequiredTool();
        }

        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView2.Rows[e.RowIndex];
                int id = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
                int courseId = Convert.ToInt32((row.FindControl("LabelCourseId") as Label).Text);
                using (MaterialEntities db = new MaterialEntities())
                {
                    CourseRequiredTool courseRequiredTool = db.CourseRequiredTools.Where(x => x.RequiredToolId == id && x.CourseId == courseId).FirstOrDefault();

                    //TODO: Make sure this is correct
                    db.CourseRequiredTools.Remove(courseRequiredTool);
                    db.SaveChanges();
                }
                this.BindGridCourseRequiredTool();
                lblMessageCourseRequiredTool.Text = "Deleted Successfully!";
                lblErrorMessageCourseRequiredTool.Text = "";
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    lblErrorMessageCourseRequiredTool.Text = ex.Message;
                }
                else
                {
                    lblErrorMessageCourseRequiredTool.Text = ex.InnerException.InnerException.Message;
                }
                lblMessageCourseRequiredTool.Text = "";
            }
        }

        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView2.EditIndex = e.NewEditIndex;
            BindGridCourseRequiredTool();
        }

        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView2.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
            int courseId = Convert.ToInt32((row.FindControl("LabelCourseId") as Label).Text);
            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                CourseRequiredTool courseRequiredTool = db.CourseRequiredTools.Where(x => x.RequiredToolId == id && x.CourseId == courseId).FirstOrDefault();

                courseRequiredTool.Active = active;
                db.SaveChanges();
            }
            GridView2.EditIndex = -1;
            BindGridCourseRequiredTool();
            lblMessageCourseRequiredTool.Text = "Update Successfully!";
            lblErrorMessageCourseRequiredTool.Text = "";
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView2.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }
        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            clearAll();
            lblMessage.Text = "";
            lblMessageCourseRequiredTool.Text = "";
            lblErrorMessage.Text = "";
            lblErrorMessageCourseRequiredTool.Text = "";
        }
        private void clearAll()
        {
            TextBoxDescription.Text = "";
            CheckBoxActive.Checked = true;
            CheckBoxCourseRequiredToolActive.Checked = true;
            DropDownListCourse.SelectedIndex = -1;
            DropDownListRequiredTool.SelectedIndex = -1;
            BindGridCourseRequiredTool();
        }
    }
}