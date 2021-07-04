namespace Averia.Core.Domain.Commands
{
    using Averia.Core.Domain.Interfaces;

    public sealed class DeleteSession : ICommand
    {
        public DeleteSession(string sessionId) => SessionId = sessionId;

        public string SessionId { get; private set; }
    }
}
