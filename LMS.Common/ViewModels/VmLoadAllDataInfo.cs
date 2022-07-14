using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Common.ViewModels
{
    public class VmLoadAllDataInfo
    {
        public VmStudent StudentInfo { get; set; }
        public VmCodingProblem CodingProblem { get; set; }
        public string codingProblemInstanceInstructions { get; set; }
        public string codingProblemInstanceSolution { get; set; }
        public string  codingProblemInstanceTestCode { get; set; }
        public string  codingProblemInstanceExpectedOutput { get; set; }
        public string codingProblemInstanceBefore { get; set; }
        public string  codingProblemInstanceAfter { get; set; }
        public string  codingProblemInstanceScript { get; set; }
        public string  codingProblemInstanceTestCodeForStudent { get; set; }
    }
}