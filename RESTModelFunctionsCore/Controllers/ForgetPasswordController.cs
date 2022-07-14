using System.Security.Cryptography;
using System.Text;
using LMS.Common.ViewModels;
using LMSLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgetPasswordController : ControllerBase
    {
        [HttpPost]
        public ActionResult<ForgetPasswordResultInfo> Post([FromBody] SCredential creds)
        {
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
                    VmStudent student = students.First();

                    string original = RESTModelFunctionsCore.Messages.ForgotPassword;
                    //var password = System.Web.Security.Membership.GeneratePassword(10, 2);
                    var password = Helper.Password.Generate(10, 2);
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

            ForgetPasswordResultInfo ri = new ForgetPasswordResultInfo()
            {
                error = error,
                success = success
            };

            ForgetPasswordResultInfo result = ri;
            return Ok(result);
        }
        private List<VmStudent> GetStudent(string email)
        {
            string sqlStudent = $@"select s.Email, s.Password from Student s where s.Email = '{email}'";

            var studentData = SQLHelper.RunSqlQuery(sqlStudent);
            List<VmStudent> studentList = new List<VmStudent>();

            foreach (var item in studentData)
            {
                VmStudent studentInfo = new VmStudent
                {
                    Email = (string)item[0],
                    Password = (string)item[1]
                };
                studentList.Add(studentInfo);
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
    }
    public class SCredential
    {
        public string registerEmail { get; set; }
    }
    public class ForgetPasswordResultInfo
    {
        public string error { get; set; }
        public string success { get; set; }
    }
}
