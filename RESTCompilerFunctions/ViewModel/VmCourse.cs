using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTCompilerFunctions.ViewModel
{
    public class VmCourse
    {
        public VmCourse()
        {
            //this.ErrorParsings = new HashSet<ErrorParsing>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}