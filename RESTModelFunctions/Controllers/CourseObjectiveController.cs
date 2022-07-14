using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using LMS.Common.HelperModels;
using LMSLibrary;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CourseObjectiveController : ApiController
    {
        public class StudentInfo
        {
            public string Hash { get; set; }
            public int CourseInstanceId { get; set; }
            public string Method { get; set; }
        }

        // POST api/Course
        public IHttpActionResult Post([FromBody] StudentInfo si)
        {
            string sqlCommand = $@"SELECT C.Id, C.Name, CO.Id AS CourseObjectiveId, CO.Description AS CourseObjective,  M.Id AS ModuleId, M.Description AS Module, M.DueDate AS ModuleDueDate, MO.Description AS ModuleObjective
                                                FROM Course C
                                                INNER JOIN CourseCourseObjective CCO
                                                ON C.Id = CCO.CourseId
                                                INNER JOIN CourseObjective CO
                                                ON CCO.CourseObjectiveId = CO.Id
                                                INNER JOIN CourseInstance CI
                                                ON C.Id = CI.CourseId
                                                INNER JOIN CourseInstanceStudent CIS
                                                ON CI.Id = CIS.CourseInstanceId
                                                INNER JOIN Student S
                                                ON CIS.StudentId = S.StudentId
                                                INNER JOIN CourseObjectiveModule COM
                                                ON COM.CourseObjectiveId = CO.Id
                                                INNER JOIN Module M
                                                ON COM.ModuleId = M.Id
                                                INNER JOIN ModuleModuleObjective MMO
                                                ON MMO.ModuleId = M.Id
                                                INNER JOIN ModuleObjective MO
                                                ON MMO.ModuleObjectiveId = MO.Id
                                                WHERE CO.Active = 1 AND M.Active = 1 AND MO.Active = 1 AND S.Hash = '{si.Hash}' AND CIS.CourseInstanceId = {si.CourseInstanceId}
                                                ORDER BY CourseObjectiveId, ModuleId, ModuleObjective;";

            var sql = SQLHelper.RunSqlQuery(sqlCommand);

            if (si.Method == "GetCourseObjective")
            {
                CourseInfo courseInfo1 = new CourseInfo()
                {
                    Id = Convert.ToInt32(sql[0][0]),
                    Name = (string)sql[0][1],
                    CourseObjectiveList = new List<CourseObjectiveInfo>()
                };

                CourseObjectiveInfo coInfo1 = new CourseObjectiveInfo
                {
                    Description = "",
                    Id = -1
                };

                ResultInfo ri1 = new ResultInfo()
                {
                    ModuleId = -1,
                    ModuleObjectives = "",
                    Description = "",
                    DueDate = ""
                };

                foreach (var co in sql)
                {
                    if (coInfo1.Id != Convert.ToInt32(co[2]))
                    {
                        coInfo1 = new CourseObjectiveInfo
                        {
                            Id = Convert.ToInt32(co[2]),
                            Description = (string)co[3],
                            Modules = new List<ResultInfo>()
                        };
                        courseInfo1.CourseObjectiveList.Add(coInfo1);
                    }

                    if (ri1.ModuleId != Convert.ToInt32(co[4]))
                    {
                        ri1 = new ResultInfo()
                        {
                            ModuleId = Convert.ToInt32(co[4]),
                            ModuleObjectives = "",
                            Description = (string)co[5],
                            DueDate = co[6].ToString()
                        };
                        coInfo1.Modules.Add(ri1);
                    }

                    if (ri1.ModuleObjectives != "")
                    {
                        ri1.ModuleObjectives += ", ";
                    }

                    ri1.ModuleObjectives += (string)co[7];
                }

                return Ok(courseInfo1);
            }
            else if (si.Method == "LoadGrades")
            {
                //MaterialEntities model = new MaterialEntities();
                //Student student = model.Students.Where(stud => stud.Hash == si.Hash).FirstOrDefault();
                //CourseInstance courseInstanceStudent = student.CourseInstances.Where(ci => ci.Id == si.CourseInstanceId).FirstOrDefault();
                //Course course = courseInstanceStudent.Course;
                var data = GetCourseInstanceInfo(si.Hash, si.CourseInstanceId);
                int studentId = (int)data[0];
                var moduleGrades = GetModuleGradeInfo(studentId, si.CourseInstanceId);
                int courseId = (int)data[2];
                //IEnumerable<CourseObjective> courseObjectives = course.CourseObjectives.Where(co => co.Active);
                IList<CourseObjectiveInfo> coList = new List<CourseObjectiveInfo>();
                var courseObjectives = moduleGrades.GroupBy(x => x.CourseObjectiveId).Select(y => y.Key).ToList();
                foreach (var co in courseObjectives)
                {
                    int courseObjectiveId = co;
                    CourseObjectiveInfo coInfo = new CourseObjectiveInfo
                    {
                        Id = courseObjectiveId
                    };

                    //List<Module> modules = co.Modules.Where(m => m.Active).ToList();
                    var modules = moduleGrades.Where(x => x.CourseObjectiveId == courseObjectiveId);
                    IList<ResultInfo> riList = new List<ResultInfo>();
                    foreach (var module in modules)
                    {
                        int moduleId = module.ModuleId;
                        Gradebook moduleGradebook = GetGradebook(data, module);
                        int percent = (int)Math.Round(moduleGradebook.CalculateWeightedGrade());
                        int completion = (int)Math.Round(moduleGradebook.CalculateTotalCompletion());

                        ResultInfo ri = new ResultInfo()
                        {
                            ModuleId = moduleId,
                            Percent = percent,
                            Completion = completion,
                            //GPA = GetGPAByPercent(courseId, percent),
                            StrokeDasharray = completion * 2.5 + " " + (500 - completion * 2.5),
                        };
                        riList.Add(ri);
                    }
                    coInfo.Modules = (List<ResultInfo>)riList;
                    coList.Add(coInfo);
                }
                return Ok(coList);
            }
            else
            {
                return Ok();
            }
        }
        private static Gradebook GetGradebook(List<object> data, VmModuleGrade moduleGrade)
        {
            int studentId = (int)data[0];
            int courseInstanceId = (int)data[1];

            string[] resultGrades;
            //System.Data.Entity.Core.Objects.ObjectResult<ModuleGrade_Result> result = model.ModuleGrade(studentId, courseInstanceId, moduleId, courseObjectiveId);
            //ModuleGrade_Result resultValue = result.FirstOrDefault();
            var resultValue = moduleGrade;
            Gradebook moduleGradebook = new Gradebook();
            //GradeWeight gradeWeight = courseInstance.GradeWeights.FirstOrDefault();

            if (data[3].ToString() != "")
            {
                //moduleGradebook.Assessment.Weight = gradeWeight.AssessmentWeight;
                //moduleGradebook.Quiz.Weight = gradeWeight.ActivityWeight;
                //moduleGradebook.Material.Weight = gradeWeight.MaterialWeight;
                //moduleGradebook.Discussion.Weight = gradeWeight.DiscussionWeight;
                //moduleGradebook.Poll.Weight = gradeWeight.PollWeight;
                //moduleGradebook.Midterm.Weight = gradeWeight.MidtermWeight;
                //moduleGradebook.Final.Weight = gradeWeight.FinalWeight;

                moduleGradebook.Quiz.Weight = (int)data[3];
                moduleGradebook.Assessment.Weight = (int)data[4];
                moduleGradebook.Material.Weight = (int)data[5];
                moduleGradebook.Discussion.Weight = (int)data[6];
                moduleGradebook.Poll.Weight = (int)data[7];
                moduleGradebook.Midterm.Weight = (int)data[8];
                moduleGradebook.Final.Weight = (int)data[9];
                //------------------AssessmentGrade----------------------
                //resultGrades = resultValue[1].ToString().Split(',');
                resultGrades = resultValue.AssessmentGrade.ToString().Split(',');
                moduleGradebook.Assessment.Grade = Convert.ToInt32(resultGrades[0]);
                moduleGradebook.Assessment.Occurrence = Convert.ToInt32(resultGrades[1]);
                moduleGradebook.Assessment.Completion = Convert.ToInt32(resultGrades[2]);
                //------------------ActivityGrade----------------------
                //resultGrades = resultValue[0].ToString().Split(',');
                resultGrades = resultValue.ActivityGrade.ToString().Split(',');
                moduleGradebook.Quiz.Grade = Convert.ToInt32(resultGrades[0]);
                moduleGradebook.Quiz.Occurrence = Convert.ToInt32(resultGrades[1]);
                moduleGradebook.Quiz.Completion = Convert.ToInt32(resultGrades[2]);

                moduleGradebook.Material.Grade = 0;
                moduleGradebook.Material.Occurrence = 0;
                moduleGradebook.Material.Completion = 0;
                //------------------DiscussionGrade----------------------
                //resultGrades = resultValue[5].ToString().Split(',');
                resultGrades = resultValue.DiscussionGrade.ToString().Split(',');
                moduleGradebook.Discussion.Grade = Convert.ToInt32(resultGrades[0]);
                moduleGradebook.Discussion.Occurrence = Convert.ToInt32(resultGrades[1]);
                moduleGradebook.Discussion.Completion = Convert.ToInt32(resultGrades[2]);
                //------------------PollGrade----------------------
                //resultGrades = resultValue[4].ToString().Split(',');
                resultGrades = resultValue.PollGrade.ToString().Split(',');
                moduleGradebook.Poll.Grade = Convert.ToInt32(resultGrades[0]);
                moduleGradebook.Poll.Occurrence = Convert.ToInt32(resultGrades[1]);
                moduleGradebook.Poll.Completion = Convert.ToInt32(resultGrades[2]);
                //------------------MidtermGrade----------------------
                //resultGrades = resultValue[3].ToString().Split(',');
                resultGrades = resultValue.MidtermGrade.ToString().Split(',');
                moduleGradebook.Midterm.Grade = Convert.ToInt32(resultGrades[0]);
                moduleGradebook.Midterm.Occurrence = Convert.ToInt32(resultGrades[1]);
                moduleGradebook.Midterm.Completion = Convert.ToInt32(resultGrades[2]);
                //------------------FinalGrade----------------------
                //resultGrades = resultValue[2].ToString().Split(',');
                resultGrades = resultValue.FinalGrade.ToString().Split(',');
                moduleGradebook.Final.Grade = Convert.ToInt32(resultGrades[0]);
                moduleGradebook.Final.Occurrence = Convert.ToInt32(resultGrades[1]);
                moduleGradebook.Final.Completion = Convert.ToInt32(resultGrades[2]);
            }
            return moduleGradebook;
        }

        private static double GetGPAByPercent(int courseId, double percent)
        {
            //GradeScale maxPossibleGPAElement = course.GradeScaleGroup.GradeScales.Where(gs => gs.MinNumberInPercent <= percent).OrderByDescending(gs => gs.MinNumberInPercent).First();
            string sqlQuery = $@"select top 1 gs.GPA from GradeScale gs
                                join Course c on gs.GradeScaleGroupId = c.GradeScaleGroupId
                                where c.Id = {courseId} 
                                and MinNumberInPercent <= {percent}
                                order by MinNumberInPercent Desc";

            var data = SQLHelper.RunSqlQuery(sqlQuery);

            return (double)data.First()[0];
        }

        private static List<Object> GetCourseInstanceInfo(string hash, int courseInstanceId)
        {
            string sqlQuery = $@"select s.StudentId, gw.CourseInstanceId, ci.CourseId, ActivityWeight, AssessmentWeight, MaterialWeight,
                                        DiscussionWeight,PollWeight,MidtermWeight, FinalWeight  from Student s
                                        join CourseInstanceStudent cis on s.StudentId = cis.StudentId
                                        Join CourseInstance ci on cis.CourseInstanceId = ci.Id
                                        left Join GradeWeight gw on ci.Id = gw.CourseInstanceId
                                        where s.Hash= '{hash}'
                                        and ci.Id = {courseInstanceId}";

            var data = SQLHelper.RunSqlQuery(sqlQuery);
            return data.FirstOrDefault();
        }
        //private static List<List<Object>> GetCourseObjectiveInfo(int courseId)
        //{
        //    string sqlQuery = $@"select co.Id CourseObjectiveId, co.Description  
        //                        from CourseCourseObjective cco
        //                        join CourseObjective co on cco.CourseObjectiveId = co.Id
        //                        where CourseId = {courseId}
        //                        and co.Active = 1";

        //    var data = SQLHelper.RunSqlQuery(sqlQuery);
        //    return data;
        //}
        //private static List<List<Object>> GetModuleInfo(int courseObjectiveId)
        //{
        //    string sqlQuery = $@"select m.Id ModuleId, m.Description 
        //                        from CourseObjectiveModule com
        //                        join Module m on com.ModuleId = m.Id
        //                        where CourseObjectiveId = {courseObjectiveId}
        //                        and m.Active = 1";

        //    var data = SQLHelper.RunSqlQuery(sqlQuery);
        //    return data;
        //}
        private static List<VmModuleGrade> GetModuleGradeInfo(int studentId, int courseInstanceId)
        {
            string sqlQuery = $@"exec ModuleGradeFullCourse {studentId}, {courseInstanceId}";
            var data = SQLHelper.RunSqlQuery(sqlQuery);

            //string sqlQuery = $@"ModuleGradeFullCourse";
            //List<Param> paramList = new List<Param>();
            //paramList.Add(new Param() { Name = "StudentId", Value = studentId });
            //paramList.Add(new Param() { Name = "CourseInstanceId", Value = courseInstanceId });

            //var data = SQLHelper.RunSqlQueryList(sqlQuery, paramList);

            List<VmModuleGrade> grades = new List<VmModuleGrade>();
            if (data.Count > 0)
            {
                foreach (var i in data)
                {
                    var mg = new VmModuleGrade
                    {
                        ActivityGrade = i[0].ToString(),
                        AssessmentGrade = i[1].ToString(),
                        FinalGrade = i[2].ToString(),
                        MidtermGrade = i[3].ToString(),
                        PollGrade = i[4].ToString(),
                        DiscussionGrade = i[5].ToString(),
                        CourseObjectiveId = (int)i[6],
                        ModuleId = (int)i[7],
                        ModuleObjectiveId = (int)i[8],

                    };
                    grades.Add(mg);
                }
            }


            return grades;
        }
        public class ResultInfo
        {
            public int ModuleId { get; set; }
            public string ModuleObjectives { get; set; }
            public int Percent { get; set; }
            public int Completion { get; set; }
            public string Description { get; set; }
            public double GPA { get; set; }
            public string DueDate { get; set; }
            public string StrokeDasharray { get; set; }
        }
        public class VmModuleGrade
        {
            public string ActivityGrade { get; set; }
            public string AssessmentGrade { get; set; }
            public string FinalGrade { get; set; }
            public string MidtermGrade { get; set; }
            public string PollGrade { get; set; }
            public string DiscussionGrade { get; set; }
            public int CourseObjectiveId { get; set; }
            public int ModuleId { get; set; }
            public int ModuleObjectiveId { get; set; }
        }

        public class CourseObjectiveInfo
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public List<ResultInfo> Modules { get; set; }
        }
        private class CourseInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public IList<CourseObjectiveInfo> CourseObjectiveList { get; set; }
        }
    }
}
