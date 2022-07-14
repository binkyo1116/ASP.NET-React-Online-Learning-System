using LMS.Common.Infos;
using LMSLibrary;
using System.Collections.Generic;

namespace CompilerFunctions
{
    public struct ResultInfo
    {
        public IEnumerable<TestResult> Tests;
        public string BestGrade;
        public bool PastDue;
        public bool MaxReached;
        public string DueDate;
        public string Now;
        public int TestPassed;
        public int TestCount;
        public int Attempts;
        public int Submissions;
        public ExecutionResult ExeResult;
        public GradeInfo GradeTable;
        public IEnumerable<CodeHint> CodeHints;
        public object KeywordCount;
        public string last;
    }
}

