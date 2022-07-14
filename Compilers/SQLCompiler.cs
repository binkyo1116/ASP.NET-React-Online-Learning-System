using System.Data.SqlClient;
using System.Text;
using LMS.Common.Infos;

namespace Compilers
{
    public class SqlCompiler : CloudCompiler
    {
        internal SqlCompiler(string folder) : base(folder) { }
        public override ExecutionResult Run(RunInfo runInfo, CompilerInfo compilerInfo)
        {
            ExecutionResult er = new ExecutionResult
            {
                Grade = 0,
                Compiled = true
            };
            string connectionString = @"Data Source=ludsampledb.database.windows.net;Initial Catalog=SampleDB;User ID=sampleuser;Password=7XTdm=/{";

            // string connectionString = ConfigurationManager.ConnectionStrings["ConnectionData"].ConnectionString;
            StringBuilder str = new StringBuilder("");
            StringBuilder str2 = new StringBuilder("");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlExpressionSolution = runInfo.Solution;
                    using (SqlCommand command2 = new SqlCommand(sqlExpressionSolution, connection))
                    {
                        SqlDataReader reader = command2.ExecuteReader();

                        //Get column names
                        _ = str2.Append(reader.GetName(0).ToString().Trim());
                        for (int i = 1; i < reader.FieldCount; i++)
                        {
                            _ = str2.Append("||" + reader.GetName(i).ToString().Trim());
                        }
                        _ = str2.Append("\n");

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                _ = str2.Append(reader.GetValue(0).ToString().Trim());

                                for (int i = 1; i < reader.FieldCount; i++)
                                {
                                    _ = str2.Append("||" + reader.GetValue(i).ToString().Trim());
                                }
                                _ = str2.Append("\n");
                            }
                        }

                        er.Expected = str2.ToString();
                        reader.Close();
                    }

                    string sqlExpressionScript = runInfo.Code;
                    using (SqlCommand command = new SqlCommand(sqlExpressionScript, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        //Get column names
                        _ = str.Append(reader.GetName(0).ToString().Trim());
                        for (int i = 1; i < reader.FieldCount; i++)
                        {
                            _ = str.Append("||" + reader.GetName(i).ToString().Trim());
                        }
                        _ = str.Append("\n");

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                _ = str.Append(reader.GetValue(0).ToString().Trim());

                                for (int i = 1; i < reader.FieldCount; i++)
                                {
                                    _ = str.Append("||" + reader.GetValue(i).ToString().Trim());
                                }
                                _ = str.Append("\n");
                            }
                        }

                        er.Output = str.ToString();
                        er.Succeeded = true;
                        reader.Close();
                    }

                }
                catch (SqlException ex)
                {
                    er.Succeeded = false;
                    er.Compiled = false;
                    er.Message.Add(ex.Message);
                    er.ActualErrors.Add(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            runInfo.ExpectedResult = "100-" + (er.Expected != null ? er.Expected.Replace("\n", "").Replace("\r", "") : "");
            er.Grade = CalculateGrade(er.Output != null ? er.Output.Replace("\n", "").Replace("\r", "") : "", runInfo.ExpectedResult, false, er.ActualErrors);
            if (er.Grade == 100)
            {
                er.ActualErrors.Clear();
                er.ActualErrors.Add("Compilation successful!");
            }
            else
                if (er.Succeeded == true)
            {
                er.ActualErrors.Clear();
                er.ActualErrors.Add("Compilation successful!");
                er.ActualErrors.Add("Your solution is incorrect");
            }
            return er;
        }
        public override ExecutionResult RunCodeForValidation(RunInfo runInfo, CompilerInfo compilerInfo)
        {
            return Run(runInfo, compilerInfo);
        }
    }
}