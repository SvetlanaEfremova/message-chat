namespace task6.Models
{
    public class MessageInputModel
    {
        public string Text { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
