using System;
using Averia.Core.Domain.Interfaces;
using Averia.Storage.Entity;
using System.Linq;

namespace Averia.Core.Domain.Server.Commands.Handlers
{
    using Averia.Core.Domain.Commands;
    using Averia.Storage.Entity.Models;
    using Averia.Transport.Server;

    public sealed class AuthCommandHandler : ICommandHandler<AuthUser>
    {
        private readonly TcpChatServer tcpChatServer;

        private readonly ChatContext context;

        public AuthCommandHandler(TcpChatServer tcpChatServer, ChatContext context)
        {
            this.tcpChatServer = tcpChatServer;
            this.context = context;
        }

        public void Execute(AuthUser command)
        {
            var findSession = context.Sessions.Single(w => w.SessionId == command.SessionId)
                              ?? throw new NotImplementedException();

            var findUser = context.Users.FirstOrDefault(w => w.Name == command.UserName);
            if (findUser == null)
                context.Users.Add(new UserEntity() { Name = command.UserName, Session = findSession });
            else
            {
                findUser.Session = findSession;
                context.Users.Update(findUser);
            }

            context.SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
