using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Common.ViewModels
{
    public class VmSubmission
    {
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
    }
}