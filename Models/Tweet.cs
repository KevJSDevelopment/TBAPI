using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TwitterBattlesAPI.ModelHelpers;

namespace TwitterBattlesAPI.Models
{
    public class Tweet
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public byte[] media { get; set; }

    }
}