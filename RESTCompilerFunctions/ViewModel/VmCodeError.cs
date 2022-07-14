using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RESTCompilerFunctions.ViewModel
{
    public class VmCodeError
    {
        public int Id { get; set; }
        public string Error { get; set; }
        public int SanitizedErrorId { get; set; }
    }
}