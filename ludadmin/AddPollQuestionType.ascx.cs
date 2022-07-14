using EFModel;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineLearningSystem
{
    public partial class AddPollQuestionType : System.Web.UI.UserControl
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
                GridView1.DataSource = db.PollQuestionTypes.OrderBy(x => x.PollTypeId).ToList();
                GridView1.DataBind();
            }
        }

        protected void AddNewPollType_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxTitle.Text))
            {
                using (MaterialEntities db = new MaterialEntities())
                {
                    PollQuestionType Polltype = new PollQuestionType()
                    {
                        TypeTitle = TextBoxTitle.Text,
                        PollOption = CheckBoxOption.Checked,
                    };
                    db.PollQuestionTypes.Add(Polltype);
                    db.SaveChanges();
                }
                ClearTextField();
                BindGrid();
                lblMessage.Text = "Save Successfully!";
                lblErrorMessage.Text = "";
            }
            else
            {
                lblErrorMessage.Text = "Sorry! Operation has been failed. Title is required.";
                lblMessage.Text = "";
            }
        }
        private void ClearTextField()
        {
            TextBoxTitle.Text = "";
        }
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGrid();
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            string title = (row.FindControl("TextBox1") as TextBox).Text;
            bool option = (row.FindControl("CheckBox1") as CheckBox).Checked;

            using (MaterialEntities db = new MaterialEntities())
            {
                PollQuestionType pollType = db.PollQuestionTypes.Where(x => x.PollTypeId == id).FirstOrDefault();
                pollType.TypeTitle = title;
                pollType.PollOption = option;
                db.SaveChanges();
            }
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
                    PollQuestionType pollType = db.PollQuestionTypes.Where(x => x.PollTypeId == id).FirstOrDefault();
                    //TODO: Make sure this is correct
                    db.PollQuestionTypes.Remove(pollType);
                    db.SaveChanges();
                    lblMessage.Text = "Deleted Successfully!";
                    lblErrorMessage.Text = "";
                }
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
                    lblErrorMessage.Text = ex.InnerException.Message;
                }
                lblMessage.Text = "";
            }
        }

        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            {
                (e.Row.FindControl("LinkButtonDelete") as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row with Course ID = " + DataBinder.Eval(e.Row.DataItem, "PollTypeId") + "?');";
            }
        }

    }
}