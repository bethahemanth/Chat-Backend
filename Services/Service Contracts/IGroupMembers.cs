using ChatAppBackend.Models;

namespace ChatApplication.Services.Service_Contracts
{
    public interface IGroupMembers
    {
        string InsertMember(GroupMember groupMember);
        string DeleteMember(int group_id,int user_id);
        List<User> GetMember(int user_id);
    }
}
