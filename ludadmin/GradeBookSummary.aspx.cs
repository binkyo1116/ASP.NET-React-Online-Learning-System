using AdminPages;
using EFModel;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace OnlineLearningSystem
{
    public partial class GradeBookSummary : System.Web.UI.Page
    {
        private readonly MaterialEntities data = new MaterialEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindDropdownCourseInstance();
            }
        }
        private void BindDropdownCourseInstance()
        {
            ddCourses.DataSource = data.CourseInstances.Where(ci => ci.Active).Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
            ddCourses.DataTextField = "Name";
            ddCourses.DataValueField = "Id";
            ddCourses.DataBind();
            ddCourses.Items.Insert(0, new ListItem("--Select one--", ""));
        }
        private void StudentGraddeControLoad(Student student, CourseInstance courseInstance)
        {
            GradeBookSummaryControl control = (GradeBookSummaryControl)Page.LoadControl("GradeBookSummaryControl.ascx");
            control.CourseInstance = courseInstance;
            control.Student = student;
            pnlStudentGradeDetails.Controls.Add(control);
        }
        private void BindDropdownStudent()
        {
            if (ddCourses.SelectedValue != "")
            {
                int courseInstanceId = Convert.ToInt32(ddCourses.SelectedValue);
                var students = data.CourseInstances.Find(courseInstanceId).Students.Where(s => !s.Test.HasValue || s.Test == false).Select(x => new { x.StudentId, x.Name });
                if (students != null)
                {
                    ddStudents.DataSource = students.OrderBy(s => s.Name).ToList();
                    ddStudents.DataTextField = "Name";
                    ddStudents.DataValueField = "StudentId";
                    ddStudents.DataBind();
                    ddStudents.Items.Insert(0, new ListItem("--All Students--", ""));
                }
            }
        }
        protected void ddCourses_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropdownStudent();
            LoadStudentTotalGrade();
        }
        protected void ddStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentTotalGrade();
        }
        private void LoadStudentTotalGrade()
        {
            if (ddCourses.SelectedValue != "")
            {
                int courseInstanceId = Convert.ToInt32(ddCourses.SelectedValue);
                CourseInstance courseInstance = data.CourseInstances.Find(courseInstanceId);
                System.Collections.Generic.IEnumerable<Student> students = from s in courseInstance.Students.Where(s => !s.Test.HasValue || s.Test == false).OrderBy(s => s.Name) select s;

                if (ddStudents.SelectedValue != "")
                {
                    int studentId = Convert.ToInt32(ddStudents.SelectedValue);
                    students = from s in students.Where(x => x.StudentId == studentId) select s;
                }

                foreach (Student student in students)
                {
                    StudentGraddeControLoad(student, courseInstance);
                }
            }
        }
    }

}