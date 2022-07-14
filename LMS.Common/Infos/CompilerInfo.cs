namespace LMS.Common.Infos
{
    public struct CompilerInfo
    {
        public string Comment { get; set; }
        public string CodeStart { get; set; }
        public string CodeEnd { get; set; }
        public string CompilerDirectory { get; set; }
        public string SourceExtension { get; set; }
        public string Compiler { get; set; }
        public string OutputExtension { get; set; }
        public string TestToolDirectory { get; set; }
        public string TestToolExe { get; set; }
        public string CompilationParameters { get; set; }
        public string OutParameters { get; set; }
    }
}
