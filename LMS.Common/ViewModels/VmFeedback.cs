using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Common.ViewModels
{
    public class VmFeedback
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public string Text { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public int CourseInstanceId { get; set; }
    }
}