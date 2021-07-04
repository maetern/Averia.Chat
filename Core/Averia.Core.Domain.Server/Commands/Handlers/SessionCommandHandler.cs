using Averia.Core.Domain.Interfaces;
using System.Collections.Generic;
using Averia.Core.Domain.Commands;
using Microsoft.Extensions.Caching.Memory;

namespace Averia.Core.Domain.Server.Commands.Handlers
{
    public sealed class SessionCommandHandler : ICommandHandler<CreateSession>
    {
        private readonly IMemoryCache memoryCache;

        public SessionCommandHandler(IMemoryCache memoryCache) => this.memoryCache = memoryCache;

        public void Execute(CreateSession command)
        {
            var sessions = memoryCache.Get<List<string>>(Constants.CacheSessionsKey);
            if (sessions == null)
                sessions = new List<string>();

            sessions.Add(command.SessionId);
            memoryCache.Set(Constants.CacheSessionsKey, sessions);
        }
    }
}
