using System.ComponentModel.DataAnnotations;
using TwitterBattlesAPI.Models;
using System.Collections.Generic;

namespace TwitterBattlesAPI.Dtos
{
    public class UserReadDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Bio { get; set; }
        public byte[] ImageFiles { get; set; } 
        public string NFTProfileImage { get; set; }
        public bool UsingNFT { get; set; }
        public byte[] BackgroundImage { get; set; }
        public ICollection<WalletAddress> WalletAddresses { get; set; }
        public ICollection<Tweet> Tweets { get; set; }
        public ICollection<Follower> Followers { get; set; }
        public ICollection<Like> LikedTweets { get; set; }
        public ICollection<Retweet> Retweets { get; set; }
    }
}