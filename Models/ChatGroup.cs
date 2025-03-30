namespace ChatAppBackend.Models
{
    public class ChatGroup
    {
        public int group_id { get; set; }
        public int owner_id { get; set; }
        public string group_name { get; set; }
        public DateTime created_at { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
    }
}