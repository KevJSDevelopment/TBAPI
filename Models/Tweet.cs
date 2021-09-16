using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TwitterBattlesAPI.ModelHelpers;

namespace TwitterBattlesAPI.Models
{
    public class Tweet
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public string mediaUrl { get; set; }
        // public int Likes { get; set; }
        // public int Retweets { get; set; }
        // public int QuoteTweets { get; set; }
        // public TweetType TweetType { get; set; }
        // public string RetweetHeader { get; set; }
        // public string QuoteTweetMessage { get; set; }

    }
}