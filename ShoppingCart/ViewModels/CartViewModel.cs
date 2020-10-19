using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.ViewModels
{
    public class CartViewModel
    {
        public string Total { get; set; }
        public bool LoggedIn { get; set; }
        public List<Cart> Cart { get; set; }
        public List<string> Amount { get; set; }
    }
}