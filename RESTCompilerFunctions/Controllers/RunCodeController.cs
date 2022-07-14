using CompilerFunctions;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RESTCompilerFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RunCodeController : ApiController
    {
        public class Input
        {
            public string Hash { get; set; }
            public int CodingProblemId { get; set; }
            public int CourseInstanceId { get; set; }
            public string Code { get; set; }
            public string Content { get; set; }
            public int CodeStructurePoints { get; set; }
        }
        // POST api/RunCode
        public IHttpActionResult Post([FromBody] Input input)
        {
            
            object result = Compiler.RunCode(input.Hash, input.CodingProblemId, input.CourseInstanceId, input.Code, input.CodeStructurePoints);
            return Ok(result);
        }
    }
}
