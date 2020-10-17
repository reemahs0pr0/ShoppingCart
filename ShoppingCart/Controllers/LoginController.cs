using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Db;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class LoginController : Controller
    {
        private readonly DbGallery db;

        public LoginController(DbGallery db)
        {
            this.db = db;
        }

        //when user click 'login'
        public IActionResult Index()
        {
            return View();
        }

        //authenticate user
        [HttpPost]
        public IActionResult Index(User userModel)
        {
            //check if user exist in db
            var userId = db.Users.Where(x => x.Username == userModel.Username && x.Password == userModel.Password).Select(x => x.Id).SingleOrDefault();

            if (userId == null)
            {
                //if cannot find user

                ViewData["errmsg"] = "User not found or incorrect password";

                return View();
            }
            else if (HttpContext.Session.GetString("userid") != null)
            {
                //if user has added to cart before logging in

                //store GUID as session id
                HttpContext.Session.SetString("sessionid", HttpContext.Session.GetString("userid"));

                //replace user id with real user id from db
                HttpContext.Session.SetString("userid", userId);

                //overwrite existing cart with new cart 
                //delete old cart
                List<Cart> carts = db.Carts.Where(x => x.UserId == userId).ToList();
                foreach (Cart cart in carts)
                {
                    db.Carts.Remove(cart);
                }
                db.SaveChanges();

                //update new cart with user id
                List<Cart> carts1 = db.Carts.Where(x => x.UserId == HttpContext.Session.GetString("sessionid")).ToList();
                foreach(Cart cart in carts1)
                {
                    cart.UserId = userId;
                }
                db.SaveChanges();

                //set 'name' key with name of user
                var name = db.Users.Where(x => x.Id == userId).Select(x => x.Name).Single();
                HttpContext.Session.SetString("name", name);

                return RedirectToAction("DisplayProduct", "Product");
            }
            else
            {
                //if user directly login

                //set session with user id and name
                HttpContext.Session.SetString("userid", userId);
                var name = db.Users.Where(x => x.Id == userId).Select(x => x.Name).Single();
                HttpContext.Session.SetString("name", name);

                return RedirectToAction("DisplayProduct", "Product");
            }
        }

        //redirect from guest 'checkout'
        public IActionResult Index2()
        {
            return View();
        }

        //same as HttpPost Index2() but redirect to 'View Cart'
        [HttpPost]
        public IActionResult Index2(User userModel)
        {
            //check if user exist in db
            var userId = db.Users.Where(x => x.Username == userModel.Username && x.Password == userModel.Password).Select(x => x.Id).SingleOrDefault();

            if (userId == null)
            {
                ViewData["errmsg"] = "User not found or incorrect password";
                return View();
            }
            else
            {
                //store GUID as session id
                HttpContext.Session.SetString("sessionid", HttpContext.Session.GetString("userid"));

                //replace user id with real user id from db
                HttpContext.Session.SetString("userid", userId);

                //overwrite existing cart with new cart 
                //delete old cart
                List<Cart> carts = db.Carts.Where(x => x.UserId == userId).ToList();
                foreach (Cart cart in carts)
                {
                    db.Carts.Remove(cart);
                }
                db.SaveChanges();

                //update new cart with user id
                List<Cart> carts1 = db.Carts.Where(x => x.UserId == HttpContext.Session.GetString("sessionid")).ToList();
                foreach (Cart cart in carts1)
                {
                    cart.UserId = userId;
                }
                db.SaveChanges();

                //set 'name' key with name of user
                var name = db.Users.Where(x => x.Id == userId).Select(x => x.Name).Single();
                HttpContext.Session.SetString("name", name);

                return RedirectToAction("DisplayCart", "Cart");
            }
        }
    }
}
