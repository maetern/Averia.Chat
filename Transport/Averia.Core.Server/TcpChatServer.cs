namespace Averia.Transport.Server
{
    using System;
    using System.Net.Sockets;
    using Averia.Core.Domain.Interfaces;
    using Microsoft.Extensions.Configuration;
    using NetCoreServer;

    public sealed class TcpChatServer : TcpServer
    {
        private ICommandDispather commandDispather;

        public TcpChatServer(IConfiguration configuration, ICommandDispather commandDispather)
            : base(configuration["EndpointTcp:Host"], int.Parse(configuration["EndpointTcp:Port"]))
            => this.commandDispather = commandDispather;

        protected override TcpSession CreateSession()
        {
            var session = new TcpChatSession(this, commandDispather);
            return session;
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat TCP server caught an error with code {error}");
        }
    }
}
