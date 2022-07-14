using EFModel;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddStudentCourse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            Student student = (Student)Session["sStudentForAddCourse"];
            if (!IsPostBack)
            {
                if (student != null)
                {
                    TextBoxName.Text = student.Name;
                    TextBoxEmail.Text = student.Email;
                    TextBoxCanvasId.Text = Convert.ToString(student.CanvasId);
                    TextBoxMark.Text = Convert.ToString(student.Mark);
                    txtUser.Text = student.UserName;
                    txtPassword.Text = student.Password;

                    btnAddNewStudent.Attributes["disabled"] = "disabled";
                    btnAddNewStudent.Style["pointer-events"] = "none";

                    LabelStudentId.Text = Convert.ToString(student.StudentId);
                    //-----------------------
                    BindGrid();
                }
                btnStudentDelete.Attributes["onclick"] = "return confirm('Do you want to delete?');";
                BindDropDown();
            }
        }
        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                int studentId = Convert.ToInt32(LabelStudentId.Text);
                System.Collections.Generic.ICollection<CourseInstance> courseInstances = db.Students.Find(studentId).CourseInstances;
                var model = from a in courseInstances
                            select new { CourseInstanceId = a.Id, Course = a.Course.Name };

                GridView1.DataSource = model.OrderBy(x => x.CourseInstanceId).ToList();
                GridView1.DataBind();
            }
        }
        private void BindDropDown()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                DropDownListCourse.DataSource = db.CourseInstances.Where(c => c.Active).Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourse.DataTextField = "Name";
                DropDownListCourse.DataValueField = "Id";
                DropDownListCourse.DataBind();
                DropDownListCourse.Items.Insert(0, new ListItem("--Select One--"));
                //----------------student List-----------------------
                DropDownListStudents.DataSource = db.Students.Select(s => new { s.StudentId, s.Name }).OrderBy(x => x.StudentId).ToList();
                DropDownListStudents.DataTextField = "Name";
                DropDownListStudents.DataValueField = "StudentId";
                DropDownListStudents.DataBind();
                DropDownListStudents.Items.Insert(0, new ListItem("--Select Student--", "0"));
            }
        }

        protected void AddNewCourse_Click(object sender, EventArgs e)
        {
            if (DropDownListCourse.SelectedIndex == 0)
            {
                lblErrorMessage.Text = "Please select a course";
                lblMessage.Text = "";
            }
            else
            {
                int studentId = Convert.ToInt32(LabelStudentId.Text);
                int courseInstanceId = Convert.ToInt32(DropDownListCourse.SelectedValue);

                using (MaterialEntities db = new MaterialEntities())
                {
                    Student student = db.Students.Find(studentId);
                    if (!student.CourseInstances.Where(x => x.Id == courseInstanceId).Any())
                    {
                        CourseInstance courseinstance = db.CourseInstances.Find(courseInstanceId);
                        student.CourseInstances.Add(courseinstance);
                        db.SaveChanges();

                        DropDownListCourse.SelectedIndex = -1;
                        lblMessage.Text = "Add Successfully";
                        lblErrorMessage.Text = "";
                        BindGrid();
                    }
                    else
                    {
                        lblErrorMessage.Text = "Sorry! already exist.";
                        lblMessage.Text = "";
                    }
                }

            }

        }
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int courseInstanceid = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                int studentId = Convert.ToInt32(LabelStudentId.Text);
                using (MaterialEntities db = new MaterialEntities())
                {
                    CourseInstance courseInstance = db.CourseInstances.Find(courseInstanceid);
                    Student student = db.Students.Find(studentId);
                    student.CourseInstances.Remove(courseInstance);
                    db.SaveChanges();
                    lblMessage.Text = "Deleted Successfully!";
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
            }
        }
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }

        protected void DropDownListStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            int studentId = Convert.ToInt32(DropDownListStudents.SelectedValue);
            if (studentId > 0)
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    Student model = db.Students.Where(x => x.StudentId == studentId).FirstOrDefault();
                    TextBoxName.Text = model.Name;
                    TextBoxEmail.Text = model.Email;
                    TextBoxCanvasId.Text = Convert.ToString(model.CanvasId);
                    TextBoxMark.Text = Convert.ToString(model.Mark);
                    txtUser.Text = model.UserName;
                    txtPassword.Text = model.Password;

                    btnAddNewStudent.Attributes["disabled"] = "disabled";
                    btnAddNewStudent.Style["pointer-events"] = "none";

                    LabelStudentId.Text = Convert.ToString(model.StudentId);

                    //---------Load Grid--------------------
                    BindGrid();
                }
            }
        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            clearAll();
        }
        private void clearAll()
        {
            TextBoxName.Text = "";
            TextBoxEmail.Text = "";
            TextBoxCanvasId.Text = "";
            TextBoxMark.Text = "";
            txtUser.Text = "";
            txtPassword.Text = "";

            DropDownListCourse.SelectedIndex = -1;
            DropDownListStudents.SelectedIndex = -1;

            LabelStudentId.Text = "";

            btnAddNewStudent.Attributes.Remove("disabled");
            btnAddNewStudent.Style["pointer-events"] = "visible";

            lblMessage.Text = "";
            lblErrorMessage.Text = "";
            GridView1.DataSource = null;
            GridView1.DataBind();
            BindDropDown();
        }

        protected void btnAddNewStudent_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (CheckValidation())
                {
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        try
                        {
                            Student student = new Student()
                            {
                                Name = TextBoxName.Text.Trim(),
                                Email = TextBoxEmail.Text.Trim(),
                                CanvasId = TextBoxCanvasId.Text.Trim() == "" ? 0 : Convert.ToInt32(TextBoxCanvasId.Text.Trim()),
                                Mark = TextBoxMark.Text.Trim() == "" ? 0 : Convert.ToInt32(TextBoxMark.Text.Trim()),
                                UserName = txtUser.Text.Trim(),
                                Password = hashPassword(txtPassword.Text.Trim()),
                                Hash = ""
                            };
                            db.Students.Add(student);
                            db.SaveChanges();
                            LabelStudentId.Text = Convert.ToString(student.StudentId);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }

                    BindDropDown();
                    btnAddNewStudent.Attributes["disabled"] = "disabled";
                    btnAddNewStudent.Style["pointer-events"] = "none";
                    lblMessage.Text = "Save successfully!";
                    lblErrorMessage.Text = "";
                }
            }
            else
            {
                lblMessage.Text = "Fill up all the fields";
                lblErrorMessage.Text = "";
            }
        }

        private bool CheckValidation()
        {

            using (MaterialEntities data = new MaterialEntities())
            {
                if (data.Students.Where(x => x.UserName == txtUser.Text).Any())
                {
                    lblErrorMessage.Text = "Sorry! User Name already exist. Please select another one.";
                    lblMessage.Text = "";
                    return false;
                }
                else if (data.Students.Where(x => x.Email == TextBoxEmail.Text).Any())
                {
                    lblErrorMessage.Text = "Sorry! E-mail already exist. Please select another one.";
                    lblMessage.Text = "";
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        protected void btnStudentUpdate_Click(object sender, EventArgs e)
        {
            int studentId = LabelStudentId.Text == "" ? 0 : Convert.ToInt32(LabelStudentId.Text);
            if (studentId > 0)
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    //-------------Activity table---------------------
                    Student student = db.Students.Where(x => x.StudentId == studentId).FirstOrDefault();

                    student.CanvasId = TextBoxCanvasId.Text.Trim() == "" ? 0 : Convert.ToInt32(TextBoxCanvasId.Text.Trim());
                    student.UserName = txtUser.Text.Trim();
                    student.Password = hashPassword(txtPassword.Text.Trim());
                    student.Name = TextBoxName.Text.Trim();
                    student.Email = TextBoxEmail.Text.Trim();
                    student.Mark = TextBoxMark.Text.Trim() == "" ? 0 : Convert.ToInt32(TextBoxMark.Text.Trim());

                    db.SaveChanges();
                }
                BindDropDown();
                lblMessage.Text = "Update Successfully";
                lblErrorMessage.Text = "";
            }
            else
            {
                lblErrorMessage.Text = "Please add an activity first";
                lblMessage.Text = "";
            }
        }

        protected void btnStudentDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int studentId = LabelStudentId.Text == "" ? 0 : Convert.ToInt32(LabelStudentId.Text);
                if (studentId > 0)
                {
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        //TODO: What is this doing? Why is it deleting the student?
                        var student = db.Students.Where(x => x.StudentId == studentId).FirstOrDefault();
                        db.Students.Remove(student);
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

        protected void btnStudentListPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudentManagement.aspx");
        }

        protected string hashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Send a password to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                // Get the hashed string.  
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                // Print the string.   
                return hash;
            }
        }

    }
}