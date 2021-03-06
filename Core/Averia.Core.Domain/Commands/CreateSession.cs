namespace Averia.Core.Domain.Commands
{
    using Averia.Core.Domain.Interfaces;

    public sealed class CreateSession : ICommand
    {
        public CreateSession(string sessionId) => SessionId = sessionId;

        public string SessionId { get; private set; }
    }
}
