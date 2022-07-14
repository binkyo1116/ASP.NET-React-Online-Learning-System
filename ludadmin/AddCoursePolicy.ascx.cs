using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddCoursePolicy : System.Web.UI.UserControl
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
                DropDownListSchool.DataSource = db.Schools.Select(x => new { x.SchoolId, x.Name }).OrderBy(x => x.SchoolId).ToList();
                DropDownListSchool.DataTextField = "Name";
                DropDownListSchool.DataValueField = "SchoolId";
                DropDownListSchool.DataBind();
                DropDownListSchool.Items.Insert(0, new ListItem("--Select One--", ""));
            }
        }
        private void BindDropDownCoursePolicy()
        {
            if (DropDownListSchool.SelectedValue != "")
            {
                int schoolId = Convert.ToInt32(DropDownListSchool.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    //var coursePolicy = db.CoursePolicies.Where(x => x.SchoolId == schoolId);
                    var model = from a in db.CoursePolicies
                                where a.SchoolId == schoolId
                                select new { CoursePolicyId = a.Id, Subtitle = a.Subtitle == "" ? "Undefined Title" : a.Subtitle };

                    DropDownListCoursePolicy.DataSource = model.OrderBy(x => x.CoursePolicyId).ToList();
                    DropDownListCoursePolicy.DataTextField = "Subtitle";
                    DropDownListCoursePolicy.DataValueField = "CoursePolicyId";
                    DropDownListCoursePolicy.DataBind();
                    DropDownListCoursePolicy.Items.Insert(0, new ListItem("--Select One--", ""));
                }
            }
        }
        //=============================Course Policy =========================
        protected void DropDownListSchool_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAll();
            BindDropDownCoursePolicy();
        }
        protected void DropDownListCoursePolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCoursePolicy.SelectedValue != "")
            {
                int coursePolicyId = Convert.ToInt32(DropDownListCoursePolicy.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    CoursePolicy coursePolicy = db.CoursePolicies.Where(x => x.Id == coursePolicyId).FirstOrDefault();

                    TextBoxSubTitle.Text = coursePolicy.Subtitle;
                    TextBoxDescription.Text = coursePolicy.Description;
                    CheckBoxActive.Checked = coursePolicy.Active;
                    btnAddCoursePolicy.Attributes["disabled"] = "disabled";
                    btnAddCoursePolicy.Style["pointer-events"] = "none";
                }
                BindGridCoursePolicyPoint();
            }
        }
        protected void btnUpdateCoursePolicy_Click(object sender, EventArgs e)
        {
            if (CoursePolicyValidation() && DropDownListCoursePolicy.SelectedValue != "")
            {
                int coursePolicyId = Convert.ToInt32(DropDownListCoursePolicy.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    CoursePolicy coursePolicy = db.CoursePolicies.Where(x => x.Id == coursePolicyId).FirstOrDefault();

                    coursePolicy.Subtitle = TextBoxSubTitle.Text.Trim();
                    coursePolicy.Description = TextBoxDescription.Text.Trim();
                    coursePolicy.Active = CheckBoxActive.Checked;

                    db.SaveChanges();

                    BindDropDownCoursePolicy();
                    DropDownListCoursePolicy.SelectedValue = Convert.ToString(coursePolicy.Id);
                    lblMessage.Text = "Update Successfully!";
                    lblErrorMessage.Text = "";
                }
            }
        }

        protected void btnDeleteCoursePolicy_Click(object sender, EventArgs e)
        {
            if (DropDownListCoursePolicy.SelectedValue != "")
            {
                try
                {
                    int coursePolicyId = Convert.ToInt32(DropDownListCoursePolicy.SelectedValue);
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        ////------------Delete---------------
                        IQueryable<CoursePolicyPoint> coursePolicyPoints = db.CoursePolicyPoints.Where(x => x.CoursePolicyId == coursePolicyId);
                        if (coursePolicyPoints.Any())
                        {
                            //TODO: Make sure this is correct
                            db.CoursePolicyPoints.RemoveRange(coursePolicyPoints);
                            db.SaveChanges();
                        }
                        //------------Delete from CouseInstance---------------
                        CoursePolicy coursePolicy = db.CoursePolicies.Where(x => x.Id == coursePolicyId).FirstOrDefault();
                        //TODO: Make sure this is correct
                        db.CoursePolicies.Remove(coursePolicy);
                        db.SaveChanges();
                    }
                    ClearAll();
                    BindDropDownCoursePolicy();
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
                        lblErrorMessage.Text = ex.InnerException.InnerException.Message;
                    }
                    lblMessage.Text = "";
                }
            }
            else {
                lblErrorMessage.Text = "Please add an Course Policy first";
                lblMessage.Text = "";
            }
        }
        private void ClearAll()
        {

            //BindDropDownSchool();
            //DropDownListSchool.SelectedIndex = -1;
            DropDownListCoursePolicy.Items.Clear();

            TextBoxDescription.Text = "";
            TextBoxSubTitle.Text = "";

            CheckBoxActive.Checked = true;
            CheckBoxPolicyPointActive.Checked = true;
            btnAddCoursePolicy.Attributes.Remove("disabled");
            btnAddCoursePolicy.Style["pointer-events"] = "visible";

            TextBoxPolicyPoint.Text = "";
            GridView1.DataSource = null;
            GridView1.DataBind();
        }
        protected void btnAddCoursePolicy_Click(object sender, EventArgs e)
        {
            if (CoursePolicyValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int schoolId = Convert.ToInt32(DropDownListSchool.SelectedValue);
                    CoursePolicy coursePolicy = new CoursePolicy()
                    {
                        SchoolId = schoolId,
                        Subtitle = TextBoxSubTitle.Text.Trim(),
                        Description = TextBoxDescription.Text.Trim(),
                        Active = CheckBoxActive.Checked
                    };
                    db.CoursePolicies.Add(coursePolicy);
                    db.SaveChanges();

                    lblMessage.Text = "Save Successfully!";
                    lblErrorMessage.Text = "";
                    btnAddCoursePolicy.Attributes["disabled"] = "disabled";
                    btnAddCoursePolicy.Style["pointer-events"] = "none";
                    BindDropDownCoursePolicy();
                    DropDownListCoursePolicy.SelectedValue = Convert.ToString(coursePolicy.Id);
                }
            }
        }

        //=======================Course Policy Point ===========================
        private void BindGridCoursePolicyPoint()
        {
            if (DropDownListCoursePolicy.SelectedValue != "")
            {
                int coursePolicyId = Convert.ToInt32(DropDownListCoursePolicy.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    var model = (from a in db.CoursePolicyPoints.Where(x => x.CoursePolicyId == coursePolicyId)
                                 select new { a.Id, a.Description, a.Active }).ToList();
                    GridView1.DataSource = model.OrderBy(x => x.Id);
                    GridView1.DataBind();
                }
            }
        }

        protected void btnAddCoursePolicyPoint_Click(object sender, EventArgs e)
        {
            if (CoursePolicyPointValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int coursePolicyId = Convert.ToInt32(DropDownListCoursePolicy.SelectedValue);
                    CoursePolicyPoint coursePolicyPoint = new CoursePolicyPoint()
                    {
                        CoursePolicyId = coursePolicyId,
                        Description = TextBoxPolicyPoint.Text.Trim(),
                        Active = CheckBoxPolicyPointActive.Checked
                    };
                    db.CoursePolicyPoints.Add(coursePolicyPoint);
                    db.SaveChanges();

                    lblMessage.Text = "Save Successfully!";
                    lblErrorMessage.Text = "";
                }
                BindGridCoursePolicyPoint();
                TextBoxPolicyPoint.Text = "";
            }

        }

        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGridCoursePolicyPoint();
        }

        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGridCoursePolicyPoint();
        }

        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;
            string description = (row.FindControl("TextBox1") as TextBox).Text;

            using (MaterialEntities db = new MaterialEntities())
            {
                CoursePolicyPoint cpp = db.CoursePolicyPoints.Where(x => x.Id == id).FirstOrDefault();
                cpp.Description = description;
                cpp.Active = active;
                db.SaveChanges();
            }
            GridView1.EditIndex = -1;
            BindGridCoursePolicyPoint();
            lblMessage.Text = "Update Successfully!";
            lblErrorMessage.Text = "";
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                using (MaterialEntities db = new MaterialEntities())
                {
                    CoursePolicyPoint cpp = db.CoursePolicyPoints.Where(x => x.Id == id).FirstOrDefault();

                    //TODO: Make sure this is correct
                    db.CoursePolicyPoints.Remove(cpp);
                    db.SaveChanges();
                }
                BindGridCoursePolicyPoint();
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
        //--------------------Validation---------------------------
        protected bool CoursePolicyValidation()
        {
            bool result = true;
            string fieldName = "";

            if (string.IsNullOrWhiteSpace(DropDownListSchool.SelectedValue))
            {
                fieldName += " School -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessage.Text = "";
            }
            return result;
        }
        protected bool CoursePolicyPointValidation()
        {
            bool result = true;
            string fieldName = "";

            if (string.IsNullOrWhiteSpace(DropDownListCoursePolicy.SelectedValue))
            {
                fieldName += " Course Policy -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxPolicyPoint.Text))
            {
                fieldName += " Point Description -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessage.Text = "";
            }
            return result;
        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            DropDownListSchool.SelectedIndex = -1;
            ClearAll();
        }
    }
}