using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using LMS.Common.HelperModels;
//using EFModel;
using LMS.Common.ViewModels;
using LMSLibrary;
using RESTModelFunctions.Helper;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UpdateProfileController : ApiController
    {
        public class StudentInfo
        {
            public string Hash { get; set; }
            public string Name { get; set; }
            public string NewPassword { get; set; }
            public string CurrentPassword { get; set; }
            public string Photo { get; set; }
            public string InfoType { get; set; }
        }

        // POST api/Course
        public IHttpActionResult Post([FromBody] StudentInfo si)
        {
            //MaterialEntities model = new MaterialEntities();

            //Student student = model.Students.Where(stud => stud.Hash == si.Hash).FirstOrDefault();
            VmStudent student = VmModelHelper.GetStudentInfoByHash(si.Hash);
            ResultInfo ri = new ResultInfo();
            if (si.InfoType == "Name")
            {
                student.Name = si.Name;
                //model.SaveChanges();
                ri.Result = "OK";
                UpdateStudentName(student.StudentId, si.Name);
            }
            else if (si.InfoType == "Password")
            {
                if (student.Password == hashPassword(si.CurrentPassword) || student.Password == si.CurrentPassword)
                {
                    student.Password = hashPassword(si.NewPassword);
                    //model.SaveChanges();
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
                //model.SaveChanges();
                UpdateStudentPhoto(student.StudentId, student.Photo);
                ri.Result = "OK";
            }

            return Ok(ri);
        }

        protected string hashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Send a password to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                // Get the hashed string.  
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                // Print the string.   
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
            string sqlQuizQuestionRating = $@"update Student set Password = {password}  where StudentId = {studentId}";

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
        public class ResultInfo
        {
            public string Result { get; set; }
        }
    }
}

