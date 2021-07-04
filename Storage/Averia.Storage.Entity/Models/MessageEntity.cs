namespace Averia.Storage.Entity.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class MessageEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Text { get; set; }

        public UserEntity Author { get; set; }

        public UserEntity Recepient { get; set; }

        public DateTime Date { get; set; }
    }
}
