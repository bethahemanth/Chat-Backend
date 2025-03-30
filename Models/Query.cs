namespace ChatAppBackend.Models
{
    public class Query
    {
        public string? TableName { get; set; }
        public List<string>? Attributes { get; set; }
        public List<List<string>>? Values { get; set; }
        public List<string>? Conditions { get; set; }
    }
}
