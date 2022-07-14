using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Common.ViewModels
{
    public class VmVariableValue
    {
        public int idVariableValue { get; set; }
        public int CodingProblemId { get; set; }
        public string VarName { get; set; }
        public string possibleValues { get; set; }
    }
}