using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Common.ViewModels
{
    public class VmGradingPolicy
    {
        public int Id { get; set; }
        public int SchoolId { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}
