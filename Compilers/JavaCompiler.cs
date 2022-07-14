using System.IO;
using System.Text.RegularExpressions;
using LMS.Common.Infos;

namespace Compilers
{
    public class JavaCompiler : CloudCompiler
    {
        internal JavaCompiler(string folder) : base(folder) { }
        public override ExecutionResult Run(RunInfo runInfo, CompilerInfo compilerInfo)
        {

            string allCode = runInfo.Before + "\n\n" +
                    runInfo.Code + "\n\n" +
                    runInfo.Solution + "\n\n" +
                    runInfo.Test + "\n\n" +
                    runInfo.After + "\n\n";

            string find = "public class";
            int index = allCode.IndexOf(find);
            string name1 = allCode.Substring(index + find.Length + 1);
            int index1 = name1.IndexOf(" ");
            string name = name1.Substring(0, index1);

            ExecutionResult result = Run(allCode, runInfo.Parameters, name);

            if (result.Compiled)
            {
                //The error messages in the output are processed
                result = GetMessagesFromOutput(result);
            }

            result.Grade = CalculateGrade(result.Output, runInfo.ExpectedResult, false, result.ActualErrors);

            return result;

        }

        private ExecutionResult Run(string code, string parameterValues, string testClass)
        {
            ExecutionResult er = new ExecutionResult();

            string appPath = CurrentFolder + @"Temp";
            string javadir = appPath + @"\..\Binaries\Java\bin";
            string javac = javadir + @"\javac.exe";
            string java = javadir + @"\java.exe";

            File.WriteAllText($@"{appPath}\{testClass}.java", code);

            er.allCode = code;
            er.Compiled = ProcessHelper.RunProcess(appPath, javac,
                $@"-cp ""{parameterValues}"" ""{testClass}.java""", Timeout,
                out _, out string compilerErrors);

            if (!er.Compiled)
            {
                er.Message.Add(Resources.Messages.TimeOut);
                return er;
            }

            if (!string.IsNullOrEmpty(compilerErrors))
            {
                er.Compiled = false;
                er.Message.Add(compilerErrors);
                return er;
            }

            er.Succeeded = ProcessHelper.RunProcess(appPath, java,
                $@" -jar {parameterValues} -cp . --select-class {testClass}", Timeout,
                out string executionOutput, out string executionErrors);

            if (!er.Succeeded)
            {
                er.Message.Add(Resources.Messages.TimeOut);
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

            return er;
        }
        public override ExecutionResult GetMessagesFromOutput(ExecutionResult er)
        {
            if (er.Output == null)
            {
                return er;
            }

            //if any test fails, a "Failures" section is generated in the output
            //Error messages also appear there
            int index_Failures = er.Output.IndexOf("Failures");

            string new_output = "";
            //Search for substrings matching the defined pattern
            string[] split = Regex.Split(er.Output, @"(\#S{.+})|(\#E{.+})");

            //Reconstructing output by replacing characters used in the metalanguage
            for (int i = 0; i < split.Length; i++)
            {
                string substring = split[i];

                if (Regex.IsMatch(substring, @"(\#S{.+})|(\#E{.+})"))
                {
                    substring = Regex.Replace(substring, @"(\#S{)|(\#E{)|(})", "");

                    //only messages that appear before "Failures" are added
                    if (index_Failures == -1 || new_output.Length < index_Failures)
                    {
                        er.TestCodeMessages.Add(substring);
                    }
                }

                new_output += substring;
            }

            er.Output = new_output;
            return er;
        }
        public override ExecutionResult RunCodeForValidation(RunInfo runInfo, CompilerInfo compilerInfo)
        {
            if (string.IsNullOrWhiteSpace(runInfo.Test))
            {
                runInfo.Code = "public " + runInfo.Solution;
                runInfo.Solution = "";
            }
            return Run(runInfo, compilerInfo);
        }
    }
}
