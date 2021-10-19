using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string Password { get; set; }

        public byte[] ImageFiles { get; set; }  

        public ICollection<Tweet> Tweets { get; set; }
        
        public ICollection<Follower> Followers { get; set; }

    }
}