using LMS.Common.ViewModels;
using LMSLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeErrorsHintsController : ControllerBase
    {
        [Route("GetCourses")]
        [HttpPost]
        public ActionResult<IEnumerable<VmCourse>> GetCourses()
        {
            IEnumerable<VmCourse> courses = GetCourse();
            return Ok(courses);
        }

        [Route("GetErrors")]
        [HttpPost]
        public ActionResult<IEnumerable<SanitizedError>> Get([FromBody] CodeErrorsHintsIncomingInfo ici)
        {
            IEnumerable<SanitizedError> errors = GetSubmissionCodeError(ici.CourseId);
            return Ok(errors);
        }

        [Route("GetHints")]
        [HttpPost]
        public ActionResult<IEnumerable<CodeHint>> GetHints([FromBody] CodeErrorsHintsIncomingInfo ici)
        {
            IEnumerable<CodeHint> hints = GetCodeHintError(ici.ErrorId);
            return Ok(hints);
        }

        [Route("UpdateHint")]
        [HttpPost]
        public ActionResult<CodeHint> UpdateHint([FromBody] CodeErrorsHintsIncomingInfo ici)
        {
            CodeHint hint = null;
            if (ici.HintId == -1)
            {
                hint = InsertCodeHint(ici.UpdatedHint, ici.ErrorId);
            }
            //editing existing hint
            else
            {
                UpdateCodeHint(ici.UpdatedHint, ici.HintId);
                hint = new CodeHint()
                {
                    Hint = ici.UpdatedHint,
                    Id = ici.HintId
                };
            }
            return Ok(hint);
        }

        [Route("GetSubmissions")]
        [HttpPost]
        public IActionResult GetSubmissions()
        {
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
    }
    public class CodeErrorsHintsIncomingInfo
    {
        public string test;
        public int CourseId;
        public int HintId;
        public int ErrorId;
        public string UpdatedHint;

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
