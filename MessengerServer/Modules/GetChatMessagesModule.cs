using System.Linq;
using MessengerServer.Models;
using MessengerServer.Security;
using Nancy;
using Nancy.Security;

namespace MessengerServer.Modules
{
    public class GetChatMessagesModule : NancyModule
    {
        public GetChatMessagesModule()
        {
            this.RequiresAuthentication();

            Post["/getchatmessages/{chat_id:int}/{start_message_id?0}"] = parameters =>
            {
                int chatId = parameters["chat_id"];
                long startMessageId = parameters["start_message_id"];

                var userId = ((Identity) Context.CurrentUser).Id;

                using (var db = new MessengerDbContext())
                {
                    var user = db.Users.Find(userId);
                    var chat = db.Chats.Find(chatId);

                    if (chat == null)
                        return HttpStatusCode.BadRequest;

                    if (!chat.Users.Contains(user))
                        return HttpStatusCode.Forbidden;

                    var result = chat.Messages
                        .Where(m => m.Id > startMessageId)
                        .OrderBy(m => m.Id)
                        .Select(m => m.Id);

                    return result;
                }
            };
        }
    }
}