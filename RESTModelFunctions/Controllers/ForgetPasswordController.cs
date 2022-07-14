using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
//using EFModel;
using LMSLibrary;
using LMS.Common.ViewModels;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ForgetPasswordController : ApiController
    {
        // POST api/Forget Password
        public class SCredential
        {
            public string registerEmail { get; set; }
        }
        public IHttpActionResult Post([FromBody] SCredential creds)
        {
            //MaterialEntities model = new MaterialEntities();
            //List<Student> students = model.Students.Where(student => student.Email.ToLower() == creds.registerEmail.ToLower()).ToList();
            List<VmStudent> students = GetStudent(creds.registerEmail.ToLower());

            string error = "";
            string success = "";
            if (students.Count < 1)
            {
                error = "The email address that you have, doesn't match with any registered email.";
            }
            else if (students.Count > 1)
            {
                error = "The email address is duplicated";
            }
            else
            {
                try
                {
                    //Student student = students.First();
                    VmStudent student = students.First();

                    string original = RESTModelFunctions.Messages.ForgotPassword;

                    var password = System.Web.Security.Membership.GeneratePassword(10, 2);
                    student.Password = hashPassword(password);
                    
                    //model.SaveChanges();

                    string msg = original.Replace("<Password>", password);

                    EmailHelper.SendEmail(
                            new EmailHelper.Message
                            {
                                Subject = "Learning System Password",
                                Recipient = student.Email,
                                Body = msg
                            }
                         );

                    success = "Your new Password has been sent to your register email successfully.";
                }
                catch (Exception)
                {
                    error = "The process failed";
                }
            }
            ResultInfo ri = new ResultInfo()
            {
                error = error,
                success = success
            };

            ResultInfo result = ri;
            System.Web.Http.Results.OkNegotiatedContentResult<ResultInfo> serializedResult = Ok(result);
            return serializedResult;
        }
        private List<VmStudent> GetStudent(string email)
        {
            string sqlStudent = $@"select s.Email, s.Password from Student s where s.Email = '{email}'";

            var studentData = SQLHelper.RunSqlQuery(sqlStudent);
            List<VmStudent> studentList = new List<VmStudent>();

            if (studentList.Count > 0)
            {
                foreach (var item in studentData)
                {
                    VmStudent studentInfo = new VmStudent
                    {
                        Email = (string)item[0],
                        Password = (string)item[1]
                    };
                    studentList.Add(studentInfo);
                }
            }
            return studentList;
        }
        private string hashPassword(string password)
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
        public class ResultInfo
        {
            public string error { get; set; }
            public string success { get; set; }
        }
    }
}
