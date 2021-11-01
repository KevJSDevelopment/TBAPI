using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Models
{
    public class Follower
    {
        [Key]
        public int FollowerId { get; set; }
        [Required]
        public string Followername { get; set; }
        [Required]
        public string DisplayName { get; set; }
    }
}