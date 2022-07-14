using EFModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddModuleObjective : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDownCourse();
                BindDropDownModule();
                BindDropDownModuleObjective();
                BindGrid();
            }
        }
        private void BindDropDownCourse()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //---------------Coruse Droup Down list----------------
                DropDownListCourse.DataSource = db.Courses.Select(x => new { x.Id, x.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourse.DataTextField = "Name";
                DropDownListCourse.DataValueField = "Id";
                DropDownListCourse.DataBind();
                DropDownListCourse.Items.Insert(0, new ListItem("--Select Course--", ""));

            }
        }
        protected void DropDownListCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCourse.SelectedValue != "")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    //---------------Coruse Objective Droup Down list----------------
                    DropDownListCourseObjective.DataSource = db.Courses.Find(Convert.ToInt32(DropDownListCourse.SelectedValue)).CourseObjectives.
                        Where(x => x.Active).Select(x => new { x.Description, x.Id }).OrderBy(x => x.Id).ToList();
                    DropDownListCourseObjective.DataTextField = "Description";
                    DropDownListCourseObjective.DataValueField = "Id";
                    DropDownListCourseObjective.DataBind();
                    DropDownListCourseObjective.Items.Insert(0, new ListItem("--Select Course Objective--", ""));
                }
            }
        }
        protected void DropDownListCourseObjective_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownModule();
        }
        private void BindDropDownModule()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                List<MenuList> module = new List<MenuList>();
                if (DropDownListCourseObjective.SelectedValue != "")
                {
                    module = db.CourseObjectives.Find(Convert.ToInt32(DropDownListCourseObjective.SelectedValue)).Modules.
                        Where(x => x.Active).Select(x => new MenuList { Id = x.Id, Title = x.Description }).OrderBy(y => y.Id).ToList();
                }
                else
                {
                    module = db.Modules.Where(x => x.Active).Select(x => new MenuList { Id = x.Id, Title = x.Description }).OrderBy(y => y.Id).ToList();
                }
                //--------------------Bind Dropdown--------------------------
                DropDownListModule.DataSource = module;
                DropDownListModule.DataTextField = "Title";
                DropDownListModule.DataValueField = "Id";
                DropDownListModule.DataBind();
                DropDownListModule.Items.Insert(0, new ListItem("--Select Module--", ""));
            }
        }
        private void BindDropDownModuleObjective()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //--------------------Bind Dropdown--------------------------
                DropDownListModuleObjective.DataSource = db.ModuleObjectives.Select(x => new { x.Id, x.Description }).OrderBy(x => x.Id).ToList();
                DropDownListModuleObjective.DataTextField = "Description";
                DropDownListModuleObjective.DataValueField = "Id";
                DropDownListModuleObjective.DataBind();
                DropDownListModuleObjective.Items.Insert(0, new ListItem("--Select Module Objective--", ""));
            }

        }
        protected void AddNewModuleObjective_Click(object sender, EventArgs e)
        {
            if (ModuleObjectiveValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    ModuleObjective moduleObj = new ModuleObjective()
                    {
                        Description = TextBoxDescription.Text,
                        Active = CheckBoxActive.Checked
                    };
                    db.ModuleObjectives.Add(moduleObj);
                    db.SaveChanges();

                    BindDropDownModuleObjective();
                    DropDownListModuleObjective.SelectedValue = Convert.ToString(moduleObj.Id);
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
            DropDownListModule.SelectedIndex = -1;
            CheckBoxActive.Checked = true;
        }

        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGrid();
        }
        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            string description = (row.FindControl("TextBox1") as TextBox).Text;
            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                ModuleObjective moduleObj = db.ModuleObjectives.Where(x => x.Id == id).FirstOrDefault();
                moduleObj.Description = description;
                moduleObj.Active = active;
                db.SaveChanges();
            }
            GridView1.EditIndex = -1;
            BindGrid();
        }
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                using (MaterialEntities db = new MaterialEntities())
                {
                    ModuleObjective moduleObj = db.ModuleObjectives.Where(x => x.Id == id).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.ModuleObjectives.Remove(moduleObj);
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
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    using (MaterialEntities db = new MaterialEntities())
            //    {
            //        //-----------------Grid Sorting course----------------
            //        DropDownList DropDownListSortCourse = (DropDownList)e.Row.FindControl("DropDownListSortCourse");
            //        DropDownListSortCourse.DataSource = db.Courses.OrderBy(x => x.Id).ToList();
            //        DropDownListSortCourse.DataTextField = "Name";
            //        DropDownListSortCourse.DataValueField = "CourseId";
            //        DropDownListSortCourse.DataBind();
            //        DropDownListSortCourse.Items.Insert(0, new ListItem("--Select One--"));
            //    }

            //}
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }
        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                if (DropDownListModule.SelectedValue != "")
                {
                    List<ModuleObjective> model = db.Modules.Find(Convert.ToInt32(DropDownListModule.SelectedValue)).ModuleObjectives.ToList();
                    SetDatainGrid(model.ToList());
                }
                else
                {
                    System.Data.Entity.DbSet<ModuleObjective> model = db.ModuleObjectives;
                    SetDatainGrid(model.ToList());
                }

            }
        }
        private void SetDatainGrid(List<ModuleObjective> moduleObj)
        {
            GridView1.DataSource = moduleObj.OrderBy(x => x.Id);
            GridView1.DataBind();
        }

        #region--------------------Validation---------------------------
        protected bool ModuleObjectiveValidation()
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
        protected bool ModuleModuleObjectiveValidation()
        {
            bool result = true;
            string fieldName = "";

            if (DropDownListModuleObjective.SelectedValue == "")
            {
                fieldName += " Module Objective -";
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

        protected void DropDownListModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListModule.SelectedValue != "")
            {
                BindGrid();
            }
        }

        protected void ShowAllModulObjectiveeList_Click(object sender, EventArgs e)
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                DropDownListModule.SelectedIndex = -1;
                List<ModuleObjective> model = db.ModuleObjectives.ToList();
                SetDatainGrid(model);
            }
        }
        private void clearAll()
        {
            TextBoxDescription.Text = "";
            CheckBoxActive.Checked = true;

            DropDownListCourse.SelectedIndex = -1;
            DropDownListCourseObjective.SelectedIndex = -1;
            BindDropDownModule();
            DropDownListModuleObjective.SelectedIndex = -1;
            BindGrid();
        }
        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            clearAll();
            lblMessage.Text = "";
            lblErrorMessage.Text = "";
        }

        protected void DropDownListModule_SelectedIndexChanged1(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void ButtonModuleModuleObjective_Click(object sender, EventArgs e)
        {
            if (ModuleModuleObjectiveValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int moduleId = Convert.ToInt32(DropDownListModule.SelectedValue);
                    int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
                    Module module = db.Modules.Find(moduleId);
                    bool exist = module.ModuleObjectives.Where(x => x.Id == moduleObjectiveId).Any();
                    if (!exist)
                    {
                        ModuleObjective moduleObjective = db.ModuleObjectives.Find(moduleObjectiveId);
                        module.ModuleObjectives.Add(moduleObjective);
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
    }
}
