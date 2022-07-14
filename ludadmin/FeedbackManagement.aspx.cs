using AdminPages;
using AdminPages.Services;
using EFModel;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class FeedbackManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindGrid();
                BindDropDownCourse();
            }
        }
        private void BindDropDownCourse()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var model = (from a in db.Feedbacks
                             select a);
                if (RadioButtonPending.Checked)
                {
                    model = from a in model where a.Status == null select a;
                }
                if (RadioButtonDone.Checked)
                {
                    model = from a in model where a.Status != null select a;
                }
                var result = (from a in model
                             join b in db.Courses on a.CourseInstance.CourseId equals b.Id
                             select new { b.Id, b.Name });

                DropDownListCourseFilter2.DataSource = result.Distinct().OrderBy(x => x.Id).ToList();
                DropDownListCourseFilter2.DataTextField = "Name";
                DropDownListCourseFilter2.DataValueField = "Id";
                DropDownListCourseFilter2.DataBind();
                DropDownListCourseFilter2.Items.Insert(0, new ListItem("--Select Course--", ""));
            }
        }
        protected void DropDownListCourseFilter2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string i = DropDownListCourseFilter2.SelectedValue;
            DropDownListCourseInstanceFilter.Items.Clear();
            DropDownListStudent.Items.Clear();
            GridView1.DataSource = null;
            GridView1.DataBind();

            if (i != "")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseId = Convert.ToInt32(i);
                    //IQueryable<CourseInstance> courseIns = db.CourseInstances.Where(ci => ci.Course.Id == courseId);
                    var model = (from a in db.Feedbacks
                                 select a);
                    if (RadioButtonPending.Checked)
                    {
                        model = from a in model where a.Status == null select a;
                    }
                    if (RadioButtonDone.Checked)
                    {
                        model = from a in model where a.Status != null select a;
                    }
                    IQueryable<CourseInstance> courseIns = (from a in model
                                                            join b in db.CourseInstances on a.CourseInstanceId equals b.Id
                                                            where a.CourseInstance.Course.Id == courseId
                                                            select b).Distinct();
                    
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
                        BindDropDownStudent();
                        BindGrid();
                        ClearDetail();
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
                BindDropDownStudent();
                BindGrid();
                ClearDetail();
            }
        }
        protected void BindDropDownListsCourseInstanceFilter(int courseInstanceId)
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var list = db.CourseInstances.Where(ci => ci.Id == courseInstanceId ).Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
                //---------------Coruse Instance Droup Down list----------------
                DropDownListCourseInstanceFilter.DataSource = list;
                DropDownListCourseInstanceFilter.DataTextField = "Name";
                DropDownListCourseInstanceFilter.DataValueField = "Id";
                DropDownListCourseInstanceFilter.DataBind();
            }
        }
        //private void BindDropDownCourseInstance()
        //{
        //    using (MaterialEntities db = new MaterialEntities())
        //    {
        //        var model = (from a in db.CourseInstances
        //                     join b in db.Feedbacks on a.Id equals b.CourseInstanceId
        //                     select new { a.Id, a.Course.Name });

        //        //---------------Coruse Droup Down list----------------
        //        DropDownListCourseInstance.DataSource = model.Distinct().ToList();
        //        DropDownListCourseInstance.DataTextField = "Name";
        //        DropDownListCourseInstance.DataValueField = "Id";
        //        DropDownListCourseInstance.DataBind();
        //        DropDownListCourseInstance.Items.Insert(0, new ListItem("--Select Course Instance--", ""));
        //    }
        //}
        private void BindDropDownStudent()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                var model = (from a in db.Feedbacks
                             where a.CourseInstanceId == courseInstanceId
                             select new { a.StudentId, a.Student.Name });

                //---------------Coruse Droup Down list----------------
                DropDownListStudent.DataSource = model.Distinct().ToList();
                DropDownListStudent.DataTextField = "Name";
                DropDownListStudent.DataValueField = "StudentId";
                DropDownListStudent.DataBind();
                DropDownListStudent.Items.Insert(0, new ListItem("--Select Student--", ""));
            }
        }
        private void BindDropDownStatus()
        {
            //---------------Coruse Droup Down list----------------
            //DropDownListStatus.Items.Insert(0, new ListItem("Low", "low"));
            //DropDownListStatus.Items.Insert(0, new ListItem("Medium", "medium"));
            //DropDownListStatus.Items.Insert(0, new ListItem("High", "high"));
            //DropDownListStatus.Items.Insert(0, new ListItem("--Select--", ""));

        }
        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                IQueryable<Feedback> model = from a in db.Feedbacks select a;
                if (DropDownListCourseInstanceFilter.SelectedValue != "")
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    model = from a in model where a.CourseInstanceId == courseInstanceId select a;
                }
                var dd = model.ToList();
                if (DropDownListStudent.SelectedValue != "")
                {
                    int studentId = Convert.ToInt32(DropDownListStudent.SelectedValue);
                    model = from a in model where a.StudentId == studentId select a;
                }
                if (RadioButtonPending.Checked)
                {
                    model = from a in model where a.Status == null select a;
                }
                if (RadioButtonDone.Checked)
                {
                    model = from a in model where a.Status != null select a;
                }
                int studentid = ((Student)Session["Student"]).StudentId;
                
                GridView1.DataSource = (from a in model
                                        select new
                                        {
                                            a.Id,
                                            StudentName = a.Student.Name,
                                            a.TimeStamp,
                                            a.Text,
                                            a.Status,
                                            CourseInstance = a.CourseInstance.Course.Name,
                                            a.Comment
                                        }).OrderBy(x => x.Id).ToList();
                GridView1.DataBind();
            }
        }
        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ShowDetail")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    int id = Convert.ToInt32(GridView1.DataKeys[rowIndex].Values[0]);
                    SupportTicket supportTicket = db.SupportTickets.Find(id);
                    TicketDetailArea.Visible = true;
                    lblCourseInstance.Text = supportTicket.CourseInstance.Course.Name;
                    lblTitle.Text = supportTicket.Title;
                    lblTokenNo.Text = supportTicket.TokenNo.ToString();
                    lblPriority.Text = supportTicket.Priority;
                    lblOpenStatus.Text = supportTicket.OpenStatus ? "Open" : "Closed";
                    lblOpenDate.Text = supportTicket.OpenedDate.ToString("dd MMM, yyyy");
                    lblFeedbackId.Text = Convert.ToString(supportTicket.Id);
                    lblPriority.Text = supportTicket.Priority;
                    if (supportTicket.OpenStatus)
                    {
                        btnCloseTicket.Visible = true;
                    }
                    else
                    {
                        btnCloseTicket.Visible = false;
                    }
                    int studentid = ((Student)Session["Student"]).StudentId;
                    IQueryable<SupportTicketMessage> SupportTicketMessages = db.SupportTicketMessages.Where(x => x.SupportTicketId == id);
                    foreach (SupportTicketMessage message in SupportTicketMessages)
                    {
                        if (message.StudentId != studentid && !message.ViewStatus)
                        {
                            message.ViewStatus = true;
                        }
                        //----------------------------
                        SupportTicketDetailControl control = (SupportTicketDetailControl)Page.LoadControl("SupportTicketDetailControl.ascx");
                        control.SupportTicketMessage = message;
                        control.Student = db.Students.Find(message.StudentId);
                        pnlFeedbackDetail.Controls.Add(control);
                    }
                    db.SaveChanges();
                    BindGrid();
                }
            }
        }
        protected void btnCloseTicket_Click(object sender, EventArgs e)
        {
            if (lblFeedbackId.Text != "")
            {
                try
                {
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        int id = Convert.ToInt32(lblFeedbackId.Text);
                        SupportTicket ticket = db.SupportTickets.Find(id);
                        ticket.OpenStatus = false;
                        db.SaveChanges();
                        //------------------------------
                        BindGrid();
                        btnCloseTicket.Visible = false;

                        IQueryable<SupportTicketMessage> SupportTicketMessages = db.SupportTicketMessages.Where(x => x.SupportTicketId == id);
                        foreach (SupportTicketMessage message in SupportTicketMessages)
                        {
                            SupportTicketDetailControl control = (SupportTicketDetailControl)Page.LoadControl("SupportTicketDetailControl.ascx");
                            control.SupportTicketMessage = message;
                            control.Student = db.Students.Find(message.StudentId);
                            pnlFeedbackDetail.Controls.Add(control);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        protected void btnSubmitMessage_Click(object sender, EventArgs e)
        {
            if (lblFeedbackId.Text != "" && txtMessage.Text.Trim() != "")
            {
                try
                {
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        int id = Convert.ToInt32(lblFeedbackId.Text);
                        int studentid = ((Student)Session["Student"]).StudentId;
                        byte[] imageBytes = new ImageResizingService().ImageBytes(fileUploadImage.PostedFile.InputStream);
                        SupportTicketMessage sm = new SupportTicketMessage
                        {
                            Message = txtMessage.Text.Trim(),
                            StudentId = studentid,
                            SupportTicketId = id,
                            Role = "Instructor",
                            Active = true,
                            ViewStatus = false,
                        };
                        if (imageBytes.Length > 0)
                        {
                            sm.Image = imageBytes;
                        }
                        db.SupportTicketMessages.Add(sm);
                        db.SaveChanges();
                        //----------------------------------------
                        txtMessage.Text = "";
                        fileUploadImage.PostedFile.InputStream.Close();
                        IQueryable<SupportTicketMessage> SupportTicketMessages = db.SupportTicketMessages.Where(x => x.SupportTicketId == id);
                        foreach (SupportTicketMessage message in SupportTicketMessages)
                        {
                            SupportTicketDetailControl control = (SupportTicketDetailControl)Page.LoadControl("SupportTicketDetailControl.ascx");
                            control.SupportTicketMessage = message;
                            control.Student = db.Students.Find(message.StudentId);
                            pnlFeedbackDetail.Controls.Add(control);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

        }

        //protected void DropDownListCourseInstance_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (DropDownListCourseInstanceFilter.SelectedValue !="") {
        //        BindGrid();
        //        BindDropDownStudent();
        //        ClearDetail();
        //    }
        //}

        protected void DropDownListStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
            ClearDetail();
        }

  
        private void ClearDetail()
        {
            lblFeedbackId.Text = "";
            lblTitle.Text = "";
            lblCourseInstance.Text = "";
            lblTokenNo.Text = "";
            lblPriority.Text = "";
            lblOpenStatus.Text = "";
            lblOpenDate.Text = "";
            TicketDetailArea.Visible = false;
        }

        //protected void DropDownListPriority_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindGrid();
        //    ClearDetail();
        //}

        protected void ShowAllList_Click(object sender, EventArgs e)
        {
            //DropDownListCourseInstanceFilter.SelectedIndex = -1;
            //DropDownListStudent.SelectedIndex = -1;
            //DropDownListStatus.SelectedIndex = -1;
            //RadioButtonClose.Checked = false;
            //RadioButtonOpen.Checked = true;
            //BindGrid();
            clearAll();
        }
        private void clearAll()
        {
            BindDropDownCourse();
            DropDownListQuarterFilter2.Items.Clear();
            DropDownListCourseInstanceFilter.Items.Clear();
            GridView1.DataSource = null;
            GridView1.DataBind();
            DropDownListStudent.Items.Clear();
            lblMessage.Text = "";
        }

        protected void RadioButtonPending_CheckedChanged(object sender, EventArgs e)
        {
            
            BindDropDownCourse();
            ClearDetail();
            if (DropDownListCourseInstanceFilter.SelectedValue != "") {
                BindGrid();
            }
        }

        protected void RadioButtonDone_CheckedChanged(object sender, EventArgs e)
        {
            BindDropDownCourse();
            ClearDetail();
            if (DropDownListCourseInstanceFilter.SelectedValue != "")
            {
                BindGrid();
            }
        }

        protected void RadioButtonAll_CheckedChanged(object sender, EventArgs e)
        {
            BindDropDownCourse();
            ClearDetail();
            if (DropDownListCourseInstanceFilter.SelectedValue != "")
            {
                BindGrid();
            }
        }
    }
}