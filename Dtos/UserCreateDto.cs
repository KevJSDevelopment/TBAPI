using System.ComponentModel.DataAnnotations;
using TwitterBattlesAPI.Models;
using System.Collections.Generic;

namespace TwitterBattlesAPI.Dtos
{
    public class UserCreateDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string Password { get; set; }

        public byte[] ImageFiles { get; set; } 
        public ICollection<Follower> Followers { get; set; }
        public ICollection<Like> LikedTweets { get; set; }
        public ICollection<Retweet> Retweets { get; set; }
        public ICollection<QuoteTweet> QuoteTweets { get; set; }
    }
}