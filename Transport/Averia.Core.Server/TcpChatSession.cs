namespace Averia.Transport.Server
{
    using System;
    using System.Text;
    using Averia.Core.Domain.Commands;
    using Averia.Core.Domain.Interfaces;
    using NetCoreServer;
    using Newtonsoft.Json;

    internal sealed class TcpChatSession : TcpSession
    {
        private readonly ICommandDispather commandDispather;

        public TcpChatSession(TcpServer server, ICommandDispather commandDispather)
            : base(server)
            => this.commandDispather = commandDispather;

        protected override void OnConnected()
        {
            commandDispather.Execute(new CreateSession(Id.ToString()));
            Console.WriteLine($"Chat TCP session with Id {Id} connected!");
        }

        protected override void OnDisconnected()
        {
            commandDispather.Execute(new DeleteSession(Id.ToString()));
            Console.WriteLine($"Chat TCP session with Id {Id} disconnected!");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            var clientCommand = JsonConvert.DeserializeObject<ICommand>(message);

            switch (clientCommand)
            {
                case SignIn signIn:
                    var authUser = new AuthUser(signIn.UserName, Id.ToString());
                    commandDispather.Execute(authUser);
                    commandDispather.Execute(new SendExistingMessages(Id.ToString()));
                    break;
                case UserMessage userMessage:
                    var addMessage = new AddMessage(userMessage.Text, Id.ToString());
                    commandDispather.Execute(addMessage);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected override void OnError(System.Net.Sockets.SocketError error)
        {
            Console.WriteLine($"Chat TCP session caught an error with code {error}");
        }
    }
}
