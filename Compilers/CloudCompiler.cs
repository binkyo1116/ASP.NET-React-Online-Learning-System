using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LMS.Common.Infos;

namespace Compilers
{
    //ftps://waws-prod-bay-103.ftp.azurewebsites.windows.net/site/wwwroot
    public class CloudCompiler
    {
        protected virtual int Timeout { get; } = 40;
        public virtual string StudentConstantPlaceholder { get; } = "${studentid}";
        private string currentFolder;
        public Func<string, string> ReplaceFuntion { get; set; }
        public string CurrentFolder
        {
            get => currentFolder;
            set => currentFolder = value.Contains("netcoreapp") ? Path.Combine(value, @"..\..\..\") : value;
        }
        protected CloudCompiler(string folder)
        {
            CurrentFolder = folder;
        }
        public static CloudCompiler GetCompiler(string language, string folder)
        {
            if (language == "C#")
            {
                return new CSharpCompiler(folder);
            }
            else if (language == "Python")
            {
                return new PythonCompiler(folder);
            }
            else if (language == "R")
            {
                return new RCompiler(folder);
            }
            else if (language == "SQL")
            {
                return new SqlCompiler(folder);
            }
            else if (language == "Java")
            {
                return new JavaCompiler(folder);
            }
            else if (language == "WebVisitor")
            {
                return new WebCompiler(folder);
            }
            else if (language == "Tableau")
            {
                return new TableauCompiler(folder);
            }
            else if (language == "Excel")
            {
                return new ExcelCompiler(folder);
            }
            else if (language == "Image")
            {
                return new ImageCompiler(folder);
            }
            else if (language == "Cpp")
            {
                return new CloudCompiler(folder);
            }
            else if (language == "DB")
            {
                return new DBCompiler(folder);
            }
            else if (language == "AzureDO")
            {
                return new AzureDOCompiler(folder);
            }
            else if (language == "Browser")
            {
                return new BrowserCompiler(folder);
            }
            else
            {
                return language == "CosmoDB"
                    ? (CloudCompiler)new CosmoDBCompiler(folder)
                    : throw new ArgumentOutOfRangeException("Unrecognized language: " + language);
            }
        }
        public virtual ExecutionResult Run(RunInfo runInfo, CompilerInfo compilerInfo)
        {
            ExecutionResult er = CheckCode(runInfo, compilerInfo);

            if (!er.Compiled)
            {
                return er;
            }

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

            string tempDirectory = $@"{CurrentFolder}Temp";
            string includeDirectory = $@"{CurrentFolder}Include";
            string source = $@"{tempDirectory}\{runInfo.StudentId}.{compilerInfo.SourceExtension}";
            string compiler = $@"{CurrentFolder}\Binaries\{compilerInfo.CompilerDirectory}\{compilerInfo.Compiler}";
            string executable = $@"{tempDirectory}\{runInfo.StudentId}.{compilerInfo.OutputExtension}";

            File.WriteAllText(source, code);

            compilerInfo.CompilationParameters = compilerInfo.CompilationParameters.Replace("{include}", includeDirectory);

            er.Compiled = ProcessHelper.RunProcess(tempDirectory, compiler,
                $@"{compilerInfo.CompilationParameters} {tempDirectory}\{runInfo.StudentId}.{compilerInfo.SourceExtension} /link /LIBPATH:{includeDirectory}\lib /out:{tempDirectory}\{runInfo.StudentId}.{compilerInfo.OutputExtension}",
                Timeout, out string compilerOutput, out string compilerErrors);

            er.Output = compilerOutput == $@"{runInfo.StudentId}.{compilerInfo.SourceExtension}{Environment.NewLine}" ? "" : compilerOutput;

            if (!er.Compiled)
            {
                er.Message.Add(Resources.Messages.TimeOut);
                return er;
            }

            if (!string.IsNullOrEmpty(er.Output))
            {
                er.Compiled = false;
                er.Message.Add(compilerOutput);
                return er;
            }

            if (!string.IsNullOrEmpty(compilerErrors))
            {
                er.Compiled = false;
                er.Message.Add(compilerErrors);
                return er;
            }

            er.Succeeded = ProcessHelper.RunProcess(tempDirectory, executable,
                runInfo.Parameters.Replace(",", " "), Timeout,
                out string executionOutput, out string executionErrors);

            if (!er.Succeeded)
            {
                er.ActualErrors.Add(Resources.Messages.TimeOut);
                return er;
            }

            if (!string.IsNullOrEmpty(executionErrors))
            {
                er.Succeeded = false;
                er.Message.Add(executionErrors);
                return er;
            }
            er.Output = executionOutput;

            er.Succeeded = true;

            if (er.Compiled)
            {
                //The error messages in the output are processed
                er = GetMessagesFromOutput(er);
            }

            er.Grade = CalculateGrade(er.Output, runInfo.ExpectedResult, false, er.ActualErrors);
            return er;
        }
        protected string RemoveSpace(string input)
        {
            if (input == null)
            {
                return input;
            }

            foreach (Tuple<string, string> remove in removes)
            {
                int i = input.IndexOf(remove.Item1, StringComparison.InvariantCulture);
                int j = input.IndexOf(remove.Item2, StringComparison.InvariantCulture);

                if (i >= 0 && j >= 0)
                {
                    input = input.Substring(0, i) + input.Substring(j);
                }
            }

            foreach (string replace in replaces)
            {
                input = input.Replace(replace, "");
            }

            return input;
        }

        private readonly List<Tuple<string, string>> removes = new List<Tuple<string, string>>()
        {
            new Tuple<string, string>("Date:", "Prob (F-statistic)"),
            new Tuple<string, string>("Time:", "Log-Likelihood")
        };
        private readonly List<string> replaces = new List<string>()
        {
            " ", "\n", "\r", "\t"
        };

        public List<string> GetDiffSolution(string actual, string solution, bool hideSolution, List<string> solutionMissing)
        {
            solution = solution.Replace(Environment.NewLine, "\n");
            solution = solution.Replace("\r", "\n");
            actual = actual.Replace(Environment.NewLine, "\n");
            actual = actual.Replace("\r", "\n");

            List<string> solutionSet = solution.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            List<string> actualSet = actual.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            List<string> solutionOriginal = solutionSet.Select(x => x).ToList();
            List<string> actualOriginal = actualSet.Select(x => x).ToList();

            for (int i = 0; i < actualSet.Count; i++)
            {
                actualSet[i] = RemoveSpace(actualSet[i]);
            }
            for (int i = 0; i < solutionSet.Count; i++)
            {
                solutionSet[i] = RemoveSpace(solutionSet[i]);
            }

            List<string> actualExtra = new List<string>();

            for (int i = 0; i < actualSet.Count; i++)
            {
                if (solutionSet.Contains(RemoveSpace(actualSet[i])))
                {
                    _ = solutionSet.Remove(RemoveSpace(actualSet[i]));
                    _ = solutionOriginal.Remove(actualOriginal[i]);
                }
                else
                {
                    actualExtra.Add($"Extra: {actualOriginal[i]}");
                }
            }
            for (int i = 0; i < solutionSet.Count; i++)
            {
                if (actualSet.Contains(RemoveSpace(solutionSet[i])))
                {
                    _ = actualSet.Remove(RemoveSpace(solutionSet[i]));
                    _ = actualOriginal.Remove(solutionOriginal[i]);
                }
                else
                {
                    if (hideSolution)
                    {
                        if (solutionSet[i].Length < 5)
                        {
                            solutionMissing.Add($"Missing: Less than 5 characters");
                        }
                        else
                        {
                            solutionMissing.Add($"Missing: {solutionOriginal[i].Substring(0, 2)} ...");
                        }
                    }
                    else
                    {
                        solutionMissing.Add($"Missing: {solutionOriginal[i]}");
                    }
                }
            }
            return actualExtra;
        }

        public List<string> GetDiffScript(string script, string code)
        {
            script = script.Replace(Environment.NewLine, "\n");
            script = script.Replace("\r", "\n");
            code = code.Replace(Environment.NewLine, "\n");
            code = code.Replace("\r", "\n");

            List<string> set1 = script.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            List<string> set2 = code.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            for (int i = 0; i < set2.Count; i++)
            {
                set2[i] = RemoveSpace(set2[i]);
            }

            List<string> diff = new List<string>();

            foreach (string s in set1)
            {
                if (set2.Contains(RemoveSpace(s)))
                {
                    _ = set2.Remove(RemoveSpace(s));
                }
                else
                {
                    diff.Add(s);
                }
            }
            return diff;
        }

        public List<string> GetOutOfBlocks(string script, string code, string codeStart, string codeEnd)
        {
            script = script.Replace(Environment.NewLine, "\n");
            script = script.Replace("\r", "\n");
            code = code.Replace(Environment.NewLine, "\n");
            code = code.Replace("\r", "\n");

            List<string> set1 = script.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            List<string> set2 = code.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            for (int i = 0; i < set1.Count; i++)
            {
                set1[i] = RemoveSpace(set1[i]);
            }

            List<string> diff = new List<string>();

            bool allowed = false;
            foreach (string s in set2)
            {
                if (!allowed)
                {
                    if (set1.Contains(RemoveSpace(s)))
                    {
                        _ = set1.Remove(RemoveSpace(s));
                    }
                    else
                    {
                        diff.Add(s);
                    }
                }
                if (CompareNoSpaces(s, codeStart))
                {
                    allowed = true;
                }
                else if (CompareNoSpaces(s, codeEnd))
                {
                    allowed = false;
                }
            }
            return diff;
        }

        public bool CompareNoSpaces(string s1, string s2)
        {
            return RemoveSpace(s1).Equals(RemoveSpace(s2));
        }

        public virtual int CalculateGrade(string actual, string expected, bool hideErrors, List<string> errors)
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
                    if (actualSimplified.Contains(lineSearch))
                    {
                        errors.Add($@"({points} / {points} points) : {original}");
                        sum += points;
                    }
                    else
                    {
                        errors.Add($@"(0 / {points} points) Failed : {original}");
                    }
                }
            }

            return sum;
        }
        public ExecutionResult CheckCode(RunInfo runInfo, CompilerInfo compilerInfo)
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
            result.Compiled = true;
            return result;
        }

        public virtual ExecutionResult GetMessagesFromOutput(ExecutionResult er)
        {
            er.TestCodeMessages.Add(er.Output);
            return er;
        }

        public virtual ExecutionResult RunCodeForValidation(RunInfo runInfo, CompilerInfo compilerInfo)
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