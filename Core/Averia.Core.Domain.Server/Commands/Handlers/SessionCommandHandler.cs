using Averia.Core.Domain.Commands;
using Averia.Core.Domain.Interfaces;
using Averia.Storage.Entity;
using Averia.Storage.Entity.Models;

namespace Averia.Core.Domain.Server.Commands.Handlers
{
    using Microsoft.EntityFrameworkCore;

    public sealed class SessionCommandHandler : ICommandHandler<CreateSession>, ICommandHandler<DeleteSession>
    {
        private static readonly object LockedObject = new object();

        private readonly ChatContext context;

        public SessionCommandHandler(ChatContext context) => this.context = context;

        public void Execute(CreateSession command)
        {
            context.Sessions.Add(new SessionEntity() { SessionId = command.SessionId });
            context.SaveChangesAsync().GetAwaiter().GetResult();
        }

        public void Execute(DeleteSession command)
        {
            lock (LockedObject)
            {
                var findUser = context.Users.FirstOrDefaultAsync(w => w.Session.SessionId == command.SessionId).GetAwaiter()
                    .GetResult();

                if (findUser != null)
                {
                    findUser.Session = null;
                }

                var session = context.Sessions.SingleAsync(w => w.SessionId == command.SessionId).GetAwaiter().GetResult();
                context.Sessions.Remove(session);
                context.SaveChangesAsync().GetAwaiter().GetResult();
            }
        }
    }
}
