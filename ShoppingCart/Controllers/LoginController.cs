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
                List<Cart> carts = db.Carts.Where(x => x.UseridOrSessionid == userId).ToList();
                foreach (Cart cart in carts)
                {
                    db.Carts.Remove(cart);
                }
                db.SaveChanges();

                //update new cart with user id
                List<Cart> carts1 = db.Carts.Where(x => x.UseridOrSessionid == HttpContext.Session.GetString("sessionid")).ToList();
                foreach(Cart cart in carts1)
                {
                    cart.UseridOrSessionid = userId;
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
                List<Cart> carts = db.Carts.Where(x => x.UseridOrSessionid == userId).ToList();
                foreach (Cart cart in carts)
                {
                    db.Carts.Remove(cart);
                }
                db.SaveChanges();

                //update new cart with user id
                List<Cart> carts1 = db.Carts.Where(x => x.UseridOrSessionid == HttpContext.Session.GetString("sessionid")).ToList();
                foreach (Cart cart in carts1)
                {
                    cart.UseridOrSessionid = userId;
                }
                db.SaveChanges();

                //set 'name' key with name of user
                var name = db.Users.Where(x => x.Id == userId).Select(x => x.Name).Single();
                HttpContext.Session.SetString("name", name);

                return RedirectToAction("DisplayCart", "Cart");
            }
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            int usernameInDb = db.Users.Where(x => x.Username == user.Username).Count();
            string lastUserId = db.Users.OrderByDescending(x => x.Id).Select(x => x.Id).First();
            user.Id = Convert.ToString(Convert.ToInt32(lastUserId) + 1);

            if (usernameInDb == 0)
            {
                db.Users.Add(new User
                {
                    Id = user.Id,
                    Username = user.Username,
                    Password = user.Password,
                    Name = user.Name
                });
                db.SaveChanges();

                if (HttpContext.Session.GetString("userid") != null)
                {
                    //if user has added to cart before registering

                    //store GUID as session id
                    HttpContext.Session.SetString("sessionid", HttpContext.Session.GetString("userid"));

                    //update new cart with user id
                    List<Cart> carts1 = db.Carts.Where(x => x.UseridOrSessionid == HttpContext.Session.GetString("sessionid")).ToList();
                    foreach (Cart cart in carts1)
                    {
                        cart.UseridOrSessionid = user.Id;
                    }
                    db.SaveChanges();
                }

                HttpContext.Session.SetString("userid", user.Id);
                HttpContext.Session.SetString("name", user.Name);

                return RedirectToAction("DisplayProduct", "Product");
            }
            else
            {
                ViewData["errmsg"] = "Username has been taken.";

                return View();
            }
        }
        public IActionResult Register2()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register2(User user)
        {
            int usernameInDb = db.Users.Where(x => x.Username == user.Username).Count();
            string lastUserId = db.Users.OrderByDescending(x => x.Id).Select(x => x.Id).First();
            user.Id = Convert.ToString(Convert.ToInt32(lastUserId) + 1);

            if (usernameInDb == 0)
            {
                db.Users.Add(new User
                {
                    Id = user.Id,
                    Username = user.Username,
                    Password = user.Password,
                    Name = user.Name
                });
                db.SaveChanges();

                if (HttpContext.Session.GetString("userid") != null)
                {
                    //if user has added to cart before registering

                    //store GUID as session id
                    HttpContext.Session.SetString("sessionid", HttpContext.Session.GetString("userid"));

                    //update new cart with user id
                    List<Cart> carts1 = db.Carts.Where(x => x.UseridOrSessionid == HttpContext.Session.GetString("sessionid")).ToList();
                    foreach (Cart cart in carts1)
                    {
                        cart.UseridOrSessionid = user.Id;
                    }
                    db.SaveChanges();
                }

                HttpContext.Session.SetString("userid", Convert.ToString(Convert.ToInt32(lastUserId) + 1));
                HttpContext.Session.SetString("name", user.Name);

                return RedirectToAction("DisplayCart", "Cart");
            }
            else
            {
                ViewData["errmsg"] = "Username has been taken.";

                return View();
            }
        }
    }
}
