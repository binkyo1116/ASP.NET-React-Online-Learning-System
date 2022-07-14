using EFModel;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class StudentManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
                BindDropdownStudent();
            }
        }
        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var model = from a in db.Students
                            select new { a.StudentId, a.Name, a.Email, a.CanvasId, a.Mark, a.UserName, a.Password };
                if (ddStudents.SelectedValue!="") {
                    var studentId = Convert.ToInt32(ddStudents.SelectedValue);
                    model = from a in model
                            where a.StudentId == studentId
                            select a;
                }
                GridView1.DataSource = model.OrderBy(x => x.StudentId).ToList();
                GridView1.DataBind();
            }
        }

        protected void AddNewStudent_Click(object sender, EventArgs e)
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
                                StudentId = GenarateId(),
                                Name = TextBoxName.Text.Trim(),
                                Email = TextBoxEmail.Text.Trim(),
                                CanvasId = TextBoxCanvasId.Text.Trim() == "" ? 0 : Convert.ToInt32(TextBoxCanvasId.Text.Trim()),
                                Mark = TextBoxMark.Text.Trim() == "" ? 0 : Convert.ToInt32(TextBoxMark.Text.Trim()),
                                UserName = txtUser.Text.Trim(),
                                Password = hashPassword(txtPassword.Text.Trim())
                            };
                            db.Students.Add(student);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                    ClearTextField();
                    BindGrid();
                    lblMessage.Text = "Save successfully!";
                }
            }
            else
            {
                lblMessage.Text = "Fill up all the fields";
            }
        }

        private int GenarateId()
        {
            int id = 0;
            using (MaterialEntities data = new MaterialEntities())
            {
                Student lastEntry = data.Students.OrderByDescending(x => x.StudentId).FirstOrDefault();
                if (lastEntry != null)
                {
                    id = ++lastEntry.StudentId;
                }
                else
                {
                    id = 1;
                }
            }
            return id;
        }

        private void ClearTextField()
        {
            TextBoxName.Text = "";
            TextBoxEmail.Text = "";
            txtUser.Text = "";
            TextBoxMark.Text = "";
            TextBoxCanvasId.Text = "";
        }

        private bool CheckValidation()
        {

            using (MaterialEntities data = new MaterialEntities())
            {
                if (data.Students.Where(x => x.UserName == txtUser.Text).Any())
                {
                    lblMessage.Text = "Sorry! User Name already exist. Please select another one.";
                    return false;
                }
                else if (data.Students.Where(x => x.Email == TextBoxEmail.Text).Any())
                {
                    lblMessage.Text = "Sorry! E-mail already exist. Please select another one.";
                    return false;
                }
                else
                {
                    return true;
                }
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

            int canvasId = Convert.ToInt32((row.FindControl("TextBox4") as TextBox).Text);
            string userName = (row.FindControl("TextBox2") as TextBox).Text;
            string password = (row.FindControl("TextBox6") as TextBox).Text;
            string name = (row.FindControl("TextBox1") as TextBox).Text;
            string email = (row.FindControl("TextBox3") as TextBox).Text;
            int mark = (row.FindControl("TextBox5") as TextBox).Text == "" ? 0 : Convert.ToInt32((row.FindControl("TextBox5") as TextBox).Text);

            using (MaterialEntities db = new MaterialEntities())
            {
                Student student = db.Students.Where(x => x.StudentId == id).FirstOrDefault();

                student.CanvasId = canvasId;
                student.UserName = userName;
                student.Password = hashPassword(password);
                student.Name = name;
                student.Email = email;
                student.Mark = mark;

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

        //protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
        //    using (MaterialEntities db = new MaterialEntities())
        //    {
        //        Student student = db.Students.Where(x => x.StudentId == id).FirstOrDefault();
        //        //TODO: Make sure this is correct
        //        //db.Students.Remove(student);
        //        db.SaveChanges();
        //    }
        //    BindGrid();
        //}

        //protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
        //    {
        //        (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row with ID = " + DataBinder.Eval(e.Row.DataItem, "StudentId") + "?');";
        //    }
        //}

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int id = Convert.ToInt32(GridView1.DataKeys[rowIndex].Values[0]);
                //Reference the GridView Row.
                GridViewRow row = GridView1.Rows[rowIndex];
                ////Fetch value of Name.
                //string name = (row.FindControl("Label1") as Label).Text;
                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Name: " + name + "');", true);

                using (MaterialEntities db = new MaterialEntities())
                {
                    Student student = db.Students.Where(x => x.StudentId == id).FirstOrDefault();
                    Session["sStudentForAddCourse"] = student;
                }
                Page.ClientScript.RegisterStartupScript(GetType(), "OpenWindow", "window.open('AddStudentCourse.aspx','_newtab');", true);
                //Response.Redirect("AddStudentCourse.aspx");
            }
        }

        protected void btnAddCourse_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "OpenWindow", "window.open('AddStudentCourse.aspx','_newtab');", true);
        }
        private void BindDropdownStudent()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var students = db.Students.Where(s => !s.Test.HasValue || s.Test == false).Select(x => new { x.StudentId, x.Name });
                ddStudents.DataSource = students.OrderBy(s => s.Name).ToList();
                ddStudents.DataTextField = "Name";
                ddStudents.DataValueField = "StudentId";
                ddStudents.DataBind();
                ddStudents.Items.Insert(0, new ListItem("--All Students--", ""));
            }
        }

        protected void ddStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
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