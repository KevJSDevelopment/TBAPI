using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Dtos
{
    public class TweetCreateDto
    {
        [Required]
        public string Message { get; set; }

        public byte[] media { get; set; }

    }
}