using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public bool Wish { get; set; }
        public Product()
        {
            Wish = false;
        }
    }
}
