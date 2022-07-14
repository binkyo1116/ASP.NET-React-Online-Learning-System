using EFModel;
using LMSLibrary;
using System;
using System.Linq;

namespace AdminPages.Services
{
    public static class GradeBookService
    {
        public static Gradebook GetGradebook(MaterialEntities model, Student student, CourseInstance courseInstance)
        {
            //return new Gradebook(true);
            // TODO: Fix this
            string[] resultGrades;
            CourseGrade_Result resultValue = model.CourseGrade(student.StudentId, courseInstance.Id).FirstOrDefault();

            Gradebook moduleGradebook = new Gradebook(true);
            GradeWeight gradeWeight = courseInstance.GradeWeights.FirstOrDefault();
            if (gradeWeight != null)
            {
                moduleGradebook.Assessment.Weight = gradeWeight.AssessmentWeight;
                moduleGradebook.Quiz.Weight = gradeWeight.ActivityWeight;
                moduleGradebook.Material.Weight = gradeWeight.MaterialWeight;
                moduleGradebook.Discussion.Weight = gradeWeight.DiscussionWeight;
                moduleGradebook.Poll.Weight = gradeWeight.PollWeight;
                moduleGradebook.Midterm.Weight = gradeWeight.MidtermWeight;
                moduleGradebook.Final.Weight = gradeWeight.FinalWeight;

                resultGrades = resultValue.AssessmentGrade.Split(',');
                moduleGradebook.Assessment.Grade = Convert.ToInt32(resultGrades[0]);
                moduleGradebook.Assessment.Occurrence = Convert.ToInt32(resultGrades[1]);
                moduleGradebook.Assessment.Completion = Convert.ToInt32(resultGrades[2]);

                resultGrades = resultValue.ActivityGrade.Split(',');
                moduleGradebook.Quiz.Grade = Convert.ToInt32(resultGrades[0]);
                moduleGradebook.Quiz.Occurrence = Convert.ToInt32(resultGrades[1]);
                moduleGradebook.Quiz.Completion = Convert.ToInt32(resultGrades[2]);

                //resultGrades = resultValue.MaterialGrade.Split(',');
                moduleGradebook.Material.Grade = 0;
                moduleGradebook.Material.Occurrence = 0;
                moduleGradebook.Material.Completion = 0;

                resultGrades = resultValue.DiscussionGrade.Split(',');
                moduleGradebook.Discussion.Grade = Convert.ToInt32(resultGrades[0]);
                moduleGradebook.Discussion.Occurrence = Convert.ToInt32(resultGrades[1]);
                moduleGradebook.Discussion.Completion = Convert.ToInt32(resultGrades[2]);

                resultGrades = resultValue.PollGrade.Split(',');
                moduleGradebook.Poll.Grade = Convert.ToInt32(resultGrades[0]);
                moduleGradebook.Poll.Occurrence = Convert.ToInt32(resultGrades[1]);
                moduleGradebook.Poll.Completion = Convert.ToInt32(resultGrades[2]);

                resultGrades = resultValue.MidtermGrade.Split(',');
                moduleGradebook.Midterm.Grade = Convert.ToInt32(resultGrades[0]);
                moduleGradebook.Midterm.Occurrence = Convert.ToInt32(resultGrades[1]);
                moduleGradebook.Midterm.Completion = Convert.ToInt32(resultGrades[2]);

                resultGrades = resultValue.FinalGrade.Split(',');
                moduleGradebook.Final.Grade = Convert.ToInt32(resultGrades[0]);
                moduleGradebook.Final.Occurrence = Convert.ToInt32(resultGrades[1]);
                moduleGradebook.Final.Completion = Convert.ToInt32(resultGrades[2]);
            }
            return moduleGradebook;
        }
        public static double GetGPAByPercent(double percent, GradeScale cgs)
        {
            if (cgs == null)
            {
                return 0;
            }

            GradeScale maxPossibleGPAElement = cgs.GradeScaleGroup.GradeScales.Where(gs => gs.MinNumberInPercent <= percent).OrderByDescending(gs => gs.MinNumberInPercent).First();
            return maxPossibleGPAElement.GPA;
        }
        public static int GetCurrentGrade(MaterialEntities model, Student student, CourseInstance courseInstance)
        {
            string[] resultGrades;
            CourseGradeCurrent_Result resultValue = model.CourseGradeCurrent(student.StudentId, courseInstance.Id).FirstOrDefault();

            Gradebook courseGradebook = new Gradebook(true);
            GradeWeight gradeWeight = courseInstance.GradeWeights.FirstOrDefault();
            if (gradeWeight != null)
            {
                courseGradebook.Assessment.Weight = gradeWeight.AssessmentWeight;
                courseGradebook.Quiz.Weight = gradeWeight.ActivityWeight;
                courseGradebook.Material.Weight = gradeWeight.MaterialWeight;
                courseGradebook.Discussion.Weight = gradeWeight.DiscussionWeight;
                courseGradebook.Poll.Weight = gradeWeight.PollWeight;
                courseGradebook.Midterm.Weight = gradeWeight.MidtermWeight;
                courseGradebook.Final.Weight = gradeWeight.FinalWeight;

                resultGrades = resultValue.AssessmentGrade.Split(',');
                courseGradebook.Assessment.Grade = Convert.ToInt32(resultGrades[0]);
                courseGradebook.Assessment.Occurrence = Convert.ToInt32(resultGrades[1]);
                courseGradebook.Assessment.Completion = Convert.ToInt32(resultGrades[2]);

                resultGrades = resultValue.ActivityGrade.Split(',');
                courseGradebook.Quiz.Grade = Convert.ToInt32(resultGrades[0]);
                courseGradebook.Quiz.Occurrence = Convert.ToInt32(resultGrades[1]);
                courseGradebook.Quiz.Completion = Convert.ToInt32(resultGrades[2]);

                //resultGrades = resultValue.MaterialGrade.Split(',');
                courseGradebook.Material.Grade = 0;
                courseGradebook.Material.Occurrence = 0;
                courseGradebook.Material.Completion = 0;

                resultGrades = resultValue.DiscussionGrade.Split(',');
                courseGradebook.Discussion.Grade = Convert.ToInt32(resultGrades[0]);
                courseGradebook.Discussion.Occurrence = Convert.ToInt32(resultGrades[1]);
                courseGradebook.Discussion.Completion = Convert.ToInt32(resultGrades[2]);

                resultGrades = resultValue.PollGrade.Split(',');
                courseGradebook.Poll.Grade = Convert.ToInt32(resultGrades[0]);
                courseGradebook.Poll.Occurrence = Convert.ToInt32(resultGrades[1]);
                courseGradebook.Poll.Completion = Convert.ToInt32(resultGrades[2]);

                resultGrades = resultValue.MidtermGrade.Split(',');
                courseGradebook.Midterm.Grade = Convert.ToInt32(resultGrades[0]);
                courseGradebook.Midterm.Occurrence = Convert.ToInt32(resultGrades[1]);
                courseGradebook.Midterm.Completion = Convert.ToInt32(resultGrades[2]);

                resultGrades = resultValue.FinalGrade.Split(',');
                courseGradebook.Final.Grade = Convert.ToInt32(resultGrades[0]);
                courseGradebook.Final.Occurrence = Convert.ToInt32(resultGrades[1]);
                courseGradebook.Final.Completion = Convert.ToInt32(resultGrades[2]);
            }

            return (int)Math.Round(courseGradebook.CalculateWeightedGrade());

        }
    }
}