using System.Text.RegularExpressions;
using LMS.Common.ViewModels;
using LMSLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTModelFunctionsCore.Helper;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        [HttpPost]
        public ActionResult<ContactResultInfo> Post([FromBody] UserCredential creds)
        {
            VmStudent student = VmModelHelper.GetStudentInfoByHash(creds.Hash);
            string error = "";
            string success = "";
            if (student != null)
            {
                ValidationInfo vInfo = ValidMessage(student.Name, student.Email, creds.Message);
                if (vInfo.Status)
                {
                    try
                    {
                        var emailbody = creds.Message.Trim() + "\n\n" + "Name: " + student.Name + "\n" + "Email: " + student.Email;
                        EmailHelper.SendEmail(
                                new EmailHelper.Message
                                {
                                    Subject = "Learning System, Message from: " + student.Name,
                                    Recipient = "marcelo@letsusedata.com",
                                    Body = emailbody
                                }
                             );
                        success = "Your Message has been sent successfully.";
                    }
                    catch (Exception)
                    {
                        error = "The process failed";
                    }
                }
                else
                {
                    error = vInfo.Note;
                }
            }
            else
            {
                error = "Sorry! Message sending is failed. Student cannot found.";
            }
            ContactResultInfo ri = new ContactResultInfo()
            {
                error = error,
                success = success
            };

            ContactResultInfo result = ri;
            return Ok(result);
        }
        private ValidationInfo ValidMessage(string senderName, string senderEmail, string message)
        {
            ValidationInfo result = new ValidationInfo();
            if (string.IsNullOrWhiteSpace(senderName))
            {
                result.Status = false;
                result.Note = "Sorry! The Name field cannot be left blank.";
            }
            else if (string.IsNullOrWhiteSpace(senderEmail))
            {
                result.Status = false;
                result.Note = "Sorry! The Email field cannot be left blank.";
            }
            else if (!validateEmail(senderEmail.Trim()))
            {
                result.Status = false;
                result.Note = "Sorry! Your Email address is not valid. Please provide a valid email address.";
            }
            else if (string.IsNullOrWhiteSpace(message))
            {
                result.Status = false;
                result.Note = "Sorry! The Message field cannot be left blank";
            }
            else if (!validateMessageLength(message.Trim()))
            {
                result.Status = false;
                result.Note = "Sorry! The message do not support more than 400 character.Total Character of your message is " + message.Length + ".";
            }
            else
            {
                result.Status = true;
            }
            return result;
        }
        private bool validateEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private bool validateMessage(string message)
        {
            string messagePattern = @"^[a-zA-Z0-9?.,:! \n]*$";
            bool isValid = Regex.IsMatch(message, messagePattern);
            return isValid;
        }
        private bool validateMessageLength(string message)
        {
            var r = true;
            if (message.Length > 400)
            {
                r = false;
            }
            return r;
        }
        private VmStudent GetStudentInfo(string hashedPassword)
        {
            string sqlQueryStudent = $@"select StudentId, Name, Email from Student where Hash = '{hashedPassword}'";

            var studentData = SQLHelper.RunSqlQuery(sqlQueryStudent);
            VmStudent studentinfo = null;

            if (studentData.Count > 0)
            {
                List<object> st = studentData[0];
                studentinfo = new VmStudent
                {
                    StudentId = (int)st[0],
                    Name = st[1].ToString(),
                    Email = st[3].ToString()
                };
            }
            return studentinfo;
        }
    }
    public class ContactResultInfo
    {
        public string error { get; set; }
        public string success { get; set; }
    }
    public class ValidationInfo
    {
        public bool Status { get; set; }
        public string Note { get; set; }
    }
    public class UserCredential
    {
        public string Hash { get; set; }
        public string Message { get; set; }

    }
}
