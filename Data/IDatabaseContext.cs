using Npgsql;

namespace ChatApplication.Data.ChatAppBackend.Data
{
    public interface IDatabaseContext
    {
        NpgsqlConnection CreateConnection();

        string ExecuteQuery(string query);
        List<T> ExecuteQuery<T>(string query, Dictionary<string, object> parameters);
        List<T> GetQuery<T>(string query);

        List<string> GetResult(string query);
    }
}