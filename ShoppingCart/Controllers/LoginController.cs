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
    public class LoginController : Controller
    {
        private readonly DbGallery db;
        private readonly UsersDAL usersDAL;
        private readonly CartsDAL cartsDAL;

        public LoginController(DbGallery db)
        {
            this.db = db;
            usersDAL = new UsersDAL(db);
            cartsDAL = new CartsDAL(db);
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
            var userId = usersDAL.FindUserId(userModel);

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
                cartsDAL.DeleteCart(userId);

                //update new cart with user id
                cartsDAL.UpdateId(userId, HttpContext.Session.GetString("sessionid"));

                //set 'name' key with name of user
                string name = usersDAL.FindName(userId);
                HttpContext.Session.SetString("name", name);

                return RedirectToAction("DisplayProduct", "Product");
            }
            else
            {
                //if user directly login

                //set session with user id and name
                HttpContext.Session.SetString("userid", userId);
                string name = usersDAL.FindName(userId);
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
            var userId = usersDAL.FindUserId(userModel);

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
                cartsDAL.DeleteCart(userId);

                //update new cart with user id
                cartsDAL.UpdateId(userId, HttpContext.Session.GetString("sessionid"));

                //set 'name' key with name of user
                string name = usersDAL.FindName(userId);
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
            int usernameInDb = usersDAL.FindUser(user.Username);
            int lastUserId = usersDAL.FindLastUserId();
            user.Id = Convert.ToString(lastUserId + 1);

            if (usernameInDb == 0)
            {
                usersDAL.AddUser(user);

                if (HttpContext.Session.GetString("userid") != null)
                {
                    //if user has added to cart before registering

                    //store GUID as session id
                    HttpContext.Session.SetString("sessionid", HttpContext.Session.GetString("userid"));

                    //update new cart with user id
                    cartsDAL.UpdateId(user.Id, HttpContext.Session.GetString("sessionid"));
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
            int usernameInDb = usersDAL.FindUser(user.Username);
            int lastUserId = usersDAL.FindLastUserId();
            user.Id = Convert.ToString(lastUserId + 1);

            if (usernameInDb == 0)
            {
                usersDAL.AddUser(user);

                if (HttpContext.Session.GetString("userid") != null)
                {
                    //if user has added to cart before registering

                    //store GUID as session id
                    HttpContext.Session.SetString("sessionid", HttpContext.Session.GetString("userid"));

                    //update new cart with user id
                    cartsDAL.UpdateId(user.Id, HttpContext.Session.GetString("sessionid"));
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
