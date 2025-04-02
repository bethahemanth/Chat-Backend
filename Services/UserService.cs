using System;
using System.Xml.Linq;
using ChatAppBackend.Models;
using ChatApplication.Data.ChatAppBackend.Data;
using ChatApplication.Data.Database1;
using ChatApplication.Repositories.Service_Contracts;
using ChatApplication.Services.Service_Contracts;
using ChatApplication.Data.Database1;
using ChatApplication.Models;
namespace ChatApplication.Services
{
    public class UserService : IUserService
    {
        private readonly IRepo _repo;
        private readonly IDatabaseContext1 _repo1;

        public UserService(IRepo repo, IDatabaseContext1 repo1)
        {
            _repo = repo;
            _repo1 = repo1;
        }

        public string RegisterUser(User user)
        {
            Query query = new Query()
            {
                TableName = "users",
                Values = new List<List<string>>()
            };

            query.Values.Add(new List<string>()
                {
                    $"username='{user.Username}'",
                    $"address='{user.Address}'",
                    $"country='{user.Country}'",
                    $"date_of_birth='{user.DateOfBirth:yyyy-MM-dd}'",
                    $"gender='{user.Gender}'",
                    $"profile_picture='{user.profile_picture}'",
                    $"email='{user.Email}'",
                    $"phone_number='{user.phone_number}'",
                    $"password='{user.Password}'"
                });

            return _repo.InsertRecord(query);
        }


        public string DeleteUser(int id)
        {
            Query query = new Query
            {
                TableName = "users",
                Conditions = new List<string>()
            };
            query.Conditions.Add($"user_id='{id}'");
            return _repo.DeleteRecord(query);
        }

        public User GetUser(int id)
        {
            Query query = new Query
            {
                TableName = "users",
                Conditions = new List<string>()
            };
            query.Conditions.Add($"user_id='{id}'");
            return _repo.GetRecords<User>(query).FirstOrDefault();
        }



        public List<User> GetAllUsers()
        {
            Query query = new Query
            {
                TableName = "users",
            };
            return _repo.GetRecords<User>(query);
        }




        public string UpdateUser(int id, User user)
        {
            Query query = new Query()
            {
                TableName = "users",
                Values = new List<List<string>>(){
                        new List<string>(){
                            $"user_id='{user.user_id}'",
                            $"username='{user.Username}'",
                            $"address='{user.Address}'",
                            $"country='{user.Country}'",
                            $"date_of_birth='{user.DateOfBirth:yyyy-MM-dd}'",
                            $"gender='{user.Gender}'",
                            $"profile_picture='{user.profile_picture}'",
                            $"email='{user.Email}'",
                            $"phone_number='{user.phone_number}'",
                            $"password='{user.Password}'",
                        }
                    },
                Conditions = new List<string>
                    {
                        $"user_id={user.user_id}"
                    }
            };
            return _repo.UpdateRecord(query);
        }


        // Other methods

        public Message GetLatestReceivedMessage(int receiverId)
        {
            Query query = new Query
            {
                TableName = "(SELECT m.sender_id, m.receiver_id, m.message, m.message_id, " +
                            "u.username AS sender_name, " +
                            "ROW_NUMBER() OVER (PARTITION BY m.sender_id ORDER BY m.message_id DESC) AS rn " +
                            "FROM messages m " +
                            "JOIN users u ON m.sender_id = u.user_id " +
                            "WHERE m.receiver_id = " + receiverId + ") AS LatestMessages",
                Conditions = new List<string>
        {
            "rn = 1"
        }
            };

            return _repo.GetRecords<Message>(query).FirstOrDefault();
        }

        public int GetLatestUserById(int user_id)
        {
            Query query = new Query
            {
                TableName = "users",
                Conditions = new List<string>()
            };
            query.Conditions.Add($"user_id='{user_id}'");

            // Assuming _repo.GetRecords<T> will fetch all columns, but you want only last_id
            return _repo.GetRecords<UserDTO>(query)
                       .Select(u => u.last_id)  // Select only last_id
                       .FirstOrDefault();       // Return the first match (or 0 if not found)
        }

        public string UploadProfilePicture(string filePath)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(filePath);
            string fullPath = Path.Combine("wwwroot/uploads", uniqueFileName);

            File.Copy(filePath, fullPath, true);

            // Return the image URL (adjust based on actual API serving logic)
            return $"http://localhost:5195/uploads/{uniqueFileName}";
        }
    }
    }
