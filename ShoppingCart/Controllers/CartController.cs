using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Db;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        private readonly DbGallery db;

        public CartController(DbGallery db)
        {
            this.db = db;
        }

        public IActionResult DisplayCart()
        {
            //store in-cart items for the user in a list
            List<Cart> cart = db.Carts.Where(x => x.UserId == HttpContext.Session.GetString("userid")).ToList();
            
            //create variable to store total price
            double total = 0;

            //add price of each item into total
            foreach (var item in cart)
            {
                total += item.Product.Price * item.Quantity;
            }

            //to display 'total' in html
            if (total == 0)
                ViewData["total"] = "0.00";
            else
                ViewData["total"] = total.ToString("#0.00");

            //send other data to View
            ViewData["cart"] = cart;
            ViewData["images_prefix"] = "/img/";

            // to highlight "Shopping" as the selected menu-item
            ViewData["Is_Shopping"] = "menu_hilite";

            // inform HTML if user is guest
            ViewData["loggedin"] = Int32.TryParse(HttpContext.Session.GetString("userid"), 
                out int userId);

            return View();
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
            int checkItemInCart = db.Carts.Where(x => x.UserId == HttpContext.Session.GetString("userid") && x.ProductId == productId).Count();
            if (checkItemInCart == 0)
            {
                db.Carts.Add(new Cart
                {
                    UserId = HttpContext.Session.GetString("userid"),
                    ProductId = productId,
                    Quantity = 1
                });
            }
            else
            {
                Cart cart = (Cart)db.Carts.Where(x => x.UserId == HttpContext.Session.GetString("userid") && x.ProductId == productId);
                cart.Quantity++;
            }
            db.SaveChanges();

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
            Cart cart = (Cart)db.Carts.Where(x => x.UserId == HttpContext.Session.GetString("userid") && x.ProductId == productId);
            cart.Quantity = quantity;
            db.SaveChanges();

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
            Cart cart = (Cart)db.Carts.Where(x => x.UserId == HttpContext.Session.GetString("userid") && x.ProductId == productId);
            db.Carts.Remove(cart);
            db.SaveChanges();

            return Json(new
            {
                success = true
            });
        }
    }
}