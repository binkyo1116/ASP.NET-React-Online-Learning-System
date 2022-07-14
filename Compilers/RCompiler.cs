using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LMS.Common.Infos;

namespace Compilers
{
    public class RCompiler : CloudCompiler
    {
        internal RCompiler(string folder) : base(folder) { }
        public override ExecutionResult Run(RunInfo runInfo, CompilerInfo compilerInfo)
        {
            //execute solution in order to get expected result
            if (string.IsNullOrEmpty(runInfo.ExpectedResult))
            {
                ExecutionResult sol = new ExecutionResult
                {
                    Grade = 0
                };

                string exedirSol = $@"{CurrentFolder}\Binaries\{compilerInfo.CompilerDirectory}";
                string tempdirSol = $@"{CurrentFolder}\Temp";
                string rSol = $@"{exedirSol}\{compilerInfo.Compiler}";

                string rfileSol = $@"{tempdirSol}\{runInfo.StudentId}Sol.{compilerInfo.SourceExtension}";

                string codeSol = runInfo.Before + Environment.NewLine +
                                runInfo.Solution + Environment.NewLine +
                                runInfo.After;
                sol.allCode = codeSol;
                File.WriteAllText(rfileSol, codeSol);

                sol.Succeeded = ProcessHelper.RunProcess(CurrentFolder, rSol,
                    rfileSol,
                    Timeout, out string executionOutputSol, out string executionErrorsSol);

                if (!sol.Succeeded)
                {
                    sol.Compiled = false;
                    sol.Message.Add(Resources.Messages.TimeOut);
                    sol.Grade = CalculateGrade("", runInfo.ExpectedResult, true, sol.ActualErrors);
                    return sol;
                }
                sol.Compiled = true;
                if (!string.IsNullOrEmpty(executionErrorsSol))
                {
                    sol.Compiled = false;
                    sol.Succeeded = false;
                    sol.Message.Add(executionErrorsSol);
                    return sol;
                }
                else
                {
                    sol.Succeeded = true;
                    sol.Output = executionOutputSol;
                    sol.TestCodeMessages.Add(executionOutputSol);
                }

                //Load solution output into expected
                runInfo.ExpectedResult = "100-" + (sol.Output != null ? sol.Output.Replace("\n", "").Replace("\r", "") : "");

            }

            ExecutionResult er = new ExecutionResult
            {
                Grade = 0
            };

            string exedir = $@"{CurrentFolder}\Binaries\{compilerInfo.CompilerDirectory}";
            string tempdir = $@"{CurrentFolder}\Temp";
            string r = $@"{exedir}\{compilerInfo.Compiler}";

            string rfile = $@"{tempdir}\{runInfo.StudentId}.{compilerInfo.SourceExtension}";

            string code = runInfo.Before + Environment.NewLine +
                            runInfo.Code + Environment.NewLine +
                            runInfo.After;
            er.allCode = code;

            File.WriteAllText(rfile, code);

            er.Succeeded = ProcessHelper.RunProcess(CurrentFolder, r,
                rfile,
                Timeout, out string executionOutput, out string executionErrors);

            if (!er.Succeeded)
            {
                er.Compiled = false;
                er.Message.Add(Resources.Messages.TimeOut);
                er.Grade = CalculateGrade("", runInfo.ExpectedResult, true, er.ActualErrors);
                return er;
            }
            er.Compiled = true;
            if (!string.IsNullOrEmpty(executionErrors))
            {
                er.Compiled = false;
                er.Succeeded = false;
                er.Message.Add(executionErrors);
                return er;
            }
            else
            {
                er.Succeeded = true;
                er.Output = executionOutput;
                er.TestCodeMessages.Add(executionOutput);
            }
            er.Grade = CalculateGrade(executionErrors + executionOutput, runInfo.ExpectedResult, true, er.ActualErrors);
            return er;
        }
        public override ExecutionResult RunCodeForValidation(RunInfo runInfo, CompilerInfo compilerInfo)
        {

            return Run(runInfo, compilerInfo);
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
                    }
                    if (actualSimplified.Equals(lineSearch))
                    {
                        errors.Add($@"({points} / {points} points): {original}");
                        sum += points;
                    }
                    else
                    {
                        errors.Add($@"(0 / {points} points) Failed: {original}");
                    }
                }
            }

            return sum;
        }
    }
}
