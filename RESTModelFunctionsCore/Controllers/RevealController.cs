using LMS.Common.ViewModels;
using LMSLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevealController : ControllerBase
    {
        [HttpPost]
        public ActionResult<RevealResultInfo> Post([FromBody] RevealQuestionInfo qi)
        {
            var queryResult = GetExpectedAnswerSQL(qi.QuestionId, qi.StudentId);
            if (IsAnswerNotAlreadySaved(qi.QuestionId, queryResult.StudentId))
            {
                SaveAnswer(qi.QuestionId, queryResult, qi.History);
            }
            var totalResult = GetTotalResult(qi.QuestionId, qi.StudentId);
            CalculateQuizGradeForSubmit(totalResult, out int recalculatedGrade, out int totalGrade, out int totalShown);
            RevealResultInfo ri = new RevealResultInfo
            {
                Answer = queryResult.ExpectedAnswer,
                TotalGrade = totalGrade,
                TotalShown = totalShown,
                MaxGrade = queryResult.MaxGrade
            };

            return Ok(ri);
        }
        private static string GetExpectedAnswer(VmQuizQuestion question)
        {
            string answer = "";
            if (question.Type == "Checkbox" || question.Type == "Radio" || question.Type == "Dropdown")
            {
                List<int> correctChoices = GetCorrectChoices(question.Id);
                answer = string.Join(",", correctChoices);
            }
            else
            {
                answer = question.Answer;
            }

            return answer;
        }
        private static List<int> GetCorrectChoices(int questionId)
        {
            string sqlcorrectChoices = $@"select qc.OptionId from QuizQuestionChoice qc 
                                              where qc.QuestionId = {questionId} and qc.Correct = 1 order by qc.OptionId asc";

            var correctChoicesData = SQLHelper.RunSqlQuery(sqlcorrectChoices);
            Dictionary<string, int> correctChoicesList = new Dictionary<string, int>();
            foreach (var item in correctChoicesData)
            {
                List<object> st = item;
                correctChoicesList.Add("OptionId", (int)st[0]);
            }

            return correctChoicesList.Select(qc => qc.Value).ToList();
        }
        private static RevealExpectedResult GetExpectedAnswerSQL(int questionId, string hash)
        {
            string sqlQuery = $@"Select top 1 Case When qq.Type = 'Checkbox' or qq.Type = 'Radio' or qq.Type = 'Dropdown' Then 
                                (SELECT STUFF((SELECT ',' + CAST(OptionId AS varchar) FROM QuizQuestionChoice t1  where t1.QuestionId =t2.QuestionId and t1.Correct = 1 FOR XML PATH('')), 1 ,1, '') AS ValueList
                                FROM QuizQuestionChoice t2
                                where t2.QuestionId = qq.Id and t2.Correct = 1
                                GROUP BY t2.QuestionId)
                                Else qq.Answer end as ExpectedAnswer, 
								qq.MaxGrade, qq.CaseSensitive, qq.ActivityId1, 
								-----------StudentId---------------------
                                (select top 1 StudentId from Student where Hash = '{hash}') As StudentId
                                from QuizQuestion qq
                                where qq.Id = {questionId}";

            var data = SQLHelper.RunSqlQuery(sqlQuery);
            var result = new RevealExpectedResult();
            if (data.Count > 0)
            {
                var d = data.First();
                result = new RevealExpectedResult
                {
                    ExpectedAnswer = d[0].ToString(),
                    MaxGrade = (int)d[1],
                    CaseSensitive = (bool)d[2],
                    ActivityId = (int)d[3],
                    StudentId = (int)d[4]
                };
            }
            return result;
        }
        private static RevealTotalResult GetTotalResult(int questionId, string hash)
        {
            string sqlQuery = $@"Select top 1 (select Sum(Grade) from StudentQuizQuestion where Id in ( 
                                         select MAX(sqq.Id) from QuizQuestion qq2 
                                          join StudentQuizQuestion sqq on qq2.Id = sqq.QuestionId
                                         join Student st on sqq.StudentId= st.StudentId
                                         where st.Hash= '{hash}'
                                         and qq2.ActivityId1 =  qq.ActivityId1
                                         Group by sqq.QuestionId)) as TotalGrade,
		                                ------------------TotalMaxGrade-----------------
		                                (select SUM(MaxGrade) from QuizQuestion
		                                where ActivityId1 = qq.ActivityId1) as TotalMaxGrade,
		                                -------------------------TotalShown------------------
		                                (select SUM(MaxGrade) from QuizQuestion
		                                where Id in (select QuestionId from StudentQuizQuestion where Id in ( 
                                         select MAX(sqq.Id) from QuizQuestion qq2 
                                          join StudentQuizQuestion sqq on qq2.Id = sqq.QuestionId
                                         join Student st on sqq.StudentId= st.StudentId
                                         where st.Hash= '{hash}'
                                         and qq2.ActivityId1 =  qq.ActivityId1
                                         Group by sqq.QuestionId) and AnswerShown = 1)) as TotalShown
                                      from QuizQuestion qq
                                      where qq.Id = {questionId}";

            var data = SQLHelper.RunSqlQuery(sqlQuery);
            var result = new RevealTotalResult();
            if (data.Count > 0)
            {
                var d = data.First();
                result = new RevealTotalResult
                {
                    TotalGrade = (int)d[0],
                    TotalMaxGrade = (int)d[1],
                    TotalShown = (int)d[2]
                };
            }
            return result;
        }

        private static bool IsAnswerNotAlreadySaved(int questionId, int studentId)
        {
            string checkRecordSQLQuery = $@"SELECT * FROM StudentQuizQuestion Where StudentId = {studentId} AND questionId = {questionId}";
            var checkData = SQLHelper.RunSqlQuery(checkRecordSQLQuery);
            return checkData.Count == 0;
        }
        private static void SaveAnswer(int questionId, RevealExpectedResult exResult, string history)
        {

                string sqlQuery = $@"INSERT INTO [dbo].[StudentQuizQuestion]
               ([StudentId]
               ,[Answer]
               ,[Expected]
               ,[Date]
               ,[AnswerShown]
               ,[History]
               ,[QuestionId])
         VALUES
               ({exResult.StudentId}
               ,'{exResult.ExpectedAnswer}'
               ,'{exResult.ExpectedAnswer}'
               ,'{DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss")}'
               ,1
               ,'{history}'
               ,{questionId})";

                var data = SQLHelper.RunSqlUpdate(sqlQuery);
        }
        public static void CalculateQuizGradeForSubmit(RevealTotalResult quizData, out int quizGrade, out int totalGrade, out int totalShown)
        {
            int maxQuizGrade = quizData.TotalMaxGrade;
            quizGrade = quizData.TotalGrade;
            int answerShownCount = quizData.TotalShown;

            totalGrade = (int)Math.Round((quizGrade * 100.0) / maxQuizGrade);
            totalShown = (int)Math.Round((answerShownCount * 100.0) / maxQuizGrade);
        }
    }

    public class RevealQuestionInfo
    {
        public int QuestionId { get; set; }
        public string StudentId { get; set; }
        public string History { get; set; }
    }
    public class RevealResultInfo
    {
        public string Answer { get; set; }
        public int TotalGrade { get; set; }
        public int TotalShown { get; set; }
        public int MaxGrade { get; set; }
    }
    public class RevealExpectedResult
    {
        public int StudentId { get; set; }
        public string ExpectedAnswer { get; set; }
        public int MaxGrade { get; set; }
        public int ActivityId { get; set; }
        public bool CaseSensitive { get; set; }
    }
    public class RevealTotalResult
    {
        public int TotalGrade { get; set; }
        public int TotalMaxGrade { get; set; }
        public int TotalShown { get; set; }
    }
}
