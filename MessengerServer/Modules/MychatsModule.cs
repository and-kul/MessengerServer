using System.Linq;
using MessengerServer.Models;
using MessengerServer.Security;
using Nancy;
using Nancy.Security;

namespace MessengerServer.Modules
{
    public class MychatsModule : NancyModule
    {
        public MychatsModule()
        {
            this.RequiresAuthentication();

            Post["/mychats"] = _ =>
            {
                var userId = ((Identity) Context.CurrentUser).Id;

                using (var db = new MessengerDbContext())
                {
                    var user = db.Users.Find(userId);

                    var result = user.Chats.Select(chat => chat.Id);
                    
                    return result;
                }
            };
        }
    }
}