using System.Collections.Generic;

namespace LMS.Common.Infos
{
    public class ExecutionResult
    {
        public bool Compiled { get; set; }
        public bool Succeeded { get; set; }
        public List<string> Message { get; } = new List<string>();
        public string Expected { get; set; }
        public string Output { get; set; }
        public List<string> ActualErrors { get; set; } = new List<string>();
        public int Grade { get; set; }
        public List<string> TestCodeMessages { get; set; } = new List<string>();
        public string allCode { get; set; }
    }
}
