using ChatAppBackend.Models;
using ChatApplication.Services;
using ChatApplication.Services.Service_Contracts;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [ActionName("Register")]
    public string RegisterUser(User user)
    {
        return _userService.RegisterUser(user);
    }

    [HttpGet]
    [ActionName("GetUserByID")]
    public User GetUserByID(int id)
    {
        return _userService.GetUser(id);
    }

    [HttpGet]
    [ActionName("GetUser")]
    public User GetUser(string email, string password)
    {
        List<User> temp = _userService.GetAllUsers();
        User? result = temp.Where(u => u.Email.Equals(email) && u.Password.Equals(password)).FirstOrDefault();

        if (result == null)
        {
            return new User
            {
                user_id = -1,
                Username = "",
                Address = "",
                Country = "",
                DateOfBirth = DateTime.MinValue, // or you can set a default date
                Gender = "",
                profile_picture = "",
                Email = "",
                phone_number = "",
                Password = ""
            };
        }

        return result;
    }

    [HttpPut]
    [ActionName("Update")]
    public string UpdateUser(int id, [FromBody] User user)
    {
        return _userService.UpdateUser(id, user);
    }

    [HttpDelete]
    [ActionName("Delete")]
    public string DeleteUser(int id)
    {
        return _userService.DeleteUser(id);
    }

    [HttpGet]
    [ActionName("GetAllUsers")]
    public List<User> GetAllUsers()
    {
        return _userService.GetAllUsers();
    }

    [HttpGet]
    [ActionName("ValidateUser")]
    public Boolean ValidateUser(string Email, string Password)
    {
        return _userService.GetAllUsers().Any(u => u.Email == Email && u.Password == Password);
    }

    [HttpGet]
    [ActionName("GetUsersSet")]
    public List<User> GetUsersSet([FromQuery] List<int> contacts)
    {
        List<User> users = _userService.GetAllUsers();
        List<User> result = new List<User>();

        contacts.ForEach(c => result.Add(users.FirstOrDefault(u => u.user_id == c)));
        return result;
    }

}
