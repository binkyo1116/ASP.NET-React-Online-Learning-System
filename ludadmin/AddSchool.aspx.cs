using EFModel;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddSchool : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Load_School_List();
            }
        }
        private void Load_School_List()
        {
            using (MaterialEntities data = new MaterialEntities())
            {
                grvSchoolList.DataSource = data.Schools.ToList();
                grvSchoolList.DataBind();
            }
        }

        protected void AddSchool_Click(object sender, EventArgs e)
        {
            if (SchoolValidation())
            {
                using (MaterialEntities data = new MaterialEntities())
                {
                    int id = GenarateSchoolId();
                    School school = new School()
                    {
                        Name = TextBoxSchoolName.Text.Trim(),
                        AcademicCalendar = TextBoxAcademicCalendar.Text.Trim(),
                        SyllabusMessage = TextBoxSyllabusMessage.Text.Trim(),
                        SchoolId = id
                    };
                    data.Schools.Add(school);
                    data.SaveChanges();
                }
                ClearTextField();
                Load_School_List();
                lblMessage.Text = "Save Successfully!";
                lblErrorMessage.Text = "";
            }
            else
            {
                lblErrorMessage.Text = "Sorry! Operation has been failed. School Name is Required.";
                lblMessage.Text = "";
            }
        }
        private int GenarateSchoolId()
        {

            int sid = 0;
            //---------------------Genarate School Id------------------
            using (MaterialEntities data = new MaterialEntities())
            {
                School lastEntry = data.Schools.OrderByDescending(x => x.SchoolId).FirstOrDefault();
                if (lastEntry != null)
                {
                    sid = ++lastEntry.SchoolId;
                }
                else
                {
                    sid = 1;
                }
            }

            return sid;
        }
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            grvSchoolList.EditIndex = e.NewEditIndex;
            Load_School_List();
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = grvSchoolList.Rows[e.RowIndex];
            int schoolid = Convert.ToInt32(grvSchoolList.DataKeys[e.RowIndex].Values[0]);

            string name = (row.FindControl("TextBox2") as TextBox).Text;
            string academicCalender = (row.FindControl("TextBox3") as TextBox).Text;
            string syllabusMessage = (row.FindControl("TextBox4") as TextBox).Text;

            using (MaterialEntities ctx = new MaterialEntities())
            {
                School school = (from c in ctx.Schools
                                 where c.SchoolId == schoolid
                                 select c).FirstOrDefault();
                school.Name = name;
                school.AcademicCalendar = academicCalender;
                school.SyllabusMessage = syllabusMessage;
                ctx.SaveChanges();
            }
            grvSchoolList.EditIndex = -1;
            Load_School_List();
        }
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int schoolid = Convert.ToInt32(grvSchoolList.DataKeys[e.RowIndex].Values[0]);
                using (MaterialEntities ctx = new MaterialEntities())
                {
                    School school = (from c in ctx.Schools
                                     where c.SchoolId == schoolid
                                     select c).FirstOrDefault();
                    //TODO: Make sure this is correct
                    ctx.Schools.Remove(school);
                    ctx.SaveChanges();
                    lblMessage.Text = "Deleted Successfully!";
                    lblErrorMessage.Text = "";
                }
                Load_School_List();
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

        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grvSchoolList.EditIndex = -1;
            Load_School_List();
        }
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != grvSchoolList.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row with School ID = " + DataBinder.Eval(e.Row.DataItem, "SchoolId") + "?');";
            }
        }
        private void ClearTextField()
        {
            TextBoxSchoolName.Text = "";
            TextBoxAcademicCalendar.Text = "";
            TextBoxSyllabusMessage.Text = "";
        }
        //--------------------Validation---------------------------
        protected bool SchoolValidation()
        {
            bool result = true;
            string fieldName = "";

            if (string.IsNullOrWhiteSpace(TextBoxSchoolName.Text))
            {
                fieldName += " School Name -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxAcademicCalendar.Text))
            {
                fieldName += " Academic Calendar -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxSyllabusMessage.Text))
            {
                fieldName += " Syllabus Message -";
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
            ClearTextField();
            lblMessage.Text = "";
            lblErrorMessage.Text = "";
        }
    }
}