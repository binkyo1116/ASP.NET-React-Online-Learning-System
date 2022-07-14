using EFModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddCodingProblemOld : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //BindDropDownCourse();
                BindDropDownCourse();
                BindDropDownCodingProblem();
                BindDropDownListLanguage();
            }
        }
        private void BindDropDownCourse()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //---------------Coruse Droup Down list----------------
                DropDownListCourseFilter1.DataSource = db.Courses.Select(x => new { x.Id, x.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourseFilter1.DataTextField = "Name";
                DropDownListCourseFilter1.DataValueField = "Id";
                DropDownListCourseFilter1.DataBind();
                DropDownListCourseFilter1.Items.Insert(0, new ListItem("--Select Course--", ""));

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
                DropDownListCourseInstance.DataSource = db.CourseInstances.Where(ci=> ci.Active).Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
                DropDownListCourseInstance.DataTextField = "Name";
                DropDownListCourseInstance.DataValueField = "Id";
                DropDownListCourseInstance.DataBind();
                DropDownListCourseInstance.Items.Insert(0, new ListItem("--Select Course Instance--", ""));

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
                DropDownListCodingProblem.DataSource = model;
                DropDownListCodingProblem.DataTextField = "Title";
                DropDownListCodingProblem.DataValueField = "Id";
                DropDownListCodingProblem.DataBind();
                DropDownListCodingProblem.Items.Insert(0, new ListItem("--Select Coding Problem--", ""));
            }
        }

        private void BindDropDownModuleObjective()
        {
            if (DropDownListCourseInstance.SelectedValue!="") {
                using (MaterialEntities db = new MaterialEntities())
                {
                    var courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                    int courseId = db.CourseInstances.Find(courseInstanceId).CourseId;
                    var modelObjectives = (from a in db.ModuleObjectives.Where(x => x.Active && x.Modules.Where(m => m.Active && m.CourseObjectives.Where(co => co.Active && co.Courses.Where(c => c.Id == courseId).Any()).Any()).Any())
                                          select new { Id = a.Id, Title = a.Description }).OrderBy(y => y.Id).ToList();
                    //List<DropDownListItem> model = new List<DropDownListItem>();
                    //model = (from a in db.ModuleObjectives
                    //         where a.Active
                    //         select new DropDownListItem { Id = a.Id, Title = a.Description }).OrderBy(y => y.Id).ToList();
                    //--------------------Bind Dropdown--------------------------
                    DropDownListModuleObjective.DataSource = modelObjectives;
                    DropDownListModuleObjective.DataTextField = "Title";
                    DropDownListModuleObjective.DataValueField = "Id";
                    DropDownListModuleObjective.DataBind();
                    DropDownListModuleObjective.Items.Insert(0, new ListItem("--Select Module--", ""));
                }
            }
        }
        private void BindDropDownModuleObjectiveFilter()
        {
            if (DropDownListCourseInstanceFilter.SelectedValue != "")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    var courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    int courseId = db.CourseInstances.Find(courseInstanceId).CourseId;
                    var modelObjectives = (from a in db.ModuleObjectives.Where(x => x.Active && x.Modules.Where(m => m.Active && m.CourseObjectives.Where(co => co.Active && co.Courses.Where(c => c.Id == courseId).Any()).Any()).Any())
                                           select new { Id = a.Id, Title = a.Description }).OrderBy(y => y.Id).ToList();
                    //List<DropDownListItem> model = new List<DropDownListItem>();
                    //model = (from a in db.ModuleObjectives
                    //         where a.Active
                    //         select new DropDownListItem { Id = a.Id, Title = a.Description }).OrderBy(y => y.Id).ToList();
                    //--------------------Bind Dropdown--------------------------
                    DropDownListModuleObjectiveFilter.DataSource = modelObjectives;
                    DropDownListModuleObjectiveFilter.DataTextField = "Title";
                    DropDownListModuleObjectiveFilter.DataValueField = "Id";
                    DropDownListModuleObjectiveFilter.DataBind();
                    DropDownListModuleObjectiveFilter.Items.Insert(0, new ListItem("--Select Module--", ""));
                }
            }
        }

        private void crearText()
        {
            TextBoxParameterValues.Text = "";
            TextBoxExpectedOutput.Text = "";
        }
        protected void btnCodingProblemListPage_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('QuizQuestionList.aspx','_newtab');", true);
        }
        private void clearAll()
        {
            DropDownListCourseFilter1.SelectedIndex = -1;
            DropDownListCourseFilter2.SelectedIndex = -1;
            DropDownListQuarter.Items.Clear();
            DropDownListQuarterFilter2.Items.Clear();
            DropDownListCourseInstance.Items.Clear();
            DropDownListCourseInstanceFilter.Items.Clear();

            TextBoxCourseInstanceCPMaxGrade.Text = "";
            TextBoxDueDate.Text = "";
            CheckBoxCourseInstanceCodingProblemActive.Checked = true;

            CheckBoxAutoGenerated.Checked = false;
            CheckBoxOutPutException.Checked = false;

            DropDownListCodingProblem.SelectedIndex = -1;
            TextBoxInstruction.Text = "";
            TextBoxScript.Text = "";
            TextBoxSolution.Text = "";
            TextBoxClassName.Text = "";
            TextBoxMethodName.Text = "";
            TextBoxParameterTypes.Text = "";
            //TextBoxLanguage.Text = "";
            DropDownListLanguage.SelectedIndex = -1;
            TextBoxTestCaseClass.Text = "";
            TextBoxBefore.Text = "";
            TextBoxAfter.Text = "";

            TextBoxMaxGrade.Text = "";
            TextBoxTitle.Text = "";
            //TextBoxType.Text = "";
            CheckBoxTypeCode.Checked = false;
            TextBoxAttempts.Text = "";
            TextBoxRole.Text = "";

            TextBoxParameterValues.Text = "";
            TextBoxExpectedOutput.Text = "";
            TextBoxOutputType.Text = "";

            btnAddNewCodingProblem.Attributes.Remove("disabled");
            btnAddNewCodingProblem.Style["pointer-events"] = "visible";

            lblMessage.Text = "";
            BindGrid();
            GridViewTest.DataSource = null;
            GridViewTest.DataBind();
        }
        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            clearAll();
        }
        protected void btnAssessmentListPage_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('CodingProblemList.aspx','_newtab');", true);
        }
        protected void DropDownListCodingProblem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListCodingProblem.SelectedValue != "")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    var codingProblem = db.CodingProblems.Find(Convert.ToInt32(DropDownListCodingProblem.SelectedValue));
                    TextBoxTitle.Text = codingProblem.Title;
                    //TextBoxType.Text = codingProblem.Type;
                    if (codingProblem.Type=="code") {
                        CheckBoxTypeCode.Checked = true;
                    }
                    TextBoxMaxGrade.Text = Convert.ToString(codingProblem.MaxGrade);
                    btnAddNewCodingProblem.Attributes["disabled"] = "disabled";
                    TextBoxInstruction.Text = codingProblem.Instructions;
                    TextBoxScript.Text = codingProblem.Script;
                    TextBoxSolution.Text = codingProblem.Solution;
                    TextBoxClassName.Text = codingProblem.ClassName;
                    TextBoxMethodName.Text = codingProblem.MethodName;
                    TextBoxParameterTypes.Text = codingProblem.ParameterTypes;
                    //TextBoxLanguage.Text = codingProblem.Language;
                    var language = db.Languages.Where(x => x.Name.ToLower() == codingProblem.Language.ToLower()).FirstOrDefault();
                    if (language!=null) {
                        DropDownListLanguage.SelectedValue = Convert.ToString(language.Id) ;
                    }
                    
                    TextBoxTestCaseClass.Text = codingProblem.TestCaseClass;
                    TextBoxBefore.Text = codingProblem.Before;
                    TextBoxAfter.Text = codingProblem.After;
                    TextBoxAttempts.Text = Convert.ToString(codingProblem.Attempts);
                    TextBoxRole.Text = Convert.ToString(codingProblem.Role);
                    if (codingProblem.UsesHint != null && codingProblem.UsesHint == 1)
                    {
                        CheckboxUsesHint.Checked = true;
                    }
                    else
                    {
                        CheckboxUsesHint.Checked = false;
                    }
                    BindGridTest();
                }
            }
        }
        protected void DropDownListCourseInstance_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
            BindDropDownModuleObjective();
        }
        protected void DropDownListModuleObjective_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
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
            using (MaterialEntities db = new MaterialEntities())
            {
                if (DropDownListModuleObjectiveFilter.SelectedValue != "" && DropDownListCourseInstanceFilter.SelectedValue != "")
                {
                    var moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjectiveFilter.SelectedValue);
                    var courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    DropDownListCodingProblem.DataSource = db.CourseInstanceCodingProblems.Where(x => x.ModuleObjectiveId == moduleObjectiveId && x.CourseInstanceId == courseInstanceId && x.Active ).Select(y => y.CodingProblem).OrderBy(x => x.Id).ToList();
                }
                else if (DropDownListCourseInstanceFilter.SelectedValue != "")
                {
                    var courseInstanceId = Convert.ToInt32(DropDownListCourseInstanceFilter.SelectedValue);
                    DropDownListCodingProblem.DataSource = db.CourseInstanceCodingProblems.Where(x => x.CourseInstanceId == courseInstanceId && x.Active).Select(y => y.CodingProblem).OrderBy(x => x.Id).ToList();
                }
                else if (DropDownListModuleObjectiveFilter.SelectedValue != "")
                {
                    var moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjectiveFilter.SelectedValue);
                    DropDownListCodingProblem.DataSource = db.CourseInstanceCodingProblems.Where(x => x.ModuleObjectiveId == moduleObjectiveId && x.Active).Select(y => y.CodingProblem).OrderBy(x => x.Id).ToList();
                }
                else
                {
                    DropDownListCodingProblem.DataSource = db.CodingProblems.Where(x=> x.Active).Select(x => new { Id = x.Id, Title = x.Title }).ToList();
                }

                //--------------- Droup Down list----------------
                DropDownListCodingProblem.DataTextField = "Title";
                DropDownListCodingProblem.DataValueField = "Id";
                DropDownListCodingProblem.DataBind();
                DropDownListCodingProblem.Items.Insert(0, new ListItem("--Select Coding Problem--", ""));
            }
        }



        #region ===============CourseInstanceCodingProblem============================================
        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var model = from a in db.CourseInstanceCodingProblems select a;
                if (DropDownListCourseInstance.SelectedValue != "")
                {
                    var courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                    model = from a in model
                            where a.CourseInstanceId == courseInstanceId
                            select a;
                }
                if (DropDownListModuleObjective.SelectedValue != "")
                {
                    var moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
                    model = from a in model
                            where a.ModuleObjectiveId == moduleObjectiveId
                            select a;
                }
                var gridResult = (from a in model
                                  select new
                                  {
                                      a.CourseInstanceId,
                                      a.ModuleObjectiveId,
                                      a.CodingProblemId,
                                      CourseInstance = a.CourseInstance.Course.Name,
                                      ModuleObjective = a.ModuleObjective.Description,
                                      CodingProblem = a.CodingProblem.Title,
                                      a.Active,
                                      a.MaxGrade,
                                      a.DueDate
                                  }).ToList();
                GridView1.DataSource = gridResult;
                GridView1.DataBind();
            }
        }
        protected void ButtonCourseInstanceCodingProblem_Click(object sender, EventArgs e)
        {
            if (CourseInstanceCodingProblemValidation())
            {
                var courseInstanceId = Convert.ToInt32(DropDownListCourseInstance.SelectedValue);
                var moduleObjectiveId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
                var codingProblemId = Convert.ToInt32(DropDownListCodingProblem.SelectedValue);

                using (MaterialEntities db = new MaterialEntities())
                {
                    var exist = db.CourseInstanceCodingProblems.Where(x => x.CourseInstanceId == courseInstanceId
                                                                     && x.ModuleObjectiveId == moduleObjectiveId
                                                                     && x.CodingProblemId == codingProblemId).Any();
                    if (!exist)
                    {
                        var model = new CourseInstanceCodingProblem()
                        {
                            CourseInstanceId = courseInstanceId,
                            ModuleObjectiveId = moduleObjectiveId,
                            CodingProblemId = codingProblemId,
                            DueDate = Convert.ToDateTime(TextBoxDueDate.Text),
                            Active = CheckBoxActive.Checked,
                            MaxGrade = Convert.ToInt32(TextBoxMaxGrade.Text.Trim()),
                        };
                        db.CourseInstanceCodingProblems.Add(model);
                        db.SaveChanges();
                        lblMessage.Text = "Save Successfully!";
                        clearAll();
                        BindDropDownCodingProblem();
                    }
                    else
                    {
                        lblMessage.Text = "Sorry! Already Exist!";
                    }
                }

            }
        }
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            this.BindGrid();
        }
        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            this.BindGrid();
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int courseInstanceid = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            int moduleObjectiveId = Convert.ToInt32((row.FindControl("LabelModuleObjectiveId") as Label).Text);
            int codingProblemId = Convert.ToInt32((row.FindControl("LabelCodingProblemId") as Label).Text);
            bool active = (row.FindControl("CheckBoxActive") as CheckBox).Checked;
            int maxGrade = Convert.ToInt32((row.FindControl("TextBoxMaxGrade") as TextBox).Text);
            DateTime dueDate = Convert.ToDateTime((row.FindControl("TextBoxDueDate") as TextBox).Text);

            using (MaterialEntities db = new MaterialEntities())
            {
                CourseInstanceCodingProblem model = db.CourseInstanceCodingProblems.Where(x => x.CourseInstanceId == courseInstanceid && x.ModuleObjectiveId == moduleObjectiveId && x.CodingProblemId == codingProblemId).FirstOrDefault();
                model.DueDate = dueDate;
                model.MaxGrade = maxGrade;
                model.Active = active;
                db.SaveChanges();
            }
            GridView1.EditIndex = -1;
            this.BindGrid();
            lblMessage.Text = "Update Successfully!";
        }
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int courseInstanceid = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            int moduleObjectiveId = Convert.ToInt32((row.FindControl("LabelModuleObjectiveId") as Label).Text);
            int codingProblemId = Convert.ToInt32((row.FindControl("LabelCodingProblemId") as Label).Text);
            //using (MaterialEntities db = new MaterialEntities())
            //{
            //    CourseInstanceCodingProblem model = db.CourseInstanceCodingProblems.Where(x => x.CourseInstanceId == courseInstanceid && x.ModuleObjectiveId == moduleObjectiveId && x.CodingProblemId == codingProblemId).FirstOrDefault();
            //    db.CourseInstanceCodingProblems.Remove(model);
            //    db.SaveChanges();
            //}
            //GridView1.EditIndex = -1;
            //this.BindGrid();
            //lblMessage.Text = "Delete Successfully!";
        }
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
            ////check if the row is the header row
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    //add the thead and tbody section programatically
            //    e.Row.TableSection = TableRowSection.TableHeader;
            //}
        }
        protected bool CourseInstanceCodingProblemValidation()
        {
            bool result = true;
            string fieldName = "";
            if (string.IsNullOrWhiteSpace(DropDownListCourseInstance.SelectedValue))
            {
                fieldName += " Course Instance -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListModuleObjective.SelectedValue))
            {
                fieldName += " Module Objective -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListCodingProblem.SelectedValue))
            {
                fieldName += " Coding Problem -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxDueDate.Text))
            {
                fieldName += " Due Date -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxMaxGrade.Text))
            {
                fieldName += " Max Grade -";
                result = false;
            }
            if (!result)
            {
                lblMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
            }
            return result;
        }
        #endregion =====================================================================================

        #region ===============CodingProblem============================================================
        protected void btnAddNewCodingProblem_Click(object sender, EventArgs e)
        {
            if (CodingProblemValidation())
            {
                var moduleObjetiveId = Convert.ToInt32(DropDownListModuleObjective.SelectedValue);
                using (MaterialEntities db = new MaterialEntities())
                {
                    int usesHint = -1;
                    if (CheckboxUsesHint.Checked)
                    {
                        usesHint = 1;
                    }
                    CodingProblem codingProblem = new CodingProblem();
                    codingProblem = new CodingProblem()
                    {
                        Instructions = TextBoxInstruction.Text.Trim(),
                        Script = TextBoxScript.Text.Trim(),
                        Solution = TextBoxSolution.Text.Trim(),
                        ClassName = TextBoxClassName.Text.Trim(),
                        MethodName = TextBoxMethodName.Text.Trim(),
                        ParameterTypes = TextBoxParameterTypes.Text.Trim(),
                        Language = DropDownListLanguage.SelectedItem.Text,
                        TestCaseClass = TextBoxTestCaseClass.Text.Trim(),
                        Before = TextBoxBefore.Text.Trim(),
                        After = TextBoxAfter.Text.Trim(),
                        MaxGrade = Convert.ToInt32(TextBoxMaxGrade.Text.Trim()),
                        Title = TextBoxTitle.Text.Trim(),
                        Type = CheckBoxTypeCode.Checked == true ? "code" : "",
                        Attempts = Convert.ToInt32(TextBoxAttempts.Text.Trim()),
                        Active = CheckBoxActive.Checked,
                        Role = Convert.ToInt32(TextBoxRole.Text.Trim()),
                        UsesHint = usesHint
                    };
                    db.CodingProblems.Add(codingProblem);
                    db.SaveChanges();

                    BindDropDownCodingProblem();
                    DropDownListCodingProblem.SelectedValue = Convert.ToString(codingProblem.Id);
                }

                lblMessage.Text = "Save Successfully";
                btnAddNewCodingProblem.Attributes["disabled"] = "disabled";
                btnAddNewCodingProblem.Style["pointer-events"] = "none";
            }

        }
        protected void btnUpdateCodingProblem_Click(object sender, EventArgs e)
        {
            if (DropDownListCodingProblem.SelectedValue != "")
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    //------------------------Coding Problem Table-------------------------------------
                    int usesHint = -1;
                    if (CheckboxUsesHint.Checked)
                    {
                        usesHint = 1;
                    }
                    CodingProblem codingproblem = db.CodingProblems.Find(Convert.ToInt32(DropDownListCodingProblem.SelectedValue));

                    codingproblem.Instructions = TextBoxInstruction.Text.Trim();
                    codingproblem.Script = TextBoxScript.Text.Trim();
                    codingproblem.Solution = TextBoxSolution.Text.Trim();
                    codingproblem.ClassName = TextBoxClassName.Text.Trim();
                    codingproblem.MethodName = TextBoxMethodName.Text.Trim();
                    codingproblem.ParameterTypes = TextBoxParameterTypes.Text.Trim();
                    codingproblem.Language = DropDownListLanguage.SelectedItem.Text;
                    codingproblem.TestCaseClass = TextBoxTestCaseClass.Text.Trim();
                    codingproblem.Before = TextBoxBefore.Text.Trim();
                    codingproblem.After = TextBoxAfter.Text.Trim();
                    codingproblem.MaxGrade = Convert.ToInt32(TextBoxMaxGrade.Text.Trim());
                    codingproblem.Title = TextBoxTitle.Text.Trim();
                    codingproblem.Type = CheckBoxTypeCode.Checked == true ? "code" : "";
                    codingproblem.Attempts = Convert.ToInt32(TextBoxAttempts.Text.Trim());
                    codingproblem.Active = CheckBoxActive.Checked;
                    codingproblem.Role = Convert.ToInt32(TextBoxRole.Text.Trim());
                    codingproblem.UsesHint = usesHint;

                    db.SaveChanges();
                    BindDropDownCodingProblem();
                    DropDownListCodingProblem.SelectedValue = Convert.ToString(codingproblem.Id);
                }
                lblMessage.Text = "Update Successfully";
            }
            else
            {
                lblMessage.Text = "Please add an Assessment first";
            }
        }
        protected bool CodingProblemValidation()
        {
            bool result = true;
            string fieldName = "Coding Problem:";
            //if (string.IsNullOrWhiteSpace(TextBoxInstruction.Text))
            //{
            //    fieldName += " Instruction -";
            //    result = false;
            //}
            //if (string.IsNullOrWhiteSpace(TextBoxScript.Text))
            //{
            //    fieldName += " Script -";
            //    result = false;
            //}
            //if (string.IsNullOrWhiteSpace(TextBoxSolution.Text))
            //{
            //    fieldName += " Solution -";
            //    result = false;
            //}
            if (string.IsNullOrWhiteSpace(TextBoxClassName.Text))
            {
                fieldName += " Class Name -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxMethodName.Text))
            {
                fieldName += " Method Name -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxParameterTypes.Text))
            {
                fieldName += " Parameter Types -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListLanguage.SelectedValue))
            {
                fieldName += " Language -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxTestCaseClass.Text))
            {
                fieldName += " Test Case Class -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxBefore.Text))
            {
                fieldName += " Before -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxAfter.Text))
            {
                fieldName += " After -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxMaxGrade.Text))
            {
                fieldName += " Max Grade -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxTitle.Text))
            {
                fieldName += " Title -";
                result = false;
            }
            //if (string.IsNullOrWhiteSpace(TextBoxType.Text))
            //{
            //    fieldName += " Type -";
            //    result = false;
            //}
            if (string.IsNullOrWhiteSpace(TextBoxAttempts.Text))
            {
                fieldName += " Attempts -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxRole.Text))
            {
                fieldName += " Role -";
                result = false;
            }
            //if (string.IsNullOrWhiteSpace(DropDownListModuleObjective.SelectedValue))
            //{
            //    fieldName += " Module Objective -";
            //    result = false;
            //}
            if (!result)
            {
                lblMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
            }
            return result;
        }

        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            string result;
            if (FileUpload1.HasFile)
            {
                try
                {
                    var appPath = HttpRuntime.AppDomainAppPath + @"Temp\\";
                    FileUpload1.SaveAs(appPath + FileUpload1.FileName);
                    var answer = File.ReadAllText(appPath + FileUpload1.FileName);
                    var byteArr = File.ReadAllBytes(appPath + FileUpload1.FileName);
                    var solutionBase64 = Convert.ToBase64String(byteArr, 0, byteArr.Length,Base64FormattingOptions.None);
                    File.Delete(appPath + FileUpload1.FileName);
                    TextBoxSolution.Text = solutionBase64;
                    result = "solution file was uploaded";
                }
                catch (Exception ex)
                {
                    result = "<br/> Error <br/>"+ ("Unable to save file <br/> {0}", ex.Message);
                }
            }
            else
            {
                result = "no file to upload";
            }
            lblmessageFile.Text = result;
        }



        #endregion =====================================================================================

        #region ===============Test=====================================================================
        protected void AddNewTest_Click(object sender, EventArgs e)
        {
            if (TestValidation())
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    Test test = new Test()
                    {
                        ParameterValues = TextBoxParameterValues.Text,
                        ExpectedOutput = TextBoxExpectedOutput.Text,
                        OutputException = CheckBoxOutPutException.Checked,
                        Autogenerated = CheckBoxAutoGenerated.Checked,
                        CodingProblemId = Convert.ToInt32(DropDownListCodingProblem.SelectedValue)
                    };
                    db.Tests.Add(test);
                    db.SaveChanges();
                    lblMessage.Text = "Save Successfully";
                }
                this.crearText();
                this.BindGridTest();
            }
        }
        protected void OnRowEditingTest(object sender, GridViewEditEventArgs e)
        {
            GridViewTest.EditIndex = e.NewEditIndex;
            this.BindGridTest();
        }
        protected void OnRowCancelingEditTest(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewTest.EditIndex = -1;
            this.BindGridTest();
        }
        protected void OnRowUpdatingTest(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridViewTest.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridViewTest.DataKeys[e.RowIndex].Values[0]);
            string parametervalues = (row.FindControl("TextBox1") as TextBox).Text;
            string expectedOutput = (row.FindControl("TextBox2") as TextBox).Text;
            bool outputException = (row.FindControl("CheckBox1") as CheckBox).Checked;
            bool autogenerated = (row.FindControl("CheckBox2") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                Test test = db.Tests.Find(id);
                test.ParameterValues = parametervalues;
                test.ExpectedOutput = expectedOutput;
                test.OutputException = outputException;
                test.Autogenerated = autogenerated;

                db.SaveChanges();
                lblMessage.Text = "Update Successfully!";
            }
            GridViewTest.EditIndex = -1;
            this.BindGridTest();
        }
        protected void OnRowDeletingTest(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = GridViewTest.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridViewTest.DataKeys[e.RowIndex].Values[0]);
            //using (MaterialEntities db = new MaterialEntities())
            //{
            //   Test test = db.Tests.Find(id);
            //    db.Tests.Remove(test);
            //    db.SaveChanges();
            //}
            //GridViewTest.EditIndex = -1;
            //this.BindGridTest();
            //lblMessage.Text = "Delete Successfully!";
        }
        protected void OnRowDataBoundTest(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridViewTest.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
            ////check if the row is the header row
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    //add the thead and tbody section programatically
            //    e.Row.TableSection = TableRowSection.TableHeader;
            //}
        }
        private void BindGridTest()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var codingProblemId = Convert.ToInt32(DropDownListCodingProblem.SelectedValue);
                var model = db.Tests.Where(x => x.CodingProblemId == codingProblemId);
                var model2 = (from a in model
                              select new { a.Id, a.ParameterValues, a.ExpectedOutput, OutputException = a.OutputException, Autogenerated = a.Autogenerated });
                GridViewTest.DataSource = model.OrderBy(x => x.Id).ToList();
                GridViewTest.DataBind();
            }
        }
        protected bool TestValidation()
        {
            bool result = true;
            string fieldName = "Test:";
            if (string.IsNullOrWhiteSpace(TextBoxParameterValues.Text))
            {
                fieldName += " Parameter Values -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(TextBoxExpectedOutput.Text))
            {
                fieldName += " Expected Output -";
                result = false;
            }
            if (string.IsNullOrWhiteSpace(DropDownListCodingProblem.SelectedValue))
            {
                fieldName += " Coding Problem -";
                result = false;
            }
            if (!result)
            {
                lblMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
            }
            return result;
        }

        #endregion =====================================================================================

        protected void DropDownListCourseFilter1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var i = DropDownListCourseFilter1.SelectedValue;
            DropDownListCourseInstance.Items.Clear();
            if (i!="") {
                
                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseId = Convert.ToInt32(i);
                    var courseIns = db.CourseInstances.Where(ci => ci.Course.Id == courseId && ci.Active);
                 
                    var list = courseIns.Select(x => new {Name = x.Quarter.StartDate+" TO "+ x.Quarter.EndDate, Id = x.Id}).OrderBy(x => x.Id).ToList();
                    //---------------Coruse Instance Droup Down list----------------
                    DropDownListQuarter.DataSource = list;
                    DropDownListQuarter.DataTextField = "Name";
                    DropDownListQuarter.DataValueField = "Id";
                    DropDownListQuarter.DataBind();
                    //--------------------------------------------------------------------------------
                    if (courseIns.Count() == 1)
                    {
                        BindDropDownListsCourseInstance(courseIns.FirstOrDefault().Id);
                        //----------------------------------------------------------
                        BindGrid();
                        BindDropDownModuleObjective();
                    }
                    else {
                        DropDownListQuarter.Items.Insert(0, new ListItem("--Select One--", ""));
                    }
                }
            }
        }

        protected void DropDownListQuarter_SelectedIndexChanged(object sender, EventArgs e)
        {
            var i = DropDownListQuarter.SelectedValue;
            if (i != "")
            {
                int courseInstanceId = Convert.ToInt32(i);
                BindDropDownListsCourseInstance(courseInstanceId);
                //------------------------------------------------
                BindGrid();
                BindDropDownModuleObjective();
            }
        }
        protected void BindDropDownListsCourseInstance(int courseInstanceId) {
            using (MaterialEntities db = new MaterialEntities())
            {
                var list = db.CourseInstances.Where(ci => ci.Id == courseInstanceId && ci.Active).Select(x => new { x.Id, x.Course.Name }).OrderBy(x => x.Id).ToList();
                //---------------Coruse Instance Droup Down list----------------
                DropDownListCourseInstance.DataSource = list;
                DropDownListCourseInstance.DataTextField = "Name";
                DropDownListCourseInstance.DataValueField = "Id";
                DropDownListCourseInstance.DataBind();
            }
        }
        protected void DropDownListCourseFilter2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var i = DropDownListCourseFilter2.SelectedValue;
            DropDownListCourseInstanceFilter.Items.Clear();
            if (i != "")
            {

                using (MaterialEntities db = new MaterialEntities())
                {
                    int courseId = Convert.ToInt32(i);
                    var courseIns = db.CourseInstances.Where(ci => ci.Course.Id == courseId && ci.Active);

                    var list = courseIns.Select(x => new { Name = x.Quarter.StartDate + " TO " + x.Quarter.EndDate , Id = x.Id }).OrderBy(x => x.Id).ToList();
                    //---------------Coruse Instance Droup Down list----------------
                    DropDownListQuarterFilter2.DataSource = list;
                    DropDownListQuarterFilter2.DataTextField = "Name";
                    DropDownListQuarterFilter2.DataValueField = "Id";
                    DropDownListQuarterFilter2.DataBind();
                    //--------------------------------------------------------------------------------
                    if (courseIns.Count() == 1)
                    {
                        BindDropDownListsCourseInstanceFilter(courseIns.FirstOrDefault().Id);
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
            var i = DropDownListQuarterFilter2.SelectedValue;
            if (i != "")
            {
                int courseInstanceId = Convert.ToInt32(i);
                BindDropDownListsCourseInstanceFilter(courseInstanceId);
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
        protected void BindDropDownListLanguage() 
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var list = db.Languages.Select(x => new { x.Id, x.Name }).OrderBy(x => x.Id).ToList();
                //---------------Droup Down list----------------
                DropDownListLanguage.DataSource = list;
                DropDownListLanguage.DataTextField = "Name";
                DropDownListLanguage.DataValueField = "Id";
                DropDownListLanguage.DataBind();
                DropDownListLanguage.Items.Insert(0, new ListItem("--Select One--", ""));
            }
        }
    }
    public class DropDownListItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}