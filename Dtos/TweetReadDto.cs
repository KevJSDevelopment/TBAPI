using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Dtos
{
    public class TweetReadDto
    {
        [Required]
        public int TweetId { get; set; }
        [Required]
        public int UserId { get; set; }

        public string Message { get; set; }
    }
}