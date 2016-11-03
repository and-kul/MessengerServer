using System;

namespace MessengerServer.Models
{
    public class Message
    {
        public long Id { get; set; }
        public DateTime SentTime { get; set; }

        public int UserId { get; set; }
        public virtual User From { get; set; }

        public byte[] Content { get; set; }

        public int ChatId { get; set; }
        public virtual Chat Chat { get; set; }
    }

}