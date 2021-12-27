using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwitterBattlesAPI.Models
{
    public class Bookmark
    {
        [Key]
        public int BookmarkId { get; set; }

        public int UserId { get; set; }

        public int TweetId { get; set; }
    }
}