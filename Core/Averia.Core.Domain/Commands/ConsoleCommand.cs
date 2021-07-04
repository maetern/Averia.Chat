namespace Averia.Core.Domain.Commands
{
    using Averia.Core.Domain.Interfaces;

    public sealed class ConsoleCommand : ICommand
    {
        public ConsoleCommand(string text) => Text = text;

        public string Text { get; private set; }
    }
}
