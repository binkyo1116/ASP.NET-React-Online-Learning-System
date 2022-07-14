using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddInstructor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindInstructorDropDown();
            }
        }
        private void BindInstructorDropDown()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                DropDownListInstructor.DataSource = db.Instructors.OrderBy(x => x.Id).ToList();
                DropDownListInstructor.DataTextField = "InstructorName";
                DropDownListInstructor.DataValueField = "Id";
                DropDownListInstructor.DataBind();
                DropDownListInstructor.Items.Insert(0, new ListItem("--Select One--", "0"));
            }
        }
        private void BindCourseInstanceDropDown()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var model = from a in db.CourseInstances
                            select new { Course = a.Course.Name, CourseInstanceId = a.Id };

                DropDownListCourse.DataSource = model.OrderBy(x => x.CourseInstanceId).ToList();
                DropDownListCourse.DataTextField = "Course";
                DropDownListCourse.DataValueField = "CourseInstanceId";
                DropDownListCourse.DataBind();
                DropDownListCourse.Items.Insert(0, new ListItem("--Select One--", "0"));
            }
        }

        protected void DropDownListInstructor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListInstructor.SelectedValue != "0")
            {
                int instructorId = Convert.ToInt32(DropDownListInstructor.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    Instructor instructor = db.Instructors.Where(x => x.Id == instructorId).FirstOrDefault();
                    TextBoxInstructorName.Text = instructor.InstructorName;
                    PanelInstructorContactInfo.Visible = true;
                    btnAddInstructor.Attributes["disabled"] = "disabled";
                    btnAddInstructor.Style["pointer-events"] = "none";
                    BindCourseInstanceDropDown();
                    BindGrid();
                    BindInstructorCourseGrid();
                }
            }
            else
            {
                PanelInstructorContactInfo.Visible = false;
            }
        }
        protected void btnAddInstructor_Click(object sender, EventArgs e)
        {
            if (InstructorValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    Instructor instructor = new Instructor()
                    {
                        InstructorName = TextBoxInstructorName.Text.Trim(),

                    };
                    db.Instructors.Add(instructor);
                    db.SaveChanges();

                    BindInstructorDropDown();
                    DropDownListInstructor.SelectedValue = Convert.ToString(instructor.Id);
                }
                lblMessage.Text = "Save Successfully!";
                lblErrorMessage.Text = "";
                btnAddInstructor.Attributes["disabled"] = "disabled";
                btnAddInstructor.Style["pointer-events"] = "none";
                BindCourseInstanceDropDown();
                PanelInstructorContactInfo.Visible = true;
            }
        }
        protected void btnUpdateInstructor_Click(object sender, EventArgs e)
        {
            if (InstructorValidation() && DropDownListInstructor.SelectedValue != "0")
            {
                int instructorId = Convert.ToInt32(DropDownListInstructor.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    Instructor instructor = db.Instructors.Where(x => x.Id == instructorId).FirstOrDefault();
                    instructor.InstructorName = TextBoxInstructorName.Text.Trim();
                    db.SaveChanges();

                    BindInstructorDropDown();
                    DropDownListInstructor.SelectedValue = Convert.ToString(instructor.Id);
                    lblMessage.Text = "Update Successfully!";
                    lblErrorMessage.Text = "";
                }
            }
        }
        protected void btnDeleteInstructor_Click(object sender, EventArgs e)
        {
            if (DropDownListInstructor.SelectedValue != "0")
            {
                try
                {
                    int instructorId = Convert.ToInt32(DropDownListInstructor.SelectedValue);
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        //------------Delete from InstructorCourse---------------
                        IQueryable<InstructorCourse> instructorCourse = db.InstructorCourses.Where(x => x.InstructorId == instructorId);
                        if (instructorCourse.Any())
                        {
                            //TODO: Make sure this is correct
                            db.InstructorCourses.RemoveRange(instructorCourse);
                            db.SaveChanges();
                        }
                        //------------Delete from ContactInformation---------------
                        IQueryable<ContactInformation> contactInformation = db.ContactInformations.Where(x => x.InstructorId == instructorId);
                        if (contactInformation.Any())
                        {
                            //TODO: Make sure this is correct
                            db.ContactInformations.RemoveRange(contactInformation);
                            db.SaveChanges();
                        }
                        //------------Delete from ContactInformation---------------
                        Instructor instructor = db.Instructors.Where(x => x.Id == instructorId).FirstOrDefault();
                        //TODO: Make sure this is correct
                        db.Instructors.Remove(instructor);
                        db.SaveChanges();
                    }
                    clearAll();
                    BindInstructorDropDown();
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
        }
        protected void btnAddContactInfo_Click(object sender, EventArgs e)
        {
            if (ContactInfoValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    ContactInformation contactInfo = new ContactInformation()
                    {
                        InstructorId = Convert.ToInt32(DropDownListInstructor.SelectedValue),
                        ContactInfo = TextBoxContactInfo.Text.Trim(),
                        Active = CheckBoxContactInfoActive.Checked,
                        Preferred = CheckBoxPreferred.Checked

                    };
                    db.ContactInformations.Add(contactInfo);
                    db.SaveChanges();

                }
                lblMessage.Text = "Save Successfully!";
                lblErrorMessage.Text = "";
                BindGrid();
                clearText();
            }
        }
        protected void btnAddInstructorCourse_Click(object sender, EventArgs e)
        {
            if (CourseValidation())
            {
                int instructorId = Convert.ToInt32(DropDownListInstructor.SelectedValue);
                int courseInstanceId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    IQueryable<InstructorCourse> checkExisting = db.InstructorCourses.Where(x => x.InstructorId == instructorId && x.CourseInstanceId == courseInstanceId);
                    if (!checkExisting.Any())
                    {


                        InstructorCourse instructorCourse = new InstructorCourse()
                        {
                            InstructorId = Convert.ToInt32(DropDownListInstructor.SelectedValue),
                            CourseInstanceId = Convert.ToInt32(DropDownListCourse.SelectedValue),
                            Active = CheckBoxContactInfoActive.Checked,
                            Role = TextBoxRole.Text.Trim()

                        };
                        db.InstructorCourses.Add(instructorCourse);
                        db.SaveChanges();

                        lblMessage.Text = "Save Successfully!";
                        lblErrorMessage.Text = "";

                    }
                    else
                    {
                        lblErrorMessage.Text = "The Course already exist!";
                        lblMessage.Text = "";
                    }
                }
                BindInstructorCourseGrid();
                clearText();
            }
        }
        private void clearText()
        {
            TextBoxContactInfo.Text = "";
            TextBoxRole.Text = "";
            CheckBoxContactInfoActive.Checked = true;
            CheckBoxPreferred.Checked = false;
            CheckBoxInstructorCourseActive.Checked = true;
        }
        private void clearAll()
        {
            DropDownListInstructor.SelectedIndex = -1;
            DropDownListCourse.Items.Clear();

            TextBoxInstructorName.Text = "";
            TextBoxContactInfo.Text = "";
            TextBoxRole.Text = "";

            CheckBoxPreferred.Checked = false;
            CheckBoxInstructorCourseActive.Checked = true;
            CheckBoxContactInfoActive.Checked = true;

            PanelInstructorContactInfo.Visible = false;

            btnAddInstructor.Attributes.Remove("disabled");
            btnAddInstructor.Style["pointer-events"] = "visible";
            //lblMessage.Text = "";
            GridView1.DataSource = null;
            GridView1.DataBind();

            GridView2.DataSource = null;
            GridView2.DataBind();
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
            string contactInfo = (row.FindControl("TextBox2") as TextBox).Text.Trim();
            bool preferred = (row.FindControl("CheckBox1") as CheckBox).Checked;
            bool active = (row.FindControl("CheckBox2") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                ContactInformation contactInformation = db.ContactInformations.Where(x => x.Id == id).FirstOrDefault();

                contactInformation.ContactInfo = contactInfo;
                contactInformation.Active = active;
                contactInformation.Preferred = preferred;

                db.SaveChanges();
            }
            GridView1.EditIndex = -1;
            BindGrid();
            lblMessage.Text = "Update Successfully!";
            lblErrorMessage.Text = "";
        }
        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                using (MaterialEntities db = new MaterialEntities())
                {
                    ContactInformation contactInformation = db.ContactInformations.Where(x => x.Id == id).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.ContactInformations.Remove(contactInformation);
                    db.SaveChanges();
                }
                BindGrid();
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

        private void BindGrid()
        {
            if (DropDownListInstructor.SelectedValue != "0")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int instructorId = Convert.ToInt32(DropDownListInstructor.SelectedValue);
                    var model = (from a in db.ContactInformations
                                 where a.InstructorId == instructorId
                                 select new { a.Id, a.ContactInfo, a.Active, a.Preferred }).ToList();
                    GridView1.DataSource = model.OrderBy(x => x.Id).ToList();
                    GridView1.DataBind();
                }
            }

        }
        private void BindInstructorCourseGrid()
        {
            if (DropDownListInstructor.SelectedValue != "0")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int instructorId = Convert.ToInt32(DropDownListInstructor.SelectedValue);
                    var model = (from a in db.InstructorCourses
                                 where a.InstructorId == instructorId
                                 select new { a.CourseInstanceId, Course = a.CourseInstance.Course.Name, a.Role, a.Active, }).ToList();
                    GridView2.DataSource = model.OrderBy(x => x.CourseInstanceId).ToList();
                    GridView2.DataBind();
                }
            }

        }

        #region--------------------Validation---------------------------
        protected bool InstructorValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(TextBoxInstructorName.Text))
            {
                fieldName += " Instructor Name -";
                result = false;
            }

            if (!result)
            {
                lblErrorMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessage.Text = "";
            }
            return result;
        }
        protected bool ContactInfoValidation()
        {
            bool result = true;
            string fieldName = "";
            if (DropDownListInstructor.SelectedValue == "0")
            {
                fieldName += " Instructor -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxContactInfo.Text))
            {
                fieldName += " Contact Information -";
                result = false;
            }

            if (!result)
            {
                lblErrorMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessage.Text = "";
            }
            return result;
        }
        protected bool CourseValidation()
        {
            bool result = true;
            string fieldName = "";
            if (DropDownListInstructor.SelectedValue == "0")
            {
                fieldName += " Instructor -";
                result = false;
            }
            if (DropDownListCourse.SelectedValue == "0")
            {
                fieldName += " Course Instance -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxRole.Text))
            {
                fieldName += " Role -";
                result = false;
            }

            if (!result)
            {
                lblErrorMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessage.Text = "";
            }
            return result;
        }
        #endregion
        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            clearAll();
            lblMessage.Text = "";
            lblErrorMessage.Text = "";
        }
        protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView2.EditIndex = -1;
            BindInstructorCourseGrid();
        }

        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView2.Rows[e.RowIndex];
                int id = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
                int instructorId = Convert.ToInt32(DropDownListInstructor.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    InstructorCourse instructorCourse = db.InstructorCourses.Where(x => x.CourseInstanceId == id && x.InstructorId == instructorId).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.InstructorCourses.Remove(instructorCourse);
                    db.SaveChanges();
                }
                BindInstructorCourseGrid();
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
                    lblErrorMessage.Text = ex.InnerException.Message;
                }
                lblMessage.Text = "";
            }
        }

        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView2.EditIndex = e.NewEditIndex;
            BindInstructorCourseGrid();
        }

        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView2.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
            string role = (row.FindControl("TextBox2") as TextBox).Text.Trim();
            bool active = (row.FindControl("CheckBox2") as CheckBox).Checked;

            int instructorId = Convert.ToInt32(DropDownListInstructor.SelectedValue);
            using (MaterialEntities db = new MaterialEntities())
            {
                InstructorCourse instructorCourse = db.InstructorCourses.Where(x => x.CourseInstanceId == id && x.InstructorId == instructorId).FirstOrDefault();

                instructorCourse.Role = role;
                instructorCourse.Active = active;

                db.SaveChanges();
            }
            GridView2.EditIndex = -1;
            BindInstructorCourseGrid();
            lblMessage.Text = "Update Successfully!";
            lblErrorMessage.Text = "";
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView2.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }
    }
}