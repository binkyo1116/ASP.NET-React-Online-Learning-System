using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
//using EFModel;
using LMSLibrary;
using LMS.Common.ViewModels;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StudentRequestLoginController : ApiController
    {
        public class StudentInfo
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string SchoolName { get; set; }
            public string CourseName { get; set; }
        }

        // POST api/Course
        public IHttpActionResult Post([FromBody] StudentInfo si)
        {
            //MaterialEntities model = new MaterialEntities();
            try
            {
                //string sqlQueryRequestLogin = $@"INSERT INTO RequestLogin (RequestLoginId, Name, Email, SchoolName, CourseName, TimeStamp)
                //                            VALUES ({GenarateId()},{si.Name.Trim()},{si.Email.Trim()},{si.SchoolName.Trim()},{si.CourseName.Trim()},{DateTime.Now});";
                //bool isSucess = SQLHelper.RunSqlUpdate(sqlQueryRequestLogin);

                string sqlQueryRequestLogin = $@"exec InsertRequestLogin '{si.Name.Trim()}' , '{si.Email.Trim()}' , '{si.SchoolName.Trim()}' , '{si.CourseName.Trim()}'";
                bool isSucess = SQLHelper.RunSqlUpdate(sqlQueryRequestLogin);

                //RequestLogin RequestLogin = new RequestLogin()
                //{
                //    RequestLoginId = GenarateId(),
                //    Name = si.Name.Trim(),
                //    Email = si.Email.Trim(),
                //    SchoolName = si.SchoolName.Trim(),
                //    CourseName = si.CourseName.Trim(),
                //    TimeStamp = DateTime.Now
                //};
                //model.RequestLogins.Add(RequestLogin);
                //model.SaveChanges();

                EmailHelper emailHelper = new EmailHelper();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Student Name: " + si.Name.Trim());
                sb.AppendLine("Student Email: " + si.Email.Trim());
                sb.AppendLine("School Name: " + si.SchoolName.Trim());
                sb.AppendLine("Course Name: " + si.CourseName.Trim());
                sb.AppendLine("Timestamp: " + DateTime.Now);

                EmailHelper.SendEmail(
                    new EmailHelper.Message
                    {
                        Subject = "Student Requesting Login",
                        Recipient = "marcelo@letsusedata.com",
                        Body = sb.ToString()
                    }
                 );
            }
            catch (Exception ex)
            {
                return Ok(new ResultInfo { Success = false, Message = ex.Message });
            }

            return Ok(new ResultInfo { Success = true });
        }
        private int GenarateId()
        {
            int id = 0;
            //MaterialEntities data = new MaterialEntities();
            //RequestLogin lastEntry = data.RequestLogins.OrderByDescending(x => x.RequestLoginId).FirstOrDefault();
            VmRequestLogin lastEntry = GetLastRequestLoginInfo();
            if (lastEntry != null)
            {
                id = ++lastEntry.RequestLoginId;
            }
            else
            {
                id = 1;
            }
            return id;
        }
        private VmRequestLogin GetLastRequestLoginInfo()
        {
            string sqlQueryVmRequestLogin = $@"select r.RequestLoginId, r.Name from RequestLogin r order by r.RequestLoginId desc";

            var VmRequestLoginData = SQLHelper.RunSqlQuery(sqlQueryVmRequestLogin);
            VmRequestLogin requestLogInfo = null;

            if (VmRequestLoginData.Count > 0)
            {
                List<object> st = VmRequestLoginData[0];
                requestLogInfo = new VmRequestLogin
                {
                    RequestLoginId = (int)st[0],
                    Name = st[1].ToString()
                };
            }

            return requestLogInfo;
        }
        public class ResultInfo
        {
            public bool Success { get; set; }
            public string Message { get; set; }
        }
    }
}

