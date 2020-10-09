using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Models
{
    public class Purchases
    {
        public int OrderId { set; get; }
        public int ProductId { set; get; }
        public int Quantity { set; get; }
        public DateTime PurchaseDate { set; get; }
        public string Image { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public List<string> ActivationCode { set; get; }

        public Purchases()
        {
            ActivationCode = new List<string>();
        }
    }
}
