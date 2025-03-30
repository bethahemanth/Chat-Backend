using System;
using System.Xml.Linq;
using ChatAppBackend.Models;
using ChatApplication.Repositories.Service_Contracts;
using ChatApplication.Services.Service_Contracts;
namespace ChatApplication.Services
{
    public class UserService: IUserService
    {
        private readonly IRepo _repo;

        public UserService(IRepo repo)
        {
            _repo = repo;
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

        

        public string UpdateUser(int id,User user)
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

    }
    }
