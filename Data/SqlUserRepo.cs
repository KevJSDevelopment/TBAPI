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

        // public Follower GetFollowerById(int id)
        // {
        //     return _context.Followers.FirstOrDefault(p => p.FollowerId == id);
        // }

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

            var tweets = _context.Tweets.Where(tweet => tweet.RepliedToTweetId == 0).ToList();

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


        public ICollection<Like> GetTweetLikes(int tweetId){
            var likes = _context.Likes.Where(l => l.TweetId == tweetId).ToList();

            return likes;
        }


        public Like CheckLike(int userId, int tweetId){
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
            var retweets = _context.Retweets.Where(r => r.TweetId == tweetId).ToList();

            return retweets;
        }

        public Retweet CheckRetweet(int userId, int tweetId){
            var retweet = _context.Retweets.FirstOrDefault(r => r.TweetId == tweetId && r.UserId == userId);

            if(retweet != null){
                User user = GetUserById(retweet.UserId);
                Tweet tweet = GetTweetById(retweet.TweetId);
                retweet.User = user;
                retweet.Tweet = tweet;
            }

            return retweet;
        }

        public ICollection<Tweet> GetTweetReplies(int tweetId){
            var replies = _context.Tweets.Where(q => q.RepliedToTweetId == tweetId).ToList();

            return replies;
        }

        public ICollection<Tweet> GetUserTweets(int userId){
            var tweets = _context.Tweets.Where(l => l.UserId == userId && l.RepliedToTweetId == 0).ToList();

            return tweets;
        }

        public ICollection<Tweet> GetUserTweetsAndReplies(int userId){
            var tweetsAndReplies = _context.Tweets.Where(l => l.UserId == userId).ToList();


            return tweetsAndReplies;
        }

        public ICollection<Tweet> GetUserMediaTweets(int userId){
            var mediaTweets = _context.Tweets.Where(t => (t.UserId == userId) && (t.Media != null) && (t.Media.Length > 0)).ToList();

            return mediaTweets;
        }

        
        public ICollection<Tweet> GetUserLikes(int userId){
            var likes = _context.Likes.Where(l => l.UserId == userId).ToList();

            List<Tweet> tweets = new List<Tweet>();

            foreach (var like in likes)
            {
                var tweet = _context.Tweets.FirstOrDefault(t => t.TweetId == like.TweetId);
                tweets.Add(tweet);
            }

            return tweets;
        }

        public void AddWallet(WalletAddress walletAddress){
            if(walletAddress == null)
            {
                throw new ArgumentNullException(nameof(walletAddress));
            }

            _context.WalletAddresses.Add(walletAddress);
        }

        public ICollection<WalletAddress> GetWalletsByUserId(int userId){
            var wallets = _context.WalletAddresses.Where(w => w.UserId == userId).ToList();
            
            return wallets;
        }

        public void AddBookmark(Bookmark bookmark){ 
            if(bookmark == null)
            {
                throw new ArgumentNullException(nameof(bookmark));
            }

            _context.Bookmarks.Add(bookmark);
        }

        public void AddMessage(Message message){
             if(message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            _context.Messages.Add(message);
        }

        public ICollection<Tweet> GetBookmarks(int userId) {
            var bookmarks = _context.Bookmarks.Where(b => b.UserId == userId).ToList();
            List<Tweet> bookmarkedTweets = new List<Tweet>();

            foreach (var bookmark in bookmarks)
            {
                var tweet = _context.Tweets.FirstOrDefault(t => t.TweetId == bookmark.TweetId);
                bookmarkedTweets.Add(tweet);
            }

            bookmarkedTweets.Sort((a, b) => DateTime.Compare(b.CreatedDate, a.CreatedDate));

            return bookmarkedTweets;
        }

        public ICollection<Notification> GetNotifications(int userId) {
            var notifications = _context.Notifications.Where(b => b.UserId == userId).ToList();

            notifications.Sort((a, b) => DateTime.Compare(b.CreatedDate, a.CreatedDate));
            
            return notifications;
        }


        public ICollection<Message> GetMessages(int loggedInUserId, int otherUserId){
            var messages = _context.Messages.Where(m => m.UserReceivingMessageId == loggedInUserId &&  m.UserWhoMessagedId == loggedInUserId || m.UserWhoMessagedId == loggedInUserId &&  m.UserReceivingMessageId == loggedInUserId).ToList();

            messages.Sort((a, b) => DateTime.Compare(b.CreatedDate, a.CreatedDate));
            
            return messages;
        }

        public void AddFollow(Follower follower){

            var followerCheck = _context.Followers.FirstOrDefault(f => f.UserBeingFollowedId == follower.UserBeingFollowedId && f.UserThatFollowedId == follower.UserThatFollowedId);

            if(followerCheck != null)
            {
                var user1 = _context.Users.FirstOrDefault(u => u.UserId == follower.UserThatFollowedId);
                var user2 = _context.Users.FirstOrDefault(u => u.UserId == follower.UserBeingFollowedId);
                throw new ArgumentException($"{user1.Username} is already following {user2.Username}");
            }

            _context.Followers.Add(follower);
        }

        public void DeleteFollow(Follower follower){

            if(follower == null)
            {
                throw new ArgumentNullException(nameof(follower));
            }

            _context.Followers.Remove(follower);
        }


        public Follower CheckFollowing(int userThatFollowedId, int userBeingFollowedId){

            Follower follower = _context.Followers.FirstOrDefault(f => f.UserThatFollowedId == userThatFollowedId && f.UserBeingFollowedId == userBeingFollowedId);

            return follower;
        }

        public ICollection<Follower> GetFollowers(int userId){
            var followers = _context.Followers.Where(f => f.UserBeingFollowedId == userId).ToList();

            return followers;
        }

        public ICollection<Follower> GetFollowing(int userId){
            var following = _context.Followers.Where(f => f.UserThatFollowedId == userId).ToList();

            return following;
        }


    }
}