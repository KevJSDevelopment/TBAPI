using System;
using System.Collections.Generic;
using System.Linq;
using TwitterBattlesAPI.Models;

namespace TwitterBattlesAPI.Data
{
    public class SqlUserRepo : IUserRepo
    {
        private readonly TwitterBattleContext _context;

        public SqlUserRepo(TwitterBattleContext context)
        {
            _context = context;
        }

        public void CreateUser(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Users.Add(user);
        }

        public void DeleteUser(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Users.Remove(user);
        }

        public void DeleteTweet(Tweet tweet)
        {
            if(tweet == null)
            {
                throw new ArgumentNullException(nameof(tweet));
            }

            _context.Tweets.Remove(tweet);
        }

        public ICollection<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(p => p.Username == username);
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(p => p.UserId == id);
        }

        public Follower GetFollowerById(int id)
        {
            return _context.Followers.FirstOrDefault(p => p.FollowerId == id);
        }

        public Tweet GetTweetById(int id)
        {
            return _context.Tweets.FirstOrDefault(p => p.TweetId == id);
        }

        public bool SaveChanges()
        {
            // save changes to database, return true if successful or false if fails
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateUser(User user)
        {
            //Nothing
        }

        public void AddTweet(Tweet tweet)
        {
            if(tweet == null)
            {
                throw new ArgumentNullException(nameof(tweet));
            }

            _context.Tweets.Add(tweet);
        }

        public ICollection<Tweet> GetTweets(int userId) 
        {
            var tweets = _context.Tweets.Where(tweet => tweet.UserId == userId).ToList();

            tweets.Sort((a, b) => DateTime.Compare(b.CreatedDate, a.CreatedDate));

            return tweets;
        }

        public ICollection<Tweet> GetTweetFeed(int userId) 
        {
            var tweets = _context.Tweets.ToList();

            tweets.Sort((a, b) => DateTime.Compare(b.CreatedDate, a.CreatedDate));

            return tweets;
        }

        public void LikeTweet(Like like){
            _context.Likes.Add(like);
        }

        public void UnlikeTweet(Like like){

            if(like == null)
            {
                throw new ArgumentNullException(nameof(like));
            }

            _context.Likes.Remove(like);
        }

        public void Retweet(Retweet retweet){
            // may need updates
            _context.Retweets.Add(retweet);
        }

        public void Unretweet(Retweet retweet){

            if(retweet == null)
            {
                throw new ArgumentNullException(nameof(retweet));
            }

            _context.Retweets.Remove(retweet);
        }

        public void QuoteTweet(QuoteTweet quoteTweet){
            _context.QuoteTweets.Add(quoteTweet);
        }

        public void DeleteQuoteTweet(QuoteTweet quoteTweet){

            if(quoteTweet == null)
            {
                throw new ArgumentNullException(nameof(quoteTweet));
            }

            _context.QuoteTweets.Remove(quoteTweet);
        }


        public ICollection<Like> GetLikes(int tweetId){
            // change, temporary code
            var likes = _context.Likes.Where(l => l.TweetId == tweetId).ToList();

            return likes;
        }


        public Like CheckLike(int userId, int tweetId){
            // change, temporary code
            var like = _context.Likes.FirstOrDefault(l => l.TweetId == tweetId && l.UserId == userId);

            if(like != null){
                User user = GetUserById(like.UserId);
                Tweet tweet = GetTweetById(like.TweetId);
                like.User = user;
                like.Tweet = tweet;
            }

            return like;
        }

        public ICollection<Retweet> GetRetweets(int tweetId){
            // change, temporary code
            var retweets = _context.Retweets.Where(r => r.TweetId == tweetId).ToList();

            return retweets;
        }

        public Retweet CheckRetweet(int userId, int tweetId){
            // change, temporary code
            var retweet = _context.Retweets.FirstOrDefault(r => r.TweetId == tweetId && r.UserId == userId);

            if(retweet != null){
                User user = GetUserById(retweet.UserId);
                Tweet tweet = GetTweetById(retweet.TweetId);
                retweet.User = user;
                retweet.Tweet = tweet;
            }

            return retweet;
        }

        public ICollection<QuoteTweet> GetQuoteTweets(int tweetId){
            // change, temporary code
            var quoteTweets = _context.QuoteTweets.Where(q => q.TweetId == tweetId).ToList();

            return quoteTweets;
        }


        public ICollection<Like> GetUserLikes(int userId){
            // change, temporary code
            var likes = _context.Likes.Where(l => l.UserId == userId).ToList();

            return likes;
        }


    }
}