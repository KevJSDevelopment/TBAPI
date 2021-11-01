using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Models
{
    public class Like : TweetUser
    {
        [Key]
        public int LikeId { get; set; }
        
    }
}