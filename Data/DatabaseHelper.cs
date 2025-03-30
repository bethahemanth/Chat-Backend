namespace ChatApplication.Data
{
    using System;
    using System.Data;
    using Npgsql;

    namespace ChatAppBackend.Data
    {
        public class DatabaseHelper
        {
            private readonly string _connectionString;

            public DatabaseHelper(string connectionString)
            {
                _connectionString = connectionString;
            }

            public IDbConnection GetConnection()
            {
                return new NpgsqlConnection(_connectionString);
            }
        }
    }
}
