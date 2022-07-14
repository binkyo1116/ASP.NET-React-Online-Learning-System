using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
//using EFModel;
using LMS.Common.ViewModels;
using RESTModelFunctions.Helper;

namespace RESTModelFunctions.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HintRatingController : ApiController
    {
        public class HintInfo
        {
            public int QuestionId { get; set; }

            public int Rating { get; set; }

            public string StudentId { get; set; }
            public int HintId { get; set; }
        }

        // POST api/Course
        public IHttpActionResult Post([FromBody] HintInfo qi)
        {
            string error = "";
            if (qi.Rating < -1 || qi.Rating > 1)
            {
                error = "Error when sending rating";
            }

            //MaterialEntities model = new MaterialEntities();
            //Student student = model.Students.Where(s => s.Hash == qi.StudentId).FirstOrDefault();
            
            VmStudent student = VmModelHelper.GetStudentInfoByHash(qi.StudentId);

            //model.SaveChanges();

            return Ok(error);
        }
    }
}
