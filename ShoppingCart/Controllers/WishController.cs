using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Data;
using ShoppingCart.Db;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class WishController : Controller
    {
        private readonly DbGallery db;

        public WishController(DbGallery db)
        {
            this.db = db;
        }

        public IActionResult DisplayWish()
        {
            //get WishList
            List<Wishlist> wishlist = db.Wishlists.Where(x => x.UserId == HttpContext.Session.GetString("userid")).ToList();

            //check if there is any pre-existing item in cart
            int count = db.Carts.Where(x => x.UserId == HttpContext.Session.GetString("userid")).Count();
            if (count != 0)
            {
                count = db.Carts.Where(x => x.UserId == HttpContext.Session.GetString("userid")).Sum(x => x.Quantity);
            }

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

            //send identifier to database to add wishlist record
            db.Wishlists.Add(new Wishlist
            {
                UserId = HttpContext.Session.GetString("userid"),
                ProductId = productId
            });
            db.SaveChanges();

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

            //send identifier to database to remove wishlist record
            Wishlist wish = (Wishlist)db.Wishlists.Where(x => x.UserId == HttpContext.Session.GetString("userid") && x.ProductId == productId);
            db.Wishlists.Remove(wish);
            db.SaveChanges();

            return Json(new
            {
                success = true
            });
        }
    }
}