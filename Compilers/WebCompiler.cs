using System;
using System.Net.Http;
using LMS.Common.Infos;

namespace Compilers
{
    public class WebCompiler : CloudCompiler
    {
        internal WebCompiler(string folder) : base(folder) { }
        public override ExecutionResult Run(RunInfo runInfo, CompilerInfo compilerInfo)
        {
            ExecutionResult er = new ExecutionResult
            {
                Grade = 0
            };
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;

            string url = runInfo.Test.Replace(StudentConstantPlaceholder, runInfo.StudentId);
            if (!string.IsNullOrEmpty(runInfo.Code))
            {
                string code = runInfo.Code.Replace("\r", "");
                string[] lines = runInfo.Code.Split('\n');
                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        int index = line.IndexOf('=');
                        if (index <= 1 || index >= line.Length - 1)
                        {
                            er.Compiled = false;
                            er.Succeeded = false;
                            er.Message.Add("Invalid submission, the submission should have the format code=value");
                            er.Grade = 0;
                            return er;
                        }
                        else
                        {
                            string key = code.Substring(0, index);
                            string value = code.Substring(index + 1);
                            url = url.Replace($"%{key}%", value);
                        }
                    }
                }
            }
            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    er.Output = hc.GetStringAsync(url).Result;
                    if (er.Output.Contains(runInfo.ExpectedResult))
                    {
                        er.Compiled = true;
                        er.Succeeded = true;
                        er.Grade = 100;
                        return er;
                    }
                    else
                    {
                        er.Compiled = true;
                        er.Succeeded = false;
                        er.Expected = runInfo.ExpectedResult;
                        er.ActualErrors.Add("Incorrect output");
                        return er;
                    }
                }
                catch (Exception ex)
                {
                    er.Compiled = false;
                    er.Succeeded = false;
                    er.Grade = 0;
                    er.Message.Add("URL: " + url);
                    er.Message.Add("Failed to Connect");
                    er.Message.Add(ex.Message);
                    return er;
                }
            }
        }
    }
}
