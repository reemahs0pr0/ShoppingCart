using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            //simulate extracting userId from Session
            int userId = 1;

            //retrieve records from Cart
            List<Cart> cart = ShoppingCartData.GetCart(userId);

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

            // use sessionId to determine if user has already logged in
            ViewData["sessionId"] = Guid.NewGuid().ToString();
            //ViewData["sessionId"] = null;

            //html response
            return View();
        }

        //action method to receive AJAX call
        [HttpPost]
        public IActionResult AddItem([FromBody] Add add)
        {
            //simulate extracting userId from Session
            int userId = 1;

            //store identifier as Add object and convert to integer
            int productId = Convert.ToInt32(add.Id);

            //send identifier to database to update quantity record
            ShoppingCartData.AddItem(userId, productId);

            return Json(new
            {
                success = true
            });
        }

        //action method to receive AJAX call
        [HttpPost]
        public IActionResult UpdateQuantity([FromBody] Update update)
        {
            //simulate extracting userId from Session
            int userId = 1;

            //store identifier and quantity as Update object and convert to integer
            int productId = Convert.ToInt32(update.Id);
            int quantity = Convert.ToInt32(update.Quantity);

            //send identifier to database to update quantity record
            ShoppingCartData.UpdateQuantity(userId, productId, quantity);

            return Json(new
            {
                success = true
            });
        }

        //action method to receive AJAX call
        [HttpPost]
        public IActionResult RemoveItem([FromBody] Remove remove)
        {
            //simulate extracting userId from Session
            int userId = 1;

            //store identifier as Remove object and convert to integer
            int productId = Convert.ToInt32(remove.Id);

            //send identifier to database to remove record
            ShoppingCartData.RemoveItem(userId, productId);

            return Json(new
            {
                success = true
            });
        }
    }
}