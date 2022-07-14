using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddGradeScale : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindDropDownCourse();
                BindDropDownGradeScaleGroup();
                //BindDropDownGradeScaleGroup2();
                //BindCourseGradeScaleGrid();
                BindGradeScaleGrid();
            }
        }
        private void BindGradeScaleGrid()
        {
            int gradeScaleGroupId = Convert.ToInt32(DropDownListGradeScaleGroup.SelectedValue);
            using (MaterialEntities db = new MaterialEntities())
            {
                GridView1.DataSource = db.GradeScales.Where(x => x.GradeScaleGroupId == gradeScaleGroupId).OrderBy(x => x.Id).ToList();
                GridView1.DataBind();
                LabelGradeScaleGroup.Text = db.GradeScaleGroups.Find(gradeScaleGroupId).Title;
            }
        }
        protected void AddNewGradeScaleGroup_Click(object sender, EventArgs e)
        {
            if (TextBoxGroupTitle.Text.Trim() != "")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    GradeScaleGroup gradeScaleGroup = new GradeScaleGroup()
                    {
                        Title = TextBoxGroupTitle.Text.Trim(),
                    };
                    db.GradeScaleGroups.Add(gradeScaleGroup);
                    db.SaveChanges();

                    BindDropDownGradeScaleGroup();
                    DropDownListGradeScaleGroup.SelectedValue = Convert.ToString(gradeScaleGroup.Id);
                }
                BindGradeScaleGrid();
                lblMessage.Text = "Save Successfully";

                AddNewGradeScaleGroup.Attributes["disabled"] = "disabled";
                AddNewGradeScaleGroup.Style["pointer-events"] = "none";
            }
            else
            {
                lblMessage.Text = "The Group Title is Required";
            }

        }
        protected void btnGradeScaleGroupUpdate_Click(object sender, EventArgs e)
        {
            if (DropDownListGradeScaleGroup.SelectedValue != "")
            {
                int groupId = Convert.ToInt32(DropDownListGradeScaleGroup.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    GradeScaleGroup gsg = db.GradeScaleGroups.Where(x => x.Id == groupId).FirstOrDefault();
                    gsg.Title = TextBoxGroupTitle.Text;
                    db.SaveChanges();

                    BindDropDownGradeScaleGroup();
                    DropDownListGradeScaleGroup.SelectedValue = Convert.ToString(gsg.Id);
                }
                lblMessage.Text = "Update Successfully";
                BindGradeScaleGrid();
            }
            else
            {
                lblMessage.Text = "Please add a Group first";
            }
        }
        protected void btnGradeScaleGroupDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (DropDownListGradeScaleGroup.SelectedValue != "")
                {
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        int groupId = Convert.ToInt32(DropDownListGradeScaleGroup.SelectedValue);
                        GradeScaleGroup gsg = db.GradeScaleGroups.Where(x => x.Id == groupId).FirstOrDefault();
                        db.GradeScaleGroups.Remove(gsg);
                        db.SaveChanges();
                        clearAll();
                        BindDropDownGradeScaleGroup();
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
        private void BindDropDownGradeScaleGroup()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                DropDownListGradeScaleGroup.DataSource = db.GradeScaleGroups.OrderBy(x => x.Id).ToList();
                DropDownListGradeScaleGroup.DataTextField = "Title";
                DropDownListGradeScaleGroup.DataValueField = "Id";
                DropDownListGradeScaleGroup.DataBind();
                //DropDownListGradeScaleGroup.Items.Insert(0, new ListItem("--Select One--", "0"));
            }
        }

        protected void AddNewGradeScale_Click(object sender, EventArgs e)
        {
            if (DropDownListGradeScaleGroup.SelectedValue != "")
            {
                if (GradeScaleValidation())
                {
                    double max = double.Parse(TextBoxMaxNumber.Text);
                    double min = double.Parse(TextBoxMinNumber.Text);
                    double gpa = double.Parse(TextBoxGPA.Text);

                    if (max <= 100 && min <= 100 && gpa <= 4)
                    {
                        using (MaterialEntities db = new MaterialEntities())
                        {
                            GradeScale gradeScale = new GradeScale()
                            {
                                GradeScaleGroupId = Convert.ToInt32(DropDownListGradeScaleGroup.SelectedValue),
                                MaxNumberInPercent = max,
                                MinNumberInPercent = min,
                                GPA = gpa,
                            };
                            db.GradeScales.Add(gradeScale);
                            db.SaveChanges();
                        }
                        ClearTextField();
                        BindGradeScaleGrid();
                        lblMessage.Text = "Save Successfully!";
                    }
                    else
                    {
                        lblMessage.Text = "Invalid data!";
                    }
                }
            }
            else
            {
                lblMessage.Text = "Please Select a Group!";
            }
        }
        private void ClearTextField()
        {
            TextBoxMaxNumber.Text = "";
            TextBoxMinNumber.Text = "";
            TextBoxGPA.Text = "";
            lblMessage.Text = "";
        }
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGradeScaleGrid();
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            double max = double.Parse((row.FindControl("TextBox2") as TextBox).Text);
            double min = double.Parse((row.FindControl("TextBox3") as TextBox).Text);
            double gpa = double.Parse((row.FindControl("TextBox4") as TextBox).Text);
            if (max <= 100 && min <= 100 && gpa <= 4)
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    GradeScale scale = db.GradeScales.Where(x => x.Id == id).FirstOrDefault();
                    scale.MaxNumberInPercent = max;
                    scale.MinNumberInPercent = min;
                    scale.GPA = gpa;
                    db.SaveChanges();
                }
                GridView1.EditIndex = -1;
                BindGradeScaleGrid();
            }
            else
            {
                lblMessage.Text = "Invalid data!";
            }

        }
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            using (MaterialEntities db = new MaterialEntities())
            {
                GradeScale scale = db.GradeScales.Where(x => x.Id == id).FirstOrDefault();
                //TODO: Make sure this is correct
                db.GradeScales.Remove(scale);
                db.SaveChanges();
            }
            BindGradeScaleGrid();
        }

        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGradeScaleGrid();
        }
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }
        protected void DropDownListGradeScaleGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListGradeScaleGroup.SelectedValue != "0")
            {
                int gradeScaleGroupId = Convert.ToInt32(DropDownListGradeScaleGroup.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    GradeScaleGroup gsg = db.GradeScaleGroups.Where(x => x.Id == gradeScaleGroupId).FirstOrDefault();
                    TextBoxGroupTitle.Text = gsg.Title;
                }

                BindGradeScaleGrid();
                AddNewGradeScaleGroup.Attributes["disabled"] = "disabled";
                AddNewGradeScaleGroup.Style["pointer-events"] = "none";
                lblMessage.Text = "";
            }
            else
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
            }
        }
        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            clearAll();
            //BindDropDownGradeScaleGroup2();
            BindDropDownGradeScaleGroup();
        }
        private void clearAll()
        {
            TextBoxGroupTitle.Text = "";
            DropDownListGradeScaleGroup.SelectedIndex = -1;
            ClearTextField();

            AddNewGradeScaleGroup.Attributes.Remove("disabled");
            AddNewGradeScaleGroup.Style["pointer-events"] = "visible";
            lblMessage.Text = "";
            //GridView1.DataSource = null;
            //GridView1.DataBind();

            BindDropDownGradeScaleGroup();
            BindGradeScaleGrid();
        }
        //========================================Grade Scale into Course=========================
        private void BindDropDownCourse()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                DropDownListCourse.DataSource = db.Courses.OrderBy(x => x.Id).ToList();
                DropDownListCourse.DataTextField = "Name";
                DropDownListCourse.DataValueField = "Id";
                DropDownListCourse.DataBind();
                DropDownListCourse.Items.Insert(0, new ListItem("--Select One--", "0"));
            }
        }
        private void BindDropDownGradeScaleGroup2()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                DropDownListGradeScaleGroup2.DataSource = db.GradeScaleGroups.OrderBy(x => x.Id).ToList();
                DropDownListGradeScaleGroup2.DataTextField = "Title";
                DropDownListGradeScaleGroup2.DataValueField = "Id";
                DropDownListGradeScaleGroup2.DataBind();
                //DropDownListGradeScaleGroup2.Items.Insert(0, new ListItem("--Select One--", "0"));
            }
        }

        protected void btnAddScaleInCourse_Click(object sender, EventArgs e)
        {

            if (DropDownListCourse.SelectedValue != "0" && DropDownListGradeScaleGroup2.SelectedValue != "0")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseId = Convert.ToInt32(DropDownListCourse.SelectedValue);
                    int groupId = Convert.ToInt32(DropDownListGradeScaleGroup2.SelectedValue);

                    //Marcelo[20210309]: This table is deprecated, make sure this can be removed
                    /*
                    if (!db.CourseGradeScaleGroup_deprecated.Where(x => x.CourseId == courseId && x.GradeScaleGroupId == groupId).Any())
                    {
                        //-----------------Check Previou Grade Scale in course--------------------
                        CourseGradeScaleGroup_deprecated oldCourseGrade = db.CourseGradeScaleGroup_deprecated.Where(x => x.CourseId == courseId).FirstOrDefault();
                        if (oldCourseGrade != null)
                        {
                            //TODO: Make sure this is correct
                            //db.CourseGradeScales.Remove(oldCourseGrade);
                            db.SaveChanges();
                        }
                        //-------------------------------------------------------------------------
                        CourseGradeScaleGroup_deprecated model = new CourseGradeScaleGroup_deprecated()
                        {
                            CourseId = courseId,
                            GradeScaleGroupId = groupId,
                        };
                        db.CourseGradeScaleGroup_deprecated.Add(model);
                        db.SaveChanges();
                        this.BindCourseGradeScaleGrid();
                        lblMessage2.Text = "Save successfully!";
                    }
                    else
                    {
                        lblMessage2.Text = "Already  Exist!";
                    }
                    */
                }
            }
            else
            {
                lblMessage2.Text = "Invalid data!";
            }
        }
        private void BindCourseGradeScaleGrid()
        {
            //Marcelo[20210309]: The Course CourseGradeSaleGroup table is deprecated, make sure this code can be removed.
            /*
            using (MaterialEntities db = new MaterialEntities())
            {

                var model = (from a in db.CourseGradeScaleGroup_deprecated
                             join c in db.Courses on a.CourseId equals c.Id
                             join g in db.GradeScaleGroups on a.GradeScaleGroupId equals g.Id
                             select new { a.GradeScaleGroupId, a.CourseId, Course = c.Name, GradeScale = g.Title });

                GridView2.DataSource = model.OrderBy(x => x.GradeScaleGroupId).ToList();
                GridView2.DataBind();
            }
            */
        }
        //protected void DropDownListCourse_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindCourseGradeScaleGrid();
        //}

        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Values[0]);
            using (MaterialEntities db = new MaterialEntities())
            {
                GradeScale model = db.GradeScales.Where(x => x.Id == id).FirstOrDefault();
                //TODO: Make sure this is correct
                //db.CourseGradeScales.Remove(CourseGS);
                db.SaveChanges();
            }
            BindCourseGradeScaleGrid();
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView2.EditIndex)
                {
                    (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
                }
            }
        }
        #region--------------------Validation---------------------------
        protected bool GradeScaleValidation()
        {
            bool result = true;
            string fieldName = "";

            if (string.IsNullOrWhiteSpace(TextBoxMaxNumber.Text))
            {
                fieldName += " Max Number -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxMinNumber.Text))
            {
                fieldName += " Min Number -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxGPA.Text))
            {
                fieldName += " GPA -";
                result = false;
            }
            if (!result)
            {
                lblMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
            }
            return result;
        }
        #endregion

        
    }
}