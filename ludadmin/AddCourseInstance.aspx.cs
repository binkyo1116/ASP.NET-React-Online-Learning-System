using EFModel;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddCourseInstance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDownCourse();
                BindDropDownQuarter();
                BindDropDownSchool();
                BindGrid();
            }
        }
        private void BindDropDownCourse()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //var model = from a in db.Courses
                //            join b in db.CourseInstances on a.CourseId
                DropDownListCourse.DataSource = db.Courses.OrderBy(x => x.Id).ToList();
                DropDownListCourse.DataTextField = "Name";
                DropDownListCourse.DataValueField = "Id";
                DropDownListCourse.DataBind();
                DropDownListCourse.Items.Insert(0, new ListItem("--Select One--", ""));
            }
        }
        private void BindDropDownQuarter()
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
        protected void AddNewCourseInstance_Click(object sender, EventArgs e)
        {
            if (CourseInstanceValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    CourseInstance courseInstence = new CourseInstance()
                    {
                        CourseId = Convert.ToInt32(DropDownListCourse.SelectedValue),
                        QuarterId = Convert.ToInt32(DropDownListQuarter.SelectedValue),
                        Active = CheckBoxActive.Checked
                    };
                    db.CourseInstances.Add(courseInstence);
                    db.SaveChanges();
                    DropDownListSearchBySchool.SelectedIndex = -1;
                    clearCourseInstance();
                    lblMessageCourseInstance.Text = "Save Successfully!";
                    lblErrorMessage.Text = "";
                }
            }
        }

        //--------------------Validation---------------------------
        protected bool CourseInstanceValidation()
        {
            bool result = true;
            string fieldName = "";

            if (string.IsNullOrWhiteSpace(DropDownListCourse.SelectedValue))
            {
                fieldName += " Course -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListQuarter.SelectedValue))
            {
                fieldName += " Quarter -";
                result = false;
            }
            if (!result)
            {
                lblErrorMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblMessageCourseInstance.Text = "";
            }
            return result;
        }
        private void clearCourseInstance()
        {
            DropDownListCourse.SelectedIndex = -1;
            DropDownListQuarter.SelectedIndex = -1;
            CheckBoxActive.Checked = true;
            BindGrid();
        }
        private void BindDropDownSchool()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //-----------School for Search-----------------------------
                DropDownListSearchBySchool.DataSource = db.Schools.ToList();
                DropDownListSearchBySchool.DataTextField = "Name";
                DropDownListSearchBySchool.DataValueField = "SchoolId";
                DropDownListSearchBySchool.DataBind();
                DropDownListSearchBySchool.Items.Insert(0, new ListItem("--Select one--", ""));
            }
        }
        protected void DropDownListSearchBySchool_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListSearchBySchool.SelectedValue != "")
            {
                BindGrid();
            }
        }
        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                if (DropDownListSearchBySchool.SelectedValue != "")
                {
                    int schoolId = Convert.ToInt32(DropDownListSearchBySchool.SelectedValue);
                    IQueryable<CourseInstance> model = db.CourseInstances.Where(x => x.Course.SchoolId == schoolId);
                    SetDatainGrid(model);
                }
                else
                {
                    SetDatainGrid(db.CourseInstances);
                }
            }
        }
        private void SetDatainGrid(IQueryable<CourseInstance> data)
        {
            System.Collections.Generic.List<CourseInstance> d = data.ToList();
            byte[] defaultv = Array.Empty<byte>();
            var model = (from a in data
                         select new { CourseInstance = "ID - " + a.Id, Id = a.Id, a.CourseId, a.QuarterId, Course = a.Course.Name, School = a.Course.School.Name, Quarter = a.Quarter.School.Name + "|" + a.Quarter.StartDate + " TO " + a.Quarter.EndDate, a.Active }).ToList();
            GridView1.DataSource = model;
            GridView1.DataBind();
        }
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == GridView1.EditIndex)
            {
                string quarterId = (e.Row.FindControl("Label2") as Label).Text;
                DropDownList quarter = e.Row.FindControl("DropDownListQuarter") as DropDownList;
                using (MaterialEntities db = new MaterialEntities())
                {
                    var model = (from a in db.Quarters
                                 select new { QuarterName = a.School.Name + " | " + a.StartDate + " TO " + a.EndDate, QuarterId = a.QuarterId }).ToList();

                    quarter.DataSource = model.OrderBy(x => x.QuarterId).ToList();
                    quarter.DataTextField = "QuarterName";
                    quarter.DataValueField = "QuarterId";
                    quarter.DataBind();
                }
                quarter.SelectedValue = quarterId;
            }
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row ?');";
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
            bool active = (row.FindControl("CheckBox1") as CheckBox).Checked;
            int quarterId = Convert.ToInt32((row.FindControl("DropDownListQuarter") as DropDownList).SelectedValue);
            using (MaterialEntities db = new MaterialEntities())
            {
                CourseInstance courseInstance = db.CourseInstances.Find(id);
                courseInstance.Active = active;
                courseInstance.QuarterId = quarterId;
                db.SaveChanges();
            }
            GridView1.EditIndex = -1;
            BindGrid();
        }
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                using (MaterialEntities db = new MaterialEntities())
                {
                    CourseInstance courseInstance = db.CourseInstances.Find(id);

                    //TODO: Make sure this is correct
                    db.CourseInstances.Remove(courseInstance);
                    db.SaveChanges();
                    lblMessageCourseInstance.Text = "Deleted Successfully!";
                    lblErrorMessage.Text = "";
                }
                BindGrid();
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
                lblMessageCourseInstance.Text = "";
            }
        
        }
        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }
        protected void ShowAllList_Click(object sender, EventArgs e)
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                SetDatainGrid(db.CourseInstances);
                lblErrorMessage.Text = "";
                lblMessageCourseInstance.Text = "";
            }
        }

    }
}