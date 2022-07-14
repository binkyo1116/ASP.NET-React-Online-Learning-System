using EFModel;
using System;
using System.IO;

namespace OnlineLearningSystem
{
    public partial class Logout : System.Web.UI.UserControl
    {
        private Student student = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            string page = Path.GetFileName(Request.PhysicalPath);

            if (page.ToLower().StartsWith("courseselection"))
            {
                return;
            }

            student = (Student)Session["Student"];
            if (student == null)
            {
                Response.Redirect("Login.html");
            }

            if (page.ToLower().StartsWith("add") || page.ToLower().StartsWith("admin")
                    || page.ToLower().StartsWith("studentmanagement")
                    || page.ToLower().StartsWith("grade"))
            {
                if (student.UserName != "admin")
                {
                    Response.Redirect("Login.html");
                }
            }
            //------------------Appear Syllabus BTN ------------------
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.html");
        }

        protected void btnComment_Click(object sender, EventArgs e)
        {
            Response.Write($"<script>window.open ('Feedback.aspx','_blank');</script>");
        }

        protected void ButtonUpdateProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("UpdateProfile.aspx");
        }

        protected void btnStatisticsPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("StatisticsPage.aspx");
        }
    }
}