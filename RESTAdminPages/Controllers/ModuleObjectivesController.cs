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
    public class ModuleObjectivesController : ApiController
    {
        private MaterialEntities db = new MaterialEntities();

        // GET: api/ModuleObjectives
        public HttpResponseMessage GetModuleObjectives(string name="",int courseId = 0)
        {
            try
            {
                IQueryable<ModuleObjective> filterModelObjectives = db.ModuleObjectives.Where(x => x.Active);
                if (name != "")
                    filterModelObjectives = filterModelObjectives.Where(x => x.Description.Contains(name));

                var modelObjectives = (from a in filterModelObjectives.Where(x => x.Active && x.Modules.Where(m => m.Active && m.CourseObjectives.Where(co => co.Active && co.Courses.Where(c => c.Id == courseId).Any()).Any()).Any())
                                       select new { Id = a.Id, Title = a.Description }).OrderBy(y => y.Id).ToList();

                List<XElement> result = new List<XElement>();
                foreach (var mo in modelObjectives)
                {
                    XElement modelObjective = new XElement("ModuleObjective",
                        new XElement("Id", mo.Id),
                        new XElement("Title", mo.Title)
                    );
                    result.Add(modelObjective);
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