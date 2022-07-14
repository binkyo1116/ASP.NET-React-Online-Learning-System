using LMS.Common.ViewModels;
using LMSLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleProgressController : ControllerBase
    {
        [HttpPost]
        public ActionResult<ModuleProgressResultInfo> Post([FromBody] ModuleProgressStudentInfo si)
        {
            return Ok();
        }
        private static Gradebook GetGradebook(VmStudent student, VmModule module, VmCourse course)
        {
            return new Gradebook();
        }
        private static double GetGPAByPercent(VmCourse course, double percent)
        {
            double maxPossibleGPAElement = GetGPAByPercent(percent, course);
            return maxPossibleGPAElement;
        }
        private static double GetGPAByPercent(double percent, VmCourse cgs)
        {
            if (cgs == null)
            {
                return 0;
            }
            string sqlQueryGradeScale = $@"select top 1 gs.Id, gs.GradeScaleGroupId,gs.MaxNumberInPercent,gs.MinNumberInPercent,gs.GPA from GradeScale gs
                                        where gs.MinNumberInPercent <= {percent} 
                                        order by gs.MinNumberInPercent desc";

            var gradeScaleData = SQLHelper.RunSqlQuery(sqlQueryGradeScale);
            VmGradeScale maxPossibleGPAElement = null;
            if (gradeScaleData.Count > 0)
            {
                List<object> st = gradeScaleData[0];
                maxPossibleGPAElement = new VmGradeScale
                {
                    Id = (int)st[0],
                    GradeScaleGroupId = (int)st[1],
                    MaxNumberInPercent = (double)st[2],
                    MinNumberInPercent = (double)st[3],
                    GPA = (double)st[4]
                };

            }
            return maxPossibleGPAElement.GPA;
        }
    }
    public class ModuleProgressStudentInfo
    {
        public string Hash { get; set; }
        public int CourseId { get; set; }
        public int CourseObjectiveId { get; set; }
    }
    public class ModuleProgressResultInfo
    {
        public int ModuleId { get; set; }
        public string ModuleObjectives { get; set; }
        public int CourseObjectiveId { get; set; }
        public int Percent { get; set; }
        public int Completion { get; set; }
        public string Description { get; set; }
        public double GPA { get; set; }
        public string DueDate { get; set; }
        public string StrokeDasharray { get; set; }
    }
}
