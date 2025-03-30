namespace ChatAppBackend.Models
{
    public class User
    {
        public int user_id { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string profile_picture { get; set; }
        public string Email { get; set; }
        public string phone_number { get; set; }
        public string Password { get; set; }
    }
}
