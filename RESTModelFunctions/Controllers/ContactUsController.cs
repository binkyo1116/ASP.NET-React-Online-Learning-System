using System;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;
//using EFModel;
using LMSLibrary;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ContactUsController : ApiController
    {
        // POST api/Contact Us
        public class UserCredential
        {
            public string SenderName { get; set; }
            public string SenderEmail { get; set; }
            public string Message { get; set; }

        }
        public IHttpActionResult Post([FromBody] UserCredential creds)
        {
            //MaterialEntities model = new MaterialEntities();

            string error = "";
            string success = "";
            ValidationInfo vInfo = ValidMessage(creds.SenderName, creds.SenderEmail, creds.Message);
            if (vInfo.Status)
            {
                try
                {
                    var emailbody = creds.Message.Trim() + "\n\n" + "Name: " + creds.SenderName + "\n" + "Email: " + creds.SenderEmail;
                    EmailHelper.SendEmail(
                            new EmailHelper.Message
                            {
                                Subject = "Learning System, Message from Contact Us page, Sender: " + creds.SenderName,
                                Recipient = "support@letsusedata.com",
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
            else if (!validateMessage(message.Trim()))
            {
                result.Status = false;
                result.Note = "Sorry! Message field do not support any special character.";
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
    }
}
