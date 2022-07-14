using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
//using EFModel;
using LMSLibrary;
using LMS.Common.ViewModels;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CourseAnnouncementController : ApiController
    {
        public class StudentInfo
        {
            public int CourseInstanceId { get; set; }
        }

        // POST api/Course
        public IHttpActionResult Post([FromBody] StudentInfo si)
        {
            //MaterialEntities model = new MaterialEntities();

            //CourseInstance courseInstance = model.CourseInstances.Where(c => c.Id == si.CourseInstanceId).FirstOrDefault();
            //List<Announcement> announcements = courseInstance.Announcements.Where(a => a.Active).OrderByDescending(a => a.Id).ToList();
            List<VmAnnouncement> announcements = GetAnnouncement(si.CourseInstanceId);

            IList<AnnouncementInfo> ri = new List<AnnouncementInfo>();

            foreach (VmAnnouncement a in announcements)
            {
                VmStudent student = GetStudentInfo(a.StudentId);
                    //model.Students.Where(stud => stud.StudentId == a.StudentId).FirstOrDefault();
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
            string sqlQueryStudent = $@"select StudentId, Name,Photo 
                                                from Student where StudentId = {id}";

            var studentData = SQLHelper.RunSqlQuery(sqlQueryStudent);
            VmStudent studentinfo = new VmStudent();
            if (studentData.Count > 0)
            {
                var st = studentData.First();
                studentinfo = new VmStudent
                {
                    StudentId = (int)st[0],
                    Name = st[1].ToString(),
                    Photo = (byte[]) st[2]
                };
            }
            return studentinfo;
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
}


