using System.ComponentModel.DataAnnotations;
using TwitterBattlesAPI.Models;

namespace TwitterBattlesAPI.Dtos
{
    public class TweetCreateDto
    {
        [Required]
        public User User { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Message { get; set; }
    }
}