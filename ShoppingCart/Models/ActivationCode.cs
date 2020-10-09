using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Models
{
    public class ActivationCode
    {
        public string Code { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }
}
