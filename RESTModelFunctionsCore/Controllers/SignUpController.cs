using LMS.Common.ViewModels;
using LMSLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        [HttpPost]
        public ActionResult<SignUpResultInfo> Post([FromBody] SignUpStudentInfo si)
        {
            string errorMessage;
            VmStudent studentUserNameInfo = GetStudentInfoByUserName(si.Username);
            if (studentUserNameInfo != null)
            {
                errorMessage = "Sorry! User Name already exist. Please select another one.";
                return Ok(new SignUpResultInfo { Success = false, Message = errorMessage });
            }

            VmStudent studentEmailInfo = new VmStudent();
            studentEmailInfo = GetStudentInfoByEmail(si.Email);

            if (studentEmailInfo != null)
            {
                errorMessage = "Sorry! E-mail already exist. Please select another one.";
                return Ok(new SignUpResultInfo { Success = false, Message = errorMessage });
            }

            try
            {
                string sqlQueryStudent = $@"INSERT INTO Student (Name, Email, UserName, Password, CanvasId, Mark, Hash)
                                                VALUES ('{si.FullName.Trim()}','{si.Email.Trim()}','{si.Username.Trim()}','{si.Password.Trim()}',{0},{0},'{Guid.NewGuid().ToString()}');";
                bool isSucess = SQLHelper.RunSqlUpdate(sqlQueryStudent);
            }
            catch (Exception ex)
            {
                return Ok(new SignUpResultInfo { Success = false, Message = ex.Message });
            }
            return Ok(new SignUpResultInfo { Success = true });
        }
        private int GenarateId()
        {
            int id = 0;
            VmStudent lastEntry = GetLastStudentInfo();
            if (lastEntry != null)
            {
                id = ++lastEntry.StudentId;
            }
            else
            {
                id = 1;
            }
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
    public class SignUpStudentInfo
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
    public class SignUpResultInfo
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
