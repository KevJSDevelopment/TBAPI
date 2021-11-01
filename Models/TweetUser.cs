using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Models
{
    public class TweetUser
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int TweetId { get; set; }
    }
}