using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.DAL;
using ShoppingCart.Db;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class WishController : Controller
    {
        private readonly DbGallery db;
        private readonly WishlistsDAL wishlistsDAL;
        private readonly CartsDAL cartsDAL;

        public WishController(DbGallery db)
        {
            this.db = db;
            wishlistsDAL = new WishlistsDAL(db);
            cartsDAL = new CartsDAL(db);
        }

        public IActionResult DisplayWish()
        {
            //get WishList
            List<Wishlist> wishlist = wishlistsDAL.GetWishList(HttpContext.Session.GetString("userid"));

            //check if there is any pre-existing item in cart
            int count = cartsDAL.CheckLastInCart(HttpContext.Session.GetString("userid"));

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
            wishlistsDAL.AddItem(HttpContext.Session.GetString("userid"), productId);

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
            wishlistsDAL.RemoveItem(HttpContext.Session.GetString("userid"), productId);

            return Json(new
            {
                success = true
            });
        }
    }
}