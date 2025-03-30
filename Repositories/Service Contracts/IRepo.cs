using ChatAppBackend.Models;

namespace ChatApplication.Repositories.Service_Contracts
{
    public interface IRepo
    {
        string CreateTable(Query query);
        string InsertRecord(Query query);
        string DeleteRecord(Query query);
        List<T> GetRecords<T>(Query query);
        string UpdateRecord(Query query);
    }
}
