using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddTextBook : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridTextBook();
                BindDropDownCourse();
                BindDropDownTextBook();
                BindGridCourseTextBook();
            }
        }
        private void BindDropDownCourse()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                DropDownListCourse.DataSource = db.Courses.Select(x => new { x.Id, x.Name }).OrderBy(x => x.Id).ToList(); ;
                DropDownListCourse.DataTextField = "Name";
                DropDownListCourse.DataValueField = "Id";
                DropDownListCourse.DataBind();
                DropDownListCourse.Items.Insert(0, new ListItem("--Select One--", ""));
                //----------------------Prerequisite-----------------------------------
            }
        }
        private void BindDropDownTextBook()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                System.Collections.Generic.List<Textbook> model = db.Textbooks.OrderBy(x => x.TextbookId).ToList();
                DropDownListTextBook.DataSource = model;
                DropDownListTextBook.DataTextField = "Description";
                DropDownListTextBook.DataValueField = "TextbookId";
                DropDownListTextBook.DataBind();
                DropDownListTextBook.Items.Insert(0, new ListItem("--Select One--", ""));
                //----------------------Prerequisite-----------------------------------
            }
        }
        private void BindGridTextBook()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //var courseInstance = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                var model = from a in db.Textbooks
                            select new { a.TextbookId, a.Description, a.Active };

                GridView1.DataSource = model.ToList();
                GridView1.DataBind();
            }
        }
        private void BindGridCourseTextBook()
        {

            using (MaterialEntities db = new MaterialEntities())
            {
                IQueryable<CourseTextbook> model = from a in db.CourseTextbooks select a;

                if (DropDownListCourse.SelectedValue != "")
                {
                    int courseId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                    model = from a in model
                            where a.CourseId == courseId
                            select a;
                }

                var result = (from a in model
                              select new { a.CourseId, Course = a.Course.Name, a.TextbookId, a.Textbook.Description, a.Active }).ToList();

                GridView2.DataSource = result;
                GridView2.DataBind();
            }
        }
        protected void btnAddTextBook_Click(object sender, EventArgs e)
        {
            if (TextBookValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    Textbook textBook = new Textbook()
                    {
                        Description = TextBoxDescription.Text.Trim(),
                        Active = CheckBoxActive.Checked
                    };
                    db.Textbooks.Add(textBook);
                    db.SaveChanges();
                    lblMessage.Text = "Save Successfully!";
                    lblErrorMessage.Text = "";
                }
                BindDropDownTextBook();
                BindGridTextBook();
                CheckBoxActive.Checked = true;
                TextBoxDescription.Text = "";
            }
        }
        protected bool TextBookValidation()
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
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGridTextBook();
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            string description = (row.FindControl("TextBox1") as TextBox).Text.Trim();

            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                Textbook textBook = db.Textbooks.Where(x => x.TextbookId == id).FirstOrDefault();

                textBook.Description = description;
                textBook.Active = active;

                db.SaveChanges();
            }
            GridView1.EditIndex = -1;
            BindGridTextBook();
            BindDropDownTextBook();
            lblMessage.Text = "Update Successfully!";
            lblErrorMessage.Text = "";
        }
        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGridTextBook();
        }
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);

                using (MaterialEntities db = new MaterialEntities())
                {
                    Textbook textBook = db.Textbooks.Where(x => x.TextbookId == id).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.Textbooks.Remove(textBook);
                    db.SaveChanges();
                }
                this.BindGridTextBook();
                BindDropDownTextBook();
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

        //===================================Course Technology Requirement=========================================
        protected void addCourseTextBook_Click(object sender, EventArgs e)
        {
            if (CourseTextBookValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                    int textBookId = Convert.ToInt32(DropDownListTextBook.SelectedValue);
                    if (!db.CourseTextbooks.Where(x => x.CourseId == courseId && x.TextbookId == textBookId).Any())
                    {
                        CourseTextbook courseTextbook = new CourseTextbook()
                        {
                            CourseId = Convert.ToInt32(DropDownListCourse.SelectedValue),
                            TextbookId = Convert.ToInt32(DropDownListTextBook.SelectedValue),
                            Active = CheckBoxCourseTextBookActive.Checked
                        };
                        db.CourseTextbooks.Add(courseTextbook);
                        db.SaveChanges();
                        lblMessageCourseTextBook.Text = "Save Successfully!";
                        lblErrorMessageCourseTextBook.Text = "";
                    }
                    else
                    {
                        lblErrorMessageCourseTextBook.Text = "Already Exist!";
                        lblMessageCourseTextBook.Text = "";
                    }

                }

                BindGridCourseTextBook();
                CheckBoxCourseTextBookActive.Checked = true;
                DropDownListTextBook.SelectedIndex = -1;
            }
        }
        protected bool CourseTextBookValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(DropDownListCourse.SelectedValue))
            {
                fieldName += " Course Instance -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListTextBook.SelectedValue))
            {
                fieldName += " Text Book -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessageCourseTextBook.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessageCourseTextBook.Text = "";
            }
            return result;
        }
        protected void DropDownListCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCourse.SelectedValue != "")
            {
                BindGridCourseTextBook();
            }
        }
        protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView2.EditIndex = -1;
            BindGridCourseTextBook();
        }
        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView2.Rows[e.RowIndex];
                int id = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
                int courseId = Convert.ToInt32((row.FindControl("LabelCourseId") as Label).Text);
                using (MaterialEntities db = new MaterialEntities())
                {
                    CourseTextbook courseTextbook = db.CourseTextbooks.Where(x => x.TextbookId == id && x.CourseId == courseId).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.CourseTextbooks.Remove(courseTextbook);
                    db.SaveChanges();
                }
                this.BindGridCourseTextBook();
                lblMessageCourseTextBook.Text = "Delete Successfully!";
                lblErrorMessageCourseTextBook.Text = "";
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    lblErrorMessageCourseTextBook.Text = ex.Message;
                }
                else
                {
                    lblErrorMessageCourseTextBook.Text = ex.InnerException.InnerException.Message;
                }
                lblErrorMessageCourseTextBook.Text = "";
            }

        }
        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView2.EditIndex = e.NewEditIndex;
            BindGridCourseTextBook();
        }
        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView2.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
            int courseId = Convert.ToInt32((row.FindControl("LabelCourseId") as Label).Text);
            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                CourseTextbook courseTextbook = db.CourseTextbooks.Where(x => x.TextbookId == id && x.CourseId == courseId).FirstOrDefault();
                courseTextbook.Active = active;
                db.SaveChanges();
            }
            GridView2.EditIndex = -1;
            BindGridCourseTextBook();
            lblMessageCourseTextBook.Text = "Update Successfully!";
            lblErrorMessageCourseTextBook.Text = "";
        }
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView2.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }
        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            clearAll();
            lblMessage.Text = "";
            lblMessageCourseTextBook.Text = "";
            lblErrorMessage.Text = "";
            lblErrorMessageCourseTextBook.Text = "";
        }
        private void clearAll()
        {
            TextBoxDescription.Text = "";
            CheckBoxActive.Checked = true;
            CheckBoxCourseTextBookActive.Checked = true;
            DropDownListCourse.SelectedIndex = -1;
            DropDownListTextBook.SelectedIndex = -1;
            BindGridCourseTextBook();
        }
    }
}