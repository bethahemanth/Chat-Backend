namespace ChatAppBackend.Models
{
    public class GroupMember
    {
        public int group_id { get; set; }
        public int user_id { get; set; }
        public DateTime joined_at { get; set; } = DateTime.UtcNow;
    }
}