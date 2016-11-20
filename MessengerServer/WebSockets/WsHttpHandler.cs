using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MessengerServer.Models;
using MessengerServer.Security;
using Microsoft.Web.WebSockets;

namespace MessengerServer.WebSockets
{
    public class WsHttpHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest || context.IsWebSocketRequestUpgrading)
            {
                var path = context.Request.Path;

                var jwtToken = path.Substring(path.LastIndexOf("/") + 1);

                Identity identity;

                try
                {
                    using (var db = new MessengerDbContext())
                    {
                        var payload = Jose.JWT.Decode<JwtToken>(jwtToken, db.Secrets.Find(1).Key);

                        var tokenExpires = DateTime.FromBinary(payload.exp);

                        identity = DateTime.UtcNow < tokenExpires ? new Identity(payload.id) : null;
                        
                    }
                }
                catch (Exception)
                {
                    identity = null;
                }

                if (identity == null) return;

                var userId = identity.Id;

                SubscribeWebSocket oldSocket;

                var oldSocketExists = Connections.WebSockets.TryRemove(userId, out oldSocket);

                if (oldSocketExists)
                {
                    oldSocket.Close();
                }

                var newSocket = new SubscribeWebSocket(userId);

                Connections.WebSockets.TryAdd(userId, newSocket);
                
                context.AcceptWebSocketRequest(newSocket);
                
            }
        }

        public bool IsReusable => false;
    }
}