using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Models
{
    public class Wishlist
    {
        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
