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
    
    public partial class InstructorAvailableHour
    {
        public int Id { get; set; }
        public int InstructorId { get; set; }
        public int CourseInstanceId { get; set; }
        public System.TimeSpan StartTime { get; set; }
        public System.TimeSpan EndTime { get; set; }
        public string DayOfWeek { get; set; }
    
        public virtual CourseInstance CourseInstance { get; set; }
        public virtual Instructor Instructor { get; set; }
    }
}
