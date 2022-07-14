using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTCompilerFunctions.ViewModel
{
    public class VmActivity
    {
        public VmActivity()
        {
            
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int MaxGrade { get; set; }
        public bool Active { get; set; }
        public int Role { get; set; }
    }
}