using AdminPages.Services;
using EFModel;
using LMSLibrary;
using System;
using System.Linq;

namespace AdminPages
{
    public partial class SubmissionControl : System.Web.UI.UserControl
    {
        public Submission submission { get; set; }
        public StudentGradable studentGradable { get; set; }
        public Student student { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadPage();
        }
        private void LoadPage()
        {
            lblStudentName.Text = student.Name;
            lblTimeStamp.Text = submission.TimeStamp.ToString("dd MMM, yyyy");
            TextBoxComment.Text = submission.Comment;
            TextBoxCode.Text = submission.Code;
            TextBoxGrade.Text = "";
            if (studentGradable != null)
            {
                TextBoxGrade.Text = Convert.ToString(studentGradable.Grade);
            }

            lblMessage.Text = "";
        }
        protected void btnSubmitGrade_Click(object sender, EventArgs e)
        {
            try
            {
                var stgrade = TextBoxGrade.Text.Trim();
                var comment = TextBoxComment.Text.Trim();
                //-----------Update Comment---------------------
                using (MaterialEntities db = new MaterialEntities())
                {
                    var sub = db.Submissions.Find(submission.Id) ;
                    sub.Comment = comment;
                    db.SaveChanges();
                }
                //-----------Update Grade---------------------
                if (studentGradable == null)
                {
                    lblMessage.Text = "Sorry! Grade is Null!";
                }
                else {
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        var gr = db.StudentGradables.Find(studentGradable.Id);
                        gr.Grade = Convert.ToInt32(stgrade);
                        db.SaveChanges();
                    }
                    lblMessage.Text = "Save Successfully!";
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}