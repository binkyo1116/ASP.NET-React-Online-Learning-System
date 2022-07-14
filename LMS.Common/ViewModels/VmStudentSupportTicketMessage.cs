using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LMS.Common.ViewModels
{
    public class VmStudentSupportTicketMessage
    {
        public string Priority { get; set; }
        public bool OpenStatus { get; set; }
        public string Title { get; set; }
        public int TokenNo { get; set; }
        public int Id { get; set; }
        public string Message { get; set; }
        public bool ViewStatus { get; set; }
        public byte[] Image { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public int StudentId { get; set; }
    }
}