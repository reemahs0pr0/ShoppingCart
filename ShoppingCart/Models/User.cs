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
        public string UserId { get; set; }

        [DisplayName("Username:")]
        public string Username { get; set; }

        [DisplayName("Password:")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
