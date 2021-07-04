namespace Averia.Core.Domain.Commands
{
    using Averia.Core.Domain.Interfaces;

    public class CreateSession : ICommand
    {
        public CreateSession(string sessionId) => SessionId = sessionId;

        public string SessionId { get; private set; }
    }
}
