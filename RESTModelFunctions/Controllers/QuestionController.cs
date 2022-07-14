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
    public class QuestionController : ApiController
    {
        public class QuestionInfo
        {
            public int QuestionId { get; set; }

            public string StudentId { get; set; }
        }

        // POST api/Course
        public IHttpActionResult Post([FromBody] QuestionInfo qi)
        {
            //MaterialEntities model = new MaterialEntities();
            //QuizQuestion question = model.QuizQuestions.Where(q => q.Id == qi.QuestionId).FirstOrDefault();
            //Student student = model.Students.Where(s => s.Hash == qi.StudentId).FirstOrDefault();
            VmQuizQuestion question = GetQuizQuestion(qi.QuestionId);
            VmStudent student = VmModelHelper.GetStudentInfoByHash(qi.StudentId);

            ResultInfo ri = GetResultInfo(question, student);

            return Ok(ri);
        }
        
        public static ResultInfo GetResultInfo(VmQuizQuestion question, VmStudent student)
        {
            //List<AnswerOption> options = model.QuizQuestionChoices.Where(qqc => qqc.QuestionId == question.Id).OrderBy(q => q.OptionId).Select(i => new AnswerOption { Id = i.OptionId, Option = i.Choice }).ToList();
            List<AnswerOption> options = GetAnswerOption(question.Id);
            if (options == null)
            {
                options = new List<AnswerOption>();
            }

            //StudentQuizQuestion studentAnswer = question.StudentQuizQuestions.Where(sqq => sqq.StudentId == student.StudentId).OrderByDescending(studentqq => studentqq.Id).FirstOrDefault();
            //QuizQuestionRating quizQuestionRating = question.QuizQuestionRatings.Where(qqr => qqr.StudentId == student.StudentId).FirstOrDefault();

            VmStudentQuizQuestion studentAnswer = GetStudentQuizQuestion(student.StudentId);
            VmQuizQuestionRating quizQuestionRating = GetQuizQuestionRating(student.StudentId);

            int questionRating = 0;
            if (quizQuestionRating != null)
            {
                questionRating = quizQuestionRating.Rating;
            }

            int grade = (studentAnswer != null) ? studentAnswer.Grade : 0;
            string expectedAnswer = GetExpectedAnswer(question.Id);

            //QuizQuestionElementStyle elementStyle = question.QuizQuestionElementStyle;
            VmQuizQuestionElementStyle elementStyle = GetQuizQuestionElementStyle(question.ElementStyleId);

            ResultInfo ri = new ResultInfo
            {
                Id = question.Id,
                Options = options,
                Prompt1 = question.Prompt1,
                Prompt2 = question.Prompt2,
                QuestionRating = questionRating,
                Grade = grade,
                MaxGrade = question.MaxGrade,
                AnswerShown = studentAnswer != null ? studentAnswer.AnswerShown : false, // Clean up
                Answer = studentAnswer != null ? studentAnswer.Answer : "", // this is bad
                ExpectedAnswer = expectedAnswer,
                Source = question.Source,
                PositionX = question.PositionX,
                PositionY = question.PositionY,
                Height = question.Height,
                Width = question.Width,
                Type = question.Type,
                VideoTimestamp = question.VideoTimestamp,
                VideoSource = question.VideoSource,
                EmbedAction = question.EmbedAction,
                CaseSens = question.CaseSensitive,
                Color = elementStyle == null ? "" : elementStyle.Color,
                FontFamily = elementStyle == null ? "" : elementStyle.FontFamily,
                BackgroundColor = elementStyle == null ? "" : elementStyle.BackgroundColor,
                FontSize = elementStyle == null ? "" : elementStyle.FontSize,
                Border = elementStyle == null ? "" : elementStyle.Border,
                PaddingLeft = elementStyle == null ? "" : elementStyle.PaddingLeft,
                PaddingRight = elementStyle == null ? "" : elementStyle.PaddingRight,
                Images = question.Images,
                UsesHint = question.UsesHint.HasValue && question.UsesHint.Value > 0,
                IsMultipleChoice = (question.Type == "Checkbox" || question.Type == "Radio" || question.Type == "Dropdown")
            };
            return ri;
        }
        private static VmQuizQuestionElementStyle GetQuizQuestionElementStyle(int? elementStyleId)
        {
            string sqlQuizQuestionElementStyle = $@"select qqes.Id, qqes.Color, qqes.FontFamily, qqes.BackgroundColor, qqes.FontSize, 
                                               qqes.Border, qqes.PaddingLeft, qqes.PaddingRight from QuizQuestionElementStyle 
                                               qqes where qqes.Id = {elementStyleId}";

            var quizQuestionElementStyleData = SQLHelper.RunSqlQuery(sqlQuizQuestionElementStyle);
            VmQuizQuestionElementStyle quizQuestionElementStyleinfo = null;

            if (quizQuestionElementStyleData.Count > 0)
            {
                List<object> st = quizQuestionElementStyleData[0];

                quizQuestionElementStyleinfo = new VmQuizQuestionElementStyle
                {
                    Id = (int)st[0],
                    Color = (string)st[1],
                    FontFamily = st[2].ToString(),
                    BackgroundColor = (string)st[3],
                    FontSize = (string)st[4],
                    Border = st[5].ToString(),
                    PaddingLeft = (string)st[6],
                    PaddingRight = (string)st[7]
                };
            }

            return quizQuestionElementStyleinfo;
        }
        private static VmStudentQuizQuestion GetStudentQuizQuestion(int studentId)
        {
            string sqlStudentQuizQuestion = $@"select sq.Id, sq.StudentId, sq.Answer, sq.Date, sq.AnswerShown, 
                                               sq.History, sq.QuestionId, sq.Grade from StudentQuizQuestion sq
                                               where sq.StudentId = {studentId} order by sq.Id desc";

            var studentQuizQuestionData = SQLHelper.RunSqlQuery(sqlStudentQuizQuestion);
            VmStudentQuizQuestion studentQuizQuestioninfo = null;

            if (studentQuizQuestionData.Count > 0)
            {
                List<object> st = studentQuizQuestionData[0];

                studentQuizQuestioninfo = new VmStudentQuizQuestion
                {
                    Id = (int)st[0],
                    StudentId = (int)st[1],
                    Answer = st[2].ToString(),
                    Date = (DateTime)st[3],
                    AnswerShown = (bool)st[4],
                    History = st[5].ToString(),
                    QuestionId = (int)st[6],
                    Grade = (int)st[7]
                };
            }

            return studentQuizQuestioninfo;
        }
        private static VmQuizQuestionRating GetQuizQuestionRating(int studentId)
        {
            string sqlQuizQuestionRating = $@"select qr.CourseId, qr.CourseObjectiveId, qr.ModuleId, qr.ModuleObjectiveId, 
                                              qr.ActivityId, qr.QuizId, qr.QuestionId, qr.StudentId, qr.Rating, qr.Timestamp,
                                              qr.Id, qr.QuestionId1 from QuizQuestionRating qr where qr.StudentId = {studentId}";

            var quizQuestionRatingData = SQLHelper.RunSqlQuery(sqlQuizQuestionRating);
            VmQuizQuestionRating quizQuestionRatinginfo = null;

            if (quizQuestionRatingData.Count > 0)
            {
                List<object> st = quizQuestionRatingData[0];

                quizQuestionRatinginfo = new VmQuizQuestionRating
                {
                    CourseId = (int)st[0],
                    CourseObjectiveId = (int)st[0],
                    ModuleId = (int)st[0],
                    ModuleObjectiveId = (int)st[0],
                    ActivityId = (int)st[0],
                    QuizId = (int)st[0],
                    QuestionId = (int)st[0],
                    StudentId = (int)st[0],
                    Rating = (int)st[0],
                    Timestamp = (DateTime)st[0],
                    Id = (int)st[0],
                    QuestionId1 = (int)st[0]
                };
            }

            return quizQuestionRatinginfo;
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
                    VideoSource = st[12].ToString(),
                    EmbedAction = (bool)st[13],
                    Id = (int)st[14],
                    ActivityId1 = (int)st[15],
                    ElementStyleId = (int)st[16],
                    Images = st[17].ToString(),
                    UsesHint = (int)st[18]
                };
            }

            return quizQuestioninfo;
        }
        private static List<AnswerOption> GetAnswerOption(int questionId)
        {
            string sqlQuizQuestionChoice = $@"select qc.OptionId, qc.QuestionId, qc.Choice, qc.Correct from QuizQuestionChoice qc 
                                                where qc.QuestionId = {questionId} order by qc.OptionId";

            var quizQuestionChoicesData = SQLHelper.RunSqlQuery(sqlQuizQuestionChoice);
            List<QuizQuestionChoice> quizQuestionChoiceList = new List<QuizQuestionChoice>();

            foreach (var item in quizQuestionChoicesData)
            {
                QuizQuestionChoice quizQuestionChoice = null;
                List<object> st = item;
                quizQuestionChoice = new QuizQuestionChoice
                {
                    OptionId = (int)st[0],
                    QuestionId = (int)st[1],
                    Choice = st[2].ToString(),
                    Correct = (bool)st[3]
                };

                quizQuestionChoiceList.Add(quizQuestionChoice);
            }

            return quizQuestionChoiceList.Select(i => new AnswerOption { Id = i.OptionId, Option = i.Choice }).ToList();
        }
        private static string GetExpectedAnswer(int QuestionId)
        {
            //MaterialEntities model = new MaterialEntities();
            //QuizQuestion question = model.QuizQuestions.Where(q => q.Id == QuestionId).FirstOrDefault();
            VmQuizQuestion question = GetQuizQuestion(QuestionId);
            string answer = "";
            if (question.Type == "Checkbox" || question.Type == "Radio" || question.Type == "Dropdown")
            {
                //List<int> correctChoices = model.QuizQuestionChoices.Where(q => q.QuestionId == QuestionId && q.Correct).OrderBy(q => q.OptionId).Select(qc => qc.OptionId).ToList();
                List<int> correctChoices = GetCorrectChoices(QuestionId);
                
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
            Dictionary<string,int> correctChoicesList = new Dictionary<string,int>();
            foreach (var item in correctChoicesData)
            {
                List<object> st = item;
                correctChoicesList.Add("OptionId", (int)st[0]);
            }

            return correctChoicesList.Select(qc => qc.Value).ToList();
        }
        public static List<ResultInfo> GetQuizQuestions(string hash, int activityId)
        {
            //List<AnswerOption> options = model.QuizQuestionChoices.Where(qqc => qqc.QuestionId == question.Id).OrderBy(q => q.OptionId).Select(i => new AnswerOption { Id = i.OptionId, Option = i.Choice }).ToList();
            //if (options == null)
            //{
            //    options = new List<AnswerOption>();
            //}

            //StudentQuizQuestion studentAnswer = question.StudentQuizQuestions.Where(sqq => sqq.StudentId == student.StudentId).OrderByDescending(studentqq => studentqq.Id).FirstOrDefault();
            //QuizQuestionRating quizQuestionRating = question.QuizQuestionRatings.Where(qqr => qqr.StudentId == student.StudentId).FirstOrDefault();

            //int questionRating = 0;
            //if (quizQuestionRating != null)
            //{
            //    questionRating = quizQuestionRating.Rating;
            //}

            //int grade = (studentAnswer != null) ? studentAnswer.Grade : 0;
            //string expectedAnswer = GetExpectedAnswer(question.Id);

            //QuizQuestionElementStyle elementStyle = question.QuizQuestionElementStyle;

            string sqlQuery = $@"select qq.Id as QuestionId, qq.Prompt1, qq.Prompt2, ISNULL(rat.Rating, 0) as Rating, ISNULL(ans.Grade, 0) as Grade, ISNULL(qq.MaxGrade, 0) as MaxGrade, ISNULL(ans.AnswerShown, 0) as AnswerShown, ans.Answer, 
                                qq.Source, ISNULL(qq.PositionX, 0) as PositionX, ISNULL(qq.PositionY, 0) PositionY, ISNULL(qq.Height, 0) as Height, ISNULL(qq.Width, 0) Width, qq.Type, qq.VideoTimestamp, qq.VideoSource, qq.EmbedAction, qq.CaseSensitive,
                                style.Color, style.FontFamily, style.BackgroundColor, style.FontSize, style.Border, style.PaddingLeft, style.PaddingRight, 
                                qq.Images , Isnull(qq.UsesHint, 0) UsesHint, Case When qq.Type = 'Checkbox' or qq.Type = 'Radio' or qq.Type = 'Dropdown' Then 1 Else 0 end IsMultipleChoice, 
                               Case When qq.Type = 'Checkbox' or qq.Type = 'Radio' or qq.Type = 'Dropdown' Then 
                                (SELECT STUFF((SELECT ',' + CAST(OptionId AS varchar) FROM QuizQuestionChoice t1  where t1.QuestionId =t2.QuestionId and t1.Correct = 1 FOR XML PATH('')), 1 ,1, '') AS ValueList
                                FROM QuizQuestionChoice t2
                                where t2.QuestionId = qq.Id and t2.Correct = 1
                                GROUP BY t2.QuestionId)
                                 Else qq.Answer end as ExpectedAnswer
                                , act.Title
                                from Activity act
                                join QuizQuestion qq on act.Id = qq.ActivityId1
                                left join QuizQuestionElementStyle style on qq.ElementStyleId = style.Id
                                left join (select * from StudentQuizQuestion where Id in ( 
	                                select MAX(sqq.Id) from QuizQuestion qq2 
	                                 join StudentQuizQuestion sqq on qq2.Id = sqq.QuestionId
	                                join Student st on sqq.StudentId= st.StudentId
	                                where st.Hash= '{hash}'
	                                and qq2.ActivityId1 = {activityId}
	                                Group by sqq.QuestionId)) ans on qq.Id =ans.QuestionId
                                left join (select qr.* from QuizQuestion qq3
	                                join QuizQuestionRating qr on qq3.Id = qr.QuestionId1
	                                join Student st on qr.StudentId= st.StudentId
	                                where st.Hash= '{hash}'
	                                and qq3.ActivityId1 = {activityId}) rat on qq.Id = rat.QuestionId1
                                where act.Id = {activityId}";


            //------------------------------------------Quize Options-----------------------------------------------------
            string sqlQueryOptions = $@"select opt.OptionId, opt.Choice, opt.QuestionId from Activity act
                                    join QuizQuestion qq on act.Id = qq.ActivityId1
                                    join QuizQuestionChoice opt on qq.Id = opt.QuestionId
                                    where act.Id = {activityId}";

            List<QuizOption> allOptions = new List<QuizOption>();
            var dataOption = SQLHelper.RunSqlQuery(sqlQueryOptions);

            if (dataOption.Count > 0)
            {
                foreach (var i in dataOption)
                {
                    var ao = new QuizOption
                    {
                        Id = (int)i[0],
                        Option = i[1].ToString(),
                        QuestionId = (int)i[2]
                    };
                    allOptions.Add(ao);
                }
            }
            //---------------------------------------------------------------------
            var data = SQLHelper.RunSqlQuery(sqlQuery);
            List<ResultInfo> results = new List<ResultInfo>();
            if (data.Count > 0)
            {
                foreach (var i in data)
                {
                    int qestionId = (int)i[0];
                    List<AnswerOption> options = allOptions.Where(x => x.QuestionId == qestionId).OrderBy(q => q.Id).Select(o => new AnswerOption { Id = o.Id, Option = o.Option }).ToList(); ;

                    var r = new ResultInfo
                    {
                        Id = (int)i[0],
                        Prompt1 = i[1].ToString(),
                        Prompt2 = i[2].ToString(),
                        QuestionRating = (int)i[3],
                        Grade = (int)i[4],
                        MaxGrade = (int)i[5],
                        AnswerShown = i[6].ToString() != "" ? (bool)i[6] : false, // Clean up
                        Answer = i[7].ToString(),
                        Source = i[8].ToString(),
                        PositionX = (int)i[9],
                        PositionY = (int)i[10],
                        Height = (int)i[11],
                        Width = (int)i[12],
                        Type = i[13].ToString(),
                        VideoTimestamp = (int)i[14],
                        VideoSource = i[15].ToString() == "" ? null : i[15].ToString(),
                        EmbedAction = i[16].ToString() != "" ? (bool)i[16] : false,
                        CaseSens = i[17].ToString() != "" ? (bool)i[17] : false,
                        Color = i[18].ToString(),
                        FontFamily = i[19].ToString(),
                        BackgroundColor = i[20].ToString(),
                        FontSize = i[21].ToString(),
                        Border = i[22].ToString(),
                        PaddingLeft = i[23].ToString(),
                        PaddingRight = i[24].ToString(),
                        Images = i[25].ToString() == "" ? null : i[25].ToString(),
                        UsesHint = (int)i[26] > 0,
                        IsMultipleChoice = (int)i[27] > 0,
                        ExpectedAnswer = i[28].ToString(),
                        Title = i[29].ToString(),
                        Options = options,

                    };
                    results.Add(r);
                }
            }


            //ResultInfo ri = new ResultInfo
            //{
            //    Id = question.Id,
            //    Options = options,
            //    Prompt1 = question.Prompt1,
            //    Prompt2 = question.Prompt2,
            //    QuestionRating = questionRating,
            //    Grade = grade,
            //    MaxGrade = question.MaxGrade,
            //    AnswerShown = studentAnswer != null ? studentAnswer.AnswerShown : false, // Clean up
            //    Answer = studentAnswer != null ? studentAnswer.Answer : "", // this is bad
            //    ExpectedAnswer = expectedAnswer,
            //    Source = question.Source,
            //    PositionX = question.PositionX,
            //    PositionY = question.PositionY,
            //    Height = question.Height,
            //    Width = question.Width,
            //    Type = question.Type,
            //    VideoTimestamp = question.VideoTimestamp,
            //    VideoSource = question.VideoSource,
            //    EmbedAction = question.EmbedAction,
            //    CaseSens = question.CaseSensitive,
            //    Color = elementStyle == null ? "" : elementStyle.Color,
            //    FontFamily = elementStyle == null ? "" : elementStyle.FontFamily,
            //    BackgroundColor = elementStyle == null ? "" : elementStyle.BackgroundColor,
            //    FontSize = elementStyle == null ? "" : elementStyle.FontSize,
            //    Border = elementStyle == null ? "" : elementStyle.Border,
            //    PaddingLeft = elementStyle == null ? "" : elementStyle.PaddingLeft,
            //    PaddingRight = elementStyle == null ? "" : elementStyle.PaddingRight,
            //    Images = question.Images,
            //    UsesHint = question.UsesHint.HasValue && question.UsesHint.Value > 0,
            //    IsMultipleChoice = (question.Type == "Checkbox" || question.Type == "Radio" || question.Type == "Dropdown")
            //};
            return results;
        }
        public class AnswerOption
        {
            public int Id { get; set; }
            public string Option { get; set; }
        }
        public class QuizOption
        {
            public int Id { get; set; }
            public int QuestionId { get; set; }
            public string Option { get; set; }
        }
        private class QuizQuestionChoice
        {
            public int OptionId { get; set; }
            public int QuestionId { get; set; }
            public string Choice { get; set; }
            public bool Correct { get; set; }
        }
        public class ResultInfo
        {
            public int Id { get; set; }
            public IEnumerable<AnswerOption> Options { get; set; }

            public string Prompt1 { get; set; }

            public string Prompt2 { get; set; }

            public int QuestionRating { get; set; }

            public int Grade { get; set; }

            public bool AnswerShown { get; set; }

            public string Answer { get; set; }

            public string ExpectedAnswer { get; set; }

            public string Source { get; set; }

            public int PositionX { get; set; }

            public int PositionY { get; set; }

            public int Height { get; set; }

            public int Width { get; set; }

            public string Type { get; set; }

            public int VideoTimestamp { get; set; }

            public string VideoSource { get; set; }

            public bool EmbedAction { get; set; }
            public bool CaseSens { get; set; }
            public string Color { get; set; }
            public string FontFamily { get; set; }
            public string BackgroundColor { get; set; }
            public string FontSize { get; set; }
            public string Border { get; set; }
            public string PaddingLeft { get; set; }
            public string PaddingRight { get; set; }
            public string Images { get; set; }
            public int MaxGrade { get; set; }
            public bool IsMultipleChoice { get; set; }
            public bool UsesHint { get; set; }
            public string Title { get; set; }

        }
    }
}
