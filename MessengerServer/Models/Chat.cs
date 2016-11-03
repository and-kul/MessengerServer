using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessengerServer.Models
{
    public class Chat
    {
        public int Id { get; set; }

        public bool IsGroup { get; set; }

        [MaxLength(40)]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();

        public long? LastMessageId { get; set; }

        [ForeignKey("LastMessageId")]
        public virtual Message LastMessage { get; set; }

        [InverseProperty("Chat")]
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }

}