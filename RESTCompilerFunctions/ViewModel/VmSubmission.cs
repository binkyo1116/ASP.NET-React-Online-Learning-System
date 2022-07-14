using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTCompilerFunctions.ViewModel
{
    public class VmSubmission
    {
        public VmSubmission()
        {
            //this.SubmissionCodeErrors = new HashSet<SubmissionCodeError>();
        }

        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Code { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<int> Grade { get; set; }
        public string History { get; set; }
        public int CodingProblemId { get; set; }
        public Nullable<int> CourseInstanceId { get; set; }
        public string Comment { get; set; }
        public string Error { get; set; }

        //public virtual CodingProblem CodingProblem { get; set; }
        //public virtual Student Student { get; set; }
        //public virtual ICollection<SubmissionCodeError> SubmissionCodeErrors { get; set; }
    }
}