using System;
using System.Threading;
using LMS.Common.Infos;
using PuppeteerSharp;

namespace Compilers
{
    public class BrowserCompiler : CloudCompiler
    {
        internal BrowserCompiler(string folder) : base(folder) { }
        public override ExecutionResult Run(RunInfo runInfo, CompilerInfo compilerInfo)
        {
            ExecutionResult er = new ExecutionResult
            {
                Grade = 0
            };

            runInfo.Test = runInfo.Test.Replace(StudentConstantPlaceholder, runInfo.StudentId);
            runInfo.Test = runInfo.Test.ToLower();

            try
            {
                using (Browser browser = Puppeteer.ConnectAsync(new ConnectOptions
                {
                    BrowserWSEndpoint = "wss://chrome.browserless.io?token=ae733b30-5781-4ab1-8288-f17d8958ea59"
                }).Result)
                {
                    Page page = browser.NewPageAsync().Result;

                    foreach (string url in runInfo.Test.Split('\n'))
                    {

                        Response result = page.GoToAsync(url).Result;
                        Thread.Sleep(2000);
                        er.Output += "\n" + page.GetContentAsync().Result;
                    }
                    _ = GetDiffSolution(er.Output, runInfo.ExpectedResult, false, er.ActualErrors);

                    if (er.ActualErrors.Count == 0)
                    {
                        er.Compiled = true;
                        er.Succeeded = true;
                        er.Grade = 100;
                        return er;
                    }
                    else
                    {
                        er.Compiled = true;
                        er.Succeeded = true;
                        er.Expected = runInfo.ExpectedResult;
                        return er;
                    }
                }
            }
            catch (Exception ex)
            {
                er.Compiled = false;
                er.Succeeded = false;
                er.Grade = 0;
                er.Message.Add("URL: " + runInfo.Test);
                er.Message.Add("Failed to Connect");
                er.Message.Add(ex.Message);
                er.Message.Add(ex.ToString());
                return er;
            }
        }
    }
}
