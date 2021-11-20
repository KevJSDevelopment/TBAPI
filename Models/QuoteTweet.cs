using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Models
{
    public class QuoteTweet : TweetUser
    {
        public int quoteTweetId { get; set; }
        public string Message { get; set; }
    }
}