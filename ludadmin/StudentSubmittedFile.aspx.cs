using EFModel;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class StudentSubmittedFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }
        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //if (DropDownListCourse.SelectedValue != "")
                //{
                //    var model = db.Courses.Find(Convert.ToInt32(DropDownListCourse.SelectedValue)).CourseObjectives.ToList();
                //    SetDatainGrid(model.ToList());
                //}
                //else
                //{
                //    var model = db.CourseObjectives;
                //    SetDatainGrid(model.ToList());
                //}

                var model = db.Submissions
                    .Where(x => x.CodingProblem.Title == "[Tableau] Calculated Fields")
                    .Where(
                    x => x.CodingProblem.Language == "Excel" ||
                    x.CodingProblem.Language == "Tableau"
                    ).Select(y => new { y.Id, StudentId = y.Student.StudentId, Student = y.Student.Name, CodingProblem = y.CodingProblem.Title })
                    .OrderByDescending(x => x.Id)
                    .ToList();

                var result = model.GroupBy(x => x.Student);

                var result1 = result.Select(x => x.First());

                foreach (var s in result1)
                {
                    Submission submission = db.Submissions.Find(s.Id);
                    //---------------------------------------------
                    string base64code = submission.Code;
                    string filename = submission.Student.StudentId + "_" + submission.Id + ".twb";
                    File.WriteAllText(@"c:\\00\\" + filename, base64code);
                }

                //SetDatainGrid(model);
                GridView1.DataSource = result1.OrderBy(x=>x.StudentId);
                GridView1.DataBind();
            }
        }
        //private void SetDatainGrid(List<Submission> model)
        //{
        //    GridView1.DataSource = model.OrderBy(x => x.Id);
        //    GridView1.DataBind();
        //}

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DownloadExcelFile")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    try
                    {
                        int rowIndex = Convert.ToInt32(e.CommandArgument);
                        int id = Convert.ToInt32(GridView1.DataKeys[rowIndex].Values[0]);
                        Submission submission = db.Submissions.Find(id);
                        //---------------------------------------------
                        string base64code = submission.Code;
                        string filename = submission.Student.StudentId + "_" + submission.Id + ".twb";
                        File.WriteAllText(@"c:\\00\\" + filename, base64code);
                        byte[] bytes = Convert.FromBase64String(base64code);
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AppendHeader("content-disposition", "attachment; filename=" + filename);
                        //Response.ContentType = "application/vnd.ms-excel";
                        //Response.AppendHeader("content-disposition", "attachment; filename="+ filename+"");
                        Response.Charset = "";
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.BinaryWrite(bytes);
                        Response.End();
                    }
                    catch (Exception ex)
                    {

                        string script = "alert(\"" + ex.Message + "\");";
                        ScriptManager.RegisterStartupScript(this, GetType(),
                                              "ServerControlScript", script, true);
                    }

                }

            }
        }
    }
}