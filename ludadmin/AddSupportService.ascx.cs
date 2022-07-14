using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddSupportService : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDownSchool();
            }
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

        private void BindGridSupportService()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                int schoolId = Convert.ToInt32(DropDownListSchool.SelectedValue);
                var model = from a in db.SupportServices
                            where a.SchoolId == schoolId
                            select new { a.SupportServicesId, a.Subtitle, a.Description, a.Active };

                GridView1.DataSource = model.ToList();
                GridView1.DataBind();
            }
        }

        protected void btnAddSupportService_Click(object sender, EventArgs e)
        {
            if (SupportServiceValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int schoolId = Convert.ToInt32(DropDownListSchool.SelectedValue);
                    SupportService SupportService = new SupportService()
                    {
                        SchoolId = schoolId,
                        Subtitle = TextBoxSubTitle.Text.Trim(),
                        Description = TextBoxDescription.Text.Trim(),
                        Active = CheckBoxActive.Checked
                    };
                    db.SupportServices.Add(SupportService);
                    db.SaveChanges();
                    lblMessage.Text = "Save Successfully!";
                    lblErrorMessage.Text = "";
                }
                BindGridSupportService();
                CheckBoxActive.Checked = true;
                TextBoxDescription.Text = "";
                TextBoxSubTitle.Text = "";
            }
        }

        protected bool SupportServiceValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(DropDownListSchool.SelectedValue))
            {
                fieldName += " School -";
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

        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGridSupportService();
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            string description = (row.FindControl("TextBox1") as TextBox).Text.Trim();
            string subtitle = (row.FindControl("TextBox2") as TextBox).Text.Trim();
            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                SupportService SupportService = db.SupportServices.Where(x => x.SupportServicesId == id).FirstOrDefault();

                SupportService.Description = description;
                SupportService.Subtitle = subtitle;
                SupportService.Active = active;
                db.SaveChanges();
            }
            GridView1.EditIndex = -1;
            BindGridSupportService();
            lblMessage.Text = "Update Successfully!";
            lblErrorMessage.Text = "";
        }
        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGridSupportService();
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);

                using (MaterialEntities db = new MaterialEntities())
                {
                    SupportService SupportService = db.SupportServices.Where(x => x.SupportServicesId == id).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.SupportServices.Remove(SupportService);
                    db.SaveChanges();
                }
                BindGridSupportService();
                lblMessage.Text = "Deleted Successfully!";
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

        protected void DropDownListSchool_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridSupportService();
        }
    }
}