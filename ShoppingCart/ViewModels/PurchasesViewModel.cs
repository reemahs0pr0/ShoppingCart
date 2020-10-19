using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.ViewModels
{
    public class PurchasesViewModel
    {
        public List<Purchases> Purchases { get; set; }
        public string Total { get; set; }
    }
}
