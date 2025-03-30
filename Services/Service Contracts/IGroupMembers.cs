using ChatAppBackend.Models;

namespace ChatApplication.Services.Service_Contracts
{
    public interface IGroupMembers
    {
        string InsertMember(GroupMember groupMember);
        string DeleteMember(int user_id);
        GroupMember GetMember(int user_id);
    }
}
