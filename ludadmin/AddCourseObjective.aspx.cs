using EFModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddCourseObjective : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
                BindDropDownCourse();
                BindDropDownCourseObjective();
            }
        }
        private void BindDropDownCourse()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                DropDownListCourse.DataSource = db.Courses.OrderBy(x => x.Id).ToList();
                DropDownListCourse.DataTextField = "Name";
                DropDownListCourse.DataValueField = "Id";
                DropDownListCourse.DataBind();
                DropDownListCourse.Items.Insert(0, new ListItem("--Select One--", ""));
            }
        }
        private void BindDropDownCourseObjective()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                DropDownListCourseObjective.DataSource = db.CourseObjectives.OrderBy(x => x.Id).ToList();
                DropDownListCourseObjective.DataTextField = "Description";
                DropDownListCourseObjective.DataValueField = "Id";
                DropDownListCourseObjective.DataBind();
                DropDownListCourseObjective.Items.Insert(0, new ListItem("--Select One--", ""));
            }
        }
        protected void DropDownListCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCourse.SelectedValue != "")
            {
                BindGrid();
            }
        }
        protected void AddNewCourseObjective_Click(object sender, EventArgs e)
        {
            if (CourseObjectiveValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    CourseObjective courseObj = new CourseObjective()
                    {
                        Description = TextBoxDescription.Text,
                        Active = CheckBoxActive.Checked
                    };
                    db.CourseObjectives.Add(courseObj);
                    db.SaveChanges();
                    BindDropDownCourseObjective();
                    DropDownListCourseObjective.SelectedValue = Convert.ToString(courseObj.Id);
                }
                ClearTextField();
                BindGrid();

                lblMessage.Text = "Save Successfully!";
                lblErrorMessage.Text = "";
            }
        }
        private void ClearTextField()
        {
            TextBoxDescription.Text = "";
            //DropDownListCourse.SelectedIndex = -1;
            CheckBoxActive.Checked = true;
        }
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGrid();
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            string description = (row.FindControl("TextBox1") as TextBox).Text;
            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                CourseObjective courseObj = (from c in db.CourseObjectives
                                             where c.Id == id
                                             select c).FirstOrDefault();

                courseObj.Description = description;
                courseObj.Active = active;
                db.SaveChanges();
            }
            GridView1.EditIndex = -1;
            BindGrid();
        }
        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            try
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    CourseObjective courseObj = db.CourseObjectives.Where(x => x.Id == id).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.CourseObjectives.Remove(courseObj);
                    db.SaveChanges();
                    lblMessage.Text = "Deleted Successfully.";
                    lblErrorMessage.Text = "";
                }
                BindGrid();
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
                //throw new Exception(ex.Message);
            }

        }
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e) 
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row ?');";
            }
        }
        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                if (DropDownListCourse.SelectedValue != "")
                {
                    List<CourseObjective> model = db.Courses.Find(Convert.ToInt32(DropDownListCourse.SelectedValue)).CourseObjectives.ToList();
                    SetDatainGrid(model.ToList());
                }
                else
                {
                    System.Data.Entity.DbSet<CourseObjective> model = db.CourseObjectives;
                    SetDatainGrid(model.ToList());
                }

            }
        }
        private void SetDatainGrid(List<CourseObjective> model)
        {

            GridView1.DataSource = model.OrderBy(x => x.Id);
            GridView1.DataBind();
        }

        //--------------------Validation---------------------------
        protected bool CourseObjectiveValidation()
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
        protected bool CourseCourseObjectiveValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(DropDownListCourse.SelectedValue))
            {
                fieldName += " Course -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListCourseObjective.SelectedValue))
            {
                fieldName += " Course Objective -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessage.Text = "";
            }
            return result;
        }
        protected void ShowAllModuleList_Click(object sender, EventArgs e)
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                DropDownListCourse.SelectedIndex = -1;
                BindGrid();
            }
        }

        protected void ButtonCourseCourseObjective_Click(object sender, EventArgs e)
        {
            if (CourseCourseObjectiveValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                    int courseObjectiveId = Convert.ToInt32(DropDownListCourseObjective.SelectedValue);
                    Course course = db.Courses.Find(courseId);
                    bool exist = course.CourseObjectives.Where(x => x.Id == courseObjectiveId).Any();
                    if (!exist)
                    {
                        CourseObjective courseObj = db.CourseObjectives.Find(courseObjectiveId);
                        course.CourseObjectives.Add(courseObj);
                        db.SaveChanges();
                        lblMessage.Text = "Add Successfully.";
                        lblErrorMessage.Text = "";
                        BindGrid();
                    }
                    else
                    {
                        lblErrorMessage.Text = "Sorry! The Course Objective already exist. Please select another one.";
                        lblMessage.Text = "";
                    }
                }
            }
        }
        private void clearAll()
        {
            TextBoxDescription.Text = "";
            CheckBoxActive.Checked = true;

            DropDownListCourse.SelectedIndex = -1;
            BindDropDownCourseObjective();
            BindGrid();
        }
        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            clearAll();
            lblMessage.Text = "";
            lblErrorMessage.Text = "";
        }
    }
}