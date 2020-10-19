using ShoppingCart.Db;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.DAL
{
    public class UsersDAL
    {
        private readonly DbGallery db;

        public UsersDAL(DbGallery db)
        {
            this.db = db;
        }

        public string FindUserId(User userModel)
        {
            string userId = db.Users.Where(x => x.Username == userModel.Username && x.Password == userModel.Password).Select(x => x.Id).SingleOrDefault();

            return userId;
        }

        public string FindName(string userId)
        {
            string name = db.Users.Where(x => x.Id == userId).Select(x => x.Name).Single();

            return name;
        }

        public int FindUser(string username)
        {
            int usernameInDb = db.Users.Where(x => x.Username == username).Count();

            return usernameInDb;
        }

        public int FindLastUserId()
        {
            string lastUserId = db.Users.OrderByDescending(x => x.Id).Select(x => x.Id).First();

            return Convert.ToInt32(lastUserId);
        }

        public void AddUser(User user)
        {
            db.Users.Add(new User
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Name = user.Name
            });
            db.SaveChanges();
        }
    }
}
