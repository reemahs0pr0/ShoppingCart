using ShoppingCart.Db;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.DAL
{
    public class CartsDAL
    {
        private readonly DbGallery db;

        public CartsDAL(DbGallery db)
        {
            this.db = db;
        }

        public List<Cart> GetCart(string id)
        {
            List<Cart> cart = db.Carts.Where(x => x.UseridOrSessionid == id).ToList();

            return cart;
        }

        public void AddItem(string id, int productId)
        {
            int checkItemInCart = db.Carts.Where(x => x.UseridOrSessionid == id && x.ProductId == productId).Count();
            if (checkItemInCart == 0)
            {
                db.Carts.Add(new Cart
                {
                    UseridOrSessionid = id,
                    ProductId = productId,
                    Quantity = 1
                });
            }
            else
            {
                Cart cart = db.Carts.Where(x => x.UseridOrSessionid == id && x.ProductId == productId).Single();
                cart.Quantity++;
            }
            db.SaveChanges();
        }

        public void UpdateQuantity(string id, int productId, int quantity)
        {
            Cart cart = db.Carts.Where(x => x.UseridOrSessionid == id && x.ProductId == productId).Single();
            cart.Quantity = quantity;
            db.SaveChanges();
        }

        public void RemoveItem(string id, int productId)
        {
            Cart cart = db.Carts.Where(x => x.UseridOrSessionid == id && x.ProductId == productId).Single();
            db.Carts.Remove(cart);
            db.SaveChanges();
        }

        public void RemoveItem(Cart item)
        {
            db.Carts.Remove(item);
            db.SaveChanges();
        }

        public void DeleteCart(string userId)
        {
            List<Cart> carts = GetCart(userId);
            foreach (Cart cart in carts)
            {
                db.Carts.Remove(cart);
            }
            db.SaveChanges();
        }

        public void UpdateId(string userId, string guid)
        {
            List<Cart> carts = GetCart(guid);
            foreach (Cart cart in carts)
            {
                cart.UseridOrSessionid = userId;
            }
            db.SaveChanges();
        }

        public int CheckLastInCart(string userId)
        {
            int count = db.Carts.Where(x => x.UseridOrSessionid == userId).Count();
            if (count != 0)
            {
                count = db.Carts.Where(x => x.UseridOrSessionid == userId).Sum(x => x.Quantity);
            }

            return count;
        }
    }
}
