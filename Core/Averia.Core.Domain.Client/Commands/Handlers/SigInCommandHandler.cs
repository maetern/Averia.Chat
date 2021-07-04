using Averia.Core.Domain.Commands;
using Averia.Core.Domain.Interfaces;

namespace Averia.Core.Domain.Client.Commands.Handlers
{
    using Averia.Transport.Client;
    using Newtonsoft.Json;

    public class SigInCommandHandler : ICommandHandler<SignIn>
    {
        private readonly TcpChatClient chatClient;

        public SigInCommandHandler(TcpChatClient chatClient) => this.chatClient = chatClient;

        public void Execute(SignIn command)
        {
            var message = JsonConvert.SerializeObject(command);
            chatClient.Send(message);
        }
    }
}
