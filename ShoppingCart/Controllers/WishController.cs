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
        public IActionResult DisplayWish()
        {
            //get WishList if logged in
            List<Wish> wishlist = WishData.GetWishList(HttpContext.Session.GetString("userid"));

            //check if there is any pre-existing item in cart
            int count = CartData.CheckLastInCart(HttpContext.Session.GetString("userid"));

            //send data to View
            ViewData["count"] = count;
            ViewData["wishlist"] = wishlist;
            ViewData["images_prefix"] = "/img/";

            // to highlight "Shopping" as the selected menu-item
            ViewData["Is_Shopping"] = "menu_hilite";

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