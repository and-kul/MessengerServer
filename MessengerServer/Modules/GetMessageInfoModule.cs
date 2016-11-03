using MessengerServer.Models;
using MessengerServer.Security;
using Nancy;
using Nancy.Security;

namespace MessengerServer.Modules
{
    public class GetMessageInfoModule : NancyModule
    {
        public GetMessageInfoModule()
        {
            this.RequiresAuthentication();

            Post["/getmessageinfo/{message_id:long}"] = parameters =>
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

                    var fromUser = message.From;

                    var result = new
                    {
                        MessageId = messageId,
                        ChatId = message.ChatId,
                        FromUserId = fromUser.Id,
                        FromUserName = fromUser.GetFullNameOrLogin(),
                        SentTime = message.SentTime.ToBinary()
                    };

                    return result;
                }
            };
        }
    }
}