namespace TwitterBattlesAPI.Models
{
    public class TweetUser
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int TweetId { get; set; }
        public virtual Tweet Tweet { get; set; }
    }
}