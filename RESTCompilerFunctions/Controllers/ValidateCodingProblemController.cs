using CompilerFunctions;
using LMS.Common.ViewModels;
//using EFModel;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RESTCompilerFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ValidateCodingProblemController : ApiController
    {
        public class RunCodeForValidationInput
        {
            public string Instructions { get; set; }
            public string Language { get; set; }
            public string Script { get; set; }
            public string Solution { get; set; }
            public string Before { get; set; }
            public string After { get; set; }
            public string ExpectedOutput { get; set; }
            public string TestCode { get; set; }
            public string ParameterTypes { get; set; }
            public Dictionary<string, string> VarValuePairs { get; set; }
        }
        public IHttpActionResult Post([FromBody] RunCodeForValidationInput input)
        {
            try
            {
                VmCodingProblem codingProblem = new VmCodingProblem()
                {
                    Instructions = input.Instructions,
                    Script = input.Script,
                    Solution = input.Solution,
                    Language = input.Language,
                    Before = input.Before,
                    After = input.After,
                    ExpectedOutput = input.ExpectedOutput,
                    TestCode = input.TestCode,
                    ParameterTypes = input.ParameterTypes
                };
                object result = Compiler.RunCodeForValidation(codingProblem, input.VarValuePairs);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
