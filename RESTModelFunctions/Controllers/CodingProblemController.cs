using System.Web.Http;
using System.Web.Http.Cors;
//using CompilerFunctions;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CodingProblem1Controller : ApiController
    {
        public class Input
        {
            public string Hash { get; set; }
            public int CodingProblemId { get; set; }
            public int CourseInstanceId { get; set; }
        }
        // POST api/GetCodingProblem
        //public IHttpActionResult Post([FromBody] Input input)
        //{
        //    object result = Compiler.GetCodingProblem(input.Hash, input.CodingProblemId, input.CourseInstanceId);
        //    return Ok(result);
        //}
    }
}
