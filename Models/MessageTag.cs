using Azure;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace task6.Models
{
    public class MessageTag
    {
        [Key, Column(Order = 0)]
        public Guid MessageId { get; set; }
        public Message Message { get; set; }

        [Key, Column(Order = 1)]
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
