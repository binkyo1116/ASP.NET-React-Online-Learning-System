using LMS.Common.ViewModels;
using LMSLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTModelFunctionsCore.Helper;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionRatingController : ControllerBase
    {
        [HttpPost]
        public ActionResult<string> Post([FromBody] QuestionRatingQuestionInfo qi)
        {
            string error = "";
            if (qi.Rating < -1 || qi.Rating > 1)
            {
                error = "Error when sending rating";
                return Ok(error);
            }
            VmQuizQuestion question = GetQuizQuestion(qi.QuestionId);
            VmStudent student = VmModelHelper.GetStudentInfoByHash(qi.StudentId);
            VmQuizQuestionRating quizQuestionRating = GetQuizQuestionRating(question.Id, student.StudentId);

            if (quizQuestionRating == null)
            {
                InsertQuizQuestionRating(qi.QuestionId, student.StudentId, qi.Rating);
            }
            else
            {
                UpdateQuizQuestionRating(quizQuestionRating.Id, qi.Rating);
            }
            return Ok(error);
        }
        private static VmQuizQuestion GetQuizQuestion(int questionId)
        {
            string sqlQuizQuestion = $@"select q.Prompt1, q.Prompt2, q.Answer, q.Source, q.MaxGrade, q.CaseSensitive,
                                        q.PositionX, q.PositionY, q.Height, q.Width, q.Type, q.VideoTimestamp, 
                                        q.VideoSource, q.EmbedAction, q.Id, q.ActivityId1, q.ElementStyleId, q.Images, 
                                        q.UsesHint from QuizQuestion q where q.Id = {questionId}";

            var quizQuestionData = SQLHelper.RunSqlQuery(sqlQuizQuestion);
            VmQuizQuestion quizQuestioninfo = null;

            if (quizQuestionData.Count > 0)
            {
                List<object> st = quizQuestionData[0];

                quizQuestioninfo = new VmQuizQuestion
                {
                    Prompt1 = st[0].ToString(),
                    Prompt2 = st[1].ToString(),
                    Answer = (string)st[2],
                    Source = st[3].ToString(),
                    MaxGrade = st[4] != DBNull.Value ? (int)st[4] : 0,
                    CaseSensitive = (bool)st[5],
                    PositionX = st[6] != DBNull.Value ? (int)st[6] : 0,
                    PositionY = st[7] != DBNull.Value ? (int)st[7] : 0,
                    Height = st[8] != DBNull.Value ? (int)st[8] : 0,
                    Width = st[9] != DBNull.Value ? (int)st[9] : 0,
                    Type = st[10].ToString(),
                    VideoTimestamp = (int)st[11],
                    VideoSource = (st[12] != DBNull.Value) ? st[12].ToString() : String.Empty,
                    EmbedAction = (bool)st[13],
                    Id = (int)st[14],
                    ActivityId1 = (int)st[15],
                    ElementStyleId = (st[16] != DBNull.Value) ? (int)st[16] : 0,
                    Images = (st[17] != DBNull.Value) ? st[17].ToString() : String.Empty,
                    UsesHint = st[18] != DBNull.Value ? (int)st[18] : 0,
                };
            }

            return quizQuestioninfo;
        }
        private static VmQuizQuestionRating GetQuizQuestionRating(int questionId, int studentId)
        {
            string sqlQuizQuestionRating = $@"select qr.CourseId, qr.CourseObjectiveId, qr.ModuleId, qr.ModuleObjectiveId, 
                                              qr.ActivityId, qr.QuizId, qr.QuestionId, qr.StudentId, qr.Rating, qr.Timestamp,
                                              qr.Id, qr.QuestionId1 from QuizQuestionRating qr where  qr.QuestionId1 = {questionId} and qr.StudentId = {studentId}";

            var quizQuestionRatingData = SQLHelper.RunSqlQuery(sqlQuizQuestionRating);
            VmQuizQuestionRating quizQuestionRatinginfo = null;

            if (quizQuestionRatingData.Count > 0)
            {
                List<object> st = quizQuestionRatingData[0];

                quizQuestionRatinginfo = new VmQuizQuestionRating
                {
                    CourseId = (int)st[0],
                    CourseObjectiveId = (int)st[1],
                    ModuleId = (int)st[2],
                    ModuleObjectiveId = (int)st[3],
                    ActivityId = (int)st[4],
                    QuizId = (int)st[5],
                    QuestionId = (int)st[6],
                    StudentId = (int)st[7],
                    Rating = (int)st[8],
                    Timestamp = (DateTime)st[9],
                    Id = (int)st[10],
                    QuestionId1 = (int)st[11]
                };
            }

            return quizQuestionRatinginfo;
        }
        private static void InsertQuizQuestionRating(int questionId, int studentId, int rating)
        {
            string sqlQuizQuestionRating = $@"INSERT INTO [dbo].[QuizQuestionRating]
           ([CourseId]
           ,[CourseObjectiveId]
           ,[ModuleId]
           ,[ModuleObjectiveId]
           ,[ActivityId]
           ,[QuizId]
           ,[QuestionId]
           ,[StudentId]
           ,[Rating]
           ,[Timestamp]
           ,[QuestionId1])
     VALUES
           (0,0,0,0,0,0,0,{studentId},{rating} ,GETDATE(),{questionId})";

            SQLHelper.RunSqlUpdate(sqlQuizQuestionRating);
        }
        private static void UpdateQuizQuestionRating(int questionId, int rating)
        {
            string sqlQuizQuestionRating = $@"Update [dbo].[QuizQuestionRating] set [Rating] = {rating} where Id = {questionId}";

            SQLHelper.RunSqlUpdate(sqlQuizQuestionRating);
        }
    }
    public class QuestionRatingQuestionInfo
    {
        public int QuestionId { get; set; }
        public int Rating { get; set; }
        public string StudentId { get; set; }
    }
}
