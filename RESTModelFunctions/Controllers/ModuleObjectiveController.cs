using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using LMSLibrary;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ModuleObjectiveController : ApiController
    {
        public class StudentInfo
        {
            public string Hash { get; set; }
            public int CourseInstanceId { get; set; }
            public int ModuleId { get; set; }
        }

        // POST api/Course
        public IHttpActionResult Post([FromBody] StudentInfo si)
        {
            String sqlQuery = $@"SELECT [StudentId],[Test] FROM [dbo].[Student] WHERE Hash = '{si.Hash}'"; 
            var sqlResult = SQLHelper.RunSqlQuery(sqlQuery).First();
            bool testStudent = sqlResult[1] != System.DBNull.Value && (bool)sqlResult[1];
            int studentId = Convert.ToInt32(sqlResult[0]);

            String sqlQuery1 = $@"SELECT ModuleObjectiveId, MO.Description, M.Description
                                      FROM ModuleModuleObjective MMO 
                                      INNER JOIN ModuleObjective MO
                                      ON MMO.ModuleObjectiveId = MO.Id
                                      INNER JOIN Module M
                                      ON M.Id = {si.ModuleId}
                                      WHERE MO.Active = 1 AND ModuleId = {si.ModuleId};";

            var sqlResult1 = SQLHelper.RunSqlQuery(sqlQuery1);

            IList<ModuleObjectiveInfo> moList = new List<ModuleObjectiveInfo>();

            foreach (List<object> mo in sqlResult1)
            {
                string activityInformationQuery = $@"exec ActivityInformation1 {testStudent}, {studentId}, {si.CourseInstanceId}, {Convert.ToInt32(mo[0])}";
                var sqlResult2 = SQLHelper.RunSqlQuery(activityInformationQuery);

                ModuleObjectiveInfo moi = new ModuleObjectiveInfo()
                {
                    Id = Convert.ToInt32(mo[0]),
                    Description = (string) mo[1],
                    Materials = RetrieveActivities(sqlResult2, "Material"),
                    Quizzes = RetrieveActivities(sqlResult2, "Quiz"),
                    Assessments = RetrieveActivities(sqlResult2, "Assessment"),
                    Polls = RetrieveActivities(sqlResult2, "Poll"),
                    Discussions = RetrieveActivities(sqlResult2, "Discussion")
                };
                moList.Add(moi);
            }

            ModuleInfo mi = new ModuleInfo()
            {
                Description = (string) sqlResult1[0][2],
                StudentId = studentId,
                ModuleObjectives = (List<ModuleObjectiveInfo>)moList
            };

            return Ok(mi);
        }
        private static List<ResultInfo> RetrieveActivities(List<List<object>> allActivities, string activityType)
        {
            IList<ResultInfo> activities = new List<ResultInfo>();

            foreach (var activity in allActivities)
            {
                if (activity[0].ToString() == activityType)
                {
                    activities.Add(new ResultInfo()
                    {
                        ActivityId = (int)activity[3],
                        Title = (string)activity[4],
                        Completion = (int)activity[5],
                        Correct = (string)activity[7],
                        Grade = (int)activity[6],
                        Revealed = (string)activity[8],
                        DueDate = activity[9] != System.DBNull.Value ? (DateTime?)activity[9] : null
                    });
                }
            }

            return (List<ResultInfo>)activities;
        }

        public class ResultInfo
        {
            public int ActivityId { get; set; }
            public string Title { get; set; }
            public Nullable<int> Completion { get; set; }
            public Nullable<int> Grade { get; set; }
            public string StudentRank { get; set; }
            public string Correct { get; set; }
            public string Revealed { get; set; }
            public string OtherStudentsGrades { get; set; }
            public DateTime? DueDate { get; internal set; }
        }

        public class ModuleObjectiveInfo
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public List<ResultInfo> Materials { get; set; }
            public List<ResultInfo> Quizzes { get; set; }
            public List<ResultInfo> Assessments { get; set; }
            public List<ResultInfo> Polls { get; set; }
            public List<ResultInfo> Discussions { get; set; }
        }

        public class ModuleInfo
        {
            public string Description { get; set; }
            public int StudentId { get; set; }
            public List<ModuleObjectiveInfo> ModuleObjectives { get; set; }
        }

    }
}


