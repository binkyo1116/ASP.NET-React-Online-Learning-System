using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RESTCompilerFunctions.ViewModel
{
    public class VmDiscussionBoard
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public DiscussionBoard()
        //{
        //    this.CourseInstanceDiscussionBoards = new HashSet<CourseInstanceDiscussionBoard>();
        //    this.GroupDiscussions = new HashSet<GroupDiscussion>();
        //}

        public string Title { get; set; }
        public bool Active { get; set; }
        public int Id { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<CourseInstanceDiscussionBoard> CourseInstanceDiscussionBoards { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<GroupDiscussion> GroupDiscussions { get; set; }
    }
}