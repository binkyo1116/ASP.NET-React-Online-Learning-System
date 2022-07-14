using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LMS.Common.ViewModels
{
    public class VmStudentQuizQuestion
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Answer { get; set; }
        public string Expected { get; set; }
        public System.DateTime Date { get; set; }
        public bool AnswerShown { get; set; }
        public string History { get; set; }
        public int QuestionId { get; set; }
        public int Grade { get; set; }
    }
}