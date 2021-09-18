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
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        void AddTweet(Tweet tweet);
        ICollection<Tweet> GetTweets();
    }
}