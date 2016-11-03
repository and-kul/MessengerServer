using System.IO;
using MessengerServer.Models;
using MessengerServer.Security;
using Nancy;
using Nancy.Security;

namespace MessengerServer.Modules
{
    public class GetMessageContentModule : NancyModule
    {
        public GetMessageContentModule()
        {
            this.RequiresAuthentication();

            Post["/getmessagecontent/{message_id:long}"] = parameters =>
            {
                long messageId = parameters["message_id"];

                var userId = ((Identity) Context.CurrentUser).Id;

                using (var db = new MessengerDbContext())
                {
                    var user = db.Users.Find(userId);

                    var message = db.Messages.Find(messageId);

                    if (message == null)
                        return HttpStatusCode.BadRequest;

                    var chat = message.Chat;

                    if (!chat.Users.Contains(user))
                        return HttpStatusCode.Forbidden;

                    var content = message.Content;

                    var response = new Response
                    {
                        StatusCode = HttpStatusCode.OK,
                        ContentType = "application/octet-stream",
                        Contents = stream =>
                        {
                            stream.Write(content, 0, content.Length);
                        }
                    };
                    

                    return response;
                }
            };
        }
    }
}