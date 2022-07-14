using LMS.Common.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTModelFunctionsCore.Helper;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HintRatingController : ControllerBase
    {
        [HttpPost]
        public ActionResult<string> Post([FromBody] HintInfo qi)
        {
            string error = "";
            if (qi.Rating < -1 || qi.Rating > 1)
            {
                error = "Error when sending rating";
            }

            VmStudent student = VmModelHelper.GetStudentInfoByHash(qi.StudentId);
            return Ok(error);
        }
    }
    public class HintInfo
    {
        public int QuestionId { get; set; }
        public int Rating { get; set; }
        public string StudentId { get; set; }
        public int HintId { get; set; }
    }
}
