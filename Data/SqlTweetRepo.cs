using System;
using System.Collections.Generic;
using System.Linq;
using TwitterBattlesAPI.Models;

namespace TwitterBattlesAPI.Data
{
    public class SqlTweetRepo
    {
        private readonly TwitterBattleContext _context;

        public SqlTweetRepo(TwitterBattleContext context)
        {
            _context = context;
        }

        public void CreateTweet(Tweet tweet)
        {
            if(tweet == null)
            {
                throw new ArgumentNullException(nameof(tweet));
            }

            _context.Tweets.Add(tweet);
        }

        public void DeleteTweet(Tweet tweet)
        {
            if(tweet == null)
            {
                throw new ArgumentNullException(nameof(tweet));
            }

            _context.Tweets.Remove(tweet);
        }

        public IEnumerable<Tweet> GetAllTweets(string username)
        {
            var user = _context.Users.FirstOrDefault(p => p.Username == username);

            return user.Tweets.ToList();
        }

        public Tweet GetTweetById(int id)
        {
            return _context.Tweets.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            // save changes to database, return true if successful or false if fails
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateTweet(Tweet tweet)
        {
            //Nothing
        }
    }
}