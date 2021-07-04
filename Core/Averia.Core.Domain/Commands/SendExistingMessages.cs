namespace Averia.Core.Domain.Commands
{
    using Averia.Core.Domain.Interfaces;

    public sealed class SendExistingMessages : ICommand
    {
        public SendExistingMessages(string sessionId, SocketTypeEnum socketType)
        {
            SessionId = sessionId;
            SocketType = socketType;
        }

        public string SessionId { get; private set; }

        public SocketTypeEnum SocketType { get; private set; }
    }
}
