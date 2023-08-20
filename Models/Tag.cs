namespace task6.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<MessageTag> MessagesAndTags { get; set; }
    }
}
