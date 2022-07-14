using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Compilers;
using LMS.Common.Infos;
using LMS.Common.SharedFunctions;

namespace CompilerFunctionsTemp
{
    public class CompilerTemp
    {
        public static ExecutionResult Run(RunInfo runInfo, CompilerInfo compilerInfo, HashSet<(string varName, string varValue, string ocurrenceNumber)> instancesData, string studentId)
        {
            CloudCompiler compiler;
            compiler = CloudCompiler.GetCompiler(runInfo.Language, runInfo.Path);
            compiler.ReplaceFuntion = (value) => SharedFunctions.InitializeVariablesInString(value, instancesData, studentId);
            return compiler.Run(runInfo, compilerInfo);
        }
        public static ExecutionResult RunCodeForValidation(RunInfo runInfo, CompilerInfo compilerInfo)
        {
            CloudCompiler compiler;
            compiler = CloudCompiler.GetCompiler(runInfo.Language, runInfo.Path);
            return compiler.RunCodeForValidation(runInfo, compilerInfo);
        }
    }
}
