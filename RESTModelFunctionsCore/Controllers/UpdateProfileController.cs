using System.Security.Cryptography;
using System.Text;
using LMS.Common.HelperModels;
using LMS.Common.ViewModels;
using LMSLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTModelFunctionsCore.Helper;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateProfileController : ControllerBase
    {
        [HttpPost]
        public ActionResult<UpdateProfileResultInfo> Post([FromBody] UpdateProfileStudentInfo si)
        {
            VmStudent student = VmModelHelper.GetStudentInfoByHash(si.Hash);
            UpdateProfileResultInfo ri = new UpdateProfileResultInfo();

            if (si.InfoType == "Name")
            {
                student.Name = si.Name;
                ri.Result = "OK";
                UpdateStudentName(student.StudentId, si.Name);
            }
            else if (si.InfoType == "Password")
            {
                if (student.Password == hashPassword(si.CurrentPassword) || student.Password == si.CurrentPassword)
                {
                    student.Password = hashPassword(si.NewPassword);
                    UpdateStudentPassword(student.StudentId, student.Password);
                    ri.Result = "OK";
                }
                else
                {
                    ri.Result = "Error";
                }
            }
            else if (si.InfoType == "Photo")
            {
                student.Photo = Convert.FromBase64String(si.Photo);
                UpdateStudentPhoto(student.StudentId, student.Photo);
                ri.Result = "OK";
            }
            return Ok(ri);
        }
        protected string hashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash;
            }
        }
        private void UpdateStudentName(int studentId, string name)
        {
            string sqlQuizQuestionRating = $@"update Student set Name = {name}  where StudentId = {studentId}";
            SQLHelper.RunSqlUpdate(sqlQuizQuestionRating);
        }
        private void UpdateStudentPassword(int studentId, string password)
        {
            string sqlQuizQuestionRating = $@"update Student set Password = '{password}' where StudentId = {studentId}";
            SQLHelper.RunSqlUpdate(sqlQuizQuestionRating);
        }
        private void UpdateStudentPhoto(int studentId, byte[] photo)
        {
            string sqlQuizQuestionRating = $@"InsertStudentImage";
            List<Param> paramList = new List<Param>();
            paramList.Add(new Param() { Name = "StudentId", Value = studentId });
            paramList.Add(new Param() { Name = "Image", Value = photo });
            SQLHelper.RunSqlUpdateWithParam(sqlQuizQuestionRating, paramList);
        }
    }
    public class UpdateProfileStudentInfo
    {
        public string Hash { get; set; }
        public string Name { get; set; }
        public string NewPassword { get; set; }
        public string CurrentPassword { get; set; }
        public string Photo { get; set; }
        public string InfoType { get; set; }
    }
    public class UpdateProfileResultInfo
    {
        public string Result { get; set; }
    }
}
