using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Models
{
    public class Tweet
    {
        [Key]
        public int TweetId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte[] Media { get; set; }
        public ICollection<Like> UserLikes { get; set; }
        public ICollection<Retweet> UserRetweets { get; set; }
        public int RepliedToTweetId { get; set; }

    }
}