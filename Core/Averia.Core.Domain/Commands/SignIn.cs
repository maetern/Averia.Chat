namespace Averia.Core.Domain.Commands
{
    using Averia.Core.Domain.Interfaces;

    public class SignIn : ICommand
    {
        public SignIn(string userName) => UserName = userName;

        public string UserName { get; set; }
    }
}
