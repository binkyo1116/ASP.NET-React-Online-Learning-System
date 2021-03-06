using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
//using EFModel;
using LMSLibrary;
using LMS.Common.ViewModels;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SubmitController : ApiController
    {
        public class QuestionInfo
        {
            public int QuestionId { get; set; }
            public string StudentId { get; set; }
            public string Answer { get; set; }
            public string Location { get; set; } // Material, Quiz
            public string History { get; set; }
        }

        // POST api/Course
        public IHttpActionResult Post([FromBody] QuestionInfo qi)
        {
            //MaterialEntities model = new MaterialEntities();

            //QuizQuestion question = model.QuizQuestions.Where(q => q.Id == qi.QuestionId).FirstOrDefault();
            //Student student = model.Students.Where(s => s.Hash == qi.StudentId).FirstOrDefault();
            //string expectedAnswer = GetExpectedAnswer(question);
            //----------------------------------------------------
            var queryResult = GetExpectedAnswerSQL(qi.QuestionId, qi.StudentId);
            var grade = CalculateQuestionGradeFromQuery(qi.Answer, queryResult);
            //----------------------------------------------------
            //int grade = CalculateQuestionGrade(qi.Answer, expectedAnswer, question);
            //if (grade != 0)
            //{
            //    expectedAnswer = qi.Answer;
            //}

            //StudentQuizQuestion sqq = new StudentQuizQuestion
            //{
            //    StudentId = student.StudentId,
            //    Answer = qi.Answer,
            //    Expected = expectedAnswer,
            //    Date = DateTime.Now,
            //    AnswerShown = false,
            //    Grade = grade,
            //    History = qi.History
            //};
            //question.StudentQuizQuestions.Add(sqq);
            //model.SaveChanges();
            SaveAnswer(qi.QuestionId, qi.Answer, queryResult, grade, qi.History);

            //Activity activity = question.Activity;
            //ActivityGrade activityGrade = model.ActivityGrades.Where(ag => ag.ActivityId == question.Activity.Id && ag.StudentId == student.StudentId).FirstOrDefault();
            //QuizGrader.CalculateGrade(activity, student, out int recalculatedGrade, out int totalGrade, out int totalShown);

            //var quizQuestionData = QuestionController.GetQuizQuestions(qi.StudentId, queryResult.ActivityId);
            //QuizGrader.CalculateQuizGrade(quizQuestionData, out int recalculatedGrade, out int totalGrade, out int totalShown);
            var totalResult = GetTotalResult(qi.QuestionId, qi.StudentId);
            CalculateQuizGradeForSubmit(totalResult, out int recalculatedGrade, out int totalGrade, out int totalShown);

            int hintId = 0;
            string hint = null;
            int hintRating = 0;

            /*
            if (grade == 0)
            {
                if (question.UsesHint.HasValue && question.UsesHint.Value != 0)
                {
                    var quizHint = HintStudent.GetQuestionHint(qi.QuestionId, qi.Answer);

                    if (quizHint != null)
                    {
                        hint = quizHint.Item2;
                        hintId = quizHint.Item1;

                        var quizHintRating = model.QuizHintProblemRatings.SingleOrDefault(qhp => qhp.HintId == hintId && qhp.StudentId == student.StudentId);
                        if (quizHintRating != null)
                        {
                            hintRating = quizHintRating.Rating;
                        }
                    }
                }
            }
            */


            ResultInfo ri = new ResultInfo
            {
                Grade = grade,
                TotalGrade = totalGrade, // (activityGrade.Grade * 100) / activityGrade.MaxGrade,
                TotalShown = totalShown, // (activityGrade.ShownGrade * 100) / activityGrade.MaxGrade,
                //MaxGrade = question.MaxGrade,
                MaxGrade = queryResult.MaxGrade,
                Hint = hint,
                HintId = hintId,
                HintRating = hintRating
            };

            return Ok(ri);
        }
        //private static ExpectedResult GetExpectedAnswer(int questionId, string hash)
        //{
        //    string sqlQuery = $@"Select top 1 Case When qq.Type = 'Checkbox' or qq.Type = 'Radio' or qq.Type = 'Dropdown' Then 
        //                        (SELECT STUFF((SELECT ',' + CAST(OptionId AS varchar) FROM QuizQuestionChoice t1  where t1.QuestionId =t2.QuestionId and t1.Correct = 1 FOR XML PATH('')), 1 ,1, '') AS ValueList
        //                        FROM QuizQuestionChoice t2
        //                        where t2.QuestionId = qq.Id and t2.Correct = 1
        //                        GROUP BY t2.QuestionId)
        //                        Else qq.Answer end as ExpectedAnswer, qq.MaxGrade, qq.CaseSensitive, qq.ActivityId1, 
        //                        (select top 1 StudentId from Student where Hash = '{hash}') As StudentId
        //                        from QuizQuestion qq
        //                        where qq.Id = {questionId}";

        //    var data = SQLHelper.RunSqlQuery(sqlQuery);
        //    var result = new ExpectedResult();
        //    if (data.Count > 0)
        //    {
        //        var d = data.First();
        //        result = new ExpectedResult
        //        {
        //         ExpectedAnswer = d[0].ToString(),
        //         MaxGrade = (int)d[1],
        //         CaseSensitive = (bool)d[2],
        //         ActivityId = (int)d[3],
        //         StudentId = (int)d[4],
        //        };
        //    }
        //    return result;
        //}
        private static void SaveAnswer(int questionId,string answer, ExpectedResult exResult, int Grade, string history)
        {
            string sqlQuery = $@"INSERT INTO [dbo].[StudentQuizQuestion]
           ([StudentId]
           ,[Answer]
           ,[Expected]
           ,[Date]
           ,[AnswerShown]
           ,[History]
           ,[QuestionId]
           ,[Grade])
     VALUES
           ({exResult.StudentId}
           ,'{answer}'
           ,'{exResult.ExpectedAnswer}'
           ,'{DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss")}'
           ,0
           ,'{history}'
           ,{questionId}
           ,{Grade})";

            var data = SQLHelper.RunSqlUpdate(sqlQuery);
        }
        private static string GetExpectedAnswer(VmQuizQuestion question)
        {
            //MaterialEntities model = new MaterialEntities();

            string answer = "";
            if (question.Type == "Checkbox" || question.Type == "Radio" || question.Type == "Dropdown")
            {
                //List<int> correctChoices = model.QuizQuestionChoices.Where(q => q.QuestionId == question.Id && q.Correct).OrderBy(q => q.OptionId).Select(qc => qc.OptionId).ToList();
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
        private static ExpectedResult GetExpectedAnswerSQL(int questionId, string hash)
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
            var result = new ExpectedResult();
            if (data.Count > 0)
            {
                var d = data.First();
                result = new ExpectedResult
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
        private static TotalResult GetTotalResult(int questionId, string hash)
        {
            string sqlQuery = $@"Select top 1 isnull((select Sum(Grade) from StudentQuizQuestion where Id in ( 
                                         select MAX(sqq.Id) from QuizQuestion qq2 
                                          join StudentQuizQuestion sqq on qq2.Id = sqq.QuestionId
                                         join Student st on sqq.StudentId= st.StudentId
                                         where st.Hash= '{hash}'
                                         and qq2.ActivityId1 =  qq.ActivityId1
                                         Group by sqq.QuestionId)), 0) as TotalGrade,
		                                ------------------TotalMaxGrade-----------------
		                                isnull((select SUM(MaxGrade) from QuizQuestion
		                                where ActivityId1 = qq.ActivityId1), 0) as TotalMaxGrade,
		                                -------------------------TotalShown------------------
		                                isnull((select SUM(MaxGrade) from QuizQuestion
		                                where Id in (select QuestionId from StudentQuizQuestion where Id in ( 
                                         select MAX(sqq.Id) from QuizQuestion qq2 
                                          join StudentQuizQuestion sqq on qq2.Id = sqq.QuestionId
                                         join Student st on sqq.StudentId= st.StudentId
                                         where st.Hash= '{hash}'
                                         and qq2.ActivityId1 =  qq.ActivityId1
                                         Group by sqq.QuestionId) and AnswerShown = 1)), 0) as TotalShown
                                      from QuizQuestion qq
                                      where qq.Id = {questionId}";

 
            var data = SQLHelper.RunSqlQuery(sqlQuery);
            var result = new TotalResult();
            if (data.Count > 0)
            {
                var d = data.First();
                result = new TotalResult
                {
                    TotalGrade = (int)d[0],
                    TotalMaxGrade = (int)d[1],
                    TotalShown = (int)d[2]
                };
            }
            return result;
        }
        private static int CalculateQuestionGrade(string answer, string expected, VmQuizQuestion question)
        {
            if (question.CaseSensitive)
            {
                return (answer.Equals(expected)) ? question.MaxGrade : 0;
            }
            else
            {
                return (answer.ToLower().Equals(expected.ToLower())) ? question.MaxGrade : 0;
            }
        }
        private static int CalculateQuestionGradeFromQuery(string answer, ExpectedResult result)
        {
            if (result.CaseSensitive)
            {
                return (answer.Equals(result.ExpectedAnswer)) ? result.MaxGrade : 0;
            }
            else
            {
                return (answer.ToLower().Equals(result.ExpectedAnswer.ToLower())) ? result.MaxGrade : 0;
            }
        }
        public static void CalculateQuizGradeForSubmit(TotalResult quizData, out int quizGrade, out int totalGrade, out int totalShown)
        {
            int maxQuizGrade = quizData.TotalMaxGrade;
            quizGrade = quizData.TotalGrade;
            int answerShownCount = quizData.TotalShown;

            totalGrade = (int)Math.Round((quizGrade * 100.0) / maxQuizGrade);
            totalShown = (int)Math.Round((answerShownCount * 100.0) / maxQuizGrade);
        }
        public class ResultInfo
        {
            public int Grade { get; set; }

            public int TotalGrade { get; set; }

            public int TotalShown { get; set; }
            public int MaxGrade { get; set; }

            public string Hint { get; set; }
            public int HintId { get; set; }
            public int HintRating { get; set; }
        }
        public class ExpectedResult
        {
            public int StudentId { get; set; }
            public string ExpectedAnswer { get; set; }
            public int MaxGrade { get; set; }
            public int ActivityId { get; set; }
            public bool CaseSensitive { get; set; }
        }
        public class TotalResult
        {
            public int TotalGrade { get; set; }
            public int TotalMaxGrade { get; set; }
            public int TotalShown { get; set; }
        }
    }
}
