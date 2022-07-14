using System;

namespace LMSLibrary
{
    public class Gradebook
    {
        public class ActivityGradeInfo
        {
            public int Grade { get; set; }
            public int Occurrence { get; set; }
            public int Weight { get; set; }
            public int Completion { get; set; }
        }

        public ActivityGradeInfo Assessment { get; set; }
        public ActivityGradeInfo Quiz { get; set; }
        public ActivityGradeInfo Material { get; set; }
        public ActivityGradeInfo Midterm { get; set; }
        public ActivityGradeInfo Final { get; set; }
        public ActivityGradeInfo Discussion { get; set; }
        public ActivityGradeInfo Poll { get; set; }
        public bool ForCourse { get; set; }

        public Gradebook(bool forCourse = false)
        {
            Assessment = new ActivityGradeInfo();
            Quiz = new ActivityGradeInfo();
            Material = new ActivityGradeInfo();
            Midterm = new ActivityGradeInfo();
            Final = new ActivityGradeInfo();
            Discussion = new ActivityGradeInfo();
            Poll = new ActivityGradeInfo();
            ForCourse = forCourse;
        }

        public Gradebook(Gradebook obj)
        {
            Assessment = new ActivityGradeInfo();
            Assessment.Weight = obj.Assessment.Weight;

            Quiz = new ActivityGradeInfo();
            Quiz.Weight = obj.Quiz.Weight;

            Material = new ActivityGradeInfo();
            Material.Weight = obj.Material.Weight;

            Midterm = new ActivityGradeInfo();
            Midterm.Weight = obj.Midterm.Weight;

            Final = new ActivityGradeInfo();
            Final.Weight = obj.Final.Weight;

            Discussion = new ActivityGradeInfo();
            Discussion.Weight = obj.Discussion.Weight;

            Poll = new ActivityGradeInfo();
            Poll.Weight = obj.Poll.Weight;

            ForCourse = obj.ForCourse;
        }

        public double CalculateWeightedGrade()
        {
            double result = 0;
            int totalWeight = GetTotalWeight();

            if (totalWeight != 0)
            {

                //result = (double)(((Assessment.Occurrence == 0) ? 0.0 : Assessment.Grade * Assessment.Weight) +
                //    ((Quiz.Occurrence == 0) ? 0.0 : Quiz.Grade * Quiz.Weight) +
                //    ((Material.Occurrence == 0) ? 0.0 : Material.Grade * Material.Weight) +
                //    ((Midterm.Occurrence == 0) ? 0.0 : Midterm.Grade * Midterm.Weight) +
                //    ((Final.Occurrence == 0) ? 0.0 : Final.Grade * Final.Weight) +
                //    ((Discussion.Occurrence == 0) ? 0.0 : Discussion.Grade * Discussion.Weight) +
                //    ((Poll.Occurrence == 0) ? 0.0 : Poll.Grade * Poll.Weight)) / totalWeight;

                //result = (double)(Assessment.Grade * Assessment.Weight +
                //        Quiz.Grade * Quiz.Weight + Material.Grade * Material.Weight +
                //        Midterm.Grade * Midterm.Weight + Final.Grade * Final.Weight +
                //        Discussion.Grade * Discussion.Weight + Poll.Grade * Poll.Weight) / totalWeight;

                int weight = ((Assessment.Occurrence == 0) ? 0 : Assessment.Weight) +
                    ((Quiz.Occurrence == 0) ? 0 : Quiz.Weight) +
                    ((Midterm.Occurrence == 0) ? 0 : Midterm.Weight) +
                    ((Final.Occurrence == 0) ? 0 : Final.Weight);

                if (weight == 0)
                    return 0;

                result = (double)(Assessment.Grade * Assessment.Weight +
                        Quiz.Grade * Quiz.Weight + Material.Grade * Material.Weight +
                        Midterm.Grade * Midterm.Weight + Final.Grade * Final.Weight +
                        Discussion.Grade * Discussion.Weight + Poll.Grade * Poll.Weight) / weight;
            }

            return result;
        }

        public double CalculateTotalCompletion()
        {
            double result = 0;
            int totalOccurence = GetTotalOccurence();

            if (totalOccurence != 0)
            {
                result = (double)(Assessment.Completion +
                                    Quiz.Completion +
                                    Material.Completion +
                                    Midterm.Completion +
                                    Final.Completion +
                                    Discussion.Completion +
                                    Poll.Completion) / totalOccurence;
            }

            return result;
        }

        public void Add(Gradebook obj)
        {
            Assessment.Grade += obj.Assessment.Grade;
            Quiz.Grade += obj.Quiz.Grade;
            Material.Grade += obj.Material.Grade;
            Midterm.Grade += obj.Midterm.Grade;
            Final.Grade += obj.Final.Grade;
            Discussion.Grade += obj.Discussion.Grade;
            Poll.Grade += obj.Poll.Grade;

            Assessment.Occurrence += obj.Assessment.Occurrence;
            Quiz.Occurrence += obj.Quiz.Occurrence;
            Material.Occurrence += obj.Material.Occurrence;
            Midterm.Occurrence += obj.Midterm.Occurrence;
            Final.Occurrence += obj.Final.Occurrence;
            Discussion.Occurrence += obj.Discussion.Occurrence;
            Poll.Occurrence += obj.Poll.Occurrence;

            Assessment.Completion += obj.Assessment.Completion;
            Quiz.Completion += obj.Quiz.Completion;
            Material.Completion += obj.Material.Completion;
            Midterm.Completion += obj.Midterm.Completion;
            Final.Completion += obj.Final.Completion;
            Discussion.Completion += obj.Discussion.Completion;
            Poll.Completion += obj.Poll.Completion;

        }

