using System.Collections.Generic;

namespace LMS.Common.Infos
{
    public struct RunInfo
    {
        public string StudentId { get; set; }
        public string Script { get; set; }
        public string Code { get; set; }
        public string Test { get; set; }
        public string Before { get; set; }
        public string After { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string ParameterTypes { get; set; }
        public string Parameters { get; set; }
        public string ExpectedResult { get; set; }
        public IEnumerable<string> Dependencies { get; set; }
        public string Solution { get; set; }
        public string Language { get; set; }
        public string Path { get; set; }
    }
}
