namespace Averia.Core.Domain.Commands
{
    using Averia.Core.Domain.Interfaces;

    public sealed class AuthUser : ICommand
    {
        public AuthUser(string userName, string sessionId)
        {
            UserName = userName;
            SessionId = sessionId;
        }

        public string UserName { get; set; }

        public string SessionId { get; set; }
    }
}
