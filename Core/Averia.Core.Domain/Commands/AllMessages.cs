namespace Averia.Core.Domain.Commands
{
    using Averia.Core.Domain.Interfaces;
    using Averia.Core.Domain.Models;

    public class AllMessages : ICommand
    {
        public AllMessages(Message[] messages) => Messages = messages;

        public Message[] Messages { get; private set; }
    }
}
