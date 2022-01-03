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
        public string Bio { get; set; }
        public string Password { get; set; }
        public byte[] ImageFiles { get; set; }  
        public string NFTProfileImage { get; set; }
        public bool UsingNFT { get; set; }
        public byte[] BackgroundImage { get; set; }
        public ICollection<WalletAddress> WalletAddresses { get; set; }
        public ICollection<Tweet> Tweets { get; set; }
        public ICollection<Bookmark> Bookmarks { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Follower> Followers { get; set; }
        public ICollection<Like> LikedTweets { get; set; }
        public ICollection<Retweet> Retweets { get; set; }

    }
}
