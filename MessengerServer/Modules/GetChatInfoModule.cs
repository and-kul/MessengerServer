using System.Linq;
using MessengerServer.Models;
using MessengerServer.Security;
using Nancy;
using Nancy.Security;

namespace MessengerServer.Modules
{
    public class GetChatInfoModule : NancyModule
    {
        public GetChatInfoModule()
        {
            this.RequiresAuthentication();

            Post["/getchatinfo/{chat_id:int}"] = parameters =>
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

                    string name;
                    if (chat.IsGroup)
                        name = chat.Name;
                    else
                    {
                        var secondUser = chat.Users.FirstOrDefault(u => u.Id != user.Id);

                        var fullName = secondUser.FirstName + secondUser.LastName;

                        name = !string.IsNullOrEmpty(fullName) ? fullName : secondUser.Login;
                    }

                    var result = new
                    {
                        ChatId = chatId,
                        Name = name,
                        LastMessageId = chat.LastMessageId
                    };


                    return result;
                }
            };
        }
    }
}