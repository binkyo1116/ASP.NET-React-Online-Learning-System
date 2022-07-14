using LMS.Common.ViewModels;
using LMSLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseAnnouncementController : ControllerBase
    {
        [HttpPost]
        public ActionResult<IList<AnnouncementInfo>> Post([FromBody] CourseAnnouncementStudentInfo si)
        {
            List<VmAnnouncement> announcements = GetAnnouncement(si.CourseInstanceId);
            IList<AnnouncementInfo> ri = new List<AnnouncementInfo>();
            foreach (VmAnnouncement a in announcements)
            {
                VmStudent student = GetStudentInfo(a.StudentId);
                byte[] photo = student.Photo;
                string imgURL = "";

                if (photo != null)
                {
                    byte[] img = photo.ToArray();
                    imgURL = "data:image;base64," + Convert.ToBase64String(img);
                }

                AnnouncementInfo ai = new AnnouncementInfo()
                {
                    Photo = imgURL,
                    Name = student.Name,
                    Title = a.Title,
                    Description = a.Description,
                    PublishedDate = a.PublishedDate.ToString("MMM dd, yyyy")
                };
                ri.Add(ai);
            }
            return Ok(ri);
        }
        private List<VmAnnouncement> GetAnnouncement(int courseInstanceId)
        {
            string sqlQueryAnnouncement = $@"select a.Id,a.Title,a.Description,a.PublishedDate,a.StudentId,a.CourseInstanceId from Announcement a
			                            inner join CourseInstance ci on ci.Id = a.CourseInstanceId
			                            where ci.id = {courseInstanceId} 
			                            and a.Active = 1
			                            order by a.Id desc";

            var announcementData = SQLHelper.RunSqlQuery(sqlQueryAnnouncement);
            List<VmAnnouncement> vmAnnouncements = new List<VmAnnouncement>();

            if (announcementData.Count > 0)
            {
                foreach (var item in announcementData)
                {
                    VmAnnouncement vmAnnouncement = new VmAnnouncement
                    {
                        Id = (int)item[0],
                        Title = (string)item[1],
                        Description = (string)item[2],
                        PublishedDate = (DateTime)item[3],
                        StudentId = (int)item[4],
                        CourseInstanceId = (int)item[5]
                    };
                    vmAnnouncements.Add(vmAnnouncement);
                }
            }
            return vmAnnouncements;
        }
        private static VmStudent GetStudentInfo(int id)
        {
            string sqlQueryStudent = $@"select StudentId, Name,Photo from Student where StudentId = {id}";

            var studentData = SQLHelper.RunSqlQuery(sqlQueryStudent);
            VmStudent studentinfo = new VmStudent();
            if (studentData.Count > 0)
            {
                var st = studentData.First();
                studentinfo = new VmStudent
                {
                    StudentId = (int)st[0],
                    Name = st[1].ToString(),
                    Photo = (byte[])st[2]
                };
            }
            return studentinfo;
        }
    }
    public class CourseAnnouncementStudentInfo
    {
        public int CourseInstanceId { get; set; }
    }
    public class AnnouncementInfo
    {
        public string Photo { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PublishedDate { get; set; }
    }
}
