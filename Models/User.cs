using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public ICollection<Tweet> Tweets { get; set; }
        // public ICollection<Tweet> LikedTweets { get; set; }
        // public ICollection<Tweet> Retweets { get; set; }
        // public ICollection<Tweet> QuoteTweets { get; set; }

    }
}