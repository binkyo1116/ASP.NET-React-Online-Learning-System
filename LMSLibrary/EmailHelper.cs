using System.Net.Mail;

namespace LMSLibrary
{
    public class EmailHelper
    {
        public struct From
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string DisplayName { get; set; }
        }

        public struct Message
        {
            public string Recipient { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
        }

        public static void SendEmail(Message message)
        {
            From from = new From();
            from.UserName = "support@letsusedata.com";
            from.Password = "Zoz65857";
            from.DisplayName = "Support";

            SendEmail(from, message);
        }

        public static void SendEmail(From from, Message message)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(message.Recipient));
            msg.From = new MailAddress(from.UserName, from.DisplayName);
            msg.Sender = new MailAddress(from.UserName, from.DisplayName);
            msg.Subject = message.Subject;
            msg.Body = message.Body;
            msg.IsBodyHtml = false;
            SmtpClient client = new SmtpClient
            {
                Host = "smtp.office365.com",
                Credentials = new System.Net.NetworkCredential(from.UserName, from.Password),
                Port = 587,
                EnableSsl = true
            };
            client.Send(msg);
        }
    }
}
