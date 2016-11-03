namespace MessengerServer.Security
{
    public class JwtToken
    {
        public long exp;
        public int id;

        public JwtToken()
        {
        }

        public JwtToken(long exp, int id)
        {
            this.exp = exp;
            this.id = id;
        }
    }
}