using System.Text;
using LMS.Common.ViewModels;
using LMSLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentRequestLoginController : ControllerBase
    {
        [HttpPost]
        public ActionResult<StudentRequestLoginResultInfo> Post([FromBody] StudentRequestLoginStudentInfo si)
        {
            try
            {
                string sqlQueryRequestLogin = $@"exec InsertRequestLogin '{si.Name.Trim()}' , '{si.Email.Trim()}' , '{si.SchoolName.Trim()}' , '{si.CourseName.Trim()}'";
                bool isSucess = SQLHelper.RunSqlUpdate(sqlQueryRequestLogin);

                EmailHelper emailHelper = new EmailHelper();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Student Name: " + si.Name.Trim());
                sb.AppendLine("Student Email: " + si.Email.Trim());
                sb.AppendLine("School Name: " + si.SchoolName.Trim());
                sb.AppendLine("Course Name: " + si.CourseName.Trim());
                sb.AppendLine("Timestamp: " + DateTime.Now);

                EmailHelper.SendEmail(
                    new EmailHelper.Message
                    {
                        Subject = "Student Requesting Login",
                        Recipient = "marcelo@letsusedata.com",
                        Body = sb.ToString()
                    }
                 );
            }
            catch (Exception ex)
            {
                return Ok(new StudentRequestLoginResultInfo { Success = false, Message = ex.Message });
            }

            return Ok(new StudentRequestLoginResultInfo { Success = true });
        }
        private int GenarateId()
        {
            int id = 0;
            VmRequestLogin lastEntry = GetLastRequestLoginInfo();
            if (lastEntry != null)
            {
                id = ++lastEntry.RequestLoginId;
            }
            else
            {
                id = 1;
            }
            return id;
        }
        private VmRequestLogin GetLastRequestLoginInfo()
        {
            string sqlQueryVmRequestLogin = $@"select r.RequestLoginId, r.Name from RequestLogin r order by r.RequestLoginId desc";

            var VmRequestLoginData = SQLHelper.RunSqlQuery(sqlQueryVmRequestLogin);
            VmRequestLogin requestLogInfo = null;

            if (VmRequestLoginData.Count > 0)
            {
                List<object> st = VmRequestLoginData[0];
                requestLogInfo = new VmRequestLogin
                {
                    RequestLoginId = (int)st[0],
                    Name = st[1].ToString()
                };
            }
            return requestLogInfo;
        }
    }
    public class StudentRequestLoginStudentInfo
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string SchoolName { get; set; }
        public string CourseName { get; set; }
    }
    public class StudentRequestLoginResultInfo
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
