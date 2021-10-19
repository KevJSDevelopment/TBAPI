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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasAlternateKey(k => k.Username)
            .HasName("AlternateKey_Username");
        }
    }
}