using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTCompilerFunctions.ViewModel
{
    public class VmCodingProblem
    {
        public VmCodingProblem()
        {
            //this.CodingProblemHints = new HashSet<CodingProblemHint>();
            //this.CourseInstanceCodingProblems = new HashSet<CourseInstanceCodingProblem>();
            //this.StudentGradables = new HashSet<StudentGradable>();
            //this.Submissions = new HashSet<Submission>();
            //this.Tests = new HashSet<Test>();
            this.VariableValues = new HashSet<VmVariableValue>();
            //this.CodingProblemInstances = new HashSet<VmCodingProblemInstance>();
        }

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

        //public virtual ICollection<CodingProblemHint> CodingProblemHints { get; set; }
        //public virtual ICollection<CourseInstanceCodingProblem> CourseInstanceCodingProblems { get; set; }
        //public virtual ICollection<StudentGradable> StudentGradables { get; set; }
        //public virtual ICollection<Submission> Submissions { get; set; }
        //public virtual ICollection<Test> Tests { get; set; }
        public virtual ICollection<VmVariableValue> VariableValues { get; set; }
        //public virtual ICollection<VmCodingProblemInstance> CodingProblemInstances { get; set; }
    }
}