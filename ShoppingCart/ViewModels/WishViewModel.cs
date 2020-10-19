using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.ViewModels
{
    public class WishViewModel
    {
        public List<Wishlist> WishList { get; set; }
        public int Count { get; set; }
    }
}
