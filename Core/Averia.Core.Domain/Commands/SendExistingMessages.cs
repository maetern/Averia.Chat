namespace Averia.Core.Domain.Commands
{
    using Averia.Core.Domain.Interfaces;

    public class SendExistingMessages : ICommand
    {
        public SendExistingMessages(string sessionId) => SessionId = sessionId;

        public string SessionId { get; set; }
    }
}
