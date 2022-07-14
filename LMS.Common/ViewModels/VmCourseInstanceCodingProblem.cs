using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Common.ViewModels
{
    public class VmCourseInstanceCodingProblem
    {
        public int CourseInstanceId { get; set; }
        public int ModuleObjectiveId { get; set; }
        public int CodingProblemId { get; set; }
        public int MaxGrade { get; set; }
        public bool Active { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public virtual VmCodingProblem CodingProblem { get; set; }
        public virtual VmCourseInstance CourseInstance { get; set; }
    }
}
