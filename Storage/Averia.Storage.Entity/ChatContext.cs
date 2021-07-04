using Microsoft.EntityFrameworkCore;

namespace Averia.Storage.Entity
{
    using Averia.Storage.Entity.Models;

    public class ChatContext : DbContext
    {
        public ChatContext()
        {
        }

        public ChatContext(DbContextOptions<ChatContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<SessionEntity> Sessions { get; set; }

        public DbSet<MessageEntity> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
