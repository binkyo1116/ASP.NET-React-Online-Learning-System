using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Common.ViewModels
{
    public class VmCodingProblem
    {
        public string Instructions { get; set; }
        public string Script { get; set; }
        public string Solution { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string ParameterTypes { get; set; }
        public string Language { get; set; }
        public string TestCaseClass { get; set; }
        public string Before { get; set; }
        public string After { get; set; }
        public int MaxGrade { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int Attempts { get; set; }
        public bool Active { get; set; }
        public int Role { get; set; }
        public int Id { get; set; }
        public string ExpectedOutput { get; set; }
        public string Parameters { get; set; }
        public string TestCode { get; set; }
        public string TestCodeForStudent { get; set; }

        public virtual ICollection<VmVariableValue> VariableValues { get; set; } = new HashSet<VmVariableValue>();
    }
}