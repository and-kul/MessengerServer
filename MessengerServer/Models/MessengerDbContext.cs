using System.Data.Entity;

namespace MessengerServer.Models
{
    public class MessengerDbContext : DbContext
    {
        //static MessengerDbContext()
        //{
        //    Database.SetInitializer(new MessengerDbInitializer());
        //}

        public MessengerDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Secret> Secrets { get; set; }
    }

}