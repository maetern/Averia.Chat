using System;
using System.Text;
using Averia.Core.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using NetCoreServer;

namespace Averia.Core.Client
{
    public class TcpChatClient : TcpClient
    {
        private readonly ICommandDispather commandDispather;

        public TcpChatClient(IConfiguration configuration, ICommandDispather commandDispather)
            : base(configuration["Endpoint:Host"], int.Parse(configuration["Endpoint:Port"]))
            => this.commandDispather = commandDispather;

        public void DisconnectAndStop()
        {
            DisconnectAsync();
        }

        protected override void OnConnected()
        {
            Console.WriteLine($"Chat TCP client connected a new session with Id {Id}");
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"Chat TCP client disconnected a session with Id {Id}");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            Console.WriteLine(Encoding.UTF8.GetString(buffer, (int)offset, (int)size));
        }

        protected override void OnError(System.Net.Sockets.SocketError error)
        {
            Console.WriteLine($"Chat TCP client caught an error with code {error}");
        }
    }
}
