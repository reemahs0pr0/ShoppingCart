using ShoppingCart.Db;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.DAL
{
    public class OrderDetailsDAL
    {
        private readonly DbGallery db;

        public OrderDetailsDAL(DbGallery db)
        {
            this.db = db;
        }

        public List<OrderDetail> GetOrderDetails(int orderId)
        {
            List<OrderDetail> orderDetails = db.OrderDetails.Where(x => x.OrderId == orderId).ToList();

            return orderDetails;
        }

        public void AddOrderDetails(int orderId, Cart item)
        {
            db.OrderDetails.Add(new OrderDetail
            {
                OrderId = orderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            });

            db.SaveChanges();
        }
    }
}
