using LMSLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollResponseController : ControllerBase
    {
        [HttpPost]
        public ActionResult<PollResponseResultInfo> Post([FromBody] PollResponseStudentInfo si)
        {
            if (si.Method == "Get")
            {
                var PQuestions = GetPollQuestionInfo(si.Hash, si.PollGroupId);
                var PAnswers = GetPollAnswers(si.Hash, si.PollGroupId, si.ModuleObjectiveId, si.CourseInstanceId);
                var POptions = GetPollOptions(si.PollGroupId);

                var results = (from a in PQuestions
                               select new PollResponseResultInfo
                               {
                                   PollQuestion = a.PollQuestion,
                                   PollQuestionId = a.PollQuestionId,
                                   isOption = a.isOption,
                                   PollAnswers = PAnswers.Where(x => x.PollQuestionId == a.PollQuestionId).Select(y =>
                                             new PollAnswer { Answer = y.Answer, PollOptionId = y.PollOptionId }).ToList(),
                                   PollOptions = POptions.Where(x => x.PollQuestionId == a.PollQuestionId).Select(x => new PollOption { PollOptionId = x.PollOptionId, Identity = x.Identity, Title = x.Title }).ToList()
                               }).ToList();
                return Ok(results);
            }
            else if (si.Method == "Add")
            {
                var PQuestions = GetPollQuestionInfo(si.Hash, si.PollGroupId);
                var PAnswers = GetPollAnswers(si.Hash, si.PollGroupId, si.ModuleObjectiveId, si.CourseInstanceId);
                var POptions = GetPollOptions(si.PollGroupId);
                foreach (studentResponse i in si.StudentResponses)
                {
                    var PQuestion = PQuestions.Where(x => x.PollQuestionId == i.PollQuestionId).FirstOrDefault();
                    if (PQuestion.isOption)
                    {
                        var pAnswer = PAnswers.Where(x => x.PollQuestionId == i.PollQuestionId);
                        if (pAnswer.Count() == 0)
                        {
                            SaveAnswer(si, PQuestion.PollQuestionId, i.OptionId, null, true);
                        }
                    }
                    else
                    {
                        if (i.TextAnswer.Trim() != "")
                        {
                            SaveAnswer(si, PQuestion.PollQuestionId, null, i.TextAnswer.Trim(), false);
                        }
                    }
                }
                return Ok(new PollResponseResultInfo() { Result = "Your post was saved" });
            }
            else
            {
                return Ok();
            }

        }
        private static List<PollResponseResultInfo> GetPollQuestionInfo(string hash, int pollGroupId)
        {
            string sqlQuery = $@"declare @testStudent bit
                                set @testStudent = (select top 1 Test from Student
                                where Hash = '{hash}'
                                and Test is not null and Test = 1)

                                select pq.Id, pq.Title, pqt.PollOption from PollGroupPollQuestion pgq
                                join PollQuestion pq on pgq.PollQuestionId = pq.Id
                                join PollQuestionType pqt on pq.PollTypeId = pqt.PollTypeId
                                where PollGroupId = {pollGroupId}
                                and Active = 1 or isnull(@testStudent,0)=1";

            var data = SQLHelper.RunSqlQuery(sqlQuery);
            List<PollResponseResultInfo> result = new List<PollResponseResultInfo>();
            if (data.Count > 0)
            {
                foreach (var i in data)
                {
                    var mg = new PollResponseResultInfo
                    {
                        PollQuestionId = (int)i[0],
                        PollQuestion = i[1].ToString(),
                        isOption = (bool)i[2]
                    };
                    result.Add(mg);
                }
            }
            return result;
        }
        private static void SaveAnswer(PollResponseStudentInfo si, int pollQuestionId, int? pollOptionId, string textAns, bool isOption)
        {
            string sqlQuery = $@"declare @studentId int
                        set @studentId = (select StudentId from Student
                        where Hash = '{si.Hash}')

                        INSERT INTO [dbo].[PollParticipantAnswer]
                                   ([StudentId]
                                   ,[PollQuestionId]
                                   ,[PollGroupId]
                                   ,[PollOptionId]
                                   ,[TextAnswer]
                                   ,[EnlistedDate]
                                   ,[ModuleObjectiveId]
                                   ,[CourseInstanceId])
                             VALUES
                                   (@studentId";
            string queryValue = "";
            if (isOption)
            {
                queryValue = $@",{pollQuestionId}
                                   ,{si.PollGroupId}
                                   ,{pollOptionId}
                                   ,NULL
                                   ,'{DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss")}'
                                   ,{si.ModuleObjectiveId}
                                   ,{si.CourseInstanceId})";
            }
            else
            {
                queryValue = $@",{pollQuestionId}
                                   ,{si.PollGroupId}
                                   ,NULL
                                   ,'{textAns}'
                                   ,'{DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss")}'
                                   ,{si.ModuleObjectiveId}
                                   ,{si.CourseInstanceId})";
            }

            string fullQuery = sqlQuery + queryValue;
            var data = SQLHelper.RunSqlUpdate(fullQuery);
        }
        private static List<PollOption> GetPollOptions(int pollGroupId)
        {
            string sqlQuery = $@"select PollOptionId, Title, [Identity],q. PollQuestionId from PollQuestionOption q
                                join PollGroupPollQuestion g on q.PollQuestionId = g.PollQuestionId
                                where g.PollGroupId = {pollGroupId}";

            var data = SQLHelper.RunSqlQuery(sqlQuery);
            List<PollOption> options = new List<PollOption>();
            if (data.Count > 0)
            {
                foreach (var i in data)
                {
                    var d = new PollOption
                    {
                        PollOptionId = (int)i[0],
                        Title = i[1].ToString(),
                        Identity = i[2].ToString(),
                        PollQuestionId = (int?)i[3]
                    };
                    options.Add(d);
                }
            }
            return options;
        }
        private static List<PollAnswer> GetPollAnswers(string hash, int pollGroupId, int ModuleObjId, int CourseInsId)
        {
            string sqlQuery = $@"declare @studentId int
                                set @studentId = (select StudentId from Student
                                where Hash = '{hash}')

                                select PollOptionId, TextAnswer, PollQuestionId from PollParticipantAnswer
                                where StudentId = @studentId
                                and PollGroupId = {pollGroupId}
                                and ModuleObjectiveId = {ModuleObjId}
                                and CourseInstanceId = {CourseInsId} ";

            var data = SQLHelper.RunSqlQuery(sqlQuery);
            List<PollAnswer> answers = new List<PollAnswer>();
            if (data.Count > 0)
            {
                foreach (var i in data)
                {
                    var d = new PollAnswer
                    {
                        PollOptionId = i[0].ToString() == "" ? null : (int?)i[0],
                        Answer = i[1].ToString(),
                        PollQuestionId = (int?)i[2]
                    };
                    answers.Add(d);
                }
            }
            return answers;
        }
    }

    public class PollResponseStudentInfo
    {
        public int CourseInstanceId { get; set; }
        public int ModuleObjectiveId { get; set; }
        public int PollGroupId { get; set; }
        public List<studentResponse> StudentResponses { get; set; }
        public string Hash { get; set; }
        public string Method { get; set; }
    }
    public class PollResponseResultInfo
    {
        public string PollQuestion { get; set; }
        public int PollQuestionId { get; set; }
        public bool isOption { get; set; }
        public string Result { get; set; }
        public List<PollOption> PollOptions { get; set; }
        public List<PollAnswer> PollAnswers { get; set; }
    }
    public class PollOption
    {
        public int PollOptionId { get; set; }
        public string Title { get; set; }
        public string Identity { get; set; }
        public int? PollQuestionId { get; set; }
    }
    public class PollAnswer
    {
        public int? PollOptionId { get; set; }
        public string Answer { get; set; }
        public int? PollQuestionId { get; set; }
    }
    public class studentResponse
    {
        public int PollQuestionId { get; set; }
        public int? OptionId { get; set; }
        public string TextAnswer { get; set; }
    }
}
