using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Db;
using ShoppingCart.Models;
using ShoppingCart.DAL;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        private readonly DbGallery db;
        private readonly CartsDAL cartsDAL;

        public CartController(DbGallery db)
        {
            this.db = db;
            cartsDAL = new CartsDAL(db);
        }

        public IActionResult DisplayCart()
        {
            CartViewModel cartViewModel = new CartViewModel();

            //get in-cart items for the user in a list
            List<Cart> cart = cartsDAL.GetCart(HttpContext.Session.GetString("userid"));

            //create variable to store total price
            double total = 0;

            //add price of each item into total
            foreach (var item in cart)
            {
                total += item.Product.Price * item.Quantity;
            }

            //to display 'total' in html
            if (total == 0)
                cartViewModel.Total = "0.00";
            else
                cartViewModel.Total = total.ToString("#0.00");

            //send other data to View
            cartViewModel.Cart = cart;
            ViewData["images_prefix"] = "/img/";

            // to highlight "Shopping" as the selected menu-item
            ViewData["Is_Shopping"] = "menu_hilite";

            // inform HTML if user is guest
            cartViewModel.LoggedIn = Int32.TryParse(HttpContext.Session.GetString("userid"), 
                out int userId);

            return View(cartViewModel);
        }

        [HttpPost]
        public IActionResult AddItem([FromBody] Add add)
        {
            //if user is not logged in, server will create GUID to act as userId
            if (HttpContext.Session.GetString("userid") == null)
            {
                HttpContext.Session.SetString("userid", Guid.NewGuid().ToString());
            }

            //store identifier as Add object and convert to integer
            int productId = Convert.ToInt32(add.Id);

            //send identifier to database to update quantity record
            cartsDAL.AddItem(HttpContext.Session.GetString("userid"), productId);

            return Json(new
            {
                success = true
            });
        }

        [HttpPost]
        public IActionResult UpdateQuantity([FromBody] Update update)
        {
            //store identifier and quantity as Update object and convert to integer
            int productId = Convert.ToInt32(update.Id);
            int quantity = Convert.ToInt32(update.Quantity);

            //send identifier to database to update quantity record
            cartsDAL.UpdateQuantity(HttpContext.Session.GetString("userid"), productId, quantity);

            return Json(new
            {
                success = true
            });
        }

        [HttpPost]
        public IActionResult RemoveItem([FromBody] Remove remove)
        {
            //store identifier as Remove object and convert to integer
            int productId = Convert.ToInt32(remove.Id);

            //send identifier to database to remove record
            cartsDAL.RemoveItem(HttpContext.Session.GetString("userid"), productId);

            return Json(new
            {
                success = true
            });
        }
    }
}