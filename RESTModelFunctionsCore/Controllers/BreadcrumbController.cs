using LMSLibrary;
using Microsoft.AspNetCore.Mvc;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreadcrumbController : Controller
    {
        [HttpPost]
        public ActionResult<BreadcrumbResultInfo> Post([FromBody] IncomingInfo ici)
        {
            BreadcrumbResultInfo ri = new BreadcrumbResultInfo();
            if (ici.CourseInstanceId != 0)
            {
                var sql = SQLHelper.RunSqlQuery($@"SELECT C.Name FROM Course C
                                                  INNER JOIN CourseInstance CI
                                                  ON C.Id = CI.CourseId
                                                  WHERE CI.Id = {ici.CourseInstanceId};");

                ri.CourseName = sql[0][0].ToString();
            }

            if (ici.ModuleId != 0)
            {
                var sql = SQLHelper.RunSqlQuery($@"SELECT Description FROM Module M WHERE M.Id = {ici.ModuleId};");
                ri.ModuleName = sql[0][0].ToString();
            }

            if (ici.PollGroupId != 0)
            {
                var sql = SQLHelper.RunSqlQuery($@"SELECT Title FROM PollGroup PG WHERE PG.Id = {ici.PollGroupId};");
                ri.PollName = sql[0][0].ToString();
            }

            if (ici.QuestionSetId != 0)
            {
                var sql = SQLHelper.RunSqlQuery($@"SELECT Title FROM Activity A WHERE A.Id = {ici.QuestionSetId};");
                ri.ActivityName = sql[0][0].ToString();
            }

            if (ici.CodingProblemId != 0)
            {
                var sql = SQLHelper.RunSqlQuery($@"SELECT CP.Title FROM CodingProblem CP WHERE CP.Id = {ici.CodingProblemId};");
                ri.AssessmentName = sql[0][0].ToString();
            }

            return Ok(ri);
        }
    }
    public class BreadcrumbResultInfo
    {
        public string CourseName { get; set; } = null;
        public string ModuleName { get; set; } = null;
        public string ModuleObjectiveName { get; set; } = null;
        public string ActivityName { get; set; } = null;
        public string AssessmentName { get; set; } = null;
        public string PollName { get; set; } = null;
    }
    public class IncomingInfo
    {
        public int CourseInstanceId { get; set; }
        public int ModuleId { get; set; }
        public int QuestionSetId { get; set; }
        public int PollGroupId { get; set; }
        public int CodingProblemId { get; set; }

    }
}
