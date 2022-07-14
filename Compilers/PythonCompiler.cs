using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LMS.Common.Infos;

namespace Compilers
{
    public class PythonCompiler : CloudCompiler
    {
        protected override int Timeout { get; } = 480;
        internal PythonCompiler(string folder) : base(folder) { }
        public override ExecutionResult Run(RunInfo runInfo, CompilerInfo compilerInfo)
        {
            ExecutionResult result = new ExecutionResult();

            List<string> scriptDiff = GetDiffScript(runInfo.Script, runInfo.Code);

            if (scriptDiff.Count > 0)
            {
                result.Compiled = false;
                result.Message.Add("Your solution modifies the original script. Please write your solution so that it only writes code in the specified places");
                result.Message.AddRange(scriptDiff);
                result.Grade = 0;
                return result;
            }

            List<string> blockedInput = GetOutOfBlocks(runInfo.Script, runInfo.Code, compilerInfo.CodeStart, compilerInfo.CodeEnd);

            if (blockedInput.Count > 0)
            {
                result.Compiled = false;
                result.Message.Add("Your solution contains code outside the marked blocks. Please write your solution so that it only writes code in the specified places");
                result.Message.AddRange(blockedInput);
                result.Grade = 0;
                return result;
            }

            if (runInfo.Script.Contains("def count_i"))
            {
                List<string> forbidden = new List<string>();

                string[] fss = new string[] { "if ", "for ", "while ", "len(", "list(" };
                foreach (string fs in fss)
                {
                    if (runInfo.Code.Contains(fs))
                    {
                        forbidden.Add("La solucion al problema no puede usar la palabra " +
                            fs.Replace(" ", "").Replace("(", ""));
                    }
                }

                if (forbidden.Count > 0)
                {
                    result.Compiled = false;
                    result.Message.AddRange(forbidden);
                    result.Grade = 0;
                    return result;
                }
            }

            runInfo.Code = runInfo.Code.Replace(compilerInfo.CodeStart, runInfo.Before);
            runInfo.Code = runInfo.Code.Replace(compilerInfo.CodeEnd, runInfo.After);

            result = Run1(runInfo, compilerInfo);
            result.Grade = CalculateGrade(result.Output, runInfo.ExpectedResult, false, result.ActualErrors);

            return result;
        }

        private ExecutionResult Run1(RunInfo runInfo, CompilerInfo compilerInfo)
        {
            ExecutionResult er = new ExecutionResult();

            try
            {
                string pythondir = CurrentFolder + $@"Binaries\{compilerInfo.CompilerDirectory}\";
                string tempFolder = CurrentFolder + @"Temp\";
                string python = pythondir + $@"{compilerInfo.Compiler}";

                string sourceFile = $@"{tempFolder}\{runInfo.StudentId}.{compilerInfo.SourceExtension}";

                er.allCode = (runInfo.Code ?? "") + Environment.NewLine + (runInfo.Test ?? "");

                File.WriteAllText(
                    sourceFile,
                    (runInfo.Code ?? "") + Environment.NewLine + (runInfo.Test ?? ""));

                er.Succeeded = ProcessHelper.RunProcess(CurrentFolder, python,
                    sourceFile,
                    Timeout, out string executionOutput, out string executionErrors);

                if (!er.Succeeded)
                {
                    er.Compiled = false;
                    if (string.IsNullOrEmpty(executionErrors))
                    {
                        er.Message.Add(Resources.Messages.TimeOut);
                    }
                    else
                    {
                        er.Message.Add(executionErrors);
                    }

                    return er;
                }

                if (!string.IsNullOrEmpty(executionErrors))
                {
                    er.Compiled = false;
                    er.Succeeded = false;
                    er.Message.Add(executionErrors);
                    return er;
                }

                er.Output = executionOutput;
                er.TestCodeMessages.Add(executionOutput);
                er.Compiled = true;
                er.Succeeded = true;
                return er;
            }
            catch (Exception ex)
            {
                er.Compiled = false;
                er.Message.Add(ex.Message);
                return er;
            }
        }
        public override int CalculateGrade(string actual, string expected, bool hideErrors, List<string> errors)
        {
            if (string.IsNullOrEmpty(actual))
            {
                return 0;
            }

            string actualSimplified = RemoveSpace(actual);

            int sum = 0;
            if (!string.IsNullOrEmpty(expected))
            {
                string[] expectedLines = expected.Split('\n');
                foreach (string line in expectedLines.Where(x => x != ""))
                {
                    string original = line.Substring(line.IndexOf('-') + 1).Replace("\r", "");
                    string lineSimplified = RemoveSpace(line);
                    int points = int.Parse(lineSimplified.Substring(0, lineSimplified.IndexOf('-')));
                    string lineSearch = lineSimplified.Substring(lineSimplified.IndexOf('-') + 1);
                    if (points == 0)
                    {
                        errors.Add($@"{original}");
                        sum += points;
                    }
                    else if (actualSimplified.Contains(lineSearch))
                    {
                        errors.Add($@"  ({points} / {points} points): {original} (Succeeded)");
                        sum += points;
                    }
                    else
                    {
                        errors.Add($@"  (0 / {points} points) {original} (Failed)");
                    }
                }
            }

            return sum;
        }


        public override ExecutionResult RunCodeForValidation(RunInfo runInfo, CompilerInfo compilerInfo)
        {
            if (string.IsNullOrWhiteSpace(runInfo.Test))
            {
                runInfo.Code = runInfo.Solution;
                runInfo.Solution = "";
            }
            return Run(runInfo, compilerInfo);
        }
    }
}


