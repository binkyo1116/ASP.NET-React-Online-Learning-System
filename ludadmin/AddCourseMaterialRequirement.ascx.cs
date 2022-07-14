using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddCourseMaterialRequirement : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridCourseMaterialRequirement();
                BindDropDownCourse();
                BindDropDownCourseMaterialRequirement();
                BindGridCourseMaterialRequirementMapping();
            }
        }
        private void BindDropDownCourse()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                DropDownListCourse.DataSource = db.Courses.Select(x => new { x.Id, x.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourse.DataTextField = "Name";
                DropDownListCourse.DataValueField = "Id";
                DropDownListCourse.DataBind();
                DropDownListCourse.Items.Insert(0, new ListItem("--Select One--", ""));
                //----------------------Prerequisite-----------------------------------
            }
        }
        private void BindDropDownCourseMaterialRequirement()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                System.Collections.Generic.List<CourseMaterialRequirement> model = db.CourseMaterialRequirements.OrderBy(x => x.Id).ToList();
                DropDownListCourseMaterialRequirement.DataSource = model;
                DropDownListCourseMaterialRequirement.DataTextField = "Description";
                DropDownListCourseMaterialRequirement.DataValueField = "Id";
                DropDownListCourseMaterialRequirement.DataBind();
                DropDownListCourseMaterialRequirement.Items.Insert(0, new ListItem("--Select One--", ""));
                //----------------------Prerequisite-----------------------------------
            }
        }

        private void BindGridCourseMaterialRequirement()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //var courseInstance = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                var model = (from a in db.CourseMaterialRequirements
                             select new { a.Id, a.Description, a.Active }).ToList();

                GridView1.DataSource = model.ToList();
                GridView1.DataBind();
            }
        }
        private void BindGridCourseMaterialRequirementMapping()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                IQueryable<CourseCourseMaterialRequirement> model = from a in db.CourseCourseMaterialRequirements select a;
                if (DropDownListCourse.SelectedValue != "")
                {
                    int courseId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                    model = from a in model
                            where a.CourseId == courseId
                            select a;
                }
                var result = from a in model
                             select new { a.CourseId, Course = a.Course.Name, a.CourseMaterialRequirementId, a.CourseMaterialRequirement.Description, a.Active };

                GridView2.DataSource = result.ToList();
                GridView2.DataBind();
            }
        }

        protected void btnAddCourseMaterialRequirement_Click(object sender, EventArgs e)
        {
            if (MaterialValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    CourseMaterialRequirement material = new CourseMaterialRequirement()
                    {
                        Description = TextBoxDescription.Text.Trim(),
                        Active = CheckBoxActive.Checked
                    };
                    db.CourseMaterialRequirements.Add(material);
                    db.SaveChanges();
                    lblMessage.Text = "Save Successfully!";
                    lblErrorMessage.Text = "";
                }
                BindDropDownCourseMaterialRequirement();
                BindGridCourseMaterialRequirement();
                CheckBoxActive.Checked = true;
                TextBoxDescription.Text = "";
            }
        }

        protected bool MaterialValidation()
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
            BindGridCourseMaterialRequirement();
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            string description = (row.FindControl("TextBox1") as TextBox).Text.Trim();

            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                CourseMaterialRequirement material = db.CourseMaterialRequirements.Where(x => x.Id == id).FirstOrDefault();

                material.Description = description;
                material.Active = active;

                db.SaveChanges();
            }
            GridView1.EditIndex = -1;
            BindGridCourseMaterialRequirement();
            BindDropDownCourseMaterialRequirement();
            lblMessage.Text = "Update Successfully!";
            lblErrorMessage.Text = "";
        }
        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGridCourseMaterialRequirement();
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);

                using (MaterialEntities db = new MaterialEntities())
                {
                    CourseMaterialRequirement material = db.CourseMaterialRequirements.Where(x => x.Id == id).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.CourseMaterialRequirements.Remove(material);
                    db.SaveChanges();
                }
                BindGridCourseMaterialRequirement();
                BindDropDownCourseMaterialRequirement();
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
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }

        //===================================Course Technology Requirement=========================================
        protected void addCourseMaterialRequirementMapping_Click(object sender, EventArgs e)
        {
            if (MaterialMappingValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                    int courseMaterialRequirementId = Convert.ToInt32(DropDownListCourseMaterialRequirement.SelectedValue);
                    if (!db.CourseCourseMaterialRequirements.Where(x => x.CourseId == courseInstanceId && x.CourseMaterialRequirementId == courseMaterialRequirementId).Any())
                    {
                        CourseCourseMaterialRequirement materialMapping = new CourseCourseMaterialRequirement()
                        {
                            CourseId = Convert.ToInt32(DropDownListCourse.SelectedValue),
                            CourseMaterialRequirementId = Convert.ToInt32(DropDownListCourseMaterialRequirement.SelectedValue),
                            Active = CheckBoxCourseMaterialRequirementMapping.Checked
                        };
                        db.CourseCourseMaterialRequirements.Add(materialMapping);
                        db.SaveChanges();
                        lblMessageCourseMaterialRequirementMapping.Text = "Save Successfully!";
                        lblErrorMessageCourseMaterialRequirementMapping.Text = "";
                    }
                    else
                    {
                        lblErrorMessageCourseMaterialRequirementMapping.Text = "Already Exist!";
                        lblMessageCourseMaterialRequirementMapping.Text = "";
                    }
                }

                BindGridCourseMaterialRequirementMapping();
                CheckBoxCourseMaterialRequirementMapping.Checked = true;
                DropDownListCourseMaterialRequirement.SelectedIndex = -1;
            }
        }
        protected bool MaterialMappingValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(DropDownListCourse.SelectedValue))
            {
                fieldName += " Course Instance -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListCourseMaterialRequirement.SelectedValue))
            {
                fieldName += " Meterial -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessageCourseMaterialRequirementMapping.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessageCourseMaterialRequirementMapping.Text = "";
            }
            return result;
        }

        protected void DropDownListCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCourse.SelectedValue != "")
            {
                BindGridCourseMaterialRequirementMapping();
            }
        }

        protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView2.EditIndex = -1;
            BindGridCourseMaterialRequirementMapping();
        }

        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView2.Rows[e.RowIndex];
                int id = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
                int courseInstanceId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    CourseCourseMaterialRequirement materialMapping = db.CourseCourseMaterialRequirements.Where(x => x.CourseMaterialRequirementId == id && x.CourseId == courseInstanceId).FirstOrDefault();

                    //TODO: Make sure this is correct
                    db.CourseCourseMaterialRequirements.Remove(materialMapping);
                    db.SaveChanges();
                }
                BindGridCourseMaterialRequirementMapping();
                lblMessageCourseMaterialRequirementMapping.Text = "Deleted Successfully!";
                lblErrorMessageCourseMaterialRequirementMapping.Text = "";
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    lblErrorMessageCourseMaterialRequirementMapping.Text = ex.Message;
                }
                else
                {
                    lblErrorMessageCourseMaterialRequirementMapping.Text = ex.InnerException.Message;
                }
                lblMessageCourseMaterialRequirementMapping.Text = "";
            }
        }

        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView2.EditIndex = e.NewEditIndex;
            BindGridCourseMaterialRequirementMapping();
        }

        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView2.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
            int courseInstanceId = Convert.ToInt32(DropDownListCourse.SelectedValue);
            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                CourseCourseMaterialRequirement materialMapping = db.CourseCourseMaterialRequirements.Where(x => x.CourseMaterialRequirementId == id && x.CourseId == courseInstanceId).FirstOrDefault();

                materialMapping.Active = active;
                db.SaveChanges();
            }
            GridView2.EditIndex = -1;
            BindGridCourseMaterialRequirementMapping();
            lblMessageCourseMaterialRequirementMapping.Text = "Update Successfully!";
            lblErrorMessageCourseMaterialRequirementMapping.Text = "";
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
            lblMessageCourseMaterialRequirementMapping.Text = "";
            lblErrorMessage.Text = "";
            lblErrorMessageCourseMaterialRequirementMapping.Text = "";
        }
        private void clearAll()
        {
            TextBoxDescription.Text = "";
            CheckBoxActive.Checked = true;
            CheckBoxCourseMaterialRequirementMapping.Checked = true;
            DropDownListCourse.SelectedIndex = -1;
            DropDownListCourseMaterialRequirement.SelectedIndex = -1;
            BindGridCourseMaterialRequirementMapping();
        }
    }
}