using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
//using EFModel;
using LMSLibrary;
using LMS.Common.ViewModels;
using RESTModelFunctions.Helper;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class QuestionRatingController : ApiController
    {
        public class QuestionInfo
        {
            public int QuestionId { get; set; }

            public int Rating { get; set; }

            public string StudentId { get; set; }
        }

        // POST api/Course
        public IHttpActionResult Post([FromBody] QuestionInfo qi)
        {
            string error = "";
            if (qi.Rating < -1 || qi.Rating > 1)
            {
                error = "Error when sending rating";
            }

            //MaterialEntities model = new MaterialEntities();

            //QuizQuestion question = model.QuizQuestions.Where(q => q.Id == qi.QuestionId).FirstOrDefault();
            //Student student = model.Students.Where(s => s.Hash == qi.StudentId).FirstOrDefault();

            VmQuizQuestion question = GetQuizQuestion(qi.QuestionId);
            VmStudent student = VmModelHelper.GetStudentInfoByHash(qi.StudentId);

            //QuizQuestionRating quizQuestionRating = question.QuizQuestionRatings.Where(qr => qr.StudentId == student.StudentId).FirstOrDefault();
            VmQuizQuestionRating quizQuestionRating = GetQuizQuestionRating(question.Id,student.StudentId);

            if (quizQuestionRating == null)
            {
                //quizQuestionRating = new QuizQuestionRating()
                //{
                //    StudentId = student.StudentId,
                //    Timestamp = DateTime.Now,
                //    Rating = qi.Rating
                //};

                //question.QuizQuestionRatings.Add(quizQuestionRating);
                InsertQuizQuestionRating(qi.QuestionId, student.StudentId, qi.Rating);
            }
            else
            {
                //quizQuestionRating.Rating = qi.Rating;
                UpdateQuizQuestionRating(quizQuestionRating.Id, qi.Rating);
            }

            //model.SaveChanges();

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
                    MaxGrade = (int)st[4],
                    CaseSensitive = (bool)st[5],
                    PositionX = (int)st[6],
                    PositionY = (int)st[7],
                    Height = (int)st[8],
                    Width = (int)st[9],
                    Type = st[10].ToString(),
                    VideoTimestamp = (int)st[11],
                    VideoSource = (st[12] != DBNull.Value) ? st[12].ToString():String.Empty,
                    EmbedAction = (bool)st[13],
                    Id = (int)st[14],
                    ActivityId1 = (int)st[15],
                    ElementStyleId = (st[16] != DBNull.Value)? (int)st[16]:0,
                    Images = (st[17] != DBNull.Value) ? st[17].ToString():String.Empty,
                    UsesHint = (int)st[18]
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
}
