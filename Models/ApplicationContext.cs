using Azure.Messaging;
using Microsoft.EntityFrameworkCore;

namespace task6.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<MessageTag> MessageTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessageTag>()
                .HasKey(mt => new { mt.MessageId, mt.TagId });
        }
    }
}
