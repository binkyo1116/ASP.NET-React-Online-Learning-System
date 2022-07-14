using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RESTCompilerFunctions.ViewModel
{
    public class VmGroupDiscussion
    {
        public Nullable<int> x_CourseId { get; set; }
        public Nullable<int> x_CourseObjectiveId { get; set; }
        public Nullable<int> x_ModuleId { get; set; }
        public Nullable<int> x_ModuleObjetiveId { get; set; }
        public Nullable<int> x_DiscussionBoardId { get; set; }
        public Nullable<int> x_GroupDiscussionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public System.DateTime PublishedDate { get; set; }
        public int StudentId { get; set; }
        public System.DateTime LastUpdateDate { get; set; }
        public bool Active { get; set; }
        public int Id { get; set; }
        public int DiscussionBoardId { get; set; }
        public int CourseInstanceId { get; set; }
    }
}