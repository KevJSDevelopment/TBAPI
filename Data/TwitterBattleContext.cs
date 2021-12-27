using Microsoft.EntityFrameworkCore;
using TwitterBattlesAPI.Models;

namespace TwitterBattlesAPI.Data
{
    public class TwitterBattleContext : DbContext
    {
        public TwitterBattleContext(DbContextOptions<TwitterBattleContext> opt) : base(opt)
        {

        }


        public DbSet<User> Users { get; set; }

        public DbSet<Tweet> Tweets { get; set; }

        public DbSet<Follower> Followers { get; set; }

        public DbSet<Like> Likes { get; set; }

        public DbSet<Retweet> Retweets { get; set; }
        
        public DbSet<WalletAddress> WalletAddresses { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Bookmark> Bookmarks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasAlternateKey(k => k.Username)
            .HasName("AlternateKey_Username");

            modelBuilder.Entity<WalletAddress>()
            .HasAlternateKey(k => k.Address)
            .HasName("AlternateKey_Address");

            modelBuilder.Entity<Follower>().HasKey(f => new { f.UserThatFollowedId, f.UserBeingFollowedId });

            modelBuilder.Entity<Like>().HasKey(l => new { l.UserId, l.TweetId });

            modelBuilder.Entity<Like>()
            .HasOne(u => u.User)
            .WithMany(tu => tu.LikedTweets)
            .HasForeignKey(ti => ti.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>()
            .HasOne(u => u.Tweet)
            .WithMany(tu => tu.UserLikes)
            .HasForeignKey(ti => ti.TweetId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Retweet>().HasKey(r => new { r.UserId, r.TweetId });

            modelBuilder.Entity<Retweet>()
            .HasOne(u => u.User)
            .WithMany(tu => tu.Retweets)
            .HasForeignKey(ti => ti.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Retweet>()
            .HasOne(u => u.Tweet)
            .WithMany(tu => tu.UserRetweets)
            .HasForeignKey(ti => ti.TweetId)
            .OnDelete(DeleteBehavior.Restrict);
            
        }
        
    }
}