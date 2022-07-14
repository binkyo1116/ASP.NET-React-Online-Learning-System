using EFModel;
using System;
using System.Linq;

namespace OnlineLearningSystem
{
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MaterialEntities data = new MaterialEntities();
            string studentHash = Request.QueryString["hash"];
            Student admin = data.Students.Where(student => student.Name == "admin").FirstOrDefault();
            Session["Data"] = data;
            Session["Student"] = admin;
        }
    }
}
