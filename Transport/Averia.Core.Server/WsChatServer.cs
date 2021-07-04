using System;
using System.Net.Sockets;
using Averia.Core.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using NetCoreServer;

namespace Averia.Transport.Server
{
    public class WsChatServer : WsServer
    {
        private ICommandDispather commandDispather;

        public WsChatServer(IConfiguration configuration, ICommandDispather commandDispather)
            : base(configuration["EndpointWs:Host"], int.Parse(configuration["EndpointWs:Port"]))
            => this.commandDispather = commandDispather;

        protected override TcpSession CreateSession()
        {
            return new WsChatSession(this, commandDispather);
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat WebSocket server caught an error with code {error}");
        }
    }
}
