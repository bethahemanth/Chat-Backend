using ChatAppBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.Services
{
    public interface IGroupService
    {
        string InsertGroup(int owner_id, string group_name, DateTime created_at);
        List<User> GetAllGroupMembers(int group_id);
        int GetGroupIdByMemberId(int user_id);
        string GetGroupNameByGroupId(int group_id);
        int GroupOwnerByGroupId(int group_id);
        List<ChatGroup> GetGroupByOwnerId(int owner_id);

        string DeleteGroup(int group_id);


    }
}
