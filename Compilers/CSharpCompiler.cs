using System;
using System.IO;
using System.Text.RegularExpressions;
using LMS.Common.Infos;

namespace Compilers
{
    public class CSharpCompiler : CloudCompiler
    {
        internal CSharpCompiler(string folder) : base(folder)
        { }

        public override ExecutionResult Run(RunInfo runInfo, CompilerInfo compilerInfo)
        {
            ExecutionResult er = new ExecutionResult();

            string tempFolder = $@"{CurrentFolder}\Temp";
            string testToolDirectory = $@"{CurrentFolder}\Binaries\{compilerInfo.TestToolDirectory}";
            string sourceFile = $@"{tempFolder}\{runInfo.StudentId}.{compilerInfo.SourceExtension}";

            string compiler = $@"{CurrentFolder}\Binaries\{compilerInfo.CompilerDirectory}\{compilerInfo.Compiler}";
            string executable = $@"{tempFolder}\{runInfo.StudentId}.{compilerInfo.OutputExtension}";

            string code = runInfo.Before +
                Environment.NewLine + Environment.NewLine +
                runInfo.Code +
                Environment.NewLine + Environment.NewLine +
                runInfo.Solution +
                Environment.NewLine + Environment.NewLine +
                runInfo.Test +
                Environment.NewLine + Environment.NewLine +
                runInfo.After;

            er.allCode = code;

            File.WriteAllText(sourceFile, code);

            //compiling
            string arguments = $@"{sourceFile} -target:library /out:{executable} " +
                                    $@"/r:mscorlib.dll " +
                                    $@"/r:{CurrentFolder}\Binaries\BuildTools\Common7\IDE\Extensions\TestPlatform\Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll " +
                                    $@"-nologo";

            er.Compiled = ProcessHelper.RunProcess(tempFolder, compiler, arguments, Timeout, out string compilationOutput, out string compilationErrors);

            if (!er.Compiled || !string.IsNullOrEmpty(compilationErrors))
            {
                er.Compiled = false;
                er.Message.Add(compilationErrors);
                er.ActualErrors.Add(compilationOutput);
            }
            else if (!string.IsNullOrEmpty(compilationOutput))
            {
                er.Compiled = false;
                er.Message.Add(compilationOutput);
                er.ActualErrors.Add(compilationOutput);
            }
            else
            {
                //executing 
                er.Succeeded = ProcessHelper.RunProcess(testToolDirectory, $@"{testToolDirectory}\vstest.console.exe", executable, Timeout, out string executionOutput, out string executionErrors);
                er.Output = executionOutput;
                if (!er.Succeeded || !string.IsNullOrEmpty(executionErrors))
                {
                    er.Succeeded = false;
                    er.Message.Add(executionErrors);
                }
            }

            if (er.Compiled)
            {
                //The error messages in the output are processed
                er = GetMessagesFromOutput(er);
            }

            er.Grade = CalculateGrade(er.Output, runInfo.ExpectedResult, false, er.ActualErrors);
            return er;
        }
        public override ExecutionResult GetMessagesFromOutput(ExecutionResult er)
        {
            if (er == null)
            {
                return er;
            }

            string new_output = "";
            //Search for substrings matching the defined pattern
            string[] split = Regex.Split(er.Output, @"(\#S{.+})|(\#E{.+})|(Passed [a-zA-Z0-9]*)|(Correctas [a-zA-Z0-9]*)");

            //Reconstructing output by replacing characters used in the metalanguage
            for (int i = 0; i < split.Length; i++)
            {
                string substring = split[i];

                if (Regex.IsMatch(substring, @"(\#S{.+})|(\#E{.+})|(Passed [a-zA-Z0-9]*)|(Correctas [a-zA-Z0-9]*)"))
                {
                    substring = Regex.Replace(substring, @"(\#S{)|(\#E{)|(})", "");
                    er.TestCodeMessages.Add(substring);
                }

                new_output += substring;
            }

            er.Output = new_output;
            return er;
        }
    }
}
