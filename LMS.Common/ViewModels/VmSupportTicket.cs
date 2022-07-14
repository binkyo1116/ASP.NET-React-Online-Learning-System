using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LMS.Common.ViewModels
{
    public class VmSupportTicket
    {
        public int Id { get; set; }
        public int TokenNo { get; set; }
        public int StudentId { get; set; }
        public string Title { get; set; }
        public string Priority { get; set; }
        public System.DateTime OpenedDate { get; set; }
        public bool OpenStatus { get; set; }
        public int CourseInstanceId { get; set; }
    }
}