using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public byte[] ImageFiles { get; set; }  
        public ICollection<Tweet> Tweets { get; set; }
        public ICollection<Follower> Followers { get; set; }
        public ICollection<Like> LikedTweets { get; set; }
        public ICollection<Retweet> Retweets { get; set; }
        public ICollection<QuoteTweet> QuoteTweets { get; set; }

    }
}