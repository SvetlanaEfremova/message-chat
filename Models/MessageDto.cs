namespace task6.Models
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public List<string> Tags { get; set; }
    }

}
