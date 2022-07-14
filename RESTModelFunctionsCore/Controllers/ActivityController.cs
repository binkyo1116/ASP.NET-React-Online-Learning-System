using LMS.Common.ViewModels;
using LMSLibrary;
using Microsoft.AspNetCore.Mvc;
using RESTModelFunctionsCore.Helper;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : Controller
    {
        [HttpPost]
        public ActionResult<ActivityResultInfo> Post([FromBody] StudentInfo si)
        {
            VmStudent student = VmModelHelper.GetStudentInfoByHash(si.Hash);
            ActivityResultInfo ri = new ActivityResultInfo();
            if (si.Type == "Assessment")
            {
                VmCodingProblem codingProblem = GetCodingProblem(si.ActivityId);
                ri.Type = codingProblem.Type;
                ri.CodingProblemId = codingProblem.Id;

            }
            else if (si.Type == "Quiz")
            {
                VmActivity activity = GetActivity(si.ActivityId);
                ri.Type = activity.Type;
                ri.Id = activity.Id;
            }
            else if (si.Type == "Material")
            {
                VmMaterial material = GetMaterial(si.ActivityId);
                SaveStudentMaterial(si, student.StudentId);

                ri.Description = material.Description;
            }

            return Ok(ri);
        }
        private VmCodingProblem GetCodingProblem(int id)
        {
            string sqlQueryStudent = $@"select Id, Type
                                        from CodingProblem
                                        where Id = {id}";

            var codingProblemData = SQLHelper.RunSqlQuery(sqlQueryStudent);
            VmCodingProblem codingProblem = null;

            if (codingProblemData.Count > 0)
            {
                List<object> st = codingProblemData[0];
                codingProblem = new VmCodingProblem
                {
                    Id = (int)st[0],
                    Type = st[1].ToString()
                };
            }
            return codingProblem;
        }
        private VmActivity GetActivity(int id)
        {
            string sqlQueryStudent = $@"select Id, Type from Activity where Id = {id}";

            var activityData = SQLHelper.RunSqlQuery(sqlQueryStudent);
            VmActivity activity = null;
            if (activityData.Count > 0)
            {
                List<object> st = activityData[0];
                activity = new VmActivity
                {
                    Id = (int)st[0],
                    Type = st[1].ToString()
                };
            }
            return activity;
        }
        private VmMaterial GetMaterial(int id)
        {
            string sqlQueryStudent = $@"select Id, Title, Description from Material where Id = {id}";

            var materialData = SQLHelper.RunSqlQuery(sqlQueryStudent);
            VmMaterial material = null;
            if (materialData.Count > 0)
            {
                List<object> st = materialData[0];
                material = new VmMaterial
                {
                    Id = (int)st[0],
                    Title = st[1].ToString(),
                    Description = st[2].ToString()

                };
            }
            return material;
        }
        private bool SaveStudentMaterial(StudentInfo si, int studentId)
        {
            string sqlQueryStudent = $@"insert into StudentMaterial (CourseId, CourseObjectiveId, 
                                        ModuleId,ModuleObjectiveId, MaterialId, StudentId, Accessed)
                                        values ({si.CourseId},
                    {si.CourseObjectiveId},
                    {si.ModuleId},
                    {si.ModuleObjectiveId},
                    {si.ActivityId},
                    {studentId},
                   '{DateTime.Now}')";

            bool isSave = false;
            isSave = SQLHelper.RunSqlUpdate(sqlQueryStudent);
            return isSave;
        }
    }
    public class StudentInfo
    {
        public int CourseId { get; set; }
        public int CourseObjectiveId { get; set; }
        public int ModuleId { get; set; }
        public int ModuleObjectiveId { get; set; }
        public int ActivityId { get; set; }
        public string Hash { get; set; }
        public string Type { get; set; }
    }
    public class ActivityResultInfo
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int QuizId { get; set; }
        public int CodingProblemId { get; set; }
        public int Id { get; set; }
    }
}
