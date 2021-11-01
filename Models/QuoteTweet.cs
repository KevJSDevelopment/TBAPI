using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Models
{
    public class QuoteTweet : TweetUser
    {
        [Key]
        public int QuoteTweetId { get; set; }
        [Required]
        public string message { get; set; }
    }
}