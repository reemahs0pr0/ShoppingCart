using ShoppingCart.Db;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.DAL
{
    public class OrdersDAL
    {
        private readonly DbGallery db;

        public OrdersDAL(DbGallery db)
        {
            this.db = db;
        }

        public List<Order> GetOrders(string userId)
        {
            List<Order> orders = db.Orders.Where(x => x.UserId == userId).OrderByDescending(x => x.Id).ToList();

            return orders;
        }

        public void AddOrder(string userId)
        {
            db.Orders.Add(new Order
            {
                UserId = userId,
                PurchaseDate = DateTime.Now.ToString("dd-MM-yyyy")
            });
            db.SaveChanges();
        }

        public int GetOrderId(string userId)
        {
            int orderId = db.Orders.Where(x => x.UserId == userId).Max(x => x.Id);

            return orderId;
        }
    }
}
