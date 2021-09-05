using System.Collections.Generic;
using TwitterBattlesAPI.Models;

namespace TwitterBattlesAPI.Data
{
    public interface ITweetRepo
    {
        bool SaveChanges();
        IEnumerable<Tweet> GetAllTweets();
        Tweet GetTweetById(int id);
        void CreateTweet(Tweet tweet);
        void DeleteTweet(Tweet tweet);
    }
}