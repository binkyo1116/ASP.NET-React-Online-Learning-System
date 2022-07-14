using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LMS.Common.ViewModels
{
    public class VmQuizQuestionRating
    {
        public int CourseId { get; set; }
        public int CourseObjectiveId { get; set; }
        public int ModuleId { get; set; }
        public int ModuleObjectiveId { get; set; }
        public int ActivityId { get; set; }
        public int QuizId { get; set; }
        public int QuestionId { get; set; }
        public int StudentId { get; set; }
        public int Rating { get; set; }
        public System.DateTime Timestamp { get; set; }
        public int Id { get; set; }
        public int QuestionId1 { get; set; }
    }
}