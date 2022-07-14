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
    public class LanguagesController : ApiController
    {
        private MaterialEntities db = new MaterialEntities();

        // GET: api/Languages
        public HttpResponseMessage GetLanguages()
        {
            try
            {
                var languages = db.Languages.Select(x => new { Id = x.Id, Title = x.Name }).ToList();

                List<XElement> result = new List<XElement>();
                foreach (var l in languages)
                {
                    XElement language = new XElement("Language",
                        new XElement("Id", l.Title),
                        new XElement("Title", l.Title)
                    );
                    result.Add(language);
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

        // GET: api/Languages/name
        [ResponseType(typeof(Language))]
        public IHttpActionResult GetLanguage(string name)
        {
            try {
                Language language = db.Languages.Where(x => x.Name == name).FirstOrDefault();
                if (language == null)
                {
                    return NotFound();
                }
                XElement lang = new XElement("Language",
                    new XElement("Id", language.Id),
                    new XElement("Title", language.Name),
                    new XElement("CodeStart", language.CodeStart),
                    new XElement("CodeEnd", language.CodeEnd)
                );
                return Ok(lang);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}