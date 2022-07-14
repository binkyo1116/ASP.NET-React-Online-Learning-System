using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddSupplie : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridSupplie();
                BindDropDownCourse();
                BindDropDownSupplie();
                BindGridCourseSupplie();
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
        private void BindDropDownSupplie()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                System.Collections.Generic.List<Supply> model = db.Supplies.OrderBy(x => x.SupplieId).ToList();
                DropDownListSupplie.DataSource = model;
                DropDownListSupplie.DataTextField = "Description";
                DropDownListSupplie.DataValueField = "SupplieId";
                DropDownListSupplie.DataBind();
                DropDownListSupplie.Items.Insert(0, new ListItem("--Select One--", ""));
                //----------------------Prerequisite-----------------------------------
            }
        }

        private void BindGridSupplie()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //var courseInstance = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                var model = from a in db.Supplies
                            select new { a.SupplieId, a.Description, a.Active };

                GridView1.DataSource = model.ToList();
                GridView1.DataBind();
            }
        }
        private void BindGridCourseSupplie()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                IQueryable<CourseSupply> model = from a in db.CourseSupplies select a;
                if (DropDownListCourse.SelectedValue != "")
                {
                    int courseId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                    model = from a in model
                            where a.CourseId == courseId
                            select a;
                }
                var result = (from a in model
                              select new { a.CourseId, Course = a.Course.Name, a.SupplyId, a.Supply.Description, a.Active }).ToList();

                GridView2.DataSource = result;
                GridView2.DataBind();
            }
        }

        protected void btnAddSupplie_Click(object sender, EventArgs e)
        {
            if (SupplieValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    Supply supplie = new Supply()
                    {
                        Description = TextBoxDescription.Text.Trim(),
                        Active = CheckBoxActive.Checked
                    };
                    db.Supplies.Add(supplie);
                    db.SaveChanges();
                    lblMessage.Text = "Save Successfully!";
                    lblErrorMessage.Text = "";
                }
                BindDropDownSupplie();
                BindGridSupplie();
                CheckBoxActive.Checked = true;
                TextBoxDescription.Text = "";
            }
        }

        protected bool SupplieValidation()
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
            BindGridSupplie();
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            string description = (row.FindControl("TextBox1") as TextBox).Text.Trim();

            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                Supply supplie = db.Supplies.Where(x => x.SupplieId == id).FirstOrDefault();

                supplie.Description = description;
                supplie.Active = active;

                db.SaveChanges();
            }
            GridView1.EditIndex = -1;
            BindGridSupplie();
            BindDropDownSupplie();
            lblMessage.Text = "Update Successfully!";
            lblErrorMessage.Text = "";
        }
        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGridSupplie();
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                using (MaterialEntities db = new MaterialEntities())
                {
                    Supply supplie = db.Supplies.Where(x => x.SupplieId == id).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.Supplies.Remove(supplie);
                    db.SaveChanges();
                }
                BindGridSupplie();
                BindDropDownSupplie();
                lblMessage.Text = "Delete Successfully!";
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
        protected void addCourseSupplie_Click(object sender, EventArgs e)
        {
            if (CourseSupplieValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                    int SupplieId = Convert.ToInt32(DropDownListSupplie.SelectedValue);
                    if (!db.CourseSupplies.Where(x => x.CourseId == courseInstanceId && x.SupplyId == SupplieId).Any())
                    {
                        CourseSupply courseSupplie = new CourseSupply()
                        {
                            CourseId = Convert.ToInt32(DropDownListCourse.SelectedValue),
                            SupplyId = Convert.ToInt32(DropDownListSupplie.SelectedValue),
                            Active = CheckBoxCourseSupplieActive.Checked
                        };
                        db.CourseSupplies.Add(courseSupplie);
                        db.SaveChanges();
                        lblMessageCourseSupplie.Text = "Save Successfully!";
                        lblErrorMessageCourseSupplie.Text = "";
                    }
                    else
                    {
                        lblErrorMessageCourseSupplie.Text = "Already Exist!";
                        lblMessageCourseSupplie.Text = "";
                    }

                }

                BindGridCourseSupplie();
                CheckBoxCourseSupplieActive.Checked = true;
                DropDownListSupplie.SelectedIndex = -1;
            }
        }
        protected bool CourseSupplieValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(DropDownListCourse.SelectedValue))
            {
                fieldName += " Course Instance -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListSupplie.SelectedValue))
            {
                fieldName += " Supplie -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessageCourseSupplie.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessageCourseSupplie.Text = "";
            }
            return result;
        }

        protected void DropDownListCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCourse.SelectedValue != "")
            {
                BindGridCourseSupplie();
            }
        }

        protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView2.EditIndex = -1;
            BindGridCourseSupplie();
        }

        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView2.Rows[e.RowIndex];
                int id = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
                int courseInstanceId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    CourseSupply courseSupplie = db.CourseSupplies.Where(x => x.SupplyId == id && x.CourseId == courseInstanceId).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.CourseSupplies.Remove(courseSupplie);
                    db.SaveChanges();
                }
                BindGridCourseSupplie();
                lblMessageCourseSupplie.Text = "Delete Successfully!";
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    lblErrorMessageCourseSupplie.Text = ex.Message;
                }
                else
                {
                    lblErrorMessageCourseSupplie.Text = ex.InnerException.InnerException.Message;
                }
                lblMessageCourseSupplie.Text = "";
            }
        }

        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView2.EditIndex = e.NewEditIndex;
            BindGridCourseSupplie();
        }

        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView2.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
            int courseInstanceId = Convert.ToInt32(DropDownListCourse.SelectedValue);
            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                CourseSupply courseSupplie = db.CourseSupplies.Where(x => x.SupplyId == id && x.CourseId == courseInstanceId).FirstOrDefault();

                courseSupplie.Active = active;
                db.SaveChanges();
            }
            GridView2.EditIndex = -1;
            BindGridCourseSupplie();
            lblMessageCourseSupplie.Text = "Update Successfully!";
            lblErrorMessageCourseSupplie.Text = "";
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
            lblMessageCourseSupplie.Text = "";
            lblErrorMessageCourseSupplie.Text = "";
        }
        private void clearAll()
        {
            TextBoxDescription.Text = "";
            CheckBoxActive.Checked = true;
            CheckBoxCourseSupplieActive.Checked = true;
            DropDownListCourse.SelectedIndex = -1;
            DropDownListSupplie.SelectedIndex = -1;
            BindGridCourseSupplie();
        }
    }
}