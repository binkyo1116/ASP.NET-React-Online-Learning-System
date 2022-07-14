using AdminPages.Services;
using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddCourse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDownSchool();
                BindDropDownGradeScale();
                BindGrid();

            }
        }

        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                if (DropDownListSearchBySchool.SelectedValue != "")
                {
                    int schoolId = Convert.ToInt32(DropDownListSearchBySchool.SelectedValue);
                    IQueryable<Course> model = db.Courses.Where(x => x.SchoolId == schoolId);
                    SetDatainGrid(model);
                }
                else
                {
                    SetDatainGrid(db.Courses);
                }

            }
        }
        private void SetDatainGrid(IQueryable<Course> data)
        {
            System.Collections.Generic.List<Course> d = data.ToList();
            byte[] defaultv = Array.Empty<byte>();
            var model = (from a in data
                         select new { CourseId = a.Id, a.Name, a.SchoolId, School = a.School.Name, a.GradeScaleGroupId, GradeScaleGroup = a.GradeScaleGroup.Title, a.Description, a.Credits, a.Department, a.Number, Picture = a.Picture == null ? defaultv : a.Picture }).ToList();

            GridView1.DataSource = model;
            GridView1.DataBind();
        }
        private void BindDropDownSchool()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //-----------School-----------------------------
                DropDownListSchool.DataSource = db.Schools.ToList();
                DropDownListSchool.DataTextField = "Name";
                DropDownListSchool.DataValueField = "SchoolId";
                DropDownListSchool.DataBind();
                DropDownListSchool.Items.Insert(0, new ListItem("--Select one--", ""));

                //-----------School for Search-----------------------------
                DropDownListSearchBySchool.DataSource = db.Schools.ToList();
                DropDownListSearchBySchool.DataTextField = "Name";
                DropDownListSearchBySchool.DataValueField = "SchoolId";
                DropDownListSearchBySchool.DataBind();
                DropDownListSearchBySchool.Items.Insert(0, new ListItem("--Select one--", ""));
            }
        }
        private void BindDropDownGradeScale()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //-----------School-----------------------------
                DropDownListGradeScale.DataSource = db.GradeScaleGroups.OrderBy(x => x.Title).ToList();
                DropDownListGradeScale.DataTextField = "Title";
                DropDownListGradeScale.DataValueField = "Id";
                DropDownListGradeScale.DataBind();
                //DropDownListGradeScale.Items.Insert(0, new ListItem("--Select one--", ""));
            }
        }

        protected void AddNewCourse_Click(object sender, EventArgs e)
        {
            if (CourseValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    byte[] imageBytes = new ImageResizingService().ImageBytes(FileUploadImage.PostedFile.InputStream);
                    Course course = new Course()
                    {
                        Name = TextBoxCourseName.Text.Trim(),
                        SchoolId = Convert.ToInt32(DropDownListSchool.SelectedValue),
                        Description = TextBoxDescription.Text.Trim(),
                        Department = TextBoxDepartment.Text.Trim(),
                        Credits = Convert.ToInt32(TextBoxCredits.Text.Trim()),
                        Number = Convert.ToInt32(TextBoxNumber.Text.Trim()),
                        Picture = imageBytes,
                        GradeScaleGroupId = Convert.ToInt32(DropDownListGradeScale.SelectedValue)
                    };
                    db.Courses.Add(course);
                    db.SaveChanges();
                    lblMessage.Text = "Saved Successfully!";
                    lblErrorMessage1.Text = "";
                    lblErrorMessage.Text = "";
                    lblSuccessMessage.Text = "";
                }

                ClearTextField();
                BindGrid();
            }
        }
        private void ClearTextField()
        {
            TextBoxCourseName.Text = "";
            TextBoxDescription.Text = "";
            TextBoxDepartment.Text = "";
            TextBoxCredits.Text = "";
            TextBoxNumber.Text = "";
        
        }
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == GridView1.EditIndex)
            {
                string SchoolId = (e.Row.FindControl("Label7") as Label).Text;
                DropDownList schoolList = e.Row.FindControl("DropDownListSchoolGV") as DropDownList;

                using (MaterialEntities db = new MaterialEntities())
                {
                    schoolList.DataSource = db.Schools.OrderBy(x => x.SchoolId).ToList();
                    schoolList.DataTextField = "Name";
                    schoolList.DataValueField = "SchoolId";
                    schoolList.DataBind();
                }
                schoolList.SelectedValue = SchoolId;
                //---------Grade Scale-----------------------
                string gradeScaleGroupId = (e.Row.FindControl("Label8") as Label).Text;
                DropDownList gradeScaleGroupList = e.Row.FindControl("DropDownListGradeScaleGroup") as DropDownList;

                using (MaterialEntities db = new MaterialEntities())
                {
                    gradeScaleGroupList.DataSource = db.GradeScaleGroups.OrderBy(x => x.Id).ToList();
                    gradeScaleGroupList.DataTextField = "Title";
                    gradeScaleGroupList.DataValueField = "Id";
                    gradeScaleGroupList.DataBind();
                }
                gradeScaleGroupList.SelectedValue = SchoolId;
            }
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row ?');";
            }
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
            string name = (row.FindControl("TextBox1") as TextBox).Text;
            int schoolid = Convert.ToInt32((row.FindControl("DropDownListSchoolGV") as DropDownList).SelectedValue);
            int gradeScaleGroupId = Convert.ToInt32((row.FindControl("DropDownListGradeScaleGroup") as DropDownList).SelectedValue);
            string description = (row.FindControl("TextBox5") as TextBox).Text;
            int number = Convert.ToInt32((row.FindControl("TextBox4") as TextBox).Text);
            string department = (row.FindControl("TextBox3") as TextBox).Text;
            int credits = Convert.ToInt32((row.FindControl("TextBox2") as TextBox).Text);
            byte[] imageBytes = new ImageResizingService().ImageBytes((row.FindControl("FileUploadImage") as FileUpload).PostedFile.InputStream);

            using (MaterialEntities db = new MaterialEntities())
            {
                Course course = (from c in db.Courses
                                 where c.Id == id
                                 select c).FirstOrDefault();

                course.Name = name;
                course.SchoolId = schoolid;
                course.Description = description;
                course.Department = department;
                course.Number = number;
                course.Credits = credits;
                course.GradeScaleGroupId = gradeScaleGroupId;
                if (imageBytes != null)
                {
                    course.Picture = imageBytes;
                }

                db.SaveChanges();
            }
            GridView1.EditIndex = -1;
            BindGrid();
        }
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            try
            {
                using (MaterialEntities db = new MaterialEntities())
                {

                    Course course = (from c in db.Courses
                                     where c.Id == id
                                     select c).FirstOrDefault();
                    db.Courses.Remove(course);
                    db.SaveChanges();
                    lblSuccessMessage.Text = "Deleted Successfully!";
                    lblErrorMessage1.Text = "";
                    lblErrorMessage.Text = "";
                    lblMessage.Text = "";
                }
                BindGrid();
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    lblErrorMessage.Text = ex.Message;
                }
                else {
                    lblErrorMessage.Text = ex.InnerException.InnerException.Message;
                }
                
                lblErrorMessage1.Text = "";
                lblMessage.Text = "";
                lblSuccessMessage.Text = "";
                //throw new Exception(ex.Message);
            }
           
        }

        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }

        //-----------------Validation-----------------
        protected bool CourseValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(TextBoxCourseName.Text))
            {
                fieldName += " CourseName -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListSchool.SelectedValue))
            {
                fieldName += " School -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxCredits.Text))
            {
                fieldName += " Credits -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxDepartment.Text))
            {
                fieldName += " Department -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxNumber.Text))
            {
                fieldName += " Number -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxDescription.Text))
            {
                fieldName += " Description -";
                result = false;
            }

            if (!result)
            {
                lblErrorMessage1.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessage.Text = "";
                lblErrorMessage.Text = "";
                lblSuccessMessage.Text = "";
            }
            return result;
        }
        protected void ShowAllList_Click(object sender, EventArgs e)
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                SetDatainGrid(db.Courses);
            }
        }

        protected void DropDownListSearchBySchool_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListSearchBySchool.SelectedValue != "")
            {
                BindGrid();
            }
        }
    }
}