using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddAnnouncement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                BindDropDownCourseInstance();
                BindGrid();
            }
        }
        private void BindDropDownCourseInstance()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //---------------Coruse Droup Down list----------------
                DropDownListCourseInstance.DataSource = db.CourseInstances.Where(ci => ci.Active).Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourseInstance.DataTextField = "Name";
                DropDownListCourseInstance.DataValueField = "Id";
                DropDownListCourseInstance.DataBind();
                DropDownListCourseInstance.Items.Insert(0, new ListItem("--Select Course Instance--", ""));
            }
        }
        protected void DropDownListCourseInstance_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }
        protected void btnAddAnnouncement_Click(object sender, EventArgs e)
        {
            if (AnnouncementValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                    int studentid = ((Student)Session["Student"]).StudentId;
                    Announcement announcement = new Announcement()
                    {
                        CourseInstanceId = courseInstanceId,
                        Title = TextBoxTitle.Text.Trim(),
                        Description = TextBoxDescription.Text.Trim(),
                        StudentId = studentid,
                        PublishedDate = DateTime.Now,
                        LastUpdateDate = DateTime.Now,
                        Active = CheckBoxActive.Checked
                    };
                    db.Announcements.Add(announcement);
                    db.SaveChanges();
                }
                ClearTextField();
                BindGrid();
                lblMessage.Text = "Save Successfully!";
                lblErrorMessage.Text = "";
            }
        }

        private void ClearTextField()
        {
            TextBoxTitle.Text = "";
            TextBoxDescription.Text = "";
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
            string title = (row.FindControl("TextBox2") as TextBox).Text;
            string description = (row.FindControl("TextBox1") as TextBox).Text;
            bool active = (row.FindControl("CheckBox2") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                Announcement announcement = db.Announcements.Find(id);
                announcement.Description = description;
                announcement.Title = title;
                announcement.Active = active;
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
            try
            {
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);

                GridViewRow row = GridView1.Rows[e.RowIndex];
                using (MaterialEntities db = new MaterialEntities())
                {
                    Announcement announcement = db.Announcements.Find(id);
                    //TODO: Make sure this is correct
                    db.Announcements.Remove(announcement);
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
            using (MaterialEntities db = new MaterialEntities())
            {
                IQueryable<Announcement> model = from a in db.Announcements select a;
                if (DropDownListCourseInstance.SelectedValue != "")
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                    model = from a in model
                            where a.CourseInstanceId == courseInstanceId
                            select a;
                }
                var gridResult = (from a in model
                                  select new { a.Id, CourseInstance = a.CourseInstance.Course.Name, a.Title, a.Description, PublishedBy = a.Student.Name, a.PublishedDate, a.Active }).ToList();
                GridView1.DataSource = gridResult;
                GridView1.DataBind();
            }
        }

        protected void DropDownListCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }
        #region--------------------Validation---------------------------
        protected bool AnnouncementValidation()
        {
            bool result = true;
            string fieldName = "";

            if (DropDownListCourseInstance.SelectedValue == "")
            {
                fieldName += " Course Instance -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxTitle.Text))
            {
                fieldName += " Title -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxDescription.Text))
            {
                fieldName += " Description -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblErrorMessage.Text = "";
            }
            return result;
        }
        #endregion

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            ClearTextField();
            DropDownListCourseInstance.SelectedIndex = -1;
            BindGrid();
        }
       
    }
}