using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace AdminPages
{
    public partial class AddNetiquette : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDownSchool();
                BindDropDownSchoolForLink();
            }
        }
        private void BindDropDownSchool()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                DropDownListSchool.DataSource = db.Schools.Select(x => new { x.Name, x.SchoolId }).OrderBy(x => x.SchoolId).ToList();
                DropDownListSchool.DataTextField = "Name";
                DropDownListSchool.DataValueField = "SchoolId";
                DropDownListSchool.DataBind();
                DropDownListSchool.Items.Insert(0, new ListItem("--Select One--", ""));
            }
        }
        private void BindDropDownNetiquette()
        {
            if (DropDownListSchool.SelectedValue != "")
            {
                int schoolId = Convert.ToInt32(DropDownListSchool.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    //var Netiquette = db.CoursePolicies.Where(x => x.SchoolId == schoolId);
                    var model = from a in db.Netiquettes
                                where a.SchoolId == schoolId
                                select new { a.Id, Title = a.Title == "" ? "Undefined Title" : a.Title };

                    DropDownListNetiquette.DataSource = model.OrderBy(x => x.Id).ToList();
                    DropDownListNetiquette.DataTextField = "Title";
                    DropDownListNetiquette.DataValueField = "Id";
                    DropDownListNetiquette.DataBind();
                    DropDownListNetiquette.Items.Insert(0, new ListItem("--Select One--", ""));
                }
            }
        }
        //=============================Netiquette =========================
        protected void DropDownListSchool_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownNetiquette();
        }
        protected void DropDownListNetiquette_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListNetiquette.SelectedValue != "")
            {
                int NetiquetteId = Convert.ToInt32(DropDownListNetiquette.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    Netiquette Netiquette = db.Netiquettes.Where(x => x.Id == NetiquetteId).FirstOrDefault();

                    TextBoxSubTitle.Text = Netiquette.Title;
                    TextBoxDescription.Text = Netiquette.Description;
                    CheckBoxActive.Checked = Netiquette.Active;
                    btnAddNetiquette.Attributes["disabled"] = "disabled";
                    btnAddNetiquette.Style["pointer-events"] = "none";
                }
                BindGridNetiquettePoint();
            }
        }
        protected void btnUpdateNetiquette_Click(object sender, EventArgs e)
        {
            if (NetiquetteValidation() && DropDownListNetiquette.SelectedValue != "")
            {
                int NetiquetteId = Convert.ToInt32(DropDownListNetiquette.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    Netiquette Netiquette = db.Netiquettes.Where(x => x.Id == NetiquetteId).FirstOrDefault();

                    Netiquette.Title = TextBoxSubTitle.Text.Trim();
                    Netiquette.Description = TextBoxDescription.Text.Trim();
                    Netiquette.Active = CheckBoxActive.Checked;

                    db.SaveChanges();

                    BindDropDownNetiquette();
                    DropDownListNetiquette.SelectedValue = Convert.ToString(Netiquette.Id);
                    lblMessage.Text = "Updated Successfully!";
                    lblErrorMessage.Text = "";
                }
            }
        }

        protected void btnDeleteNetiquette_Click(object sender, EventArgs e)
        {
            if (DropDownListNetiquette.SelectedValue != "")
            {
                try
                {
                    int NetiquetteId = Convert.ToInt32(DropDownListNetiquette.SelectedValue);
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        ////------------Delete---------------
                        IQueryable<NetiquettePoint> NetiquettePoints = db.NetiquettePoints.Where(x => x.NetiquetteId == NetiquetteId);
                        if (NetiquettePoints.Any())
                        {
                            //TODO: Make sure this is correct
                            db.NetiquettePoints.RemoveRange(NetiquettePoints);
                            db.SaveChanges();
                        }
                        //------------Delete from CouseInstance---------------
                        Netiquette Netiquette = db.Netiquettes.Where(x => x.Id == NetiquetteId).FirstOrDefault();
                        //TODO: Make sure this is correct
                        db.Netiquettes.Remove(Netiquette);
                        db.SaveChanges();
                    }
                    BindDropDownNetiquette();
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
                lblErrorMessageLink.Text = "please select a Netiquette!";
                lblMessage.Text = "";
            }

        }
        private void ClearAll()
        {
            DropDownListSchool.SelectedIndex = -1;
            DropDownListNetiquette.Items.Clear();

            TextBoxDescription.Text = "";
            TextBoxSubTitle.Text = "";

            CheckBoxActive.Checked = true;
            btnAddNetiquette.Attributes.Remove("disabled");
            btnAddNetiquette.Style["pointer-events"] = "visible";

            TextBoxPolicyPoint.Text = "";
            GridView1.DataSource = null;
            GridView1.DataBind();
        }
        protected void btnAddNetiquette_Click(object sender, EventArgs e)
        {
            if (NetiquetteValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int schoolId = Convert.ToInt32(DropDownListSchool.SelectedValue);
                    Netiquette Netiquette = new Netiquette()
                    {
                        SchoolId = schoolId,
                        Title = TextBoxSubTitle.Text.Trim(),
                        Description = TextBoxDescription.Text.Trim(),
                        Active = CheckBoxActive.Checked
                    };
                    db.Netiquettes.Add(Netiquette);
                    db.SaveChanges();

                    lblMessage.Text = "Save Successfully!";
                    lblErrorMessage.Text = "";
                    btnAddNetiquette.Attributes["disabled"] = "disabled";
                    btnAddNetiquette.Style["pointer-events"] = "none";
                    BindDropDownNetiquette();
                    DropDownListNetiquette.SelectedValue = Convert.ToString(Netiquette.Id);
                }
            }
        }

        //======================= Netiquette Point ===========================
        private void BindGridNetiquettePoint()
        {
            if (DropDownListNetiquette.SelectedValue != "")
            {
                int NetiquetteId = Convert.ToInt32(DropDownListNetiquette.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    var model = (from a in db.NetiquettePoints.Where(x => x.NetiquetteId == NetiquetteId)
                                 select new { a.Id, a.Description, a.Active }).ToList();
                    GridView1.DataSource = model.OrderBy(x => x.Id);
                    GridView1.DataBind();
                }
            }
        }

        protected void btnAddNetiquettePoint_Click(object sender, EventArgs e)
        {
            if (NetiquettePointValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int NetiquetteId = Convert.ToInt32(DropDownListNetiquette.SelectedValue);
                    NetiquettePoint NetiquettePoint = new NetiquettePoint()
                    {
                        NetiquetteId = NetiquetteId,
                        Description = TextBoxPolicyPoint.Text.Trim(),
                        Active = CheckBoxActive.Checked
                    };
                    db.NetiquettePoints.Add(NetiquettePoint);
                    db.SaveChanges();

                    lblMessage.Text = "Saved Successfully!";
                    lblErrorMessage.Text = "";
                }
                BindGridNetiquettePoint();
                TextBoxPolicyPoint.Text = "";
            }

        }

        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGridNetiquettePoint();
        }

        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGridNetiquettePoint();
        }

        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;
            string description = (row.FindControl("TextBox1") as TextBox).Text;

            using (MaterialEntities db = new MaterialEntities())
            {
                NetiquettePoint cpp = db.NetiquettePoints.Where(x => x.Id == id).FirstOrDefault();
                cpp.Description = description;
                cpp.Active = active;
                db.SaveChanges();
            }
            GridView1.EditIndex = -1;
            BindGridNetiquettePoint();
            lblMessage.Text = "Updated Successfully!";
            lblErrorMessage.Text = "";
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                using (MaterialEntities db = new MaterialEntities())
                {
                    NetiquettePoint cpp = db.NetiquettePoints.Where(x => x.Id == id).FirstOrDefault();

                    //TODO: Make sure this is correct
                    db.NetiquettePoints.Remove(cpp);
                    db.SaveChanges();
                }
                this.BindGridNetiquettePoint();
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
        protected bool NetiquetteValidation()
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
        protected bool NetiquettePointValidation()
        {
            bool result = true;
            string fieldName = "";

            if (string.IsNullOrWhiteSpace(DropDownListNetiquette.SelectedValue))
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
                lblErrorMessage.Text = "";
            }
            return result;
        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            DropDownListSchool.SelectedIndex = -1;
            lblMessage.Text = "";
            lblErrorMessage.Text = "";
            ClearAll();
        }

        #region==============================Netiquette Link=====================================
        private void BindDropDownSchoolForLink()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                DropDownListSchoolForLink.DataSource = db.Schools.Select(x => new { x.Name, x.SchoolId }).OrderBy(x => x.SchoolId).ToList();
                DropDownListSchoolForLink.DataTextField = "Name";
                DropDownListSchoolForLink.DataValueField = "SchoolId";
                DropDownListSchoolForLink.DataBind();
                DropDownListSchoolForLink.Items.Insert(0, new ListItem("--Select One--", ""));
            }
        }
        protected void DropDownListSchoolForLink_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownNetiquetteForLink();
        }
        private void BindDropDownNetiquetteForLink()
        {
            if (DropDownListSchoolForLink.SelectedValue != "")
            {
                int schoolId = Convert.ToInt32(DropDownListSchoolForLink.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    //var Netiquette = db.CoursePolicies.Where(x => x.SchoolId == schoolId);
                    var model = from a in db.Netiquettes
                                where a.SchoolId == schoolId
                                select new { a.Id, Title = a.Title == "" ? "Undefined Title" : a.Title };

                    DropDownListNetiquetteForLink.DataSource = model.OrderBy(x => x.Id).ToList();
                    DropDownListNetiquetteForLink.DataTextField = "Title";
                    DropDownListNetiquetteForLink.DataValueField = "Id";
                    DropDownListNetiquetteForLink.DataBind();
                    DropDownListNetiquetteForLink.Items.Insert(0, new ListItem("--Select One--", ""));
                }
            }
        }
        protected void DropDownListNetiquetteForLink_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListNetiquetteForLink.SelectedValue != "")
            {
                BindGridNetiquetteLink();
            }
        }
        private void BindGridNetiquetteLink()
        {
            if (DropDownListNetiquetteForLink.SelectedValue != "")
            {
                int NetiquetteId = Convert.ToInt32(DropDownListNetiquetteForLink.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    var model = (from a in db.NetiquetteLinks.Where(x => x.NetiquetteId == NetiquetteId)
                                 select new { a.Id, a.Description, a.Title, a.Link, a.Active }).ToList();
                    GridView2.DataSource = model.OrderBy(x => x.Id);
                    GridView2.DataBind();
                }
            }
        }
        private void ClearLink()
        {
            TextBoxLinkDescription.Text = "";
            TextBoxLinkTitle.Text = "";
            TextBoxLink.Text = "";
            CheckBoxLinkActive.Checked = true;
        }
        protected void ButtonAddNetiquetteLink_Click(object sender, EventArgs e)
        {
            if (NetiquetteLinkValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int NetiquetteId = Convert.ToInt32(DropDownListNetiquetteForLink.SelectedValue);
                    NetiquetteLink NetiquetteLink = new NetiquetteLink()
                    {
                        NetiquetteId = NetiquetteId,
                        Description = TextBoxLinkDescription.Text.Trim(),
                        Title = TextBoxLinkTitle.Text.Trim(),
                        Link = TextBoxLink.Text.Trim(),
                        Active = CheckBoxLinkActive.Checked
                    };
                    db.NetiquetteLinks.Add(NetiquetteLink);
                    db.SaveChanges();

                    lblMessageLink.Text = "Saved Successfully!";
                    lblErrorMessageLink.Text = "";
                }
                BindGridNetiquetteLink();
                ClearLink();
            }
        }
        protected bool NetiquetteLinkValidation()
        {
            bool result = true;
            string fieldName = "";

            if (string.IsNullOrWhiteSpace(DropDownListNetiquetteForLink.SelectedValue))
            {
                fieldName += " Netiquette -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxLinkTitle.Text))
            {
                fieldName += " Title -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxLinkDescription.Text))
            {
                fieldName += " Description -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxLink.Text))
            {
                fieldName += " Link -";
                result = false;
            }
            if (!result)
            {
                lblMessageLink.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblErrorMessageLink.Text = "";
            }
            return result;
        }
        protected void OnRowEditingLink(object sender, GridViewEditEventArgs e)
        {
            GridView2.EditIndex = e.NewEditIndex;
            BindGridNetiquetteLink();
        }

        protected void OnRowCancelingEditLink(object sender, GridViewCancelEditEventArgs e)
        {
            GridView2.EditIndex = -1;
            BindGridNetiquetteLink();
        }

        protected void OnRowUpdatingLink(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView2.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;
            string description = (row.FindControl("TextBox1") as TextBox).Text;
            string title = (row.FindControl("TextBox2") as TextBox).Text;
            string link = (row.FindControl("TextBox3") as TextBox).Text;

            using (MaterialEntities db = new MaterialEntities())
            {
                NetiquetteLink model = db.NetiquetteLinks.Where(x => x.Id == id).FirstOrDefault();
                model.Description = description;
                model.Title = title;
                model.Link = link;
                model.Active = active;
                db.SaveChanges();
            }
            GridView2.EditIndex = -1;
            BindGridNetiquetteLink();
            lblMessage.Text = "Update Successfully!";
        }
        protected void OnRowDeletingLink(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
                using (MaterialEntities db = new MaterialEntities())
                {
                    NetiquetteLink cpp = db.NetiquetteLinks.Where(x => x.Id == id).FirstOrDefault();

                    //TODO: Make sure this is correct
                    db.NetiquetteLinks.Remove(cpp);
                    db.SaveChanges();
                }
                this.BindGridNetiquetteLink();
                lblMessageLink.Text = "Deleted Successfully!";
                lblErrorMessageLink.Text = "";
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    lblErrorMessageLink.Text = ex.Message;
                }
                else
                {
                    lblErrorMessageLink.Text = ex.InnerException.Message;
                }
                lblMessageLink.Text = "";
            }
        }
        protected void OnRowDataBoundLink(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView2.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }
        #endregion================================================================================
    }
}