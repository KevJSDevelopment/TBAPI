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

        public DbSet<UserFollower> UserFollowers { get; set; }

        public DbSet<Like> Likes { get; set; }

        public DbSet<Retweet> Retweets { get; set; }

        public DbSet<QuoteTweet> QuoteTweets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasAlternateKey(k => k.Username)
            .HasName("AlternateKey_Username");

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

            modelBuilder.Entity<QuoteTweet>().HasKey(qt => new { qt.quoteTweetId });

            modelBuilder.Entity<QuoteTweet>()
            .HasOne(u => u.User)
            .WithMany(tu => tu.QuoteTweets)
            .HasForeignKey(ti => ti.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuoteTweet>()
            .HasOne(u => u.Tweet)
            .WithMany(tu => tu.UserReplies)
            .HasForeignKey(ti => ti.TweetId)
            .OnDelete(DeleteBehavior.Restrict);
        }
        
    }
}