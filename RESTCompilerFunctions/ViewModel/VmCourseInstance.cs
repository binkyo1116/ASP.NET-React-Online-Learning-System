using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTCompilerFunctions.ViewModel
{
    public class VmCourseInstance
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VmCourseInstance()
        {
            //this.Announcements = new HashSet<Announcement>();
            //this.PollParticipantAnswers = new HashSet<PollParticipantAnswer>();
            //this.SupportTickets = new HashSet<SupportTicket>();
            //this.CourseInstanceActivities = new HashSet<CourseInstanceActivity>();
            //this.CourseInstanceCodingProblems = new HashSet<CourseInstanceCodingProblem>();
            //this.CourseInstanceDiscussionBoards = new HashSet<CourseInstanceDiscussionBoard>();
            //this.CourseInstanceMaterials = new HashSet<CourseInstanceMaterial>();
            //this.CourseInstancePollGroups = new HashSet<CourseInstancePollGroup>();
            //this.CourseInstanceSessions = new HashSet<CourseInstanceSession>();
            //this.GradeWeights = new HashSet<GradeWeight>();
            //this.GroupDiscussions = new HashSet<GroupDiscussion>();
            //this.InstructionMethods = new HashSet<InstructionMethod>();
            //this.InstructorCourses = new HashSet<InstructorCourse>();
            //this.StudentCourseAccesses = new HashSet<StudentCourseAccess>();
            //this.Students = new HashSet<Student>();
            //this.Feedbacks = new HashSet<Feedback>();
            //this.StudentCourseInstanceCompletions = new HashSet<StudentCourseInstanceCompletion>();
            //this.InstructorAvailableHours = new HashSet<InstructorAvailableHour>();
            //this.CourseInstructionMethods = new HashSet<CourseInstructionMethod>();
        }

        public int Id { get; set; }
        public bool Active { get; set; }
        public int QuarterId { get; set; }
        public int CourseId { get; set; }
        public bool Testing { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Announcement> Announcements { get; set; }
        //public virtual Course Course { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<PollParticipantAnswer> PollParticipantAnswers { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<SupportTicket> SupportTickets { get; set; }
        //public virtual Quarter Quarter { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<CourseInstanceActivity> CourseInstanceActivities { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<CourseInstanceCodingProblem> CourseInstanceCodingProblems { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<CourseInstanceDiscussionBoard> CourseInstanceDiscussionBoards { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<CourseInstanceMaterial> CourseInstanceMaterials { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<CourseInstancePollGroup> CourseInstancePollGroups { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<CourseInstanceSession> CourseInstanceSessions { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<GradeWeight> GradeWeights { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<GroupDiscussion> GroupDiscussions { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<InstructionMethod> InstructionMethods { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<InstructorCourse> InstructorCourses { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<StudentCourseAccess> StudentCourseAccesses { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Student> Students { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Feedback> Feedbacks { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<StudentCourseInstanceCompletion> StudentCourseInstanceCompletions { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<InstructorAvailableHour> InstructorAvailableHours { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<CourseInstructionMethod> CourseInstructionMethods { get; set; }
    }
}