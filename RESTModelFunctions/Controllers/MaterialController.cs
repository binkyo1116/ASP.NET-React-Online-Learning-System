//using EFModel;
using LMSLibrary;
using System;
//using HintsLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;


namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MaterialController : ApiController
    {
        public class QuestionSetInfo
        {
            public int QuestionSetId { get; set; }

            public string QuestionSetType { get; set; }

            public string StudentHash { get; set; }
        }

        public class QuestionHint
        {
            public int HintId { get; set; }
            public string Hint { get; set; }
            public int HintRating { get; set; }
        }

        // POST api/Course
        public IHttpActionResult Post([FromBody] QuestionSetInfo qsi)
        {
            //MaterialEntities model = new MaterialEntities();
            //Student student = model.Students.Where(s => s.Hash == qsi.StudentHash).FirstOrDefault();
            //Activity activity = model.Activities.Where(a => a.Id == qsi.QuestionSetId).FirstOrDefault();
            //List<QuestionController.ResultInfo> quizQuestions = activity.QuizQuestions.Select(qq => QuestionController.GetResultInfo(model, qq, student)).ToList();

            Dictionary<int, QuestionHint> quizQuestionHints = new Dictionary<int, QuestionHint>();
            var quizQuestionData = QuestionController.GetQuizQuestions(qsi.StudentHash, qsi.QuestionSetId);

            //-------------Old Comment Area---------------------
            /*
            foreach (var question in quizQuestions)
            {
				if (question.UsesHint)
				{
					var quizHint = HintStudent.GetQuestionHint(question.Id, question.Answer);

					if (quizHint != null)
                    {
						var hintRating = model.QuizHintProblemRatings.SingleOrDefault(qhpr => qhpr.StudentId == student.StudentId && qhpr.HintId == quizHint.Item1);
						var rating = 0;

						if (hintRating != null)
						{
							rating = hintRating.Rating;

						}

						quizQuestionHints.Add(question.Id, new QuestionHint{
							HintId = quizHint.Item1,
							Hint = quizHint.Item2,
							HintRating = rating
						});
					}
				}
			}
			*/
            //--------------------------------------
            //var materials = activity.ActivityMaterials.Join(model.Materials, (am => am.MaterialId), (m => m.Id), ((am, m) => new { Title = m.Title, Link = m.Description })).ToList();

            var materials = GetMaterialInfo(qsi.QuestionSetId);
            var title = quizQuestionData.FirstOrDefault() == null ? "" : quizQuestionData.FirstOrDefault().Title;
            //ActivityGrade activityGrade = model.ActivityGrades.Where(ag => ag.StudentId == student.StudentId && ag.ActivityId == activity.Id).FirstOrDefault();
            //QuizGrader.CalculateGrade(activity, student, out int recalculatedGrade, out int totalGrade, out int totalShown);
            QuizGrader.CalculateQuizGrade(quizQuestionData, out int recalculatedGrade, out int totalGrade, out int totalShown);
            return Ok(new { QuizQuestions = quizQuestionData, QuizQuestionHints = quizQuestionHints, TotalGrade = totalGrade, TotalShown = totalShown, Title = title, Materails = materials });
        }
        private static List<MaterialInfo> GetMaterialInfo(int activityId)
        {
            string sqlQuery = $@"select m.Title, m.Description from ActivityMaterial am
                                join Material m on am.MaterialId = m.Id
                                where ActivityId = {activityId}";

            var data = SQLHelper.RunSqlQuery(sqlQuery);
            List<MaterialInfo> meterials = new List<MaterialInfo>();
            if (data.Count > 0)
            {
                foreach (var i in data)
                {
                    var mi = new MaterialInfo
                    {
                        Title = i[0].ToString(),
                        Link = i[1].ToString()
                    };
                    meterials.Add(mi);
                }
            }
            return meterials;
        }
        public class MaterialInfo
        {
            public string Title { get; set; }
            public string Link { get; set; }
        }
    }
}

