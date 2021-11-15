namespace TwitterBattlesAPI.Models
{
    public class TweetUser
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int TweetId { get; set; }
        public Tweet Tweet { get; set; }
    }
}