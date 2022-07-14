using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
//using EFModel;
using LMSLibrary;
using LMS.Common.ViewModels;
using RESTModelFunctions.Helper;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FeedbackController : ApiController
    {
        public class IncomingInfo
        {
            public string StudentHash { get; set; }
            public int CourseInstanceId { get; set; }
            public string Feedback { get; set; }
            public string Method { get; set; }
            public bool OnlyMine { get; set; }
        }

        // POST api/Course
        public IHttpActionResult Post([FromBody] IncomingInfo ici)
        {
            //MaterialEntities model = new MaterialEntities();

            //Student student = model.Students.Where(s => s.Hash == ici.StudentHash).FirstOrDefault();
            VmStudent student = VmModelHelper.GetStudentInfoByHash(ici.StudentHash); ;

            if (ici.Method == "Save")
            {

                string sqlQueryStudent = $@"INSERT INTO Feedback (StudentId, TimeStamp, Text, CourseInstanceId)
                                            VALUES ({student.StudentId}, '{DateTime.Now}', '{ici.Feedback}', {ici.CourseInstanceId})";
                bool isSucess = SQLHelper.RunSqlUpdate(sqlQueryStudent);
                //Feedback fb = new Feedback
                //{
                //    StudentId = student.StudentId,
                //    TimeStamp = DateTime.Now,
                //    Text = ici.Feedback,
                //    CourseInstanceId = ici.CourseInstanceId
                //};

                //model.Feedbacks.Add(fb);
                //var result = model.SaveChanges();
                int result = isSucess ? 1 : 0;

                return Ok(result);
            }
            else if (ici.Method == "GetList")
            {
                List<VmFeedback> courseFeedbacks = GetAnnouncement(ici.CourseInstanceId, student.StudentId, ici.OnlyMine);
                //if (ici.OnlyMine)
                //{
                //    courseFeedbacks = model.Feedbacks.Where(f => f.CourseInstanceId == ici.CourseInstanceId && f.StudentId == student.StudentId).ToList();
                //}
                //else
                //{
                //    courseFeedbacks = model.Feedbacks.Where(f => f.CourseInstanceId == ici.CourseInstanceId).ToList();
                //}

                VmCourseInstance courseInstance = VmModelHelper.GetCourseInstanceById(ici.CourseInstanceId);
                VmCourse course = VmModelHelper.GetCourseById(courseInstance.CourseId);

                ResultInfo ri = new ResultInfo
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

                ri.FeedbackList = (List<FeedbackInfo>)feedbackList.OrderByDescending(x=> x.FeedbackId).ToList();

                return Ok(ri);
            }
            else if (ici.Method == "GetCourses")
            {
                var courseList = GetStudentCourse(student.StudentId);
                    //model.Students.Where(s => s.Hash == ici.StudentHash).FirstOrDefault().CourseInstances.Where(c => c.Active).Select(c => new { c.Id, c.Course.Name }).ToList();

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
                        Id = (item[0] != DBNull.Value)? (int)item[0] : 0, //(int)item[0],
                        StudentId = (item[1] != DBNull.Value) ? (int)item[1] : 0, //(int)item[1],
                        Text = (item[2] != DBNull.Value) ? item[2].ToString() : string.Empty, //item[2].ToString(),
                        Status = (item[3] != DBNull.Value) ? item[3].ToString() : string.Empty,//item[3].ToString(),
                        Comment = (item[4] != DBNull.Value) ? item[4].ToString() : string.Empty, //item[4].ToString(),
                        CourseInstanceId = (item[5] != DBNull.Value) ? (int)item[5] : 0
                    };
                    feedbackInfos.Add(feedbackInfo);
                }
            }
            return feedbackInfos;
        }

        public class StudentCourse
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class ResultInfo
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
}
