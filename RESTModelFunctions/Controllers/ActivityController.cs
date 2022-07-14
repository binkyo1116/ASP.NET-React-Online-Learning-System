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
    public class ActivityController : ApiController
    {
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

        // POST api/Course
        public IHttpActionResult Post([FromBody] StudentInfo si)
        {
            //MaterialEntities model = new MaterialEntities();

            //Student student = model.Students.Where(stud => stud.Hash == si.Hash).FirstOrDefault();
            VmStudent student = VmModelHelper.GetStudentInfoByHash(si.Hash);

            ResultInfo ri = new ResultInfo();

            if (si.Type == "Assessment")
            {
                //CodingProblem codingProblem = model.CodingProblems.Where(cp => cp.Id == si.ActivityId).FirstOrDefault();
                VmCodingProblem codingProblem = GetCodingProblem(si.ActivityId);
                ri.Type = codingProblem.Type;
                ri.CodingProblemId = codingProblem.Id;

            }
            else if (si.Type == "Quiz")
            {
                //Activity activity = model.Activities.Where(a => a.Id == si.ActivityId).FirstOrDefault();
                VmActivity activity = GetActivity(si.ActivityId);
                ri.Type = activity.Type;
                ri.Id = activity.Id;
            }
            else if (si.Type == "Material")
            {
                //Material material = model.Materials.Where(a => a.Id == si.ActivityId).FirstOrDefault();

                VmMaterial material = GetMaterial(si.ActivityId); 

                //StudentMaterial studentMaterial = new StudentMaterial()
                //{
                //    CourseId = si.CourseId,
                //    CourseObjectiveId = si.CourseObjectiveId,
                //    ModuleId = si.ModuleId,
                //    ModuleObjectiveId = si.ModuleObjectiveId,
                //    MaterialId = si.ActivityId,
                //    StudentId = student.StudentId,
                //    Accessed = DateTime.Now
                //};

                //model.StudentMaterials.Add(studentMaterial);
                //model.SaveChanges();
                SaveStudentMaterial(si, student.StudentId);

                ri.Description = material.Description;

            }

            return Ok(ri);
        }

        
        private VmCodingProblem GetCodingProblem(int id)
        {
            string sqlQueryStudent = $@"select Id, Type
                                        from CodingProblem
                                        where Id = {id}"; // AND (Password = '{hashedPassword}' OR Password = '{password}');

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
            string sqlQueryStudent = $@"select Id, Type
                                        from Activity
                                        where Id = {id}"; // AND (Password = '{hashedPassword}' OR Password = '{password}');

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
            string sqlQueryStudent = $@"select Id, Title, Description
                                        from Material
                                        where Id = {id}"; // AND (Password = '{hashedPassword}' OR Password = '{password}');

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

        private bool SaveStudentMaterial(StudentInfo si,int studentId)
        {
            string sqlQueryStudent = $@"insert into StudentMaterial (CourseId, CourseObjectiveId, 
ModuleId,ModuleObjectiveId, MaterialId, StudentId, Accessed)
values ({si.CourseId},
                    {si.CourseObjectiveId},
                    {si.ModuleId},
                    {si.ModuleObjectiveId},
                    {si.ActivityId},
                    {studentId},
                   {DateTime.Now})";
            
            bool isSave = false;
            isSave = SQLHelper.RunSqlUpdate(sqlQueryStudent);


            


            return isSave;
        }

        public class ResultInfo
        {
            public string Type { get; set; }
            public string Description { get; set; }
            public int QuizId { get; set; }
            public int CodingProblemId { get; set; }
            public int Id { get; set; }
        }
    }
}
