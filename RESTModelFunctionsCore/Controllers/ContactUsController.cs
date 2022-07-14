using System.Text.RegularExpressions;
using LMSLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        [HttpPost]
        public ActionResult<ContactUsResultInfo> Post([FromBody] ContactUsUserCredential creds)
        {
            string error = "";
            string success = "";
            ContactUsValidationInfo vInfo = ValidMessage(creds.SenderName, creds.SenderEmail, creds.Message);
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
            ContactUsResultInfo ri = new ContactUsResultInfo()
            {
                error = error,
                success = success
            };

            ContactUsResultInfo result = ri;
            return Ok(result);
        }
        private ContactUsValidationInfo ValidMessage(string senderName, string senderEmail, string message)
        {
            ContactUsValidationInfo result = new ContactUsValidationInfo();
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
    }
    public class ContactUsUserCredential
    {
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Message { get; set; }

    }
    public class ContactUsResultInfo
    {
        public string error { get; set; }
        public string success { get; set; }
    }
    public class ContactUsValidationInfo
    {
        public bool Status { get; set; }
        public string Note { get; set; }
    }
}
