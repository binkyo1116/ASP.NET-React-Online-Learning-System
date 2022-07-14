using EFModel;
using System;
using System.Linq;

namespace AdminPages
{
    public partial class SupportTicketDetailControl : System.Web.UI.UserControl
    {
        public SupportTicketMessage SupportTicketMessage { get; set; }
        public Student Student { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadMessageDetailPage();
        }
        private void LoadMessageDetailPage()
        {

            PersonName.Text = Student.Name;
            byte[] photo = SupportTicketMessage.Student.Photo;

            if (photo != null)
            {
                byte[] img = photo.ToArray();
                PersonImage.ImageUrl = "data:image;base64," + Convert.ToBase64String(img);
            }
            lblTextMessage.Text = SupportTicketMessage.Message;

            byte[] ScreenShort = SupportTicketMessage.Image;
            if (ScreenShort != null)
            {
                byte[] img = ScreenShort.ToArray();
                imgScreenShort.ImageUrl = "data:image;base64," + Convert.ToBase64String(img);
            }
        }
    }
}