using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
//using EFModel;
using LMSLibrary;
using LMS.Common.ViewModels;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SignUpController : ApiController
    {
        public class StudentInfo
        {
            public string FullName { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }

        // POST api/Course
        public IHttpActionResult Post([FromBody] StudentInfo si)
        {
            //using (MaterialEntities model = new MaterialEntities())
            //{
            string errorMessage;
            //VmStudent studentUserNameInfo = new VmStudent();
            VmStudent studentUserNameInfo = GetStudentInfoByUserName(si.Username);

            //if (model.Students.Where(x => x.UserName == si.Username).Any())
            if (studentUserNameInfo != null)
            {
                errorMessage = "Sorry! User Name already exist. Please select another one.";
                return Ok(new ResultInfo { Success = false, Message = errorMessage });
            }

            VmStudent studentEmailInfo = new VmStudent();
            studentEmailInfo = GetStudentInfoByEmail(si.Email);

            //else if (model.Students.Where(x => x.Email == si.Email).Any())
            if (studentEmailInfo != null)
            {
                errorMessage = "Sorry! E-mail already exist. Please select another one.";
                return Ok(new ResultInfo { Success = false, Message = errorMessage });
            }

            try
            {
                string sqlQueryStudent = $@"INSERT INTO Student (Name, Email, UserName, Password, CanvasId, Mark, Hash)
                                                VALUES ('{si.FullName.Trim()}','{si.Email.Trim()}','{si.Username.Trim()}','{si.Password.Trim()}',{0},{0},'{Guid.NewGuid().ToString()}');";
                bool isSucess = SQLHelper.RunSqlUpdate(sqlQueryStudent);


                //Student student = new Student()
                //{
                //    StudentId = GenarateId(),
                //    Name = si.FullName.Trim(),
                //    Email = si.Email.Trim(),
                //    UserName = si.Username.Trim(),
                //    Password = si.Password.Trim(),
                //    CanvasId = 0,
                //    Mark = 0,
                //    Hash = Guid.NewGuid().ToString()
                //};
                //model.Students.Add(student);
                //model.SaveChanges();
            }
            catch (Exception ex)
            {
                return Ok(new ResultInfo { Success = false, Message = ex.Message });
            }
            return Ok(new ResultInfo { Success = true });

            //}
        }
        public class ResultInfo
        {
            public bool Success { get; set; }
            public string Message { get; set; }
        }
        private int GenarateId()
        {
            int id = 0;
            //using (MaterialEntities data = new MaterialEntities())
            //{
            //Student lastEntry = data.Students.OrderByDescending(x => x.StudentId).FirstOrDefault();
            VmStudent lastEntry = GetLastStudentInfo();
            if (lastEntry != null)
            {
                id = ++lastEntry.StudentId;
            }
            else
            {
                id = 1;
            }
            //}
            return id;
        }

        private VmStudent GetLastStudentInfo()
        {
            string sqlQueryStudent = $@"select s.StudentId, s.Name from Student s order by s.StudentId desc";

            var studentData = SQLHelper.RunSqlQuery(sqlQueryStudent);
            VmStudent studentinfo = null;

            if (studentData.Count > 0)
            {
                List<object> st = studentData[0];

                studentinfo = new VmStudent
                {
                    StudentId = (int)st[0],
                    Name = st[1].ToString()
                };
            }

            return studentinfo;
        }
        private VmStudent GetStudentInfoByUserName(string userName)
        {
            string sqlQueryStudent = $@"select s.StudentId, s.UserName, s.Email from Student s where s.UserName = '{userName}'";

            var studentData = SQLHelper.RunSqlQuery(sqlQueryStudent);
            VmStudent studentinfo = null;

            if (studentData.Count > 0)
            {
                List<object> st = studentData[0];

                studentinfo = new VmStudent
                {
                    StudentId = (int)st[0],
                    UserName = st[1].ToString(),
                    Email = st[2].ToString()
                };
            }

            return studentinfo;
        }
        private VmStudent GetStudentInfoByEmail(string email)
        {
            string sqlQueryStudent = $@"select s.StudentId, s.UserName, s.Email from Student s where s.Email = '{email}'";

            var studentData = SQLHelper.RunSqlQuery(sqlQueryStudent);
            VmStudent studentinfo = null;

            if (studentData.Count > 0)
            {
                List<object> st = studentData[0];

                studentinfo = new VmStudent
                {
                    StudentId = (int)st[0],
                    UserName = st[1].ToString(),
                    Email = st[2].ToString()
                };
            }

            return studentinfo;
        }
    }
}

