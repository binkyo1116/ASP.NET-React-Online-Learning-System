using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Common.ViewModels
{
    public class VmStudent
    {
        public int StudentId { get; set; }
        public Nullable<int> CanvasId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Nullable<int> Mark { get; set; }
        public byte[] Photo { get; set; }
        public Nullable<bool> Test { get; set; }
        public string Hash { get; set; }
        public string Password2 { get; set; }
    }
}