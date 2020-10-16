using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Models
{
    public class User
    {
        public string Id { get; set; }

        [DisplayName("Username:")]
        [Required]
        public string Username { get; set; }

        [DisplayName("Password:")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
