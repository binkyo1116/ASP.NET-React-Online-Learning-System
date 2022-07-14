using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LMS.Common.ViewModels
{
    public class VmQuizQuestionElementStyle
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public string FontFamily { get; set; }
        public string BackgroundColor { get; set; }
        public string FontSize { get; set; }
        public string Border { get; set; }
        public string PaddingLeft { get; set; }
        public string PaddingRight { get; set; }
    }
}