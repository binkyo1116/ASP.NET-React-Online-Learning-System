using System;
using System.Text;
using LMS.Common.Infos;
using Microsoft.Azure.Cosmos;

namespace Compilers
{
    public class CosmoDBCompiler : CloudCompiler
    {
        // ADD THIS PART TO YOUR CODE

        // The Azure Cosmos DB endpoint for running this sample.
        private static readonly string EndpointUri = "https://ccgadmin.documents.azure.com:443/";
        // The primary key for the Azure Cosmos account.
        private static readonly string PrimaryKey = "jPcsbc721s8XtDCm3G1TwXlAX0uznTfyd0BI3XrT3dFB1na7cp0dmG6JlgJ4MllQIgfodSG3GFlx1E8io56NKg==";

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database database;

        // The container we will create.
        private Container container;

        // The name of the database and container we will create
        private readonly string databaseId = "ccgcosmodb";
        private readonly string containerId = "cccosmocontainer";

        internal CosmoDBCompiler(string connection) : base(connection) { }
        public override ExecutionResult Run(RunInfo runInfo, CompilerInfo compilerInfo)
        {
            ExecutionResult result = Run(runInfo.StudentId, runInfo.Code, runInfo.Test, "Program");
            result.Grade = CalculateGrade(result.Output, runInfo.ExpectedResult, false, result.ActualErrors);

            return result;
        }
        private ExecutionResult Run(string studentId, string code, string parameterValues, string testClass)
        {
            cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);

            ExecutionResult er = new ExecutionResult();

            try
            {
                string sqlQueryText = parameterValues.Replace(StudentConstantPlaceholder, studentId);

                QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);

                database = cosmosClient.GetDatabase(databaseId);
                container = database.GetContainer(containerId);

                StringBuilder sb = new StringBuilder();

                FeedIterator<object> queryResultSetIterator = container.GetItemQueryIterator<object>(queryDefinition);

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<object> currentResultSet = queryResultSetIterator.ReadNextAsync().Result;
                    foreach (object family in currentResultSet)
                    {
                        _ = sb.AppendLine(family.ToString());
                    }
                    string result = sb.ToString();
                    if (string.IsNullOrEmpty(result))
                    {
                        er.Compiled = false;
                        er.Succeeded = false;
                        er.Message.Add("Incorrect Output, try running the query manually till it produces the expected output");
                        er.Output = "";
                    }
                    else
                    {
                        er.Compiled = true;
                        er.Succeeded = true;
                        er.Output = result.ToString();
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
