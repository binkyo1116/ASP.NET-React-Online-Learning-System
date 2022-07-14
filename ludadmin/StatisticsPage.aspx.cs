using EFModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class StatisticsPage : System.Web.UI.Page
    {
        MaterialEntities data;
        Student student;
        Course course;
        protected void Page_Load(object sender, EventArgs e)
        {
            //data = (MaterialEntities)Session["Data"];
            //student = (Student)Session["Student"];
            //course = (Course)Session["Course"];

            //if (!Page.IsPostBack)
            //{
            //    if ((course != null && student.UserName != "admin") || (student.UserName != "admin" && student.Courses.Count() == 1))
            //    {
            //        PanelCourseList.Visible = false;
            //        StatisticData();
            //    }
            //    else
            //    {
            //        BindDropDown();
            //    }
            //}
        }
        private void BindDropDown()
        {
            //IEnumerable<Course> courseList = new List<Course>();
            //-----------------------For Admin---------------------------------------
            //if (student.UserName == "admin")
            //{
            //    PanelStudentList.Visible = true;
            //    courseList = data.Courses;
            //}
            //else
            //{
            //    courseList = student.Courses;
            //}
            //-------------------------------------------------------------------------
            //-----------Course List-------------------------
            //ddnCourses.DataSource = courseList.ToList();
            //ddnCourses.DataTextField = "Name";
            //ddnCourses.DataValueField = "CourseId";
            //ddnCourses.DataBind();
            //ddnCourses.Items.Insert(0, new ListItem("--Select a Course--", "0"));
        }
        private void BindingStudentList()
        {
            //var courseId = Convert.ToInt32(ddnCourses.SelectedValue);
            ////-----------Students List for quiz-----------------------
            //DropDownListStudent.DataSource = data.Courses.Where(x => x.CourseId == courseId).FirstOrDefault().Students.ToList();
            //DropDownListStudent.DataTextField = "Name";
            //DropDownListStudent.DataValueField = "StudentId";
            //DropDownListStudent.DataBind();
            //DropDownListStudent.Items.Insert(0, new ListItem("--Select a Student--", "0"));
        }
        //============================Statistics for Quiz ================================
        //private void StatisticData()
        //{
            //-------------------For Admin-------------------------
            //Student individualStudent = new Student();
            //var courseName = "";
            //int courseId = 0;
            //int SelectedstudentId = DropDownListStudent.SelectedValue!=""? Convert.ToInt32(DropDownListStudent.SelectedValue):0;
            //if (student.UserName == "admin")
            //{
            //    individualStudent = data.Students.Where(x => x.StudentId == SelectedstudentId).FirstOrDefault();
            //    courseName = ddnCourses.SelectedItem.Text;
            //    courseId = Convert.ToInt32(ddnCourses.SelectedValue);
            //}
            //else
            //{
            //    individualStudent = student;
            //    if (course != null)
            //    {
            //        courseName = course.Name;
            //        courseId = course.CourseId;
            //    }
            //    else if (student.Courses.Count() == 1)
            //    {
            //        var stc = student.Courses.FirstOrDefault();
            //        courseName = stc.Name;
            //        courseId = stc.CourseId;
            //    }
            //    else
            //    {
            //        courseName = ddnCourses.SelectedItem.Text;
            //        courseId = Convert.ToInt32(ddnCourses.SelectedValue);
            //    }
            //}
            //------------------------------------------------------
            //List<ModuleGraphData> quizdata = new List<ModuleGraphData>();
            //List<ModuleGraphData> assessmentGrade = new List<ModuleGraphData>();
            //var activities = new List<Activity>();
            //var activities = (from q in data.Activities
            //                  join mo in data.ModuleObjectives on q.ModuleObjectiveId equals mo.Id
            //                  join m in data.Modules on q.ModuleId equals m.Id

            //                  where q.CourseId == courseId
            //                  && (!q.Active.HasValue || q.Active.Value)
            //                  && mo.Active == true
            //                  && mo.Module.CourseObjective.Active == true
            //                  && q.ModuleObjective.Module.Active == true

            //                  select q);
            //----------Quiz-------------------

            //var quiz = (from q in activities
            //            where q.ActivitiyGrades.Count > 0
            //            && q.ModuleObjective.Module.DueDate <= DateTime.Now
            //            && (q.ModuleObjective.Module.Description.ToLower() != "midterm" && q.ModuleObjective.Module.Description.ToLower() != "final")
            //            select new ModuleGraphData
            //            {
            //                label = q.ModuleObjective.Module.Description.Length > 8 ? q.ModuleObjective.Module.Description.Substring(0, 9) : q.ModuleObjective.Module.Description + " - MO " + q.ModuleObjectiveId + " - " + q.Title,
            //                quizList = q.ActivitiyGrades.Where(sg => (!sg.Student.Test.HasValue || !sg.Student.Test.Value))
            //                .Select(x => (int)((double)x.Grade / x.Activity.MaxGrade * 100)).ToList(),
            //                individualGrade = q.ActivitiyGrades.Where(x => x.StudentId == individualStudent.StudentId
            //                && (!x.Student.Test.HasValue || !x.Student.Test.Value)).Select(y => (int)((double)y.Grade / y.Activity.MaxGrade * 100)).FirstOrDefault()
            //            });

            //quizdata.AddRange(quiz);

            //---------Assessment-------------
            //var assessments = (from ass in data.Assessments
            //                   where ass.CourseId == courseId
            //                   && (!ass.Active.HasValue || ass.Active.Value)
            //                   && ass.ModuleObjective.Active == true
            //                   && ass.ModuleObjective.Module.Active == true
            //                   && ass.ModuleObjective.Module.CourseObjective.Active == true
            //                   select ass);

            //var assm = (from ass in assessments
            //            where ass.StudentGradables.Count > 0
            //            && ass.ModuleObjective.Module.DueDate <= DateTime.Now
            //            && (ass.ModuleObjective.Module.Description.ToLower() != "midterm" && ass.ModuleObjective.Module.Description.ToLower() != "final")
            //            select new ModuleGraphData
            //            {
            //                label = ass.ModuleObjective.Module.Description.Length > 8 ? ass.ModuleObjective.Module.Description.Substring(0, 9) : ass.ModuleObjective.Module.Description + " - MO " + ass.ModuleObjetiveId + " - " + (ass.Title.Length > 15 ? ass.Title.Substring(0, 12) + ".." : ass.Title),
            //                quizList = ass.StudentGradables.Where(sg => (!sg.Student.Test.HasValue || !sg.Student.Test.Value)).Select(x => x.Grade).ToList(),
            //                individualGrade = ass.StudentGradables.Where(x => x.StudentId == individualStudent.StudentId
            //                && (!x.Student.Test.HasValue || !x.Student.Test.Value)).Select(y => y.Grade).FirstOrDefault()
            //            });
            //assessmentGrade.AddRange(assm);

            //===========================================================================================================
            //--------------------------------Calculate students current and overall grade------------------------------
            //the overall grade is: 45% assessments + 10% quizzes + 20% midterm + 25% final
            //---------------Calculate assessments Current Avarage grade-------------------------
            //PanelOverallGrade.Visible = true;
            //var individualAssessment = assessmentGrade.Select(x => x.individualGrade);
            //int assAverageGrade = 0;
            //if (individualAssessment.Any())
            //{
            //    assAverageGrade = (int)((double)individualAssessment.Sum() / (individualAssessment.Count() * 100) * 45);
            //}
            //LabelCurrentAssessmentGrade.Text = assAverageGrade + "%";

            //---------------Calculate quizzes Current Avarage grade-------------------------
            //var individualQuiz = quizdata.Select(x => x.individualGrade);
            //int quizAverageGrade = 0;
            //if (individualQuiz.Any())
            //{
            //    quizAverageGrade = (int)((double)individualQuiz.Sum() / (individualQuiz.Count() * 100) * 10);
            //}
            //LabelCurrentQuizGrade.Text = quizAverageGrade + "%";

            //---------------Calculate Midterm Current Avarage grade---------------------------------------------------
            //int midtermAverageGrade = 0;
            //int midtermOverallGrade = 0;
            //var midterm = data.Modules.Where(x => x.Active == true && x.CourseId == courseId && x.Description.ToLower() == "midterm").FirstOrDefault();
            //if (midterm != null)
            //{
                //var MidterCurrentGradeforQuiz = (from q in activities
                //                                 where q.ModuleObjective.Module.Description.ToLower() == "midterm"
                //                                 select new ModuleGraphData
                //                                 {
                //                                     individualGrade = q.ActivitiyGrades.Where(x => x.StudentId == individualStudent.StudentId
                //                            && (!x.Student.Test.HasValue || !x.Student.Test.Value)).Select(y => (int)((double)y.Grade / y.Activity.MaxGrade * 100)).FirstOrDefault()
                //                                 }).ToList();

                //var MidterCurrentGradeforAssessment = (from ass in assessments
                //                                       where (ass.ModuleObjective.Module.Description.ToLower() == "midterm")
                //                                       select new ModuleGraphData
                //                                       {
                //                                           individualGrade = ass.StudentGradables.Where(x => x.StudentId == individualStudent.StudentId
                //                                         && (!x.Student.Test.HasValue || !x.Student.Test.Value)).Select(y => y.Grade).FirstOrDefault()
                //                                       }).ToList();
                //var midTAss = MidterCurrentGradeforAssessment.Select(x => x.individualGrade);
                //var midTQuix = MidterCurrentGradeforQuiz.Select(x => x.individualGrade);
                //var countMidtermGrade = midTAss.Count() + midTQuix.Count();
                //if (countMidtermGrade > 0)
                //{
                //    midtermAverageGrade = (int)((double)(midTAss.Sum() + midTQuix.Sum()) / (countMidtermGrade * 100) * 20);
                //}

            //    if (midterm.DueDate <= DateTime.Now || midtermAverageGrade > 0)
            //    {
            //        midtermOverallGrade = 20;
            //    }
            //    else
            //    {
            //        midtermOverallGrade = 0;
            //    }
            //}
            //LabelOverallMidterm.Text = midtermOverallGrade + "%";
            //LabelCurrentMidtermGrade.Text = midtermAverageGrade + "%";
            //------------------------------------------------------------------------------------------------------
            //---------------Calculate Final Current Avarage grade---------------------------------------------------
            //int finalOverallGrade = 0;
            //int finalAverageGrade = 0;
            //var final = data.Modules.Where(x => x.Active == true && x.CourseId == courseId && x.Description.ToLower() == "final").FirstOrDefault();
            //if (final != null)
            //{

                //var FinalCurrentGradeforQuiz = (from q in activities
                //                                where q.ModuleObjective.Module.Description.ToLower() == "final"
                //                                select new ModuleGraphData
                //                                {
                //                                    individualGrade = q.ActivitiyGrades.Where(x => x.StudentId == individualStudent.StudentId
                //                           && (!x.Student.Test.HasValue || !x.Student.Test.Value)).Select(y => (int)((double)y.Grade / y.Activity.MaxGrade * 100)).FirstOrDefault()
                //                                }).ToList();

                //var FinalCurrentGradeforAssessment = (from ass in assessments
                //                                      where (ass.ModuleObjective.Module.Description.ToLower() == "final")
                //                                      select new ModuleGraphData
                //                                      {
                //                                          individualGrade = ass.StudentGradables.Where(x => x.StudentId == individualStudent.StudentId
                //                                        && (!x.Student.Test.HasValue || !x.Student.Test.Value)).Select(y => y.Grade).FirstOrDefault()
                //                                      }).ToList();

                //var finalAss = FinalCurrentGradeforAssessment.Select(x => x.individualGrade);
                //var finalQuix = FinalCurrentGradeforQuiz.Select(x => x.individualGrade);
                //var countFinalGrade = finalAss.Count() + finalQuix.Count();

                //if (countFinalGrade > 0)
                //{
                //    finalAverageGrade = (int)((double)(finalAss.Sum() + finalQuix.Sum()) / (countFinalGrade * 100) * 25);
                //}

            //    if (final.DueDate <= DateTime.Now || finalAverageGrade > 0)
            //    {
            //        finalOverallGrade = 25;
            //    }
            //    else
            //    {
            //        finalOverallGrade = 0;
            //    }
            //}
            //LabelOverallFinal.Text = finalOverallGrade + "%";
            //LabelCurrentFinalGrade.Text = finalAverageGrade + "%";

            //---------------------------------------Total Grade--------------------------------------------------
            //int averageTotal = 0;
            //var totalGradeInPercent = assAverageGrade + quizAverageGrade + midtermAverageGrade + finalAverageGrade;
            //if (totalGradeInPercent > 0)
            //{
            //    averageTotal = (int)(((double)totalGradeInPercent / (55 + midtermOverallGrade + finalOverallGrade) * 100));
            //}

            //LabelTotalGrade.Text = averageTotal + "%";

            //--------------------Calculate GPA---------------------------
            //var gradeScale = (from a in data.CourseGradeScales
            //                  where a.CourseId == courseId
            //                  select a.GradeScaleGroup.GradeScales).FirstOrDefault();
            //var gpa = gradeScale.Where(x => x.MaxNumberInPercent >= averageTotal && x.MinNumberInPercent <= averageTotal).FirstOrDefault();
            //if (gpa != null)
            //{
            //    LabelGPA.Text = Convert.ToString(gpa.GPA);
            //}
            //----------------------------------------------------------------------------------------------------------
            //=====================JS Serializer====================
            //var serializer = new JavaScriptSerializer();
            //var course_name = "";
            //if (student.UserName == "admin")
            //{
            //    course_name = serializer.Serialize("Course Title: " + courseName + ", Student Name: " + individualStudent.Name);
            //}
            //else
            //{
            //    course_name = serializer.Serialize("Course Title: " + courseName);
            //}
            //var quiz_sample_data = serializer.Serialize(quizdata);
            //var assessment_sample_data = serializer.Serialize(assessmentGrade);
            // add script tag to the page
        //    this.ClientScript.RegisterClientScriptBlock(this.GetType(),
        //         "courseName", "<script>var course_name = " + course_name + ", Student;</script>");

        //    this.ClientScript.RegisterClientScriptBlock(this.GetType(),
        //        "mydata", "<script>var statistic_data_grade = " + quiz_sample_data + ";</script>");

        //    this.ClientScript.RegisterClientScriptBlock(this.GetType(),
        //      "mydataAss", "<script>var statistic_data_assessment = " + assessment_sample_data + ";</script>");
        //}
        //===========================Statistics for Assessment ===========================
        //protected void ddnCourses_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (student.UserName == "admin")
        //    {
        //        BindingStudentList();
        //        PanelOverallGrade.Visible = false;
        //    }
        //    else
        //    {
        //        if (ddnCourses.SelectedValue != "0")
        //        {
        //            StatisticData();
        //        }
        //    }
        //}

        //protected void DropDownListStudent_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddnCourses.SelectedValue != "0")
        //    {
        //        StatisticData();
        //    }
        //}
    }
    class ModuleGraphData
    {
        public string label { get; set; }
        public int? individualGrade { get; set; }
        public List<int> quizList { get; set; }
    }
}