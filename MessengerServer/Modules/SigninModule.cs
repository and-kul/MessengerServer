using MessengerServer.Security;
using Nancy;
using Nancy.ModelBinding;

namespace MessengerServer.Modules
{
    public class SigninModule : NancyModule
    {
        public SigninModule()
        {
            Post["/signin"] = _ =>
            {
                try
                {
                    var guest = this.Bind<SimpleUser>();

                    var token = Authorization.GetToken(guest);

                    if (token == null) return HttpStatusCode.Unauthorized;

                    var response = new Response
                    {
                        StatusCode = HttpStatusCode.OK,
                        Headers = {["Authorization"] = token}
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