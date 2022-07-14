using System.Web.Http;
using System.Web.Http.Cors;
using LMSLibrary;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BreadcrumbController : ApiController
    {
        public class IncomingInfo
        {
            public int CourseInstanceId { get; set; }
            public int ModuleId { get; set; }
            public int QuestionSetId { get; set; }
            public int PollGroupId { get; set; }
            public int CodingProblemId { get; set; }

        }

        public IHttpActionResult Post([FromBody] IncomingInfo ici)
        {
            ResultInfo ri = new ResultInfo();

            if (ici.CourseInstanceId != 0)
            {
                var sql = SQLHelper.RunSqlQuery($@"SELECT C.Name
                                                  FROM Course C
                                                  INNER JOIN CourseInstance CI
                                                  ON C.Id = CI.CourseId
                                                  WHERE CI.Id = {ici.CourseInstanceId};");
                
                ri.CourseName = (string)sql[0][0];
            }

            if (ici.ModuleId != 0)
            {
                var sql = SQLHelper.RunSqlQuery($@"SELECT Description 
                                                    FROM Module M 
                                                    WHERE M.Id = {ici.ModuleId};");
                ri.ModuleName = (string)sql[0][0];
            }

            if (ici.PollGroupId != 0)
            {
                var sql = SQLHelper.RunSqlQuery($@"SELECT Title FROM PollGroup PG WHERE PG.Id = {ici.PollGroupId};");
                ri.PollName = (string)sql[0][0];
            }
           
            if (ici.QuestionSetId != 0)
            {
                var sql = SQLHelper.RunSqlQuery($@"SELECT Title FROM Activity A WHERE A.Id = {ici.QuestionSetId};");
                ri.ActivityName = (string)sql[0][0];
            }

            if (ici.CodingProblemId != 0)
            {
                var sql = SQLHelper.RunSqlQuery($@"SELECT CP.Title FROM CodingProblem CP WHERE CP.Id = {ici.CodingProblemId};");
                ri.AssessmentName = (string)sql[0][0];
            }

            return Ok(ri);
        }

        public class ResultInfo
        {
            public string CourseName { get; set; } = null;
            public string ModuleName { get; set; } = null;
            public string ModuleObjectiveName { get; set; } = null;
            public string ActivityName { get; set; } = null;
            public string AssessmentName { get; set; } = null;
            public string PollName { get; set; } = null; 
        }

    }
}