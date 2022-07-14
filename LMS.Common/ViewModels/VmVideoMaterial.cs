using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LMS.Common.ViewModels
{
    public class VmVideoMaterial
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public List<VmVideoMaterialStep> VideoMaterialSteps { get; set; }
    }
}