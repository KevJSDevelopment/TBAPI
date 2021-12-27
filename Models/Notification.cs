using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        public int UserId { get; set; }

        public int InteractingUserId { get; set; }

        public int TweetId { get; set; }

        public int ReplyTweetId { get; set; }

        public bool liked { get; set; }

        public bool retweeted { get; set; }

        public bool replied { get; set; }
    }
}