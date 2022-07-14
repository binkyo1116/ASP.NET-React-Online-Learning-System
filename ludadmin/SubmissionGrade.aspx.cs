using AdminPages;
using EFModel;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class SubmissionGrade : System.Web.UI.Page
    {
        public bool isControlLoad { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDownCourse();
            }
            if (IsPostBack)
            {
                LoadSubControl();
            }
        }
        #region================Dropdown List============================================
        private void BindDropDownCourse()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                DropDownListCourseFilter2.DataSource = db.Courses.Select(x => new { x.Id, x.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourseFilter2.DataTextField = "Name";
                DropDownListCourseFilter2.DataValueField = "Id";
                DropDownListCourseFilter2.DataBind();
                DropDownListCourseFilter2.Items.Insert(0, new ListItem("--Select Course--", ""));
            }
        }
        private void BindDropDownCourseInstance()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //---------------Coruse Instance Droup Down list----------------
                DropDownListCourseInstanceFilter.DataSource = db.CourseInstances.Where(ci => ci.Active).Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourseInstanceFilter.DataTextField = "Name";
                DropDownListCourseInstanceFilter.DataValueField = "Id";
                DropDownListCourseInstanceFilter.DataBind();
                DropDownListCourseInstanceFilter.Items.Insert(0, new ListItem("--Select Course Instance Filter--", ""));
            }
        }
        private void BindDropDownCodingProblem()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var model = db.CodingProblems.Select(x => new { Id = x.Id, Title = x.Title }).ToList();
                //--------------- Droup Down list----------------
                //DropDownListCodingProblem.DataSource = model;
                //DropDownListCodingProblem.DataTextField = "Title";
                //DropDownListCodingProblem.DataValueField = "Id";
                //DropDownListCodingProblem.DataBind();
                //DropDownListCodingProblem.Items.Insert(0, new ListItem("--Select Coding Problem--", ""));
                //-------------------------------------------
            }
        }
        private void BindDropDownModuleObjectiveFilter()
        {
            if (DropDownListCourseInstanceFilter.SelectedValue != "")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    int courseId = db.CourseInstances.Find(courseInstanceId).CourseId;
                    var modelObjectives = (from a in db.ModuleObjectives.Where(x => x.Active && x.Modules.Where(m => m.Active && m.CourseObjectives.Where(co => co.Active && co.Courses.Where(c => c.Id == courseId).Any()).Any()).Any())
                                           select new { Id = a.Id, Title = a.Description }).OrderBy(y => y.Id).ToList();
                    //--------------------Bind Dropdown--------------------------
                    DropDownListModuleObjectiveFilter.DataSource = modelObjectives;
                    DropDownListModuleObjectiveFilter.DataTextField = "Title";
                    DropDownListModuleObjectiveFilter.DataValueField = "Id";
                    DropDownListModuleObjectiveFilter.DataBind();
                    DropDownListModuleObjectiveFilter.Items.Insert(0, new ListItem("--Select Module--", ""));
                }
            }
        }
        private void clearAll()
        {
            DropDownListCourseFilter2.SelectedIndex = -1;
            DropDownListQuarterFilter2.Items.Clear();
            DropDownListCourseInstanceFilter.Items.Clear();
            PanelSubmission.Controls.Clear();
            DropDownListCodingProblem.Items.Clear();
            DropDownListModuleObjectiveFilter.Items.Clear();
            DropDownListStudent.Items.Clear();
            lblMessage.Text = "";
        }
        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            clearAll();
        }
        protected void DropDownListCodingProblem_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadSubControl();
            PanelSubmission.Controls.Clear();
        }
        protected void LoadSubControl()
        {
            //if (isControlLoad)
            //{
            //    return;
            //}
            if (DropDownListCodingProblem.SelectedValue != "")
            {
                PanelSubmission.Controls.Clear();
                isControlLoad = true;
                using (MaterialEntities db = new MaterialEntities())
                {
                    var codingProblemId = Convert.ToInt32(DropDownListCodingProblem.SelectedValue);
                    var courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    //var codingProblem = db.CodingProblems.Find(codingProblemId);
                    if (DropDownListStudent.SelectedValue != "")
                    {
                        var studentId = Convert.ToInt32(DropDownListStudent.SelectedValue);
                        var submission = db.CodingProblems.Find(codingProblemId)
                        .Submissions.Where(s => s.StudentId == studentId && s.CourseInstanceId == courseInstanceId)
                        .OrderByDescending(s => s.Id)
                        .FirstOrDefault();
                        //----------------------------------------
                        if (submission != null)
                        {

                            var grade = db.StudentGradables.Where(x => x.StudentId == submission.StudentId && x.CodingProblemId == submission.CodingProblemId & x.CourseInstanceId == submission.CourseInstanceId).FirstOrDefault();

                            SubmissionControl control = (SubmissionControl)Page.LoadControl("SubmissionControl.ascx");
                            control.submission = submission;
                            control.studentGradable = grade;
                            control.student = submission.Student;
                            PanelSubmission.Controls.Add(control);
                        }
                    }
                    else
                    {
                        var submissions = db.CodingProblems.Find(codingProblemId).Submissions.Where(s => s.CourseInstanceId == courseInstanceId);
                        var students = db.CourseInstances.Where(ci => ci.Id == courseInstanceId && ci.Active).FirstOrDefault().Students;
                        foreach (var student in students)
                        {
                            var submission = db.CodingProblems.Find(codingProblemId)
                              .Submissions.Where(s => s.StudentId == student.StudentId)
                              .OrderByDescending(s => s.Id)
                              .FirstOrDefault();
                            if (submission != null)
                            {
                                var grade = db.StudentGradables.Where(x => x.StudentId == student.StudentId && x.CodingProblemId == codingProblemId).FirstOrDefault();
                                SubmissionControl control = (SubmissionControl)Page.LoadControl("SubmissionControl.ascx");
                                control.submission = submission;
                                control.studentGradable = grade;
                                control.student = submission.Student;
                                PanelSubmission.Controls.Add(control);
                            }
                        }
                    }
                }
            }
        }
        protected void DropDownListCourseInstanceFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterCodingProblem();
            BindDropDownModuleObjectiveFilter();
        }
        protected void DropDownListModuleObjectiveFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterCodingProblem();
        }
        private void filterCodingProblem()
        {
            PanelSubmission.Controls.Clear();
            using (MaterialEntities db = new MaterialEntities())
            {
                if (DropDownListModuleObjectiveFilter.SelectedValue != "" && DropDownListCourseInstanceFilter.SelectedValue != "")
                {
                    int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjectiveFilter.SelectedValue);
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    DropDownListCodingProblem.DataSource = db.CourseInstanceCodingProblems.Where(x => x.ModuleObjectiveId == moduleObjectiveId && x.CourseInstanceId == courseInstanceId && x.Active).Select(y => y.CodingProblem).OrderBy(x => x.Id).ToList();
                }
                else if (DropDownListCourseInstanceFilter.SelectedValue != "")
                {
                    int courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    DropDownListCodingProblem.DataSource = db.CourseInstanceCodingProblems.Where(x => x.CourseInstanceId == courseInstanceId && x.Active).Select(y => y.CodingProblem).OrderBy(x => x.Id).ToList();
                }
                else if (DropDownListModuleObjectiveFilter.SelectedValue != "")
                {
                    int moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjectiveFilter.SelectedValue);
                    DropDownListCodingProblem.DataSource = db.CourseInstanceCodingProblems.Where(x => x.ModuleObjectiveId == moduleObjectiveId && x.Active).Select(y => y.CodingProblem).OrderBy(x => x.Id).ToList();
                }
                else
                {
                    DropDownListCodingProblem.DataSource = db.CodingProblems.Where(x => x.Active).Select(x => new { Id = x.Id, Title = x.Title }).ToList();
                }
                //--------------- Droup Down list----------------
                DropDownListCodingProblem.DataTextField = "Title";
                DropDownListCodingProblem.DataValueField = "Id";
                DropDownListCodingProblem.DataBind();
                DropDownListCodingProblem.Items.Insert(0, new ListItem("--Select Coding Problem--", ""));
            }
        }

        protected void DropDownListCourseFilter2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string i = DropDownListCourseFilter2.SelectedValue;
            PanelSubmission.Controls.Clear();
            DropDownListCourseInstanceFilter.Items.Clear();
            DropDownListCodingProblem.Items.Clear();
            DropDownListStudent.Items.Clear();
            DropDownListModuleObjectiveFilter.Items.Clear();
            if (i != "")
            {

                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseId = Convert.ToInt32(i);
                    IQueryable<CourseInstance> courseIns = db.CourseInstances.Where(ci => ci.Course.Id == courseId && ci.Active);

                    var list = courseIns.Select(x => new { Name = x.Quarter.StartDate + " TO " + x.Quarter.EndDate, Id = x.Id }).OrderBy(x => x.Id).ToList();
                    //---------------Coruse Instance Droup Down list----------------
                    DropDownListQuarterFilter2.DataSource = list;
                    DropDownListQuarterFilter2.DataTextField = "Name";
                    DropDownListQuarterFilter2.DataValueField = "Id";
                    DropDownListQuarterFilter2.DataBind();
                    //--------------------------------------------------------------------------------
                    if (courseIns.Count() == 1)
                    {
                        BindDropDownListsCourseInstanceFilter(courseIns.FirstOrDefault().Id);
                        BindDropDownListsStudent(courseIns.FirstOrDefault().Id);
                        //--------------------------------------------
                        filterCodingProblem();
                        BindDropDownModuleObjectiveFilter();
                    }
                    else
                    {
                        DropDownListQuarterFilter2.Items.Insert(0, new ListItem("--Select One--", ""));
                    }
                }
            }
        }
        protected void DropDownListQuarterFilter2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string i = DropDownListQuarterFilter2.SelectedValue;
            if (i != "")
            {
                int courseInstanceId = Convert.ToInt32(i);
                BindDropDownListsCourseInstanceFilter(courseInstanceId);
                BindDropDownListsStudent(courseInstanceId);
                //---------------------------------------
                filterCodingProblem();
                BindDropDownModuleObjectiveFilter();
            }
        }
        protected void BindDropDownListsCourseInstanceFilter(int courseInstanceId)
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var list = db.CourseInstances.Where(ci => ci.Id == courseInstanceId && ci.Active).Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
                //---------------Coruse Instance Droup Down list----------------
                DropDownListCourseInstanceFilter.DataSource = list;
                DropDownListCourseInstanceFilter.DataTextField = "Name";
                DropDownListCourseInstanceFilter.DataValueField = "Id";
                DropDownListCourseInstanceFilter.DataBind();
            }
        }
        protected void BindDropDownListsStudent(int courseInstanceId)
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var list = db.CourseInstances.Where(ci => ci.Id == courseInstanceId && ci.Active).FirstOrDefault().Students.Select(s => new { s.Name, Id = s.StudentId }).ToList();
                //---------------Coruse Instance Droup Down list----------------
                DropDownListStudent.DataSource = list;
                DropDownListStudent.DataTextField = "Name";
                DropDownListStudent.DataValueField = "Id";
                DropDownListStudent.DataBind();
                DropDownListStudent.Items.Insert(0, new ListItem("--Select One--", ""));
            }
        }
        #endregion =====================================================================================
        protected void DropDownListStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadSubControl();
            PanelSubmission.Controls.Clear();
        }

        protected void btnSearchResult_Click(object sender, EventArgs e)
        {
            //LoadSubControl();

        }

        protected void DropDownListCProblem_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}