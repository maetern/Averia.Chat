namespace Averia.Transport.Client
{
    using System;
    using System.Text;
    using Averia.Core.Domain.Commands;
    using Averia.Core.Domain.Interfaces;
    using Microsoft.Extensions.Configuration;
    using NetCoreServer;
    using Newtonsoft.Json;

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
            var data = JsonConvert.DeserializeObject<ICommand>(Encoding.UTF8.GetString(buffer, (int)offset, (int)size));
            var sendMessages = data as AllMessages ?? throw new NullReferenceException();

            foreach (var message in sendMessages.Messages) 
                Console.WriteLine($"{message.Date.ToShortTimeString()} {message.Author}: {message.Text}");
        }

        protected override void OnError(System.Net.Sockets.SocketError error)
        {
            Console.WriteLine($"Chat TCP client caught an error with code {error}");
        }
    }
}
