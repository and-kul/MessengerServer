using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using MessengerServer.Models;
using MessengerServer.Security;
using Nancy;
using Nancy.Security;

namespace MessengerServer.Modules
{
    public class GetChatUsersModule : NancyModule
    {
        public GetChatUsersModule()
        {
            this.RequiresAuthentication();

            Post["/getchatusers/{chat_id:int}"] = parameters =>
            {
                int chatId = parameters["chat_id"];

                var userId = ((Identity) Context.CurrentUser).Id;

                using (var db = new MessengerDbContext())
                {
                    var user = db.Users.Find(userId);
                    var chat = db.Chats.Find(chatId);

                    if (chat == null)
                        return HttpStatusCode.BadRequest;

                    if (!chat.Users.Contains(user))
                        return HttpStatusCode.Forbidden;

                    var result =
                        chat.Users.Select(u => new
                        {
                            Id = u.Id,
                            Login = u.Login,
                            FullName = u.GetFullNameOrLogin()
                        });

                    return result;
                }
            };
        }
    }
}