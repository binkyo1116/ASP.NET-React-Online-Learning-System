using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
//using EFModel;
using LMSLibrary;
using LMS.Common.ViewModels;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/CodeErrorsHints")]
    public class CodeErrorsHintsController : ApiController
    {
        public class IncomingInfo
        {
            public string test;
            public int CourseId;
            public int HintId;
            public int ErrorId;
            public string UpdatedHint;

        }

        // POST api/<controller>/GetCourses
        [Route("GetCourses")]
        [HttpPost]
        public IHttpActionResult GetCourses()
        {
            //MaterialEntities model = new MaterialEntities();

            /*
             * TODO: only retrieve active courses. There are inactive courses that are returned
             * **/

            IEnumerable<VmCourse> courses = GetCourse();//model.Courses.Select(c => new Course() { Id = c.Id, Name = c.Name });

            return Ok(courses);
        }

        // POST api/<controller>/GetErrors
        [Route("GetErrors")]
        [HttpPost]
        public IHttpActionResult Get([FromBody] IncomingInfo ici)
        {
            //MaterialEntities model = new MaterialEntities();

            //IEnumerable<SanitizedError> errors = model.SubmissionCodeErrors
            //                                .Where(sce => sce.Submission.CourseInstanceId == ici.CourseId)
            //                                .Select(sce => new SanitizedError
            //                                {
            //                                    Error = sce.CodeError.SanitizedCodeError.Error,
            //                                    Id = sce.CodeError.SanitizedErrorId,
            //                                }).Distinct();
            IEnumerable<SanitizedError> errors = GetSubmissionCodeError(ici.CourseId);
            return Ok(errors);
        }

        // POST api/<controller>/GetHints
        [Route("GetHints")]
        [HttpPost]
        public IHttpActionResult GetHints([FromBody] IncomingInfo ici)
        {
            //MaterialEntities model = new MaterialEntities();

            //IEnumerable<CodeHint> hints = model.SanitizedCodeErrorCodeHints
            //                                .Where(sce => sce.SanitizedCodeErrorId == ici.ErrorId)
            //                                .Select(sanitizedCodeErrorCodeHint => new CodeHint
            //                                {
            //                                    Hint = sanitizedCodeErrorCodeHint.CodeHint.Hint,
            //                                    Id = sanitizedCodeErrorCodeHint.CodeHintId
            //                                });
            IEnumerable<CodeHint> hints = GetCodeHintError(ici.ErrorId);
            return Ok(hints);
        }

        [Route("UpdateHint")]
        [HttpPost]
        public IHttpActionResult UpdateHint([FromBody] IncomingInfo ici)
        {
            //MaterialEntities model = new MaterialEntities();


            //EFModel.CodeHint codeHint;
            CodeHint hint = null;
            //new hint to add
            if (ici.HintId == -1)
            {
                //codeHint = new EFModel.CodeHint
                //{
                //    Hint = ici.UpdatedHint
                //};

                //model.CodeHints.Add(codeHint);

                //SanitizedCodeErrorCodeHint newCodeErrorCodeHint = new SanitizedCodeErrorCodeHint
                //{
                //    SanitizedCodeErrorId = ici.ErrorId,
                //    CodeHintId = codeHint.Id
                //};

                //model.SanitizedCodeErrorCodeHints.Add(newCodeErrorCodeHint);

                hint = InsertCodeHint(ici.UpdatedHint, ici.ErrorId);


            }
            //editing existing hint
            else
            {
                UpdateCodeHint(ici.UpdatedHint, ici.HintId);
                //codeHint = model.CodeHints.Where(ch => ch.Id == ici.HintId).FirstOrDefault();
                //codeHint.Hint = ici.UpdatedHint;
                hint = new CodeHint()
                {
                    Hint = ici.UpdatedHint,
                    Id = ici.HintId
                };
            }

            //model.SaveChanges();

            //Return updated hint to request
            //CodeHint hint = new CodeHint
            //{
            //    Hint = codeHint.Hint,
            //    Id = codeHint.Id
            //};

            return Ok(hint);
        }


        // POST api/<controller>/GetSubmissions
        [Route("GetSubmissions")]
        [HttpPost]
        public IHttpActionResult GetSubmissions()
        {
            //MaterialEntities model = new MaterialEntities();

            //CodeError CodeError = model.SanitizedCodeErrors.Where(ce => ce.Id == 3).Select(sce => sce.CodeErrors.FirstOrDefault()).FirstOrDefault();
            //string submissionCode = model.SubmissionCodeErrors.Where(ce => CodeError.Id == ce.CodeErrorId).OrderByDescending(sce => sce.Id).Select(sce => sce.Submission.Code).FirstOrDefault();

            VmCodeError CodeError = GetCodeError();
            string submissionCode = GetSubmissionError(CodeError.Id);

            return Ok();
        }

        private List<VmCourse> GetCourse()
        {
            string sqlQueryStudent = $@"select Id, Name from Course";

            var courseData = SQLHelper.RunSqlQuery(sqlQueryStudent);
            List<VmCourse> courseList = new List<VmCourse>();


            foreach (var item in courseData)
            {
                VmCourse course = null;

                List<object> st = item;

                course = new VmCourse
                {
                    Id = (int)st[0],
                    Name = st[1].ToString()

                };

                courseList.Add(course);

            }


            return courseList;
        }
        private List<SanitizedError> GetSubmissionCodeError(int courseId)
        {
            string sqlQueryStudent = $@"select Distinct
scr.Id,scr.Error
from SanitizedCodeError scr 
inner join CodeError cr on cr.SanitizedErrorId = scr.Id
inner join SubmissionCodeError sce on sce.CodeErrorId = cr.Id
inner join Submission s on sce.SubmissionId = s.Id
where s.CourseInstanceId = {courseId}";

            var sanitizedErrorData = SQLHelper.RunSqlQuery(sqlQueryStudent);
            List<SanitizedError> sanitizedErrorList = new List<SanitizedError>();


            foreach (var item in sanitizedErrorData)
            {
                SanitizedError sanitizedError = null;

                List<object> st = item;

                sanitizedError = new SanitizedError
                {
                    Id = (int)st[0],
                    Error = st[1].ToString()

                };

                sanitizedErrorList.Add(sanitizedError);

            }


            return sanitizedErrorList;
        }

        private List<CodeHint> GetCodeHintError(int ErrorId)
        {
            string sqlQueryStudent = $@"select ch.Hint,sc.CodeHintId
                                        from SanitizedCodeErrorCodeHint sc
                                        inner join CodeHint ch on sc.CodeHintId = ch.Id
                                        where sc.SanitizedCodeErrorId =  {ErrorId}";

            var codeHintData = SQLHelper.RunSqlQuery(sqlQueryStudent);
            List<CodeHint> codeHintList = new List<CodeHint>();


            foreach (var item in codeHintData)
            {
                CodeHint codeHint = null;

                List<object> st = item;

                codeHint = new CodeHint
                {
                    Hint = st[0].ToString(),
                    Id = (int)st[1]

                };

                codeHintList.Add(codeHint);

            }


            return codeHintList;
        }
        private VmCodeError GetCodeError()
        {
            string sqlQueryQuarter = $@"select top 1 ce.* from SanitizedCodeError as sc
inner join CodeError ce on sc.Id = ce.SanitizedErrorId
where sc.Id = 3";

            var codeHintData = SQLHelper.RunSqlQuery(sqlQueryQuarter);
            VmCodeError vmCodeError = null;


            if (codeHintData.Count > 0)
            {

                List<object> st = codeHintData[0];

                vmCodeError = new VmCodeError
                {
                    Id = (int)st[0],
                    Error = st[1].ToString(),
                    SanitizedErrorId = (int)st[2]

                };

            }
            return vmCodeError;
        }
        private string GetSubmissionError(int errorId)
        {
            string sqlQueryQuarter = $@"select top 1 s.Code from SubmissionCodeError as sc
inner join Submission as s on sc.SubmissionId = s.Id
where sc.CodeErrorId = {errorId}
order by sc.Id desc";

            var codeHintData = SQLHelper.RunSqlQuery(sqlQueryQuarter);
            string error = null;

            if (codeHintData.Count > 0)
            {
                List<object> st = codeHintData[0];
                error = st[0].ToString();     
            }
            return error;
        }

        private CodeHint InsertCodeHint(string hint, int errorId)
        {
            string sqlQueryQuarter = $@"exec CreateHint '{hint}' , {errorId}";

            var codeHintData = SQLHelper.RunSqlQuery(sqlQueryQuarter);
            CodeHint codeHint = null;


            if (codeHintData.Count > 0)
            {

                List<object> st = codeHintData[0];

                codeHint = new CodeHint
                {
                    Id = (int)st[0],
                    Hint = st[1].ToString()
                };

            }
            return codeHint;
        }

        private void UpdateCodeHint(string hint, int hintId)
        {
            string sqlQueryQuarter = $@"update CodeHint set Hint = '{hint}' where Id = {hintId}";

            SQLHelper.RunSqlUpdate(sqlQueryQuarter);
        }

        public class Course
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class SanitizedError
        {
            public string Error { get; set; }
            public int Id { get; set; }
            public IEnumerable<CodeHint> Hints { get; set; }

        }

        public class CodeHint
        {
            public string Hint { get; set; }
            public int Id { get; set; }
        }
    }
}