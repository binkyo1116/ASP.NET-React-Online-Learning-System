using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
//using EFModel;
using LMSLibrary;
using LMS.Common.ViewModels;
using RESTModelFunctions.Helper;
using LMS.Common.HelperModels;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TicketingSystemController : ApiController
    {
        public class IncomingInfo
        {
            public string StudentHash { get; set; }
            public int CourseInstanceId { get; set; }
            public int? SupportTicketId { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
            public string Photo { get; set; }
            public string Priority { get; set; }
            public string Method { get; set; }
            public bool Opened { get; set; }
            public bool Closed { get; set; }
        }

        // POST api/TicketingSystem
        public IHttpActionResult Post([FromBody] IncomingInfo ici)
        {
            //MaterialEntities model = new MaterialEntities();

            //Student student = model.Students.Where(s => s.Hash == ici.StudentHash).FirstOrDefault();
            VmStudent student = VmModelHelper.GetStudentInfoByHash(ici.StudentHash);

            if (ici.Method == "SaveNewTicket")
            {
                try
                {
                    //SupportTicket st = new SupportTicket
                    //{
                    //    StudentId = student.StudentId,
                    //    TokenNo = GenarateToken(model),
                    //    Title = ici.Title,
                    //    Priority = ici.Priority,
                    //    OpenedDate = DateTime.Now,
                    //    OpenStatus = true,
                    //    CourseInstanceId = ici.CourseInstanceId
                    //};

                    //model.SupportTickets.Add(st);
                    ////-----------Save Messadge--------------------
                    //SupportTicketMessage sm = new SupportTicketMessage
                    //{
                    //    Message = ici.Message,
                    //    StudentId = student.StudentId,
                    //    SupportTicketId = st.Id,
                    //    Role = "Student",
                    //    Active = true
                    //};
                    //if (ici.Photo != "")
                    //{
                    //    sm.Image = Convert.FromBase64String(ici.Photo);
                    //}
                    //model.SupportTicketMessages.Add(sm);
                    //model.SaveChanges();
                    byte[] image = Convert.FromBase64String(ici.Photo);
                    InsertSupportTicket(ici.Title, student.StudentId, ici.Priority, ici.CourseInstanceId, ici.Message, image);
                    return Ok("");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else if (ici.Method == "GetList")
            {
                //IQueryable<SupportTicket> supportTickets = from a in model.SupportTickets.Where(f => f.CourseInstanceId == ici.CourseInstanceId && f.StudentId == student.StudentId) select a;
                List<VmSupportTicket> supportTickets = GetVmSupportTicket(ici.CourseInstanceId, student.StudentId);

                var listOfCourse = GetCourseInstance(ici.CourseInstanceId);
                if (ici.Opened)
                {
                    //supportTickets = from a in supportTickets where a.OpenStatus select a;
                    supportTickets = supportTickets.Where(a => a.OpenStatus).ToList();
                }
                else if (ici.Closed)
                {
                    //supportTickets = from a in supportTickets where !a.OpenStatus select a;
                    supportTickets = supportTickets.Where(a => !a.OpenStatus).ToList();
                }
                ResultInfo ri = new ResultInfo
                {
                    //CourseName = model.CourseInstances.Where(ci => ci.Id == ici.CourseInstanceId).FirstOrDefault().Course.Name
                    CourseName = listOfCourse[0].Name
                };

                IList<SupportTicketList> supportTicketList = new List<SupportTicketList>();
                foreach (VmSupportTicket f in supportTickets)
                {
                    SupportTicketList st = new SupportTicketList
                    {
                        Id = f.Id,
                        Title = f.Title,
                        //UnreadMessage = f.SupportTicketMessages.Where(x => x.StudentId != student.StudentId && !x.ViewStatus).Count(),
                        UnreadMessage = GetUnreadMessageCount(f.Id, student.StudentId),
                        Status = f.OpenStatus == true ? "Open" : "Closed",
                        TokenNo = f.TokenNo.ToString().Length < 5 ? f.TokenNo.ToString().PadLeft(5, '0') : f.TokenNo.ToString()
                    };
                    supportTicketList.Add(st);
                }

                ri.SupportTicketList = (List<SupportTicketList>)supportTicketList;

                return Ok(ri);
            }
            else if (ici.Method == "GetMessage")
            {
                //SupportTicket supportTicket = model.SupportTickets.Find(ici.SupportTicketId);
                //ICollection<SupportTicketMessage> messages = supportTicket.SupportTicketMessages;

                List<VmStudentSupportTicketMessage> supportTickets = GetVmStudentSupportTicketMessage(ici.SupportTicketId ?? 0);
                var listOfCourse = GetCourseInstance(ici.CourseInstanceId);
                SingleSupportTicket st = new SingleSupportTicket
                {
                    //CourseName = model.CourseInstances.Where(ci => ci.Id == ici.CourseInstanceId).FirstOrDefault().Course.Name,
                    CourseName = listOfCourse[0].Name,
                    Priority = supportTickets[0].Priority,
                    Status = supportTickets[0].OpenStatus == true ? "Open" : "Closed",
                    Title = supportTickets[0].Title,
                    TokenNo = supportTickets[0].TokenNo.ToString().Length < 5 ? supportTickets[0].TokenNo.ToString().PadLeft(5, '0') : supportTickets[0].TokenNo.ToString()
                };

                IList<SupportTicketMessageList> supportTicketMessageLists = new List<SupportTicketMessageList>();
                foreach (VmStudentSupportTicketMessage r in supportTickets)
                {
                    if (r.StudentId != student.StudentId && !r.ViewStatus)
                    {
                        //r.ViewStatus = true;
                        //model.SaveChanges();
                        UpdateSupportTicketMessage(r.Id);
                    }

                    //---------------------------
                    SupportTicketMessageList sl = new SupportTicketMessageList
                    {
                        Id = r.Id,
                        Message = r.Message
                    };
                    if (r.Image != null)
                    {
                        byte[] img = r.Image.ToArray();
                        sl.ContentImage = "data:image;base64," + Convert.ToBase64String(img);
                    };
                    sl.PersonName = r.Name;
                    sl.Role = r.Role;
                    if (r.Photo != null)
                    {
                        byte[] img = r.Photo.ToArray();
                        sl.PersonImage = "data:image;base64," + Convert.ToBase64String(img);
                    };
                    supportTicketMessageLists.Add(sl);
                }

                st.SupportTicketMessageList = (List<SupportTicketMessageList>)supportTicketMessageLists;

                return Ok(st);
            }
            else if (ici.Method == "SaveMessage")
            {
                try
                {
                    //-----------Save Messadge--------------------
                    if (ici.SupportTicketId != null)
                    {
                        //SupportTicketMessage sm = new SupportTicketMessage
                        //{
                        //    Message = ici.Message,
                        //    StudentId = student.StudentId,
                        //    SupportTicketId = Convert.ToInt32(ici.SupportTicketId),
                        //    Role = "Student",
                        //    Active = true,
                        //    ViewStatus = false,
                        //};
                        //if (ici.Photo != "")
                        //{
                        //    sm.Image = Convert.FromBase64String(ici.Photo);
                        //}
                        //model.SupportTicketMessages.Add(sm);
                        //model.SaveChanges();
                        byte[] image = Convert.FromBase64String(ici.Photo);
                        InsertSupportTicketMessage(ici.SupportTicketId ?? 0, student.StudentId, ici.Message, image);
                    }
                    return Ok("");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else if (ici.Method == "CloseTicket")
            {
                try
                {
                    //-----------Close Ticket------------
                    if (ici.SupportTicketId != null)
                    {
                        //SupportTicket ticket = model.SupportTickets.Find(ici.SupportTicketId);
                        //ticket.OpenStatus = false;
                        //model.SaveChanges();

                        UpdateSupportTicket(ici.SupportTicketId ?? 0);
                    }
                    return Ok("");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else if (ici.Method == "GetCourses")
            {
                //var courseList = model.Students.Where(s => s.Hash == ici.StudentHash).FirstOrDefault().CourseInstances.Where(c => c.Active).Select(c => new { c.Id, c.Course.Name }).ToList();
                var courseList = GetStudentCourse(student.StudentId);
                return Ok(courseList);
            }
            else
            {
                return Ok("");
            }
        }

        private int GetUnreadMessageCount( int supportTicketId, int studentId)
        {
            string sqlQueryCourseInstance = $@"select COUNT(*) as messageCount from SupportTicketMessage where SupportTicketId = {supportTicketId} and StudentId != {studentId} and ViewStatus = 0";

            var courseInstanceData = SQLHelper.RunSqlQuery(sqlQueryCourseInstance);
            var result = (int)courseInstanceData.First()[0];
            return result;
        }

        private void InsertSupportTicket(string Title, int StudentId, string Priority, int CourseInstanceId, string Message, byte[] image)
        {
            //string sqlQueryQuarter = $@"exec GenerateSupportTicket '{Title}' , {StudentId}, '{Priority}', {CourseInstanceId}, '{Message}', {image}";

            //SQLHelper.RunSqlQuery(sqlQueryQuarter);

            string sqlQuizQuestionRating = $@"GenerateSupportTicket";
            List<Param> paramList = new List<Param>();
            paramList.Add(new Param() { Name = "Title", Value = Title });
            paramList.Add(new Param() { Name = "StudentId", Value = StudentId });
            paramList.Add(new Param() { Name = "Priority", Value = Priority });
            paramList.Add(new Param() { Name = "CourseInstanceId", Value = CourseInstanceId });
            paramList.Add(new Param() { Name = "Message", Value = Message });
            paramList.Add(new Param() { Name = "Image", Value = image });
            SQLHelper.RunSqlUpdateWithParam(sqlQuizQuestionRating, paramList);
        }

        private void InsertSupportTicketMessage(int SupportTicketId, int StudentId, string Message, byte[] image)
        {
            //       string sqlQueryQuarter = $@"INSERT INTO [dbo].[SupportTicketMessage]
            //      ([SupportTicketId]
            //      ,[StudentId]
            //      ,[Message]
            //      ,[Image]
            //      ,[Role]
            //      ,[Active]
            //      ,[ViewStatus])
            //VALUES
            //      ({SupportTicketId},
            //      {StudentId}
            //      ,'{Message}'
            //      ,{image}
            //      ,'Student'
            //      ,1,0)";

            //       SQLHelper.RunSqlQuery(sqlQueryQuarter);
            string sqlQuizQuestionRating = $@"InsertSupportTicketMessage";
            List<Param> paramList = new List<Param>();
            paramList.Add(new Param() { Name = "SupportTicketId", Value = SupportTicketId });
            paramList.Add(new Param() { Name = "StudentId", Value = StudentId });
            paramList.Add(new Param() { Name = "Message", Value = Message });
            paramList.Add(new Param() { Name = "Image", Value = image });
            SQLHelper.RunSqlUpdateWithParam(sqlQuizQuestionRating, paramList);
        }

        private void UpdateSupportTicket(int SupportTicketId)
        {
            string sqlQueryQuarter = $@"update SupportTicket set OpenStatus = 0 where Id = {SupportTicketId}";

            SQLHelper.RunSqlQuery(sqlQueryQuarter);
        }

        private void UpdateSupportTicketMessage(int SupportTicketId)
        {
            string sqlQueryQuarter = $@"update SupportTicketMessage set ViewStatus = 1 where Id = {SupportTicketId}";

            SQLHelper.RunSqlQuery(sqlQueryQuarter);
        }

        private List<FeedbackController.StudentCourse> GetStudentCourse(int studentId)
        {
            string sqlStudentCourse = $@"select ci.Id,c.Name from Course c
                                    inner join CourseInstance ci on ci.CourseId = c.Id
                                    inner join CourseInstanceStudent cis on cis.CourseInstanceId = ci.Id
                                    where ci.Active = 1 and cis.StudentId = {studentId}";


            var studentCourseData = SQLHelper.RunSqlQuery(sqlStudentCourse);
            List<FeedbackController.StudentCourse> studentCourses = new List<FeedbackController.StudentCourse>();

            if (studentCourseData.Count > 0)
            {
                foreach (var item in studentCourseData)
                {
                    FeedbackController.StudentCourse studentCourseInfo = new FeedbackController.StudentCourse
                    {
                        Id = (int)item[0],
                        Name = (string)item[1]
                    };
                    studentCourses.Add(studentCourseInfo);
                }
            }
            return studentCourses;
        }

        private List<FeedbackController.StudentCourse> GetCourseInstance(int instanceId)
        {
            string sqlStudentCourse = $@"select c.Id,ci.Name from CourseInstance c
                                    inner join Course ci on c.CourseId = ci.Id
                                    where c.Id = {instanceId}";


            var studentCourseData = SQLHelper.RunSqlQuery(sqlStudentCourse);
            List<FeedbackController.StudentCourse> studentCourses = new List<FeedbackController.StudentCourse>();

            if (studentCourseData.Count > 0)
            {
                foreach (var item in studentCourseData)
                {
                    FeedbackController.StudentCourse studentCourseInfo = new FeedbackController.StudentCourse
                    {
                        Id = (int)item[0],
                        Name = (string)item[1]
                    };
                    studentCourses.Add(studentCourseInfo);
                }
            }
            return studentCourses;
        }

        private List<VmSupportTicket> GetVmSupportTicket(int instanceId, int studentId)
        {
            string sqlStudentCourse = $@"select * from SupportTicket where CourseInstanceId = {instanceId} and StudentId = {studentId}";


            var studentCourseData = SQLHelper.RunSqlQuery(sqlStudentCourse);
            List<VmSupportTicket> studentCourses = new List<VmSupportTicket>();

            if (studentCourseData.Count > 0)
            {
                foreach (var item in studentCourseData)
                {
                    VmSupportTicket studentCourseInfo = new VmSupportTicket
                    {
                        Id = (int)item[0],
                        TokenNo = (int)item[1],
                        StudentId = (int)item[2],
                        Title = (string)item[3],
                        Priority = (string)item[4],
                        OpenStatus = (bool)item[6],
                        CourseInstanceId = (int)item[7]
                    };
                    studentCourses.Add(studentCourseInfo);
                }
            }
            return studentCourses;
        }

        private List<VmStudentSupportTicketMessage> GetVmStudentSupportTicketMessage(int supportId)
        {
            string sqlStudentCourse = $@"select st.Priority,st.OpenStatus,st.Title,st.TokenNo,stm.Id,stm.Message,
stm.ViewStatus,stm.Image,stm.Role,s.Name,s.Photo,stm.StudentId from SupportTicket as st
inner join SupportTicketMessage  as stm on st.Id = stm.SupportTicketId
inner join Student  as s on s.StudentId = stm.StudentId
where st.Id ={supportId}";


            var studentCourseData = SQLHelper.RunSqlQuery(sqlStudentCourse);
            List<VmStudentSupportTicketMessage> vmStudentSupportTicketMessages = new List<VmStudentSupportTicketMessage>();

            if (studentCourseData.Count > 0)
            {
                foreach (var item in studentCourseData)
                {
                    VmStudentSupportTicketMessage vmStudentSupportTicket = new VmStudentSupportTicketMessage
                    {
                        Priority = (string)item[0],
                        OpenStatus = (bool)item[1],
                        Title = (string)item[2],
                        TokenNo = (int)item[3],
                        Id = (int)item[4],
                        Message = (string)item[5],
                        ViewStatus = (bool)item[6],
                        Image = (byte[])item[7],
                        Role = (string)item[8],
                        Name = (string)item[9],
                        Photo = (byte[])item[10],
                        StudentId = (int)item[11]
                    };
                    vmStudentSupportTicketMessages.Add(vmStudentSupportTicket);
                }
            }
            return vmStudentSupportTicketMessages;
        }
        //private int GenarateToken(MaterialEntities model)
        //{
        //    int token = 0;
        //    //---------------------Genarate School Id------------------
        //    SupportTicket lastEntry = model.SupportTickets.OrderByDescending(x => x.TokenNo).FirstOrDefault();
        //    if (lastEntry != null)
        //    {
        //        token = lastEntry.TokenNo + 1;
        //    }
        //    else
        //    {
        //        token = 1;
        //    }
        //    return token;
        //}

        public class ResultInfo
        {
            public string CourseName { get; set; }
            public List<SupportTicketList> SupportTicketList { get; set; }
        }

        public class SupportTicketList
        {
            public int Id { get; set; }
            public string TokenNo { get; set; }
            public string Title { get; set; }
            public string Status { get; set; }
            public int UnreadMessage { get; set; }

        }
        public class SingleSupportTicket
        {
            public string TokenNo { get; set; }
            public string Title { get; set; }
            public string Status { get; set; }
            public string Priority { get; set; }
            public string CourseName { get; set; }
            public List<SupportTicketMessageList> SupportTicketMessageList { get; set; }
        }

        public class SupportTicketMessageList
        {
            public int Id { get; set; }
            public string Message { get; set; }
            public string ContentImage { get; set; }
            public string PersonName { get; set; }
            public string Role { get; set; }
            public string PersonImage { get; set; }

        }
    }
}


