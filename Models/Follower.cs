using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Models
{
    public class Follower
    {
        
        public int UserThatFollowedId { get; set; }

        public int UserBeingFollowedId { get; set; }

    }
}