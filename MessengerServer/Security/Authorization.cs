using System;
using System.Linq;
using Jose;
using MessengerServer.Models;

namespace MessengerServer.Security
{
    public static class Authorization
    {
        private static readonly TimeSpan TOKEN_LIFETIME = new TimeSpan(days: 1, hours: 0, minutes: 0, seconds: 0);

        public static string GetToken(SimpleUser guest)
        {
            using (var db = new MessengerDbContext())
            {
                var match = db.Users.FirstOrDefault(u => u.Login == guest.Login);

                if (match == null || guest.Hash != match.Hash)
                {
                    return null;
                }

                var exp = (DateTime.UtcNow + TOKEN_LIFETIME).ToBinary();
                var id = match.Id;

                var payload = new JwtToken(exp, id);

                var token = JWT.Encode(payload, db.Secrets.Find(1).Key, JwsAlgorithm.HS256);

                return token;
            }

        }

    }
}