using System;
using System.Threading.Tasks;
using MessengerServer.Models;
using MessengerServer.Security;
using MessengerServer.WebSockets;
using Nancy;
using Nancy.Security;
using Newtonsoft.Json;

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
                    chat.LastMessage = message;
                   
                    db.SaveChanges();

                    Task.Run(() =>
                    {
                        foreach (var userInChat in chat.Users)
                        {
                            SubscribeWebSocket webSocket;
                            var socketExists = Connections.WebSockets.TryGetValue(userInChat.Id,
                                out webSocket);

                            if (socketExists)
                            {
                                webSocket.Send(JsonConvert.SerializeObject(new {chatId}));
                            }

                        }
                    });


                    return HttpStatusCode.OK;
                }
            };
        }
    }
}