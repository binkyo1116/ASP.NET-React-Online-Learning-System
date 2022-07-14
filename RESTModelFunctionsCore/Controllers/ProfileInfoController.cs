using LMS.Common.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTModelFunctionsCore.Helper;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileInfoController : ControllerBase
    {
        [HttpPost]
        public ActionResult<ProfileResultInfo> Post([FromBody] ProfileStudentInfo si)
        {
            VmStudent student = VmModelHelper.GetStudentInfoByHash(si.Hash);
            ProfileResultInfo ri = new ProfileResultInfo();
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
    }
    public class ProfileStudentInfo
    {
        public string Hash { get; set; }
        public bool StudentId { get; set; }
    }
    public class ProfileResultInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public int StudentId { get; set; }
    }
}
