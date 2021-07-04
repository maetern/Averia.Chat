using Averia.Core.Domain.Commands;
using Averia.Core.Domain.Interfaces;

namespace Averia.Core.Domain.Client.Commands.Handlers
{
    using Averia.Transport.Client;
    using Newtonsoft.Json;

    public sealed class ConsoleCommandHandler : ICommandHandler<ConsoleCommand>
    {
        private readonly TcpChatClient tcpChatClient;

        public ConsoleCommandHandler(TcpChatClient tcpChatClient) => this.tcpChatClient = tcpChatClient;

        public void Execute(ConsoleCommand command)
        {
            switch (command.Text.ToLowerInvariant())
            {
                case "$exit":
                    tcpChatClient.DisconnectAndStop();
                    break;
                default:
                    var message = new UserMessage(command.Text);
                    tcpChatClient.Send(JsonConvert.SerializeObject(message));
                    break;
            }
        }
    }
}
