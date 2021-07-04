namespace Averia.Core.Domain.Commands
{
    using Averia.Core.Domain.Interfaces;

    public class AddMessage : ICommand
    {
        public AddMessage(string text, string authorSessionId)
        {
            Text = text;
            AuthorSessionId = authorSessionId;
        }

        public string Text { get; set; }

        public string AuthorSessionId { get; set; }
    }
}
