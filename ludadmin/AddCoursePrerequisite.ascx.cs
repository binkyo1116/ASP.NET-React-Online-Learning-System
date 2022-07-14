using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddCoursePrerequisite : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //this.BindGrid();
                BindDropDown();
            }
        }
        private void BindDropDown()
        {
            using (MaterialEntities db = new MaterialEntities())
            {


                var model = db.Courses.Select(x => new { x.Id, x.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourse.DataSource = model;
                DropDownListCourse.DataTextField = "Name";
                DropDownListCourse.DataValueField = "Id";
                DropDownListCourse.DataBind();
                DropDownListCourse.Items.Insert(0, new ListItem("--Select One--", ""));
                //----------------------Prerequisite-----------------------------------
                DropDownListCoursePrerequisite.DataSource = model;
                DropDownListCoursePrerequisite.DataTextField = "Name";
                DropDownListCoursePrerequisite.DataValueField = "Id";
                DropDownListCoursePrerequisite.DataBind();
                DropDownListCoursePrerequisite.Items.Insert(0, new ListItem("--Select One--", ""));
                //----------------------Corequisite-----------------------------------
                DropDownListCourseCorequisite.DataSource = model;
                DropDownListCourseCorequisite.DataTextField = "Name";
                DropDownListCourseCorequisite.DataValueField = "Id";
                DropDownListCourseCorequisite.DataBind();
                DropDownListCourseCorequisite.Items.Insert(0, new ListItem("--Select One--", ""));
            }
        }

        private void BindGridPrerequisite()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                int courseId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                var model = from a in db.CoursePrerequisites
                            where a.CourseId == courseId
                            select new { a.PrerequisiteCourseId, PrerequisiteCourse = a.Course1.Name, a.Active };

                GridViewCoursePrerequisite.DataSource = model.ToList();
                GridViewCoursePrerequisite.DataBind();
            }
        }
        private void BindGridCorequisite()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                int courseId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                var model = (from a in db.CourseCorequisites
                             join b in db.Courses on a.CorequisiteCourseId equals b.Id
                             where a.CourseId == courseId
                             select new { a.CorequisiteCourseId, CorequisiteCourse = b.Name, a.Active }).ToList();

                GridViewCourseCorequisite.DataSource = model.ToList();
                GridViewCourseCorequisite.DataBind();
            }
        }

        protected void btnCoursePrerequisite_Click(object sender, EventArgs e)
        {
            if (PrerequisiteValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                    int PrerequisiteId = Convert.ToInt32(DropDownListCoursePrerequisite.SelectedValue);

                    if (!db.CoursePrerequisites.Where(x => x.PrerequisiteCourseId == PrerequisiteId && x.CourseId == courseId).Any())
                    {
                        CoursePrerequisite coursePrerequisite = new CoursePrerequisite()
                        {
                            CourseId = courseId,
                            PrerequisiteCourseId = PrerequisiteId,
                            Active = CheckBoxActivePrerequisite.Checked
                        };
                        db.CoursePrerequisites.Add(coursePrerequisite);
                        db.SaveChanges();
                        lblMessage.Text = "Save Successfully!";
                        lblErrorMessage.Text = "";
                    }
                    else
                    {
                        lblErrorMessage.Text = "Already exist!";
                        lblMessage.Text = "";

                    }
                }

                BindGridPrerequisite();
                DropDownListCoursePrerequisite.SelectedIndex = -1;
                CheckBoxActivePrerequisite.Checked = true;
            }
        }
        protected void btnCourseCorequisite_Click(object sender, EventArgs e)
        {
            if (CorequisiteValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                    int corequisiteId = Convert.ToInt32(DropDownListCourseCorequisite.SelectedValue);

                    if (!db.CourseCorequisites.Where(x => x.CorequisiteCourseId == corequisiteId && x.CourseId == courseId).Any())
                    {
                        CourseCorequisite courseCorequisite = new CourseCorequisite()
                        {
                            CourseId = courseId,
                            CorequisiteCourseId = corequisiteId,
                            Active = CheckBoxActiveCorequisite.Checked
                        };
                        db.CourseCorequisites.Add(courseCorequisite);
                        db.SaveChanges();
                        lblMessage.Text = "Save Successfully!";
                        lblErrorMessage.Text = "";

                    }
                    else
                    {
                        lblErrorMessage.Text = "Already exist!";
                        lblMessage.Text = "";
                    }
                }
                BindGridCorequisite();
                DropDownListCourseCorequisite.SelectedIndex = -1;
                CheckBoxActiveCorequisite.Checked = true;
            }
        }
        protected bool PrerequisiteValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(DropDownListCourse.SelectedValue))
            {
                fieldName += " Course -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListCoursePrerequisite.SelectedValue))
            {
                fieldName += " Prerequisite -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessage.Text = "";
            }
            return result;
        }
        protected bool CorequisiteValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(DropDownListCourse.SelectedValue))
            {
                fieldName += " Course -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListCourseCorequisite.SelectedValue))
            {
                fieldName += " Corequisite -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessage.Text = "";
            }
            return result;
        }

        protected void DropDownListCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCourse.SelectedValue != "")
            {
                BindGridPrerequisite();
                BindGridCorequisite();
            }
        }

        protected void GridViewCoursePrerequisite_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int courseId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                int id = Convert.ToInt32(GridViewCoursePrerequisite.DataKeys[e.RowIndex].Values[0]);
                using (MaterialEntities db = new MaterialEntities())
                {
                    CoursePrerequisite coursePrerequisite = db.CoursePrerequisites.Where(x => x.PrerequisiteCourseId == id && x.CourseId == courseId).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.CoursePrerequisites.Remove(coursePrerequisite);
                    db.SaveChanges();
                    lblMessage.Text = "Deleted Successfully!";
                    lblErrorMessage.Text = "";
                }
                BindGridPrerequisite();
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

        protected void GridViewCourseCorequisite_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int courseId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                int id = Convert.ToInt32(GridViewCourseCorequisite.DataKeys[e.RowIndex].Values[0]);
                using (MaterialEntities db = new MaterialEntities())
                {
                    CourseCorequisite courseCorequisite = db.CourseCorequisites.Where(x => x.CorequisiteCourseId == id && x.CourseId == courseId).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.CourseCorequisites.Remove(courseCorequisite);
                    db.SaveChanges();
                    lblMessage.Text = "Deleted Successfully!";
                    lblErrorMessage.Text = "";
                }
                BindGridCorequisite();
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

        protected void GridViewCoursePrerequisite_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridViewCoursePrerequisite.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }

        protected void GridViewCourseCorequisite_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridViewCourseCorequisite.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }

        protected void GridViewCoursePrerequisite_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewCoursePrerequisite.EditIndex = e.NewEditIndex;
            BindGridPrerequisite();
        }

        protected void GridViewCoursePrerequisite_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewCoursePrerequisite.EditIndex = -1;
            BindGridPrerequisite();

        }

        protected void GridViewCoursePrerequisite_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridViewCoursePrerequisite.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridViewCoursePrerequisite.DataKeys[e.RowIndex].Values[0]);

            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;
            int courseId = Convert.ToInt32(DropDownListCourse.SelectedValue);
            using (MaterialEntities db = new MaterialEntities())
            {
                CoursePrerequisite coursePrerequisite = db.CoursePrerequisites.Where(x => x.PrerequisiteCourseId == id && x.CourseId == courseId).FirstOrDefault();
                coursePrerequisite.Active = active;
                db.SaveChanges();
            }

            GridViewCoursePrerequisite.EditIndex = -1;
            BindGridPrerequisite();
            lblMessage.Text = "Update Successfully!";
            lblErrorMessage.Text = "";
        }

        protected void GridViewCourseCorequisite_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewCourseCorequisite.EditIndex = e.NewEditIndex;
            BindGridCorequisite();
        }

        protected void GridViewCourseCorequisite_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewCourseCorequisite.EditIndex = -1;
            BindGridCorequisite();
        }

        protected void GridViewCourseCorequisite_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridViewCourseCorequisite.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridViewCourseCorequisite.DataKeys[e.RowIndex].Values[0]);

            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;
            int courseId = Convert.ToInt32(DropDownListCourse.SelectedValue);
            using (MaterialEntities db = new MaterialEntities())
            {
                CourseCorequisite courseCorequisite = db.CourseCorequisites.Where(x => x.CorequisiteCourseId == id && x.CourseId == courseId).FirstOrDefault();
                courseCorequisite.Active = active;
                db.SaveChanges();
            }

            GridViewCourseCorequisite.EditIndex = -1;
            BindGridCorequisite();
            lblMessage.Text = "Update Successfully!";
            lblErrorMessage.Text = "";
        }
    }
}