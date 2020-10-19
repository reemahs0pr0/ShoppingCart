using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.ViewModels
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; }
        public List<Product> SearchedProducts { get;  set; }
        public List<Purchases> TopProducts { get; set; }
        public List<int> Top3Qty { get; set; }
        public List<Wishlist> Wishlists { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
    }
}
