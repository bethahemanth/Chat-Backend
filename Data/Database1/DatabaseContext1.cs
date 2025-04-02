namespace ChatApplication.Data.Database1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ChatApplication.Data.ChatAppBackend.Data;
    //using Database;
    using Newtonsoft.Json;
    using Npgsql;
        public class DatabaseContext1 : IDatabaseContext1
        {

        private readonly string connectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=root;Database=Chat;SSL Mode=prefer";

        public NpgsqlConnection CreateConnection()
            {
                NpgsqlConnection connection = new(connectionString);
                connection.Open();

                return connection;
            }

            public string ExecuteQuery(string query)
            {
                string result = "";
                using NpgsqlConnection connection = CreateConnection();
                try
                {
                    using NpgsqlCommand cmd = new();
                    cmd.Connection = connection;
                    cmd.CommandText = query;
                    cmd.CommandTimeout = 120;
                    cmd.ExecuteNonQuery();
                    result = "Query Executed Successfully";
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                    result = "Query Executed Successfully";
                }

                return result;
            }
        public List<T> GetTableData<T>(string query)
        {
            query = $"with cte as ({query}) select json_agg(cte) from cte";
            List<string> result = GetResult(query);
            List<T> records = new List<T>();
            if (result.Count > 0)
            {
                try
                {
                    records = JsonConvert.DeserializeObject<List<T>>(result[0]);
                }
                catch (JsonReaderException ex)
                {
                    // Log the JSON string and the exception
                    Console.WriteLine($"Failed to deserialize JSON: {result[0]}");
                    Console.WriteLine(ex.Message);
                }
            }
            return records;
        }

            public List<string> GetResult(string query)
            {
                List<string> result = [];
                using NpgsqlConnection connection = CreateConnection();
                try
                {
                    using NpgsqlCommand cmd = new();
                    cmd.Connection = connection;
                    cmd.CommandTimeout = 120;
                    cmd.CommandText = query;
                    NpgsqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string row = dr[0]?.ToString();
                        if (!string.IsNullOrEmpty(row))
                        {
                            result.Add(row);
                        }
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return result;
            }
        }
    }