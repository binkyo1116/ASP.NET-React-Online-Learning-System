using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddGradingPolicy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridGradingPolicy();
                BindDropDownSchool();
                //BindDropDownGradingPolicy();
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
                //----------------------Prerequisite-----------------------------------
            }
        }
        //private void BindDropDownGradingPolicy()
        //{
        //    using (MaterialEntities db = new MaterialEntities())
        //    {
        //        var model = db.GradingPolicies.OrderBy(x => x.GradingPolicyId).ToList();
        //        DropDownListGradingPolicy.DataSource = model;
        //        DropDownListGradingPolicy.DataTextField = "Description";
        //        DropDownListGradingPolicy.DataValueField = "GradingPolicyId";
        //        DropDownListGradingPolicy.DataBind();
        //        DropDownListGradingPolicy.Items.Insert(0, new ListItem("--Select One--", ""));
        //        //----------------------Prerequisite-----------------------------------
        //    }
        //}

        private void BindGridGradingPolicy()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var model = from a in db.GradingPolicies
                            select new { a.SchoolId, School = a.School.Name, a.Id, a.Description, a.Active };

                GridView1.DataSource = model.ToList();
                GridView1.DataBind();
            }
        }
        //private void BindGridSchoolGradingPolicy()
        //{
        //    using (MaterialEntities db = new MaterialEntities())
        //    {
        //        var schoolId = DropDownListSchool.SelectedValue!=""? Convert.ToInt32(DropDownListSchool.SelectedValue): 0;
        //        if (schoolId > 0)
        //        {
        //            var model = from a in db.SchoolGradingPolicies
        //                        where a.SchoolId == schoolId
        //                        select new { a.GradingPolicyId, a.SchoolId, GradingPolicy = a.GradingPolicy.Description, School = a.School.Name, a.Active };

        //            GridView2.DataSource = model.ToList();
        //            GridView2.DataBind();
        //        }
        //        else {
        //            var model = from a in db.SchoolGradingPolicies
        //                        select new { a.GradingPolicyId, a.SchoolId, GradingPolicy = a.GradingPolicy.Description, School = a.School.Name, a.Active };

        //            GridView2.DataSource = model.ToList();
        //            GridView2.DataBind();
        //        }

        //    }
        //}

        protected void btnAddGradingPolicy_Click(object sender, EventArgs e)
        {
            if (policyValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int schoolId = Convert.ToInt32(DropDownListSchool.SelectedValue);
                    if (!db.GradingPolicies.Where(x => x.SchoolId == schoolId).Any())
                    {
                        GradingPolicy policy = new GradingPolicy()
                        {
                            SchoolId = schoolId,
                            Description = TextBoxDescription.Text.Trim(),
                            Active = CheckBoxActive.Checked
                        };
                        db.GradingPolicies.Add(policy);
                        db.SaveChanges();
                        lblMessage.Text = "Save Successfully!";
                        lblErrorMessage.Text = "";
                    }
                    else
                    {
                        lblErrorMessage.Text = "The School already Exist!";
                        lblMessage.Text = "";
                    }

                }
                //BindDropDownGradingPolicy();
                BindGridGradingPolicy();
                CheckBoxActive.Checked = true;
                TextBoxDescription.Text = "";
            }
        }

        protected bool policyValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(TextBoxDescription.Text))
            {
                fieldName += " Description -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListSchool.Text))
            {
                fieldName += " School -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
            }
            return result;
        }

        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGridGradingPolicy();
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            string description = (row.FindControl("TextBox1") as TextBox).Text.Trim();
            int schoolId = Convert.ToInt32((row.FindControl("DropDownListSchoolGV") as DropDownList).SelectedValue);

            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                GradingPolicy policy = db.GradingPolicies.Where(x => x.Id == id).FirstOrDefault();
                policy.SchoolId = schoolId;
                policy.Description = description;
                policy.Active = active;

                db.SaveChanges();
            }
            GridView1.EditIndex = -1;
            BindGridGradingPolicy();
            //BindDropDownGradingPolicy();
            lblMessage.Text = "Update Successfully!";
            lblErrorMessage.Text = "";
        }
        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGridGradingPolicy();
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);

                using (MaterialEntities db = new MaterialEntities())
                {
                    GradingPolicy policy = db.GradingPolicies.Where(x => x.Id == id).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.GradingPolicies.Remove(policy);
                    db.SaveChanges();
                }
                BindGridGradingPolicy();
                //BindDropDownGradingPolicy();
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
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == GridView1.EditIndex)
            {
                string SchoolId = (e.Row.FindControl("SchoolId") as Label).Text;
                DropDownList schoolList = e.Row.FindControl("DropDownListSchoolGV") as DropDownList;

                using (MaterialEntities db = new MaterialEntities())
                {
                    schoolList.DataSource = db.Schools.OrderBy(x => x.SchoolId).ToList();
                    schoolList.DataTextField = "Name";
                    schoolList.DataValueField = "SchoolId";
                    schoolList.DataBind();
                    schoolList.Items.Insert(0, new ListItem("--Select One--", ""));
                }
                schoolList.SelectedValue = SchoolId;
            }
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }


    }
}