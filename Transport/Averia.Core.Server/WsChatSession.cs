using System;
using System.Net.Sockets;
using System.Text;
using Averia.Core.Domain.Commands;
using Averia.Core.Domain.Interfaces;
using NetCoreServer;
using Newtonsoft.Json;

namespace Averia.Transport.Server
{
    using Averia.Core.Domain;

    public sealed class WsChatSession : WsSession
    {
        private readonly ICommandDispather commandDispather;

        public WsChatSession(WsServer server, ICommandDispather commandDispather)
            : base(server)
            => this.commandDispather = commandDispather;

        public override void OnWsConnected(HttpRequest request)
        {
            commandDispather.Execute(new CreateSession(Id.ToString()));
            Console.WriteLine($"Chat WebSocket session with Id {Id} connected!");
        }

        public override void OnWsDisconnected()
        {
            commandDispather.Execute(new DeleteSession(Id.ToString()));
            Console.WriteLine($"Chat WebSocket session with Id {Id} disconnected!");
        }

        public override void OnWsReceived(byte[] buffer, long offset, long size)
        {
            string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            var clientCommand = JsonConvert.DeserializeObject<ICommand>(message);

            switch (clientCommand)
            {
                case SignIn signIn:
                    var authUser = new AuthUser(signIn.UserName, Id.ToString());
                    commandDispather.Execute(authUser);
                    commandDispather.Execute(new SendExistingMessages(Id.ToString(), SocketTypeEnum.Ws));
                    break;
                case UserMessage userMessage:
                    var addMessage = new AddMessage(userMessage.Text, Id.ToString());
                    commandDispather.Execute(addMessage);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat WebSocket session caught an error with code {error}");
        }
    }
}
