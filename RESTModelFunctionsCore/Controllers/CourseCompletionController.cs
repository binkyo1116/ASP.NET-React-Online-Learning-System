using LMS.Common.ViewModels;
using LMSLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTModelFunctionsCore.Helper;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseCompletionController : ControllerBase
    {
        [HttpPost]
        public ActionResult<List<CourseCompletionSummary>> Post([FromBody] CourseCompletionStudentInfo si)
        {
            VmStudent student = VmModelHelper.GetStudentInfoByHash(si.Hash);
            List<CourseCompletionSummary> result = GetCourseCompletionSummary(student.Test.Value, student.StudentId, si.CourseInstanceId);

            return Ok(result);
        }
        private List<CourseCompletionSummary> GetCourseCompletionSummary(bool test, int studentId, int courseInstanceId)
        {
            string sqlQueryQuarter = $@"exec CourseCompletionSummary {test} , {studentId} , {courseInstanceId}";

            var courseCompletionSummaryData = SQLHelper.RunSqlQuery(sqlQueryQuarter);
            List<CourseCompletionSummary> courseCompletionSummaryList = new List<CourseCompletionSummary>();

            foreach (var item in courseCompletionSummaryData)
            {
                CourseCompletionSummary courseCompletionSummary = null;

                List<object> st = item;

                courseCompletionSummary = new CourseCompletionSummary
                {
                    CourseObjectiveId = st[0] == null ? 0 : (int)st[0],
                    CourseObjective = st[1]?.ToString(),
                    ModuleObjectiveId = string.IsNullOrEmpty(st[2].ToString()) ? 0 : (int)st[2],
                    ModuleObjective = string.IsNullOrEmpty(st[3].ToString()) ? "" : st[2].ToString(),
                    ActivityType = string.IsNullOrEmpty(st[4].ToString()) ? "" : st[4].ToString(),
                    ActivityId = string.IsNullOrEmpty(st[5].ToString()) ? 0 : (int)st[5],
                    Title = string.IsNullOrEmpty(st[6].ToString()) ? "" : st[6].ToString(),
                    ItemsCompleted = st[7] == null ? 0 : (int)st[7],
                    Items = st[8] == null ? 0 : (int)st[8],
                    Completed = st[9] == null ? 0 : (int)st[9]
                };

                courseCompletionSummaryList.Add(courseCompletionSummary);
            }
            return courseCompletionSummaryList;
        }
    }

    public class CourseCompletionStudentInfo
    {
        public string Hash { get; set; }
        public int CourseInstanceId { get; set; }
    }
    public class CourseCompletionSummary
    {
        public Nullable<int> CourseObjectiveId { get; set; }
        public string CourseObjective { get; set; }
        public Nullable<int> ModuleObjectiveId { get; set; }
        public string ModuleObjective { get; set; }
        public string ActivityType { get; set; }
        public Nullable<int> ActivityId { get; set; }
        public string Title { get; set; }
        public Nullable<int> ItemsCompleted { get; set; }
        public Nullable<int> Items { get; set; }
        public Nullable<int> Completed { get; set; }
    }
}
