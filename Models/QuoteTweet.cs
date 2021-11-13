using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Models
{
    public class QuoteTweet : TweetUser
    {
        public string Message { get; set; }
    }
}