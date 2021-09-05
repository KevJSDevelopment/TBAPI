using System.ComponentModel.DataAnnotations;
using TwitterBattlesAPI.Models;

namespace TwitterBattlesAPI.Dtos
{
    public class TweetReadDto
    {
        public User User { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
    }
}