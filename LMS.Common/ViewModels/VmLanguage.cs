using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Common.ViewModels
{
    public class VmLanguage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Keywords { get; set; }
        public string KeywordsOutput { get; set; }
        public string Comment { get; set; }
        public string CodeStart { get; set; }
        public string CodeEnd { get; set; }
        public string CompilerDirectory { get; set; }
        public string SourceExtension { get; set; }
        public string Compiler { get; set; }
        public string OutputExtension { get; set; }
        public string CompilationParameters { get; set; }
        public string TestToolDirectory { get; set; }
        public string TestToolExe { get; set; }
        public string OutParameters { get; set; }
    }
}