using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Xml.Linq;
using EFModel;

namespace RESTAdminPages.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CourseInstancesController : ApiController
    {
        private MaterialEntities db = new MaterialEntities();

        // GET: api/CourseInstances
        public HttpResponseMessage GetCourseInstances(int courseId = 0, string quarterName = "", string instanceName = "", bool getQuarter=false)
        {
            try
            {
                IQueryable<CourseInstance> courseIns = db.CourseInstances.Where(ci => ci.Course.Id == courseId && ci.Active);

                if (quarterName != "")
                    courseIns = courseIns.Where(x => (x.Quarter.StartDate + " TO " + x.Quarter.EndDate).Contains(quarterName));
                if (instanceName != "")
                    courseIns = courseIns.Where(x => x.Course.Name.Contains(instanceName));

                List<XElement> result = new List<XElement>();
                if (getQuarter)
                {
                    var courseInstances = courseIns.Select(x => new { Quarter = x.Quarter.StartDate + " TO " + x.Quarter.EndDate, Id = x.Id }).OrderBy(x => x.Id).ToList();
                    foreach (var ci in courseInstances)
                    {
                        XElement courseInstance = new XElement("Quarter",
                            new XElement("Id", ci.Id),
                            new XElement("Title", ci.Quarter)
                        );
                        result.Add(courseInstance);
                    }
                }
                else
                {
                    var courseInstances = courseIns.Select(x => new { Id = x.Id, CourseName = x.Course.Name }).OrderBy(x => x.Id).ToList();
                    foreach (var ci in courseInstances)
                    {
                        XElement courseInstance = new XElement("CourseInstance",
                            new XElement("Id", ci.Id),
                            new XElement("Title", ci.CourseName)
                        );
                        result.Add(courseInstance);
                    }
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