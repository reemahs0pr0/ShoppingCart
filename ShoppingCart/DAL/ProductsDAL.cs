using ShoppingCart.Db;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.DAL
{
    public class ProductsDAL
    {
        private readonly DbGallery db;

        public ProductsDAL(DbGallery db)
        {
            this.db = db;
        }

        public List<Product> GetAllProducts()
        {
            List<Product> productlists = db.Products.ToList();

            return productlists;
        }

        public int GetNoOfOrders()
        {
            int order = db.OrderDetails.Count();

            return order;
        }

        public dynamic GetTopSellingProduct()
        {
            var list = db.OrderDetails.GroupBy(x => x.ProductId).Select(x => new { x.Key, TotalQty = x.Sum(x => x.Quantity) }).OrderByDescending(x => x.TotalQty).ToList();

            return list;
        }

        public List<Product> GetProductSearch(string search)
        {
            List<Product> searchedproductlists = db.Products.Where(x => x.Description.Contains(search)).ToList();

            return searchedproductlists;
        }
    }
}
