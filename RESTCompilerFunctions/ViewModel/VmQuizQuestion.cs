using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RESTCompilerFunctions.ViewModel
{
    public class VmQuizQuestion
    {
        public string Prompt1 { get; set; }
        public string Prompt2 { get; set; }
        public string Answer { get; set; }
        public string Source { get; set; }
        public int MaxGrade { get; set; }
        public bool CaseSensitive { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string Type { get; set; }
        public int VideoTimestamp { get; set; }
        public string VideoSource { get; set; }
        public bool EmbedAction { get; set; }
        public int Id { get; set; }
        public int ActivityId1 { get; set; }
        public Nullable<int> ElementStyleId { get; set; }
        public string Images { get; set; }
        public Nullable<int> UsesHint { get; set; }
    }
}