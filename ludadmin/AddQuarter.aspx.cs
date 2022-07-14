using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddQuarter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                BindGridSession();
                BindGridQuarter();

                BindDropDownDays();
                BindDropDownSchool();

                BindDropDownSession();
                BindCourseInstanceDropDown();
            }
        }

        private void BindDropDownDays()
        {
            Array days = Enum.GetValues(typeof(Days));
            foreach (Days day in days)
            {
                DropDownListLectureDay.Items.Add(new ListItem(day.ToString(), ((int)day).ToString()));
            }
            DropDownListLectureDay.Items.Insert(0, new ListItem("--Select One--", ""));
        }

        private void BindDropDownSchool()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                DropDownListSchool.DataSource = db.Schools.OrderBy(x => x.SchoolId).ToList();
                DropDownListSchool.DataTextField = "Name";
                DropDownListSchool.DataValueField = "SchoolId";
                DropDownListSchool.DataBind();
                DropDownListSchool.Items.Insert(0, new ListItem("--Select One--", ""));
            }
        }

        private void BindCourseInstanceDropDown()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                DropDownListCourseInstance.DataSource = db.CourseInstances.Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourseInstance.DataTextField = "Name";
                DropDownListCourseInstance.DataValueField = "Id";
                DropDownListCourseInstance.DataBind();
                DropDownListCourseInstance.Items.Insert(0, new ListItem("--Select One--", ""));
            }
        }
        private void BindDropDownSession()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var model = (from a in db.Sessions
                             select new { SessionName = a.LectureDay + " - " + a.StartTime.Hours + ":" + a.StartTime.Minutes + " TO " + a.EndTime.Hours + ":" + a.EndTime.Minutes, SessionId = a.SessionId }).ToList();

                DropDownListSession.DataSource = model.OrderBy(x => x.SessionId);
                DropDownListSession.DataTextField = "SessionName";
                DropDownListSession.DataValueField = "SessionId";
                DropDownListSession.DataBind();
                DropDownListSession.Items.Insert(0, new ListItem("--Select One--", ""));
            }
        }
        //--------------------Validation---------------------------
        protected bool QuarterValidation()
        {
            bool result = true;
            string fieldName = "";

            if (string.IsNullOrWhiteSpace(DropDownListSchool.SelectedValue))
            {
                fieldName += " School -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxName.Text))
            {
                fieldName += " Name -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxStartDate.Text))
            {
                fieldName += " Start Date -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxEndDate.Text))
            {
                fieldName += " End Date -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxWithdrawDate.Text))
            {
                fieldName += " Withdraw Date -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessageQuarter.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessageQuarter.Text = "";
            }
            return result;
        }
        protected bool SessionValidation()
        {
            bool result = true;
            string fieldName = "";

            if (string.IsNullOrWhiteSpace(DropDownListLectureDay.SelectedValue))
            {
                fieldName += " Lecture Day -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxStartTime.Text))
            {
                fieldName += " Start Time -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxEndTime.Text))
            {
                fieldName += " End Time -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxLocation.Text))
            {
                fieldName += " Location -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxLocationDescription.Text))
            {
                fieldName += " Description -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessageSession.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessageSession.Text = "";
            }
            return result;
        }
        protected bool CourseInstanceSessionValidation()
        {
            bool result = true;
            string fieldName = "";

            if (string.IsNullOrWhiteSpace(DropDownListCourseInstance.SelectedValue))
            {
                fieldName += " Course Instance -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListSession.SelectedValue))
            {
                fieldName += " Session -";
                result = false;
            }

            if (!result)
            {
                lblErrorMessageSession.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessageSession.Text = "";
            }
            return result;
        }
        //----------------------------------------------------------
        private enum Days
        {
            Monday = 1,
            Tuesday = 2,
            Wednesday = 3,
            Thursday = 4,
            Friday = 5,
            Saturday = 6,
            Sunday = 7
        }
        //=========================Quarter===================================
        protected void btnAddQuarter_Click(object sender, EventArgs e)
        {
            if (QuarterValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    Quarter quarter = new Quarter()
                    {
                        SchoolId = Convert.ToInt32(DropDownListSchool.SelectedValue),
                        StartDate = Convert.ToDateTime(TextBoxStartDate.Text),
                        EndDate = Convert.ToDateTime(TextBoxEndDate.Text),
                        WithdrawDate = Convert.ToDateTime(TextBoxWithdrawDate.Text),
                        Active = CheckBoxActive.Checked,
                        Name = TextBoxName.Text.Trim()
                    };
                    db.Quarters.Add(quarter);
                    db.SaveChanges();

                    lblMessageQuarter.Text = "Save Successfully!";
                    lblErrorMessageQuarter.Text = "";
                }
                BindGridQuarter();

                clearQuarter();
            }
        }
        private void clearAll()
        {
            BindDropDownSchool();
            BindDropDownSession();
            BindCourseInstanceDropDown();

            //DropDownListQuarter.SelectedIndex = -1;
            DropDownListSchool.SelectedIndex = -1;
            DropDownListCourseInstance.SelectedIndex = -1;
            DropDownListSession.SelectedIndex = -1;
            TextBoxStartDate.Text = "";
            TextBoxEndDate.Text = "";
            TextBoxWithdrawDate.Text = "";
            CheckBoxQuarterActive.Checked = true;

            CheckBoxActive.Checked = true;

            DropDownListLectureDay.SelectedIndex = -1;
            TextBoxStartTime.Text = "";
            TextBoxEndTime.Text = "";
            TextBoxLocation.Text = "";
            TextBoxLocationDescription.Text = "";

            lblMessageQuarter.Text = "";
            lblErrorMessageQuarter.Text = "";
            lblMessageSession.Text = "";
            lblErrorMessageSession.Text = "";
            //PanelCourseInstance.Visible = false;
            //PanelSession.Visible = false;
            GridViewCourseInstanceSession.DataSource = null;
            GridViewCourseInstanceSession.DataBind();
        }

        private void clearQuarter()
        {
            DropDownListSchool.SelectedIndex = -1;

            TextBoxStartDate.Text = "";
            TextBoxEndDate.Text = "";
            TextBoxWithdrawDate.Text = "";
            CheckBoxQuarterActive.Checked = true;
        }
        private void clearSession()
        {
            //BindDropDownSession();

            DropDownListLectureDay.SelectedIndex = -1;
            TextBoxStartTime.Text = "";
            TextBoxEndTime.Text = "";
            TextBoxLocation.Text = "";
            TextBoxLocationDescription.Text = "";

        }
        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            clearAll();
        }
        //=============================Course Instance=========================
        protected void DropDownListCourseInstance_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCourseInstance.SelectedValue != "")
            {
                BindGridCourseInstanceSession();
            }
        }

        //=============================Course Session=========================
        protected void btnAddSession_Click(object sender, EventArgs e)
        {
            if (SessionValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    Session session = new Session()
                    {
                        LectureDay = DropDownListLectureDay.SelectedItem.Text,
                        StartTime = Convert.ToDateTime(TextBoxStartTime.Text).TimeOfDay,
                        EndTime = Convert.ToDateTime(TextBoxEndTime.Text).TimeOfDay,
                        Location = TextBoxLocation.Text.Trim(),
                        Description = TextBoxLocationDescription.Text.Trim(),
                        Active = CheckBoxSessionActive.Checked
                    };
                    db.Sessions.Add(session);
                    db.SaveChanges();

                    lblMessageSession.Text = "Save Successfully!";
                    lblErrorMessageSession.Text = "";
                }
                BindDropDownSession();
                BindGridSession();
                clearSession();
            }
        }

        private void BindGridSession()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                GridViewSession.DataSource = db.Sessions.OrderBy(x => x.SessionId).ToList();
                GridViewSession.DataBind();
            }
        }

        protected void GridViewSession_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewSession.EditIndex = e.NewEditIndex;
            BindGridSession();
        }

        protected void GridViewSession_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewSession.EditIndex = -1;
            BindGridSession();
        }

        protected void GridViewSession_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridViewSession.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridViewSession.DataKeys[e.RowIndex].Values[0]);

            string lectureDay = (row.FindControl("TextBox1") as TextBox).Text;
            TimeSpan startTime = Convert.ToDateTime((row.FindControl("TextBox2") as TextBox).Text).TimeOfDay;
            TimeSpan endTime = Convert.ToDateTime((row.FindControl("TextBox3") as TextBox).Text).TimeOfDay;
            string location = (row.FindControl("TextBox4") as TextBox).Text;
            string description = (row.FindControl("TextBox5") as TextBox).Text;

            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                Session session = db.Sessions.Where(x => x.SessionId == id).FirstOrDefault();

                session.LectureDay = lectureDay;
                session.StartTime = startTime;
                session.EndTime = endTime;
                session.Location = location;
                session.Active = active;
                session.Description = description;

                db.SaveChanges();
            }

            GridViewSession.EditIndex = -1;
            BindGridSession();
            BindDropDownSession();
            lblMessageSession.Text = "Update Successfully!";
            lblErrorMessageSession.Text = "";
        }

        protected void GridViewSession_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GridViewSession.DataKeys[e.RowIndex].Values[0]);
                using (MaterialEntities db = new MaterialEntities())
                {
                    Session session = db.Sessions.Where(x => x.SessionId == id).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.Sessions.Remove(session);
                    db.SaveChanges();
                }
                BindGridSession();
                BindDropDownSession();
                lblMessageSession.Text = "Delete Successfully!";
                lblErrorMessageSession.Text = "";
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    lblErrorMessageSession.Text = ex.Message;
                }
                else
                {
                    lblErrorMessageSession.Text = ex.InnerException.InnerException.Message;
                }
                lblMessageSession.Text = "";
            }
        }

        protected void GridViewSession_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridViewSession.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }
        //---------------------------------Quarter Grid view-------------------------
        private void BindGridQuarter()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var model = (from a in db.Quarters
                             select new { a.QuarterId, a.SchoolId, School = a.School.Name, a.StartDate, a.EndDate, a.WithdrawDate, a.Active, a.Name }).ToList();
                GridViewQuarter.DataSource = model.OrderBy(x => x.QuarterId);
                GridViewQuarter.DataBind();
            }
        }
        protected void GridViewQuarter_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewQuarter.EditIndex = -1;
            BindGridQuarter();
        }

        protected void GridViewQuarter_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewQuarter.EditIndex = e.NewEditIndex;
            BindGridQuarter();
        }

        protected void GridViewQuarter_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridViewQuarter.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridViewQuarter.DataKeys[e.RowIndex].Values[0]);

            DateTime startDate = Convert.ToDateTime((row.FindControl("TextBox1") as TextBox).Text);
            DateTime endDate = Convert.ToDateTime((row.FindControl("TextBox2") as TextBox).Text);

            DateTime withdrawDate = Convert.ToDateTime((row.FindControl("TextBox3") as TextBox).Text);
            int schoolId = Convert.ToInt32((row.FindControl("DropDownListSchoolGV") as DropDownList).SelectedValue);
            string name = (row.FindControl("TextBox4") as TextBox).Text.Trim();
            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                Quarter quarter = db.Quarters.Where(x => x.QuarterId == id).FirstOrDefault();
                quarter.SchoolId = schoolId;
                quarter.StartDate = startDate;
                quarter.EndDate = endDate;
                quarter.WithdrawDate = withdrawDate;
                quarter.Active = active;
                quarter.Name = name;
                db.SaveChanges();
            }

            GridViewQuarter.EditIndex = -1;
            BindGridQuarter();

            lblMessageQuarter.Text = "Update Successfully!";
            lblErrorMessageQuarter.Text = "";
        }

        protected void GridViewQuarter_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GridViewQuarter.DataKeys[e.RowIndex].Values[0]);
                using (MaterialEntities db = new MaterialEntities())
                {
                    Quarter quarter = db.Quarters.Where(x => x.QuarterId == id).FirstOrDefault();
                    //TODO: Make sure this is correct

                    db.Quarters.Remove(quarter);
                    db.SaveChanges();
                }
                BindGridQuarter();
                lblMessageQuarter.Text = "Delete Successfully!";
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    lblErrorMessageQuarter.Text = ex.Message;
                }
                else
                {
                    lblErrorMessageQuarter.Text = ex.InnerException.InnerException.Message;
                }
                lblMessageQuarter.Text = "";
            }

        }

        protected void GridViewQuarter_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == GridViewQuarter.EditIndex)
            {
                string SchoolId = (e.Row.FindControl("Label5") as Label).Text;
                DropDownList schoolList = e.Row.FindControl("DropDownListSchoolGV") as DropDownList;

                using (MaterialEntities db = new MaterialEntities())
                {
                    schoolList.DataSource = db.Schools.OrderBy(x => x.SchoolId).ToList();
                    schoolList.DataTextField = "Name";
                    schoolList.DataValueField = "SchoolId";
                    schoolList.DataBind();
                    schoolList.Items.Insert(0, new ListItem("--Select One--", ""));
                }
                schoolList.SelectedValue = SchoolId;
            }
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridViewQuarter.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }
        //--------------------------Course Instance Session-------------------------------
        private void BindGridCourseInstanceSession()
        {
            if (DropDownListCourseInstance.SelectedValue != "")
            {
                int courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    var model = (from a in db.CourseInstanceSessions.Where(x => x.CourseInstanceId == courseInstanceId)
                                 select new { a.SessionId, a.Session.LectureDay, a.Session.StartTime, a.Session.EndTime, a.Session.Location, a.Active }).ToList();
                    GridViewCourseInstanceSession.DataSource = model.OrderBy(x => x.SessionId);
                    GridViewCourseInstanceSession.DataBind();
                }
            }
        }

        protected void btnAddCourseInstanceSession_Click(object sender, EventArgs e)
        {
            if (CourseInstanceSessionValidation())
            {

                using (MaterialEntities db = new MaterialEntities())
                {
                    int CourseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                    int SessionId = Convert.ToInt32(DropDownListSession.SelectedValue);

                    if (!db.CourseInstanceSessions.Where(x => x.CourseInstanceId == CourseInstanceId && x.SessionId == SessionId).Any())
                    {
                        CourseInstanceSession courseInsSession = new CourseInstanceSession()
                        {
                            CourseInstanceId = CourseInstanceId,
                            SessionId = SessionId,
                            Active = CheckBoxActive.Checked
                        };
                        db.CourseInstanceSessions.Add(courseInsSession);
                        db.SaveChanges();
                    }
                    else
                    {
                        lblErrorMessageSession.Text = "Already exist!";
                        lblMessageSession.Text = "";
                    }
                }

                BindGridCourseInstanceSession();
                DropDownListSession.SelectedIndex = -1;
            }

        }

        protected void GridViewCourseInstanceSession_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewCourseInstanceSession.EditIndex = e.NewEditIndex;
            BindGridCourseInstanceSession();
        }

        protected void GridViewCourseInstanceSession_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewCourseInstanceSession.EditIndex = -1;
            BindGridCourseInstanceSession();
        }

        protected void GridViewCourseInstanceSession_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridViewCourseInstanceSession.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridViewCourseInstanceSession.DataKeys[e.RowIndex].Values[0]);

            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;
            int courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
            using (MaterialEntities db = new MaterialEntities())
            {
                CourseInstanceSession cis = db.CourseInstanceSessions.Where(x => x.SessionId == id && x.CourseInstanceId == courseInstanceId).FirstOrDefault();
                cis.Active = active;
                db.SaveChanges();
            }

            GridViewCourseInstanceSession.EditIndex = -1;
            BindGridCourseInstanceSession();
            lblMessageSession.Text = "Update Successfully!";
            lblErrorMessageSession.Text = "";
        }

        protected void GridViewCourseInstanceSession_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GridViewCourseInstanceSession.DataKeys[e.RowIndex].Values[0]);
                int courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    CourseInstanceSession cis = db.CourseInstanceSessions.Where(x => x.SessionId == id && x.CourseInstanceId == courseInstanceId).FirstOrDefault();

                    //TODO: Make sure this is correct
                    db.CourseInstanceSessions.Remove(cis);
                    db.SaveChanges();
                }
                BindGridCourseInstanceSession();
                lblMessageSession.Text = "Delete Successfully!";
                lblErrorMessageSession.Text = "";
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    lblErrorMessageSession.Text = ex.Message;
                }
                else
                {
                    lblErrorMessageSession.Text = ex.InnerException.InnerException.Message;
                }
                lblMessageSession.Text = "";
            }
        }

        protected void GridViewCourseInstanceSession_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridViewCourseInstanceSession.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }
    }
}