namespace ChatAppBackend.Models
{
    public class Chat
    {
        public int ChatId { get; set; }
        public int SenderId { get; set; }
        public int receiver_id { get; set; }
        public int MessageId { get; set; }
    }
}