using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTCompilerFunctions.ViewModel
{
    public class VmAnnouncement
    {
        public VmAnnouncement()
        {
            //this.ErrorParsings = new HashSet<ErrorParsing>();
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public System.DateTime PublishedDate { get; set; }
        public int StudentId { get; set; }
        public System.DateTime LastUpdateDate { get; set; }
        public bool Active { get; set; }
        public int Id { get; set; }
        public int CourseInstanceId { get; set; }
    }
}