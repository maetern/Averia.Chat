using System;
using Averia.Core.Domain.Interfaces;
using Averia.Storage.Entity;
using System.Linq;

namespace Averia.Core.Domain.Server.Commands.Handlers
{
    using Averia.Core.Domain.Commands;
    using Averia.Core.Domain.Models;
    using Averia.Storage.Entity.Models;
    using Averia.Transport.Server;

    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;

    public sealed class UserMessageCommandHandler : ICommandHandler<AddMessage>, ICommandHandler<SendExistingMessages>
    {
        private readonly TcpChatServer tcpChatServer;

        private readonly ChatContext context;

        public UserMessageCommandHandler(TcpChatServer tcpChatServer, ChatContext context)
        {
            this.tcpChatServer = tcpChatServer;
            this.context = context;
        }

        public void Execute(AddMessage command)
        {
            var findAuthor = context.Users.Single(w => w.Session.SessionId == command.AuthorSessionId)
                              ?? throw new NotImplementedException();

            var users = context.Users.Where(w => w.Session != null);
            foreach (var user in users)
            {
                var message = new MessageEntity() { Author = findAuthor, Recepient = user, Text = command.Text, Date = DateTime.UtcNow };

                context.Messages.AddAsync(message).GetAwaiter().GetResult();
                context.SaveChangesAsync().GetAwaiter().GetResult();

                var tcpSession = tcpChatServer.FindSession(Guid.Parse(user.Session.SessionId))
                                 ?? throw new NotImplementedException();

                var messages = new Message[] { new Message(message.Author.Name, message.Text, message.Date) };
                tcpSession.SendAsync(JsonConvert.SerializeObject(new AllMessages(messages)));
            }
        }

        public void Execute(SendExistingMessages command)
        {
            var findUser = context.Users.FirstOrDefaultAsync(w => w.Session.SessionId == command.SessionId).GetAwaiter()
                .GetResult();

            if (findUser != null)
            {
                var tcpSession = tcpChatServer.FindSession(Guid.Parse(findUser.Session.SessionId))
                                 ?? throw new NullReferenceException();

                var messagesDb = context.Messages.Where(w => w.Recepient.Name == findUser.Name).ToListAsync().GetAwaiter()
                    .GetResult();

                if (messagesDb.Any())
                {
                    var messagesSend = messagesDb.Select(w => new Message(w.Author.Name, w.Text, w.Date)).ToArray();
                    tcpSession.Send(JsonConvert.SerializeObject(new AllMessages(messagesSend)));
                }
            }
        }
    }
}
