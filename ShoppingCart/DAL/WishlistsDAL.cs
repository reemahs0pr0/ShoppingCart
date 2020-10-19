using ShoppingCart.Db;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.DAL
{
    public class WishlistsDAL
    {
        private readonly DbGallery db;

        public WishlistsDAL(DbGallery db)
        {
            this.db = db;
        }

        public List<Wishlist> GetWishList(string userId)
        {
            List<Wishlist> wishlist = db.Wishlists.Where(x => x.UserId == userId).ToList();

            return wishlist;
        }

        public void AddItem(string userId, int productId)
        {
            db.Wishlists.Add(new Wishlist
            {
                UserId = userId,
                ProductId = productId
            });
            db.SaveChanges();
        }

        public void RemoveItem(string userId, int productId)
        {
            Wishlist wish = db.Wishlists.Where(x => x.UserId == userId && x.ProductId == productId).Single();
            db.Wishlists.Remove(wish);
            db.SaveChanges();
        }
    }
}
