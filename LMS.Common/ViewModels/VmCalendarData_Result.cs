using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LMS.Common.ViewModels
{
    public class VmCalendarData_Result
    {
        public int CourseInstanceId { get; set; }
        public string CourseName { get; set; }
        public string ActivityType { get; set; }
        public int ActivityId { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<int> Completion { get; set; }
        public int ModuleObjectiveId { get; set; }
    }
}