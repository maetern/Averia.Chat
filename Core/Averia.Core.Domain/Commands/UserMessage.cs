using Averia.Core.Domain.Interfaces;

namespace Averia.Core.Domain.Commands
{
    public class UserMessage : ICommand
    {
        public UserMessage(string text) => Text = text;

        public string Text { get; set; }
    }
}
