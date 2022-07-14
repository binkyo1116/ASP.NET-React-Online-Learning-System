using System.Linq;
using System.Xml.Linq;
using LMS.Common.Infos;

namespace Compilers
{
    public class TableauCompiler : CloudCompiler
    {
        internal TableauCompiler(string folder) : base(folder) { }

        public override ExecutionResult Run(RunInfo runInfo, CompilerInfo compilerInfo)
        {
            ExecutionResult er = new ExecutionResult
            {
                Grade = 0
            };


            if (!string.IsNullOrEmpty(runInfo.Code))
            {
                if (!string.IsNullOrEmpty(runInfo.ExpectedResult))
                {
                    XElement xml = XElement.Parse(runInfo.Code).Descendants("worksheets").FirstOrDefault();
                    er.Compiled = true;
                    er.Succeeded = true;
                    er.Grade = CalculateGrade(xml.ToString(), runInfo.ExpectedResult, false, er.ActualErrors);
                }
            }
            return er;
        }
    }
}
