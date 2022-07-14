using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Xml.Linq;
using EFModel;

namespace RESTAdminPages.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TestTemplateController : ApiController
    {
        private MaterialEntities db = new MaterialEntities();

        // GET api/TestTemplate?languageName=Java
        public HttpResponseMessage Get(String languageName)
        {
            List<XElement> VariableValuesElements = new List<XElement>();
            IQueryable<TestTemplate> testTemplates = db.TestTemplates.Where(tt => tt.Language == languageName);

            XElement myObj;
            foreach (var tt in testTemplates)
            {
                myObj = new XElement("TestTemplateElement",
                    new XElement("description", tt.Description),
                    new XElement("generatedCodeStart", tt.GeneratedCodeStart),
                    new XElement("generatedCodeEnd", tt.GeneratedCodeEnd),
                    new XElement("language", tt.Language),
                    new XElement("template", tt.Template));
                VariableValuesElements.Add(myObj);
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, VariableValuesElements);
            return response;
        }
    }
}
