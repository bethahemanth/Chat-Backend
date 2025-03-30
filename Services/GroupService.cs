using System.Collections.Generic;
using ChatAppBackend.Models;
using ChatApplication.Repositories;
using ChatApplication.Repositories.Service_Contracts;
using global::ChatAppBackend.Models;
namespace ChatApplication.Services
{
    public class GroupService : IGroupService
    {
        private readonly IRepo _repo;

        public GroupService(IRepo repo)
        {
            _repo = repo;
        }

        public string InsertGroup(int owner_id, string group_name, DateTime created_at)
        {
            Query query = new Query()
            {
                TableName = "chat_group",  // Assuming you have a table named "groups"
                Values = new List<List<string>>()
            };

            query.Values.Add(new List<string>()
    {
       // Add group_id, assuming it's an integer type
        $"owner_id='{owner_id}'",  // Assuming owner_id is a string (like user ID or GUID)
        $"group_name='{group_name}'",  // Assuming group_name is a string
        $"created_at='{created_at:yyyy-MM-dd HH:mm:ss}'"  // Format DateTime for SQL insertion
    });

            return _repo.InsertRecord(query);  // Assuming _repo.InsertRecord handles the SQL query execution
        }

        public List<User> GetAllGroupMembers(int group_id)
        {
            Query query = new Query()
            {
                TableName = "group_members",  // Assuming there is a "group_members" table
                Conditions = new List<string>()
                {
                    $"group_id={group_id}"  // Retrieve all members of the specified group
                }
            };

            List<GroupMember> temp= _repo.GetRecords<GroupMember>(query);  // This will return the list of users in the group
            Query subQuery = new Query() { 
                TableName="users"
            };
            List<User> allUsers = _repo.GetRecords<User>(subQuery);
            List<User> result = new List<User>();

            temp.ForEach(memb => result.Add(allUsers.FirstOrDefault(u => u.user_id == memb.user_id)));

            return result;

        }
        public List<int> GetGroupIdByMemberId(int user_id)
        {
            Query query = new Query()
            {
                TableName = "group_members",  // Assuming a table for group memberships
                Conditions = new List<string>()
        {
            $"user_id={user_id}"  // Find the group(s) associated with the user
        }
            };

            var result = _repo.GetRecords<GroupMember>(query);  // Assuming the query will return group IDs
            List<int> temp = result.Select(g=>g.group_id).ToList();
            return temp;  // Return the first group ID the user is part of (if any)
        }
        public string GetGroupNameByGroupId(int group_id)
        {
            Query query = new Query()
            {
                TableName = "chat_group",  // Assuming a "groups" table for group details
                Conditions = new List<string>()
        {
            $"group_id={group_id}"  // Find the group by its ID
        }
            };

            var result = _repo.GetRecords<ChatGroup>(query);  // Retrieve the group details
            return result.FirstOrDefault()?.group_name;  // Return the group name (or null if not found)
        }
        public int GroupOwnerByGroupId(int group_id)
        {
            Query query = new Query()
            {
                TableName = "chat_group",  // Assuming a "groups" table
                Conditions = new List<string>()
        {
            $"group_id={group_id}"  // Find the group by its ID
        }
            };

            var result = _repo.GetRecords<ChatGroup>(query);  // Retrieve the group details
            return result.FirstOrDefault()?.owner_id ?? 0;  // Return the owner ID (or 0 if not found)
        }
        public List<ChatGroup> GetGroupByOwnerId(int owner_id)
        {
            Query query = new Query()
            {
                TableName = "chat_group",  // Assuming a "groups" table
                Conditions = new List<string>()
        {
            $"owner_id={owner_id}"  // Find all groups owned by the user
        }
            };

            return _repo.GetRecords<ChatGroup>(query);  // Return a list of groups owned by the user
        }


        public string DeleteGroup(int group_id)
        {
            // Create a query to delete the group based on the group_id
            Query query = new Query()
            {
                TableName = "chat_group",  // Specify the table to delete from
                Conditions = new List<string>()
                {
                    $"group_id={group_id}"  // Condition to delete the group with the specified group_id
                }
            };

            // Call the repository's DeleteRecord method to perform the deletion
            return _repo.DeleteRecord(query);  // Assuming DeleteRecord handles the SQL deletion
        }

        public ChatGroup GetGroupInfo(int group_id)
        {
            Query query = new Query()
            {
                TableName = "chat_group",
                Conditions = new List<string>()
                {
                    $"group_id={group_id}"  // Condition to delete the group with the specified group_id
                }
            };
            return _repo.GetRecords<ChatGroup>(query).FirstOrDefault(); 
        }

    }

}
