using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTCompilerFunctions.ViewModel
{
    public class VmStudentGradeable
    {
        public int StudentId { get; set; }
        public int Grade { get; set; }
        public int MaxGrade { get; set; }
        public int Id { get; set; }
        public Nullable<int> CodingProblemId { get; set; }
        public Nullable<int> CourseInstanceId { get; set; }

        //public virtual CodingProblem CodingProblem { get; set; }
        //public virtual Student Student { get; set; }
    }
}