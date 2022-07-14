using EFModel;
using LMSLibrary;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class ApproveRequestLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            //Student student = (Student)Session["sStudentForAddCourse"];
            if (!IsPostBack)
            {
                //if (student != null)
                //{
                //    TextBoxName.Text = student.Name;
                //    TextBoxEmail.Text = student.Email;
                //    TextBoxCanvasId.Text = Convert.ToString(student.CanvasId);
                //    TextBoxMark.Text = Convert.ToString(student.Mark);
                //    txtUser.Text = student.UserName;
                //    txtPassword.Text = student.Password;

                //    btnAddNewStudent.Attributes["disabled"] = "disabled";
                //    btnAddNewStudent.Style["pointer-events"] = "none";

                //    LabelStudentId.Text = Convert.ToString(student.StudentId);
                //    //-----------------------
                //    BindGrid();
                //}
                BindRequestLoginGrid();
                BindDropDownCourse();
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
                //DropDownListCourseInstance.DataSource = db.CourseInstances.Where(c => c.Active).Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
                //DropDownListCourseInstance.DataTextField = "Name";
                //DropDownListCourseInstance.DataValueField = "Id";
                //DropDownListCourseInstance.DataBind();
                //DropDownListCourseInstance.Items.Insert(0, new ListItem("--Select One--"));
                //----------------student List-----------------------
                DropDownListStudents.DataSource = db.Students.Select(s => new { s.StudentId, s.Name }).OrderBy(x => x.StudentId).ToList();
                DropDownListStudents.DataTextField = "Name";
                DropDownListStudents.DataValueField = "StudentId";
                DropDownListStudents.DataBind();
                DropDownListStudents.Items.Insert(0, new ListItem("--Select Student--", "0"));
            }
        }
        private void BindDropDownCourse()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                DropDownListCourseFilter2.DataSource = db.Courses.Select(x => new { x.Id, x.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourseFilter2.DataTextField = "Name";
                DropDownListCourseFilter2.DataValueField = "Id";
                DropDownListCourseFilter2.DataBind();
                DropDownListCourseFilter2.Items.Insert(0, new ListItem("--Select Course--", ""));
            }
        }
        protected void DropDownListCourseFilter2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string i = DropDownListCourseFilter2.SelectedValue;
            DropDownListCourseInstance.Items.Clear();
            if (i != "")
            {

                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseId = Convert.ToInt32(i);
                    IQueryable<CourseInstance> courseIns = db.CourseInstances.Where(ci => ci.Course.Id == courseId && ci.Active);

                    var list = courseIns.Select(x => new { Name = x.Quarter.StartDate + " TO " + x.Quarter.EndDate, Id = x.Id }).OrderBy(x => x.Id).ToList();
                    //---------------Coruse Instance Droup Down list----------------
                    DropDownListQuarterFilter2.DataSource = list;
                    DropDownListQuarterFilter2.DataTextField = "Name";
                    DropDownListQuarterFilter2.DataValueField = "Id";
                    DropDownListQuarterFilter2.DataBind();
                    //--------------------------------------------------------------------------------
                    if (courseIns.Count() == 1)
                    {
                        BindDropDownListsCourseInstanceFilter(courseIns.FirstOrDefault().Id);
                    }
                    else
                    {
                        DropDownListQuarterFilter2.Items.Insert(0, new ListItem("--Select One--", ""));
                    }
                }
            }
        }
        protected void DropDownListQuarterFilter2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string i = DropDownListQuarterFilter2.SelectedValue;
            if (i != "")
            {
                int courseInstanceId = Convert.ToInt32(i);
                BindDropDownListsCourseInstanceFilter(courseInstanceId);
            }
        }
        protected void BindDropDownListsCourseInstanceFilter(int courseInstanceId)
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var list = db.CourseInstances.Where(ci => ci.Id == courseInstanceId && ci.Active).Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
                //---------------Coruse Instance Droup Down list----------------
                DropDownListCourseInstance.DataSource = list;
                DropDownListCourseInstance.DataTextField = "Name";
                DropDownListCourseInstance.DataValueField = "Id";
                DropDownListCourseInstance.DataBind();
            }
        }
        protected void AddNewCourse_Click(object sender, EventArgs e)
        {
            if (DropDownListCourseInstance.SelectedValue =="")
            {
                lblErrorMessage.Text = "Please select a course";
                lblMessage.Text = "";
            }
            else
            {
                if (LabelStudentId.Text != "")
                {

                    int studentId = Convert.ToInt32(LabelStudentId.Text);
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);

                    using (MaterialEntities db = new MaterialEntities())
                    {
                        Student student = db.Students.Find(studentId);
                        if (!student.CourseInstances.Where(x => x.Id == courseInstanceId).Any())
                        {
                            CourseInstance courseinstance = db.CourseInstances.Find(courseInstanceId);
                            student.CourseInstances.Add(courseinstance);
                            db.SaveChanges();

                            DropDownListCourseFilter2.SelectedIndex=-1;
                            DropDownListQuarterFilter2.Items.Clear();
                            DropDownListCourseInstance.Items.Clear();

                            lblMessage.Text = "Add Successfully";
                            lblErrorMessage.Text = "";
                            BindGrid();
                            PanelCourseList.Visible = true;
                        }
                        else
                        {
                            lblErrorMessage.Text = "Sorry! already exist.";
                            lblMessage.Text = "";
                        }
                    }
                }
                else {
                    lblErrorMessage.Text = "Please add a student!";
                    lblMessage.Text = "";
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

                    lblMessage.Text = "Delete Successfully!";
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
                    PanelAddStudent.Visible = true;
                    PanelAddCourse.Visible = true;
                    PanelCourseList.Visible = true;
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

            DropDownListCourseFilter2.SelectedIndex = -1;
            DropDownListQuarterFilter2.Items.Clear();
            DropDownListCourseInstance.Items.Clear();
            //DropDownListStudents.SelectedIndex = -1;

            LabelStudentId.Text = "";

            btnAddNewStudent.Attributes.Remove("disabled");
            btnAddNewStudent.Style["pointer-events"] = "visible";

            lblMessage.Text = "";
            lblErrorMessage.Text = "";
            GridView1.DataSource = null;
            GridView1.DataBind();
            BindDropDown();
            PanelAddStudent.Visible = false;
            PanelAddCourse.Visible = false;
            PanelCourseList.Visible = false;
            TextBoxCourseByStudetn.Text = "";
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
                            PanelAddCourse.Visible = true;
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
                lblErrorMessage.Text = "Fill up all the fields";
                lblMessage.Text = "";
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
        //protected void btnStudentDelete_Click(object sender, EventArgs e)
        //{
        //    int studentId = LabelStudentId.Text == "" ? 0 : Convert.ToInt32(LabelStudentId.Text);
        //    if (studentId > 0)
        //    {
        //        using (MaterialEntities db = new MaterialEntities())
        //        {
        //            //TODO: What is this doing? Why is it deleting the student?
        //            /*var student = db.Students.Where(x => x.StudentId == Convert.ToInt32(LabelStudentId.Text));
        //            db.Students.Remove(student);
        //            db.SaveChanges();*/
        //        }
        //        clearAll();
        //        lblMessage.Text = "Delete Successfully";
        //    }
        //    else
        //    {
        //        lblMessage.Text = "Please add an activity first";
        //    }
        //}
        protected void btnStudentListPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudentManagement.aspx");
        }
        #region================================ Request Login==================================
        private void BindRequestLoginGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var model = from a in db.RequestLogins select a;
                           

                if (RadioButtonPending.Checked)
                {
                    model = from a in model where a.ApprovalStatus==0 select a;
                }
                if (RadioButtonApproved.Checked)
                {
                    model = from a in model where a.ApprovalStatus==1 select a;
                }
                if (RadioButtonReject.Checked)
                {
                    model = from a in model where a.ApprovalStatus==2 select a;
                }
                var requestList = from a in model
                            select new { a.RequestLoginId, a.SchoolName, a.CourseName, a.Name, a.Email, a.TimeStamp, Status = a.ApprovalStatus, ApprovalStatus = a.ApprovalStatus == 0 ? "Pending" : a.ApprovalStatus == 1 ? "Approved" : "Reject" };

                GridViewRequestLogin.DataSource = requestList.OrderBy(x => x.RequestLoginId).ToList();
                GridViewRequestLogin.DataBind();
            }
        }
        #endregion===============================================================================

        protected void GridViewRequestLogin_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Approve")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    clearAll();
                    //-----------------Find Request----------------------------------
                    int requestId = Convert.ToInt32(e.CommandArgument);
                    var requestModel = db.RequestLogins.Find(requestId);
                    requestModel.ApprovalStatus = 1;
                    db.SaveChanges();
                    BindRequestLoginGrid();
                    //-----------------------------Load Request--------------------------------------
                    TextBoxName.Text = requestModel.Name;
                    TextBoxEmail.Text = requestModel.Email;
                    TextBoxCourseByStudetn.Text = requestModel.CourseName;
                    var password = System.Web.Security.Membership.GeneratePassword(10, 2);
                    //SendMail(requestModel.Name, requestModel.Email, password);
                    string msg = "Your login request has been approved. Your username is: " + requestModel.Name + ". Your password is: " + password;
                    EmailHelper.SendEmail(
                            new EmailHelper.Message
                            {
                                Subject = "Learning System Password",
                                Recipient = requestModel.Email,
                                Body = msg
                            }
                         );
                    txtPassword.Text = password;
                    txtUser.Text = requestModel.Email;
                    PanelAddStudent.Visible = true;
                    lblMessage.Text = "Request approve Successfully!";
                    lblErrorMessage.Text = "";
                }
            }
            else if (e.CommandName == "Reject") 
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    //-----------------Find Request----------------------------------
                    int requestId = Convert.ToInt32(e.CommandArgument);
                    var requestModel = db.RequestLogins.Find(requestId);
                    requestModel.ApprovalStatus = 2;
                    db.SaveChanges();
                    BindRequestLoginGrid();
                }
            }
        }

        //protected void SendMail(string name, string email, string password)
        //{
        //    MailMessage msg = new MailMessage();
        //    msg.From = new MailAddress("letsusedata2021@outlook.com");
        //    msg.To.Add(email);
        //    msg.Body = "Your login request has been approved. Your username is: "+ name + ". Your password is: " + password;
        //    msg.IsBodyHtml = true;
        //    msg.Subject = "User approved";
        //    SmtpClient smt = new SmtpClient("smtp-mail.outlook.com");
        //    smt.Port = 587;
        //    smt.Credentials = new NetworkCredential("letsusedata2021@outlook.com", "Qss25255");
        //    smt.EnableSsl = true;
        //    smt.Send(msg);
        //}

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


        protected void RadioButtonPending_CheckedChanged(object sender, EventArgs e)
        {
            BindRequestLoginGrid();
        }

        protected void RadioButtonApproved_CheckedChanged(object sender, EventArgs e)
        {
            BindRequestLoginGrid();
        }

        protected void RadioButtonReject_CheckedChanged(object sender, EventArgs e)
        {
            BindRequestLoginGrid();
        }

        protected void GridViewRequestLogin_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int status = Convert.ToInt32((e.Row.FindControl("Label2") as Label).Text);
                LinkButton approve = e.Row.FindControl("LinkButtonApprove") as LinkButton;
                LinkButton reject = e.Row.FindControl("LinkButtonReject") as LinkButton;

                if (status == 1) {
                    approve.Visible = false;
                    reject.Visible = false;
                } else if(status == 2){
                    approve.Visible = true;
                    reject.Visible = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.FindControl("LinkButtonApprove") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to approve this student?');";
                (e.Row.FindControl("LinkButtonReject") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to reject this student?');";
            }
        }
    }
}