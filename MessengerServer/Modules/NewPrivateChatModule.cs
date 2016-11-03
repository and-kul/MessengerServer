using System;
using System.Linq;
using MessengerServer.Models;
using MessengerServer.Security;
using Nancy;
using Nancy.Security;

namespace MessengerServer.Modules
{
    public class NewPrivateChatModule : NancyModule
    {
        public NewPrivateChatModule()
        {
            this.RequiresAuthentication();

            Post["/newprivatechat/{with_user_login}"] = parameters =>
            {
                string withUserLogin = parameters["with_user_login"];

                var userId = ((Identity)Context.CurrentUser).Id;

                using (var db = new MessengerDbContext())
                {
                    var user = db.Users.Find(userId);
                    var secondUser = db.Users.FirstOrDefault(u => u.Login == withUserLogin);

                    if (secondUser == null)
                        return HttpStatusCode.BadRequest;

                    if (user.Chats.Any(ch => ch.IsGroup == false && ch.Users.Contains(secondUser)))
                        return HttpStatusCode.Conflict;

                    var newChat = new Chat {IsGroup = false};

                    newChat.Users.Add(user);
                    newChat.Users.Add(secondUser);

                    db.Chats.Add(newChat);

                    db.SaveChanges();

                    var result = new
                    {
                        ChatId = newChat.Id
                    };

                    return result;
                }
            };
        }
    }
}