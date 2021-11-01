using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Models
{
    public class Retweet : TweetUser
    {
        [Key]
        public int RetweetId { get; set; }
    }
}