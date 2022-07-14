using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddInstructionMethod : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //this.BindGrid();
                BindDropDown();
            }
        }
        private void BindDropDown()
        {
            using (MaterialEntities db = new MaterialEntities())
            {

                DropDownListCourseInstance.DataSource = db.CourseInstances.Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList(); ;
                DropDownListCourseInstance.DataTextField = "Name";
                DropDownListCourseInstance.DataValueField = "Id";
                DropDownListCourseInstance.DataBind();
                DropDownListCourseInstance.Items.Insert(0, new ListItem("--Select One--", ""));
                //----------------------Prerequisite-----------------------------------
            }
        }

        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                int courseInstance = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                var model = from a in db.InstructionMethods
                            where a.CourseInstanceId == courseInstance
                            select new { a.Id, a.Description, a.Active };

                GridView1.DataSource = model.ToList();
                GridView1.DataBind();
            }
        }


        protected void btnCoursePrerequisite_Click(object sender, EventArgs e)
        {
            if (MethodValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);

                    InstructionMethod instructionMethod = new InstructionMethod()
                    {
                        CourseInstanceId = courseInstanceId,
                        Description = TextBoxDescription.Text.Trim(),
                        Active = CheckBoxActive.Checked
                    };
                    db.InstructionMethods.Add(instructionMethod);
                    db.SaveChanges();
                    lblMessage.Text = "Save Successfully!";
                    lblErrorMessage.Text = "";
                }

                BindGrid();
                CheckBoxActive.Checked = true;
                TextBoxDescription.Text = "";
            }
        }

        protected bool MethodValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(DropDownListCourseInstance.SelectedValue))
            {
                fieldName += " Course -";
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

        protected void DropDownListCourseInstance_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCourseInstance.SelectedValue != "")
            {
                BindGrid();
            }
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
            string description = (row.FindControl("TextBox1") as TextBox).Text.Trim();

            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                InstructionMethod instructionMethod = db.InstructionMethods.Where(x => x.Id == id).FirstOrDefault();

                instructionMethod.Description = description;
                instructionMethod.Active = active;

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
                    InstructionMethod instructionMethod = db.InstructionMethods.Where(x => x.Id == id).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.InstructionMethods.Remove(instructionMethod);
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
    }
}