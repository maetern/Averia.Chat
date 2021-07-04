﻿using System;
using Averia.Core.Domain.Commands;
using Averia.Core.Domain.Interfaces;
using Averia.Storage.Entity;
using System.Linq;

namespace Averia.Core.Domain.Server.Commands.Handlers
{
    using Averia.Transport.Server;

    public sealed class ConsoleCommandHandler : ICommandHandler<ConsoleCommand>
    {
        private readonly TcpChatServer tcpChatServer;

        private readonly ChatContext context;

        public ConsoleCommandHandler(TcpChatServer tcpChatServer, ChatContext context)
        {
            this.tcpChatServer = tcpChatServer;
            this.context = context;
        }

        public void Execute(ConsoleCommand command)
        {
            switch (command.Text.ToLowerInvariant())
            {
                case "exit":
                    tcpChatServer.Stop();
                    break;
                case "ls":
                    var sessions = context.Sessions.ToList();
                    if (sessions.Any())
                        foreach (var session in sessions)
                            Console.WriteLine($"{session.SessionId}");
                    else
                        Console.WriteLine("Sessions empty");

                    break;
            }
        }
    }
}
