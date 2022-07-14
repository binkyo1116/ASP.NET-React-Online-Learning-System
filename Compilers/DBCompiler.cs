using System;
using System.Data.SqlClient;
using System.Text;
using LMS.Common.Infos;

namespace Compilers
{
    public class DBCompiler : CloudCompiler
    {
        internal DBCompiler(string connection) : base(connection) { }
        public override ExecutionResult Run(RunInfo runInfo, CompilerInfo compilerInfo)
        {
            ExecutionResult result = Run(runInfo.StudentId, runInfo.Code, runInfo.Solution, "Program");
            result.Grade = CalculateGrade(result.Output, runInfo.ExpectedResult, false, result.ActualErrors);

            return result;
        }
        private ExecutionResult Run(string studentId, string code, string parameterValues, string testClass)
        {
            string connectionString = @"Data Source=ccgserver.database.windows.net;Initial Catalog=Week 2;User ID=ccgadmin;Password=N9jvLf65";

            ExecutionResult er = new ExecutionResult();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(parameterValues.Replace(StudentConstantPlaceholder, studentId), connection);
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    SqlDataReader reader = command.ExecuteReader();
                    int count = reader.FieldCount;

                    while (reader.Read())
                    {
                        string line = "";
                        for (int i = 0; i < count; i++)
                        {
                            line += reader[i] + " ";
                        }
                        _ = sb.AppendLine(line);
                    }
                    string result = sb.ToString();
                    if (string.IsNullOrEmpty(result))
                    {
                        er.Compiled = true;
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
                er.ActualErrors.Add(ex.Message);
            }
            return er;
        }
    }
}
