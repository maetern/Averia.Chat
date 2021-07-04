using System;

namespace Averia.Core.Domain.Models
{
    public class Message
    {
        public Message(string author, string text, DateTime date)
        {
            Author = author;
            Text = text;
            Date = date;
        }

        public string Author { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }
    }
}
