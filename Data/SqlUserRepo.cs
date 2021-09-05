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

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(p => p.Id == id);
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
    }
}