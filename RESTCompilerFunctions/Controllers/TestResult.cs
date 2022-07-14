using System.Collections.Generic;

namespace CompilerFunctions
{
    public struct TestResult
    {
        public string Input;
        public string Expected;
        public string Actual;
        public IEnumerable<string> ActualErrors;
        public string Result;
        public bool Passed;
    }
}

