using Microsoft.AspNet.SignalR.Client;
using System;

namespace ClientCore
{
    public class ChatClient
    {
        private HubConnection _connection;
        private IHubProxy _proxy;
        private string user;

        public delegate void OnReceiveMessage(string username, string message);
        public OnReceiveMessage ReceiveMessage;

        public ChatClient(string user)
        {
            this.user = user;
            _connection = new HubConnection("http://localhost:4543/");
            _proxy = _connection.CreateHubProxy("ChatHub");
            _connection.Headers.Add("Username", user);
        }

        public bool StartClient()
        {
            ConfigProxy();
            try
            {
                _connection.Start().Wait(10000);
                if (_connection.State != ConnectionState.Connected)
                    throw new TimeoutException();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private void ConfigProxy()
        {
            _proxy.On<string, string>("Broadcast", (user, message) =>
            {
                ReceiveMessage(user, message);
            });
        }

        public void Say(string message)
        {
            _proxy.Invoke("NewMessage", user, message);
        }

        public void Disconnect()
        {
            _connection.Stop();
        }
    }
}
