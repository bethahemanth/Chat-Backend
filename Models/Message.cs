namespace ChatAppBackend.Models
{
    public class Message
    {
        public int message_id { get; set; }
        public int sender_id { get; set; }
        public int receiver_id { get; set; }
        public string message { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? destroy_at { get; set; }
    }
}
