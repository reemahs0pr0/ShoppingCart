using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ShoppingCart.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UseridOrSessionid { get; set; }
        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
