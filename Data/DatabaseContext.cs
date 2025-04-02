using ChatApplication.Data.ChatAppBackend.Data;
using Newtonsoft.Json;
using Npgsql;

namespace ChatApplication.Data
{
    public class DatabaseContext : IDatabaseContext
    {
        //private readonly string connectionString = "Server=172.162.12.47;Port=5432;UserDetails Id=postgres;Password=n@v@yUg@kw!x##;Database=FreshersTraining;Pooling=true;MaxPoolSize=100;Include Error Detail=true";
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
                result = "Query Failed";
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        //public List<T> GetQuery<T>(string query)
        //{
        //    List<T> result = new();
        //    using NpgsqlConnection connection = CreateConnection();
        //    try
        //    {
        //        using NpgsqlCommand cmd = new();
        //        cmd.Connection = connection;
        //        cmd.CommandText = query;
        //        cmd.CommandTimeout = 120;
        //        NpgsqlDataReader dr = cmd.ExecuteReader();

        //        while (dr.Read())
        //        {
        //            Vehicle v = new()
        //            {
        //                VIN = dr[0].ToString(),
        //                Model = dr[1].ToString(),
        //                Date = dr[2].ToString(),
        //                Status = dr[3].ToString(),
        //            };
        //            result.Add(v);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }

        //    return result;
        //}
        public List<T> GetQuery<T>(string query)
        {
            query = $"with cte as ({query}) select json_agg(cte) from cte";
            List<string> result = GetResult(query);

            // Initialize an empty list of T
            List<T> records = new List<T>();

            if (result.Count > 0)
            {
                // Log or inspect result[0]
                Console.WriteLine($"Result: {result[0]}");

                try
                {
                    // Attempt to deserialize the entire result
                    records = JsonConvert.DeserializeObject<List<T>>(result[0]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deserializing: {ex.Message}");
                }
            }

            return records;
        }

        public List<T> ExecuteQuery<T>(string query, Dictionary<string, object> parameters)
        {
            List<T> result = new List<T>();

            // Using a connection to PostgreSQL database
            using (var connection = new NpgsqlConnection(connectionString))
            {
                // Open connection
                connection.Open();

                // Create a command object
                using (var command = new NpgsqlCommand(query, connection))
                {
                    // Add parameters to the command
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }

                    // Execute the query and get the reader
                    using (var reader = command.ExecuteReader())
                    {
                        // Loop through the data reader and map it to the result type
                        while (reader.Read())
                        {
                            var item = Activator.CreateInstance<T>();

                            // Map data from the reader to the properties of T
                            foreach (var property in typeof(T).GetProperties())
                            {
                                // Check if the column exists in the reader
                                if (reader.GetOrdinal(property.Name) >= 0)
                                {
                                    property.SetValue(item, reader[property.Name]);
                                }
                            }

                            // Add the item to the result list
                            result.Add(item);
                        }
                    }
                }
            }

            return result;
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
