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
    
    public partial class SupportTicket
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SupportTicket()
        {
            this.SupportTicketMessages = new HashSet<SupportTicketMessage>();
        }
    
        public int Id { get; set; }
        public int TokenNo { get; set; }
        public int StudentId { get; set; }
        public string Title { get; set; }
        public string Priority { get; set; }
        public System.DateTime OpenedDate { get; set; }
        public bool OpenStatus { get; set; }
        public int CourseInstanceId { get; set; }
    
        public virtual CourseInstance CourseInstance { get; set; }
        public virtual Student Student { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupportTicketMessage> SupportTicketMessages { get; set; }
    }
}
