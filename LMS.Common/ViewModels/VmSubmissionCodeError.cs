using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Common.ViewModels
{
    public class VmSubmissionCodeError
    {
        public int Id { get; set; }
        public int CodeErrorId { get; set; }
        public int SubmissionId { get; set; }

        public virtual VmCodeError CodeError { get; set; }
        public virtual VmSubmission Submission { get; set; }
    }
}
