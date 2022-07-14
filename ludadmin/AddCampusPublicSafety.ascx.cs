using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddCampusPublicSafety : System.Web.UI.UserControl
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
        private void BindDropDownCampusPublicSafety()
        {
            if (DropDownListSchool.SelectedValue != "")
            {
                int schoolId = Convert.ToInt32(DropDownListSchool.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    var model = from a in db.CampusPublicSafeties
                                where a.SchoolId == schoolId
                                select new { a.Id, Subtitle = a.Subtitle == "" ? "Undefined Title" : a.Subtitle };

                    DropDownListCampusPublicSafety.DataSource = model.OrderBy(x => x.Id).ToList();
                    DropDownListCampusPublicSafety.DataTextField = "Subtitle";
                    DropDownListCampusPublicSafety.DataValueField = "Id";
                    DropDownListCampusPublicSafety.DataBind();
                    DropDownListCampusPublicSafety.Items.Insert(0, new ListItem("--Select One--", ""));
                }
            }
        }
        //=============================Course Policy =========================
        protected void DropDownListSchool_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAll();
            BindDropDownCampusPublicSafety();
        }
        protected void DropDownListCampusPublicSafety_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCampusPublicSafety.SelectedValue != "")
            {
                int Id = Convert.ToInt32(DropDownListCampusPublicSafety.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    CampusPublicSafety CampusPublicSafety = db.CampusPublicSafeties.Where(x => x.Id == Id).FirstOrDefault();

                    TextBoxSubTitle.Text = CampusPublicSafety.Subtitle;
                    TextBoxDescription.Text = CampusPublicSafety.Description;
                    CheckBoxActive.Checked = CampusPublicSafety.Active;
                    btnAddCampusPublicSafety.Attributes["disabled"] = "disabled";
                    btnAddCampusPublicSafety.Style["pointer-events"] = "none";
                }
                BindGridCampusPublicSafetyPoint();
            }
        }
        protected void btnUpdateCampusPublicSafety_Click(object sender, EventArgs e)
        {
            if (CampusPublicSafetyValidation() && DropDownListCampusPublicSafety.SelectedValue != "")
            {
                int CampusPublicSafetyId = Convert.ToInt32(DropDownListCampusPublicSafety.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    CampusPublicSafety CampusPublicSafety = db.CampusPublicSafeties.Where(x => x.Id == CampusPublicSafetyId).FirstOrDefault();

                    CampusPublicSafety.Subtitle = TextBoxSubTitle.Text.Trim();
                    CampusPublicSafety.Description = TextBoxDescription.Text.Trim();
                    CampusPublicSafety.Active = CheckBoxActive.Checked;

                    db.SaveChanges();

                    BindDropDownCampusPublicSafety();
                    DropDownListCampusPublicSafety.SelectedValue = Convert.ToString(CampusPublicSafety.Id);
                    lblMessage.Text = "Update Successfully!";
                    lblErrorMessage.Text = "";
                }
            }
        }

        protected void btnDeleteCampusPublicSafety_Click(object sender, EventArgs e)
        {
            if (DropDownListCampusPublicSafety.SelectedValue != "")
            {
                try
                {
                    int CampusPublicSafetyId = Convert.ToInt32(DropDownListCampusPublicSafety.SelectedValue);
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        ////------------Delete---------------
                        IQueryable<CampusPublicSafetyPoint> CampusPublicSafetyPoints = db.CampusPublicSafetyPoints.Where(x => x.CampusPublicSafetyId == CampusPublicSafetyId);
                        if (CampusPublicSafetyPoints.Any())
                        {
                            //TODO: Make sure this is correct
                            db.CampusPublicSafetyPoints.RemoveRange(CampusPublicSafetyPoints);
                            db.SaveChanges();
                        }
                        //------------Delete from CouseInstance---------------
                        CampusPublicSafety CampusPublicSafety = db.CampusPublicSafeties.Where(x => x.Id == CampusPublicSafetyId).FirstOrDefault();
                        //TODO: Make sure this is correct
                        db.CampusPublicSafeties.Remove(CampusPublicSafety);
                        db.SaveChanges();
                    }
                    BindDropDownCampusPublicSafety();
                    ClearAll();
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
            else {
                lblErrorMessage.Text = "Please Select a Campus Public Safety";
                lblMessage.Text = "";
            }
        }
        private void ClearAll()
        {
            //BindDropDownSchool();
            //DropDownListSchool.SelectedIndex = -1;
            DropDownListCampusPublicSafety.Items.Clear();
            BindDropDownCampusPublicSafety();
            TextBoxDescription.Text = "";
            TextBoxSubTitle.Text = "";

            CheckBoxActive.Checked = true;
            CheckBoxPointActive.Checked = true;
            btnAddCampusPublicSafety.Attributes.Remove("disabled");
            btnAddCampusPublicSafety.Style["pointer-events"] = "visible";

            TextBoxPolicyPoint.Text = "";
            GridView1.DataSource = null;
            GridView1.DataBind();
        }
        protected void btnAddCampusPublicSafety_Click(object sender, EventArgs e)
        {
            if (CampusPublicSafetyValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int schoolId = Convert.ToInt32(DropDownListSchool.SelectedValue);
                    CampusPublicSafety campusPublicSafety = new CampusPublicSafety()
                    {
                        SchoolId = schoolId,
                        Subtitle = TextBoxSubTitle.Text.Trim(),
                        Description = TextBoxDescription.Text.Trim(),
                        Active = CheckBoxActive.Checked
                    };
                    db.CampusPublicSafeties.Add(campusPublicSafety);
                    db.SaveChanges();

                    lblMessage.Text = "Save Successfully!";
                    lblErrorMessage.Text = "";
                    btnAddCampusPublicSafety.Attributes["disabled"] = "disabled";
                    btnAddCampusPublicSafety.Style["pointer-events"] = "none";
                    BindDropDownCampusPublicSafety();
                    DropDownListCampusPublicSafety.SelectedValue = Convert.ToString(campusPublicSafety.Id);
                }
            }
        }

        //=======================Course Policy Point ===========================
        private void BindGridCampusPublicSafetyPoint()
        {
            if (DropDownListCampusPublicSafety.SelectedValue != "")
            {
                int CampusPublicSafetyId = Convert.ToInt32(DropDownListCampusPublicSafety.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    var model = (from a in db.CampusPublicSafetyPoints.Where(x => x.CampusPublicSafetyId == CampusPublicSafetyId)
                                 select new { a.Id, a.Description, a.Active }).ToList();
                    GridView1.DataSource = model.OrderBy(x => x.Id);
                    GridView1.DataBind();
                }
            }
        }

        protected void btnAddCampusPublicSafetyPoint_Click(object sender, EventArgs e)
        {
            if (CampusPublicSafetyPointValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int CampusPublicSafetyId = Convert.ToInt32(DropDownListCampusPublicSafety.SelectedValue);
                    CampusPublicSafetyPoint CampusPublicSafetyPoint = new CampusPublicSafetyPoint()
                    {
                        CampusPublicSafetyId = CampusPublicSafetyId,
                        Description = TextBoxPolicyPoint.Text.Trim(),
                        Active = CheckBoxPointActive.Checked
                    };
                    db.CampusPublicSafetyPoints.Add(CampusPublicSafetyPoint);
                    db.SaveChanges();

                    lblMessage.Text = "Save Successfully!";
                    lblErrorMessage.Text = "";
                }
                BindGridCampusPublicSafetyPoint();
                TextBoxPolicyPoint.Text = "";
            }
        }

        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGridCampusPublicSafetyPoint();
        }

        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGridCampusPublicSafetyPoint();
        }

        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;
            string description = (row.FindControl("TextBox1") as TextBox).Text;

            using (MaterialEntities db = new MaterialEntities())
            {
                CampusPublicSafetyPoint cpp = db.CampusPublicSafetyPoints.Where(x => x.Id == id).FirstOrDefault();
                cpp.Description = description;
                cpp.Active = active;
                db.SaveChanges();
            }
            GridView1.EditIndex = -1;
            BindGridCampusPublicSafetyPoint();
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
                    CampusPublicSafetyPoint cpp = db.CampusPublicSafetyPoints.Where(x => x.Id == id).FirstOrDefault();

                    //TODO: Make sure this is correct
                    db.CampusPublicSafetyPoints.Remove(cpp);
                    db.SaveChanges();
                }
                BindGridCampusPublicSafetyPoint();
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
        //--------------------Validation---------------------------
        protected bool CampusPublicSafetyValidation()
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
        protected bool CampusPublicSafetyPointValidation()
        {
            bool result = true;
            string fieldName = "";

            if (string.IsNullOrWhiteSpace(DropDownListCampusPublicSafety.SelectedValue))
            {
                fieldName += " Campus Public Safety -";
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