using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class WishController : Controller
    {
        public IActionResult Index()
        {
            //create product list to store items details
            List<Product> productlists = ProductData.GetAllProducts();

            //get WishList if logged in
            List<Product> wishlist = new List<Product>();
            if (Int32.TryParse(HttpContext.Session.GetString("userid"),
              out int userId2) == true)
            {
                wishlist = WishData.GetWishList(productlists, userId2);
            }

            ViewData["wishlist"] = wishlist;


            return View();
        }

        [HttpPost]
        public IActionResult AddToWishList([FromBody] Add add)
        {
            //store identifier as Add object and convert to integer
            int productId = Convert.ToInt32(add.Id);

            //send identifier to database to update quantity record
            WishData.AddItem(HttpContext.Session.GetString("userid"), productId);

            return Json(new
            {
                success = true
            });
        }
        [HttpPost]
        public IActionResult RemoveFromWishList([FromBody] Remove remove)
        {
            //store identifier as Remove object and convert to integer
            int productId = Convert.ToInt32(remove.Id);

            //send identifier to database to remove record
            WishData.RemoveItem(HttpContext.Session.GetString("userid"), productId);

            return Json(new
            {
                success = true
            });
        }
    }
}
