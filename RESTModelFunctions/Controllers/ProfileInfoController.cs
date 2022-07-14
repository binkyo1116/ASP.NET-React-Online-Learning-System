using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
//using EFModel;
using LMS.Common.ViewModels;
using RESTModelFunctions.Helper;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProfileInfoController : ApiController
    {
        public class StudentInfo
        {
            public string Hash { get; set; }
            public bool StudentId { get; set; }
        }

        // POST api/Course
        public IHttpActionResult Post([FromBody] StudentInfo si)
        {
            //MaterialEntities model = new MaterialEntities();

            //Student student = model.Students.Where(stud => stud.Hash == si.Hash).FirstOrDefault();
            VmStudent student = VmModelHelper.GetStudentInfoByHash(si.Hash);

            ResultInfo ri = new ResultInfo();
            if (si.StudentId)
            {
                ri.StudentId = student.StudentId;
            }
            else
            {
                byte[] photo = student.Photo;
                string imgURL = "";

                if (photo != null)
                {
                    byte[] img = photo.ToArray();
                    imgURL = "data:image;base64," + Convert.ToBase64String(img);
                }

                ri.UserName = student.UserName;
                ri.Password = student.Password;
                ri.FullName = student.Name;
                ri.Email = student.Email;
                ri.Photo = imgURL;
            }
            return Ok(ri);
        }


        public class ResultInfo
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Photo { get; set; }
            public int StudentId { get; set; }

        }

    }
}

