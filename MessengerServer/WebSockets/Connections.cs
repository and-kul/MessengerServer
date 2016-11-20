using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessengerServer.WebSockets
{
    public static class Connections
    {
        public static ConcurrentDictionary<int, SubscribeWebSocket> WebSockets = new ConcurrentDictionary<int, SubscribeWebSocket>();

    }
}