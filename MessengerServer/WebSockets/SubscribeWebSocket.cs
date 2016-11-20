using System;
using Microsoft.Web.WebSockets;

namespace MessengerServer.WebSockets
{
    public class SubscribeWebSocket : WebSocketHandler
    {
        public int UserId;

        public SubscribeWebSocket(int userId)
        {
            UserId = userId;
        }

        public override void OnClose()
        {
            base.OnClose();
        }

        public override void OnError()
        {
            base.OnError();
        }

    }
}