        public void NormalizeGrade()
        {
            if (Assessment.Occurrence > 1)
            {
                Assessment.Grade = (int)Math.Round((double)Assessment.Grade / Assessment.Occurrence);
                Assessment.Completion = (int)Math.Round((double)Assessment.Completion / Assessment.Occurrence);
                Assessment.Occurrence = 1;
            }
            if (Quiz.Occurrence > 1)
            {
                Quiz.Grade = (int)Math.Round((double)Quiz.Grade / Quiz.Occurrence);
                Quiz.Completion = (int)Math.Round((double)Quiz.Completion / Quiz.Occurrence);
                Quiz.Occurrence = 1;
            }
            if (Material.Occurrence > 1)
            {
                Material.Grade = (int)Math.Round((double)Material.Grade / Material.Occurrence);
                Material.Completion = (int)Math.Round((double)Material.Completion / Material.Occurrence);
                Material.Occurrence = 1;
            }
            if (Midterm.Occurrence > 1)
            {
                Midterm.Grade = (int)Math.Round((double)Midterm.Grade / Midterm.Occurrence);
                Midterm.Completion = (int)Math.Round((double)Midterm.Completion / Midterm.Occurrence);
                Midterm.Occurrence = 1;
            }
            if (Final.Occurrence > 1)
            {
                Final.Grade = (int)Math.Round((double)Final.Grade / Final.Occurrence);
                Final.Completion = (int)Math.Round((double)Final.Completion / Final.Occurrence);
                Final.Occurrence = 1;
            }
            if (Discussion.Occurrence > 1)
            {
                Discussion.Grade = (int)Math.Round((double)Discussion.Grade / Discussion.Occurrence);
                Discussion.Completion = (int)Math.Round((double)Discussion.Completion / Discussion.Occurrence);
                Discussion.Occurrence = 1;
            }
            if (Poll.Occurrence > 1)
            {
                Poll.Grade = (int)Math.Round((double)Poll.Grade / Poll.Occurrence);
                Poll.Completion = (int)Math.Round((double)Poll.Completion / Poll.Occurrence);
                Poll.Occurrence = 1;
            }

        }

        public int GetTotalWeight()
        {
            if (ForCourse)
            {
                return (Assessment.Weight + Quiz.Weight + Material.Weight + Midterm.Weight + Final.Weight +
                        Discussion.Weight + Poll.Weight);
            }
            else
            {
                return ((Assessment.Occurrence == 0) ? 0 : Assessment.Weight) +
                    ((Quiz.Occurrence == 0) ? 0 : Quiz.Weight) +
                    ((Material.Occurrence == 0) ? 0 : Material.Weight) +
                    ((Midterm.Occurrence == 0) ? 0 : Midterm.Weight) +
                    ((Final.Occurrence == 0) ? 0 : Final.Weight) +
                    ((Discussion.Occurrence == 0) ? 0 : Discussion.Weight) +
                    ((Poll.Occurrence == 0) ? 0 : Poll.Weight);
            }
        }

        public int GetTotalOccurence()
        {
            return (Assessment.Occurrence +
                Quiz.Occurrence +
                Material.Occurrence +
                Midterm.Occurrence +
                Final.Occurrence +
                Discussion.Occurrence +
                Poll.Occurrence);
        }


        public string GetLetterGrade(int iTotalCompletion, int iGradeScaleGroupId)
        {
            string sqlQueryGPA = $@"SELECT GPA FROM GradeScale
                                    WHERE GradeScaleGroupId={iGradeScaleGroupId}
                                    AND MinNumberInPercent <={iTotalCompletion}
                                    AND MaxNumberInPercent >={iTotalCompletion}";
            // Note: in the db, Max and Min percent values DON'T overlap

            var dbRows = SQLHelper.RunSqlQuery(sqlQueryGPA);
            var dbCols = dbRows[0]; // We should get only one row
            double dGPA = (double)dbCols[0]; // We requested only one column

            // Convert GPA to textal equivalent
            if (dGPA >= 4.00f && iTotalCompletion == 100) return "A+";
            if (dGPA >= 4.00f) return "A";
            if (dGPA >= 3.67f) return "A-";
            if (dGPA >= 3.33f) return "B+";
            if (dGPA >= 3.00f) return "B";
            if (dGPA >= 2.67f) return "B-";
            if (dGPA >= 2.33f) return "C+";
            if (dGPA >= 2.00f) return "C";
            if (dGPA >= 1.67f) return "C-";
            if (dGPA >= 1.33f) return "D+";
            if (dGPA >= 1.00f) return "D";
            if (dGPA >= 0.67f) return "D-";

            return "F"; // (the lowest grade)
        }
    }
}
