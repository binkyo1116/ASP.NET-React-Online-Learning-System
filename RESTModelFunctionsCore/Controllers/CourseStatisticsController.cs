using System.Text;
using LMS.Common.ViewModels;
using LMSLibrary;
using RESTModelFunctionsCore.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseStatisticsController : ControllerBase
    {
        [HttpPost]
        public ActionResult<CourseStatisticsResultInfo> Post([FromBody] CourseStatisticsStudentInfo si)
        {
            VmStudent student = VmModelHelper.GetStudentInfoByHash(si.Hash);
            VmCourseInstance courseInstance = VmModelHelper.GetCourseInstanceById(si.CourseInstanceId);
            VmGradeScale gradeScale = GetGradeScale(courseInstance.Id);
            Gradebook courseGradebook = GetGradebook(student, courseInstance);
            int totalGrade = (int)Math.Round(courseGradebook.CalculateWeightedGrade());
            int totalCompletion = (int)Math.Round(courseGradebook.CalculateTotalCompletion());
            double totalGPA = GetGPAByPercent(totalGrade, gradeScale);
            int totalWeight = courseGradebook.GetTotalWeight();
            double totalWeightedGrade = GradingHelper.CalculateWeightedGrade(totalGrade, totalWeight);
            int totalActualGrade = GetCurrentGrade(student, courseInstance);

            VmCourse course = VmModelHelper.GetCourseById(courseInstance.CourseId);
            CourseStatisticsResultInfo ri = new CourseStatisticsResultInfo()
            {
                CourseName = course.Name,
                Assessment = new CourseStatisticsActivityInfo()
                {
                    Weight = courseGradebook.Assessment.Weight,
                    Grade = courseGradebook.Assessment.Grade,
                    WeightedGrade = GradingHelper.CalculateWeightedGrade(courseGradebook.Assessment.Grade, courseGradebook.Assessment.Weight),
                    Completion = courseGradebook.Assessment.Completion,
                    GPA = GetGPAByPercent(courseGradebook.Assessment.Grade, gradeScale)

                },
                Quiz = new CourseStatisticsActivityInfo()
                {
                    Weight = courseGradebook.Quiz.Weight,
                    Grade = courseGradebook.Quiz.Grade,
                    WeightedGrade = GradingHelper.CalculateWeightedGrade(courseGradebook.Quiz.Grade, courseGradebook.Quiz.Weight),
                    Completion = courseGradebook.Quiz.Completion,
                    GPA = GetGPAByPercent(courseGradebook.Quiz.Grade, gradeScale)

                },
                Material = new CourseStatisticsActivityInfo()
                {
                    Weight = courseGradebook.Material.Weight,
                    Grade = courseGradebook.Material.Grade,
                    WeightedGrade = GradingHelper.CalculateWeightedGrade(courseGradebook.Material.Grade, courseGradebook.Material.Weight),
                    Completion = courseGradebook.Material.Completion,
                    GPA = GetGPAByPercent(courseGradebook.Material.Grade, gradeScale)

                },
                Midterm = new CourseStatisticsActivityInfo()
                {
                    Weight = courseGradebook.Midterm.Weight,
                    Grade = courseGradebook.Midterm.Grade,
                    WeightedGrade = GradingHelper.CalculateWeightedGrade(courseGradebook.Midterm.Grade, courseGradebook.Midterm.Weight),
                    Completion = courseGradebook.Midterm.Completion,
                    GPA = GetGPAByPercent(courseGradebook.Midterm.Grade, gradeScale)
                },
                Final = new CourseStatisticsActivityInfo()
                {
                    Weight = courseGradebook.Final.Weight,
                    Grade = courseGradebook.Final.Grade,
                    WeightedGrade = GradingHelper.CalculateWeightedGrade(courseGradebook.Final.Grade, courseGradebook.Final.Weight),
                    Completion = courseGradebook.Final.Completion,
                    GPA = GetGPAByPercent(courseGradebook.Final.Grade, gradeScale)
                },
                Poll = new CourseStatisticsActivityInfo()
                {
                    Weight = courseGradebook.Poll.Weight,
                    Grade = courseGradebook.Poll.Grade,
                    WeightedGrade = GradingHelper.CalculateWeightedGrade(courseGradebook.Poll.Grade, courseGradebook.Poll.Weight),
                    Completion = courseGradebook.Poll.Completion,
                    GPA = GetGPAByPercent(courseGradebook.Poll.Grade, gradeScale)
                },
                Discussion = new CourseStatisticsActivityInfo()
                {
                    Weight = courseGradebook.Discussion.Weight,
                    Grade = courseGradebook.Discussion.Grade,
                    WeightedGrade = GradingHelper.CalculateWeightedGrade(courseGradebook.Discussion.Grade, courseGradebook.Discussion.Weight),
                    Completion = courseGradebook.Discussion.Completion,
                    GPA = GetGPAByPercent(courseGradebook.Discussion.Grade, gradeScale)
                },
                Total = new CourseStatisticsActivityInfo()
                {
                    Weight = totalWeight,
                    Grade = totalGrade,
                    WeightedGrade = 10,
                    Completion = totalCompletion,
                    GPA = totalGPA,
                    CurrentGrade = totalActualGrade,
                    CurrentGPA = GetGPAByPercent(totalGrade, gradeScale)
                }
            };
            return Ok(ri);
        }
        private VmGradeScale GetGradeScale(int courseInstanceId)
        {
            string sqlQueryGradeScale = $@"select gs.Id, gs.GradeScaleGroupId,gs.MaxNumberInPercent,gs.MinNumberInPercent,gs.GPA from GradeScale gs
                                            inner join GradeScaleGroup gsg on gs.GradeScaleGroupId = gsg.Id
                                            inner join Course c on gsg.Id = c.GradeScaleGroupId
                                            inner join CourseInstance ci on ci.CourseId = c.Id
                                            where ci.Id = {courseInstanceId}";

            var gradeScaleData = SQLHelper.RunSqlQuery(sqlQueryGradeScale);
            VmGradeScale gradeScaleinfo = null;
            if (gradeScaleData.Count > 0)
            {
                List<object> st = gradeScaleData[0];
                gradeScaleinfo = new VmGradeScale
                {
                    Id = (int)st[0],
                    GradeScaleGroupId = (int)st[1],
                    MaxNumberInPercent = (double)st[2],
                    MinNumberInPercent = (double)st[3],
                    GPA = (double)st[4]
                };
            }
            return gradeScaleinfo;
        }
        private static CourseGradeResult GetCourseGrade(int studentId, int courseInstanceId)
        {
            string sqlQueryCourseGrade = $@"exec CourseGrade {studentId}, {courseInstanceId}";
            var courseGradeData = SQLHelper.RunSqlQuery(sqlQueryCourseGrade);
            CourseGradeResult courseGradeinfo = null;
            if (courseGradeData.Count > 0)
            {
                List<object> st = courseGradeData[0];

                courseGradeinfo = new CourseGradeResult
                {
                    ActivityGrade = st[0].ToString(),
                    AssessmentGrade = st[1].ToString(),
                    FinalGrade = st[2].ToString(),
                    MidtermGrade = st[3].ToString(),
                    PollGrade = st[4].ToString(),
                    DiscussionGrade = st[5].ToString()
                };
            }
            return courseGradeinfo;
        }
        private static VmGradeWeight GetGradeWeight(int courseInstanceId)
        {
            string sqlQueryGradeWeight = $@"select CourseInstanceId,Id,ActivityWeight,AssessmentWeight,MaterialWeight,
                                            DiscussionWeight,PollWeight,MidtermWeight,FinalWeight 
                                            from GradeWeight where CourseInstanceId = {courseInstanceId}";

            var gradeWeightData = SQLHelper.RunSqlQuery(sqlQueryGradeWeight);
            VmGradeWeight gradeWeightinfo = null;
            if (gradeWeightData.Count > 0)
            {
                List<object> st = gradeWeightData[0];
                gradeWeightinfo = new VmGradeWeight
                {
                    CourseInstanceId = (int)st[0],
                    Id = (int)st[1],
                    ActivityWeight = (int)st[2],
                    AssessmentWeight = (int)st[3],
                    MaterialWeight = (int)st[4],
                    DiscussionWeight = (int)st[5],
                    PollWeight = (int)st[6],
                    MidtermWeight = (int)st[7],
                    FinalWeight = (int)st[8]
                };
            }
            return gradeWeightinfo;
        }
        private static CourseGradeCurrentResult GetCourseGradeCurrent(int studentId, int courseInstanceId)
        {
            string sqlQueryCourseGradeCurrent = $@"exec CourseGradeCurrent {studentId}, {courseInstanceId}";

            var courseGradeCurrentData = SQLHelper.RunSqlQuery(sqlQueryCourseGradeCurrent);
            CourseGradeCurrentResult courseGradeCurrentinfo = null;
            if (courseGradeCurrentData.Count > 0)
            {
                List<object> st = courseGradeCurrentData[0];
                courseGradeCurrentinfo = new CourseGradeCurrentResult
                {
                    ActivityGrade = st[0].ToString(),
                    AssessmentGrade = st[1].ToString(),
                    FinalGrade = st[2].ToString(),
                    MidtermGrade = st[3].ToString(),
                    PollGrade = st[4].ToString(),
                    DiscussionGrade = st[5].ToString()
                };
            }
            return courseGradeCurrentinfo;
        }
        private double GetGPAByPercent(double percent, VmGradeScale cgs)
        {
            if (cgs == null)
            {
                return 0;
            }
            string sqlQueryGradeScale = $@"select top 1 gs.Id, gs.GradeScaleGroupId,gs.MaxNumberInPercent,gs.MinNumberInPercent,gs.GPA from GradeScale gs
                                        where gs.GradeScaleGroupId = {cgs.GradeScaleGroupId}
                                        and gs.MinNumberInPercent <= {percent} 
                                        order by gs.MinNumberInPercent desc";

            var gradeScaleData = SQLHelper.RunSqlQuery(sqlQueryGradeScale);
            VmGradeScale maxPossibleGPAElement = null;


            if (gradeScaleData.Count > 0)
            {

                List<object> st = gradeScaleData[0];

                maxPossibleGPAElement = new VmGradeScale
                {
                    Id = (int)st[0],
                    GradeScaleGroupId = (int)st[1],
                    MaxNumberInPercent = (double)st[2],
                    MinNumberInPercent = (double)st[3],
                    GPA = (double)st[4]
                };

            }
            return maxPossibleGPAElement.GPA;
        }
        private static Gradebook GetGradebook(VmStudent student, VmCourseInstance courseInstance)
        {
            string[] resultGrades;
            CourseGradeResult resultValue = GetCourseGrade(student.StudentId, courseInstance.Id);

            Gradebook moduleGradebook = new Gradebook(true);
            VmGradeWeight gradeWeight = GetGradeWeight(courseInstance.Id);
            if (gradeWeight != null)
            {
                moduleGradebook.Assessment.Weight = gradeWeight.AssessmentWeight;
                moduleGradebook.Quiz.Weight = gradeWeight.ActivityWeight;
                moduleGradebook.Material.Weight = gradeWeight.MaterialWeight;
                moduleGradebook.Discussion.Weight = gradeWeight.DiscussionWeight;
                moduleGradebook.Poll.Weight = gradeWeight.PollWeight;
                moduleGradebook.Midterm.Weight = gradeWeight.MidtermWeight;
                moduleGradebook.Final.Weight = gradeWeight.FinalWeight;

                resultGrades = resultValue.AssessmentGrade.Split(',');
                moduleGradebook.Assessment.Grade = Convert.ToInt32(resultGrades[0]);
                moduleGradebook.Assessment.Occurrence = Convert.ToInt32(resultGrades[1]);
                moduleGradebook.Assessment.Completion = Convert.ToInt32(resultGrades[2]);

                resultGrades = resultValue.ActivityGrade.Split(',');
                moduleGradebook.Quiz.Grade = Convert.ToInt32(resultGrades[0]);
                moduleGradebook.Quiz.Occurrence = Convert.ToInt32(resultGrades[1]);
                moduleGradebook.Quiz.Completion = Convert.ToInt32(resultGrades[2]);

                moduleGradebook.Material.Grade = 0;
                moduleGradebook.Material.Occurrence = 0;
                moduleGradebook.Material.Completion = 0;

                resultGrades = resultValue.DiscussionGrade.Split(',');
                moduleGradebook.Discussion.Grade = Convert.ToInt32(resultGrades[0]);
                moduleGradebook.Discussion.Occurrence = Convert.ToInt32(resultGrades[1]);
                moduleGradebook.Discussion.Completion = Convert.ToInt32(resultGrades[2]);

                resultGrades = resultValue.PollGrade.Split(',');
                moduleGradebook.Poll.Grade = Convert.ToInt32(resultGrades[0]);
                moduleGradebook.Poll.Occurrence = Convert.ToInt32(resultGrades[1]);
                moduleGradebook.Poll.Completion = Convert.ToInt32(resultGrades[2]);

                resultGrades = resultValue.MidtermGrade.Split(',');
                moduleGradebook.Midterm.Grade = Convert.ToInt32(resultGrades[0]);
                moduleGradebook.Midterm.Occurrence = Convert.ToInt32(resultGrades[1]);
                moduleGradebook.Midterm.Completion = Convert.ToInt32(resultGrades[2]);

                resultGrades = resultValue.FinalGrade.Split(',');
                moduleGradebook.Final.Grade = Convert.ToInt32(resultGrades[0]);
                moduleGradebook.Final.Occurrence = Convert.ToInt32(resultGrades[1]);
                moduleGradebook.Final.Completion = Convert.ToInt32(resultGrades[2]);
            }
            return moduleGradebook;
        }
        private static int GetCurrentGrade(VmStudent student, VmCourseInstance courseInstance)
        {
            string[] resultGrades;
            CourseGradeCurrentResult resultValue = GetCourseGradeCurrent(student.StudentId, courseInstance.Id);

            Gradebook courseGradebook = new Gradebook(true);
            VmGradeWeight gradeWeight = GetGradeWeight(courseInstance.Id);
            if (gradeWeight != null)
            {
                courseGradebook.Assessment.Weight = gradeWeight.AssessmentWeight;
                courseGradebook.Quiz.Weight = gradeWeight.ActivityWeight;
                courseGradebook.Material.Weight = gradeWeight.MaterialWeight;
                courseGradebook.Discussion.Weight = gradeWeight.DiscussionWeight;
                courseGradebook.Poll.Weight = gradeWeight.PollWeight;
                courseGradebook.Midterm.Weight = gradeWeight.MidtermWeight;
                courseGradebook.Final.Weight = gradeWeight.FinalWeight;

                resultGrades = resultValue.AssessmentGrade.Split(',');
                courseGradebook.Assessment.Grade = Convert.ToInt32(resultGrades[0]);
                courseGradebook.Assessment.Occurrence = Convert.ToInt32(resultGrades[1]);
                courseGradebook.Assessment.Completion = Convert.ToInt32(resultGrades[2]);

                resultGrades = resultValue.ActivityGrade.Split(',');
                courseGradebook.Quiz.Grade = Convert.ToInt32(resultGrades[0]);
                courseGradebook.Quiz.Occurrence = Convert.ToInt32(resultGrades[1]);
                courseGradebook.Quiz.Completion = Convert.ToInt32(resultGrades[2]);

                courseGradebook.Material.Grade = 0;
                courseGradebook.Material.Occurrence = 0;
                courseGradebook.Material.Completion = 0;

                resultGrades = resultValue.DiscussionGrade.Split(',');
                courseGradebook.Discussion.Grade = Convert.ToInt32(resultGrades[0]);
                courseGradebook.Discussion.Occurrence = Convert.ToInt32(resultGrades[1]);
                courseGradebook.Discussion.Completion = Convert.ToInt32(resultGrades[2]);

                resultGrades = resultValue.PollGrade.Split(',');
                courseGradebook.Poll.Grade = Convert.ToInt32(resultGrades[0]);
                courseGradebook.Poll.Occurrence = Convert.ToInt32(resultGrades[1]);
                courseGradebook.Poll.Completion = Convert.ToInt32(resultGrades[2]);

                resultGrades = resultValue.MidtermGrade.Split(',');
                courseGradebook.Midterm.Grade = Convert.ToInt32(resultGrades[0]);
                courseGradebook.Midterm.Occurrence = Convert.ToInt32(resultGrades[1]);
                courseGradebook.Midterm.Completion = Convert.ToInt32(resultGrades[2]);

                resultGrades = resultValue.FinalGrade.Split(',');
                courseGradebook.Final.Grade = Convert.ToInt32(resultGrades[0]);
                courseGradebook.Final.Occurrence = Convert.ToInt32(resultGrades[1]);
                courseGradebook.Final.Completion = Convert.ToInt32(resultGrades[2]);
            }

            return (int)Math.Round(courseGradebook.CalculateWeightedGrade());
        }
    }
    public class CourseStatisticsStudentInfo
    {
        public string Hash { get; set; }
        public int CourseInstanceId { get; set; }
    }
    public class CourseStatisticsResultInfo
    {
        public string CourseName { get; set; }
        public CourseStatisticsActivityInfo Assessment { get; set; }
        public CourseStatisticsActivityInfo Quiz { get; set; }
        public CourseStatisticsActivityInfo Material { get; set; }
        public CourseStatisticsActivityInfo Midterm { get; set; }
        public CourseStatisticsActivityInfo Final { get; set; }
        public CourseStatisticsActivityInfo Poll { get; set; }
        public CourseStatisticsActivityInfo Discussion { get; set; }
        public CourseStatisticsActivityInfo Total { get; set; }
    }
    public class CourseStatisticsActivityInfo
    {
        public int Weight { get; set; }
        public int Grade { get; set; }
        public int CurrentGrade { get; set; }
        public double WeightedGrade { get; set; }
        public int Completion { get; set; }
        public double GPA { get; set; }
        public double CurrentGPA { get; set; }

    }
    public class CourseGradeResult
    {
        public string ActivityGrade { get; set; }
        public string AssessmentGrade { get; set; }
        public string FinalGrade { get; set; }
        public string MidtermGrade { get; set; }
        public string PollGrade { get; set; }
        public string DiscussionGrade { get; set; }
    }
    public partial class CourseGradeCurrentResult
    {
        public string ActivityGrade { get; set; }
        public string AssessmentGrade { get; set; }
        public string FinalGrade { get; set; }
        public string MidtermGrade { get; set; }
        public string PollGrade { get; set; }
        public string DiscussionGrade { get; set; }
    }
}
