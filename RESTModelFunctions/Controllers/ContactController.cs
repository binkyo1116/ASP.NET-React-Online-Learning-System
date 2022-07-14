using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;
using LMSLibrary;
using LMS.Common.ViewModels;
using RESTModelFunctions.Helper;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ContactController : ApiController
    {
        // POST api/Contact Us
        public class UserCredential
        {
            public string Hash { get; set; }
            public string Message { get; set; }

        }
        public IHttpActionResult Post([FromBody] UserCredential creds)
        {
            //MaterialEntities model = new MaterialEntities();
            //VmStudent student = GetStudentInfo(creds.Hash);
            VmStudent student = VmModelHelper.GetStudentInfoByHash(creds.Hash);
            //model.Students.Where(stud => stud.Hash == creds.Hash).FirstOrDefault();
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
            else {
                error = "Sorry! Message sending is failed. Student cannot found.";
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
            //else if (!validateMessage(message.Trim()))
            //{
            //    result.Status = false;
            //    result.Note = "Sorry! Message field do not support any special character.";
            //}
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
        public class ResultInfo
        {
            public string error { get; set; }
            public string success { get; set; }
        }
        public class ValidationInfo
        {
            public bool Status { get; set; }
            public string Note { get; set; }
        }
        private VmStudent GetStudentInfo(string hashedPassword)
        {
            string sqlQueryStudent = $@"select StudentId, Name, Email
                                        from Student
                                        where Hash = '{hashedPassword}'"; // AND (Password = '{hashedPassword}' OR Password = '{password}');

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
}
