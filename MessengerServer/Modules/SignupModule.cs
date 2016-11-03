using System.Linq;
using MessengerServer.Models;
using MessengerServer.Security;
using Nancy;
using Nancy.ModelBinding;
using HttpStatusCode = Nancy.HttpStatusCode;

namespace MessengerServer.Modules
{
    public class SignupModule : NancyModule
    {
        public SignupModule()
        {
            Post["/signup"] = _ =>
            {
                try
                {
                    var newUser = this.Bind<User>();
                    using (var db = new MessengerDbContext())
                    {
                        if (db.Users.Any(u => u.Login == newUser.Login))
                        {
                            return HttpStatusCode.Conflict;
                        }

                        db.Users.Add(newUser);
                        db.SaveChanges();
                    }

                    var response = new Response
                    {
                        StatusCode = HttpStatusCode.OK,
                        Headers = {["Authorization"] = Authorization.GetToken(new SimpleUser(newUser))}
                    };

                    return response;
                }
                catch (ModelBindingException)
                {
                    return HttpStatusCode.BadRequest;
                }
            };
        }
    }
}