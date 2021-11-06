using System.ComponentModel.DataAnnotations;
using TwitterBattlesAPI.Models;
using System.Collections.Generic;

namespace TwitterBattlesAPI.Dtos
{
    public class UserLoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}