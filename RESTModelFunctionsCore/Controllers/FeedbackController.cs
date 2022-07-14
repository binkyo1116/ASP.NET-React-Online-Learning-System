using LMS.Common.ViewModels;
using LMSLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTModelFunctionsCore.Helper;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        [HttpPost]
        public ActionResult<FeedbackResultInfo> Post([FromBody] FeedbackIncomingInfo ici)
        {
            VmStudent student = VmModelHelper.GetStudentInfoByHash(ici.StudentHash); ;

            if (ici.Method == "Save")
            {

                string sqlQueryStudent = $@"INSERT INTO Feedback (StudentId, TimeStamp, Text, CourseInstanceId)
                                            VALUES ({student.StudentId}, '{DateTime.Now}', '{ici.Feedback}', {ici.CourseInstanceId})";
                bool isSucess = SQLHelper.RunSqlUpdate(sqlQueryStudent);
                int result = isSucess ? 1 : 0;
                return Ok(result);
            }
            else if (ici.Method == "GetList")
            {
                List<VmFeedback> courseFeedbacks = GetAnnouncement(ici.CourseInstanceId, student.StudentId, ici.OnlyMine);
                VmCourseInstance courseInstance = VmModelHelper.GetCourseInstanceById(ici.CourseInstanceId);
                VmCourse course = VmModelHelper.GetCourseById(courseInstance.CourseId);

                FeedbackResultInfo ri = new FeedbackResultInfo
                {
                    CourseName = course.Name
                };

                IList<FeedbackInfo> feedbackList = new List<FeedbackInfo>();
                foreach (VmFeedback f in courseFeedbacks)
                {
                    FeedbackInfo fbi = new FeedbackInfo
                    {
                        FeedbackId = f.Id,
                        Description = f.Text,
                        Status = f.Status,
                        Comment = f.Comment,
                        IsMine = (f.StudentId == student.StudentId)
                    };

                    feedbackList.Add(fbi);
                }

                ri.FeedbackList = (List<FeedbackInfo>)feedbackList.OrderByDescending(x => x.FeedbackId).ToList();

                return Ok(ri);
            }
            else if (ici.Method == "GetCourses")
            {
                var courseList = GetStudentCourse(student.StudentId);
                return Ok(courseList);
            }
            else
            {
                return Ok("");
            }
        }
        private List<StudentCourse> GetStudentCourse(int studentId)
        {
            string sqlStudentCourse = $@"select ci.Id,c.Name from Course c
                                    inner join CourseInstance ci on ci.CourseId = c.Id
                                    inner join CourseInstanceStudent cis on cis.CourseInstanceId = ci.Id
                                    where ci.Active = 1 and cis.StudentId = {studentId}";

            var studentCourseData = SQLHelper.RunSqlQuery(sqlStudentCourse);
            List<StudentCourse> studentCourses = new List<StudentCourse>();

            if (studentCourseData.Count > 0)
            {
                foreach (var item in studentCourseData)
                {
                    StudentCourse studentCourseInfo = new StudentCourse
                    {
                        Id = (int)item[0],
                        Name = (string)item[1]
                    };
                    studentCourses.Add(studentCourseInfo);
                }
            }
            return studentCourses;
        }
        private List<VmFeedback> GetAnnouncement(int courseInstanceId, int studentId, bool onlyMe)
        {
            string sqlFeedback = string.Empty;
            if (onlyMe)
            {
                sqlFeedback = $@"select Id, StudentId, Text, Status,Comment,CourseInstanceId  from Feedback
                                                where CourseInstanceId = {courseInstanceId} and StudentId = {studentId}";
            }
            else
            {
                sqlFeedback = $@"select Id, StudentId, Text, Status,Comment,CourseInstanceId  from Feedback
                                                where CourseInstanceId = {courseInstanceId} ";
            }

            var feedbackInfoData = SQLHelper.RunSqlQuery(sqlFeedback);
            List<VmFeedback> feedbackInfos = new List<VmFeedback>();

            if (feedbackInfoData.Count > 0)
            {
                foreach (var item in feedbackInfoData)
                {
                    VmFeedback feedbackInfo = new VmFeedback
                    {
                        Id = (int)item[0],
                        StudentId = (int)item[1],
                        Text = item[2] == DBNull.Value?string.Empty: item[2].ToString(),
                        Status = item[3] == DBNull.Value ? string.Empty : item[3].ToString(),
                        Comment = item[4] == DBNull.Value ? string.Empty : item[4].ToString(),
                        CourseInstanceId = (int)item[5]
                    };
                    feedbackInfos.Add(feedbackInfo);
                }
            }
            return feedbackInfos;
        }
    }
    public class FeedbackIncomingInfo
    {
        public string StudentHash { get; set; }
        public int CourseInstanceId { get; set; }
        public string Feedback { get; set; }
        public string Method { get; set; }
        public bool OnlyMine { get; set; }
    }
    public class StudentCourse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class FeedbackResultInfo
    {
        public string CourseName { get; set; }
        public List<FeedbackInfo> FeedbackList { get; set; }
    }
    public class FeedbackInfo
    {
        public int FeedbackId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public bool IsMine { get; set; }
    }
}
