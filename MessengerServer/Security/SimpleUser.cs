using MessengerServer.Models;

namespace MessengerServer.Security
{
    public class SimpleUser
    {
        public SimpleUser()
        {
        }

        public SimpleUser(string login, string hash)
        {
            Login = login;
            Hash = hash;
        }

        public SimpleUser(User user)
        {
            Login = user.Login;
            Hash = user.Hash;
        }

        public string Login { get; set; }
        public string Hash { get; set; }
    }
}