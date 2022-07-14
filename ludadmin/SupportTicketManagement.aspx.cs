using AdminPages;
using AdminPages.Services;
using EFModel;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class SupportTicketManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
                BindDropDownCourseInstance();
                BindDropDownStudent();
                BindDropDownPriority();
            }
        }
        private void BindDropDownCourseInstance()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var model = (from a in db.CourseInstances
                             join b in db.SupportTickets on a.Id equals b.CourseInstanceId
                             select new { a.Id, a.Course.Name });

                //---------------Coruse Droup Down list----------------
                DropDownListCourseInstance.DataSource = model.Distinct().ToList();
                DropDownListCourseInstance.DataTextField = "Name";
                DropDownListCourseInstance.DataValueField = "Id";
                DropDownListCourseInstance.DataBind();
                DropDownListCourseInstance.Items.Insert(0, new ListItem("--Select Course Instance--", ""));
            }
        }
        private void BindDropDownStudent()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var model = (from a in db.Students
                             join b in db.SupportTickets on a.StudentId equals b.StudentId
                             select new { a.StudentId, a.Name });

                //---------------Coruse Droup Down list----------------
                DropDownListStudent.DataSource = model.Distinct().ToList();
                DropDownListStudent.DataTextField = "Name";
                DropDownListStudent.DataValueField = "StudentId";
                DropDownListStudent.DataBind();
                DropDownListStudent.Items.Insert(0, new ListItem("--Select Student--", ""));
            }
        }
        private void BindDropDownPriority()
        {
            //---------------Coruse Droup Down list----------------
            DropDownListPriority.Items.Insert(0, new ListItem("Low", "low"));
            DropDownListPriority.Items.Insert(0, new ListItem("Medium", "medium"));
            DropDownListPriority.Items.Insert(0, new ListItem("High", "high"));
            DropDownListPriority.Items.Insert(0, new ListItem("--Select--", ""));

        }
        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                IQueryable<SupportTicket> model = from a in db.SupportTickets select a;
                if (DropDownListPriority.SelectedValue != "")
                {
                    string prioriy = Convert.ToString(DropDownListPriority.SelectedValue);
                    model = from a in model where a.Priority == prioriy select a;
                }
                if (DropDownListCourseInstance.SelectedValue != "")
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                    model = from a in model where a.CourseInstanceId == courseInstanceId select a;
                }
                if (DropDownListStudent.SelectedValue != "")
                {
                    int studentId = Convert.ToInt32(DropDownListStudent.SelectedValue);
                    model = from a in model where a.StudentId == studentId select a;
                }
                if (RadioButtonOpen.Checked)
                {
                    model = from a in model where a.OpenStatus select a;
                }
                if (RadioButtonClose.Checked)
                {
                    model = from a in model where !a.OpenStatus select a;
                }
                int studentid = ((Student)Session["Student"]).StudentId;
                GridView1.DataSource = (from a in model
                                        select new
                                        {
                                            a.Id,
                                            TokenNo = a.TokenNo,
                                            StudentName = a.Student.Name,
                                            a.Title,
                                            a.Priority,
                                            OpenedDate = a.OpenedDate,
                                            OpeneStatus = a.OpenStatus ? "Open" : "Closed",
                                            CourseInstance = a.CourseInstance.Course.Name,
                                            UnreadMessage = a.SupportTicketMessages.Where(x => x.StudentId != studentid && !x.ViewStatus).Count()
                                        }).OrderBy(x => x.Id).ToList();
                GridView1.DataBind();
                System.Collections.Generic.List<SupportTicket> xx = model.ToList();
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
                    lblSupportTicketId.Text = Convert.ToString(supportTicket.Id);
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
                        pnlSupportTicketDetail.Controls.Add(control);
                    }
                    db.SaveChanges();
                    BindGrid();
                }
            }
        }
        protected void btnCloseTicket_Click(object sender, EventArgs e)
        {
            if (lblSupportTicketId.Text != "")
            {
                try
                {
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        int id = Convert.ToInt32(lblSupportTicketId.Text);
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
                            pnlSupportTicketDetail.Controls.Add(control);
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
            if (lblSupportTicketId.Text != "" && txtMessage.Text.Trim() != "")
            {
                try
                {
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        int id = Convert.ToInt32(lblSupportTicketId.Text);
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
                            pnlSupportTicketDetail.Controls.Add(control);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

        }

        protected void DropDownListCourseInstance_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
            ClearDetail();
        }

        protected void DropDownListStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
            ClearDetail();
        }

        protected void RadioButtonOpen_CheckedChanged(object sender, EventArgs e)
        {
            BindGrid();
            ClearDetail();
        }

        protected void RadioButtonClose_CheckedChanged(object sender, EventArgs e)
        {
            BindGrid();
            ClearDetail();
        }
        private void ClearDetail()
        {
            lblSupportTicketId.Text = "";
            lblTitle.Text = "";
            lblCourseInstance.Text = "";
            lblTokenNo.Text = "";
            lblPriority.Text = "";
            lblOpenStatus.Text = "";
            lblOpenDate.Text = "";
            TicketDetailArea.Visible = false;
        }

        protected void DropDownListPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
            ClearDetail();
        }

        protected void ShowAllList_Click(object sender, EventArgs e)
        {
            DropDownListCourseInstance.SelectedIndex = -1;
            DropDownListStudent.SelectedIndex = -1;
            DropDownListPriority.SelectedIndex = -1;
            RadioButtonClose.Checked = false;
            RadioButtonOpen.Checked = true;
            BindGrid();
        }
    }
}