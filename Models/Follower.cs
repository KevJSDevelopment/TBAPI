using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Models
{
    public class Follower
    {
        [Key]
        public int FollowerId { get; set; }
        public string Followername { get; set; }
        public string DisplayName { get; set; }
    }
}