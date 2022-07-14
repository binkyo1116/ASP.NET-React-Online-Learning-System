using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddTechnologyRequirement : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridTechnology();
                BindDropDownCourse();
                BindDropDownTechnologyRequirement();
                BindGridCourseTechnology();
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
        private void BindDropDownTechnologyRequirement()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                System.Collections.Generic.List<TechnologyRequirement> model = db.TechnologyRequirements.OrderBy(x => x.Id).ToList();
                DropDownListTechnologyRequirement.DataSource = model;
                DropDownListTechnologyRequirement.DataTextField = "Description";
                DropDownListTechnologyRequirement.DataValueField = "Id";
                DropDownListTechnologyRequirement.DataBind();
                DropDownListTechnologyRequirement.Items.Insert(0, new ListItem("--Select One--", ""));
                //----------------------Prerequisite-----------------------------------
            }
        }

        private void BindGridTechnology()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //var courseInstance = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                var model = from a in db.TechnologyRequirements
                            select new { a.Id, a.Description, a.Active };

                GridView1.DataSource = model.ToList();
                GridView1.DataBind();
            }
        }
        private void BindGridCourseTechnology()
        {

            using (MaterialEntities db = new MaterialEntities())
            {
                IQueryable<CourseTechnologyRequirement> model = from a in db.CourseTechnologyRequirements select a;

                if (DropDownListCourse.SelectedValue != "")
                {
                    int courseId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                    model = from a in model
                            where a.CourseId == courseId
                            select a;
                }
                var result = from a in model
                             select new { a.CourseId, Course = a.Course.Name, a.TechnologyRequirementId, a.TechnologyRequirement.Description, a.Active };

                GridView2.DataSource = result.ToList();
                GridView2.DataBind();
            }
        }
        protected void btnAddTechnologyRequirement_Click(object sender, EventArgs e)
        {
            if (TechnologyValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    TechnologyRequirement technologyRequirement = new TechnologyRequirement()
                    {
                        Description = TextBoxDescription.Text.Trim(),
                        Active = CheckBoxActive.Checked
                    };
                    db.TechnologyRequirements.Add(technologyRequirement);
                    db.SaveChanges();
                    lblMessage.Text = "Save Successfully!";
                    lblErrorMessage.Text = "";
                }
                BindDropDownTechnologyRequirement();
                BindGridTechnology();
                CheckBoxActive.Checked = true;
                TextBoxDescription.Text = "";
            }
        }
        protected bool TechnologyValidation()
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
            BindGridTechnology();
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            string description = (row.FindControl("TextBox1") as TextBox).Text.Trim();

            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                TechnologyRequirement technologyRequirement = db.TechnologyRequirements.Where(x => x.Id == id).FirstOrDefault();

                technologyRequirement.Description = description;
                technologyRequirement.Active = active;

                db.SaveChanges();
            }
            GridView1.EditIndex = -1;
            BindGridTechnology();
            BindDropDownTechnologyRequirement();
            lblMessage.Text = "Update Successfully!";
            lblErrorMessage.Text = "";
        }
        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGridTechnology();
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                using (MaterialEntities db = new MaterialEntities())
                {
                    TechnologyRequirement technologyRequirement = db.TechnologyRequirements.Where(x => x.Id == id).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.TechnologyRequirements.Remove(technologyRequirement);
                    db.SaveChanges();
                }
                this.BindGridTechnology();
                BindDropDownTechnologyRequirement();
                lblMessage.Text = "Delete Successfully!";
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
        protected void addCourseTechnologyRequirement_Click(object sender, EventArgs e)
        {
            if (CourseTechnologyValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                    int technologyRequirementId = Convert.ToInt32(DropDownListTechnologyRequirement.SelectedValue);
                    if (!db.CourseTechnologyRequirements.Where(x => x.CourseId == courseId && x.TechnologyRequirementId == technologyRequirementId).Any())
                    {
                        CourseTechnologyRequirement courseTechnology = new CourseTechnologyRequirement()
                        {
                            CourseId = courseId,
                            TechnologyRequirementId = technologyRequirementId,
                            Active = CheckBoxCourseTechnologyActive.Checked
                        };
                        db.CourseTechnologyRequirements.Add(courseTechnology);
                        db.SaveChanges();
                        lblMessageCourseTechnology.Text = "Save Successfully!";
                        lblErrorMessageCourseTechnology.Text = "";
                    }
                    else
                    {
                        lblErrorMessageCourseTechnology.Text = "Already Exist!";
                        lblMessageCourseTechnology.Text = "";
                    }

                }

                BindGridCourseTechnology();
                CheckBoxCourseTechnologyActive.Checked = true;
                DropDownListTechnologyRequirement.SelectedIndex = -1;
            }
        }
        protected bool CourseTechnologyValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(DropDownListCourse.SelectedValue))
            {
                fieldName += " Course -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListTechnologyRequirement.SelectedValue))
            {
                fieldName += " Technology -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessageCourseTechnology.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessageCourseTechnology.Text = "";
            }
            return result;
        }

        protected void DropDownListCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCourse.SelectedValue != "")
            {
                BindGridCourseTechnology();
            }
        }

        protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView2.EditIndex = -1;
            BindGridCourseTechnology();
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
                    CourseTechnologyRequirement courseTechnologyRequirement = db.CourseTechnologyRequirements.Where(x => x.TechnologyRequirementId == id && x.CourseId == courseId).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.CourseTechnologyRequirements.Remove(courseTechnologyRequirement);
                    db.SaveChanges();
                }
                this.BindGridCourseTechnology();
                lblMessageCourseTechnology.Text = "Delete Successfully!";
                lblErrorMessageCourseTechnology.Text = "";
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    lblErrorMessageCourseTechnology.Text = ex.Message;
                }
                else
                {
                    lblErrorMessageCourseTechnology.Text = ex.InnerException.InnerException.Message;
                }
                lblMessageCourseTechnology.Text = "";
            }

        }

        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView2.EditIndex = e.NewEditIndex;
            BindGridCourseTechnology();
        }

        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView2.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
            int courseId = Convert.ToInt32((row.FindControl("LabelCourseId") as Label).Text);
            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                CourseTechnologyRequirement courseTechnologyRequirement = db.CourseTechnologyRequirements.Where(x => x.TechnologyRequirementId == id && x.CourseId == courseId).FirstOrDefault();
                courseTechnologyRequirement.Active = active;
                db.SaveChanges();
            }
            GridView2.EditIndex = -1;
            BindGridCourseTechnology();
            lblMessageCourseTechnology.Text = "Update Successfully!";
            lblErrorMessageCourseTechnology.Text = "";
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
            lblErrorMessage.Text = "";
            lblMessageCourseTechnology.Text = "";
            lblErrorMessageCourseTechnology.Text = "";

        }
        private void clearAll()
        {
            TextBoxDescription.Text = "";

            CheckBoxActive.Checked = true;
            CheckBoxCourseTechnologyActive.Checked = true;

            DropDownListCourse.SelectedIndex = -1;
            DropDownListTechnologyRequirement.SelectedIndex = -1;
            BindGridCourseTechnology();
        }
    }
}