namespace ChatAppBackend.Models
{
    public class MessageStatus
    {
        public int MessageStatusId { get; set; }
        public int MessageId { get; set; }
        public string Status { get; set; } // "sent", "received", "read"
        public DateTime StatusTime { get; set; } = DateTime.UtcNow;
    }
}