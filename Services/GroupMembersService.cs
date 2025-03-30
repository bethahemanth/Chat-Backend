using System;
using System.Collections.Generic;
using ChatAppBackend.Models;
using ChatApplication.Repositories.Service_Contracts;
using ChatApplication.Services.Service_Contracts;

namespace ChatApplication.Services
{
    public class GroupMembersService : IGroupMembers
    {
        private readonly IRepo _repo;

        public GroupMembersService(IRepo repo)
        {
            _repo = repo;
        }

        public string InsertMember(GroupMember groupMember)
        {
            Query query = new Query()
            {
                TableName = "group_members",
                Values = new List<List<string>>()
            };

            query.Values.Add(new List<string>()
            {
                $"group_id={groupMember.group_id}",
                $"user_id={groupMember.user_id}",
                $"joined_at='{groupMember.joined_at:yyyy-MM-dd HH:mm:ss}'"
            });

            return _repo.InsertRecord(query);
        }

        public string DeleteMember(int group_id, int user_id)
        {
            Query query = new Query
            {
                TableName = "group_members",
                Conditions = new List<string>()
            };
            query.Conditions.Add($"user_id={user_id}");
            query.Conditions.Add($"group_id={group_id}");
            return _repo.DeleteRecord(query);
        }

        public List<User> GetMember(int user_id)
        {
            Query query = new Query
            {
                TableName = "group_members",
                Conditions = new List<string>()
            };
            query.Conditions.Add($"group_id={user_id}");
            List<GroupMember> temp= _repo.GetRecords<GroupMember>(query);
            Query subQuery = new Query
            {
                TableName = "users",
            };
            List<User> allUsers = _repo.GetRecords<User>(subQuery);
            List<User> result = new List<User>();
            temp.ForEach(memb => result.Add(allUsers.FirstOrDefault(user=>user.user_id==memb.user_id)));

            return result; 

        }
    }
}
