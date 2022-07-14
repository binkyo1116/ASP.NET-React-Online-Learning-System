using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Common.ViewModels
{
    public class VmGradeWeight
    {
        public int CourseInstanceId { get; set; }
        public int Id { get; set; }
        public int ActivityWeight { get; set; }
        public int AssessmentWeight { get; set; }
        public int MaterialWeight { get; set; }
        public int DiscussionWeight { get; set; }
        public int PollWeight { get; set; }
        public int MidtermWeight { get; set; }
        public int FinalWeight { get; set; }
    }
}