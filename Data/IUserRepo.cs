using System.Collections.Generic;
using TwitterBattlesAPI.Models;

namespace TwitterBattlesAPI.Data
{
    public interface IUserRepo
    {
        bool SaveChanges();
        ICollection<User> GetAllUsers();
        User GetUserByUsername(string username);
        User GetUserById(int id);
        Follower GetFollowerById(int id);
        Tweet GetTweetById(int id);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        void DeleteTweet(Tweet tweet);
        void AddTweet(Tweet tweet);
        void LikeTweet(Like like);
        void UnlikeTweet(Like like);
        void Retweet(Retweet retweet);
        void Unretweet(Retweet retweet);

        Like CheckLike(int userId, int tweetId);
        ICollection<Like> GetTweetLikes(int tweetId);
        Retweet CheckRetweet(int userId, int tweetId);
        ICollection<Retweet> GetRetweets(int tweetId);
        ICollection<Tweet> GetTweetReplies(int tweetId);

        
        ICollection<Tweet> GetUserTweets(int userId);
        ICollection<Tweet> GetUserTweetsAndReplies(int userId);
        ICollection<Tweet> GetUserMediaTweets(int userId);
        ICollection<Tweet> GetUserLikes(int userId);

        ICollection<Tweet> GetTweets(int userId);
        ICollection<Tweet> GetTweetFeed(int userId);
    }
}