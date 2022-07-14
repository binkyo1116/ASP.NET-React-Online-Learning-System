using AdminPages.Services;
using EFModel;
using LMSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class GradeBook : System.Web.UI.Page
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
        private void BindDropdownModuleObjective()
        {
            if (ddCourses.SelectedValue != "")
            {
                int courseInstanceId = Convert.ToInt32(ddCourses.SelectedValue);
                int courseId = data.CourseInstances.Find(courseInstanceId).CourseId;
                IQueryable<ModuleObjective> modelObjectives = from a in data.ModuleObjectives.Where(x => x.Active && x.Modules.Where(m => m.Active && m.CourseObjectives.Where(co => co.Active && co.Courses.Where(c => c.Id == courseId).Any()).Any()).Any())
                                                              select a;

                var model1 = (from a in modelObjectives
                              join cia in data.CourseInstanceActivities.Where(x => x.CourseInstanceId == courseInstanceId) on a.Id equals cia.ModuleObjectiveId
                              join b in data.ActivityGrades on cia.ActivityId equals b.ActivityId
                              select new { a.Id, a.Description }).Distinct();

                var model2 = (from a in modelObjectives
                              join c in data.CourseInstanceCodingProblems on a.Id equals c.ModuleObjectiveId
                              join n in data.StudentGradables on c.CodingProblemId equals n.CodingProblemId
                              where n.CourseInstanceId == courseInstanceId && c.CourseInstanceId == courseInstanceId
                              select new { c.ModuleObjective.Id, c.ModuleObjective.Description }).Distinct();

                var model3 = model1.Union(model2);
                DropDownListModuleObjective.DataSource = model3.ToList();
                DropDownListModuleObjective.DataTextField = "Description";
                DropDownListModuleObjective.DataValueField = "Id";
                DropDownListModuleObjective.DataBind();
                DropDownListModuleObjective.Items.Insert(0, new ListItem("--Select one--", ""));
            }
        }
        private void BindDropdownActivity()
        {
            if (DropDownListModuleObjective.SelectedValue != "")
            {
                int courseInstanceId = Convert.ToInt32(ddCourses.SelectedValue);
                int courseId = data.CourseInstances.Find(courseInstanceId).CourseId;
                int modelObjectiveId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
                IQueryable<ModuleObjective> modelObjectives = from a in data.ModuleObjectives.Where(x => x.Id == modelObjectiveId && x.Active && x.Modules.Where(m => m.Active && m.CourseObjectives.Where(co => co.Active && co.Courses.Where(c => c.Id == courseId).Any()).Any()).Any())
                                                              select a;

                var model = (from a in modelObjectives
                             join cia in data.CourseInstanceActivities.Where(x => x.CourseInstanceId == courseInstanceId) on a.Id equals cia.ModuleObjectiveId
                             join b in data.ActivityGrades on cia.ActivityId equals b.ActivityId
                             select new { cia.Activity.Id, cia.Activity.Title }).Distinct().ToList();

                DropDownListActivity.DataSource = model.ToList();
                DropDownListActivity.DataTextField = "Title";
                DropDownListActivity.DataValueField = "Id";
                DropDownListActivity.DataBind();
                DropDownListActivity.Items.Insert(0, new ListItem("--Select one--", ""));
            }
            else
            {
                DropDownListActivity.Items.Clear();
            }
        }
        private void BindDropdownAssessment()
        {
            if (DropDownListModuleObjective.SelectedValue != "")
            {
                int courseInstanceId = Convert.ToInt32(ddCourses.SelectedValue);
                int courseId = data.CourseInstances.Find(courseInstanceId).CourseId;
                int modelObjectiveId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
                IQueryable<ModuleObjective> modelObjectives = from a in data.ModuleObjectives.Where(x => x.Id == modelObjectiveId && x.Active && x.Modules.Where(m => m.Active && m.CourseObjectives.Where(co => co.Active && co.Courses.Where(c => c.Id == courseId).Any()).Any()).Any())
                                                              select a;


                var model = (from a in modelObjectives
                             join c in data.CourseInstanceCodingProblems on a.Id equals c.ModuleObjectiveId
                             join n in data.StudentGradables on c.CodingProblemId equals n.CodingProblemId
                             where n.CourseInstanceId == courseInstanceId && c.CourseInstanceId == courseInstanceId
                             select new { Title = c.CodingProblem.Title, Id = c.CodingProblemId }).Distinct();

                DropDownListAssessment.DataSource = model.ToList();
                DropDownListAssessment.DataTextField = "Title";
                DropDownListAssessment.DataValueField = "Id";
                DropDownListAssessment.DataBind();
                DropDownListAssessment.Items.Insert(0, new ListItem("--Select one--", ""));
            }
            else
            {
                DropDownListAssessment.Items.Clear();
            }
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
                    ddStudents.Items.Insert(0, new ListItem("--Select one--", ""));
                }
            }
        }
        private void LoadGrid()
        {
            if (ddCourses.SelectedValue != "")
            {
                int courseInstanceId = Convert.ToInt32(ddCourses.SelectedValue);
                int courseId = data.CourseInstances.Find(courseInstanceId).CourseId;

                IQueryable<Student> students = from s in data.Students.Where(s => !s.Test.HasValue || s.Test == false) select s;
                IQueryable<ModuleObjective> moduleObjectives = from a in data.ModuleObjectives.Where(x => x.Active && x.Modules.Where(m => m.Active && m.CourseObjectives.Where(co => co.Active && co.Courses.Where(c => c.Id == courseId).Any()).Any()).Any())
                                                               select a;
                IQueryable<CourseInstanceActivity> courseInstanceActivity = from cia in data.CourseInstanceActivities
                                                                            where cia.CourseInstanceId == courseInstanceId
                                                                            select cia;

                IQueryable<CourseInstanceCodingProblem> codingProblems = from a in data.CourseInstanceCodingProblems.Where(x => x.CourseInstanceId == courseInstanceId) select a;

                if (ddStudents.SelectedValue != "")
                {
                    int studentId = Convert.ToInt32(ddStudents.SelectedValue);
                    students = from s in students.Where(x => x.StudentId == studentId) select s;
                }
                if (DropDownListModuleObjective.SelectedValue != "")
                {
                    int moId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
                    courseInstanceActivity = from cia in courseInstanceActivity.Where(x => x.ModuleObjectiveId == moId) select cia;
                    codingProblems = from cp in codingProblems.Where(x => x.ModuleObjectiveId == moId) select cp;
                }
                //----------------------Activity Grade-----------------------
                List<ActivityGrade> model = (from a in moduleObjectives
                                             join ci in courseInstanceActivity on a.Id equals ci.ModuleObjectiveId
                                             join b in data.ActivityGrades on ci.ActivityId equals b.ActivityId
                                             join student in students on b.StudentId equals student.StudentId
                                             orderby a.Id, ci.Activity.Title, student.Name
                                             select new ActivityGrade { ModuleObjective = a.Description, Activity = ci.Activity.Title, Student = student.Name, Grade = b.Grade, MaxGrade = b.MaxGrade }).ToList();
                grdActivites.DataSource = model;
                grdActivites.DataBind();

                //----------------------Coding Problem Grade------------------

                var codingProblemGrades =
                    from a in moduleObjectives
                    join codingProblem in codingProblems on a.Id equals codingProblem.ModuleObjectiveId
                    join sg in data.StudentGradables on codingProblem.CodingProblemId equals sg.CodingProblemId
                    join student in students on sg.StudentId equals student.StudentId
                    where sg.CourseInstanceId == courseInstanceId
                    orderby a.Id, codingProblem.CodingProblem.Title, student.Name
                    select new { ModuleObjective = a.Description, Assessment = codingProblem.CodingProblem.Title, Student = student.Name, Grade = sg.Grade, MaxGrade = sg.MaxGrade };

                grdAssessments.DataSource = codingProblemGrades.ToList();
                grdAssessments.DataBind();
            }
            else
            {
                ddStudents.SelectedIndex = -1;
                grdAssessments.DataSource = null;
                grdAssessments.DataBind();

                grdActivites.DataSource = null;
                grdActivites.DataBind();
            }
        }

        private void LoadGridActivity()
        {
            if (ddCourses.SelectedValue != "" && DropDownListModuleObjective.SelectedValue != "" && DropDownListActivity.SelectedValue != "")
            {
                int moId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
                int activityId = Convert.ToInt32(DropDownListActivity.SelectedValue);
                int courseInstanceId = Convert.ToInt32(ddCourses.SelectedValue);
                int courseId = data.CourseInstances.Find(courseInstanceId).CourseId;

                IQueryable<Student> students = from s in data.Students.Where(s => !s.Test.HasValue || s.Test == false) select s;
                IQueryable<ModuleObjective> moduleObjectives = from a in data.ModuleObjectives.Where(x => x.Id == moId && x.Active && x.Modules.Where(m => m.Active && m.CourseObjectives.Where(co => co.Active && co.Courses.Where(c => c.Id == courseId).Any()).Any()).Any())
                                                               select a;
                IQueryable<CourseInstanceActivity> courseInstanceActivity = from cia in data.CourseInstanceActivities
                                                                            where cia.CourseInstanceId == courseInstanceId
                                                                            && cia.ModuleObjectiveId == moId
                                                                            select cia;

                if (ddStudents.SelectedValue != "")
                {
                    int studentId = Convert.ToInt32(ddStudents.SelectedValue);
                    students = from s in students.Where(x => x.StudentId == studentId) select s;
                }
                //----------------------Activity Grade-----------------------
                List<ActivityGrade> model = (from a in moduleObjectives
                                             join ci in courseInstanceActivity on a.Id equals ci.ModuleObjectiveId
                                             join b in data.ActivityGrades on ci.ActivityId equals b.ActivityId
                                             join student in students on b.StudentId equals student.StudentId
                                             where ci.ActivityId == activityId
                                             orderby a.Id, ci.Activity.Title, student.Name
                                             select new ActivityGrade { ModuleObjective = a.Description, Activity = ci.Activity.Title, Student = student.Name, Grade = b.Grade, MaxGrade = b.MaxGrade }).ToList();

                grdActivites.DataSource = model;
                grdActivites.DataBind();
            }
        }

        private void LoadGridAssessment()
        {
            if (ddCourses.SelectedValue != "" && DropDownListModuleObjective.SelectedValue != "" && DropDownListAssessment.SelectedValue != "")
            {
                int courseInstanceId = Convert.ToInt32(ddCourses.SelectedValue);
                int courseId = data.CourseInstances.Find(courseInstanceId).CourseId;
                int assessmentId = Convert.ToInt32(DropDownListAssessment.SelectedValue);
                int moId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);

                IQueryable<Student> students = from s in data.Students.Where(s => !s.Test.HasValue || s.Test == false) select s;
                IQueryable<ModuleObjective> moduleObjectives = from a in data.ModuleObjectives.Where(x => x.Id == moId && x.Active && x.Modules.Where(m => m.Active && m.CourseObjectives.Where(co => co.Active && co.Courses.Where(c => c.Id == courseId).Any()).Any()).Any())
                                                               select a;

                IQueryable<CourseInstanceCodingProblem> codingProblems = from a in data.CourseInstanceCodingProblems
                                                                         where a.CourseInstanceId == courseInstanceId
                                                                                 && a.ModuleObjectiveId == moId
                                                                         select a;

                if (ddStudents.SelectedValue != "")
                {
                    int studentId = Convert.ToInt32(ddStudents.SelectedValue);
                    students = from s in students.Where(x => x.StudentId == studentId) select s;
                }

                //----------------------Coding Problem Grade------------------

                var codingProblemGrades =
                    from a in moduleObjectives
                    join codingProblem in codingProblems on a.Id equals codingProblem.ModuleObjectiveId
                    join sg in data.StudentGradables on codingProblem.CodingProblemId equals sg.CodingProblemId
                    join student in students on sg.StudentId equals student.StudentId
                    where sg.CourseInstanceId == courseInstanceId
                    && codingProblem.CodingProblemId == assessmentId
                    orderby a.Id, codingProblem.CodingProblem.Title, student.Name
                    select new { ModuleObjective = a.Description, Assessment = codingProblem.CodingProblem.Title, Student = student.Name, Grade = sg.Grade, MaxGrade = sg.MaxGrade };

                grdAssessments.DataSource = codingProblemGrades.ToList();
                grdAssessments.DataBind();
            }
        }

        protected void ddCourses_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropdownStudent();
            BindDropdownModuleObjective();
            LoadGrid();
            LoadStudentTotalGrade();
        }

        protected void DropDownListActivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGridActivity();
        }

        protected void DropDownListAssessment_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGridAssessment();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //check if the row is the header row
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //add the thead and tbody section programatically
                e.Row.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void DropDownListModuleObjective_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
            BindDropdownActivity();
            BindDropdownAssessment();
        }

        protected void ddStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
            LoadStudentTotalGrade();
        }
        private void LoadStudentTotalGrade()
        {
            if (ddCourses.SelectedValue != "")
            {
                int courseInstanceId = Convert.ToInt32(ddCourses.SelectedValue);
                IEnumerable<Student> students = from s in data.CourseInstances.Find(courseInstanceId).Students.Where(s => !s.Test.HasValue || s.Test == false) select s;

                if (ddStudents.SelectedValue != "")
                {
                    int studentId = Convert.ToInt32(ddStudents.SelectedValue);
                    students = from s in students.Where(x => x.StudentId == studentId) select s;
                }

                //----------------------------------------------------
                List<StudentTotalGrade> studentTotalGrade = new List<StudentTotalGrade>();
                //--------------Total Grade-------------------------- 
                foreach (Student student in students)
                {
                    CourseInstance courseInstance = student.CourseInstances.Where(ci => ci.Id == courseInstanceId).FirstOrDefault();
                    if (courseInstance != null)
                    {
                        Gradebook courseGradebook = GradeBookService.GetGradebook(data, student, courseInstance);
                        int totalGrade = (int)Math.Round(courseGradebook.CalculateWeightedGrade());

                        StudentTotalGrade stg = new StudentTotalGrade { StudentName = student.Name, TotalGrade = totalGrade + "%" };
                        studentTotalGrade.Add(stg);
                    }
                }
                grdTotalGrade.DataSource = studentTotalGrade.ToList();
                grdTotalGrade.DataBind();
            }
        }
    }
    public class ActivityGrade
    {
        public string ModuleObjective { get; set; }
        public string Activity { get; set; }
        public string Student { get; set; }
        public int Grade { get; set; }
        public int MaxGrade { get; set; }
    }
    public class StudentTotalGrade
    {
        public string StudentName { get; set; }
        public string TotalGrade { get; set; }
    }
}