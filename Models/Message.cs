namespace task6.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public List<MessageTag> MessagesAndTags { get; set; }
    }
}
