using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTCompilerFunctions.ViewModel
{
    public class VmVariableValue
    {
        public VmVariableValue()
        {
            //this.CodingProblemInstances = new HashSet<CodingProblemInstance>();
        }

        public int idVariableValue { get; set; }
        public int CodingProblemId { get; set; }
        public string VarName { get; set; }
        public string possibleValues { get; set; }

        //public virtual CodingProblem CodingProblem { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<CodingProblemInstance> CodingProblemInstances { get; set; }
    }
}