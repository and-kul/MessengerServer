using System.Data.Entity.Migrations;
using System.Linq;
using MessengerServer.Models;
using Nancy.Cryptography;

namespace MessengerServer.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<MessengerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MessengerDbContext db)
        {
            if (!db.Secrets.Any())
            {
                var keyGenerator = new RandomKeyGenerator();

                var secret = new Secret { Key = keyGenerator.GetBytes(256) };

                db.Secrets.Add(secret);

                db.SaveChanges();
            }

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    db.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}