using Nancy;

namespace MessengerServer.Modules
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = _ => "Hello World";
        }
    }
}