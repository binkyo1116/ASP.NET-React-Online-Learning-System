using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Common.ViewModels
{
    public class VmCourseCorequisite
    {
        public int CourseId { get; set; }
        public int CorequisiteCourseId { get; set; }
        public bool Active { get; set; }
        public int GroupId { get; set; }
        public string Type { get; set; }
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string TypeCourseName { get; set; }

    }
}
