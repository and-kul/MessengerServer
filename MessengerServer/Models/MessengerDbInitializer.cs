using System.Data.Entity;
using Nancy.Cryptography;

namespace MessengerServer.Models
{
    public class MessengerDbInitializer : CreateDatabaseIfNotExists<MessengerDbContext>
    {
        protected override void Seed(MessengerDbContext db)
        {
            var keyGenerator = new RandomKeyGenerator();

            var secret = new Secret { Key = keyGenerator.GetBytes(256) };

            db.Secrets.Add(secret);

            db.SaveChanges();

        }
    }
}