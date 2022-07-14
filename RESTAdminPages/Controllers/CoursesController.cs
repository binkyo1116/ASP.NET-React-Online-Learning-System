using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml.Linq;
using EFModel;

namespace RESTAdminPages.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CoursesController : ApiController
    {
        private MaterialEntities db = new MaterialEntities();

        // GET: api/Courses
        public HttpResponseMessage GetCourses(string name="")
        {
            try { 
                IQueryable<Course> filterCourses = db.Courses;
                if (name != "")
                    filterCourses = filterCourses.Where(x => x.Name.Contains(name));

                var courses = filterCourses.Select(x => new { Id = x.Id, Title = x.Name }).ToList();

                List<XElement> result = new List<XElement>();
                foreach (var c in courses)
                {
                    XElement course = new XElement("Course",
                        new XElement("Id", c.Id),
                        new XElement("Title", c.Title)
                    );
                    result.Add(course);
                }
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
                return response;
            }
            catch (Exception e)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
                return response;
            }
        }

    }
}