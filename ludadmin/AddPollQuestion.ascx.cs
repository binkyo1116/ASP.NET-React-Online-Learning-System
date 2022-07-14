using EFModel;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddPollQuestion : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMyPollGrid();
                BindDropDownPollType();
                btnPollQuestionDelete.Attributes["onclick"] = "return confirm('Do you want to delete?');";
            }
        }
        private void BindMyPollGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                var model = (from a in db.PollQuestions
                             select new { a.Id, a.Title, PollType = a.PollQuestionType.TypeTitle, a.EnlistedDate });
                GridViewMyPoll.DataSource = model.OrderBy(x => x.Id).ToList();
                GridViewMyPoll.DataBind();
            }
        }
        private void BindDropDownPollType()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                //---------------Coruse Droup Down list----------------
                DropDownListPollType.DataSource = db.PollQuestionTypes.OrderBy(x => x.PollTypeId).ToList();
                DropDownListPollType.DataTextField = "TypeTitle";
                DropDownListPollType.DataValueField = "PollTypeId";
                DropDownListPollType.DataBind();
                DropDownListPollType.Items.Insert(0, new ListItem("--Select Type--", "0"));
            }
        }
        private enum alphabetList { A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z }
        protected void AddNewPollQuestion_Click(object sender, EventArgs e)
        {
            try
            {
                if (PollQuestionValidation())
                {
                    if (CheckBox2.Checked && DropDownListGroupList.SelectedValue == "0")
                    {
                        lblMessage.Text = "Please Select a Group";
                        lblErrorMessage.Text = "";
                    }
                    else
                    {
                        using (MaterialEntities db = new MaterialEntities())
                        {
                            int pollType = Convert.ToInt32(DropDownListPollType.SelectedValue);
                            PollQuestion pollQuestion = new PollQuestion()
                            {
                                Title = TextBoxQuestionTitle.Text.Trim(),
                                PollTypeId = pollType,
                                EnlistedDate = DateTime.Now,
                                EnlistedBy = 1
                            };
                            db.PollQuestions.Add(pollQuestion);
                            db.SaveChanges();
                            //-----------------------Add Group-------------------------------
                            if (CheckBox2.Checked && DropDownListGroupList.SelectedValue != "0")
                            {
                                PollGroupPollQuestion model = new PollGroupPollQuestion()
                                {
                                    PollGroupId = Convert.ToInt32(DropDownListGroupList.SelectedValue),
                                    PollQuestionId = pollQuestion.Id,
                                    Active = true
                                };
                                db.PollGroupPollQuestions.Add(model);
                                db.SaveChanges();
                            }

                            LabelPollQuestionTitle.Text = pollQuestion.Title;
                            LabelPollQuestionId.Text = Convert.ToString(pollQuestion.Id);
                        }

                        BindMyPollGrid();
                        lblMessage.Text = "Save Successfully!";
                        lblErrorMessage.Text = "";
                        AddNewPollQuestion.Attributes["disabled"] = "disabled";
                        AddNewPollQuestion.Style["pointer-events"] = "none";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void ClearText()
        {
            TextBoxPollOption.Text = "";
        }

        private void SavePollOptions(int pollQId)
        {

        }
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGrid();
        }
        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            string title = (row.FindControl("TextBox1") as TextBox).Text;
            bool answer = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                PollQuestionOption pollOption = db.PollQuestionOptions.Where(x => x.PollOptionId == id).FirstOrDefault();
                pollOption.Title = title;
                pollOption.CorrectAnswer = answer;
                db.SaveChanges();
            }
            lblMessage.Text = "Update Successfully";
            lblErrorMessage.Text = "";
            GridView1.EditIndex = -1;
            BindGrid();
        }
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                using (MaterialEntities db = new MaterialEntities())
                {
                    PollQuestionOption pollOption = db.PollQuestionOptions.Where(x => x.PollOptionId == id).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.PollQuestionOptions.Remove(pollOption);
                    db.SaveChanges();
                }
                lblMessage.Text = "Deleted Successfully";
                lblErrorMessage.Text = "";
                BindGrid();
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    lblErrorMessage.Text = ex.Message;
                }
                else
                {
                    lblErrorMessage.Text = ex.InnerException.InnerException.Message;
                }
                lblMessage.Text = "";
            }
        
        }
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }
        private void BindGrid()
        {
            using (MaterialEntities db = new MaterialEntities())
            {
                int pollQuesId = Convert.ToInt32(LabelPollQuestionId.Text);
                IQueryable<PollQuestionOption> model = db.PollQuestionOptions.Where(x => x.PollQuestionId == pollQuesId);
                GridView1.DataSource = model.OrderBy(x => x.Identity).ToList();
                GridView1.DataBind();
            }
        }

        protected void btnPollQuestionUpdate_Click(object sender, EventArgs e)
        {
            int pollQid = LabelPollQuestionId.Text == "" ? 0 : Convert.ToInt32(LabelPollQuestionId.Text);
            if (pollQid > 0)
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    //-------------Poll Question table---------------------
                    PollQuestion pollQuestion = db.PollQuestions.Where(x => x.Id == pollQid).FirstOrDefault();
                    pollQuestion.Title = TextBoxQuestionTitle.Text;
                    db.SaveChanges();
                    LabelPollQuestionTitle.Text = pollQuestion.Title;
                }
                lblMessage.Text = "Update Successfully";
                lblErrorMessage.Text = "";
                BindMyPollGrid();
            }
            else
            {
                lblErrorMessage.Text = "Please add an Question first";
                lblMessage.Text = "";
            }
        }

        protected void btnPollQuestionDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int pollQid = LabelPollQuestionId.Text == "" ? 0 : Convert.ToInt32(LabelPollQuestionId.Text);
                if (pollQid != 0)
                {
                    using (MaterialEntities db = new MaterialEntities())
                    {
                        IQueryable<PollQuestionOption> pollOptions = db.PollQuestionOptions.Where(x => x.PollQuestionId == pollQid);
                        //TODO: Make sure this is correct
                        if (pollOptions.Any()) {
                            db.PollQuestionOptions.RemoveRange(pollOptions);
                        }
                        //-------------Poll Question table---------------------
                        PollQuestion pollQuestion = db.PollQuestions.Where(x => x.Id == pollQid).FirstOrDefault();
                        //TODO: Make sure this is correct
                        db.PollQuestions.Remove(pollQuestion);
                        db.SaveChanges();
                    }
                    clearAll();
                    lblMessage.Text = "Deleted Successfully";
                    lblErrorMessage.Text = "";
                    BindMyPollGrid();
                }
                else
                {
                    lblErrorMessage.Text = "Please add an poll first";
                    lblMessage.Text = "";
                }
            }
            catch (Exception ex)
            {

                if (ex.InnerException == null)
                {
                    lblErrorMessage.Text = ex.Message;
                }
                else
                {
                    lblErrorMessage.Text = ex.InnerException.InnerException.Message;
                }
                lblMessage.Text = "";
            }
            
        }
        private void clearAll()
        {
            TextBoxQuestionTitle.Text = "";
            TextBoxPollOption.Text = "";

            LabelPollQuestionId.Text = "";
            LabelPollQuestionTitle.Text = "";

            PanelPollOption.Visible = false;

            CheckBox2.Checked = false;
            PanelAddPollGroup.Visible = false;
            DropDownListGroupList.Items.Clear();

            AddNewPollQuestion.Attributes.Remove("disabled");
            AddNewPollQuestion.Style["pointer-events"] = "visible";

            lblMessage.Text = "";
            lblErrorMessage.Text = "";
            lblListMessage.Text = "";
            GridView1.DataSource = null;
            GridView1.DataBind();
            BindDropDownPollType();
        }

        protected void ButtonAddPollOption_Click(object sender, EventArgs e)
        {
            int questionId = LabelPollQuestionId.Text == "" ? 0 : Convert.ToInt32(LabelPollQuestionId.Text);
            if (questionId > 0 && TextBoxPollOption.Text.Trim() != "")
            {
                int index = GridView1.Rows.Count;
                using (MaterialEntities db = new MaterialEntities())
                {
                    PollQuestionOption pollOption = new PollQuestionOption()
                    {
                        PollQuestionId = questionId,
                        Title = TextBoxPollOption.Text.Trim(),
                        CorrectAnswer = CheckBoxCorrectAns.Checked,
                        Identity = Enum.GetName(typeof(alphabetList), index)
                    };
                    db.PollQuestionOptions.Add(pollOption);
                    db.SaveChanges();
                }
                lblMessage.Text = "Save Successfully";
                lblErrorMessage.Text = "";
                ClearText();
                BindGrid();
            }
            else
            {
                lblErrorMessage.Text = "Please Add a Question first";
                lblMessage.Text = "";
            }
        }

        protected void ButtonRefressTypeList_Click(object sender, EventArgs e)
        {
            clearAll();
        }
        //====================Poll List page ========================================
        protected void GridViewMyPoll_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            //try
            //{
            //    int pollQid = Convert.ToInt32(GridViewMyPoll.DataKeys[e.RowIndex].Values[0]);
            //    using (MaterialEntities db = new MaterialEntities())
            //    {
            //        IQueryable<PollQuestionOption> pollOptions = db.PollQuestionOptions.Where(x => x.PollQuestionId == pollQid);

            //        //TODO: Make sure this is correct
            //        if (pollOptions.Any()) {
            //        db.PollQuestionOptions.RemoveRange(pollOptions);
            //        }
            //        //-------------Poll Question table---------------------
            //        PollQuestion pollQuestion = db.PollQuestions.Where(x => x.Id == pollQid).FirstOrDefault();
            //        //TODO: Make sure this is correct
            //        db.PollQuestions.Remove(pollQuestion);
            //        db.SaveChanges();
            //    }
            //    BindMyPollGrid();
            //}
            //catch (Exception ex)
            //{
            //    if (ex.InnerException == null)
            //    {
            //        lblErrorMessage.Text = ex.Message;
            //    }
            //    else
            //    {
            //        lblErrorMessage.Text = ex.InnerException.InnerException.Message;
            //    }
            //    lblMessage.Text = "";
            //}
        }

        protected void GridViewMyPoll_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "PollEdit" || e.CommandName == "PollDelete")
            {
                clearAll();
                using (MaterialEntities db = new MaterialEntities())
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    int pollQid = Convert.ToInt32(GridViewMyPoll.DataKeys[rowIndex].Values[0]);
                    //----------------------PollQuestion------------------------
                    PollQuestion pollQuestion = db.PollQuestions.Where(x => x.Id == pollQid).FirstOrDefault();

                    BindDropDownPollType();
                    DropDownListPollType.SelectedValue = Convert.ToString(pollQuestion.PollTypeId);
                    TextBoxQuestionTitle.Text = pollQuestion.Title;
                    LabelPollQuestionId.Text = Convert.ToString(pollQuestion.Id);
                    LabelPollQuestionTitle.Text = pollQuestion.Title;

                    AddNewPollQuestion.Attributes["disabled"] = "disabled";
                    AddNewPollQuestion.Style["pointer-events"] = "none";
                    lblMessage.Text = "";
                    lblErrorMessage.Text = "";
                    lblListMessage.Text = "";

                    //----------------------Poll Option------------------------
                    IQueryable<PollQuestionOption> pollOptions = db.PollQuestionOptions.Where(x => x.PollQuestionId == pollQid);
                    if (pollOptions.Any())
                    {
                        GridView1.DataSource = pollOptions.ToList();
                        GridView1.DataBind();
                        TextBoxPollOption.Text = "";
                    }
                }
                Page.ClientScript.RegisterStartupScript(GetType(), "EditPanalShow", "sessionStorage.setItem('btnIdPoll', 'v-pills-create-poll');", true);

                //Response.Redirect("AddStudentCourse.aspx");
            }
        }

        protected void GridViewMyPoll_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridViewMyPoll.EditIndex)
            //{
            //    (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            //}
        }

        protected void DropDownListPollType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectdetId = Convert.ToInt32(DropDownListPollType.SelectedValue);
            using (MaterialEntities db = new MaterialEntities())
            {
                bool type = db.PollQuestionTypes.Where(x => x.PollTypeId == selectdetId).FirstOrDefault().PollOption;
                if (type)
                {
                    PanelPollOption.Visible = true;
                }
                else
                {
                    PanelPollOption.Visible = false;
                }
            }
        }

        protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                PanelAddPollGroup.Visible = true;

                using (MaterialEntities db = new MaterialEntities())
                {
                    //---------------Droup Down list Group----------------
                    DropDownListGroupList.DataSource = db.PollGroups.Where(x => x.Active).OrderBy(x => x.Id).ToList();
                    DropDownListGroupList.DataTextField = "Title";
                    DropDownListGroupList.DataValueField = "Id";
                    DropDownListGroupList.DataBind();
                    DropDownListGroupList.Items.Insert(0, new ListItem("--Select Group--", "0"));
                }
            }
            else
            {
                PanelAddPollGroup.Visible = false;
                DropDownListGroupList.Items.Clear();
            }
        }
        #region--------------------Validation---------------------------
        protected bool PollQuestionValidation()
        {
            bool result = true;
            string fieldName = "";

            if (DropDownListPollType.SelectedValue == "0")
            {
                fieldName += " Poll Type -";
                result = false;
            }

            if (string.IsNullOrWhiteSpace(TextBoxQuestionTitle.Text))
            {
                fieldName += " Question Title -";
                result = false;
            }
            if (!result)
            {
                lblMessage.Text = "Sorry! Operation has been failed. Required Fields: " + fieldName + ".";
                lblErrorMessage.Text = "";
            }
            return result;
        }
        #endregion
    }
}