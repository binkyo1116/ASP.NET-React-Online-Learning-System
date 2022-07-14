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
    public class ModuleProgressController : ApiController
    {
        public class StudentInfo
        {
            public string Hash { get; set; }
            public int CourseId { get; set; }
            public int CourseObjectiveId { get; set; }
        }

        // POST api/Course
        public IHttpActionResult Post([FromBody] StudentInfo si)
        {
            return Ok();
        }
        //private static Gradebook GetGradebook(MaterialEntities model, Student student, Module module, Course course)
        //{
        //    return new Gradebook();
        //}
        private static Gradebook GetGradebook(VmStudent student, VmModule module, VmCourse course)
        {
            return new Gradebook();
        }

        private static double GetGPAByPercent(VmCourse course, double percent)
        {
            //GradeScale maxPossibleGPAElement = course.GradeScaleGroup.GradeScales.Where(gs => gs.MinNumberInPercent <= percent).OrderByDescending(gs => gs.MinNumberInPercent).First();
            //return maxPossibleGPAElement.GPA;
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
            //GradeScale maxPossibleGPAElement = cgs.GradeScaleGroup.GradeScales.Where(gs => gs.MinNumberInPercent <= percent).OrderByDescending(gs => gs.MinNumberInPercent).First();
            return maxPossibleGPAElement.GPA;
        }

        public class ResultInfo
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
}

