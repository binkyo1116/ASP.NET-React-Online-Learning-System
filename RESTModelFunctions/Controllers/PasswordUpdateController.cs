using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
//using EFModel;
using LMS.Common.ViewModels;
using RESTModelFunctions.Helper;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PasswordUpdateController : ApiController
    {
        public class StudentInfo
        {
            public string NewPassword { get; set; }
            public string CurrentPassword { get; set; }
            public string StudentId { get; set; }
        }

        // POST api/Course
        public IHttpActionResult Post([FromBody] StudentInfo si)
        {
            //MaterialEntities model = new MaterialEntities();

            //Student student = model.Students.Where(stud => stud.Hash == si.StudentId).FirstOrDefault();            
            VmStudent student = VmModelHelper.GetStudentInfoByHash(si.StudentId);

            ResultInfo ri;

            if (student.Password == si.CurrentPassword)
            {
                student.Password = si.NewPassword;
                //model.SaveChanges();

                ri = new ResultInfo()
                {
                    Result = "OK"
                };
            }
            else
            {
                ri = new ResultInfo()
                {
                    Result = ""
                };
            }

            return Ok(ri);

        }


        public class ResultInfo
        {
            public string Result { get; set; }
        }

    }
}

