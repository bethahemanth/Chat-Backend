using ChatAppBackend.Models;

namespace ChatApplication.Services.Service_Contracts
{
    public interface IUserService
    {
        string RegisterUser(User user);
        User GetUser(int id);
        string UpdateUser(int id, User user);
        string DeleteUser(int id);
        List<User> GetAllUsers();


    }
}
