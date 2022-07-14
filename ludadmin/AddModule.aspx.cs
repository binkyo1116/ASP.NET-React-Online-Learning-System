using EFModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddModule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDownCourse();
                BindDropDownModule();
                BindDropDownCourseObjective();
                BindGrid();
            }
        }
        private void BindDropDownCourse()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //---------------Coruse Droup Down list----------------
                DropDownListCourse.DataSource = db.Courses.Select(x => new { x.Name, x.Id }).OrderBy(x => x.Id).ToList();
                DropDownListCourse.DataTextField = "Name";
                DropDownListCourse.DataValueField = "Id";
                DropDownListCourse.DataBind();
                DropDownListCourse.Items.Insert(0, new ListItem("--Select Course--", ""));
            }
        }
        private void BindDropDownModule()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //---------------Module Droup Down list----------------
                DropDownListModule.DataSource = db.Modules.Select(x => new { x.Id, x.Description }).OrderBy(x => x.Id).ToList();
                DropDownListModule.DataTextField = "Description";
                DropDownListModule.DataValueField = "Id";
                DropDownListModule.DataBind();
                DropDownListModule.Items.Insert(0, new ListItem("--Select Module--", ""));
            }
        }
        private void BindDropDownCourseObjective()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                List<MenuList> courseObjectives = new List<MenuList>();
                if (DropDownListCourse.SelectedValue != "")
                {
                    courseObjectives = db.Courses.Find(Convert.ToInt32(DropDownListCourse.SelectedValue)).CourseObjectives.
                        Where(co => co.Active).Select(x => new MenuList { Id = x.Id, Title = x.Description }).OrderBy(y => y.Id).ToList();
                }
                else
                {
                    courseObjectives = db.CourseObjectives.Where(co => co.Active).Select(x => new MenuList { Id = x.Id, Title = x.Description }).OrderBy(y => y.Id).ToList();
                }
                //--------------------Bind Dropdown--------------------------
                DropDownListCourseObjective.DataSource = courseObjectives;
                DropDownListCourseObjective.DataValueField = "Id";
                DropDownListCourseObjective.DataTextField = "Title";
                DropDownListCourseObjective.DataBind();
                DropDownListCourseObjective.Items.Insert(0, new ListItem("--Select One--", ""));
            }
        }
        protected void AddNewModule_Click(object sender, EventArgs e)
        {
            if (ModuleValidation())
            {

                using (MaterialEntities db = new MaterialEntities())
                {
                    Module module = new Module()
                    {
                        Description = TextBoxDescription.Text,
                        Active = CheckBoxActive.Checked,
                        DueDate = Convert.ToDateTime(TextBoxDueDate.Text)
                    };
                    db.Modules.Add(module);
                    db.SaveChanges();

                    BindDropDownModule();
                    DropDownListModule.SelectedValue = Convert.ToString(module.Id);
                }
                crearText();
                BindGrid();
                lblMessage.Text = "Save Successfully!";
                lblErrorMessage.Text = "";
            }
        }
        private void crearText()
        {
            TextBoxDescription.Text = "";
            CheckBoxActive.Checked = true;
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
            string description = (row.FindControl("TextBox1") as TextBox).Text;

            DateTime dueDate = Convert.ToDateTime((row.FindControl("TextBox2") as TextBox).Text);
            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                Module module = db.Modules.Where(x => x.Id == id).FirstOrDefault();
                module.Description = description;
                module.Active = active;
                module.DueDate = dueDate;
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
                    Module module = db.Modules.Where(x => x.Id == id).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.Modules.Remove(module);
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

        protected void DropDownListCourse_SelectedIndexChanged(object sender, EventArgs e)
        {

            if ((sender as DropDownList).SelectedValue != "")
            {
                BindDropDownCourseObjective();
            }
        }
        protected void DropDownListCourseObjective_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as DropDownList).SelectedValue != "")
            {
                BindGrid();
            }
        }
        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                if (DropDownListCourseObjective.SelectedValue != "")
                {
                    List<Module> module = db.CourseObjectives.Find(Convert.ToInt32(DropDownListCourseObjective.SelectedValue)).Modules.ToList();
                    SetDatainGrid(module);
                }
                else
                {
                    List<Module> module = db.Modules.ToList();
                    SetDatainGrid(module);
                }
            }
        }
        private void SetDatainGrid(List<Module> module)
        {
            GridView1.DataSource = module.OrderBy(x => x.Id);
            GridView1.DataBind();
        }

        #region--------------------Validation---------------------------
        protected bool ModuleValidation()
        {
            bool result = true;
            string fieldName = "";

            if (string.IsNullOrWhiteSpace(TextBoxDueDate.Text))
            {
                fieldName += " Due Date -";
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
                lblMessage.Text = "";
            }
            return result;
        }
        protected bool CourseObjectiveModuleValidation()
        {
            bool result = true;
            string fieldName = "";

            if (DropDownListCourseObjective.SelectedValue == "")
            {
                fieldName += " Course Objective -";
                result = false;
            }
            if (DropDownListModule.SelectedValue == "")
            {
                fieldName += " Module -";
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

        protected void ShowAllModuleList_Click(object sender, EventArgs e)
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                List<Module> module = db.Modules.ToList();
                SetDatainGrid(module);
            }
        }

        protected void ButtonCourseObjectiveModule_Click(object sender, EventArgs e)
        {
            if (CourseObjectiveModuleValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseObjectiveId = Convert.ToInt32(DropDownListCourseObjective.SelectedValue);
                    int moduleId = Convert.ToInt32(DropDownListModule.SelectedValue);
                    CourseObjective courseObjective = db.CourseObjectives.Find(courseObjectiveId);
                    bool exist = courseObjective.Modules.Where(x => x.Id == moduleId).Any();
                    if (!exist)
                    {
                        Module module = db.Modules.Find(moduleId);
                        courseObjective.Modules.Add(module);
                        db.SaveChanges();
                        lblMessage.Text = "Add Successfully.";
                        lblErrorMessage.Text = "";
                        BindGrid();
                    }
                    else
                    {
                        lblErrorMessage.Text = "Sorry! The Module already exist. Please select another one.";
                        lblMessage.Text = "";
                    }
                }
            }
        }

        protected void DropDownListCourseObjective_SelectedIndexChanged1(object sender, EventArgs e)
        {
            BindGrid();
        }
        private void clearAll()
        {
            TextBoxDescription.Text = "";
            CheckBoxActive.Checked = true;

            DropDownListCourse.SelectedIndex = -1;
            BindDropDownCourseObjective();
            DropDownListModule.SelectedIndex = -1;
            BindGrid();
        }
        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            clearAll();
            lblMessage.Text = "";
            lblErrorMessage.Text = "";
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row ?');";
            }
        }
    }
}
public class MenuList
{
    public int Id { get; set; }
    public string Title { get; set; }
}