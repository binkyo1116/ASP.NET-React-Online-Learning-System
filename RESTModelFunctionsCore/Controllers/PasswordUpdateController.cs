using LMS.Common.ViewModels;
using LMSLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTModelFunctionsCore.Helper;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordUpdateController : ControllerBase
    {
        [HttpPost]
        public ActionResult<PasswordUpdateResultInfo> Post([FromBody] PasswordUpdateStudentInfo si)
        {
            VmStudent student = VmModelHelper.GetStudentInfoById(si.StudentId);
            PasswordUpdateResultInfo ri;

            if (student.Password == si.CurrentPassword)
            {
                student.Password = si.NewPassword;
                string sqlQuery = $@"update Student
                                       set Password = '{si.NewPassword}' where StudentId = {si.StudentId}";

                SQLHelper.RunSqlUpdate(sqlQuery);
                ri = new PasswordUpdateResultInfo()
                {
                    Result = "OK"
                };
            }
            else
            {
                ri = new PasswordUpdateResultInfo()
                {
                    Result = ""
                };
            }

            return Ok(ri);
        }
    }

    public class PasswordUpdateStudentInfo
    {
        public string NewPassword { get; set; }
        public string CurrentPassword { get; set; }
        public string StudentId { get; set; }
    }
    public class PasswordUpdateResultInfo
    {
        public string Result { get; set; }
    }
}
