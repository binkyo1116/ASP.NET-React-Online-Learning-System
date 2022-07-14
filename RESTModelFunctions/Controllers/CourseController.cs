using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using LMSLibrary;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CourseController : ApiController
    {
        public class StudentInfo
        {
            public string Hash { get; set; }
            public bool IsAdmin { get; set; }
            public string AdminHash { get; set; }
            public string SelectedStudentId { get; set; }
            public string Method { get; set; }
        }

        // POST api/Course
        public IHttpActionResult Post([FromBody] StudentInfo si)
        {
            //MaterialEntities model = new MaterialEntities();
            //====================================Get Course Cards===============================
            if (si.Method == "Get")
            {
                String sqlQuery = $@"SELECT CI.Id, C.Name, Q.Name
                                        FROM CourseInstanceStudent CIS 
                                        INNER JOIN CourseInstance CI
                                        ON CIS.CourseInstanceId = CI.Id
                                        INNER JOIN Course C
                                        ON CI.CourseId = C.Id
                                        INNER JOIN Student S
                                        ON S.StudentId = CIS.StudentId
                                        INNER JOIN Quarter Q
                                        ON CI.QuarterId = Q.QuarterId
                                        WHERE Hash = '{si.Hash}'";

                if (si.IsAdmin)
                {
                    sqlQuery += " AND (CI.Testing = 1 OR CI.Active = 1)";
                } else {
                    sqlQuery += " AND CI.Active = 1";
                }

                var sqlResult = SQLHelper.RunSqlQuery(sqlQuery);

                IList<ResultInfo> courses = new List<ResultInfo>();

                foreach (List<object> result in sqlResult)
                {
                    ResultInfo rinfo = new ResultInfo { 
                        CourseInstanceId = (int)result[0], 
                        Name = (string)result[1], 
                        Picture = "", 
                        TotalGrade = 0, 
                        TotalCompletion = 0,
                        Quarter = (string)result[2]
                    };
                    courses.Add(rinfo);
                }
                //--------------------------Check Admin---------------------------------------
                IList<StudentList> studentList = new List<StudentList>();
                if (si.IsAdmin && si.AdminHash != "")
                {
                    //var admin = model.Students.Where(stud => stud.Hash == si.AdminHash).FirstOrDefault();
                    string sqlQueryAdmin = $@"select UserName, Password
                                               from Student
                                               where Hash = '{si.AdminHash}'";

                    var admin = SQLHelper.RunSqlQuery(sqlQueryAdmin).First();
                    if (admin != null)
                    {
                        if (CheckAdminUser((string)admin[0], (string)admin[1]))
                        {
                            string sqlQueryStudent = $@"select StudentId, Name 
                                                        from Student
                                                        where Test is null or Test = 0";

                            var studenResult = SQLHelper.RunSqlQuery(sqlQueryStudent);
                            foreach (List<object> result in studenResult)
                            {
                                StudentList student = new StudentList { Id = (int)result[0], Name = (string)result[1] };
                                studentList.Add(student);
                            }
                            studentList.OrderBy(s => s.Name);

                            //studentList = model.Students.Where(s => !s.Test.HasValue || s.Test == false).Select(x => new StudentList
                            //{
                            //    Id = x.StudentId,
                            //    Name = x.Name
                            //}).OrderBy(s => s.Name).ToList();
                        }
                    }
                }
                //------------------------------------------------------------------
                CourseResult courseResult = new CourseResult
                {
                    CourseList = courses,
                    StudentList = studentList
                };
                //System.Web.Http.Results.OkNegotiatedContentResult<IList<ResultInfo>> serializedResult = Ok(courses);
                System.Web.Http.Results.OkNegotiatedContentResult<CourseResult> serializedResult = Ok(courseResult);
                return serializedResult;
            }

            //====================================Navigate Selected Student==========================================
            else if (si.Method == "NavigateStudent")
            {
                StudentResultInfo StudentResult = new StudentResultInfo();
                if (si.IsAdmin && si.AdminHash != "" && si.SelectedStudentId != "")
                {
                    //---------------------------------Get Admin UserName and password-----------------------------------
                    //var admin = model.Students.Where(stud => stud.Hash == si.AdminHash).FirstOrDefault();
                    string sqlQueryAdmin = $@"select UserName, Password
                                               from Student
                                               where Hash = '{si.AdminHash}'";

                    var admin = SQLHelper.RunSqlQuery(sqlQueryAdmin).First();
                    //------------------------------------------------------------------------------------------------------
                    if (admin != null)
                    {
                        if (CheckAdminUser((string)admin[0], (string)admin[1]))
                        {
                            //--------------------------------Get Student Info--------------------------------
                            //var studentModel = model.Students.Find(studentId);
                            var studentId = Convert.ToInt32(si.SelectedStudentId);
                            string sqlQueryStudent = $@"select Hash, Name, Photo, (select Count(*) 
                                                        from CourseInstanceStudent
                                                        where StudentId = s.StudentId ) as CourseInstance from Student s
                                                        where s.StudentId = {studentId}";

                            var studentModel = SQLHelper.RunSqlQuery(sqlQueryStudent).First();
                            //------------------------------------------------------------------------------------
                            string error = "";
                            string studentIdHash = "-1";
                            string studentName = "";
                            string studentPicture = "";

                            if (studentModel == null)
                            {
                                error = "Student not found";
                            }
                            else
                            {
                                if ((int)studentModel[3] == 0)
                                {
                                    error = "Student not registered in a course";
                                }
                                else
                                {
                                    studentIdHash = (string)studentModel[0];
                                    studentName = (string)studentModel[1];
                                    if (studentIdHash.Length != 36)
                                    {
                                        try
                                        {
                                            studentIdHash = Guid.NewGuid().ToString();
                                            string sqlQueryUPdateHashValue = $@"update Student set Hash = '{studentIdHash}'
                                                                            where StudentId ={studentId}";
                                            var UpdateHashValue = SQLHelper.RunSqlUpdate(sqlQueryUPdateHashValue);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    //--------------------------Image--------------------------------
                                    if (studentModel[2].ToString() != "")
                                    {
                                        var picturebyte = (Byte[])studentModel[2];
                                        byte[] picture = picturebyte;

                                        if (picture != null)
                                        {
                                            byte[] img = picture.ToArray();
                                            studentPicture = "data:image;base64," + Convert.ToBase64String(img);
                                        }
                                    }
                                }
                            }
                            StudentResult = new StudentResultInfo
                            {
                                studentIdHash = studentIdHash,
                                StudentName = studentName,
                                Picture = studentPicture,
                                error = error
                            };
                        }
                    }
                }
                System.Web.Http.Results.OkNegotiatedContentResult<StudentResultInfo> serializedResult = Ok(StudentResult);
                return serializedResult;
            }
            else if (si.Method == "Grades")
            {
                //------------------------------Course Grade---------------------------------------------
                //Student student = model.Students.Where(stud => stud.Hash == si.Hash).FirstOrDefault();
                string sqlQueryGrade = $@"declare @studernId int =  (select StudentId
                                            from Student where Hash ='{si.Hash}') 
                                            exec CourseGradeForAllCourseInstance @studernId";

                var CourseGrade = SQLHelper.RunSqlQuery(sqlQueryGrade);
                IList<ResultInfo> courses = new List<ResultInfo>();

                //List<CourseGradeForAllCourseInstance_Result> resultValue = model.CourseGradeForAllCourseInstance(int.Parse(student.First()[0])).ToList();
                List<CourseGradeForAllCourseInstance> resultValue = new List<CourseGradeForAllCourseInstance>();
                foreach (List<object> result in CourseGrade)
                {
                    CourseGradeForAllCourseInstance courseGradResult = new CourseGradeForAllCourseInstance { CourseInstanceId = (int)result[0], ActivityGrade = (string)result[1], AssessmentGrade = (string)result[2], FinalGrade = (string)result[3], MidtermGrade = (string)result[4], PollGrade = (string)result[5], DiscussionGrade = (string)result[6] };
                    resultValue.Add(courseGradResult);
                }

                //---------------------------------Get Course Instance with GradeWeight----------------------------------------
                string sqlQueryCourseInstance = $@"	select ci.Id CourseInstanceId, c.Name, ActivityWeight, AssessmentWeight, MaterialWeight, DiscussionWeight, PollWeight, MidtermWeight, FinalWeight 
	                                                    from CourseInstance ci
	                                                    join Course c on ci.CourseId = c.Id
	                                                    join CourseInstanceStudent cis on ci.Id = cis.CourseInstanceId
	                                                    join Student s on cis.StudentId = s.StudentId
	                                                    left join GradeWeight g on ci.Id = g.CourseInstanceId
	                                                    where s.Hash = '{si.Hash}'";
                if (si.IsAdmin)
                {
                    sqlQueryCourseInstance += " AND (CI.Testing = 1 OR CI.Active = 1)";
                }
                else
                {
                    sqlQueryCourseInstance += " AND CI.Active = 1";
                }

                var allCourseInstance = SQLHelper.RunSqlQuery(sqlQueryCourseInstance);
                foreach (List<object> courseInstance in allCourseInstance)
                {
                    /* 
                    [Marcelo: TODO]
                    if (!courseInstance.Testing && !courseInstance.Active) continue;
                    if (!courseInstance.Active && student.Name != "s1" && student.Name != "s2" && student.Name != "s3"
                            && student.Name != "s4")
                        break;
                    */
                    string imgURL = "";
                    int totalGrade = 0, totalCompletion = 0;
                    CourseGrade result = new CourseGrade();
                    if (courseInstance != null)
                    {
                        result = (from b in resultValue
                                  where b.CourseInstanceId == (int)courseInstance[0]
                                  select new CourseGrade
                                  {
                                      ActivityGrade = b.ActivityGrade,
                                      AssessmentGrade = b.AssessmentGrade,
                                      DiscussionGrade = b.DiscussionGrade,
                                      FinalGrade = b.FinalGrade,
                                      MidtermGrade = b.MidtermGrade,
                                      PollGrade = b.PollGrade
                                  }).FirstOrDefault();
                        if (result != null)
                        {
                            Gradebook courseGradebook = GetGradebook(courseInstance, result);
                            totalGrade = (int)Math.Round(courseGradebook.CalculateWeightedGrade());
                            totalCompletion = (int)Math.Round(courseGradebook.CalculateTotalCompletion());
                        }
                    }
                    ResultInfo rinfo = new ResultInfo 
                    { 
                        CourseInstanceId = (int)courseInstance[0], 
                        Name = (string)courseInstance[1], 
                        Picture = imgURL, 
                        TotalGrade = totalGrade, // TODO
                        TotalCompletion = totalGrade
                    };
                    courses.Add(rinfo);
                }
                //--------------------------Check Admin---------------------------------------

                //------------------------------------------------------------------
                CourseResult courseResult = new CourseResult
                {
                    CourseList = courses
                };
                //System.Web.Http.Results.OkNegotiatedContentResult<IList<ResultInfo>> serializedResult = Ok(courses);
                System.Web.Http.Results.OkNegotiatedContentResult<CourseResult> serializedResult = Ok(courseResult);
                return serializedResult;
            }
            else
            {
                return Ok();
            }
        }
        private bool CheckAdminUser(string userName, string password)
        {
            var result = false;
            var userIpAddress = HttpContext.Current.Request.UserHostAddress;
            var authenticIp = Admin.IP.Where(x => x == userIpAddress).FirstOrDefault();
            if (Admin.UserName == userName && Admin.Password == password && !string.IsNullOrEmpty(authenticIp))
            {
                result = true;
            }
            return result;
        }
        private static Gradebook GetGradebook(List<object> courseInstance, CourseGrade result)
        {
            //return new Gradebook(true);
            // TODO: Fix this
            string[] resultGrades;
            CourseGrade resultValue = result;
            Gradebook moduleGradebook = new Gradebook(true);
            //GradeWeight gradeWeight = courseInstance.GradeWeights.FirstOrDefault();
            if (courseInstance[2].ToString() != "" || courseInstance[3].ToString() != "" || courseInstance[4].ToString() != "" || courseInstance[5].ToString() != "" || courseInstance[6].ToString() != "" || courseInstance[7].ToString() != "" || courseInstance[8].ToString() != "")
            {
                moduleGradebook.Assessment.Weight = (int)courseInstance[3];
                moduleGradebook.Quiz.Weight = (int)courseInstance[2];
                moduleGradebook.Material.Weight = (int)courseInstance[4];
                moduleGradebook.Discussion.Weight = (int)courseInstance[5];
                moduleGradebook.Poll.Weight = (int)courseInstance[6];
                moduleGradebook.Midterm.Weight = (int)courseInstance[7];
                moduleGradebook.Final.Weight = (int)courseInstance[8];

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
        public class CourseResult
        {
            public IList<ResultInfo> CourseList { get; set; }
            public IList<StudentList> StudentList { get; set; }
        }
        public class ResultInfo
        {
            public string Name { get; set; }
            public int CourseInstanceId { get; set; }
            public string Picture { get; set; }
            public int TotalGrade { get; set; }
            public int TotalCompletion { get; set; }
            public string Quarter { get; set; }
        }
        public class StudentList
        {
            public string Name { get; set; }
            public int Id { get; set; }
        }
        public class StudentResultInfo
        {
            public string studentIdHash { get; set; }
            public string error { get; set; }
            public string StudentName { get; set; }
            public string Picture { get; set; }
        }
        private static class Admin
        {
            private static string userName = "s1";
            private static string password = "p1";
            private static List<string> ip = new List<string> {
                "::1",
                "73.181.250.230"
            };

            public static string UserName { get => userName; }
            public static string Password { get => password; }
            public static List<string> IP { get => ip; }
        }
        private class CourseGradeForAllCourseInstance
        {
            public int CourseInstanceId { get; set; }
            public string ActivityGrade { get; set; }
            public string AssessmentGrade { get; set; }
            public string FinalGrade { get; set; }
            public string MidtermGrade { get; set; }
            public string PollGrade { get; set; }
            public string DiscussionGrade { get; set; }
        }
        private class CourseGrade
        {
            public string ActivityGrade { get; set; }
            public string AssessmentGrade { get; set; }
            public string FinalGrade { get; set; }
            public string MidtermGrade { get; set; }
            public string PollGrade { get; set; }
            public string DiscussionGrade { get; set; }
        }
    }
}
