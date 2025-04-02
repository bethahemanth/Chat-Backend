using Npgsql;

namespace ChatApplication.Data.Database1
{
    public interface IDatabaseContext1
    {
        NpgsqlConnection CreateConnection();

        string ExecuteQuery(string query);

        public List<T> GetTableData<T>(string query);
    }
}
