using LMS.Common.ViewModels;
using LMSLibrary;

namespace RESTModelFunctionsCore.Controllers
{
    public static class QuizGrader
    {
        public static void CalculateGrade(VmActivity activity, VmStudent student, out int quizGrade, out int totalGrade, out int totalShown)
        {
            int maxQuizGrade = GetMaxQuizGrade(activity.Id);
            IEnumerable<VmStudentQuizQuestion> qqStudentAnswers = GetStudentQuizQuestion(activity.Id, student.StudentId);

            quizGrade = qqStudentAnswers.Select(qqa => qqa.Grade).Sum();
            int answerShownCount = GetAnswerShownCount(activity.Id, student.StudentId);

            totalGrade = (int)Math.Round((quizGrade * 100.0) / maxQuizGrade);
            totalShown = (int)Math.Round((answerShownCount * 100.0) / maxQuizGrade);
        }
        public static void CalculateQuizGrade(List<QuestionResultInfo> quizData, out int quizGrade, out int totalGrade, out int totalShown)
        {
            int maxQuizGrade = quizData.Select(qq => qq.MaxGrade).Sum();
            quizGrade = quizData.Select(qqa => qqa.Grade).Sum();
            int answerShownCount = quizData.Where(qqa => qqa.AnswerShown).Select(qqa => qqa.MaxGrade).Sum();

            totalGrade = (int)Math.Round((quizGrade * 100.0) / maxQuizGrade);
            totalShown = (int)Math.Round((answerShownCount * 100.0) / maxQuizGrade);
        }
        public static int GetMaxQuizGrade(int activityId)
        {
            string sqlQueryMaxQuizGrade = $@"select  sum(MaxGrade) from QuizQuestion where ActivityId1 = {activityId}";

            var maxQuizGradeData = SQLHelper.RunSqlQuery(sqlQueryMaxQuizGrade);
            int maxQuizGrade = 0;

            if (maxQuizGradeData.Count > 0)
            {
                List<object> st = maxQuizGradeData[0];
                maxQuizGrade = (int)st[0];
            }
            return maxQuizGrade;
        }
        public static int GetAnswerShownCount(int activityId, int studentId)
        {
            string sqlAnswerShownCount = $@"select sum(q.MaxGrade) from StudentQuizQuestion sqq 
                                            inner join QuizQuestion q on sqq.QuestionId = q.Id
                                            where sqq.StudentId = {studentId}
                                            and q.ActivityId1 = {activityId}
                                            and sqq.AnswerShown  =1";

            var answerShownCountData = SQLHelper.RunSqlQuery(sqlAnswerShownCount);
            int answerShownCount = 0;

            if (answerShownCountData.Count > 0)
            {
                List<object> st = answerShownCountData[0];
                answerShownCount = (int)st[0];
            }
            return answerShownCount;
        }
        private static List<VmStudentQuizQuestion> GetStudentQuizQuestion(int activityId, int studentId)
        {
            string sqlStudentQuizQuestion = $@"select sqq.Id,sqq.StudentId,sqq.Answer,sqq.Expected,sqq.Date,sqq.AnswerShown,
                                            sqq.History,sqq.QuestionId,sqq.Grade from StudentQuizQuestion sqq
                                            inner join QuizQuestion q on sqq.QuestionId = q.Id
                                            where sqq.StudentId = {studentId}
                                            and q.ActivityId1 = {activityId}
                                            order by sqq.QuestionId desc";

            var studentQuizQuestionData = SQLHelper.RunSqlQuery(sqlStudentQuizQuestion);
            List<VmStudentQuizQuestion> studentQuizQuestionList = new List<VmStudentQuizQuestion>();

            foreach (var item in studentQuizQuestionData)
            {
                VmStudentQuizQuestion studentQuizQuestion = null;
                List<object> st = item;
                studentQuizQuestion = new VmStudentQuizQuestion
                {
                    Id = (int)st[0],
                    StudentId = (int)st[1],
                    Answer = (string)st[2],
                    Expected = (string)st[3],
                    Date = (DateTime)st[4],
                    AnswerShown = (bool)st[5],
                    History = st[6].ToString(),
                    QuestionId = (int)st[7],
                    Grade = (int)st[8]
                };

                studentQuizQuestionList.Add(studentQuizQuestion);
            }

            return studentQuizQuestionList;
        }
    }
}
