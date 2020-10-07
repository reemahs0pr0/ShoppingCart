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
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            //create cart list to store items details
            List<Cart> cart = new List<Cart>();

            //retrieve records from Cart from userId
            if (HttpContext.Session.GetString("guid") == null)
            {
                cart = ShoppingCartData.GetCart(HttpContext.Session.GetString("userid"));
            }
            //replace GUID with real userID after logging in
            else
            {
                ShoppingCartData.UpdateId(HttpContext.Session.GetString("userid"), 
                    HttpContext.Session.GetString("guid"));
                cart = ShoppingCartData.GetCart(HttpContext.Session.GetString("userid"));
            }

            //create variable to store total price
            double total = 0;

            //add price of each item into total
            foreach (var item in cart)
            {
                total += item.Price * item.Quantity;
            }

            //send data to View
            if (total == 0)
                ViewData["total"] = "0.00";
            else
                ViewData["total"] = total.ToString("#0.00");
            ViewData["cart"] = cart;
            ViewData["images_prefix"] = "/img/";

            // to highlight "Shopping" as the selected menu-item
            ViewData["Is_Shopping"] = "menu_hilite";

            // inform HTML if user is guest
            ViewData["guest"] = HttpContext.Session.GetInt32("guest");

            //html response
            return View();
        }

        //action method to receive AJAX call
        [HttpPost]
        public IActionResult AddItem([FromBody] Add add)
        {
            //if user is not logged in, server will create GUID to act as userId
            if (HttpContext.Session.GetString("userid") == null)
            {
                HttpContext.Session.SetString("userid", Guid.NewGuid().ToString());
                //extra key-value to confirm if guest
                HttpContext.Session.SetInt32("guest", 1);
            }

            //store identifier as Add object and convert to integer
            int productId = Convert.ToInt32(add.Id);

            //send identifier to database to update quantity record
            ShoppingCartData.AddItem(HttpContext.Session.GetString("userid"), productId);

            return Json(new
            {
                success = true
            });
        }

        //action method to receive AJAX call
        [HttpPost]
        public IActionResult UpdateQuantity([FromBody] Update update)
        {
            //store identifier and quantity as Update object and convert to integer
            int productId = Convert.ToInt32(update.Id);
            int quantity = Convert.ToInt32(update.Quantity);

            //send identifier to database to update quantity record
            ShoppingCartData.UpdateQuantity(HttpContext.Session.GetString("userid"), productId, 
                quantity);

            return Json(new
            {
                success = true
            });
        }

        //action method to receive AJAX call
        [HttpPost]
        public IActionResult RemoveItem([FromBody] Remove remove)
        {
            //store identifier as Remove object and convert to integer
            int productId = Convert.ToInt32(remove.Id);

            //send identifier to database to remove record
            ShoppingCartData.RemoveItem(HttpContext.Session.GetString("userid"), productId);

            return Json(new
            {
                success = true
            });
        }
    }
}