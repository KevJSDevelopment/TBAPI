using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        public int UserWhoMessagedId { get; set; }

        public int UserReceivingMessageId { get; set; }

        public string MessageContent { get; set; }
    }
}