using System;
using MessengerServer.Models;
using MessengerServer.Security;
using Nancy;
using Nancy.Security;

namespace MessengerServer.Modules
{
    public class SendMessageModule : NancyModule
    {
        public SendMessageModule()
        {
            this.RequiresAuthentication();

            Post["/sendmessage/{chat_id:int}"] = parameters =>
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

                    var content = new byte[Request.Body.Length];

                    Request.Body.Read(content, 0, content.Length);

                    var message = new Message
                    {
                        Chat = chat,
                        From = user,
                        SentTime = DateTime.UtcNow,
                        Content = content
                    };

                    db.Messages.Add(message);
                    db.SaveChanges();

                    return HttpStatusCode.OK;
                }
            };
        }
    }
}