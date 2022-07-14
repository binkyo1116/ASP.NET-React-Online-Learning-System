using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LMS.Common.HelperModels;

namespace LMSLibrary
{
    public static class SQLHelper
    {
        public static string DefaultConnectionString = "data source = letsusedata.database.windows.net; initial catalog = Material; persist security info=True;user id = marcelo; password=rR`jR34rpVh_>wUr;";

        public static List<List<object>> RunSqlQuery(string sqlQuery)
        {
            var connectionInfo = DefaultConnectionString;
            if(System.Configuration.ConfigurationManager.ConnectionStrings["MaterialEntities1"] != null)
                connectionInfo = System.Configuration.ConfigurationManager.ConnectionStrings["MaterialEntities1"].ConnectionString;
            return RunSqlQuery(connectionInfo, sqlQuery);
        }

        public static bool RunSqlUpdate(string sqlQuery)
        {
            var connectionInfo = DefaultConnectionString;
            if (System.Configuration.ConfigurationManager.ConnectionStrings["MaterialEntities1"] != null)
                connectionInfo = System.Configuration.ConfigurationManager.ConnectionStrings["MaterialEntities1"].ConnectionString;
            return RunSqlUpdate(connectionInfo, sqlQuery);
        }
        public static List<List<object>> RunSqlQuery(string connectionInfo, string sqlQuery)
        {
            List<List<object>> result = new List<List<object>>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionInfo))
                {
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        //command.CommandTimeout = 240;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<object> temp = new List<object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    temp.Add(reader.GetValue(i));
                                }
                                result.Add(temp);
                            }
                        }
                    }
                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return result;
        }
        //public static List<List<object>> RunSqlQueryList(string spName, List<Param> paramList)
        //{
        //    List<List<object>> result = new List<List<object>>();
        //    try
        //    {
        //        var connectionInfo = System.Configuration.ConfigurationManager.ConnectionStrings["MaterialEntities1"].ConnectionString;
        //        using (SqlConnection connection = new SqlConnection(connectionInfo))
        //        {
        //            using (SqlCommand command = new SqlCommand(spName, connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                foreach (Param item in paramList)
        //                {
        //                    command.Parameters.AddWithValue($"@{item.Name}", item.Value);
        //                }

        //                SqlDataAdapter dataadapter = new SqlDataAdapter();
        //                  // seconds
        //                DataTable dt = new DataTable();
        //                //connection.Open();
        //                dataadapter.SelectCommand = command;
        //                dataadapter.SelectCommand.CommandTimeout = 240;
        //                dataadapter.Fill(dt);
        //                if(dt.Rows.Count > 0)
        //                {
                            
        //                }
        //                //using (SqlDataReader reader = command.ExecuteReader())
        //                //{
        //                //    while (reader.Read())
        //                //    {
        //                //        List<object> temp = new List<object>();
        //                //        for (int i = 0; i < reader.FieldCount; i++)
        //                //        {
        //                //            temp.Add(reader.GetValue(i));
        //                //        }
        //                //        result.Add(temp);
        //                //    }
        //                //}
        //            }
        //        }
        //        return result;
        //    }
        //    catch (SqlException e)
        //    {
        //        Console.WriteLine(e.ToString());
        //        return result;
        //    }
        //}

        public static bool RunSqlUpdate(string connectionInfo, string sqlQuery)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionInfo))
                {
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public static bool RunSqlUpdateWithParam(string spName, List<Param> paramList)
        {
            try
            {
                var connectionInfo = DefaultConnectionString;
                if (System.Configuration.ConfigurationManager.ConnectionStrings["MaterialEntities1"] != null)
                    connectionInfo = System.Configuration.ConfigurationManager.ConnectionStrings["MaterialEntities1"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionInfo))
                {
                    using (SqlCommand command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        foreach (Param item in paramList)
                        {
                            command.Parameters.AddWithValue($"@{item.Name}", item.Value);
                        }
                        //command.Parameters.AddWithValue("@StudentId", studentId);
                        //command.Parameters.AddWithValue("@Image", photo);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
        
    }
}
