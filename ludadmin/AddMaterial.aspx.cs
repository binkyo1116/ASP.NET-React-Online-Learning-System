using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddMaterial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDownCourseInstance();
                filterMaterial();
                //this.BindGrid();
                //BindDropDownModuleObjective();
            }
        }
        private void BindDropDownCourseInstance()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //---------------Coruse Ins Droup Down list----------------
                //db.Courses.OrderBy(x => x.Id).ToList();
                DropDownListCourseInstance.DataSource = db.CourseInstances.Where(ci => ci.Active).Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourseInstance.DataTextField = "Name";
                DropDownListCourseInstance.DataValueField = "Id";
                DropDownListCourseInstance.DataBind();
                DropDownListCourseInstance.Items.Insert(0, new ListItem("--Select Course Instance--", ""));

                DropDownListCourseInstanceFilter.DataSource = db.CourseInstances.Where(ci => ci.Active).Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourseInstanceFilter.DataTextField = "Name";
                DropDownListCourseInstanceFilter.DataValueField = "Id";
                DropDownListCourseInstanceFilter.DataBind();
                DropDownListCourseInstanceFilter.Items.Insert(0, new ListItem("--Select Course Instance--", ""));
            }
        }

        private void BindDropDownModuleObjective()
        {
            if (DropDownListCourseInstance.SelectedValue != "")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                    int courseId = db.CourseInstances.Find(courseInstanceId).CourseId;
                    var modelObjectives = (from a in db.ModuleObjectives.Where(x => x.Active && x.Modules.Where(m => m.Active && m.CourseObjectives.Where(co => co.Active && co.Courses.Where(c => c.Id == courseId).Any()).Any()).Any())
                                           select new { Id = a.Id, Title = a.Description }).OrderBy(y => y.Id).ToList();

                    //--------------------Bind Dropdown--------------------------
                    DropDownListModuleObjective.DataSource = modelObjectives;
                    DropDownListModuleObjective.DataTextField = "Title";
                    DropDownListModuleObjective.DataValueField = "Id";
                    DropDownListModuleObjective.DataBind();
                    DropDownListModuleObjective.Items.Insert(0, new ListItem("--Select Module--", ""));
                }
            }
        }
        private void BindDropDownModuleObjectiveFilter()
        {
            if (DropDownListCourseInstanceFilter.SelectedValue != "")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    int courseId = db.CourseInstances.Find(courseInstanceId).CourseId;
                    var modelObjectives = (from a in db.ModuleObjectives.Where(x => x.Active && x.Modules.Where(m => m.Active && m.CourseObjectives.Where(co => co.Active && co.Courses.Where(c => c.Id == courseId).Any()).Any()).Any())
                                           select new { Id = a.Id, Title = a.Description }).OrderBy(y => y.Id).ToList();

                    //--------------------Bind Dropdown--------------------------
                    DropDownListModuleObjectiveFilter.DataSource = modelObjectives;
                    DropDownListModuleObjectiveFilter.DataTextField = "Title";
                    DropDownListModuleObjectiveFilter.DataValueField = "Id";
                    DropDownListModuleObjectiveFilter.DataBind();
                    DropDownListModuleObjectiveFilter.Items.Insert(0, new ListItem("--Select Module--", ""));
                }
            }
        }
        protected void DropDownListCourseInstanceFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterMaterial();
            BindDropDownModuleObjectiveFilter();
        }
        protected void DropDownListModuleObjectiveFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterMaterial();
        }
        private void filterMaterial()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                if (DropDownListModuleObjectiveFilter.SelectedValue != "" && DropDownListCourseInstanceFilter.SelectedValue != "")
                {
                    int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjectiveFilter.SelectedValue);
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    ddlMaterial.DataSource = db.CourseInstanceMaterials.Where(x => x.ModuleObjectiveId == moduleObjectiveId && x.CourseInstanceId == courseInstanceId).Select(y => y.Material).OrderBy(x => x.Id).ToList();
                }
                else if (DropDownListCourseInstanceFilter.SelectedValue != "")
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    ddlMaterial.DataSource = db.CourseInstanceMaterials.Where(x => x.CourseInstanceId == courseInstanceId).Select(y => y.Material).OrderBy(x => x.Id).ToList();
                }
                else if (DropDownListModuleObjectiveFilter.SelectedValue != "")
                {
                    int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjectiveFilter.SelectedValue);
                    ddlMaterial.DataSource = db.CourseInstanceMaterials.Where(x => x.ModuleObjectiveId == moduleObjectiveId).Select(y => y.Material).OrderBy(x => x.Id).ToList(); ;
                }
                else
                {
                    ddlMaterial.DataSource = db.Materials.Select(x => new { Id = x.Id, Title = x.Title }).ToList();
                }

                //--------------- Droup Down list----------------
                ddlMaterial.DataTextField = "Title";
                ddlMaterial.DataValueField = "Id";
                ddlMaterial.DataBind();
                ddlMaterial.Items.Insert(0, new ListItem("--Select Material--", ""));
            }
        }


        protected void AddNewMaterial_Click(object sender, EventArgs e)
        {
            if (MaterialValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    Material material = new Material()
                    {
                        Title = TextBoxTitle.Text,
                        Description = TextBoxDescription.Text,
                        Active = CheckBoxActive.Checked
                    };
                    db.Materials.Add(material);
                    db.SaveChanges();
                    //----------------------------------
                    filterMaterial();
                    ddlMaterial.SelectedValue = Convert.ToString(material.Id);
                }
                //this.crearText();
                BindGrid();
                lblMessage.Text = "Save Successfully!";
                lblErrorMessage.Text = "";
                AddNewMaterial.Attributes["disabled"] = "disabled";
                AddNewMaterial.Style["pointer-events"] = "none";
            }
        }
        private void crearText()
        {
            TextBoxDescription.Text = "";
            TextBoxTitle.Text = "";
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
            int ci = Convert.ToInt32((row.FindControl("Label1") as Label).Text);
            int m = Convert.ToInt32((row.FindControl("Label2") as Label).Text);
            int moi = Convert.ToInt32((row.FindControl("Label3") as Label).Text);
            DateTime dueDate = Convert.ToDateTime((row.FindControl("TextBox2") as TextBox).Text);
            using (MaterialEntities db = new MaterialEntities())
            {
                CourseInstanceMaterial material = db.CourseInstanceMaterials.Where(x => x.CourseInstanceId == ci && x.ModuleObjectiveId == moi && x.MaterialId == m).FirstOrDefault();

                material.DueDate = dueDate;
                db.SaveChanges();
            }
            GridView1.EditIndex = -1;
            BindGrid();
            lblMessage.Text = "Save Successfully!";
            lblErrorMessage.Text = "";
        }
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row ?');";
            }
        }
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int ci = Convert.ToInt32((row.FindControl("Label1") as Label).Text);
                int m = Convert.ToInt32((row.FindControl("Label2") as Label).Text);
                int moi = Convert.ToInt32((row.FindControl("Label3") as Label).Text);
                if (ci != 0 && m != 0 && moi != 0)
                {
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        CourseInstanceMaterial material = db.CourseInstanceMaterials.Where(x => x.CourseInstanceId == ci && x.ModuleObjectiveId == moi && x.MaterialId == m).FirstOrDefault();

                        db.CourseInstanceMaterials.Remove(material);
                        db.SaveChanges();
                    }
                    GridView1.EditIndex = -1;
                    this.BindGrid();
                    lblMessage.Text = "Delete Successfully!";
                    lblErrorMessage.Text = "";
                }
                else
                {
                    lblErrorMessage.Text = "Sorry! Selection process is not correct.";
                }
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
        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                IQueryable<CourseInstanceMaterial> model = from a in db.CourseInstanceMaterials select a;
                if (DropDownListCourseInstance.SelectedValue != "")
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                    model = from a in model.Where(x => x.CourseInstanceId == courseInstanceId) select a;
                }
                if (DropDownListModuleObjective.SelectedValue != "")
                {
                    int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
                    model = from a in model.Where(x => x.ModuleObjectiveId == moduleObjectiveId) select a;
                }
                SetDatainGrid(model);
            }
        }
        private void SetDatainGrid(IQueryable<CourseInstanceMaterial> CImaterial)
        {
            var model = (from a in CImaterial
                         select new { a.CourseInstanceId, CourseInstance = a.CourseInstance.Course.Name, a.MaterialId, Material = a.Material.Title, a.ModuleObjectiveId, ModuleObjective = a.ModuleObjective.Description, a.DueDate });

            GridView1.DataSource = model.ToList();
            GridView1.DataBind();
        }

        #region--------------------Validation---------------------------
        protected bool MaterialValidation()
        {
            bool result = true;
            string fieldName = "";

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
                lblMessage.Text = "";
            }
            return result;
        }
        protected bool CourseInstanceMaterialValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(DropDownListCourseInstance.SelectedValue))
            {
                fieldName += " Course Instance -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListModuleObjective.SelectedValue))
            {
                fieldName += " Module Objective -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(ddlMaterial.SelectedValue))
            {
                fieldName += " Module Objective -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxDueDate.Text))
            {
                fieldName += " Due Date -";
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
                SetDatainGrid(db.CourseInstanceMaterials);
                filterMaterial();
            }
        }

        protected void ButtonSubmitCourseInstanceMaterial_Click(object sender, EventArgs e)
        {
            if (CourseInstanceMaterialValidation())
            {
                int courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
                int materialId = Convert.ToInt32(ddlMaterial.SelectedValue);

                using (MaterialEntities db = new MaterialEntities())
                {
                    bool exist = db.CourseInstanceMaterials.Where(x => x.CourseInstanceId == courseInstanceId
                                                                     && x.ModuleObjectiveId == moduleObjectiveId
                                                                     && x.MaterialId == materialId).Any();
                    if (!exist)
                    {
                        CourseInstanceMaterial model = new CourseInstanceMaterial()
                        {
                            CourseInstanceId = Convert.ToInt32(courseInstanceId),
                            ModuleObjectiveId = moduleObjectiveId,
                            MaterialId = materialId,
                            DueDate = Convert.ToDateTime(TextBoxDueDate.Text)
                        };
                        db.CourseInstanceMaterials.Add(model);
                        db.SaveChanges();
                        lblMessage.Text = "Save Successfully!";
                        lblErrorMessage.Text = "";
                        //clearAll();
                        BindGrid();
                    }
                    else
                    {
                        lblErrorMessage.Text = "Sorry! Already Exist!";
                        lblMessage.Text = "";
                    }
                }

            }
        }

        protected void ddlMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                Material model = db.Materials.Find(Convert.ToInt32(ddlMaterial.SelectedValue));
                TextBoxTitle.Text = model.Title;
                TextBoxDescription.Text = model.Description;
                CheckBoxActive.Checked = model.Active;
                AddNewMaterial.Attributes["disabled"] = "disabled";
                AddNewMaterial.Style["pointer-events"] = "none";
            }
        }
        protected void btnMaterialUpdate_Click(object sender, EventArgs e)
        {
            if (ddlMaterial.SelectedValue != "")
            {
                int materialId = Convert.ToInt32(ddlMaterial.SelectedValue);

                using (MaterialEntities db = new MaterialEntities())
                {
                    //-------------Material table---------------------
                    Material material = db.Materials.Find(materialId);
                    material.Title = TextBoxTitle.Text;
                    material.Description = TextBoxDescription.Text;

                    material.Active = CheckBoxActive.Checked;
                    db.SaveChanges();

                    filterMaterial();
                    ddlMaterial.SelectedValue = Convert.ToString(material.Id);
                }
                lblMessage.Text = "Update Successfully";
                lblErrorMessage.Text = "";
            }
            else
            {
                lblErrorMessage.Text = "Please add an Material first";
                lblMessage.Text = "";
            }
        }
        private void clearAll()
        {
            TextBoxTitle.Text = "";
            TextBoxDescription.Text = "";
            ddlMaterial.SelectedIndex = -1;
            CheckBoxActive.Checked = true;

            DropDownListCourseInstance.SelectedIndex = -1;
            DropDownListModuleObjective.SelectedIndex = -1;
            DropDownListCourseInstanceFilter.SelectedIndex = -1;
            DropDownListModuleObjectiveFilter.SelectedIndex = -1;

            AddNewMaterial.Attributes.Remove("disabled");
            AddNewMaterial.Style["pointer-events"] = "visible";
            //BindGrid();
            //lblMessage.Text = "";
            GridView1.DataSource = null;
            GridView1.DataBind();
        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            clearAll();
            lblMessage.Text = "";
            lblErrorMessage.Text = "";
            filterMaterial();
        }

        protected void DropDownListCourseInstance_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
            BindDropDownModuleObjective();

        }

        protected void DropDownListModuleObjective_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnMaterialDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlMaterial.SelectedValue != "")
                {
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        Material material = db.Materials.Find(Convert.ToInt32(ddlMaterial.SelectedValue));
                        db.Materials.Remove(material);
                        db.SaveChanges();
                        clearAll();
                        filterMaterial();
                        BindGrid();
                    }
                    lblMessage.Text = "Delete Successfully";
                    lblErrorMessage.Text = "";
                }
                else
                {
                    lblErrorMessage.Text = "Please add an Material first";
                    lblMessage.Text = "";
                }
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
        //protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    //check if the row is the header row
        //    if (e.Row.RowType == DataControlRowType.Header)
        //    {
        //        //add the thead and tbody section programatically
        //        e.Row.TableSection = TableRowSection.TableHeader;
        //    }
        //}
    }
}