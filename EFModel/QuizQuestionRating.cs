//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class QuizQuestionRating
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
    
        public virtual QuizQuestion QuizQuestion { get; set; }
        public virtual Student Student { get; set; }
    }
}
