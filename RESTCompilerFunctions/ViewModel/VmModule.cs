using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RESTCompilerFunctions.ViewModel
{
    public class VmModule
    {
        public string Description { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public bool Active { get; set; }
        public int Id { get; set; }
        public string Location { get; set; }
    }
}