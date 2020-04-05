using Microsoft.AspNet.SignalR;

namespace Server.Hubs
{
    public class ChatHub : Hub
    {
        public void NewMessage(string name, string message)
        {
            Clients.All.Broadcast(name, message);
        }
    }
}