using System;
using System.Collections.Generic;
using Averia.Core.Domain.Commands;
using Averia.Core.Domain.Interfaces;
using Averia.Core.Server;
using Microsoft.Extensions.Caching.Memory;

namespace Averia.Core.Domain.Server.Commands.Handlers
{
    public class ConsoleCommandHandler : ICommandHandler<ConsoleCommand>
    {
        private readonly TcpChatServer tcpChatServer;

        private readonly IMemoryCache memoryCache;

        public ConsoleCommandHandler(TcpChatServer tcpChatServer, IMemoryCache memoryCache)
        {
            this.tcpChatServer = tcpChatServer;
            this.memoryCache = memoryCache;
        }

        public void Execute(ConsoleCommand command)
        {
            switch (command.Text.ToLowerInvariant())
            {
                case "exit":
                    tcpChatServer.Stop();
                    break;
                case "ls":
                    var sessions = memoryCache.Get<List<string>>(Constants.CacheSessionsKey);
                    if (sessions != null)
                    {
                        foreach (var sessionId in sessions)
                            Console.WriteLine($"{sessionId}");
                    }
                    else
                        Console.WriteLine("Sessions empty");

                    break;
            }
        }
    }
}
