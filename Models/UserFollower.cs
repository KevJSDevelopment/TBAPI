using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Models
{
    public class UserFollower
    {
        [Key]
        public int UserFollowerId { get; set; }
        [Required]
        public int FollowerId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}