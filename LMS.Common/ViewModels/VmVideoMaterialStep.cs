using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LMS.Common.ViewModels
{
    public class VmVideoMaterialStep
    {
        public int VideoMaterialId { get; set; }
        public int TimeStamp { get; set; }
        public string Action { get; set; }
        public int Xs1 { get; set; }
        public int Ys1 { get; set; }
        public int Xs2 { get; set; }
        public int Ys2 { get; set; }
        public int Xe1 { get; set; }
        public int Ye1 { get; set; }
        public int Xe2 { get; set; }
        public int Ye2 { get; set; }
        public string Text { get; set; }
        public int Id { get; set; }
        public string Style { get; set; }
    }
}