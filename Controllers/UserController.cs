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
        var user = _userService.GetUser(id);
                                                                            
        if (user != null && !string.IsNullOrEmpty(user.profile_picture))
        {
            user.profile_picture = $"http://localhost:5195/uploads/{Path.GetFileName(user.profile_picture)}";
        }

        return user;
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

    [HttpGet]
    [ActionName("GetUserById1")]
    public User GetUser(int id)
    {
        return _userService.GetUser(id);
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

    [HttpGet]
    [ActionName("GetLastestUserById")]
    public int GetLatestUserById(int user_id)
    {
        return _userService.GetLatestUserById(user_id);
    }


    //    [HttpPost]
    //  [Consumes("multipart/form-data")]
    //  [ActionName("UploadProfilePicture")]
    //public IActionResult UploadProfilePicture([FromForm] IFormFile file)
    //{
    //    if (file == null || file.Length == 0)
    //    {
    //        return BadRequest(new { message = "No file uploaded" });
    //    }

    //    string uploadsFolder = Path.Combine("wwwroot/uploads");
    //    if (!Directory.Exists(uploadsFolder))
    //    {
    //        Directory.CreateDirectory(uploadsFolder);
    //    }

    //    // Generate a unique filename
    //    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
    //    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

    //    // Save the file
    //    using (var fileStream = new FileStream(filePath, FileMode.Create))
    //    {
    //        file.CopyTo(fileStream);
    //    }

    //    // **Return the correct absolute URL**
    //    string fileUrl = $"http://localhost:5195/uploads/{uniqueFileName}";
    //    return Ok(new { imageUrl = fileUrl });
    //}

    [HttpPost("upload-profile-picture")]
    [Consumes("multipart/form-data")]
    [ActionName("UploadProfilePicture")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UploadProfilePicture([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "No file uploaded" });
        }

        // Define the uploads folder (in wwwroot)
        string uploadsFolder = Path.Combine("wwwroot", "uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        // Generate a unique filename for the uploaded file
        string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        // Save the uploaded file to the specified path
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        // Return the URL of the uploaded file
        string fileUrl = $"http://localhost:5195/uploads/{uniqueFileName}";
        return Ok(new { imageUrl = fileUrl });
    }


}
