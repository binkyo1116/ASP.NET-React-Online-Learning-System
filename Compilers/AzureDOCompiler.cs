using System;
using System.Net.Http;
using System.Net.Http.Headers;
using LMS.Common.Infos;

namespace Compilers
{
    public class AzureDOCompiler : CloudCompiler
    {
        internal AzureDOCompiler(string connection) : base(connection) { }
        public override ExecutionResult Run(RunInfo runInfo, CompilerInfo compilerInfo)
        {
            ExecutionResult result = Run(runInfo.StudentId, runInfo.Parameters);

            result.Grade = result.Output.Contains(runInfo.ExpectedResult) ? 100 : 0;
            return result;
        }
        private ExecutionResult Run(string studentId, string parameterValues)
        {
            string organization = "christina0864";
            string projectName = "CloudComputingGurus";
            string repoName = "CloudComputingGurus";
            string filePath = parameterValues.Replace(StudentConstantPlaceholder, studentId);
            string token = "ohbgdnjoy66slsbirxpxj2v5mgj3f6z52ooruzeilyuwrbyojpea";

            string uri = $"https://dev.azure.com/{organization}/{projectName}/_apis/git/repositories/{repoName}/items?path={filePath}&download=true&api-version=5.0";

            ExecutionResult er = new ExecutionResult();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string authorizationToken = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", token)));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationToken);

                    HttpResponseMessage response = client.GetAsync(uri).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        er.Compiled = false;
                        er.Message.Add(response.ReasonPhrase);
                    }
                    else
                    {
                        er.Compiled = true;
                        er.Succeeded = true;
                        er.Output = response.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                er.Compiled = false;
                er.Message.Add(ex.Message);
            }
            return er;
        }
    }
}
