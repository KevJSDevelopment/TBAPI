using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Dtos
{
    public class UserCreateDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ImageUrl { get; set; }
    }
}