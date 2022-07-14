using EFModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddNonAcademicDay : System.Web.UI.UserControl
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
                var model = (from a in db.Quarters
                             select new { QuarterName = a.School.Name + " | " + a.StartDate + " TO " + a.EndDate, QuarterId = a.QuarterId }).ToList();

                DropDownListQuarter.DataSource = model.OrderBy(x => x.QuarterId);
                DropDownListQuarter.DataTextField = "QuarterName";
                DropDownListQuarter.DataValueField = "QuarterId";
                DropDownListQuarter.DataBind();
                DropDownListQuarter.Items.Insert(0, new ListItem("--Select One--", ""));
            }
        }

        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                int quarterId = Convert.ToInt32(DropDownListQuarter.SelectedValue);
                var model = from a in db.NonAcademicDays
                            where a.QuarterId == quarterId
                            select new { a.Id, a.Description, a.Type, a.StartDate, a.EndTime };

                GridView1.DataSource = model.ToList();
                GridView1.DataBind();
            }
        }


        protected void btnAddNewNonAcademicDay_Click(object sender, EventArgs e)
        {
            if (MethodValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int QuarterId = Convert.ToInt32(DropDownListQuarter.SelectedValue);

                    NonAcademicDay model = new NonAcademicDay()
                    {
                        QuarterId = QuarterId,
                        Description = TextBoxDescription.Text.Trim(),
                        Type = DropDownListType.SelectedValue, /*TextBoxType.Text.Trim(),*/
                        StartDate = Convert.ToDateTime(TextBoxStartDate.Text.Trim()),
                        EndTime = Convert.ToDateTime(TextBoxEndDate.Text.Trim()),
                    };
                    db.NonAcademicDays.Add(model);
                    db.SaveChanges();
                    lblMessage.Text = "Saved Successfully!";
                    lblErrorMessage.Text = "";
                }

                BindGrid();
                CleartTextBox();
            }
        }
        protected void CleartTextBox() {
            TextBoxDescription.Text = "";
            DropDownListType.SelectedIndex = -1;
            TextBoxStartDate.Text = "";
            TextBoxEndDate.Text = "";
        }
        protected bool MethodValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(DropDownListQuarter.SelectedValue))
            {
                fieldName += " Quarter -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxDescription.Text))
            {
                fieldName += " Description -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListType.SelectedValue))
            {
                fieldName += " Type -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxStartDate.Text))
            {
                fieldName += " Start Date -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxEndDate.Text))
            {
                fieldName += " End Date -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessage.Text = "";
            }
            return result;
        }

        protected void DropDownListQuarter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListQuarter.SelectedValue != "")
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
            string type = ((row.FindControl("DropDownListTypeGV") as DropDownList).SelectedValue).ToString();
            string startDate = (row.FindControl("TextBox2") as TextBox).Text.Trim();
            string endTime = (row.FindControl("TextBox3") as TextBox).Text.Trim();

            using (MaterialEntities db = new MaterialEntities())
            {
                NonAcademicDay model = db.NonAcademicDays.Where(x => x.Id == id).FirstOrDefault();

                model.Description = description;
                model.Type = type;
                model.StartDate = Convert.ToDateTime(startDate);
                model.EndTime = Convert.ToDateTime(endTime);

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
                    NonAcademicDay model = db.NonAcademicDays.Where(x => x.Id == id).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.NonAcademicDays.Remove(model);
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
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == GridView1.EditIndex)
            {
                string type = (e.Row.FindControl("Label5") as Label).Text;
                DropDownList dlType = e.Row.FindControl("DropDownListTypeGV") as DropDownList;
                List<ListItem> list = new List<ListItem>() { new ListItem { Text = "School", Value = "School" }, new ListItem { Text = "Holiday", Value = "Holiday"}};

                using (MaterialEntities db = new MaterialEntities())
                {
                    dlType.DataSource = list;
                    //dlType.DataTextField = "Name";
                    //dlType.DataValueField = "Value";
                    dlType.SelectedValue = type;
                    dlType.DataBind();
                }
               
                
            }
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }

        protected void ClearAddNonAcademicDay_Click(object sender, EventArgs e)
        {
            CleartTextBox();
            BindDropDown();
            GridView1.DataSource = null;
            GridView1.DataBind();
        }
    }
}